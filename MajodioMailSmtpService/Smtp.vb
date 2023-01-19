Imports System.ServiceProcess
Imports Majodio.Mail.Server
Imports System.Threading
Imports Majodio.Mail.common.Configuration

Public Class Smtp
    Inherits System.ServiceProcess.ServiceBase
    Private _Smtp As Tcp.ListenTcpServer
    Private _adminMessageManager As AdminMessageManager
#Region " Component Designer generated code "

    Public Sub New()
        MyBase.New()

        ' This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    <MTAThread()> _
    Shared Sub Main()
        System.ServiceProcess.ServiceBase.Run(New System.ServiceProcess.ServiceBase() {New Smtp})
    End Sub

    'UserService overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  Do not modify it
    ' using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
        Me.ServiceName = "Majodio Smtp"
    End Sub

#End Region

    Private _PingTmr As System.Timers.Timer

    Protected Overrides Sub OnStart(ByVal args() As String)
        _Smtp = New Tcp.ListenTcpServer(Tcp.ServerType.Smtp)
        _Smtp.StartServer()

        _adminMessageManager = New AdminMessageManager()
        _adminMessageManager.Start()

        'Dim C As New Config
        If RemoteConfigClient.RemoteConfig.SendUsageInformation Then
            PingTmr_Elapsed(Nothing, Nothing)
            _PingTmr = New System.Timers.Timer(Majodio.Common.Constants.MESSAGE_PING_RATE)
            AddHandler _PingTmr.Elapsed, AddressOf PingTmr_Elapsed
            _PingTmr.AutoReset = True
            _PingTmr.Start()
        End If
    End Sub

    Private Sub PingTmr_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
        Majodio.Common.Messaging.Ping.Ping("Majodio Mail SMTP", System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(4))
    End Sub

    Protected Overrides Sub OnStop()
        If Not IsNothing(_PingTmr) Then
            _PingTmr.Stop()
            RemoveHandler _PingTmr.Elapsed, AddressOf PingTmr_Elapsed
            _PingTmr = Nothing
        End If
        Dim i As Integer = 0
        If Not IsNothing(_Smtp) Then
            _Smtp.StopServer()
            While Not _Smtp.CurrentStatus = Tcp.ServerStatus.Stopped And i < 20
                Thread.Sleep(500)
                i += 1
            End While
            _Smtp = Nothing
        End If

        If Not IsNothing(_adminMessageManager) Then
            _adminMessageManager.Stop()
        End If
    End Sub

End Class
