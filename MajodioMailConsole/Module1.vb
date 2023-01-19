Imports System.threading
Imports System.IO
Imports Majodio.Mail.Common.Configuration
Imports System.Runtime.Remoting

Module Module1
    Private _DOut As DebugOutput
    Private _configServerInstance As Majodio.Mail.Common.Configuration.Server
    Private _messageFolderServerInstance As Majodio.Mail.Common.Storage.QueuedMail.Server
    Private _logServerInstance As Log.Server

    Sub Main()
        'CType(Debug.Listeners(0), DefaultTraceListener).LogFileName = "c:\log.txt"

        Debug.Listeners.RemoveAt(0)

        Dim T As New Thread(AddressOf OpenDebug)
        T.Start()

        'Debug.Listeners.Add(New TextWriterTraceListener(Console.Out))

        Dim configServer As AppDomain = AppDomain.CreateDomain("ConfigServer")
        _configServerInstance = CType(configServer.CreateInstanceAndUnwrap("MajodioMailCommon", "Majodio.Mail.Common.Configuration.Server"), Majodio.Mail.Common.Configuration.Server)

        Dim messageServer As AppDomain = AppDomain.CreateDomain("MessageFolderServer")
        _messageFolderServerInstance = CType(messageServer.CreateInstanceAndUnwrap("MajodioMailCommon", "Majodio.Mail.Common.Storage.QueuedMail.Server"), Majodio.Mail.Common.Storage.QueuedMail.Server)

        Dim logServer As AppDomain = AppDomain.CreateDomain("LogServer")
        _logServerInstance = CType(logServer.CreateInstanceAndUnwrap("MajodioMailServer", "Majodio.Mail.Server.Log.Server"), Log.Server)

        Dim S As New Tcp.ListenTcpServer(Tcp.ServerType.Smtp)
        Dim P As New Tcp.ListenTcpServer(Tcp.ServerType.Pop3)
        Dim SP As New Tcp.ListenTcpServer(Tcp.ServerType.SecurePop3)
        Dim I As New Tcp.ListenTcpServer(Tcp.ServerType.Imap4)
        Dim Q As New QueueMonitor
        Dim PR As New Pop3.RelayManager
        Dim amm As New AdminMessageManager()


        Console.WriteLine("SMTP Server Starting")
        Majodio.Common.Utilities.TraceMe("SMTP Server Starting")
        S.StartServer()
        Console.WriteLine("POP3 Server Starting")
        Majodio.Common.Utilities.TraceMe("POP3 Server Starting")
        P.StartServer()
        Majodio.Common.Utilities.TraceMe("Secure POP3 Server Starting")
        Try
            SP.StartServer()
        Catch ex As Exception
            Majodio.Common.Utilities.TraceMe(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
        Console.WriteLine("IMAP4 Server Starting")
        Majodio.Common.Utilities.TraceMe("IMAP4 Server Starting")
        I.StartServer()
        Console.WriteLine("Queue Monitor Server Starting")
        Majodio.Common.Utilities.TraceMe("Queue Monitor Server Starting")
        Q.Start()
        Majodio.Common.Utilities.TraceMe("POP3 Relay Manager Server Starting")
        PR.Start()
        Majodio.Common.Utilities.TraceMe("Admin Message Manager Starting")
        amm.Start()
        Console.WriteLine()
        Console.WriteLine("All servers started.  Press Enter to shutdown servers")
        Console.ReadLine()
        Console.WriteLine("SMTP Server Stopping")
        Majodio.Common.Utilities.TraceMe("SMTP Server Stopping")
        S.StopServer()
        Console.WriteLine("POP3 Server Stopping")
        Majodio.Common.Utilities.TraceMe("POP3 Server Stopping")
        P.StopServer()
        Console.WriteLine("IMAP4 Server Stopping")
        Majodio.Common.Utilities.TraceMe("IMAP4 Server Stopping")
        I.StopServer()
        Console.WriteLine("Queue Monitor Server Stopping")
        Majodio.Common.Utilities.TraceMe("Queue Monitor Server Stopping")
        Q.Stop()
        Majodio.Common.Utilities.TraceMe("POP3 Relay manager Server Stopping")
        PR.Stop()
        Majodio.Common.Utilities.TraceMe("Admin Message Manager Stopping")
        amm.Stop()
        Console.WriteLine("Log Server Stopping")
        Majodio.Common.Utilities.TraceMe("Log Server Stopping")
        _logServerInstance.StopServer()
        Console.WriteLine("Config Server Stopping")
        Majodio.Common.Utilities.TraceMe("Config Server Stopping")
        _configServerInstance.StopServer()
        _messageFolderServerInstance.StopServer()
        Do
            Console.Write(". ")
            Thread.Sleep(500)
        Loop While S.CurrentStatus <> Tcp.ServerStatus.Stopped Or P.CurrentStatus <> Tcp.ServerStatus.Stopped Or I.CurrentStatus <> Tcp.ServerStatus.Stopped
        CloseDebug()
        Console.WriteLine(vbCrLf & "Server shutdown.  Press any key to exit")
        Majodio.Common.Utilities.TraceMe("Server shutdown")
        Console.Read()
    End Sub

    Private Sub OpenDebug()
        _DOut = New DebugOutput
        System.Windows.Forms.Application.Run(_DOut)
        _DOut.Focus()
    End Sub

    Private Delegate Sub DelCloseDebug()
    Private Sub CloseDebug()
        If _DOut.InvokeRequired Then
            Dim D As New DelCloseDebug(AddressOf CloseDebug)
            _DOut.Invoke(D)
        Else
            _DOut.Close()
        End If
    End Sub

    Private _pingtmr As System.Timers.Timer
    Private _Pop3 As Tcp.ListenTcpServer
    Private Sub PingTmr_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
        Majodio.Common.Messaging.Ping.Ping("Majodio Mail POP3", System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4))
    End Sub
End Module
