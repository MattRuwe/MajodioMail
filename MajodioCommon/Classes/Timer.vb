Imports System.Threading


Namespace Majodio.Common
    Public Delegate Sub TimerElapsed()

    Public Class Timer
        Private _elapsed As TimerElapsed
        Private _interval As Long
        Private _running As Boolean
        Private _syncLock As New Object()
        Private _timerThread As Thread
        Private _runOnce As Boolean = False

        Public Sub New(ByVal elapsed As TimerElapsed, ByVal interval As Long)
            _elapsed = elapsed
            _interval = interval
            _running = False
        End Sub

        Public Sub New(ByVal elapsed As TimerElapsed)
            Me.New(elapsed, 1000)
        End Sub

        Public Sub New(ByVal elapsed As TimerElapsed, ByVal interval As Long, ByVal runOnce As Boolean)
            Me.new(elapsed, interval)
            _runOnce = runOnce
        End Sub

        Public ReadOnly Property Running() As Boolean
            Get
                Return _running
            End Get
        End Property

        Public Sub Start()
            If Not _running Then
                SyncLock (_syncLock)
                    If Not _running Then
                        _running = True
                        _timerThread = New Thread(AddressOf Runtimer)
                        _timerThread.Name = "ScheduleTimerThread"
                        _timerThread.Start()
                    End If
                End SyncLock
            End If
        End Sub

        Public Sub [Stop]()
            If _running Then
                SyncLock (_syncLock)
                    _running = False
                    If _timerThread.ThreadState = ThreadState.WaitSleepJoin Then
                        _timerThread.Interrupt()
                    End If
                End SyncLock
            End If
        End Sub

        Private Sub Runtimer()
            While _running
                Try
                    Thread.Sleep(_interval)
                Catch ex As ThreadInterruptedException
                End Try
                SyncLock (_syncLock)
                    If _running Then
                        Try
                            _elapsed()
                        Catch
                            'Do nothing - The underlying code should handle the error
                        End Try
                    End If
                    If _runOnce Then
                        _running = False
                    End If
                End SyncLock
            End While
        End Sub
    End Class
End Namespace