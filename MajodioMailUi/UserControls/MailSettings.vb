Public Class MailSettings

    Public Config As New Settings

    Private _ConfigObject As Object

    Public Property ConfigObject() As Object
        Get
            Return _ConfigObject
        End Get
        Set(ByVal value As Object)
            _ConfigObject = value
        End Set
    End Property

    Public Overrides Sub Initialize()
        If IsNothing(_ConfigObject) Then
            pgMailSettings.SelectedObject = Config
        Else
            pgMailSettings.SelectedObject = _ConfigObject
        End If
    End Sub
End Class
