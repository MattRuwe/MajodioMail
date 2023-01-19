Namespace Imap4
    Public Class CommandParameter
        Private _Parameter As String

        Public Sub New(ByVal Parameter As String)
            _Parameter = Parameter.Trim
        End Sub

        Public Property Value() As String
            Get
                Return _Parameter
            End Get
            Set(ByVal Value As String)
                _Parameter = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return _Parameter
        End Function
    End Class
End Namespace