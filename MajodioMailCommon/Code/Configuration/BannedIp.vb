Namespace Configuration
    <Serializable()> _
    Public Class BannedIp
        Private _Ip As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal IP As String)
            _Ip = IP
        End Sub

        Public Property IP() As String
            Get
                Return _Ip
            End Get
            Set(ByVal value As String)
                _Ip = value
            End Set
        End Property

        Friend ReadOnly Property ConvertToIpAddress() As System.Net.IPAddress
            Get
                If IsNothing(_Ip) Then
                    Return Nothing
                Else
                    Return System.Net.IPAddress.Parse(_Ip)
                End If

            End Get
        End Property

        Public Overrides Function ToString() As String
            Return _Ip
        End Function
    End Class

End Namespace