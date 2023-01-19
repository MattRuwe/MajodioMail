Namespace Headers
    Public Class MimeMessageHeaderCollection
        Inherits System.Collections.CollectionBase

        Public Function Add(ByVal Header As MimeMessageHeaderBase) As MimeMessageHeaderBase
            If IsNothing(Header) Then
                Throw New NullReferenceException("Header must not be null")
            End If
            If IsNothing(Header.Name) Then
                Throw New NullReferenceException("Header.Name must not be null")
            End If
            Dim Index As Integer = IndexOf(Header.Name)
            If Index > -1 Then
                MyBase.List.Item(Index) = Header
            Else
                MyBase.List.Add(Header)
            End If
            Return Header
        End Function

        Public Function Add(ByVal Headers As MimeMessageHeaderCollection) As MimeMessageHeaderCollection
            For i As Integer = 0 To Headers.Count - 1
                Add(Headers(i))
            Next
            Return Headers
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Header As MimeMessageHeaderBase)
            If Index > MyBase.Count Then
                Throw New IndexOutOfRangeException("The value " & Index & " is larger than the size of the collection " & MyBase.Count)
            End If
            MyBase.List.Insert(Index, Header)
        End Sub

        Public Function IndexOf(ByVal HeaderName As String) As Integer
            Return IndexOf(0, HeaderName)
        End Function

        Public Function IndexOf(ByVal StartIndex As Integer, ByVal HeaderName As String) As Integer
            Dim i As Integer
            Dim RVal As Integer = -1
            Dim CurrentHeader As MimeMessageHeaderBase
            If StartIndex < 0 Then
                Throw New IndexOutOfRangeException("Start index cannot be less than 0")
            End If
            If StartIndex > MyBase.List.Count AndAlso MyBase.List.Count > 0 Then
                Throw New IndexOutOfRangeException("Start index must be less than Count")
            End If
            If Not IsNothing(HeaderName) Then
                For i = StartIndex To MyBase.List.Count - 1
                    CurrentHeader = CType(MyBase.List(i), MimeMessageHeaderBase)
                    If Not IsNothing(CurrentHeader) AndAlso Not IsNothing(CurrentHeader.Name) AndAlso CurrentHeader.Name.Trim.ToLower = HeaderName.Trim.ToLower Then
                        RVal = i
                        Exit For
                    End If
                Next
            End If
            Return RVal
        End Function

        Public Function IndexOf(ByVal HeaderType As MimeMessageHeaderTypes) As Integer
            Return IndexOf(0, HeaderType)
        End Function

        Public Function IndexOf(ByVal StartIndex As Integer, ByVal HeaderType As MimeMessageHeaderTypes) As Integer
            Dim RVal As Integer = -1
            For i As Integer = StartIndex To MyBase.List.Count - 1
                If CType(MyBase.List(i), Headers.MimeMessageHeaderBase).HeaderType = HeaderType Then
                    RVal = i
                    Exit For
                End If
            Next
            Return RVal
        End Function

        Default Public Property Item(ByVal Index As Integer) As MimeMessageHeaderBase
            Get
                If Index < MyBase.List.Count Then
                    Return CType(MyBase.List(Index), MimeMessageHeaderBase)
                Else
                    Throw New IndexOutOfRangeException
                End If
            End Get
            Set(ByVal Value As MimeMessageHeaderBase)
                MyBase.List(Index) = Value
            End Set
        End Property

        Default Public Property Item(ByVal HeaderName As String) As MimeMessageHeaderBase
            Get
                Dim HeaderIndex As Integer = IndexOf(HeaderName)
                If HeaderIndex > -1 Then
                    Return CType(MyBase.List.Item(HeaderIndex), MimeMessageHeaderBase)
                Else
                    Throw New IndexOutOfRangeException
                End If
            End Get
            Set(ByVal Value As MimeMessageHeaderBase)
                Dim HeaderIndex As Integer = IndexOf(HeaderName)
                If HeaderIndex > -1 Then
                    MyBase.List.Item(HeaderIndex) = Value
                Else
                    Throw New IndexOutOfRangeException
                End If
            End Set
        End Property

        Default Public Property Item(ByVal HeaderType As MimeMessageHeaderTypes) As MimeMessageHeaderBase
            Get
                Dim Index As Integer
                Dim RVal As MimeMessageHeaderBase = Nothing

                Index = IndexOf(HeaderType)
                If Index > -1 Then
                    RVal = Item(Index)
                End If
                Return RVal
            End Get
            Set(ByVal value As MimeMessageHeaderBase)
                Dim Index As Integer

                Index = IndexOf(HeaderType)
                If Index > -1 Then
                    Item(Index) = value
                End If
            End Set
        End Property

        Public Overloads ReadOnly Property Count(ByVal HeaderName As String) As Integer
            Get
                Dim Index As Integer = IndexOf(HeaderName)
                Dim RVal As Integer = 0
                While Index > -1
                    RVal += 1
                    Index = IndexOf(Index + 1, HeaderName)
                End While
                Return RVal
            End Get
        End Property
    End Class
End Namespace