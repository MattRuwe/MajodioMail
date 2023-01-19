Namespace Imap4
    Public Class ResponseCollection
        Inherits CollectionBase

        Public Sub New()

        End Sub

        Public Sub New(ByVal Response As Response)
            Add(Response)
            '_NewServerState = Imap4State.Unknown
        End Sub

        Public Sub Add(ByVal Response As Response)
            MyBase.List.Add(Response)
        End Sub

        Public Sub Add(ByVal Items As ResponseCollection)
            For i As Integer = 0 To Items.Count - 1
                Add(Items(i))
            Next
        End Sub

        Public Sub Insert(ByVal Index As Int32, ByVal Items As ResponseCollection)
            Dim CurrentPosition As Int32 = Index
            If Not IsNothing(Items) Then
                For i As Integer = 0 To Items.Count - 1
                    Insert(CurrentPosition, Items(i))
                    CurrentPosition += 1
                Next
            End If
        End Sub

        Public Sub Insert(ByVal Index As Integer, ByVal Response As Response)
            If Not IsNothing(Response) Then
                MyBase.List.Insert(Index, Response)
            End If
        End Sub

        Default Public Property Item(ByVal Index As Integer) As Response
            Get
                Return CType(MyBase.List.Item(Index), Response)
            End Get
            Set(ByVal Value As Response)
                MyBase.List.Item(Index) = Value
            End Set
        End Property
    End Class
End Namespace