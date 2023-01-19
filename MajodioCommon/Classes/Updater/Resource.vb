Imports System

Namespace Majodio.Common.Assembly.Updater
    <Serializable()> _
    Public Class Resource
        ' Methods
        Public Sub New()
        End Sub


        ' Properties
        Public Property AddedAtRuntime() As Boolean
            Get
                Return Me._AddedAtRuntime
            End Get
            Set(ByVal value As Boolean)
                Me._AddedAtRuntime = value
            End Set
        End Property

        Public Property FilePath() As String
            Get
                Return Me._FilePath
            End Get
            Set(ByVal value As String)
                Me._FilePath = value
            End Set
        End Property

        Public Property IsFolder() As Boolean
            Get
                Return Me._IsFolder
            End Get
            Set(ByVal value As Boolean)
                Me._IsFolder = value
            End Set
        End Property

        Public Property LastModified() As DateTime
            Get
                Return Me._LastModified
            End Get
            Set(ByVal value As DateTime)
                Me._LastModified = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return Me._Url
            End Get
            Set(ByVal value As String)
                Me._Url = value
            End Set
        End Property


        ' Fields
        Private _AddedAtRuntime As Boolean
        Private _FilePath As String
        Private _IsFolder As Boolean
        Private _LastModified As DateTime
        Private _Name As String
        Private _Url As String
    End Class
End Namespace


