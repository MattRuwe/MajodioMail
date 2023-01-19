<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServiceControl
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
        Me.gbManuallyStartServer = New System.Windows.Forms.GroupBox
        Me.btnManuallyStartServer = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.btnControlQueue = New System.Windows.Forms.Button
        Me.lblQueueStatus = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnControlPop3 = New System.Windows.Forms.Button
        Me.lblPop3Status = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnControlSmtp = New System.Windows.Forms.Button
        Me.lblSmtpStatus = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.btnControlLogger = New System.Windows.Forms.Button
        Me.lblLoggerStatus = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.gbManuallyStartServer.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbManuallyStartServer
        '
        Me.gbManuallyStartServer.Controls.Add(Me.btnManuallyStartServer)
        Me.gbManuallyStartServer.Location = New System.Drawing.Point(14, 357)
        Me.gbManuallyStartServer.Name = "gbManuallyStartServer"
        Me.gbManuallyStartServer.Size = New System.Drawing.Size(200, 66)
        Me.gbManuallyStartServer.TabIndex = 11
        Me.gbManuallyStartServer.TabStop = False
        Me.gbManuallyStartServer.Text = "Manually Start Server Processes"
        '
        'btnManuallyStartServer
        '
        Me.btnManuallyStartServer.Location = New System.Drawing.Point(9, 32)
        Me.btnManuallyStartServer.Name = "btnManuallyStartServer"
        Me.btnManuallyStartServer.Size = New System.Drawing.Size(75, 23)
        Me.btnManuallyStartServer.TabIndex = 0
        Me.btnManuallyStartServer.Text = "Start"
        Me.btnManuallyStartServer.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnControlQueue)
        Me.GroupBox3.Controls.Add(Me.lblQueueStatus)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Location = New System.Drawing.Point(14, 185)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 80)
        Me.GroupBox3.TabIndex = 9
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Queue Service"
        '
        'btnControlQueue
        '
        Me.btnControlQueue.Location = New System.Drawing.Point(8, 40)
        Me.btnControlQueue.Name = "btnControlQueue"
        Me.btnControlQueue.Size = New System.Drawing.Size(75, 23)
        Me.btnControlQueue.TabIndex = 4
        Me.btnControlQueue.Text = "Start/Stop"
        '
        'lblQueueStatus
        '
        Me.lblQueueStatus.AutoSize = True
        Me.lblQueueStatus.Location = New System.Drawing.Point(72, 16)
        Me.lblQueueStatus.Name = "lblQueueStatus"
        Me.lblQueueStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblQueueStatus.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(8, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Status:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnControlPop3)
        Me.GroupBox2.Controls.Add(Me.lblPop3Status)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 99)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 80)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "POP3 Service"
        '
        'btnControlPop3
        '
        Me.btnControlPop3.Location = New System.Drawing.Point(8, 40)
        Me.btnControlPop3.Name = "btnControlPop3"
        Me.btnControlPop3.Size = New System.Drawing.Size(75, 23)
        Me.btnControlPop3.TabIndex = 3
        Me.btnControlPop3.Text = "Start/Stop"
        '
        'lblPop3Status
        '
        Me.lblPop3Status.AutoSize = True
        Me.lblPop3Status.Location = New System.Drawing.Point(72, 16)
        Me.lblPop3Status.Name = "lblPop3Status"
        Me.lblPop3Status.Size = New System.Drawing.Size(0, 13)
        Me.lblPop3Status.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 16)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Status:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnControlSmtp)
        Me.GroupBox1.Controls.Add(Me.lblSmtpStatus)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Location = New System.Drawing.Point(14, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 80)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "SMTP Service"
        '
        'btnControlSmtp
        '
        Me.btnControlSmtp.Location = New System.Drawing.Point(8, 40)
        Me.btnControlSmtp.Name = "btnControlSmtp"
        Me.btnControlSmtp.Size = New System.Drawing.Size(75, 23)
        Me.btnControlSmtp.TabIndex = 2
        Me.btnControlSmtp.Text = "Start/Stop"
        '
        'lblSmtpStatus
        '
        Me.lblSmtpStatus.AutoSize = True
        Me.lblSmtpStatus.Location = New System.Drawing.Point(72, 16)
        Me.lblSmtpStatus.Name = "lblSmtpStatus"
        Me.lblSmtpStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblSmtpStatus.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Status:"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.btnControlLogger)
        Me.GroupBox6.Controls.Add(Me.lblLoggerStatus)
        Me.GroupBox6.Controls.Add(Me.Label15)
        Me.GroupBox6.Location = New System.Drawing.Point(14, 271)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(200, 80)
        Me.GroupBox6.TabIndex = 10
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Logging Service"
        '
        'btnControlLogger
        '
        Me.btnControlLogger.Location = New System.Drawing.Point(8, 40)
        Me.btnControlLogger.Name = "btnControlLogger"
        Me.btnControlLogger.Size = New System.Drawing.Size(75, 23)
        Me.btnControlLogger.TabIndex = 4
        Me.btnControlLogger.Text = "Start/Stop"
        '
        'lblLoggerStatus
        '
        Me.lblLoggerStatus.AutoSize = True
        Me.lblLoggerStatus.Location = New System.Drawing.Point(72, 16)
        Me.lblLoggerStatus.Name = "lblLoggerStatus"
        Me.lblLoggerStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblLoggerStatus.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(8, 16)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(40, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Status:"
        '
        'ServiceControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbManuallyStartServer)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox6)
        Me.Name = "ServiceControl"
        Me.Size = New System.Drawing.Size(833, 759)
        Me.gbManuallyStartServer.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbManuallyStartServer As System.Windows.Forms.GroupBox
    Friend WithEvents btnManuallyStartServer As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnControlQueue As System.Windows.Forms.Button
    Friend WithEvents lblQueueStatus As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnControlPop3 As System.Windows.Forms.Button
    Friend WithEvents lblPop3Status As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnControlSmtp As System.Windows.Forms.Button
    Friend WithEvents lblSmtpStatus As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents btnControlLogger As System.Windows.Forms.Button
    Friend WithEvents lblLoggerStatus As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
