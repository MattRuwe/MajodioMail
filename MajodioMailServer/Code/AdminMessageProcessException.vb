Public Class AdminMessageProcessException
    Inherits System.Exception

    Private _details As String

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal details As String)
        MyBase.New(message)
        _details = details
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public ReadOnly Property Details() As String
        Get
            Return _details
        End Get
    End Property
End Class
