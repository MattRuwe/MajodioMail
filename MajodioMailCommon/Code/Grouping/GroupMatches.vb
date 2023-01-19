Imports Majodio.Mail.Common.Grouping

Namespace Grouping
    Public Class GroupMatchCollection
        Inherits CollectionBase

        Public Sub Add(ByVal S As GroupMatch)
            MyBase.List.Add(S)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As GroupMatch
            Get
                Return MyBase.List(Index)
            End Get
            Set(ByVal Value As GroupMatch)
                MyBase.List(Index) = Value
            End Set
        End Property
    End Class

    Public Class GroupMatch
        Private _Index As Integer
        Private _Length As Integer
        Private _Value As String
        Private _GroupType As GroupingType

        Public Sub New(ByVal Index As Integer, ByVal Value As String, ByVal GroupType As GroupingType)
            _Index = Index
            _Length = Value.Length
            _Value = Value
            _GroupType = GroupType
        End Sub

        Public ReadOnly Property Index() As Integer
            Get
                Return _Index
            End Get
        End Property

        Public ReadOnly Property Value() As String
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Length() As Integer
            Get
                Return _Length
            End Get
        End Property

        Public ReadOnly Property GroupType() As GroupingType
            Get
                Return _GroupType
            End Get
        End Property
    End Class
End Namespace