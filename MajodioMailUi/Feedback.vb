Public Class Feedback
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtComments As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnCancelSend As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtComments = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnCancelSend = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name (Optional)"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(8, 24)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(280, 20)
        Me.txtName.TabIndex = 1
        Me.txtName.Text = ""
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(8, 64)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(280, 20)
        Me.txtEmail.TabIndex = 2
        Me.txtEmail.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Email (Optional)"
        '
        'txtComments
        '
        Me.txtComments.Location = New System.Drawing.Point(8, 104)
        Me.txtComments.Multiline = True
        Me.txtComments.Name = "txtComments"
        Me.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComments.Size = New System.Drawing.Size(280, 96)
        Me.txtComments.TabIndex = 3
        Me.txtComments.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Comments"
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(128, 208)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.TabIndex = 4
        Me.btnSend.Text = "&Send"
        '
        'btnCancelSend
        '
        Me.btnCancelSend.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelSend.Location = New System.Drawing.Point(208, 208)
        Me.btnCancelSend.Name = "btnCancelSend"
        Me.btnCancelSend.TabIndex = 5
        Me.btnCancelSend.Text = "&Cancel"
        '
        'Feedback
        '
        Me.AcceptButton = Me.btnSend
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancelSend
        Me.ClientSize = New System.Drawing.Size(298, 239)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelSend)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtComments)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Feedback"
        Me.Text = "Feedback"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnCancelSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelSend.Click
        Me.Close()
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If txtEmail.Text.Trim.Length > 0 AndAlso Not Majodio.Common.EmailAddress.IsValidAddress(txtEmail.Text) Then
            MsgBox("Invalid email format", MsgBoxStyle.Information, "Validation Error")
            txtEmail.Focus()
            Exit Sub
        End If
        If txtComments.Text.Trim.Length = 0 Then
            MsgBox("Please enter your comments.", MsgBoxStyle.Information, "Validation Error")
            txtComments.Focus()
            Exit Sub
        End If
        Dim M As New Majodio.Common.Messaging.Feedback("Majodio Mail", txtEmail.Text, txtName.Text, txtComments.Text)
        M.Send()
        MsgBox("Thank you!  Your feedback has been delivered.", MsgBoxStyle.OKOnly, "Thank you")
        Me.Close()
    End Sub
End Class
