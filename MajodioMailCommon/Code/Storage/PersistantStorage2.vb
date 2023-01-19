'Imports System.xml
'Imports System.Threading
'Imports System.IO
'Imports System.Collections.Specialized

'Public Delegate Sub PersistentStorageChangedEventHandler(ByVal Sender As Object, ByVal E As PersistentStorageChangedEventArgs)

'Public MustInherit Class PersistentStorage
'    Private _XmlFilePath As String
'    Private Shared _DocumentHash As Hashtable
'    Private Shared _InitialLoadTimeHash As Hashtable
'    Private Shared _DocumentDateHash As Hashtable
'    Private Shared _CacheCleanupTimer As System.Timers.Timer
'    Private Shared _FolderWatcher As FileSystemWatcher

'    Private _FileWatcher As FileSystemWatcher

'    Public MustOverride Function GetDefaultXml() As XmlDocument


'    Public Event Changed As PersistentStorageChangedEventHandler

'    Shared Sub New()
'        InitializeCache()
'    End Sub

'    Public Sub New(ByVal XmlFilePath As String)
'        _XmlFilePath = XmlFilePath.Trim.ToLower
'        VerifyXmlFileExists()
'        LoadXmlFile()
'        InitializeWatcher()
'    End Sub

'    Protected Overrides Sub Finalize()
'        FinalizeWatcher()
'    End Sub

'#Region " Cache Management"
'    Private Shared Sub InitializeCache()
'        _DocumentHash = New Hashtable
'        _InitialLoadTimeHash = New Hashtable
'        _CacheCleanupTimer = New System.Timers.Timer(15000)
'        _CacheCleanupTimer.AutoReset = False
'        AddHandler _CacheCleanupTimer.Elapsed, AddressOf CacheCleanupTimer_Elapsed
'        _CacheCleanupTimer.Start()

'        'Initialize the folder watcher
'        _FolderWatcher = New FileSystemWatcher(GetApplicationDirectory() & "\" & DATA_FILES_FOLDER, "*.xml")
'        _FolderWatcher.IncludeSubdirectories = True
'        _FolderWatcher.NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.Size Or NotifyFilters.CreationTime
'        AddHandler _FolderWatcher.Changed, AddressOf FolderWatcher_Changed
'        AddHandler _FolderWatcher.Deleted, AddressOf FolderWatcher_Changed
'        AddHandler _FolderWatcher.Created, AddressOf FolderWatcher_Changed
'    End Sub

'    Private Shared Sub CacheCleanupTimer_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
'        CleanupCache()
'        _CacheCleanupTimer.Start()
'    End Sub

'    Private Shared Sub FolderWatcher_Changed(ByVal Sender As Object, ByVal E As FileSystemEventArgs)

'    End Sub

'    Public Shared Sub CleanupCache()
'        Dim KeysToRemove As New ArrayList
'        Dim RemovedObject As Object
'        Try
'            Monitor.Enter(_DocumentHash)
'            For Each Key As String In _DocumentHash.Keys
'                If Not File.Exists(Key) Or _InitialLoadTimeHash.ContainsKey(Key) AndAlso TimeSpan.FromTicks(DateTime.Now.Ticks - CType(_InitialLoadTimeHash(Key), DateTime).Ticks).TotalSeconds > 60 Then
'                    '_DocumentHash.Remove(Key)
'                    Monitor.Enter(_DocumentHash(Key))
'                    KeysToRemove.Add(Key)
'                End If
'            Next
'            For i As Integer = 0 To KeysToRemove.Count - 1
'                RemovedObject = _DocumentHash(KeysToRemove(i))
'                _DocumentHash.Remove(KeysToRemove(i))
'                _InitialLoadTimeHash.Remove(KeysToRemove(i))
'                Monitor.Exit(RemovedObject)
'            Next
'        Finally
'            Monitor.Exit(_DocumentHash)
'        End Try
'    End Sub

'    Public Sub DeleteCache()
'        DeleteCache(_XmlFilePath)
'    End Sub

'    Public Shared Sub DeleteCache(ByVal FilePath As String)
'        Try
'            System.Threading.Monitor.Enter(_DocumentHash)
'            If _DocumentHash.ContainsKey(FilePath.ToLower) Then
'                _DocumentHash.Remove(FilePath.ToLower)
'            End If
'        Finally
'            System.Threading.Monitor.Exit(_DocumentHash)
'        End Try
'    End Sub

'    Public Shared Sub DeleteCacheFolder(ByVal FolderPath As String)
'        Try
'            Monitor.Enter(_DocumentHash)
'            For Each Key As String In _DocumentHash.Keys
'                If FolderPath.ToLower = Key.Substring(0, FolderPath.Length).ToLower Then
'                    _DocumentHash.Remove(Key)
'                End If
'            Next
'        Finally
'            Monitor.Exit(_DocumentHash)
'        End Try
'    End Sub
'#End Region
'#Region " File Change Management"
'    Protected Sub FinalizeWatcher()
'        If Not IsNothing(_FileWatcher) Then
'            _FileWatcher.EnableRaisingEvents = False
'            RemoveHandler _FileWatcher.Changed, AddressOf FolderWatcher_OnChanged
'            'RemoveHandler _FileWatcher.Created, AddressOf FolderWatcher_OnChanged
'            'RemoveHandler _FileWatcher.Deleted, AddressOf FolderWatcher_OnChanged
'            'RemoveHandler _FileWatcher.Renamed, AddressOf FolderWatcher_OnRenamed
'        End If
'        MyBase.Finalize()
'    End Sub

'    Private Sub InitializeWatcher()
'        _FileWatcher = New FileSystemWatcher

'        _FileWatcher.Path = Path.GetDirectoryName(_XmlFilePath)
'        _FileWatcher.Filter = Path.GetFileName(_XmlFilePath)
'        _FileWatcher.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)

'        AddHandler _FileWatcher.Changed, AddressOf FolderWatcher_OnChanged
'        'AddHandler _FileWatcher.Created, AddressOf FolderWatcher_OnChanged
'        'AddHandler _FileWatcher.Deleted, AddressOf FolderWatcher_OnChanged
'        'AddHandler _FileWatcher.Renamed, AddressOf FolderWatcher_OnRenamed

'        _FileWatcher.EnableRaisingEvents = True
'    End Sub

'    Public Sub FolderWatcher_OnChanged(ByVal Sender As Object, ByVal E As FileSystemEventArgs)
'        RaiseEvent Changed(Me, EventArgs.Empty)
'    End Sub

'    Public Sub FolderWatcher_OnRenamed(ByVal Sender As Object, ByVal E As RenamedEventArgs)
'        RaiseEvent Changed(Me, EventArgs.Empty)
'    End Sub
'#End Region
'    Public ReadOnly Property XmlFilePath() As String
'        Get
'            Return _XmlFilePath
'        End Get
'    End Property

'    Public Shared Function FixPath(ByVal FolderPath As String) As String
'        Dim RVal As String = FolderPath
'        'Remove any begining slashes
'        While RVal.Length > 0 AndAlso RVal.StartsWith("\")
'            RVal = RVal.Substring(1)
'        End While
'        'Remove any double slashes
'        While RVal.IndexOf("\\") > -1
'            RVal = RVal.Replace("\\", "\")
'        End While
'        'Make sure the path ends with a slash
'        If RVal.Length > 0 AndAlso Not RVal.EndsWith("\") Then
'            RVal &= "\"
'        End If
'        Return RVal
'    End Function

'    Private Shared VerifyXmlFileExistsLock As New Object
'    Public Sub VerifyXmlFileExists()
'        Try
'            System.Threading.Monitor.Enter(VerifyXmlFileExistsLock)
'            Dim Directories As String()
'            Dim CurrentDirectory As String
'            Dim i As Integer
'            Dim XmlDoc As XmlDocument
'            If Not File.Exists(_XmlFilePath) Then
'                Directories = Split(_XmlFilePath, "\")
'                For i = 0 To Directories.GetUpperBound(0) - 1
'                    If i = 0 Then
'                        CurrentDirectory = Directories(i)
'                    Else
'                        CurrentDirectory &= "\" & Directories(i)
'                    End If
'                    If Not Directory.Exists(CurrentDirectory) Then
'                        Directory.CreateDirectory(CurrentDirectory)
'                    End If
'                Next
'                XmlDoc = GetDefaultXml()
'                XmlDoc.Save(_XmlFilePath)
'            End If
'        Finally
'            System.Threading.Monitor.Exit(VerifyXmlFileExistsLock)
'        End Try
'    End Sub

'    Protected Sub LoadXmlFile()
'        LoadXmlFile(_XmlFilePath)
'    End Sub

'    Protected Sub LoadXmlFile(ByVal Path As String)
'        Try
'            System.Threading.Monitor.Enter(_DocumentHash)
'            Dim XD As XmlDocument
'            If Not _DocumentHash.ContainsKey(Path.ToLower) Then
'                XD = New XmlDocument
'                XD.Load(Path)
'                _DocumentHash.Add(Path.ToLower, XD)
'                _InitialLoadTimeHash.Add(Path.ToLower, DateTime.Now)
'                _DocumentDateHash.Add(Path.ToLower, File.GetLastWriteTime(Path.ToLower))
'            End If
'        Finally
'            System.Threading.Monitor.Exit(_DocumentHash)
'        End Try
'    End Sub

'    Protected Sub SaveXmlFile()
'        Try
'            AcquireDocumentLock()
'            XmlDoc.Save(_XmlFilePath)
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Sub

'    Protected Sub CopyNode(ByVal ParentOldNode As XmlElement, ByRef ParentNewNode As XmlElement)
'        Try
'            AcquireDocumentLock()
'            Dim NewElem As XmlElement
'            Dim NewAttr As XmlAttribute
'            For Each OldElem As XmlElement In ParentOldNode.ChildNodes
'                NewElem = CreateXmlElement(ParentNewNode, OldElem.Name)
'                For Each OldAttr As XmlAttribute In OldElem.Attributes
'                    NewAttr = XmlDoc.CreateAttribute(OldAttr.Name)
'                    NewAttr.Value = OldAttr.Value
'                    NewElem.Attributes.Append(NewAttr)
'                Next
'                CopyNode(OldElem, NewElem)
'            Next
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Sub

'    Protected ReadOnly Property XmlDoc() As XmlDocument
'        Get
'            Try
'                Monitor.Enter(_DocumentHash)
'                If Not _DocumentHash.Contains(_XmlFilePath) Then
'                    LoadXmlFile()
'                End If
'                Return CType(_DocumentHash(_XmlFilePath), XmlDocument)
'            Finally
'                Monitor.Exit(_DocumentHash)
'            End Try
'        End Get
'    End Property

'    Protected Function GetXmlValue(ByVal XPath As String) As String
'        Return GetXmlValue(XPath, "value")
'    End Function

'    Protected Function GetXmlValue(ByVal XPath As String, ByVal AttributeName As String) As String
'        Try
'            AcquireDocumentLock()
'            Dim RVal As String = String.Empty
'            Dim Element As XmlElement
'            If Not IsNothing(XmlDoc) Then
'                Element = XmlDoc.SelectSingleNode(XPath)
'                If Not IsNothing(Element) AndAlso Not IsNothing(Element.Attributes(AttributeName)) Then
'                    RVal = Element.Attributes(AttributeName).Value()
'                End If
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Function SetXmlValue(ByVal XPath As String, ByVal Value As String) As String
'        SetXmlValue(XPath, "value", Value)
'    End Function

'    Protected Function SetXmlValue(ByVal XPath As String, ByVal AttributeName As String, ByVal Value As String) As String
'        Try
'            AcquireDocumentLock()
'            Dim Element As XmlElement
'            If Not IsNothing(XmlDoc) Then
'                Element = XmlDoc.SelectSingleNode(XPath)
'                If IsNothing(Element) Then
'                    Dim LastSlash As Integer = XPath.LastIndexOf("/")
'                    If LastSlash > 0 Then
'                        Dim NVCol As New NameValueCollection
'                        NVCol.Add(AttributeName, Value)
'                        Element = Me.CreateXmlElement(GetSingleXmlElement(XPath.Substring(0, LastSlash)), XPath.Substring(LastSlash + 1), NVCol)
'                    End If
'                End If
'                If IsNothing(Element.Attributes(AttributeName)) Then
'                    Dim Attribute As XmlAttribute = XmlDoc.CreateAttribute(AttributeName)
'                    Attribute.Value = Value
'                    Element.Attributes.Append(Attribute)
'                Else
'                    Element.Attributes(AttributeName).Value() = Value
'                End If
'                XmlDoc.Save(_XmlFilePath)
'            End If
'        Finally
'            ReleaseDocumentLock()
'        End Try

'    End Function

'    Protected Function GetSingleXmlElement(ByVal XPath As String) As XmlElement
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlElement
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.SelectSingleNode(XPath)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Function GetXmlElements(ByVal XPath As String) As XmlNodeList
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlNodeList
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.SelectNodes(XPath)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    ''' -----------------------------------------------------------------------------
'    ''' <summary>
'    '''     Creates an element without adding it to a parent node
'    ''' </summary>
'    ''' <param name="Name"></param>
'    ''' <returns></returns>
'    ''' <remarks>
'    ''' </remarks>
'    ''' <history>
'    ''' 	[ruwem]	3/15/2005	Created
'    ''' </history>
'    ''' -----------------------------------------------------------------------------
'    Protected Function CreateXmlElement(ByVal Name As String) As XmlElement
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlElement
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.CreateElement(Name)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String) As XmlElement
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlElement
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.CreateElement(Name)
'                If Not IsNothing(ParentNode) Then
'                    ParentNode.AppendChild(RVal)
'                Else
'                    XmlDoc.AppendChild(RVal)
'                End If
'                XmlDoc.Save(_XmlFilePath)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal AttributeName As String, ByVal AttributeValue As String) As XmlElement
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlElement
'            Dim Attribute As XmlAttribute
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.CreateElement(Name)
'                Attribute = XmlDoc.CreateAttribute(AttributeName)
'                Attribute.Value = AttributeValue
'                RVal.Attributes.Append(Attribute)
'                If Not IsNothing(ParentNode) Then
'                    ParentNode.AppendChild(RVal)
'                Else
'                    XmlDoc.AppendChild(RVal)
'                End If
'                XmlDoc.Save(_XmlFilePath)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal Attributes As NameValueCollection) As XmlElement
'        Try
'            AcquireDocumentLock()
'            Dim RVal As XmlElement
'            Dim AttributeName As String
'            Dim Attribute As XmlAttribute
'            If Not IsNothing(XmlDoc) Then
'                RVal = XmlDoc.CreateElement(Name)
'                If Not IsNothing(Attributes) Then
'                    For Each AttributeName In Attributes.Keys
'                        Attribute = XmlDoc.CreateAttribute(AttributeName)
'                        Attribute.Value = Attributes(AttributeName)
'                        RVal.Attributes.Append(Attribute)
'                    Next
'                End If
'                If Not IsNothing(ParentNode) Then
'                    ParentNode.AppendChild(RVal)
'                Else
'                    XmlDoc.AppendChild(RVal)
'                End If
'                XmlDoc.Save(_XmlFilePath)
'            End If
'            Return RVal
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Function

'    Protected Sub DeleteXmlElement(ByVal Node As XmlElement)
'        Try
'            AcquireDocumentLock()
'            Dim ParentNode As XmlElement
'            If Not IsNothing(Node) Then
'                ParentNode = Node.ParentNode
'                ParentNode.RemoveChild(Node)
'                XmlDoc.Save(_XmlFilePath)
'            End If
'        Finally
'            ReleaseDocumentLock()
'        End Try
'    End Sub

'    Private Sub AcquireDocumentLock()
'        Monitor.Enter(XmlDoc)
'    End Sub

'    Private Sub ReleaseDocumentLock()
'        Monitor.Exit(XmlDoc)
'    End Sub

'    Private Sub AcquireHashLock()
'        Monitor.Enter(_DocumentHash)
'    End Sub

'    Private Sub ReleaseHashLock()
'        Monitor.Exit(_DocumentHash)
'    End Sub
'End Class