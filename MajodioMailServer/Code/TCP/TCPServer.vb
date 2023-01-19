Imports System.Net.Sockets
Imports System.Net
Imports Majodio.Mail.Common.Configuration

Namespace Tcp
    Public Class TCPServer
        Public Shared Function GetSmtpSocket() As Socket
            Dim RVal As Socket
            Dim port As Integer = RemoteConfigClient.RemoteConfig.SmtpTcpPort
            'Dim Config As New Majodio.Mail.Common.Configuration.Config
            'RVal = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            RVal = New Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp)
            RVal.Bind(New IPEndPoint(System.Net.IPAddress.Any, port))
            RVal.Listen(10)
            Return RVal
        End Function

        Public Shared Function GetPop3Socket() As Socket
            Dim RVal As Socket
            Dim port As Integer = RemoteConfigClient.RemoteConfig.Pop3TcpPort
            'Dim Config As New Majodio.Mail.Common.Configuration.Config
            'RVal = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            RVal = New Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp)
            RVal.Bind(New IPEndPoint(System.Net.IPAddress.Any, port))
            RVal.Listen(10)
            Return RVal
        End Function

        Public Shared Function GetSecurePop3Socket() As Socket
            Dim RVal As Socket
            Dim port As Integer = RemoteConfigClient.RemoteConfig.Pop3SecureTcpPort
            'Dim Config As New Majodio.Mail.Common.Configuration.Config
            RVal = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            RVal.Bind(New IPEndPoint(System.Net.IPAddress.Any, port))
            RVal.Listen(10)
            Return RVal
        End Function

        Public Shared Function GetImap4Socket() As Socket
            Dim RVal As Socket
            Dim port As Integer = RemoteConfigClient.RemoteConfig.Imap4TcpPort
            'Dim Config As New Majodio.Mail.Common.Configuration.Config
            RVal = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            RVal.Bind(New IPEndPoint(System.Net.IPAddress.Any, port))
            RVal.Listen(10)
            Return RVal
        End Function
    End Class
End Namespace