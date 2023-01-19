Public Class SmtpServerInterfaceCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Item As ISmtpServer)
        MyBase.List.Add(Item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As ISmtpServer
        Get
            Return CType(MyBase.List(Index), ISmtpServer)
        End Get
        Set(ByVal value As ISmtpServer)
            MyBase.List(Index) = value
        End Set
    End Property
End Class