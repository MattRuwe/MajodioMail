Namespace Pop3
    Public Class ConnectionStatsCollection
        Inherits System.Collections.CollectionBase

        Public Sub Add(ByVal ConnectionStats As ConnectionStats)
            MyBase.List.Add(ConnectionStats)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ConnectionStats
            Get
                Return CType(MyBase.List(Index), ConnectionStats)
            End Get
            Set(ByVal value As ConnectionStats)
                MyBase.List(Index) = value
            End Set
        End Property
    End Class
End Namespace