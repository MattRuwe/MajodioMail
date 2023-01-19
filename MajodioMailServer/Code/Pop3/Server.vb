Imports System.Security.Cryptography
Imports Majodio.Mail.Common.Configuration

Namespace Pop3
    Public Class Server
        Private _CurrentServerStatus As Status
        Private _CurrentServerState As State
        'Private _Config As Majodio.Mail.Common.Configuration.Config
        Private _IsSecureConnection As Boolean = False

        Public Sub New()
            _CurrentServerStatus = Status.Connected
            _CurrentServerState = State.None
            '_Config = New Majodio.Mail.Common.Configuration.Config
        End Sub

        Public Property CurrentServerStatus() As Status
            Get
                Return _CurrentServerStatus
            End Get
            Set(ByVal Value As Status)
                _CurrentServerStatus = Value
            End Set
        End Property

        Private _Username As EmailAddress
        Public Property Username() As EmailAddress
            Get
                If IsNothing(_Username) Then
                    Return New EmailAddress
                Else
                    Return _Username
                End If
            End Get
            Set(ByVal Value As EmailAddress)
                _Username = Value
            End Set
        End Property

        Private _Password As String
        Public Property Password() As String
            Get
                If IsNothing(_Password) Then
                    Return String.Empty
                Else
                    Return _Password
                End If
            End Get
            Set(ByVal Value As String)
                _Password = Value
            End Set
        End Property

        Private _Messages As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
        Public Property Messages() As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
            Get
                If IsNothing(_Messages) Then
                    _Messages = New Majodio.Mail.Common.Storage.QueuedMail.MessageCollection
                End If
                Return _Messages
            End Get
            Set(ByVal Value As Majodio.Mail.Common.Storage.QueuedMail.MessageCollection)
                _Messages = Value
            End Set
        End Property

        Private _DeletedMessages As ArrayList
        Public ReadOnly Property DeletedMessages() As ArrayList
            Get
                If IsNothing(_DeletedMessages) Then
                    _DeletedMessages = New ArrayList
                End If
                Return _DeletedMessages
            End Get
        End Property

        Public ReadOnly Property IsSecureConnection() As Boolean
            Get
                Return _IsSecureConnection
            End Get
        End Property

        Public ReadOnly Property NumberOfMessages() As Integer
            Get
                Dim MessageTotal As Integer = 0
                Dim i As Integer
                For i = 0 To Messages.Count - 1
                    If DeletedMessages.IndexOf(Messages(i).MessageId) = -1 Then
                        MessageTotal += 1
                    End If
                Next
                Return MessageTotal
            End Get
        End Property

        Public ReadOnly Property TotalMessageSize() As Integer
            Get
                Dim i As Integer
                Dim TotalSize As Integer = 0
                For i = 0 To Messages.Count - 1
                    If DeletedMessages.IndexOf(Messages(i).MessageId) = -1 Then
                        TotalSize += Messages(i).MessageSize
                    End If
                Next
                Return TotalSize
            End Get
        End Property

        Public Function ProcessMsg(ByVal Msg As String) As Pop3.Response()
            Dim RVal() As Pop3.Response
            Try
                Dim Msgs As String() = ParseMessage(Msg)
                Dim Email As EmailAddress
                Dim i As Integer
                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Syntax Error; Unknown command")}
                If Not IsNothing(Msgs) Then
                    If Msgs.GetUpperBound(0) > -1 Then
                        Select Case Msgs(0).Trim.ToLower
                            Case "auth"
                                If _CurrentServerState = Pop3.State.Authorization Or _CurrentServerState = Pop3.State.None Then
                                    If Msgs.GetUpperBound(0) = 0 Then
                                        RVal = New Pop3.Response() {New Pop3.Response(ResponseCode.Error, "No authentication type specified.")}
                                        'RVal = New Pop3.Response() {New Pop3.Response(ResponseCode.Empty, "None"), New Pop3.Response(ResponseCode.Empty, ".")}
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(ResponseCode.Error, "Not implemented")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Not in the authorization state")}
                                End If
                            Case "capa"
                                RVal = New Response() { _
                                        New Response(ResponseCode.Ok, "Capability list follows"), _
                                        New Response(ResponseCode.Empty, "TOP"), _
                                        New Response(ResponseCode.Empty, "USER")}
                                If RemoteConfigClient.RemoteConfig.SSLCertificatePath.Trim.Length > 0 And System.IO.File.Exists(RemoteConfigClient.RemoteConfig.SSLCertificatePath) Then
                                    ReDim Preserve RVal(RVal.GetUpperBound(0) + 1)
                                    RVal(RVal.GetUpperBound(0)) = New Response(ResponseCode.Empty, "STARTTLS")
                                End If
                                ReDim Preserve RVal(RVal.GetUpperBound(0) + 1)
                                RVal(RVal.GetUpperBound(0)) = New Response(ResponseCode.Empty, ".")
                            Case "stls"
                                If Not IsNothing(RemoteConfigClient.RemoteConfig.SSLCertificatePath) AndAlso RemoteConfigClient.RemoteConfig.SSLCertificatePath.Trim.Length > 0 Then
                                    RVal = New Pop3.Response() {New Pop3.Response(ResponseCode.Ok, "Begin TLS negotiation")}
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Illegal command")}
                                End If
                            Case "user"
                                If _CurrentServerState = Pop3.State.Authorization Or _CurrentServerState = Pop3.State.None Then
                                    If Msgs.GetUpperBound(0) >= 1 Then
                                        If Majodio.Common.EmailAddress.IsValidAddress(Msgs(1)) Then
                                            Email = New EmailAddress(Msgs(1))
                                            Me.Username = Email
                                            _CurrentServerState = Pop3.State.Authorization
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, "Welcome " & Email.GetUsername & ", password required")}
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "The username entered is not in the correct format - i.e. username@domain.com")}
                                        End If
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "No username included")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Not in the authorization state")}
                                End If
                            Case "pass"
                                If _CurrentServerState = Pop3.State.Authorization Then
                                    If Msgs.GetUpperBound(0) >= 1 Then
                                        If RemoteConfigClient.RemoteDomain.AuthorizeUser(Username.GetDomain, Username.GetUsername, Msgs(1)) Then
                                            If ConnectionStatsManager.IsIpWithinConnectionLimit(Username.ToString(EmailStringFormat.Address)) Then
                                                ConnectionStatsManager.AddConnection(Username.ToString(EmailStringFormat.Address))
                                                Me.Password = Msgs(1)
                                                Messages = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateFolder.GetAllMailMessages(Username.GetDomain, Username.GetUsername)
                                                _CurrentServerState = Pop3.State.Transaction
                                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, "Mailbox locked and ready")}
                                            Else
                                                RVal = New Pop3.Response() {New Pop3.Response(ResponseCode.Error, "You have exceeded your connection count")}
                                            End If
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Invalid Username/Password Combination")}
                                        End If
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "No password included")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Not in the authorization state")}
                                End If
                            Case "stat"
                                If _CurrentServerState = Pop3.State.Transaction Then
                                    If _CurrentServerState = Pop3.State.Transaction Then
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, NumberOfMessages & " " & TotalMessageSize)}
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Not in the transaction state")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "server is not in the transaction state")}
                                End If
                            Case "list"
                                If _CurrentServerState = Pop3.State.Transaction Then
                                    If Msgs.GetUpperBound(0) = 0 Then
                                        ReDim RVal(0)
                                        Dim j As Integer = 0
                                        RVal(0) = New Pop3.Response(Pop3.ResponseCode.Ok, Messages.Count & " messages (" & TotalMessageSize & " octets)")
                                        For i = 0 To Messages.Count - 1
                                            If DeletedMessages.IndexOf(Messages(i).MessageId) = -1 Then
                                                j += 1
                                                ReDim Preserve RVal(j + 1)
                                                RVal(j) = New Pop3.Response(Pop3.ResponseCode.Empty, i + 1 & " " & Messages(i).MessageSize)
                                            End If
                                        Next
                                        If j = 0 Then
                                            ReDim Preserve RVal(1)
                                        End If
                                        RVal(j + 1) = New Pop3.Response(Pop3.ResponseCode.Empty, ".")
                                    ElseIf Msgs.GetUpperBound(0) >= 1 Then
                                        If IsNumeric(Msgs(1)) Then
                                            If CType(Msgs(1), Integer) > 0 And CType(Msgs(1), Integer) <= Messages.Count Then
                                                If DeletedMessages.IndexOf(Messages(Msgs(1) - 1).MessageId) = -1 Then
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, Msgs(1) & " " & Messages(Msgs(1) - 1).MessageSize)}
                                                Else
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, it has been marked for deletion")}
                                                End If
                                            Else
                                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, " & Messages.Count & " messages in maildrop")}
                                            End If
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, message id are required to be numeric")}
                                        End If
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "server is not in the transaction state")}
                                End If
                            Case "retr"
                                If _CurrentServerState = Pop3.State.Transaction Then
                                    If Msgs.GetUpperBound(0) >= 1 Then
                                        If IsNumeric(Msgs(1)) Then
                                            If CType(Msgs(1), Integer) > 0 And CType(Msgs(1), Integer) <= Messages.Count Then
                                                If DeletedMessages.IndexOf(Messages(Msgs(1) - 1).MessageId) = -1 Then
                                                    ReDim RVal(2)
                                                    RVal(0) = New Pop3.Response(Pop3.ResponseCode.Ok, Messages(Msgs(1) - 1).MessageSize & " octets")
                                                    RVal(1) = New Pop3.Response(Pop3.ResponseCode.Empty, Messages(Msgs(1) - 1).GetStringMessageContent.Replace(vbCrLf & "." & vbCrLf, vbCrLf & ".." & vbCrLf))
                                                    RVal(2) = New Pop3.Response(Pop3.ResponseCode.Empty, ".")
                                                    Messages(Msgs(1) - 1).Seen = True
                                                    Messages(Msgs(1) - 1).Save()
                                                    'Logger.AddMessagesDelivered()
                                                    Performance.IncrementMessagesDelivered()
                                                Else
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, it has been marked for deletion")}
                                                End If
                                            Else
                                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, " & Me.NumberOfMessages & " messages in maildrop")}
                                            End If
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, message id are required to be numeric")}
                                        End If
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "No message id found")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "server is not in the transaction state")}
                                End If
                            Case "dele"
                                If _CurrentServerState = Pop3.State.Transaction Then
                                    If Msgs.GetUpperBound(0) >= 1 Then
                                        If IsNumeric(Msgs(1)) Then
                                            If CType(Msgs(1), Integer) > 0 And CType(Msgs(1), Integer) <= Messages.Count Then
                                                If DeletedMessages.IndexOf(Messages(Msgs(1) - 1).MessageId) = -1 Then
                                                    DeletedMessages.Add(Messages(Msgs(1) - 1).MessageId)
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, "message deleted")}
                                                Else
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, it has been marked for deletion")}
                                                End If
                                            End If
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, " & Me.NumberOfMessages & " messages in maildrop")}
                                        End If
                                    Else
                                        RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "No message id indicated")}
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "server is not in the transaction state")}
                                End If
                            Case "uidl"
                                If _CurrentServerState = Pop3.State.Transaction Then
                                    Dim Hash As HashAlgorithm
                                    Hash = New SHA1Managed
                                    If Msgs.GetUpperBound(0) = 0 Then
                                        ReDim RVal(0)
                                        Dim j As Integer = 0
                                        RVal(0) = New Pop3.Response(Pop3.ResponseCode.Ok, "unique-id listing follows")
                                        For i = 0 To Messages.Count - 1
                                            If DeletedMessages.IndexOf(Messages(i).MessageId) = -1 Then
                                                j += 1
                                                ReDim Preserve RVal(j + 1)
                                                RVal(j) = New Pop3.Response(Pop3.ResponseCode.Empty, (i + 1) & " " & Convert.ToBase64String(Hash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Messages(i).GetStringMessageContent))))
                                            End If
                                        Next
                                        If j = 0 Then
                                            ReDim Preserve RVal(1)
                                        End If
                                        RVal(j + 1) = New Pop3.Response(Pop3.ResponseCode.Empty, ".")
                                    ElseIf Msgs.GetUpperBound(0) >= 1 Then
                                        If IsNumeric(Msgs(1)) Then
                                            If CType(Msgs(1), Integer) > 0 And CType(Msgs(1), Integer) <= Messages.Count Then
                                                If DeletedMessages.IndexOf(Messages(Msgs(1) - 1).MessageId) = -1 Then
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, Msgs(1) & " " & Convert.ToBase64String(Hash.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Messages(Msgs(1) - 1).GetStringMessageContent))))}
                                                Else
                                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, it has been marked for deletion")}
                                                End If
                                            Else
                                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, " & Messages.Count & " messages in maildrop")}
                                            End If
                                        Else
                                            RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "no such message, message id are required to be numeric")}
                                        End If
                                    End If
                                Else
                                    RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "server is not in the transaction state")}
                                End If
                            Case "last"
                                Dim LastReadMsg As Integer = 0
                                For i = 0 To _Messages.Count - 1
                                    If DeletedMessages.IndexOf(_Messages(i).MessageId) = -1 Then
                                        If _Messages(i).Seen = True Then
                                            LastReadMsg = i + 1
                                        End If
                                    End If
                                Next
                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, LastReadMsg)}
                            Case "quit"
                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Ok, "Majodio POP3 signing off")}
                                If Not IsNothing(_DeletedMessages) Then
                                    For i = 0 To _DeletedMessages.Count - 1
                                        Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage.DeleteMessage(Username.GetDomain, Username.GetUsername, CType(_DeletedMessages.Item(i), String), False)
                                    Next
                                End If
                                _CurrentServerStatus = Pop3.Status.Closed
                            Case Else
                                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "Illegal command")}
                        End Select
                    End If
                End If
            Catch exc As Exception
                Log.Logger.WriteLog("A fatal error occurred while processing POP3 request")
                Log.Logger.WriteLog(exc.Source)
                Log.Logger.WriteLog(exc.Message)
                Log.Logger.WriteLog(exc.StackTrace)
                RVal = New Pop3.Response() {New Pop3.Response(Pop3.ResponseCode.Error, "A fatal error occurred.  Please see the system log for details")}
            End Try
            Return RVal
        End Function

        Private Function ParseMessage(ByVal Msg As String) As String()
            Dim RVal As String() = Nothing
            Dim Count As Integer = -1
            Dim StartIndex As Integer = 0
            Dim EndIndex As Integer = 0
            If Not IsNothing(Msg) Then
                Msg = Msg.Replace(":", " ")
                While StartIndex < Msg.Length
                    If Msg.Substring(StartIndex, 1) = """" Then
                        EndIndex = Msg.IndexOf("""", StartIndex)
                    Else
                        EndIndex = Msg.IndexOf(" ", StartIndex)
                    End If
                    If EndIndex = -1 Then
                        EndIndex = Msg.Length
                    End If
                    Count += 1
                    ReDim Preserve RVal(Count)
                    RVal(Count) = Msg.Substring(StartIndex, EndIndex - StartIndex)
                    StartIndex = EndIndex + 1
                    If StartIndex < Msg.Length Then
                        While Msg.Substring(StartIndex, 1) = " " And StartIndex < Msg.Length
                            StartIndex += 1
                        End While
                    End If
                End While
            End If
            Return RVal
        End Function
    End Class
End Namespace

