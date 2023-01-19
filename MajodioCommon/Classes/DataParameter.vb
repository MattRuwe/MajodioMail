Public Class DataParameter
    Inherits System.Data.Common.DbParameter

    Private _parameterName As String
    Private _value As Object
    Private _isNullable As Boolean = True
    Private _size As Integer = -1
    Private _sourceColumn As String
    Private _dbType As System.Data.DbType
    Private _direction As System.Data.ParameterDirection
    Private _sourceColumnMapping As Boolean = False
    Private _sourceVersion As System.Data.DataRowVersion

    Public Sub New()

    End Sub

    Public Sub New(ByVal name As String, ByVal value As Object)
        _parameterName = name
        _value = value
    End Sub


    Public Overrides Property DbType() As System.Data.DbType
        Get
            Return _dbType
        End Get
        Set(ByVal value As System.Data.DbType)
            _dbType = value
        End Set
    End Property

    Public Overrides Property Direction() As System.Data.ParameterDirection
        Get
            Return _direction
        End Get
        Set(ByVal value As System.Data.ParameterDirection)
            _direction = value
        End Set
    End Property

    Public Overrides Property IsNullable() As Boolean
        Get
            Return _isNullable
        End Get
        Set(ByVal value As Boolean)
            _isNullable = value
        End Set
    End Property

    Public Overrides Property ParameterName() As String
        Get
            Return _parameterName
        End Get
        Set(ByVal value As String)
            _parameterName = value
        End Set
    End Property

    Public Overrides Sub ResetDbType()
        _dbType = Data.DbType.Object
    End Sub

    Public Overrides Property Size() As Integer
        Get
            Return _size
        End Get
        Set(ByVal value As Integer)
            _size = value
        End Set
    End Property

    Public Overrides Property SourceColumn() As String
        Get
            Return _sourceColumn
        End Get
        Set(ByVal value As String)
            _sourceColumn = value
        End Set
    End Property

    Public Overrides Property SourceColumnNullMapping() As Boolean
        Get
            Return _sourceColumnMapping
        End Get
        Set(ByVal value As Boolean)
            _sourceColumnMapping = value
        End Set
    End Property

    Public Overrides Property SourceVersion() As System.Data.DataRowVersion
        Get
            Return _sourceVersion
        End Get
        Set(ByVal value As System.Data.DataRowVersion)
            _sourceVersion = value
        End Set
    End Property

    Public Overrides Property Value() As Object
        Get
            Return _value
        End Get
        Set(ByVal value As Object)
            _value = value
        End Set
    End Property
End Class
