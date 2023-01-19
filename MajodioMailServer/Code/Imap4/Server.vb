Imports System.Net
Imports System.Threading

Namespace Imap4
    Public Class Server
        Private _ClientIp As IPAddress
        Private _SocketServer As Tcp.SocketServer

        Private _QueueCoordinator As QueueCoordinator
        Private _ServerStatus As Imap4ServerStatus

        Public Enum Imap4ServerStatus
            Running
            Stopping
            Stopped
        End Enum

        Shared Sub New()
            'ResponseMut = New System.Threading.Mutex(False, "ProcessResponse")
        End Sub

        Public Sub New(ByVal SS As Tcp.SocketServer)
            _SocketServer = SS
            _QueueCoordinator = New QueueCoordinator
            AddHandler _QueueCoordinator.ResponseReady, AddressOf Queue_ResponseReady
            AddHandler _QueueCoordinator.ClientDisconnect, AddressOf ClientDisconnect
        End Sub

        Public Sub Start()
            _ServerStatus = Imap4ServerStatus.Running
            _SocketServer.BeginReceiveData(vbCrLf, AddressOf DataReceived)
            _QueueCoordinator.SendGreeting()
        End Sub

        Public Sub [Stop]()
            _ServerStatus = Imap4ServerStatus.Stopped
        End Sub


        Public ReadOnly Property ServerStatus() As Imap4ServerStatus
            Get
                Return _ServerStatus
            End Get
        End Property

        Private Sub DataReceived(ByVal AR As IAsyncResult)
            Dim Dlgt As Tcp.ReceiveDataDelegate
            Dim Data As String
            Try
                Dlgt = CType(AR.AsyncState, Tcp.ReceiveDataDelegate)
                Data = Dlgt.EndInvoke(AR)
                _QueueCoordinator.ParseCommand(Data)
                While _QueueCoordinator.CurrentCommand.BytesExpected > -1
                    Data = _SocketServer.ReceiveData(_QueueCoordinator.CurrentCommand.BytesExpected)
                    Data &= _SocketServer.ReceiveData(vbCrLf)
                    _QueueCoordinator.ParseCommand(Data)
                End While
            Catch ex1 As Exception
                Dim MailEx As New MailException("An exception occurred while processing an IMAP4 message", ex1)
                MailEx.Save()
                _QueueCoordinator.ResetCurrentCommand()
                _SocketServer.SendData("* NO AN INTERNAL SERVER ERROR OCCURRED.  COMMAND RESET" & vbCrLf)
                Log.Logger.WriteLog("An error occurred while processing an IMAP4 connection" & vbCrLf & ex1.Source & vbCrLf & ex1.Message & vbCrLf & ex1.StackTrace)
            Finally
                If _ServerStatus = Imap4ServerStatus.Running Then
                    _SocketServer.BeginReceiveData(vbCrLf, AddressOf DataReceived)
                End If
            End Try
        End Sub

        Private Sub Queue_ResponseReady(ByVal Sender As Object, ByVal E As ResponseReadyEventArgs)
            Dim Response As New System.Text.StringBuilder
            Try
                If Not IsNothing(E) AndAlso Not IsNothing(E.Responses) Then
                    For i As Integer = 0 To E.Responses.Count - 1
                        If Not IsNothing(_SocketServer) AndAlso Not IsNothing(_SocketServer.Connection) AndAlso _SocketServer.Connection.Connected Then
                            Response.Append(E.Responses(i).ToString)
                        End If
                    Next
                    _SocketServer.SendData(Response.ToString)
                End If
            Finally
                If _ServerStatus = Imap4ServerStatus.Stopping Then
                    _ServerStatus = Imap4ServerStatus.Stopped
                    RemoveHandler _QueueCoordinator.ResponseReady, AddressOf Queue_ResponseReady
                End If
            End Try
        End Sub

        Private Sub ClientDisconnect(ByVal Sender As Object, ByVal E As EventArgs)
            _ServerStatus = Imap4ServerStatus.Stopping
            RemoveHandler _QueueCoordinator.ClientDisconnect, AddressOf ClientDisconnect
        End Sub

    End Class
End Namespace