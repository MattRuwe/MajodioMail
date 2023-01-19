Imports System.Collections.Specialized
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Headers
    ''' -----------------------------------------------------------------------------
    ''' Project	 : MajodioMailServer
    ''' Class	 : Mail.Server.Mime.Headers.ContentType
    ''' 
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[ruwem]	2/28/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class ContentType
        Inherits MimeMessageHeaderBase

        Private _Parameters As NameValueCollection
        Private _ContentType As String
        Private _ContentSubType As String
        Private _Charset As String
        Private _Boundary As String

#Region " Constructors"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Setup the object with default settings as defined in RFC 2045 Page 14
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	2/28/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
            ContentType = "text"
            ContentSubType = "plain"
            CharSet = "us-ascii"
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Setup the object with a set ContentType and ContentSubType, but default
        ''' the charset to us-ascii
        ''' </summary>
        ''' <param name="ContentType"></param>
        ''' <param name="ContentSubType"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	2/28/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal ContentType As String, ByVal ContentSubType As String)
            Me.ContentType = ContentType
            Me.ContentSubType = ContentSubType
            Me.CharSet = "us-ascii"
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Setup the object with a set ContentType, SubContentType, and charset
        ''' </summary>
        ''' <param name="ContentType"></param>
        ''' <param name="ContentSubType"></param>
        ''' <param name="Charset"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	2/28/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal ContentType As String, ByVal ContentSubType As String, ByVal Charset As String)
            Me.ContentType = ContentType
            Me.ContentSubType = ContentSubType
            Me.CharSet = Charset
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="HeaderValue"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	3/4/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New(ByVal HeaderValue As String)
            Value = HeaderValue
        End Sub
#End Region
#Region " Public Properties"
        Public Property ContentType() As String
            Get
                Return _ContentType
            End Get
            Set(ByVal Value As String)
                _ContentType = Value
            End Set
        End Property

        Public Property ContentSubType() As String
            Get
                Return _ContentSubType
            End Get
            Set(ByVal Value As String)
                _ContentSubType = Value
            End Set
        End Property

        Public Property CharSet() As String
            Get
                Dim RVal As String
                If IsNothing(_Charset) OrElse _Charset.Trim.Length = 0 Then
                    RVal = "us-ascii"
                Else
                    RVal = _Charset
                End If
                Return RVal
            End Get
            Set(ByVal Value As String)
                _Charset = Value
            End Set
        End Property

        Public Property Boundary() As String
            Get
                Return _Boundary
            End Get
            Set(ByVal Value As String)
                _Boundary = Value
            End Set
        End Property

        Public ReadOnly Property Parameters() As NameValueCollection
            Get
                If IsNothing(_Parameters) Then
                    _Parameters = New NameValueCollection
                End If
                Return _Parameters
            End Get
        End Property

        Public Overrides ReadOnly Property HeaderType() As MimeMessageHeaderTypes
            Get
                Return MimeMessageHeaderTypes.ContentType
            End Get
        End Property

        Public Overrides Property Name() As String
            Get
                Return "content-type"
            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Public Overrides Property Value() As String
            Get
                Dim RVal As New StringBuilder
                RVal.Append(ContentType)
                RVal.Append("/")
                RVal.Append(ContentSubType)
                RVal.Append("; charset=")
                RVal.Append(CharSet)
                If Parameters.Count > 0 Then
                    RVal.Append(";")
                    For Each Name As String In Parameters
                        RVal.Append(Name & " = " & Parameters(Name) & ";")
                    Next
                    RVal.Remove(RVal.Length - 2, 1)
                End If
                Return RVal.ToString
            End Get
            Set(ByVal Value As String)
                Dim TmpVal As String = Value
                'Remove any comments from the string
                TmpVal = Regex.Replace(TmpVal, "(?im)\((.|\r\n|\n|\r)*\)", String.Empty)

                'Find the content-type contenttype/contentsubtype
                Dim matches As MatchCollection
                matches = Regex.Matches(TmpVal, "(?is)(?<ContentType>\S+)/(?<ContentSubType>[^;]+);?\s*((?<name>.*?)=\\""(?<value>.*?)\\""($|(\s*;\s*))|(?<name>.*?)=""(?<value>.*?)""($|(\s*;\s*))|(?<name>.*)=(?<value>.*?)($|(\s*;\s*)))*")
                If matches.Count > 0 Then
                    For i As Integer = 0 To matches.Count - 1
                        If matches(i).Groups("ContentType").Value.Trim.Length > 0 AndAlso matches(i).Groups("ContentSubType").Value.Trim.Length > 0 Then
                            ContentType = matches(i).Groups("ContentType").Value.Trim.ToLower
                            ContentSubType = matches(i).Groups("ContentSubType").Value.Trim.ToLower
                        End If
                        If matches(i).Groups("name").Captures.Count > 0 AndAlso matches(i).Groups("name").Captures.Count > 0 Then
                            For j As Integer = 0 To matches(i).Groups("name").Captures.Count - 1
                                Select Case matches(i).Groups("name").Captures(j).Value.Trim.ToLower
                                    Case "charset"
                                        CharSet = matches(i).Groups("value").Captures(j).Value
                                    Case "boundary"
                                        Boundary = matches(i).Groups("value").Captures(j).Value
                                    Case Else
                                        Parameters.Add(matches(i).Groups("name").Captures(j).Value, matches(i).Groups("value").Captures(j).Value)
                                End Select
                            Next
                        End If

                    Next
                End If
            End Set
        End Property
#End Region
#Region " Public Methods"
        Public Sub GenerateNewBoundary()
            Me.Boundary = Guid.NewGuid.ToString.Replace("-", "")
        End Sub

        Public Overrides Function GetFormattedHeader() As String
            Return String.Empty
        End Function
#End Region
    End Class
End Namespace