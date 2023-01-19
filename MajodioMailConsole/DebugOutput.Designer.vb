<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DebugOutput
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkAlwaysOnTop = New System.Windows.Forms.CheckBox
        Me.chkTransparent = New System.Windows.Forms.CheckBox
        Me.btnClear = New System.Windows.Forms.Button
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.txtDebugOutput = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkAlwaysOnTop)
        Me.Panel1.Controls.Add(Me.chkTransparent)
        Me.Panel1.Controls.Add(Me.btnClear)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(990, 46)
        Me.Panel1.TabIndex = 0
        '
        'chkAlwaysOnTop
        '
        Me.chkAlwaysOnTop.AutoSize = True
        Me.chkAlwaysOnTop.Location = New System.Drawing.Point(184, 12)
        Me.chkAlwaysOnTop.Name = "chkAlwaysOnTop"
        Me.chkAlwaysOnTop.Size = New System.Drawing.Size(98, 17)
        Me.chkAlwaysOnTop.TabIndex = 2
        Me.chkAlwaysOnTop.Text = "Always On Top"
        Me.chkAlwaysOnTop.UseVisualStyleBackColor = True
        '
        'chkTransparent
        '
        Me.chkTransparent.AutoSize = True
        Me.chkTransparent.Location = New System.Drawing.Point(94, 12)
        Me.chkTransparent.Name = "chkTransparent"
        Me.chkTransparent.Size = New System.Drawing.Size(83, 17)
        Me.chkTransparent.TabIndex = 1
        Me.chkTransparent.Text = "Transparent"
        Me.chkTransparent.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClear.Location = New System.Drawing.Point(12, 12)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 0
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 46)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(990, 3)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'txtDebugOutput
        '
        Me.txtDebugOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDebugOutput.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDebugOutput.Location = New System.Drawing.Point(0, 49)
        Me.txtDebugOutput.Multiline = True
        Me.txtDebugOutput.Name = "txtDebugOutput"
        Me.txtDebugOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtDebugOutput.Size = New System.Drawing.Size(990, 571)
        Me.txtDebugOutput.TabIndex = 2
        Me.txtDebugOutput.WordWrap = False
        '
        'DebugOutput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClear
        Me.ClientSize = New System.Drawing.Size(990, 620)
        Me.Controls.Add(Me.txtDebugOutput)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "DebugOutput"
        Me.Opacity = 0.75
        Me.Text = "DebugOutput"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents txtDebugOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents chkTransparent As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlwaysOnTop As System.Windows.Forms.CheckBox
End Class
