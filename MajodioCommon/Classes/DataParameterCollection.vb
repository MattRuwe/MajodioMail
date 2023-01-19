'Imports System.Collections
'Imports System.Collections.Generic
'Imports System.Data
'Imports System.Data.common


'Public Class DataParameterCollection
'    Inherits System.Data.Common.DbParameterCollection

'    Private _items As List(Of DataParameter)

'    Public Overrides Function Add(ByVal value As Object) As Integer
'        Me.InnerList.Add(DirectCast(value, DataParameter))
'        Return (Me.Count - 1)
'    End Function

'    Public Overrides Sub AddRange(ByVal values As System.Array)
'        For Each parameter1 As DataParameter In values
'            Me.InnerList.Add(parameter1)
'        Next
'    End Sub

'    Public Overrides Sub Clear()
'        Dim list1 As List(Of DataParameter) = Me.InnerList
'        If (Not list1 Is Nothing) Then
'            list1.Clear()
'        End If
'    End Sub

'    Public Overloads Overrides Function Contains(ByVal value As Object) As Boolean

'    End Function

'    Public Overloads Overrides Function Contains(ByVal value As String) As Boolean

'    End Function

'    Public Overrides Sub CopyTo(ByVal array As System.Array, ByVal index As Integer)

'    End Sub

'    Public Overrides ReadOnly Property Count() As Integer
'        Get

'        End Get
'    End Property

'    Public Overrides Function GetEnumerator() As System.Collections.IEnumerator

'    End Function

'    Protected Overloads Overrides Function GetParameter(ByVal index As Integer) As System.Data.Common.DbParameter

'    End Function

'    Protected Overloads Overrides Function GetParameter(ByVal parameterName As String) As System.Data.Common.DbParameter

'    End Function

'    Public Overloads Overrides Function IndexOf(ByVal value As Object) As Integer

'    End Function

'    Public Overloads Overrides Function IndexOf(ByVal parameterName As String) As Integer

'    End Function

'    Public Overrides Sub Insert(ByVal index As Integer, ByVal value As Object)

'    End Sub

'    Public Overrides ReadOnly Property IsFixedSize() As Boolean
'        Get

'        End Get
'    End Property

'    Public Overrides ReadOnly Property IsReadOnly() As Boolean
'        Get

'        End Get
'    End Property

'    Public Overrides ReadOnly Property IsSynchronized() As Boolean
'        Get

'        End Get
'    End Property

'    Public Overrides Sub Remove(ByVal value As Object)

'    End Sub

'    Public Overloads Overrides Sub RemoveAt(ByVal index As Integer)

'    End Sub

'    Public Overloads Overrides Sub RemoveAt(ByVal parameterName As String)

'    End Sub

'    Protected Overloads Overrides Sub SetParameter(ByVal index As Integer, ByVal value As System.Data.Common.DbParameter)

'    End Sub

'    Protected Overloads Overrides Sub SetParameter(ByVal parameterName As String, ByVal value As System.Data.Common.DbParameter)

'    End Sub

'    Public Overrides ReadOnly Property SyncRoot() As Object
'        Get

'        End Get
'    End Property

'    Private ReadOnly Property InnerList() As List(Of DataParameter)
'        Get
'            Dim rVal As List(Of DataParameter) = Me._items
'            If (rVal Is Nothing) Then
'                rVal = New List(Of DataParameter)
'                Me._items = rVal
'            End If
'            Return rVal
'        End Get
'    End Property


'End Class
