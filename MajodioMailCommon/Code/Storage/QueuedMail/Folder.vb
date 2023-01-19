Imports System.IO
Imports System.Xml
Imports Majodio.Mail.Common.Storage
Imports Majodio.Mail.Common.Configuration
Imports Majodio.Mail.Common.DataTypes
Imports Majodio.Mail.Common.Storage.QueuedMail

Namespace Storage.QueuedMail
    <Serializable()> _
    Public Class Folder
        Inherits PersistentStorage

        Private _FolderName As String
        Private _DomainName As String
        Private _Username As String
        Private _FolderPath As String
        Private _NewlyCreated As Boolean

        Public Sub New()

        End Sub

        Public Sub New(ByVal domainName As String, ByVal username As String)
            Initialize(domainName, username)
        End Sub

        Public Sub New(ByVal domainName As String, ByVal username As String, ByVal folderPath As String)
            Initialize(domainName, username, folderPath)
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String)
            MyBase.Initialize(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & domainName.ToLower & "\" & username.ToLower & "\" & MAIL_FOLDER_CONFIG_FILENAME)
            _DomainName = domainName
            _Username = username
            _FolderPath = String.Empty
            _FolderName = "INBOX"
            If UniqueId.Value = -1 Then
                'Dim Domain As New Domains
                UniqueId = RemoteConfigClient.RemoteDomain.GetNextUid(_DomainName, _Username)
                _NewlyCreated = True
            Else
                _NewlyCreated = False
            End If
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String, ByVal folderPath As String)
            MyBase.Initialize(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & domainName.ToLower & "\" & username.ToLower & "\" & PersistentStorage.FixPath(ConvertInboxToEmptyString(ChangeImapToFileSystem(folderPath))) & MAIL_FOLDER_CONFIG_FILENAME)
            _DomainName = domainName
            _Username = username
            _FolderPath = PersistentStorage.FixPath(folderPath)
            If UniqueId.Value = -1 Then
                UniqueId = RemoteConfigClient.RemoteDomain.GetNextUid(_DomainName, _Username)
                _NewlyCreated = True
            Else
                _NewlyCreated = False
            End If
        End Sub

        Private Function ChangeImapToFileSystem(ByVal Path As String) As String
            Dim RVal As String = Path
            If RVal.Length > 0 Then
                RVal = RVal.Replace(IMAP4_HEIRARCHY_DELIMETER, "\")
            End If
            Return RVal
        End Function

        Private Function ChangeFileSystemToImap(ByVal Path As String) As String
            Dim RVal As String = Path
            If RVal.Length > 0 Then
                RVal = RVal.Replace("\", IMAP4_HEIRARCHY_DELIMETER)
            End If
            Return RVal
        End Function

        Private Function ConvertInboxToEmptyString(ByVal Path As String) As String
            Dim rval As String
            If Path.ToLower = "inbox" Or Path.ToLower = "inbox\" Then
                rval = ""
            Else
                rval = Path
            End If
            Return rval
        End Function

        Public Overrides Function GetDefaultXml() As XmlDocument
            Dim RVal As New XmlDocument

            Dim Attribute As XmlAttribute
            'Setup the root node
            Dim QueuedMailFolderNode As XmlElement = RVal.CreateNode(XmlNodeType.Element, "folder", "")

            'Add the root node to the document
            RVal.AppendChild(QueuedMailFolderNode)

            'Create the unique_id Node
            Dim QueuedMailFolderUid As XmlElement = RVal.CreateNode(XmlNodeType.Element, "unique_id", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = -1
            QueuedMailFolderUid.Attributes.Append(Attribute)
            QueuedMailFolderNode.AppendChild(QueuedMailFolderUid)

            'Create the no_select Node
            Dim QueuedMailNoSelect As XmlElement = RVal.CreateNode(XmlNodeType.Element, "no_select", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = "False"
            QueuedMailNoSelect.Attributes.Append(Attribute)
            QueuedMailFolderNode.AppendChild(QueuedMailNoSelect)

            Return RVal
        End Function

        Public ReadOnly Property AbsoluteFolderPath() As String
            Get
                Return GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & _DomainName.ToLower & "\" & _Username.ToLower & "\" & _FolderPath
            End Get
        End Property

        Public ReadOnly Property UserRelativeFolderPath() As String
            Get
                Return _FolderPath
            End Get
        End Property

        Public ReadOnly Property FolderName() As String
            Get
                Dim RVal As String = UserRelativeFolderPath
                If RVal = String.Empty Then
                    RVal = "INBOX" & IMAP4_HEIRARCHY_DELIMETER
                End If
                RVal = ChangeFileSystemToImap(RVal)
                Return RVal
            End Get
        End Property

        Public ReadOnly Property NewlyCreated() As Boolean
            Get
                Return _NewlyCreated
            End Get
        End Property

        Public Function CopyMessage(ByVal SourceMessage As QueuedMail.Message) As Boolean
            Return CopyMessage(_DomainName, _Username, _FolderPath, SourceMessage)
        End Function

        Public Function CopyMessage(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String, ByVal SourceMessage As QueuedMail.Message) As Boolean
            Dim RVal As Boolean = True
            Dim NewMessage As QueuedMail.Message = New Message()
            Dim MID As MessageId
            Dim Folder As QueuedMail.Folder = New Folder(DomainName, Username, FolderPath)

            Folder.UniqueId = New UniqueId(Folder.UniqueId.Value + 1)

            MID = NewMessage.GetNewMessageId

            File.Copy(SourceMessage.XmlFilePath, Folder.AbsoluteFolderPath & MID.Value & ".xml")
            File.Copy(SourceMessage.XmlFilePath & ".msg", Folder.AbsoluteFolderPath & MID.Value & ".xml.msg")
            If NewMessage.MessageExists(DomainName, Username, FolderPath, MID) Then
                NewMessage.Initialize(DomainName, Username, MID, FolderPath)
                NewMessage.UniqueId = Folder.UniqueId
            Else
                RVal = False
            End If
            Return RVal
        End Function

        Public Function GetSubFolders(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As QueuedMail.FolderCollection
            Dim RVal As New QueuedMail.FolderCollection
            Dim FullPath As String
            Dim Folders As String()
            Dim FullPathLength As Integer
            FullPath = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName.ToLower & "\" & Username.ToLower & "\" & FolderPath
            FullPathLength = FullPath.Length - FolderPath.Length
            Folders = Directory.GetDirectories(FullPath)
            For i As Integer = 0 To Folders.GetUpperBound(0)
                Folders(i) = Folders(i).Substring(FullPathLength)
                If IsFolderPathValid(Folders(i)) Then
                    RVal.Add(New Folder(DomainName, Username, Folders(i)))
                End If
            Next
            Return RVal
        End Function

        Public Function GetNumberOfMessages() As Integer
            Return GetNumberOfMessages(_DomainName, _Username, _FolderPath)
        End Function

        Public Function GetNumberOfMessages(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As Integer
            Dim RVal As Integer = 0
            Dim Files As String()
            Dim FullPath As String
            Try
                FolderPath = PersistentStorage.FixPath(FolderPath)

                FullPath = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName.ToLower & "\" & Username.ToLower & "\" & FolderPath
                Files = Directory.GetFiles(FullPath, "*.xml")
                For i As Integer = 0 To Files.GetUpperBound(0)
                    If Majodio.Common.IoUtilities.GetFilename(Files(i), False).ToLower <> MAIL_FOLDER_CONFIG_FILENAME.ToLower Then
                        RVal += 1
                    End If
                Next
            Catch exc As Exception
                'Log.Logger.WriteLog("An error occurred while attempting to get the number of messages in the mailbox for " & Username & "@" & DomainName & ".  The details follow:" & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
            End Try
            Return RVal
        End Function

        Public Function GetNumberOfMessages(ByVal DomainName As String, ByVal Username As String) As Integer
            Return GetNumberOfMessages(DomainName, Username, String.Empty)
        End Function

        Public Function GetFirstUnseenMessageSequence() As Integer
            Return GetFirstUnseenMessageSequence(_DomainName, _Username, _FolderPath)
        End Function

        Public Function GetFirstUnseenMessageSequence(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As Integer
            Dim Messages As QueuedMail.MessageCollection = Nothing
            Dim RVal As Integer = 0
            Try
                Messages = GetAllMailMessages(DomainName, Username, FolderPath)
                For i As Integer = 0 To Messages.Count - 1
                    If Not Messages(i).Seen Then
                        RVal = i + 1
                        Exit For
                    End If
                Next
                Return RVal
            Finally
                If Not IsNothing(Messages) Then
                    Messages.Clear()
                End If
            End Try
        End Function

        Public Function GetRecentMessages() As QueuedMail.MessageCollection
            Return GetRecentMessages(_DomainName, _Username, _FolderPath)
        End Function

        Public Function GetRecentMessages(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As QueuedMail.MessageCollection
            Dim RVal As New QueuedMail.MessageCollection
            Dim Messages As QueuedMail.MessageCollection = Nothing
            Try
                Messages = GetAllMailMessages(DomainName, Username, FolderPath)
                For i As Integer = 0 To Messages.Count - 1
                    If Messages(i).Recent Then
                        RVal.Add(Messages(i))
                    End If
                Next
                Return RVal
            Finally
                If Not IsNothing(Messages) Then
                    Messages.Clear()
                End If
            End Try
        End Function

        Public Function GetAllMailMessages() As QueuedMail.MessageCollection
            Return GetAllMailMessages(_DomainName, _Username, _FolderPath)
        End Function

        Public Function GetAllMailMessages(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As QueuedMail.MessageCollection
            Dim Files As String()
            Dim RVal As New QueuedMail.MessageCollection
            Dim i As Integer
            Dim PeriodIndex As Integer
            Dim SlashIndex As Integer
            Dim FullPath As String
            Dim Filename As String
            Try
                FullPath = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName.ToLower & "\" & Username.ToLower & "\" & PersistentStorage.FixPath(ConvertInboxToEmptyString(FolderPath))
                If Not Directory.Exists(FullPath) Then
                    Directory.CreateDirectory(FullPath)
                End If
                Files = Directory.GetFiles(FullPath)
                'ReDim RVal(Files.GetUpperBound(0))
                For i = 0 To Files.GetUpperBound(0)
                    If Path.GetExtension(Files(i)).ToLower = ".xml" Then
                        PeriodIndex = Files(i).LastIndexOf(".xml")
                        SlashIndex = Files(i).LastIndexOf("\")
                        Filename = Files(i).Substring(SlashIndex + 1, PeriodIndex - SlashIndex - 1)
                        If Filename.ToLower & ".xml" <> MAIL_FOLDER_CONFIG_FILENAME.ToLower AndAlso PeriodIndex > -1 AndAlso PeriodIndex < Files(i).Length Then
                            RVal.Add(New Message(DomainName, Username, New MessageId(Filename), FolderPath))
                        End If
                    End If
                Next
            Catch exc As Exception
                'Log.Logger.WriteLog("An error occurred while attempting to retrieve all mail from the mailbox for " & Username & "@" & DomainName & ".  The details follow:" & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
            End Try
            Return RVal
        End Function

        Public Function GetAllMailMessages(ByVal DomainName As String, ByVal Username As String) As QueuedMail.MessageCollection
            Return GetAllMailMessages(DomainName, Username, String.Empty)
        End Function

        Public Function GetMailMessages(ByVal Range As String, ByVal UseUid As Boolean) As QueuedMail.MessageCollection
            Return GetMailMessages(_DomainName, _Username, _FolderPath, Range, UseUid)
        End Function

        Public Function GetMailMessages(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String, ByVal Range As String, ByVal UseUid As Boolean) As QueuedMail.MessageCollection
            Dim TmpRVal As QueuedMail.MessageCollection
            Dim RVal As New QueuedMail.MessageCollection
            Dim Start As Integer = -1
            Dim [End] As Integer = -1
            Dim ColonIndex As Integer
            Dim FoundStartUid As Boolean = False
            Dim FoundEndUid As Boolean = False
            Dim ArrRange As String()
            Dim CurrentRange As String

            ArrRange = Split(Range, ",")
            For i As Integer = 0 To ArrRange.GetUpperBound(0)
                CurrentRange = ArrRange(i).Trim
                TmpRVal = GetAllMailMessages(DomainName, Username, FolderPath)

                ColonIndex = CurrentRange.IndexOf(":")
                If ColonIndex > -1 Then
                    If IsNumeric(CurrentRange.Substring(0, ColonIndex)) Then
                        Start = CurrentRange.Substring(0, ColonIndex)
                    Else
                        Start = 0
                    End If
                    If IsNumeric(CurrentRange.Substring(ColonIndex + 1)) Then
                        [End] = CurrentRange.Substring(ColonIndex + 1)
                    Else
                        [End] = Integer.MaxValue
                    End If
                ElseIf IsNumeric(CurrentRange) Then
                    Start = CurrentRange
                    [End] = CurrentRange
                Else
                    Throw New ArgumentException("The value for range must be in one of two forms: D:D or D")
                End If

                If UseUid Then
                    For j As Integer = 0 To TmpRVal.Count - 1
                        If Start = TmpRVal(j).UniqueId.Value Then
                            Start = j + 1
                            FoundStartUid = True
                        End If
                        If [End] = TmpRVal(j).UniqueId.Value Then
                            [End] = j + 1
                            FoundEndUid = True
                        End If
                        If FoundStartUid And FoundEndUid Then
                            Exit For
                        End If
                    Next
                    If Not FoundStartUid Then
                        Start = 1
                    End If
                    If Not FoundEndUid Then
                        [End] = Integer.MaxValue
                    End If
                Else
                    Start -= 1
                    [End] -= 1
                End If

                If Start > 0 AndAlso Start - 1 < TmpRVal.Count Then
                    For j As Integer = 1 To Start - 1
                        TmpRVal.RemoveAt(0)
                        [End] -= 1
                    Next
                    Start = 0
                Else
                    TmpRVal.Clear()
                End If
                While TmpRVal.Count - 1 > [End]
                    TmpRVal.RemoveAt([End] + 1)
                End While
                RVal.Add(TmpRVal)
            Next

            Return RVal
        End Function

        Public Function GetMailMessage(ByVal UniqueId As UniqueId) As QueuedMail.Message
            Return GetMailMessage(_DomainName, _Username, _FolderPath, UniqueId)
        End Function

        Public Function GetMailMessage(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String, ByVal UniqueId As UniqueId) As QueuedMail.Message
            Dim RVal As QueuedMail.Message = Nothing
            Dim Messages As QueuedMail.MessageCollection

            Messages = GetAllMailMessages(DomainName, Username, FolderPath)

            For i As Integer = 0 To Messages.Count - 1
                If Messages(i).UniqueId.Value = UniqueId.Value Then
                    RVal = Messages(i)
                    Exit For
                End If
            Next

            Return RVal
        End Function

        Public Function GetTotalMessagesSize() As Integer
            Return GetTotalMessagesSize(_DomainName, _Username, _FolderPath)
        End Function

        Public Function GetTotalMessagesSize(ByVal DomainName As String, ByVal Username As String) As Integer
            GetTotalMessagesSize(DomainName, Username, String.Empty)
        End Function

        Public Function GetTotalMessagesSize(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As Integer
            Dim RVal As Integer = 0
            Dim Messages As QueuedMail.MessageCollection
            Dim i As Integer
            Messages = GetAllMailMessages(DomainName, Username, FolderPath)
            For i = 0 To Messages.Count - 1
                RVal += Messages(i).MessageSize
            Next
            Return RVal
        End Function

        Public Function IsFolderPathValid(ByVal FolderPath As String) As Boolean
            Dim RVal As Boolean = True
            Dim PathArray As String() = Split(PersistentStorage.FixPath(FolderPath), "\")
            For i As Integer = 0 To PathArray.GetUpperBound(0)
                If PathArray(i).Length > 0 Then
                    If PathArray(i).Substring(0, 1) = "_" Then
                        RVal = False
                        Exit For
                    End If
                End If
            Next
            Return RVal
        End Function

        Public Function FolderExists(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As Boolean
            Dim RVal As Boolean = True
            If Not FolderPath.ToLower = "inbox" And Not FolderPath.ToLower = "inbox\" Then
                Dim PathArray As String() = Split(PersistentStorage.FixPath(FolderPath), "\")
                Dim CurrentPath As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName & "\" & Username & "\"
                For i As Integer = 0 To PathArray.GetUpperBound(0)
                    If PathArray(i).Length > 0 Then
                        CurrentPath &= PathArray(i) & "\"
                        If Not Directory.Exists(CurrentPath) Then
                            RVal = False
                            Exit For
                        End If
                    End If
                Next
            End If
            Return RVal
        End Function

        Public Sub DeleteFolder()
            Dim HasChildren As Boolean
            Dim CurrentFolder As QueuedMail.Folder
            HasChildren = FolderContainsChildren()
            If HasChildren And Not NoSelect Then
                'Per RFC 2060 Page 26 if the folder has inferior heirarchial names and the folder
                'does not have the /NoSelect flag, the folder should be set /NoSelect
                NoSelect = True
            ElseIf Not HasChildren Then
                'Per RFC 2060 Page 26 if the folder does not have children then it should be deleted
                Directory.Delete(AbsoluteFolderPath, True)
                CurrentFolder = ParentFolder
                If Not IsNothing(CurrentFolder) AndAlso CurrentFolder.NoSelect Then
                    CurrentFolder.DeleteFolder()
                End If
                'PersistentStorage.CleanupCache()
            End If
        End Sub

        Public Function FolderContainsChildren(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String) As Boolean
            Dim RVal As Boolean = False
            Dim Path As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName & "\" & Username & "\" & PersistentStorage.FixPath(FolderPath)
            Dim Directories As String() = Directory.GetDirectories(Path)
            Dim SlashIndex As Integer
            For i As Integer = 0 To Directories.GetUpperBound(0)
                SlashIndex = Directories(i).LastIndexOf("\")
                If Directories(i).Length > 0 AndAlso IsFolderPathValid(Directories(i).Substring(SlashIndex + 1)) Then
                    RVal = True
                    Exit For
                End If
            Next
            Return RVal
        End Function

        Public Function GetFolderFromPath(ByVal Path As String) As String
            Dim RVal As String = Path
            Dim RootPath As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER
            If RVal.Substring(0, RootPath.Length).ToLower = RootPath.ToLower Then
                RVal = RVal.Substring(RootPath.Length)
            End If
            Return RVal
        End Function

        Public Function FolderContainsChildren() As Boolean
            Dim RVal As Boolean = False
            Dim Path As String = AbsoluteFolderPath
            Dim Directories As String() = Directory.GetDirectories(Path)
            Dim SlashIndex As Integer
            For i As Integer = 0 To Directories.GetUpperBound(0)
                SlashIndex = Directories(i).LastIndexOf("\")
                If IsFolderPathValid(Directories(i).Substring(SlashIndex + 1)) Then
                    RVal = True
                    Exit For
                End If
            Next
            Return RVal
        End Function

        Public ReadOnly Property ChildFolders() As QueuedMail.FolderCollection
            Get
                Dim RVal As New QueuedMail.FolderCollection
                Dim Directories As String()
                Directories = Directory.GetDirectories(AbsoluteFolderPath)
                For i As Integer = 0 To Directories.GetUpperBound(0)
                    RVal.Add(New Folder(_DomainName, _Username, UserRelativeFolderPath & Directories(i)))
                Next
                Return RVal
            End Get
        End Property

        Public Function HasParent() As Boolean
            Dim Path As String
            Dim Index As Integer
            Dim RVal As Boolean

            RVal = False
            Path = AbsoluteFolderPath
            While Path.EndsWith("\")
                Path = Path.Substring(0, Path.Length - 1)
            End While
            Index = Path.LastIndexOf("\")
            If Index > -1 Then
                Path = Path.Substring(0, Index)
                RVal = File.Exists(Path & "\" & MAIL_FOLDER_CONFIG_FILENAME)
            End If
            Return RVal
        End Function

        Public ReadOnly Property ParentFolder() As QueuedMail.Folder
            Get
                Dim RVal As QueuedMail.Folder = Nothing
                Dim Path As String
                Dim Index As Integer
                If HasParent() Then
                    Path = UserRelativeFolderPath
                    While Path.EndsWith("\")
                        Path = Path.Substring(0, Path.Length - 1)
                    End While
                    Index = Path.LastIndexOf("\")
                    If Index > -1 Then
                        Path = Path.Substring(0, Index)
                        RVal = New Folder(_DomainName, _Username, Path)
                    End If
                End If
                Return RVal
            End Get
        End Property

        Public Function Rename(ByVal NewPath As String) As QueuedMail.Folder
            Dim RVal As QueuedMail.Folder = Nothing
            Dim NewAbsPath As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & _DomainName & "\" & _Username & "\" & NewPath
            Dim OldAbsPath As String = GetApplicationDirectory()
            Dim NewFolder As QueuedMail.Folder
            Dim Files As String()
            Try
                If Not FolderExists(_DomainName, _Username, NewPath) Then
                    NewFolder = New Folder(_DomainName, _Username, NewPath)
                    NewFolder.UniqueId = UniqueId
                    'TODO Move all of the files in the old folder to the new folder
                    Files = Directory.GetFiles(Me.AbsoluteFolderPath)
                    For i As Integer = 0 To Files.GetUpperBound(0)
                        If Majodio.Common.IoUtilities.GetFilename(Files(i), False).ToLower <> Majodio.Mail.Common.Constants.MAIL_FOLDER_CONFIG_FILENAME.ToLower Then
                            File.Move(Files(i), NewFolder.AbsoluteFolderPath & "\" & Majodio.Common.IoUtilities.GetFilename(Files(i), False))
                        End If
                    Next
                    DeleteFolder()
                    RVal = NewFolder
                Else
                    RVal = Nothing
                End If
            Catch ex As Exception
                RVal = Nothing
            End Try
            Return RVal
        End Function

        Public Property NoSelect() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/folder/no_select")
                If IsNothing(TmpVal) Then
                    RVal = False
                ElseIf CType(TmpVal, String).Trim.Length = 0 Then
                    RVal = False
                Else
                    RVal = CType(TmpVal, Boolean)
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/folder/no_select", Value)
            End Set
        End Property

        Public Property Marked() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/folder/marked")
                If IsNothing(TmpVal) Then
                    RVal = False
                ElseIf CType(TmpVal, String).Trim.Length = 0 Then
                    RVal = False
                Else
                    RVal = CType(TmpVal, Boolean)
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/folder/marked", Value)
            End Set
        End Property

        Public Property UniqueId() As UniqueId
            Get
                Dim TmpVal As Object
                Dim RVal As UniqueId
                TmpVal = MyBase.GetXmlValue("/folder/unique_id")
                If Not IsNumeric(TmpVal) Then
                    RVal = New UniqueId(-1)
                Else
                    RVal = New UniqueId(CType(TmpVal, Int32))
                End If
                Return RVal
            End Get
            Set(ByVal Value As UniqueId)
                MyBase.SetXmlValue("/folder/unique_id", Value.Value)
            End Set
        End Property

        Public ReadOnly Property Recent() As Integer
            Get
                Dim RVal As Integer = 0
                Dim Messages As QueuedMail.MessageCollection
                Messages = GetAllMailMessages()
                For i As Integer = 0 To Messages.Count - 1
                    If Messages(i).Recent Then
                        RVal += 1
                    End If
                Next
                Return RVal
            End Get
        End Property

        Public ReadOnly Property Unseen() As Integer
            Get
                Dim RVal As Integer = 0
                Dim Messages As QueuedMail.MessageCollection
                Messages = GetAllMailMessages()
                For i As Integer = 0 To Messages.Count - 1
                    If Messages(i).Seen = False Then
                        RVal += 1
                    End If
                Next
                Return RVal
            End Get
        End Property
    End Class

End Namespace