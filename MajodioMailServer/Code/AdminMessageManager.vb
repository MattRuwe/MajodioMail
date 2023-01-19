Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports System.Reflection

Imports Majodio.Mail.Common
Imports Majodio.Mail.Common.Configuration
Imports Majodio.Mail.Common.Storage.QueuedMail

'Imports domain = majodio.Mail.Common.Configuration.remoteconfigclient

Public Class AdminMessageManager
    Private _adminMailTimer As Majodio.Common.Timer
    'Private _config As Majodio.Mail.Common.Configuration.Config

    Public Sub New()
        _adminMailTimer = New Majodio.Common.Timer(AddressOf TimerElapsed, ADMIN_MESSAGE_MAnAGER_POLL_INTERVAL)
        '_config = New Majodio.Mail.Common.Configuration.Config
    End Sub

    Public Sub Start()
        _adminMailTimer.Start()
    End Sub

    Public Sub [Stop]()
        _adminMailTimer.Stop()
    End Sub

    Private Sub TimerElapsed()
        Dim adminMessages As MessageCollection
        adminMessages = FindMessages()

        For i As Integer = 0 To adminMessages.Count - 1
            Try
                ProcessMessage(adminMessages(i))
                SendResponseMessage(adminMessages(i), "Your command was executed successfully", True)
            Catch ex As AdminMessageProcessException
                SendResponseMessage(adminMessages(i), ex.Message & vbCrLf & ex.Details, False)
            Catch ex As Exception
                SendResponseMessage(adminMessages(i), ex.Message & vbCrLf & ex.StackTrace, False)
            End Try
            Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.DeleteMessage(adminMessages(i).MessageId, False)
        Next
    End Sub

    Private Function FindMessages() As MessageCollection
        Dim rVal As New MessageCollection
        rVal.GetAllAdminMessages()

        Return rVal
    End Function

    Private Sub ProcessMessage(ByVal queuedMessage As Message)
        Dim mimeMessage As Mime.Message
        Dim command As AdminCommand

        If IsNothing(queuedMessage) Then
            Throw New ArgumentNullException("queuedMessage")
        End If

        mimeMessage = New Mime.Message(queuedMessage)
        command = GetCommand(mimeMessage)
        If IsCommandAuthenticated(command) Then
            ProcessCommand(command)
        Else
            Throw New AdminMessageProcessException("The password that you issued is not correct")
        End If
    End Sub

    Private Function GetCommand(ByVal message As Mime.Message) As AdminCommand
        Dim rVal As New AdminCommand
        Dim argumentsBodyPart As Mime.MessageBodyPart

        If IsNothing(message) Then
            Throw New ArgumentNullException("message")
        End If

        rVal.Command = message.Subject.ToLower

        argumentsBodyPart = message.FindBodyPart(New Mime.Headers.ContentType("text", "plain"))

        If IsNothing(argumentsBodyPart) Then
            Throw New AdminMessageProcessException("Could not find a MIME message part with a content type of text/plain.")
        End If

        rVal.Parameters = ParseParameters(argumentsBodyPart.GetCanonicalFormContent(False))

        Return rVal
    End Function

    Private Function ParseParameters(ByVal rawText As String) As NameValueCollection
        Dim rVal As New NameValueCollection

        Dim commandNameValues As MatchCollection

        commandNameValues = Regex.Matches(rawText, "(?im)(?<name>^.*?)\W*:\W*(?<value>.*?)\W*$")

        For i As Integer = 0 To commandNameValues.Count - 1
            If IsNothing(rVal(commandNameValues(i).Groups("name").Value.ToLower)) Then
                rVal.Add(commandNameValues(i).Groups("name").Value.ToLower, commandNameValues(i).Groups("value").Value)
            End If
        Next

        Return rVal
    End Function

    Private Function IsCommandAuthenticated(ByVal command As AdminCommand) As Boolean
        Dim rVal As Boolean = False
        If IsNothing(command) OrElse IsNothing(command.Parameters) OrElse IsNothing(command.Parameters("adminpassword")) Then
            Throw New AdminMessageProcessException("The AdminPassword parameter was not found", GetHelpText)
        End If
        If RemoteConfigClient.RemoteConfig.ServerAdminPassword = command.Parameters("adminpassword") Then
            rVal = True
        End If

        Return rVal
    End Function

    Private Function ProcessCommand(ByVal command As AdminCommand) As Boolean
        Dim rVal As Boolean = True
        If IsNothing(command) Then
            Throw New ArgumentNullException("command")
        End If
        Select Case command.Command.ToLower
            Case "addaccount"
                If IsNothing(command.Parameters("emailaddress")) Then
                    Throw New AdminMessageProcessException("The AddAccount command requires the EmailAddress parameter be set")
                End If

                If IsNothing(command.Parameters("emailpassword")) Then
                    Throw New AdminMessageProcessException("The AddAccount command requires the EmailPassword parameter be set")
                End If
                AddAccount(command.Parameters("emailaddress"), command.Parameters("emailpassword"))
            Case "deleteaccount"
                If IsNothing(command.Parameters("emailaddress")) Then
                    Throw New AdminMessageProcessException("The DeleteAccount command requires the EmailAddress parameter be set")
                End If
                DeleteAccount(command.Parameters("emailaddress"))
            Case "changepassword"
                If IsNothing(command.Parameters("emailaddress")) Then
                    Throw New AdminMessageProcessException("The ChangePassword command requires the EmailAddress parameter be set")
                End If

                If IsNothing(command.Parameters("newpassword")) Then
                    Throw New AdminMessageProcessException("The ChangePassword command requires the NewPassword parameter be set")
                End If

                ChangePassword(command.Parameters("emailaddress"), command.Parameters("newpassword"))
            Case "addalias"
                If IsNothing(command.Parameters("aliasemailaddress")) Then
                    Throw New AdminMessageProcessException("The AddAlias command requires the AliasEmailAddress parameter be set")
                End If

                If IsNothing(command.Parameters("destinationemailaddress")) Then
                    Throw New AdminMessageProcessException("The AddAlias command requires the DestinationEmailAddress parameter be set")
                End If

                If IsNothing(command.Parameters("isregex")) Then
                    Throw New AdminMessageProcessException("The AddAlias command requires the IsRegex parameter be set")
                End If

                AddAlias(command.Parameters("aliasemailaddress"), command.Parameters("destinationemailaddress"), Boolean.Parse(command.Parameters("isregex")))
            Case "deletealias"
                If IsNothing(command.Parameters("aliasemailaddress")) Then
                    Throw New AdminMessageProcessException("The DeleteAlias command requires the AliasEmailAddress parameter be set")
                End If

                DeleteAlias(command.Parameters("aliasemailaddress"))
            Case "deletealiasrealaddress"
                If IsNothing(command.Parameters("aliasemailaddress")) Then
                    Throw New AdminMessageProcessException("The DeleteAliasRealAddress command requires the AliasEmailAddress parameter be set")
                End If

                If IsNothing(command.Parameters("realemailaddress")) Then
                    Throw New AdminMessageProcessException("The DeleteAliasRealAddress command requires the RealEmailAddress parameter be set")
                End If

                DeleteAliasRealAddress(command.Parameters("aliasemailaddress"), command.Parameters("realemailaddress"))
            Case "addspamrule"

            Case Else
                Throw New AdminMessageProcessException("The command (" & command.Command & ") is not recognized", GetHelpText)
        End Select
        Return rVal
    End Function

    Private Function GetHelpText() As String
        Dim rVal As String

        Dim ioStream As Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Majodio.Mail.Server.AdminMessageHelp.txt")

        Dim textStream As New StreamReader(ioStream)

        rVal = textStream.ReadToEnd()

        Return rVal
    End Function

    Private Sub AddSpamRule(ByVal name As String, ByVal regex As String, ByVal action As String, ByVal applyActionIfRegexMatches As String)
        'Dim config As New Majodio.Mail.Common.Configuration.Config
        Dim newAction As New SpamRuleAction
        Dim rules As SpamRuleCollection = RemoteConfigClient.RemoteConfig.SpamRules

        If RemoteConfigClient.RemoteConfig.SpamRuleExists(name) Then
            Throw New AdminMessageProcessException("The SpamRule (" & name & ") already exists")
        End If

        Try
            System.Text.RegularExpressions.Regex.IsMatch(String.Empty, regex)
        Catch ex As ArgumentException
            Throw New AdminMessageProcessException("The regular expression (" & regex & ") is not valid (" & ex.Message & ")")
        End Try

        Select Case action.Trim.ToLower
            Case "delete"
                newAction = SpamRuleAction.Delete
            Case "mark"
                newAction = SpamRuleAction.Mark
            Case "noaction"
                newAction = SpamRuleAction.NoAction
            Case Else
                Throw New AdminMessageProcessException("The SpamAction (" & action & ") is not recognized")
        End Select

        RemoteConfigClient.RemoteConfig.AddSpamRule(New SpamRule(name, regex, rules.GetMaxOrderNumber + 1, newAction, applyActionIfRegexMatches))
    End Sub

    Private Sub AddAccount(ByVal email As String, ByVal password As String)
        'Dim domain As New Domains
        Dim userDetails As UserDetails
        Dim emailAddress As EmailAddress

        If Not Majodio.Common.EmailAddress.IsValidAddress(email) Then
            Throw New AdminMessageProcessException("The email address (" & email & ") is not a valid email address")
        Else
            emailAddress = New EmailAddress(email)
        End If

        If Not RemoteConfigClient.RemoteDomain.DomainExists(emailAddress.GetDomain) Then
            Throw New AdminMessageProcessException("The domain name (" & emailAddress.GetDomain & ") does not exist on this server")
        End If

        If RemoteConfigClient.RemoteDomain.UserExists(emailAddress.GetDomain, emailAddress.GetUsername) Then
            Throw New AdminMessageProcessException("The user (" & emailAddress.GetUsername & ") already exists.")
        End If

        userDetails = New UserDetails(emailAddress.GetUsername, password, 0)

        RemoteConfigClient.RemoteDomain.AddUser(emailAddress.GetDomain, userDetails)
    End Sub

    Private Sub DeleteAccount(ByVal email As String)
        Dim emailAddress As EmailAddress

        If Not Majodio.Common.EmailAddress.IsValidAddress(email) Then
            Throw New AdminMessageProcessException("The email address (" & email & ") is not valid")
        End If

        emailAddress = New EmailAddress(email)
        If Not RemoteConfigClient.RemoteDomain.UserExists(emailAddress.GetDomain, emailAddress.GetUsername) Then
            Throw New AdminMessageProcessException("The email address (" & email & ") does not exist on this server")
        End If

        RemoteConfigClient.RemoteDomain.DeleteUser(emailAddress.GetDomain, emailAddress.GetUsername)
    End Sub

    Private Sub ChangePassword(ByVal email As String, ByVal newPassword As String)
        DeleteAccount(email)
        AddAccount(email, newPassword)
    End Sub

    Private Sub AddAlias(ByVal email As String, ByVal destinationEmail As String, ByVal isRegEx As Boolean)
        Dim emailaddress As EmailAddress
        Dim destinationEmailAddress As EmailAddress
        Dim aliasDetails As AliasDetails

        If Not Majodio.Common.EmailAddress.IsValidAddress(email) Then
            Throw New AdminMessageProcessException("The alias email address (" & email & ") is not valid")
        End If

        If Not Majodio.Common.EmailAddress.IsValidAddress(destinationEmail) Then
            Throw New AdminMessageProcessException("The destination email address (" & destinationEmail & ") is not valid")
        End If

        emailaddress = New EmailAddress(email)
        destinationEmailAddress = New EmailAddress(destinationEmail)

        If Not RemoteConfigClient.RemoteDomain.DomainExists(emailaddress.GetDomain) Then
            Throw New AdminMessageProcessException("The domain (" & emailaddress.GetDomain & ") does not exist")
        End If
        If Not RemoteConfigClient.RemoteDomain.AliasExists(emailaddress.GetDomain, emailaddress.GetUsername) Then
            aliasDetails = New AliasDetails(emailaddress.GetUsername, isRegEx)
            RemoteConfigClient.RemoteDomain.AddAlias(emailaddress.GetDomain, aliasDetails)
        End If
        If RemoteConfigClient.RemoteDomain.AlaisRealAddressExsists(emailaddress.GetDomain, emailaddress.GetUsername, destinationEmailAddress.ToString(EmailStringFormat.Address)) Then
            Throw New AdminMessageProcessException("The alias real address (" & destinationEmail & ") already exists for the alias email (" & email & ")")
        End If
        RemoteConfigClient.RemoteDomain.AddAliasRealAddress(emailaddress.GetDomain, emailaddress.GetUsername, destinationEmailAddress.ToString(EmailStringFormat.Address))
    End Sub

    Private Sub DeleteAlias(ByVal aliasEmail As String)
        Dim emailaddress As EmailAddress

        If Not Majodio.Common.EmailAddress.IsValidAddress(aliasEmail) Then
            Throw New AdminMessageProcessException("The alias email address (" & aliasEmail & ") is not valid")
        End If

        emailaddress = New EmailAddress(aliasEmail)

        RemoteConfigClient.RemoteDomain.DeleteAlias(emailaddress.GetDomain, emailaddress.GetUsername)
    End Sub

    Private Sub DeleteAliasRealAddress(ByVal aliasEmail As String, ByVal realEmail As String)
        Dim aliasEmailAddress As EmailAddress

        If Not Majodio.Common.EmailAddress.IsValidAddress(aliasEmail) Then
            Throw New AdminMessageProcessException("The alias email address (" & aliasEmail & ") is not valid")
        End If

        If Not Majodio.Common.EmailAddress.IsValidAddress(realEmail) Then
            Throw New AdminMessageProcessException("The alias email address (" & realEmail & ") is not valid")
        End If

        aliasEmailAddress = New EmailAddress(aliasEmail)

        If Not RemoteConfigClient.RemoteDomain.AliasExists(aliasEmailAddress.GetDomain, aliasEmailAddress.GetUsername) Then
            Throw New AdminMessageProcessException("The alias email address (" & aliasEmail & ") does not exist")
        End If

        If Not RemoteConfigClient.RemoteDomain.AlaisRealAddressExsists(aliasEmailAddress.GetDomain, aliasEmailAddress.GetUsername, realEmail) Then
            Throw New AdminMessageProcessException("The real email address (" & realEmail & ") does not exist for alias (" & aliasEmail & ")")
        End If

        RemoteConfigClient.RemoteDomain.DeleteAliasRealAddress(aliasEmailAddress.GetDomain, aliasEmailAddress.GetUsername, realEmail)
    End Sub

    Private Sub SendResponseMessage(ByVal originalMessage As Storage.QueuedMail.Message, ByVal message As String, ByVal successful As Boolean)
        Dim requestMimeMessage As Mime.Message
        Dim responseMimeMessage As Mime.Message
        Dim contentBodypart As Mime.MessageBodyPart
        Dim domainDetails As DomainDetails()
        Dim toIndex As Integer

        requestMimeMessage = New Mime.Message(originalMessage)
        contentBodypart = requestMimeMessage.FindBodyPart(New Mime.Headers.ContentType("text", "plain"))

        If Not IsNothing(contentBodypart) Then
            responseMimeMessage = New Mime.Message(True)
            responseMimeMessage.Subject = "Re: " & requestMimeMessage.Subject
            toIndex = -1

            'Find the domain that the email was originally sent to (and make sure it exists on this server)
            For i As Integer = 0 To requestMimeMessage.ToAddresses.Count - 1
                If RemoteConfigClient.RemoteDomain.DomainExists(requestMimeMessage.ToAddresses(i).GetDomain) And RemoteConfigClient.RemoteConfig.ServerAdminUsername.ToLower.Trim = requestMimeMessage.ToAddresses(i).GetUsername.Trim.ToLower Then
                    toIndex = i
                    Exit For
                End If
            Next

            If toIndex > -1 Then
                'We found the domain name so set the FromAddress
                responseMimeMessage.FromAddress = New EmailAddress(RemoteConfigClient.RemoteConfig.ServerAdminUsername & "@" & requestMimeMessage.ToAddresses(toIndex).GetDomain)
            Else
                'We couldn't find the domain name, so pick the first domain name available on the server
                Dim domainName As String
                domainDetails = RemoteConfigClient.RemoteDomain.GetDomains
                If domainDetails.GetUpperBound(0) > -1 Then
                    domainName = domainDetails(0).Name
                Else
                    'There are no domains on this server, so issue the unknown domain
                    domainName = "unknown.com"
                End If
                'Set the From Address
                responseMimeMessage.FromAddress = New EmailAddress(RemoteConfigClient.RemoteConfig.ServerAdminUsername & "@" & domainName)
            End If

            'Set the body text
            If successful Then
                responseMimeMessage.RootBodyPart.BodyPartContent = message & vbCrLf & vbCrLf & vbCrLf
            Else
                responseMimeMessage.RootBodyPart.BodyPartContent = "The command that you issued failed.  The error was: " & message & vbCrLf & vbCrLf & vbCrLf
            End If
            With responseMimeMessage.RootBodyPart
                .BodyPartContent &= "To: "
                For i As Integer = 0 To requestMimeMessage.ToAddresses.Count - 1
                    .BodyPartContent &= requestMimeMessage.ToAddresses(i).ToString(EmailStringFormat.NameAddress)
                    If i < requestMimeMessage.ToAddresses.Count - 1 Then
                        .BodyPartContent &= ", "
                    End If
                Next
                .BodyPartContent &= vbCrLf
                .BodyPartContent &= "From: " & requestMimeMessage.FromAddress.ToString(EmailStringFormat.NameAddress) & vbCrLf
                .BodyPartContent &= "Subject: " & requestMimeMessage.Subject & vbCrLf & vbCrLf

                .BodyPartContent &= requestMimeMessage.BodyPartContent
            End With

            'Set the SMTP to/from
            responseMimeMessage.SmtpToAddress = requestMimeMessage.SmtpFromAddress
            responseMimeMessage.SmtpFromAddress = requestMimeMessage.SmtpToAddress

            responseMimeMessage.AddReceivedHeader(GetLocalIpAddress, System.Net.IPAddress.Parse(GetLocalIpAddress), System.Net.Dns.GetHostName)
            'Send the message
            responseMimeMessage.GetQueuedMessage(originalMessage.From.GetDomain, originalMessage.From.GetUsername)
        End If
    End Sub
End Class
