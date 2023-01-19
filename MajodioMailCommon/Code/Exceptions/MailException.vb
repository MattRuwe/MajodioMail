

Public Class MailException
    Inherits Exception

    Private _MethodName As String
    Private _Parameters() As System.Reflection.ParameterInfo
    Private _AssemblyName As String
    Private _ExceptionItems As ExceptionItemCollection
    Private _CorrectiveAction As String

    Public Sub New(ByVal message As String)
        Me.New(message, Nothing, 1)
    End Sub

    Public Sub New(ByVal message As String, ByVal skipFrames As Integer)
        Me.New(message, Nothing, skipFrames)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        Me.New(message, innerException, 1)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception, ByVal skipFrames As Integer)
        MyBase.New(message, innerException)
        Dim SF As New StackFrame(skipFrames)
        _MethodName = SF.GetMethod.Name
        _Parameters = SF.GetMethod.GetParameters
        _ExceptionItems = New ExceptionItemCollection()
    End Sub

    Public ReadOnly Property ExceptionItems() As ExceptionItemCollection
        Get
            Return _ExceptionItems
        End Get
    End Property

    Public ReadOnly Property MethodName() As String
        Get
            Return _MethodName
        End Get
    End Property

    Public ReadOnly Property Parameters() As System.Reflection.ParameterInfo()
        Get
            Return _Parameters
        End Get
    End Property

    Public Property CorrectiveAction() As String
        Get
            Return _CorrectiveAction
        End Get
        Set(ByVal value As String)
            _CorrectiveAction = value
        End Set
    End Property

    Public Sub Save()
        ExceptionClient.Instance.Save(Me)
    End Sub

    Public Sub SaveAndThrow()
        ExceptionClient.Instance.SaveAndThrow(Me)
    End Sub
End Class