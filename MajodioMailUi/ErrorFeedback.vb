Imports System.Windows.Forms

Public Class ErrorFeedback

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If txtEmail.Text.Trim.Length > 0 AndAlso Not Majodio.Common.EmailAddress.IsValidAddress(txtEmail.Text) Then
            MsgBox("Invalid email format", MsgBoxStyle.Information, "Validation Error")
            txtEmail.Focus()
            Exit Sub
        End If

        If MsgBox("Majodio Mail is about to send an error report to help fix a possible bug with the application.  Error information may contain private information and should only be sent if you feel comfortable sending the information that is on the screen.  Are you sure you want to send this information?", MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Dim M As New Majodio.Common.Messaging.Feedback("Majodio Mail (Error)", txtEmail.Text, txtName.Text, txtComments.Text & vbCrLf & txtErrorDetail.Text)
            M.Send()
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Property ErrorDetail() As String
        Get
            Return txtErrorDetail.Text
        End Get
        Set(ByVal value As String)
            txtErrorDetail.Text = value
        End Set
    End Property

End Class
