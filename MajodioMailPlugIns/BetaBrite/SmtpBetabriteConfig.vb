Imports Majodio.Mail.PlugIn
Imports Microsoft.Win32
Imports System.ComponentModel
Imports System.ComponentModel.Design

Namespace BetaBrite
    Public Class SmtpBetabriteConfig
        Implements IPluginConfiguration

        Public ReadOnly Property Title() As String Implements IPluginConfiguration.Title
            Get
                Return "Majodio Mail - Betabrite configuration"
            End Get
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("New Mail Message Display Time"), _
        Description("The number of seconds that the new mail message should appear on the Betabrite sign")> _
        Public Property NewMailMessageDisplayTime() As Int32
            Get
                Dim rVal As Int32 = 150
                Dim tmpRval As Object = BetabriteRegistry.GetRegistryValue("NewMailMessageDisplayTime")
                If Not IsNothing(tmpRval) AndAlso IsNumeric(tmpRval) Then
                    rVal = Convert.ToInt32(tmpRval)
                Else
                    NewMailMessageDisplayTime = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As Int32)
                BetabriteRegistry.SetRegistryValue("NewMailMessageDisplayTime", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Mail Blocked Display Time"), _
        Description("The number of seconds that the mail blocked message should appear on the Betabrite sign")> _
        Public Property MailBlockedDisplayTime() As Int32
            Get
                Dim rVal As Int32 = 150
                Dim tmpRval As Object = BetabriteRegistry.GetRegistryValue("MailBlockedDisplayTime")
                If Not IsNothing(tmpRval) AndAlso IsNumeric(tmpRval) Then
                    rVal = Convert.ToInt32(tmpRval)
                Else
                    MailBlockedDisplayTime = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As Int32)
                BetabriteRegistry.SetRegistryValue("MailBlockedDisplayTime", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Default Message"), _
        Description("The default message that will be displayed on the sign.  This is factory preset to the current date/time.")> _
        Public Property DefaultMessage() As String
            Get
                Dim rVal As String = "<calldate=ddd> <calldate=mm/dd/yy> -=- <calltime>"
                Dim tmpRval = BetabriteRegistry.GetRegistryValue("DefaultMessage")
                If Not IsNothing(tmpRval) Then
                    rVal = tmpRval
                Else
                    DefaultMessage = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As String)
                BetabriteRegistry.SetRegistryValue("DefaultMessage", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Server"), _
        Description("The name of the server that has the Betabrite remoting server installed.  This software can be obtained from Majodio Software (http://www.majodio.com)")> _
        Public Property Server() As String
            Get
                Dim rVal As String = "filesvr3"
                Dim tmpRval = BetabriteRegistry.GetRegistryValue("Server")
                If Not IsNothing(tmpRval) Then
                    rVal = tmpRval
                Else
                    BetabriteRegistry.SetRegistryValue("Server", rVal)
                End If
                Return rVal
            End Get
            Set(ByVal value As String)
                BetabriteRegistry.SetRegistryValue("Server", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Enabled"), _
        Description("Determines if this particular plugin is enabled for the Majodio Mail Server.  If this is set to false, the server will not interact with this plugin.")> _
        Public Property Enabled() As Boolean
            Get
                Dim rVal As Int32 = True
                Dim tmpRval As Object = BetabriteRegistry.GetRegistryValue("Enabled")
                If Not IsNothing(tmpRval) Then
                    rVal = Convert.ToBoolean(tmpRval)
                Else
                    Enabled = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As Boolean)
                BetabriteRegistry.SetRegistryValue("Enabled", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Show Mail Blocked Message"), _
        Description("If true, the sign will show messages that were blocked via the DNSBL feature")> _
        Public Property ShowMailBlockedMessage() As Boolean
            Get
                Dim rVal As Int32 = True
                Dim tmpRval As Object = BetabriteRegistry.GetRegistryValue("ShowMailBlockedMessage")
                If Not IsNothing(tmpRval) Then
                    rVal = Convert.ToBoolean(tmpRval)
                Else
                    ShowMailBlockedMessage = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As Boolean)
                BetabriteRegistry.SetRegistryValue("ShowMailBlockedMessage", value)
            End Set
        End Property

        <Category("Majodio Mail - Betabrite configuration"), _
        DisplayName("Show New Mail"), _
        Description("If true, the sign will show message as they are received by the mail server")> _
        Public Property ShowNewMail() As Boolean
            Get
                Dim rVal As Int32 = True
                Dim tmpRval As Object = BetabriteRegistry.GetRegistryValue("ShowNewMail")
                If Not IsNothing(tmpRval) Then
                    rVal = Convert.ToBoolean(tmpRval)
                Else
                    ShowNewMail = rVal
                End If
                Return rVal
            End Get
            Set(ByVal value As Boolean)
                BetabriteRegistry.SetRegistryValue("ShowNewMail", value)
            End Set
        End Property
    End Class

End Namespace