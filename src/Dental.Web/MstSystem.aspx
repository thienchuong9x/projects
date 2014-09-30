<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstSystem.aspx.cs" Inherits="MstSystem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:UpdatePanel ID="updatePanelMstSystem" runat="server">
    <ContentTemplate>
        <asp:Panel ID="panelGrid" runat="server">
            <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input"></asp:Label>
            <asp:GridView ID="gridSystem" runat="server" ShowHeader="false" DataKeyNames="Parameter"
                CssClass="gridView" EnableModelValidation="True" style="margin-top:10px"
                AutoGenerateColumns="False" onrowcreated="gridSystem_RowCreated" 
                AllowPaging="True" onpageindexchanging="gridSystem_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Parameter">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Value">
                        <ItemStyle Width="300px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <PagerStyle CssClass="pagination" BackColor="White" />
                <RowStyle  CssClass="tr_body" />
            </asp:GridView>
            <div style="margin-top: 15px">
                <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click" />
                <asp:Button ID="btEdit" runat="server" onclick="btEdit_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="panelInput" runat="server" Visible="false">
            <table>
                <tbody>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblParameter" runat="server" CssClass="lable_input"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxParameter" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="valRequired_Parameter" runat="server" ControlToValidate="tbxParameter"
                                ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValue" runat="server" CssClass="lable_input"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxValue" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="valRequired_Value" runat="server" ControlToValidate="tbxValue"
                                ForeColor="Red">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </tbody>
            </table>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btSave" runat="server" onclick="btSave_Click" />
                <asp:Button ID="btDelete" runat="server" Text="Delete" 
                    CausesValidation="false" onclick="btDelete_Click" />
                <asp:Button ID="btCancel" runat="server" CausesValidation="false" 
                    onclick="btCancel_Click" />
            </p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

