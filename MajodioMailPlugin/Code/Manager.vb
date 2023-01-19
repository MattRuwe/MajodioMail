Imports Majodio.Mail.Common

Imports System.IO
Imports System.Reflection

Public Class Manager

    Private Shared _LogPlugIns As LogServerInterfaceCollection
    Private Shared _Imap4PlugIns As Imap4ServerInterfaceCollection
    Private Shared _Pop3PlugIns As Pop3ServerInterfaceCollection
    Private Shared _QueuePlugIns As QueueMonitorInterfaceCollection
    Private Shared _SmtpPlugIns As SmtpServerInterfaceCollection

    Shared Sub New()
        ReloadPlugIns()
    End Sub

    Private Shared Sub ReloadPlugIns()
        Dim PlugInDir As String = GetApplicationDirectory() ' & PLUG_IN_DIRECTORY
        Dim Files() As String
        Dim CurrentAssembly As Assembly
        Dim Types() As Type

        Dim TempPlugin As Object = Nothing

        Majodio.Common.Utilities.TraceMe("Checking for plugins")

        _LogPlugIns = New LogServerInterfaceCollection
        _Imap4PlugIns = New Imap4ServerInterfaceCollection
        _Pop3PlugIns = New Pop3ServerInterfaceCollection
        _QueuePlugIns = New QueueMonitorInterfaceCollection
        _SmtpPlugIns = New SmtpServerInterfaceCollection

        Files = Directory.GetFiles(PlugInDir, "*.dll")

        For i As Integer = 0 To Files.GetUpperBound(0)
            CurrentAssembly = [Assembly].LoadFile(Files(i))
            Types = CurrentAssembly.GetTypes()
            For j As Integer = 0 To Types.GetUpperBound(0)
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.ILogServer).FullName)) Then
                    Majodio.Common.Utilities.TraceMe("Log Plugin Found.  " & Types(j).FullName)
                    _LogPlugIns.Add(Activator.CreateInstance(Types(j)))
                End If
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.IImap4Server).FullName)) Then
                    Majodio.Common.Utilities.TraceMe("IMAP4 Plugin Found.  " & Types(j).FullName)
                    _Imap4PlugIns.Add(Activator.CreateInstance(Types(j)))
                End If
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.IPop3Server).FullName)) Then
                    Majodio.Common.Utilities.TraceMe("POP3 Plugin Found.  " & Types(j).FullName)
                    _Pop3PlugIns.Add(Activator.CreateInstance(Types(j)))
                End If
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.IQueueMonitor).FullName)) Then
                    Majodio.Common.Utilities.TraceMe("Queue Monitor Plugin Found.  " & Types(j).FullName)
                    _QueuePlugIns.Add(Activator.CreateInstance(Types(j)))
                End If
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.ISmtpServer).FullName)) Then
                    Majodio.Common.Utilities.TraceMe("SMTP Plugin Found.  " & Types(j).FullName)
                    _SmtpPlugIns.Add(Activator.CreateInstance(Types(j)))
                End If
            Next
        Next
    End Sub

    Public Shared ReadOnly Property LogServerInterfaces() As LogServerInterfaceCollection
        Get
            Return _LogPlugIns
        End Get
    End Property

    Public Shared ReadOnly Property Imap4ServerInterfaces() As Imap4ServerInterfaceCollection
        Get
            Return _Imap4PlugIns
        End Get
    End Property

    Public Shared ReadOnly Property Pop3ServerInterfaces() As Pop3ServerInterfaceCollection
        Get
            Return _Pop3PlugIns
        End Get
    End Property

    Public Shared ReadOnly Property QueueMonitorInterfaces() As QueueMonitorInterfaceCollection
        Get
            Return _QueuePlugIns
        End Get
    End Property

    Public Shared ReadOnly Property SmtpServerInterfaces() As SmtpServerInterfaceCollection
        Get
            Return _SmtpPlugIns
        End Get
    End Property
#Region " SMTP Server Interface"
    Private Delegate Sub DnsblBlockedIpDelegate(ByVal IpAddress As System.Net.IPAddress)
    Public Shared Sub DnsblBlockedIp(ByVal IpAddress As System.Net.IPAddress)
        For i As Integer = 0 To _SmtpPlugIns.Count - 1
            Try
                If _SmtpPlugIns(i).Enabled Then
                    Dim Dlgt As New DnsblBlockedIpDelegate(AddressOf _SmtpPlugIns(i).DnsblBlockedIp)
                    Dlgt.BeginInvoke(IpAddress, Nothing, Nothing)
                End If
            Catch ex As Exception
                'Logger.WriteError(ex, "An error occurred while attempting to execute the DnsblBlockedIp method.")
            End Try
        Next
    End Sub

    Private Delegate Sub DnsblBlockedMessageDelegate(ByVal Message As Mime.Message, ByVal IpAddress As System.Net.IPAddress)
    Public Shared Sub DnsblBlockedMessage(ByVal Message As Mime.Message, ByVal IpAddress As System.Net.IPAddress)
        For i As Integer = 0 To _SmtpPlugIns.Count - 1
            Try
                If _SmtpPlugIns(i).Enabled Then
                    Dim Dlgt As New DnsblBlockedMessageDelegate(AddressOf _SmtpPlugIns(i).DnsblBlockedMessage)
                    Dlgt.BeginInvoke(Message, IpAddress, Nothing, Nothing)
                End If
            Catch ex As Exception
                'Logger.WriteError(ex, "An error occurred while attempting to execute the MailReceived method.")
            End Try
        Next
    End Sub

    Private Delegate Sub MailReceivedDelegate(ByVal Message As Mime.Message, ByVal RelayConnection As Boolean)
    Public Shared Sub MailReceived(ByVal Message As Mime.Message, ByVal IsRelayConnection As Boolean)
        For i As Integer = 0 To _SmtpPlugIns.Count - 1
            Try
                If _SmtpPlugIns(i).Enabled Then
                    Dim Dlgt As New MailReceivedDelegate(AddressOf _SmtpPlugIns(i).MailReceived)
                    Dlgt.BeginInvoke(Message, IsRelayConnection, Nothing, Nothing)
                End If
            Catch ex As Exception
                'Logger.WriteError(ex, "An error occurred while attempting to execute the MailReceived method.")
            End Try
        Next
    End Sub
#End Region
End Class