Imports System.IO
Imports Majodio.Mail.Server
Imports System.Text

Public Class LogFile
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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblTotalPop3Sessions As System.Windows.Forms.Label
    Friend WithEvents lblTotalSmtpSessions As System.Windows.Forms.Label
    Friend WithEvents lblCurrentPop3Sessions As System.Windows.Forms.Label
    Friend WithEvents lblCurrentSmtpSessions As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblTimedoutPop3Sessions As System.Windows.Forms.Label
    Friend WithEvents lblTimedoutSmtpSessions As System.Windows.Forms.Label
    Friend WithEvents lblMessagesReceived As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblMessagesRejectedDnsbl As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblMessagesDelivered As System.Windows.Forms.Label
    Friend WithEvents dgvLog As System.Windows.Forms.DataGridView
    Friend WithEvents lblMessagesRejectedInvalidFrom As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblConnectionsRejectedDnsbl As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.lblConnectionsRejectedDnsbl = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblMessagesRejectedInvalidFrom = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblMessagesDelivered = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblMessagesRejectedDnsbl = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblMessagesReceived = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblTimedoutPop3Sessions = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblTimedoutSmtpSessions = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblTotalPop3Sessions = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblTotalSmtpSessions = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblCurrentPop3Sessions = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblCurrentSmtpSessions = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.dgvLog = New System.Windows.Forms.DataGridView
        Me.Panel1.SuspendLayout()
        CType(Me.dgvLog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblConnectionsRejectedDnsbl)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.lblMessagesRejectedInvalidFrom)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.lblMessagesDelivered)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblMessagesRejectedDnsbl)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.lblMessagesReceived)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.lblTimedoutPop3Sessions)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblTimedoutSmtpSessions)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.lblTotalPop3Sessions)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.lblTotalSmtpSessions)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.lblCurrentPop3Sessions)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblCurrentSmtpSessions)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(636, 88)
        Me.Panel1.TabIndex = 3
        '
        'lblConnectionsRejectedDnsbl
        '
        Me.lblConnectionsRejectedDnsbl.AutoSize = True
        Me.lblConnectionsRejectedDnsbl.Location = New System.Drawing.Point(359, 40)
        Me.lblConnectionsRejectedDnsbl.Name = "lblConnectionsRejectedDnsbl"
        Me.lblConnectionsRejectedDnsbl.Size = New System.Drawing.Size(13, 13)
        Me.lblConnectionsRejectedDnsbl.TabIndex = 21
        Me.lblConnectionsRejectedDnsbl.Text = "0"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(189, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(154, 13)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Connections Rejected DNSBL:"
        '
        'lblMessagesRejectedInvalidFrom
        '
        Me.lblMessagesRejectedInvalidFrom.AutoSize = True
        Me.lblMessagesRejectedInvalidFrom.Location = New System.Drawing.Point(359, 8)
        Me.lblMessagesRejectedInvalidFrom.Name = "lblMessagesRejectedInvalidFrom"
        Me.lblMessagesRejectedInvalidFrom.Size = New System.Drawing.Size(13, 13)
        Me.lblMessagesRejectedInvalidFrom.TabIndex = 19
        Me.lblMessagesRejectedInvalidFrom.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(164, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Messages Rejected Invalid From:"
        '
        'lblMessagesDelivered
        '
        Me.lblMessagesDelivered.AutoSize = True
        Me.lblMessagesDelivered.Location = New System.Drawing.Point(532, 56)
        Me.lblMessagesDelivered.Name = "lblMessagesDelivered"
        Me.lblMessagesDelivered.Size = New System.Drawing.Size(13, 13)
        Me.lblMessagesDelivered.TabIndex = 17
        Me.lblMessagesDelivered.Text = "0"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(388, 56)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(106, 13)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Messages Delivered:"
        '
        'lblMessagesRejectedDnsbl
        '
        Me.lblMessagesRejectedDnsbl.AutoSize = True
        Me.lblMessagesRejectedDnsbl.Location = New System.Drawing.Point(359, 24)
        Me.lblMessagesRejectedDnsbl.Name = "lblMessagesRejectedDnsbl"
        Me.lblMessagesRejectedDnsbl.Size = New System.Drawing.Size(13, 13)
        Me.lblMessagesRejectedDnsbl.TabIndex = 15
        Me.lblMessagesRejectedDnsbl.Text = "0"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(189, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(143, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Messages Rejected DNSBL:"
        '
        'lblMessagesReceived
        '
        Me.lblMessagesReceived.AutoSize = True
        Me.lblMessagesReceived.Location = New System.Drawing.Point(160, 56)
        Me.lblMessagesReceived.Name = "lblMessagesReceived"
        Me.lblMessagesReceived.Size = New System.Drawing.Size(13, 13)
        Me.lblMessagesReceived.TabIndex = 13
        Me.lblMessagesReceived.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(107, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Messages Received:"
        '
        'lblTimedoutPop3Sessions
        '
        Me.lblTimedoutPop3Sessions.AutoSize = True
        Me.lblTimedoutPop3Sessions.Location = New System.Drawing.Point(532, 24)
        Me.lblTimedoutPop3Sessions.Name = "lblTimedoutPop3Sessions"
        Me.lblTimedoutPop3Sessions.Size = New System.Drawing.Size(13, 13)
        Me.lblTimedoutPop3Sessions.TabIndex = 11
        Me.lblTimedoutPop3Sessions.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(388, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(133, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Timed-out POP3 Sessions:"
        '
        'lblTimedoutSmtpSessions
        '
        Me.lblTimedoutSmtpSessions.AutoSize = True
        Me.lblTimedoutSmtpSessions.Location = New System.Drawing.Point(160, 24)
        Me.lblTimedoutSmtpSessions.Name = "lblTimedoutSmtpSessions"
        Me.lblTimedoutSmtpSessions.Size = New System.Drawing.Size(13, 13)
        Me.lblTimedoutSmtpSessions.TabIndex = 9
        Me.lblTimedoutSmtpSessions.Text = "0"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(135, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Timed-out SMTP Sessions:"
        '
        'lblTotalPop3Sessions
        '
        Me.lblTotalPop3Sessions.AutoSize = True
        Me.lblTotalPop3Sessions.Location = New System.Drawing.Point(532, 40)
        Me.lblTotalPop3Sessions.Name = "lblTotalPop3Sessions"
        Me.lblTotalPop3Sessions.Size = New System.Drawing.Size(13, 13)
        Me.lblTotalPop3Sessions.TabIndex = 7
        Me.lblTotalPop3Sessions.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(388, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(110, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Total POP3 Sessions:"
        '
        'lblTotalSmtpSessions
        '
        Me.lblTotalSmtpSessions.AutoSize = True
        Me.lblTotalSmtpSessions.Location = New System.Drawing.Point(160, 40)
        Me.lblTotalSmtpSessions.Name = "lblTotalSmtpSessions"
        Me.lblTotalSmtpSessions.Size = New System.Drawing.Size(13, 13)
        Me.lblTotalSmtpSessions.TabIndex = 5
        Me.lblTotalSmtpSessions.Text = "0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 40)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Total SMTP Sessions:"
        '
        'lblCurrentPop3Sessions
        '
        Me.lblCurrentPop3Sessions.AutoSize = True
        Me.lblCurrentPop3Sessions.Location = New System.Drawing.Point(532, 8)
        Me.lblCurrentPop3Sessions.Name = "lblCurrentPop3Sessions"
        Me.lblCurrentPop3Sessions.Size = New System.Drawing.Size(13, 13)
        Me.lblCurrentPop3Sessions.TabIndex = 3
        Me.lblCurrentPop3Sessions.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(388, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Current POP3 Sessions:"
        '
        'lblCurrentSmtpSessions
        '
        Me.lblCurrentSmtpSessions.AutoSize = True
        Me.lblCurrentSmtpSessions.Location = New System.Drawing.Point(160, 8)
        Me.lblCurrentSmtpSessions.Name = "lblCurrentSmtpSessions"
        Me.lblCurrentSmtpSessions.Size = New System.Drawing.Size(13, 13)
        Me.lblCurrentSmtpSessions.TabIndex = 1
        Me.lblCurrentSmtpSessions.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Current SMTP Sessions:"
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Enabled = False
        Me.Splitter1.Location = New System.Drawing.Point(0, 88)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(636, 3)
        Me.Splitter1.TabIndex = 4
        Me.Splitter1.TabStop = False
        '
        'dgvLog
        '
        Me.dgvLog.AllowUserToAddRows = False
        Me.dgvLog.AllowUserToDeleteRows = False
        Me.dgvLog.AllowUserToResizeRows = False
        Me.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLog.Location = New System.Drawing.Point(0, 91)
        Me.dgvLog.MultiSelect = False
        Me.dgvLog.Name = "dgvLog"
        Me.dgvLog.ReadOnly = True
        Me.dgvLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvLog.ShowEditingIcon = False
        Me.dgvLog.ShowRowErrors = False
        Me.dgvLog.Size = New System.Drawing.Size(636, 407)
        Me.dgvLog.TabIndex = 5
        '
        'LogFile
        '
        Me.AutoScroll = True
        Me.Controls.Add(Me.dgvLog)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "LogFile"
        Me.Size = New System.Drawing.Size(636, 498)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.dgvLog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _Tmr As System.Timers.Timer
    Private _Logger As Majodio.Mail.Server.Log.Logger
    Private _LatestLogId As String
    Private _dtLog As DataTable
    Private _Bs As BindingSource

    Private Delegate Sub RefreshData()

    Public Overrides Sub Initialize()
        'Dim FilePath As String = Majodio.Mail.Server.GetApplicationDirectory() & "\" & Majodio.Mail.Server.Constants.LOG_FILE_DIRECTORY & "\" & Majodio.Mail.Server.Functions.GetSerializedDate & ".txt"
        '_LatestLogId = String.Empty
        'If File.Exists(FilePath) Then
        '    Dim SR As StreamReader = File.OpenText(FilePath)
        '    txtLog.Text = SR.ReadToEnd()
        '    SR.Close()
        'End If
        InitializeDataTable()
        _Tmr = New System.Timers.Timer(1000)
        _Tmr.BeginInit()
        _Tmr.AutoReset = False
        _Tmr.EndInit()
        _Tmr.Start()
        AddHandler _Tmr.Elapsed, AddressOf _Tmr_Elapsed
        _Bs = New BindingSource(_dtLog, String.Empty)
        dgvLog.DataSource = _Bs.DataSource
    End Sub

    Private Sub _Tmr_Elapsed(ByVal Sender As Object, ByVal E As Timers.ElapsedEventArgs)
        GetLatestLogEntries()
        RefreshSessionCounters()
        _Tmr.Start()
    End Sub

    Private Sub InitializeDataTable()
        _dtLog = New DataTable("log")
        _dtLog.BeginInit()
        _dtLog.Columns.Add(New DataColumn("Time Stamp", GetType(String)))
        _dtLog.Columns.Add(New DataColumn("Thread ID", GetType(String)))
        _dtLog.Columns.Add(New DataColumn("Message", GetType(String)))
        _dtLog.EndInit()
    End Sub

    Private Sub GetLatestLogEntries()
        If dgvLog.InvokeRequired Then
            Dim D As New RefreshData(AddressOf GetLatestLogEntries)
            Me.Invoke(D, Nothing)
        Else
            Dim Messages As Majodio.Mail.Server.Log.Message()
            Dim i As Integer
            Dim StartIndex As Integer = 0
            Dim DR As DataRow
            Dim ScrollToEnd As Boolean
            Dim LastDisplayedRowIndex As Integer
            Dim Message As String
            Dim ArrMessages As String()

            LastDisplayedRowIndex = dgvLog.FirstDisplayedScrollingRowIndex + dgvLog.DisplayedRowCount(True)
            If LastDisplayedRowIndex >= dgvLog.RowCount - 1 Then
                ScrollToEnd = True
            Else
                ScrollToEnd = False
            End If

            _dtLog.BeginLoadData()
            Messages = Majodio.Mail.Server.Log.Logger.GetLatestLogEntries()
            If Not IsNothing(Messages) Then
                For i = 0 To Messages.GetUpperBound(0)
                    If Messages(i).Id = _LatestLogId Then
                        StartIndex = i + 1
                    End If
                Next
                For i = StartIndex To Messages.GetUpperBound(0)
                    Message = Messages(i).Msg
                    Message = Message.Replace(vbCrLf, vbCr)
                    Message = Message.Replace(vbLf, vbCr)
                    ArrMessages = Split(Message, vbCr)
                    For j As Int32 = 0 To ArrMessages.GetUpperBound(0)
                        If ArrMessages(j).Trim.Length > 0 Then
                            DR = _dtLog.NewRow
                            DR.Item("Time Stamp") = Messages(i).Id
                            DR.Item("Thread ID") = Messages(i).ThreadId
                            DR.Item("Message") = ArrMessages(j)
                            _dtLog.Rows.Add(DR)
                        End If
                    Next
                Next
                _LatestLogId = Messages(Messages.GetUpperBound(0)).Id
                While _dtLog.Rows.Count > MAX_LOG_FILE_ENTRIES
                    _dtLog.Rows.RemoveAt(0)
                End While
                _dtLog.EndLoadData()
            End If
            If ScrollToEnd Then
                dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.RowCount - 1
            End If
            dgvLog.Columns(0).Width = 120
            dgvLog.Columns(1).Width = 80
        End If
    End Sub

    Public Sub RefreshSessionCounters()
        If Me.InvokeRequired Then
            Dim d As New RefreshData(AddressOf RefreshSessionCounters)
            Me.Invoke(d, Nothing)
        Else
            lblCurrentSmtpSessions.Text = Performance.CurrentSmtpSessions
            lblTimedoutSmtpSessions.Text = Performance.TimedoutSmtpSessions
            lblTotalSmtpSessions.Text = Performance.TotalSmtpSessions
            lblMessagesReceived.Text = Performance.MessagesReceived
            lblMessagesRejectedInvalidFrom.Text = Performance.MessagesRejectedInvalidFrom
            lblMessagesRejectedDnsbl.Text = Performance.MessagesRejectedDnsbl
            lblConnectionsRejectedDnsbl.Text = Performance.ConnectionsRejectedDnsbl

            lblCurrentPop3Sessions.Text = Performance.CurrentPop3Sessions
            lblTotalPop3Sessions.Text = Performance.TotalPop3Sessions
            lblTimedoutPop3Sessions.Text = Performance.TimedoutPop3Sessions
            lblMessagesDelivered.Text = Performance.MessagesDelivered
        End If
    End Sub
End Class
