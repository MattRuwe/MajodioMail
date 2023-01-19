Imports System.Diagnostics

Public Class Performance
#Region " Install/Uninstall Counters"
    Public Shared Sub InstallCounters()
        If Not PerformanceCounterCategory.Exists(PERF_COUNTER_CATEGORY_NAME) Then
            PerformanceCounterCategory.Create(PERF_COUNTER_CATEGORY_NAME, "Contains statistics about the current status of Majodio Mail", PerformanceCounterCategoryType.SingleInstance, GetPerformanceCounterCollection)
        End If
    End Sub

    Public Shared Sub UnInstallCounters()
        Try
            If PerformanceCounterCategory.Exists(PERF_COUNTER_CATEGORY_NAME) Then
                PerformanceCounterCategory.Delete(PERF_COUNTER_CATEGORY_NAME)
            End If
        Catch ex1 As Exception
        End Try
    End Sub

    Private Shared Function GetPerformanceCounterCollection() As CounterCreationDataCollection
        Dim CCD As CounterCreationData
        Dim RVal As New CounterCreationDataCollection

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_CURRENT_SMTP_SESSIONS
        CCD.CounterHelp = "The current number of sessions open to the SMTP server"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_TIMED_OUT_SMTP_SESSIONS
        CCD.CounterHelp = "The total number of SMTP sessions that have timed out since the server was last restarted"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_TOTAL_SMTP_SESSIONS
        CCD.CounterHelp = "The total number of sessions that have been opened to the SMTP server since the server was last restarted"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_MESSAGES_RECEIVED
        CCD.CounterHelp = "The total number of messages to have been received on all of the SMTP sessions."
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)
        '
        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_CONNECTIONS_REJECTED_DNSBL
        CCD.CounterHelp = "The total number of connections that have been reject due to DNSBL violations."
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_MESSAGES_REJECTED_DNSBL
        CCD.CounterHelp = "The total number of messages that have been reject due to DNSBL violations."
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_MESSAGES_REJECTED_INVALID_FROM
        CCD.CounterHelp = "The total number of message that have been rejected due to an e-mail address that does not match the incoming IP address."
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_CURRENT_POP3_SESSIONS
        CCD.CounterHelp = "The current number of sessions open to the POP3 server"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_TIMED_OUT_POP3_SESSIONS
        CCD.CounterHelp = "The total number of POP3 sessions that have timed out since the server was last restarted"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_TOTAL_POP3_SESSIONS
        CCD.CounterHelp = "The total number of sessions that have been opened to the POP3 server since the server was last restarted"
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        CCD = New CounterCreationData
        CCD.CounterName = PERF_COUNTER_MESSAGES_DELIVERED
        CCD.CounterHelp = "The total number of messages to have been delivered on all of the POP3 sessions."
        CCD.CounterType = PerformanceCounterType.NumberOfItems32
        RVal.Add(CCD)

        Return RVal
    End Function
#End Region
#Region " Counter Value"
    Private Shared Function GetCounter(ByVal CounterName As String) As PerformanceCounter
        If Not PerformanceCounterCategory.Exists(PERF_COUNTER_CATEGORY_NAME) OrElse Not PerformanceCounterCategory.CounterExists(CounterName, PERF_COUNTER_CATEGORY_NAME) Then
            UnInstallCounters()
            InstallCounters()
        End If
        Dim RVal As New PerformanceCounter
        RVal.CategoryName = PERF_COUNTER_CATEGORY_NAME
        RVal.CounterName = CounterName
        RVal.ReadOnly = False
        Return RVal
    End Function
#Region " Current SMTP Sessions"
    Public Shared ReadOnly Property CurrentSmtpSessions() As Integer
        Get

            Return GetCounter(PERF_COUNTER_CURRENT_SMTP_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementCurrentSmtpSessions()
        GetCounter(PERF_COUNTER_CURRENT_SMTP_SESSIONS).Increment()
    End Sub

    Public Shared Sub DecrementCurrentSmtpSessions()
        If CurrentSmtpSessions > 0 Then
            GetCounter(PERF_COUNTER_CURRENT_SMTP_SESSIONS).Decrement()
        Else
            GetCounter(PERF_COUNTER_CURRENT_SMTP_SESSIONS).RawValue = 0
        End If
    End Sub
#End Region
#Region " Timed-out SMTP Sessions"
    Public Shared ReadOnly Property TimedoutSmtpSessions() As Integer
        Get
            Return GetCounter(PERF_COUNTER_TIMED_OUT_SMTP_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementTimedoutSmtpSessions()
        GetCounter(PERF_COUNTER_TIMED_OUT_SMTP_SESSIONS).Increment()
    End Sub
#End Region
#Region " Total SMTP Sessions"
    Public Shared ReadOnly Property TotalSmtpSessions() As Integer
        Get
            Return GetCounter(PERF_COUNTER_TOTAL_SMTP_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementTotalSmtpSessions()
        GetCounter(PERF_COUNTER_TOTAL_SMTP_SESSIONS).Increment()
    End Sub
#End Region
#Region " Messages Received"
    Public Shared ReadOnly Property MessagesReceived() As Integer
        Get
            Return GetCounter(PERF_COUNTER_MESSAGES_RECEIVED).NextValue
        End Get
    End Property

    Public Shared Sub IncrementMessagesReceived()
        GetCounter(PERF_COUNTER_MESSAGES_RECEIVED).Increment()
    End Sub
#End Region
#Region " Messages Rejected DNSBL"
    Public Shared ReadOnly Property ConnectionsRejectedDnsbl() As Integer
        Get
            Return GetCounter(PERF_COUNTER_CONNECTIONS_REJECTED_DNSBL).NextValue
        End Get
    End Property

    Public Shared Sub IncrementConnectionsRejectedDnsbl()
        GetCounter(PERF_COUNTER_CONNECTIONS_REJECTED_DNSBL).Increment()
    End Sub
#End Region
#Region " Messages Rejected DNSBL"
    Public Shared ReadOnly Property MessagesRejectedDnsbl() As Integer
        Get
            Return GetCounter(PERF_COUNTER_MESSAGES_REJECTED_DNSBL).NextValue
        End Get
    End Property

    Public Shared Sub IncrementMessagesRejectedDnsbl()
        GetCounter(PERF_COUNTER_MESSAGES_REJECTED_DNSBL).Increment()
    End Sub
#End Region
#Region " Messages Rejected Invalid From Address"
    Public Shared ReadOnly Property MessagesRejectedInvalidFrom() As Integer
        Get
            Return GetCounter(PERF_COUNTER_MESSAGES_REJECTED_INVALID_FROM).NextValue
        End Get
    End Property

    Public Shared Sub IncrementMessagesRejectedInvalidFrom()
        GetCounter(PERF_COUNTER_MESSAGES_REJECTED_INVALID_FROM).Increment()
    End Sub
#End Region
#Region " Current POP3 Sessions"
    Public Shared ReadOnly Property CurrentPop3Sessions() As Integer
        Get
            Return GetCounter(PERF_COUNTER_CURRENT_POP3_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementCurrentPop3Sessions()
        GetCounter(PERF_COUNTER_CURRENT_POP3_SESSIONS).Increment()
    End Sub

    Public Shared Sub DecrementCurrentPop3Sessions()
        If CurrentPop3Sessions > 0 Then
            GetCounter(PERF_COUNTER_CURRENT_POP3_SESSIONS).Decrement()
        Else
            GetCounter(PERF_COUNTER_CURRENT_POP3_SESSIONS).RawValue = 0
        End If
    End Sub
#End Region
#Region " Timed-out POP3 Sessions"
    Public Shared ReadOnly Property TimedoutPop3Sessions() As Integer
        Get
            Return GetCounter(PERF_COUNTER_TIMED_OUT_POP3_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementTimedoutPop3Sessions()
        GetCounter(PERF_COUNTER_TIMED_OUT_POP3_SESSIONS).Increment()
    End Sub
#End Region
#Region " Total POP3 Sessions"
    Public Shared ReadOnly Property TotalPop3Sessions() As Integer
        Get
            Return GetCounter(PERF_COUNTER_TOTAL_POP3_SESSIONS).NextValue
        End Get
    End Property

    Public Shared Sub IncrementTotalPop3Sessions()
        GetCounter(PERF_COUNTER_TOTAL_POP3_SESSIONS).Increment()
    End Sub
#End Region
#Region " Messages Delivered"
    Public Shared ReadOnly Property MessagesDelivered() As Integer
        Get
            Return GetCounter(PERF_COUNTER_MESSAGES_DELIVERED).NextValue
        End Get
    End Property

    Public Shared Sub IncrementMessagesDelivered()
        GetCounter(PERF_COUNTER_MESSAGES_DELIVERED).Increment()
    End Sub
#End Region
#End Region
End Class

