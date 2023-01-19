Namespace Majodio.Common.Schedule
    Public Class OnceSchedule
        Inherits ScheduleEvent

        Public Overrides Function GetNextScheduledRun() As Date
            Return _startDate.Add(StartTime)
        End Function


        Private _startDate As Date
        Public Property StartDate() As Date
            Get
                Return _startDate
            End Get
            Set(ByVal value As Date)
                _startDate = value
            End Set
        End Property

    End Class
End Namespace

