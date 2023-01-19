Imports System.Collections.Generic

Partial Class _Default
    Inherits System.Web.UI.Page

    Private _contentControl As UserControlBase

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsPostBack Then
            Session.Remove("CurrentState")
        End If

        If Not IsNothing(Session("CurrentState")) Then
            SetCurrentControl(Session("CurrentState"), Nothing, Nothing, False)
        ElseIf IsNothing(Session("CurrentState")) AndAlso Not IsNothing(Request("action")) Then
            SetCurrentControl(Request("action"), Nothing, Nothing, True)
        Else
            SetCurrentControl(String.Empty, Nothing, Nothing, True)
        End If
    End Sub

    Private Sub SetCurrentControl(ByVal content As String, ByVal arguments As Dictionary(Of String, Object), ByVal alternateViewState As StateBag, ByVal isInitialLoad As Boolean)
        Dim controlPath As String = String.Empty

        Session("CurrentState") = content

        Select Case content.Trim.ToLower
            Case ""
                controlPath = "Logon"
            Case "mailmain"
                controlPath = "MailMain"
            Case Else
                controlPath = "UnknownState"
        End Select
        _contentControl = LoadControl("~/UserControls/" & controlPath & ".ascx")
        _contentControl.Initialize(arguments, alternateViewState, isInitialLoad)

        upnlContent.ContentTemplateContainer.Controls.Clear()
        upnlContent.ContentTemplateContainer.Controls.Add(_contentControl)

        MyBase.Title = _contentControl.PageTitle
        AddHandler _contentControl.StateChanged, AddressOf ContentControl_StateChanged
    End Sub

    Private Sub ContentControl_StateChanged(ByVal sender As Object, ByVal e As StateChangedEventArgs)
        SetCurrentControl(e.NewState, e.Arguments, e.AlternateViewState, True)
    End Sub
End Class