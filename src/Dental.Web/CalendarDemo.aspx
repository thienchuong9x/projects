<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="CalendarDemo.aspx.cs" Inherits="CalendarDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>            

</asp:Content>

