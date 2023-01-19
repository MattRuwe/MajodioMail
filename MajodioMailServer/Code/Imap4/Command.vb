Imports System.Text.RegularExpressions

Namespace Imap4

    Public Enum CommandStatus
        Empty
        ReceivedTag
        ReceivedCommand
        AwaitingContinuation
        Received
        Working
        Complete
        ErrorDetected
    End Enum

    Public Enum ClientCommands
        Capability
        Noop
        Logout
        Authenticate
        Login
        [Select]
        Examine
        Create
        Delete
        Rename
        Subscribe
        Unsubscribe
        List
        Lsub
        Append
        Check
        Close
        Expunge
        Search
        Fetch
        [Partial]
        Store
        Status
        Copy
        Uid
        Unknown
    End Enum

    Public Enum ServerResponses
        Ok
        No
        Bad
        Preauth
        Bye
        Capability
        List
        Lsub
        Search
        Flags
        Exists
        Recent
        Expunge
        Fetch
        Unknown
    End Enum

    Public Class Command
        Private _Tag As String
        Private _Command As String
        Private _Parameters As CommandParameterCollection
        Private _CurrentStatus As CommandStatus
        Private _BytesExpected As Integer
        Private _CommandResponse As Response
        Private _Imap4State As SessionState

        'Public Event WorkComplete as 

        Public Sub New(ByVal Imap4State As SessionState)
            _Imap4State = Imap4State
            _Tag = String.Empty
            _Command = String.Empty
            _Parameters = New CommandParameterCollection
            _CurrentStatus = CommandStatus.Empty
            _BytesExpected = -1
        End Sub

        Public Sub New(ByVal Imap4State As SessionState, ByVal Command As String)
            Me.New(Imap4State)
            _Command = Command
        End Sub

        Public Sub New(ByVal Imap4State As SessionState, ByVal Command As String, ByVal Tag As String)
            Me.New(Imap4State, Command)
            _Tag = Tag
        End Sub

        Public Sub New(ByVal Imap4State As SessionState, ByVal Command As String, ByVal Tag As String, ByVal Parameters As CommandParameterCollection)
            Me.New(Imap4State, Command, Tag)

            If Not IsNothing(Parameters) AndAlso Parameters.Count > 0 Then
                _Parameters = Parameters
            Else
                _Parameters = Nothing
            End If
        End Sub

        Public Sub Parse(ByVal Command As String)
            _BytesExpected = -1
            Dim ParameterString As String
            Dim M As Match
            If Not IsNothing(Command) Then
                If _Tag.Trim.Length = 0 OrElse _Command.Trim.Length = 0 Then
                    If Regex.IsMatch(Command, "(?i)[^{]+\{\d+}") Then
                        'The command contains a literal
                        M = Regex.Match(Command, "(?i)\s*(?<tag>\S+)\s+(?<Command>\S+)(?<Parameters>[^\{]+)\{(?<BytesExpected>\d+)}")
                        _Tag = M.Groups("tag").Value
                        _Command = M.Groups("Command").Value
                        ParameterString = M.Groups("Parameters").Value
                        _BytesExpected = M.Groups("BytesExpected").Value
                        _CurrentStatus = CommandStatus.AwaitingContinuation
                    Else
                        M = Regex.Match(Command, "(?i)\s*(?<tag>\S+)\s+(?<command>\S+)\s*(?<parameters>.*)")
                        If M.Success Then
                            _Tag = M.Groups("tag").Value
                            _Command = M.Groups("command").Value
                            ParameterString = M.Groups("parameters").Value
                            _Parameters.Add(ParameterString)
                            _CurrentStatus = CommandStatus.Received
                        Else
                            _CurrentStatus = CommandStatus.ErrorDetected
                        End If
                    End If
                Else
                    If Regex.IsMatch(Command, "(?i)[^\{]+\{\d+}") Then
                        'There are additional parameters
                        M = Regex.Match(Command, "(?i)(?<Parameters>[^\{]+)\{(?<BytesExpected>\d+)}")
                        If M.Success Then
                            Parameters.Add(New CommandParameter(M.Groups("Parameters").Value))
                            _BytesExpected = M.Groups("BytesExpected").Value
                            _CurrentStatus = CommandStatus.AwaitingContinuation
                        End If
                    Else
                        'This is the last parameter
                        Parameters.Add(New CommandParameter(Command))
                        _CurrentStatus = CommandStatus.Received
                    End If
                End If
            End If
        End Sub

        Public ReadOnly Property Imap4State() As SessionState
            Get
                Return _Imap4State
            End Get
        End Property

        Public ReadOnly Property Tag() As String
            Get
                If IsNothing(_Tag) OrElse _Tag.Trim.Length = 0 Then
                    _Tag = "*"
                End If
                Return _Tag
            End Get
        End Property

        Public ReadOnly Property Command() As String
            Get
                If IsNothing(_Command) Then
                    _Command = String.Empty
                End If
                Return _Command
            End Get
        End Property

        Public ReadOnly Property Parameters() As CommandParameterCollection
            Get
                If IsNothing(_Parameters) Then
                    _Parameters = New CommandParameterCollection
                End If
                Return _Parameters
            End Get
        End Property

        Public ReadOnly Property CurrentStatus() As CommandStatus
            Get
                Return _CurrentStatus
            End Get
        End Property

        Public ReadOnly Property BytesExpected() As Integer
            Get
                Return _BytesExpected
            End Get
        End Property

        Public ReadOnly Property CommandResponse() As Response
            Get
                Return _CommandResponse
            End Get
        End Property

        Public Shared Function GetCommand(ByVal Command As String) As ClientCommands
            Dim RVal As ClientCommands = ClientCommands.Unknown
            If Command.IndexOf(" ") = -1 Then
                Select Case Command.Trim.ToLower
                    Case "capability"
                        RVal = ClientCommands.Capability
                    Case "noop"
                        RVal = ClientCommands.Noop
                    Case "logout"
                        RVal = ClientCommands.Logout
                    Case "authenticate"
                        RVal = ClientCommands.Authenticate
                    Case "login"
                        RVal = ClientCommands.Login
                    Case "select"
                        RVal = ClientCommands.Select
                    Case "examine"
                        RVal = ClientCommands.Examine
                    Case "create"
                        RVal = ClientCommands.Create
                    Case "delete"
                        RVal = ClientCommands.Delete
                    Case "rename"
                        RVal = ClientCommands.Rename
                    Case "subscribe"
                        RVal = ClientCommands.Subscribe
                    Case "unsubscribe"
                        RVal = ClientCommands.Unsubscribe
                    Case "list"
                        RVal = ClientCommands.List
                    Case "lsub"
                        RVal = ClientCommands.Lsub
                    Case "append"
                        RVal = ClientCommands.Append
                    Case "check"
                        RVal = ClientCommands.Check
                    Case "close"
                        RVal = ClientCommands.Close
                    Case "expunge"
                        RVal = ClientCommands.Expunge
                    Case "search"
                        RVal = ClientCommands.Search
                    Case "fetch"
                        RVal = ClientCommands.Fetch
                    Case "partial"
                        RVal = ClientCommands.[Partial]
                    Case "store"
                        RVal = ClientCommands.Store
                    Case "status"
                        RVal = ClientCommands.Status
                    Case "copy"
                        RVal = ClientCommands.Copy
                    Case "uid"
                        RVal = ClientCommands.Uid
                End Select
            End If
            Return RVal
        End Function
    End Class
End Namespace