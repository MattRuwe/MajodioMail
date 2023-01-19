
Namespace Smtp
    <Serializable()> _
    Public Enum ResponseCode
        Other = 0
        SystemStatus = 211
        HelpMessage = 214
        ServiceReady = 220
        ServiceClosing = 221
        Ok = 250
        UserNotLocalWillForward = 251
        CannotVrfyUser = 252
        StartMailInput = 354
        ServiceNotAvailable = 421
        MailboxBusy = 450
        ErrorWhileProcessing = 451
        InsufficientSystemStorage = 452
        CommandUnrecognized = 500
        SyntaxErrorInParametersOrArguments = 501
        CommandNotImplemented = 502
        BadSequenceOfCommands = 503
        MailboxNotFound = 550
        UserNotLocal = 551
        ExceededStorageAllocation = 552
        MailboxNameNotAllowed = 553
        TransactionFailed = 554
    End Enum

    <Serializable()> _
    Public Class Response
        Private _ResponseCode As ResponseCode
        Private _Msg As String

        Public Sub New(ByVal ResponseCode As ResponseCode)
            _ResponseCode = ResponseCode
            Select Case ResponseCode
                Case ResponseCode.SystemStatus
                Case ResponseCode.HelpMessage
                Case ResponseCode.ServiceReady
                Case ResponseCode.ServiceClosing
                Case ResponseCode.Ok
                    _Msg = "ok"
                Case ResponseCode.UserNotLocalWillForward
                Case ResponseCode.CannotVrfyUser
                Case ResponseCode.StartMailInput
                    _Msg = "ok, send it; end with <CRLF>.<CRLF>"
                Case ResponseCode.ServiceNotAvailable
                Case ResponseCode.MailboxBusy
                Case ResponseCode.ErrorWhileProcessing
                Case ResponseCode.InsufficientSystemStorage
                Case ResponseCode.CommandUnrecognized
                    _Msg = "Syntax Error, Command Unrecognized"
                Case ResponseCode.SyntaxErrorInParametersOrArguments
                Case ResponseCode.CommandNotImplemented
                Case ResponseCode.BadSequenceOfCommands
                Case ResponseCode.MailboxNotFound
                    _Msg = "The specified mailbox could not be found"
                Case ResponseCode.UserNotLocal
                Case ResponseCode.ExceededStorageAllocation
                Case ResponseCode.MailboxNameNotAllowed
                Case ResponseCode.TransactionFailed
            End Select
        End Sub

        Public Sub New(ByVal ResponseCode As ResponseCode, ByVal Msg As String)
            _ResponseCode = ResponseCode
            _Msg = Msg
        End Sub

        Public ReadOnly Property ResponseCode() As ResponseCode
            Get
                Return _ResponseCode
            End Get
        End Property

        Public ReadOnly Property Msg() As String
            Get
                Return _Msg
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim RVal As String = String.Empty
            If _Msg.Length >= 1 Then
                If _Msg.Substring(0, 1) = "-" Then
                    RVal = _ResponseCode & _Msg
                Else
                    RVal = _ResponseCode & " " & _Msg
                End If
                If RVal.Substring(RVal.Length - 3, 2) <> vbCrLf Then
                    RVal &= vbCrLf
                End If
            End If
            Return RVal
        End Function
    End Class
End Namespace