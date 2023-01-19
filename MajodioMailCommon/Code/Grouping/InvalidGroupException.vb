Namespace Grouping
    Public Class InvalidGroupException
        Inherits ApplicationException

        Public Sub New()
            MyBase.New("An error occurred while parsing the group")
        End Sub

        Public Sub New(ByVal Msg As String)
            MyBase.New(Msg)
        End Sub

    End Class
End Namespace