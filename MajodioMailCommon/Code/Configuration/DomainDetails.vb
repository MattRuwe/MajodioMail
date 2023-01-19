Imports System.Collections.Specialized

Namespace Configuration
    <Serializable()> _
    Public Class DomainDetails
        Private _Name As String

        Public Sub New(ByVal Name As String)
            Me.Name = Name
        End Sub

        Public Function GetNameValueCollection() As NameValueCollection
            Dim RVal As New NameValueCollection
            RVal.Add("name", Name)
            Return RVal
        End Function

        Public Property Name() As String
            Get
                If IsNothing(_Name) Then
                    _Name = String.Empty
                End If
                Return _Name.ToLower
            End Get
            Set(ByVal Value As String)
                If IsNothing(Value) Then
                    _Name = String.Empty
                Else
                    _Name = Value.ToLower
                End If
            End Set
        End Property
    End Class
End Namespace