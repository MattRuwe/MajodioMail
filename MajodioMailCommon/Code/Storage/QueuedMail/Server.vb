Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Remoting.Channels.Tcp

Namespace Storage.QueuedMail
    <Serializable()> _
    Public Class Server
        Private _folderChannel As IChannel
        Private _folderTcpChannel As TcpServerChannel
        Private _messageChannel As IChannel
        Private _messageTcpChannel As TcpServerChannel

        Public Sub New()
            StartServer()
        End Sub

        Private Sub StartServer()
            'Dim configServerBinaryProvider As New System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider
            Dim configServerSoapProvider As New System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider

            'configServerBinaryProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full
            configServerSoapProvider.TypeFilterLevel = Runtime.Serialization.Formatters.TypeFilterLevel.Full

            'Dim tcpServerChannel As New TcpServerChannel("BinaryMessageFolder", FOLDER_MESSAGE_TCP_PORT, configServerBinaryProvider)
            Dim httpServerChannel As New HttpServerChannel("SoapMessageFolder", FOLDER_MESSAGE_HTTP_PORT, configServerSoapProvider)

            'ChannelServices.RegisterChannel(tcpServerChannel, True)
            ChannelServices.RegisterChannel(httpServerChannel, False)

            Dim remoteMessage As New ActivatedServiceTypeEntry(GetType(Message))
            Dim remoteFolder As New ActivatedServiceTypeEntry(GetType(Folder))

            RemotingConfiguration.ApplicationName = FOLDER_MESSAGE_REMOTING_APP_NAME
            RemotingConfiguration.RegisterActivatedServiceType(remoteMessage)
            RemotingConfiguration.RegisterActivatedServiceType(remoteFolder)

            RemotingConfiguration.CustomErrorsEnabled(True)
        End Sub

        Public Sub StopServer()
            ChannelServices.UnregisterChannel(_folderChannel)
            ChannelServices.UnregisterChannel(_messageChannel)
            'ChannelServices.UnregisterChannel(_tcpChannel)

        End Sub
    End Class
End Namespace