Namespace Headers
    ''' -----------------------------------------------------------------------------
    ''' Project	 : MajodioMailMimeParser
    ''' Class	 : Mail.Mime.Headers.Received
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	7/12/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class Received
        Inherits MimeMessageHeaderBase

        Public Sub New(ByVal HeaderValue As String)
            MyBase.New("received", HeaderValue)
        End Sub

        Public Overrides Property Name() As String
            Get
                Return "received"
            End Get
            Set(ByVal Value As String)
                Throw New NotImplementedException("This method cannot be called")
            End Set
        End Property

        Public ReadOnly Property DateTime() As String
            Get
                Dim SemicolonIndex As Integer
                Dim RVal As String = String.Empty
                SemicolonIndex = MyBase.Value.IndexOf(";")
                If SemicolonIndex > -1 Then
                    RVal = MyBase.Value.Substring(SemicolonIndex + 1)
                End If
                Return RVal.Trim
            End Get
        End Property

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.Received
            End Get
        End Property
    End Class
End Namespace