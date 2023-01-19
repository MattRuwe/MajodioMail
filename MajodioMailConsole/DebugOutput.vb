Public Class DebugOutput
    Private _DW As DebugWriter

    Private Sub DebugOutput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _DW = New DebugWriter(txtDebugOutput)
        Debug.Listeners.Add(New TextWriterTraceListener(_DW))
        chkTransparent_CheckedChanged(Me, EventArgs.Empty)
        chkAlwaysOnTop_CheckedChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.txtDebugOutput.Clear()
    End Sub

    Private Sub chkTransparent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTransparent.CheckedChanged
        If chkTransparent.Checked Then
            MyBase.Opacity = 0.75
        Else
            MyBase.Opacity = 1
        End If
    End Sub

    Private Sub chkAlwaysOnTop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAlwaysOnTop.CheckedChanged
        MyBase.TopMost = chkAlwaysOnTop.Checked
    End Sub
End Class