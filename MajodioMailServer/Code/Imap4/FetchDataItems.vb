Imports System.Text
Imports Majodio.Mail.Common.Grouping

Namespace Imap4
    ''' -----------------------------------------------------------------------------
    ''' Project	 : MajodioMailServer
    ''' Class	 : Mail.Server.Imap4.Imap4FetchDataItems
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    '''   Class used to store which items are to be retrieved with an IMAP4 FETCH 
    '''   command.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	7/13/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class FetchDataItems
        Private _Items As FetchItems()

        Private _BodySections As FetchBodyCollection

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="DataItems"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal DataItems As String)
            Initialize()
            Parse(DataItems)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Initialize()
            _Items = Nothing
            _BodySections = New FetchBodyCollection
        End Sub

        Public Sub AddItem(ByVal NewItem As FetchItems)
            If IsNothing(_Items) Then
                ReDim _Items(0)
            ElseIf Array.IndexOf(_Items, NewItem) = -1 Then
                ReDim Preserve _Items(_Items.GetUpperBound(0) + 1)
            End If
            If Array.IndexOf(_Items, NewItem) = -1 Then
                _Items(_Items.GetUpperBound(0)) = NewItem
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="DataItems"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Parse(ByVal DataItems As String)
            Dim PartGroup As EmbeddedGroup
            Dim CurrentIndex As Integer = 0
            'Dim GroupIndex As Integer
            Dim Headers As String
            Dim ArrHeaders As String()
            Dim InnerGroupIndex As Integer

            'Determine which message details need to be retrieved
            While CurrentIndex < DataItems.Length - 1
                If CurrentIndex + 3 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 3).ToLower = "all" Then
                    CurrentIndex += 3
                    '_All = True
                    AddItem(FetchItems.All)
                ElseIf CurrentIndex + 13 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 13).ToLower = "bodystructure" Then
                    CurrentIndex += 13
                    '_BodyStructure = True
                    AddItem(FetchItems.BodyStructure)
                ElseIf CurrentIndex + 4 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 4).ToLower = "body" Then
                    PartGroup = New EmbeddedGroup(DataItems)
                    If CurrentIndex + 9 < DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 9).ToLower = "body.peek" Then
                        CurrentIndex += 9
                        '_BodyPeek = True
                        AddItem(FetchItems.BodyPeek)
                    Else
                        CurrentIndex += 4
                        '_Body = True
                        AddItem(FetchItems.Body)
                    End If

                    InnerGroupIndex = -1
                    For i As Integer = 0 To PartGroup.InnerGroup.Count - 1
                        If PartGroup.InnerGroup(i).StartIndex >= CurrentIndex And CurrentIndex < PartGroup.InnerGroup(i).StartIndex + PartGroup.InnerGroup(i).GroupText.Length Then
                            InnerGroupIndex = i
                            Exit For
                        End If
                    Next
                    Dim FetchBody As FetchBody = Nothing
                    If InnerGroupIndex > -1 Then

                        If PartGroup.InnerGroup(InnerGroupIndex).GroupText.ToLower.Trim.IndexOf("header.fields.not") > -1 Then
                            FetchBody = New FetchBody
                            FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.HeaderFieldsNot
                            Headers = PartGroup.InnerGroup(InnerGroupIndex).InnerGroup(0).GroupText
                            ArrHeaders = Split(Headers, " ")
                            FetchBody.AddHeader(ArrHeaders)
                        ElseIf PartGroup.InnerGroup(InnerGroupIndex).GroupText.ToLower.Trim.IndexOf("header.fields") > -1 Then
                            FetchBody = New FetchBody
                            FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.HeaderFields
                            Headers = PartGroup.InnerGroup(InnerGroupIndex).InnerGroup(0).GroupText
                            ArrHeaders = Split(Headers, " ")
                            FetchBody.AddHeader(ArrHeaders)
                            CurrentIndex += PartGroup.InnerGroup(InnerGroupIndex).ToString.Length + 2
                        ElseIf PartGroup.InnerGroup(InnerGroupIndex).GroupText.ToLower.Trim.Trim.IndexOf("header") > -1 Then
                            FetchBody = New FetchBody
                            FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.Header
                        ElseIf PartGroup.InnerGroup(InnerGroupIndex).GroupText.ToLower.Trim.IndexOf("mime") > -1 Then
                            FetchBody = New FetchBody
                            FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.Mime
                        ElseIf PartGroup.InnerGroup(InnerGroupIndex).GroupText.ToLower.Trim.IndexOf("text") > -1 Then
                            FetchBody = New FetchBody
                            FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.Text
                        End If
                        If Not IsNothing(FetchBody) Then
                            _BodySections.Add(FetchBody)
                        End If
                    Else
                        FetchBody = New FetchBody
                        FetchBody.SectionSpecifier = Imap4FetchSectionSpecifier.None
                        _BodySections.Add(FetchBody)
                    End If
                ElseIf CurrentIndex + 8 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 8).ToLower = "envelope" Then
                    CurrentIndex += 8
                    '_Envelope = True
                    AddItem(FetchItems.Envelope)
                ElseIf CurrentIndex + 4 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 4).ToLower = "fast" Then
                    CurrentIndex += 4
                    '_Fast = True
                    AddItem(FetchItems.Fast)
                ElseIf CurrentIndex + 5 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 5).ToLower = "flags" Then
                    CurrentIndex += 5
                    '_Flags = True
                    AddItem(FetchItems.Flags)
                ElseIf CurrentIndex + 4 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 4).ToLower = "full" Then
                    CurrentIndex += 4
                    '_Full = True
                    AddItem(FetchItems.Full)
                ElseIf CurrentIndex + 12 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 12).ToLower = "internaldate" Then
                    CurrentIndex += 12
                    '_InternalDate = True
                    AddItem(FetchItems.InternalDate)
                ElseIf CurrentIndex + 13 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 13).ToLower = "rfc822.header" Then
                    CurrentIndex += 13
                    '_Rfc822Header = True
                    AddItem(FetchItems.Rfc822Header)
                ElseIf CurrentIndex + 11 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 11).ToLower = "rfc822.size" Then
                    CurrentIndex += 11
                    '_Rfc822Size = True
                    AddItem(FetchItems.Rfc822Size)
                ElseIf CurrentIndex + 11 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 11).ToLower = "rfc822.text" Then
                    CurrentIndex += 11
                    '_Rfc822Text = True
                    AddItem(FetchItems.Rfc822Text)
                ElseIf CurrentIndex + 6 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 6).ToLower = "rfc822" Then
                    CurrentIndex += 6
                    '_Rfc822 = True
                    AddItem(FetchItems.Rfc822)
                ElseIf CurrentIndex + 3 <= DataItems.Length AndAlso DataItems.Substring(CurrentIndex, 3).ToLower = "uid" Then
                    CurrentIndex += 3
                    '_Uid = True
                    AddItem(FetchItems.Uid)
                Else
                    CurrentIndex += 1
                End If
            End While
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetParsedMessage(ByVal Message As Majodio.Mail.Common.Storage.QueuedMail.Message) As String
            Dim RVal As New StringBuilder
            Dim MimeMessage As Mime.Message
            Dim AddSpace As Boolean = False

            MimeMessage = New Mime.Message(Message)

            'BODY
            RVal.Append("(")
            For h As Integer = 0 To _Items.GetUpperBound(0)
                If AddSpace Then
                    RVal.Append(" ")
                    AddSpace = False
                End If

                If _Items(h) = FetchItems.Body Or _Items(h) = FetchItems.BodyPeek Then
                    AddSpace = True
                    RVal.Append(GetBody(MimeMessage))
                End If

                'BODYSTRUCTURE
                If _Items(h) = FetchItems.BodyStructure Then
                    AddSpace = True
                    RVal.Append("BODYSTRUCTURE " & GetBodyStructure(MimeMessage))
                End If

                'ENVELOPE
                If _Items(h) = FetchItems.Envelope Then
                    AddSpace = True
                    RVal.Append(GetEnvelope(MimeMessage))
                End If

                If _Items(h) = FetchItems.Rfc822Size Then
                    AddSpace = True
                    RVal.Append(GetRfc822Size(MimeMessage))
                End If

                If _Items(h) = FetchItems.Flags Then
                    Dim Flags As String = GetFlags(Message)
                    'If Flags.Length > 0 Then
                    AddSpace = True
                    RVal.Append(GetFlags(Message))
                    'End If
                End If

                If _Items(h) = FetchItems.InternalDate Then
                    AddSpace = True
                    RVal.Append(GetInternalDate(MimeMessage))
                End If

                If _Items(h) = FetchItems.Uid Then
                    AddSpace = True
                    RVal.Append(GetUid(Message))
                End If
            Next
            RVal.Append(")")
            Return RVal.ToString
        End Function

        Private Function GetBody(ByVal Message As Mime.Message) As String
            Dim RVal As New StringBuilder
            Dim TmpResponse As StringBuilder
            Dim TmpResponse2 As StringBuilder
            Dim BodyText As String

            If _BodySections.Count > 0 AndAlso Not _BodySections(0).SectionSpecifier = Imap4FetchSectionSpecifier.None Then
                For i As Integer = 0 To _BodySections.Count - 1
                    If (Body OrElse BodyPeek) Then
                        TmpResponse = New StringBuilder
                        TmpResponse2 = New StringBuilder
                        TmpResponse.Append("BODY[")
                        If _BodySections(i).SectionSpecifier = Imap4FetchSectionSpecifier.HeaderFields Then
                            TmpResponse.Append("HEADER.FIELDS (")
                            If _BodySections(i).HeaderFields.GetUpperBound(0) > -1 Then
                                For j As Integer = 0 To _BodySections(i).HeaderFields.GetUpperBound(0)
                                    If j < _BodySections(i).HeaderFields.GetUpperBound(0) Then
                                        TmpResponse.Append(_BodySections(i).HeaderFields(j) & " ")
                                        If Message.Headers.IndexOf(_BodySections(i).HeaderFields(j)) > -1 Then
                                            TmpResponse2.Append(Message.Headers(_BodySections(i).HeaderFields(j)).Name & ": " & Message.Headers(_BodySections(i).HeaderFields(j)).Value & vbCrLf)
                                        End If
                                    Else
                                        TmpResponse.Append(_BodySections(i).HeaderFields(j) & ")")
                                        If Message.Headers.IndexOf(_BodySections(i).HeaderFields(j)) > -1 Then
                                            TmpResponse2.Append(Message.Headers(_BodySections(i).HeaderFields(j)).Name & ": " & Message.Headers(_BodySections(i).HeaderFields(j)).Value)
                                        End If
                                    End If
                                Next
                            Else
                                TmpResponse.Append(")")
                            End If
                            If TmpResponse2.ToString.Length > 0 Then
                                TmpResponse.Append("] {" & TmpResponse2.ToString.Length + 2 & "}" & vbCrLf)
                                RVal.Append(TmpResponse.ToString & TmpResponse2.ToString & vbCrLf)
                            Else
                                TmpResponse.Append("] {2}" & vbCrLf)
                                RVal.Append(TmpResponse.ToString & TmpResponse2.ToString & vbCrLf)
                            End If
                        ElseIf _BodySections(i).SectionSpecifier = Imap4FetchSectionSpecifier.Text Then
                            'TODO FINISH THIS!
                        End If
                    End If
                Next
            Else
                BodyText = Message.RawMessage
                RVal.Append("BODY[] {")
                RVal.Append(BodyText.Length)
                RVal.Append("}")
                RVal.Append(vbCrLf)
                RVal.Append(BodyText)
            End If

            Return RVal.ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetBodyStructure(ByVal Message As Mime.MessageBodyPart) As String
            Dim RVal As StringBuilder
            Dim Index As Integer
            Dim CTH As Mime.Headers.ContentType

            RVal = New StringBuilder
            If Message.BodyParts.Count > 0 Then

            End If
            RVal.Append("(")
            Index = Message.Headers.IndexOf(Mime.Headers.MimeMessageHeaderTypes.ContentType)
            If Index > -1 Then
                CTH = Message.Headers(Index)
                If CTH.ContentType.ToLower = "multipart" Then
                    For i As Integer = 0 To Message.BodyParts.Count - 1
                        RVal.Append(GetBodyStructure(Message.BodyParts(i)))
                    Next
                    RVal.Append(" """)
                    RVal.Append(CTH.ContentSubType)
                    RVal.Append("""")
                    If CTH.Parameters.Count > 0 Then
                        For Each Name As String In CTH.Parameters
                            RVal.Append("""" & Name & """ """ & CTH.Parameters(Name) & """")
                        Next
                        RVal.Append(")")
                    Else
                        RVal.Append(" NIL")
                    End If
                    RVal.Append(" NIL NIL")
                Else
                    RVal.Append("""" & CTH.ContentType & """ """ & CTH.ContentSubType & """")
                    If CTH.Parameters.Count > 0 Or CTH.CharSet <> String.Empty Then
                        RVal.Append(" (")
                        If CTH.CharSet <> String.Empty Then
                            RVal.Append("""charset"" """)
                            RVal.Append(CTH.CharSet & """")
                        End If
                        If CTH.Parameters.Count > 0 Then
                            For Each Name As String In CTH.Parameters
                                RVal.Append(" """ & Name & """ """ & CTH.Parameters(Name) & """")
                            Next
                        End If
                        RVal.Append(")")
                        RVal.Append(" NIL NIL")
                        Index = Message.Headers.IndexOf(Mime.Headers.MimeMessageHeaderTypes.ContentTransferEncoding)
                        If Index > -1 Then
                            RVal.Append(" """)
                            RVal.Append(Message.Headers(Index).Value)
                            RVal.Append("""")
                        Else
                            RVal.Append(" NIL")
                        End If
                        RVal.Append(" ")
                        RVal.Append(Majodio.Functions.GetStringCount(Message.BodyPartContent, vbCrLf))
                        RVal.Append(" ")
                        RVal.Append(Message.BodyPartContent.Length)
                        RVal.Append(" NIL NIL NIL")
                    End If
                End If
            End If
            RVal.Append(")")
            Return RVal.ToString
        End Function
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetEnvelope(ByVal Message As Mime.Message) As String
            Dim RVal As New StringBuilder
            Dim HeaderIndex As Integer

            RVal.Append("ENVELOPE (")

            'Date
            HeaderIndex = Message.Headers.IndexOf(Mime.Headers.MimeMessageHeaderTypes.Received)
            If HeaderIndex > -1 Then
                RVal.Append("""")
                RVal.Append(CType(Message.Headers(HeaderIndex), Mime.Headers.Received).DateTime)
                RVal.Append("""")
            End If

            'Subject
            RVal.Append(" ")
            If Message.Subject <> String.Empty Then
                RVal.Append("{")
                RVal.Append(Message.Subject.Length)
                RVal.Append("}")
                RVal.Append(vbCrLf)
                RVal.Append(Message.Subject)
                RVal.Append(" ")
            End If

            'From
            If Not IsNothing(Message.FromAddress) Then
                RVal.Append("(" & Message.FromAddress.ToString(EmailStringFormat.Imap4Envelope) & ")")
            Else
                RVal.Append("(NIL)")
            End If
            RVal.Append(" ")

            'Sender
            If Not IsNothing(Message.SmtpFromAddress) Then
                RVal.Append("(" & Message.SmtpFromAddress.ToString(EmailStringFormat.Imap4Envelope) & ")")
            Else
                RVal.Append("(NIL)")
            End If
            RVal.Append(" ")

            'Reply-To
            RVal.Append(Message.ToAddresses.GetImap4EnvelopAddresses())
            RVal.Append(" ")

            'To
            RVal.Append(Message.ToAddresses.GetImap4EnvelopAddresses())
            RVal.Append(" ")

            'Cc
            RVal.Append(Message.CcAddresses.GetImap4EnvelopAddresses())
            RVal.Append(" ")

            'Bcc
            RVal.Append(Message.BccAddresses.GetImap4EnvelopAddresses())
            RVal.Append(" ")

            'In-Reply-To
            RVal.Append("NIL ")

            'Message-Id
            HeaderIndex = Message.Headers.IndexOf("message-id")
            If HeaderIndex > -1 Then
                RVal.Append("""" & Message.Headers(HeaderIndex).Value & """")
            Else
                RVal.Append("NIL")
            End If

            RVal.Append(")")
            Return RVal.ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetRfc822Size(ByVal Message As Mime.Message) As String
            Dim RVal As New StringBuilder

            RVal.Append("RFC822.SIZE ")
            RVal.Append(Message.RawMessage.Length)

            Return RVal.ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetFlags(ByVal Message As Majodio.Mail.Common.Storage.QueuedMail.Message) As String
            Dim RVal As New StringBuilder

            RVal.Append("FLAGS (")
            If Message.Answered Or Message.Deleted Or Message.Draft Or Message.Flagged Or Message.Recent Or Message.Seen Then

                If Message.Answered Then
                    RVal.Append(" \ANSWERED")
                End If
                If Message.Deleted Then
                    RVal.Append(" \DELETED")
                End If
                If Message.Draft Then
                    RVal.Append(" \DRAFT")
                End If
                If Message.Flagged Then
                    RVal.Append(" \FLAGGED")
                End If
                If Message.Recent Then
                    RVal.Append(" \RECENT")
                End If
                If Message.Seen Then
                    RVal.Append(" \SEEN")
                End If
            End If
            RVal.Append(")")

            Return RVal.ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetInternalDate(ByVal Message As Mime.Message) As String
            Dim RVal As New StringBuilder
            Dim HeaderIndex As Integer

            HeaderIndex = Message.Headers.IndexOf(Mime.Headers.MimeMessageHeaderTypes.Received)

            RVal.Append("INTERNALDATE ")
            If HeaderIndex > -1 Then
                RVal.Append("""")
                RVal.Append(CType(Message.Headers(HeaderIndex), Mime.Headers.Received).DateTime)
                RVal.Append("""")
            End If

            Return RVal.ToString
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Message"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	7/13/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Function GetUid(ByVal Message As Majodio.Mail.Common.Storage.QueuedMail.Message) As String
            Dim RVal As New StringBuilder

            RVal.Append("UID ")
            RVal.Append(Message.UniqueId.Value)

            Return RVal.ToString
        End Function
#Region " Public Properties"
        Public ReadOnly Property All() As Boolean
            Get
                Return Array.IndexOf(_Items, FetchItems.All) > -1
            End Get
        End Property

        Public ReadOnly Property BodyPeek() As Boolean
            Get
                'Return _BodyPeek
                Return Array.IndexOf(_Items, FetchItems.BodyPeek) > -1
            End Get
        End Property

        Public ReadOnly Property Body() As Boolean
            Get
                'Return _Body
                Return Array.IndexOf(_Items, FetchItems.Body) > -1
            End Get
        End Property

        Public ReadOnly Property BodyStructure() As Boolean
            Get
                'Return _BodyStructure
                Return Array.IndexOf(_Items, FetchItems.BodyStructure) > -1
            End Get
        End Property

        Public ReadOnly Property Envelope() As Boolean
            Get
                'Return _Envelope
                Return Array.IndexOf(_Items, FetchItems.Envelope) > -1
            End Get
        End Property

        Public ReadOnly Property Fast() As Boolean
            Get
                'Return _Fast
                Return Array.IndexOf(_Items, FetchItems.Fast) > -1
            End Get
        End Property

        Public ReadOnly Property Flags() As Boolean
            Get
                'Return _Flags
                Return Array.IndexOf(_Items, FetchItems.Flags) > -1
            End Get
        End Property

        Public ReadOnly Property Full() As Boolean
            Get
                'Return _Full
                Return Array.IndexOf(_Items, FetchItems.Full) > -1
            End Get
        End Property

        Public ReadOnly Property InternalDate() As Boolean
            Get
                'Return _InternalDate
                Return Array.IndexOf(_Items, FetchItems.InternalDate) > -1
            End Get
        End Property

        Public ReadOnly Property Rfc822Header() As Boolean
            Get
                'Return _Rfc822Header
                Return Array.IndexOf(_Items, FetchItems.Rfc822Header) > -1
            End Get
        End Property

        Public ReadOnly Property Rfc822Size() As Boolean
            Get
                'Return _Rfc822Size
                Return Array.IndexOf(_Items, FetchItems.Rfc822Size) > -1
            End Get
        End Property

        Public ReadOnly Property Rfc822Text() As Boolean
            Get
                'Return _Rfc822Text
                Return Array.IndexOf(_Items, FetchItems.Rfc822Text) > -1
            End Get
        End Property

        Public ReadOnly Property Rfc822() As Boolean
            Get
                'Return _Rfc822
                Return Array.IndexOf(_Items, FetchItems.Rfc822) > -1
            End Get
        End Property

        Public Property Uid() As Boolean
            Get
                'Return _Uid
                Return Array.IndexOf(_Items, FetchItems.Uid) > -1
            End Get
            Set(ByVal Value As Boolean)
                If Array.IndexOf(_Items, FetchItems.Uid) = -1 Then
                    AddItem(FetchItems.Uid)
                End If
            End Set
        End Property

        'Public ReadOnly Property BodyHeader() As Boolean
        '    Get
        '        Return _BodyHeader
        '    End Get
        'End Property

        'Public ReadOnly Property BodyHeaderFields() As Boolean
        '    Get
        '        Return _BodyHeaderFields
        '    End Get
        'End Property

        'Public ReadOnly Property BodyHeaderFieldsNot() As Boolean
        '    Get
        '        Return _BodyHeaderFieldsNot
        '    End Get
        'End Property

        'Public ReadOnly Property BodyMime() As Boolean
        '    Get
        '        Return _BodyMime
        '    End Get
        'End Property

        'Public ReadOnly Property BodyText() As Boolean
        '    Get
        '        Return _BodyText
        '    End Get
        'End Property

        'Public ReadOnly Property BodyIncludedHeaderFields() As String()
        '    Get
        '        Return _BodyIncludedHeaderFields
        '    End Get
        'End Property

        'Public ReadOnly Property BodyExcludedHeaderFields() As String()
        '    Get
        '        Return _BodyExcludedHeaderFields
        '    End Get
        'End Property
#End Region
    End Class
End Namespace