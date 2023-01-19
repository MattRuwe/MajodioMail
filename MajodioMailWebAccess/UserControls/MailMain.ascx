<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MailMain.ascx.vb" Inherits="UserControls_MailMain" %>
<asp:Label id="lblLastUpdate" runat="server" Text="" /><br />
<asp:Label ID="lblMessageCount" runat="server" Text="Label"><br /></asp:Label><asp:LinkButton
    ID="lbRefresh" runat="server">Refresh</asp:LinkButton>
<asp:GridView ID="gvMessages" runat="server">
</asp:GridView>
