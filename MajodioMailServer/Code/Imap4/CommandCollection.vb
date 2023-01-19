Namespace Imap4
    Public Delegate Sub CommandCompleteEventHandler(ByVal Sender As Object, ByVal E As CommandCompleteEventArgs)

    Public Class CommandCompleteEventArgs
        Inherits EventArgs

        Public Sub New(ByVal WorkItem As Command)

        End Sub

    End Class

    Public Class CommandCollection
        Inherits CollectionBase

        Public Sub Add(ByVal Command As Command)
            MyBase.List.Add(Command)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As Command
            Get
                Return CType(MyBase.List.Item(Index), Command)
            End Get
            Set(ByVal Value As Command)
                MyBase.List.Item(Index) = Value
            End Set
        End Property

        Private _CommandQueue As Queue
        Public Event WorkCompleted As CommandCompleteEventHandler
    End Class
End Namespace