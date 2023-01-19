Public Class QueueMonitorInterfaceCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Item As IQueueMonitor)
        MyBase.List.Add(Item)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As IQueueMonitor
        Get
            Return CType(MyBase.List(Index), IQueueMonitor)
        End Get
        Set(ByVal value As IQueueMonitor)
            MyBase.List(Index) = value
        End Set
    End Property
End Class