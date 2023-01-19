Namespace Storage
    Public Class PersistentStorageChangedEventArgs
        Inherits EventArgs

        Public Enum Actions
            Created
            Deleted
            Updated
        End Enum

        Private _FilePath As String
        Private _XPath As String
        Private _Action As Actions

        Public Sub New(ByVal Action As Actions)
            _Action = Action
        End Sub

        Public Sub New(ByVal Action As Actions, ByVal XPath As String)
            Me.New(Action)
            _XPath = XPath
        End Sub

        Public ReadOnly Property XPath() As String
            Get
                Dim RVal As String
                If IsNothing(_XPath) Then
                    _XPath = String.Empty
                End If
                RVal = _XPath
                Return RVal
            End Get
        End Property
    End Class
End Namespace