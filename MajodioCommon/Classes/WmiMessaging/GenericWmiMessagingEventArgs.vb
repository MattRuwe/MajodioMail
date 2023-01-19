Namespace WmiMessaging
    <Serializable()> _
    Public Class GenericWmiMessagingEventArgs
        Inherits WmiMessagingEventArgsBase


        Private _application As String
        Private _category As String
        Private _message As String
        Private _system As String

        Public Sub New(ByVal application As String, ByVal category As String, ByVal message As String, ByVal system As String)
            _application = application
            _category = category
            _message = message
            _system = system
        End Sub

        Public Property Application() As String
            Get
                Return _application
            End Get
            Set(ByVal value As String)
                _application = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return _category
            End Get
            Set(ByVal value As String)
                _category = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        Public Property System() As String
            Get
                Return _system
            End Get
            Set(ByVal value As String)
                _system = value
            End Set
        End Property
    End Class
End Namespace

