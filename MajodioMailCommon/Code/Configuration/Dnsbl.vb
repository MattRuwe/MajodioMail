Namespace Configuration
    <Serializable()> _
    Public Class Dnsbl
        Private _Dnsbl As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal Dnsbl As String)
            _Dnsbl = Dnsbl
        End Sub

        Public Property DNSBL() As String
            Get
                Return _Dnsbl
            End Get
            Set(ByVal value As String)
                _Dnsbl = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return _Dnsbl
        End Function
    End Class

End Namespace