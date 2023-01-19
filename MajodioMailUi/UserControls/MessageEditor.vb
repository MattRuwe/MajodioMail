Imports System.IO

Public Class MessageEditor
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
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents lvMessages As System.Windows.Forms.ListView
    Friend WithEvents tvMessages As System.Windows.Forms.TreeView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lstTo As System.Windows.Forms.ListBox
    Friend WithEvents txtBody As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents chkSeen As System.Windows.Forms.CheckBox
    Friend WithEvents chkAnswered As System.Windows.Forms.CheckBox
    Friend WithEvents chkDeleted As System.Windows.Forms.CheckBox
    Friend WithEvents chkDraft As System.Windows.Forms.CheckBox
    Friend WithEvents chkEncrypted As System.Windows.Forms.CheckBox
    Friend WithEvents chkFlagged As System.Windows.Forms.CheckBox
    Friend WithEvents chkRecent As System.Windows.Forms.CheckBox
    Friend WithEvents lstCc As System.Windows.Forms.ListBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lstBcc As System.Windows.Forms.ListBox
    Friend WithEvents lblBcc As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MessageEditor))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.tvMessages = New System.Windows.Forms.TreeView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.lvMessages = New System.Windows.Forms.ListView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label2 = New System.Windows.Forms.Label
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtFrom = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtSubject = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.lstTo = New System.Windows.Forms.ListBox
        Me.txtBody = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.chkRecent = New System.Windows.Forms.CheckBox
        Me.chkFlagged = New System.Windows.Forms.CheckBox
        Me.chkEncrypted = New System.Windows.Forms.CheckBox
        Me.chkDraft = New System.Windows.Forms.CheckBox
        Me.chkDeleted = New System.Windows.Forms.CheckBox
        Me.chkAnswered = New System.Windows.Forms.CheckBox
        Me.chkSeen = New System.Windows.Forms.CheckBox
        Me.lstCc = New System.Windows.Forms.ListBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.lstBcc = New System.Windows.Forms.ListBox
        Me.lblBcc = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 672)
        Me.Panel1.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.tvMessages)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 23)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(200, 649)
        Me.Panel3.TabIndex = 2
        '
        'tvMessages
        '
        Me.tvMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvMessages.ImageIndex = -1
        Me.tvMessages.Location = New System.Drawing.Point(0, 0)
        Me.tvMessages.Name = "tvMessages"
        Me.tvMessages.SelectedImageIndex = -1
        Me.tvMessages.Size = New System.Drawing.Size(200, 649)
        Me.tvMessages.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Folders"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(200, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 672)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(203, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 672)
        Me.Panel2.TabIndex = 2
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.lvMessages)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 23)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(200, 649)
        Me.Panel4.TabIndex = 3
        '
        'lvMessages
        '
        Me.lvMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvMessages.GridLines = True
        Me.lvMessages.LargeImageList = Me.ImageList1
        Me.lvMessages.Location = New System.Drawing.Point(0, 0)
        Me.lvMessages.MultiSelect = False
        Me.lvMessages.Name = "lvMessages"
        Me.lvMessages.Size = New System.Drawing.Size(200, 649)
        Me.lvMessages.SmallImageList = Me.ImageList1
        Me.lvMessages.StateImageList = Me.ImageList1
        Me.lvMessages.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(32, 32)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 23)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Messages"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Splitter2
        '
        Me.Splitter2.Location = New System.Drawing.Point(403, 0)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(3, 672)
        Me.Splitter2.TabIndex = 3
        Me.Splitter2.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "From:"
        '
        'txtFrom
        '
        Me.txtFrom.Location = New System.Drawing.Point(104, 80)
        Me.txtFrom.Name = "txtFrom"
        Me.txtFrom.ReadOnly = True
        Me.txtFrom.Size = New System.Drawing.Size(320, 20)
        Me.txtFrom.TabIndex = 5
        Me.txtFrom.Text = ""
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(21, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "To:"
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(104, 280)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.ReadOnly = True
        Me.txtSubject.Size = New System.Drawing.Size(320, 20)
        Me.txtSubject.TabIndex = 9
        Me.txtSubject.Text = ""
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 280)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Subject:"
        '
        'lstTo
        '
        Me.lstTo.Location = New System.Drawing.Point(104, 112)
        Me.lstTo.Name = "lstTo"
        Me.lstTo.Size = New System.Drawing.Size(320, 43)
        Me.lstTo.TabIndex = 10
        '
        'txtBody
        '
        Me.txtBody.Location = New System.Drawing.Point(104, 312)
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.ReadOnly = True
        Me.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtBody.Size = New System.Drawing.Size(320, 248)
        Me.txtBody.TabIndex = 12
        Me.txtBody.Text = ""
        Me.txtBody.WordWrap = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 312)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(33, 16)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Body:"
        '
        'Panel5
        '
        Me.Panel5.AutoScroll = True
        Me.Panel5.Controls.Add(Me.lstBcc)
        Me.Panel5.Controls.Add(Me.lblBcc)
        Me.Panel5.Controls.Add(Me.lstCc)
        Me.Panel5.Controls.Add(Me.Label7)
        Me.Panel5.Controls.Add(Me.chkRecent)
        Me.Panel5.Controls.Add(Me.chkFlagged)
        Me.Panel5.Controls.Add(Me.chkEncrypted)
        Me.Panel5.Controls.Add(Me.chkDraft)
        Me.Panel5.Controls.Add(Me.chkDeleted)
        Me.Panel5.Controls.Add(Me.chkAnswered)
        Me.Panel5.Controls.Add(Me.chkSeen)
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Controls.Add(Me.lstTo)
        Me.Panel5.Controls.Add(Me.txtBody)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Controls.Add(Me.Label6)
        Me.Panel5.Controls.Add(Me.txtSubject)
        Me.Panel5.Controls.Add(Me.txtFrom)
        Me.Panel5.Controls.Add(Me.Label5)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(406, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(434, 672)
        Me.Panel5.TabIndex = 13
        '
        'chkRecent
        '
        Me.chkRecent.Enabled = False
        Me.chkRecent.Location = New System.Drawing.Point(176, 32)
        Me.chkRecent.Name = "chkRecent"
        Me.chkRecent.Size = New System.Drawing.Size(80, 24)
        Me.chkRecent.TabIndex = 20
        Me.chkRecent.Text = "Recent"
        '
        'chkFlagged
        '
        Me.chkFlagged.Enabled = False
        Me.chkFlagged.Location = New System.Drawing.Point(88, 32)
        Me.chkFlagged.Name = "chkFlagged"
        Me.chkFlagged.Size = New System.Drawing.Size(80, 24)
        Me.chkFlagged.TabIndex = 19
        Me.chkFlagged.Text = "Flagged"
        '
        'chkEncrypted
        '
        Me.chkEncrypted.Enabled = False
        Me.chkEncrypted.Location = New System.Drawing.Point(8, 32)
        Me.chkEncrypted.Name = "chkEncrypted"
        Me.chkEncrypted.Size = New System.Drawing.Size(80, 24)
        Me.chkEncrypted.TabIndex = 18
        Me.chkEncrypted.Text = "Encrypted"
        '
        'chkDraft
        '
        Me.chkDraft.Enabled = False
        Me.chkDraft.Location = New System.Drawing.Point(256, 8)
        Me.chkDraft.Name = "chkDraft"
        Me.chkDraft.Size = New System.Drawing.Size(80, 24)
        Me.chkDraft.TabIndex = 17
        Me.chkDraft.Text = "Draft"
        '
        'chkDeleted
        '
        Me.chkDeleted.Enabled = False
        Me.chkDeleted.Location = New System.Drawing.Point(176, 8)
        Me.chkDeleted.Name = "chkDeleted"
        Me.chkDeleted.Size = New System.Drawing.Size(80, 24)
        Me.chkDeleted.TabIndex = 16
        Me.chkDeleted.Text = "Deleted"
        '
        'chkAnswered
        '
        Me.chkAnswered.Enabled = False
        Me.chkAnswered.Location = New System.Drawing.Point(88, 8)
        Me.chkAnswered.Name = "chkAnswered"
        Me.chkAnswered.Size = New System.Drawing.Size(80, 24)
        Me.chkAnswered.TabIndex = 15
        Me.chkAnswered.Text = "Answered"
        '
        'chkSeen
        '
        Me.chkSeen.Enabled = False
        Me.chkSeen.Location = New System.Drawing.Point(8, 8)
        Me.chkSeen.Name = "chkSeen"
        Me.chkSeen.Size = New System.Drawing.Size(56, 24)
        Me.chkSeen.TabIndex = 14
        Me.chkSeen.Text = "Seen"
        '
        'lstCc
        '
        Me.lstCc.Location = New System.Drawing.Point(105, 160)
        Me.lstCc.Name = "lstCc"
        Me.lstCc.Size = New System.Drawing.Size(320, 43)
        Me.lstCc.TabIndex = 22
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 160)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(21, 16)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Cc:"
        '
        'lstBcc
        '
        Me.lstBcc.Location = New System.Drawing.Point(105, 208)
        Me.lstBcc.Name = "lstBcc"
        Me.lstBcc.Size = New System.Drawing.Size(320, 43)
        Me.lstBcc.TabIndex = 24
        '
        'lblBcc
        '
        Me.lblBcc.AutoSize = True
        Me.lblBcc.Location = New System.Drawing.Point(9, 208)
        Me.lblBcc.Name = "lblBcc"
        Me.lblBcc.Size = New System.Drawing.Size(26, 16)
        Me.lblBcc.TabIndex = 23
        Me.lblBcc.Text = "Bcc:"
        '
        'MessageEditor
        '
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Splitter2)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "MessageEditor"
        Me.Size = New System.Drawing.Size(840, 672)
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Overrides Sub Initialize()
        FillTree(String.Empty, Nothing)
    End Sub

    Private Sub FillTree(ByVal CurrentDirectory As String, ByVal ParentNode As TreeNode)
        Dim Folders As String()
        Dim NewParentNode As TreeNode
        Dim NodeCollection As TreeNodeCollection = Nothing

        If CurrentDirectory = String.Empty Then
            CurrentDirectory = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER
        End If
        Folders = Directory.GetDirectories(CurrentDirectory)
        For i As Integer = 0 To Folders.GetUpperBound(0)
            NewParentNode = New TreeNode(Majodio.Common.IoUtilities.GetFoldername(Folders(i), False))
            NewParentNode.Tag = Folders(i)
            If IsNothing(ParentNode) Then
                tvMessages.Nodes.Add(NewParentNode)
            Else
                ParentNode.Nodes.Add(NewParentNode)
            End If
            FillTree(Folders(i), NewParentNode)
        Next
    End Sub

    Private Sub tvMessages_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMessages.AfterSelect
        Dim Files As String()
        Dim LVI As ListViewItem
        Dim CurrentMsg As Majodio.Mail.Common.Storage.QueuedMail.Message

        Files = Directory.GetFiles(CType(tvMessages.SelectedNode.Tag, String), "*.xml")
        lvMessages.Items.Clear()
        For i As Integer = 0 To Files.GetUpperBound(0)
            If Path.GetFileName(Files(i)).Trim.ToLower <> Majodio.Mail.Common.Constants.MAIL_FOLDER_CONFIG_FILENAME.ToLower Then
                CurrentMsg = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage(Files(i))
                LVI = New ListViewItem
                LVI.Text = CurrentMsg.From.ToString(EmailStringFormat.Address)
                LVI.Tag = CurrentMsg
                LVI.ImageIndex = 0
                lvMessages.Items.Add(LVI)
            End If
        Next
    End Sub

    Private Sub lvMessages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvMessages.SelectedIndexChanged
        Dim Mime As Majodio.Mail.Mime.Message
        Dim Msg As Majodio.Mail.Common.Storage.QueuedMail.Message

        If lvMessages.SelectedItems.Count > 0 Then
            Msg = CType(lvMessages.SelectedItems(0).Tag, Majodio.Mail.Common.Storage.QueuedMail.Message)
            Mime = New Mime.Message(Msg)

            If Not IsNothing(Mime) Then
                If Not IsNothing(Mime.FromAddress) Then
                    txtFrom.Text = Mime.FromAddress.ToString(EmailStringFormat.NameAddressBraces)
                End If

                lstTo.Items.Clear()
                If Not IsNothing(Mime.ToAddresses) Then
                    For i As Integer = 0 To Mime.ToAddresses.Count - 1
                        lstTo.Items.Add(Mime.ToAddresses(i).ToString(EmailStringFormat.NameAddressBraces))
                    Next
                End If

                lstCc.Items.Clear()
                If Not IsNothing(Mime.CcAddresses) Then
                    For i As Integer = 0 To Mime.CcAddresses.Count - 1
                        lstCc.Items.Add(Mime.CcAddresses(i).ToString(EmailStringFormat.NameAddressBraces))
                    Next
                End If

                lstBcc.Items.Clear()
                If Not IsNothing(Mime.BccAddresses) Then
                    For i As Integer = 0 To Mime.BccAddresses.Count - 1
                        lstBcc.Items.Add(Mime.BccAddresses(i).ToString(EmailStringFormat.NameAddressBraces))
                    Next
                End If

                txtSubject.Text = Mime.Subject
                txtBody.Text = Mime.RawMessage
                chkSeen.Checked = Msg.Seen
                chkAnswered.Checked = Msg.Answered
                chkDeleted.Checked = Msg.Deleted
                chkDraft.Checked = Msg.Draft
                chkEncrypted.Checked = Msg.Encrypted
                chkFlagged.Checked = Msg.Flagged
                chkRecent.Checked = Msg.Recent
            End If
        End If
    End Sub
End Class
