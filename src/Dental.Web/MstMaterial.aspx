<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstMaterial.aspx.cs" Inherits="MstMaterial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript">    
    function checkPriority(chk) {
        var tbxAmount = document.getElementById("<%= tbxAmountByMinimumUnit.ClientID %>").value;
        if (chk.checked && tbxAmount != "1") {

            document.getElementById("<%= lbTest.ClientID %>").innerHTML = document.getElementById('<%= txtPriority.ClientID %>').value;
            document.getElementById("<%= lbTest.ClientID %>").style.visibility = 'visible';
        }
        else {
            document.getElementById("<%= lbTest.ClientID %>").innerHTML = " ";
            document.getElementById("<%= lbTest.ClientID %>").style.visibility = 'hidden';
        }
    }

    function Changed() {
        var text = document.getElementById('<%=txtSupCd.ClientID %>');
        var drop = document.getElementById('<%=DropDownListSup.ClientID %>');
        drop.value = text.value;
        if (drop.value == "") {
            text.value = "";
            text.focus();
        }
    }
</script>

<style type="text/css">
    .style1
    {
        height: 32px;
    }
</style>

<asp:UpdatePanel ID="updatePanelMstMaterial" runat="server">
    <ContentTemplate>
        <asp:Panel ID="panelGrid" runat="server">
            <asp:Label ID="lblModuleTitle" runat="server"   
                Visible="False"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCode" runat="server"  />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server"  />
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="200px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                    <td>
                        <asp:CheckBox ID="cbShowTerminate" runat="server" AutoPostBack="True" 
                            oncheckedchanged="cbShowTerminate_CheckedChanged" />
                        <asp:Label ID="lbShowTerminate" runat="server"/>
                   </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" OnClick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" OnClick="btClear_Click" />
                        <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gridMaterial" runat="server" ShowHeader="false" DataKeyNames="MaterialCd"
                CssClass="gridView" EnableModelValidation="True" 
                AutoGenerateColumns="False" style="margin-top:10px"
                OnRowCreated="gridMaterial_RowCreated" AllowPaging="True" 
                onpageindexchanging="gridMaterial_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MaterialCd">
                        <ItemStyle Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MaterialNm">
                        <ItemStyle Width="130px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MaterialCADNm">
                        <ItemStyle Width="130px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductMaker">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductCd">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductNm">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="LentPrice">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ApplyDate"> <%--cell8--%>
                        <ItemStyle HorizontalAlign="Center" Font-Size="Small" Width="80px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TerminateDate"> <%--cell9--%>
                        <ItemStyle HorizontalAlign="Center" Font-Size="Small" Width="80px"></ItemStyle>
                    </asp:BoundField>                  
                </Columns>
                 <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" /> 
                <RowStyle CssClass="tr_body" />
            </asp:GridView>
            <asp:Label ID="lbMessage" runat="server"></asp:Label>
                &nbsp;<div style="margin-top: 15px">
                <asp:Button ID="btRegister" runat="server" OnClick="btRegister_Click" />
                <asp:Button ID="btEdit" runat="server" OnClick="btEdit_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="panelInput" runat="server" Visible="false">
<table>
<tr>
<td valign="top">
        <asp:Panel ID="MaterialPanelInput" runat= "server">
            <table>
                <tbody>
                    <tr>
                        <td style="width: 120px">
                            <asp:Label ID="lblMaterialCd" runat="server" ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxMaterialCd" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly_MaterialCd" 
                                ControlToValidate="tbxMaterialCd"  ValidationGroup="MaterialGroup"
                                ForeColor="Red" Display="Dynamic" 
                                ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="valRequired_MaterialCd" runat="server" 
                                ControlToValidate="tbxMaterialCd"  ValidationGroup="MaterialGroup"
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMaterialNm" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxMaterialNm" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRequired_MaterialNm" runat="server" 
                                ControlToValidate="tbxMaterialNm"  ValidationGroup="MaterialGroup" 
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMaterialCADNm" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxMaterialCADNm" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valRequired_MaterialCADNm" runat="server" 
                                ControlToValidate="tbxMaterialCADNm"  ValidationGroup="MaterialGroup" 
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblProductMaker" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxProductMaker" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblProductNm" runat="server"  ></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="tbxProductNm" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblProductCd" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxProductCd" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbLentPrice" runat="server" Text="Lent Price" ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxLentPrice" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="valLentPrice" 
                                ControlToValidate="tbxLentPrice"  ValidationGroup="MaterialGroup" 
                                ForeColor="Red" Display="Dynamic" 
                                ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>

                            <asp:RequiredFieldValidator ID="valRequired_LentPrice" runat="server" 
                                ControlToValidate="tbxLentPrice"  ValidationGroup="MaterialGroup" 
                                ForeColor="Red">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>

                   <tr>
                        <td>
                            <asp:Label ID="lblShadeFlg" runat="server"   />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxShadeFlg" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblApplyDate" runat="server"   />
                        </td>
                        <td>
                            <asp:TextBox ID="txtApplyDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                            <asp:HyperLink ID="hplApplyDate" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="Val_ApplyDate" runat="server" ControlToValidate="txtApplyDate" 
                                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="MaterialGroup">*</asp:CompareValidator>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTerminateDate" runat="server"   />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerminateDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                            <asp:HyperLink ID="hplTerminateDate" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="Val_TerminateDate" runat="server" ControlToValidate="txtTerminateDate" 
                                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="MaterialGroup">*</asp:CompareValidator>

                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblUnitNm" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxUnitNm" runat="server" MaxLength="5" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td> </td>
                        <td> 
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red"  
                                ValidationGroup="MaterialGroup"/>
                        </td>
                    </tr>
                </tbody>
            </table>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btSave" runat="server" OnClick="btSave_Click" 
                    ValidationGroup="MaterialGroup" />
                <asp:Button ID="btDelete" runat="server" OnClick="btDelete_Click" Text="Delete" 
                    CausesValidation="false" />
                <asp:Button ID="btCancel" runat="server" OnClick="btCancel_Click" CausesValidation="false" />
            </p>
    </asp:Panel>
</td>

<td valign="top">
        <asp:Label ID="lblOtherUnit" runat="server" />       
        <asp:Label ID="lblCheckNewMaterial" runat="server" Visible="False"/>
        <asp:Panel ID="UnitpanelGrid" runat="server">
            <asp:GridView ID="gridUnit" runat="server" ShowHeader="false" DataKeyNames="UnitCd"
                CssClass="gridView" EnableModelValidation="True" style="margin-top:5px"
                AutoGenerateColumns="False" onrowcreated="gridUnit_RowCreated" 
                PageSize="5" onpageindexchanging="gridUnit_PageIndexChanging" 
                AllowPaging="True">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="UnitNm"><%-- cell 1--%>
                    </asp:BoundField>

                    <asp:BoundField DataField="Priority"><%-- cell 2--%>
                        <ItemStyle HorizontalAlign="Center"/>
                     </asp:BoundField>

                    <asp:BoundField DataField="AmountByMinimumUnit"/><%-- cell 3--%>

                    <asp:BoundField DataField="ApplyDate"><%-- cell 4--%>
                        <ItemStyle HorizontalAlign="Center" Font-Size="Small"/>
                     </asp:BoundField>

                    <asp:BoundField DataField="TerminateDate"><%-- cell 5--%>
                            <ItemStyle HorizontalAlign="Center" Font-Size="Small"/>
                    </asp:BoundField>                                 
                
                    <asp:BoundField DataField="UnitNote"><%-- cell 6--%>
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                </Columns> 
                <PagerSettings FirstPageText="First" LastPageText="Last" 
                Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
                PreviousPageText="Prev" />
            <PagerStyle BackColor="White" CssClass="pagination" />                           
                <RowStyle CssClass="tr_body" />
            </asp:GridView>
                    <asp:CheckBox ID="cbShowAllUnit" runat="server" AutoPostBack="True" oncheckedchanged="cbShowAllUnit_CheckedChanged" Text="Show terminated units" />
            <div style="margin-top: 15px">
                <asp:Button ID="btRegister1" runat="server" onclick="btRegister1_Click" />
                <asp:Button ID="btEdit1" runat="server" onclick="btEdit1_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="UnitpanelInput" runat="server" Visible="false">
            <table>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblUnitCd" runat="server"   
                                style="text-align: left"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxUnitCd" runat="server" Enabled="false" Width="80px" 
                                MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUnitNm1" runat="server" style="text-align: left"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxUnitNm1" runat="server" Width="80px" MaxLength="10"></asp:TextBox>

                             <asp:RequiredFieldValidator ID="req_UnitName" runat="server" 
                                ControlToValidate="tbxUnitNm1"  ValidationGroup="UnitGroup" 
                                ForeColor="Red">*</asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPriority" runat="server"  style="text-align: left"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbPriority" runat="server"  onclick="checkPriority(this);" />

                            &nbsp;
                            <asp:TextBox ID="txtPriority" runat="server" Width="0px" 
                                CausesValidation="True" BorderStyle="None" ForeColor="White"></asp:TextBox>

                            <asp:Label ID="lbTest" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAmountByMinimumUnit" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxAmountByMinimumUnit" runat="server" Width="80px" 
                                AutoPostBack="True" ontextchanged="tbxAmountByMinimumUnit_TextChanged" 
                                MaxLength="10"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" 
                                ID="valNumbersOnly_AmountByMinimumUnit" ControlToValidate="tbxAmountByMinimumUnit"
                                ForeColor="Red" 
                                ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" 
                                ValidationGroup="UnitGroup">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lblApplyDate1" runat="server"   />
                        </td>
                        <td>
                            <asp:TextBox ID="txtApplyDate1" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                            <asp:HyperLink ID="hplApplyDate1" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="val_ApplyDate1" runat="server" ControlToValidate="txtApplyDate1" 
                                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="UnitGroup">*</asp:CompareValidator>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTerminateDate1" runat="server"   />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerminateDate1" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                            <asp:HyperLink ID="hplTerminateDate1" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="val_TerminateDate1" runat="server" ControlToValidate="txtTerminateDate1" 
                                    Operator="DataTypeCheck" Type="Date" ForeColor="Red" 
                                    ValidationGroup="UnitGroup">*</asp:CompareValidator>

                        </td>
                    </tr>


                    <tr>
                        <td>
                            <asp:Label ID="lblUnitNote" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbxUnitNote" runat="server" Width="300px" MaxLength="40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td> 
                <asp:Button ID="btSave1" runat="server" onclick="btSave1_Click" 
                                ValidationGroup="UnitGroup" />
                <asp:Button ID="btDelete1" runat="server" CausesValidation="false" 
                    onclick="btDelete1_Click" />
                <asp:Button ID="btCancel1" runat="server" CausesValidation="false" 
                    onclick="btCancel1_Click" />                        
                        </td>
                    </tr>
                    <tr>
                         <td></td>
                         <td>
                             <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" 
                                 ValidationGroup="UnitGroup"/>
                         </td>                   
                    </tr>
                </tbody>
            </table>
            <p style="text-align: left; margin-top: 15px;">

            </p>
        </asp:Panel>
<%--<!-- =============== -->--%>
    <br>
    <asp:Panel ID="PricePanelGrid" runat="server">
        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblCheckNewMaterialPrice" runat="server" Visible="False"></asp:Label>
        <asp:GridView ID="gridMaterialPrice" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" CssClass="gridView" style="margin-top:5px"
            DataKeyNames="MaterialCd,SupplierCd,StartDate" EmptyDataText="" 
            onpageindexchanging="gridMaterialPrice_PageIndexChanging" 
            onrowcreated="gridMaterialPrice_RowCreated" PageSize="5" ShowHeader="False" 
            ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:TemplateField> <%--cell 0--%>
                    <ItemTemplate>
                        <asp:CheckBox ID="Check" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SupplierCd"/> <%--cell 1--%>
                <asp:BoundField DataField=""> <%--cell 2--%>
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="StartDate"><%--cell 3--%>
                     <ItemStyle HorizontalAlign="Center" Font-Size="Small" Width="80px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EndDate"><%--cell 4--%>
                     <ItemStyle HorizontalAlign="Center" Font-Size="Small" Width="80px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="PriceNm"><%--cell 5--%>
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Price"><%--cell 6--%>
                <ItemStyle Width="50px" />
                </asp:BoundField>
            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" 
                Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
                PreviousPageText="Prev" />
            <PagerStyle BackColor="White" CssClass="pagination" />
            <RowStyle CssClass="tr_body" />
        </asp:GridView>
        <asp:CheckBox ID="cb" runat="server" AutoPostBack="True" 
            oncheckedchanged="cb_CheckedChanged" Text="Show terminated materials" />
        <div style="margin-top: 15px">
            <asp:Button ID="btRegister2" runat="server" onclick="btRegister2_Click" />
            <asp:Button ID="btEdit2" runat="server" onclick="btEdit2_Click" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PricePanelInput" runat="server" Visible="false">
        <table class="auto">
            <tbody>
                <tr>
                    <td class="auto">
                        <asp:Label ID="lblMaCd" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMaCd" runat="server" Enabled="False" Width="80px" 
                            MaxLength="10"></asp:TextBox>
                        <asp:TextBox ID="txtMaName" runat="server" Enabled="false" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupCd" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSupCd" runat="server" onkeyup= "Changed(this.id)"
                             Width="80px" MaxLength="10"></asp:TextBox>
                        <asp:DropDownList ID="DropDownListSup" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListSup_SelectedIndexChanged" 
                            Width="155px" Height="22px" />
                        <asp:RequiredFieldValidator ID="rqSupCd" runat="server" 
                            ControlToValidate="txtSupCd" Display="Dynamic" ForeColor="Red" 
                            ValidationGroup="MaterialPrice">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStartDate" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStartDate" runat="server" onkeyup="ValidateTextDate(this)" 
                            Width="80px"></asp:TextBox>
                        <asp:HyperLink ID="hplStartDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png">HyperLink</asp:HyperLink>
                        <asp:RequiredFieldValidator ID="rqStartDate" runat="server" 
                            ControlToValidate="txtStartDate" Display="Dynamic" ForeColor="Red" 
                            ValidationGroup="MaterialPrice">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="val_StartDate" runat="server" ControlToValidate="txtStartDate" 
                                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="MaterialPrice">*</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="lblEndDDate" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtEndDate" runat="server" onkeyup="ValidateTextDate(this)" 
                            Width="80px"></asp:TextBox>
                        <asp:HyperLink ID="hplEndDate" runat="server" ImageUrl="~/Styles/images/calendar.png">HyperLink</asp:HyperLink>
                        <asp:CompareValidator ID="val_EndDate" runat="server" ControlToValidate="txtEndDate" 
                                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                                    ValidationGroup="MaterialPrice">*</asp:CompareValidator>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPriceName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPriceName" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPrice" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrice" runat="server" onkeyup="ValidateTextNumber(this)" 
                            MaxLength="10" Width="80px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqPrice" runat="server" 
                            ControlToValidate="txtPrice" ForeColor="Red" 
                            ValidationGroup="MaterialPrice">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btSave2" runat="server" onclick="btSave2_Click" 
                            ValidationGroup="MaterialPrice" />
                        <asp:Button ID="btDelete2" runat="server" CausesValidation="false" 
                            onclick="btDelete2_Click" />
                        <asp:Button ID="btCancel2" runat="server" CausesValidation="false" 
                            onclick="btCancel2_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ForeColor="Red" 
                            ValidationGroup="MaterialPrice" />
                    </td>
                </tr>
            </tbody>
        </table>
        <p style="text-align: left; margin-top: 15px;">
        </p>
    </asp:Panel>
</td>

</tr>
</table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

