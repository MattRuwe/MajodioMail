Imports System.IO
Imports System.Xml
Imports Majodio.Mail.Common.Storage
Imports Majodio.Mail.common.Configuration
Imports Majodio.Mail.Common.datatypes
Imports Majodio.Common

Namespace Storage.QueuedMail
    <Serializable()> _
    Public Enum MessageType
        QueuedMessage
        LocalUserMessage
    End Enum

    <Serializable()> _
    Public Class Message
        Inherits PersistentStorage

        Public Sub New()

        End Sub

        Public Sub New(ByVal messagePath As String)
            Initialize(messagePath)
        End Sub

        'Creates a new mail message
        Public Sub New(ByVal domainName As String, ByVal username As String)
            Initialize(domainName, username)
        End Sub

        'Retrieves an existing mail message from the queue
        Public Sub New(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId)
            Initialize(domainName, username, messageId)
        End Sub

        Public Sub New(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId, ByVal folderPath As String)
            Initialize(domainName, username, messageId, folderPath)
        End Sub

        Public Sub New(ByVal domainName As String, ByVal username As String, ByVal folderPath As String)
            Initialize(domainName, username, folderPath)
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String)
            MyBase.Initialize(GetMailboxFolder(domainName, username) & "\" & GetSerializedDateTime() & " " & Guid.NewGuid.ToString & ".xml")
            'Assign the new message a new unique id
            If RemoteConfigClient.RemoteDomain.UserExists(domainName, username) Then
                Me.UniqueId = RemoteConfigClient.RemoteDomain.GetNextUid(domainName, username)
            End If
        End Sub

        Public Overloads Sub Initialize(ByVal messagePath As String)
            MyBase.Initialize(messagePath)
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId)
            MyBase.Initialize(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & domainName & "\" & username & "\" & messageId.Value & ".xml")
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String, ByVal messageId As MessageId, ByVal folderPath As String)
            MyBase.Initialize(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & domainName & "\" & username & "\" & FixPath(folderPath) & messageId.Value & ".xml")
        End Sub

        Public Overloads Sub Initialize(ByVal domainName As String, ByVal username As String, ByVal folderPath As String)
            MyBase.Initialize(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & domainName & "\" & username & "\" & FixPath(folderPath) & GetNewMessageId.Value & ".xml")
        End Sub

        Public Function GetNewMessageId() As MessageId
            Dim RVal As MessageId
            RVal = New MessageId(GetSerializedDateTime() & " " & Guid.NewGuid.ToString)
            Return RVal
        End Function

        Public Function MessageExists(ByVal DomainName As String, ByVal Username As String, ByVal FolderPath As String, ByVal MID As MessageId) As Boolean
            Dim RVal As Boolean
            RVal = File.Exists(GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName & "\" & Username & "\" & FixPath(FolderPath) & MID.Value & ".xml")
            Return RVal
        End Function

        Private Shared Function GetMailboxFolder(ByVal DomainName As String, ByVal Username As String) As String
            Dim RVal As String = String.Empty
            If Not RemoteConfigClient.RemoteDomain.DomainExists(DomainName) Then
                RVal = GetApplicationDirectory() & "\" & QUEUED_MAIL_FOLDER
            Else
                RVal = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName.ToLower & "\" & Username.ToLower
            End If
            Return RVal
        End Function

        Public ReadOnly Property MessageId() As String
            Get
                Return ConvertFilenameToMessageId(XmlFilePath)
            End Get
        End Property

        Private Function ConvertFilenameToMessageId(ByVal Filename As String) As String
            Dim RVal As String = String.Empty
            Dim PeriodIndex As Integer
            Dim SlashIndex As Integer
            PeriodIndex = Filename.LastIndexOf(".xml")
            SlashIndex = Filename.LastIndexOf("\")
            If PeriodIndex > -1 And PeriodIndex < XmlFilePath.Length Then
                RVal = Filename.Substring(SlashIndex + 1, PeriodIndex - SlashIndex - 1)
            End If
            Return RVal
        End Function

        'Public Function GetMailMessage() As Mime.Message
        '    Dim RVal As New Mime.Message(GetStringMessageContent)
        '    RVal.SmtpFromAddress = Me.From
        '    RVal.SmtpToAddress = Me.[To]
        '    Return RVal
        'End Function

        Public Function DeleteMessage(ByVal MoveToFailedFolder As Boolean) As Boolean
            Return DeleteMessage(Me.MessageId, MoveToFailedFolder)
        End Function

        Public Function DeleteMessage(ByVal DomainName As String, ByVal Username As String, ByVal MessageId As String, ByVal MoveToFailedFolder As Boolean) As Boolean
            Dim Path As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER & "\" & DomainName.ToLower & "\" & Username.ToLower
            Return DeleteMessage(Path, MessageId, MoveToFailedFolder)
        End Function

        Public Function DeleteMessage(ByVal MessageId As String, ByVal MoveToFailedFolder As Boolean) As Boolean
            Dim Path As String = GetMessagePath(MessageId)
            Return DeleteMessage(Path, MessageId, MoveToFailedFolder)
        End Function

        Private Shared Function DeleteMessage(ByVal FolderPath As String, ByVal Filename As String, ByVal MoveToFailedFolder As Boolean) As Boolean
            Dim RVal As Boolean = True
            Try
                'Dim Config As New Config
                If MoveToFailedFolder Then
                    If Not Directory.Exists(FolderPath & "\" & FAILED_MAIL_FOLDER) Then
                        Directory.CreateDirectory(FolderPath & "\" & FAILED_MAIL_FOLDER)
                    End If
                    'If the XML file alraedy exists in the failed mail folder, delete it permanently
                    If File.Exists(FolderPath & "\" & Filename & ".xml") And Not File.Exists(FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml") Then
                        File.Move(FolderPath & "\" & Filename & ".xml", FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml")
                    ElseIf File.Exists(FolderPath & "\" & Filename & ".xml") AndAlso File.Exists(FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml") Then
                        File.Delete(FolderPath & "\" & Filename & ".xml")
                    End If
                    'If the MSG file already exists in the failed mail folder, delete it permanently
                    If File.Exists(FolderPath & "\" & Filename & ".xml.msg") And Not File.Exists(FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml.msg") Then
                        File.Move(FolderPath & "\" & Filename & ".xml.msg", FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml.msg")
                    ElseIf File.Exists(FolderPath & "\" & Filename & ".xml.msg") AndAlso File.Exists(FolderPath & "\" & FAILED_MAIL_FOLDER & "\" & Filename & ".xml.msg") Then
                        File.Delete(FolderPath & "\" & Filename & ".xml.msg")
                    End If
                Else
                    If RemoteConfigClient.RemoteConfig.RetainMessages Then
                        If Not Directory.Exists(FolderPath & "\" & RETAINED_MAIL_FOLDER) Then
                            Directory.CreateDirectory(FolderPath & "\" & RETAINED_MAIL_FOLDER)
                        End If
                        'If the XML file already exists in the retained mail folder, delete it permanently
                        If Not File.Exists(FolderPath & "\" & RETAINED_MAIL_FOLDER & "\" & Filename & ".xml") Then
                            File.Move(FolderPath & "\" & Filename & ".xml", FolderPath & "\" & RETAINED_MAIL_FOLDER & "\" & Filename & ".xml")
                        Else
                            File.Delete(FolderPath & "\" & Filename & ".xml")
                        End If
                        'If the MSG file already exists in the retained mail folder, delete it permanently
                        If File.Exists(FolderPath & "\" & Filename & ".xml.msg") And Not File.Exists(FolderPath & "\" & RETAINED_MAIL_FOLDER & "\" & Filename & ".xml.msg") Then
                            File.Move(FolderPath & "\" & Filename & ".xml.msg", FolderPath & "\" & RETAINED_MAIL_FOLDER & "\" & Filename & ".xml.msg")
                        ElseIf File.Exists(FolderPath & "\" & Filename & ".xml.msg") AndAlso File.Exists(FolderPath & "\" & RETAINED_MAIL_FOLDER & "\" & Filename & ".xml.msg") Then
                            File.Delete(FolderPath & "\" & Filename & ".xml.msg")
                        End If
                    Else
                        File.Delete(FolderPath & "\" & Filename & ".xml")
                        File.Delete(FolderPath & "\" & Filename & ".xml.msg")
                    End If
                End If
                'PersistentStorage.CleanupCache()
            Catch exc As Exception
                'Log.Logger.WriteLog(exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
                RVal = False
            End Try
            Return RVal
        End Function

        Private Shared Function GetMessagePath(ByVal MessageId As String) As String
            Return GetMessagePath(MessageId, GetApplicationDirectory() & "\" & QUEUED_MAIL_MAILBOX_FOLDER)
        End Function

        Private Shared Function GetMessagePath(ByVal MessageId As String, ByVal Path As String) As String
            Dim Folders As String()
            Dim RVal As String = String.Empty
            Folders = Directory.GetDirectories(Path)
            For i As Integer = 0 To Folders.GetUpperBound(0)
                If File.Exists(Folders(i) & "\" & MessageId & ".xml") Then
                    RVal = Folders(i)
                    Exit For
                Else
                    RVal = GetMessagePath(MessageId, Folders(i))
                    If RVal <> String.Empty Then
                        Exit For
                    End If
                End If
            Next
            Return RVal
        End Function

        Public Function GetUndeliveredMessages() As QueuedMail.MessageCollection
            Dim RVal As New QueuedMail.MessageCollection
            Dim Files As String()
            Dim i As Integer
            Files = Directory.GetFiles(GetApplicationDirectory() & "\" & QUEUED_MAIL_FOLDER, "*.xml")
            For i = 0 To Files.GetUpperBound(0)
                RVal.Add(New Message(Files(i)))
            Next
            Return RVal
        End Function

        Public Function GetUndeliveredMessage(ByVal MessageId As String) As QueuedMail.Message
            Dim RVal As QueuedMail.Message = Nothing
            Dim MessagePath As String = GetApplicationDirectory() & "\" & QUEUED_MAIL_FOLDER & "\" & MessageId & ".xml"
            If File.Exists(MessagePath) Then
                RVal = New Message(MessagePath)
            End If
            Return RVal
        End Function

        'Public Sub SetMailMessage(ByVal Message As Mime.Message)
        '    If Not IsNothing(Message) Then
        '        If Message.Headers.Count("from") > 0 Then
        '            Me.From = Message.SmtpFromAddress
        '            Me.To = Message.SmtpToAddress
        '            Me.SetMessageContent(Message.RawMessage)
        '        End If
        '    End If
        'End Sub

        Public Property ContentInAlternateFile() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/ContentInAlternateFile")
                If IsNothing(TmpVal) Then
                    RVal = False
                ElseIf CType(TmpVal, String).Trim.Length = 0 Then
                    RVal = False
                Else
                    Try
                        RVal = CType(TmpVal, Boolean)
                    Catch
                        RVal = False
                    End Try
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/message/ContentInAlternateFile", Value)
            End Set
        End Property

        Public Property From() As EmailAddress
            Get
                Return New EmailAddress(MyBase.GetXmlValue("/message/from"))
            End Get
            Set(ByVal Value As EmailAddress)
                If Not IsNothing(Value) Then
                    MyBase.SetXmlValue("/message/from", Value.ToString)
                Else
                    MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/message/from"))
                End If
            End Set
        End Property

        Public Property [To]() As EmailAddress
            Get
                Return New EmailAddress(MyBase.GetXmlValue("/message/to"))
            End Get
            Set(ByVal Value As EmailAddress)
                If Not IsNothing(Value) Then
                    MyBase.SetXmlValue("/message/to", Value.ToString)
                Else
                    MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/message/to"))
                End If
            End Set
        End Property

        Public Property DateTime() As DateTime
            Get
                Dim RVal As String = MyBase.GetXmlValue("/message/date_time")
                If IsDate(RVal) Then
                    Return System.DateTime.Parse(RVal)
                Else
                    Return System.DateTime.MinValue
                End If
            End Get
            Set(ByVal Value As DateTime)
                If Not IsNothing(Value) AndAlso IsDate(Value) Then
                    MyBase.SetXmlValue("/message/date_time", Value.ToString("dd-MMM-yyyy HH:mm:ss"))
                Else
                    MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/message/date_time"))
                End If
            End Set
        End Property

        Public Function GetStreamMessageContent() As StreamReader
            Dim RVal As StreamReader = Nothing

            If ContentInAlternateFile Then
                If File.Exists(MyBase.XmlFilePath & ".msg") Then
                    RVal = New StreamReader(MyBase.XmlFilePath & ".msg")
                Else
                    Throw New FileNotFoundException("The content file for the message could not be found.  The filename is: """ & MyBase.XmlFilePath & ".msg""", MyBase.XmlFilePath & ".msg")
                End If
            Else
                'Needed to handle backward compatibility
                Dim Buffer As Byte()
                Dim Message As String = XmlMessageContent
                Dim MS As MemoryStream
                Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(Message)
                MS = New MemoryStream(Buffer)
                RVal = New StreamReader(MS)
            End If
            Return RVal
        End Function

        Private _StringMessageContent As String = String.Empty
        Public Function GetStringMessageContent() As String
            Dim RVal As String = String.Empty
            Dim SR As StreamReader = Nothing
            Try
                If _StringMessageContent = String.Empty Then
                    SR = GetStreamMessageContent()
                    If Me.Encrypted Then
                        _StringMessageContent = Majodio.DecryptString(SR.ReadToEnd)
                    Else
                        _StringMessageContent = SR.ReadToEnd
                    End If

                    RVal = _StringMessageContent
                Else
                    RVal = _StringMessageContent
                End If
                Return RVal
            Finally
                If Not IsNothing(SR) Then
                    SR.Close()
                    SR = Nothing
                End If
            End Try
        End Function

        Public Sub SetMessageContent(ByVal Content As String)
            ContentInAlternateFile = True
            Dim FS As FileStream = Nothing
            Dim SR As StreamWriter = Nothing
            'Dim C As Config = New Config()
            Try
                _StringMessageContent = Content
                FS = New FileStream(MyBase.XmlFilePath & ".msg", FileMode.Create, FileAccess.Write)
                SR = New StreamWriter(FS)

                If RemoteConfigClient.RemoteConfig.EncryptEmail Then
                    SR.Write(Majodio.EncryptString(Content))
                    Encrypted = True
                Else
                    SR.Write(Content)
                    Encrypted = False
                End If
                MessageSize = Content.Length
            Finally
                If Not IsNothing(SR) Then
                    SR.Close()
                End If
            End Try
        End Sub


        Private ReadOnly Property XmlMessageContent() As String
            Get
                Dim TmpVal As String
                TmpVal = MyBase.GetXmlValue("/message/message_content")
                Dim RVal As String
                If Encrypted Then
                    RVal = DecryptString(TmpVal)
                Else
                    RVal = TmpVal
                End If
                Return RVal
            End Get
            'Set(ByVal Value As String)
            '    If Not IsNothing(Value) AndAlso Value.Length > 0 Then
            '        Dim Config As New Configuration.Config
            '        If Config.EncryptEmail Then
            '            Encrypted = True
            '            MyBase.SetXmlValue("/message/message_content", EncryptString(Value))
            '        Else
            '            Encrypted = False
            '            MyBase.SetXmlValue("/message/message_content", Value)
            '        End If
            '        MessageSize = Value.Length
            '    Else
            '        MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/message/message_content"))
            '    End If
            'End Set
        End Property

        Public Property UniqueId() As UniqueId
            Get
                Dim TmpVal As Object
                Dim RVal As UniqueId
                TmpVal = MyBase.GetXmlValue("/message/unique_id")
                If Not IsNumeric(TmpVal) Then
                    RVal = New UniqueId(1)
                Else
                    RVal = New UniqueId(CType(TmpVal, Int32))
                End If
                Return RVal
            End Get
            Set(ByVal Value As UniqueId)
                MyBase.SetXmlValue("/message/unique_id", Value.Value)
            End Set
        End Property

        Public Property Seen() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/seen")
                If IsNothing(TmpVal) Then
                    RVal = False
                ElseIf CType(TmpVal, String).Trim.Length = 0 Then
                    RVal = False
                Else
                    Try
                        RVal = CType(TmpVal, Boolean)
                    Catch
                        RVal = False
                    End Try
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/message/seen", Value)
            End Set
        End Property

        Public Property Answered() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/answered")
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
                MyBase.SetXmlValue("/message/answered", Value)
            End Set
        End Property

        Public Property Flagged() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/flagged")
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
                MyBase.SetXmlValue("/message/flagged", Value)
            End Set
        End Property

        Public Property Deleted() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/deleted")
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
                MyBase.SetXmlValue("/message/deleted", Value)
            End Set
        End Property

        Public Property Draft() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/draft")
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
                MyBase.SetXmlValue("/message/draft", Value)
            End Set
        End Property

        Public Property Recent() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/recent")
                If IsNothing(TmpVal) Then
                    RVal = True
                ElseIf CType(TmpVal, String).Trim.Length = 0 Then
                    RVal = True
                Else
                    RVal = CType(TmpVal, Boolean)
                End If
                If RVal Then
                    Recent = False
                End If
                Return RVal
            End Get
            Set(ByVal Value As Boolean)
                MyBase.SetXmlValue("/message/recent", Value)
            End Set
        End Property

        Public Property MessageSize() As Integer
            Get
                Dim TmpVal As Object
                Dim RVal As Int64
                TmpVal = MyBase.GetXmlValue("/message/messagesize")
                If IsNothing(TmpVal) OrElse CType(TmpVal, String).Trim.Length = 0 OrElse Not IsNumeric(TmpVal) Then
                    RVal = 0
                Else
                    RVal = CType(TmpVal, Integer)
                End If
                Return RVal
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/message/messagesize", Value)
            End Set
        End Property

        Public Property Encrypted() As Boolean
            Get
                Dim TmpVal As Object
                Dim RVal As Boolean
                TmpVal = MyBase.GetXmlValue("/message/encrypted")
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
                MyBase.SetXmlValue("/message/encrypted", Value)
            End Set
        End Property

        Public Property LastSendAttempt() As DateTime
            Get
                Dim TempVal As String = MyBase.GetXmlValue("/message/LastSendAttempt")
                Dim RVal As DateTime
                If Not IsNothing(TempVal) Then
                    If IsDate(TempVal) Then
                        RVal = CType(TempVal, DateTime)
                    Else
                        RVal = Date.MinValue
                    End If
                Else
                    RVal = Date.MinValue
                End If
                Return RVal
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetXmlValue("/message/LastSendAttempt", Value.ToString)
            End Set
        End Property

        Public Property SendAttemptsMade() As Integer
            Get
                Dim TempVal As Object
                Dim RVal As Integer = 0
                TempVal = MyBase.GetXmlValue("/message/SendAttemptsMade")
                If IsNumeric(TempVal) Then
                    RVal = TempVal
                End If
                Return RVal
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetXmlValue("/message/SendAttemptsMade", Value)
            End Set
        End Property

        Public Property LastResult() As Smtp.Response
            Get
                Dim TempVal As String
                Dim RVal As Smtp.Response = Nothing
                Dim SpaceIndex As Integer

                TempVal = MyBase.GetXmlValue("/message/LastResult")
                If Not IsNothing(TempVal) Then
                    If TempVal.Trim.Length > 0 Then
                        SpaceIndex = TempVal.IndexOf(" ")
                        If SpaceIndex = 3 Then
                            RVal = New Smtp.Response(TempVal.Substring(0, 3), TempVal.Substring(4))
                        End If
                    End If
                End If
                Return RVal
            End Get
            Set(ByVal Value As Smtp.Response)
                If Not IsNothing(Value) Then
                    MyBase.SetXmlValue("/message/LastResult", Value.ToString)
                Else
                    MyBase.SetXmlValue("/message/LastResult", String.Empty)
                End If
            End Set
        End Property

        Public Sub Save()
            MyBase.SaveXmlFile()
        End Sub

        Public Overrides Function GetDefaultXml() As XmlDocument
            Dim RVal As New XmlDocument

            Dim Attribute As XmlAttribute
            'Setup the root node
            Dim QueuedMailMessageMessageNode As XmlElement = RVal.CreateNode(XmlNodeType.Element, "message", "")

            'Add the root node to the document
            RVal.AppendChild(QueuedMailMessageMessageNode)

            'Create the from Node
            Dim QueuedMailMessageFrom As XmlElement = RVal.CreateNode(XmlNodeType.Element, "from", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = String.Empty
            QueuedMailMessageFrom.Attributes.Append(Attribute)
            QueuedMailMessageMessageNode.AppendChild(QueuedMailMessageFrom)

            'Create the to Node
            Dim QueuedMailMessageTo As XmlElement = RVal.CreateNode(XmlNodeType.Element, "to", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = String.Empty
            QueuedMailMessageTo.Attributes.Append(Attribute)
            QueuedMailMessageMessageNode.AppendChild(QueuedMailMessageTo)

            'Create the date_time Node
            Dim QueuedMailMessageDateTime As XmlElement = RVal.CreateNode(XmlNodeType.Element, "date_time", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = String.Empty
            QueuedMailMessageDateTime.Attributes.Append(Attribute)
            QueuedMailMessageMessageNode.AppendChild(QueuedMailMessageDateTime)

            'Create the message_content Node
            Dim QueuedMailMessageMessageContent As XmlElement = RVal.CreateNode(XmlNodeType.Element, "message_content", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = String.Empty
            QueuedMailMessageMessageContent.Attributes.Append(Attribute)
            QueuedMailMessageMessageNode.AppendChild(QueuedMailMessageMessageContent)

            'Create the unique_id Node
            Dim QueuedMailMessageUniqueId As XmlElement = RVal.CreateNode(XmlNodeType.Element, "unique_id", "")
            Attribute = RVal.CreateAttribute("value")
            Attribute.Value = 1
            QueuedMailMessageUniqueId.Attributes.Append(Attribute)
            QueuedMailMessageMessageNode.AppendChild(QueuedMailMessageUniqueId)
            Return RVal
        End Function

        Public Overloads Function Equals(ByVal Obj As Object) As Boolean
            Dim RVal As Boolean = True
            If Not Obj.GetType Is GetType(Message) Then
                RVal = False
            Else
                Dim CompMessage As Message = CType(Obj, Message)
                If Me.Answered <> CompMessage.Answered Then
                    RVal = False
                ElseIf Me.DateTime <> CompMessage.DateTime Then
                    RVal = False
                ElseIf Me.Deleted <> CompMessage.Deleted Then
                    RVal = False
                ElseIf Me.Draft <> CompMessage.Draft Then
                    RVal = False
                ElseIf Me.Encrypted <> CompMessage.Encrypted Then
                    RVal = False
                ElseIf Me.Flagged <> CompMessage.Flagged Then
                    RVal = False
                ElseIf Me.From.ToString(EmailStringFormat.NameAddress) <> CompMessage.From.ToString(EmailStringFormat.NameAddress) Then
                    RVal = False
                ElseIf Me.GetStringMessageContent <> CompMessage.GetStringMessageContent Then
                    RVal = False
                ElseIf Me.MessageId <> CompMessage.MessageId Then
                    RVal = False
                ElseIf Me.MessageSize <> CompMessage.MessageSize Then
                    RVal = False
                ElseIf Me.Recent <> CompMessage.Recent Then
                    RVal = False
                ElseIf Me.Seen <> CompMessage.Seen Then
                    RVal = False
                ElseIf Me.To.ToString(EmailStringFormat.NameAddress) <> CompMessage.To.ToString(EmailStringFormat.NameAddress) Then
                    RVal = False
                ElseIf Me.UniqueId.Value <> CompMessage.UniqueId.Value Then
                    RVal = False
                End If
            End If
            Return RVal
        End Function
    End Class
End Namespace