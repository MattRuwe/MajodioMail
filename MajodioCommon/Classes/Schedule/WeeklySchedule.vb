Namespace Majodio.Common.Schedule
    Public Class WeeklySchedule
        Inherits ScheduleEvent

        Private _daysToRun(6) As Boolean
        Private _dayHistory(6) As Date

        Public Sub New()
            _weeksBetweenRun = 1
            For i As Integer = 0 To 6
                _dayHistory(i) = Date.MinValue
            Next
        End Sub

        Public Overrides Function GetNextScheduledRun() As Date
            Dim rVal As DateTime
            Dim currentDayOfWeek As DayOfWeek = Date.Today.DayOfWeek
            Dim searchDate As Date

            Verify()

            'Should we start looking from today, or has it already run?
            If MyBase.LastRunTime = DateTime.MinValue AndAlso DateTime.Now < New DateTime(DateTime.Today.Ticks + StartTime.Ticks) Then
                searchDate = DateTime.Today
            Else
                searchDate = DateTime.Today.AddDays(1)
            End If

            'Find the day of the week that is scheduled next
            While Not IsDayScheduledToRun(searchDate.DayOfWeek)
                searchDate = searchDate.AddDays(1)
            End While

            rVal = searchDate

            'Determine if we should skip this date due to WeeksBetweenRun
            If _dayHistory(Me.GetDayOfWeekIndex(rVal)) > Date.MinValue Then
                rVal = _dayHistory(GetDayOfWeekIndex(rVal)).AddDays(WeeksBetweenRun * 7)
            End If

            rVal = rVal.AddTicks(StartTime.Ticks)

            Return rVal
        End Function

        Private Sub Verify()
            Dim verified As Boolean = False
            For i As Integer = 0 To 6
                If _daysToRun(i) = True Then
                    verified = True
                End If
            Next

            If Not verified Then
                Throw New InvalidOperationException("No days where specified to run")
            End If
        End Sub


        Protected Overrides Sub OnElapsed()
            Select Case Date.Today.DayOfWeek
                Case DayOfWeek.Sunday
                    _dayHistory(0) = DateTime.Today
                Case DayOfWeek.Monday
                    _dayHistory(1) = DateTime.Today
                Case DayOfWeek.Tuesday
                    _dayHistory(2) = DateTime.Today
                Case DayOfWeek.Wednesday
                    _dayHistory(3) = DateTime.Today
                Case DayOfWeek.Thursday
                    _dayHistory(4) = DateTime.Today
                Case DayOfWeek.Friday
                    _dayHistory(5) = DateTime.Today
                Case DayOfWeek.Saturday
                    _dayHistory(6) = DateTime.Today
            End Select
        End Sub
        Public Sub SetDayOfWeek(ByVal dayOfWeek As System.DayOfWeek, ByVal run As Boolean)
            Select Case dayOfWeek
                Case System.DayOfWeek.Sunday
                    _daysToRun(0) = run
                Case System.DayOfWeek.Monday
                    _daysToRun(1) = run
                Case System.DayOfWeek.Tuesday
                    _daysToRun(2) = run
                Case System.DayOfWeek.Wednesday
                    _daysToRun(3) = run
                Case System.DayOfWeek.Thursday
                    _daysToRun(4) = run
                Case System.DayOfWeek.Friday
                    _daysToRun(5) = run
                Case System.DayOfWeek.Saturday
                    _daysToRun(6) = run
            End Select
        End Sub

        Private Function GetDayOfWeekIndex(ByVal runDate As Date) As Integer
            Dim rVal As Integer = -1
            Select Case runDate.DayOfWeek
                Case DayOfWeek.Sunday
                    rVal = 0
                Case DayOfWeek.Monday
                    rVal = 1
                Case DayOfWeek.Tuesday
                    rVal = 2
                Case DayOfWeek.Wednesday
                    rVal = 3
                Case DayOfWeek.Thursday
                    rVal = 4
                Case DayOfWeek.Friday
                    rVal = 5
                Case DayOfWeek.Saturday
                    rVal = 6
            End Select
            Return rVal
        End Function

        Private Function IsDayScheduledToRun(ByVal day As DayOfWeek) As Boolean
            Dim rVal As Boolean

            Select Case day
                Case DayOfWeek.Sunday
                    rVal = _daysToRun(0)
                Case DayOfWeek.Monday
                    rVal = _daysToRun(1)
                Case DayOfWeek.Tuesday
                    rVal = _daysToRun(2)
                Case DayOfWeek.Wednesday
                    rVal = _daysToRun(3)
                Case DayOfWeek.Thursday
                    rVal = _daysToRun(4)
                Case DayOfWeek.Friday
                    rVal = _daysToRun(5)
                Case DayOfWeek.Saturday
                    rVal = _daysToRun(6)
            End Select

            Return rVal
        End Function

        Private _weeksBetweenRun As Integer
        Public Property WeeksBetweenRun() As Integer
            Get
                Return _weeksBetweenRun
            End Get
            Set(ByVal value As Integer)
                If value < 1 Then
                    Throw New ArgumentException("Value for WeeksBetweenRun must be greater than or equal to 1")
                End If
                _weeksBetweenRun = value
            End Set
        End Property
    End Class

End Namespace
