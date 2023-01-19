Public Class Pop3ServerInterfaceCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Item As IPop3Server)
        MyBase.List.Add(Item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As IPop3Server
        Get
            Return CType(MyBase.List(Index), IPop3Server)
        End Get
        Set(ByVal value As IPop3Server)
            MyBase.List(Index) = value
        End Set
    End Property
End Class