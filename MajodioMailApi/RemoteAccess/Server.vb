Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Remoting.Channels.Tcp

Namespace RemoteAccess
    Public Class Server
        Private Chan As IChannel
        Private Const REMOTE_ACCESS_CHANNEL_NAME As String = "MajodioMailRemoteAccess"
        Private Const REMOTE_ACCESS_CLIENT_CONNECTION_LIMIT As Integer = 10
        Private Const REMOTE_ACCESS_TCP_PORT As Integer = 28710
        Private Const REMOTE_ACCESS_PATH As String = "RemoteAccess"

        Public Sub StartServer()
            Dim ServerProvider As New System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider
            Dim ClientProvider As New System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider

            ServerProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

            Dim Props As IDictionary = New Hashtable

            Props("name") = REMOTE_ACCESS_CHANNEL_NAME
            Props("clientConnectionLimit") = REMOTE_ACCESS_CLIENT_CONNECTION_LIMIT
            Props("port") = REMOTE_ACCESS_TCP_PORT

            Chan = New HttpChannel(Props, ClientProvider, ServerProvider)
            'Chan = New TcpChannel(Props, ClientProvider, ServerProvider)

            'Register it
            ChannelServices.RegisterChannel(Chan, False)

            RemotingConfiguration.RegisterWellKnownServiceType( _
                GetType(Majodio.Mail.Server.Log.Manager), _
                REMOTE_ACCESS_PATH, WellKnownObjectMode.Singleton)
            RemotingConfiguration.CustomErrorsEnabled(True)

            'LogManager1 = New LogManager
        End Sub

        Public Sub StopServer()
            ChannelServices.UnregisterChannel(Chan)
        End Sub
    End Class
End Namespace