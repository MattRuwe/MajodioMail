Namespace Imap4
    Public Enum ResponseCode
        Ok 'Success
        No 'Failure
        Bad 'Invalid syntax
        Continuation
        None
    End Enum

    Public Class Response
        Private _ResponseCode As ResponseCode
        Private _Tag As String
        Private _ResponseText As String
        Private _ResponseData As String

        Public Sub New()
            _ResponseCode = ResponseCode.Ok
            _Tag = "*"
            _ResponseText = String.Empty
        End Sub

        Public Sub New(ByVal ResponseCode As ResponseCode)
            _ResponseCode = ResponseCode
            _Tag = "*"
            _ResponseText = String.Empty
        End Sub

        Public Sub New(ByVal ResponseCode As ResponseCode, ByVal ResponseText As String)
            _Tag = "*"
            _ResponseCode = ResponseCode
            _ResponseText = ResponseText
        End Sub

        Public Sub New(ByVal ResponseCode As ResponseCode, ByVal ResponseText As String, ByVal Tag As String)
            _ResponseCode = ResponseCode
            _ResponseText = ResponseText
            _Tag = Tag
        End Sub

        Public Property Tag() As String
            Get
                Return _Tag
            End Get
            Set(ByVal Value As String)
                _Tag = Value
            End Set
        End Property

        Public Property ResponseCode() As ResponseCode
            Get
                Return _ResponseCode
            End Get
            Set(ByVal Value As ResponseCode)
                _ResponseCode = Value
            End Set
        End Property

        Public Property ResponseText() As String
            Get
                Return _ResponseText
            End Get
            Set(ByVal Value As String)
                _ResponseText = Value
            End Set
        End Property

        Public Property ResponseData() As String
            Get
                Return _ResponseData
            End Get
            Set(ByVal Value As String)
                _ResponseData = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim RVal As String = _ResponseText
            Dim TmpTag As String = IIf(_Tag.Trim.Length = 0, String.Empty, _Tag & " ")
            If Not IsNothing(ResponseData) AndAlso ResponseData.Trim.Length > 0 Then
                RVal = ResponseData & vbCrLf
            End If
            Select Case _ResponseCode
                Case ResponseCode.Ok
                    RVal = TmpTag & "OK " & _ResponseText & vbCrLf
                Case ResponseCode.Bad
                    RVal = TmpTag & "BAD " & _ResponseText & vbCrLf
                Case ResponseCode.No
                    RVal = TmpTag & "NO " & _ResponseText & vbCrLf
                Case ResponseCode.Continuation
                    RVal = "+ Ready for additional command text" & vbCrLf
                Case ResponseCode.None
                    RVal = TmpTag & _ResponseText & vbCrLf
            End Select
            Return RVal
        End Function
    End Class
End Namespace