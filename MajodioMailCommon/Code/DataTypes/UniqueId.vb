Namespace DataTypes
    <Serializable()> _
    Public Class UniqueId
        Private _Value As Integer

        Public Sub New()

        End Sub

        Public Sub New(ByVal UniqueId As Integer)
            _Value = UniqueId
        End Sub

        Public Property Value() As Integer
            Get
                Return _Value
            End Get
            Set(ByVal Value As Integer)
                _Value = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return _Value.ToString
        End Function
    End Class
End Namespace