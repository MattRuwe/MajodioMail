Namespace Headers
    Public MustInherit Class MimeMessageHeaderBase
        Private _Name As String
        Private _Value As String

        Public Sub New()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        '''   The method is protected because not all headers implement Name/Value 
        '''   in a standard way.
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <param name="Value"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	2/28/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub New(ByVal Name As String, ByVal Value As String)
            _Name = Name
            _Value = Value
        End Sub

        Public Overridable Function GetFormattedHeader() As String
            Return _Name & ": " & _Value
        End Function

        Public MustOverride ReadOnly Property HeaderType() As MimeMessageHeaderTypes

        Public Overridable Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        Public Overridable Property Value() As String
            Get
                Return _Value
            End Get
            Set(ByVal Value As String)
                _Value = Value
            End Set
        End Property
    End Class
End Namespace