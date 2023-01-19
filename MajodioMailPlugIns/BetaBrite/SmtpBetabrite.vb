Imports Majodio.BetaBrite.Common
Imports System.Timers
Imports System.Threading
Imports Majodio.Mail.Plugin

Namespace BetaBrite
    Public Class SmtpBetabrite
        Implements ISmtpServer

        Private _tmrBetaBrite As Timers.Timer

        Public Sub New()

        End Sub

        Public Sub DnsblBlockedMessage(ByVal Message As Mime.Message, ByVal IpAddress As System.Net.IPAddress) Implements ISmtpServer.DnsblBlockedMessage
            Dim Config As New SmtpBetabriteConfig
            If Config.ShowMailBlockedMessage Then
                Dim C As Client = Nothing
                Try
                    C = New Client(Config.Server)
                    Dim Message1 As String = "Message Blocked"
                    Dim Message2 As String
                    Message2 = "From: " & Message.FromAddress.ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                    For i As Integer = 0 To Message.ToAddresses.Count - 1
                        Message2 &= "To: " & Message.ToAddresses(i).ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                    Next
                    Message2 &= "IP: " & IpAddress.ToString()

                    C.Sign.Open(1)
                    C.Sign.UseMemoryText("A"c, Message1.Length * 2)
                    C.Sign.UseMemoryText("B"c, Message2.Length * 2)
                    C.Sign.AllocateMemory()

                    C.Sign.SetText("A"c, Message1, Thirdparty.BetaBrite.Protocol.Transition.Flash)
                    C.Sign.SetText("B"c, Message2, Thirdparty.BetaBrite.Protocol.Transition.CompressedRotate)
                    C.Sign.SetRunSequence("AB")

                    If IsNothing(_tmrBetaBrite) Then
                        _tmrBetaBrite = New System.Timers.Timer()
                        _tmrBetaBrite.AutoReset = False
                        AddHandler _tmrBetaBrite.Elapsed, AddressOf ResetBetaBriteMesage
                    Else
                        _tmrBetaBrite.Stop()
                    End If
                    _tmrBetaBrite.Interval = Config.MailBlockedDisplayTime * 1000
                    _tmrBetaBrite.Start()
                Finally
                    Try
                        If Not IsNothing(C) AndAlso Not IsNothing(C.Sign) Then
                            C.Sign.Close()
                            C = Nothing
                        End If
                    Catch ex As Exception
                        'Do Nothing
                    End Try
                End Try
            End If
        End Sub

        Public Sub DnsblBlockedIp(ByVal IpAddress As System.Net.IPAddress) Implements ISmtpServer.DnsblBlockedIp
            Dim Config As New SmtpBetabriteConfig
            If Config.ShowMailBlockedMessage Then
                Dim C As Client = Nothing
                Try
                    C = New Client(Config.Server)
                    Dim Message1 As String = "Mail Blocked"
                    Dim Message2 As String = IpAddress.ToString()

                    C.Sign.Open(1)
                    C.Sign.UseMemoryText("A"c, Message1.Length * 2)
                    C.Sign.UseMemoryText("B"c, Message2.Length * 2)
                    C.Sign.AllocateMemory()


                    C.Sign.SetText("A"c, Message1, Thirdparty.BetaBrite.Protocol.Transition.Flash)
                    C.Sign.SetText("B"c, Message2, Thirdparty.BetaBrite.Protocol.Transition.CompressedRotate)
                    C.Sign.SetRunSequence("AB")

                    If IsNothing(_tmrBetaBrite) Then
                        _tmrBetaBrite = New System.Timers.Timer()
                        _tmrBetaBrite.AutoReset = False
                        AddHandler _tmrBetaBrite.Elapsed, AddressOf ResetBetaBriteMesage
                    Else
                        _tmrBetaBrite.Stop()
                    End If
                    _tmrBetaBrite.Interval = Config.MailBlockedDisplayTime * 1000
                    _tmrBetaBrite.Start()
                Finally
                    Try
                        If Not IsNothing(C) AndAlso Not IsNothing(C.Sign) Then
                            C.Sign.Close()
                            C = Nothing
                        End If
                    Catch ex As Exception
                        'Do Nothing
                    End Try
                End Try
            End If
        End Sub

        Public Sub MailReceived(ByVal Message As Mime.Message, ByVal IsRelayConnection As Boolean) Implements ISmtpServer.MailReceived
            If Not IsRelayConnection Then
                Dim Config As New SmtpBetabriteConfig
                If Config.ShowNewMail Then
                    Dim C As Client = Nothing
                    Try
                        C = New Client(Config.Server)
                        Dim Message1 As String = "NEW MAIL"
                        Dim Message2 As String = String.Empty

                        Message2 = "From: " & Message.FromAddress.ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                        For i As Integer = 0 To Message.ToAddresses.Count - 1
                            Message2 &= "To: " & (i + 1) & ") " & Message.ToAddresses(i).ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                        Next
                        For i As Integer = 0 To Message.CcAddresses.Count - 1
                            Message2 &= "Cc: " & (i + 1) & ") " & Message.CcAddresses(i).ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                        Next
                        For i As Integer = 0 To Message.BccAddresses.Count - 1
                            Message2 &= "Bcc: " & (i + 1) & ") " & Message.BccAddresses(i).ToString(Majodio.Common.EmailStringFormat.NameAddress) & "<newline>"
                        Next

                        Message2 &= "Subject: " & Message.Subject

                        C.Sign.Open(1)
                        C.Sign.UseMemoryText("A"c, Message1.Length * 2)
                        C.Sign.UseMemoryText("B"c, Message2.Length * 2)
                        C.Sign.AllocateMemory()


                        C.Sign.SetText("A"c, Message1, Thirdparty.BetaBrite.Protocol.Transition.Flash)
                        C.Sign.SetText("B"c, Message2, Thirdparty.BetaBrite.Protocol.Transition.CompressedRotate)
                        C.Sign.SetRunSequence("AB")

                        If IsNothing(_tmrBetaBrite) Then
                            _tmrBetaBrite = New System.Timers.Timer()
                            _tmrBetaBrite.AutoReset = False
                            AddHandler _tmrBetaBrite.Elapsed, AddressOf ResetBetaBriteMesage
                        Else
                            _tmrBetaBrite.Stop()
                        End If
                        _tmrBetaBrite.Interval = Config.NewMailMessageDisplayTime * 1000
                        _tmrBetaBrite.Start()
                    Catch Ex As Exception
                    Finally
                        Try
                            If Not IsNothing(C) AndAlso Not IsNothing(C.Sign) Then
                                C.Sign.Close()
                                C = Nothing
                            End If
                        Catch ex As Exception
                            'Do Nothing
                        End Try
                    End Try

                End If
            End If
        End Sub

        Private Sub ResetBetaBriteMesage(ByVal Sender As Object, ByVal E As Timers.ElapsedEventArgs)
            Dim Config As New SmtpBetabriteConfig
            Dim C As Client = Nothing
            Try
                C = New Client(Config.Server)

                C.Sign.Open(1)
                C.Sign.UseMemoryText("A"c, Config.DefaultMessage.Length * 2)
                C.Sign.AllocateMemory()


                C.Sign.SetText("A"c, Config.DefaultMessage, Thirdparty.BetaBrite.Protocol.Transition.CompressedRotate)
                C.Sign.SetRunSequence("A")
            Finally
                Try
                    If Not IsNothing(C) AndAlso Not IsNothing(C.Sign) Then
                        C.Sign.Close()
                        C = Nothing
                    End If
                Catch ex As Exception
                    'Do Nothing
                End Try
            End Try
        End Sub

        Public ReadOnly Property Enabled() As Boolean Implements ISmtpServer.Enabled
            Get
                Dim Config As New SmtpBetabriteConfig
                Return Config.Enabled
            End Get
        End Property
    End Class
End Namespace