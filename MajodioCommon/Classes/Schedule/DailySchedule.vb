Namespace Majodio.Common.Schedule
    Public Class DailySchedule
        Inherits ScheduleEvent

        Public Sub New()
            _daysBetweenRun = 1
        End Sub

        Public Overrides Function GetNextScheduledRun() As DateTime
            Dim rVal As DateTime
            If (MyBase.LastRunTime.Ticks < DateTime.Now.Ticks And LastRunTime <> DateTime.MinValue) Or (LastRunTime = DateTime.MinValue And StartTime.Ticks < DateTime.Now.TimeOfDay.Ticks) Then
                rVal = DateTime.Today.AddDays(_daysBetweenRun)
            Else
                rVal = DateTime.Today
            End If

            rVal = rVal.Add(MyBase.StartTime)

            Return rVal
        End Function

        Private _daysBetweenRun As Integer
        Public Property DaysBetweenRun() As Integer
            Get
                Return _daysBetweenRun
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Throw New ArgumentException("Value for WeeksBetweenRun must be greater than or equal to 1")
                End If
                _daysBetweenRun = value
            End Set
        End Property

    End Class
End Namespace
