Imports system

Namespace Majodio.Data
    Public Class DataException
        Inherits ApplicationException
        Public Sub New(ByVal Sql As String, ByVal Err As System.Exception)
            MyBase.New(Err.Message & " : Sql = " & Sql)
        End Sub
    End Class
End Namespace

Namespace Majodio.Common
    Public Class InvalidAddressException
        Inherits ApplicationException

        Public Sub New(ByVal Address As String)
            MyBase.New("The address (" & Address & ") is invalid")
        End Sub
    End Class
End Namespace