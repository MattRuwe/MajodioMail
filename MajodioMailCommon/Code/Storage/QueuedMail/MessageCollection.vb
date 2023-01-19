Imports System.IO

Imports Majodio.Mail.Common.DataTypes

Namespace Storage.QueuedMail
    <Serializable()> _
    Public Class MessageCollection
        Inherits System.Collections.CollectionBase

        Public Function Add(ByVal Message As QueuedMail.Message) As QueuedMail.Message
            MyBase.List.Add(Message)
            Return Message
        End Function

        Public Sub Add(ByVal Messages As QueuedMail.MessageCollection)
            If Not IsNothing(Messages) AndAlso Messages.Count > 0 Then
                For i As Integer = 0 To Messages.Count - 1
                    Add(Messages(i))
                Next
            End If
        End Sub

        Default Public Property Item(ByVal Index As Integer) As QueuedMail.Message
            Get
                Return CType(MyBase.List.Item(Index), QueuedMail.Message)
            End Get
            Set(ByVal Value As QueuedMail.Message)
                MyBase.List.Item(Index) = Value
            End Set
        End Property

        Public Function IndexOf(ByVal UID As UniqueId) As Integer
            Dim RVal As Integer = -1
            For i As Integer = 0 To Count - 1
                If Item(i).UniqueId.Value = UID.Value Then
                    RVal = i
                    Exit For
                End If
            Next
            Return RVal
        End Function

        Public Sub GetAllAdminMessages()
            'Dim config As New Configuration.Config
            Dim domainFolders As String()
            Dim adminFolders As String()
            Dim messageFiles As String()
            Dim message As Message


            domainFolders = Directory.GetDirectories(Majodio.Mail.Common.Functions.GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER)

            If Not IsNothing(domainFolders) Then
                For i As Integer = 0 To domainFolders.GetUpperBound(0)
                    'Check to see if there is an administrator folder here

                    adminFolders = Directory.GetDirectories(domainFolders(i))
                    If Not IsNothing(adminFolders) Then
                        For j As Integer = 0 To adminFolders.GetUpperBound(0)
                            If Majodio.Functions.GetDirectoryName(adminFolders(j)).ToLower.Trim = Majodio.Mail.Common.Configuration.RemoteConfigClient.RemoteConfig.ServerAdminUsername.ToLower.Trim Then
                                messageFiles = Directory.GetFiles(adminFolders(j), "*.xml")
                                For k As Integer = 0 To messageFiles.GetUpperBound(0)
                                    message = RemoteClient.Instance.CreateMessage(messageFiles(k))
                                    Add(message)
                                Next
                            End If
                        Next
                    End If
                Next
            End If
        End Sub
    End Class
End Namespace