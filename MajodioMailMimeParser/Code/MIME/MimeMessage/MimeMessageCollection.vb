Public Class MimeMessageCollection
    Inherits System.Collections.CollectionBase

    Public Sub Add(ByVal Message As MimeMessageCollection)
        MyBase.List.Add(Message)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As Message
        Get
            Return CType(MyBase.List.Item(Index), Message)
        End Get
        Set(ByVal Value As Message)
            MyBase.List.Item(Index) = Value
        End Set
    End Property
End Class