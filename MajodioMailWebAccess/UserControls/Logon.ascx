<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Logon.ascx.vb" Inherits="UserControls_Logon" %>
<asp:Panel ID="pnlLogon" runat="server">
<table>
    <tr>
        <td style="width: 100px">
            Username:</td>
        <td style="width: 100px">
            <asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="width: 100px">
            Password:</td>
        <td style="width: 100px">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:LinkButton ID="btnLogon" runat="server">Logon</asp:LinkButton></td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
    </tr>
</table>
</asp:Panel>