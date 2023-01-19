Imports System.Collections.Specialized
Imports Majodio.Common

Namespace Configuration
    <Serializable()> _
    Public Class AliasDetails
        Public Sub New(ByVal Username As String, ByVal IsRegex As Boolean)
            Me.Username = Username
            Me.IsRegex = IsRegex
        End Sub

        Public Function GetNameValueCollection() As NameValueCollection
            Dim RVal As New NameValueCollection
            RVal.Add("username", Username)
            RVal.Add("isregex", IsRegex)
            Return RVal
        End Function

        Private _Username As String
        Public Property Username() As String
            Get
                If IsNothing(_Username) Then
                    Return String.Empty
                Else
                    Return _Username.ToLower
                End If
            End Get
            Set(ByVal Value As String)
                If IsNothing(Value) Then
                    _Username = String.Empty
                Else
                    _Username = Value.ToLower
                End If
            End Set
        End Property

        Private _RealAddresses As EmailAddressCollection
        Public ReadOnly Property RealAddresses() As EmailAddressCollection
            Get
                If IsNothing(_RealAddresses) Then
                    _RealAddresses = New EmailAddressCollection
                End If
                Return _RealAddresses
            End Get
        End Property

        Private _IsRegex As Boolean = False
        Public Property IsRegex() As Boolean
            Get
                Return _IsRegex
            End Get
            Set(ByVal value As Boolean)
                _IsRegex = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Username
        End Function
    End Class
End Namespace