Imports System.Collections.Specialized

Public Class AdminCommand
    Private _command As String
    Private _password As String
    Private _parameters As NameValueCollection

    Public Property Command() As String
        Get
            Return _command
        End Get
        Set(ByVal value As String)
            _command = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property Parameters() As NameValueCollection
        Get
            Return _parameters
        End Get
        Set(ByVal value As NameValueCollection)
            _parameters = value
        End Set
    End Property
End Class
