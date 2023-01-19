Imports Majodio.Mail.Server
Imports System.Xml
Imports System.IO
Imports Majodio.Mail.Mime
Imports System.Reflection
Imports Majodio.Common
Imports Majodio.Mail.Common.Configuration
Imports System.Runtime.Remoting


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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents btnPlugin As System.Windows.Forms.Button
    Friend WithEvents btnIsInDnsbl As System.Windows.Forms.Button
    Friend WithEvents btnUpgrade As System.Windows.Forms.Button
    Friend WithEvents btnCheckIp As System.Windows.Forms.Button
    Friend WithEvents btnGetMx As System.Windows.Forms.Button
    Friend WithEvents btnGetFqdn As System.Windows.Forms.Button
    Friend WithEvents btnPopClient As System.Windows.Forms.Button
    Friend WithEvents btnGenerateMailException As System.Windows.Forms.Button
    Friend WithEvents btnMime As System.Windows.Forms.Button
    Friend WithEvents btnPop3Relay As System.Windows.Forms.Button
    Friend WithEvents btnRemoting As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.btnPop3Relay = New System.Windows.Forms.Button
        Me.btnMime = New System.Windows.Forms.Button
        Me.btnGenerateMailException = New System.Windows.Forms.Button
        Me.btnPopClient = New System.Windows.Forms.Button
        Me.btnGetFqdn = New System.Windows.Forms.Button
        Me.btnGetMx = New System.Windows.Forms.Button
        Me.btnCheckIp = New System.Windows.Forms.Button
        Me.btnUpgrade = New System.Windows.Forms.Button
        Me.btnIsInDnsbl = New System.Windows.Forms.Button
        Me.btnPlugin = New System.Windows.Forms.Button
        Me.btnTest = New System.Windows.Forms.Button
        Me.txtOutput = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.btnRemoting = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(536, 573)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.btnRemoting)
        Me.TabPage1.Controls.Add(Me.btnPop3Relay)
        Me.TabPage1.Controls.Add(Me.btnMime)
        Me.TabPage1.Controls.Add(Me.btnGenerateMailException)
        Me.TabPage1.Controls.Add(Me.btnPopClient)
        Me.TabPage1.Controls.Add(Me.btnGetFqdn)
        Me.TabPage1.Controls.Add(Me.btnGetMx)
        Me.TabPage1.Controls.Add(Me.btnCheckIp)
        Me.TabPage1.Controls.Add(Me.btnUpgrade)
        Me.TabPage1.Controls.Add(Me.btnIsInDnsbl)
        Me.TabPage1.Controls.Add(Me.btnPlugin)
        Me.TabPage1.Controls.Add(Me.btnTest)
        Me.TabPage1.Controls.Add(Me.txtOutput)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(528, 547)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btnPop3Relay
        '
        Me.btnPop3Relay.Location = New System.Drawing.Point(423, 33)
        Me.btnPop3Relay.Name = "btnPop3Relay"
        Me.btnPop3Relay.Size = New System.Drawing.Size(75, 23)
        Me.btnPop3Relay.TabIndex = 11
        Me.btnPop3Relay.Text = "POP3 Relay"
        Me.btnPop3Relay.UseVisualStyleBackColor = True
        '
        'btnMime
        '
        Me.btnMime.Location = New System.Drawing.Point(341, 34)
        Me.btnMime.Name = "btnMime"
        Me.btnMime.Size = New System.Drawing.Size(75, 23)
        Me.btnMime.TabIndex = 10
        Me.btnMime.Text = "Mime"
        Me.btnMime.UseVisualStyleBackColor = True
        '
        'btnGenerateMailException
        '
        Me.btnGenerateMailException.Location = New System.Drawing.Point(177, 33)
        Me.btnGenerateMailException.Name = "btnGenerateMailException"
        Me.btnGenerateMailException.Size = New System.Drawing.Size(157, 23)
        Me.btnGenerateMailException.TabIndex = 9
        Me.btnGenerateMailException.Text = "Generate Mail Exception"
        Me.btnGenerateMailException.UseVisualStyleBackColor = True
        '
        'btnPopClient
        '
        Me.btnPopClient.Location = New System.Drawing.Point(89, 33)
        Me.btnPopClient.Name = "btnPopClient"
        Me.btnPopClient.Size = New System.Drawing.Size(81, 23)
        Me.btnPopClient.TabIndex = 8
        Me.btnPopClient.Text = "POP Client"
        Me.btnPopClient.UseVisualStyleBackColor = True
        '
        'btnGetFqdn
        '
        Me.btnGetFqdn.Location = New System.Drawing.Point(9, 33)
        Me.btnGetFqdn.Name = "btnGetFqdn"
        Me.btnGetFqdn.Size = New System.Drawing.Size(75, 23)
        Me.btnGetFqdn.TabIndex = 7
        Me.btnGetFqdn.Text = "Get FQDN"
        Me.btnGetFqdn.UseVisualStyleBackColor = True
        '
        'btnGetMx
        '
        Me.btnGetMx.Location = New System.Drawing.Point(423, 4)
        Me.btnGetMx.Name = "btnGetMx"
        Me.btnGetMx.Size = New System.Drawing.Size(75, 23)
        Me.btnGetMx.TabIndex = 6
        Me.btnGetMx.Text = "Get MX"
        Me.btnGetMx.UseVisualStyleBackColor = True
        '
        'btnCheckIp
        '
        Me.btnCheckIp.Location = New System.Drawing.Point(341, 4)
        Me.btnCheckIp.Name = "btnCheckIp"
        Me.btnCheckIp.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckIp.TabIndex = 5
        Me.btnCheckIp.Text = "Check IP"
        Me.btnCheckIp.UseVisualStyleBackColor = True
        '
        'btnUpgrade
        '
        Me.btnUpgrade.Location = New System.Drawing.Point(259, 4)
        Me.btnUpgrade.Name = "btnUpgrade"
        Me.btnUpgrade.Size = New System.Drawing.Size(75, 23)
        Me.btnUpgrade.TabIndex = 4
        Me.btnUpgrade.Text = "Upgrade"
        Me.btnUpgrade.UseVisualStyleBackColor = True
        '
        'btnIsInDnsbl
        '
        Me.btnIsInDnsbl.Location = New System.Drawing.Point(177, 4)
        Me.btnIsInDnsbl.Name = "btnIsInDnsbl"
        Me.btnIsInDnsbl.Size = New System.Drawing.Size(75, 23)
        Me.btnIsInDnsbl.TabIndex = 3
        Me.btnIsInDnsbl.Text = "IsInDnsbl"
        Me.btnIsInDnsbl.UseVisualStyleBackColor = True
        '
        'btnPlugin
        '
        Me.btnPlugin.Location = New System.Drawing.Point(89, 3)
        Me.btnPlugin.Name = "btnPlugin"
        Me.btnPlugin.Size = New System.Drawing.Size(81, 23)
        Me.btnPlugin.TabIndex = 2
        Me.btnPlugin.Text = "Plugin Loader"
        Me.btnPlugin.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(8, 3)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(75, 23)
        Me.btnTest.TabIndex = 1
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'txtOutput
        '
        Me.txtOutput.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtOutput.Location = New System.Drawing.Point(0, 278)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtOutput.Size = New System.Drawing.Size(528, 269)
        Me.txtOutput.TabIndex = 0
        Me.txtOutput.WordWrap = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Multiselect = True
        '
        'btnRemoting
        '
        Me.btnRemoting.Location = New System.Drawing.Point(8, 62)
        Me.btnRemoting.Name = "btnRemoting"
        Me.btnRemoting.Size = New System.Drawing.Size(75, 23)
        Me.btnRemoting.TabIndex = 13
        Me.btnRemoting.Text = "Remoting"
        Me.btnRemoting.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(536, 573)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "Main"
        Me.Text = "Majodio Mail Tester"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        'Dim P As Pop3RelayDetails()
        'D.AddPop3Relay(New Pop3RelayDetails("mail.ruwe.net", "matt@ruwe.net", "September28", 60, New EmailAddress("majodio@yahoo.com")))
        'D.AddPop3Relay(New Pop3RelayDetails("mail2.ruwe.net", "matt2@ruwe.net", "September28", 60, New EmailAddress("majodio@yahoo.com")))
        RemoteConfigClient.RemoteDomain.DeletePop3Relay("mail2.ruwe.net", "matt2@ruwe.net")
    End Sub

    Public Sub EndSend(ByVal ar As IAsyncResult)
        'MsgBox("End!")
    End Sub

    Private Sub btnPlugin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlugin.Click
        Dim PlugInDir As String = GetApplicationDirectory() ' & PLUG_IN_DIRECTORY
        Dim Files() As String
        Dim CurrentAssembly As Assembly
        Dim Types() As Type


        Files = Directory.GetFiles(PlugInDir, "*.dll")
        'ReDim Files(0)
        'Files(0) = GetApplicationDirectory() & "\majodiomailplugin.dll"

        For i As Integer = 0 To Files.GetUpperBound(0)
            CurrentAssembly = [Assembly].LoadFile(Files(i))
            txtOutput.Text &= vbCrLf & CurrentAssembly.ToString() & vbCrLf
            Types = CurrentAssembly.GetTypes()
            For j As Integer = 0 To Types.GetUpperBound(0)
                If Not IsNothing(Types(j).GetInterface(GetType(Majodio.Mail.Plugin.ISmtpServer).FullName)) Then
                    txtOutput.Text &= "FOUND INTERFACE!" & vbCrLf
                    txtOutput.Text &= Types(j).ToString & vbCrLf
                End If
            Next
        Next
    End Sub

    Private Sub btnIsInDnsbl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIsInDnsbl.Click
        'MsgBox(Majodio.Mail.Server.AreMessageLinksInDnsbl(txtOutput.Text))
        MsgBox(Majodio.Mail.Server.Functions.IsInDnsbl(System.Net.IPAddress.Parse(txtOutput.Text)).IsInDnsbl)
    End Sub

    Private Sub btnUpgrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpgrade.Click
        Dim U As New Majodio.Mail.Installer.UpdatePreviousInstalls
        U.ShowDialog()
    End Sub

    Private Sub btnCheckIp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckIp.Click
        Majodio.Mail.Server.Functions.IsIpInRange(System.Net.IPAddress.Parse("90.208.192.17"), System.Net.IPAddress.Parse("90.255.255.255"))
    End Sub

    Private Sub btnGetMx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetMx.Click
        Dim mx As New DnsLib.DnsLite

    End Sub

    Private Sub btnGetFqdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetFqdn.Click
        MsgBox(Majodio.Mail.Common.Functions.GetFqdn())
    End Sub

    Private Sub btnPopClient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopClient.Click
        Dim S As New Api.SmtpMessage
        Dim P As New Pop3.Client()
        Dim Message As Mime.Message

        S.To.Add("matt@ruwe.net")
        S.From = New Majodio.Common.EmailAddress("matt@ruwe.net")
        S.Body = "Some text"
        S.Send()

        P.Connect("localhost", "matt@ruwe.net", "m#6@nr00")
        Message = P.GetMessage(0)
        txtOutput.Text = Message.Subject

    End Sub

    Private Sub btnGenerateMailException_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateMailException.Click
        Dim MailEx As New MailException("test", New Exception("Inner Exception"))
        MailEx.ExceptionItems.Add("ITEM", "test1", "test2")
        MailEx.ExceptionItems.Add("ITEM", "test3", "test4")
        MailEx.ExceptionItems.Add("ITEM", "test5", "test6")
        MailEx.ExceptionItems.Add("ITEM", "test7", "test8")
        MailEx.ExceptionItems.Add("ITEM", "test9", "test10")
        MailEx.Save()
    End Sub

    Private Sub btnMime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMime.Click
        Dim m As Mime.Message

        m = New Mime.Message(txtOutput.Text)
        'Dim bp As Mime.MessageBodyPart = m.FindBodyPart(New Mime.Headers.ContentType("text", "plain"))
        'm.Normalize()

        txtOutput.Text = m.RawMessage()
    End Sub

    Private Sub btnPop3Relay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPop3Relay.Click
        Dim p As New Pop3.RelayManager()
        p.Start()

    End Sub

    Private Sub btnRemoting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoting.Click
        'Dim server As Majodio.Mail.Common.Storage.QueuedMail.Server
        'server = New Majodio.Mail.Common.Storage.QueuedMail.Server
        Dim configServer As AppDomain = AppDomain.CreateDomain("ConfigServer")
        Dim configServerInstance As ObjectHandle = configServer.CreateInstance("MajodioMailCommon", "Majodio.Mail.Common.Configuration.Server")

        Dim messageServer As AppDomain = AppDomain.CreateDomain("Server")
        Dim messageServerInstance As ObjectHandle = messageServer.CreateInstance("MajodioMailCommon", "Majodio.Mail.Common.Storage.QueuedMail.Server")

        'Dim message As Majodio.Mail.Common.Storage.QueuedMail.Message = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage("ruwe.net", "matt")
        'message.SetMessageContent("test message")

        'message = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage("ruwe.net", "matt")
        'message.SetMessageContent("test message2")

        Dim folder As Majodio.Mail.Common.Storage.QueuedMail.Folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder("ruwe.net", "matt")

        folder = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder("ruwe.net", "matt")
    End Sub
End Class
