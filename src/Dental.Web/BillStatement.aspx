<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BillStatement.aspx.cs" Inherits="BillStatement" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

 <style type="text/css">
 .hiddencol
   {
       display: none;
   }
 .saveButton 
  { 
        position: relative;
            margin-left:350px;
         top:30px;
            z-index:1;
            width: 18px;
            left: 64px;
            height: 18px;
  }
</style>
<script type = "text/javascript">
    function RoundSystemPrice(num) {
        var hiddenRoundSystem = document.getElementById("<%=hiddenRoundSystem.ClientID%>").value;
        if (hiddenRoundSystem.toUpperCase() == "ROUNDUP") {
            num = Math.ceil(num);
        }
        else if (hiddenRoundSystem.toUpperCase() == "4DOWN5UP") {
            num = Math.floor(num + 0.5);
        }
        else
            num = Math.floor(num);
        return num;
    }
    function ConfirmIssue(msgConfirm) {
        var hiddenIsMultiBillCd = document.getElementById("<%=hiddenIsMultiBillCd.ClientID%>").value;
        if (hiddenIsMultiBillCd != "") {
            var GridView = document.getElementById("<%=gvOrderList.ClientID %>");
            var inputList = GridView.getElementsByTagName("input");
            var billCd = "";
            for (var i = 1; i < inputList.length; i++) //except checkall
            {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].checked) {
                    if (billCd == "")
                        billCd = row.cells[11].innerText;
                    else if (billCd != row.cells[11].innerText) {
                        billCd = "-1";
                        break;
                    }
                }
            }
            if (billCd != -1) {
                var hiddenChooseBillCd = document.getElementById("<%=hiddenChooseBillCd.ClientID%>").value;
                if (hiddenChooseBillCd == "-1" || (billCd != "" && hiddenChooseBillCd != "" && billCd != hiddenChooseBillCd))
                    billCd = "-1";
            }
            if (billCd == "-1") {
                return confirm(msgConfirm);
            }
        }
        return true;
    }
    function CalculateTotal() {
        var insuranced = 0;
        if (parseFloat(document.getElementById('<%=hiddenInsured.ClientID%>').value)) {
            insuranced = RoundSystemPrice(parseFloat(document.getElementById('<%= hiddenInsured.ClientID%>').value));
            document.getElementById('<%= txtBillingAmountInsured.ClientID%>').value = insuranced;
        }
        else {
            document.getElementById('<%= txtBillingAmountInsured.ClientID%>').value = "";
        }
        var unInsuranced = 0;
        if (parseFloat(document.getElementById('<%=hiddenUninsured.ClientID%>').value)) {
            unInsuranced = RoundSystemPrice(parseFloat(document.getElementById('<%= hiddenUninsured.ClientID%>').value));
            document.getElementById('<%= txtBillingAmountUnInsured.ClientID%>').value = unInsuranced;
        }
        else {
            document.getElementById('<%= txtBillingAmountUnInsured.ClientID%>').value = "";
        }
        document.getElementById('<%=txtSum.ClientID%>').value = insuranced + unInsuranced;
        var fee = 0;
        if (parseFloat(document.getElementById('<%=txtAdjustedFeeInsured.ClientID%>').value))
            fee = parseFloat(document.getElementById('<%= txtAdjustedFeeInsured.ClientID%>').value);

        document.getElementById('<%=txtBillingTotalAmount.ClientID%>').value = insuranced + unInsuranced - fee;
    }
    function CalInsuranced(row) {
        var Insuranced = row.cells[8].innerText;
        var price = 0;
        if (parseFloat(row.cells[9].innerText))
            price = price + parseFloat(row.cells[9].innerText);
        if (parseFloat(row.cells[10].innerText))
            price = price + parseFloat(row.cells[10].innerText);

        var fieldSetInsuranced = null;
        if (Insuranced == "True") {
            fieldSetInsuranced = document.getElementById('<%= hiddenInsured.ClientID%>');
            // fieldSetInsuranced = document.getElementById('<%= txtBillingAmountInsured.ClientID%>');
        }
        else
        // fieldSetInsuranced = document.getElementById('<%=txtBillingAmountUnInsured.ClientID%>');
            fieldSetInsuranced = document.getElementById('<%=hiddenUninsured.ClientID%>');

        var oldValue = 0;
        if (parseFloat(fieldSetInsuranced.value))
            oldValue = parseFloat(fieldSetInsuranced.value);

        var indexChildNode = 1;
        var isIE = navigator.appName;
        if (isIE == "Microsoft Internet Explorer") {
            if (navigator.appVersion.indexOf("MSIE 8") >= 0)
                indexChildNode = 0;
        }

        if (row.cells[0].childNodes[indexChildNode].checked)
            fieldSetInsuranced.value = parseFloat(oldValue) + parseFloat(price);
        else {
            if (Math.floor(parseFloat(oldValue)) == Math.floor(parseFloat(price))) {
                fieldSetInsuranced.value = 0;
                document.getElementById('<%=hiddenInsured.ClientID%>').value = 0;
                document.getElementById('<%=hiddenUninsured.ClientID%>').value = 0;
            }
            else
                fieldSetInsuranced.value = parseFloat(oldValue) - parseFloat(price);
        }
        CalculateTotal();
    }
    function Check_Click(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        var GridView = document.getElementById("<%=gvOrderList.ClientID %>");
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var headerCheckBox = inputList[0];
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;
        CalInsuranced(row);
    }
    function checkAll(objRef) {
        var GridView = document.getElementById("<%=gvOrderList.ClientID %>");
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                if (objRef.checked) {
                    if (inputList[i].checked == false) {
                        inputList[i].checked = true;
                        CalInsuranced(row);
                    }
                }
                else {
                    if (inputList[i].checked == true) {
                        inputList[i].checked = false;
                        CalInsuranced(row);
                    }
                }
            }
        }
    }
  </script>

<asp:Panel ID="panelBillStatement" runat="server">
     <div> 
         <asp:RadioButton ID="rbNewIssue" runat="server" GroupName="issueType" Checked="True" 
                            AutoPostBack="True" oncheckedchanged="rbNewIssue_CheckedChanged" />
          <asp:Label ID="lbYearMonth" runat="server" style="margin-left:30px"/>
          <asp:DropDownList ID="dlYear" runat="server" AutoPostBack="True" 
                        Height="22px" 
                        Width="60px">
           </asp:DropDownList>
           <asp:DropDownList ID="dlmonth" runat="server" AutoPostBack="True" 
                        Height="22px" 
                        Width="50px">
           </asp:DropDownList>
     </div>
     <div>
     <asp:CheckBox ID="cbSearch" runat="server"  style="padding-left:30px;" 
             AutoPostBack="True" oncheckedchanged="cbSearch_CheckedChanged"/> 
     </div>

     <asp:Panel ID="panelConditions"  runat="server" Enabled="False">
            <table>
                <tr>
                    <td style="padding-left:25px">
                       <asp:Label ID="lbInvoiceRecipient" runat="server" Text="Invoice Recipient"></asp:Label>
                   </td>
                   <td>
                        <asp:TextBox ID="txtInvoiceRecipient" runat="server" OnTextChanged="txtInvoiceRecipient_TextChanged"
                                    Width="34px" AutoPostBack="True"></asp:TextBox>
                         <asp:DropDownList ID="dropDownBill" runat="server" 
                            AutoPostBack="True" Height="22px"
                                    OnSelectedIndexChanged="dropDownBill_SelectedIndexChanged" 
                            Width="180px">
                                </asp:DropDownList>
                  </td>
                  <td></td>
                    <td><asp:Label ID="lbClinicName" runat="server" Text="Clinic Name"></asp:Label></td>
                    <td>
                      <asp:TextBox ID="txtDentalOfficeCd" runat="server" OnTextChanged="txtDentalOfficeCd_TextChanged"
                            Width="34px" AutoPostBack="True"></asp:TextBox>
                        <asp:DropDownList ID="dropDownDentalOffice" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="dropDownDentalOffice_SelectedIndexChanged" 
                            Width="180px">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td style="padding-left:25px">
                        <asp:Label ID="lbOrderDate" runat="server" Text="Order Date (M/d/Y)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="80px"></asp:TextBox><asp:HyperLink
                            ID="hplFromDate" runat="server" ImageUrl="~/Styles/images/calendar.png" 
                            ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="80px"></asp:TextBox><asp:HyperLink
                            ID="hplToDate" runat="server" ImageUrl="~/Styles/images/calendar.png" 
                            ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>
                    <td style="width:10px">&nbsp;</td>
                    <td>
                        <asp:Label ID="lbDeliveryDate" runat="server" Text="Delivery Date"></asp:Label>                                         
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryFromDate" runat="server" Width="80px"></asp:TextBox><asp:HyperLink
                            ID="hplDeliveryFromDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtDeliveryToDate" runat="server" Width="80px"></asp:TextBox><asp:HyperLink
                            ID="hplDeliveryToDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left:25px">
                        <asp:Label ID="lbDeliveryType" runat="server" 
                            Text="Delivery Type"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbUnInsured" runat="server" Checked="true" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="cbInsured" runat="server" Checked="true" />
                        </td>
                    <td></td>                    
                     <td style="width:100px">
                        <asp:Label ID="lbOrderNo" runat="server" Text="Order No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
      </asp:Panel>  

     <asp:Panel ID="panelSearch" runat="server" Enabled="False">
                <asp:Button ID="btnSearch" runat="server" Text="Search" Width="64px" OnClick="btnSearch_Click" style="margin-left:25px;" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="64px" OnClick="btnClear_Click" style="margin-right:605px;"    /> 
                <asp:DropDownList ID="dlNumber" runat="server" Width="64px" 
                    onselectedindexchanged="dlNumber_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
                 <asp:Label ID="lbRowPerPage" runat="server" Text="rows/page"></asp:Label>           
    </asp:Panel>
    <asp:GridView ID="gvOrderList" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" BackColor="White" CssClass="gridView" 
        DataKeyNames="OrderSeq" EnableModelValidation="True" 
        OnPageIndexChanging="gvOrderList_PageIndexChanging" 
        OnRowCreated="gvOrderList_RowCreated" ShowHeader="False" 
        style="margin-top:10px;">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="Check" runat="server" onclick = "Check_Click(this);" />
                </ItemTemplate>
                <ItemStyle Width="20px" />
            </asp:TemplateField>
            <asp:BoundField DataField="OrderSeq" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="OrderNo">
            <ItemStyle />
            </asp:BoundField>
            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" DataFormatString="{0:yyyy/MM/dd}"
                        HtmlEncode="false" ItemStyle-Width="90px">
                 <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
           </asp:BoundField>
           <asp:BoundField DataField="DeliveredDate" HeaderText="DeliveredDate" DataFormatString="{0:yyyy/MM/dd}"
                        HtmlEncode="false" ItemStyle-Width="90px">
                 <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# GetDentalOfficeName(Eval("DentalOfficeCd").ToString())%>
                </ItemTemplate>
                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="PatientNm">
            <ItemStyle Width="120px" />
            </asp:BoundField>
            <asp:BoundField DataField="DetailNm" />
            <asp:BoundField DataField="ProsthesisNm" />
            <asp:BoundField DataField="InsuranceKbn"/>
            <asp:BoundField DataField="MaterialPrice" /> <%-- 10--%>
            <asp:BoundField DataField="ProcessPrice" />  <%-- 11--%>
            <asp:BoundField DataField="MaterialNm" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="DetailSeq" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="MaterialCd" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="SupplierCd" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="DentalOfficeCd" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="BillStatementNo" HeaderStyle-Width="0px" />
            <asp:BoundField DataField="Amount" HeaderStyle-Width="0px"/> 
            <asp:BoundField DataField="BillCd" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" /> <%-- 19--%>
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" 
            Mode="NumericFirstLast" NextPageText="Next" PageButtonCount="5" 
            PreviousPageText="Prev" />
        <PagerStyle BackColor="White" CssClass="pagination" />
        <RowStyle CssClass="tr_body" />
    </asp:GridView>
        <asp:Label ID="lbMessage" runat="server"></asp:Label>

<asp:Panel ID="panelInput" runat="server">
     <table border="0" style="margin-top:10px">
        <tr>
            <td>
                <asp:Label ID="lbInvoiceIssueDate" runat="server" Text="Invoice Issue Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtIssueDate" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink  ID="hplIssueDate" runat="server" 
                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                &nbsp;
                <asp:CompareValidator ID="RequiredDateIssueDate" runat="server" ControlToValidate="txtIssueDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="groupIssue">*</asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredIssueDate" runat="server" 
                                ControlToValidate="txtIssueDate"  ForeColor="Red" ValidationGroup="groupIssue">*</asp:RequiredFieldValidator>

            </td>
            <td></td>
            <td>
                <asp:Label ID="lbBillingAmount" runat="server" Text="Billing Amount"></asp:Label>
            </td>
               <td>
                <asp:Label ID="lbSum" runat="server" Text="Sum"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbAdjustedFee" runat="server" Text="Adjusted Fee"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbTotal" runat="server" Text="Total"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbComment" runat="server" Text="Comment"/>

            </td>
            <td>
                        <asp:TextBox ID="txtComment" runat="server" Width="318px" MaxLength="99" ></asp:TextBox>

            </td>
            <td>
                <asp:Label ID="lblInsured" runat="server" Text="Insured"></asp:Label>
            </td>
            <td>
                        <asp:TextBox ID="txtBillingAmountInsured" runat="server" Width="80px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox>
            </td>
            <td> 
                    <asp:TextBox ID="txtSum" runat="server" Width="80px" ReadOnly="True" 
                            Enabled="False"></asp:TextBox>
            </td>
            <td>
                        <asp:TextBox ID="txtAdjustedFeeInsured" runat="server" Width="80px" 
                            onkeyup ="CalculateTotal(this);" Enabled="False"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="RequiredAdjustedFee" ControlToValidate="txtAdjustedFeeInsured"
                                ForeColor="Red" Display="Dynamic" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)"  ValidationGroup="groupIssue">*</asp:RegularExpressionValidator>

            </td>
            <td>
               <asp:TextBox ID="txtBillingTotalAmount" runat="server" Width="80px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>

            </td>
            <td>
                <asp:Label ID="lblUnInsured" runat="server" Text="UnInsured"></asp:Label>
            </td>
            <td>
                        <asp:TextBox ID="txtBillingAmountUnInsured" runat="server" Width="80px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox>
            </td>
            <td>

            </td>
            <td>
                        
            </td>
        </tr>
</table>
</asp:Panel>

     <table>
         <tr>
                    <td>
                        <asp:RadioButton ID="rbReissueDelete" runat="server" GroupName="issueType" 
                            AutoPostBack="True" oncheckedchanged="rbNewIssue_CheckedChanged" />
                    </td>
                </tr>
            </table>

<asp:Panel ID="panelReIssue" runat="server" Enabled ="false">
            <table>
                <tr>
                    <td style="width:10px">&nbsp;</td>
                    <td style="margin-left:20px" >
                        <asp:Label ID="lbType" runat="server" Text="Type"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbReissue" runat="server" Checked="True" 
                            GroupName="Type" AutoPostBack="True" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="rbDelete" runat="server" GroupName="Type" 
                            AutoPostBack="True" />
                    </td>


                    <td style="width:60px">&nbsp;</td>
                    <td >
                        <asp:Label ID="lbInvoiceNo" runat="server" Text="Invoice No."></asp:Label>
                        <asp:TextBox ID="txtInvoiceNo" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredInvoiceNo" runat="server" ControlToValidate="txtInvoiceNo"  ForeColor="Red" 
                           ValidationGroup="groupIssue" Enabled="False">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
</asp:Panel>
 
   <div style="margin-bottom:48px;">  
       <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red"  ValidationGroup="groupIssue"/>
        <asp:Button ID="btnPrint" runat="server" onclick="btnPrint_Click" Text="Issue" ValidationGroup="groupIssue"/>
  </div>
       <asp:HiddenField ID="hiddenOfficeCd" runat="server" />
       <asp:HiddenField ID="hiddenIsMultiBillCd" runat="server" value="" />
       <asp:HiddenField ID="hiddenChooseBillCd" runat="server" value="" />
       <asp:HiddenField ID="hiddenRoundSystem" runat="server" value="" />
       <asp:HiddenField ID="hiddenUninsured" runat="server" value="" />
       <asp:HiddenField ID="hiddenInsured" runat="server" value="" />
</asp:Panel>

<asp:Panel ID="panelReport" runat="server" Visible="false">
    <asp:Button ID="btnBack" runat="server" Text="Back" onclick="btnBack_Click"/>
    <asp:ImageButton ID="ImageSave" runat="server" ImageUrl="~/Styles/images/save.png" onclick="ImageSave_Click" CssClass="saveButton" />
    <br />  
<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="" Width="910px" 
        ShowPrintButton="False" ShowRefreshButton="False">
</rsweb:ReportViewer>

</asp:Panel>



</asp:Content>

