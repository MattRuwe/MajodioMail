Imports System.IO
Imports System.Reflection
Imports Majodio.Mail.common.Configuration

Public Class Main
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
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents tvOptions As System.Windows.Forms.TreeView
    Friend WithEvents pnlContents As System.Windows.Forms.Panel
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFeedback As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileClose As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFeedbackSendFeedback As System.Windows.Forms.MenuItem
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelpContents As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFeedbackSupportForums As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelpDonate As System.Windows.Forms.MenuItem
    Friend WithEvents ilIcon As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Settings")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Plugin Configuration")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Domain Settings")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("POP3 Relay Settings")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Service Controller")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Current Log Activity")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Messages")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Errors")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.tvOptions = New System.Windows.Forms.TreeView
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.pnlContents = New System.Windows.Forms.Panel
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileClose = New System.Windows.Forms.MenuItem
        Me.mnuFeedback = New System.Windows.Forms.MenuItem
        Me.mnuFeedbackSendFeedback = New System.Windows.Forms.MenuItem
        Me.mnuFeedbackSupportForums = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuHelpContents = New System.Windows.Forms.MenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.MenuItem
        Me.mnuHelpDonate = New System.Windows.Forms.MenuItem
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider
        Me.ilIcon = New System.Windows.Forms.ImageList(Me.components)
        Me.SuspendLayout()
        '
        'tvOptions
        '
        Me.tvOptions.Dock = System.Windows.Forms.DockStyle.Left
        Me.tvOptions.Location = New System.Drawing.Point(0, 0)
        Me.tvOptions.Name = "tvOptions"
        TreeNode1.Name = ""
        TreeNode1.Text = "Settings"
        TreeNode2.Name = "Plugin Configuration"
        TreeNode2.Text = "Plugin Configuration"
        TreeNode3.Name = ""
        TreeNode3.Text = "Domain Settings"
        TreeNode4.Name = "Node0"
        TreeNode4.Text = "POP3 Relay Settings"
        TreeNode5.Name = "ServiceController"
        TreeNode5.Text = "Service Controller"
        TreeNode5.ToolTipText = "Service Controller"
        TreeNode6.Name = ""
        TreeNode6.Text = "Current Log Activity"
        TreeNode7.Name = ""
        TreeNode7.Text = "Messages"
        TreeNode8.Name = "Node1"
        TreeNode8.Text = "Errors"
        Me.tvOptions.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6, TreeNode7, TreeNode8})
        Me.tvOptions.Size = New System.Drawing.Size(152, 560)
        Me.tvOptions.TabIndex = 0
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(152, 0)
        Me.Splitter1.MinExtra = 100
        Me.Splitter1.MinSize = 100
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 560)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'pnlContents
        '
        Me.pnlContents.AutoScroll = True
        Me.pnlContents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContents.Location = New System.Drawing.Point(155, 0)
        Me.pnlContents.Name = "pnlContents"
        Me.pnlContents.Size = New System.Drawing.Size(797, 560)
        Me.pnlContents.TabIndex = 2
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuFeedback, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileClose})
        Me.mnuFile.Text = "&File"
        '
        'mnuFileClose
        '
        Me.mnuFileClose.Index = 0
        Me.mnuFileClose.Text = "&Close"
        '
        'mnuFeedback
        '
        Me.mnuFeedback.Index = 1
        Me.mnuFeedback.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFeedbackSendFeedback, Me.mnuFeedbackSupportForums})
        Me.mnuFeedback.Text = "F&eedback"
        '
        'mnuFeedbackSendFeedback
        '
        Me.mnuFeedbackSendFeedback.Index = 0
        Me.mnuFeedbackSendFeedback.Text = "&Send Feedback/Report Bug"
        '
        'mnuFeedbackSupportForums
        '
        Me.mnuFeedbackSupportForums.Index = 1
        Me.mnuFeedbackSupportForums.Text = "&Support Forms"
        '
        'mnuHelp
        '
        Me.mnuHelp.Index = 2
        Me.mnuHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuHelpContents, Me.mnuHelpAbout, Me.mnuHelpDonate})
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpContents
        '
        Me.mnuHelpContents.Index = 0
        Me.mnuHelpContents.Text = "&Contents"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Index = 1
        Me.mnuHelpAbout.Text = "&About"
        '
        'mnuHelpDonate
        '
        Me.mnuHelpDonate.Index = 2
        Me.mnuHelpDonate.Text = "&Donate!"
        '
        'HelpProvider1
        '
        Me.HelpProvider1.HelpNamespace = "Help.chm"
        '
        'ilIcon
        '
        Me.ilIcon.ImageStream = CType(resources.GetObject("ilIcon.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilIcon.TransparentColor = System.Drawing.Color.Transparent
        Me.ilIcon.Images.SetKeyName(0, "")
        '
        'Main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(952, 560)
        Me.Controls.Add(Me.pnlContents)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.tvOptions)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "Main"
        Me.Text = "Majodio Mail Server"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents _SysTray As Majodio.Common.Shell.SysTray
    Private _PingTmr As System.Timers.Timer
    'Private _Config As Config
    Private _PluginConfigs As ArrayList
    Private Const PLUGIN_CONFIGURATION_TITLE As String = "Plugin Configuration"

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadPluginConfigs()
        CheckPluginConifgurationNode()
        MyBase.Text = MyBase.Text & " " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4)
        '_Config = New Config
        If RemoteConfigClient.RemoteConfig.SendUsageInformation() Then
            Majodio.Common.Messaging.SendMessage.SendMessage("Majodio Mail UI " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4), "UI Started")
            PingTmr_Elapsed(Nothing, Nothing)
            _PingTmr = New System.Timers.Timer(Majodio.Common.Constants.MESSAGE_PING_RATE)
            AddHandler _PingTmr.Elapsed, AddressOf PingTmr_Elapsed
            _PingTmr.AutoReset = True
            _PingTmr.Start()
        End If
        Dim DeliverableMessageTmr As New System.Timers.Timer(5000)
        DeliverableMessageTmr.AutoReset = False
        DeliverableMessageTmr.Start()
        AddHandler DeliverableMessageTmr.Elapsed, AddressOf DisplayDeliverableMessages

        LoadControl()

        _SysTray = New Majodio.Common.Shell.SysTray(Me)

        _SysTray.ToolTipText = "Majodio Mail"
        _SysTray.IconImageList = ilIcon
        _SysTray.IconIndex = 0
    End Sub

    Private Sub PingTmr_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
        Majodio.Common.Messaging.Ping.Ping("Majodio Mail UI", System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4))
    End Sub

    Private Sub DisplayDeliverableMessages(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
        Dim M As New Majodio.Common.Messaging.DeliverableMessages("Majodio Mail UI", System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4), Majodio.Common.Messaging.DeliverableMessages.GetLatestMessageId)
        Dim Messages As String()
        Messages = M.GetDeliverableMessages()
        If Not IsNothing(Messages) AndAlso Messages.GetUpperBound(0) > -1 Then
            For i As Int32 = 0 To Messages.GetUpperBound(0)
                MsgBox(Messages(i), MsgBoxStyle.OkOnly, "Message from Majodio Software")
            Next
        End If
    End Sub

    Public Sub LoadControl()
        If IsNothing(tvOptions.SelectedNode) Then
            tvOptions.SelectedNode = tvOptions.Nodes(0)
        End If
        Select Case tvOptions.SelectedNode.FullPath.ToLower
            Case "settings"
                LoadContentPanel(New MailSettings)
            Case "service controller"
                LoadContentPanel(New ServiceControl)
            Case "domain settings"
                LoadContentPanel(New DomainSettings)
            Case "current log activity"
                LoadContentPanel(New LogFile)
            Case "messages"
                LoadContentPanel(New MessageEditor)
            Case "errors"
                LoadContentPanel(New Errors)
            Case "pop3 relay settings"
                LoadContentPanel(New Pop3RelaySettings)
            Case Else
                If tvOptions.SelectedNode.FullPath.StartsWith(PLUGIN_CONFIGURATION_TITLE) Then
                    Dim MS As New MailSettings
                    MS.ConfigObject = tvOptions.SelectedNode.Tag
                    LoadContentPanel(MS)
                Else
                    LoadContentPanel(New Empty)
                End If
        End Select
    End Sub

    Private Sub LoadContentPanel(ByVal C As SettingsControlBase)
        pnlContents.Controls.Clear()
        If Not IsNothing(C) Then
            C.Dock = DockStyle.Fill
            C.Initialize()
            C.AutoScroll = True
            pnlContents.Controls.Add(C)
        End If
    End Sub

    Private Sub tvOptions_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvOptions.AfterSelect
        LoadControl()
    End Sub

    Private Sub mnuFeedbackSendFeedback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFeedbackSendFeedback.Click
        Dim F As New Feedback
        F.ShowDialog(Me)
    End Sub

    Private Sub mnuFileClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileClose.Click
        Me.Close()
    End Sub

    Private Sub mnuHelpContents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpContents.Click
        Help.ShowHelp(Me, "file://" & Application.StartupPath & "\help.pdf")
    End Sub

    Private Sub Main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Dim R As New Majodio.Mail.Common.Registry
        If Me.WindowState = FormWindowState.Minimized Then
            Me.ShowInTaskbar = False
            _SysTray.ShowInSysTray = True
            If R.GetValue("ShowMinizeNotification", 0) = 0 Then
                _SysTray.ShowBalloonTip("Majodio Mail is running in the background.  Click here to restore.", Shell.NotifyIconBalloonIconFlags.NIIF_INFO, "Notice")
                R.SetValue("ShowMinizeNotification", 1)
            End If
        End If
    End Sub

    Private Sub Main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        _SysTray.ShowInSysTray = False
        _SysTray.Dispose()
        _SysTray = Nothing
        If RemoteConfigClient.RemoteConfig.SendUsageInformation Then
            Majodio.Common.Messaging.SendMessage.SendMessage("Majodio Mail UI " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4), "UI Closed")
        End If
    End Sub

    Private Sub _SysTray_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _SysTray.Click
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
        _SysTray.ShowInSysTray = False
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        If Not IsNothing(_SysTray) Then
            _SysTray.AssignHandle(Me.Handle)
        End If
        MyBase.OnHandleCreated(e)
    End Sub

    Public Sub LoadPluginConfigs()
        _PluginConfigs = New ArrayList
        Dim Files() As String
        Dim CurrentAssembly As Assembly
        Dim Types() As Type

        Files = System.IO.Directory.GetFiles(GetApplicationDirectory(), "*.dll")
        For i As Integer = 0 To Files.GetUpperBound(0)
            Try
                CurrentAssembly = Assembly.LoadFile(Files(i))
                Types = CurrentAssembly.GetTypes()
                For j As Integer = 0 To Types.GetUpperBound(0)
                    If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.IPluginConfiguration).FullName)) Then
                        Majodio.Common.Utilities.TraceMe("Plugin Configuration Found:  " & Types(j).FullName)
                        _PluginConfigs.Add(Activator.CreateInstance(Types(j)))
                    End If
                Next
            Catch ex As Exception
                Continue For
            End Try
        Next
    End Sub

    Public Sub CheckPluginConifgurationNode()
        If (IsNothing(_PluginConfigs) OrElse _PluginConfigs.Count = 0) AndAlso tvOptions.Nodes.IndexOfKey(PLUGIN_CONFIGURATION_TITLE) > -1 Then
            tvOptions.Nodes(PLUGIN_CONFIGURATION_TITLE).Remove()
        Else
            Dim ParentNode As TreeNode = tvOptions.Nodes(PLUGIN_CONFIGURATION_TITLE)
            Dim NewNode As TreeNode
            For i As Integer = 0 To _PluginConfigs.Count - 1
                NewNode = New TreeNode(CType(_PluginConfigs(i), Majodio.Mail.Plugin.IPluginConfiguration).Title)
                NewNode.Tag = _PluginConfigs(i)
                ParentNode.Nodes.Add(NewNode)
            Next
        End If
    End Sub

    Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click
        Dim About As New AboutBox()
        About.ShowDialog(Me)
    End Sub

    Private Sub mnuFeedbackSupportForums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFeedbackSupportForums.Click
        Help.ShowHelp(Me, "http://www.majodio.com/forum")
    End Sub

    Private Sub mnuHelpDonate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpDonate.Click
        Help.ShowHelp(Me, "https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=support%40majodio%2ecom&item_name=Majodio%20Mail&no_shipping=2&no_note=1&tax=0&currency_code=USD&bn=PP%2dDonationsBF&charset=UTF%2d8")
    End Sub
End Class
