Imports Microsoft.Win32

Public Class BetabriteRegistry

    Private Const REG_KEY_PATH = "Software\Majodio\MailServer\BetabritePlugin"

    Public Shared Function GetRegistryValue(ByVal ValueName As String) As Object
        Dim regKey As RegistryKey = Nothing
        Dim rVal As Object = Nothing
        Try
            regKey = Registry.LocalMachine.OpenSubKey(REG_KEY_PATH, True)
            If IsNothing(regKey) Then
                regKey = Registry.LocalMachine.CreateSubKey(REG_KEY_PATH)
            End If

            rVal = regKey.GetValue(ValueName)
            Return rVal
        Finally
            If Not IsNothing(regKey) Then
                regKey.Close()
            End If
        End Try
    End Function

    Public Shared Sub SetRegistryValue(ByVal ValueName As String, ByVal Value As Object)
        Dim regKey As RegistryKey = Nothing
        Try
            regKey = Registry.LocalMachine.OpenSubKey(REG_KEY_PATH, True)
            If IsNothing(regKey) Then
                regKey = Registry.LocalMachine.CreateSubKey(REG_KEY_PATH)
            End If
            regKey.SetValue(ValueName, Value)
        Finally
            If Not IsNothing(regKey) Then
                regKey.Close()
            End If
        End Try
    End Sub
End Class
