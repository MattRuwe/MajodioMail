Imports Majodio.Common
Imports Majodio.Mail.Mime
Imports Majodio.Mail.Server.Functions
Imports System.Threading
Imports Majodio.Mail.Common

''' <summary>
''' Sends e-mail to local or remote remote stores.
''' </summary>
''' <remarks>This class is essentially a wrapper for the <see cref="T:Majodio.Mail.Mime.Message" /> class making it easier to use.</remarks>
Public Class SmtpMessage

    Private _MimeMessage As Message
    ''' <summary>
    ''' Default constructor initializes the class
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Reset()
    End Sub

    Private _LockObject As Object = New Object

    Private Delegate Sub SendDelegate()
    ''' <summary>
    ''' Sends the message asynchronously i.e. the method will immediately return and send the e-mail in the background.
    ''' </summary>
    ''' <param name="Callback">The method to invoke when the call completes</param>
    ''' <remarks>This method creates a new thread that sends the e-mail.  Care must be taken no to use all of the 
    ''' threads in the threadpool or an exception may occurr.</remarks>
    Public Sub BeginSend(ByVal Callback As AsyncCallback)
        Dim D As New SendDelegate(AddressOf Send)
        D.BeginInvoke(Callback, Nothing)
    End Sub

    ''' <summary>
    ''' After the properties of the object have been initialized, this method sends the mail message to the appropriate recipient(s).
    ''' </summary>
    ''' <exception cref="T:Majodio.Mail.Api.SmtpMessageException">
    ''' Occurs if the From is not set, or if no recipients (to, cc, bcc) are set.
    ''' </exception>
    ''' <remarks>The sending SMTP client does not report back error messages if the message could not be delivered
    ''' to the destination SMTP server.  Also, unlike many other SMTP components, Majodio's does not require that
    ''' you set an SMTP server as the componet handles this for you.  However, if you have the component behind a 
    ''' firewall, make sure that at least outgoing TCP/IP connections can be made through TCP port 25.</remarks>
    Public Sub Send()
        Try
            Monitor.Enter(_LockObject)
            If IsNothing(From) Then
                Throw New SmtpMessageException("The message does not have a from address")
            End If
            If [To].Count = 0 And Cc.Count = 0 And Bcc.Count = 0 Then
                Throw New SmtpMessageException("The message does not have any recipients")
            End If
            For i As Integer = 0 To [To].Count - 1
                Send(_MimeMessage, [To](i))
            Next
            For i As Integer = 0 To Cc.Count - 1
                Send(_MimeMessage, Cc(i))
            Next
            For i As Integer = 0 To Bcc.Count - 1
                Send(_MimeMessage, Bcc(i))
            Next
        Finally
            Monitor.Exit(_LockObject)
        End Try
    End Sub

    Private Sub Send(ByVal MimeMessage As Message, ByVal Recipient As EmailAddress)
        Try
            Monitor.Enter(_LockObject)
            Dim QM As Majodio.Mail.Common.Storage.QueuedMail.Message
            MimeMessage.SmtpToAddress = Recipient
            MimeMessage.AddReceivedHeader(GetLocalIpAddress, System.Net.IPAddress.Parse(GetLocalIpAddress), System.Net.Dns.GetHostName)
            QM = MimeMessage.GetQueuedMessage(Recipient.GetDomain, Recipient.GetUsername)
            QM.DateTime = DateTime.Now
            QM.Save()
        Finally
            Monitor.Exit(_LockObject)
        End Try
    End Sub

    ''' <summary>
    ''' <para>Returns the object to an initialized state.  All information inside the <see cref="P:Majodio.Mail.Api.SmtpMessage.MimeMessage" /> object is destroyed.</para>
    ''' </summary>
    ''' <remarks>When this method is invoked, a new instance of the <see cref="T:Majodio.Mail.Mime.Message" /> class is created.
    ''' All of the information that was contained previous is purged.  This method is generally useful when you have sent one e-mail
    ''' and need to send another.</remarks>
    Public Sub Reset()
        Try
            Monitor.Enter(_LockObject)
            _MimeMessage = New Message(True)
        Finally
            Monitor.Exit(_LockObject)
        End Try
    End Sub

    ''' <summary>
    ''' The email address(es) that the message will be delivered to
    ''' </summary>
    ''' <returns>A collection of <see cref="T:Majodio.Common.EmailAddress" /> objects that represent who the e-mail will be delivered to.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property [To]() As EmailAddressCollection
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.ToAddresses
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' The email address(es) that the message will be cc'ed to
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of <see cref="T:Majodio.Common.EmailAddress" /> objects that represent who the e-mail will be carbon copied (CC'ed) to.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Cc() As EmailAddressCollection
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.CcAddresses
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Gets the collection of 
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of <see cref="T:Majodio.Common.EmailAddress" /> objects that represent who the e-mail will be blind carbon copied (BCC'ed) to.</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Bcc() As EmailAddressCollection
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.BccAddresses
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the subject of the e-mail address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Subject() As String
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.Subject
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
        Set(ByVal value As String)
            Try
                Monitor.Enter(_LockObject)
                _MimeMessage.Subject = value
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Set
    End Property

    ''' <summary>
    ''' The Body of the e-mail address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Body() As String
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.RootBodyPart.BodyPartContent
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
        Set(ByVal value As String)
            Try
                Monitor.Enter(_LockObject)
                _MimeMessage.RootBodyPart.BodyPartContent = value
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Set
    End Property

    ''' <summary>
    ''' The From email address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property From() As EmailAddress
        Get
            Try
                Monitor.Enter(_LockObject)
                Return _MimeMessage.FromAddress
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Get
        Set(ByVal value As EmailAddress)
            Try
                Monitor.Enter(_LockObject)
                _MimeMessage.FromAddress = value
                _MimeMessage.SmtpFromAddress = value
            Finally
                Monitor.Exit(_LockObject)
            End Try
        End Set
    End Property

    ''' <summary>
    ''' The underlying mime message that forms the message
    ''' </summary>
    ''' <returns>An instance of the mime message that is encapsulated by this object</returns>
    ''' <remarks>The property provides access to the underlying MIME message.  This is a very
    ''' powerful feature that requires knowledge of MIME message makeup and the Majodio Mail
    ''' MIME interface.  Please refer to the <see cref="T:Majodio.Mail.Mime.Message" /> documentation for more information.</remarks>
    Public ReadOnly Property MimeMessage() As Mime.Message
        Get
            Return _MimeMessage
        End Get
    End Property
End Class
