Imports System.ServiceProcess
Imports Majodio.Mail.Server.pop3

Public Class Pop3Relay
    Private _manager As RelayManager
    Protected Overrides Sub OnStart(ByVal args() As String)
        _manager = New RelayManager
        _manager.Start()
    End Sub

    Protected Overrides Sub OnStop()
        _manager.Stop()
    End Sub

End Class
