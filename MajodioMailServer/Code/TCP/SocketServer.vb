Imports System.Net
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports Majodio.Mail.Common.Configuration

Namespace Tcp
    Public Delegate Function ReceiveDataDelegate(ByVal TerminatingString As String) As String
    Public Delegate Function SendDataDelegate(ByVal Data As String) As Boolean

    Public Class SocketServer
        Implements IDisposable

        Private _LastActivity As DateTime
        Private _AcceptedConnection As Socket
        Private _ReceiveBuffer As String
        Private _ReceiveAsyncCallback As AsyncCallback
        Private _SendAsyncCallback As AsyncCallback
        Private _IsSecureConnection As Boolean
        Private _dataReceiveTimeout As Integer


        Private _Ssl As SslStream

        Public Sub Dispose() Implements System.IDisposable.Dispose
            If Not IsNothing(_AcceptedConnection) Then
                If _AcceptedConnection.Connected Then
                    _AcceptedConnection.Close()
                End If
                _AcceptedConnection = Nothing
            End If
            _ReceiveBuffer = Nothing
        End Sub

        Public Sub New(ByRef AcceptedConnection As Socket)

            _LastActivity = Now
            _AcceptedConnection = AcceptedConnection
            _dataReceiveTimeout = 30
            _AcceptedConnection.ReceiveTimeout = DataReceiveTimeout * 1000
            ReceiveBuffer = String.Empty

            '_Ssl = New SslStream(New NetworkStream(AcceptedConnection))
        End Sub

        Public Sub New(ByVal AcceptedConnection As Socket, ByVal ReceiveAsyncCallback As AsyncCallback, ByVal SendAsyncCallback As AsyncCallback)
            Me.New(AcceptedConnection)
            _ReceiveAsyncCallback = ReceiveAsyncCallback
            _SendAsyncCallback = SendAsyncCallback
        End Sub

        Public ReadOnly Property IsSecureConnection() As Boolean
            Get
                Return _IsSecureConnection
            End Get
        End Property

        Public Sub MakeConnectionSecure()
            If Not _IsSecureConnection Then
                'Dim Config As New Majodio.Mail.Common.Configuration.Config()
                _Ssl.AuthenticateAsServer(New X509Certificate(RemoteConfigClient.RemoteConfig.SSLCertificatePath))
                Majodio.Common.Utilities.TraceMe("_ssl.IsAuthenticated = " & _Ssl.IsAuthenticated)
                If _Ssl.IsAuthenticated Then
                    _IsSecureConnection = True
                End If
            End If
        End Sub

        Private _receivedTimeoutExpired As Boolean
        Public ReadOnly Property ReceivedTimeoutExpired() As Boolean
            Get
                Return _receivedTimeoutExpired
            End Get
        End Property

        Public Property DataReceiveTimeout() As Integer
            Get
                Return _DataReceiveTimeout
            End Get
            Set(ByVal Value As Integer)
                _DataReceiveTimeout = Value
            End Set
        End Property

        Public Property LastActivity() As DateTime
            Get
                Dim RVal As DateTime = DateTime.Now
                If Not IsNothing(_LastActivity) Then
                    RVal = _LastActivity
                End If
                Return RVal
            End Get
            Set(ByVal Value As DateTime)
                _LastActivity = Value
            End Set
        End Property

        Public Property Connection() As Socket
            Get
                Return _AcceptedConnection
            End Get
            Set(ByVal Value As Socket)
                If Not IsNothing(Value) AndAlso Value.Connected Then
                    _AcceptedConnection = Value
                Else
                    Throw New Exception("The socket must be connected")
                End If
            End Set
        End Property

        Private Property ReceiveBuffer() As String
            Get
                Return _ReceiveBuffer
            End Get
            Set(ByVal Value As String)
                _ReceiveBuffer = Value
            End Set
        End Property

        Public Sub CloseAcceptedConnection()
            If Not IsNothing(_AcceptedConnection) AndAlso _AcceptedConnection.Connected() Then
                _AcceptedConnection.Shutdown(SocketShutdown.Both)
                _AcceptedConnection.Close()
                _AcceptedConnection = Nothing
            End If
        End Sub

        Public Sub BeginSendData(ByVal Data As String, ByVal Callback As AsyncCallback)
            Dim Dlgt As New SendDataDelegate(AddressOf SendData)
            Dlgt.BeginInvoke(Data, Callback, Dlgt)
        End Sub

        Public Function SendData(ByVal Data As String) As Boolean
            Dim RVal As Boolean
            Dim CurrentChunkSize As Integer
            Dim CurrentChunk(SEND_DATA_CHUNK_SIZE) As Byte
            Dim CurrentPosition As Integer = 0
            Dim BytesSent As Integer
            Dim ByteData As Byte()
            Try
                RVal = True
                ByteData = System.Text.ASCIIEncoding.ASCII.GetBytes(Data)
                While Not IsNothing(_AcceptedConnection) AndAlso _AcceptedConnection.Connected AndAlso CurrentPosition < ByteData.Length
                    CurrentChunkSize = SEND_DATA_CHUNK_SIZE
                    If CurrentPosition + CurrentChunkSize > ByteData.Length Then
                        CurrentChunkSize = ByteData.Length - CurrentPosition
                    End If
                    If IsSecureConnection Then
                        _Ssl.Write(ByteData, CurrentPosition, CurrentChunkSize)
                        BytesSent = ByteData.Length
                    Else
                        BytesSent = _AcceptedConnection.Send(ByteData, CurrentPosition, CurrentChunkSize, SocketFlags.None)
                    End If

                    If BytesSent <> CurrentChunkSize Then
                        Log.Logger.WriteLog(TCP_SEND_DATA_BYTE_COUNT_MISMATCH_LOG_ENTRY)
                    End If
                    CurrentPosition += BytesSent
                End While
                Log.Logger.WriteLog(Data, "Server: ")
                Majodio.Common.Utilities.TraceMe(Threading.Thread.CurrentThread.ManagedThreadId & ":S:" & Data)
            Catch exc As Exception
                If exc.GetType Is GetType(SocketException) AndAlso CType(exc, SocketException).ErrorCode = 10054 Then
                    Log.Logger.WriteLog(TCP_SEND_DATA_ERROR_LOG_ENTRY & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
                    Dim mailEx As New MailException("An exception occurred while receiving data from the client", exc)
                    mailEx.Save()
                End If
                RVal = False
            End Try
            _LastActivity = DateTime.Now
            Return RVal
        End Function

        Public Sub BeginReceiveData(ByVal TerminatingString As String, ByVal CallBack As AsyncCallback)
            Dim Dlgt As New ReceiveDataDelegate(AddressOf ReceiveData)
            Dlgt.BeginInvoke(TerminatingString, CallBack, Dlgt)
        End Sub

        Public Function ReceiveData(ByVal TerminatingString As String) As String
            Dim buffer(2047) As Byte
            Dim bytesReceived As Long = 1
            Dim rVal As String = String.Empty
            Dim terminatorIndex As Long
            Dim tmpStringBuffer As String = String.Empty
            Dim memoryStream As New System.IO.MemoryStream

            'Dim receivedString As String
            Try
                If IsNothing(ReceiveBuffer) Then
                    ReceiveBuffer = String.Empty
                End If
                While Not ContainsTerminator(memoryStream, TerminatingString, bytesReceived) AndAlso Not IsNothing(_AcceptedConnection) AndAlso _AcceptedConnection.Connected And bytesReceived > 0
                    Try
                        If _IsSecureConnection Then
                            bytesReceived = _Ssl.Read(buffer, 0, 8192)
                        Else
                            bytesReceived = _AcceptedConnection.Receive(buffer, 0, 2048, SocketFlags.None)
                        End If
                        _LastActivity = DateTime.Now
                    Catch exc As SocketException
                        If exc.ErrorCode = 10060 Then 'timeout
                            _receivedTimeoutExpired = True
                        ElseIf exc.ErrorCode = 10004 Then 'application is exiting
                        Else
                            Log.Logger.WriteLog(TCP_RECEIVE_DATA_ERROR_SOCKET_LOG_ENTRY & exc.ErrorCode)
                        End If

                        Exit While
                    Catch exc As ObjectDisposedException
                        Log.Logger.WriteLog(TCP_RECEIVE_DATA_ERROR_CONNECTION_CLOSED_LOG_ENTRY)
                        Exit While
                    End Try
                    memoryStream.Write(buffer, 0, bytesReceived)
                End While
                'TmpStringBuffer = System.Text.ASCIIEncoding.ASCII.GetString(memoryStream.GetBuffer(), 0, memoryStream.Length)
                tmpStringBuffer = System.Text.Encoding.UTF8.GetString(memoryStream.GetBuffer, 0, memoryStream.Length)
                TmpStringBuffer = ReceiveBuffer & TmpStringBuffer
                If Not IsNothing(TmpStringBuffer) AndAlso TmpStringBuffer.Length > 0 Then
                    TerminatorIndex = TmpStringBuffer.IndexOf(TerminatingString)
                    If TerminatorIndex > -1 Then
                        RVal = TmpStringBuffer.Substring(0, TerminatorIndex)
                        ReceiveBuffer = TmpStringBuffer.Substring(TerminatorIndex + TerminatingString.Length, TmpStringBuffer.Length - (TerminatorIndex + TerminatingString.Length))
                        Majodio.Common.Utilities.TraceMe(System.Threading.Thread.CurrentThread.ManagedThreadId & ":C:" & RVal)
                        Log.Logger.WriteLog(RVal, TCP_RECEIVE_DATA_PREPEND_TEXT_LOG_ENTRY)
                    Else
                        RVal = Nothing
                        ReceiveBuffer = TmpStringBuffer
                    End If
                Else
                    Log.Logger.WriteLog(TCP_RECEIVE_DATA_ERROR_NO_DATA_LOG_ENTRY)
                    RVal = Nothing
                End If
            Catch exc As Exception
                Log.Logger.WriteLog(TCP_RECEIVE_DATA_ERROR_GENERAL_LOG_ENTRY & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
            Finally
                If Not IsNothing(memoryStream) Then
                    memoryStream.Close()
                End If
            End Try
            Return RVal
        End Function

        Public Function ReceiveData(ByVal Bytes As Integer) As String
            Dim TmpBuffer(Bytes) As Byte
            Dim Buffer(Bytes) As Byte
            Dim BytesReceived As Integer = 0
            Dim TotalBytesReceived As Integer = 0
            Dim RVal As String
            While TotalBytesReceived < Bytes
                If IsSecureConnection Then
                    BytesReceived = _Ssl.Read(TmpBuffer, 0, Bytes)
                Else
                    BytesReceived = _AcceptedConnection.Receive(TmpBuffer, Bytes, SocketFlags.None)
                End If

                System.Buffer.BlockCopy(TmpBuffer, 0, Buffer, TotalBytesReceived, BytesReceived)
                TotalBytesReceived += BytesReceived
            End While
            'RVal = System.Text.ASCIIEncoding.ASCII.GetString(Buffer, 0, TotalBytesReceived)
            RVal = System.Text.Encoding.UTF8.GetString(Buffer, 0, TotalBytesReceived)
            Return RVal
        End Function

        Private Function ContainsTerminator(ByVal MS As System.IO.MemoryStream, ByVal TerminatingString As String, ByVal BytesLastRead As Integer) As Boolean
            Dim RVal As Boolean = False
            Dim Buffer As Byte()
            Dim TmpStringBuffer As String
            Dim Offset As Integer
            Dim Count As Integer

            If Not IsNothing(ReceiveBuffer) AndAlso ReceiveBuffer.IndexOf(TerminatingString) > -1 Then
                RVal = True
            ElseIf MS.Length > 0 Then
                Offset = MS.Length - BytesLastRead - TerminatingString.Length - 1
                If Offset < 0 Then
                    Offset = 0
                End If
                If Offset = 0 Then
                    Count = MS.Length
                Else
                    Count = MS.Length - Offset
                End If
                ReDim Buffer(Count - 1)
                MS.Position = Offset
                MS.Read(Buffer, 0, Count)
                'TmpStringBuffer = System.Text.ASCIIEncoding.ASCII.GetString(Buffer)
                TmpStringBuffer = System.Text.Encoding.UTF8.GetString(Buffer, 0, Count)
                If TmpStringBuffer.IndexOf(TerminatingString) > -1 Then
                    RVal = True
                End If
            End If

            Return RVal
        End Function
    End Class
End Namespace