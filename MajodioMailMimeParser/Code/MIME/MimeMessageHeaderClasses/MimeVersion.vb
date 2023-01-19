Imports System.Text.RegularExpressions

Namespace Headers
    Public Class MimeVersion
        Inherits MimeMessageHeaderBase

        Public Sub New()
            MyBase.New("MIME-Version", "1.0")
        End Sub

        Public Sub New(ByVal Version As String)
            MyBase.New("MIME-Version", Version)
            If Not Regex.IsMatch(Version, "\d\.\d") Then
                Throw New FormatException("The format of version is not correct")
            End If
        End Sub

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.MimeVersion
            End Get
        End Property
    End Class
End Namespace