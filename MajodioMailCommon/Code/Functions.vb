Imports System.IO
Imports System.Net
Imports System.Management

Public Module Functions
    
    Public Function GetApplicationDirectory() As String
        Dim RK As Registry
        Dim RVal As String = Directory.GetCurrentDirectory
        Dim TmpRVal As Object
        RK = New Registry
        TmpRVal = RK.GetValue("ApplicationDirectory")
        If IsNothing(TmpRVal) Then
            RK.SetValue("ApplicationDirectory", RVal)
        ElseIf Not TmpRVal.GetType Is GetType(String) Then
            Try
                RK.SetValue("ApplicationDirectory", RVal)
            Catch exc As Exception
                'WriteLog("An error occurred while attempting to read the application directory from the registry.  The error details follow:" & vbCrLf & exc.Source & vbCrLf & exc.Message & vbCrLf & exc.StackTrace)
            End Try
        Else
            RVal = TmpRVal
        End If
        Return RVal.ToLower
    End Function

    Public Sub SetApplicationDirectory(ByVal Value As String)
        Dim RK As Registry = Nothing
        RK = New Registry
        RK.SetValue("ApplicationDirectory", Value)
    End Sub

    Public Function MajodioQuoteSplit(ByVal Expression As String, ByVal Delimeter As String, ByVal RemoveQuotes As Boolean) As String()
        Dim TmpRVal As New ArrayList
        Dim RVal As String() = New String() {}
        Dim StartIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim TmpString As String
        Dim CurrentChar As String
        For i As Integer = 0 To Expression.Length - 1
            CurrentChar = Expression.Substring(i, 1)
            If CurrentChar = """" Then
                i = Expression.IndexOf("""", i + 1)
                If i = -1 Then
                    i = Expression.Length
                End If
            ElseIf CurrentChar = "(" Then
                i = Expression.IndexOf(")", i + 1)
                If i = -1 Then
                    i = Expression.Length
                End If
            ElseIf Expression.Substring(i, Delimeter.Length) = Delimeter Then
                'Found a delimeter
                EndIndex = i
                TmpString = Expression.Substring(StartIndex, EndIndex - StartIndex)
                If RemoveQuotes Then
                    TmpString = MajodioTrim(TmpString, "(")
                    TmpString = MajodioTrim(TmpString, ")")
                    TmpString = MajodioTrim(TmpString, """")
                End If
                TmpRVal.Add(TmpString)
                StartIndex = EndIndex + 1
            End If
        Next
        If StartIndex < Expression.Length Then '- 1 Then
            TmpString = Expression.Substring(StartIndex, Expression.Length - StartIndex)
            If RemoveQuotes Then
                TmpString = MajodioTrim(TmpString, "(")
                TmpString = MajodioTrim(TmpString, ")")
                TmpString = MajodioTrim(TmpString, """")
            End If
            TmpRVal.Add(TmpString)
        End If
        ReDim RVal(TmpRVal.Count - 1)
        TmpRVal.CopyTo(RVal, 0)
        Return RVal
    End Function

    Public Function GetLocalIpAddress() As String
        Dim strHostName As String
        strHostName = System.Net.Dns.GetHostName()
        Dim ipEntry As IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
        Dim addr As IPAddress() = ipEntry.AddressList
        Dim RVal As String = String.Empty
        If Not IsNothing(addr) Then
            If addr.GetUpperBound(0) > -1 Then
                RVal = addr(0).ToString()
            End If
        End If
        If RVal.Trim.Length = 0 Then
            RVal = "127.0.0.1"
        End If
        Return RVal
    End Function

    Public Function GetFqdn() As String
        Dim MC As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim MO As ManagementObject
        Dim MOC As ManagementObjectCollection = MC.GetInstances()

        Dim RVal As String = String.Empty

        For Each MO In MOC
            If CType(MO("ipEnabled"), Boolean) Then
                RVal = CType(MO("DNSHostname"), String).Trim() & "." & CType(MO("DNSDomain"), String)
                Exit For
            End If
        Next
        Return RVal
    End Function
End Module
