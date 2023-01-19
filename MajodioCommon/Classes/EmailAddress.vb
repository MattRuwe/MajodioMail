Imports System.Text.RegularExpressions

Namespace Majodio.Common
    <Serializable()> _
    Public Class EmailAddress
        Public Sub New()
            _Address = String.Empty
            _Name = String.Empty
        End Sub

        Public Sub New(ByVal Email As String)
            Me.new()
            Dim BracesStartIndex As Integer
            Dim BracesEndIndex As Integer
            BracesStartIndex = Email.IndexOf("<")
            BracesEndIndex = Email.IndexOf(">")
            If BracesStartIndex > -1 AndAlso BracesEndIndex > -1 AndAlso Email.IndexOf("<", BracesEndIndex) = -1 AndAlso BracesStartIndex < BracesEndIndex Then
                Name = Email.Substring(0, BracesStartIndex).Trim
                'Matt Ruwe <ruwem@tsainc.com>  blah blah blah
                Address = Email.Substring(BracesStartIndex + 1, BracesEndIndex - BracesStartIndex - 1)
            Else
                Address = Email
            End If
        End Sub

        Public Sub New(ByVal Email As String, ByVal Name As String)
            Me.New(Email)
            Me.Name = Name
        End Sub

        Private _Address As String
        Public Property Address() As String
            Get
                If Not IsNothing(_Address) Then
                    Return _Address
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal Value As String)
                If Not IsNothing(Value) AndAlso Value.Trim.Length > 0 Then
                    If Not Majodio.Functions.IsEmailAddress(Value) Then
                        Throw New InvalidAddressException(Value)
                    End If
                    Dim CompAddress As String = Value
                    If CompAddress.Substring(0, 1) = "<" Then
                        CompAddress = CompAddress.Substring(1)
                    End If
                    If CompAddress.Substring(CompAddress.Length - 1, 1) = ">" Then
                        CompAddress = CompAddress.Substring(0, CompAddress.Length - 1)
                    End If
                    _Address = CompAddress
                Else
                    _Address = String.Empty
                End If
            End Set
        End Property

        Private _Name As String
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        Public Function GetUsername() As String
            Dim AtIndex As Integer
            Dim RVal As String = String.Empty
            If Majodio.Functions.IsEmailAddress(Address) Then
                AtIndex = Address.IndexOf("@")
                RVal = Address.Substring(0, AtIndex)
            End If
            Return RVal
        End Function

        Public Function GetDomain() As String
            Dim AtIndex As Integer
            Dim RVal As String = String.Empty
            If Majodio.Functions.IsEmailAddress(Address) Then
                AtIndex = Address.IndexOf("@")
                RVal = Address.Substring(AtIndex + 1)
            End If
            Return RVal
        End Function


        Public Overloads Function ToString() As String
            Return Address
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Format"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[ruwem]	3/13/2005	Fixed a problem with the EmailStringFormat.AddressBraces and 
        '''                         EmailStringFormat.NameAddressBraces areas that was causing a 
        '''                         problem when sending emails with blank MAIL FROM's.  The 
        '''                         routine now formats the email with less than or greater than  
        '''                         instead of an empty string
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overloads Function ToString(ByVal Format As EmailStringFormat) As String
            Dim RVal As String = String.Empty
            Select Case Format
                Case EmailStringFormat.NameAddressBraces
                    If Address <> String.Empty AndAlso Name <> String.Empty Then
                        RVal = Name & " <" & Address & ">"
                    ElseIf Address <> String.Empty Then
                        RVal = "<" & Address & ">"
                    Else
                        RVal = "<>"
                    End If
                Case EmailStringFormat.NameAddress
                    If Address <> String.Empty AndAlso Name <> String.Empty Then
                        RVal = Name & " " & Address
                    ElseIf Address <> String.Empty Then
                        RVal = Address
                    ElseIf Name <> String.Empty Then
                        RVal = Name
                    End If
                Case EmailStringFormat.AddressBraces
                    If Not IsNothing(Address) AndAlso Address <> String.Empty Then
                        RVal = "<" & Address & ">"
                    Else
                        RVal = "<>"
                    End If
                Case EmailStringFormat.Name
                    RVal = Name
                Case EmailStringFormat.Address
                    RVal = Address
                Case EmailStringFormat.Imap4Envelope
                    RVal = "("
                    If Name <> String.Empty Then
                        While Name.StartsWith("""")
                            Name = Name.Substring(1)
                        End While
                        While Name.EndsWith("""")
                            Name = Name.Substring(0, Name.Length - 1)
                        End While
                        RVal &= "{" & Name.Length & "}" & vbCrLf & Name & " "
                    Else
                        RVal &= "NIL "
                    End If
                    RVal &= "NIL "
                    'RVal &= " " & Me.ToString(EmailStringFormat.Address) & " "
                    RVal &= """" & GetUsername() & """ "
                    RVal &= """" & GetDomain() & """"
                    RVal &= ")"
            End Select
            Return RVal
        End Function

        Public Shared Function IsValidAddress(ByVal email As String) As Boolean
            Return Regex.IsMatch(email, "^([0-9a-zA-Z]+[-._+&amp;])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$")
        End Function

        Private _tag As Object
        Public Property Tag() As Object
            Get
                Return _tag
            End Get
            Set(ByVal value As Object)
                _tag = value
            End Set
        End Property

    End Class
End Namespace