Imports System.Collections.Specialized
Imports Majodio.Common

<Serializable()> _
Public Class Pop3RelayDetails
    Private _serverAddress As String
    Private _username As String
    Private _password As String
    Private _intervalSeconds As Integer
    Private _deliveryAccounts As Majodio.Common.EmailAddressCollection
    Private _lastProcessedTime As Long

    Public Sub New(ByVal serverAddress As String, ByVal username As String, ByVal password As String, ByVal intervalSeconds As Integer, ByVal lastProcessedTime As Long, ByVal deliveryAccounts As Majodio.Common.EmailAddressCollection)
        _serverAddress = serverAddress.ToLower
        _username = username.ToLower
        _password = password
        _intervalSeconds = intervalSeconds
        _lastProcessedTime = lastProcessedTime
        If Not IsNothing(deliveryAccounts) Then
            _deliveryAccounts = deliveryAccounts
        Else
            _deliveryAccounts = New Majodio.Common.EmailAddressCollection
        End If
    End Sub

    Public Sub New(ByVal serverAddress As String, ByVal username As String, ByVal password As String, ByVal intervalSeconds As Integer, ByVal lastProcessedTime As Long, ByVal deliveryAccount As EmailAddress)
        _serverAddress = serverAddress.ToLower
        _username = username.ToLower
        _password = password
        _intervalSeconds = intervalSeconds
        _lastProcessedTime = lastProcessedTime
        _deliveryAccounts = New EmailAddressCollection
        _deliveryAccounts.Add(deliveryAccount)
    End Sub

    Public Property ServerAddress() As String
        Get
            Return _serverAddress
        End Get
        Set(ByVal value As String)
            _serverAddress = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return _username
        End Get
        Set(ByVal value As String)
            _username = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property IntervalSeconds() As Integer
        Get
            Return _intervalSeconds
        End Get
        Set(ByVal value As Integer)
            _intervalSeconds = value
        End Set
    End Property

    Public Property DeliveryAccounts() As Majodio.Common.EmailAddressCollection
        Get
            Return _deliveryAccounts
        End Get
        Set(ByVal value As Majodio.Common.EmailAddressCollection)
            _deliveryAccounts = value
        End Set
    End Property

    Public Property LastProcessedTime() As Long
        Get
            Return _lastProcessedTime
        End Get
        Set(ByVal value As Long)
            _lastProcessedTime = value
        End Set
    End Property

    Public Function GetNameValueCollection() As NameValueCollection
        Dim RVal As New NameValueCollection
        RVal.Add("serveraddress", ServerAddress)
        RVal.Add("username", Username)
        RVal.Add("password", Password)
        RVal.Add("intervalseconds", IntervalSeconds)
        RVal.Add("lastprocessedtime", LastProcessedTime)
        Return RVal
    End Function
End Class
