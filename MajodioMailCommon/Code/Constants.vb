Public Module Constants
    'Trial software settings
#If TRIAL_VERSION = False Then
    Public Const TRIAL_SOFTWARE As Boolean = False
#Else
    Public Const TRIAL_SOFTWARE as Boolean = True
#End If

    Public Const TRIAL_SOFTWARE_MAX_CONNECTIONS As Integer = 1000

    ' Version
    Public Const MAIL_ASSEMBLY_VERSION As String = "1.2.55.0"

    ' Date Formats
    Public Const INTERNATIONAL_DATE_TIME_FORMAT As String = "dd-MMM-yyyy HH:mm:ss"

    ' Performance Counters
    Public Const PERF_COUNTER_CATEGORY_NAME As String = "Majodio Mail"
    Public Const PERF_COUNTER_CURRENT_SMTP_SESSIONS As String = "Current SMTP Sessions"
    Public Const PERF_COUNTER_TIMED_OUT_SMTP_SESSIONS As String = "Timed-out SMTP Sessions"
    Public Const PERF_COUNTER_TOTAL_SMTP_SESSIONS As String = "Total SMTP Sessions"
    Public Const PERF_COUNTER_MESSAGES_RECEIVED As String = "Messages Received"
    Public Const PERF_COUNTER_CONNECTIONS_REJECTED_DNSBL As String = "Connections Rejected DNSBL"
    Public Const PERF_COUNTER_MESSAGES_REJECTED_DNSBL As String = "Messages Rejected DNSBL"
    Public Const PERF_COUNTER_MESSAGES_REJECTED_INVALID_FROM As String = "Messages Rejected Invalid From"
    Public Const PERF_COUNTER_CURRENT_POP3_SESSIONS As String = "Current POP3 Sessions"
    Public Const PERF_COUNTER_TIMED_OUT_POP3_SESSIONS As String = "Timed-out POP3 Sessions"
    Public Const PERF_COUNTER_TOTAL_POP3_SESSIONS As String = "Total POP3 Sessions"
    Public Const PERF_COUNTER_MESSAGES_DELIVERED As String = "Messages Delivered"

    ' Folder locations/names
    Public Const DATA_FILES_FOLDER As String = "MajodioMail"
    Public Const CONFIG_FILE_LOCATION As String = DATA_FILES_FOLDER & "\config\config.xml"
    Public Const DOMAIN_FILE_LOCATION As String = DATA_FILES_FOLDER & "\config\domains.xml"
    Public Const POP3RELAY_CONFIG_FILE_LOCATION As String = DATA_FILES_FOLDER & "\config\pop3relay.xml"
    Public Const MAIL_FOLDER_CONFIG_FILENAME As String = "config.xml"
    Public Const QUEUED_MAIL_MAILBOX_FOLDER As String = DATA_FILES_FOLDER & "\mailboxes"
    Public Const QUEUED_MAIL_FOLDER As String = QUEUED_MAIL_MAILBOX_FOLDER & "\queue"
    Public Const UNDELIVERABLE_MAIL_FOLDER As String = QUEUED_MAIL_MAILBOX_FOLDER & "\undeliverable"
    Public Const LOG_FILE_DIRECTORY As String = DATA_FILES_FOLDER & "\log"
    Public Const ERROR_FILE_DIRECTORY As String = DATA_FILES_FOLDER & "\errors"
    Public Const RETAINED_MAIL_FOLDER As String = "_RetainedMail"
    Public Const FAILED_MAIL_FOLDER As String = "_FailedMail"

    ' Service TCP Send/Receive Behavior
    Public Const DATA_RECEIVE_TIMEOUT As Integer = 300
    Public Const SEND_DATA_CHUNK_SIZE As Int16 = 8192
    Public Const RECEIVE_DATA_CHUNK_SIZE As Int16 = 8192

    ' Service TCP Port Conifguration
    Public Const SMTP_DEFAULT_TCP_PORT As Integer = 25
    Public Const POP3_DEFAULT_TCP_PORT As Integer = 110
    Public Const POP3_DEFAULT_SECURE_TCP_PORT As Integer = 995
    Public Const IMAP4_DEFAULT_TCP_PORT As Integer = 143

    ' Logger Config
    Public Const MAX_LOG_FILE_ENTRY_LENGTH As Integer = 2048
    Public Const LOG_LATEST_MESSAGES_QUEUE_SIZE As Integer = 100

    ' Logger Remoting configuration
    Public Const LOGGER_CHANNEL_NAME As String = "MajodioLogger"
    Public Const LOGGER_CLIENT_CONNECTION_LIMIT As Integer = 50
    Public Const LOGGER_REMOTE_HOST As String = "localhost"
    Public Const LOGGER_TCP_PORT As Integer = 26465
    Public Const LOGGER_PATH As String = "MailLogger"

    ' Config Remoting configuration
    Public Const CONFIG_CHANNEL_NAME As String = "MajodioConfig"
    Public Const CONFIG_CLIENT_CONNECTION_LIMIT As Integer = 50
    Public Const CONFIG_REMOTE_HOST As String = "localhost"
    Public Const CONFIG_TCP_PORT As Integer = 26466
    Public Const CONFIG_PATH As String = "MailConfig"

    ' Domain Remoting configuration
    Public Const DOMAIN_CHANNEL_NAME As String = "MajodioDomain"
    Public Const DOMAIN_CLIENT_CONNECTION_LIMIT As Integer = 50
    Public Const DOMAIN_REMOTE_HOST As String = "localhost"
    Public Const DOMAIN_TCP_PORT As Integer = 26467
    Public Const DOMAIN_PATH As String = "MailDomain"

    ' Queued Mail Folder/Messages Remoting configuration
    Public Const FOLDER_MESSAGE_TCP_PORT As Integer = 26468
    Public Const FOLDER_MESSAGE_HTTP_PORT As Integer = 26469
    Public Const FOLDER_MESSAGE_REMOTING_APP_NAME As String = "RemoteFolderMessage"
    Public Const FOLDER_MESSAGE_REMOTING_HOST As String = "localhost"

    

    ' Messages
    ' SMTP
    Public Const SMTP_SESSION_STARTING_LOG_ENTRY As String = "************Starting SMTP session************"
    Public Const SMTP_SESSION_ENDING_LOG_ENTRY As String = "************Ending SMTP session************"
    Public Const SMTP_SESSION_EXPIRED_LOG_ENTRY As String = "Session expired; closing connection"
    Public Const SMTP_SERVER_CRASHED_LOG_ENTRY As String = "A fatal error occurred which caused the SMTP server session to crash: "
    Public Const SMTP_MESSAGE_QUEUED_RESPONSE As String = "Message Queued"
    ' POP3
    Public Const POP3_SESSION_STARTING_LOG_ENTRY As String = "************Starting POP3 session************"
    Public Const POP3_SESSION_ENDING_LOG_ENTRY As String = "************Ending POP3 session************"
    Public Const POP3_SESSION_EXPIRED_LOG_ENTRY As String = "Session expired; closing connection"
    Public Const POP3_SERVER_CRASHED_LOG_ENTRY As String = "A fatal error occurred which caused the POP3 server session to crash: "
    ' IMAP4
    Public Const IMAP4_SESSION_STARTING_LOG_ENTRY As String = "************Starting IMAP4 session************"
    Public Const IMAP4_SESSION_ENDING_LOG_ENTRY As String = "************Ending IMAP4 session************"
    Public Const IMAP4_LOGOUT_MESSAGE As String = "BYE MAJODIO IMAP4rev1 SERVER LOGGING OUT"
    Public Const IMAP4_HEIRARCHY_DELIMETER As Char = "/"c
    ' TCP
    Public Const TCP_SEND_DATA_ERROR_LOG_ENTRY As String = "An error occurred while sending data to the client:"
    Public Const TCP_SEND_DATA_BYTE_COUNT_MISMATCH_LOG_ENTRY As String = "WARNING: The number of bytes actually sent does not match the expected number of bytes that should have been sent"
    Public Const TCP_RECEIVE_DATA_ERROR_SOCKET_LOG_ENTRY As String = "A socket exception has occurred.  The error code is: "
    Public Const TCP_RECEIVE_DATA_ERROR_CONNECTION_CLOSED_LOG_ENTRY As String = "While reading data from the socket, the connection was apparently closed."
    Public Const TCP_RECEIVE_DATA_PREPEND_TEXT_LOG_ENTRY As String = "Client: "
    Public Const TCP_RECEIVE_DATA_ERROR_NO_DATA_LOG_ENTRY As String = "No data was returned from the receive buffer"
    Public Const TCP_RECEIVE_DATA_ERROR_GENERAL_LOG_ENTRY As String = "An error occurred while moving data from the socket"

    ' Special Mailboxes
    Public Const MAILBOX_INBOX As String = "INBOX"

    'SMTP Constants
    Public Const SMTP_POSTMASTER_ACCOUNT_NAME As String = "postmaster"
    Public Const SERVER_ADMIN_ACCOUNT_USERNAME As String = "serveradmin"
    Public Const SERVER_ADMIN_ACCOUNT_PASSWORD As String = "serveradmin"

    'Admin Message Manager
    Public Const ADMIN_MESSAGE_MAnAGER_POLL_INTERVAL As Integer = 5000

    'Plug In Constants
    Public Const PLUG_IN_DIRECTORY As String = "\plugins"
End Module
