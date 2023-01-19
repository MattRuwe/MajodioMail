Imports Majodio.Mail.Common.Grouping

Namespace Grouping
    Public Class EmbeddedGroup
        Private _OutterGroupText As String
        Private _OutterGroupType As GroupingType
        Private _StartIndex As Integer
        Private _InnerGroups As EmbeddedGroupCollection
        Private _ContainsGroup As Boolean

        Public Sub New(ByVal S As String)
            _StartIndex = -1
            _OutterGroupText = String.Empty
            _OutterGroupType = GroupingType.None
            _ContainsGroup = False
            Parse(S)
        End Sub

        Public Sub New(ByVal S As String, ByVal StartIndex As Integer, ByVal GT As GroupingType)
            _StartIndex = StartIndex
            _OutterGroupText = String.Empty
            _OutterGroupType = GT
            _ContainsGroup = False
            Parse(S)
        End Sub

        Public ReadOnly Property StartIndex() As Integer
            Get
                Return _StartIndex
            End Get
        End Property

        Public ReadOnly Property GroupType() As GroupingType
            Get
                Return _OutterGroupType
            End Get
        End Property

        Public ReadOnly Property GroupText() As String
            Get
                Return _OutterGroupText
            End Get
        End Property

        Public ReadOnly Property ContainsGroup() As Boolean
            Get
                Return _ContainsGroup
            End Get
        End Property

        Public ReadOnly Property InnerGroup() As EmbeddedGroupCollection
            Get
                If IsNothing(_InnerGroups) Then
                    _InnerGroups = New EmbeddedGroupCollection
                End If
                Return _InnerGroups
            End Get
        End Property

        Public Shared Function IsValidGroupString(ByVal Expression As String) As Boolean
            Dim RVal As Boolean
            If Majodio.Functions.GetCharCount(Expression, "("c) = GetCharCount(Expression, ")"c) And _
                    Majodio.Functions.GetCharCount(Expression, "["c) = GetCharCount(Expression, "]"c) And _
                    Majodio.Functions.GetCharCount(Expression, "{"c) = GetCharCount(Expression, "}"c) And _
                    Majodio.Functions.GetCharCount(Expression, """"c) Mod 2 = 0 Then
                RVal = True
            Else
                RVal = False
            End If
            Return RVal
        End Function

        'Public Function IsIndexStartIndex(ByVal Index As Integer, ByVal Recursive As Boolean) As Boolean
        '    Dim RVal As Boolean = False
        '    For i As Integer = 0 To InnerGroup.Count - 1
        '        If InnerGroup(i).StartIndex = Index Then
        '            RVal = True
        '            Exit For
        '        ElseIf Recursive Then
        '            RVal = InnerGroup(i).IsIndexStartIndex(Index, True)
        '        End If
        '    Next
        '    Return RVal
        'End Function

        Public Function GetIndex(ByVal StartIndex As Integer) As Integer
            Dim RVal As Integer = -1
            For i As Integer = 0 To InnerGroup.Count - 1
                If InnerGroup(i).StartIndex = StartIndex Then
                    RVal = i
                End If
            Next
            Return RVal
        End Function

        Public Sub Parse(ByVal Expression As String)
            Dim M As GroupMatchCollection
            Dim ES As EmbeddedGroup
            Dim CurrentPosition As Integer = 0
            Dim TotalCharsInInnerstring As Integer = 0
            If IsValidGroupString(Expression) Then
                M = GetOutterGroupMatches(Expression)
                If Not IsNothing(M) AndAlso M.Count > 0 Then
                    If M(0).Index > 0 Then
                        _OutterGroupText = Expression.Substring(0, M(0).Index)
                    End If
                    CurrentPosition = M(0).Index + M(0).Length
                    For i As Integer = 0 To M.Count - 1
                        _ContainsGroup = True
                        ES = New EmbeddedGroup(M(i).Value.Substring(1, M(i).Value.Length - 2), M(i).Index, M(i).GroupType)
                        InnerGroup.Add(ES)
                        If M(i).Index > 0 And CurrentPosition < Expression.Length Then
                            If i < M.Count - 1 Then
                                _OutterGroupText &= Expression.Substring(CurrentPosition, M(i + 1).Index - CurrentPosition)
                                CurrentPosition = M(i + 1).Index + M(i + 1).Length
                            Else
                                _OutterGroupText &= Expression.Substring(CurrentPosition)
                            End If
                        ElseIf M(i).Index = 0 And i < M.Count - 1 Then
                            _OutterGroupText &= Expression.Substring(CurrentPosition, M(i + 1).Index - CurrentPosition)
                            CurrentPosition = M(i + 1).Index + M(i + 1).Length
                        End If
                    Next
                Else
                    _OutterGroupText = Expression
                End If
            Else
                Throw New InvalidGroupException("The number of opening group markers needs to match the number of closing group markers")
            End If
        End Sub

        Private Function GetOutterGroupMatches(ByVal Expression As String) As GroupMatchCollection
            Dim RVal As New GroupMatchCollection
            Dim Count As Integer = 0
            Dim CurrentChar As Char
            Dim InsideGroup As Boolean = False
            Dim StartIndex As Integer = -1
            Dim EndIndex As Integer = -1
            Dim StartingGroupType As GroupingType
            Dim InsideQuotes As Boolean = False
            If Not IsNothing(Expression) Then
                For i As Integer = 0 To Expression.Length - 1
                    CurrentChar = Expression.Substring(i, 1)
                    If CurrentChar = "(" Or CurrentChar = "[" Or CurrentChar = "{" Or (CurrentChar = """" And Not InsideQuotes) Then
                        If Not InsideGroup Then
                            StartIndex = i
                            Select Case CurrentChar
                                Case "("
                                    StartingGroupType = GroupingType.Parenthesis
                                Case "["
                                    StartingGroupType = GroupingType.Brackets
                                Case "{"
                                    StartingGroupType = GroupingType.Braces
                                Case """"
                                    StartingGroupType = GroupingType.DoubleQuotes
                                    InsideQuotes = True
                            End Select
                        End If
                        InsideGroup = True
                        Count += 1
                    ElseIf (CurrentChar = ")" Or CurrentChar = "]" Or CurrentChar = "}" Or (CurrentChar = """" And InsideQuotes)) And InsideGroup Then
                        Count -= 1
                        If Count = 0 Then
                            If (StartingGroupType = GroupingType.Parenthesis And CurrentChar = ")") Or _
                               (StartingGroupType = GroupingType.Brackets And CurrentChar = "]") Or _
                               (StartingGroupType = GroupingType.Braces And CurrentChar = "}") Or _
                               (StartingGroupType = GroupingType.DoubleQuotes And CurrentChar = """") Then
                                EndIndex = i
                                RVal.Add(New GroupMatch(StartIndex, Expression.Substring(StartIndex, (EndIndex - StartIndex) + 1), StartingGroupType))
                                InsideGroup = False
                                If InsideQuotes And CurrentChar = """" Then
                                    InsideQuotes = False
                                End If
                            End If

                        End If
                    End If
                Next
            End If
            Return RVal
        End Function

        Public Overrides Function ToString() As String
            Dim RVal As String = _OutterGroupText
            Dim LeftOver As String = String.Empty
            Dim CurrentIndex As Integer = 0
            Dim TmpString As String = String.Empty
            For i As Integer = 0 To InnerGroup.Count - 1
                TmpString = InnerGroup(i).ToString
                If InnerGroup(i).StartIndex > -1 Then
                    LeftOver = RVal.Substring(InnerGroup(i).StartIndex)
                    RVal = RVal.Substring(0, InnerGroup(i).StartIndex)
                    If TmpString.Length > 0 Then
                        Select Case InnerGroup(i).GroupType
                            Case GroupingType.Parenthesis
                                RVal &= "(" & TmpString & ")"
                            Case GroupingType.Brackets
                                RVal &= "[" & TmpString & "]"
                            Case GroupingType.Braces
                                RVal &= "{" & TmpString & "}"
                            Case GroupingType.DoubleQuotes
                                RVal &= """" & TmpString & """"
                        End Select
                    End If
                    RVal &= LeftOver
                ElseIf InnerGroup(i).StartIndex >= RVal.Length - 1 Then
                    Select Case InnerGroup(i).GroupType
                        Case GroupingType.Parenthesis
                            RVal &= "(" & TmpString & ")"
                        Case GroupingType.Brackets
                            RVal &= "[" & TmpString & "]"
                        Case GroupingType.Braces
                            RVal &= "{" & TmpString & "}"
                        Case GroupingType.DoubleQuotes
                            RVal &= """" & TmpString & """"
                    End Select
                End If
            Next
            Return RVal
        End Function

    End Class

End Namespace