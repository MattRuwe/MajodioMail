Public Class Registry
    Inherits Majodio.Common.Registry

    Protected Overrides ReadOnly Property RegistryRootPath() As String
        Get
            Return "Software\Majodio\MailServer"
        End Get
    End Property
End Class
