Imports Microsoft.VisualBasic
Imports Majodio.Mail.Common.Storage.QueuedMail
Imports System.Data


Public Class Messages
    Private Shared _instance As New Messages

    Private Sub New()

    End Sub

    Public Shared ReadOnly Property Instance() As Messages
        Get
            Return _instance
        End Get
    End Property

    Public Function GetMessageDataTable(ByVal username As String, ByVal domain As String, ByVal folderPath As String) As DataTable
        Dim rVal As DataTable
        Dim folder As Folder
        If folderPath.Trim.Length = 0 Then
            folder = New Folder(domain, username)
        Else
            folder = New Folder(domain, username, folderPath)
        End If

        Dim messages As MessageCollection = folder.GetAllMailMessages()

        rVal = New DataTable

        rVal.Columns.Add("Answered", GetType(Boolean))
        rVal.Columns.Add("ContentInAlternateFile", GetType(Boolean))
        rVal.Columns.Add("DateTime", GetType(DateTime))
        rVal.Columns.Add("Deleted", GetType(Boolean))
        rVal.Columns.Add("Draft", GetType(Boolean))
        rVal.Columns.Add("Encrypted", GetType(Boolean))
        rVal.Columns.Add("Flagged", GetType(Boolean))
        rVal.Columns.Add("SmtpFrom", GetType(Majodio.Common.EmailAddress))
        rVal.Columns.Add("MessageId", GetType(String))
        rVal.Columns.Add("MessageSize", GetType(Integer))
        rVal.Columns.Add("Recent", GetType(Boolean))
        rVal.Columns.Add("Seen", GetType(Boolean))
        rVal.Columns.Add("SendAttemptsMade", GetType(Integer))
        rVal.Columns.Add("SmtpTo", GetType(Majodio.Common.EmailAddress))
        rVal.Columns.Add("UniqueId", GetType(Majodio.Mail.Common.DataTypes.UniqueId))
        rVal.Columns.Add("XmlFilePath", GetType(String))
        rVal.Columns.Add("MessageContent", GetType(String))
        rVal.Columns.Add("MimeFrom", GetType(Majodio.Common.EmailAddress))
        rVal.Columns.Add("MimeTo", GetType(Majodio.Common.EmailAddressCollection))
        rVal.Columns.Add("MimeCc", GetType(Majodio.Common.EmailAddressCollection))
        rVal.Columns.Add("MimeBcc", GetType(Majodio.Common.EmailAddressCollection))
        rVal.Columns.Add("Subject", GetType(String))

        Dim row As DataRow
        Dim mimeMessage As Majodio.Mail.Mime.Message
        For i As Integer = 0 To messages.Count - 1
            row = rVal.NewRow()
            mimeMessage = New Majodio.Mail.Mime.Message(messages(i))
            row("Answered") = messages(i).Answered
            row("ContentInAlternateFile") = messages(i).ContentInAlternateFile
            row("DateTime") = messages(i).DateTime
            row("Deleted") = messages(i).Deleted
            row("Draft") = messages(i).Draft
            row("Encrypted") = messages(i).Encrypted
            row("Flagged") = messages(i).Flagged
            row("SmtpFrom") = messages(i).From
            row("MessageId") = messages(i).MessageId
            row("MessageSize") = messages(i).MessageSize
            row("Recent") = messages(i).Recent
            row("Seen") = messages(i).Seen
            row("SendAttemptsMade") = messages(i).SendAttemptsMade
            row("SmtpTo") = messages(i).To
            row("UniqueId") = messages(i).UniqueId
            row("XmlFilePath") = messages(i).XmlFilePath
            row("MessageContent") = messages(i).GetStringMessageContent
            row("MimeFrom") = mimeMessage.FromAddress
            row("MimeTo") = mimeMessage.ToAddresses
            row("MimeCc") = mimeMessage.CcAddresses
            row("MimeBcc") = mimeMessage.BccAddresses
            row("Subject") = mimeMessage.Subject
            rVal.Rows.Add(row)
        Next

        Return rVal
    End Function

    Public Function GetMessageCount(ByVal username As String, ByVal domain As String) As Integer
        Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder(domain, username).GetNumberOfMessages()
    End Function
End Class
