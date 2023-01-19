Namespace Configuration
    <Serializable()> _
    Public Class DnsblCollection
        Inherits CollectionBase

        Public Event ContentsChanged As EventHandler

        Public Sub New()

        End Sub

        Public Sub Add(ByVal Item As Dnsbl)
            MyBase.List.Add(Item)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As Dnsbl
            Get
                Return CType(MyBase.List(Index), Dnsbl)
            End Get
            Set(ByVal value As Dnsbl)
                MyBase.List(Index) = value
            End Set
        End Property

        Protected Overrides Sub OnClearComplete()
            MyBase.OnClearComplete()
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnInsertComplete(index, value)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemoveComplete(index, value)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            MyBase.OnSetComplete(index, oldValue, newValue)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub
    End Class
End Namespace