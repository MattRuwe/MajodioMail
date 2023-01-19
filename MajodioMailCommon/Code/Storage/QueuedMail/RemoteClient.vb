Imports Majodio.Mail.Common.DataTypes
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Activation
Imports System.Runtime.Remoting.Channels.Tcp
Imports System.Runtime.Remoting.Channels

Namespace Storage.QueuedMail
    <Serializable()> _
    Public Class RemoteClient
        Private Shared _instance As New RemoteClient

        Private Sub New()

        End Sub

        Public Shared ReadOnly Property Instance() As RemoteClient
            Get
                Return _instance
            End Get
        End Property

        Public Function CreateFolder(ByVal domainName As String, ByVal username As String) As Folder
            Dim rVal As Folder = CreateFolder()
            rVal.Initialize(domainName, username)
            Return rVal
        End Function

        Public Function CreateFolder(ByVal domainName As String, ByVal username As String, ByVal folderPath As String) As Folder
            Dim rVal As Folder = CreateFolder()
            rVal.Initialize(domainName, username, folderPath)
            Return rVal
        End Function

        Public Function CreateFolder() As Folder
            Dim rVal As Folder = Nothing
            Dim attr As UrlAttribute() = {New UrlAttribute("http://" & FOLDER_MESSAGE_REMOTING_HOST & ":" & FOLDER_MESSAGE_HTTP_PORT & "/" & FOLDER_MESSAGE_REMOTING_APP_NAME)}
            Dim objectHandle As ObjectHandle = Nothing

            objectHandle = Activator.CreateInstance("MajodioMailCommon", "Majodio.Mail.Common.Storage.QueuedMail.Folder", attr)

            rVal = CType(objectHandle.Unwrap, Majodio.Mail.Common.Storage.QueuedMail.Folder)

            Return rVal
        End Function

        Public Function CreateMessage(ByVal messagePath As String) As Message
            Dim rVal As Message
            rVal = CreateMessage()
            rVal.Initialize(messagePath)
            Return rVal
        End Function

        Public Function CreateMessage(ByVal domainName As String, ByVal username As String) As Message
            Dim rVal As Message
            rVal = CreateMessage()
            rVal.Initialize(domainName, username)
            Return rVal
        End Function

        Public Function CreateMessage(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId) As Message
            Dim rVal As Message
            rVal = CreateMessage()
            rVal.Initialize(domainName, username, messageId)
            Return rVal
        End Function

        Public Function CreateMessage(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId, ByVal folderPath As String) As Message
            Dim rVal As Message
            rVal = CreateMessage()
            rVal.Initialize(domainName, username, messageId, folderPath)
            Return rVal
        End Function

        Public Function CreateMessage(ByVal domainName As String, ByVal username As String, ByVal folderPath As String) As Message
            Dim rVal As Message
            rVal = CreateMessage()
            rVal.Initialize(domainName, username, folderPath)
            Return rVal
        End Function

        Public Function CreateMessage() As Message
            Dim rVal As Message
            Dim attr As UrlAttribute() = {New UrlAttribute("http://" & FOLDER_MESSAGE_REMOTING_HOST & ":" & FOLDER_MESSAGE_HTTP_PORT & "/" & FOLDER_MESSAGE_REMOTING_APP_NAME)}
            Dim objectHandle As ObjectHandle = Nothing

            objectHandle = Activator.CreateInstance("MajodioMailCommon", "Majodio.Mail.Common.Storage.QueuedMail.Message", attr)

            rVal = CType(objectHandle.Unwrap, Majodio.Mail.Common.Storage.QueuedMail.Message)

            Return rVal
        End Function
    End Class
End Namespace