Namespace Storage.QueuedMail
    <Serializable()> _
    Public Class SystemWatcher
        Implements IDisposable
#Region " Fields"
        Private _Folder As Folder
        Private _Messages As MessageCollection
        Private _NewMessages As MessageCollection
        Private _ChangedMessages As MessageCollection
        Private _DeletedMessages As MessageCollection
#End Region
#Region " Events"
        Public Event FolderChanged As FolderChangedEventHandler
#End Region
#Region " Constructor"
        Public Sub New(ByVal Folder As Folder)
            _Folder = Folder
        End Sub
#End Region
#Region " Destructor"
        Protected Overrides Sub Finalize()

        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose

        End Sub
#End Region
#Region " Private Methods"
        'Private Sub Initialize()
        '    _tmrFolderCheck = New System.Timers.Timer
        '    _tmrFolderCheck.Interval = 5000
        '    _tmrFolderCheck.AutoReset = False
        '    AddHandler _tmrFolderCheck.Elapsed, AddressOf tmrFolderCheck_Elapsed
        '    _tmrFolderCheck.Start()

        '    _Messages = _Folder.GetAllMailMessages()
        'End Sub

        'Private Sub tmrFolderCheck_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
        '    CheckForChanges()
        '    If _NewMessages.Count > 0 Or _ChangedMessages.Count > 0 Or _DeletedMessages.Count > 0 Then
        '        RaiseEvent FolderChanged(Me, New FolderChangedEventArgs(_NewMessages, _ChangedMessages, _DeletedMessages))
        '    End If
        '    _tmrFolderCheck.Start()
        'End Sub
#End Region
#Region " Public Methods"
        Public Sub CheckForChanges()
            Dim CurrentMessages As MessageCollection

            _NewMessages = New MessageCollection
            _ChangedMessages = New MessageCollection
            _DeletedMessages = New MessageCollection

            Dim Index As Integer
            Try
                If Not IsNothing(_Messages) Then
                    CurrentMessages = _Folder.GetAllMailMessages()
                    For i As Integer = 0 To CurrentMessages.Count - 1
                        Index = _Messages.IndexOf(CurrentMessages(i).UniqueId)
                        If Index = -1 Then
                            _NewMessages.Add(CurrentMessages(i))
                        ElseIf Not CurrentMessages(i).Equals(_Messages(Index)) Then
                            _ChangedMessages.Add(_Messages(Index))
                        End If
                    Next

                    For i As Integer = 0 To _Messages.Count - 1
                        If (_Messages(i).FileExists) Then
                            Index = CurrentMessages.IndexOf(_Messages(i).UniqueId)
                            If Index = -1 Then
                                _DeletedMessages.Add(_Messages(i))
                            End If
                        Else
                            _DeletedMessages.Add(_Messages(i))
                        End If
                    Next
                End If
            Catch Ex As Exception
                Throw New Majodio.Mail.Common.Exceptions.MessageNotFoundException(Ex)
            End Try
        End Sub

        Public Sub ResetAll()
            _Messages = _Folder.GetAllMailMessages()
            ResetChanges()
        End Sub

        Public Sub ResetChanges()
            _NewMessages = New MessageCollection
            _ChangedMessages = New MessageCollection
            _DeletedMessages = New MessageCollection
        End Sub
#End Region
#Region " Public Properties"
        Public ReadOnly Property NewMessages() As MessageCollection
            Get
                If IsNothing(_NewMessages) Then
                    _NewMessages = New MessageCollection
                End If
                Return _NewMessages
            End Get
        End Property

        Public ReadOnly Property ChangedMessages() As MessageCollection
            Get
                If IsNothing(_ChangedMessages) Then
                    _ChangedMessages = New MessageCollection
                End If
                Return _ChangedMessages
            End Get
        End Property

        Public ReadOnly Property DeletedMessages() As MessageCollection
            Get
                If IsNothing(_DeletedMessages) Then
                    _DeletedMessages = New MessageCollection
                End If
                Return _DeletedMessages
            End Get
        End Property

        Public ReadOnly Property FolderHasChanged() As Boolean
            Get
                Return ChangedMessages.Count > 0 OrElse _NewMessages.Count > 0 OrElse _DeletedMessages.Count > 0
            End Get
        End Property
#End Region
    End Class
End Namespace