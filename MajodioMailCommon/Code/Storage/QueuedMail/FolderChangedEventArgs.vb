Namespace Storage.QueuedMail
    <Serializable()> _
    Public Delegate Sub FolderChangedEventHandler(ByVal Source As Object, ByVal E As FolderChangedEventArgs)

    <Serializable()> _
    Public Class FolderChangedEventArgs
        Inherits EventArgs
        Private _NewMessages As MessageCollection
        Private _ChangedMessages As MessageCollection
        Private _DeletedMessages As MessageCollection

        Public Sub New(ByVal NewMessages As MessageCollection, ByVal ChangedMessages As MessageCollection, ByVal DeletedMessages As MessageCollection)
            _NewMessages = NewMessages
            _ChangedMessages = ChangedMessages
            _DeletedMessages = DeletedMessages
        End Sub

        Public ReadOnly Property NewMessages() As MessageCollection
            Get
                Return _NewMessages
            End Get
        End Property

        Public ReadOnly Property ChangedMessages() As MessageCollection
            Get
                Return _ChangedMessages
            End Get
        End Property

        Public ReadOnly Property Deletedmessages() As MessageCollection
            Get
                Return _DeletedMessages
            End Get
        End Property
    End Class
End Namespace



'Public Class MessagesChangedEventArgs
'    Inherits EventArgs

'    Public Enum MessageChangeType
'        MessageUpdated
'        MessageDeleted
'        MessageCreated
'    End Enum

'    Private _ChangeType As MessageChangeType
'    Private _MessageFolder As String
'    Private _MessageId As String
'    Private _Tag As Object

'    Public Sub New(ByVal CT As MessageChangeType)
'        _ChangeType = CT
'        _MessageFolder = String.Empty
'        _Tag = Nothing
'    End Sub

'    Public Sub New(ByVal CT As MessageChangeType, ByVal MessageFolder As String, ByVal MessageId As String)
'        Me.New(CT)
'        _MessageFolder = QueuedMailFolder.GetFolderFromPath(MessageFolder)
'        _Tag = Nothing
'    End Sub

'    Public Sub New(ByVal CT As MessageChangeType, ByVal MessageFolder As String, ByVal MessageId As String, ByVal Tag As Object)
'        Me.New(CT, MessageFolder, MessageId)
'        _Tag = Tag
'    End Sub

'    Public ReadOnly Property ChangeType() As MessageChangeType
'        Get
'            Return _ChangeType
'        End Get
'    End Property

'    Public ReadOnly Property MessageFolder() As String
'        Get
'            Return _MessageFolder
'        End Get
'    End Property

'    Public ReadOnly Property MessageId() As String
'        Get
'            Return _MessageFolder
'        End Get
'    End Property

'    Public ReadOnly Property Tag() As Object
'        Get
'            Return _Tag
'        End Get
'    End Property
'End Class
