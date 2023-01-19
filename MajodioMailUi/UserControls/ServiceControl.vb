Public Class ServiceControl
#Region "Service Controller"
    Dim _SmtpSc As System.ServiceProcess.ServiceController
    Dim _Pop3Sc As System.ServiceProcess.ServiceController
    Dim _QueueSc As System.ServiceProcess.ServiceController
    Dim _LoggerSc As System.ServiceProcess.ServiceController
    Private _ServiceTimer As System.Timers.Timer

    Private Sub ServiceTime_Elapsed(ByVal Sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        Initialize()
        _ServiceTimer.Start()
    End Sub

    Public Overrides Sub Initialize()
        If IsNothing(_ServiceTimer) Then
            _ServiceTimer = New System.Timers.Timer(1000)
            _ServiceTimer.AutoReset = False
            AddHandler _ServiceTimer.Elapsed, AddressOf ServiceTime_Elapsed
        End If

        If IsNothing(_SmtpSc) Then
            _SmtpSc = New System.ServiceProcess.ServiceController("Majodio Smtp")
        End If
        If IsNothing(_Pop3Sc) Then
            _Pop3Sc = New System.ServiceProcess.ServiceController("Majodio Pop3")
        End If
        If IsNothing(_QueueSc) Then
            _QueueSc = New System.ServiceProcess.ServiceController("Majodio Queue Monitor")
        End If
        If IsNothing(_LoggerSc) Then
            _LoggerSc = New System.ServiceProcess.ServiceController("Majodio Logger")
        End If

        Try
            lblSmtpStatus.Text = System.Enum.GetName(GetType(System.ServiceProcess.ServiceControllerStatus), _SmtpSc.Status)
            If _SmtpSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
                btnControlSmtp.Text = "Start"
                btnControlSmtp.Enabled = True
            ElseIf _SmtpSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
                btnControlSmtp.Text = "Stop"
                btnControlSmtp.Enabled = True
            Else
                btnControlSmtp.Text = "Start"
                btnControlSmtp.Enabled = False
            End If
        Catch ex As Exception
            lblSmtpStatus.Text = "unknown"
            btnControlSmtp.Text = "Start"
            btnControlSmtp.Enabled = False
        End Try

        Try
            lblPop3Status.Text = System.Enum.GetName(GetType(System.ServiceProcess.ServiceControllerStatus), _Pop3Sc.Status)
            If _Pop3Sc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
                btnControlPop3.Text = "Start"
                btnControlPop3.Enabled = True
            ElseIf _Pop3Sc.Status = ServiceProcess.ServiceControllerStatus.Running Then
                btnControlPop3.Text = "Stop"
                btnControlPop3.Enabled = True
            Else
                btnControlPop3.Text = "Start"
                btnControlPop3.Enabled = False
            End If
        Catch ex As Exception
            lblPop3Status.Text = "unknown"
            btnControlPop3.Text = "Start"
            btnControlPop3.Enabled = False
        End Try

        Try
            lblQueueStatus.Text = System.Enum.GetName(GetType(System.ServiceProcess.ServiceControllerStatus), _QueueSc.Status)
            If _QueueSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
                btnControlQueue.Text = "Start"
                btnControlQueue.Enabled = True
            ElseIf _QueueSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
                btnControlQueue.Text = "Stop"
                btnControlQueue.Enabled = True
            Else
                btnControlQueue.Text = "Start"
                btnControlQueue.Enabled = False
            End If
        Catch ex As Exception
            lblQueueStatus.Text = "unknown"
            btnControlQueue.Text = "Start"
            btnControlQueue.Enabled = False
        End Try

        Try
            lblLoggerStatus.Text = System.Enum.GetName(GetType(System.ServiceProcess.ServiceControllerStatus), _LoggerSc.Status)
            If _LoggerSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
                btnControlLogger.Text = "Start"
                btnControlLogger.Enabled = True
            ElseIf _LoggerSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
                btnControlLogger.Text = "Stop"
                btnControlLogger.Enabled = True
            Else
                btnControlLogger.Text = "Start"
                btnControlLogger.Enabled = False
            End If
        Catch ex As Exception
            lblLoggerStatus.Text = "unknown"
            btnControlLogger.Text = "Start"
            btnControlLogger.Enabled = False
        End Try
        If btnControlSmtp.Enabled Or btnControlPop3.Enabled Or btnControlQueue.Enabled Or btnControlLogger.Enabled Then
            gbManuallyStartServer.Visible = False
        Else
            gbManuallyStartServer.Visible = True
        End If

    End Sub

    Private Sub btnControlSmtp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnControlSmtp.Click
        btnControlSmtp.Enabled = False
        If _SmtpSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
            _SmtpSc.Stop()
            _SmtpSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Stopped)
        ElseIf _SmtpSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
            _SmtpSc.Start()
            _SmtpSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Running)
        End If
        Initialize()
    End Sub

    Private Sub btnControlPop3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnControlPop3.Click
        btnControlPop3.Enabled = False
        If _Pop3Sc.Status = ServiceProcess.ServiceControllerStatus.Running Then
            _Pop3Sc.Stop()
            _Pop3Sc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Stopped)
        ElseIf _Pop3Sc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
            _Pop3Sc.Start()
            _Pop3Sc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Running)
        End If
        Initialize()
    End Sub

    Private Sub btnControlQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnControlQueue.Click
        btnControlQueue.Enabled = False
        If _QueueSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
            _QueueSc.Stop()
            _QueueSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Stopped)
        ElseIf _QueueSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
            _QueueSc.Start()
            _QueueSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Running)
        End If
        Initialize()
    End Sub

    Private Sub btnControlLogger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnControlLogger.Click
        btnControlLogger.Enabled = False
        If _LoggerSc.Status = ServiceProcess.ServiceControllerStatus.Running Then
            _LoggerSc.Stop()
            _LoggerSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Stopped)
        ElseIf _LoggerSc.Status = ServiceProcess.ServiceControllerStatus.Stopped Then
            _LoggerSc.Start()
            _LoggerSc.WaitForStatus(ServiceProcess.ServiceControllerStatus.Running)
        End If
        Initialize()
    End Sub

    Private Sub btnManuallyStartServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManuallyStartServer.Click
        Dim T As New Threading.Thread(AddressOf StartMailConsole)
        T.Start()
    End Sub

    Private Sub StartMailConsole()
        Dim PI As ProcessStartInfo
        Dim P As Process
        DisableStartMailConsole()
        PI = New ProcessStartInfo(Majodio.Mail.Common.GetApplicationDirectory & "\majodiomailconsole.exe")
        P = New Process
        P.StartInfo = PI
        P.Start()
        While Not P.HasExited
            System.Threading.Thread.Sleep(1000)
        End While
        EnableStartMailConsole()
    End Sub

    Private Delegate Sub StartMailConsoleDelegate()

    Private Sub EnableStartMailConsole()
        If Me.InvokeRequired Then
            Dim D As New StartMailConsoleDelegate(AddressOf EnableStartMailConsole)
            Me.Invoke(D)
        Else
            btnManuallyStartServer.Enabled = True
        End If
    End Sub

    Private Sub DisableStartMailConsole()
        If Me.InvokeRequired Then
            Dim D As New StartMailConsoleDelegate(AddressOf DisableStartMailConsole)
            Me.Invoke(D)
        Else
            btnManuallyStartServer.Enabled = False
        End If
    End Sub
#End Region
End Class
