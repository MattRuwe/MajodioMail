Namespace Majodio.Common.Schedule
    Public Class ScheduleTimer
        Private _schedules As New ArrayList

        Public Sub Add(ByVal scheduleEvent As ScheduleEvent)
            _schedules.Add(scheduleEvent)
        End Sub
    End Class

End Namespace
