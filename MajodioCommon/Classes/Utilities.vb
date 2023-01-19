Imports System.Diagnostics
Imports System.Reflection


Namespace Majodio.Common
    Public NotInheritable Class Utilities

        Private Shared StartTime As DateTime = DateTime.Now

        <Conditional("DEBUG")> _
        Public Shared Sub TraceMe(ByVal Msg As String)
            Dim Trace As New StackTrace
            Dim ParentFrame As StackFrame = Trace.GetFrame(1)
            Dim ParentMethod As MethodBase = ParentFrame.GetMethod()
            Dim Now As DateTime = DateTime.Now
            Debug.WriteLine(((Now.Ticks - StartTime.Ticks) / TimeSpan.TicksPerSecond).ToString & ": " & ParentMethod.DeclaringType.Name & "." & ParentMethod.Name & IIf(Not IsNothing(Msg) AndAlso Msg.Trim.Length > 0, ": " & Msg, String.Empty))
        End Sub

        <Conditional("DEBUG")> _
        Public Shared Sub TraceMe()
            TraceMe(Nothing)
        End Sub

        Public Shared Sub GcFullCollect()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Sub

    End Class
End Namespace