Imports System.ComponentModel
Imports System.web
Imports System.Web.UI
Imports System.Web.UI.WebControls


Namespace Majodio.Web.UI.WebControls
    <DefaultProperty("Text"), _
     ToolboxData("<{0}:PayPalBuyNowButton runat=""server"" id=""PayPalBuyNowButton1""></{0}:PayPalBuyNowButton>"), _
     Designer(GetType(Majodio.Web.UI.Design.PalPalBuyNowButtonDesigner))> _
    Public Class PayPalBuyNowButton
        Inherits System.Web.UI.WebControls.WebControl

        Protected Overrides Sub CreateChildControls()
            Dim HL As New HyperLink()
            HL = GetHyperLink()
            Controls.Add(HL)
        End Sub

        Public Enum UrlBehavior
            NoData = 0
            GetMethod = 1
            PostMethod = 2
        End Enum

        Public Enum BackgroundColor
            Black = 1
            White = 0
        End Enum

        Public Enum CurrencyCodes
            USD
            EUR
            GBP
            CAD
            JPY
        End Enum

        ''** Custom fields
        'Private _LinkText As String = "Buy Now"
        'Private _ButtonImageUrl As String = String.Empty

        ''** PayPal fields
        'Private _Business As String = String.Empty
        'Private _ItemName As String = String.Empty
        'Private _ItemNumber As Integer = -1
        'Private _Amount As Single = 0
        'Private _ImageUrl As String = String.Empty
        'Private _NoShipping As Boolean = False
        'Private _Return As String = String.Empty
        'Private _Rm As UrlBehavior = UrlBehavior.NoData
        'Private _CancelReturn As String = String.Empty
        'Private _NoNote As Boolean = False
        'Private _Cn As String = String.Empty
        'Private _Cs As BackgroundColor = BackgroundColor.White
        'Private _On0 As String = String.Empty
        'Private _Os0 As String = String.Empty
        'Private _On1 As String = String.Empty
        'Private _Os1 As String = String.Empty
        'Private _Quantity As Integer = 0
        'Private _UndefinedQuantity As Boolean = False
        'Private _Shipping As Single = 0
        'Private _Shipping2 As Single = 0
        'Private _Handling As Single = 0
        'Private _Custom As String = String.Empty
        'Private _Invoice As String = String.Empty
        'Private _Tax As Single = 0
        'Private _CurrencyCode As CurrencyCodes = CurrencyCodes.USD
        'Private _Lc As String = String.Empty

        Private Function GetHyperLink() As System.Web.UI.WebControls.HyperLink
            Dim HL As New System.Web.UI.WebControls.HyperLink()

            If ButtonImageUrl <> String.Empty Then
                HL.ImageUrl = ButtonImageUrl
            ElseIf LinkText <> String.Empty Then
                HL.Text = LinkText
            Else
                'Throw New PropertyRequiredException("ButtonImageUrl or LinkText")
                HL.Text = "Buy Now"
            End If
            HL.NavigateUrl = GetLink()

            Return HL
        End Function

        Private Function URLEncode(ByVal str As String) As String
            Return System.Web.HttpContext.Current.Server.UrlEncode(str)
        End Function

        Private Function GetLink() As String
            Dim Link As String
            If Not HttpContext.Current.Request.IsLocal AndAlso Business = String.Empty Then
                Throw New PropertyRequiredException("Business")
            ElseIf HttpContext.Current.Request.IsLocal AndAlso TestBusiness = String.Empty Then
                Throw New PropertyRequiredException("TestBusiness")
            End If
            If HttpContext.Current.Request.IsLocal Then
                Link = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" & URLEncode(TestBusiness)
            Else
                Link = "https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" & URLEncode(Business)
            End If

            If ItemName <> String.Empty Then
                Link &= "&item_name=" & URLEncode(ItemName)
            End If
            If ItemNumber > 0 Then
                Link &= "&item_number=" & URLEncode(ItemNumber)
            End If
            If Amount <> 0 Then
                Link &= "&amount=" & URLEncode(Amount)
            End If
            If ImageUrl <> String.Empty Then
                Link &= "&image_url=" & URLEncode(ImageUrl)
            End If
            If NoShipping Then
                Link &= "&no_shipping=1"
            End If
            If ReturnUrl <> String.Empty Then
                Link &= "&return=" & URLEncode(ReturnUrl)
            End If
            Select Case ReturnUrlBehavior
                Case UrlBehavior.GetMethod
                    Link &= "&rm=1"
                Case UrlBehavior.PostMethod
                    Link &= "&rm=2"
            End Select
            If CancelReturnUrl <> String.Empty Then
                Link &= "&cancel_return=" & URLEncode(CancelReturnUrl)
            End If
            If NoNote Then
                Link &= "&no_note=1"
            End If
            If NoteLabel <> String.Empty Then
                Link &= "&cn=" & URLEncode(NoteLabel)
            End If
            If ScreenBackground = BackgroundColor.Black Then
                Link &= "&cs=1"
            End If
            If CustomName1 <> String.Empty And CustomValue1 <> String.Empty Then
                Link &= "&on0=" & URLEncode(CustomName1) & "&os0=" & URLEncode(CustomValue1)
            End If
            If CustomName2 <> String.Empty And CustomValue2 <> String.Empty Then
                Link &= "&on1=" & URLEncode(CustomName2) & "&os1=" & URLEncode(CustomValue2)
            End If
            If Quantity > 0 Then
                Link &= "&quantity=" & URLEncode(Quantity)
            End If
            If AllowEditQuantity Then
                Link &= "&undefined_quantity=1"
            End If
            If InitialShippingCost > 0 Then
                Link &= "&shipping=" & URLEncode(InitialShippingCost)
            End If
            If AdditionalShippingCost > 0 Then
                Link &= "&shipping2=" & URLEncode(AdditionalShippingCost)
            End If
            If HandlingCost > 0 Then
                Link &= "&handling=" & URLEncode(HandlingCost)
            End If
            If Custom <> String.Empty Then
                Link &= "&custom=" & URLEncode(Custom)
            End If
            If Invoice <> String.Empty Then
                Link &= "&invoice=" & URLEncode(Invoice)
            End If
            If Tax > 0 Then
                Link &= "&tax=" & URLEncode(Tax)
            End If
            Select Case CurrencyCode
                Case CurrencyCodes.CAD
                    Link &= "&currency_code=CAD"
                Case CurrencyCodes.EUR
                    Link &= "&currency_code=EUR"
                Case CurrencyCodes.GBP
                    Link &= "&currency_code=GBP"
                Case CurrencyCodes.JPY
                    Link &= "&currency_code=JPY"
            End Select
            If Location <> String.Empty Then
                Link &= "&lc=" & URLEncode(Location)
            End If
            Return Link
        End Function

        <Bindable(True), _
         Category("Behavior")> _
        Public Property LinkText() As String
            Get
                If IsNothing(ViewState("LinkText")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("LinkText"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("LinkText") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ButtonImageUrl() As String
            Get
                If IsNothing(ViewState("ButtonImageUrl")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("ButtonImageUrl"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ButtonImageUrl") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Business() As String
            Get
                If IsNothing(ViewState("Business")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("Business"), String)
                End If
            End Get
            Set(ByVal Value As String)
                If InStr(Value, "@") > -1 And InStr(Value, ".") > InStr(Value, "@") Then
                    ViewState("Business") = Value
                Else
                    Throw New ArgumentException("The value for Business needs to be an e-mail address")
                End If
            End Set
        End Property

        Public Property TestBusiness() As String
            Get
                If IsNothing(ViewState("TestBusiness")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("TestBusiness"), String)
                End If
            End Get
            Set(ByVal value As String)
                If InStr(value, "@") > -1 And InStr(value, ".") > InStr(value, "@") Then
                    ViewState("TestBusiness") = value
                Else
                    Throw New ArgumentException("The value for Business needs to be an e-mail address")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ItemName() As String
            Get
                If IsNothing(ViewState("ItemName")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("ItemName"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("ItemName") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ItemNumber() As Integer
            Get
                If IsNothing(ViewState("ItemNumber")) Then
                    Return -1
                Else
                    Return CType(ViewState("ItemNumber"), Integer)
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("ItemNumber") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Amount() As Single
            Get
                If IsNothing(ViewState("Amount")) Then
                    Return 0
                Else
                    Return CType(ViewState("Amount"), Single)
                End If
            End Get
            Set(ByVal Value As Single)
                If Value >= 0 Then
                    ViewState("Amount") = Value
                Else
                    Throw New ArgumentException("The value for Amount is required to be a positive number")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ImageUrl() As String
            Get
                If IsNothing(ViewState("ImageUrl")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("ImageUrl"), String)
                End If
            End Get
            Set(ByVal Value As String)
                If Value.ToLower.IndexOf("http://") > -1 Or Value.ToLower.IndexOf("https://") > -1 Then
                    ViewState("ImageUrl") = Value
                Else
                    Throw New ArgumentException("The value for ImageUrl is required to contain http:// or https://")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property NoShipping() As Boolean
            Get
                If IsNothing(ViewState("NoShipping")) Then
                    Return False
                Else
                    Return CType(ViewState("NoShipping"), Boolean)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("NoShipping") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ReturnUrl() As String
            Get
                If IsNothing(ViewState("ReturnUrl")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("ReturnUrl"), String)
                End If
            End Get
            Set(ByVal Value As String)
                If Value.ToLower.IndexOf("http://") > -1 Or Value.ToLower.IndexOf("https://") > -1 Then
                    ViewState("ReturnUrl") = Value
                Else
                    Throw New ArgumentException("The value for ReturnUrl is required to contain http:// or https://")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ReturnUrlBehavior() As UrlBehavior
            Get
                If IsNothing(ViewState("ReturnUrlBehavior")) Then
                    Return UrlBehavior.NoData
                Else
                    Return CType(ViewState("ReturnUrlBehavior"), UrlBehavior)
                End If
            End Get
            Set(ByVal Value As UrlBehavior)
                ViewState("ReturnUrlBehavior") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CancelReturnUrl() As String
            Get
                If IsNothing(ViewState("CancelReturnUrl")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("CancelReturnUrl"), String)
                End If
            End Get
            Set(ByVal Value As String)
                If Value.ToLower.IndexOf("http://") > -1 Or Value.ToLower.IndexOf("https://") > -1 Then
                    ViewState("CancelReturnUrl") = Value
                Else
                    Throw New ArgumentException("The value for CancelReturnUrl is required to contain http:// or https://")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property NoNote() As Boolean
            Get
                If IsNothing(ViewState("NoNote")) Then
                    Return False
                Else
                    Return CType(ViewState("NoNote"), Boolean)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("NoNote") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property NoteLabel() As String
            Get
                If IsNothing(ViewState("NoteLabel")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("NoteLabel"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("NoteLabel") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property ScreenBackground() As BackgroundColor
            Get
                If IsNothing(ViewState("ScreenBackground")) Then
                    Return BackgroundColor.White
                Else
                    Return CType(ViewState("ScreenBackground"), BackgroundColor)
                End If
            End Get
            Set(ByVal Value As BackgroundColor)
                ViewState("ScreenBackground") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CustomName1() As String
            Get
                If IsNothing(ViewState("CustomName1")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("CustomName1"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("CustomName1") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CustomValue1() As String
            Get
                If IsNothing(ViewState("CustomValue1")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("CustomValue1"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("CustomValue1") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CustomName2() As String
            Get
                If IsNothing(ViewState("CustomName2")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("CustomName2"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("CustomName2") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CustomValue2() As String
            Get
                If IsNothing(ViewState("CustomValue2")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("CustomValue2"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("CustomValue2") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Quantity() As Integer
            Get
                If IsNothing(ViewState("Quantity")) Then
                    Return 0
                Else
                    Return CType(ViewState("Quantity"), Integer)
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("Quantity") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property AllowEditQuantity() As Boolean
            Get
                If IsNothing(ViewState("AllowEditQuantity")) Then
                    Return False
                Else
                    Return CType(ViewState("AllowEditQuantity"), Boolean)
                End If
            End Get
            Set(ByVal Value As Boolean)
                ViewState("AllowEditQuantity") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property InitialShippingCost() As Single
            Get
                If IsNothing(ViewState("InitialShippingCost")) Then
                    Return 0
                Else
                    Return CType(ViewState("InitialShippingCost"), Single)
                End If
            End Get
            Set(ByVal Value As Single)
                If Value > 0 Then
                    ViewState("InitialShippingCost") = Value
                Else
                    Throw New ArgumentException("The value for InitialShippingCost is required to be a positive number")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property AdditionalShippingCost() As Single
            Get
                If IsNothing(ViewState("AdditionalShippingCost")) Then
                    Return 0
                Else
                    Return CType(ViewState("AdditionalShippingCost"), Single)
                End If
            End Get
            Set(ByVal Value As Single)
                If Value > 0 Then
                    ViewState("AdditionalShippingCost") = Value
                Else
                    Throw New ArgumentException("The value for AdditionalShippingCost is required to be a positive number")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property HandlingCost() As Single
            Get
                If IsNothing(ViewState("HandlingCost")) Then
                    Return 0
                Else
                    Return CType(ViewState("HandlingCost"), Single)
                End If
            End Get
            Set(ByVal Value As Single)
                If Value > 0 Then
                    ViewState("HandlingCost") = Value
                Else
                    Throw New ArgumentException("The value for HandlingCost is required to be a positive number")
                End If
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Custom() As String
            Get
                If IsNothing(ViewState("Custom")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("Custom"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("Custom") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Invoice() As String
            Get
                If IsNothing(ViewState("Invoice")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("Invoice"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("Invoice") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Tax() As Single
            Get
                If IsNothing(ViewState("Tax")) Then
                    Return 0
                Else
                    Return CType(ViewState("Tax"), Single)
                End If
            End Get
            Set(ByVal Value As Single)
                ViewState("Tax") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property CurrencyCode() As CurrencyCodes
            Get
                If IsNothing(ViewState("CurrencyCode")) Then
                    Return 0
                Else
                    Return CType(ViewState("CurrencyCode"), CurrencyCodes)
                End If
            End Get
            Set(ByVal Value As CurrencyCodes)
                ViewState("CurrencyCode") = Value
            End Set
        End Property

        <Bindable(True), _
         Category("Behavior")> _
        Public Property Location() As String
            Get
                If IsNothing(ViewState("Location")) Then
                    Return String.Empty
                Else
                    Return CType(ViewState("Location"), String)
                End If
            End Get
            Set(ByVal Value As String)
                ViewState("Location") = Value
            End Set
        End Property
    End Class

    Public Class ValueRequiredException
        Inherits System.ApplicationException

        Public Sub New(ByVal Variable As String)
            MyBase.New("The variable " & Variable & " is required, but was not set.")
        End Sub
    End Class

    Public Class PropertyRequiredException
        Inherits System.ApplicationException

        Public Sub New(ByVal PropertyName As String)
            MyBase.New("The property " & PropertyName & " is required, but was not set.")
        End Sub
    End Class
End Namespace

Namespace Majodio.Web.UI.Design
    Public Class PalPalBuyNowButtonDesigner
        Inherits System.Web.UI.Design.ControlDesigner
        Public Overrides Function GetDesignTimeHtml() As String
            Return "Buy Now Button"
        End Function

    End Class
End Namespace