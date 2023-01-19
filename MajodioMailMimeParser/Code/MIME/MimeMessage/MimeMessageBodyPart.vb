Imports System.Text.RegularExpressions
Imports Majodio.Mail.Mime.Headers

''' -----------------------------------------------------------------------------
''' Project	 : MajodioMailServer
''' Class	 : Mail.Server.MimeMessageBodyPart
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' 
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[ruwem]	2/17/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class MessageBodyPart

#Region " Private Fields"
    Private _Boundary As String
    Private _Preamble As String
    Private _BodyPartContent As String
    Private _BodyParts As MimeMessageBodyPartCollection
    Private _ParentBodyPart As MessageBodyPart
    Private _Headers As MimeMessageHeaderCollection
#End Region

#Region " Constructors"
    Public Sub New()
        
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RawText"></param>
    ''' <param name="ParentBodyPart"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/8/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub New(ByVal RawText As String, ByVal ParentBodyPart As MessageBodyPart)
        Dim ModifiedRawText As String = RawText
        If Not IsNothing(RawText) AndAlso RawText.Trim.Length > 0 Then
            ModifiedRawText = ModifiedRawText.Replace(vbLf & vbLf, vbCrLf & vbCrLf)

            ModifiedRawText = Regex.Replace(ModifiedRawText, "(?im)[^\r]{1}\n{1}", AddressOf CaptureText)

            _ParentBodyPart = ParentBodyPart
            Parse(ModifiedRawText)
        End If
    End Sub

    Private Function CaptureText(ByVal M As Match) As String
        Dim RVal As String = M.ToString
        If RVal.Length = 2 Then
            RVal = RVal.Substring(0, 1) & vbCrLf
        End If
        Return RVal
    End Function
#End Region

#Region " Public Property"
    Public Property Boundary() As String
        Get
            Return _Boundary
        End Get
        Set(ByVal Value As String)
            _Boundary = Value
        End Set
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property BodyParts() As MimeMessageBodyPartCollection
        Get
            If IsNothing(_BodyParts) Then
                _BodyParts = New MimeMessageBodyPartCollection(_Boundary)
            End If
            Return _BodyParts
        End Get
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/9/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property ParentBodyPart() As MessageBodyPart
        Get
            Return _ParentBodyPart
        End Get
        Set(ByVal Value As MessageBodyPart)
            _ParentBodyPart = Value
        End Set
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property Headers() As MimeMessageHeaderCollection
        Get
            If IsNothing(_Headers) Then
                _Headers = New MimeMessageHeaderCollection
            End If
            Return _Headers
        End Get
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/9/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Property BodyPartContent() As String
        Get
            Return _BodyPartContent
        End Get
        Set(ByVal Value As String)
            _BodyPartContent = Value
        End Set
    End Property

    Public Property Preamble() As String
        Get
            Return _Preamble & vbCrLf
        End Get
        Set(ByVal Value As String)
            _Preamble = Value
        End Set
    End Property
#End Region

#Region " Public Methods"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RawText"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub Parse(ByVal RawText As String)
        Dim ContentTypeIndex As Integer
        'Retrieve all of the headers
        Headers.Add(GetHeaderCollection(GetRawHeaderText(RawText)))

        'Determine if the ContentType header exists, and if it does, find the boundary for the text
        ContentTypeIndex = Headers.IndexOf(MimeMessageHeaderTypes.ContentType)
        If ContentTypeIndex > -1 AndAlso CType(Headers(ContentTypeIndex), ContentType).ContentType.ToLower = "multipart" AndAlso CType(Headers(ContentTypeIndex), ContentType).Boundary <> "" Then
            'Setup the boundary variable
            _Boundary = CType(Headers(ContentTypeIndex), ContentType).Boundary

            'Add any child body parts to this instance's body part collection
            BodyParts.Add(GetBodyPartCollection(GetRawBodyText(RawText), _Boundary, Me))
        Else
            _BodyPartContent = GetRawBodyText(RawText)
        End If
    End Sub

    Public Overridable Function GetCanonicalFormContent(ByVal transformPart As Boolean) As String
        Dim contentTypeHeaderIndex As Integer
        Dim transferEncodingHeaderIndex As Integer
        Dim contentHeader As ContentType
        Dim encodingHeader As ContentTransferEncoding

        Dim rVal As String = _BodyPartContent

        contentTypeHeaderIndex = Headers.IndexOf(MimeMessageHeaderTypes.ContentType)
        transferEncodingHeaderIndex = Headers.IndexOf(MimeMessageHeaderTypes.ContentTransferEncoding)

        If contentTypeHeaderIndex > -1 AndAlso transferEncodingHeaderIndex > -1 Then
            contentHeader = CType(Headers(contentTypeHeaderIndex), ContentType)
            encodingHeader = CType(Headers(transferEncodingHeaderIndex), ContentTransferEncoding)
            If contentHeader.ContentType.ToLower = "text" And encodingHeader.Value.ToLower = "quoted-printable" Then

                'Replace the =XX with the equivalent ASCII characters
                rVal = Regex.Replace(rVal, "(?i)=(?<character>[0-9a-fA-F]{2})", AddressOf ReplaceHex)

                'Replace the = at the end of a line with continuation
                rVal = Regex.Replace(rVal, "(?i)=\r\n{0,1}", String.Empty)
                If transformPart Then
                    encodingHeader.Value = "7bit"
                    _BodyPartContent = rVal
                End If
            ElseIf contentHeader.ContentType.ToCharArray = "text" And encodingHeader.Value.ToLower = "base64" Then
                Try
                    rVal = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(rVal))
                Catch ex As FormatException
                    Throw New FormatException("The mime message specified a BASE64 Encoded body part, but does not follow that specification", ex)
                End Try
                If transformPart Then
                    encodingHeader.Value = "7bit"
                    _BodyPartContent = rVal
                End If
            End If
        End If

        Return rVal
    End Function

    Private Function ReplaceHex(ByVal match As Match) As String
        Dim asciiValue As Integer
        Dim rVal As String = String.Empty

        'Convert the hex value to decimal
        asciiValue = System.Convert.ToInt32(match.Groups("character").Value, 16)

        'Convert the decimal value to a character
        rVal = Chr(asciiValue)

        Return rVal
    End Function
#End Region

#Region " Protected Methods"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BodyText"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Sub ParseBody(ByVal BodyText As String)
        BodyParts.Add(GetBodyPartCollection(BodyText, BodyParts.Boundary, Me))
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="HeaderText"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function GetHeaderCollection(ByVal HeaderText As String) As MimeMessageHeaderCollection
        Dim ModifiedHeaderText As String = HeaderText
        Dim HeaderLines As String()
        Dim i As Integer
        Dim HeaderName As String
        Dim HeaderValue As String
        Dim ColonIndex As Integer
        Dim NewHeader As MimeMessageHeaderBase
        Dim RVal As New MimeMessageHeaderCollection

        If HeaderText.Trim.Length > 0 Then
            ModifiedHeaderText = Regex.Replace(ModifiedHeaderText, "(?im)(\r\n|\r|\n)", vbLf)
            'ModifiedHeaderText = Regex.Replace(ModifiedHeaderText, "(?im)\n", vbLf)
            'Unfold the headers (i.e. remove any extraneous CR LF's)
            ModifiedHeaderText = Regex.Replace(HeaderText, "(?im)\n\s+", " ")

            'Split the headers into lines
            HeaderLines = Split(ModifiedHeaderText, vbLf)
            If Not IsNothing(HeaderLines) Then
                'Go through each line and determine what type of header it is
                For i = 0 To HeaderLines.GetUpperBound(0)
                    ColonIndex = HeaderLines(i).IndexOf(":")
                    If ColonIndex > -1 Then
                        HeaderName = HeaderLines(i).Substring(0, ColonIndex).Trim
                        HeaderValue = HeaderLines(i).Substring(ColonIndex + 1).Trim
                        'Remove quotes
                        'If HeaderValue.Substring(0, 1) = """" Then
                        '    Dim QuoteIndex As Integer = HeaderValue.IndexOf("""", 1)
                        '    If QuoteIndex > -1 Then
                        '        HeaderValue = HeaderValue.Substring(1, QuoteIndex - 1)
                        '    End If
                        'End If
                        Select Case HeaderName.Trim.ToLower
                            Case "received"
                                NewHeader = New Received(HeaderValue)
                            Case "content-type"
                                NewHeader = New ContentType(HeaderValue)
                            Case "content-transfer-encoding"
                                NewHeader = New ContentTransferEncoding(HeaderValue)
                            Case Else
                                NewHeader = New OtherHeader(HeaderName, HeaderValue)
                        End Select
                        RVal.Add(NewHeader)
                    End If
                Next
            End If
        End If
        Return RVal
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BodyText"></param>
    ''' <param name="Boundary"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function GetBodyPartCollection(ByVal BodyText As String, ByVal Boundary As String, ByVal ParentBodyPart As MessageBodyPart) As MimeMessageBodyPartCollection
        Dim RVal As New MimeMessageBodyPartCollection(Boundary)
        'Dim BodyArr As String()
        Dim Index As Integer
        'dim NextIndex As Integer
        RVal.Boundary = Boundary
        Dim BP As MessageBodyPart
        'Dim EndFound As Boolean = False
        Dim FullBoundary As String = "--" & Boundary
        Dim BodyPart As String
        If Not IsNothing(BodyText) AndAlso BodyText.Trim.Length > 0 AndAlso Not IsNothing(Boundary) AndAlso Boundary.Trim.Length > 0 Then
            If Boundary.Trim.Length > 0 Then
                Index = BodyText.ToLower.IndexOf(FullBoundary.ToLower)
                _Preamble = BodyText.Substring(0, Index)
                While Index > -1
                    BP = Nothing
                    BodyPart = GetBodyPartString(BodyText, Index, FullBoundary) ', NextIndex, EndFound)
                    'Index = NextIndex
                    If BodyPart.Trim.Length > 0 Then
                        BP = New MessageBodyPart(BodyPart, ParentBodyPart)
                    End If
                    If Not IsNothing(BP) Then
                        RVal.Add(BP)
                    End If
                    'If EndFound Then
                    '    Exit While
                    'End If
                End While
            End If
        End If
        Return RVal
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BodyText"></param>
    ''' <param name="StartIndex"></param>
    ''' <param name="Boundary"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/9/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Function GetBodyPartString(ByVal BodyText As String, ByRef StartIndex As Integer, ByVal Boundary As String) As String
        Dim RVal As String = BodyText
        Dim IntStartIndex As Integer
        Dim IntEndIndex As Integer
        Dim EndFound As Boolean = False
        Dim CharsToAdd As Integer = 2
        If StartIndex + Boundary.Length >= BodyText.Length Then
            'The mime message is incorrectly formatted.  Deal with this by setting the index to the end of the message
            IntEndIndex = BodyText.Length
            EndFound = True
        End If

        If BodyText.Substring(StartIndex + Boundary.Length, 1) = vbLf Then
            CharsToAdd = 1
        End If

        'Does the start of the bodytext boundary have a CrLf on the end of it?  It must in order to continue.
        If BodyText.Length > StartIndex + Boundary.Length + CharsToAdd - 1 AndAlso (BodyText.Substring(StartIndex + Boundary.Length, CharsToAdd) = vbCrLf OrElse BodyText.Substring(StartIndex + Boundary.Length, CharsToAdd) = vbLf) Then
            IntStartIndex = StartIndex + Boundary.Length + CharsToAdd
            IntEndIndex = BodyText.ToLower.IndexOf(Boundary.ToLower, IntStartIndex + Boundary.Length)
            If IntEndIndex > -1 Then
                'Found the ending boundary
                If BodyText.Length > (IntEndIndex + Boundary.Length + 2) AndAlso BodyText.Substring(IntEndIndex + Boundary.Length, 2) = "--" Then
                    EndFound = True
                End If
            Else
                'The message doesn't appear to have a ending boundary - assume the end of the message
                IntEndIndex = BodyText.Length
                EndFound = True
            End If
        Else
            IntEndIndex = BodyText.Length
            EndFound = True
        End If
        'EndIndex = IntEndIndex
        If EndFound Then
            StartIndex = -1
        Else
            StartIndex = IntEndIndex
        End If
        If BodyText.Length > IntEndIndex Then
            RVal = BodyText.Substring(IntStartIndex, IntEndIndex - IntStartIndex)
        End If
        Return RVal
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RawMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function GetRawHeaderText(ByVal RawMessage As String) As String
        Dim RVal As String = String.Empty
        Dim HeaderEndIndex As Integer
        'Get the boundary of the header/body text
        'RFC #822 section 3.1 page 5
        If RawMessage.Substring(0, 2) <> vbCrLf Then
            HeaderEndIndex = RawMessage.IndexOf(vbCrLf & vbCrLf)
            If HeaderEndIndex = -1 Then
                HeaderEndIndex = RawMessage.IndexOf(vbLf & vbLf)
            End If
            If HeaderEndIndex > -1 Then
                'Found the boundary
                RVal = RawMessage.Substring(0, HeaderEndIndex)
            End If
        End If
        Return RVal
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RawMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	3/7/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Protected Function GetRawBodyText(ByVal RawMessage As String) As String
        Dim RVal As String = RawMessage
        Dim HeaderEndIndex As Integer
        Dim CharsToAdd As Integer = 4
        
        'Get the boundary of the header/body text
        'RFC #822 section 3.1 page 5
        If RawMessage.Substring(0, 1) = vbLf Then
            'Remove the starting line feed
            RVal = RawMessage.Substring(1)
        ElseIf RawMessage.Substring(0, 2) = vbCrLf Then
            'Remove the starting carriage return
            RVal = RawMessage.Substring(2)
        ElseIf RawMessage.Substring(0, 2) <> vbCrLf Then
            HeaderEndIndex = RawMessage.IndexOf(vbCrLf & vbCrLf)
            If HeaderEndIndex = -1 Then
                HeaderEndIndex = RawMessage.IndexOf(vbLf & vbLf)
                CharsToAdd = 2
            End If
            If HeaderEndIndex > -1 Then
                If RawMessage.Substring(0, HeaderEndIndex).Trim.Length > 0 Then
                    'Found the boundary
                    RVal = RawMessage.Substring(HeaderEndIndex + CharsToAdd)
                ElseIf HeaderEndIndex = 0 Then
                    RVal = RawMessage.Substring(CharsToAdd)
                End If
            End If
        End If

        Return RVal
    End Function
#End Region

End Class