<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Pop3RelaySettings
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.lstPop3RelayAccounts = New System.Windows.Forms.ListBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblPop3RelayAccounts = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnDeleteEmailAddress = New System.Windows.Forms.Button
        Me.btnAddEmailAddress = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtDeliveryAccount = New System.Windows.Forms.TextBox
        Me.lstDeliveryAccounts = New System.Windows.Forms.ListBox
        Me.nudIntervalSeconds = New System.Windows.Forms.NumericUpDown
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtServerAddress = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudIntervalSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstPop3RelayAccounts)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnDelete)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnNew)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnSave)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.nudIntervalSeconds)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtPassword)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtUsername)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtServerAddress)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Size = New System.Drawing.Size(534, 430)
        Me.SplitContainer1.SplitterDistance = 215
        Me.SplitContainer1.TabIndex = 1
        '
        'lstPop3RelayAccounts
        '
        Me.lstPop3RelayAccounts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstPop3RelayAccounts.FormattingEnabled = True
        Me.lstPop3RelayAccounts.Location = New System.Drawing.Point(0, 23)
        Me.lstPop3RelayAccounts.Name = "lstPop3RelayAccounts"
        Me.lstPop3RelayAccounts.Size = New System.Drawing.Size(215, 407)
        Me.lstPop3RelayAccounts.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblPop3RelayAccounts)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(215, 23)
        Me.Panel1.TabIndex = 0
        '
        'lblPop3RelayAccounts
        '
        Me.lblPop3RelayAccounts.AutoSize = True
        Me.lblPop3RelayAccounts.Location = New System.Drawing.Point(49, 7)
        Me.lblPop3RelayAccounts.Name = "lblPop3RelayAccounts"
        Me.lblPop3RelayAccounts.Size = New System.Drawing.Size(113, 13)
        Me.lblPop3RelayAccounts.TabIndex = 0
        Me.lblPop3RelayAccounts.Text = "POP3 Relay Accounts"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(34, 393)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(115, 393)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(196, 393)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnDeleteEmailAddress)
        Me.GroupBox1.Controls.Add(Me.btnAddEmailAddress)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtDeliveryAccount)
        Me.GroupBox1.Controls.Add(Me.lstDeliveryAccounts)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 111)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(266, 275)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Delivery Accounts"
        '
        'btnDeleteEmailAddress
        '
        Me.btnDeleteEmailAddress.Enabled = False
        Me.btnDeleteEmailAddress.Location = New System.Drawing.Point(154, 92)
        Me.btnDeleteEmailAddress.Name = "btnDeleteEmailAddress"
        Me.btnDeleteEmailAddress.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteEmailAddress.TabIndex = 13
        Me.btnDeleteEmailAddress.Text = "D&elete"
        Me.btnDeleteEmailAddress.UseVisualStyleBackColor = True
        '
        'btnAddEmailAddress
        '
        Me.btnAddEmailAddress.Location = New System.Drawing.Point(6, 63)
        Me.btnAddEmailAddress.Name = "btnAddEmailAddress"
        Me.btnAddEmailAddress.Size = New System.Drawing.Size(75, 23)
        Me.btnAddEmailAddress.TabIndex = 12
        Me.btnAddEmailAddress.Text = "&Add"
        Me.btnAddEmailAddress.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Email Address:"
        '
        'txtDeliveryAccount
        '
        Me.txtDeliveryAccount.Location = New System.Drawing.Point(6, 36)
        Me.txtDeliveryAccount.Name = "txtDeliveryAccount"
        Me.txtDeliveryAccount.Size = New System.Drawing.Size(223, 20)
        Me.txtDeliveryAccount.TabIndex = 10
        '
        'lstDeliveryAccounts
        '
        Me.lstDeliveryAccounts.FormattingEnabled = True
        Me.lstDeliveryAccounts.Location = New System.Drawing.Point(6, 92)
        Me.lstDeliveryAccounts.Name = "lstDeliveryAccounts"
        Me.lstDeliveryAccounts.Size = New System.Drawing.Size(141, 173)
        Me.lstDeliveryAccounts.TabIndex = 9
        '
        'nudIntervalSeconds
        '
        Me.nudIntervalSeconds.Location = New System.Drawing.Point(99, 85)
        Me.nudIntervalSeconds.Name = "nudIntervalSeconds"
        Me.nudIntervalSeconds.Size = New System.Drawing.Size(63, 20)
        Me.nudIntervalSeconds.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Interval Seconds:"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(99, 59)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(173, 20)
        Me.txtPassword.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Password:"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(99, 33)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(173, 20)
        Me.txtUsername.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Username:"
        '
        'txtServerAddress
        '
        Me.txtServerAddress.Location = New System.Drawing.Point(99, 7)
        Me.txtServerAddress.Name = "txtServerAddress"
        Me.txtServerAddress.Size = New System.Drawing.Size(173, 20)
        Me.txtServerAddress.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server Address:"
        '
        'Pop3RelaySettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "Pop3RelaySettings"
        Me.Size = New System.Drawing.Size(534, 430)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudIntervalSeconds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstPop3RelayAccounts As System.Windows.Forms.ListBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblPop3RelayAccounts As System.Windows.Forms.Label
    Friend WithEvents txtServerAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lstDeliveryAccounts As System.Windows.Forms.ListBox
    Friend WithEvents nudIntervalSeconds As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteEmailAddress As System.Windows.Forms.Button
    Friend WithEvents btnAddEmailAddress As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtDeliveryAccount As System.Windows.Forms.TextBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class
