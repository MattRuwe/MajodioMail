
Imports System.web
Imports System.Web.ui

Namespace Majodio
    Public Module MajodioDebug
        Public Sub WriteChildControls(ByVal ParentControl As System.Web.UI.Control, ByVal CurrentContext As System.Web.HttpContext, Optional ByVal Level As Integer = 0)
            CurrentContext.Trace.IsEnabled = True
            Dim ChildControl As Control
            If Level = 0 Then
                CurrentContext.Trace.Write("Majodio.Debug", Space(Level * 3) & "Parent UniqueId = " & ParentControl.UniqueID & " Type = " & ParentControl.GetType.ToString)
            End If
            For Each ChildControl In ParentControl.Controls
                CurrentContext.Trace.Write("Majodio.Debug", Space(Level * 3) & "Child UniqueId = " & ChildControl.UniqueID & " Type = " & ChildControl.GetType.ToString)
                WriteChildControls(ChildControl, CurrentContext, Level + 1)
            Next
        End Sub
    End Module
End Namespace