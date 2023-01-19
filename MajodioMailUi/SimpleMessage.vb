'Imports System.Net.sockets
'Imports System.Net

'Public Class SimpleMessage

'    Public Event StatusUpdate As EventHandler

'    Private _Iterations As Integer
'    Public Sub New(ByVal Iterations As Integer)
'        _Iterations = Iterations
'    End Sub

'    Public Sub StartProcessing()
'        Dim i As Integer
'        For i = 0 To _Iterations - 1
'            _PercentComplete = (i / _Iterations) * 100
'            SendSimpleMessage()
'            RaiseEvent StatusUpdate(Me, EventArgs.Empty)
'        Next
'    End Sub

'    Private _PercentComplete As Integer
'    Public ReadOnly Property PercentComplete() As Integer
'        Get
'            Return _PercentComplete
'        End Get
'    End Property

'    Private Shared Sub SendSimpleMessage()
'        Dim S As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
'        S.Connect(New IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 25))
'        Dim response As String
'        If S.Connected Then
'            Dim SS As New Majodio.Mail.Server.Tcp.SocketServer(S)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("ehlo me" & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("mail from matt@ruwe.net" & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("rcpt to matt@ruwe.net" & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("data" & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("SUBJECT: A test message" & vbCrLf)
'            SS.SendData("FROM: matt@ruwe.net" & vbCrLf)
'            SS.SendData(vbCrLf & "This is a test message" & vbCrLf)
'            SS.SendData("." & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            SS.SendData("quit" & vbCrLf)
'            Do
'                response = SS.ReceiveData(vbCrLf)
'            Loop While response.Substring(3, 1) = "-"
'            S.Close()
'        End If
'    End Sub
'End Class
