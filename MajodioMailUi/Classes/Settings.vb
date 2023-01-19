Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports Majodio.Mail.Common
Imports Majodio.Mail.common.Configuration

<Serializable()> _
Public Class Settings
    Private _bannedIps As BannedIpCollection = Nothing
    Private _relayIps As RelayIpCollection = Nothing
    Private _dnsbls As DnsblCollection = Nothing
    Private _spamRules As SpamRuleCollection = Nothing

    Public Sub New()
        '_Config = New Config
        '_RelayIps = New RelayIpCollection
        '_Dnsbls = New DnsblCollection
        '_spamRules = New SpamRuleCollection
    End Sub

    <DisplayName("Server Administrator Username"), _
    Description("The username for the server administrator")> _
    Public Property ServerAdminUsername() As String
        Get
            Return RemoteConfigClient.RemoteConfig.ServerAdminUsername
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.ServerAdminUsername = value
        End Set
    End Property

    <DisplayName("Server Administrator Password"), _
    Description("The password for the server administrator")> _
    Public Property ServerAdminPassword() As String
        Get
            Return RemoteConfigClient.RemoteConfig.ServerAdminPassword
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.ServerAdminPassword = value
        End Set
    End Property

    <EditorAttribute(GetType(FolderNameEditor), GetType(UITypeEditor)), _
    DisplayName("Application Directory")> _
    Public Property ApplicationDirectory() As String
        Get
            Return GetApplicationDirectory()
        End Get
        Set(ByVal value As String)
            SetApplicationDirectory(value)
        End Set
    End Property

    <Description("If set to true, all email stored on the server will be encrypted"), _
    DisplayName("Encrypt Email")> _
    Public Property EncryptEmail() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.EncryptEmail
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.EncryptEmail = value
        End Set
    End Property

    <Category("Queue Monitor"), _
    Description("The number of attempts the Queue Monitor should attempt while sending a message to a remote SMTP server"), _
    DisplayName("Failed Mail Retry Attempts")> _
    Public Property FailedMailRetryAttempts() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.FailedMailRetryAttempts
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.FailedMailRetryAttempts = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("The message that will be sent to the client upon receiving the QUIT command from a remote SMTP server"), _
    DisplayName("Good Bye Message")> _
    Public Property GoodByeMessage() As String
        Get
            Return RemoteConfigClient.RemoteConfig.GoodbyeMsg
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.GoodbyeMsg = value
        End Set
    End Property

    <Category("IMAP4"), _
    Description("The TCP port that the IMAP server should listen on.  This default for this value is 143 and should not be changed unless needed."), _
    DisplayName("IMAP4 TCP Port")> _
    Public Property Imap4TcpPort() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.Imap4TcpPort
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.Imap4TcpPort = value
        End Set
    End Property

    <Category("POP3"), _
    Description("The number of connections that will be allowed from a particular user within the number of seconds specified by MaxPop3ConnectionInterval.  If the client exceeds this count then the server will reject the connection."), _
    DisplayName("Max POP3 Connection Per Interval")> _
    Public Property MaxPop3ConnectionsPerInterval() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.MaxPop3ConnectionsPerInterval
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.MaxPop3ConnectionsPerInterval = value
        End Set
    End Property

    <Category("POP3"), _
    Description("The number of seconds that need to elapse after a user is authenticated before that connection is removed from the pool of remembered connections"), _
    DisplayName("POP3 Connection Interval")> _
    Public Property MaxPop3ConnectionInterval() As Int64
        Get
            Return RemoteConfigClient.RemoteConfig.MaxPop3ConnectionInterval / TimeSpan.TicksPerSecond
        End Get
        Set(ByVal value As Int64)
            RemoteConfigClient.RemoteConfig.MaxPop3ConnectionInterval = value * TimeSpan.TicksPerSecond
        End Set
    End Property

    <Category("POP3"), _
    Description("The TCP port that the POP3 server should listen on.  This default for this value is 110 and should not be changed unless needed."), _
    DisplayName("POP3 TCP Port")> _
    Public Property Pop3TcpPort() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.Pop3TcpPort
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.Pop3TcpPort = value
        End Set
    End Property

    <Category("POP3"), _
    Description("The TCP port that the POP3 server should listen on for secure POP3 connections.  This default for this value is 995 and should not be changed unless needed."), _
    DisplayName("POP3 Secure TCP Port")> _
    Public Property Pop3SecureTcpPort() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.Pop3SecureTcpPort
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.Pop3SecureTcpPort = value
        End Set
    End Property

    <Category("Queue Monitor"), _
    Description("The number of seconds that will pass between checks of the queue monitor.  The timer does not restart until the current queue check is completed."), _
    DisplayName("Queue Monitor Check Interval")> _
    Public Property QueueMonitorCheckInterval() As Int32
        Get
            Return RemoteConfigClient.RemoteConfig.QueueCheckInterval / 1000
        End Get
        Set(ByVal value As Int32)
            RemoteConfigClient.RemoteConfig.QueueCheckInterval = value * 1000
        End Set
    End Property

    <Category("SMTP"), _
    Description("If set to true, the server will reject SMTP session that do not provide an SMTP FROM that matches the incoming IP address.  If a MAIL FROM is not issued with SMTP session and the Require SMTP From option is false, this command will have no effect.  This setting may have a negative impact on performance of the mail server, but will increase the effectiveness of blocking spam."), _
    DisplayName("Required SMTP From Match Incoming IP")> _
    Public Property RequireFromMatchIncomingIp() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.RequireFromMatchIncomingIp
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.RequireFromMatchIncomingIp = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("If set to true, this will cause the SMTP server to accept all e-mails regardless of recipient or if they are found to be in offenders in the DNSBL.  The emails, however, will be delivered to a SPAM box."), _
    DisplayName("Accept All Mail")> _
    Public Property AcceptAllMail() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.AcceptAllMail
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.AcceptAllMail = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("If set to true, the server will reject SMTP connections that send an empty MAIL FROM command (e.g. MAIL FROM <>).  This is a common practice for spammers, but some legitimate senders also do this."), _
    DisplayName("Require SMTP From")> _
    Public Property RequireSmtpFrom() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.RequireSmtpFrom
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.RequireSmtpFrom = value
        End Set
    End Property

    <Description("If set to true, the server will not delete delivered e-mail, instead it will move the mail to a folder named " & Majodio.Mail.Common.RETAINED_MAIL_FOLDER), _
    DisplayName("Retain Mail")> _
    Public Property RetainMessages() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.RetainMessages
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.RetainMessages = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("If set to true, the SMTP server will scan all incoming mail for hyperlinks and check to see if they resolve to an IP address listed with the DNSBL"), _
    DisplayName("Scan Message Links")> _
    Public Property ScanMessageLinks() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.ScanMessageLinks
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.ScanMessageLinks = value
        End Set
    End Property

    <Description("If set to true, the server will send usage information to Majodio Software for fixing bugs and usage patterns.  If set to false, the server will NOT send any information to Majodio Software.  We respect your privacy!"), _
    DisplayName("Send Usage Information")> _
    Public Property SendUsageInformation() As Boolean
        Get
            Return RemoteConfigClient.RemoteConfig.SendUsageInformation
        End Get
        Set(ByVal value As Boolean)
            RemoteConfigClient.RemoteConfig.SendUsageInformation = value
        End Set
    End Property
    <Description("The primary IP address that is assigned to this server"), _
    DisplayName("Server IP")> _
    Public ReadOnly Property ServerIp() As String
        Get
            Return RemoteConfigClient.RemoteConfig.ServerIp
        End Get
    End Property

    <Category("SMTP"), _
    Description("The servername that will appear in the SMTP greeting."), _
    DisplayName("Server Name")> _
    Public Property ServerName() As String
        Get
            Return RemoteConfigClient.RemoteConfig.ServerName
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.ServerName = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("The fully qualified domain name (FQDN) that will be sent to remote server during an outgoing SMTP connection.  " & _
        "This defaults to the current machine's FQDN.  In certain circumstances, this may be inaccurate like when a server hosts " & _
        "multiple domains or is behind a firewall.  It is important to set this to the correct address so your e-mail does not get " & _
        "marked as spam."), _
    DisplayName("Server DNS Name")> _
    Public Property ServerDnsName() As String
        Get
            Return RemoteConfigClient.RemoteConfig.ServerDnsName
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.ServerDnsName = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("The TCP port that the SMTP server should listen on.  This default for this value is 25 and should not be changed unless needed."), _
    DisplayName("SMTP TCP Port")> _
    Public Property SmtpTcpPort() As Integer
        Get
            Return RemoteConfigClient.RemoteConfig.SmtpTcpPort
        End Get
        Set(ByVal value As Integer)
            RemoteConfigClient.RemoteConfig.SmtpTcpPort = value
        End Set
    End Property

    <Category("SMTP"), _
    Description("The message that will be sent to the client upon receiving a new connection from a remote SMTP server/client."), _
    DisplayName("Welcome Message")> _
    Public Property WelcomeMessage() As String
        Get
            Return RemoteConfigClient.RemoteConfig.WelcomeMsg
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.WelcomeMsg = value
        End Set
    End Property

    <Category("SMTP"), _
    EditorAttribute(GetType(CollectionEditor), GetType(UITypeEditor)), _
    Description("Banned IPs are denied access to the SMTP server."), _
    DisplayName("Banned IPs")> _
    Public ReadOnly Property BannedIps() As BannedIpCollection
        Get
            If IsNothing(_BannedIps) Then
                _BannedIps = RemoteConfigClient.RemoteConfig.BannedIps
                AddHandler _BannedIps.ContentsChanged, AddressOf BannedIpsChangedHandler
            End If

            Return _BannedIps
        End Get
    End Property

    Private Sub BannedIpsChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        'Dim bannedIps As BannedIpCollection = CType(sender, BannedIpCollection)
        RemoteConfigClient.RemoteConfig.ClearBannedIps()
        For i As Integer = 0 To _bannedIps.Count - 1
            RemoteConfigClient.RemoteConfig.AddBannedIp(_bannedIps(i).IP)
        Next
    End Sub


    <Category("SMTP"), _
    EditorAttribute(GetType(CollectionEditor), GetType(UITypeEditor))> _
    Public ReadOnly Property SpamRules() As SpamRuleCollection
        Get
            If IsNothing(_spamRules) Then
                _spamRules = RemoteConfigClient.RemoteConfig.SpamRules
                AddHandler _spamRules.ContentsChanged, AddressOf SpamRuleChangedHandler
            End If
            Return _spamRules
        End Get
    End Property

    Private Sub SpamRuleChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        RemoteConfigClient.RemoteConfig.ClearSpamRules()
        For i As Integer = 0 To _spamRules.Count - 1
            RemoteConfigClient.RemoteConfig.AddSpamRule(_spamRules(i))
        Next
    End Sub

    <Category("SMTP"), _
    EditorAttribute(GetType(CollectionEditor), GetType(UITypeEditor)), _
    Description("The DNSBL (Domain Name Server Blocking List) is used to detect e-mail from IP addresses that are known to be associated with spammers.  If a server from one of these listed IP addresses attempts to connect to this mail server, the connection will be denied."), _
    DisplayName("DNSBL Server(s)")> _
    Public ReadOnly Property DnsblServers() As DnsblCollection
        Get
            If IsNothing(_dnsbls) Then
                _dnsbls = RemoteConfigClient.RemoteConfig.Dnsbls
                AddHandler _dnsbls.ContentsChanged, AddressOf DnsblChangedHandler
            End If
            Return _dnsbls
        End Get
    End Property

    Private Sub DnsblChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        RemoteConfigClient.RemoteConfig.ClearDnsbls()
        For i As Integer = 0 To _dnsbls.Count - 1
            RemoteConfigClient.RemoteConfig.AddDnsbl(_dnsbls(i))
        Next
    End Sub

    <Category("SMTP"), _
    EditorAttribute(GetType(CollectionEditor), GetType(UITypeEditor)), _
    Description("Each relay IP is either a distinct IP address or a subnet mask."), _
    DisplayName("Relay IPs")> _
    Public ReadOnly Property RelayIps() As RelayIpCollection
        Get
            If IsNothing(_relayIps) Then
                _relayIps = RemoteConfigClient.RemoteConfig.RelayIps
                AddHandler _relayIps.ContentsChanged, AddressOf RelayIpChangedHandler
            End If

            Return _relayIps
        End Get
    End Property

    Private Sub RelayIpChangedHandler(ByVal sender As Object, ByVal e As EventArgs)
        RemoteConfigClient.RemoteConfig.ClearRelayIps()
        For i As Integer = 0 To _relayIps.Count - 1
            RemoteConfigClient.RemoteConfig.AddRelayIp(_relayIps(i).IP)
        Next
    End Sub

    <Category("SMTP"), _
    Description("The account that will be assigned the postmaster responsibility.  This account will reply to failed messages and process incoming mail server operations.  If you wish to disable postmaster processing, then leave this entry blank."), _
    DisplayName("Postmaster Account Name")> _
    Public Property PostmasterAccountName() As String
        Get
            Return RemoteConfigClient.RemoteConfig.PostmasterAccountName
        End Get
        Set(ByVal value As String)
            If value.IndexOf("@") = -1 And value.IndexOf(" ") = -1 Then
                RemoteConfigClient.RemoteConfig.PostmasterAccountName = value
            Else
                Throw New Exception("Value cannot contain an ""@"" sign or spaces")
            End If
        End Set
    End Property

    <EditorAttribute(GetType(FileNameEditor), GetType(UITypeEditor)), _
    Description("The Path of the SSL Certificate that will be used to secure communications between the remote client " & _
                "and this server.  By entering the path to the SSL certificate you are automatically enabling Secure " & _
                "SMTP/POP3/IMAP.  POP3 has two secure operting modes.  One method uses the STLS command which starts " & _
                "with a standard POP3 connection and creates a secure connection after issuing the STLS command.  The " & _
                "other method is to have a POP3 server running on port 995 and any connections made to that port are " & _
                "required to be secured.  If this property is not set (i.e. empty string) no servers will run on the " & _
                "secure POP3 port."), _
    DisplayName("SSL Certificate Path")> _
    Public Property SSLCertificatePath() As String
        Get
            Return RemoteConfigClient.RemoteConfig.SSLCertificatePath
        End Get
        Set(ByVal value As String)
            RemoteConfigClient.RemoteConfig.SSLCertificatePath = value
        End Set
    End Property
End Class
