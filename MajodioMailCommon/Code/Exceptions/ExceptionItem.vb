Public Class ExceptionItem
    Private _Category As String
    Private _Name As String
    Private _Value As String

    Public Sub New(ByVal Category As String, ByVal Name As String, ByVal Value As String)
        _Category = Category
        _Name = Name
        _Value = Value
    End Sub

    Public ReadOnly Property Category() As String
        Get
            Return _Category
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Public ReadOnly Property Value() As String
        Get
            Return _Value
        End Get
    End Property
End Class
