Imports System.Xml
Imports System.io
Imports System.Net
Imports Majodio.Mail.Common.Storage
Imports System.Text.RegularExpressions

Namespace Configuration
    <Serializable()> _
    Public Class Config
        Inherits PersistentStorage

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	3/15/2005	Added functionality to migrate the data from the old 
        '''                         1.1 format to the new 1.2 format
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
            MyBase.New(GetApplicationDirectory() & "\" & CONFIG_FILE_LOCATION)
        End Sub

        Public Sub PerformUpdgrades()
            Dim RootNode As XmlElement = Nothing
            Dim OldNode As XmlElement = MyBase.GetSingleXmlElement("/SmtpConfiguration")
            'Check to see if the config file is using the SmtpConfiguration node which is now obsolete
            If Not IsNothing(OldNode) And IsNothing(MyBase.GetSingleXmlElement("/Configuration")) Then
                'The config file format is obsolete and needs to be upgraded
                'Migrate the information to the Configuration node instead
                RootNode = MyBase.CreateXmlElement("Configuration")
                MyBase.CopyNode(OldNode, RootNode)
                MyBase.XmlDoc.RemoveAll()
                MyBase.XmlDoc.AppendChild(RootNode)
            ElseIf Not IsNothing(MyBase.GetSingleXmlElement("/Configuration")) Then
                RootNode = MyBase.GetSingleXmlElement("/Configuration")
            End If

            If Not IsNothing(RootNode) Then
                'Check to see if the config file is using the TimerInterval node which is now obsolete.
                'It was replaced with the QueueCheckInterval node
                OldNode = MyBase.GetSingleXmlElement("/Configuration/TimerInterval")
                If Not IsNothing(OldNode) AndAlso IsNothing(MyBase.GetSingleXmlElement("/Configuration/QueueCheckInterval")) Then
                    MyBase.SetXmlValue("/Configuration/QueueCheckInterval", MyBase.GetXmlValue("/Configuration/TimerInterval"))
                    MyBase.DeleteXmlElement(OldNode)
                End If

                'Check to see if the config file is using the DNSBL node which is now obsolete.
                'It was replaced with the DNSBL node (which can be used multiple times)
                OldNode = MyBase.GetSingleXmlElement("/Configuration/TimerCheckInterval")
                If Not IsNothing(OldNode) AndAlso IsNothing(MyBase.GetSingleXmlElement("/Configuration/DNSBL")) Then
                    MyBase.SetXmlValue("/Configuration/DNSBL", MyBase.GetXmlValue("/Configuration/DnsblServer"))
                    MyBase.DeleteXmlElement(OldNode)
                End If

                'Update the version number
                OldNode = MyBase.GetSingleXmlElement("/Configuration/ServerName")
                If Not IsNothing(OldNode) Then
                    Dim ServerName As String = MyBase.GetXmlValue("/Configuration/ServerName")
                    ServerName = Regex.Replace(ServerName, "(?im)\d+\.\d+\.\d+\.\d", System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4))
                    MyBase.SetXmlValue("/Configuration/ServerName", ServerName)
                End If
            End If
            MyBase.SaveXmlFile()
        End Sub

        Public Overrides Function GetDefaultXml() As System.Xml.XmlDocument
            Dim RVal As New XmlDocument

            Dim Attribute As XmlAttribute
            'Setup the root node
            Dim ConfigurationNode As XmlElement = RVal.CreateNode(XmlNodeType.Element, "Configuration", "")

            'Add the root node to the document
            RVal.AppendChild(ConfigurationNode)

            'Create the ServerName Node
            Dim SmtpServerName As XmlElement = RVal.CreateNode(XmlNodeType.Element, "ServerName", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "Majodio ESMTP Version " & System.Reflection.Assembly.GetCallingAssembly.GetName.Version.ToString(4)
            SmtpServerName.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpServerName)

            'Server DNS Name
            Dim ServerDnsName As XmlElement = RVal.CreateNode(XmlNodeType.Element, "ServerDnsName", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = GetFqdn()
            SmtpServerName.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(ServerDnsName)

            'Create the ServerIp Node
            Dim SmtpServerIp As XmlElement = RVal.CreateNode(XmlNodeType.Element, "ServerIp", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = GetLocalIpAddress()
            SmtpServerIp.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpServerIp)

            'Create the WelcomeMsg Node
            Dim SmtpWelcomeMsg As XmlElement = RVal.CreateNode(XmlNodeType.Element, "WelcomeMsg", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "Welcome to the Majodio ESMTP Server"
            SmtpWelcomeMsg.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpWelcomeMsg)

            'Create the Goodbye Node
            Dim SmtpGoodbyeMsg As XmlElement = RVal.CreateNode(XmlNodeType.Element, "GoodbyeMsg", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "Goodbye"
            SmtpGoodbyeMsg.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpGoodbyeMsg)

            'Create the RelayIP Node
            Dim SmtpRelayIps As XmlElement = RVal.CreateNode(XmlNodeType.Element, "RelayIp", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "255.255.255.255"
            SmtpRelayIps.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpRelayIps)

            'Queue Check Interval
            Dim QueueCheckInterval As XmlElement = RVal.CreateNode(XmlNodeType.Element, "QueueCheckInterval", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "30000"
            QueueCheckInterval.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(QueueCheckInterval)

            'Send Usage Information
            Dim SmtpSendUsageInformation As XmlElement = RVal.CreateNode(XmlNodeType.Element, "SendUsageInformation", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "True"
            SmtpSendUsageInformation.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpSendUsageInformation)

            'DNSBL
            Dim SmtpDnsbl As XmlElement = RVal.CreateNode(XmlNodeType.Element, "DNSBL", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "sbl-xbl.spamhaus.org"
            SmtpDnsbl.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpDnsbl)

            'Scan Message Links
            Dim ScanMessageLinks As XmlElement = RVal.CreateNode(XmlNodeType.Element, "ScanMessageLinks", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = True
            ScanMessageLinks.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(ScanMessageLinks)

            'Failed Mail Retry Attempts
            Dim FailedMailRetryAttempts As XmlElement = RVal.CreateNode(XmlNodeType.Element, "FailedMailRetryAttempts", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "5"
            FailedMailRetryAttempts.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(FailedMailRetryAttempts)

            'Encyrpt Email
            Dim EncryptEmail As XmlElement = RVal.CreateNode(XmlNodeType.Element, "EncryptEmail", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "true"
            EncryptEmail.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(EncryptEmail)

            'SMTP TCP Port
            Dim SmtpTcpPort As XmlElement = RVal.CreateNode(XmlNodeType.Element, "SmtpTcpPort", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = SMTP_DEFAULT_TCP_PORT
            SmtpTcpPort.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SmtpTcpPort)

            'POP3 TCP Port
            Dim Pop3TcpPort As XmlElement = RVal.CreateNode(XmlNodeType.Element, "Pop3TcpPort", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = POP3_DEFAULT_TCP_PORT
            Pop3TcpPort.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(Pop3TcpPort)

            'POP3 Secure TCP Port
            Dim Pop3SecureTcpPort As XmlElement = RVal.CreateNode(XmlNodeType.Element, "Pop3SecureTcpPort", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = POP3_DEFAULT_SECURE_TCP_PORT
            Pop3SecureTcpPort.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(Pop3SecureTcpPort)

            'IMAP4 TCP Port
            Dim Imap4TcpPort As XmlElement = RVal.CreateNode(XmlNodeType.Element, "Imap4TcpPort", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = IMAP4_DEFAULT_TCP_PORT
            Imap4TcpPort.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(Imap4TcpPort)

            'Retain Messages
            Dim RetainMessages As XmlElement = RVal.CreateNode(XmlNodeType.Element, "RetainMessages", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = False
            RetainMessages.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(RetainMessages)

            'Require SMTP From
            Dim RequireSmtpFrom As XmlElement = RVal.CreateNode(XmlNodeType.Element, "RequireSmtpFrom", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = True
            RequireSmtpFrom.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(RequireSmtpFrom)

            'Max Pop3 Connections Per Interval
            Dim MaxPop3ConnectionsPerInterval As XmlElement = RVal.CreateNode(XmlNodeType.Element, "MaxPop3ConnectionsPerInterval", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = True
            MaxPop3ConnectionsPerInterval.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(MaxPop3ConnectionsPerInterval)

            'Max Pop3 Connection Interval
            Dim MaxPop3ConnectionInterval As XmlElement = RVal.CreateNode(XmlNodeType.Element, "MaxPop3ConnectionInterval", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = True
            MaxPop3ConnectionInterval.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(MaxPop3ConnectionInterval)

            'Postmaster Account Name
            Dim PostmasterAccountName As XmlElement = RVal.CreateNode(XmlNodeType.Element, "PostmasterAccountName", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = SMTP_POSTMASTER_ACCOUNT_NAME
            PostmasterAccountName.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(PostmasterAccountName)

            'SSLCertificate Path
            Dim SSLCertificatePath As XmlElement = RVal.CreateNode(XmlNodeType.Element, "SSLCertificatePath", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = String.Empty
            SSLCertificatePath.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(SSLCertificatePath)

            'Required From Match Incoming IP
            Dim RequireFromMatchIncomingIp As XmlElement = RVal.CreateNode(XmlNodeType.Element, "RequireFromMatchIncomingIp", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "False"
            RequireFromMatchIncomingIp.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(RequireFromMatchIncomingIp)

            'Accept All Mail
            Dim acceptAllMail As XmlElement = RVal.CreateNode(XmlNodeType.Element, "AcceptAllMail", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "False"
            RequireFromMatchIncomingIp.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(acceptAllMail)

            'Server Admin
            Dim ServerAdminAccountName As XmlElement = RVal.CreateNode(XmlNodeType.Element, "ServerAdmin", "")
            Attribute = RVal.CreateAttribute("username")
            Attribute.Value = SERVER_ADMIN_ACCOUNT_USERNAME
            ServerAdminAccountName.Attributes.Append(Attribute)
            Attribute = RVal.CreateAttribute("password")
            Attribute.Value = SERVER_ADMIN_ACCOUNT_PASSWORD
            ServerAdminAccountName.Attributes.Append(Attribute)
            ConfigurationNode.AppendChild(ServerAdminAccountName)

            Return RVal
        End Function

#Region "Configuration Settings"
        Private Const READER_WRITER_LOCK_TIMEOUT As Integer = 5000

        Private Shared _configLock As New System.Threading.ReaderWriterLock
        Public Property ServerAdminUsername() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/ServerAdmin", "username", _configLock)
            End Get
            Set(ByVal value As String)
                MyBase.SetXmlValue("/Configuration/ServerAdmin", "username", value, _configLock)
            End Set
        End Property

        Public Property ServerAdminPassword() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/ServerAdmin", "password", _configLock)
            End Get
            Set(ByVal value As String)
                MyBase.SetXmlValue("/Configuration/ServerAdmin", "password", value, _configLock)
            End Set
        End Property

        Public Property ServerName() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/ServerName", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/ServerName", Value, _configLock)
            End Set
        End Property

        Public Property ServerDnsName() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/ServerDnsName", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/ServerDnsName", Value, _configLock)
            End Set
        End Property

        Public Property SSLCertificatePath() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/SSLCertificatePath", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/SSLCertificatePath", Value, _configLock)
            End Set
        End Property

        Public Property ServerIp() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/ServerIp", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/ServerIp", Value, _configLock)
            End Set
        End Property

        Public Property WelcomeMsg() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/WelcomeMsg", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/WelcomeMsg", Value, _configLock)
            End Set
        End Property

        Public Property GoodbyeMsg() As String
            Get
                Return MyBase.GetXmlValue("/Configuration/GoodbyeMsg", _configLock)
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/GoodbyeMsg", Value, _configLock)
            End Set
        End Property

        Public Property QueueCheckInterval() As Integer
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/QueueCheckInterval", _configLock)
                Dim rval As Integer = 20
                If IsNumeric(TempVal) Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/QueueCheckInterval", Value, _configLock)
            End Set
        End Property

        Public Property SendUsageInformation() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/SendUsageInformation", _configLock)
                Dim rval As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/SendUsageInformation", Value, _configLock)
            End Set
        End Property

        Public Property EncryptEmail() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/EncryptEmail", _configLock)
                Dim rval As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/EncryptEmail", Value, _configLock)
            End Set
        End Property

        Public Property ScanMessageLinks() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/ScanMessageLinks", _configLock)
                Dim RVal As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    RVal = TempVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/ScanMessageLinks", Value, _configLock)
            End Set
        End Property

        Public Property SmtpTcpPort() As Integer
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/SmtpTcpPort", _configLock)
                Dim rval As Integer = SMTP_DEFAULT_TCP_PORT
                If IsNumeric(TempVal) Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/SmtpTcpPort", Value, _configLock)
            End Set
        End Property

        Public Property Pop3TcpPort() As Integer
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/Pop3TcpPort", _configLock)
                Dim rval As Integer = POP3_DEFAULT_TCP_PORT
                If IsNumeric(TempVal) Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/Pop3TcpPort", Value, _configLock)
            End Set
        End Property

        Public Property Pop3SecureTcpPort() As Integer
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/Pop3SecureTcpPort", _configLock)
                Dim rval As Integer = POP3_DEFAULT_SECURE_TCP_PORT
                If IsNumeric(TempVal) Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/Pop3SecureTcpPort", Value, _configLock)
            End Set
        End Property

        Public Property Imap4TcpPort() As Integer
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/Imap4TcpPort", _configLock)
                Dim rval As Integer = IMAP4_DEFAULT_TCP_PORT
                If IsNumeric(TempVal) Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/Imap4TcpPort", Value, _configLock)
            End Set
        End Property

        Public Property FailedMailRetryAttempts() As Integer
            Get
                Dim TmpVal As Object
                Dim RVal As Integer = 5
                TmpVal = MyBase.GetXmlValue("/Configuration/FailedMailRetryAttempts", _configLock)
                If IsNumeric(TmpVal) Then
                    RVal = TmpVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/FailedMailRetryAttempts", Value, _configLock)
            End Set
        End Property

        Public Property RetainMessages() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/RetainMessages", _configLock)
                Dim RVal As Boolean = False
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    RVal = TempVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/RetainMessages", Value, _configLock)
            End Set
        End Property

        Public Property RequireSmtpFrom() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/RequireSmtpFrom", _configLock)
                Dim RVal As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    RVal = TempVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/RequireSmtpFrom", Value, _configLock)
            End Set
        End Property

        Public Property RequireFromMatchIncomingIp() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/RequireFromMatchIncomingIp", _configLock)
                Dim rval As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/RequireFromMatchIncomingIp", Value, _configLock)
            End Set
        End Property

        Public Property AcceptAllMail() As Boolean
            Get
                Dim TempVal As Object = MyBase.GetXmlValue("/Configuration/AcceptAllMail", _configLock)
                Dim rval As Boolean = True
                If Not IsNothing(TempVal) AndAlso CType(TempVal, String).Trim.Length > 0 Then
                    rval = TempVal
                End If
                Return rval
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/Configuration/AcceptAllMail", Value, _configLock)
            End Set
        End Property

        Public Property MaxPop3ConnectionsPerInterval() As Integer
            Get
                Dim TmpVal As Object
                Dim RVal As Integer = 0
                TmpVal = MyBase.GetXmlValue("/Configuration/MaxPop3ConnectionsPerInterval", _configLock)
                If IsNumeric(TmpVal) Then
                    RVal = TmpVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/Configuration/MaxPop3ConnectionsPerInterval", Value, _configLock)
            End Set
        End Property

        Public Property MaxPop3ConnectionInterval() As Int64
            Get
                Dim TmpVal As Object
                Dim RVal As Int64 = TimeSpan.TicksPerHour
                TmpVal = MyBase.GetXmlValue("/Configuration/MaxPop3ConnectionInterval", _configLock)
                If IsNumeric(TmpVal) Then
                    RVal = TmpVal
                End If
                Return RVal
            End Get
            Set(ByVal value As Int64)
                MyBase.SetXmlValue("/Configuration/MaxPop3ConnectionInterval", value, _configLock)
            End Set
        End Property

        Public Property PostmasterAccountName() As String
            Get
                Dim TmpVal As Object
                Dim RVal As String = SMTP_POSTMASTER_ACCOUNT_NAME

                TmpVal = MyBase.GetXmlValue("/Configuration/PostmasterAccountName", _configLock)
                If Not IsNothing(TmpVal) Then
                    RVal = TmpVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As String)
                MyBase.SetXmlValue("/Configuration/PostmasterAccountName", Value, _configLock)
            End Set
        End Property
#Region " RelayIps"
        Public Property RelayIps() As RelayIpCollection
            Get
                Dim rVal As New RelayIpCollection
                Dim replayIps As IPAddress()
                replayIps = GetRelayIps()
                For i As Integer = 0 To replayIps.GetUpperBound(0)
                    rVal.Add(New RelayIp(replayIps(i).ToString))
                Next
                Return rVal
            End Get
            Set(ByVal value As RelayIpCollection)
                ClearRelayIps()
                For i As Integer = 0 To value.Count - 1
                    AddRelayIp(value(i).ConvertToIpAddress)
                Next
            End Set
        End Property

        Public Sub ClearRelayIps()
            Dim elements As XmlNodeList = MyBase.GetXmlElements("/Configuration/RelayIp", _configLock)
            For Each element As XmlElement In elements
                MyBase.DeleteXmlElement(element)
            Next
        End Sub

        Public Function GetRelayIps() As IPAddress()
            Dim RVal As IPAddress() = Nothing
            Dim RelayIps As XmlNodeList
            Dim i As Integer
            RelayIps = MyBase.GetXmlElements("/Configuration/RelayIp", _configLock)
            If Not IsNothing(RelayIps) Then
                ReDim RVal(RelayIps.Count - 1)
                For i = 0 To RelayIps.Count - 1
                    RVal(i) = IPAddress.Parse(RelayIps(i).Attributes("value").Value)
                Next
            End If
            Return RVal
        End Function

        Public Function RelayIpExists(ByVal Ip As IPAddress) As Boolean
            Dim RVal As Boolean
            If Not IsNothing(Ip) Then
                RVal = Not IsNothing(MyBase.GetSingleXmlElement("/Configuration/RelayIp[@value=""" & Ip.ToString & """]", _configLock))
            Else
                RVal = False
            End If
            Return RVal
        End Function

        Public Sub AddRelayIp(ByVal ip As String)
            If Not IsNothing(ip) Then
                AddRelayIp(IPAddress.Parse(ip))
            End If
        End Sub

        Public Sub AddRelayIp(ByVal Ip As IPAddress)
            Dim NVCol As New System.Collections.Specialized.NameValueCollection
            If Not IsNothing(Ip) AndAlso Not RelayIpExists(Ip) Then
                NVCol.Add("value", Ip.ToString)
                Dim Element As XmlElement = MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/Configuration", _configLock), "RelayIp", NVCol, _configLock)
                'MyBase.SaveXmlFile()
            End If
        End Sub

        Public Sub DeleteRelayIp(ByVal Ip As IPAddress)
            If RelayIpExists(Ip) Then
                MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/RelayIp[@value=""" & Ip.ToString & """]", _configLock), _configLock)
            End If
        End Sub
#End Region

#Region " DNSBL"
        Public Property Dnsbls() As DnsblCollection
            Get
                Dim rVal As New DnsblCollection
                Dim dnsblsString As String()
                dnsblsString = GetDnsbl()
                For i As Integer = 0 To dnsblsString.GetUpperBound(0)
                    rVal.Add(New Dnsbl(dnsblsString(i)))
                Next
                Return rVal
            End Get
            Set(ByVal value As DnsblCollection)
                Dim dnsblsString As String()
                dnsblsString = GetDnsbl()
                For i As Integer = 0 To dnsblsString.GetUpperBound(0)
                    DeleteDnsbl(dnsblsString(i))
                Next
                For i As Integer = 0 To value.Count - 1
                    AddDnsbl(value(i).ToString)
                Next
            End Set
        End Property

        Public Function GetDnsbl() As String()
            Dim RVal As String() = Nothing
            Dim Dnsbls As XmlNodeList
            Dim i As Integer
            Dnsbls = MyBase.GetXmlElements("/Configuration/DNSBL", _configLock)
            If Not IsNothing(Dnsbls) Then
                ReDim RVal(Dnsbls.Count - 1)
                For i = 0 To Dnsbls.Count - 1
                    RVal(i) = Dnsbls(i).Attributes("value").Value
                Next
            End If
            Return RVal
        End Function

        Public Function DnsblExists(ByVal Dnsbl As String) As Boolean
            Dim RVal As Boolean
            If Not IsNothing(Dnsbl) Then
                RVal = Not IsNothing(MyBase.GetSingleXmlElement("/Configuration/DNSBL[@value=""" & Dnsbl & """]", _configLock))
            Else
                RVal = False
            End If
            Return RVal
        End Function

        Public Sub ClearDnsbls()
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/DNSBL", _configLock))
        End Sub

        Public Sub AddDnsbl(ByVal dnsbl As Dnsbl)
            AddDnsbl(dnsbl.DNSBL)
        End Sub

        Public Sub AddDnsbl(ByVal Dnsbl As String)
            Dim NVCol As New System.Collections.Specialized.NameValueCollection
            If Not IsNothing(Dnsbl) AndAlso Not DnsblExists(Dnsbl) Then
                NVCol.Add("value", Dnsbl)
                Dim Element As XmlElement = MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/Configuration", _configLock), "DNSBL", NVCol, _configLock)
                'MyBase.SaveXmlFile()
            End If
        End Sub

        Public Sub DeleteDnsbl(ByVal Dnsbl As String)
            If DnsblExists(Dnsbl) Then
                MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/DNSBL[@value=""" & Dnsbl & """]", _configLock))
            End If
        End Sub
#End Region
#Region " Banned IP's"
        Public Property BannedIps() As BannedIpCollection
            Get
                Dim rVal As New BannedIpCollection
                Dim Ips As System.Net.IPAddress()
                Ips = GetBannedIps()
                For i As Integer = 0 To Ips.GetUpperBound(0)
                    rVal.Add(New BannedIp(Ips(i).ToString))
                Next
                Return rVal
            End Get
            Set(ByVal value As BannedIpCollection)
                Dim Ips As System.Net.IPAddress()
                Ips = GetBannedIps()
                For i As Integer = 0 To Ips.GetUpperBound(0)
                    DeleteBannedIp(Ips(i))
                Next
                For i As Integer = 0 To value.Count - 1
                    AddBannedIp(value(i).ConvertToIpAddress)
                Next
            End Set
        End Property

        Public Function BannedIpExists(ByVal Ip As IPAddress) As Boolean
            Dim RVal As Boolean
            If Not IsNothing(Ip) Then
                RVal = Not IsNothing(MyBase.GetSingleXmlElement("/Configuration/BannedIp[@value=""" & Ip.ToString & """]", _configLock))
            Else
                RVal = False
            End If
            Return RVal
        End Function

        Public Function GetBannedIps() As IPAddress()
            Dim RVal As IPAddress() = Nothing
            Dim BannedIps As XmlNodeList
            Dim i As Integer
            BannedIps = MyBase.GetXmlElements("/Configuration/BannedIp", _configLock)
            If Not IsNothing(BannedIps) Then
                ReDim RVal(BannedIps.Count - 1)
                For i = 0 To BannedIps.Count - 1
                    RVal(i) = IPAddress.Parse(BannedIps(i).Attributes("value").Value)
                Next
            End If
            Return RVal
        End Function

        Public Sub ClearBannedIps()
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/BannedIp", _configLock), _configLock)
        End Sub

        Public Sub AddBannedIp(ByVal Ip As IPAddress)
            Dim NVCol As New System.Collections.Specialized.NameValueCollection
            If Not IsNothing(Ip) AndAlso Not BannedIpExists(Ip) Then
                NVCol.Add("value", Ip.ToString)
                Dim Element As XmlElement = MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/Configuration", _configLock), "BannedIp", NVCol, _configLock)
                'MyBase.SaveXmlFile()
            End If
        End Sub

        Public Sub AddBannedIp(ByVal ip As String)
            If Not IsNothing(ip) Then
                AddBannedIp(IPAddress.Parse(ip))
            End If
        End Sub

        Public Sub AddBannedIp(ByVal Ips As BannedIpCollection)
            For i As Integer = 0 To Ips.Count - 1
                AddBannedIp(Ips(i).ConvertToIpAddress)
            Next
        End Sub

        Public Sub DeleteBannedIp(ByVal Ip As IPAddress)
            If BannedIpExists(Ip) Then
                MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/BannedIp[@value=""" & Ip.ToString & """]", _configLock))
            End If
        End Sub
#End Region
#Region " Spam Rules"
        Public Property SpamRules() As SpamRuleCollection
            Get
                Dim rVal As New SpamRuleCollection
                Dim rule As SpamRule
                Dim nodeList As XmlNodeList

                nodeList = MyBase.GetXmlElements("/Configuration/SpamRules/Rule", _configLock)
                If Not IsNothing(nodeList) Then
                    For i As Integer = 0 To nodeList.Count - 1
                        rule = New SpamRule(nodeList(i).Attributes("name").Value, nodeList(i).Attributes("regex").Value, Integer.Parse(nodeList(i).Attributes("order").Value), Integer.Parse(nodeList(i).Attributes("action").Value), Boolean.Parse(nodeList(i).Attributes("ApplyActionIfRegexMatches").Value))
                        rVal.Add(rule)
                    Next

                    rVal.EnsureNoOrderGaps()
                End If

                Return rVal
            End Get
            Set(ByVal value As SpamRuleCollection)
                MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/SpamRules", _configLock))

                For i As Integer = 0 To value.Count - 1
                    AddSpamRule(value(i))
                Next
            End Set
        End Property

        Public Sub AddSpamRule(ByVal rule As SpamRule)
            Dim attributes As New System.Collections.Specialized.NameValueCollection

            If IsNothing(MyBase.GetSingleXmlElement("/Configuration/SpamRules", _configLock)) Then
                MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/Configuration", _configLock), "SpamRules", _configLock)
            End If

            If Not SpamRuleExists(rule.Name) Then
                attributes.Add("name", rule.Name.ToLower)
                attributes.Add("regex", rule.RegEx)
                attributes.Add("action", rule.Action)
                attributes.Add("order", rule.Order)
                attributes.Add("ApplyActionIfRegexMatches", rule.ApplyActionIfRegexMatches)
                MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/Configuration/SpamRules", _configLock), "Rule", attributes, _configLock)
            End If
        End Sub

        Public Function SpamRuleExists(ByVal ruleName As String) As Boolean
            Dim rVal As Boolean

            If IsNothing(ruleName) Then
                rVal = False
            Else
                rVal = Not IsNothing(MyBase.GetSingleXmlElement("/Configuration/SpamRules/Rule[@name=""" & ruleName.ToLower & """]", _configLock))
            End If

            Return rVal
        End Function

        Public Sub ClearSpamRules()
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/SpamRules", _configLock))
        End Sub
        Public Sub DeleteSpamRule(ByVal ruleName As String)
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/Configuration/SpamRules/Rule[@name=""" & ruleName.ToLower & """]", _configLock))
        End Sub
#End Region
#End Region
    End Class
End Namespace