Imports System.Management
Imports System.Management.Instrumentation
Imports System.Reflection

Namespace WmiMessaging
    <Serializable()> _
    Public Delegate Sub WmiMessageArrivedEventHandler(ByVal sender As Object, ByVal e As WmiMessagingEventArgsBase)

    <Serializable()> _
    Public Class WmiMessagingEventListner

        Private _watcher As ManagementEventWatcher
        Private _manuallyStopped As Boolean = False
        Private _failedToStopWatcher As Boolean = False
        Private _serverName As String
        Private _classType As Type
        Private _classInstance As WmiMessagingEventBase

        Public Event WmiMessageArrived As WmiMessageArrivedEventHandler

        Public Sub New()
            Me.New("localhost")
        End Sub

        Public Sub New(ByVal serverName As String)
            Me.New(serverName, GetType(GenericWmiMessagingEvent))
        End Sub

        Public Sub New(ByVal serverName As String, ByVal classType As Type)
            _serverName = serverName
            _classType = classType
        End Sub

        Private Sub Initialize()
            'Determine if _classType derives from WmiBaseEvent (which it must)
            If Not _classType.BaseType Is GetType(WmiMessagingEventBase) Then
                Throw New ArgumentException("ClassType must derive from System.Management.Instrumentation.BaseEvent")
            End If

            _watcher = New ManagementEventWatcher()
            _watcher.Scope.Path.NamespacePath = "Root\MajodioWmiMessaging"
            _watcher.Scope.Path.Server = _serverName
            _watcher.Scope.Path.ClassName = _classType.Name
            _watcher.Query = New EventQuery("WQL", "SELECT * FROM " & _classType.Name)

            _classInstance = Activator.CreateInstance(_classType)

            AddHandler _watcher.EventArrived, AddressOf Watcher_EventArrived
            AddHandler _watcher.Stopped, AddressOf Watcher_Stopped
        End Sub

        Public Sub Start()
            Initialize()
            _watcher.Start()
        End Sub

        Public Sub [Stop]()
            _manuallyStopped = True
            _watcher.Stop()
        End Sub

        Public Property ServerName() As String
            Get
                Return _serverName
            End Get
            Set(ByVal value As String)
                _serverName = value
            End Set
        End Property

        Public Property ClassType() As Type
            Get
                Return _classType
            End Get
            Set(ByVal value As Type)
                _classType = value
            End Set
        End Property

        Private Sub Watcher_EventArrived(ByVal sender As Object, ByVal e As EventArrivedEventArgs)
            Dim eventArgs As WmiMessagingEventArgsBase

            eventArgs = _classInstance.GetEventArgsInstace(e.NewEvent.Properties)

            RaiseEvent WmiMessageArrived(Me, eventArgs)
        End Sub

        Private Sub Watcher_Stopped(ByVal sender As Object, ByVal e As StoppedEventArgs)
            Try
                If Not _manuallyStopped Then
                    If Not _failedToStopWatcher Then
                        Try
                            _watcher.Start()
                        Catch ex As Exception
                            _failedToStopWatcher = True
                            _watcher.Stop()
                        End Try
                    End If
                End If
            Catch ex As Exception

            End Try

        End Sub
    End Class
End Namespace