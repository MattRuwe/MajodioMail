Namespace Headers
    Public Class ContentTransferEncoding
        Inherits MimeMessageHeaderBase

        Public Sub New()
            MyBase.New("content-transfer-encoding", String.Empty)
        End Sub

        Public Sub New(ByVal Value As String)
            MyBase.New("content-transfer-encoding", Value)
        End Sub

        Public Overrides Function GetFormattedHeader() As String
            Return String.Empty
        End Function

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.ContentTransferEncoding
            End Get
        End Property
    End Class
End Namespace