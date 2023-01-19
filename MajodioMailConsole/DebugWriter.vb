Imports System.IO
Imports Majodio.Mail.common

Public Class DebugWriter
    Inherits TextWriter

    Private _T As System.Windows.Forms.TextBox
    Private _SW As StreamWriter

    Public Sub New(ByVal T As System.Windows.Forms.TextBox)
        Dim dirPath As String

        If Not Directory.Exists(Majodio.Mail.Common.GetApplicationDirectory & "\" & LOG_FILE_DIRECTORY) Then
            Directory.CreateDirectory(Majodio.Mail.Common.GetApplicationDirectory & "\" & LOG_FILE_DIRECTORY)
        End If

        dirPath = Majodio.Mail.Common.GetApplicationDirectory & "\" & LOG_FILE_DIRECTORY & "\" & Guid.NewGuid.ToString() & ".txt"
        _T = T
        _SW = New StreamWriter(dirPath)
    End Sub

    Public Overrides ReadOnly Property Encoding() As System.Text.Encoding
        Get
            Return System.Text.Encoding.UTF8
        End Get
    End Property

    Private Delegate Sub DelWrite(ByVal Value As String)
    Public Overrides Sub Write(ByVal Value As String)
        If _T.InvokeRequired Then
            Dim D As New DelWrite(AddressOf Write)
            _T.Invoke(D, New Object() {Value})
        Else
            _T.AppendText(Value)
            _SW.Write(Value)
            _SW.Flush()
        End If
    End Sub

    Private Delegate Sub DelWriteLine(ByVal Value As String)
    Public Overrides Sub WriteLine(ByVal value As String)
        If _T.InvokeRequired Then
            Dim D As New DelWriteLine(AddressOf WriteLine)
            _T.Invoke(D, New Object() {value})
        ElseIf Not _T.IsDisposed Then
            _T.AppendText(value & vbCrLf)
            _SW.WriteLine(value)
            _SW.Flush()
        End If
    End Sub
End Class