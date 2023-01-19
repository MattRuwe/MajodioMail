'Imports System.xml
'Imports System.Threading
'Imports System.IO
'Imports System.Collections.Specialized

'Public MustInherit Class Xml
'    Private Shared _XmlHash As System.Collections.Hashtable
'    Private _XmlFilePath As String
'    Private FSW As FileSystemWatcher

'    Public MustOverride Function GetDefaultXml() As XmlDocument

'    Shared Sub New()
'        _XmlHash = New System.Collections.Hashtable
'    End Sub

'    Public Sub New(ByVal XmlFilePath As String)
'        Dim FolderPath As String
'        Dim LastSlashIndex As Integer
'        LastSlashIndex = XmlFilePath.LastIndexOf("\")
'        If LastSlashIndex > -1 Then
'            FolderPath = XmlFilePath.Substring(0, LastSlashIndex)
'            FSW = New FileSystemWatcher(FolderPath)
'            AddHandler FSW.Changed, AddressOf OnChanged
'        End If
'        _XmlFilePath = XmlFilePath
'        VerifyXmlFileExists()
'        LoadXmlFile()
'    End Sub

'    Private Sub OnChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)
'        Dim M As Mutex = LockDoc(XmlFilePath)
'        If Not IsNothing(XmlDoc) Then
'            XmlDoc = Nothing
'            VerifyXmlFileExists()
'            LoadXmlFile()
'        End If
'        UnlockDoc(M)
'    End Sub

'    'Private Sub OnRenamed(ByVal source As Object, ByVal e As RenamedEventArgs)
'    '    VerifyXmlFileExists()
'    '    LoadXmlFile()
'    'End Sub

'    Private Property XmlDoc() As XmlDocument
'        Get
'            Return CType(_XmlHash(_XmlFilePath), XmlDocument)
'        End Get
'        Set(ByVal Value As XmlDocument)
'            _XmlHash(_XmlFilePath) = Value
'        End Set
'    End Property

'    Public ReadOnly Property XmlFilePath() As String
'        Get
'            Return _XmlFilePath
'        End Get
'    End Property

'    Private Shared rwLocked As ReaderWriterLock
'    Private Shared M As Mutex
'    Public Sub VerifyXmlFileExists()

'        Try
'            If IsNothing(M) Then
'                M = New Mutex(False, "VerifyXmlFile")
'            End If
'            M.WaitOne()
'        Catch exc As Exception
'            If IsNothing(rwLocked) Then
'                rwLocked = New ReaderWriterLock
'            End If
'            rwLocked.AcquireWriterLock(Timeout.Infinite)
'        End Try
'        Try
'            Dim Directories As String()
'            Dim CurrentDirectory As String
'            Dim i As Integer
'            'Dim XmlDoc As XmlDocument
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
'                SaveXmlFile()
'            End If
'        Finally
'            If Not IsNothing(rwLocked) AndAlso rwLocked.IsWriterLockHeld() Then
'                rwLocked.ReleaseWriterLock()
'            End If
'            If Not IsNothing(M) Then
'                Try
'                    M.ReleaseMutex()
'                Catch ex As Exception

'                End Try
'            End If
'        End Try
'    End Sub

'    Protected Sub LoadXmlFile()
'        LoadXmlFile(_XmlFilePath)
'    End Sub

'    Protected Sub LoadXmlFile(ByVal Path As String)
'        Dim XmlDoc As XmlDocument
'        _XmlFilePath = Path
'        If IsNothing(XmlDoc) Then
'            Dim M As Mutex = LockDoc(Me.XmlFilePath)
'            XmlDoc = New XmlDocument
'            XmlDoc.Load(Path)
'            Me.XmlDoc = XmlDoc
'            UnlockDoc(M)
'        End If
'    End Sub

'    Protected Sub SaveXmlFile()
'        FSW.EnableRaisingEvents = False
'        XmlDoc.Save(_XmlFilePath)
'        FSW.EnableRaisingEvents = True
'    End Sub

'    Protected Function GetXmlValue(ByVal XPath As String) As String
'        Return GetXmlValue(XPath, "value")
'    End Function

'    Protected Function GetXmlValue(ByVal XPath As String, ByVal AttributeName As String) As String
'        Dim RVal As String = String.Empty
'        Dim Element As XmlElement
'        If Not IsNothing(XmlDoc) Then
'            Element = XmlDoc.SelectSingleNode(XPath)
'            If Not IsNothing(Element) Then
'                RVal = Element.Attributes(AttributeName).Value()
'            End If
'        End If
'        Return RVal
'    End Function

'    Protected Function SetXmlValue(ByVal XPath As String, ByVal Value As String) As String
'        SetXmlValue(XPath, "value", Value)
'    End Function

'    Protected Function SetXmlValue(ByVal XPath As String, ByVal AttributeName As String, ByVal Value As String) As String
'        Dim Element As XmlElement
'        Dim M As Mutex = LockDoc(Me.XmlFilePath)
'        If Not IsNothing(XmlDoc) Then
'            Element = XmlDoc.SelectSingleNode(XPath)
'            If IsNothing(Element) Then
'                Dim LastSlash As Integer = XPath.LastIndexOf("/")
'                If LastSlash > 0 Then
'                    Dim NVCol As New NameValueCollection
'                    NVCol.Add(AttributeName, Value)
'                    Element = Me.CreateXmlElement(GetSingleXmlElement(XPath.Substring(0, LastSlash)), XPath.Substring(LastSlash + 1), NVCol)
'                End If
'            End If
'            Element.Attributes(AttributeName).Value() = Value
'            SaveXmlFile()
'        End If
'        UnlockDoc(M)
'    End Function

'    Protected Function GetSingleXmlElement(ByVal XPath As String) As XmlElement
'        Dim RVal As XmlElement
'        If Not IsNothing(XmlDoc) Then
'            RVal = XmlDoc.SelectSingleNode(XPath)
'        End If
'        Return RVal
'    End Function

'    Protected Function GetXmlElements(ByVal XPath As String) As XmlNodeList
'        Dim RVal As XmlNodeList
'        If Not IsNothing(XmlDoc) Then
'            RVal = XmlDoc.SelectNodes(XPath)
'        End If
'        Return RVal
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String) As XmlElement
'        Dim RVal As XmlElement
'        Dim Attribute As XmlAttribute
'        Dim M As Mutex = LockDoc(Me.XmlFilePath)
'        If Not IsNothing(XmlDoc) Then
'            RVal = XmlDoc.CreateElement(Name)
'            If Not IsNothing(ParentNode) Then
'                ParentNode.AppendChild(RVal)
'            Else
'                XmlDoc.AppendChild(RVal)
'            End If
'            SaveXmlFile()
'        End If
'        UnlockDoc(M)
'        Return RVal
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal AttributeName As String, ByVal AttributeValue As String) As XmlElement
'        Dim RVal As XmlElement
'        Dim Attribute As XmlAttribute
'        Dim M As Mutex = LockDoc(Me.XmlFilePath)
'        If Not IsNothing(XmlDoc) Then
'            RVal = XmlDoc.CreateElement(Name)
'            Attribute = XmlDoc.CreateAttribute(AttributeName)
'            Attribute.Value = AttributeValue
'            RVal.Attributes.Append(Attribute)
'            If Not IsNothing(ParentNode) Then
'                ParentNode.AppendChild(RVal)
'            Else
'                XmlDoc.AppendChild(RVal)
'            End If
'            SaveXmlFile()
'        End If
'        UnlockDoc(M)
'        Return RVal
'    End Function

'    Protected Function CreateXmlElement(ByVal ParentNode As XmlElement, ByVal Name As String, ByVal Attributes As NameValueCollection) As XmlElement
'        Dim RVal As XmlElement
'        Dim AttributeName As String
'        Dim Attribute As XmlAttribute
'        Dim M As Mutex = LockDoc(Me.XmlFilePath)
'        If Not IsNothing(XmlDoc) Then
'            RVal = XmlDoc.CreateElement(Name)
'            If Not IsNothing(Attributes) Then
'                For Each AttributeName In Attributes.Keys
'                    Attribute = XmlDoc.CreateAttribute(AttributeName)
'                    Attribute.Value = Attributes(AttributeName)
'                    RVal.Attributes.Append(Attribute)
'                Next
'            End If
'            If Not IsNothing(ParentNode) Then
'                ParentNode.AppendChild(RVal)
'            Else
'                XmlDoc.AppendChild(RVal)
'            End If
'            SaveXmlFile()
'        End If
'        UnlockDoc(M)
'        Return RVal
'    End Function

'    Protected Sub DeleteXmlElement(ByVal Node As XmlElement)
'        Dim ParentNode As XmlElement
'        Dim M As Mutex = LockDoc(Me.XmlFilePath)
'        If Not IsNothing(Node) Then
'            ParentNode = Node.ParentNode
'            ParentNode.RemoveChild(Node)
'            SaveXmlFile()
'        End If
'        UnlockDoc(M)
'    End Sub

'    Private Function LockDoc(ByVal Path As String) As Mutex
'        Dim M As New Mutex(False, AppDomain.GetCurrentThreadId & Path.Replace("\", String.Empty))
'        M.WaitOne()
'        Return M
'    End Function

'    Private Sub UnlockDoc(ByVal M As Mutex)
'        M.ReleaseMutex()
'    End Sub
'End Class
