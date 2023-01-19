Imports System.xml
Imports System.IO
Imports System.Collections.Specialized
Imports System.threading

Namespace Storage
    Public Delegate Sub PersistentStorageChangedEventHandler(ByVal Sender As Object, ByVal E As PersistentStorageChangedEventArgs)

    ''' -----------------------------------------------------------------------------
    ''' Project	 : MajodioMailCommon
    ''' Class	 : Mail.Common.Storage.PersistentStorage
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	8/18/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class PersistentStorage
        Inherits MarshalByRefObject
#Region " Private Fields"
        Private _xmlDocument As XmlDocument
        Public _XmlFilePath As String
        'Private Shared _DocumentCache As DocumentCacheManagement
        Private _FileWatcher As FileSystemWatcher
        Private _FolderWatcher As FileSystemWatcher
        Private _LastFileChanged As DateTime
        Private _initialized As Boolean = False
#End Region
#Region " Public Events"
        'Public Event Changed As PersistentStorageChangedEventHandler
        Public Event FolderChanged As PersistentStorageChangedEventHandler
#End Region
        Public MustOverride Function GetDefaultXml() As XmlDocument

#Region " Shared Constrctors"
        Shared Sub New()
            '_DocumentCache = New DocumentCacheManagement
        End Sub
#End Region
#Region " Instance Constructors/Destructors"
        Public Sub New()

        End Sub

        Public Sub New(ByVal XmlFilePath As String)
            Initialize(XmlFilePath)
        End Sub

        Protected Overrides Sub Finalize()
            'FinalizeWatcher()
        End Sub
#End Region
#Region " File Change Management"
        'Protected Sub FinalizeWatcher()
        '    If Not IsNothing(_FileWatcher) Then
        '        _FileWatcher.EnableRaisingEvents = False
        '        RemoveHandler _FileWatcher.Changed, AddressOf FolderWatcher_OnChanged
        '        RemoveHandler _FileWatcher.Created, AddressOf FolderWatcher_OnChanged
        '        RemoveHandler _FileWatcher.Deleted, AddressOf FolderWatcher_OnChanged
        '        RemoveHandler _FileWatcher.Renamed, AddressOf FolderWatcher_OnRenamed
        '    End If
        '    MyBase.Finalize()
        'End Sub

        'Private Sub InitializeWatcher()
        '    _FileWatcher = New FileSystemWatcher
        '    _FolderWatcher = New FileSystemWatcher

        '    _FileWatcher.Path = Path.GetDirectoryName(_XmlFilePath)
        '    _FileWatcher.Filter = Path.GetFileName(_XmlFilePath)
        '    _FileWatcher.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)

        '    AddHandler _FileWatcher.Changed, AddressOf FolderWatcher_OnChanged
        '    AddHandler _FileWatcher.Created, AddressOf FolderWatcher_OnChanged
        '    AddHandler _FileWatcher.Deleted, AddressOf FolderWatcher_OnChanged
        '    AddHandler _FileWatcher.Renamed, AddressOf FolderWatcher_OnRenamed

        '    _FileWatcher.EnableRaisingEvents = True
        '    _LastFileChanged = DateTime.Now
        'End Sub


        'Public Sub FolderWatcher_OnChanged(ByVal Sender As Object, ByVal E As FileSystemEventArgs)
        '    Try
        '        System.Threading.Monitor.Enter(_LastFileChanged)
        '        If TimeSpan.FromTicks(DateTime.Now.Ticks - _LastFileChanged.Ticks).TotalSeconds > 5 Then
        '            If E.ChangeType = WatcherChangeTypes.Changed Then
        '                OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Updated))
        '            ElseIf E.ChangeType = WatcherChangeTypes.Created Then
        '                OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Created))
        '            ElseIf E.ChangeType = WatcherChangeTypes.Deleted Then
        '                OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Deleted))
        '            End If
        '            _LastFileChanged = DateTime.Now
        '        End If
        '    Finally
        '        System.Threading.Monitor.Exit(_LastFileChanged)
        '    End Try
        'End Sub

        'Public Sub FolderWatcher_OnRenamed(ByVal Sender As Object, ByVal E As RenamedEventArgs)
        '    Try
        '        System.Threading.Monitor.Enter(_LastFileChanged)
        '        If TimeSpan.FromTicks(DateTime.Now.Ticks - _LastFileChanged.Ticks).TotalSeconds > 5 Then
        '            OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Updated))
        '            _LastFileChanged = DateTime.Now
        '        End If
        '    Finally
        '        System.Threading.Monitor.Exit(_LastFileChanged)
        '    End Try
        'End Sub

        'Public Sub OnFolderChanged(ByVal E As PersistentStorageChangedEventArgs)
        '    RaiseEvent Changed(Me, E)
        'End Sub
#End Region
#Region " Public Shared Methods"
        'Public Shared Sub CleanupCache()
        '    DocumentCacheManagement.CleanupCache()
        'End Sub

        Public Shared Function FixPath(ByVal FolderPath As String) As String
            Dim RVal As String = FolderPath
            'Remove any begining slashes
            While RVal.Length > 0 AndAlso RVal.StartsWith("\")
                RVal = RVal.Substring(1)
            End While
            'Remove any double slashes
            While RVal.IndexOf("\\") > -1
                RVal = RVal.Replace("\\", "\")
            End While
            'Make sure the path ends with a slash
            If RVal.Length > 0 AndAlso Not RVal.EndsWith("\") Then
                RVal &= "\"
            End If
            Return RVal
        End Function
#End Region
#Region " Public Instance Methods"
        Public Sub VerifyXmlFileExists()
            Try
                'DocumentCacheManagement.AquireHashLock()
                Dim Directories As String()
                Dim CurrentDirectory As String = String.Empty
                Dim i As Integer
                Dim XmlDoc As XmlDocument
                Dim Index As Integer
                If Not File.Exists(_XmlFilePath) OrElse FileLen(_XmlFilePath) = 0 Then
                    Directories = Split(_XmlFilePath, "\")
                    For i = 0 To Directories.GetUpperBound(0) - 1
                        If i = 0 Then
                            CurrentDirectory = Directories(i)
                        Else
                            CurrentDirectory &= "\" & Directories(i)
                        End If
                        If CurrentDirectory.Length > 0 AndAlso Not Directory.Exists(CurrentDirectory) Then
                            If CurrentDirectory.StartsWith("\\") Then
                                Index = CurrentDirectory.LastIndexOf("\")
                                If Index > -1 And Index > 1 Then
                                    Directory.CreateDirectory(CurrentDirectory)
                                End If
                            Else
                                Directory.CreateDirectory(CurrentDirectory)
                            End If

                        End If
                    Next
                    XmlDoc = GetDefaultXml()
                    XmlDoc.Save(_XmlFilePath)
                    ''OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Created))
                End If
            Finally
                'DocumentCacheManagement.ReleaseHashLock()
            End Try
        End Sub

        Public Function FileExists() As Boolean
            Return File.Exists(_XmlFilePath)
        End Function
#End Region
#Region " Public Instance Properties"
        Public ReadOnly Property XmlFilePath() As String
            Get
                Return _XmlFilePath
            End Get
        End Property
#End Region
#Region " Protected Instance Properties"
        Protected ReadOnly Property XmlDoc() As XmlDocument
            Get
                Try
                    'DocumentCacheManagement.AquireHashLock()
                    LoadXmlFile()
                    Return _xmlDocument
                Finally
                    'DocumentCacheManagement.ReleaseHashLock()
                End Try
            End Get
        End Property
#End Region
#Region " Protected Instance Methods"
        Protected Overridable Sub Initialize(ByVal xmlFilePath As String)
            If Not _initialized Then
                While xmlFilePath.IndexOf("\\") > -1
                    xmlFilePath = xmlFilePath.Replace("\\", "\")
                End While
                _XmlFilePath = xmlFilePath.Trim.ToLower
                VerifyXmlFileExists()
                LoadXmlFile()
                'InitializeWatcher()
                _initialized = True
            Else
                Throw New InvalidOperationException("The initialize method can only be invoked once per object instance")
            End If
        End Sub

        Protected Sub LoadXmlFile()
            LoadXmlFile(_XmlFilePath, CType(Nothing, ReaderWriterLock))
        End Sub

        Protected Sub LoadXmlFile(ByVal path As String)
            LoadXmlFile(path, CType(Nothing, ReaderWriterLock))
        End Sub


        Protected Sub LoadXmlFile(ByVal Path As String, ByVal lock As ReaderWriterLock)
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireReaderLock(5000)
                End If
                If IsNothing(_xmlDocument) Then
                    _xmlDocument = New XmlDocument
                    _xmlDocument.Load(Path)
                End If
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseReaderLock()
                End If
            End Try
        End Sub

        Protected Sub SaveXmlFile()
            SaveXmlFile(CType(Nothing, ReaderWriterLock))
        End Sub

        Protected Sub SaveXmlFile(ByVal lock As ReaderWriterLock)
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                XmlDoc.Save(_XmlFilePath)
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Sub

        Protected Sub CopyNode(ByVal ParentOldNode As XmlElement, ByRef ParentNewNode As XmlElement)
            CopyNodeRec(ParentOldNode, ParentNewNode)
            'OnFolderChanged(New PersistentStorageChangedEventArgs(PersistentStorageChangedEventArgs.Actions.Updated))
        End Sub

        Private Sub CopyNodeRec(ByVal ParentOldNode As XmlElement, ByRef ParentNewNode As XmlElement)
            Try
                'AcquireDocumentLock()
                Dim NewElem As XmlElement
                Dim NewAttr As XmlAttribute
                For Each OldElem As XmlElement In ParentOldNode.ChildNodes
                    NewElem = CreateXmlElement(ParentNewNode, OldElem.Name)
                    For Each OldAttr As XmlAttribute In OldElem.Attributes
                        NewAttr = XmlDoc.CreateAttribute(OldAttr.Name)
                        NewAttr.Value = OldAttr.Value
                        NewElem.Attributes.Append(NewAttr)
                    Next
                    CopyNodeRec(OldElem, NewElem)
                Next
            Finally
                'ReleaseDocumentLock()
            End Try
        End Sub

        Protected Function GetXmlValue(ByVal XPath As String) As String
            Return GetXmlValue(XPath, "value")
        End Function

        Protected Function GetXmlValue(ByVal xPath As String, ByVal lock As System.Threading.ReaderWriterLock) As String
            Return GetXmlValue(xPath, "value", lock)
        End Function

        Protected Function GetXmlValue(ByVal xpath As String, ByVal attributeName As String) As String
            Return GetXmlValue(xpath, attributeName, Nothing)
        End Function

        Protected Function GetXmlValue(ByVal XPath As String, ByVal AttributeName As String, ByVal lock As System.Threading.ReaderWriterLock) As String
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireReaderLock(5000)
                End If
                Dim RVal As String = String.Empty
                Dim Element As XmlElement
                If Not IsNothing(XmlDoc) Then
                    Element = XmlDoc.SelectSingleNode(XPath)
                    If Not IsNothing(Element) AndAlso Not IsNothing(Element.Attributes(AttributeName)) Then
                        RVal = Element.Attributes(AttributeName).Value()
                    End If
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseReaderLock()
                End If
            End Try
        End Function

        Protected Sub SetXmlValue(ByVal XPath As String, ByVal Value As String)
            SetXmlValue(XPath, "value", Value)
        End Sub

        Protected Sub SetXmlValue(ByVal xPath As String, ByVal value As String, ByVal lock As System.Threading.ReaderWriterLock)
            SetXmlValue(xPath, "value", value, lock)
        End Sub

        Protected Sub SetXmlValue(ByVal xpath As String, ByVal attributeName As String, ByVal value As String)
            SetXmlValue(xpath, attributeName, value, CType(Nothing, ReaderWriterLock))
        End Sub

        Protected Sub SetXmlValue(ByVal XPath As String, ByVal AttributeName As String, ByVal Value As String, ByVal lock As System.Threading.ReaderWriterLock)
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim Element As XmlElement
                If Not IsNothing(XmlDoc) Then
                    Element = XmlDoc.SelectSingleNode(XPath)
                    If IsNothing(Element) Then
                        Dim LastSlash As Integer = XPath.LastIndexOf("/")
                        If LastSlash > 0 Then
                            Dim NVCol As New NameValueCollection
                            NVCol.Add(AttributeName, Value)
                            Element = Me.CreateXmlElement(GetSingleXmlElement(XPath.Substring(0, LastSlash)), XPath.Substring(LastSlash + 1), NVCol)
                        End If
                    End If
                    If IsNothing(Element.Attributes(AttributeName)) Then
                        Dim Attribute As XmlAttribute = XmlDoc.CreateAttribute(AttributeName)
                        Attribute.Value = Value
                        Element.Attributes.Append(Attribute)
                    Else
                        Element.Attributes(AttributeName).Value() = Value
                    End If
                    SaveXmlFile()
                End If
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try

        End Sub

        Protected Function GetSingleXmlElement(ByVal xPath As String) As XmlElement
            Return GetSingleXmlElement(xPath, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Function GetSingleXmlElement(ByVal XPath As String, ByVal lock As System.Threading.ReaderWriterLock) As XmlElement
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireReaderLock(5000)
                End If
                Dim RVal As XmlElement = Nothing
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.SelectSingleNode(XPath)
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseReaderLock()
                End If
            End Try
        End Function

        Protected Function GetXmlElements(ByVal xPath As String) As XmlNodeList
            Return GetXmlElements(xPath, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Function GetXmlElements(ByVal XPath As String, ByVal lock As System.Threading.ReaderWriterLock) As XmlNodeList
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireReaderLock(5000)
                End If
                Dim RVal As XmlNodeList = Nothing
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.SelectNodes(XPath)
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseReaderLock()
                End If
            End Try
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        '''     Creates an element without adding it to a parent node
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	3/15/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Function CreateXmlElement(ByVal Name As String, ByVal lock As ReaderWriterLock) As XmlElement
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim RVal As XmlElement = Nothing
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.CreateElement(Name)
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Function

        Protected Function CreateXmlElement(ByVal name As String) As XmlElement
            Return CreateXmlElement(name, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal lock As ReaderWriterLock) As XmlElement
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim RVal As XmlElement = Nothing
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.CreateElement(Name)
                    If Not IsNothing(ParentNode) Then
                        ParentNode.AppendChild(RVal)
                    Else
                        XmlDoc.AppendChild(RVal)
                    End If
                    SaveXmlFile()
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String) As XmlElement
            Return CreateXmlElement(ParentNode, Name, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal AttributeName As String, ByVal AttributeValue As String, ByVal lock As ReaderWriterLock) As XmlElement
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim RVal As XmlElement = Nothing
                Dim Attribute As XmlAttribute
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.CreateElement(Name)
                    Attribute = XmlDoc.CreateAttribute(AttributeName)
                    Attribute.Value = AttributeValue
                    RVal.Attributes.Append(Attribute)
                    If Not IsNothing(ParentNode) Then
                        ParentNode.AppendChild(RVal)
                    Else
                        XmlDoc.AppendChild(RVal)
                    End If
                    SaveXmlFile()
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal AttributeName As String, ByVal AttributeValue As String) As XmlElement
            Return CreateXmlElement(ParentNode, Name, AttributeName, AttributeValue, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal Attributes As NameValueCollection, ByVal lock As ReaderWriterLock) As XmlElement
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim RVal As XmlElement = Nothing
                Dim AttributeName As String
                Dim Attribute As XmlAttribute
                If Not IsNothing(XmlDoc) Then
                    RVal = XmlDoc.CreateElement(Name)
                    If Not IsNothing(Attributes) Then
                        For Each AttributeName In Attributes.Keys
                            Attribute = XmlDoc.CreateAttribute(AttributeName)
                            Attribute.Value = Attributes(AttributeName)
                            RVal.Attributes.Append(Attribute)
                        Next
                    End If
                    If Not IsNothing(ParentNode) Then
                        ParentNode.AppendChild(RVal)
                    Else
                        XmlDoc.AppendChild(RVal)
                    End If
                    SaveXmlFile()
                End If
                Return RVal
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Function

        Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal Attributes As NameValueCollection) As XmlElement
            Return CreateXmlElement(ParentNode, Name, Attributes, CType(Nothing, ReaderWriterLock))
        End Function

        Protected Sub DeleteXmlElement(ByVal Node As XmlElement, ByVal lock As ReaderWriterLock)
            Try
                If Not IsNothing(lock) Then
                    lock.AcquireWriterLock(5000)
                End If
                Dim ParentNode As XmlElement
                If Not IsNothing(Node) Then
                    ParentNode = Node.ParentNode
                    ParentNode.RemoveChild(Node)
                    SaveXmlFile()
                End If
            Finally
                If Not IsNothing(lock) Then
                    lock.ReleaseWriterLock()
                End If
            End Try
        End Sub

        Protected Sub DeleteXmlElement(ByVal Node As XmlElement)
            DeleteXmlElement(Node, CType(Nothing, ReaderWriterLock))
        End Sub
#End Region
        '#Region " Private Instance Methods"
        '        Private Sub AcquireDocumentLock()
        '            While Not DocumentCacheManagement.AquireDocumentLock(_XmlFilePath)
        '                LoadXmlFile()
        '            End While
        '        End Sub

        '        Private Sub ReleaseDocumentLock()
        '            DocumentCacheManagement.ReleaseDocumentLock(_XmlFilePath)
        '        End Sub
        '#End Region
    End Class
End Namespace