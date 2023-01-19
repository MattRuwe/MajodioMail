Namespace Storage.QueuedMail

    <Serializable()> _
    Public Class FolderNotFoundException
        Inherits Exception

        Public Sub New(ByVal Domain As String, ByVal Username As String, ByVal FolderPath As String)
            MyBase.New("The folder " & FolderPath & " does not exist for user " & Username & "@" & Domain)
        End Sub
    End Class
End Namespace