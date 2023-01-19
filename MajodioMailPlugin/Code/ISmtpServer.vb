Public Interface ISmtpServer
    Sub MailReceived(ByVal Message As Mime.Message, ByVal RelayConnection As Boolean)
    Sub DnsblBlockedIp(ByVal IpAddress As System.Net.IPAddress)
    Sub DnsblBlockedMessage(ByVal Message As Mime.Message, ByVal IpAddress As System.Net.IPAddress)
    ReadOnly Property Enabled() As Boolean
End Interface