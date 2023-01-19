Imports System.Xml
Imports System.IO

Namespace Storage
    Public Class DocumentCache
        Private _Document As XmlDocument
        Private _LoadTime As DateTime
        Private _LastUpdateTime As DateTime
        Private _FilePath As String
        Private _FileDate As DateTime
        Private _FileSize As Int64

        Public Sub New(ByVal Document As XmlDocument, ByVal FilePath As String)
            _Document = Document
            _LoadTime = DateTime.Now
            _LastUpdateTime = DateTime.Now
            _FilePath = FilePath
            If File.Exists(FilePath) Then
                _FileDate = File.GetLastWriteTime(FilePath)
                _FileSize = FileLen(FilePath)
            End If
        End Sub

        Public ReadOnly Property Document() As XmlDocument
            Get
                _LastUpdateTime = DateTime.Now
                Return _Document
            End Get
        End Property

        Public ReadOnly Property LoadTime() As DateTime
            Get
                Return _LoadTime
            End Get
        End Property

        Public ReadOnly Property LastUpdateTime() As DateTime
            Get
                Return _LastUpdateTime
            End Get
        End Property

        Public ReadOnly Property FilePath() As String
            Get
                Return _FilePath
            End Get
        End Property

        Public ReadOnly Property FileDate() As DateTime
            Get
                Return _FileDate
            End Get
        End Property

        Public ReadOnly Property FileSize() As Int64
            Get
                Return _FileSize
            End Get
        End Property
    End Class
End Namespace

