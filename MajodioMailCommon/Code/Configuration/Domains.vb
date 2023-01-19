Imports System.Xml
Imports System.IO
Imports System.Collections.Specialized
Imports Majodio.Common
Imports System.Text.RegularExpressions
Imports Majodio.Mail.Common.DataTypes
Imports Majodio.Mail.Common.Storage

Namespace Configuration
    <Serializable()> _
    Public Class Domains
        Inherits PersistentStorage

        Private Shared _domainLock As New System.Threading.ReaderWriterLock

        Public Sub New()
            MyBase.New(GetApplicationDirectory() & "\" & DOMAIN_FILE_LOCATION)
        End Sub

        Public Overrides Function GetDefaultXml() As System.Xml.XmlDocument
            Dim RVal As New XmlDocument

            'Setup the root node
            Dim DomainNode As XmlElement = RVal.CreateNode(XmlNodeType.Element, "domains", "")

            'Add the root node to the document
            RVal.AppendChild(DomainNode)

            'Create the ServerIp Node
            Dim Pop3Relay As XmlElement = RVal.CreateNode(XmlNodeType.Element, "pop3relay", "")
            DomainNode.AppendChild(Pop3Relay)

            Return RVal
        End Function

#Region " Domain Management"
        Public Function Ping() As String
            Return "Pong"
        End Function

        Public Sub AddDomain(ByVal Domain As DomainDetails)
            If Not DomainExists(Domain.Name) Then
                Dim Element As XmlElement = MyBase.CreateXmlElement(MyBase.GetSingleXmlElement("/domains"), "domain", Domain.GetNameValueCollection, _domainLock)
            End If
        End Sub

        Public Sub UpdateDomain(ByVal OldDomain As DomainDetails, ByVal NewDomain As DomainDetails)
            If DomainExists(OldDomain.Name) Then
                MyBase.SetXmlValue("/domains/domain[@name=""" & OldDomain.Name.ToLower & """]", "name", NewDomain.Name, _domainLock)
            End If
        End Sub

        Public Sub DeleteDomain(ByVal DomainName As String)
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]", _domainLock))
        End Sub

        Public Function DomainExists(ByVal Name As String) As Boolean
            Dim RVal As Boolean
            RVal = Not IsNothing(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & Name.ToLower & """]", _domainLock))
            Return RVal
        End Function

        Public Function GetDomains() As DomainDetails()
            Dim Domains As XmlNodeList = MyBase.GetXmlElements("/domains/domain", _domainLock)
            Dim i As Integer
            Dim RVal As DomainDetails() = Nothing
            For i = 0 To Domains.Count - 1
                ReDim Preserve RVal(i)
                RVal(i) = New DomainDetails(Domains.Item(i).Attributes("name").Value)
            Next
            Return RVal
        End Function
#End Region
#Region " User Management"
        Public Sub AddUser(ByVal DomainName As String, ByVal User As UserDetails)
            If DomainExists(DomainName) And Not UserExists(DomainName, User.Username) Then
                Dim Domain As XmlElement = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName & """]", _domainLock)
                MyBase.CreateXmlElement(Domain, "user", User.GetNameValueCollection, _domainLock)
            End If
        End Sub

        Public Sub DeleteUser(ByVal DomainName As String, ByVal Username As String)
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", _domainLock))
        End Sub

        Public Function UserExists(ByVal DomainName As String, ByVal Username As String) As Boolean
            Dim RVal As Boolean
            RVal = Not IsNothing(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", _domainLock))
            Return RVal
        End Function

        Public Function GetUser(ByVal DomainName As String, ByVal UserName As String) As UserDetails
            Dim RVal As UserDetails
            Dim User As XmlElement = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & UserName.ToLower & """]", _domainLock)
            RVal = New UserDetails(User.Attributes("username").Value, User.Attributes("password").Value, User.Attributes("unique_id").Value)
            Return RVal
        End Function

        Public Function GetUsers(ByVal DomainName As String) As UserDetails()
            Dim Users As XmlNodeList = MyBase.GetXmlElements("/domains/domain[@name=""" & DomainName.ToLower & """]/user", _domainLock)
            Dim i As Integer
            Dim RVal As UserDetails()
            ReDim RVal(Users.Count - 1)
            Dim Username As String
            Dim Password As String
            Dim UniqueId As String
            For i = 0 To Users.Count - 1
                If Not IsNothing(Users.Item(i).Attributes("username")) Then
                    Username = Users.Item(i).Attributes("username").Value
                Else
                    Username = String.Empty
                End If
                If Not IsNothing(Users.Item(i).Attributes("password")) Then
                    Password = Users.Item(i).Attributes("password").Value
                Else
                    Password = String.Empty
                End If
                If Not IsNothing(Users.Item(i).Attributes("unique_id")) Then
                    UniqueId = Users.Item(i).Attributes("unique_id").Value
                Else
                    MyBase.SetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "unique_id", 1, _domainLock)
                    UniqueId = 1
                End If
                RVal(i) = New UserDetails(Username, Password, UniqueId)
            Next
            Return RVal
        End Function

        Public Function AuthorizeUser(ByVal DomainName As String, ByVal Username As String, ByVal Password As String) As Boolean
            Dim RVal As Boolean = False
            RVal = Not IsNothing(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """ and @password=""" & Password & """]", _domainLock))
            If UserExists(DomainName, Username) Then
                If RVal And UserExists(DomainName, Username) Then
                    'Mark the successful authentication attempt
                    MyBase.SetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastSuccessfulAuthentication", DateTime.Now.ToString(Majodio.Mail.Common.Constants.INTERNATIONAL_DATE_TIME_FORMAT), _domainLock)
                Else
                    'Mark the failed authentication attempt
                    MyBase.SetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastFailedAuthentication", DateTime.Now.ToString(Majodio.Mail.Common.Constants.INTERNATIONAL_DATE_TIME_FORMAT), _domainLock)
                End If
            End If
            Return RVal
        End Function

        Public Function GetNextUid(ByVal DomainName As String, ByVal Username As String) As UniqueId
            Dim TmpRval As String
            Dim RVal As UniqueId = New UniqueId(-1)
            If UserExists(DomainName, Username) Then
                TmpRval = MyBase.GetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "unique_id", _domainLock)
                If IsNothing(TmpRval) OrElse TmpRval.Trim.Length = 0 OrElse Not IsNumeric(TmpRval) Then
                    RVal = New UniqueId(1)
                Else
                    RVal = New UniqueId(Int32.Parse(TmpRval) + 1)
                End If
                MyBase.SetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "unique_id", RVal.Value, _domainLock)
            End If
            Return RVal
        End Function

        Public Property MessageLastReceived(ByVal DomainName As String, ByVal Username As String) As DateTime
            Get
                Dim RVal As DateTime = DateTime.MinValue
                If UserExists(DomainName, Username) Then
                    Try
                        RVal = DateTime.Parse(MyBase.GetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastMessageReceived", _domainLock))
                    Catch ex As FormatException
                    End Try
                End If
                Return RVal
            End Get
            Set(ByVal Value As DateTime)
                If UserExists(DomainName, Username) Then
                    MyBase.SetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastMessageReceived", Value.ToString(Majodio.Mail.Common.Constants.INTERNATIONAL_DATE_TIME_FORMAT), _domainLock)
                End If
            End Set
        End Property

        Public ReadOnly Property LastSuccessfulAuthentication(ByVal DomainName As String, ByVal Username As String) As DateTime
            Get
                Dim RVal As DateTime = DateTime.MinValue
                If UserExists(DomainName, Username) Then
                    Try
                        RVal = DateTime.Parse(MyBase.GetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastSuccessfulAuthentication", _domainLock))
                    Catch ex As FormatException
                    End Try
                End If
                Return RVal
            End Get
        End Property

        Public ReadOnly Property LastFailedAuthentication(ByVal DomainName As String, ByVal Username As String) As DateTime
            Get
                Dim RVal As DateTime = DateTime.MinValue
                If UserExists(DomainName, Username) Then
                    Try
                        RVal = DateTime.Parse(MyBase.GetXmlValue("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", "LastFailedAuthentication", _domainLock))
                    Catch ex As FormatException
                    End Try
                End If
                Return RVal
            End Get
        End Property
#End Region
#Region " User Subscription Management:"
        Public Function SubscriptionExists(ByVal DomainName As String, ByVal Username As String, ByVal Folder As String) As Boolean
            Dim RVal As Boolean = False
            If UserExists(DomainName, Username) Then
                RVal = Not IsNothing(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]/subscription[@folder=""" & Folder & """]", _domainLock))
            End If
            Return RVal
        End Function

        Public Sub AddSubscription(ByVal DomainName As String, ByVal Username As String, ByVal Folder As String)
            Dim NewElem As XmlElement
            Dim NewAttr As XmlAttribute
            Dim User As XmlElement
            If Folder.Trim.Length > 0 AndAlso UserExists(DomainName, Username) AndAlso Not SubscriptionExists(DomainName, Username, Folder) Then
                User = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]", _domainLock)
                NewElem = MyBase.CreateXmlElement(User, "subscription", _domainLock)
                NewAttr = XmlDoc.CreateAttribute("folder")
                NewAttr.Value = Folder.Replace("\", "/")
                NewElem.Attributes.Append(NewAttr)
                MyBase.SaveXmlFile(_domainLock)
            End If
        End Sub

        Public Sub RemoveSubscription(ByVal DomainName As String, ByVal Username As String, ByVal Folder As String)
            Dim Subscription As XmlElement
            If UserExists(DomainName, Username) AndAlso SubscriptionExists(DomainName, Username, Folder) Then
                Subscription = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]/subscription[@folder=""" & Folder & """]", _domainLock)
                MyBase.DeleteXmlElement(Subscription, _domainLock)
            End If
        End Sub

        Public Function GetSubscriptions(ByVal DomainName As String, ByVal Username As String, ByVal Pattern As String) As String()
            Dim TmpRVal As New ArrayList
            Dim RVal As String()
            Dim Subscriptions As String()
            Subscriptions = GetSubscriptions(DomainName, Username)
            If Not IsNothing(Subscriptions) Then
                For i As Integer = 0 To Subscriptions.GetUpperBound(0)
                    If Regex.IsMatch(Subscriptions(i), Pattern) Then
                        TmpRVal.Add(Subscriptions(i))
                    End If
                Next
            End If
            If Not IsNothing(TmpRVal) Then
                RVal = TmpRVal.ToArray(GetType(String))
            Else
                ReDim RVal(-1)
            End If
            Return RVal
        End Function

        Public Function GetSubscriptions(ByVal DomainName As String, ByVal Username As String) As String()
            Dim RVal As String() = Nothing
            Dim Subscriptions As XmlNodeList
            Subscriptions = MyBase.GetXmlElements("/domains/domain[@name=""" & DomainName.ToLower & """]/user[@username=""" & Username.ToLower & """]/subscription", _domainLock)
            If Not IsNothing(Subscriptions) Then
                ReDim RVal(Subscriptions.Count - 1)
                For i As Integer = 0 To Subscriptions.Count - 1
                    If Not IsNothing(Subscriptions(i).Attributes("folder")) Then
                        RVal(i) = Subscriptions(i).Attributes("folder").Value
                    Else
                        RVal(i) = String.Empty
                    End If
                Next
            End If
            Return RVal
        End Function
#End Region
#Region " Alias Management"
        Public Function AliasExists(ByVal DomainName As String, ByVal AliasUsername As String) As Boolean
            Dim RVal As Boolean
            Dim AliasNode As XmlElement
            AliasNode = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & AliasUsername.ToLower & """]", _domainLock)
            RVal = Not IsNothing(AliasNode)
            Return RVal
        End Function

        Public Function AlaisRealAddressExsists(ByVal DomainName As String, ByVal AliasUsername As String, ByVal RealAddress As String) As Boolean
            Dim RealAddressNode As XmlElement
            RealAddressNode = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & AliasUsername.ToLower & """]/realaddress[@value=""" & RealAddress.ToLower & """]", _domainLock)
            Return Not IsNothing(RealAddressNode)
        End Function

        Public Function GetAliases(ByVal DomainName As String) As AliasDetails()
            Dim Aliases As XmlNodeList = MyBase.GetXmlElements("/domains/domain[@name=""" & DomainName.ToLower & """]/alias", _domainLock)
            Dim i As Integer
            Dim RVal As AliasDetails() = Nothing
            Dim IsRegex As Boolean = False
            For i = 0 To Aliases.Count - 1
                ReDim Preserve RVal(i)
                If Not IsNothing(Aliases.Item(i).Attributes("isregex")) Then
                    IsRegex = CBool(Aliases.Item(i).Attributes("isregex").Value)
                End If
                RVal(i) = New AliasDetails(Aliases.Item(i).Attributes("username").Value, IsRegex)
                RVal(i).RealAddresses.Add(GetAliasRealAddresses(DomainName, RVal(i).Username, False))
            Next
            Return RVal
        End Function

        Public Sub AddAlias(ByVal DomainName As String, ByVal [Alias] As AliasDetails)
            Dim AliasNode As XmlElement
            Dim NvCol As NameValueCollection
            Dim i As Integer
            If DomainExists(DomainName) And Not AliasExists(DomainName, [Alias].Username) Then
                Dim Domain As XmlElement = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]", _domainLock)
                AliasNode = MyBase.CreateXmlElement(Domain, "alias", [Alias].GetNameValueCollection)
                For i = 0 To [Alias].RealAddresses.Count - 1
                    NvCol = New NameValueCollection
                    NvCol.Add("value", [Alias].RealAddresses(i).ToString(EmailStringFormat.Address).ToLower)
                    MyBase.CreateXmlElement(AliasNode, "realaddress", NvCol, _domainLock)
                Next
            End If
        End Sub

        Public Sub DeleteAliasRealAddress(ByVal DomainName As String, ByVal AliasUsername As String, ByVal AliasRealAddress As String)
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & AliasUsername.ToLower & """]/realaddress[@value=""" & AliasRealAddress.ToLower & """]", _domainLock), _domainLock)
        End Sub

        Public Sub AddAliasRealAddress(ByVal DomainName As String, ByVal AliasUsername As String, ByVal AliasRealAddress As String)
            Dim AliasNode As XmlElement
            Dim NvCol As New NameValueCollection
            If AliasExists(DomainName, AliasUsername) AndAlso Not AlaisRealAddressExsists(DomainName, AliasUsername, AliasRealAddress) Then
                NvCol.Add("value", AliasRealAddress.ToLower)
                AliasNode = MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & AliasUsername.ToLower & """]", _domainLock)
                If Not IsNothing(AliasNode) Then
                    MyBase.CreateXmlElement(AliasNode, "realaddress", NvCol, _domainLock)
                End If
            End If
        End Sub

        Public Sub DeleteAlias(ByVal DomainName As String, ByVal Username As String)
            MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & Username.ToLower & """]", _domainLock), _domainLock)
        End Sub

        Public Function GetAliasRealAddresses(ByVal DomainName As String, ByVal AliasUsername As String, ByVal recursive As Boolean) As EmailAddressCollection
            Dim RVal As EmailAddressCollection = New EmailAddressCollection
            Dim AliasRealAddresses As XmlNodeList = MyBase.GetXmlElements("/domains/domain[@name=""" & DomainName.ToLower & """]/alias[@username=""" & AliasUsername.ToLower & """]/realaddress", _domainLock)
            Dim emailAddress As EmailAddress
            Dim emailAddresses As EmailAddressCollection

            Dim i As Integer
            If Not IsNothing(AliasRealAddresses) AndAlso AliasRealAddresses.Count > 0 Then
                For i = 0 To AliasRealAddresses.Count - 1
                    emailAddress = New EmailAddress(AliasRealAddresses.Item(i).Attributes("value").Value)
                    If recursive AndAlso AliasExists(emailAddress.GetDomain, emailAddress.GetUsername) Then
                        'The alias points to another alias, we need to add all of the real addresses for that alias
                        'This is done recursively
                        emailAddresses = GetAliasRealAddresses(emailAddress.GetDomain, emailAddress.GetUsername, True)
                        RVal.Add(emailAddresses)
                    Else
                        'The alias points to a real email address so continue
                        RVal.Add(emailAddress)
                    End If
                Next
            End If
            Return RVal
        End Function

        Public Function GetAliasRegExMatch(ByVal DomainName As String, ByVal Username As String) As AliasDetails
            Dim RVal As AliasDetails = Nothing
            Dim Aliases As AliasDetails() = Me.GetAliases(DomainName)
            If Not IsNothing(Aliases) Then
                For i As Integer = 0 To Aliases.GetUpperBound(0)
                    If Aliases(i).IsRegex Then
                        If Regex.IsMatch(Username, Aliases(i).Username) Then
                            RVal = Aliases(i)
                            Exit For
                        End If
                    End If
                Next
            End If
            Return RVal
        End Function
#End Region
#Region " POP3 Client Management"
        Public Function GetNewPop3RelayElement() As XmlElement
            Dim rVal As XmlElement = Nothing
            Dim domain As XmlElement = Nothing

            rVal = MyBase.GetSingleXmlElement("/domains/pop3relay", _domainLock)
            If rVal Is Nothing Then
                domain = MyBase.GetSingleXmlElement("/domains")
                rVal = MyBase.CreateXmlElement(domain, "pop3relay", _domainLock)
            End If

            Return rVal
        End Function

        Public Sub AddPop3Relay(ByVal relay As Pop3RelayDetails)
            Dim pop3Relay As XmlElement
            If Pop3RelayExists(relay.ServerAddress, relay.Username) Then
                pop3Relay = MyBase.GetSingleXmlElement("/domains/pop3relay/relay[@serveraddress=""" & relay.ServerAddress.ToLower & """ and @username=""" & relay.Username.ToLower & """]", _domainLock)
                MyBase.DeleteXmlElement(pop3Relay)
            End If

            pop3Relay = GetNewPop3RelayElement()
            Dim relayNode As XmlElement
            relayNode = MyBase.CreateXmlElement(pop3Relay, "relay", relay.GetNameValueCollection)
            For i As Integer = 0 To relay.DeliveryAccounts.Count - 1
                MyBase.CreateXmlElement(relayNode, "deliveryaddress", "value", relay.DeliveryAccounts(i).ToString(EmailStringFormat.Address), _domainLock)
            Next
        End Sub

        Public Function Pop3RelayExists(ByVal serverAddress As String, ByVal username As String) As Boolean
            Dim rVal As Boolean

            rVal = Not IsNothing(MyBase.GetSingleXmlElement("/domains/pop3relay/relay[@serveraddress=""" & serverAddress.ToLower & """ and @username=""" & username.ToLower & """]", _domainLock))

            Return rVal
        End Function

        Public Function GetPop3Relay(ByVal serverAddress As String, ByVal username As String) As Pop3RelayDetails
            Dim rval As Pop3RelayDetails = Nothing
            Dim element As XmlElement
            Dim deliveryElements As XmlNodeList
            Dim password As String
            Dim intervalSeconds As Integer
            Dim lastProcessedTime As Long
            Dim deliveryAccounts As New EmailAddressCollection

            element = GetSingleXmlElement("/domains/pop3relay/relay[@serveraddress=""" & serverAddress.ToLower & """ and @username=""" & username.ToLower & """]", _domainLock)

            If Not IsNothing(element) Then
                password = element.Attributes("password").Value
                intervalSeconds = Convert.ToInt32(element.Attributes("intervalseconds").Value)
                lastProcessedTime = Convert.ToInt64(element.Attributes("lastprocessedtime").Value)
                deliveryElements = element.ChildNodes
                For i As Integer = 0 To deliveryElements.Count - 1
                    deliveryAccounts.Add(deliveryElements(i).Attributes("value").Value)
                Next
                rval = New Pop3RelayDetails(serverAddress, username, password, intervalSeconds, lastProcessedTime, deliveryAccounts)
            End If

            Return rval
        End Function

        Public Function GetPop3Relays() As Pop3RelayDetails()
            Dim rVal As Pop3RelayDetails() = Nothing
            Dim tmpRval As New ArrayList
            Dim serverAddress As String
            Dim username As String
            Dim password As String
            Dim intervalSeconds As Integer
            Dim lastProcessedTime As Long
            Dim deliveryAccounts As EmailAddressCollection
            Dim mainElements As XmlNodeList
            Dim deliveryElements As XmlNodeList

            mainElements = MyBase.GetXmlElements("/domains/pop3relay/relay", _domainLock)
            If Not IsNothing(mainElements) Then
                For i As Integer = 0 To mainElements.Count - 1
                    serverAddress = mainElements(i).Attributes("serveraddress").Value
                    username = mainElements(i).Attributes("username").Value
                    password = mainElements(i).Attributes("password").Value
                    intervalSeconds = Convert.ToInt32(mainElements(i).Attributes("intervalseconds").Value)
                    lastProcessedTime = Convert.ToInt64(mainElements(i).Attributes("lastprocessedtime").Value)
                    deliveryElements = mainElements(i).ChildNodes
                    deliveryAccounts = New EmailAddressCollection
                    If Not IsNothing(deliveryElements) AndAlso deliveryElements.Count > 0 Then
                        For j As Integer = 0 To deliveryElements.Count - 1
                            deliveryAccounts.Add(deliveryElements(j).Attributes("value").Value)
                        Next
                    End If
                    tmpRval.Add(New Pop3RelayDetails(serverAddress, username, password, intervalSeconds, lastProcessedTime, deliveryAccounts))
                Next
            End If

            rVal = tmpRval.ToArray(GetType(Pop3RelayDetails))

            Return rVal
        End Function

        Public Sub DeletePop3Relay(ByVal serverAddress As String, ByVal username As String)
            If Pop3RelayExists(serverAddress, username) Then
                MyBase.DeleteXmlElement(MyBase.GetSingleXmlElement("/domains/pop3relay/relay[@serveraddress=""" & serverAddress & """ and @username=""" & username & """]", _domainLock), _domainLock)
            End If
        End Sub

        Public Sub UpdateLastProcessedTime(ByVal serverAddress As String, ByVal username As String)
            If Pop3RelayExists(serverAddress, username) Then
                MyBase.SetXmlValue("/domains/pop3relay/relay[@serveraddress=""" & serverAddress.ToLower & """ and @username=""" & username.ToLower & """]", "lastprocessedtime", DateTime.Now.Ticks, _domainLock)
            End If
        End Sub
#End Region
    End Class
End Namespace