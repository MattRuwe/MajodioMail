<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MailScanner
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.nudThreads = New System.Windows.Forms.NumericUpDown
        Me.txtOutput = New System.Windows.Forms.TextBox
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblIpsScanned = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblEmailCount = New System.Windows.Forms.Label
        CType(Me.nudThreads, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Threads"
        '
        'nudThreads
        '
        Me.nudThreads.Location = New System.Drawing.Point(64, 7)
        Me.nudThreads.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudThreads.Name = "nudThreads"
        Me.nudThreads.Size = New System.Drawing.Size(47, 20)
        Me.nudThreads.TabIndex = 1
        Me.nudThreads.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'txtOutput
        '
        Me.txtOutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutput.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutput.Location = New System.Drawing.Point(15, 72)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtOutput.Size = New System.Drawing.Size(558, 399)
        Me.txtOutput.TabIndex = 2
        Me.txtOutput.WordWrap = False
        '
        'btnStartStop
        '
        Me.btnStartStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartStop.Location = New System.Drawing.Point(498, 477)
        Me.btnStartStop.Name = "btnStartStop"
        Me.btnStartStop.Size = New System.Drawing.Size(75, 23)
        Me.btnStartStop.TabIndex = 3
        Me.btnStartStop.Text = "Start"
        Me.btnStartStop.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(118, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "IPs Scanned:"
        '
        'lblIpsScanned
        '
        Me.lblIpsScanned.AutoSize = True
        Me.lblIpsScanned.Location = New System.Drawing.Point(234, 9)
        Me.lblIpsScanned.Name = "lblIpsScanned"
        Me.lblIpsScanned.Size = New System.Drawing.Size(74, 13)
        Me.lblIpsScanned.TabIndex = 5
        Me.lblIpsScanned.Text = "lblIpsScanned"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(121, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Email Servers Found:"
        '
        'lblEmailCount
        '
        Me.lblEmailCount.AutoSize = True
        Me.lblEmailCount.Location = New System.Drawing.Point(234, 26)
        Me.lblEmailCount.Name = "lblEmailCount"
        Me.lblEmailCount.Size = New System.Drawing.Size(70, 13)
        Me.lblEmailCount.TabIndex = 7
        Me.lblEmailCount.Text = "lblEmailCount"
        '
        'MailScanner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 519)
        Me.Controls.Add(Me.lblEmailCount)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblIpsScanned)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnStartStop)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.nudThreads)
        Me.Controls.Add(Me.Label1)
        Me.Name = "MailScanner"
        Me.Text = "MailScanner"
        CType(Me.nudThreads, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudThreads As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnStartStop As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblIpsScanned As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblEmailCount As System.Windows.Forms.Label
End Class
