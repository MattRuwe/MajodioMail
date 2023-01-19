
Partial Class UserControls_MailMain
    Inherits UserControlBase

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Refresh()
    End Sub

    Private Sub SetMessageCount()
        lblMessageCount.Text = Messages.Instance.GetMessageCount(LoggedInUsername, LoggedInDomain)
    End Sub

    Private Sub BindMessages()
        gvMessages.DataSource = Messages.Instance.GetMessageDataTable(LoggedInUsername, LoggedInDomain, String.Empty)
        gvMessages.DataBind()
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        Refresh()
    End Sub

    Private Sub Refresh()
        lblLastUpdate.Text = DateTime.Now
        SetMessageCount()
        BindMessages()
    End Sub

End Class
