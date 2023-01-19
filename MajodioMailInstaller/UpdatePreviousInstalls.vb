Imports System.io
Imports system.Windows.Forms
Imports System.Xml

Public Class UpdatePreviousInstalls

    Private Sub UpdatePreviousInstalls_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Start()
    End Sub

    Private Sub Start()
        lblStatus.Text = "Moving mail from folder ""RetainedMail"" to ""_RetainedMail""."
        Application.DoEvents()
        UpgradeRetainedMailFolders()
        lblStatus.Text = "Upgrading configuration file"
        pbUpgradeProgress.Increment(50)
        Application.DoEvents()
        UpdateConfiguration()
        pbUpgradeProgress.Increment(49)
        Application.DoEvents()
        System.Threading.Thread.Sleep(500)
        Me.Close()
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	10/5/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub UpgradeRetainedMailFolders()
        UpgradeRetainedMailFolder(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CurrentFolder"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	10/5/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub UpgradeRetainedMailFolder(ByVal CurrentFolder As String)
        Dim Folders As String()
        Dim RootFolder As Boolean
        RootFolder = (GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER = CurrentFolder)
        If Directory.Exists(CurrentFolder) Then
            Folders = Directory.GetDirectories(CurrentFolder)
            If RootFolder Then
                pbUpgradeProgress.Maximum = Folders.GetUpperBound(0)
            End If
            For i As Integer = 0 To Folders.GetUpperBound(0)
                If RootFolder Then
                    pbUpgradeProgress.PerformStep()
                End If
                UpgradeRetainedMailFolder(Folders(i))
                If Directory.Exists(Folders(i) & "\" & "RetainedMail") Then
                    'Move the contents to the _RetainedMail Folder
                    If Not Directory.Exists(Folders(i) & "\" & "_RetainedMail") Then
                        Directory.Move(Folders(i) & "\RetainedMail", Folders(i) & "\_RetainedMail")
                    Else
                        For Each FileName As String In Directory.GetFiles(Folders(i) & "\RetainedMail")
                            File.Move(FileName, Folders(i) & "\_RetainedMail\" & Path.GetFileName(FileName))
                        Next
                        Directory.Delete(Folders(i) & "\RetainedMail")
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub UpdateConfiguration()
        Dim ConfigFilePath As String = GetApplicationDirectory() & "\" & CONFIG_FILE_LOCATION
        If File.Exists(ConfigFilePath) Then
            Dim Config As New Majodio.Mail.Common.Configuration.Config()
            Config.PerformUpdgrades()
        End If
    End Sub
End Class