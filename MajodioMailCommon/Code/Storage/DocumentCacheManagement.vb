'Imports System.Threading
'Imports System.IO
'Imports System.Xml


'Namespace Storage
'    Public Class DocumentCacheManagement
'#Region " Private Fields"
'        Private Const _CACHE_CLEANUP_INTERVAL As Integer = 15
'        Private Shared _DocumentHash As Hashtable
'        Private Shared _CacheCleanupTimer As System.Timers.Timer
'        'Private Shared _FolderWatcher As FileSystemWatcher
'        Private Shared _FilesChanged As Hashtable
'#End Region
'#Region " Constructor/Destructor"
'        Shared Sub New()
'            InitializeCache()
'        End Sub
'#End Region
'#Region " Cache Management"
'        Private Shared Sub InitializeCache()
'            _DocumentHash = New Hashtable
'            _CacheCleanupTimer = New System.Timers.Timer(30000)
'            _CacheCleanupTimer.AutoReset = False
'            AddHandler _CacheCleanupTimer.Elapsed, AddressOf CacheCleanupTimer_Elapsed
'            _CacheCleanupTimer.Start()
'        End Sub

'        Private Shared Sub CacheCleanupTimer_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
'            CleanupCache()
'            _CacheCleanupTimer.Start()
'        End Sub

'        Public Shared Sub CleanupCache()
'            Dim KeysToRemove As New ArrayList
'            Dim RemovedObject As Object
'            Try
'                If Not Monitor.TryEnter(_DocumentHash, 500) Then
'                    Exit Sub
'                End If
'                For Each Key As String In _DocumentHash.Keys
'                    If DocumentHasChanged(DocumentCache(Key)) OrElse TimeSpan.FromTicks(DateTime.Now.Ticks - DocumentCache(Key).LoadTime.Ticks).TotalSeconds > _CACHE_CLEANUP_INTERVAL Then
'                        If Not Monitor.TryEnter(Document(Key), 500) Then
'                            Exit Sub
'                        End If
'                        KeysToRemove.Add(Key)
'                        Majodio.Common.Utilities.TraceMe("Removing Document " & Key & " from the document caache")
'                    Else
'                        Majodio.Common.Utilities.TraceMe("Retaining Document " & Key & " within the cache")
'                    End If
'                Next
'                For i As Integer = 0 To KeysToRemove.Count - 1
'                    RemovedObject = Document(KeysToRemove(i))
'                    _DocumentHash.Remove(KeysToRemove(i))
'                    Monitor.Exit(RemovedObject)
'                Next
'            Finally
'                Monitor.Exit(_DocumentHash)
'            End Try
'        End Sub

'        Private Shared Function DocumentHasChanged(ByVal Doc As DocumentCache) As Boolean
'            Dim RVal As Boolean = False
'            If Not File.Exists(Doc.FilePath) Then
'                RVal = True
'            ElseIf Doc.FileDate <> File.GetLastWriteTime(Doc.FilePath) Then
'                RVal = True
'            End If
'            Return RVal
'        End Function

'        Public Shared ReadOnly Property Document(ByVal Path As String) As XmlDocument
'            Get
'                Dim TmpRVal As DocumentCache
'                Dim RVal As XmlDocument = Nothing
'                TmpRVal = DocumentCache(Path)
'                If Not IsNothing(TmpRVal) Then
'                    RVal = TmpRVal.Document
'                End If
'                Return RVal
'            End Get
'        End Property

'        Private Shared ReadOnly Property DocumentCache(ByVal Path As String) As DocumentCache
'            Get
'                Try
'                    Monitor.Enter(_DocumentHash)
'                    Dim RVal As DocumentCache = Nothing
'                    If _DocumentHash.ContainsKey(Path.Trim.ToLower) Then
'                        RVal = _DocumentHash(Path.Trim.ToLower)
'                    End If
'                    Return RVal
'                Finally
'                    Monitor.Exit(_DocumentHash)
'                End Try
'            End Get
'        End Property

'        Public Sub Add(ByVal Path As String, ByVal Doc As XmlDocument)
'            Try
'                Monitor.Enter(_DocumentHash)
'                Dim NewDoc As New DocumentCache(Doc, Path)
'                _DocumentHash.Add(Path.Trim.ToLower, NewDoc)
'            Finally
'                Monitor.Exit(_DocumentHash)
'            End Try
'        End Sub

'        Public Shared Function AquireDocumentLock(ByVal Path As String) As Boolean
'            'Trace.WriteLine("Acquiring document lock for path " & Path)
'            Dim RVal As Boolean = True
'            Dim Attempts As Integer = 0
'            Try
'                If Not IsNothing(Document(Path)) Then
'                    'Found the Document in the hash so try to lock it
'                    While Not Monitor.TryEnter(Document(Path), 250) And Attempts < 10
'                        Attempts += 1
'                    End While
'                    If Attempts >= 10 Then
'                        Throw New SynchronizationLockException("Couldn't obtain a lock on the HashLock after " & Attempts & " attempts.")
'                    End If
'                Else
'                    'Couldn't find the document in the hash so return false so the calling routine can load it
'                    RVal = False
'                End If
'            Catch ex As Exception
'                Dim MailEx As New MailException("Could not obtain a DocumentLock", ex)
'                MailEx.ExceptionItems.Add("PARAMETER", "Path", Path)
'                MailEx.ExceptionItems.Add("ITEM", "Attempts", Attempts)
'                MailEx.Save()
'                RVal = False
'            End Try
'            'If RVal Then
'            '    Trace.WriteLine("Document lock established")
'            'Else
'            '    Trace.WriteLine("Could not establish document lock")
'            'End If
'            Return RVal
'        End Function

'        Public Shared Sub ReleaseDocumentLock(ByVal Path As String)
'            Try
'                'Trace.WriteLine("Releasing Document Lock for Path " & Path)
'                Monitor.Exit(Document(Path))
'            Catch ex As Exception
'                Trace.WriteLine("Could not release document lock for path " & Path)
'                Dim MailEx As New MailException("Could not release a DocumentLock", ex)
'                MailEx.ExceptionItems.Add("PARAMETER", "Path", Path)
'                MailEx.Save()
'            End Try
'            'Trace.WriteLine("Successfully released document lock for path " & Path)
'        End Sub

'        Public Shared Sub AquireHashLock()
'            'Trace.WriteLine("Acquiring hash lock")
'            Dim Attempts As Integer = 0
'            Try
'                While Not Monitor.TryEnter(_DocumentHash, 250) And Attempts < 10
'                    Attempts += 1
'                End While
'                If Attempts >= 10 Then
'                    Throw New SynchronizationLockException("Couldn't obtain a lock on the HashLock after " & Attempts & " attempts.")
'                End If
'            Catch ex As Exception
'                Trace.WriteLine("Could not obtain hash lock")
'                Dim MailEx As New MailException("Could not obtain a HashLock", ex)
'                MailEx.SaveAndThrow()
'            End Try
'            'Trace.WriteLine("Successfully obtained hash lock")
'        End Sub

'        Public Shared Sub ReleaseHashLock()
'            Try
'                'Trace.WriteLine("Releaseing hash lock")
'                Monitor.Exit(_DocumentHash)
'            Catch ex As Exception
'                Trace.WriteLine("Could not release hash lock")
'                Dim MailEx As New MailException("Could not release a HashLock", ex)
'                Throw MailEx
'            End Try
'            'Trace.WriteLine("Successfully released hash lock")
'        End Sub
'#End Region
'    End Class
'End Namespace