Imports System.IO
Imports System.Threading
Imports System.Management
Imports System.Net
Imports System.Net.Sockets
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports Majodio.Mail.common.Configuration

Public Structure DnsblResults
    Public IsInDnsbl As Boolean
    Public DnsblResult As IPHostEntry
    Public DnsblServer As String
End Structure


Public Module Functions
    Public Function CanIpRelay(ByVal clientIpAddress As IPAddress) As Boolean
        Dim RVal As Boolean = False
        Try
            Dim RelayIps As IPAddress()
            Dim i As Integer
            RelayIps = RemoteConfigClient.RemoteConfig.GetRelayIps()
            For i = 0 To RelayIps.GetUpperBound(0)
                If IsIpInRange(clientIpAddress, RelayIps(i)) Then
                    RVal = True
                    Exit For
                End If
            Next
        Catch Exc As Exception
            Log.Logger.WriteLog("An error occurred " & Exc.Source & " " & Exc.Message & " " & Exc.StackTrace)
        End Try

        Return RVal
    End Function

    Public Function IsInboundIpFromDomain(ByVal InboundIp As IPAddress, ByVal Domain As String) As Boolean
        Dim RVal As Boolean = False
        Dim Addresses As IPAddress()
        Dim Host As IPHostEntry
        Dim HostName As String
        Dim M As Match


        Try
            Host = System.Net.Dns.GetHostEntry(InboundIp)
            HostName = Host.HostName.ToLower
        Catch ex As SocketException
            'Couldn't resolve the IP address
            HostName = String.Empty
        End Try

        M = Regex.Match(HostName, "(?i)(?<=.*)(?<domain>[A-Za-z0-9-]+)(\.[A-Za-z0-9-]+)$")
        If M.Success Then
            HostName = M.Groups("domain").Value
        End If
        M = Regex.Match(Domain, "(?i)(?<=.*)(?<domain>[A-Za-z0-9-]+)(\.[A-Za-z0-9-]+)$")
        If M.Success Then
            Domain = M.Groups("domain").Value
        End If
        If HostName.Length >= Domain.Length AndAlso HostName.Substring(HostName.Length - Domain.Length) = Domain.ToLower Then
            RVal = True
        Else
            Addresses = GetHostDnsMxRecords(Domain)
            If Not IsNothing(Addresses) Then
                For i As Integer = 0 To Addresses.GetUpperBound(0)
                    If Addresses(i).ToString = InboundIp.ToString Then
                        RVal = True
                        Exit For
                    End If
                Next
            End If
        End If

        Return RVal
    End Function

    Public Function GetHostDnsMxRecords(ByVal Domain As String) As IPAddress()
        Dim RVal As IPAddress() = Nothing

        Dim Results As New ArrayList
        Dim DnsLite As New DnsLib.DnsLite
        Dim Addresses As IPHostEntry
        Dim StartIndex As Integer
        DnsLite.setDnsServers(GetDns())

        Results = DnsLite.getMXRecords(Domain)
        If Results.Count > 0 Then
            For i As Integer = 0 To Results.Count - 1
                Addresses = System.Net.Dns.GetHostEntry(CType(Results(i), DnsLib.MXRecord).exchange)
                If Not IsNothing(RVal) Then
                    StartIndex = RVal.GetUpperBound(0) + 1
                    ReDim Preserve RVal(StartIndex + Addresses.AddressList.GetUpperBound(0))
                Else
                    StartIndex = 0
                    ReDim RVal(Addresses.AddressList.GetUpperBound(0))
                End If

                For j As Integer = 0 To Addresses.AddressList.GetUpperBound(0)
                    RVal(StartIndex + j) = Addresses.AddressList(j)
                Next
            Next
        Else
            Try
                RVal = System.Net.Dns.GetHostEntry(Domain).AddressList
            Catch ex As Exception
            End Try
        End If
        Return RVal
    End Function

    Public Function IsInDnsbl(ByVal IP As IPAddress) As DnsblResults
        Dim IpReversed As String = String.Empty
        Dim TmpIp As String()
        Dim DnsblServer As String()
        'Dim Config As New Majodio.Mail.Common.Configuration.Config
        Dim i As Integer
        Dim RVal As DnsblResults = Nothing
        Dim DnsResult As IPHostEntry
        RVal.IsInDnsbl = False
        DnsblServer = RemoteConfigClient.RemoteConfig.GetDnsbl
        If Not IsNothing(IP) AndAlso Not IsNothing(DnsblServer) AndAlso DnsblServer.Length > 0 Then
            For j As Integer = 0 To DnsblServer.GetUpperBound(0)
                TmpIp = IP.ToString.Split(".")
                If TmpIp.GetUpperBound(0) = 3 Then
                    For i = TmpIp.GetUpperBound(0) To 0 Step -1
                        If i > 0 Then
                            IpReversed &= TmpIp(i) & "."
                        Else
                            IpReversed &= TmpIp(i)
                        End If
                    Next
                    Try
                        DnsResult = System.Net.Dns.GetHostEntry(IpReversed & "." & DnsblServer(j))
                        RVal.IsInDnsbl = True
                        RVal.DnsblResult = DnsResult
                        RVal.DnsblServer = DnsblServer(j)
                        Exit For
                    Catch exc As Exception
                        ' Do nothing because RVal is already set to false
                    End Try
                End If
            Next
        End If
        Return RVal
    End Function

    Public Function AreMessageLinksInDnsbl(ByVal MessageText As String) As DnsblResults
        'Optimistically assume that the message is not spam
        Dim RVal As DnsblResults = New DnsblResults
        Dim TmpRval As DnsblResults
        Dim RegExPattern As String = "(?im)http://(?<url>[^""/\s\?<>!@#\$%\^\{}\[\]\(\)\|\\&\*()_\+]+)"
        Dim MC As MatchCollection = Nothing
        Dim IPs As System.Net.IPAddress()
        Dim AddressesChecked As New Hashtable
        RVal.IsInDnsbl = False
        RVal.DnsblServer = String.Empty
        Try
            If Not IsNothing(MessageText) AndAlso MessageText.Trim.Length > 0 Then
                MC = Regex.Matches(MessageText, RegExPattern)
                For Each M As Match In MC
                    Try
                        IPs = System.Net.Dns.GetHostEntry(M.Groups("url").Value()).AddressList
                        For i As Integer = 0 To IPs.GetUpperBound(0)
                            If Not AddressesChecked.Contains(IPs(i).ToString) Then
                                TmpRval = IsInDnsbl(IPs(i))
                                If TmpRval.IsInDnsbl Then
                                    RVal = TmpRval
                                    Exit For
                                Else
                                    AddressesChecked.Add(IPs(i).ToString, True)
                                End If
                            End If
                        Next
                        If RVal.IsInDnsbl Then
                            Exit For
                        End If
                    Catch ex As Exception
                    End Try
                Next
            End If
        Catch ex As Exception
            Dim mailEx As New MailException("An exception occurred while scanning the message for spam links", ex)
            mailEx.ExceptionItems.Add("PARAMETER", "RegExPattern", RegExPattern)
            If Not IsNothing(MC) Then
                mailEx.ExceptionItems.Add("ITEM", "MC.Count", MC.Count)
            End If
            mailEx.ExceptionItems.Add("ITEM", "MessageText", MessageText)
            mailEx.Save()
        End Try
        Return RVal
    End Function

    Public Function IsIpInRange(ByVal IncomingIp As System.Net.IPAddress, ByVal CompareToIp As System.Net.IPAddress) As Boolean
        Dim RVal As Boolean = False
        Dim i As Integer
        Dim IncomingBytes As Byte() = IncomingIp.GetAddressBytes
        Dim CompareToBytes As Byte() = CompareToIp.GetAddressBytes
        If IncomingBytes.GetUpperBound(0) = CompareToBytes.GetUpperBound(0) Then
            RVal = True
            For i = 0 To IncomingIp.GetAddressBytes.GetUpperBound(0)
                'WriteLog("is " & CType(IncomingBytes(i) And CompareToBytes(i), String) & " = " & CType(IncomingBytes(i), String))
                If CType(IncomingBytes(i) And CompareToBytes(i), Integer) <> CType(IncomingBytes(i), Integer) Then
                    RVal = False
                    Exit For
                ElseIf CType(CompareToBytes(i), Integer) <> 255 And CType(IncomingBytes(i), Integer) <> CType(CompareToBytes(i), Integer) Then
                    RVal = False
                    Exit For
                End If
            Next
        End If
        'WriteLog("IsIpInRange returning " & RVal)
        Return RVal
    End Function

    Public Function GetDns() As ArrayList
        Dim MC As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim MO As ManagementObject
        Dim MOC As ManagementObjectCollection = MC.GetInstances()
        Dim DNS As String() = Nothing
        Dim TmpDns As String() = Nothing
        Dim RVal As New ArrayList
        Dim i As Integer
        For Each MO In MOC
            If CType(MO("ipEnabled"), Boolean) Then
                TmpDns = CType(MO("DNSServerSearchOrder"), String())
                If Not IsNothing(TmpDns) Then
                    For i = 0 To TmpDns.GetUpperBound(0)
                        If IsNothing(DNS) Then
                            ReDim DNS(0)
                        Else
                            ReDim Preserve DNS(DNS.GetUpperBound(0) + 1)
                        End If
                        DNS(DNS.GetUpperBound(0)) = TmpDns(i)
                    Next
                End If
            End If
        Next
        For i = 0 To DNS.GetUpperBound(0)
            RVal.Add(DNS(i))
        Next
        Return RVal
    End Function

    Public Sub SendUsageInformation(ByVal Message As String)
        'Dim Config As New Majodio.Mail.Common.Configuration.Config
        If RemoteConfigClient.RemoteConfig.SendUsageInformation Then
            Dim SM As New Majodio.Common.Messaging.SendMessage("Majodio Mail Server " & System.Reflection.Assembly.GetCallingAssembly.GetName.Version.ToString(4), Message)
            Dim T As New Thread(AddressOf SM.SendMessage)
            T.Name = "SendUsageInformation"
            T.Start()
        End If
    End Sub
End Module