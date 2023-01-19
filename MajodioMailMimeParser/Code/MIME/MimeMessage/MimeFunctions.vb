Imports Majodio.Mail.Mime.headers
Imports System.Text.RegularExpressions

Module MimeFunctions
    'Public Function MajodioSplit(ByVal Expression As String, ByVal Delimeter As String, ByVal QuoteDelimeter As String) As String()
    '    Dim TmpRVal As New ArrayList
    '    Dim RVal As String() = New String() {}
    '    Dim StartIndex As Integer = 0
    '    Dim EndIndex As Integer = 0
    '    For i As Integer = 0 To Expression.Length - 1
    '        'Check to see if the current char is a quoted character
    '        If Expression.Substring(i, QuoteDelimeter.Length) = QuoteDelimeter Then
    '            'It is, so now go into CurrentlyQuoted mode
    '            i = Expression.IndexOf(QuoteDelimeter, i + QuoteDelimeter.Length)
    '            If i = -1 Then
    '                i = Expression.Length
    '            End If
    '        ElseIf Expression.Substring(i, Delimeter.Length) = Delimeter Then
    '            'Found a delimeter
    '            'StartIndex = i + Delimeter.Length
    '            'EndIndex = Expression.IndexOf(Delimeter, StartIndex)
    '            EndIndex = i
    '            TmpRVal.Add(Expression.Substring(StartIndex, EndIndex - StartIndex))
    '            StartIndex = EndIndex + 1
    '        End If
    '    Next
    '    If StartIndex < Expression.Length - 1 Then
    '        TmpRVal.Add(Expression.Substring(StartIndex))
    '    End If
    '    ReDim RVal(TmpRVal.Count - 1)
    '    TmpRVal.CopyTo(RVal, 0)
    '    Return RVal
    'End Function

    'Public Function MajodioTrim(ByVal Expression As String, ByVal CharToTrim As Char) As String
    '    Dim StartIndex As Integer
    '    Dim EndIndex As Integer
    '    Dim RVal As String = Expression
    '    For i As Integer = 0 To Expression.Length - 1
    '        If Expression.Substring(i, 1) <> CharToTrim Then
    '            StartIndex = i
    '            Exit For
    '        End If
    '    Next
    '    For i As Integer = Expression.Length - 1 To 0 Step -1
    '        If Expression.Substring(i, 1) <> CharToTrim Then
    '            EndIndex = i
    '            Exit For
    '        End If
    '    Next
    '    If StartIndex < EndIndex Then
    '        RVal = Expression.Substring(StartIndex, EndIndex - StartIndex + 1)
    '    End If
    '    Return RVal
    'End Function
End Module