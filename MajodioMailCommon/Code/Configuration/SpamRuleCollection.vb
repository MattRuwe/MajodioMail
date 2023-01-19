Namespace Configuration
    <Serializable()> _
    Public Class SpamRuleCollection
        Inherits CollectionBase

        Public Event ContentsChanged As EventHandler

        Public Sub Add(ByVal item As SpamRule)
            Dim maxOrderNumber As Integer = GetMaxOrderNumber()
            'Add the item
            If item.Order = -1 Then
                item.Order = Me.Count
            End If
            MyBase.List.Add(item)
            EnsureNoDuplicateOrderNumbers()
            Sort()
        End Sub

        Default Public Property Item(ByVal index As Integer) As SpamRule
            Get
                Return CType(MyBase.List.Item(index), SpamRule)
            End Get
            Set(ByVal value As SpamRule)
                MyBase.List.Item(index) = value
            End Set
        End Property

        Public Function GetMaxOrderNumber() As Integer
            Dim rVal As Integer = 0
            For i As Integer = 0 To Me.Count - 1
                If Me(i).Order > rVal Then
                    rVal = Me(i).Order
                End If
            Next

            Return rVal
        End Function

        Public Sub SwapOrderNumbers(ByVal index As Integer)
            'Swaps the indexes order number with the value one index higher
            Dim tmpOrderNumber As Integer

            If index >= Count - 1 Then
                Throw New ArgumentOutOfRangeException("index", "Cannot be greater than or equal to the last item index in the collection.")
            End If

            tmpOrderNumber = Me(index + 1).Order
            Me(index + 1).Order = Me(index).Order
            Me(index).Order = tmpOrderNumber

            Sort()
        End Sub

        Public Sub Sort()
            MyBase.InnerList.Sort(New SpamRuleComparer)
        End Sub

        Public Sub EnsureNoOrderGaps()
            'Make sure that the Order numbers are sequencial (without any gaps in between)
            For i As Integer = 0 To Count - 1
                Me(i).Order = i + 1
            Next
        End Sub

        Private Sub EnsureNoDuplicateOrderNumbers()
            'Ensure that the order number doesn't already exist
            Dim orderTable As New Hashtable
            For i As Integer = 0 To Me.Count - 1
                If orderTable.ContainsKey(Me(i).Order) Then
                    Throw New InvalidOperationException("The order number (" & Me(i).Order & ") was found twice.  This value must be unique")
                ElseIf Me(i).Order > -1 Then
                    orderTable.Add(Me(i).Order, True)
                End If
            Next
        End Sub

        Private Sub EnsureNoDuplicateNames()
            Dim nameTable As New Hashtable
            For i As Integer = 0 To Me.Count - 1
                If nameTable.ContainsKey(Me(i).Name) Then
                    Throw New InvalidOperationException("The name (" & Me(i).Name & ") was found twice.  This value must be unique")
                End If
            Next
        End Sub

        Protected Overrides Sub OnClearComplete()
            MyBase.OnClearComplete()
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnInsertComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnInsertComplete(index, value)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnRemoveComplete(ByVal index As Integer, ByVal value As Object)
            MyBase.OnRemoveComplete(index, value)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Protected Overrides Sub OnSetComplete(ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
            MyBase.OnSetComplete(index, oldValue, newValue)
            RaiseEvent ContentsChanged(Me, EventArgs.Empty)
        End Sub

        Private Class SpamRuleComparer
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim srx As SpamRule = CType(x, SpamRule)
                Dim sry As SpamRule = CType(y, SpamRule)

                Return srx.CompareTo(sry)
            End Function
        End Class
    End Class
End Namespace