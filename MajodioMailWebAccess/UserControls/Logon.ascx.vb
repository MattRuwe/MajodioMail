Imports Majodio.Mail.Common.Configuration
Imports Majodio.Common

Partial Class UserControls_Logon
    Inherits UserControlBase

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lblError.Visible = False
        lblError.Text = String.Empty
    End Sub

    Protected Sub btnLogon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogon.Click

        Dim userEmail As EmailAddress
        Dim password As String
        If Not Majodio.Common.EmailAddress.IsValidAddress(txtEmailAddress.Text) Then
            SetErrorLabel("The email address (" & txtEmailAddress.Text & ") is not valid.")
        Else
            userEmail = New EmailAddress(txtEmailAddress.Text)
            password = txtPassword.Text
            If Not Majodio.Mail.Common.Configuration.RemoteConfigClient.RemoteDomain.AuthorizeUser(userEmail.GetDomain, userEmail.GetUsername, password) Then
                SetErrorLabel("Incorrect Logon")
            Else
                MyBase.SetLogonInformation(userEmail.GetDomain, userEmail.GetUsername)
                MyBase.RaiseStateChanged("MailMain", Nothing)
            End If
        End If

    End Sub

    Private Sub SetErrorLabel(ByVal message As String)
        lblError.Visible = True
        lblError.Text = message
    End Sub
End Class
