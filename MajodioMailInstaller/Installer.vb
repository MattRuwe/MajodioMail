Imports System.ComponentModel
Imports Majodio.Common.Messaging
Imports System.Reflection

<RunInstaller(True)> _
Public Class Installer
    Inherits System.Configuration.Install.Installer

    Public Overrides Sub Install(ByVal SavedState As IDictionary)
        MyBase.Install(SavedState)
        Majodio.Mail.Common.Performance.InstallCounters()
        SendMessage.SendMessage("MajodioMailInstaller " & [Assembly].GetExecutingAssembly.GetName.Version.ToString(4), "Install")
        Dim Update As New UpdatePreviousInstalls()
        Update.ShowDialog()
    End Sub

    Public Overrides Sub Commit(ByVal SavedState As IDictionary)
        MyBase.Commit(SavedState)
        SendMessage.SendMessage("MajodioMailInstaller " & [Assembly].GetExecutingAssembly.GetName.Version.ToString(4), "Commit")
    End Sub

    Public Overrides Sub Rollback(ByVal SavedState As IDictionary)
        MyBase.Rollback(SavedState)
        SendMessage.SendMessage("MajodioMailInstaller " & [Assembly].GetExecutingAssembly.GetName.Version.ToString(4), "Rollback")
    End Sub

    Public Overrides Sub Uninstall(ByVal SavedState As IDictionary)
        SendMessage.SendMessage("MajodioMailInstaller " & [Assembly].GetExecutingAssembly.GetName.Version.ToString(4), "Uninstall")
        Majodio.Mail.Common.Performance.UnInstallCounters()
        Dim UO As New UninstallOptions
        UO.ApplicationPath = Majodio.Mail.Common.GetApplicationDirectory()
        UO.ShowDialog()
        MyBase.Uninstall(SavedState)
    End Sub
End Class
