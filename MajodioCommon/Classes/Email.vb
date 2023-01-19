Imports System.Net.Mail

Namespace Majodio.Web.Mail
    'Name:          Email
    'Description:   Used to send email
    'Remarks:       None
    'Author:        Matt Ruwe
    'Release Date:  1/23/2004
    'History
    'Author     Date        Remarks
    Public Class Email
        Private _MM As System.Net.Mail.MailMessage

        Public Sub New()
            _MM = New System.Net.Mail.MailMessage()
        End Sub

        Public ReadOnly Property Recipient() As MailAddressCollection
            Get
                Return _MM.To
            End Get
        End Property

        Public Property From() As MailAddress
            Get
                Return _MM.From
            End Get
            Set(ByVal Value As MailAddress)
                _MM.From = Value
            End Set
        End Property

        Public Property Body() As String
            Get
                Return _MM.Body
            End Get
            Set(ByVal Value As String)
                _MM.Body = Value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _MM.Subject
            End Get
            Set(ByVal Value As String)
                _MM.Subject = Value
            End Set
        End Property

        Public Sub Send()
            'System.Net.Mail.SmtpMail.SmtpServer = "mail.majodio.com"
            'System.Web.Mail.SmtpMail.Send(_MM)
            Dim S As New SmtpClient
            S.Host = "mail.majodio.com"
            S.Send(_MM)
        End Sub

    End Class
End Namespace
