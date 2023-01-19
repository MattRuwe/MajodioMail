Imports System.IO
Imports System.Timers
Imports System.Threading
Imports Majodio.Mail.Common.Configuration

Public Class QueueMonitor
    Private _queuedMailTimer As Majodio.Common.Timer
    'Private _Config As Majodio.Mail.Common.Configuration.Config
    Private _QueuePath As String
    Private _CurrentStatus As Status

    Public Enum Status
        Stopped
        RunningIdle
        RunningProcessing
    End Enum

    Public Sub New()
        '_Config = New Majodio.Mail.Common.Configuration.Config
        _queuedMailTimer = New Majodio.Common.Timer(AddressOf Timer_Elapsed, CType(RemoteConfigClient.RemoteConfig, Config).QueueCheckInterval)
    End Sub

    Public Sub Start()
        If _CurrentStatus = Status.Stopped Then
            _QueuePath = GetApplicationDirectory() & "\" & QUEUED_MAIL_FOLDER
            If Not Directory.Exists(_QueuePath) Then
                Directory.CreateDirectory(_QueuePath)
            End If

            _queuedMailTimer.Start()
            'Timer_Elapsed(Nothing, Timers.ElapsedEventArgs.Empty)

            _CurrentStatus = Status.RunningIdle
        Else
            Throw New Exception("Queue could not start as it appears to already be running")
        End If
    End Sub

    Public Sub [Stop]()
        If _CurrentStatus = Status.RunningProcessing Or _CurrentStatus = Status.RunningIdle Then
            _queuedMailTimer.Stop()
            _CurrentStatus = Status.Stopped
        End If
    End Sub

    Public Sub Timer_Elapsed()
        Dim Th As New Thread(AddressOf ProcessQueuedMessages)
        Dim Count As Integer = 0
        Try
            'ProcessQueuedMessages()
            Th.Name = "QueueMonitor"
            Th.Start()
            'Make sure that the queue prcessing does not execute for more than 1 hour
            While Th.IsAlive AndAlso Count < 1440
                System.Threading.Thread.Sleep(2500)
                Count += 1
            End While
            If Th.IsAlive Then
                Th.Abort()
            End If
        Catch ex1 As Exception
            SendUsageInformation("Error occurred while attempting to start the queue thread: " & ex1.Source & " : " & ex1.Message & " : " & ex1.StackTrace)
            Log.Logger.WriteLog("Error occurred while attempting to start the queue thread: " & vbCrLf & ex1.Source & vbCrLf & ex1.Message & vbCrLf & ex1.StackTrace)
        Finally

        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/15/2005	messages will now delete when the send attempts
    '''                         exceeds the failed mail retry attempts
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub ProcessQueuedMessages()
        'Log.Logger.WriteLog("Checking for queued mail")
        Majodio.Common.Utilities.TraceMe("Checking for queued mail")
        Dim SuccessfullMessages As New ArrayList
        Dim PermanentlyFailedMessages As New ArrayList
        Try
            If _CurrentStatus = Status.RunningIdle Then
                _CurrentStatus = Status.RunningProcessing
                Dim UndeliverableMessage As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
                Dim CurrentMessage As Majodio.Mail.Common.Storage.QueuedMail.Message = Nothing
                Dim CurrentMimeMessage As Mime.Message
                Dim SmtpClinet As Smtp.Client
                Dim i As Integer
                Dim Result As Majodio.Mail.Common.Smtp.Response()
                UndeliverableMessage = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.GetUndeliveredMessages()
                For i = 0 To UndeliverableMessage.Count - 1
                    Try
                        CurrentMessage = UndeliverableMessage.Item(i)
                        CurrentMimeMessage = new Mime.Message(CurrentMessage)
                        SmtpClinet = New Smtp.Client(CurrentMimeMessage)
                        Result = SmtpClinet.Send(True)
                        If Not IsNothing(Result) AndAlso Result.GetUpperBound(0) > -1 AndAlso Not IsNothing(Result(0)) AndAlso Result(0).ResponseCode = Majodio.Mail.Common.Smtp.ResponseCode.Ok Then
                            'The message was successfully sent
                            SuccessfullMessages.Add(CurrentMessage.MessageId)
                        ElseIf CurrentMessage.SendAttemptsMade > RemoteConfigClient.RemoteConfig.FailedMailRetryAttempts Then
                            'The maximum number of retry attempts has been made, deleting the message and notifying the sender
                            If Not IsNothing(CurrentMessage.LastResult) Then
                                Majodio.Mail.Server.Smtp.Client.SendErrors(CurrentMimeMessage, 0, CurrentMessage.LastResult)
                            Else
                                Majodio.Mail.Server.Smtp.Client.SendErrors(CurrentMimeMessage, 0, New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.Other, "The message was not delivered because of an unknown problem"))
                            End If
                            PermanentlyFailedMessages.Add(CurrentMessage.MessageId)
                        ElseIf Result(0).ResponseCode = Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing OrElse Result(0).ResponseCode = Majodio.Mail.Common.Smtp.ResponseCode.TransactionFailed Then
                            'Permanent Failures which cannot be recovered from
                            CurrentMessage.LastResult = Result(0)
                            PermanentlyFailedMessages.Add(CurrentMessage.MessageId)
                            Smtp.Client.SendErrors(CurrentMimeMessage, i, New Majodio.Mail.Common.Smtp.Response(Result(0).ResponseCode, Result(0).Msg))
                        Else
                            'Possibly a temporary problem that we'll keep trying to resolve.
                            With CurrentMessage
                                If Not IsNothing(Result) AndAlso Result.GetUpperBound(0) > -1 Then
                                    .LastResult = Result(0)
                                Else
                                    .LastResult = New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing, "No response code returned from the SMTP client")
                                End If
                                .LastSendAttempt = DateTime.Now
                                .SendAttemptsMade += 1
                                .Save()
                            End With
                        End If
                    Catch ex1 As ThreadAbortException
                        CurrentMessage.LastResult = New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing, "The thread was prematurely aborted, probably because it had been executing too long.")
                        Majodio.Mail.Server.Smtp.Client.SendErrors(New Mime.Message(CurrentMessage), 0, CurrentMessage.LastResult)
                        PermanentlyFailedMessages.Add(CurrentMessage.MessageId)
                        'CurrentMessage.DeleteMessage(True)
                        Throw ex1
                    Catch ex1 As Exception
                        Try
                            Dim MailEx As New MailException("An exception occurred while processing a message in the queue", ex1)
                            MailEx.Save()
                            CurrentMessage.LastResult = New Majodio.Mail.Common.Smtp.Response(Majodio.Mail.Common.Smtp.ResponseCode.ErrorWhileProcessing, "Error occurred while processing the message: " & vbCrLf & ex1.Source & vbCrLf & ex1.Message & vbCrLf & ex1.StackTrace)
                            Majodio.Mail.Server.Smtp.Client.SendErrors(New Mime.Message(CurrentMessage), 0, CurrentMessage.LastResult)
                        Catch ex2 As Exception
                        Finally
                            SendUsageInformation("Error occurred while processing one queued message: " & ex1.Source & " : " & ex1.Message & " : " & ex1.StackTrace)
                            Log.Logger.WriteLog("Error occurred while processing one queued message." & vbCrLf & ex1.Source & vbCrLf & ex1.Message & vbCrLf & ex1.StackTrace)
                            CurrentMessage.DeleteMessage(True)
                        End Try

                    End Try
                Next
            End If
        Catch ex1 As ThreadAbortException
        Catch ex1 As Exception
            Dim MailEx As New MailException("An exception occurred while processing the message(s) in the queue", ex1)
            MailEx.Save()
            SendUsageInformation("Error occurred in ProcessQueuedMessages: " & ex1.Source & " : " & ex1.Message & " : " & ex1.StackTrace)
            Log.Logger.WriteLog("Error occurred in ProcessQueuedMessages:" & vbCrLf & ex1.Source & vbCrLf & ex1.Message & vbCrLf & ex1.StackTrace)
        Finally
            Try
                Dim Message As Majodio.Mail.Common.Storage.QueuedMail.Message
                For i As Integer = 0 To SuccessfullMessages.Count - 1
                    Message = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.GetUndeliveredMessage(SuccessfullMessages(i))
                    Message.DeleteMessage(False)
                Next
                For i As Integer = 0 To PermanentlyFailedMessages.Count - 1
                    Message = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.GetUndeliveredMessage(PermanentlyFailedMessages(i))
                    Message.DeleteMessage(True)
                Next
            Catch ex As Exception

            End Try
            _CurrentStatus = Status.RunningIdle
        End Try
    End Sub
End Class
