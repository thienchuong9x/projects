<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Register.aspx.cs" Inherits="Dental.Account.Register" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel runat="server" ID="updatePanel">
  
      <ContentTemplate>
      <asp:Panel runat = "server" ID="panel" DefaultButton="btnRegister">
          <table>
              <tr>
                  <td>
                      <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="ReqValidatorUserName" runat="server" 
                          ControlToValidate="txtUserName" Display="Dynamic" ForeColor="Red" 
                          ValidationGroup="checkRegister"></asp:RequiredFieldValidator>
                      <asp:RegularExpressionValidator ID="RegValidatorUserLenght" runat="server" 
                          ControlToValidate="txtUserName" Display="Dynamic" ForeColor="Red" 
                          ValidationExpression="^[\s\S]{1,20}$" ValidationGroup="checkRegister"></asp:RegularExpressionValidator>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Label ID="lblPassWord" runat="server" Text="Label"></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtPassWord" runat="server" CssClass="textbox" 
                          TextMode="Password" Width="300px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="ReqValidatorPassWord" runat="server" 
                          ControlToValidate="txtPassWord" Display="Dynamic" ForeColor="Red" 
                          ValidationGroup="checkRegister"></asp:RequiredFieldValidator>
                      <asp:RegularExpressionValidator ID="RegValidatorPassWordLength" runat="server" 
                          ControlToValidate="txtPassWord" Display="Dynamic" ForeColor="Red" 
                          ValidationExpression="^[\s\S]{7,}$" ValidationGroup="checkRegister"></asp:RegularExpressionValidator>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Label ID="lblConfirmPassWord" runat="server" Text="Label"></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtConfirmPassWord" runat="server" CssClass="textbox" 
                          TextMode="Password" Width="300px"></asp:TextBox>
                      <asp:CompareValidator ID="CompValidatorPassWord" runat="server" 
                          ControlToCompare="txtPassWord" ControlToValidate="txtConfirmPassWord" 
                          Display="Dynamic" ForeColor="Red" ValidationGroup="checkRegister"></asp:CompareValidator>
                      <asp:RequiredFieldValidator ID="ReqValidatorPassWordConfirm" runat="server" 
                          ControlToValidate="txtConfirmPassWord" Display="Dynamic" ForeColor="Red" 
                          ValidationGroup="checkRegister"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                  <td>
                      <asp:Label ID="lblEmail" runat="server" Text="Label"></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" Width="300px"></asp:TextBox>
                      <asp:RegularExpressionValidator ID="RegValidatorEmail" runat="server" 
                          ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red" 
                          ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                          ValidationGroup="checkRegister"></asp:RegularExpressionValidator>
                      <asp:RequiredFieldValidator ID="ReqValidatorEmail" runat="server" 
                          ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red" 
                          ValidationGroup="checkRegister"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                  <td>&nbsp;</td>
                  <td>
                      <asp:Button ID="btnRegister" runat="server" CssClass="btn" 
                          onclick="btnRegister_Click" Text="Button" ValidationGroup="checkRegister" />
                  </td>
              </tr>
              <tr>
                  <td>&nbsp;</td>
                  <td>&nbsp;</td>
              </tr>
          </table>
          </asp:Panel>
      </ContentTemplate>
    
  </asp:UpdatePanel>
</asp:Content>
