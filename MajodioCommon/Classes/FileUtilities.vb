Imports System.IO
Namespace Majodio.Common
    Public Class IoUtilities
        Public Shared Function GetFilename(ByVal Path As String, ByVal VerifyFile As Boolean) As String
            Dim RVal As String = String.Empty
            Dim Index As Int32

            Index = Path.LastIndexOf("\")
            If Index > -1 Then
                RVal = Path.Substring(Index + 1)
                If VerifyFile AndAlso Not File.Exists(Path) Then
                    RVal = String.Empty
                End If
            End If
            Return RVal
        End Function

        Public Shared Function GetFoldername(ByVal Path As String, ByVal VerifyFolder As Boolean) As String
            Dim RVal As String = String.Empty
            Dim Index As Int32
            While Path.EndsWith("\")
                Path = Path.Substring(0, Path.Length - 1)
            End While

            Index = Path.LastIndexOf("\")
            If Index > -1 Then
                RVal = Path.Substring(Index + 1)
                If VerifyFolder AndAlso Not Directory.Exists(Path) Then
                    RVal = String.Empty
                End If
            End If
            Return RVal
        End Function

        'Public Shared Function GetDirectoryName(ByRef Path As String) As String
        '    Dim RVal As String
        '    Dim Index As Integer

        '    RVal = System.IO.Path.GetDirectoryName(Path)
        '    Index = RVal.LastIndexOf(System.IO.Path.PathSeparator, Path.Length - 2)
        '    If Index > -1 Then
        '        RVal = Path.Substring(Index)
        '        Path = Path.Substring(0, Index)
        '    End If
        '    Return RVal
        'End Function
    End Class
End Namespace