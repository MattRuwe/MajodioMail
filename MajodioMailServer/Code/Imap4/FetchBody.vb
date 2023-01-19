Namespace Imap4
    Public Class FetchBodyCollection
        Inherits CollectionBase

        Public Sub Add(ByVal Item As FetchBody)
            MyBase.List.Add(Item)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As FetchBody
            Get
                Return MyBase.List.Item(Index)
            End Get
            Set(ByVal Value As FetchBody)
                MyBase.List.Item(Index) = Value
            End Set
        End Property
    End Class

    Public Class FetchBody
        Private _SectionNumber As String
        Private _HeaderFields As String()
        Private _SectionSpecifier As Imap4FetchSectionSpecifier
        Private _SubSections As FetchBody

        Public Property SectionSpecifier() As Imap4FetchSectionSpecifier
            Get
                Return _SectionSpecifier
            End Get
            Set(ByVal Value As Imap4FetchSectionSpecifier)
                _SectionSpecifier = Value
            End Set
        End Property

        Public Property SectionNumber() As String
            Get
                Return _SectionNumber
            End Get
            Set(ByVal Value As String)
                _SectionNumber = Value
            End Set
        End Property

        Public ReadOnly Property HeaderFields() As String()
            Get
                Return _HeaderFields
            End Get
        End Property

        Public Sub AddHeader(ByVal Name As String)
            If IsNothing(_HeaderFields) Then
                ReDim _HeaderFields(0)
            Else
                ReDim Preserve _HeaderFields(_HeaderFields.GetUpperBound(0) + 1)
            End If
            _HeaderFields(_HeaderFields.GetUpperBound(0)) = Name
        End Sub

        Public Sub AddHeader(ByVal Name As String())
            Dim StartIndex As Integer = 0
            If Not IsNothing(Name) AndAlso Name.GetUpperBound(0) > -1 Then
                If Not IsNothing(_HeaderFields) Then
                    StartIndex = _HeaderFields.GetUpperBound(0)
                End If
                If StartIndex <= 0 Then
                    ReDim _HeaderFields(Name.GetUpperBound(0))
                Else
                    ReDim Preserve _HeaderFields(Name.GetUpperBound(0) + StartIndex)
                End If
                For i As Integer = 0 To Name.GetUpperBound(0)
                    _HeaderFields(StartIndex + i) = Name(i)
                Next
            End If
        End Sub
    End Class
End Namespace