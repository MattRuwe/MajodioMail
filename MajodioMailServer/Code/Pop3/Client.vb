Imports Majodio.Mail.Server.Tcp
Imports System.Net.Sockets
Imports System.Text.RegularExpressions

Namespace Pop3
    ''' <summary>
    ''' A POP3 client capable of performing POP3 operations
    ''' </summary>
    ''' <remarks>Use this class as a base for implementing POP3 commands/operations.  Primitive methods are already implemented.</remarks>
    Public Class Client

        Private _host As String
        Private _port As Int32
        Private _username As String
        Private _password As String
        Private _SS As SocketServer
        Private _Connection As Socket

        ''' <summary>
        ''' Instantiates a new instance of the POP3 class which will connect by default on port 110.
        ''' </summary>
        ''' <remarks>The username/password for the connection is not set and attempting to call the Connect method with empty paramters will fail.</remarks>
        Public Sub New()
            _port = 110
        End Sub

        Public Sub New(ByVal Host As String, ByVal Username As String, ByVal Password As String)
            _host = Host
            _username = Username
            _password = Password
        End Sub

        ''' <summary>
        ''' Uses the values set during construction or through properties to connect to a POP3 host
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Connect()
            Connect(_host, _port, _username, _password)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Host"></param>
        ''' <param name="Username"></param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        Public Sub Connect(ByVal Host As String, ByVal Username As String, ByVal Password As String)
            Connect(Host, Username, Password, _port)
        End Sub

        Public Sub Connect(ByVal Host As String, ByVal Username As String, ByVal Password As String, ByVal Port As Int32)
            Dim RData As String
            Dim PResponse As Response

            If IsNothing(Host) OrElse Host.Trim.Length = 0 Then
                Throw New ArgumentException("The hostname can not be empty/null")
            End If
            If IsNothing(Username) OrElse Username.Trim.Length = 0 Then
                Throw New ArgumentException("The username cannot be empty/null")
            End If
            If IsNothing(Password) Then
                Password = String.Empty
            End If
            _host = Host
            _port = Port
            _username = Username
            _password = Password

            _Connection = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            _Connection.Connect(_host, _port)

            If Not _Connection.Connected Then
                Throw New ArgumentException("Could not connect to host " & _host & " on port " & _port)
            Else
                _SS = New SocketServer(_Connection)
            End If

            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)
            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ArgumentException("The server " & Host & " is not currently accepting connections:  " + PResponse.Msg)
            End If

            _SS.SendData("user " & Username & vbCrLf)
            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)
            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ArgumentException("The server rejected the username " & Username & ":  " & PResponse.Msg)
            End If

            _SS.SendData("pass " & Password & vbCrLf)
            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)
            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ArgumentException("The server rejected the username/password: " & PResponse.Msg)
            End If
        End Sub

        Public Sub Disconnect()
            Dim rData As String
            Dim response As Response
            If _SS.Connection.Connected Then
                _SS.SendData("quit" & vbCrLf)
                rData = _SS.ReceiveData(vbCrLf)
                response = New Response(rData)
                If response.ResponseCode <> ResponseCode.Ok Then
                    Throw New InvalidOperationException("Failed to close the connection with the remote POP3 server: " & response.Msg)
                End If
            End If
        End Sub

        Public Function GetMessageCount() As Integer
            Dim RData As String = String.Empty
            Dim PResponse As Response
            Dim RVal As Integer = 0
            Dim M As Match

            If Not _Connection.Connected Then
                Throw New ClientException("Cannot retrieve message count when no connection is present")
            End If

            _SS.SendData("STAT" & vbCrLf)
            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)

            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ClientException("Server responded with an error: " & PResponse.Msg)
            End If

            M = Regex.Match(PResponse.Msg, "(?is)(?<Messages>\d+)\s+(?<Size>\d+)")
            If Not M.Success Then
                Throw New ClientException("The number and size of messages could not be determined: " & PResponse.Msg)
            End If

            RVal = Convert.ToInt32(M.Groups("Messages").Value)

            Return RVal

        End Function

        Public Function GetMessage(ByVal MessageIndex As Integer) As Mime.Message
            Dim RVal As Mime.Message
            Dim RData As String
            Dim PResponse As Response

            If Not _Connection.Connected Then
                Throw New ClientException("Cannot retrieve message count when no connection is present")
            End If

            If MessageIndex < 0 Then
                Throw New IndexOutOfRangeException("The value for MessageIndex (" & MessageIndex & ") must be greater than or equal to 0")
            End If

            _SS.SendData("RETR " & MessageIndex + 1 & vbCrLf)
            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)
            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ClientException("The server returned an error code: " & PResponse.Msg)
            End If

            RData = _SS.ReceiveData(vbCrLf & "." & vbCrLf)
            While RData.Length > 1 And RData.StartsWith(vbLf)
                RData = RData.Substring(1)
            End While

            RVal = New Mime.Message(RData)

            Return RVal
        End Function

        Public Sub DeleteMessage(ByVal MessageIndex As Integer)
            Dim RData As String
            Dim PResponse As Response

            If Not _Connection.Connected Then
                Throw New ClientException("Cannot retrieve message count when no connection is present")
            End If

            If MessageIndex < 0 Then
                Throw New IndexOutOfRangeException("The value for MessageIndex (" & MessageIndex & ") must be greater than or equal to 0")
            End If

            _SS.SendData("DELE " & MessageIndex + 1 & vbCrLf)
            RData = _SS.ReceiveData(vbCrLf)
            PResponse = New Response(RData)
            If PResponse.ResponseCode <> ResponseCode.Ok Then
                Throw New ClientException("The server return an error code: " & PResponse.Msg)
            End If

        End Sub

    End Class
End Namespace