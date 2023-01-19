Imports System.IO

Public Class UninstallOptions
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnYes As System.Windows.Forms.Button
    Friend WithEvents btnNo As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnYes = New System.Windows.Forms.Button
        Me.btnNo = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnYes
        '
        Me.btnYes.Location = New System.Drawing.Point(48, 80)
        Me.btnYes.Name = "btnYes"
        Me.btnYes.TabIndex = 2
        Me.btnYes.Text = "&Yes"
        '
        'btnNo
        '
        Me.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnNo.Location = New System.Drawing.Point(128, 80)
        Me.btnNo.Name = "btnNo"
        Me.btnNo.TabIndex = 1
        Me.btnNo.Text = "&No"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(288, 56)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Do you want to remove all of the saved mail and configuration settings?  If you c" & _
        "hoose yes, any undelivered mail and any configuration information will be perman" & _
        "ently deleted."
        '
        'UninstallOptions
        '
        Me.AcceptButton = Me.btnNo
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnNo
        Me.ClientSize = New System.Drawing.Size(298, 111)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnNo)
        Me.Controls.Add(Me.btnYes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UninstallOptions"
        Me.ShowInTaskbar = False
        Me.Text = "Remove Mail/Configuration Information?"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _ApplicationPath As String
    Public Property ApplicationPath() As String
        Get
            If _ApplicationPath.EndsWith("\") Then
                Return _ApplicationPath & DATA_FILES_FOLDER
            Else
                Return _ApplicationPath & "\" & DATA_FILES_FOLDER
            End If
        End Get
        Set(ByVal Value As String)
            _ApplicationPath = Value
        End Set
    End Property

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            If MsgBox("Are you sure?", MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
                Directory.Delete(ApplicationPath, True)
            End If
        Catch ex As Exception
            MsgBox("An error occurred while attempting to delete the directory " & ApplicationPath & ".  More details can be found in the event log", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Error")
            Majodio.Functions.WriteToEventLog("Error while attempting to delete directory " & ApplicationPath, ex)
        End Try
        Me.Close()
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Me.Close()
    End Sub
End Class
