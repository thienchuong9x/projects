<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstTech.aspx.cs" Inherits="MstTech" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

   <script type="text/javascript">
       function GetDropDownValue() {
           var IndexValue = document.getElementById('<%=selDentalOffice.ClientID %>').selectedIndex;

           var SelectedVal = document.getElementById('<%=selDentalOffice.ClientID %>').options[IndexValue].value;

           document.getElementById('<%= txtDentalOfficeCd.ClientID %>').value = SelectedVal;


       }
</script>

<script type="text/javascript">
    function ValidateTextDate(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(/[^\d\/]+/g, '');
        }
    }
</script>

<script type="text/javascript">
    function ValidateTextNumber(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(/[^\d]+/g, '');
        }
    }
</script>

<script type="text/javascript">
    function ValidateText(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(/[^\d\.]+/g, '');
        }
    }

</script>



<asp:UpdatePanel ID="updatePanel" runat=server>
<ContentTemplate>
<asp:Panel ID ="panelShow" runat =server>
<div>
    <table class="auto">
        <tr>
            <td class="auto">
                <asp:Label ID="lblTechCd_search" runat="server"></asp:Label>
            </td>
            <td class="auto">
                <asp:TextBox ID="txtTechCd_search" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblTechNm_Search" runat="server"></asp:Label>
            </td>
            <td class="auto">
                <asp:TextBox ID="txtTechNm_search" runat="server" Width="300px" MaxLength="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblStartDate_Search" runat="server"></asp:Label>
            </td>
            <td class="auto">
                <asp:TextBox ID="txtStartDate1" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplStartDate1" runat="server" 
                    ImageUrl="~/Styles/images/calendar.png"></asp:HyperLink>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" 
                    Text="~"></asp:Label>
                <asp:TextBox ID="txtStartDate2" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplStartDate2" runat="server" 
                    ImageUrl="~/Styles/Images/calendar.png"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:CheckBox ID="cbAvailabelTech" runat="server" AutoPostBack="True" 
                    oncheckedchanged="cbAvailabelTech_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" />
            </td>
        </tr>
    </table>
    </div>
    <br />
<div>
    <asp:GridView ID="gridTech" runat="server" AutoGenerateColumns="False" 
        ShowHeader="False"
        CssClass="gridView" AllowPaging="True" onrowcreated="gridTech_RowCreated" onpageindexchanging="gridTech_PageIndexChanging" 
        >
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TechCd">
              <ItemStyle HorizontalAlign="Right"/>
            </asp:BoundField>

            <asp:BoundField DataField="TechNm"/>
            <asp:BoundField DataField="StandardTechPrice">
             <ItemStyle HorizontalAlign="Right"/>
            </asp:BoundField>

            <asp:BoundField DataField="Editable" ItemStyle-HorizontalAlign=Center 
                ItemStyle-VerticalAlign=Middle>
            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="TechGroup">
             <ItemStyle HorizontalAlign="Right"/>
            </asp:BoundField>
            <asp:BoundField DataField="StartDate">
             <ItemStyle HorizontalAlign="Center"/>
            </asp:BoundField>
            <asp:BoundField DataField="EndDate">
             <ItemStyle HorizontalAlign="Center"/>
            </asp:BoundField>
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
      <PagerStyle CssClass="pagination" BackColor="White" />
      <RowStyle  CssClass="tr_body" />
    </asp:GridView>
    <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
    <asp:Label ID="lblNorecord" runat="server" Visible="False"></asp:Label>
    <br />
    <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click" />
    <asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" />
    </div>
</asp:Panel>
<br />
<asp:Panel ID="panelEdit" runat =server Visible = false>

    <table class="auto">
        <tr>
            <td valign="top" class="auto">
                <asp:Label ID="lblTechTitle" runat="server" Text="Label"></asp:Label>
            </td>
            <td valign="top" class="auto">
                <asp:Label ID="lblTechPriceTitle" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto" valign="top">
                <asp:Panel ID="PanelEditTech" runat="server">
                    <table class="auto">
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblTechCd" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtTechCd" runat="server" onkeyup="ValidateTextNumber(this)" 
                                    Width="80px" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTechCd" runat="server" 
                                    ControlToValidate="txtTechCd" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="checkTech">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtStartDate" runat="server" onkeyup="ValidateTextDate(this)" 
                                    Width="80px"></asp:TextBox>
                                <asp:HyperLink ID="hplStartDate" runat="server" 
                                    ImageUrl="~/Styles/Images/calendar.png"></asp:HyperLink>
                                <asp:RequiredFieldValidator ID="reqStartDate" runat="server" 
                                    ControlToValidate="txtStartDate" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="checkTech">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidatorStartDate" runat="server" 
                                    ControlToValidate="txtStartDate" Display="Dynamic" ForeColor="Red" 
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="checkTech">*</asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtEndDate" runat="server" onkeyup="ValidateTextDate(this)" 
                                    Width="80px"></asp:TextBox>
                                <asp:HyperLink ID="hplEndDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"></asp:HyperLink>
                                <asp:CompareValidator ID="CompareValidatorEndDate" runat="server" 
                                    ControlToValidate="txtEndDate" Display="Dynamic" ForeColor="Red" 
                                    Operator="DataTypeCheck" Type="Date" ValidationGroup="checkTech">*</asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblTechNm" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtTechNm" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTechNm" runat="server" 
                                    ControlToValidate="txtTechNm" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="checkTech">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblStandardTechPrice" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtStandardTechPrice" runat="server" 
                                    onkeyup="ValidateTextNumber(this)" Width="80px" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqStandardTechPrice" runat="server" 
                                    ControlToValidate="txtStandardTechPrice" ForeColor="Red" 
                                    ValidationGroup="checkTech">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblTechGroup" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtTechGroup" runat="server" 
                                    onkeyup="ValidateTextNumber(this)" Width="80px" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblEditable" runat="server"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:CheckBox ID="cbEditable" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto" colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                                    ValidationGroup="checkTech" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSave" runat="server" 
                                    style="height: 26px" ValidationGroup="checkTech" Height="26px" 
                                    onclick="btnSave_Click" />
                                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                                    Visible="False" onclick="btnDelete_Click" />
                                <asp:Button ID="btnCancel" runat="server" Height="26px" 
                                    onclick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td class="auto" valign="top">
                <asp:Panel ID="PanelTechPrice" runat="server">
                    <asp:GridView ID="gridTechPrice" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CssClass="gridView" ShowHeader="False" 
                        onrowcreated="gridTechPrice_RowCreated" 
                        onpageindexchanging="gridTechPrice_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TechCd" Visible="False" />
                            <asp:BoundField DataField="DentalOfficeCd" >
                                   <ItemStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="TechNm" Visible="False" />
                            <asp:BoundField DataField="StartDate" Visible="False" />
                            <asp:BoundField DataField="DentalOfficeNm">
                                 <ItemStyle HorizontalAlign="Left"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="TechPrice">
                                <ItemStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings FirstPageText="First" LastPageText="Last" 
                            Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
                            PreviousPageText="Prev" />
                        <PagerStyle BackColor="White" CssClass="pagination" />
                        <RowStyle CssClass="tr_body" />
                    </asp:GridView>
                    <asp:Label ID="LabelNorecord" runat="server" Visible="False"></asp:Label>
                    <br />
                    <asp:Button ID="btnRegisterTechPrice" runat="server" 
                        ValidationGroup="checkTech" onclick="btnRegisterTechPrice_Click" />
                    <asp:Button ID="btnEditTechPrice" runat="server" 
                        onclick="btnEditTechPrice_Click" />
                </asp:Panel>
                <asp:Panel ID="PanelEditTechPrice" runat="server" Visible="False">
                    <table class="auto">
                        <tr>
                            <td>
                                <asp:Label ID="LabelDentalOffice" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDentalOfficeCd" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtDentalOfficeCd_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="selDentalOffice" runat="server" 
                                    OnChange="javascript:GetDropDownValue();" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="valRequiredDentalOffice" runat="server" 
                                    ControlToValidate="txtDentalOfficeCd" ForeColor="Red" 
                                    ValidationGroup="checkTechPrice">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelTechPrice" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTechPrice" runat="server" Width="80px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTechPrice" runat="server" 
                                    ControlToValidate="txtTechPrice" ErrorMessage="*" ForeColor="Red" 
                                    ValidationGroup="checkTechPrice">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" 
                                    ValidationGroup="checkTechPrice" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnSaveTechPrice" runat="server" Text="Button" 
                                    ValidationGroup="checkTechPrice" onclick="btnSaveTechPrice_Click" />
                                <asp:Button ID="btnDeleteTechPrice" runat="server" Enabled="False" 
                                    Text="Button" onclick="btnDeleteTechPrice_Click" />
                                <asp:Button ID="btnCancelTechPrice" runat="server" Text="Button" 
                                    onclick="btnCancelTechPrice_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

