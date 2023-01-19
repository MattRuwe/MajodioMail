Namespace Exceptions
    Public Class MessageNotFoundException
        Inherits ApplicationException

        Public Sub New()
            Me.New(CType(Nothing, Exception))
        End Sub

        Public Sub New(ByVal InnerException As Exception)
            MyBase.New("The specified mail message could not be found", InnerException)
        End Sub
        Public Sub New(ByVal Message As String, ByVal InnerException As Exception)
            MyBase.New(Message, InnerException)
        End Sub
    End Class
End Namespace