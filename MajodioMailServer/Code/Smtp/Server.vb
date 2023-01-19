Imports System.Net
Imports System.Text.RegularExpressions

Imports Majodio.Mail.common.Configuration
Imports Majodio.Mail.Common.smtp

Namespace Smtp
    Public Enum Status
        Connected
        Closed
    End Enum

    Public Class Server
        Private _deliverable As Boolean

        Public Sub New(ByVal ClientIp As IPAddress)
            CurrentStatus = Status.Connected
            _ClientCanProcessServiceExtensions = False
            _IsRelayConnection = False
            _ClientIp = ClientIp
            _deliverable = True
        End Sub

        Public Sub Reset()
            Me.FromAddress = Nothing
            _ToAddresses = New EmailAddressCollection
            _Data = String.Empty
        End Sub

        Private _ClientIp As IPAddress
        Public ReadOnly Property ClientIp() As IPAddress
            Get
                Return _ClientIp
            End Get
        End Property

        Public Property Deliverable() As Boolean
            Get
                Return _deliverable
            End Get
            Set(ByVal value As Boolean)
                _deliverable = value
            End Set
        End Property


        Private _IsRelayConnection As Boolean
        Public ReadOnly Property IsRelayConnection() As Boolean
            Get
                Return _IsRelayConnection
            End Get
        End Property

        Public ReadOnly Property CanRelay() As Boolean
            Get
                Dim RVal As Boolean = False
                Try
                    Dim RelayIps As IPAddress()
                    Dim i As Integer
                    RelayIps = RemoteConfigClient.RemoteConfig.GetRelayIps()
                    For i = 0 To RelayIps.GetUpperBound(0)
                        'WriteLog("Is ClientIP (" & _ClientIp.ToString & ") in the needed range (" & RelayIps(i).ToString & ")")
                        If IsIpInRange(_ClientIp, RelayIps(i)) Then
                            RVal = True
                            Exit For
                        End If
                    Next
                Catch Exc As Exception
                    Log.Logger.WriteLog("An error occurred " & Exc.Source & " " & Exc.Message & " " & Exc.StackTrace)
                End Try
                'WriteLog("Client CanRelay Returning " & RVal)
                Return RVal
            End Get
        End Property

        Private _CurrentStatus As Status
        Public Property CurrentStatus() As Status
            Get
                Return _CurrentStatus
            End Get
            Set(ByVal Value As Status)
                _CurrentStatus = Value
            End Set
        End Property

        Private _ClientCanProcessServiceExtensions As Boolean
        Public Property ClientCanProcessServiceExtensions() As Boolean
            Get
                Return _ClientCanProcessServiceExtensions
            End Get
            Set(ByVal Value As Boolean)
                _ClientCanProcessServiceExtensions = Value
            End Set
        End Property

        Private _ClientReportedAddress As String
        Public ReadOnly Property ClientReportedAddress() As String
            Get
                Return _ClientReportedAddress
            End Get
        End Property

        Private _FromAddress As EmailAddress
        Public Property FromAddress() As EmailAddress
            Get
                If Not IsNothing(_FromAddress) Then
                    Return _FromAddress
                Else
                    Return New EmailAddress
                End If
            End Get
            Set(ByVal Value As EmailAddress)
                _FromAddress = Value
            End Set
        End Property

        Private _ToAddresses As New EmailAddressCollection
        Public Sub AddTo(ByVal [To] As EmailAddress)
            _ToAddresses.Add([To])
        End Sub

        Private _Data As String
        Public Function AddData(ByVal Data As String) As String
            _Data = Data
            Return Data
        End Function

        Public Function GetWelcomeMessage() As String
            Return RemoteConfigClient.RemoteConfig.ServerName()
        End Function

        Public Function GenerateReceivedMessageHeader() As String
            Dim RVal As String
            'Received: from webserver [90.0.0.5] by majodio.com (SMTPD32-6.06) id ABFC2BC012E; Wed, 29 Sep 2004 13:16:28 -0500
            RVal = "Received: from " & Me.ClientReportedAddress & " [" & _ClientIp.ToString & "] by " & RemoteConfigClient.RemoteConfig.ServerName & " id " & GetSerializedDateTime() & "; " & DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss") & " " & GetLocalUtcOffset()
            Return RVal
        End Function

        Private _IsSecureConnection As Boolean = False
        Public ReadOnly Property IsSecureConection() As Boolean
            Get
                Return _IsSecureConnection
            End Get
        End Property

        Public Function ProcessCommand(ByVal Msg As String) As Response()
            Dim RVal As Response()
            Dim Email As EmailAddress
            Dim i As Integer
            Try
                Dim Msgs As String() = ParseMessage(Msg)
                RVal = New Response() {New Response(ResponseCode.CommandUnrecognized)}
                If Not IsNothing(Msgs) Then
                    If Msgs.GetUpperBound(0) > -1 Then
                        Select Case Msgs(0).Trim.ToLower
                            Case "quit"
                                RVal = New Response() {New Response(ResponseCode.Ok, RemoteConfigClient.RemoteConfig.GoodbyeMsg)}
                                CurrentStatus = Status.Closed
                            Case "ehlo"
                                Me.ClientCanProcessServiceExtensions = True
                                If Msgs.GetUpperBound(0) >= 1 Then
                                    _ClientReportedAddress = Msgs(1)
                                Else
                                    _ClientReportedAddress = String.Empty
                                End If
                                RVal = New Response() {New Response(ResponseCode.Ok, "-Hello " & _ClientReportedAddress), _
                                                           New Response(ResponseCode.Ok, "-" & RemoteConfigClient.RemoteConfig.WelcomeMsg)}
                                If Not IsNothing(RemoteConfigClient.RemoteConfig.SSLCertificatePath) AndAlso RemoteConfigClient.RemoteConfig.SSLCertificatePath.Trim.Length > 0 Then
                                    ReDim Preserve RVal(RVal.GetUpperBound(0) + 1)
                                    RVal(RVal.GetUpperBound(0)) = New Response(ResponseCode.Ok, "-STARTTLS")
                                End If
                                ReDim Preserve RVal(RVal.GetUpperBound(0) + 1)
                                RVal(RVal.GetUpperBound(0)) = New Response(ResponseCode.Ok, "PIPELINING")
                            Case "helo"
                                RVal = New Response() {New Response(ResponseCode.Ok, "Hello " & _ClientReportedAddress & " " & RemoteConfigClient.RemoteConfig.WelcomeMsg)}
                                Me.ClientCanProcessServiceExtensions = False
                                If Msgs.GetUpperBound(0) >= 1 Then
                                    _ClientReportedAddress = Msgs(1)
                                End If
                            Case "starttls"
                                If Not IsNothing(RemoteConfigClient.RemoteConfig.SSLCertificatePath) AndAlso RemoteConfigClient.RemoteConfig.SSLCertificatePath.Trim.Length > 0 Then
                                    Reset()
                                    _ClientReportedAddress = String.Empty
                                    _IsSecureConnection = True
                                    Reset()
                                    RVal = New Response() {New Response(ResponseCode.ServiceReady, "Ready to start secure connection")}
                                Else
                                    RVal = New Response() {New Response(ResponseCode.CommandUnrecognized)}
                                End If
                            Case "mail"
                                If Msgs.GetUpperBound(0) >= 2 Then
                                    If Msgs(1).Trim.ToLower = "from" And Msgs(2).Trim.Length > 0 Then
                                        RVal = New Response() {New Response(ResponseCode.Ok)}
                                        If Majodio.Common.EmailAddress.IsValidAddress(Msgs(2)) Then
                                            FromAddress = New EmailAddress(Msgs(2))
                                            If RemoteConfigClient.RemoteConfig.RequireFromMatchIncomingIp AndAlso Not CanRelay AndAlso Not IsInboundIpFromDomain(_ClientIp, FromAddress.GetDomain) Then
                                                RVal = New Response() {New Response(ResponseCode.ErrorWhileProcessing, "Your e-mail (" & FromAddress.ToString(EmailStringFormat.Address) & ") does not resolve back to your IP (" & _ClientIp.ToString & ")")}
                                                Performance.IncrementMessagesRejectedInvalidFrom()
                                                FromAddress = Nothing
                                            End If
                                        ElseIf RemoteConfigClient.RemoteConfig.RequireSmtpFrom Then
                                            Throw New InvalidAddressException(Msgs(2))
                                        End If
                                    Else
                                        RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "Invalid RFC Syntax for <" & ClientReportedAddress & ">")}
                                    End If
                                Else
                                    RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "Invalid RFC Syntax for <" & ClientReportedAddress & ">")}
                                End If
                            Case "rcpt"
                                If Msgs.GetUpperBound(0) >= 2 Then
                                    'Make sure the client issued a to
                                    If Msgs(1).Trim.ToLower = "to" And Msgs(2).Trim.Length > 0 Then
                                        'if the server requires a from ensure that the client provided it
                                        If Me.FromAddress.ToString(EmailStringFormat.AddressBraces).Length > 0 Or Not RemoteConfigClient.RemoteConfig.RequireSmtpFrom Then
                                            'RVal = New Response() {New Response(ResponseCode.Ok)}
                                            Email = New EmailAddress(Msgs(2))
                                            Dim RegexAlias As AliasDetails = RemoteConfigClient.RemoteDomain.GetAliasRegExMatch(Email.GetDomain, Email.GetUsername)
                                            If RemoteConfigClient.RemoteDomain.UserExists(Email.GetDomain, Email.GetUsername) Then
                                                AddTo(Email)
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok its for " & Email.ToString(EmailStringFormat.AddressBraces))}
                                            ElseIf RemoteConfigClient.RemoteDomain.AliasExists(Email.GetDomain, Email.GetUsername) AndAlso RemoteConfigClient.RemoteDomain.GetAliasRealAddresses(Email.GetDomain, Email.GetUsername, True).Count > 0 Then
                                                'AddTo(_Domains.GetAliasRealAddress(Email.GetDomain, Email.GetUsername))
                                                Dim Addresses As EmailAddressCollection
                                                Addresses = RemoteConfigClient.RemoteDomain.GetAliasRealAddresses(Email.GetDomain, Email.GetUsername, True)
                                                For i = 0 To Addresses.Count - 1
                                                    AddTo(Addresses(i))
                                                Next
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok its for " & Email.ToString(EmailStringFormat.AddressBraces))}
                                            ElseIf Not IsNothing(RegexAlias) AndAlso RegexAlias.RealAddresses.Count > 0 Then
                                                Dim Addresses As EmailAddressCollection
                                                Addresses = RegexAlias.RealAddresses
                                                For i = 0 To Addresses.Count - 1
                                                    AddTo(Addresses(i))
                                                Next
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok its for " & Email.ToString(EmailStringFormat.AddressBraces))}
                                            ElseIf CanRelay And Not RemoteConfigClient.RemoteDomain.DomainExists(Email.GetDomain) Then
                                                _IsRelayConnection = True
                                                AddTo(Email)
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok will relay this message to " & Email.ToString(EmailStringFormat.AddressBraces))}
                                            ElseIf RemoteConfigClient.RemoteConfig.ServerAdminUsername = Email.GetUsername AndAlso RemoteConfigClient.RemoteDomain.DomainExists(Email.GetDomain) Then
                                                'This message is for server administration
                                                AddTo(Email)
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok message to be sent to the server admin")}
                                            ElseIf RemoteConfigClient.RemoteConfig.AcceptAllMail Then
                                                Email.Tag = False
                                                AddTo(Email)
                                                RVal = New Response() {New Response(ResponseCode.Ok, "ok recipient was not found so the mail will be trashed")}
                                            Else
                                                RVal = New Response() {New Response(ResponseCode.MailboxNotFound, "That mailbox was not found on this system")}
                                            End If
                                        Else
                                            RVal = New Response() {New Response(ResponseCode.BadSequenceOfCommands, "Must have sender first")}
                                        End If
                                    Else
                                        RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "Invalid RFC Syntax for <" & ClientReportedAddress & ">")}
                                    End If
                                Else
                                    RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "Invalid RFC Syntax for <" & ClientReportedAddress & ">")}
                                End If
                            Case "data"
                                RVal = New Response() {New Response(ResponseCode.BadSequenceOfCommands, "Cannot issue data command without any mail recipients")}
                                If Not IsNothing(_ToAddresses) Then
                                    If _ToAddresses.Count > 0 Then
                                        RVal = New Response() {New Response(ResponseCode.StartMailInput)}
                                    End If
                                End If
                            Case "rset"
                                Me.Reset()
                                RVal = New Response() {New Response(ResponseCode.Ok)}
                            Case "noop"
                                RVal = New Response() {New Response(ResponseCode.Ok)}
                            Case "vrfy"
                                If Msgs.GetUpperBound(0) >= 1 Then
                                    If Majodio.Common.EmailAddress.IsValidAddress(Msgs(1)) Then
                                        Email = New EmailAddress(Msgs(1))
                                        If RemoteConfigClient.RemoteDomain.UserExists(Email.GetDomain, Email.GetUsername) Or RemoteConfigClient.RemoteDomain.AliasExists(Email.GetDomain, Email.GetUsername) Then
                                            RVal = New Response() {New Response(ResponseCode.Ok, "User Name " & Email.ToString(EmailStringFormat.AddressBraces))}
                                        Else
                                            RVal = New Response() {New Response(ResponseCode.MailboxNotFound)}
                                        End If
                                    Else
                                        RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "No user entered")}
                                    End If
                                Else
                                    RVal = New Response() {New Response(ResponseCode.SyntaxErrorInParametersOrArguments, "The email entered is invalid")}
                                End If
                            Case Else
                                RVal = New Response() {New Response(ResponseCode.CommandUnrecognized)}
                        End Select
                    End If
                End If
            Catch Exc As InvalidAddressException
                RVal = New Response() {New Response(ResponseCode.ErrorWhileProcessing, "The email address entered is not valid")}
            End Try
            Return RVal
        End Function

        Public Function QueueMail(ByVal MessageContent As String) As Response
            Dim RVal As Response
            AddData(MessageContent)
            Dim i As Integer
            Dim Msg As Majodio.Mail.Common.Storage.QueuedMail.Message
            Dim MessageRejectedDnsbl As Boolean = False
            Dim DnsblResults As DnsblResults
            Dim deliverMessage As Boolean = True

            DnsblResults = Functions.AreMessageLinksInDnsbl(MessageContent)
            If RemoteConfigClient.RemoteConfig.GetDnsbl.Length > 0 AndAlso RemoteConfigClient.RemoteConfig.ScanMessageLinks AndAlso Not CanRelay AndAlso DnsblResults.IsInDnsbl Then
                MessageRejectedDnsbl = True
            End If
            If Not MessageRejectedDnsbl Or RemoteConfigClient.RemoteConfig.AcceptAllMail Then
                MessageContent = RunMessageThroughSpamFilter(MessageContent)
                If Not IsNothing(_ToAddresses) AndAlso Not IsNothing(MessageContent) AndAlso MessageContent.Trim.Length > 0 Then
                    For i = 0 To _ToAddresses.Count - 1
                        If (IsNothing(_ToAddresses(i).Tag) OrElse CType(_ToAddresses(i).Tag, Boolean) = True) AndAlso Deliverable AndAlso Not MessageRejectedDnsbl Then
                            'The message is to a user on this server or going to be forwarded to a relayed user
                            Msg = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage(_ToAddresses(i).GetDomain, _ToAddresses(i).GetUsername)
                            Dim filePath As String = Msg._XmlFilePath
                            deliverMessage = True
                        Else
                            'This message is not for any users on this server and will be put into the undeliverable folder
                            Msg = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage(Majodio.Mail.Common.GetApplicationDirectory & "\" & UNDELIVERABLE_MAIL_FOLDER & "\" & Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.GetNewMessageId.Value & ".xml")
                            deliverMessage = False
                        End If

                        Msg.From = _FromAddress
                        Msg.To = _ToAddresses(i)
                        Msg.DateTime = DateTime.Now
                        Msg.SetMessageContent(GenerateReceivedMessageHeader() & vbCrLf & MessageContent)
                        Log.Logger.WriteLog("Saving Queued Message")
                        Msg.Save()
                        RemoteConfigClient.RemoteDomain.MessageLastReceived(_ToAddresses(i).GetDomain, _ToAddresses(i).GetUsername) = DateTime.Now
                        Log.Logger.WriteLog("Queued Message Saved")

                        'Only notify the plugin if the message was actually delivered
                        If deliverMessage Then
                            Try
                                Plugin.Manager.MailReceived(New Mime.Message(Msg), _IsRelayConnection)
                            Catch ex As Exception
                                Log.Logger.WriteError(ex, "Error occurred while attempting to call ISmtpServer.MailReceived.  Server continued normally.")
                                Dim mailEx As New MailException("Error occurred while attempting to call ISmtpServer.MailReceived.  Server continued normally.", ex)
                                mailEx.Save()
                            End Try
                        End If
                    Next
                End If
                RVal = New Response(ResponseCode.Ok, SMTP_MESSAGE_QUEUED_RESPONSE)
            Else
                Performance.IncrementMessagesRejectedDnsbl()
                RVal = New Response(ResponseCode.ErrorWhileProcessing, "Message rejected: The message contains links listed with " & DnsblResults.DnsblServer)
                Try
                    Plugin.Manager.DnsblBlockedMessage(New Mime.Message(MessageContent), Me.ClientIp)
                Catch ex As Exception
                    Log.Logger.WriteError(ex, "Error occurred while attempting to call ISmtpServer.DnsblBlockedMessage.  Server continued normally.")
                End Try
            End If
            Return RVal
        End Function

        Private Function RunMessageThroughSpamFilter(ByVal messageContent As String) As String
            Dim rVal As String = messageContent
            Dim spamRules As SpamRuleCollection
            Dim message As Mime.Message
            Dim regexMatch As Match
            Dim nextAction As SpamRuleAction
            Try
                spamRules = RemoteConfigClient.RemoteConfig.SpamRules()
                For i As Integer = 0 To spamRules.Count - 1
                    nextAction = SpamRuleAction.NoAction

                    regexMatch = Regex.Match(messageContent, spamRules(i).RegEx)
                    If regexMatch.Success And spamRules(i).ApplyActionIfRegexMatches Then
                        nextAction = spamRules(i).Action
                    ElseIf Not regexMatch.Success And Not spamRules(i).ApplyActionIfRegexMatches Then
                        nextAction = spamRules(i).Action
                    End If

                    Select Case nextAction
                        Case SpamRuleAction.Delete
                            rVal = String.Empty
                        Case SpamRuleAction.Mark
                            message = New Mime.Message(messageContent)
                            message.Headers.Add(New Mime.Headers.OtherHeader("X-MAJODIO-SPAM", "True"))
                            rVal = message.RawMessage
                    End Select
                Next
            Catch ex As Exception

            End Try

            Return rVal
        End Function

        Private Function ParseMessage(ByVal Msg As String) As String()
            Dim RVal As String() = Nothing
            Dim tmpRVal As New ArrayList()
            Dim StartIndex As Integer = 0
            Dim EndIndex As Integer = 0
            Dim tmpMessage As String = String.Empty
            Try
                If Not IsNothing(Msg) Then

                    Msg = Msg.Replace(":", " ")
                    While StartIndex < Msg.Length
                        If Msg.Substring(StartIndex, 1) = """" Then
                            EndIndex = Msg.IndexOf("""", StartIndex)
                        Else
                            EndIndex = Msg.IndexOf(" ", StartIndex)
                        End If
                        If EndIndex = -1 Then
                            EndIndex = Msg.Length
                        End If
                        tmpMessage = Msg.Substring(StartIndex, EndIndex - StartIndex)
                        If tmpMessage.Trim.Length > 0 Then
                            tmpRVal.Add(tmpMessage)
                        End If
                        StartIndex = EndIndex + 1
                        If StartIndex < Msg.Length Then
                            While StartIndex < Msg.Length AndAlso Msg.Substring(StartIndex, 1) = " "
                                StartIndex += 1
                            End While
                        End If
                    End While
                End If
                RVal = tmpRVal.ToArray(GetType(String))
                Return RVal
            Catch ex As Exception
                Dim mailException As New MailException("An exception occurred while attempting to parse the SMTP message", ex)
                mailException.ExceptionItems.Add("PARAMETER", "Msg", Msg)
                Throw mailException
            End Try
        End Function
    End Class
End Namespace