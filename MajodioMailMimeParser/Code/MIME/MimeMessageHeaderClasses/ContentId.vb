Namespace Headers
    Public Class ContentId
        Inherits MimeMessageHeaderBase

        Public Sub New()
            MyBase.New(String.Empty, String.Empty)
        End Sub

        Public Sub New(ByVal Name As String, ByVal Value As String)
            MyBase.New(Name, Value)
        End Sub

        Public Overrides Function GetFormattedHeader() As String
            Return String.Empty
        End Function

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.ContentId
            End Get
        End Property
    End Class
End Namespace