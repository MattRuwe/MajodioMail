Imports System.Text.RegularExpressions

Namespace Majodio.Common
    Public Class EmailAddressCollection
        Inherits System.Collections.CollectionBase

        Public Event CollectionChanged As EventHandler

        Public Sub Add(ByVal Email As String)
            Dim TmpEmail As EmailAddress = Nothing
            Dim Emails As String()
            Email = Regex.Replace(Email, "(?i)(""|').+?(\1)", String.Empty)

            Email = Replace(Email, ",", ";").Trim.ToLower
            Email = Replace(Email, ":", ";").Trim.ToLower
            Emails = Split(Email, ";")
            For i As Integer = 0 To Emails.GetUpperBound(0)
                If Emails(i).Trim.Length > 0 Then
                    If Functions.IsEmailAddress(Emails(i)) Then
                        TmpEmail = New EmailAddress(Emails(i))
                        Add(TmpEmail)
                    Else
                        Throw New InvalidAddressException(Emails(i))
                    End If
                End If
            Next
        End Sub

        Public Function Add(ByVal Email As EmailAddress) As EmailAddress
            MyBase.List.Add(Email)
            RaiseEvent CollectionChanged(Me, EventArgs.Empty)
            Return Email
        End Function

        Public Sub Add(ByVal Emails As EmailAddressCollection)
            Dim i As Integer
            If Not IsNothing(Emails) Then
                For i = 0 To Emails.Count - 1
                    Add(Emails(i))
                Next
            End If
        End Sub

        Default Public Property Item(ByVal Index As Integer) As EmailAddress
            Get
                Return CType(MyBase.List(Index), EmailAddress)
            End Get
            Set(ByVal Value As EmailAddress)
                MyBase.List(Index) = Value
                RaiseEvent CollectionChanged(Me, EventArgs.Empty)
            End Set
        End Property

        Public Function GetImap4EnvelopAddresses() As String
            Dim RVal As String = String.Empty
            If MyBase.Count > 0 Then
                RVal &= "("
                For i As Integer = 0 To MyBase.Count - 1
                    RVal &= Item(i).ToString(EmailStringFormat.Imap4Envelope)
                Next
                RVal &= ")"
            Else
                RVal &= "NIL"
            End If
            Return RVal
        End Function
    End Class
End Namespace