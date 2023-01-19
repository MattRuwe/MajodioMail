Namespace Configuration
    Public Class RemoteConfigClient
        Private Shared _remoteConfig As Majodio.Mail.Common.Configuration.Config
        Private Shared _remoteDomains As Majodio.Mail.Common.Configuration.Domains

        Public Shared ReadOnly Property RemoteConfig() As Majodio.Mail.Common.Configuration.Config

            Get
                If IsNothing(_remoteConfig) Then
                    Majodio.Common.Utilities.TraceMe("Creating new instance of RemotConfig object")
                    _remoteConfig = CType(Activator.GetObject(GetType(Majodio.Mail.Common.Configuration.Config), "http://" & CONFIG_REMOTE_HOST & ":" & CONFIG_TCP_PORT & "/" & CONFIG_PATH), Majodio.Mail.Common.Configuration.Config)
                End If
                Return _remoteConfig
            End Get
        End Property

        Public Shared ReadOnly Property RemoteDomain() As Majodio.Mail.Common.Configuration.Domains
            Get
                If IsNothing(_remoteDomains) Then
                    Try
                        Majodio.Common.Utilities.TraceMe("Creating new instance of RemoteDomain object")
                        _remoteDomains = CType(Activator.GetObject(GetType(Majodio.Mail.Common.Configuration.Domains), "http://" & DOMAIN_REMOTE_HOST & ":" & DOMAIN_TCP_PORT & "/" & DOMAIN_PATH), Majodio.Mail.Common.Configuration.Domains)
                        _remoteDomains.Ping()
                    Catch ex As System.Net.WebException
                        _remoteDomains = New Domains
                    End Try
                End If
                Return _remoteDomains
            End Get
        End Property
    End Class
End Namespace