Namespace Headers
    Public Class OtherHeader
        Inherits MimeMessageHeaderBase

        Public Sub New()
            MyBase.New(String.Empty, String.Empty)
        End Sub

        Public Sub New(ByVal Name As String, ByVal Value As String)
            MyBase.New(Name, Value)
        End Sub

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.Other
            End Get
        End Property
    End Class
End Namespace

