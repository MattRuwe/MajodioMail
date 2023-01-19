Imports Microsoft.Win32

Namespace Majodio.Common.Messaging
    Public Class SendMessage
        Private _Category As String
        Private _Message As String

        Public Sub New(ByVal Category As String, ByVal Message As String)
            _Category = Category
            _Message = Message
        End Sub

        Public Shared Sub SendMessage(ByVal Category As String, ByVal Message As String)
            Dim SM As New SendMessage(Category, Message)
            Dim T As New Threading.Thread(AddressOf SM.SendMessage)
            T.Name = "SendMessage"
            T.Start()
        End Sub

        Public Sub SendMessage()
            Try
                'Dim Messaging As New Majodio.Common.Messaging.messaging
                'Messaging.InsertMessage(_Category, _Message)
            Catch ex As Exception

            End Try
        End Sub

    End Class

    Public Class Feedback
        Private _Product As String
        Private _Email As String
        Private _Name As String
        Private _Message As String

        Public Sub New(ByVal Product As String, ByVal Email As String, ByVal Name As String, ByVal Message As String)
            _Product = Product
            _Email = Email
            _Name = Name
            _Message = Message
        End Sub

        Public Sub Send()
            Try
                'Dim Messaging As New Majodio.Common.Messaging.messaging
                'Messaging.SendFeedback(_Product, _Email, _Name, _Message)
            Catch ex As Exception

            End Try
        End Sub
    End Class

    Public Class Ping
        Private _Product As String
        Private _Version As String

        Public Sub New(ByVal Product As String, ByVal Version As String)
            _Product = Product
            _Version = Version
        End Sub

        Public Shared Sub Ping(ByVal Product As String, ByVal Version As String)
            Dim PT As New Ping(Product, Version)
            Dim T As New Threading.Thread(AddressOf PT.Ping)
            T.Name = "SendPing"
            T.Start()
        End Sub

        Public Sub Ping()
            Try
                'Dim Messaging As New Majodio.Common.Messaging.messaging
                'Messaging.Ping(_Product, _Version)
            Catch Ex As Exception

            End Try
        End Sub
    End Class

    Public Class DeliverableMessages
        Private _Product As String
        Private _Version As String
        Private _LatestMessageId As Integer

        Private Sub New()
            _Product = String.Empty
            _Version = String.Empty
            _LatestMessageId = -1
        End Sub

        Public Sub New(ByVal Product As String, ByVal Version As String, ByVal LatestMessageId As Integer)
            Me.New()
            _Product = Product
            _Version = Version
            _LatestMessageId = LatestMessageId
        End Sub

        Public Sub New(ByVal Product As String, ByVal LatestMessageId As Integer)
            Me.New()
            _Product = Product
            _LatestMessageId = LatestMessageId
        End Sub

        Public Shared Function GetLatestMessageId() As Int32
            Dim TmpRVal As Object
            Dim RVal As Integer = -1
            TmpRVal = GetMajdoioRegKey.GetValue("LatestDeliverableMessageId")
            If Not IsNothing(TmpRVal) And IsNumeric(TmpRVal) Then
                RVal = TmpRVal
            End If
            Return RVal
        End Function

        Public Shared Sub SetLatestMessageId(ByVal Value As Int32)
            Dim R As RegistryKey
            R = GetMajdoioRegKey()
            R.SetValue("LatestDeliverableMessageId", Value)
        End Sub

        Public Function GetDeliverableMessages() As String()
            'Dim Ids As Integer()
            'Dim M As New Majodio.Common.Messaging.messaging
            Dim RVal As String() = Nothing
            'Ids = M.GetNewDeliverableMessageIds(_Product, _Version, _LatestMessageId)
            'If Not IsNothing(Ids) AndAlso Ids.GetUpperBound(0) > -1 Then
            '    For i As Int32 = 0 To Ids.GetUpperBound(0)
            '        If IsNothing(RVal) OrElse RVal.GetUpperBound(0) < 0 Then
            '            ReDim RVal(0)
            '        Else
            '            ReDim Preserve RVal(RVal.GetUpperBound(0) + 1)
            '        End If
            '        RVal(RVal.GetUpperBound(0)) = M.GetDeliverableMessage(Ids(i))
            '    Next
            '    SetLatestMessageId(Ids(Ids.GetUpperBound(0)))
            'End If
            Return RVal
        End Function
    End Class
End Namespace