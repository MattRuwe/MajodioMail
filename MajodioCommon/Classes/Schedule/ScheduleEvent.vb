Namespace Majodio.Common.Schedule
    Public MustInherit Class ScheduleEvent
        Public MustOverride Function GetNextScheduledRun() As DateTime

        Private _timer As Majodio.Common.Timer

        Public Event ScheduleElapsed As eventhandler

        Public Sub New()
            LastRunTime = DateTime.MinValue
        End Sub

        Public Sub Start()
            InitializeTimer()
        End Sub

        Public Sub [Stop]()
            _timer.Stop()
        End Sub


        Private Sub InitializeTimer()
            Dim nextScheduledRun As DateTime = GetNextScheduledRun()
            If nextScheduledRun <= DateTime.Now Then
                Throw New InvalidOperationException("The Next Scheduled Run (" & nextScheduledRun.ToString() & ") is in the past")
                _timer.Stop()
            Else
                Dim ticksUntilElapsed As Long

                ticksUntilElapsed = nextScheduledRun.Ticks - DateTime.Now.Ticks
                _timer = New Majodio.Common.Timer(AddressOf Elapsed, ticksUntilElapsed / TimeSpan.TicksPerMillisecond, True)
                _timer.Start()
            End If
            
        End Sub

        Private Sub Elapsed()
            _lastRunTime = DateTime.Now
            RaiseElapsed()
            InitializeTimer()
        End Sub

        Protected Sub RaiseElapsed()
            OnElapsed()
            RaiseEvent ScheduleElapsed(Me, EventArgs.Empty)
        End Sub

        Protected Overridable Sub OnElapsed()

        End Sub

        Private _startTime As TimeSpan
        Public Property StartTime() As TimeSpan
            Get
                Return _startTime
            End Get
            Set(ByVal value As TimeSpan)
                If value.TotalHours >= 24 Then
                    Throw New ArgumentException("Start time must be a timespan of less than 1 day")
                End If
                _startTime = value
            End Set
        End Property

        Private _lastRunTime As DateTime
        Public Property LastRunTime() As DateTime
            Get
                Return _lastRunTime
            End Get
            Set(ByVal value As DateTime)
                _lastRunTime = value
            End Set
        End Property

        Private _tag As Object
        Public Property Tag() As Object
            Get
                Return _tag
            End Get
            Set(ByVal value As Object)
                _tag = value
            End Set
        End Property

    End Class

End Namespace
