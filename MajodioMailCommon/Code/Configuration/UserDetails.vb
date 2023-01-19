Imports System.Collections.Specialized
Namespace Configuration
    <Serializable()> _
    Public Class UserDetails
        Public Sub New(ByVal Username As String, ByVal Password As String, ByVal UniqueId As Integer)
            Me.Username = Username
            Me.Password = Password
            Me.UniqueId = UniqueId
        End Sub

        Public Function GetNameValueCollection() As NameValueCollection
            Dim RVal As New NameValueCollection
            RVal.Add("username", Username)
            RVal.Add("password", Password)
            RVal.Add("unique_id", UniqueId)
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
                ElseIf Value.IndexOf(" ") > -1 OrElse Value.IndexOf("@") > -1 Then
                    Throw New ArgumentException("Error setting username, the value (" & Value & ") is invalid")
                Else
                    _Username = Value.ToLower
                End If
            End Set
        End Property

        Private _Password As String
        Public Property Password() As String
            Get
                If IsNothing(_Password) Then
                    Return String.Empty
                Else
                    Return _Password
                End If
            End Get
            Set(ByVal Value As String)
                If IsNothing(Value) Then
                    _Username = String.Empty
                Else
                    _Password = Value
                End If
            End Set
        End Property

        Private _UniqueId As Integer
        Public Property UniqueId() As Integer
            Get
                Return _UniqueId
            End Get
            Set(ByVal Value As Integer)
                _UniqueId = Value
            End Set
        End Property

        Private _LastFailedLogon As Date
        Public Property LastFailedLogon() As Date
            Get
                Return _LastFailedLogon
            End Get
            Set(ByVal value As Date)
                _LastFailedLogon = value
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return Username
        End Function
    End Class
End Namespace