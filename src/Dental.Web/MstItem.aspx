<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstItem.aspx.cs" Inherits="MstItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:UpdatePanel ID="panelUpdate" runat="server">
   <ContentTemplate >

<asp:Panel ID="PanelSow" runat="server">
    <asp:Label ID="lblTitle" runat="server" Text="Label" Visible="False"></asp:Label>

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
                        <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" />
                        <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" />
                    </td>
                </tr>


            </table>
            <br />
    <asp:GridView ID="GridViewItem" runat="server" AutoGenerateColumns="False" 
        ShowHeader="False" 
        CssClass="gridView" AllowPaging="True" 
        DataKeyNames="ItemNo" onrowcreated="GridViewItem_RowCreated" 
        onpageindexchanging="GridViewItem_PageIndexChanging">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" BorderStyle="None" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ItemCathegory" HeaderText="ItemCathegory">
                <ItemStyle Width="150px"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" >
                <ItemStyle Width="50px"></ItemStyle >
            </asp:BoundField>
            <asp:BoundField DataField="ItemNm" HeaderText="ItemNm" >
                <ItemStyle Width="150px" ></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ItemValue" HeaderText="ItemValue">
                <ItemStyle Width="80px" ></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ViewOrder" HeaderText="ViewOrder" >
                <ItemStyle Width="80px" ></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="IsDeleted">
              <ItemStyle VerticalAlign=Middle HorizontalAlign=Center ></ItemStyle>
            </asp:BoundField>
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
      <PagerStyle CssClass="pagination" BackColor="White" />
      <RowStyle  CssClass="tr_body" />
     </asp:GridView>
    
    <asp:Label ID="lblNoRecord" runat="server" Text="Label"></asp:Label>
   
    <p style="margin-top:15px ; text-align:left">
        
        <asp:Button ID="btnReg" runat="server" Text="Register" onclick="btnReg_Click" />
        <asp:Button ID="btnEdit" runat="server" Text="Edit" onclick="btnEdit_Click" />
        
    </p>
</asp:Panel>
<asp:Panel ID="PanelEdit" runat="server" Visible="False">
    
    <table class="auto">
        <tr>
            <td class="auto">
                <asp:Label ID="lblItemCathegory" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtItemCathegory" runat="server" Width="150px" MaxLength="20" 
                    onkeyup="CallServerMethod()"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorICa" runat="server" Display=Dynamic 
                    ControlToValidate="txtItemCathegory" ForeColor="Red"></asp:RequiredFieldValidator >
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblItemNo" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtItemNo" runat="server" Width="80px" MaxLength="10" 
                    onkeyup="ValidateText(this)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorIno" runat="server"  Display=Dynamic
                    ControlToValidate="txtItemNo" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorICa"  Display=Dynamic 
                    runat="server" ControlToValidate="txtItemNo" ForeColor="Red" 
                    ValidationExpression="\d+"></asp:RegularExpressionValidator>
            </td>
            <td>
                <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblItemNm" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtItemNm" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblItemValue" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtItemValue" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblViewOrder" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtViewOrder" runat="server" Width="80px" MaxLength="10" 
                    onkeyup="ValidateText(this)"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorIvo" Display=Dynamic 
                    runat="server" ControlToValidate="txtViewOrder" ForeColor="Red" 
                    ValidationExpression="\d+"></asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <p style="margin-top:15px;text-align:left">
        
        <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" 
            onclick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
            CausesValidation=false onclick="btnCancel_Click" />
        
    </p>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

