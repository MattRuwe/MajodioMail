Imports System.IO

Public Class Main2

    Private corpus As New Majodio.Mail.Server.Bayesian.SpamCorpus
    Private _wait As New System.Threading.AutoResetEvent(False)
    Private _currentFile As String

    Private Sub btnCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        corpus.GetMessageProbability(txtMessage.Text)
    End Sub

    Private Sub btnAddHam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddHam.Click
        If chkMoveFiles.Checked Then
            If Not Directory.Exists(txtPath.Text & "/ham") Then
                Directory.CreateDirectory(txtPath.Text & "/ham")
            End If
            File.Move(_currentFile, txtPath.Text & "/ham/" & Path.GetFileName(_currentFile))
        End If
        corpus.ParseMessage(txtMessage.Text, False)
        _wait.Set()
    End Sub

    Private Sub btnAddSpam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSpam.Click
        If chkMoveFiles.Checked Then
            If Not Directory.Exists(txtPath.Text & "/spam") Then
                Directory.CreateDirectory(txtPath.Text & "/spam")
            End If
            File.Move(_currentFile, txtPath.Text & "/spam/" & Path.GetFileName(_currentFile))
        End If
        corpus.ParseMessage(txtMessage.Text, True)
        _wait.Set()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Dim T As New Threading.Thread(AddressOf ProcessFiles)
        T.Start()
    End Sub

    Private Sub ProcessFiles()
        Dim files As String()
        Dim sr As StreamReader
        Dim messageprob As Single
        Dim messageText As String
        If Directory.Exists(txtPath.Text) Then
            files = Directory.GetFiles(txtPath.Text, "*.msg")

            For i As Integer = 0 To files.GetUpperBound(0)
                _currentFile = files(i)
                sr = New StreamReader(files(i))
                messageText = sr.ReadToEnd()
                sr.Close()

                messageprob = corpus.GetMessageProbability(messageText)
                SetProbabilityLabel(messageprob)
                If GetCbFunction() = "Check Each Message" Then
                    SetMessageText(messageText)
                    _wait.WaitOne()
                ElseIf GetCbFunction() = "Mark All Messages as Ham" Then
                    corpus.ParseMessage(messageText, False)
                ElseIf GetCbFunction() = "Mark All Messages as Spam" Then
                    corpus.ParseMessage(messageText, True)
                End If
                System.Windows.Forms.Application.DoEvents()
            Next
        End If
    End Sub

    Private Delegate Function GetCbFunctionDelegate() As String
    Private Function GetCbFunction() As String
        If cbFunction.InvokeRequired Then
            Dim d As New GetCbFunctionDelegate(AddressOf GetCbFunction)
            Return cbFunction.Invoke(d)
        Else
            Return cbFunction.SelectedItem
        End If
    End Function

    Private Delegate Sub SetMessageTextDelegate(ByVal message As String)
    Private Sub SetMessageText(ByVal message As String)
        If txtMessage.InvokeRequired Then
            Dim d As New SetMessageTextDelegate(AddressOf SetMessageText)
            txtMessage.Invoke(d, New Object() {message})
        Else
            txtMessage.Text = message
        End If
    End Sub

    Private Delegate Sub SetProbabilityLabelDelegate(ByVal probability As Single)
    Private Sub SetProbabilityLabel(ByVal probability As Single)
        If lblSpamProbability.InvokeRequired Then
            Dim d As New SetProbabilityLabelDelegate(AddressOf SetProbabilityLabel)
            lblSpamProbability.Invoke(d, New Object() {probability})
        Else
            lblSpamProbability.Text = String.Format("{0:#.#######}", probability)
        End If
    End Sub

    Private Sub btnOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutput.Click
        txtMessage.Text = corpus.ToString()
    End Sub

    Private Sub btnSerialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSerialize.Click
        If Not IsNothing(corpus) Then
            corpus.Serialize("C:\Documents and Settings\ruwem\Desktop\Emails\test.corpus")
        End If
    End Sub

    Private Sub btnDeserialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeserialize.Click
        corpus = Majodio.Mail.Server.Bayesian.SpamCorpus.Deserialize("C:\Documents and Settings\ruwem\Desktop\Emails\test.corpus")
    End Sub
End Class