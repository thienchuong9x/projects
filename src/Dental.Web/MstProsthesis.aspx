<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstProsthesis.aspx.cs" Inherits="MstProsthesis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <script type="text/javascript">
        function GetDropDownValue() {
            var IndexValue = document.getElementById('<%=selTechPrice.ClientID %>').selectedIndex;

            var SelectedVal = document.getElementById('<%=selTechPrice.ClientID %>').options[IndexValue].value;

            document.getElementById('<%= txtTechPriceSearch.ClientID %>').value = SelectedVal;
        }
</script>

<script type="text/javascript">
    function GetDropDownValue1() {
        var IndexValue = document.getElementById('<%=selTechPriceEdit.ClientID %>').selectedIndex;

        var SelectedVal = document.getElementById('<%=selTechPriceEdit.ClientID %>').options[IndexValue].value;

        document.getElementById('<%= txtTechCd.ClientID %>').value = SelectedVal;
    }
</script>

<script type="text/javascript">
//    $(document).ready(function () {
//        $('input[type="checkbox"]').dnnCheckbox();
//    });

    function ValidateTextNumber(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(/[^\d]+/g, '');
        }
    }    
</script>
<asp:UpdatePanel ID="updatePanelProsthesis" runat="server">
<ContentTemplate>

        <asp:Panel ID="panelGrid" runat="server">
            <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input"></asp:Label>
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
                        <asp:TextBox ID="txtName" runat="server" MaxLength="40" Width="300px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProcessSearch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProcessSearch" runat="server" AutoPostBack="True" 
                            Width="80px" MaxLength="10" ontextchanged="txtProcessSearch_TextChanged"></asp:TextBox>
                        <asp:DropDownList ID="selProcess" runat="server" AutoPostBack="True" 
                            DataTextField="ProcessNm" DataValueField="ProcessCd" Width="155px" 
                            onselectedindexchanged="selProcess_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTechPriceSearch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTechPriceSearch" runat="server" AutoPostBack="True" 
                            Width="80px" MaxLength="10" ontextchanged="txtTechPriceSearch_TextChanged"></asp:TextBox>
                        <asp:DropDownList ID="selTechPrice" runat="server" 
                            DataTextField="TechNm" DataValueField="TechCd" Width="155px" 
                            OnChange="javascript:GetDropDownValue();">
                        </asp:DropDownList>
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

            <asp:GridView ID="gridProsthesis" runat="server" ShowHeader="False" 
                style="margin-top:10px" DataKeyNames="ProsthesisCd" CssClass="gridView"
                AutoGenerateColumns="False" 
                AllowPaging="True" onrowcreated="gridProsthesis_RowCreated" 
                onpageindexchanging="gridProsthesis_PageIndexChanging" >
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProsthesisCd" HeaderText="Prosthesis Code">
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>                   
                    <asp:BoundField DataField="ProsthesisAbbNm" HeaderText="Prosthesis Abb Name">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProsthesisNm" HeaderText="Prosthesis Name">
                        <ItemStyle Width="300px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProsthesisType" HeaderText="Prosthesis Type">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StumpFlg">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign=Middle></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MinimumProcess" HeaderText="Minimum Process">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Process">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TechPrice" >
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
      <PagerStyle CssClass="pagination" BackColor="White" />
      <RowStyle  CssClass="tr_body" />
            </asp:GridView>
            <asp:Label ID="lblNoRecordProsthesis" runat="server" Visible="False"></asp:Label>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btRegister" runat="server" onclick="btRegister_Click" />
                <asp:Button ID="btEdit" runat="server" onclick="btEdit_Click" />
                <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
            </p>
        </asp:Panel>
        <table class="auto">
            <tr>
                <td class="auto" valign="top">
                    <asp:Panel ID="panelEdit" runat="server" Visible="False">
                        <asp:Label ID="lblProsthesisReg" runat="server"></asp:Label>
                        <br />
                        <br />
                        <table class="auto">
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblProsthesisCd" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="tbxProsthesisCd" runat="server" 
                                        onkeyup="ValidateTextNumber(this)" Width="80px" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequired_ProsthesisCd" runat="server" 
                                        ControlToValidate="tbxProsthesisCd" Display="Dynamic" ForeColor="Red" 
                                        ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblProsthesisAbbNm" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="tbxProsthesisAbbNm" runat="server" MaxLength="5" Width="80px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequired_ProsthesisAbbNm" runat="server" 
                                        ControlToValidate="tbxProsthesisAbbNm" Display="Dynamic" ForeColor="Red" 
                                        ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblProsthesisNm" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="tbxProsthesisNm" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequired_ProsthesisNm" runat="server" 
                                        ControlToValidate="tbxProsthesisNm" Display="Dynamic" ForeColor="Red" 
                                        ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblProsthesisType" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:DropDownList ID="DropDownListProsthesisType" runat="server" Width="155px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="valRequired_ProsthesisType" runat="server" 
                                        ControlToValidate="DropDownListProsthesisType" Display="Dynamic" ForeColor="Red" 
                                        ValidationGroup="Save" Visible="False">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblStump" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:CheckBox ID="cbStump" runat="server" />
                                </td>
                            </tr>
                            <tr class="auto">
                                <td class="auto" style="text-align: left">
                                    <asp:Label ID="lblMinimumProcess" runat="server"></asp:Label>
                                </td>
                                <td class="auto" style="text-align: left">
                                    <asp:TextBox ID="tbxMinimumProcess" runat="server" MaxLength="5" 
                                        onkeyup="ValidateTextNumber(this)" Width="80px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="valNumbersOnly_MinimumProcess" 
                                        runat="server" ControlToValidate="tbxMinimumProcess" Display="Dynamic" 
                                        ForeColor="Red" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" 
                                        ValidationGroup="Save">*</asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="valRequired_MinimumProcess" runat="server" 
                                        ControlToValidate="tbxMinimumProcess" Display="Dynamic" ForeColor="Red" 
                                        ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto" colspan="2">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                                        ValidationGroup="Save" />
                                </td>
                            </tr>
                        </table>
                        <p style="text-align: left; margin-top: 15px;">
                            <asp:Button ID="btSave" runat="server" 
                                ValidationGroup="Save" onclick="btSave_Click" />
                            <asp:Button ID="btDelete" runat="server" onclick="btDelete_Click" />
                            <asp:Button ID="btCancel" runat="server" CausesValidation="false" 
                                onclick="btCancel_Click" />
                        </p>
                    </asp:Panel>
                </td>
                <td class="auto" valign="top">
                    <asp:Panel ID="PanelProcessView" runat="server" Visible="False">
                        <asp:Label ID="lblProcess" runat="server"></asp:Label>
                        
                        <asp:GridView ID="gridProcess" runat="server" ShowHeader="False"
                        CssClass="gridView" AutoGenerateColumns="False" AllowPaging="True" 
                            onrowcreated="gridProcess_RowCreated" 
                            onpageindexchanging="gridProcess_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbProcess" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProcessCd" />
                                <asp:BoundField DataField="ProcessNm" />
                                <asp:BoundField DataField="IsDeleted" Visible="False" />
                                <asp:BoundField DataField="DisplayOrder" Visible="False" />
                            </Columns>
                      <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
      <PagerStyle CssClass="pagination" BackColor="White" />
      <RowStyle  CssClass="tr_body" />
                        </asp:GridView>

                        <asp:Label ID="lblNoRecord" runat="server"></asp:Label>
                        <br />
                        <asp:Button ID="btnRegProcess" runat="server" ValidationGroup="Save" 
                            onclick="btnRegProcess_Click" />
                        <asp:Button ID="btnEditProcess" runat="server" onclick="btnEditProcess_Click" />

                        <asp:CheckBox ID="cbShowIsDeleted" runat="server" AutoPostBack="True" 
                            Visible="False" />
                        <br />
                    </asp:Panel>
                    <asp:Panel ID="PanelProcessEdit" runat="server" Visible="False">
                    <table>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblProsthesisCd0" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxProsthesisCd0" runat="server" Enabled="False" 
                                MaxLength="10" onkeyup="ValidateTextNumber(this)" Width="80px"></asp:TextBox>
                            <asp:TextBox ID="tbxProsthesisNm0" runat="server" Enabled="False" 
                                MaxLength="30" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="LabelProcessCd" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcessCd" runat="server" AutoPostBack="True" 
                                    MaxLength="10" Width="80px" ontextchanged="txtProcessCd_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="selProcessRegister" runat="server" AutoPostBack="True" 
                                    DataTextField="ProcessNm" DataValueField="ProcessCd" 
                                    Width="155px" 
                                    onselectedindexchanged="selProcessRegister_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="valRequiredInput_ProcessCd" runat="server" 
                                    ControlToValidate="txtProcessCd" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="ValidationProcess">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="LabelDisplayOrder" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDisplayOrder" runat="server" Width="80px" onkeyup="ValidateTextNumber(this)"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                </table>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" 
                            ValidationGroup="ValidationProcess" />
                        <asp:Button ID="btnSave" runat="server" 
                            ValidationGroup="ValidationProcess" onclick="btnSave_Click" />
                        <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="PanelViewTechPrice" runat="server" Visible="False">
                        <asp:Label ID="lblTechPrice" runat="server"></asp:Label>
                         <asp:GridView ID="gridViewTech" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CssClass="gridView" ShowHeader="False" 
                            style="margin-top:0px; text-align: center;" 
                            onrowcreated="gridViewTech_RowCreated" 
                            onpageindexchanging="gridViewTech_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbTechPrice" runat="server" AutoPostBack="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TechCd" />
                                <asp:BoundField DataField="TechNm" />
                                <asp:BoundField DataField="StandardTechPrice" />
                                <asp:BoundField DataField="Editable" />
                                <asp:BoundField DataField="TechGroup" />
                                <asp:BoundField DataField="StartDate" HtmlEncode="false">
                                </asp:BoundField>
                                <asp:BoundField DataField="EndDate" HtmlEncode="false">
                                </asp:BoundField>
                                <asp:BoundField DataField="DisplayOrder" Visible="False" />
                            </Columns>
                            <PagerSettings FirstPageText="First" LastPageText="Last" 
                                Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
                                PreviousPageText="Prev" />
                            <PagerStyle BackColor="White" CssClass="pagination" />
                            <RowStyle CssClass="tr_body" />
                            
                        </asp:GridView>

                        <asp:Label ID="lblNorecordFoundTech" runat="server" Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnRegTechPrice" runat="server" 
                            ValidationGroup="Save" onclick="btnRegTechPrice_Click" />
                        <asp:Button ID="btnEditTechPrice" runat="server" 
                            onclick="btnEditTechPrice_Click" />
                        <asp:Button ID="btnViewTechPice" runat="server" 
                            onclick="btnViewTechPice_Click" />

                        <asp:CheckBox ID="chkAvailableTechPrice" runat="server" AutoPostBack="True" 
                            Visible="False" />
                    </asp:Panel>
                    <asp:Panel ID="PanelViewTechPriceDetail" runat="server" Visible="False">
                        <asp:Label ID="LabelTechPrice" runat="server"></asp:Label>
                        <asp:GridView ID="GridViewTechPrice" runat="server" AutoGenerateColumns="False" 
                            CssClass="gridView" 
                            ShowHeader="False" onrowcreated="GridViewTechPrice_RowCreated">
                            <Columns>
                                <asp:BoundField DataField="TechNm" />
                                <asp:BoundField DataField="StartDate" />
                                <asp:BoundField DataField="DentalOfficeNm" />
                                <asp:BoundField DataField="TechPrice" />
                            </Columns>
                            <PagerSettings FirstPageText="First" LastPageText="Last" 
                                Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
                                PreviousPageText="Prev" />
                            <PagerStyle BackColor="White" CssClass="pagination" />
                            <RowStyle CssClass="tr_body" />
                            
                        </asp:GridView>
                        <asp:Label ID="lblNorecordTechPrice" runat="server" Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelTechDetail" runat="server" 
                            onclick="btnCancelTechDetail_Click" />
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="PanelTechPriceEdit" runat="server" Visible="False">
                        <table class="auto">
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblProsthesisCd1" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="tbxProsthesisCd1" runat="server" Enabled="False" 
                                        MaxLength="10" onkeyup="ValidateTextNumber(this)" Width="80px"></asp:TextBox>
                                    <asp:TextBox ID="tbxProsthesisNm1" runat="server" Enabled="False" 
                                        MaxLength="30" Width="150px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblTechPriceCd" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="txtTechCd" runat="server" AutoPostBack="True" Width="80px" 
                                        MaxLength="10" ontextchanged="txtTechCd_TextChanged"></asp:TextBox>
                                    <asp:DropDownList ID="selTechPriceEdit" runat="server" 
                                        DataTextField="TechNm" DataValueField="TechCd" Width="155px" 
                                        OnChange="javascript:GetDropDownValue1()" Height="22px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="valRequiredNumber_TechCd" runat="server" 
                                        ControlToValidate="txtTechCd" ForeColor="Red" 
                                        ValidationGroup="SaveTechPrice">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto">
                                    <asp:Label ID="lblDisplayOrderTech" runat="server"></asp:Label>
                                </td>
                                <td class="auto">
                                    <asp:TextBox ID="txtDisplayOrderTech" runat="server" 
                                        onkeyup="ValidateTextNumber(this)" Width="80px" MaxLength="5"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ForeColor="Red" 
                            ValidationGroup="SaveTechPrice" />
                        <asp:Button ID="btnSavetechPrice" runat="server" 
                            ValidationGroup="SaveTechPrice" onclick="btnSavetechPrice_Click" />
                        <asp:Button ID="btnDeleteTechPrice" runat="server" 
                            onclick="btnDeleteTechPrice_Click" />
                        <asp:Button ID="btnCancelTechPrice" runat="server" CausesValidation="False" 
                            onclick="btnCancelTechPrice_Click" />
                        <br />
                    </asp:Panel>
                    <br />
                </td>
            </tr>
        </table>
        <br />
 </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

