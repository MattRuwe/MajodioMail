Imports majodio.Mail.Mime.Headers
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Majodio.Common
Imports System.Text


''' <summary>
''' An implementation of a MIME message
''' </summary>
''' <remarks>
''' This class allows access to all features of a MIME message including the headers, body parts, and sub body parts.
''' </remarks>
Public Class Message
    Inherits MessageBodyPart
#Region " Constructors"
    ''' <summary>
    ''' Creates a new MIME message object based on a string
    ''' </summary>
    ''' <param name="RawText">A string that contains a properly formatted MIME message</param>
    ''' <remarks>This method parses the input string and initializes all of the properties of the MIME message by recursively creating
    ''' new body parts and adding them to the Message object.</remarks>
    Public Sub New(ByVal RawText As String)
        MyBase.New(RawText, Nothing)
        If Not IsNothing(RawText) AndAlso RawText.Trim.Length > 0 Then
            InitializeProperties()
            'AddParsedByMajodioHeader()
        End If
    End Sub

    ''' <summary>
    ''' Creates a new MIME message object from scratch
    ''' </summary>
    ''' <param name="GenerateNewBoundary">If set to true, a new root body part is created initialized with a new Boundary ID.</param>
    ''' <remarks>The method is generally usful when creating a MIME message from scratch.  If you don't already have a message
    ''' then this is the method to use.</remarks>
    Public Sub New(ByVal GenerateNewBoundary As Boolean)
        If GenerateNewBoundary Then
            Dim CT As New ContentType("multipart", "alternative")
            Dim MV As New MimeVersion()
            Dim BP As New MessageBodyPart
            'AddParsedByMajodioHeader()
            CT.GenerateNewBoundary()
            Boundary = CT.Boundary
            Preamble = "This is a multipart mime message.  If you're seeing this message, you mail client is" & vbCrLf & "not capable of viewing mime messages."
            Headers.Add(MV)
            Headers.Add(CT)
            BP.Boundary = Boundary
            BP.Headers.Add(New ContentType("text", "plain"))
            BodyParts.Add(BP)
        End If
    End Sub

    ''' <summary>
    ''' Creates a new MIME message object from a QueuedMail.<see cref="T:Majodio.Mail.Common.Storage.QueuedMail.Message" /> object
    ''' </summary>
    ''' <param name="Message">An instance of a QueuedMail.Message object that will be used to initialize the MIME message</param>
    ''' <remarks>The QueuedMail.Message object contains more information than Mime.Message object does.  For instance, the QueuedMail.Message object
    ''' keeps track of the SMTP FROM and SMTP TO values which are the actual sender/receiver that were used during the SMTP exchange.  Those values 
    ''' are subsequently stored in the <see cref="P:Majodio.Mail.Mime.Message.SmtpFromAddress" /> and <see cref="P:Majodio.Mail.Mime.Message.SmtpToAddress" /> 
    ''' respectively.</remarks>
    Public Sub New(ByVal Message As Majodio.Mail.Common.Storage.QueuedMail.Message)
        Me.New(Message.GetStringMessageContent)
        SmtpFromAddress = Message.From
        SmtpToAddress = Message.[To]
    End Sub
#End Region

#Region " Private Fields"
    Private _Headers As MimeMessageHeaderCollection
    Private _BodyParts As MimeMessageBodyPartCollection
    Private _SmtpFromAddress As EmailAddress
    Private _SmtpToAddress As EmailAddress
    Private _MimeToAddresses As New EmailAddressCollection
    Private _MimeCcAddresses As New EmailAddressCollection
    Private _MimeBccAddresses As New EmailAddressCollection
#End Region

#Region " Public Properties"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RootBodyPart() As MessageBodyPart
        Get
            Dim RVal As MessageBodyPart = Nothing
            If BodyParts.Count > 0 Then
                RVal = BodyParts(0)
            End If
            Return RVal
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SmtpFromAddress() As EmailAddress
        Get
            Return _SmtpFromAddress
        End Get
        Set(ByVal Value As EmailAddress)
            _SmtpFromAddress = Value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SmtpToAddress() As EmailAddress
        Get
            Return _SmtpToAddress
        End Get
        Set(ByVal value As EmailAddress)
            _SmtpToAddress = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FromAddress() As EmailAddress
        Get
            Dim RVal As EmailAddress = Nothing
            Dim Index As Integer = Headers.IndexOf("from")
            If Index > -1 Then
                RVal = New EmailAddress(Headers("from").Value)
            End If
            Return RVal
        End Get
        Set(ByVal Value As EmailAddress)
            Dim Index As Integer = Headers.IndexOf("from")
            If Index > -1 Then
                Headers(Index) = New OtherHeader("from", Value.ToString(EmailStringFormat.NameAddressBraces))
            Else
                Headers.Add(New OtherHeader("from", Value.ToString(EmailStringFormat.NameAddressBraces)))
            End If
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Subject() As String
        Get
            Dim RVal As String = String.Empty
            Dim Index As Integer = Headers.IndexOf("subject")
            If Index > -1 Then
                RVal = Headers("subject").Value
            End If
            Return RVal
        End Get
        Set(ByVal Value As String)
            Dim Index As Integer = Headers.IndexOf("subject")
            If Index > -1 Then
                Headers(Index) = New OtherHeader("subject", Value)
            Else
                Headers.Add(New OtherHeader("subject", Value))
            End If
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ToAddresses() As EmailAddressCollection
        Get
            Return _MimeToAddresses
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CcAddresses() As EmailAddressCollection
        Get
            Return _MimeCcAddresses
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property BccAddresses() As EmailAddressCollection
        Get
            Return _MimeBccAddresses
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Received() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal Value As String)

        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RawMessage() As String
        Get
            AddToAddresses()
            AddCcAddresses()
            AddBccAddresses()
            Return GetRawMessageContent(Me)
        End Get
        Set(ByVal Value As String)
            MyBase.Parse(Value)
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MessageText() As String
        Get
            Return GetMessageText(Me)
        End Get
    End Property
#End Region

#Region " Public Methods"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ClientReportedAddress"></param>
    ''' <param name="ClientIP"></param>
    ''' <param name="ServerName"></param>
    ''' <remarks></remarks>
    Public Sub AddReceivedHeader(ByVal ClientReportedAddress As String, ByVal ClientIP As System.Net.IPAddress, ByVal ServerName As String)
        Dim Header As New OtherHeader
        Header.Name = "RECEIVED"
        Header.Value = "from " & ClientReportedAddress & " [" & ClientIP.ToString & "] by " & ServerName & _
                       " id " & Functions.GetSerializedDateTime() & "; " & DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss") & " " & GetLocalUtcOffset()
        Headers.Insert(0, Header)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Domain"></param>
    ''' <param name="Username"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetQueuedMessage(ByVal Domain As String, ByVal Username As String) As Majodio.Mail.Common.Storage.QueuedMail.Message
        Dim rVal As Majodio.Mail.Common.Storage.QueuedMail.Message = Majodio.Mail.Common.Storage.QueuedMail.RemoteClient.Instance.CreateMessage(Domain, Username)
        If Me.Headers.Count("from") > 0 Then
            rVal.From = SmtpFromAddress
            rVal.To = SmtpToAddress
            rVal.SetMessageContent(RawMessage)
        End If
        Return rVal
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="type"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindBodyPart(ByVal type As ContentType) As MessageBodyPart
        Return FindBodyPart(type, Me)
    End Function

    Public Sub Normalize()
        NormalizeBodyPart(Me)
    End Sub

    Private Sub NormalizeBodyPart(ByVal currentPart As MessageBodyPart)
        If Not IsNothing(currentPart) Then
            currentPart.GetCanonicalFormContent(True)
            If Not IsNothing(currentPart.BodyParts) Then
                For Each BodyPart As MessageBodyPart In currentPart.BodyParts
                    NormalizeBodyPart(BodyPart)
                Next
            End If
        End If
    End Sub
#End Region

#Region " Private Methods"
    Private Function FindBodyPart(ByVal type As ContentType, ByVal currentBodyPart As MessageBodyPart) As MessageBodyPart
        Dim rVal As MessageBodyPart = Nothing
        Dim headerIndex As Integer
        headerIndex = currentBodyPart.Headers.IndexOf(MimeMessageHeaderTypes.ContentType)

        'Check the current body part first
        If headerIndex > -1 AndAlso CType(currentBodyPart.Headers(headerIndex), ContentType).ContentType.ToLower = Type.ContentType.ToLower AndAlso CType(currentBodyPart.Headers(headerIndex), ContentType).ContentSubType.ToLower = Type.ContentSubType.ToLower Then
            'Found it!
            rVal = currentBodyPart
        Else
            'see if any of the child parts have the contentype we're looking for
            For i As Integer = 0 To currentBodyPart.BodyParts.Count - 1
                rVal = FindBodyPart(Type, currentBodyPart.BodyParts(i))
                If Not IsNothing(rVal) Then
                    Exit For
                End If
            Next
        End If

        Return rVal
    End Function

    Private Sub AddToAddresses()
        Dim HeaderValue As String = String.Empty
        Dim Index As Integer = Headers.IndexOf("to")
        If ToAddresses.Count > 0 Then
            For i As Integer = 0 To ToAddresses.Count - 1
                HeaderValue &= ToAddresses(i).ToString(EmailStringFormat.NameAddressBraces) & ", "
            Next
            HeaderValue = HeaderValue.Substring(0, HeaderValue.Length - 2)
        End If
        If Index > -1 Then
            Headers(Index).Value = HeaderValue
        Else
            Headers.Add(New OtherHeader("to", HeaderValue))
        End If
    End Sub

    Private Sub AddCcAddresses()
        Dim HeaderValue As String = String.Empty
        Dim Index As Integer = Headers.IndexOf("cc")
        If CcAddresses.Count > 0 Then
            For i As Integer = 0 To CcAddresses.Count - 1
                HeaderValue &= CcAddresses(i).ToString(EmailStringFormat.NameAddressBraces) & ", "
            Next
            HeaderValue = HeaderValue.Substring(0, HeaderValue.Length - 2)
        End If
        If Index > -1 Then
            Headers(Index).Value = HeaderValue
        Else
            Headers.Add(New OtherHeader("cc", HeaderValue))
        End If
    End Sub

    Private Sub AddBccAddresses()
        Dim HeaderValue As String = String.Empty
        Dim Index As Integer = Headers.IndexOf("bcc")
        If BccAddresses.Count > 0 Then
            For i As Integer = 0 To BccAddresses.Count - 1
                HeaderValue &= BccAddresses(i).ToString(EmailStringFormat.NameAddressBraces) & ", "
            Next
            HeaderValue = HeaderValue.Substring(0, HeaderValue.Length - 2)
        End If
        If Index > -1 Then
            Headers(Index).Value = HeaderValue
        Else
            Headers.Add(New OtherHeader("bcc", HeaderValue))
        End If
    End Sub

    'Private Sub AddParsedByMajodioHeader()
    '    If MyBase.Headers.IndexOf("x-MajodioMimeParser") = -1 Then
    '        Dim CurrentAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
    '        Headers.Add(New Headers.OtherHeader("X-MajodioMimeParser", "This mime message was parsed by" & vbCrLf & "    " & CurrentAssembly.GetName.ToString))
    '    End If
    'End Sub

    Private Shared Function GetRawMessageContent(ByVal BP As MessageBodyPart) As String
        Dim RVal As New System.Text.StringBuilder
        For i As Integer = 0 To BP.Headers.Count - 1
            Select Case BP.Headers(i).HeaderType
                Case MimeMessageHeaderTypes.ContentType
                    Dim Header As ContentType = BP.Headers(i)
                    RVal.Append("content-type: ")
                    RVal.Append(Header.ContentType & "/" & Header.ContentSubType & "; charset=""" & Header.CharSet & """")
                    If Not IsNothing(Header) AndAlso Not IsNothing(Header.Boundary) AndAlso Header.Boundary.Trim.Length > 0 Then
                        RVal.Append("; boundary=""" & Header.Boundary & """")
                    End If
                    If Header.Parameters.Count > 0 Then
                        For Each Key As String In Header.Parameters.Keys
                            RVal.Append("; " & Key & "=""" & Header.Parameters(Key) & """")
                        Next
                    End If
                    RVal.Append(vbCrLf)
                Case Else
                    If BP.Headers(i).Value.Trim.Length > 0 Then
                        RVal.Append(BP.Headers(i).Name & ": " & BP.Headers(i).Value & vbCrLf)
                    End If
            End Select
        Next
        RVal.Append(vbCrLf)
        RVal.Append(BP.Preamble)

        RVal.Append(GetMessageText(BP))

        Return RVal.ToString
    End Function

    Private Shared Function GetMessageText(ByVal BP As MessageBodyPart) As String
        Dim RVal As New StringBuilder
        For i As Integer = 0 To BP.BodyParts.Count - 1
            RVal.Append(vbCrLf & "--" & BP.Boundary & vbCrLf)
            RVal.Append(GetRawMessageContent(BP.BodyParts(i)))
            If i = BP.BodyParts.Count - 1 Then
                RVal.Append(vbCrLf & "--" & BP.Boundary & "--" & vbCrLf)
            End If
        Next

        RVal.Append(BP.BodyPartContent)

        Return RVal.ToString
    End Function

    Private Sub InitializeProperties()
        Dim Index As Integer

        Index = Headers.IndexOf("from")
        If Index > -1 AndAlso Majodio.Common.EmailAddress.IsValidAddress(Headers(Index).Value) Then
            FromAddress = New EmailAddress(Headers(Index).Value)
        End If

        Index = Headers.IndexOf("to")
        While Index > -1
            If Majodio.Common.EmailAddress.IsValidAddress(Headers(Index).Value) Then
                ToAddresses.Add(Headers(Index).Value)
            End If
            Index = Headers.IndexOf(Index + 1, "to")
        End While

        Index = Headers.IndexOf("cc")
        While Index > -1
            If Majodio.Common.EmailAddress.IsValidAddress(Headers(Index).Value) Then
                CcAddresses.Add(Headers(Index).Value)
            End If
            Index = Headers.IndexOf(Index + 1, "cc")
        End While

        Index = Headers.IndexOf("bcc")
        While Index > -1
            If Majodio.Common.EmailAddress.IsValidAddress(Headers(Index).Value) Then
                BccAddresses.Add(Headers(Index).Value)
            End If
            Index = Headers.IndexOf(Index + 1, "bcc")
        End While

        Index = Headers.IndexOf("content-type")
        If Index > -1 Then
            BodyParts.Boundary = New ContentType(Headers(Index).Value).Boundary
        End If
    End Sub
#End Region

End Class
