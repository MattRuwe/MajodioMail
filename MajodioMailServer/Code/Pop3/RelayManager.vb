Imports Majodio.Mail.Common
Imports Majodio.Mail.Common.Configuration

Namespace Pop3
    Public Class RelayManager
        Private _client As Client
        Private _timer As Majodio.Common.Timer

        Public Sub New()
            _client = New Client()
            _timer = New Majodio.Common.Timer(New Majodio.Common.TimerElapsed(AddressOf Process))
        End Sub

        Public Sub Start()
            _timer.Start()
            Majodio.Mail.Server.Log.Logger.WriteLog("The POP3 relay manager has started")
        End Sub

        Public Sub [Stop]()
            _timer.Stop()
            Majodio.Mail.Server.Log.Logger.WriteLog("The POP3 relay manager has stopped")
        End Sub

        Private Sub Process()
            Dim relayDetails As Pop3RelayDetails() = RemoteConfigClient.RemoteDomain.GetPop3Relays()
            Dim intervalSeconds As Integer
            Dim lastProcessedTime As Long
            Dim currentTime As Long
            Dim nextProcessedTime As Long
            Dim message As Mime.Message
            Dim messageCount As Integer
            For i As Integer = 0 To relayDetails.GetUpperBound(0)
                Try
                    'Setup the time variables
                    intervalSeconds = relayDetails(i).IntervalSeconds
                    lastProcessedTime = relayDetails(i).LastProcessedTime
                    currentTime = DateTime.Now.Ticks
                    nextProcessedTime = lastProcessedTime + (intervalSeconds * TimeSpan.TicksPerSecond)

                    'Check if it is time to connect to the POP3 server
                    If nextProcessedTime <= currentTime Then
                        'Retrieve the messages from the POP3 server
                        _client.Connect(relayDetails(i).ServerAddress, relayDetails(i).Username, relayDetails(i).Password)
                        RemoteConfigClient.RemoteDomain.UpdateLastProcessedTime(relayDetails(i).ServerAddress, relayDetails(i).Username)
                        messageCount = _client.GetMessageCount
                        For j As Integer = 0 To messageCount - 1

                            message = _client.GetMessage(j)
                            _client.DeleteMessage(j)
                            Majodio.Mail.Server.Log.Logger.WriteLog("POP3 relay manager connected to " & relayDetails(i).ServerAddress & " and retrieved " & messageCount & " message(s)")

                            'Deliver the messages to their destination
                            For k As Integer = 0 To relayDetails(i).DeliveryAccounts.Count - 1
                                Try
                                    Dim queuedMessage As Majodio.Mail.Common.Storage.QueuedMail.Message
                                    queuedMessage = message.GetQueuedMessage(relayDetails(i).DeliveryAccounts(k).GetDomain, relayDetails(i).DeliveryAccounts(k).GetUsername)
                                    queuedMessage.DateTime = DateTime.Now
                                    queuedMessage.To = relayDetails(i).DeliveryAccounts(k)
                                    queuedMessage.From = message.FromAddress
                                    queuedMessage.Save()
                                    Majodio.Mail.Server.Log.Logger.WriteLog("POP3 relay manager delivered queued message to " & relayDetails(i).DeliveryAccounts(k).ToString(EmailStringFormat.NameAddress))
                                Catch ex As Exception
                                    Dim mailEx As New MailException("An exception occurred in the POP3 Relay Manager while attempting to deliver email to " & relayDetails(i).DeliveryAccounts(k).ToString, ex)
                                    mailEx.Save()
                                    Majodio.Mail.Server.Log.Logger.WriteError(ex, "An exception occurred in the POP3 Relay Manager while attempting to deliver email to " & relayDetails(i).DeliveryAccounts(k).ToString)
                                End Try
                            Next
                        Next
                        _client.Disconnect()
                    End If
                Catch ex As Exception
                    Dim mailEx As New MailException("An exception occurred in the POP3 Relay Manager while attempting to check e-mail from " & relayDetails(i).Username & "@" & relayDetails(i).ServerAddress, ex)
                    mailEx.Save()
                    Majodio.Mail.Server.Log.Logger.WriteError(ex, "An exception occurred in the POP3 Relay Manager while attempting to check e-mail from " & relayDetails(i).Username & "@" & relayDetails(i).ServerAddress)
                End Try

            Next
        End Sub

    End Class
End Namespace
