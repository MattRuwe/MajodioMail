Imports System.IO

Public Class Errors
    Public Overrides Sub Initialize()
        LoadErrorList()
    End Sub

    Private Sub LoadErrorList()
        Dim files As String()
        Dim errorDirectory As String
        Dim lvi As ListViewItem

        lvErrors.Clear()
        errorDirectory = Majodio.Mail.Common.GetApplicationDirectory & "\" & Common.Constants.ERROR_FILE_DIRECTORY
        If Directory.Exists(errorDirectory) Then
            files = Directory.GetFiles(errorDirectory)
            For i As Integer = 0 To files.GetUpperBound(0)
                lvi = New ListViewItem(Path.GetFileName(files(i)))
                lvi.Tag = files(i)
                lvErrors.Items.Add(lvi)
            Next
        End If
    End Sub

    Private Sub lvErrors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvErrors.Click

    End Sub

    Private Sub lvErrors_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvErrors.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then

            cmsErrors.Show(Me, e.Location)
        End If
    End Sub

    Private Sub lvErrors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvErrors.SelectedIndexChanged
        Dim SR As StreamReader
        If lvErrors.SelectedItems.Count = 1 Then
            SR = New StreamReader(CType(lvErrors.SelectedItems(0).Tag, String))
            txtErrorDetail.Text = SR.ReadToEnd
            SR.Close()
            btnSendError.Enabled = True
        Else
            btnSendError.Enabled = False
        End If
    End Sub

    Private Sub btnSendError_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendError.Click
        If lvErrors.SelectedItems.Count = 1 Then
            Dim feedback As New ErrorFeedback()
            feedback.ErrorDetail = txtErrorDetail.Text
            feedback.ShowDialog()
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        File.Delete(lvErrors.SelectedItems(0).Tag.ToString())
        LoadErrorList()
    End Sub
End Class
