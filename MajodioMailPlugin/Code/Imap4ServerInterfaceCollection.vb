Public Class Imap4ServerInterfaceCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Item As IImap4Server)
        MyBase.List.Add(Item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As IImap4Server
        Get
            Return CType(MyBase.List(Index), IImap4Server)
        End Get
        Set(ByVal value As IImap4Server)
            MyBase.List(Index) = value
        End Set
    End Property
End Class