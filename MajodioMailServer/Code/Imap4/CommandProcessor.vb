Imports System.Text.RegularExpressions
Imports System.Text
Imports Majodio.Mail.Common.Configuration
Imports Majodio.Mail.Common.Storage.QueuedMail

Namespace Imap4
    ''' -----------------------------------------------------------------------------
    ''' Project	 : MajodioMailServer
    ''' Class	 : Mail.Server.Imap4.CommandProcessor
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	7/20/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class CommandProcessor
        Private _Command As Command
        Private _Coordinator As QueueCoordinator
        Private _UntaggedResponses As ResponseCollection

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <param name="QueueCoordinator"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal Command As Command, ByRef QueueCoordinator As QueueCoordinator)
            _Command = Command
            _Coordinator = QueueCoordinator
            _UntaggedResponses = New ResponseCollection
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public ReadOnly Property Command() As Command
            Get
                Return _Command
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function ProcessCommand() As ResponseCollection
            Dim RVal As New ResponseCollection
           
            If Command.Command = String.Empty Then
                RVal.Add(New ResponseCollection(New Response(ResponseCode.Bad, "command unknown, or syntax error")))
            Else
                Select Case Majodio.Mail.Server.Imap4.Command.GetCommand(Command.Command)
                    Case ClientCommands.Login
                        RVal.Add(Login(Command))
                    Case ClientCommands.Capability
                        RVal.Add(Capability(Command))
                    Case ClientCommands.Logout
                        RVal.Add(Logout(Command))
                    Case ClientCommands.Noop
                        RVal.Add(Noop(Command))
                    Case ClientCommands.Check
                        RVal.Add(Check(Command))
                    Case ClientCommands.Create
                        RVal.Add(Create(Command))
                    Case ClientCommands.Delete
                        RVal.Add(Delete(Command))
                    Case ClientCommands.Rename
                        RVal.Add(Rename(Command))
                    Case ClientCommands.Select
                        RVal.Add([Select](Command))
                    Case ClientCommands.Examine
                        RVal.Add(Examine(Command))
                    Case ClientCommands.List
                        RVal.Add(List(Command))
                    Case ClientCommands.Lsub
                        RVal.Add(LSub(Command))
                    Case ClientCommands.Subscribe
                        RVal.Add(Subscribe(Command))
                    Case ClientCommands.Unsubscribe
                        RVal.Add(Unsubscribe(Command))
                    Case ClientCommands.Fetch
                        RVal.Add(Fetch(Command))
                    Case ClientCommands.Uid
                        RVal.Add(UID(Command))
                    Case ClientCommands.Status
                        RVal.Add(Status(Command))
                    Case ClientCommands.Close
                        RVal.Add(Close(Command))
                    Case ClientCommands.Store
                        RVal.Add(Store(Command))
                    Case ClientCommands.Expunge
                        RVal.Add(Expunge(Command))
                    Case ClientCommands.Copy
                        RVal.Add(Copy(Command))
                    Case Else
                        If Command.Command.Trim.Length > 0 AndAlso Command.Tag.Trim.Length > 0 Then
                            RVal.Add(New ResponseCollection(New Response(ResponseCode.Bad, "Command " & Command.Command & " unknown", Command.Tag)))
                        ElseIf Command.Command.Trim.Length > 0 Then
                            RVal.Add(New ResponseCollection(New Response(ResponseCode.Bad, "Command " & Command.Command & " unknown")))
                        Else
                            RVal.Add(New ResponseCollection(New Response(ResponseCode.Bad, "Command unknown")))
                        End If
                End Select
            End If
            RVal.Insert(0, GetStatusUpdate)
            Return RVal
        End Function

        Public Function GetStatusUpdate() As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Watcher As Majodio.Mail.Common.Storage.QueuedMail.SystemWatcher
            Dim SelectedFolder As Majodio.Mail.Common.Storage.QueuedMail.Folder

            SelectedFolder = _Coordinator.SelectedFolder
            Watcher = _Coordinator.QueueSystemWatcher

            If Not IsNothing(Watcher) Then
                Watcher.CheckForChanges()
                If Watcher.NewMessages.Count > 0 Then
                    RVal.Add(New Imap4.Response(ResponseCode.None, SelectedFolder.GetAllMailMessages().Count & " EXISTS"))
                    RVal.Add(New Imap4.Response(ResponseCode.None, Watcher.NewMessages.Count & " RECENT"))
                    RVal.Add(New Imap4.Response(ResponseCode.Ok, "[UNSEEN " & SelectedFolder.Unseen & "] Message " & SelectedFolder.GetFirstUnseenMessageSequence & " is the first unessn message"))
                    Watcher.ResetAll()
                End If

            End If

            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Login(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            If Command.Parameters.Count = 2 And Command.Imap4State = SessionState.NonAuthenticated Then
                Dim Email As New EmailAddress(_Command.Parameters(0).Value)
                If RemoteConfigClient.RemoteDomain.AuthorizeUser(Email.GetDomain, Email.GetUsername, _Command.Parameters(1).Value) Then
                    'Credentials are good
                    Response.ResponseCode = ResponseCode.Ok
                    Response.Tag = Command.Tag
                    Response.ResponseText = "login completed, now in authenticated state"
                    _Coordinator.CurrentUser = Email
                    _Coordinator.CurrentImap4State = SessionState.Authenticated
                    '_Coordinator.SelectedFolder = New Folder(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, String.Empty)
                    _Coordinator.SelectedFolder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, String.Empty)
                Else
                    'Credentials fail
                    Response.ResponseCode = ResponseCode.No
                    Response.Tag = Command.Tag
                    Response.ResponseText = "login failure:  user name or password rejected"
                End If
            ElseIf _Command.Parameters.Count <> 2 Then
                Response.ResponseCode = ResponseCode.Bad
                Response.Tag = Command.Tag
                Response.ResponseText = "command accepts 2 parameters"
            ElseIf Command.Imap4State <> SessionState.NonAuthenticated Then
                Response.ResponseCode = ResponseCode.Bad
                Response.Tag = Command.Tag
                Response.ResponseText = "command not valid unless in non-authenticated state"
            End If
            RVal.Add(Response)
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Logout(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            If Command.Parameters.Count = 0 Then
                RVal.Add(New Response(ResponseCode.Ok, IMAP4_LOGOUT_MESSAGE))

                RVal.Add(New Response(ResponseCode.Ok, "LOUGOUT completed", Command.Tag))
                _Coordinator.CurrentUser = Nothing
                _Coordinator.CurrentImap4State = SessionState.Logout
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Capability(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            RVal.Add(New Response(ResponseCode.None, "CAPABILITY IMAP4 IMAP4rev1"))
            RVal.Add(New Response(ResponseCode.Ok, "CAPABILITY completed", Command.Tag))
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Noop(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            Response.ResponseCode = ResponseCode.Ok
            Response.ResponseText = "NOOP completed"
            Response.Tag = Command.Tag
            RVal.Add(Response)
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/24/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Check(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            If _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "Cannot execute command except in selected state", Command.Tag))
            Else
                If Command.Parameters.Count > 0 Then
                    RVal.Add(New Response(ResponseCode.Bad, "Command does not accept any parameters", Command.Tag))
                Else
                    RVal.Add(New Response(ResponseCode.Ok, "CHECK completed", Command.Tag))
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Create(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            Dim Folder As Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder
            Dim CurrentPath As String = String.Empty
            Dim ArrCurrentPath As String() = Nothing
            If _Coordinator.CurrentImap4State <> SessionState.NonAuthenticated Then
                If Command.Parameters.Count = 1 Then
                    ArrCurrentPath = Split(Command.Parameters(0).Value, "\")
                    For i As Integer = 0 To ArrCurrentPath.GetUpperBound(0)
                        CurrentPath &= ArrCurrentPath(i) & "\"
                        If (i < ArrCurrentPath.GetUpperBound(0) OrElse Not Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, CurrentPath)) _
                           AndAlso Folder.IsFolderPathValid(Command.Parameters(0).Value) Then
                            Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, CurrentPath)
                            If Folder.NewlyCreated AndAlso i < ArrCurrentPath.GetUpperBound(0) Then
                                Folder.NoSelect = True
                            End If
                            Response.ResponseCode = ResponseCode.Ok
                            Response.ResponseText = "CREATE completed"
                            Response.Tag = Command.Tag
                        ElseIf i = ArrCurrentPath.GetUpperBound(0) AndAlso Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, CurrentPath) Then
                            Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, CurrentPath)
                            If Folder.NoSelect Then
                                Folder.NoSelect = False
                                Response.ResponseCode = ResponseCode.Ok
                                Response.ResponseText = "folder created"
                                Response.Tag = Command.Tag
                            Else
                                Response.ResponseCode = ResponseCode.No
                                Response.ResponseText = "create failure: can't create mailbox with that name - already exists"
                                Response.Tag = Command.Tag
                                Exit For
                            End If
                        Else
                            Response.ResponseCode = ResponseCode.No
                            Response.ResponseText = "create failure: can't create mailbox with that name"
                            Response.Tag = Command.Tag
                            Exit For
                        End If
                    Next
                Else
                    Response.ResponseCode = ResponseCode.Bad
                    Response.Tag = Command.Tag
                    Response.ResponseText = "command accepts 1 parameter"
                End If
            Else
                Response.ResponseCode = ResponseCode.No
                Response.ResponseText = "Cannot issue create command in non-authenticated state"
                Response.Tag = Command.Tag
            End If
            RVal.Add(Response)
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Delete(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            Dim QFolder As Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder
            If _Coordinator.CurrentImap4State <> SessionState.NonAuthenticated Then
                If Command.Parameters.Count = 1 Then
                    If QFolder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                        QFolder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value)
                        If QFolder.FolderContainsChildren() And QFolder.NoSelect Then
                            Response.Tag = Command.Tag
                            Response.ResponseCode = ResponseCode.No
                            Response.ResponseText = "delete failure:  can't delete mailbox with that name"
                        Else
                            QFolder.DeleteFolder()
                            Response.Tag = Command.Tag
                            Response.ResponseCode = ResponseCode.Ok
                            Response.ResponseText = "delete completed"
                        End If
                    Else
                        Response.Tag = Command.Tag
                        Response.ResponseCode = ResponseCode.No
                        Response.ResponseText = "delete failure:  can't delete mailbox with that name"
                    End If
                Else
                    Response.ResponseCode = ResponseCode.Bad
                    Response.Tag = Command.Tag
                    Response.ResponseText = "command accepts 1 parameter"
                End If
            Else
                Response.ResponseCode = ResponseCode.No
                Response.ResponseText = "Cannot issue create command in non-authenticated state"
                Response.Tag = Command.Tag
            End If
            RVal.Add(Response)
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Rename(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            Dim Folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            If _Coordinator.CurrentImap4State <> SessionState.NonAuthenticated Then
                If Command.Parameters.Count = 2 Then
                    If Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                        Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value)
                        Folder = Folder.Rename(Command.Parameters(1).Value)
                        If Not IsNothing(Folder) Then
                            Response.Tag = Command.Tag
                            Response.ResponseCode = ResponseCode.Ok
                            Response.ResponseText = "rename completed"
                        Else
                            Response.Tag = Command.Tag
                            Response.ResponseCode = ResponseCode.No
                            Response.ResponseText = "rename failed"
                        End If
                    End If
                Else
                    Response.ResponseCode = ResponseCode.Bad
                    Response.Tag = Command.Tag
                    Response.ResponseText = "command accepts 2 parameters"
                End If
            Else
                Response.ResponseCode = ResponseCode.No
                Response.ResponseText = "Cannot issue rename command in non-authenticated state"
                Response.Tag = Command.Tag
            End If
            RVal.Add(Response)
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function [Select](ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As New Response
            Dim Folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            Dim UnseenMessageIndex As Integer
            If _Coordinator.CurrentImap4State <> SessionState.NonAuthenticated Then
                If Command.Parameters.Count = 1 Then
                    If Command.Parameters(0).Value.Trim.ToLower <> MAILBOX_INBOX.ToLower AndAlso Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                        Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value)
                    ElseIf Command.Parameters(0).Value.Trim.ToLower = MAILBOX_INBOX.ToLower Or Command.Parameters(0).Value.Trim.ToLower = MAILBOX_INBOX.ToLower & "/" Then
                        'The user has selected the inbox folder
                        Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, String.Empty)
                    End If
                    If Not IsNothing(Folder) AndAlso Not Folder.NoSelect Then
                        _Coordinator.SelectedFolderReadonly = False
                        Me._Coordinator.SelectedFolder = Folder

                        Response = New Response(ResponseCode.None, Folder.GetNumberOfMessages & " EXISTS")
                        RVal.Add(Response)

                        Response = New Response(ResponseCode.None, Folder.GetRecentMessages.Count & " RECENT")
                        RVal.Add(Response)
                        UnseenMessageIndex = Folder.GetFirstUnseenMessageSequence
                        If UnseenMessageIndex > 0 Then
                            Response = New Response(ResponseCode.Ok, "[UNSEEN " & UnseenMessageIndex & "] MESSAGE " & UnseenMessageIndex & " is first unseen")
                            RVal.Add(Response)
                        End If

                        Response = New Response(ResponseCode.None, "FLAGS (\Answered \Flagged \Deleted \Seen \Draft)")
                        RVal.Add(Response)

                        Response = New Response(ResponseCode.Ok, "[PERMANENTFLAGS (\Deleted \Seen)] Limited")
                        RVal.Add(Response)

                        Response = New Response(ResponseCode.Ok, "[UIDVALIDITY " & Folder.UniqueId.Value & "] UIDs valid")
                        RVal.Add(Response)

                        Response = New Response(ResponseCode.Ok, "[READ-WRITE] SELECT completed", Command.Tag)
                        RVal.Add(Response)
                        _Coordinator.CurrentImap4State = SessionState.Selected
                    ElseIf Not IsNothing(Folder) AndAlso Folder.NoSelect Then
                        RVal.Add(New Response(ResponseCode.No, "That folder cannot be selected", Command.Tag))
                    Else
                        'The folder cannot be found
                        Response.ResponseCode = ResponseCode.Bad
                        Response.Tag = Command.Tag
                        Response.ResponseText = "The folder " & Command.Parameters(0).Value & " does not exist"
                        RVal.Add(Response)
                    End If
                Else
                    Response.ResponseCode = ResponseCode.Bad
                    Response.Tag = Command.Tag
                    Response.ResponseText = "command accepts 1 parameter"
                    RVal.Add(Response)
                End If
            Else
                Response.ResponseCode = ResponseCode.No
                Response.ResponseText = "Cannot issue select command in non-authenticated state"
                Response.Tag = Command.Tag
                RVal.Add(Response)
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Examine(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Response As Response
            If Command.Parameters.Count <> 1 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 1 parameter", Command.Tag))
            Else
                RVal = [Select](Command)
                For i As Integer = RVal.Count - 1 To 0 Step -1
                    If RVal(i).ResponseText.ToLower.IndexOf("[read-write] select completed") > -1 Then
                        RVal(i).ResponseText = "[READ-ONLY] EXAMINE completed"
                    ElseIf RVal(i).ResponseText.ToLower.IndexOf("[permanentflags (") > -1 Then
                        RVal(i).ResponseText = "[PERMANENTFLAGS ()] No permanent flags permitted"
                    ElseIf RVal(i).ResponseCode = ResponseCode.Bad Or RVal(i).ResponseCode = ResponseCode.No Then
                        Response = RVal(i)
                        RVal.Clear()
                        If Response.Tag.Trim.Length = 0 Then
                            Response.Tag = Command.Tag
                        End If
                        RVal.Add(Response)
                    End If
                Next
            End If

            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function List(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Folders As Majodio.Mail.Common.Storage.QueuedMail.FolderCollection
            Dim CurrentFolder As String
            If Command.Parameters.Count <> 2 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 2 parameters", Command.Tag))
            Else
                'Get a list of all folders
                Folders = Majodio.Mail.Common.Storage.QueuedMail.FolderCollection.RetrieveAllFolders(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername)
                For i As Integer = 0 To Folders.Count - 1
                    CurrentFolder = Folders(i).FolderName
                    If Regex.IsMatch(CurrentFolder, "(?im)" & Command.Parameters(0).Value & Command.Parameters(1).Value.Replace("*", ".*")) Then
                        RVal.Add(New Response(ResponseCode.None, "LIST (\" & IIf(Folders(i).Marked, "Marked", "Unmarked") & ") """ & IMAP4_HEIRARCHY_DELIMETER & """ """ & CurrentFolder.Substring(0, CurrentFolder.Length - 1) & """"))
                    End If
                Next
                RVal.Add(New Response(ResponseCode.Ok, "LIST completed", Command.Tag))
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function LSub(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Folders As String()
            Dim ResponseText As String
            Dim Folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            If Command.Parameters.Count <> 2 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 2 parameters", Command.Tag))
            Else
                Folders = RemoteConfigClient.RemoteDomain.GetSubscriptions(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, "(?i)" & Command.Parameters(0).Value & Command.Parameters(1).Value.Replace("*", ".*"))
                For i As Integer = 0 To Folders.GetUpperBound(0)
                    If Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Folders(i)) Then
                        Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Folders(i))
                        ResponseText = "LSUB ( "
                        If Not Folder.Marked Then
                            ResponseText &= "\Unmarked "
                        Else
                            ResponseText &= "\Marked "
                        End If
                        If Folder.NoSelect Then
                            ResponseText &= "\Noselect "
                        End If
                        ResponseText = ResponseText.Substring(0, ResponseText.Length - 1) & ") "
                        ResponseText &= """" & IMAP4_HEIRARCHY_DELIMETER & """ """ & Folders(i) & """"
                        RVal.Add(New Response(ResponseCode.None, ResponseText))
                    End If
                Next
                RVal.Add(New Response(ResponseCode.Ok, "LSUB completed", Command.Tag))
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Subscribe(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection

            If Command.Parameters.Count <> 1 Then

            Else
                Dim Folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
                If Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                    If Not RemoteConfigClient.RemoteDomain.SubscriptionExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                        RemoteConfigClient.RemoteDomain.AddSubscription(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value)
                        RVal.Add(New Response(ResponseCode.Ok, "SUBSCRIBE completed", Command.Tag))
                    Else
                        RVal.Add(New Response(ResponseCode.Ok, "SUBSCRIBE completed - folder already subscribed to", Command.Tag))
                    End If
                Else
                    RVal.Add(New Response(ResponseCode.No, "The folder specified does not exist", Command.Tag))
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Unsubscribe(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            If Command.Parameters.Count <> 1 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 1 parameter", Command.Tag))
            Else
                If RemoteConfigClient.RemoteDomain.SubscriptionExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value) Then
                    RemoteConfigClient.RemoteDomain.RemoveSubscription(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(0).Value)
                    RVal.Add(New Response(ResponseCode.Ok, "UNSUBSCRIBE completed", Command.Tag))
                Else
                    RVal.Add(New Response(ResponseCode.No, "That folder is not subscribed to", Command.Tag))
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Fetch(ByVal Command As Command) As ResponseCollection
            Return Fetch(Command, False)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <param name="UseUid"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Fetch(ByVal Command As Command, ByVal UseUid As Boolean) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim MC As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
            Dim SecondParameter As String
            Dim FDI As FetchDataItems

            If IsNothing(_Coordinator.SelectedFolder) OrElse _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "not in selected state", Command.Tag))
            Else
                If Command.Parameters.Count <> 2 Then
                    RVal.Add(New Response(ResponseCode.Bad, "command accepts 2 parameters", Command.Tag))
                Else
                    MC = _Coordinator.SelectedFolder.GetMailMessages(Command.Parameters(0).Value, UseUid)
                    SecondParameter = Command.Parameters(1).Value
                    FDI = New FetchDataItems(SecondParameter)

                    If UseUid And Not FDI.Uid Then
                        FDI.Uid = True
                    End If

                    For i As Integer = 0 To MC.Count - 1
                        RVal.Add(New Response(ResponseCode.None, IIf(UseUid, MC(i).UniqueId.Value, i + 1) & " FETCH " & FDI.GetParsedMessage(MC(i))))
                    Next

                    RVal.Add(New Response(ResponseCode.Ok, "FETCH completed", Command.Tag))

                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function UID(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim NewCommand As Command

            If Command.Parameters.Count = 0 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 1 parameter", Command.Tag))
            Else
                NewCommand = New Command(Command.Imap4State, Command.Parameters(0).Value, Command.Tag)
                For i As Integer = 1 To Command.Parameters.Count - 1
                    NewCommand.Parameters.Add(Command.Parameters(i))
                Next
                Select Case Majodio.Mail.Server.Imap4.Command.GetCommand(NewCommand.Command)
                    Case ClientCommands.Fetch
                        RVal.Add(Fetch(NewCommand, True))
                    Case ClientCommands.Store
                        RVal.Add(Store(NewCommand, True))
                    Case ClientCommands.Copy
                        RVal.Add(Copy(NewCommand, True))
                    Case Else
                        RVal.Add(New Response(ResponseCode.Bad, "Unknown UID command", Command.Tag))
                End Select
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Status(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim ArrParts As String()
            Dim MailBox As String
            Dim Folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            Dim Response As StringBuilder

            If Command.Parameters.Count <> 2 Then
                RVal.Add(New Response(ResponseCode.Bad, "command accepts 2 parameters", Command.Tag))
            Else
                MailBox = Command.Parameters(0).Value
                ArrParts = Split(Command.Parameters(1).Value, " ")
                If Not Folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, MailBox) Then
                    RVal.Add(New Response(ResponseCode.No, "That folder does not exist", Command.Tag))
                Else
                    Folder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, MailBox)
                    If Not IsNothing(ArrParts) AndAlso ArrParts.GetUpperBound(0) > -1 Then
                        Response = New StringBuilder
                        Response.Append("(")
                        For i As Integer = 0 To ArrParts.GetUpperBound(0)
                            If i > 0 Then
                                Response.Append(" ")
                            End If
                            Select Case ArrParts(i).ToLower.Trim
                                Case "messages"
                                    Response.Append("MESSAGES ")
                                    Response.Append(Folder.GetAllMailMessages.Count)
                                Case "recent"
                                    Response.Append("RECENT ")
                                    Response.Append(Folder.Recent)
                                Case "uidnext"
                                Case "uidvalidity"
                                Case "unseen"
                                    Response.Append("UNSEEN ")
                                    Response.Append(Folder.Unseen)
                            End Select
                        Next
                        Response.Append(")")
                        RVal.Add(New Response(ResponseCode.None, "STATUS """ & MailBox & """ " & Response.ToString))
                        RVal.Add(New Response(ResponseCode.Ok, "STATUS completed", Command.Tag))
                    End If
                End If

            End If

            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Close(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            If IsNothing(_Coordinator.SelectedFolder) OrElse _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "not in selected state", Command.Tag))
            Else
                If Command.Parameters.Count <> 0 Then
                    RVal.Add(New Response(ResponseCode.Bad, "command does not accept any parameters", Command.Tag))
                Else
                    Expunge(Command)
                    _Coordinator.CurrentImap4State = SessionState.Authenticated
                    RVal.Add(New Response(ResponseCode.Ok, "CLOSE completed", Command.Tag))
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Store(ByVal Command As Command) As ResponseCollection
            Return Store(Command, False)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <param name="UseUid"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/20/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Store(ByVal Command As Command, ByVal UseUid As Boolean) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim MessageRange As String
            Dim DataItemName As String
            Dim DataItemValues As String
            Dim ArrDataItemValues As String()
            Dim Silent As Boolean
            Dim Replace As Boolean
            Dim Add As Boolean
            Dim Remove As Boolean
            Dim FetchCommand As Command
            Dim FetchParameters As CommandParameterCollection

            Dim Messages As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
            If IsNothing(_Coordinator.SelectedFolder) OrElse _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "not in selected state", Command.Tag))
            Else
                If Command.Parameters.Count <> 3 Then
                    RVal.Add(New Response(ResponseCode.Bad, "command accepts 3 parameters", Command.Tag))
                Else
                    MessageRange = Command.Parameters(0).Value
                    DataItemName = Command.Parameters(1).Value
                    DataItemValues = Command.Parameters(2).Value
                    If DataItemName.Trim.StartsWith("+") Then
                        Add = True
                    ElseIf DataItemName.Trim.StartsWith("-") Then
                        Remove = True
                    Else
                        Replace = True
                    End If
                    If DataItemName.ToLower.IndexOf("silent") > -1 Then
                        Silent = True
                    End If

                    Messages = _Coordinator.SelectedFolder.GetMailMessages(MessageRange, UseUid)

                    If IsNothing(Messages) OrElse Messages.Count = 0 Then
                        RVal.Add(New Response(ResponseCode.No, "No message(s) found", Command.Tag))
                    Else
                        For i As Integer = 0 To Messages.Count - 1
                            ArrDataItemValues = Split(DataItemValues, " ")
                            For j As Integer = 0 To ArrDataItemValues.GetUpperBound(0)
                                Select Case ArrDataItemValues(j).Trim.ToLower
                                    Case "\answered"
                                        If Replace Then
                                            Messages(i).Answered = True
                                        ElseIf Add Then
                                            Messages(i).Answered = True
                                        ElseIf Remove Then
                                            Messages(i).Answered = False
                                        End If
                                    Case "\deleted"
                                        If Replace Then
                                            Messages(i).Deleted = True
                                        ElseIf Add Then
                                            Messages(i).Deleted = True
                                        ElseIf Remove Then
                                            Messages(i).Deleted = False
                                        End If
                                    Case "\draft"
                                        If Replace Then
                                            Messages(i).Draft = True
                                        ElseIf Add Then
                                            Messages(i).Draft = True
                                        ElseIf Remove Then
                                            Messages(i).Draft = False
                                        End If
                                    Case "\flagged"
                                        If Replace Then
                                            Messages(i).Flagged = True
                                        ElseIf Add Then
                                            Messages(i).Flagged = True
                                        ElseIf Remove Then
                                            Messages(i).Flagged = False
                                        End If
                                    Case "\recent"
                                        If Add Then
                                            Messages(i).Recent = True
                                        ElseIf Remove Then
                                            Messages(i).Recent = False
                                        End If
                                    Case "\seen"
                                        If Replace Then
                                            Messages(i).Seen = True
                                        ElseIf Add Then
                                            Messages(i).Seen = True
                                        ElseIf Remove Then
                                            Messages(i).Seen = False
                                        End If
                                End Select
                            Next
                        Next
                        If Not Silent Then
                            FetchParameters = New CommandParameterCollection
                            FetchParameters.Add(New CommandParameter(MessageRange))
                            FetchParameters.Add(New CommandParameter("FLAGS"))
                            FetchCommand = New Command(_Coordinator.CurrentImap4State, "fetch", Command.Tag, FetchParameters)
                            RVal.Add(Fetch(FetchCommand, UseUid))
                            RVal.RemoveAt(RVal.Count - 1)
                        End If
                        RVal.Add(New Response(ResponseCode.Ok, "STORE completed", Command.Tag))
                    End If
                End If
            End If
            Return RVal
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/22/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Expunge(ByVal Command As Command) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim Messages As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
            If IsNothing(_Coordinator.SelectedFolder) OrElse _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "not in selected state", Command.Tag))
            Else
                If Command.Parameters.Count > 0 Then
                    RVal.Add(New Response(ResponseCode.Bad, "command does not accept any parameters", Command.Tag))
                Else
                    Messages = _Coordinator.SelectedFolder.GetAllMailMessages()
                    For i As Integer = 0 To Messages.Count - 1
                        If Messages(i).Deleted Then
                            Messages(i).DeleteMessage(False)
                            RVal.Add(New Response(ResponseCode.None, i + 1 & " EXPUNGE"))
                        End If
                    Next
                    RVal.Add(New Response(ResponseCode.Ok, "EXPUNGE completed", Command.Tag))
                    Messages = Nothing
                End If
            End If

            Return RVal
        End Function

        Public Function Copy(ByVal Command As Command) As ResponseCollection
            Return Copy(Command, False)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Command"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/22/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function Copy(ByVal Command As Command, ByVal UseUid As Boolean) As ResponseCollection
            Dim RVal As New ResponseCollection
            Dim folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            Dim copyToFolder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder()
            Dim Messages As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
            Dim Range As String

            If IsNothing(_Coordinator.SelectedFolder) OrElse _Coordinator.CurrentImap4State <> SessionState.Selected Then
                RVal.Add(New Response(ResponseCode.Bad, "not in selected state", Command.Tag))
            Else
                If Command.Parameters.Count <> 2 Then
                    RVal.Add(New Response(ResponseCode.Bad, "command accepts 2 parameters", Command.Tag))
                Else
                    Folder = _Coordinator.SelectedFolder
                    Range = Command.Parameters(0).Value
                    If Not folder.FolderExists(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(1).Value) Then
                        RVal.Add(New Response(ResponseCode.No, "that folder does not exist", Command.Tag))
                    Else
                        Messages = folder.GetMailMessages(Range, UseUid)
                        copyToFolder.Initialize(_Coordinator.CurrentUser.GetDomain, _Coordinator.CurrentUser.GetUsername, Command.Parameters(1).Value)

                        For i As Integer = 0 To Messages.Count - 1
                            copyToFolder.CopyMessage(Messages(i))
                        Next
                        RVal.Add(New Response(ResponseCode.Ok, "COPY completed", Command.Tag))
                    End If
                End If
            End If

            Return RVal
        End Function
    End Class
End Namespace