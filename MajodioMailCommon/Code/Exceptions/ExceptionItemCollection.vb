Public Class ExceptionItemCollection
    Inherits CollectionBase

    Public Sub Add(ByVal item As ExceptionItem)
        MyBase.List.Add(Item)
    End Sub

    Public Sub Add(ByVal category As String, ByVal name As String, ByVal value As String)
        Add(New ExceptionItem(category, name, value))
    End Sub

    Default Public Property Item(ByVal index As Integer) As ExceptionItem
        Get
            Return CType(MyBase.List.Item(Index), ExceptionItem)
        End Get
        Set(ByVal value As ExceptionItem)
            MyBase.List.Item(Index) = value
        End Set
    End Property
End Class
