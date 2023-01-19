''' -----------------------------------------------------------------------------
''' Project	 : MajodioMailServer
''' Class	 : Mail.Server.MimeMessageBodyPartCollection
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' 
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[ruwem]	2/27/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class MimeMessageBodyPartCollection
    Inherits CollectionBase

    Public Sub New(ByVal Boundary As String)
        _Boundary = Boundary
    End Sub

    Private _Boundary As String
    Public Property Boundary() As String
        Get
            Dim RVal As String = String.Empty
            If Not IsNothing(_Boundary) Then
                RVal = _Boundary
            End If
            Return RVal
        End Get
        Set(ByVal Value As String)
            _Boundary = Value
        End Set
    End Property

    Public Function Add(ByVal BP As MessageBodyPart) As MessageBodyPart
        MyBase.List.Add(BP)
        Return BP
    End Function

    Public Function Add(ByVal BPC As MimeMessageBodyPartCollection) As MimeMessageBodyPartCollection
        For i As Integer = 0 To BPC.Count - 1
            Add(BPC(i))
        Next
        Return BPC
    End Function

    Default Public Property Item(ByVal Index As Integer) As MessageBodyPart
        Get
            Return CType(MyBase.List.Item(Index), MessageBodyPart)
        End Get
        Set(ByVal Value As MessageBodyPart)
            MyBase.List.Item(Index) = Value
        End Set
    End Property

End Class