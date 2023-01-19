Imports System.Net
Imports System.Net.Sockets
Imports System.threading
Imports Majodio.Mail.Common.Configuration

Namespace Tcp
    Public Delegate Sub ServerShutdown()

    Public Class ListenTcpServer
        Private _Listener As Socket
        Private _ServerNumber As Long
        Private _ServerAvailable As Boolean
        Private _CurrentStatus As ServerStatus
        Private _ListenThread As Thread
        Private _ServerInitialized As Boolean
        Private _OpenConnections As Integer
        Private _CurrentServerType As ServerType
        'Private _Config As Majodio.Mail.Common.Configuration.Config

        Private _TotalConnectionsReceived As Int64

        Public Event ServerShutdown As ServerShutdown

        Public Sub New(ByVal ST As ServerType)
            Log.Logger.WriteLog("About to start " & System.Enum.GetName(GetType(ServerType), ST) & " server")
            _CurrentStatus = ServerStatus.Stopped
            _ServerInitialized = False
            _CurrentServerType = ST
            '_Config = New Majodio.Mail.Common.Configuration.Config
            _TotalConnectionsReceived = 0
        End Sub

        Private Sub Initialize()
            Log.Logger.WriteLog("Initializing Server")
            _ServerNumber = 0
            _OpenConnections = 0
            _CurrentStatus = ServerStatus.Starting
            _ServerAvailable = True
            If _CurrentServerType = ServerType.Smtp Then
                Log.Logger.WriteLog("Starting a SMTP listener")
                _Listener = TCPServer.GetSmtpSocket()
                SendUsageInformation("SMTP Server Started")
            ElseIf _CurrentServerType = ServerType.Pop3 Then
                Log.Logger.WriteLog("Starting a POP3 listener")
                _Listener = TCPServer.GetPop3Socket()
                SendUsageInformation("POP3 Server Started")
            ElseIf _CurrentServerType = ServerType.SecurePop3 Then
                If System.IO.File.Exists(RemoteConfigClient.RemoteConfig.SSLCertificatePath) Then
                    Log.Logger.WriteLog("Starting a Secure POP3 listener")
                    _Listener = TCPServer.GetSecurePop3Socket()
                    SendUsageInformation("Secure POP3 Server Started")
                Else
                    Log.Logger.WriteLog("Cannot start secure POP3 server because no x509 certificate was available")
                    Throw New Exception("Cannot start secure POP3 server because no x509 certificate was available")
                End If
            ElseIf _CurrentServerType = ServerType.Imap4 Then
                Log.Logger.WriteLog("Starting a IMAP4 listener")
                _Listener = TCPServer.GetImap4Socket()
                SendUsageInformation("IMAP4 Server Started")
            End If
            _ServerInitialized = True
        End Sub

        Public Sub StartServer()
            Try
                If Not _ServerInitialized Then
                    Initialize()
                End If
                _ListenThread = New Thread(AddressOf StartServerThread)
                _ListenThread.Name = "StartServer"
                _ListenThread.Start()
            Catch exc As SocketException
                If exc.ErrorCode = 10048 Then
                    Majodio.Common.Utilities.TraceMe("A server is already listening on the specified port.")
                    Log.Logger.WriteLog("A server is already listening on the specified port")
                End If
                _CurrentStatus = ServerStatus.Stopped
            End Try
        End Sub

        Private Sub StartServerThread()
            _CurrentStatus = ServerStatus.Running
            _CurrentStatus = ServerStatus.WaitingForConnect
            _Listener.BeginAccept(New AsyncCallback(AddressOf AcceptClientConnections), _Listener)
        End Sub

        Private Sub AcceptClientConnections(ByVal ar As IAsyncResult)
            Dim AcceptedConnection As Socket
            Dim ChildServer As CoreProcessingServer
            Dim childServerThread As Thread = Nothing
            Dim RemoteIp As IPAddress
            If TRIAL_SOFTWARE Then
                _TotalConnectionsReceived += 1
            End If

            AcceptedConnection = CType(ar.AsyncState(), Socket).EndAccept(ar)
            RemoteIp = IPAddress.Parse(CType(AcceptedConnection.RemoteEndPoint, IPEndPoint).Address.ToString())
            If _CurrentStatus = ServerStatus.Running Or _CurrentStatus = ServerStatus.WaitingForConnect Or _CurrentStatus = ServerStatus.ConnectionReceived Then
                If Not TRIAL_SOFTWARE OrElse _TotalConnectionsReceived < TRIAL_SOFTWARE_MAX_CONNECTIONS Then
                    If Not RemoteConfigClient.RemoteConfig.BannedIpExists(RemoteIp) Then
                        ChildServer = New CoreProcessingServer(Me, AcceptedConnection)
                        If _CurrentServerType = ServerType.Smtp Then
                            childServerThread = New Thread(AddressOf ChildServer.ProcessSmtpConnection)
                            childServerThread.Name = "SMTP_SERVER_" & _ServerNumber
                        ElseIf _CurrentServerType = ServerType.Pop3 Then
                            childServerThread = New Thread(AddressOf ChildServer.ProcessPop3Connection)
                            childServerThread.Name = "POP3_SERVER_" & _ServerNumber
                        ElseIf _CurrentServerType = ServerType.SecurePop3 Then
                            'Change the connection to an SSL secured connection
                            ChildServer.MakeConnectionSecure()
                            childServerThread = New Thread(AddressOf ChildServer.ProcessPop3Connection)
                            childServerThread.Name = "SECURE_POP3_SERVER_" & _ServerNumber
                        ElseIf _CurrentServerType = ServerType.Imap4 Then
                            childServerThread = New Thread(AddressOf ChildServer.ProcessImap4Connection)
                            childServerThread.Name = "IMAP4_SERVER_" & _ServerNumber
                        End If
                        childServerThread.Start()
                        Utilities.TraceMe("New child server thread started: " & childServerThread.Name)
                        _ServerNumber += 1
                    Else
                        Log.Logger.WriteLog("Conection denied from banned IP: " & RemoteIp.ToString)
                        CloseConnection(AcceptedConnection)
                    End If
                Else
                    Log.Logger.WriteLog("Connection denied from IP (" & RemoteIp.ToString & ") because the trial period has expired.  Please purchase a full version from Majodio Software (http://www.majodio.com)")
                    CloseConnection(AcceptedConnection)
                End If
                StartServer()
            End If
        End Sub

        Private Shared Sub CloseConnection(ByRef Connection As Socket)
            If Not IsNothing(Connection) Then
                Connection.Shutdown(SocketShutdown.Both)
                Connection.Close()
            End If
        End Sub

        Public Sub StopServer()
            If Not IsNothing(_Listener) AndAlso _Listener.Connected Then
                _Listener.Shutdown(SocketShutdown.Both)
                _Listener.Close()
            End If
            If OpenConnections > 0 Then
                _CurrentStatus = ServerStatus.Stopping
            Else
                _CurrentStatus = ServerStatus.Stopped
            End If
            If _CurrentServerType = ServerType.Smtp Then
                SendUsageInformation("SMTP Server Stopped")
            ElseIf _CurrentServerType = ServerType.Pop3 Then
                SendUsageInformation("POP3 Server Stopped")
            ElseIf _CurrentServerType = ServerType.Imap4 Then
                SendUsageInformation("IMAP4 Server Stopped")
            End If
            _ServerAvailable = False
            RaiseEvent ServerShutdown()
        End Sub

        Public ReadOnly Property CurrentStatus() As ServerStatus
            Get
                Return _CurrentStatus
            End Get
        End Property

        Public ReadOnly Property OpenConnections() As Integer
            Get
                Return _OpenConnections
            End Get
        End Property

        Friend Sub AddOpenConnection()
            _OpenConnections += 1
        End Sub

        Friend Sub SubtractOpenConnection()
            Dim M As Mutex = Nothing
            Try
                M = New Mutex(False, "SubtractOpenConnection")
                M.WaitOne()
                _OpenConnections -= 1
                If _OpenConnections = 0 And _CurrentStatus = ServerStatus.Stopping Then
                    _CurrentStatus = ServerStatus.Stopped
                End If
            Finally
                M.ReleaseMutex()
            End Try
        End Sub
    End Class
End Namespace