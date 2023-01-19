Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class StateChangedEventArgs
    Inherits EventArgs

    Private _newState As String
    Private _arguments As Dictionary(Of String, Object)
    Private _alternateViewState As StateBag

    Public Sub New()
        _arguments = New Dictionary(Of String, Object)
    End Sub

    Public Sub New(ByVal newState As String)
        Me.New()
        _newState = newState
        _alternateViewState = Nothing
    End Sub

    Public Sub New(ByVal newState As String, ByVal arguments As Dictionary(Of String, Object))
        _newState = newState
        _arguments = arguments
        _alternateViewState = Nothing
    End Sub

    Public Sub New(ByVal newState As String, ByVal arguments As Dictionary(Of String, Object), ByVal alternateViewstate As StateBag)
        _newState = newState
        _arguments = arguments
        _alternateViewState = alternateViewstate
    End Sub

    Public Property NewState() As String
        Get
            Return _newState
        End Get
        Set(ByVal value As String)
            _newState = value
        End Set
    End Property

    Public ReadOnly Property Arguments() As Dictionary(Of String, Object)
        Get
            Return _arguments
        End Get
    End Property

    Public ReadOnly Property AlternateViewState() As StateBag
        Get
            Return _alternateViewState
        End Get
    End Property
End Class