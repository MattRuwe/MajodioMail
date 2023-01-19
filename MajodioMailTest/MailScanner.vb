Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports System.IO


Public Class MailScanner
    Private _ipLock As New Object

    Private _ipsScannedCountLock As New Object
    Private _ipsScannedCount As Long = 0

    Private _emailCountLock As New Object
    Private _emailCount As Long = 0

    Private Sub MailScanner_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblIpsScanned.Text = String.Empty
        lblEmailCount.Text = String.Empty
        _ip = My.Settings.StartIp
    End Sub

    Private Sub MailScanner_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not IsNothing(_streamWriter) Then
            _streamWriter.Close()
            _streamWriter.Dispose()
        End If
        If Not IsNothing(_scanThreads) Then
            StopScanning()
        End If
        My.Settings.StartIp = _ip
        My.Settings.Save()
    End Sub

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        btnStartStop.Enabled = False
        If btnStartStop.Text = "Start" Then
            StartScanning()
            btnStartStop.Text = "Stop"
        Else
            StopScanning()
            btnStartStop.Text = "Start"
        End If
        btnStartStop.Enabled = True
    End Sub

    Private _scanThreads() As Thread
    Private Sub StartScanning()
        System.Threading.ThreadPool.SetMaxThreads(350, 350)
        ReDim _scanThreads(nudThreads.Value)
        For i As Integer = 1 To nudThreads.Value
            _scanThreads(i) = New Thread(AddressOf Scan)
            _scanThreads(i).Start()
        Next
    End Sub

    Private Sub StopScanning()
        For i As Integer = 1 To _scanThreads.GetUpperBound(0)
            _scanThreads(i).Abort()
        Next
        _scanThreads = Nothing
    End Sub

    Private Sub Scan()
        Dim nextIp As IPAddress
        Dim connection As Socket
        Do
            nextIp = GetNextIpAddress()
            'UpdateOutput("Scanning IP: " & nextIp.ToString)
            connection = GetConnection(nextIp)
            If Not IsNothing(connection) Then
                UpdateOutput("IP " & nextIp.ToString & " is an e-mail server")
                AppendResultsToLog("IP " & nextIp.ToString & " is an e-mail server")
                IncrementEmailCount()
                UpdateEmailCountLabel()
            End If
            IncrementIpsScanned()
            UpdateIpsScannedLabel()
        Loop
    End Sub

    Private Sub IncrementIpsScanned()
        SyncLock (_ipsScannedCountLock)
            _ipsScannedCount += 1
        End SyncLock
    End Sub

    Private Delegate Sub UpdateCountDelegate()
    Private Sub UpdateIpsScannedLabel()
        If lblIpsScanned.InvokeRequired Then
            Dim d As New UpdateCountDelegate(AddressOf UpdateIpsScannedLabel)
            lblIpsScanned.Invoke(d)
        Else
            SyncLock (_ipsScannedCountLock)
                lblIpsScanned.Text = _ipsScannedCount
            End SyncLock
        End If
    End Sub

    Private Sub IncrementEmailCount()
        SyncLock (_emailCountLock)
            _emailCount += 1
        End SyncLock
    End Sub

    Private Sub UpdateEmailCountLabel()
        If lblIpsScanned.InvokeRequired Then
            Dim d As New UpdateCountDelegate(AddressOf UpdateEmailCountLabel)
            lblIpsScanned.Invoke(d)
        Else
            SyncLock (_emailCountLock)
                lblEmailCount.Text = _emailCount
            End SyncLock
        End If
    End Sub

    Private _ip As UInt64 = 0

    Private Function GetNextIpAddress() As IPAddress
        Dim rVal As IPAddress
        SyncLock _ipLock
            _ip += 1
            rVal = New IPAddress(_ip)
        End SyncLock

        Return rVal
    End Function

    Private Function GetConnection(ByVal ipAddress As IPAddress) As Socket
        Dim rVal As Socket = Nothing

        Try
            rVal = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            rVal.Connect(ipAddress, 25)
        Catch ex As SocketException
            If ex.ErrorCode <> 10060 Then
                UpdateOutput("Encountered error while processing IP (" & ipAddress.ToString & "): " & ex.Message)
            End If
            rVal = Nothing
        End Try

        Return rVal
    End Function

    Private Delegate Sub UpdateOutputDelegate(ByVal message As String)
    Private Sub UpdateOutput(ByVal message As String)
        If txtOutput.InvokeRequired Then
            Dim d As New UpdateOutputDelegate(AddressOf UpdateOutput)
            txtOutput.Invoke(d, New Object() {Thread.CurrentThread.ManagedThreadId.ToString.PadRight(6) & message})
        Else
            If Not message.EndsWith(vbCrLf) Then
                message &= vbCrLf
            End If
            txtOutput.AppendText(message)
        End If
    End Sub

    Private _streamWriterLock As New Object
    Private _streamWriter As StreamWriter
    Private Sub AppendResultsToLog(ByVal message As String)
        SyncLock (_streamWriterLock)
            If IsNothing(_streamWriter) Then
                _streamWriter = New StreamWriter("c:\ipresults.txt", True)
                _streamWriter.WriteLine("Processing started at: " & DateTime.Now.ToString())
                _streamWriter.Flush()
            End If

            _streamWriter.WriteLine(message)
        End SyncLock
    End Sub

End Class