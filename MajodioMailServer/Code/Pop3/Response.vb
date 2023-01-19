Imports System.IO
Imports System.Text.RegularExpressions

Namespace Pop3
    Public Class Response
        Private _ResponseCode As ResponseCode
        Private _Msg As String

        Public Sub New(ByVal ResponseCode As ResponseCode)
            _ResponseCode = ResponseCode
            Select Case ResponseCode
                Case Else
                    _Msg = "Unknown Response Code"
            End Select
        End Sub

        Public Sub New(ByVal ResponseCode As ResponseCode, ByVal Msg As String)
            _ResponseCode = ResponseCode
            _Msg = Msg
        End Sub

        Public Sub New(ByVal Pop3Response As String)
            Dim M As Match
            M = Regex.Match(Pop3Response, "(?is)(?<ResponseCode>[\S]+)\s+(?<Message>.*)")
            If M.Success Then
                Select Case M.Groups("ResponseCode").Value.ToLower
                    Case "+ok"
                        _ResponseCode = Pop3.ResponseCode.Ok
                    Case "-err"
                        _ResponseCode = Pop3.ResponseCode.Error
                    Case Else
                        Throw New ArgumentException("The Response Code (" + M.Groups("ResponseCode").Value + ") was not recognized")
                End Select
                _Msg = M.Groups("Message").Value
            End If
        End Sub


        Public ReadOnly Property ResponseCode() As ResponseCode
            Get
                Return _ResponseCode
            End Get
        End Property

        Public ReadOnly Property Msg() As String
            Get
                Dim RVal As String = _Msg

                Return RVal
            End Get
        End Property


        Public Overrides Function ToString() As String
            Dim RVal As String = String.Empty
            If ResponseCode = ResponseCode.Ok Then
                RVal = "+OK "
            ElseIf ResponseCode = ResponseCode.Error Then
                RVal = "-Err "
            End If
            RVal &= Msg & vbCrLf
            Return RVal
        End Function
    End Class
End Namespace