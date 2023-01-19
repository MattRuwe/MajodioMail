Namespace Majodio.Common.Schedule
    Public Class MonthlySchedule
        Inherits ScheduleEvent

        Private _monthsToRun(11) As Boolean
        Private _monthHistory(11) As Date

        Public Sub New()
            For i As Integer = 0 To 11
                _monthsToRun(i) = False
                _monthHistory(i) = Date.MinValue
            Next
        End Sub

        Public Overrides Function GetNextScheduledRun() As Date
            Dim rVal As Date = Date.MinValue

            'Start by determining the start date
            If MyBase.LastRunTime > Date.MinValue Then
                rVal = LastRunTime.Date.AddDays(1)
            Else
                rVal = Date.Today
            End If

            If _ScheduleType = MonthlyScheduleType.AbsoluteDay Then
                'Determine the next month for the schedule
                'Use DayOfMonthToRun

                'Next determine which day of the month it should run on
                While rVal.Day <> DayOfMonthToRun Or Not _monthsToRun(rVal.Month - 1)
                    rVal.AddDays(1)
                End While
            ElseIf _ScheduleType = MonthlyScheduleType.RelativeDay Then
                'Determine the next day for the schedule
                Dim currentDate As Date = rVal
                Do
                    If GetRelativeDay(rVal) = RelativeDayToRun And rVal.DayOfWeek = DayOfWeekToRun Then
                        Exit Do
                    Else
                        rVal = rVal.AddDays(1)
                    End If
                Loop
            End If

            'Set the time of day to run
            rVal = rVal.AddTicks(MyBase.StartTime.Ticks)

            Return rVal
        End Function

        Public Sub SetMonthToRun(ByVal month As Integer, ByVal run As Boolean)
            _monthsToRun(month - 1) = run
        End Sub

        Private Function GetRelativeDay(ByVal runDate As Date) As RelativeDayOfMonth
            Dim rVal As RelativeDayOfMonth = RelativeDayOfMonth.First

            If runDate.Day <= 7 Then
                rVal = RelativeDayOfMonth.First
            ElseIf runDate.Day <= 14 Then
                rVal = RelativeDayOfMonth.Second
            ElseIf runDate.Day <= 21 Then
                rVal = RelativeDayOfMonth.Third
            ElseIf runDate.Day <= 28 Then
                rVal = RelativeDayOfMonth.Fourth
                'TODO Also need to determine if this is the last day in the month
                Dim lastDay As Date = GetLastDayOfMonth(runDate)
                If runDate.Day > lastDay.AddDays(-7).Day Then
                    rVal = RelativeDayOfMonth.FourthAndLast
                End If
            ElseIf runDate.Day > 28 Then
                rVal = RelativeDayOfMonth.Last
            End If

            Return rVal
        End Function

        Private Function GetLastDayOfMonth(ByVal runDate As Date) As Date
            Dim rVal As Date = runDate
            Dim currentDate As Date = runDate.AddDays(1)

            While rVal.Day <= currentDate.Day
                rVal = currentDate
                currentDate = currentDate.AddDays(1)
            End While

            Return rVal
        End Function

        Private _dayOfMonthToRun As Integer
        Public Property DayOfMonthToRun() As Integer
            Get
                Return _dayOfMonthToRun
            End Get
            Set(ByVal value As Integer)
                If value < 1 Or value > 31 Then
                    Throw New ArgumentException("The value for DayOfMonthToRun (" & value & ") cannot be less than 1 or greater than 31")
                End If
                _dayOfMonthToRun = value
            End Set
        End Property

        Private _relativeDayToRun As RelativeDayOfMonth
        Public Property RelativeDayToRun() As RelativeDayOfMonth
            Get
                Return _relativeDayToRun
            End Get
            Set(ByVal value As RelativeDayOfMonth)
                _relativeDayToRun = value
            End Set
        End Property

        Private _ScheduleType As MonthlyScheduleType
        Public Property ScheduleType() As MonthlyScheduleType
            Get
                Return _ScheduleType
            End Get
            Set(ByVal value As MonthlyScheduleType)
                _ScheduleType = value
            End Set
        End Property

        Private _dayOfWeekToRun As DayOfWeek
        Public Property DayOfWeekToRun() As DayOfWeek
            Get
                Return _dayOfWeekToRun
            End Get
            Set(ByVal value As DayOfWeek)
                _dayOfWeekToRun = value
            End Set
        End Property


        Public Enum MonthlyScheduleType
            AbsoluteDay
            RelativeDay
        End Enum

        Public Enum RelativeDayOfMonth
            First
            Second
            Third
            Fourth
            Last
            FourthAndLast
        End Enum
    End Class

End Namespace
