Namespace Imap4
    'These states are defined in RFC 2060 Page 11
    Public Enum SessionState
        NonAuthenticated
        Authenticated
        Selected
        Logout
        Unknown
    End Enum
End Namespace