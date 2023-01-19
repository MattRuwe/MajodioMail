Namespace Imap4
    Public Delegate Sub QueueResponseReadyDelegate(ByVal Sender As Object, ByVal E As ResponseReadyEventArgs)

    Public Class QueueCoordinator
        Private _Commands As CommandCollection
        Private _Responses As ResponseCollection
        Private _CurrentCommand As Command
        Private _CurrentImap4State As SessionState
        Private _SelectedFolder As Majodio.Mail.Common.Storage.QueuedMail.Folder
        Private _SelectedFolderReadonly As Boolean
        Private _QueueSystemWatcher As Majodio.Mail.Common.Storage.QueuedMail.SystemWatcher

        Private _CurrentUser As EmailAddress

        Public Event ResponseReady As QueueResponseReadyDelegate
        Public Event ClientDisconnect As EventHandler

        Public Sub New()
            _CurrentImap4State = SessionState.NonAuthenticated
            SelectedFolderReadonly = False
        End Sub

        Public Sub SendGreeting()
            RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(New ResponseCollection(New Response(ResponseCode.Ok, "MAJODIO IMAP4 SERVICE READY")), -1))
        End Sub

        Public Sub ParseCommand(ByVal Command As String)
            Try
                If CurrentCommand.CurrentStatus <> CommandStatus.Complete Then
                    CurrentCommand.Parse(Command)
                    Dim Response As ResponseCollection
                    Dim PreviousState As SessionState = CurrentImap4State
                    Select Case CurrentCommand.CurrentStatus
                        Case CommandStatus.AwaitingContinuation
                            RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(New ResponseCollection(New Response(ResponseCode.Continuation)), CurrentCommand.BytesExpected))
                        Case CommandStatus.Received
                            Response = ProcessCommand()
                            RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(Response, -1))
                            If CurrentImap4State = SessionState.Logout Then
                                RaiseEvent ClientDisconnect(Me, EventArgs.Empty)
                            End If
                            _CurrentCommand = Nothing
                        Case CommandStatus.ErrorDetected
                            RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(New ResponseCollection(New Response(ResponseCode.Bad, "Command unknown or syntax error")), -1))
                            _CurrentCommand = Nothing
                    End Select
                Else
                    _CurrentCommand = Nothing
                End If
            Catch Ex As Exception
                If IsNothing(CurrentCommand) Then
                    Log.Logger.WriteError(Ex, "An exception occurred while processing a command")
                Else
                    Log.Logger.WriteError(Ex, "An exception occurred while processing command (" & CurrentCommand.Tag & " " & CurrentCommand.Command & ")")
                    RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(New ResponseCollection(New Response(ResponseCode.Bad, "Error occurred while processing command (" & Ex.Message & ")", CurrentCommand.Tag)), -1))
                End If
            End Try
        End Sub

        Public Function ProcessCommand() As ResponseCollection
            Dim RVal As ResponseCollection
            Dim PC As CommandProcessor
            PC = New CommandProcessor(CurrentCommand, Me)
            RVal = PC.ProcessCommand()
            Return RVal
        End Function

        Public Sub ResetCurrentCommand()
            _CurrentCommand = Nothing
        End Sub

        Public Property CurrentUser() As EmailAddress
            Get
                Return _CurrentUser
            End Get
            Set(ByVal Value As EmailAddress)
                _CurrentUser = Value
            End Set
        End Property

        Public Property SelectedFolderReadonly() As Boolean
            Get
                Return _SelectedFolderReadonly
            End Get
            Set(ByVal Value As Boolean)
                _SelectedFolderReadonly = Value
            End Set
        End Property

        Public Property SelectedFolder() As Majodio.Mail.Common.Storage.QueuedMail.Folder
            Get
                Return _SelectedFolder
            End Get
            Set(ByVal Value As Majodio.Mail.Common.Storage.QueuedMail.Folder)
                _SelectedFolder = Value
                If Not IsNothing(Value) Then
                    _QueueSystemWatcher = New Majodio.Mail.Common.Storage.QueuedMail.SystemWatcher(Value)
                    _QueueSystemWatcher.ResetAll()
                Else
                    _QueueSystemWatcher = Nothing
                End If
            End Set
        End Property

        Public ReadOnly Property QueueSystemWatcher() As Majodio.Mail.Common.Storage.QueuedMail.SystemWatcher
            Get
                Return _QueueSystemWatcher
            End Get
        End Property


        Public Property CurrentImap4State() As SessionState
            Get
                Return _CurrentImap4State
            End Get
            Set(ByVal Value As SessionState)
                _CurrentImap4State = Value
            End Set
        End Property

        Public ReadOnly Property CurrentCommand() As Command
            Get
                If IsNothing(_CurrentCommand) Then
                    _CurrentCommand = New Command(_CurrentImap4State)
                End If
                Return _CurrentCommand
            End Get
        End Property

        Public ReadOnly Property Responses() As ResponseCollection
            Get
                If IsNothing(_Responses) Then
                    _Responses = New ResponseCollection
                End If
                Return _Responses
            End Get
        End Property

        'Public ReadOnly Property SelectedFolderChanged()
        '    Get
        '        Try
        '            System.Threading.Monitor.Enter(_SelectedFolderChanged)
        '            Dim RVal As Boolean
        '            RVal = _SelectedFolderChanged
        '            If _SelectedFolderChanged Then
        '                _SelectedFolderChanged = False
        '            End If
        '            Return RVal
        '        Finally
        '            System.Threading.Monitor.Exit(_SelectedFolderChanged)
        '        End Try
        '    End Get
        'End Property

        'Private Sub SelectedFolder_Changed(ByVal Sender As Object, ByVal E As Majodio.Mail.Common.Storage.PersistentStorageChangedEventArgs)
        '    'Dim RC As New ResponseCollection
        '    'RC.Add(New Response(ResponseCode.None, "Folder Changed"))
        '    'RaiseEvent ResponseReady(Me, New ResponseReadyEventArgs(RC, -1))
        '    'Try
        '    '    System.Threading.Monitor.Enter(_SelectedFolderChanged)
        '    '    _SelectedFolderChanged = True
        '    'Finally
        '    '    System.Threading.Monitor.Exit(_SelectedFolderChanged)
        '    'End Try
        'End Sub
    End Class

    Public Class ResponseReadyEventArgs
        Inherits EventArgs

        Private _Responses As ResponseCollection
        Private _BytesExpected As Integer

        Public Sub New(ByVal Responses As ResponseCollection, ByVal BytesExpected As Integer)
            _Responses = Responses
        End Sub

        Public ReadOnly Property Responses() As ResponseCollection
            Get
                Return _Responses
            End Get
        End Property

        Public ReadOnly Property BytesExpected() As Integer
            Get
                Return _BytesExpected
            End Get
        End Property
    End Class
End Namespace