<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstProcess.aspx.cs" Inherits="MstProcess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="updatePanelMstProcess" runat="server">
    <ContentTemplate>
        <asp:Panel ID="panelHeader" runat="server">
            <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input" 
                Visible="False"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCode" runat="server"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="200px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                    
                        &nbsp;</td>
                    <td>
                        <asp:CheckBox ID="cbIsDeleted" runat="server" AutoPostBack="True" 
                            oncheckedchanged="cbIsDeleted_CheckedChanged" />

                    </td>
                </tr>


                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" onclick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" onclick="btClear_Click" />
                    </td>
                </tr>


            </table>

            <asp:GridView runat="server" ID="gridViewProcess" AutoGenerateColumns="False" style="margin-top:10px"
                DataKeyNames="ProcessCd" ShowHeader="False" OnRowCreated="gridViewProcess_RowCreated"
                CssClass="gridView" AllowPaging="True" 
                onpageindexchanging="gridViewProcess_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProcessCd">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="IsDeleted">
                    <ItemStyle VerticalAlign=Middle HorizontalAlign=Center/>
                   </asp:BoundField> 
                </Columns>
                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
              <PagerStyle CssClass="pagination" BackColor="White" />
              <RowStyle  CssClass="tr_body" />
                    </asp:GridView>
            <asp:Label ID="lblNoRecord" runat="server" Text="Label"></asp:Label>
           
            <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
           
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btnRegister" CssClass="MasterManagementButton" runat="server" 
                    Text="Register" onclick="btnRegister_Click" />
                <asp:Button ID="btnEdit"     CssClass="MasterManagementButton" runat="server" 
                    Text="Edit" onclick="btnEdit_Click" />
            </p>
        </asp:Panel>
        <asp:Panel ID="panelDetail" runat="server" Visible="false">
            <asp:Panel ID="PanelMasterManagement" runat="server">
                <table class="auto">
                    <tr>
                        <td class="auto">
                            <asp:Label ID="LabelProcessCd" runat="server"></asp:Label>
                        </td>
                        <td class="auto">
                            <asp:TextBox ID="txtProcessCd" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                        </td>
                        <td class="auto">
                            <asp:RequiredFieldValidator ID="valRequiredInput_ProcessCd" runat="server" ControlToValidate="txtProcessCd"
                                ForeColor="Red" Display="Dynamic" ValidationGroup = "ValidationProcess" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="valRequiredNumber_ProcessCd"
                                ControlToValidate="txtProcessCd" ForeColor="Red" Display="Dynamic" ValidationExpression="^\d+$" ValidationGroup = "ValidationProcess">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="LabelProcessNm" runat="server"></asp:Label>
                        </td>
                        <td class="auto">
                            <asp:TextBox ID="TextProcessNm" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="30" Width="200px"></asp:TextBox>
                        </td>
                        <td class="auto">
                            <asp:RequiredFieldValidator ID="valRequired_ProcessNm" runat="server" ControlToValidate="TextProcessNm"
                                ForeColor="Red" ValidationGroup = "ValidationProcess">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="ButtonSave" CssClass="MasterManagementButton" runat="server" 
                    Text="OK" ValidationGroup = "ValidationProcess" 
                    onclick="ButtonSave_Click" />
                <asp:Button ID="ButtonDelete" CssClass="MasterManagementButton" runat="server" 
                    Text="Delete" CausesValidation="false" onclick="ButtonDelete_Click" />
                <asp:Button ID="ButtonCancel" CssClass="MasterManagementButton" runat="server" 
                    Text="Cancel" CausesValidation="false" onclick="ButtonCancel_Click" />
            </p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

