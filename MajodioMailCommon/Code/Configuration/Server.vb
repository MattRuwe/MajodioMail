Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Remoting.Channels.Tcp

Namespace Configuration
    <Serializable()> _
    Public Class Server
        'Private _configChannel As IChannel
        'Private _domainChannel As IChannel
        Public Sub New()
            StartServer()
        End Sub

        Private Sub StartServer()
            'Start the config server
            Dim configServerProvider As New System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider
            Dim configClientProvider As New System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider

            configServerProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

            Dim Properties As IDictionary = New Hashtable

            Properties("name") = CONFIG_CHANNEL_NAME
            Properties("clientConnectionLimit") = CONFIG_CLIENT_CONNECTION_LIMIT
            Properties("port") = CONFIG_TCP_PORT

            Dim configChannel As New HttpChannel(Properties, configClientProvider, configServerProvider)

            'Register it
            ChannelServices.RegisterChannel(configChannel, False)

            RemotingConfiguration.RegisterWellKnownServiceType( _
                GetType(Majodio.Mail.Common.Configuration.Config), _
                CONFIG_PATH, WellKnownObjectMode.Singleton)


            'Start the domain server
            Dim domainServerProvider As New System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider
            Dim domainClientProvider As New System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider

            domainServerProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

            Properties = New Hashtable

            Properties("name") = DOMAIN_CHANNEL_NAME
            Properties("clientConnectionLimit") = DOMAIN_CLIENT_CONNECTION_LIMIT
            Properties("port") = DOMAIN_TCP_PORT

            Dim domainChannel As New HttpChannel(Properties, domainClientProvider, domainServerProvider)

            'Register it
            ChannelServices.RegisterChannel(domainChannel, False)

            RemotingConfiguration.RegisterWellKnownServiceType( _
                GetType(Majodio.Mail.Common.Configuration.Domains), _
                DOMAIN_PATH, WellKnownObjectMode.Singleton)

            RemotingConfiguration.CustomErrorsEnabled(True)
        End Sub

        Public Sub StopServer()
            Dim configChannel As IChannel = Nothing
            Dim domainChannel As IChannel = Nothing
            For i As Integer = 0 To ChannelServices.RegisteredChannels.GetUpperBound(0)
                If ChannelServices.RegisteredChannels(i).ChannelName = CONFIG_CHANNEL_NAME Then
                    configChannel = ChannelServices.RegisteredChannels(i)
                End If
                If ChannelServices.RegisteredChannels(i).ChannelName = DOMAIN_CHANNEL_NAME Then
                    domainChannel = ChannelServices.RegisteredChannels(i)
                End If
            Next
            If Not IsNothing(configChannel) Then
                ChannelServices.UnregisterChannel(configChannel)
            End If
            If Not IsNothing(domainChannel) Then
                ChannelServices.UnregisterChannel(domainChannel)
            End If
            'ChannelServices.UnregisterChannel(_configChannel)
            'ChannelServices.UnregisterChannel(_domainChannel)
        End Sub
    End Class
End Namespace