Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports Majodio.Mail.common.Configuration
Imports Majodio.Mail.common.Smtp

Namespace Tcp
    Public Class CoreProcessingServer
        Private WithEvents _ParentThread As ListenTcpServer
        Private _RemoteIp As IPAddress
        Private _TcpSendReceive As SocketServer
        'Private _Config As Majodio.Mail.Common.Configuration.Config
        Public Event SessionClosed As eventhandler

        Friend Sub New(ByVal ParentThread As ListenTcpServer, ByVal AcceptedConnection As Socket)
            Majodio.Common.Utilities.TraceMe(Thread.CurrentThread.ManagedThreadId & ": Child server created")
            Me._ParentThread = ParentThread
            _RemoteIp = CType(AcceptedConnection.RemoteEndPoint, IPEndPoint).Address
            _ParentThread.AddOpenConnection()
            _TcpSendReceive = New SocketServer(AcceptedConnection)
            '_Config = New Majodio.Mail.Common.Configuration.Config
            Log.Logger.WriteLog("Connection opened from " & _RemoteIp.ToString)
            AddHandler _ParentThread.ServerShutdown, AddressOf OnServerShutdown
        End Sub

        Public Sub MakeConnectionSecure()
            _TcpSendReceive.MakeConnectionSecure()
        End Sub

        Private Sub OnServerShutdown()
            Majodio.Common.Utilities.TraceMe(Thread.CurrentThread.ManagedThreadId & ": Child server received shutdown notification")
            _ParentThread.SubtractOpenConnection()
            CloseAcceptedConnection()
        End Sub

        Friend Sub ProcessSmtpConnection()
            Dim ResponseMsg As Response()
            Dim RData As String
            Dim Smtp As New Smtp.Server(_RemoteIp)
            Dim DnsblResult As DnsblResults
            Dim i As Integer
            Dim MsgDelivered As Boolean = False
            SendUsageInformation("Processing SMTP Request")
            'Log.Logger.AddSmtpSession()
            Performance.IncrementCurrentSmtpSessions()
            Performance.IncrementTotalSmtpSessions()
            Log.Logger.WriteLog(SMTP_SESSION_STARTING_LOG_ENTRY)
            Majodio.Common.Utilities.TraceMe(Thread.CurrentThread.ManagedThreadId & ": SMTP Child Server processing request")
            Try
                DnsblResult = IsInDnsbl(_RemoteIp)
                Smtp.Deliverable = (Not DnsblResult.IsInDnsbl)
                If Not CanIpRelay(_RemoteIp) AndAlso DnsblResult.IsInDnsbl And Not RemoteConfigClient.RemoteConfig.AcceptAllMail Then
                    'The connection cannot be accepted, because it was found in the DNSBL and is NOT an IP address that can relay
                    Try
                        Plugin.Manager.DnsblBlockedIp(_RemoteIp)
                    Catch ex As Exception
                        Log.Logger.WriteError(ex, "Error occurred while attempting to call ISmtpServer.MailReceived.  Server continued normally.")
                    End Try
                    ResponseMsg = New Majodio.Mail.Common.Smtp.Response() {New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ServiceNotAvailable, "This server will not accept mail from your IP address because it is listed with " & DnsblResult.DnsblServer & ".")}
                    SendData(ResponseMsg)
                    Log.Logger.WriteLog("The connection was denied because the remote IP was found in the DNSBL")
                    'Log.Logger.AddMessagesRejectedDnsbl()
                    Performance.IncrementConnectionsRejectedDnsbl()
                    If Not IsNothing(DnsblResult.DnsblResult) Then
                        Log.Logger.WriteLog("The DNSBL returned the following addresses:")
                        For i = 0 To DnsblResult.DnsblResult.AddressList.GetUpperBound(0)
                            If Not IsNothing(DnsblResult.DnsblResult.AddressList(i)) Then
                                Log.Logger.WriteLog(DnsblResult.DnsblResult.AddressList(i).ToString)
                            End If
                        Next
                    End If
                Else
                    SendData(New Response() {New Response(Majodio.Mail.Common.Smtp.ResponseCode.ServiceReady, System.Net.Dns.GetHostName() & " " & Smtp.GetWelcomeMessage() & " Service Ready")})
                    Do
                        RData = _TcpSendReceive.ReceiveData(vbCrLf)
                        If _TcpSendReceive.ReceivedTimeoutExpired Then
                            Log.Logger.WriteLog(SMTP_SESSION_EXPIRED_LOG_ENTRY)
                            'Log.Logger.AddTimedoutSmtpSession()
                            Performance.IncrementTimedoutSmtpSessions()
                            Exit Do
                        End If
                        If Not IsNothing(RData) Then
                            ResponseMsg = Smtp.ProcessCommand(RData)
                            If Not IsNothing(ResponseMsg) AndAlso ResponseMsg.GetUpperBound(0) > -1 Then
                                SendData(ResponseMsg)
                                If Smtp.IsSecureConection And Not _TcpSendReceive.IsSecureConnection Then
                                    _TcpSendReceive.MakeConnectionSecure()
                                    If Not _TcpSendReceive.IsSecureConnection Then
                                        SendData(New Response(ResponseCode.ErrorWhileProcessing, "Error while securing connection"))
                                        Exit Do
                                    End If
                                Else
                                    If ResponseMsg(0).ResponseCode = Majodio.Mail.Common.Smtp.ResponseCode.StartMailInput Then
                                        RData = _TcpSendReceive.ReceiveData(vbCrLf & "." & vbCrLf)
                                        ResponseMsg = New Majodio.Mail.Common.Smtp.Response() {Smtp.QueueMail(RData)}
                                        'Microsoft's SmtpClient expects that the connection is reset following each e-mail message
                                        'this is to fix this "problem".  They should issue a RSET command instead.
                                        Smtp.Reset()
                                        SendData(ResponseMsg)
                                        If ResponseMsg(0).Msg = SMTP_MESSAGE_QUEUED_RESPONSE Then
                                            MsgDelivered = True
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Loop While Smtp.CurrentStatus <> Server.Smtp.Status.Closed _
                               AndAlso _ParentThread.CurrentStatus <> ServerStatus.Stopping _
                               AndAlso _ParentThread.CurrentStatus <> ServerStatus.Stopped _
                               AndAlso Not IsNothing(_TcpSendReceive) _
                               AndAlso Not IsNothing(_TcpSendReceive.Connection) _
                               AndAlso _TcpSendReceive.Connection.Connected
                    If MsgDelivered Then
                        'Log.Logger.AddMessagesReceived()
                        Performance.IncrementMessagesReceived()
                    End If

                End If
            Catch exc As Exception
                Dim MailEx As New MailException("An exception occurred while processing an SMTP message", exc)
                MailEx.Save()
                SendUsageInformation("Error occurred while processing a SMTP connection: " & exc.Source & " : " & exc.Message & " : " & exc.StackTrace)
                Majodio.Common.Utilities.TraceMe("Error Occurred: " & exc.Source & " : " & exc.Message & " : " & exc.StackTrace)
                Log.Logger.WriteLog(SMTP_SERVER_CRASHED_LOG_ENTRY & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
                If Not IsNothing(_TcpSendReceive) AndAlso _
                   Not IsNothing(_TcpSendReceive.Connection) AndAlso _
                   _TcpSendReceive.Connection.Connected Then
                    SendData(New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing, "-The server encoutered an error while processing your request"))
                    SendData(New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing, "Server Admin - Please check the server log for details"))
                End If
            Finally
                CloseAcceptedConnection()
                If Not IsNothing(Smtp) Then
                    Smtp.CurrentStatus = Server.Smtp.Status.Closed
                End If
            End Try

            Log.Logger.WriteLog(SMTP_SESSION_ENDING_LOG_ENTRY)
            'Log.Logger.RemoveSmtpSession()
            Performance.DecrementCurrentSmtpSessions()

            RaiseEvent SessionClosed(Me, EventArgs.Empty)
        End Sub

        Friend Sub ProcessPop3Connection()
            Dim Pop3 As New Pop3.Server
            Dim Response As Pop3.Response()
            Dim RData As String
            SendUsageInformation("Processing POP3 Request")
            'Log.Logger.AddPop3Session()
            Performance.IncrementCurrentPop3Sessions()
            Performance.IncrementTotalPop3Sessions()
            Log.Logger.WriteLog(POP3_SESSION_STARTING_LOG_ENTRY)
            Majodio.Common.Utilities.TraceMe(Thread.CurrentThread.ManagedThreadId & ": POP3 Child Server processing request")
            SendData(New Pop3.Response(Majodio.Mail.Server.Pop3.ResponseCode.Ok, "POP3 " & Majodio.Mail.Common.Configuration.RemoteConfigClient.RemoteConfig.ServerName & " Majodio Mail " & System.Reflection.Assembly.GetCallingAssembly.GetName.Version.ToString(4)))
            Try
                Do
                    RData = _TcpSendReceive.ReceiveData(vbCrLf)
                    If _TcpSendReceive.ReceivedTimeoutExpired() Then
                        Log.Logger.WriteLog(POP3_SESSION_EXPIRED_LOG_ENTRY)
                        'Log.Logger.AddTimedoutPop3Session()
                        Performance.IncrementTimedoutPop3Sessions()
                        Exit Do
                    End If
                    If Not IsNothing(RData) Then
                        Response = Pop3.ProcessMsg(RData)
                        If Not IsNothing(Response) Then
                            If Not SendData(Response) Then
                                Exit Do
                            End If
                            If Pop3.IsSecureConnection And Not _TcpSendReceive.IsSecureConnection Then
                                _TcpSendReceive.MakeConnectionSecure()
                            End If
                        End If
                    Else
                        Exit Do
                    End If
                Loop While Pop3.CurrentServerStatus <> Majodio.Mail.Server.Pop3.Status.Closed And _ParentThread.CurrentStatus <> ServerStatus.Stopping And _ParentThread.CurrentStatus <> ServerStatus.Stopped And _TcpSendReceive.Connection.Connected
            Catch exc As Exception
                Dim MailEx As New MailException("An exception occurred while processing an POP3 message", exc)
                MailEx.Save()
                SendUsageInformation("Error occurred while processing a POP3 connection: " & exc.Source & " : " & exc.Message & " : " & exc.StackTrace)
                Log.Logger.WriteLog(POP3_SERVER_CRASHED_LOG_ENTRY & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
                If _TcpSendReceive.Connection.Connected Then
                    SendData(New Pop3.Response(Majodio.Mail.Server.Pop3.ResponseCode.Error, "The server encoutered an error while processing your request"))
                    SendData(New Pop3.Response(Majodio.Mail.Server.Pop3.ResponseCode.Error, "Server Admin - Please check the server logs for details"))
                End If
            Finally
                CloseAcceptedConnection()
                If Not IsNothing(Pop3) Then
                    Pop3.CurrentServerStatus = Majodio.Mail.Server.Pop3.Status.Closed
                End If
            End Try
            Log.Logger.WriteLog(POP3_SESSION_ENDING_LOG_ENTRY)
            'Log.Logger.RemovePop3Session()
            Performance.DecrementCurrentPop3Sessions()
            RaiseEvent SessionClosed(Me, EventArgs.Empty)
        End Sub

        Friend Sub ProcessImap4Connection()
            Try
                SendUsageInformation("Processing IMAP4 Request")
                Log.Logger.WriteLog(IMAP4_SESSION_STARTING_LOG_ENTRY)
                _TcpSendReceive.DataReceiveTimeout = 1800 '30 minutes
                Dim IM4 As New Imap4.Server(_TcpSendReceive)
                IM4.Start()
                While IM4.ServerStatus = Imap4.Server.Imap4ServerStatus.Running AndAlso _ParentThread.CurrentStatus <> ServerStatus.Stopping AndAlso _ParentThread.CurrentStatus <> ServerStatus.Stopped AndAlso Not IsNothing(_TcpSendReceive.Connection) AndAlso _TcpSendReceive.Connection.Connected
                    System.Threading.Thread.Sleep(1000)
                End While
                IM4.Stop()
                RaiseEvent SessionClosed(Me, EventArgs.Empty)
            Finally
                CloseAcceptedConnection()
                Log.Logger.WriteLog(IMAP4_SESSION_ENDING_LOG_ENTRY)
            End Try
        End Sub

        Private Sub CloseAcceptedConnection()
            If Not IsNothing(_TcpSendReceive) Then
                _TcpSendReceive.CloseAcceptedConnection()
            End If
            If Not IsNothing(_ParentThread) Then
                _ParentThread.SubtractOpenConnection()
            End If

            RemoveHandler _ParentThread.ServerShutdown, AddressOf OnServerShutdown

            Utilities.TraceMe("Child Server Thread " & System.Threading.Thread.CurrentThread.Name & " stopping")
        End Sub

        Friend Function SendData(ByVal Data As Majodio.Mail.Common.Smtp.Response()) As Boolean
            Dim RVal As Boolean = True
            Dim i As Integer
            For i = 0 To Data.GetUpperBound(0)
                RVal = SendData(Data(i))
                If Not RVal Then
                    Exit For
                End If
            Next
            Return RVal
        End Function

        Friend Function SendData(ByVal Data As Majodio.Mail.Common.Smtp.Response) As Boolean
            Dim RVal As Boolean = True
            RVal = _TcpSendReceive.SendData(Data.ToString)
            Return RVal
        End Function

        Friend Function SendData(ByVal Data As Pop3.Response) As Boolean
            Dim RVal As Boolean = True
            RVal = _TcpSendReceive.SendData(Data.ToString)
            Return RVal
        End Function

        Friend Function SendData(ByVal Data As Pop3.Response()) As Boolean
            Dim i As Integer
            Dim RVal As Boolean = True
            For i = 0 To Data.GetUpperBound(0)
                RVal = SendData(Data(i))
                If Not RVal Then
                    Exit For
                End If
            Next
            Return True
        End Function
    End Class
End Namespace