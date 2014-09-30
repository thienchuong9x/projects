<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCLogin.ascx.cs" Inherits="UCLogin" %>
<asp:Panel ID="panelPopUp" runat="server" DefaultButton="btnPopUpLogin">
        
           <div id="toPopup" style="display:block">
          	
        <div class="close"></div>
       	<span class="ecs_tooltip">Press Esc to close <span class="arrow"></span></span>
		<div id="popup_content"> <!--your content start-->
            
            <%--<p align="center"><a href="#" class="livebox">Click Here Trigger</a></p>--%>
            <table>
                <tr>
                    <td colspan="2" style="text-align:left">
                       <h1><asp:Label ID="lblTiltePopUpLogin" runat="server"></asp:Label></h1>
                       <hr />
                    </td>
                </tr>
                <tr>
                    <td class="auto">
                        <asp:Label ID="lblPopUpUserName" runat="server" Text="UserName"></asp:Label>:</td>
                    <td>
                        <asp:TextBox ID="txtPopUpUserName" runat="server" Width="200px" CssClass="textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto">
                        <asp:Label ID="lblPopUpPassWord" runat="server"></asp:Label>:</td>
                    <td>
                        <asp:TextBox ID="txtPopUpPassWord" runat="server" Width="200px" 
                            CssClass="textbox" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto">
                        &nbsp;</td>
                    <td>
                      <asp:CheckBox ID="cbRememberMe" runat="server" />
                    </td>
                </tr>
                <tr>
                 <td>
                      &nbsp;</td>
                    <td class="auto">
                        <asp:Button ID="btnPopUpLogin" runat="server" CssClass="btn" 
                            onclick="btnLogin_Click" />
                    </td>
                   
                </tr>
            </table>
            
        </div> <!--your content end-->
    
    </div> <!--toPopup end-->
        
    </asp:Panel>

