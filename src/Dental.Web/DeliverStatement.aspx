<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="DeliverStatement.aspx.cs" Inherits="DeliverStatement" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">
     .saveButton 
      { 
            position: relative;
            margin-left:350px;
            top:30px;
            z-index:1;
            width: 18px;
      }
</style>
<script type="text/javascript" src="<%= Page.ResolveUrl("~/Resources/Shared/scripts/jquery/jquery.template.min.js") %>"></script>
<script type = "text/javascript">

    function ChangedClinicCd() {
        var text = document.getElementById('<%=txtClinicName.ClientID %>');
        var drop = document.getElementById('<%=dlDentalOffice.ClientID %>');
        drop.value = text.value;
        if (drop.value == "") {
            text.value = "";
            text.focus();
        }
        drop.onchange();
    }

    function ChangedStaffCd() {
        var text = document.getElementById('<%=txtSalesman.ClientID %>');
        var drop = document.getElementById('<%=dlStaff.ClientID %>');
        var dropOffice = document.getElementById('<%=dlDentalOffice.ClientID %>');
        drop.value = text.value;
        if (drop.value == "") {
            text.value = "";
            text.focus();
            return;
        }
        drop.onchange();
    }
    
    function disabledPrint() {

        document.getElementById('<%= btPrint.ClientID %>').disabled = true;
        document.getElementById('<%= btPrint.ClientID %>').style.cursor = "wait";
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
                    }
                }
                else {
                    if (inputList[i].checked == true) {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    }
    function consur() {
        if (document.getElementById('<%=txtIssueDate.ClientID%>').value == '') {
            document.getElementById('<%=viewSearch.ClientID%>').style.cursor = '';
            document.getElementById('<%=btPrint.ClientID%>').style.cursor = '';
        }
        else {
            document.getElementById('<%=viewSearch.ClientID%>').style.cursor = 'wait';
            document.getElementById('<%=btPrint.ClientID%>').style.cursor = 'wait';
        }
    }
</script>


<asp:Panel ID="viewSearch" runat="server">


        <asp:Panel ID="Panel1" runat="server" BorderWidth="0" BorderColor="#C6C6C6">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbOrderNo" runat="server" Text="Order No."></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbOrderDate" runat="server" Text="Order Date (M/d/Y)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Width="80px" ></asp:TextBox><asp:HyperLink
                            ID="hplFromDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="80px" ></asp:TextBox><asp:HyperLink
                            ID="hplToDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>


                    <td>
                        <asp:Label ID="lbDeliveryDate" runat="server" Text="Delivery Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryFromDate" runat="server" Width="80px" ></asp:TextBox><asp:HyperLink
                            ID="hplDeliveryFromDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtDeliveryToDate" runat="server" Width="80px" ></asp:TextBox><asp:HyperLink
                            ID="hplDeliveryToDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>
                </tr>
                <tr>

                    <td>
                        <asp:Label ID="lbSalesman" runat="server" Text="Salesman"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSalesman" runat="server" onkeyup="ChangedStaffCd(this.id)" Width="50px"></asp:TextBox>
                        <asp:DropDownList ID="dlStaff" runat="server" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="dlStaff_SelectedIndexChanged"
                            Width="180px">
                        </asp:DropDownList>

                        &nbsp;&nbsp;&nbsp;&nbsp;

                    </td>


                    <td>
                        <asp:Label ID="lbClinicName" runat="server" Text="Clinic Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClinicName" onkeyup = "ChangedClinicCd(this.id)" runat="server" Width="50px"></asp:TextBox>
                        <asp:DropDownList ID="dlDentalOffice" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="dlDentalOffice_SelectedIndexChanged" Width="180px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbxDepositCompleteFlg" runat="server" />
                        <asp:Label ID="lblIncludeCompleteOrder" runat="server" 
                            Text="Include Completed Order"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btOk" runat="server" Text="Search" Width="64px" OnClick="btOk_Click" />
                        <asp:Button ID="btCancel" runat="server" Text="Clear" Width="64px" 
                            OnClick="btCancel_Click" />
                        <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
                    </td>
                    <td></td>
                    <td></td>
                    <td align="right">
                         <asp:DropDownList ID="dlNumber" runat="server" 
                            onselectedindexchanged="dlNumber_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbRowPerPage" runat="server" Text="rows/page"></asp:Label>            

                    </td>
                </tr>
            </table>
        </asp:Panel>

        <br />
        <asp:GridView ID="gvOrderList" runat="server" BackColor="White" ShowHeader="False" CssClass="gridView" DataKeyNames="OrderSeq"
            EnableModelValidation="True" OnRowCreated="gvOrderList_RowCreated" AutoGenerateColumns="False"
            OnPageIndexChanging="gvOrderList_PageIndexChanging" AllowPaging="True">
            <Columns>
                <asp:TemplateField> <%--cell 0--%>
                    <ItemTemplate>
                        <asp:CheckBox ID="Check" runat="server" onclick = "Check_Click(this);" />
                        <asp:HiddenField ID="HiddenOrderSeq" runat="server" Value='<%# Eval("OrderSeq").ToString()%>' />
                    </ItemTemplate>
                    <ItemStyle Width="20px"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderSeq" HeaderStyle-Width="0px"></asp:BoundField><%--cell 1--%>
                <asp:BoundField DataField="DetailSeq" HeaderStyle-Width="0px"></asp:BoundField><%--cell 2--%>
                <asp:BoundField DataField="OrderNo"><%--cell 3--%>
                    <ItemStyle></ItemStyle>
                </asp:BoundField>
                
                <asp:BoundField DataField="OrderDate"><%--cell 4--%>
                         <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="DeliverDate"><%--cell 5--%>
                         <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="DeliveredDate"><%--cell 6--%>
                         <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:BoundField>
                                
                <asp:BoundField DataField="DentalOfficeCd"><%--cell 7--%>
                         <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="PatientNm"><%--cell 8--%>
                    <ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="ProsthesisNm"/><%--cell 9--%>

                <asp:BoundField DataField="ToothNumberStr"/><%--cell 10--%>
                <asp:BoundField DataField="InsuranceKbn"/><%--cell 11--%>
                <%--<asp:TemplateField>
                    <ItemTemplate>
                        <%# showInsuranceKbn(Eval("InsuranceKbn").ToString())%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:TemplateField>--%>

                <asp:TemplateField><%--cell 12--%>
                    <ItemTemplate>
                       <%-- <%# showType(Eval("TrialOrderFlgReport").ToString(), Eval("RemanufactureFlgReport").ToString())%>--%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:TemplateField>

                <asp:BoundField DataField="TrialOrderFlg"/><%--cell 13--%>
                <asp:BoundField DataField="RemanufactureFlg"/><%--cell 14--%>

            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" />
            <RowStyle  CssClass="tr_body" />
        </asp:GridView>
        <asp:Label ID="lbMessage" runat="server"></asp:Label>
        <asp:CheckBox ID="cbTemp" runat="server" Visible="False" />
<br /> 
<asp:CheckBox ID="cbxPrintPerOrder" runat="server" />
<asp:Label ID="lbPrintPerOrder" runat="server" Text="Print Per Order"></asp:Label>
<br /> 
 
 <asp:Label ID="lbIssueDate" runat="server" Text="Delivery Date"></asp:Label>
 <asp:TextBox ID="txtIssueDate" runat="server" Width="80px" 
            ValidationGroup="PrintGroup"></asp:TextBox>
 <asp:HyperLink  ID="hplIssueDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
 <asp:CompareValidator ID="val_IssueDate" runat="server" ControlToValidate="txtIssueDate" 
               Operator="DataTypeCheck" Type="Date" Display="Dynamic" 
        ForeColor="Red" ValidationGroup="PrintGroup"></asp:CompareValidator>
 <asp:RequiredFieldValidator ID="valRequired_IssueDate" runat="server" 
        ControlToValidate="txtIssueDate"  ForeColor="Red" ValidationGroup="PrintGroup"></asp:RequiredFieldValidator>
 <br />       <br />
<asp:Button ID="btPrint" runat="server" Text="Print" onclick="btPrint_Click" ValidationGroup="PrintGroup" />
<br />
</asp:Panel>
<asp:Panel ID="viewReport" runat="server" Visible="false">
   <asp:Button ID="btBack" runat="server" Text="Back" onclick="btBack_Click"/>
   <%--<asp:HyperLink ID="hyperlinkDentalFont" runat="server" Text="Download Dental Font" ></asp:HyperLink>--%>    
    <asp:LinkButton ID="LinkDentalFont" runat="server" onclick="LinkDentalFont_Click">Download Dental Font</asp:LinkButton>    
    <asp:ImageButton ID="ImageSave" runat="server" ImageUrl="~/Styles/images/save.png" onclick="ImageSave_Click" CssClass="saveButton" />
   <%--<asp:HyperLink ID="hyperlinkSave" runat="server" ImageUrl="~/Styles/images/Save.gif" CssClass="saveButton"></asp:HyperLink>--%>
   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="" Width="900px" 
        ShowPrintButton="False" ShowExportControls="False" 
        ShowRefreshButton="False"/>
</asp:Panel>

</asp:Content>

