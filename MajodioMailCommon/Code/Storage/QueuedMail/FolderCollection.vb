Namespace Storage.QueuedMail

    <Serializable()> _
    Public Class FolderCollection
        Inherits CollectionBase

        Public Sub Add(ByVal Folder As QueuedMail.Folder)
            If Not IsNothing(Folder) Then
                MyBase.List.Add(Folder)
            End If
        End Sub

        Public Sub Add(ByVal Folders As QueuedMail.FolderCollection)
            If Not IsNothing(Folders) Then
                For i As Integer = 0 To Folders.Count - 1
                    Add(Folders(i))
                Next
            End If
        End Sub

        Default Public Property Item(ByVal Index As Integer) As QueuedMail.Folder
            Get
                Return MyBase.List.Item(Index)
            End Get
            Set(ByVal Value As QueuedMail.Folder)
                MyBase.List.Item(Index) = Value
            End Set
        End Property

        Public Shared Function RetrieveAllFolders(ByVal DomainName As String, ByVal Username As String) As QueuedMail.FolderCollection
            Return RetrieveAllFolders(DomainName, Username, String.Empty)
        End Function

        Private Shared Function RetrieveAllFolders(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As QueuedMail.FolderCollection
            Dim RVal As New QueuedMail.FolderCollection
            Dim StartIndex As Integer = 0
            If FolderPath = String.Empty Then
                RVal.Add(RemoteClient.Instance.CreateFolder(DomainName, Username, String.Empty))
                StartIndex = 1
            End If
            RVal.Add(RemoteClient.Instance.CreateFolder.GetSubFolders(DomainName, Username, FolderPath))
            For i As Integer = StartIndex To RVal.Count - 1
                RVal.Add(RetrieveAllFolders(DomainName, Username, RVal(i).UserRelativeFolderPath))
            Next
            Return RVal
        End Function
    End Class
End Namespace