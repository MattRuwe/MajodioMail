Imports System.Net
Imports System.Net.Sockets
Imports Majodio.Mail.Mime
Imports Majodio.Mail.common.Configuration
Imports Majodio.Mail.Common.Smtp

Namespace Smtp
    Public Class Client
        Private _Message As Message
        'Private _config As New Majodio.Mail.Common.Configuration.Config

        Public Sub New(ByVal Message As Message)
            _Message = Message
        End Sub

        Public Function Send(ByVal NotifyOfErrors As Boolean) As Response()
            Return Send(_Message, NotifyOfErrors)
        End Function

        Public Function Send(ByVal Message As Message, ByVal NotifyOfErrors As Boolean) As Response()
            Dim RVal As Response() = Nothing
            'Dim i As Integer
            'Dim SmtpServer As Socket
            Dim SendReceiveData As Tcp.SocketServer
            Dim RData As String
            Dim S As Socket = Nothing
            Try
                If IsNothing(Message) Then
                    Throw New NullReferenceException("SMTP client cannot send null message")
                End If
                If IsNothing(Message.SmtpFromAddress) Then
                    Throw New NullReferenceException("SMTP client cannot send message with null from address")
                End If
                If IsNothing(Message.SmtpToAddress) Then
                    Throw New NullReferenceException("SMTP client cannot send message without any recipients")
                End If
                Try
                    S = GetConnectionToServer(Message.SmtpToAddress.GetDomain)
                Catch exc As Exception
                    RVal = New Response() {New Response(ResponseCode.ErrorWhileProcessing, "The domain for user " & Message.SmtpToAddress.ToString(EmailStringFormat.AddressBraces) & " could not be located")}
                End Try
                If IsNothing(RVal) Then
                    SendReceiveData = New Tcp.SocketServer(S)
                    'SmtpServer = GetConnectionToServer(Message.ToAddresses(i).GetDomain)
                    'If Not SmtpServer.Connected Then
                    If Not SendReceiveData.Connection.Connected Then
                        Majodio.Common.Utilities.TraceMe("Unable to connect to domain " & Message.SmtpToAddress.GetDomain)
                        Log.Logger.WriteLog("Unable to connect to domain " & Message.SmtpToAddress.GetDomain)
                        RVal = New Response() {New Response(ResponseCode.ServiceNotAvailable, "The server could not be contacted")}
                    Else
                        Log.Logger.WriteLog("************Starting SMTP client session************")
                        Log.Logger.WriteLog("Connection made to " & CType(SendReceiveData.Connection.RemoteEndPoint, IPEndPoint).Address.ToString)
                        RData = SendReceiveData.ReceiveData(vbCrLf)
                        While RData.Substring(3, 1) = "-"
                            RData = SendReceiveData.ReceiveData(vbCrLf)
                        End While
                        If GetResponseCode(RData) = 220 Then
                            SendHello(SendReceiveData)
                            SendReceiveData.SendData("MAIL FROM:" & Message.SmtpFromAddress.ToString(EmailStringFormat.AddressBraces) & vbCrLf)
                            RData = SendReceiveData.ReceiveData(vbCrLf)
                            If GetResponseCode(RData) = 250 Then
                                SendReceiveData.SendData("RCPT TO:" & Message.SmtpToAddress.ToString(EmailStringFormat.AddressBraces) & vbCrLf)
                                RData = SendReceiveData.ReceiveData(vbCrLf)
                                If GetResponseCode(RData) = 250 Or GetResponseCode(RData) = 251 Or GetResponseCode(RData) = 252 Then
                                    SendReceiveData.SendData("DATA" & vbCrLf)
                                    RData = SendReceiveData.ReceiveData(vbCrLf)
                                    If GetResponseCode(RData) = 354 Then
                                        SendReceiveData.SendData(Message.RawMessage.Replace(vbCrLf & ".", vbCrLf & "..") & vbCrLf & "." & vbCrLf)
                                        RData = SendReceiveData.ReceiveData(vbCrLf)
                                        If GetResponseCode(RData) <> 250 Then
                                            SendReceiveData.CloseAcceptedConnection()
                                            RVal = New Response() {New Response(GetResponseCode(RData), GetResponseMessage(RData))}
                                        End If
                                    Else
                                        SendQuit(SendReceiveData)
                                        SendReceiveData.CloseAcceptedConnection()
                                        RVal = New Response() {New Response(GetResponseCode(RData), GetResponseMessage(RData))}
                                    End If
                                ElseIf GetResponseCode(RData) = 450 OrElse GetResponseCode(RData) = 451 OrElse GetResponseCode(RData) = 550 OrElse GetResponseCode(RData) = 551 OrElse GetResponseCode(RData) = 552 OrElse GetResponseCode(RData) = 554 Then
                                    'If NotifyOfErrors Then
                                    '    SendErrors(Message, i, New Response(GetResponseCode(RData), GetResponseMessage(RData)))
                                    'End If
                                    RVal = New Response() {New Response(GetResponseCode(RData), GetResponseMessage(RData))}
                                End If
                            Else
                                SendQuit(SendReceiveData)
                                SendReceiveData.CloseAcceptedConnection()
                                RVal = New Response() {New Response(GetResponseCode(RData), GetResponseMessage(RData))}
                            End If
                            SendQuit(SendReceiveData)
                            SendReceiveData.CloseAcceptedConnection()
                            If IsNothing(RVal) Then
                                RVal = New Response() {New Response(ResponseCode.Ok, "Message Successfully Sent")}
                            End If
                            Log.Logger.WriteLog("************Ending SMTP client session************")
                        Else
                            RVal = New Response() {New Response(GetResponseCode(RData), GetResponseMessage(RData))}
                        End If
                        End If
                End If
            Catch ex As Exception
                RVal = New Response() {New Response(ResponseCode.ErrorWhileProcessing, "An error occurred while processing the mail message.  Details: " & vbCrLf & ex.Message)}
            End Try
            Return RVal
        End Function

        Public Shared Sub SendErrors(ByVal Message As Message, ByVal ToIndex As Integer, ByVal Response As Response)
            Dim Client As Client
            Dim ErrorMessage As New Message(True)
            Dim MessageText As String

            ErrorMessage.SmtpFromAddress = New EmailAddress("postmaster@" & Message.FromAddress.GetDomain)
            ErrorMessage.FromAddress = New EmailAddress("postmaster@" & Message.FromAddress.GetDomain)
            ErrorMessage.SmtpToAddress = Message.FromAddress
            ErrorMessage.ToAddresses.Add(Message.FromAddress)
            MessageText = "The following message could not be delivered.  The following error occurred:" & vbCrLf
            MessageText &= Response.ToString() & vbCrLf
            If Message.RawMessage.Length > 4096 Then
                MessageText &= "The message content has been trucated because it was too long" & vbCrLf
                MessageText &= Message.RawMessage.Substring(0, 4096)
            Else
                MessageText &= Message.RawMessage
            End If
            ErrorMessage.RootBodyPart.BodyPartContent = MessageText

            'ErrorMessage.AddNewHeaders("postmaster@" & Message.FromAddress.GetDomain, String.Empty, Message.FromAddress.ToString(EmailStringFormat.Address), "Unable to send email")
            If RemoteConfigClient.RemoteDomain.UserExists(Message.SmtpFromAddress.GetDomain, Message.SmtpFromAddress.GetUsername) Then
                Dim QueuedMessage As Majodio.Mail.Common.Storage.QueuedMail.Message
                QueuedMessage = ErrorMessage.GetQueuedMessage(Message.SmtpFromAddress.GetDomain, Message.SmtpFromAddress.GetUsername)
                QueuedMessage.Save()
            Else
                Client = New Client(ErrorMessage)
                Client.Send(False)
            End If
        End Sub

        Private Sub CloseConnection(ByRef SmtpServer As Socket)
            If Not IsNothing(SmtpServer) Then
                If SmtpServer.Connected Then
                    SmtpServer.Shutdown(SocketShutdown.Both)
                    SmtpServer.Close()
                    SmtpServer = Nothing
                End If
            End If
        End Sub

        Private Sub SendHello(ByVal SendReceive As Tcp.SocketServer)
            Dim RData As String
            Dim serverDnsName As String = RemoteConfigClient.RemoteConfig.ServerDnsName
            If IsNothing(serverDnsName) OrElse serverDnsName.Trim.Length = 0 Then
                serverDnsName = GetFqdn()
            End If
            SendReceive.SendData("EHLO " & serverDnsName & vbCrLf)
            RData = SendReceive.ReceiveData(vbCrLf)
            If GetResponseCode(RData) = 250 Then
                While RData.Substring(3, 1) = "-" And GetResponseCode(RData) = 250
                    RData = SendReceive.ReceiveData(vbCrLf)
                End While
            Else
                SendReceive.SendData("HELO " & serverDnsName & vbCrLf)
                RData = SendReceive.ReceiveData(vbCrLf)
            End If
        End Sub

        Private Sub SendQuit(ByVal SendReceive As Tcp.SocketServer)
            SendReceive.SendData("QUIT" & vbCrLf)
        End Sub

        Private Shared Function GetResponseCode(ByVal Msg As String) As Integer
            Dim RVal As Integer = -1
            If Msg.Length >= 3 Then
                If IsNumeric(Msg.Substring(0, 3)) Then
                    RVal = Msg.Substring(0, 3)
                End If
            End If
            Return RVal
        End Function

        Private Shared Function GetResponseMessage(ByVal Msg As String) As String
            Dim RVal As String = String.Empty
            If Msg.Length >= 4 Then
                If Msg.IndexOf(" ") = 3 Then
                    RVal = Msg.Substring(3)
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Domain"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	2/18/2005	Updated to try standard DNS entry if no MX records are available
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetConnectionToServer(ByVal Domain As String) As Socket
            Dim S As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            Dim HostIp As IPHostEntry()

            Dim DnsList As ArrayList
            Dim Results As New ArrayList
            DnsList = GetDns()
            Dim DnsLite As New DnsLib.DnsLite
            DnsLite.setDnsServers(DnsList)
            Majodio.Common.Utilities.TraceMe("Attempting to retrieve MX records from domain " & Domain)
            Results = DnsLite.getMXRecords(Domain)
            Dim i, j As Integer
            If Results.Count = 0 Then

            End If
            If Results.Count > 0 Then
                '** Found the mx records
                ReDim HostIp(Results.Count - 1)
                i = 0
                While i < Results.Count And Not S.Connected
                    HostIp(i) = System.Net.Dns.GetHostEntry(CType(Results(i), DnsLib.MXRecord).exchange)
                    Majodio.Common.Utilities.TraceMe("Retrieved the following MX records from DNS")
                    Majodio.Common.Utilities.TraceMe(CType(Results(i), DnsLib.MXRecord).exchange & " " & CType(Results(i), DnsLib.MXRecord).preference)
                    j = 0
                    While j <= HostIp(i).AddressList.GetUpperBound(0) And Not S.Connected
                        Majodio.Common.Utilities.TraceMe("Attempting to connect to " & HostIp(i).AddressList(j).ToString)
                        Try
                            S.Connect(New IPEndPoint(HostIp(i).AddressList(j), 25))
                        Catch exc As Exception
                            Majodio.Common.Utilities.TraceMe("Could not connect to " & HostIp(i).AddressList(j).ToString)
                        End Try
                        j += 1
                    End While
                    i += 1
                End While
            Else
                '** Couldn't find any MX records, see if the domain can be found in the local DNS
                ReDim HostIp(0)
                Try
                    HostIp(0) = System.Net.Dns.GetHostEntry(Domain)
                Catch exc As Exception
                End Try
                If Not IsNothing(HostIp(0)) AndAlso Not IsNothing(HostIp(0).AddressList) AndAlso HostIp(0).AddressList.GetUpperBound(0) > -1 Then
                    For i = 0 To HostIp(0).AddressList.GetUpperBound(0)
                        If Not S.Connected Then
                            Try
                                S.Connect(New IPEndPoint(HostIp(0).AddressList(i), SMTP_DEFAULT_TCP_PORT))
                                Exit For
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
            End If
            If Not S.Connected Then
                Throw New System.Net.Sockets.SocketException("Couldn't find the mx record for host " & Domain)
            Else
                Return S
            End If

        End Function

    End Class
End Namespace