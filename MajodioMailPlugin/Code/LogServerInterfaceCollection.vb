Public Class LogServerInterfaceCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Item As ILogServer)
        MyBase.List.Add(Item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As ILogServer
        Get
            Return CType(MyBase.List(Index), ILogServer)
        End Get
        Set(ByVal value As ILogServer)
            MyBase.List(Index) = value
        End Set
    End Property
End Class