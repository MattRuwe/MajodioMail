Imports Majodio.Mail.Server
Imports Majodio.Mail.common.Configuration

Public Class DomainSettings
    Inherits SettingsControlBase

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDomainName As System.Windows.Forms.TextBox
    Friend WithEvents Splitter3 As System.Windows.Forms.Splitter
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSaveDomain As System.Windows.Forms.Button
    Friend WithEvents btnNewDomain As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSaveUser As System.Windows.Forms.Button
    Friend WithEvents btnNewUser As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnNewAliasUser As System.Windows.Forms.Button
    Friend WithEvents btnSaveAliasUser As System.Windows.Forms.Button
    Friend WithEvents txtAliasUsername As System.Windows.Forms.TextBox
    Friend WithEvents lstAliases As System.Windows.Forms.ListBox
    Friend WithEvents lstUsers As System.Windows.Forms.ListBox
    Friend WithEvents lstDomains As System.Windows.Forms.ListBox
    Friend WithEvents btnDeleteDomain As System.Windows.Forms.Button
    Friend WithEvents btnDeleteUser As System.Windows.Forms.Button
    Friend WithEvents btnDeleteAlias As System.Windows.Forms.Button
    Friend WithEvents btnAddRealAddress As System.Windows.Forms.Button
    Friend WithEvents btnRemoveRealAddress As System.Windows.Forms.Button
    Friend WithEvents lstAliasRealAddresses As System.Windows.Forms.ListBox
    Friend WithEvents txtAliasRealAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblLastFailedLogon As System.Windows.Forms.Label
    Friend WithEvents LastSuccessfulLogon As System.Windows.Forms.Label
    Friend WithEvents chkIsRegex As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lstAliases = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Splitter3 = New System.Windows.Forms.Splitter
        Me.lstUsers = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lstDomains = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.chkIsRegex = New System.Windows.Forms.CheckBox
        Me.btnRemoveRealAddress = New System.Windows.Forms.Button
        Me.btnAddRealAddress = New System.Windows.Forms.Button
        Me.txtAliasRealAddress = New System.Windows.Forms.TextBox
        Me.lstAliasRealAddresses = New System.Windows.Forms.ListBox
        Me.btnDeleteAlias = New System.Windows.Forms.Button
        Me.btnNewAliasUser = New System.Windows.Forms.Button
        Me.btnSaveAliasUser = New System.Windows.Forms.Button
        Me.txtAliasUsername = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblLastFailedLogon = New System.Windows.Forms.Label
        Me.LastSuccessfulLogon = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.btnDeleteUser = New System.Windows.Forms.Button
        Me.btnNewUser = New System.Windows.Forms.Button
        Me.btnSaveUser = New System.Windows.Forms.Button
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnDeleteDomain = New System.Windows.Forms.Button
        Me.btnNewDomain = New System.Windows.Forms.Button
        Me.btnSaveDomain = New System.Windows.Forms.Button
        Me.txtDomainName = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Splitter2)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(216, 511)
        Me.Panel1.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.AutoScroll = True
        Me.Panel4.Controls.Add(Me.lstAliases)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Controls.Add(Me.Splitter3)
        Me.Panel4.Controls.Add(Me.lstUsers)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(107, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(109, 511)
        Me.Panel4.TabIndex = 2
        '
        'lstAliases
        '
        Me.lstAliases.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstAliases.Location = New System.Drawing.Point(0, 235)
        Me.lstAliases.Name = "lstAliases"
        Me.lstAliases.Size = New System.Drawing.Size(109, 264)
        Me.lstAliases.Sorted = True
        Me.lstAliases.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Location = New System.Drawing.Point(0, 212)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 23)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Aliases"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter3
        '
        Me.Splitter3.Cursor = System.Windows.Forms.Cursors.HSplit
        Me.Splitter3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter3.Location = New System.Drawing.Point(0, 209)
        Me.Splitter3.Name = "Splitter3"
        Me.Splitter3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Splitter3.Size = New System.Drawing.Size(109, 3)
        Me.Splitter3.TabIndex = 2
        Me.Splitter3.TabStop = False
        '
        'lstUsers
        '
        Me.lstUsers.Dock = System.Windows.Forms.DockStyle.Top
        Me.lstUsers.Location = New System.Drawing.Point(0, 23)
        Me.lstUsers.Name = "lstUsers"
        Me.lstUsers.Size = New System.Drawing.Size(109, 186)
        Me.lstUsers.Sorted = True
        Me.lstUsers.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 23)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Users"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter2
        '
        Me.Splitter2.Location = New System.Drawing.Point(104, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(3, 511)
        Me.Splitter2.TabIndex = 1
        Me.Splitter2.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lstDomains)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(104, 511)
        Me.Panel3.TabIndex = 0
        '
        'lstDomains
        '
        Me.lstDomains.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstDomains.Location = New System.Drawing.Point(0, 23)
        Me.lstDomains.Name = "lstDomains"
        Me.lstDomains.Size = New System.Drawing.Size(104, 485)
        Me.lstDomains.Sorted = True
        Me.lstDomains.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Domains"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(216, 0)
        Me.Splitter1.MinExtra = 200
        Me.Splitter1.MinSize = 100
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 511)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.GroupBox3)
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Controls.Add(Me.GroupBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(219, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(430, 511)
        Me.Panel2.TabIndex = 2
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.chkIsRegex)
        Me.GroupBox3.Controls.Add(Me.btnRemoveRealAddress)
        Me.GroupBox3.Controls.Add(Me.btnAddRealAddress)
        Me.GroupBox3.Controls.Add(Me.txtAliasRealAddress)
        Me.GroupBox3.Controls.Add(Me.lstAliasRealAddresses)
        Me.GroupBox3.Controls.Add(Me.btnDeleteAlias)
        Me.GroupBox3.Controls.Add(Me.btnNewAliasUser)
        Me.GroupBox3.Controls.Add(Me.btnSaveAliasUser)
        Me.GroupBox3.Controls.Add(Me.txtAliasUsername)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 289)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(403, 205)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Alias"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 65)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "Real Address"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(55, 13)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "Username"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 43)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Regular Expression"
        '
        'chkIsRegex
        '
        Me.chkIsRegex.AutoSize = True
        Me.chkIsRegex.Location = New System.Drawing.Point(116, 43)
        Me.chkIsRegex.Name = "chkIsRegex"
        Me.chkIsRegex.Size = New System.Drawing.Size(15, 14)
        Me.chkIsRegex.TabIndex = 21
        Me.chkIsRegex.UseVisualStyleBackColor = True
        '
        'btnRemoveRealAddress
        '
        Me.btnRemoveRealAddress.Location = New System.Drawing.Point(244, 89)
        Me.btnRemoveRealAddress.Name = "btnRemoveRealAddress"
        Me.btnRemoveRealAddress.Size = New System.Drawing.Size(16, 23)
        Me.btnRemoveRealAddress.TabIndex = 16
        Me.btnRemoveRealAddress.Text = ">"
        '
        'btnAddRealAddress
        '
        Me.btnAddRealAddress.Location = New System.Drawing.Point(244, 65)
        Me.btnAddRealAddress.Name = "btnAddRealAddress"
        Me.btnAddRealAddress.Size = New System.Drawing.Size(16, 23)
        Me.btnAddRealAddress.TabIndex = 15
        Me.btnAddRealAddress.Text = "<"
        '
        'txtAliasRealAddress
        '
        Me.txtAliasRealAddress.Location = New System.Drawing.Point(276, 65)
        Me.txtAliasRealAddress.Name = "txtAliasRealAddress"
        Me.txtAliasRealAddress.Size = New System.Drawing.Size(112, 20)
        Me.txtAliasRealAddress.TabIndex = 17
        '
        'lstAliasRealAddresses
        '
        Me.lstAliasRealAddresses.Location = New System.Drawing.Point(116, 65)
        Me.lstAliasRealAddresses.Name = "lstAliasRealAddresses"
        Me.lstAliasRealAddresses.Size = New System.Drawing.Size(120, 95)
        Me.lstAliasRealAddresses.TabIndex = 14
        '
        'btnDeleteAlias
        '
        Me.btnDeleteAlias.Location = New System.Drawing.Point(252, 169)
        Me.btnDeleteAlias.Name = "btnDeleteAlias"
        Me.btnDeleteAlias.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteAlias.TabIndex = 20
        Me.btnDeleteAlias.Text = "Delete"
        '
        'btnNewAliasUser
        '
        Me.btnNewAliasUser.Location = New System.Drawing.Point(116, 169)
        Me.btnNewAliasUser.Name = "btnNewAliasUser"
        Me.btnNewAliasUser.Size = New System.Drawing.Size(75, 23)
        Me.btnNewAliasUser.TabIndex = 18
        Me.btnNewAliasUser.Text = "New User"
        '
        'btnSaveAliasUser
        '
        Me.btnSaveAliasUser.Location = New System.Drawing.Point(196, 169)
        Me.btnSaveAliasUser.Name = "btnSaveAliasUser"
        Me.btnSaveAliasUser.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveAliasUser.TabIndex = 19
        Me.btnSaveAliasUser.Text = "Save"
        '
        'txtAliasUsername
        '
        Me.txtAliasUsername.Location = New System.Drawing.Point(116, 16)
        Me.txtAliasUsername.Name = "txtAliasUsername"
        Me.txtAliasUsername.Size = New System.Drawing.Size(120, 20)
        Me.txtAliasUsername.TabIndex = 13
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblLastFailedLogon)
        Me.GroupBox2.Controls.Add(Me.LastSuccessfulLogon)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.btnDeleteUser)
        Me.GroupBox2.Controls.Add(Me.btnNewUser)
        Me.GroupBox2.Controls.Add(Me.btnSaveUser)
        Me.GroupBox2.Controls.Add(Me.txtPassword)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.txtUsername)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 112)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(403, 171)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Users"
        '
        'lblLastFailedLogon
        '
        Me.lblLastFailedLogon.AutoSize = True
        Me.lblLastFailedLogon.Location = New System.Drawing.Point(139, 44)
        Me.lblLastFailedLogon.Name = "lblLastFailedLogon"
        Me.lblLastFailedLogon.Size = New System.Drawing.Size(85, 13)
        Me.lblLastFailedLogon.TabIndex = 16
        Me.lblLastFailedLogon.Text = "LastFailedLogon"
        '
        'LastSuccessfulLogon
        '
        Me.LastSuccessfulLogon.AutoSize = True
        Me.LastSuccessfulLogon.Location = New System.Drawing.Point(136, 20)
        Me.LastSuccessfulLogon.Name = "LastSuccessfulLogon"
        Me.LastSuccessfulLogon.Size = New System.Drawing.Size(109, 13)
        Me.LastSuccessfulLogon.TabIndex = 15
        Me.LastSuccessfulLogon.Text = "LastSuccessfulLogon"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(14, 37)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(94, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Last Failed Logon:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(118, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Last Successful Logon:"
        '
        'btnDeleteUser
        '
        Me.btnDeleteUser.Location = New System.Drawing.Point(223, 115)
        Me.btnDeleteUser.Name = "btnDeleteUser"
        Me.btnDeleteUser.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteUser.TabIndex = 12
        Me.btnDeleteUser.Text = "Delete"
        '
        'btnNewUser
        '
        Me.btnNewUser.Location = New System.Drawing.Point(88, 115)
        Me.btnNewUser.Name = "btnNewUser"
        Me.btnNewUser.Size = New System.Drawing.Size(75, 23)
        Me.btnNewUser.TabIndex = 10
        Me.btnNewUser.Text = "New User"
        '
        'btnSaveUser
        '
        Me.btnSaveUser.Location = New System.Drawing.Point(169, 115)
        Me.btnSaveUser.Name = "btnSaveUser"
        Me.btnSaveUser.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveUser.TabIndex = 11
        Me.btnSaveUser.Text = "Save"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(88, 89)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(192, 20)
        Me.txtPassword.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 87)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 23)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Password"
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(88, 63)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(192, 20)
        Me.txtUsername.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 23)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Username"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnDeleteDomain)
        Me.GroupBox1.Controls.Add(Me.btnNewDomain)
        Me.GroupBox1.Controls.Add(Me.btnSaveDomain)
        Me.GroupBox1.Controls.Add(Me.txtDomainName)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 16)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(403, 88)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Domains"
        '
        'btnDeleteDomain
        '
        Me.btnDeleteDomain.Location = New System.Drawing.Point(228, 42)
        Me.btnDeleteDomain.Name = "btnDeleteDomain"
        Me.btnDeleteDomain.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteDomain.TabIndex = 7
        Me.btnDeleteDomain.Text = "Delete"
        '
        'btnNewDomain
        '
        Me.btnNewDomain.Location = New System.Drawing.Point(88, 42)
        Me.btnNewDomain.Name = "btnNewDomain"
        Me.btnNewDomain.Size = New System.Drawing.Size(80, 23)
        Me.btnNewDomain.TabIndex = 5
        Me.btnNewDomain.Text = "New Domain"
        '
        'btnSaveDomain
        '
        Me.btnSaveDomain.Location = New System.Drawing.Point(174, 42)
        Me.btnSaveDomain.Name = "btnSaveDomain"
        Me.btnSaveDomain.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveDomain.TabIndex = 6
        Me.btnSaveDomain.Text = "Save"
        '
        'txtDomainName
        '
        Me.txtDomainName.Location = New System.Drawing.Point(88, 16)
        Me.txtDomainName.Name = "txtDomainName"
        Me.txtDomainName.Size = New System.Drawing.Size(216, 20)
        Me.txtDomainName.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 23)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Domain Name:"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 20000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 100
        '
        'DomainSettings
        '
        Me.AutoScroll = True
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "DomainSettings"
        Me.Size = New System.Drawing.Size(649, 511)
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Overrides Sub Initialize()
        InitializeDomains()
        With ToolTip1
            .SetToolTip(lstDomains, "The domains that your mail server listens on")
            .SetToolTip(lstUsers, "The users within the selected domain")
            .SetToolTip(lstAliases, "The aliases within the selected domain")
            .SetToolTip(txtDomainName, "The domain name to add, edit, or delete.  Example:  yahoo.com")
            .SetToolTip(txtUsername, "The username within the selected domain.  Example:  don (with no domain or @ sign)")
            .SetToolTip(txtPassword, "The password for the selected user")
            .SetToolTip(txtAliasUsername, "The alias' username.  Example:  don (with no domain or @ sign)")
            .SetToolTip(lstAliasRealAddresses, "The real addresses for the selected alias username")
            .SetToolTip(txtAliasRealAddress, "Enter a new real address here")
        End With
    End Sub

    Private Sub InitializeDomains()
        Dim i As Integer
        Dim Domains As DomainDetails() = RemoteConfigClient.RemoteDomain.GetDomains()
        lstDomains.Items.Clear()
        If Not IsNothing(Domains) Then
            For i = 0 To Domains.GetUpperBound(0)
                lstDomains.Items.Add(Domains(i).Name)
            Next
        End If
        lstDomains.SelectedIndex = -1
        lstDomains_SelectedIndexChanged(Nothing, EventArgs.Empty)
        lstUsers_SelectedIndexChanged(Nothing, EventArgs.Empty)
        lstAliases_SelectedIndexChanged(Nothing, EventArgs.Empty)
    End Sub

    Private Sub InitializeUsernames()
        txtUsername.Text = String.Empty
        txtPassword.Text = String.Empty
        lstUsers.Items.Clear()
        If lstDomains.SelectedIndex > -1 Then
            Dim Users As UserDetails() = RemoteConfigClient.RemoteDomain.GetUsers(lstDomains.SelectedItem)
            Dim i As Integer
            If Not IsNothing(Users) Then
                For i = 0 To Users.GetUpperBound(0)
                    lstUsers.Items.Add(Users(i))
                Next
            End If
        End If
    End Sub

    Private Sub InitializeAliases()
        Dim SelectedAlias As AliasDetails = lstAliases.SelectedItem
        txtAliasUsername.Text = String.Empty
        chkIsRegex.Checked = False
        lstAliasRealAddresses.Items.Clear()
        lstAliases.Items.Clear()
        If lstDomains.SelectedIndex > -1 Then
            Dim Aliases As AliasDetails() = RemoteConfigClient.RemoteDomain.GetAliases(lstDomains.SelectedItem)
            Dim i As Integer
            If Not IsNothing(Aliases) Then
                For i = 0 To Aliases.GetUpperBound(0)
                    lstAliases.Items.Add(Aliases(i))
                Next
                If Not IsNothing(SelectedAlias) Then
                    lstAliases.SelectedIndex = lstAliases.Items.IndexOf(SelectedAlias)
                End If
            End If
        End If
    End Sub

    Private Sub lstDomains_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstDomains.SelectedIndexChanged
        If lstDomains.SelectedIndex = -1 Then
            btnNewDomain.Enabled = False
            txtUsername.Enabled = False
            txtPassword.Enabled = False

            txtAliasUsername.Enabled = False
            chkIsRegex.Enabled = False
            txtAliasRealAddress.Enabled = False
            lstAliasRealAddresses.Enabled = False
        Else
            btnNewDomain.Enabled = True
            txtUsername.Enabled = True
            txtPassword.Enabled = True

            txtAliasUsername.Enabled = True
            chkIsRegex.Enabled = True
            txtAliasRealAddress.Enabled = True
            lstAliasRealAddresses.Enabled = True

            txtDomainName.Text = lstDomains.SelectedItem
        End If
        btnDeleteDomain.Enabled = True
        txtDomainName_TextChanged(sender, e)
        InitializeUsernames()
        InitializeAliases()
    End Sub

    Private Sub txtDomainName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDomainName.TextChanged
        If txtDomainName.Text.Trim.Length = 0 Then
            btnSaveDomain.Enabled = False
        Else
            btnSaveDomain.Enabled = True
        End If
        If lstDomains.SelectedIndex = -1 Then
            btnDeleteDomain.Enabled = False
        End If
    End Sub

    Private Sub btnNewDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewDomain.Click
        lstDomains.SelectedIndex = -1
        lstDomains_SelectedIndexChanged(sender, e)
        txtDomainName.Text = String.Empty
        txtDomainName.Focus()
    End Sub

    Private Sub btnNewUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewUser.Click
        lstUsers.SelectedIndex = -1
        txtUsername.Focus()
    End Sub

    Private Sub btnNewAliasUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewAliasUser.Click
        lstAliases.SelectedIndex = -1
        txtAliasUsername.Focus()
    End Sub

    Private Sub lstUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstUsers.SelectedIndexChanged
        Dim Username As String
        Dim DomainName As String
        If lstUsers.SelectedIndex = -1 Then
            LastSuccessfulLogon.Text = String.Empty
            lblLastFailedLogon.Text = String.Empty
            btnNewUser.Enabled = False
            btnDeleteUser.Enabled = False
            txtUsername.Text = String.Empty
            txtPassword.Text = String.Empty
        Else
            Username = CType(lstUsers.SelectedItem, UserDetails).Username
            DomainName = lstDomains.SelectedItem
            btnNewUser.Enabled = True
            btnDeleteUser.Enabled = True
            txtUsername.Text = CType(lstUsers.SelectedItem, UserDetails).Username
            txtPassword.Text = CType(lstUsers.SelectedItem, UserDetails).Password
            LastSuccessfulLogon.Text = RemoteConfigClient.RemoteDomain.LastSuccessfulAuthentication(DomainName, Username).ToString(INTERNATIONAL_DATE_TIME_FORMAT)
            lblLastFailedLogon.Text = RemoteConfigClient.RemoteDomain.LastFailedAuthentication(DomainName, Username).ToString(INTERNATIONAL_DATE_TIME_FORMAT)
            lstAliases.SelectedIndex = -1
        End If
        btnDeleteUser.Enabled = True
        txtUsername_TextChanged(sender, e)
    End Sub

    Private Sub txtUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged, txtPassword.TextChanged
        If txtUsername.Text.Trim.Length = 0 Or txtPassword.Text.Trim.Length = 0 Then
            btnSaveUser.Enabled = False
        Else
            btnSaveUser.Enabled = True
        End If
        If lstUsers.SelectedIndex = -1 Then
            btnDeleteUser.Enabled = False
        End If
    End Sub

    Private Sub lstAliases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstAliases.SelectedIndexChanged
        Dim i As Integer
        Dim AD As AliasDetails
        btnNewAliasUser.Enabled = False
        btnDeleteAlias.Enabled = False
        txtAliasUsername.Text = String.Empty
        chkIsRegex.Checked = False
        lstAliasRealAddresses.Items.Clear()
        If lstAliases.SelectedIndex > -1 Then
            btnNewAliasUser.Enabled = True
            btnDeleteAlias.Enabled = True
            txtAliasUsername.Text = CType(lstAliases.SelectedItem, AliasDetails).Username
            chkIsRegex.Checked = CType(lstAliases.SelectedItem, AliasDetails).IsRegex
            AD = CType(lstAliases.SelectedItem, AliasDetails)
            For i = 0 To AD.RealAddresses.Count - 1
                lstAliasRealAddresses.Items.Add(AD.RealAddresses(i).ToString(EmailStringFormat.Address))
            Next
            lstUsers.SelectedIndex = -1
        End If
        txtAliasUsername_TextChanged(sender, e)
    End Sub

    Private Sub txtAliasUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAliasUsername.TextChanged
        If txtAliasUsername.Text.Trim.Length = 0 Then
            btnSaveAliasUser.Enabled = False
        Else
            btnSaveAliasUser.Enabled = True
        End If
        If lstAliases.SelectedIndex = -1 Then
            btnDeleteAlias.Enabled = False
        End If
    End Sub

    Private Sub btnDeleteDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteDomain.Click
        If lstDomains.SelectedIndex > -1 Then
            If MsgBox("Deleting a domain will also delete all users and aliases associated with the domain.  Are you sure this is what you want to do?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
                RemoteConfigClient.RemoteDomain.DeleteDomain(lstDomains.SelectedItem)
                InitializeDomains()
            End If
        End If
        txtDomainName.Text = String.Empty
    End Sub

    Private Sub btnDeleteUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteUser.Click
        If lstUsers.SelectedIndex > -1 Then
            RemoteConfigClient.RemoteDomain.DeleteUser(lstDomains.SelectedItem, CType(lstUsers.SelectedItem, UserDetails).Username)
            InitializeUsernames()
        End If
    End Sub

    Private Sub btnSaveDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveDomain.Click
        Dim DomainName As String = txtDomainName.Text.Trim.ToLower
        If lstDomains.SelectedIndex = -1 Then
            btnDeleteDomain_Click(sender, e)
            RemoteConfigClient.RemoteDomain.AddDomain(New DomainDetails(DomainName))
        Else
            RemoteConfigClient.RemoteDomain.UpdateDomain(New DomainDetails(lstDomains.SelectedItem.ToString()), New DomainDetails(DomainName))
        End If
        InitializeDomains()
        lstDomains.SelectedItem = DomainName
        txtDomainName.Text = String.Empty
        lstDomains_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub btnSaveUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveUser.Click
        If txtUsername.Text.IndexOf("@") > -1 Then
            MsgBox("The username field cannot contain an @ sign", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Invalid Username")
        ElseIf txtUsername.Text.IndexOf(" ") > -1 Then
            MsgBox("The username field cannot contain a space", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Invalid Username")
        Else
            Dim User As New UserDetails(txtUsername.Text, txtPassword.Text, 0)
            btnDeleteUser_Click(sender, e)
            RemoteConfigClient.RemoteDomain.AddUser(lstDomains.SelectedItem, User)
            InitializeUsernames()
            lstUsers.SelectedItem = User
            txtUsername.Text = String.Empty
            txtPassword.Text = String.Empty
        End If

    End Sub
#Region "Aliases"
    Private Sub btnDeleteAlias_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAlias.Click
        If lstAliases.SelectedIndex > -1 Then
            RemoteConfigClient.RemoteDomain.DeleteAlias(lstDomains.SelectedItem, CType(lstAliases.SelectedItem, AliasDetails).Username)
            InitializeAliases()
        End If
    End Sub

    Private Sub btnSaveAliasUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAliasUser.Click
        SaveAliasUserInfo(True)
    End Sub

    Private Sub btnAddRealAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRealAddress.Click
        If Not Majodio.Common.EmailAddress.IsValidAddress(txtAliasRealAddress.Text) Then
            MsgBox("Invalid email address", MsgBoxStyle.Exclamation, "Invalid format")
            Exit Sub
        End If
        Dim SelectedIndex As Integer
        SelectedIndex = lstAliases.SelectedIndex
        Dim AD As AliasDetails
        lstAliasRealAddresses.Items.Add(String.Copy(txtAliasRealAddress.Text))
        AD = SaveAliasUserInfo(False)
        txtAliasRealAddress.Text = String.Empty
        txtAliasRealAddress.Focus()
        If SelectedIndex > -1 Then
            lstAliases.SelectedIndex = SelectedIndex
        ElseIf Not IsNothing(AD) Then
            For i As Integer = 0 To lstAliases.Items.Count - 1
                If CType(lstAliases.Items(i), AliasDetails).Username = AD.Username Then
                    lstAliases.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub btnRemoveRealAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveRealAddress.Click
        Dim AliasUser As New AliasDetails(txtAliasUsername.Text, chkIsRegex.Checked)
        Dim SelectedIndex As Integer
        SelectedIndex = lstAliases.SelectedIndex
        If lstAliasRealAddresses.SelectedIndex > -1 Then
            txtAliasRealAddress.Text = lstAliasRealAddresses.SelectedItem
            lstAliasRealAddresses.Items.RemoveAt(lstAliasRealAddresses.SelectedIndex)
            SaveAliasUserInfo(False)
            lstAliases.SelectedIndex = SelectedIndex
        End If
    End Sub

    Private Function SaveAliasUserInfo(ByVal ClearFields As Boolean) As AliasDetails
        Dim AliasUser As AliasDetails = Nothing
        Try
            AliasUser = New AliasDetails(txtAliasUsername.Text, chkIsRegex.Checked)
            If Not chkIsRegex.Checked AndAlso txtUsername.Text.IndexOf("@") > -1 Then
                MsgBox("The username field cannot contain an @ sign", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Invalid Username")
            Else
                Dim i As Integer
                For i = 0 To lstAliasRealAddresses.Items.Count - 1
                    AliasUser.RealAddresses.Add(New EmailAddress(lstAliasRealAddresses.Items(i)))
                Next
                btnDeleteAlias_Click(Nothing, EventArgs.Empty)
                RemoteConfigClient.RemoteDomain.AddAlias(lstDomains.SelectedItem, AliasUser)
                InitializeAliases()
                If ClearFields Then
                    txtAliasUsername.Text = String.Empty
                    chkIsRegex.Checked = False
                    lstAliasRealAddresses.Items.Clear()
                    txtAliasRealAddress.Text = String.Empty
                    lstAliases.SelectedItem = AliasUser
                End If
            End If
        Catch ex As Exception
            MsgBox("An error occurred while attempting to save the alias (" & txtAliasUsername.Text & ")." & vbCrLf & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Cannot save alias")
        End Try
        Return AliasUser
    End Function
#End Region

    Private Sub txtAliasRealAddress_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAliasRealAddress.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnAddRealAddress_Click(sender, EventArgs.Empty)
        End If
    End Sub
End Class
