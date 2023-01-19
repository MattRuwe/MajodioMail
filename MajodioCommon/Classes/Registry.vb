Imports Microsoft.Win32

Namespace Majodio.Common
    Public MustInherit Class Registry
        Protected MustOverride ReadOnly Property RegistryRootPath() As String

        Public Function GetValue(ByVal Name As String, ByVal DefaultValue As Object) As Object
            Dim RK As RegistryKey = Nothing
            Dim RVal As Object = Nothing
            Try
                RK = GetKey()
                RVal = RK.GetValue(Name, DefaultValue)
                Return RVal
            Finally
                If Not IsNothing(RK) Then
                    RK.Close()
                    RK = Nothing
                End If
            End Try
        End Function

        Public Function GetValue(ByVal Name As String) As Object
            Dim RK As RegistryKey = Nothing
            Dim RVal As Object = Nothing
            Try
                RK = GetKey()
                RVal = RK.GetValue(Name, Nothing)
                Return RVal
            Finally
                If Not IsNothing(RK) Then
                    RK.Close()
                    RK = Nothing
                End If
            End Try
        End Function

        Public Sub SetValue(ByVal Name As String, ByVal Value As Object)
            Dim RK As RegistryKey = Nothing
            Try
                RK = GetKey()
                RK.SetValue(Name, Value)
            Finally
                If Not IsNothing(RK) Then
                    RK.Close()
                    RK = Nothing
                End If
            End Try
        End Sub

        Private Function GetKey() As RegistryKey
            Dim RVal As RegistryKey
            RVal = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegistryRootPath, True)
            If IsNothing(RVal) Then
                RVal = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(RegistryRootPath)
            End If
            Return RVal
        End Function
    End Class

End Namespace