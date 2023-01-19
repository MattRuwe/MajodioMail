Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class UserControlBase
    Inherits System.Web.UI.UserControl

    Private _arguments As Dictionary(Of String, Object)
    Private _alternateViewstate As StateBag
    Private _isInitialLoad As Boolean

    Public Event StateChanged As StateChangedEventHandler

    Public Sub Initialize(ByVal arguments As Dictionary(Of String, Object), ByVal alternateViewState As StateBag, ByVal isInitialLoad As Boolean)
        _arguments = arguments
        _alternateViewstate = alternateViewState
        _isInitialLoad = isInitialLoad
    End Sub

    Public Sub RaiseStateChanged(ByVal newState As String, ByVal arguments As Dictionary(Of String, Object))
        Dim eventArgs As New StateChangedEventArgs(newState, arguments, Me.ViewState)
        RaiseEvent StateChanged(Me, eventArgs)
    End Sub

    Protected ReadOnly Property IsInitialLoad() As Boolean
        Get
            Return _isInitialLoad
        End Get
    End Property

    Protected ReadOnly Property Arguments() As Dictionary(Of String, Object)
        Get
            Return _arguments
        End Get
    End Property

    Protected ReadOnly Property AlternateViewstate() As StateBag
        Get
            Return _alternateViewstate
        End Get
    End Property

    Public Overridable ReadOnly Property PageTitle() As String
        Get
            Return "Majodio Mail Web Access"
        End Get
    End Property

    Public ReadOnly Property LoggedIn() As Boolean
        Get
            Return (Not IsNothing(LoggedInUsername) AndAlso Not IsNothing(LoggedInDomain))
        End Get
    End Property

    Public ReadOnly Property LoggedInDomain() As String
        Get
            Return Session("LoggedInDomain")
        End Get
    End Property

    Public ReadOnly Property LoggedInUsername() As String
        Get
            Return Session("LoggedInUsername")
        End Get
    End Property

    Public Sub LogOut()
        Session.RemoveAll()
    End Sub

    Public Sub SetLogonInformation(ByVal domain As String, ByVal username As String)
        Session("LoggedInDomain") = domain
        Session("LoggedInUsername") = username
    End Sub
End Class
