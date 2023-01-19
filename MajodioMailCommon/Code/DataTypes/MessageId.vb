Namespace DataTypes
    <Serializable()> _
    Public Class MessageId
        Private _Value As String

        Public Sub New(ByVal Value As String)
            _Value = Value
        End Sub

        Public Property Value() As String
            Get
                Return _Value
            End Get
            Set(ByVal Value As String)
                _Value = Value
            End Set
        End Property
    End Class
End Namespace