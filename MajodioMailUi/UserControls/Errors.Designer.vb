<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Errors
    Inherits SettingsControlBase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lvErrors = New System.Windows.Forms.ListView
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtErrorDetail = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnSendError = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmsErrors = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.cmsErrors.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lvErrors)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtErrorDetail)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(729, 522)
        Me.SplitContainer1.SplitterDistance = 243
        Me.SplitContainer1.TabIndex = 0
        '
        'lvErrors
        '
        Me.lvErrors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvErrors.Location = New System.Drawing.Point(0, 13)
        Me.lvErrors.Name = "lvErrors"
        Me.lvErrors.Size = New System.Drawing.Size(243, 509)
        Me.lvErrors.TabIndex = 1
        Me.lvErrors.UseCompatibleStateImageBehavior = False
        Me.lvErrors.View = System.Windows.Forms.View.Details
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Errors"
        '
        'txtErrorDetail
        '
        Me.txtErrorDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtErrorDetail.Location = New System.Drawing.Point(0, 56)
        Me.txtErrorDetail.Multiline = True
        Me.txtErrorDetail.Name = "txtErrorDetail"
        Me.txtErrorDetail.ReadOnly = True
        Me.txtErrorDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtErrorDetail.Size = New System.Drawing.Size(482, 466)
        Me.txtErrorDetail.TabIndex = 1
        Me.txtErrorDetail.WordWrap = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnSendError)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(482, 56)
        Me.Panel1.TabIndex = 0
        '
        'btnSendError
        '
        Me.btnSendError.Enabled = False
        Me.btnSendError.Location = New System.Drawing.Point(7, 21)
        Me.btnSendError.Name = "btnSendError"
        Me.btnSendError.Size = New System.Drawing.Size(75, 23)
        Me.btnSendError.TabIndex = 1
        Me.btnSendError.Text = "Send Error"
        Me.btnSendError.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Errors Details"
        '
        'cmsErrors
        '
        Me.cmsErrors.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteToolStripMenuItem})
        Me.cmsErrors.Name = "ContextMenuStrip1"
        Me.cmsErrors.Size = New System.Drawing.Size(117, 26)
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.DeleteToolStripMenuItem.Text = "&Delete"
        '
        'Errors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Errors"
        Me.Size = New System.Drawing.Size(729, 522)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.cmsErrors.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtErrorDetail As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnSendError As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lvErrors As System.Windows.Forms.ListView
    Friend WithEvents cmsErrors As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
