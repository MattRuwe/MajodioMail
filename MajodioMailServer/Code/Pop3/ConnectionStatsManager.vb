Imports System.Threading
Imports Majodio.Mail.Common.Configuration

Namespace Pop3
    Public Class ConnectionStatsManager

        Private Shared Stats As Hashtable
        Private Shared MaintenanceTimer As System.Timers.Timer

        Shared Sub New()
            Stats = New Hashtable
            MaintenanceTimer = New System.Timers.Timer
            AddHandler MaintenanceTimer.Elapsed, AddressOf MaintenanceTimer_Elapsed
            With MaintenanceTimer
                .AutoReset = False
                .Interval = 10000
                .Start()
            End With
        End Sub

        Public Shared Function IsIpWithinConnectionLimit(ByVal EmailAddress As String) As Boolean
            Dim RVal As Boolean = True
            'Dim Config As New Majodio.Mail.Common.Configuration.Config

            If Stats.ContainsKey(EmailAddress) Then
                If CType(Stats(EmailAddress), ConnectionStatsCollection).Count >= RemoteConfigClient.RemoteConfig.MaxPop3ConnectionsPerInterval AndAlso RemoteConfigClient.RemoteConfig.MaxPop3ConnectionsPerInterval > 0 Then
                    RVal = False
                End If
            End If
            Return RVal
        End Function

        Public Shared Sub AddConnection(ByVal EmailAddress As String)
            Dim ConnectionStats As ConnectionStatsCollection
            Try
                Monitor.Enter(Stats)
                If Not Stats.ContainsKey(EmailAddress) Then
                    ConnectionStats = New ConnectionStatsCollection
                    Stats.Add(EmailAddress, ConnectionStats)
                Else
                    ConnectionStats = Stats(EmailAddress)
                End If
                ConnectionStats.Add(New ConnectionStats())
            Finally
                Monitor.Exit(Stats)
            End Try
        End Sub


        Private Shared Sub MaintenanceTimer_Elapsed(ByVal Sender As Object, ByVal E As System.Timers.ElapsedEventArgs)
            MaintainConnectionStats()
            MaintenanceTimer.Start()
        End Sub

        Private Shared Sub MaintainConnectionStats()
            Dim KeysToRemove As New ArrayList
            'Dim Config As New Majodio.Mail.Common.Configuration.Config
            Dim CurrentIndex As Integer
            Try
                Monitor.Enter(Stats)
                'Loop through each item within the hash
                For Each Key As String In Stats.Keys
                    'Loop through each item in the collection within the current key and check to 
                    'see if the item has expired or not (i.e. been in the cache for more than 1 hour)
                    CurrentIndex = 0
                    For i As Integer = 0 To CType(Stats(Key), ConnectionStatsCollection).Count - 1
                        If (DateTime.Now.Ticks - CType(Stats(Key), ConnectionStatsCollection)(CurrentIndex).ConnectionTime.Ticks) / RemoteConfigClient.RemoteConfig.MaxPop3ConnectionInterval > 1 Then
                            If CType(Stats(Key), ConnectionStatsCollection).Count > 1 Then
                                CType(Stats(Key), ConnectionStatsCollection).RemoveAt(CurrentIndex)
                                CurrentIndex -= 1
                            Else
                                KeysToRemove.Add(Key)
                                'Stats.Remove(Key)
                            End If
                        End If
                        CurrentIndex += 1
                    Next
                Next

                For i As Integer = 0 To KeysToRemove.Count - 1
                    Stats.Remove(KeysToRemove(i))
                Next
            Finally
                Monitor.Exit(Stats)
            End Try
        End Sub

        'Private Class ItemToRemove
        '    Public Key As String
        '    Public Index As Integer

        '    Public Sub New(ByVal K As String, ByVal I As Integer)
        '        Key = K
        '        Index = I
        '    End Sub
        'End Class
    End Class
End Namespace