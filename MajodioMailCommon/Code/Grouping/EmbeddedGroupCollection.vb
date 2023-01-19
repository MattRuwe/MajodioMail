Namespace Grouping
    Public Class EmbeddedGroupCollection
        Inherits CollectionBase

        Public Sub Add(ByVal Item As EmbeddedGroup)
            MyBase.List.Add(Item)
        End Sub

        Public Sub Add(ByVal ItemCollection As EmbeddedGroupCollection)
            For i As Integer = 0 To ItemCollection.Count - 1
                Add(ItemCollection(i))
            Next
        End Sub

        Default Public Property Item(ByVal Index As Integer) As EmbeddedGroup
            Get
                Return MyBase.List(Index)
            End Get
            Set(ByVal Value As EmbeddedGroup)
                MyBase.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace