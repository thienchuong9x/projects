<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OrderInput.aspx.cs" Inherits="OrderInput" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<style type="text/css">
 .hiddencol
   {
       display: none;
   }
</style>
<link rel="stylesheet" type="text/css" href="Styles/OrderInput.css" />
<script type="text/javascript" src="Scripts/jquery.min.js"></script>
<script type="text/javascript" src="Scripts/jquery.maphilight.min.js"></script>
<script type="text/javascript" language="javascript">
    function ImageMapMouseHover() {
        $('.map').maphilight();  //{ fillColor: '#CCFFFF', strokeWidth: 2, fillOpacity: 0.7 });
    }

    function ImageMapClick(toothNumber) {
        ImageMapMouseHover();
        var input = document.querySelector("input[id*=hiddenTooth]");
        input.value = toothNumber;
        var button = document.getElementById("<%= ButtonSaveBridge.ClientID %>");
        button.click();
    }
    function CheckWarningInput(Message) {
        var dentistName = document.getElementById('<%=TextDentist.ClientID%>').value;
        var patientLastName = document.getElementById('<%=TextPatientLastNm.ClientID%>').value;
        var patientFirstName = document.getElementById('<%=TextPatientFirstNm.ClientID%>').value;
        if (dentistName == "" || patientLastName == "" || patientFirstName == "") {
            if (confirm(Message)) {
                return true;
            }
            else
                return false;
        }
        return true;
    }
    function ConfirmDeleteTooth(msgConfirm) {
        if (confirm(msgConfirm) == true) {
            return true;
        }
        return false;
    }
    function setDeadLine() {
        var dueDateValue = document.getElementById('<%=TextDueDate.ClientID%>').value;
        if (dueDateValue != "") {
            var transferdayValue = document.getElementById("<%=hiddenTransferDay.ClientID%>").value;
            if (transferdayValue == "") {
                document.getElementById('<%=TextDeadLine.ClientID%>').value = dueDateValue;
            }
            else {
                var intTransferDay = parseInt(transferdayValue);
                var dtDueDate = new Date(dueDateValue);
                dtDueDate.setDate(dtDueDate.getDate() - intTransferDay);
                var formatDate = '<%= System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern %>';
                document.getElementById('<%=TextDeadLine.ClientID%>').value = dtDueDate.format(formatDate);
            }
        }
        else {
            document.getElementById('<%=TextDeadLine.ClientID%>').value = "";
        }
    }
    function ConfirmGoPage(msgConfirm) {
        return confirm(msgConfirm);
    }
    function checkTextAreaMaxLength(textBox, e, length) {

        var mLen = textBox["MaxLength"];
        if (null == mLen)
            mLen = length;
        var maxLength = parseInt(mLen);
        if (!checkSpecialKeys(e)) {
            if (textBox.value.length > maxLength - 1) {
                if (window.event)//IE
                {
                    e.returnValue = false;
                    return false;
                }
                else//Firefox
                    e.preventDefault();
            }
        }
    }
    function checkSpecialKeys(e) {
        if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 35 && e.keyCode != 36 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
            return false;
        else
            return true;
    }
 

</script>
 
<div class="divOrderInput">
    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" ForeColor="Red" ValidationGroup="RegisterOrderGroup" />
    <asp:Panel ID="panelOrderInput" runat="server">
        <div class="divOrderHeader">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
              
                    <table class="OrderInput">
                        <tr>
                            <td>
                                <asp:Label ID="LabelOrderNo" runat="server">Order No</asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:TextBox ID="TextOrderNo" runat="server" CssClass="OrderInputTextEntry" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredTextOrderNo" runat="server" ControlToValidate="TextOrderNo"
                                    CssClass="failureNotification" ErrorMessage="TextOrderNo is required." ToolTip="TextOrderNo is required."
                                   ForeColor="Red" ValidationGroup="RegisterOrderGroup">*</asp:RequiredFieldValidator>
                                   <asp:CustomValidator ID="RequiredUniqueTextOrderNo" runat="server" ControlToValidate="TextOrderNo"
                                    CssClass="failureNotification" ForeColor="Red" ValidationGroup="RegisterOrderGroup">*</asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelTrialOrder" runat="server">Trial Order</asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID = "checkTrialOrder" runat="server"/>
                            </td>

                            <td colspan="2">
                                <asp:Label ID="LabelRemanufacture" runat="server">Re-ManuFacture</asp:Label>
                                <asp:CheckBox ID ="checkRemanufacture" runat="server"/>
                            </td>

                            <td>
                                <asp:Label ID="LabelRefOrderNo" runat="server">Ref OrderNo</asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextRefOrderNo" runat="server" CssClass="OrderInputTextEntry"  Width="80px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelOrderDate" runat="server">Order Date</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextOrderDate" runat="server" CssClass="OrderInputTextEntry" 
                                    AutoPostBack="True"  ontextchanged="TextOrderDate_TextChanged"></asp:TextBox>
                                <asp:HyperLink ID="hplOrderDate" runat="server" ImageUrl="~/Styles/images/calendar.png"
                                    ToolTip="Select on Calendar"></asp:HyperLink>
                                <asp:RequiredFieldValidator ID="RequiredTextOrderDate" runat="server" ControlToValidate="TextOrderDate"
                                    CssClass="failureNotification" ErrorMessage="OrderDate is required." ToolTip="OrderDate is required."
                                    ValidationGroup="RegisterOrderGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="RequiredDateTimeTextOrder" runat="server" ControlToValidate="TextOrderDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterOrderGroup">*</asp:CompareValidator>
                            </td>
                            <td>
                                <asp:Label ID="LabelSalesman" runat="server">Salesman</asp:Label>
                            </td>

                            <td>
                                <asp:TextBox ID="TextStaffCd" runat="server" Width="70px" AutoPostBack="True" OnTextChanged="TextStaffCd_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="DropDownSalesMan" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownStaff_SelectedIndexChanged"
                                    Width="120px" Height="22px">
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:Label ID="LabelPatient" runat="server">Patient</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextPatientLastNm" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextPatientFirstNm" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelDueDate" runat="server">Due Date</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextDueDate" runat="server" CssClass="OrderInputTextEntry" onchange="javascript:setDeadLine();"></asp:TextBox>
                                <asp:HyperLink ID="hplDueDate" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                                <asp:RequiredFieldValidator ID="RequiredTextDueDate" runat="server" ControlToValidate="TextDueDate"
                                    CssClass="failureNotification" ErrorMessage="TextDueDate is required." ToolTip="TextDueDate is required."
                                    ValidationGroup="RegisterOrderGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="RequiredDateTimeTextDue" runat="server" ControlToValidate="TextDueDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterOrderGroup">*</asp:CompareValidator>
                            </td>
                             <td>
                                <asp:Label ID="LabelOfficeCd" runat="server">Clinic</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextOfficeCd" runat="server" Width="70px" AutoPostBack="True" OnTextChanged="TextOfficeCd_TextChanged"></asp:TextBox>
                            
                                <asp:DropDownList ID="DropDownDentalOffice" runat="server" AutoPostBack="True" Width="120px" Height="22px"
                                    OnSelectedIndexChanged="DropDownDentalOffice_SelectedIndexChanged">
                                </asp:DropDownList>
                            
                                <asp:RequiredFieldValidator ID="RequiredTextOfficeCd" runat="server" ControlToValidate="TextOfficeCd"
                                    CssClass="failureNotification" ErrorMessage="OfficeCode is required." ToolTip="OfficeCode is required."
                                    ValidationGroup="RegisterOrderGroup" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>

                           
                            <td>
                                <asp:Label ID="LabelSexAge" runat="server">Sex/Age</asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownSex" runat="server" Height="22px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="TextAge" runat="server" Width="40px"></asp:TextBox>
                                 <asp:RegularExpressionValidator runat="server" ID="RequiredNumberAge"
                                ControlToValidate="TextAge" ForeColor="Red" ValidationExpression="^[0-9]+$" ValidationGroup="RegisterOrderGroup">*
                               </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="LabelYear" runat="server" Width="70px">Year</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelDeadLine" runat="server">Dead Line</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextDeadLine" runat="server" CssClass="OrderInputTextEntry" BackColor="#CCCCCC"
                                    ReadOnly="True"></asp:TextBox>
                            </td>

                             <td>
                                <asp:Label ID="LabelDentist" runat="server">Dentist</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextDentist" runat="server" Width="140px" MaxLength="20"></asp:TextBox>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelSetDate" runat="server">Set Date</asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextSetDate" runat="server" CssClass="OrderInputTextEntry"></asp:TextBox>
                                <asp:HyperLink ID="hplSetDate" runat="server" 
                                    ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                                <asp:CompareValidator ID="RequiredDateTimeTextSet" runat="server" ControlToValidate="TextSetDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterOrderGroup">*</asp:CompareValidator>
                            </td>
                           
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="LabelBorrowPart" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBorrowPart" runat="server" TextMode="MultiLine" 
                                    Width="220px" Height="62px" MaxLength="100" onkeyDown="return checkTextAreaMaxLength(this,event,'100');"></asp:TextBox>
                            </td>
                            <td valign="top">
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="LabelComments" runat="server">Comments</asp:Label>
                         
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="TextComments" runat="server" CssClass="OrderInputTextEntry" 
                                    Height="62px" MaxLength="100" TextMode="MultiLine" Width="220px" onkeyDown="return checkTextAreaMaxLength(this,event,'100');"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <asp:HiddenField ID="hiddenTransferDay" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="divOrderDetail"  style="padding-top:10px">
            <asp:GridView runat="server" ID="gridViewOrderDetail" AutoGenerateColumns="false" CssClass="gridView"
                ShowHeader="false" OnRowCreated="gridViewOrderDetail_RowCreated" Width="890px">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderSeq"></asp:BoundField>
                    <asp:BoundField DataField="DetailNm" HeaderText="DetailNm"></asp:BoundField>
                    <asp:BoundField DataField="ProsthesisNm" HeaderText="ProsthesisName"  ItemStyle-Width="120px"></asp:BoundField>
                    <asp:BoundField DataField="MaterialNm" HeaderText="Material name" ItemStyle-Width="120px"></asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Material quantity" ItemStyle-Width="40px"></asp:BoundField>
                    <asp:BoundField DataField="ToothNumber" HeaderText="Tooth" ItemStyle-Width="40px"></asp:BoundField>
                    <asp:BoundField DataField="CompleteDate" HeaderText="completion date" DataFormatString="{0:yyyy/MM/dd}"
                        HtmlEncode="false" ItemStyle-Width="120px">
                    </asp:BoundField>
                    <asp:BoundField DataField="MaterialPrice" HeaderText="Material Price"  ItemStyle-Width="60px"></asp:BoundField>
                    <asp:BoundField DataField="BridgedID" HeaderText="BridgedID"  ItemStyle-Width="60px"></asp:BoundField>

                    <asp:BoundField DataField="ProcessPrice" HeaderText="Tech Price"  ItemStyle-Width="60px"></asp:BoundField>
                    <asp:BoundField DataField="DeliveryStatementNo" HeaderText="DeliveryStatementNo"  ItemStyle-Width="110px"></asp:BoundField>

                    <asp:BoundField DataField="MaterialCd" HeaderText="MaterialCd"></asp:BoundField>
                    <asp:BoundField DataField="Shape" HeaderText="Shape"></asp:BoundField>
                    <asp:BoundField DataField="Shade" HeaderText="Shade"></asp:BoundField>
                    <asp:BoundField DataField="Anatomykit" HeaderText="Anatomykit"></asp:BoundField>
                    <asp:BoundField DataField="CadOutputDone" HeaderText="CadOutputDone"></asp:BoundField>
                    <asp:BoundField DataField="InsuranceKbn" HeaderText="InsuranceKbn"></asp:BoundField>
                    <asp:BoundField DataField="DetailSeq" HeaderText="DetailSeq"></asp:BoundField>
                    <asp:BoundField DataField="ProsthesisCd" HeaderText="ProsthesisCd"></asp:BoundField>

                    <asp:BoundField DataField="ChildFlg" HeaderText="ChildFlg"></asp:BoundField>
                    <asp:BoundField DataField="GapFlg" HeaderText="GapFlg"></asp:BoundField>
                    <asp:BoundField DataField="DentureFlg" HeaderText="DentureFlg"></asp:BoundField>
                    <asp:BoundField DataField="BillStatementNo" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                    <asp:BoundField DataField="OldMaterialCd" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                </Columns>
                <RowStyle CssClass="tr_body" />
            </asp:GridView>
            <p style="text-align: left; margin-top: 5px;">
                <asp:Button ID="btnSave" CssClass="MasterManagementButton" runat="server" Text="Create/Edit"
                    OnClick="btnSave_Click" />
                
            </p>
        </div>
        <p style="text-align: right; padding-right: 15px;">
            <asp:LinkButton ID="lBtnDownload" runat="server" onclick="lBtnDownload_Click">Download CAD/CAM File</asp:LinkButton>
            <asp:HyperLink ID="HyperLink1" runat="server" Visible="False" Style="margin-right: 10px">Download CAD/CAM File</asp:HyperLink>
            <asp:Button ID="btnRegister" CssClass="MasterManagementButton" runat="server" Text="Register"
                ValidationGroup="RegisterOrderGroup" OnClick="btnRegister_Click" />
            <asp:Button ID="btnGoProcess" CssClass="MasterManagementButton" runat="server" Text="Go Process"
                      onclick="btnGoProcess_Click"/>
            <asp:Button ID="btnGoTechPrice" CssClass="MasterManagementButton" 
                    runat="server" Text="Go TechPrice" onclick="btnGoTechPrice_Click" />
            <asp:Button ID="btnCancel" CssClass="MasterManagementButton" runat="server" Text="Cancel"
                OnClick="btnCancel_Click" />
        </p>
    </asp:Panel>
   
    <asp:Panel ID="panelOrderInputDetail" runat="server" Visible="False">
            <div id="tabs-1" style="height: 520px; width: 820px; z-index: 1" class="ui-tabs-panel">
                <div id="left">
                    <img alt="" id="imageJam" class="map" src="Styles/images/detail_jam.png"
                        usemap="#map" />
                    <map id="map" name="map">
                        <area alt="" shape="poly" coords="133,19,119,21,114,29,125,45,135,45,146,35,146,25,143,22"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;11&quot;)" />
                        <area alt="" shape="poly" coords="108,28,100,26,89,31,88,39,99,49,108,50,112,44,113,31,109,28"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;12&quot;)" />
                        <area alt="" shape="poly" coords="79,38,73,39,69,40,65,43,66,57,73,64,84,65,92,62,93,52,91,44,85,39"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;13&quot;)" />
                        <area alt="" shape="poly" coords="54,61,49,68,50,79,64,86,75,85,81,79,79,71,66,60"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;14&quot;)" />
                        <area alt="" shape="poly" coords="46,85,36,92,36,103,42,110,62,112,69,109,72,105,71,98,65,91,61,89"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;15&quot;)" />
                        <area alt="" shape="poly" coords="31,120,30,134,33,144,50,150,63,148,67,142,67,135,69,126,64,118,50,113,37,113"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;16&quot;)" />
                        <area alt="" shape="poly" coords="64,161,48,151,30,149,26,155,28,175,38,182,57,184,62,178,61,175,63,170,63,168"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;17&quot;)" />
                        <area alt="" shape="poly" coords="34,186,46,185,60,191,65,198,63,205,62,214,47,216,36,213,32,210,28,202,27,192"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;18&quot;)" />
                        <area alt="" shape="poly" coords="151,25,151,31,158,43,164,45,172,43,182,32,181,25,174,20,163,19,157,20,155,22"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;21&quot;)" />
                        <area alt="" shape="poly" coords="199,26,193,26,188,27,185,31,184,41,189,49,193,50,206,42,209,35,208,31"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;22&quot;)" />
                        <area alt="" shape="poly" coords="206,44,205,56,207,63,215,66,225,64,231,57,233,53,232,43,229,40,220,37,212,38"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;23&quot;)" />
                        <area alt="" shape="poly" coords="246,63,249,75,245,81,232,87,222,86,217,81,216,74,220,68,234,59"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;24&quot;)" />
                        <area alt="" shape="poly" coords="246,85,228,93,224,101,226,109,237,113,250,112,260,106,260,91,252,85"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;25&quot;)" />
                        <area alt="" shape="poly" coords="246,114,231,120,228,127,230,136,231,147,239,150,255,148,268,140,266,119,259,113"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;26&quot;)" />
                        <area alt="" shape="poly" coords="262,148,236,157,232,164,235,172,235,180,242,184,259,181,268,174,272,162,270,153"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;27&quot;)" />
                        <area alt="" shape="poly" coords="252,186,238,189,233,196,235,211,238,215,254,215,266,209,268,196,268,188,265,186"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;28&quot;)" />
                        <area alt="" shape="poly" coords="150,414,150,422,154,428,164,429,171,422,170,414,162,406,158,406"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;31&quot;)" />
                        <area alt="" shape="poly" coords="180,402,189,404,194,411,194,418,189,426,180,426,174,422,172,412,174,405"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;32&quot;)" />
                        <area alt="" shape="poly" coords="220,408,220,400,213,391,203,392,197,398,196,407,199,413,208,416,217,413"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;33&quot;)" />
                        <area alt="" shape="poly" coords="215,377,216,388,224,395,235,395,242,389,241,381,232,373,223,372"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;34&quot;)" />
                        <area alt="" shape="poly" coords="232,348,243,350,254,354,255,364,251,374,238,375,228,370,223,364,225,353"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;35&quot;)" />
                        <area alt="" shape="poly" coords="234,312,230,335,233,343,243,348,261,348,266,339,266,333,271,324,265,308,253,304,243,305"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;36&quot;)" />
                        <area alt="" shape="poly" coords="239,274,238,295,245,302,265,302,272,299,275,279,269,270,258,268,246,268"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;37&quot;)" />
                        <area alt="" shape="poly" coords="239,241,238,259,245,266,266,265,273,259,272,243,264,234,254,233,247,234"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;38&quot;)" />
                        <area alt="" shape="poly" coords="139,405,129,411,126,419,128,426,136,429,144,427,146,425,146,417,147,414"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;41&quot;)" />
                        <area alt="" shape="poly" coords="119,402,108,405,102,411,102,419,108,425,116,426,124,422,125,407"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;42&quot;)" />
                        <area alt="" shape="poly" coords="101,397,101,410,94,416,82,415,77,409,77,401,81,393,88,391,92,391,95,392,99,395"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;43&quot;)" />
                        <area alt="" shape="poly" coords="82,378,82,385,78,392,70,396,61,395,56,389,56,380,61,375,72,372,78,373"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;44&quot;)" />
                        <area alt="" shape="poly" coords="68,349,74,357,74,365,68,372,51,375,45,373,42,367,43,354,53,349,54,349"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;45&quot;)" />
                        <area alt="" shape="poly" coords="36,307,44,305,58,308,64,312,68,339,59,347,40,349,34,348,29,339,29,332,25,325,27,316,32,308"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;46&quot;)" />
                        <area alt="" shape="poly" coords="31,269,52,270,59,274,60,295,51,303,31,302,24,298,23,277"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;47&quot;)" />
                        <area alt="" shape="poly" coords="35,234,50,235,59,242,59,260,51,267,28,265,23,260,24,248,27,239"
                            nohref="nohref" onmousedown="ImageMapClick(&quot;48&quot;)" />
                        <!--Out of Prosthetics-->
                        <area alt="" shape="poly" coords="127,9,82,21,51,39,33,64,11,207,12,331,73,429,161,443,228,417,272,368,288,248,280,124,259,56,212,17,168,4"
                            onmousemove="ImageMapMouseHover();" nohref="nohref" />
                    </map>
                    <asp:Button ID="btnAddNoTooth" runat="server" Text="Others" 
                        onclick="btnAddNoTooth_Click" />
                </div>
                
                <div id="right">
                    <asp:Panel ID="panelTreeView" runat="server" Width="480px" BorderWidth="1px" BorderColor="#CCCCCC"
                        CssClass="panelTreeViewTooth" Height="230px">
                        <asp:TreeView ID="TreeViewTooth" runat="server" CssClass="treeViewTooth" ShowLines="True"
                            Width="480px" OnSelectedNodeChanged="TreeViewTooth_SelectedNodeChanged" ShowCheckBoxes="Leaf">
                            <SelectedNodeStyle BackColor="#99CCFF" />
                        </asp:TreeView>
                    </asp:Panel>

                    <asp:Button ID="ButtonSaveBridge" runat="server" Text="Create/Delete Bridge" Style="margin: 5px 2px 5px 5px;"
                        OnClick="ButtonSaveBridge_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete"  Style="margin: 5px 2px 5px 2px;"
                    OnClick="btnDelete_Click" />
                   <asp:Button ID="btnUp" runat="server" Text="Up"  Style="margin: 5px 2px 5px 2px;"  onclick="btnUp_Click" />
                   <asp:Button ID="btnDown" runat="server" Text="Down"  Style="margin: 5px 2px 5px 2px;" onclick="btnDown_Click" />

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table>
                               <tr>
                                    <td style="width: 120px">
                                        <asp:Label ID="LabelDetailNm" runat="server">Detail Nm</asp:Label>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtDetailNm" runat="server" Width="205px" MaxLength="30" onkeypress="return event.keyCode != 13;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 120px">
                                        <asp:Label ID="LabelInsurance" runat="server">Insurance</asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="radioInsuranceTrue" runat="server"  Text="TRUE"  Checked="true" GroupName="groupInsurance"/>
                                        <asp:RadioButton ID="radioInsuranceFalse" runat="server"  Text = "FALSE" GroupName="groupInsurance"/>
                                    </td>
                                </tr>
  
                                <tr>
                                    <td style="width: 120px">
                                        <asp:Label ID="LabelProsthesis" runat="server">Type</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProsthesisAbbNm" runat="server" Width="40px" 
                                            ontextchanged="txtProsthesisAbbNm_TextChanged"  AutoPostBack="true"></asp:TextBox>
                                        <asp:TextBox ID="txtProsthesisNm" runat="server" Width="200px"
                                            Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                  <td></td>
                                  <td style="padding-left:48px">
                                     <asp:TextBox ID="txtProsthesisType" runat="server" Width="150px"
                                          Enabled="False"></asp:TextBox></td>
                                </tr>
                                
                                <tr>
                                     <td style="width: 120px">
                                        <asp:Label ID="LabelToothType" runat="server">ToothType</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="checkChildTooth" runat="server" Text="Child Tooth" 
                                            oncheckedchanged="checkChildTooth_CheckedChanged"  AutoPostBack="true"/>
                                        <asp:CheckBox ID="checkGap" runat="server" Text="Gap" />
                                        <asp:CheckBox ID="checkDenture" runat="server" Text="Denture" 
                                            oncheckedchanged="checkDenture_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="LabelMaterial" runat="server">Material</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownMaterial" runat="server" Width="140px" Height="22px" AutoPostBack="True">
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;
                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelCAD" runat="server">CAD Data</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="checkCAD" runat="server" Text="Output" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelShape" runat="server">Shape</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownShape" runat="server" Width="140px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LabelShade" runat="server">Shade</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownShade" runat="server" Width="140px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td src="Styles/images/detail_jam.png">
                                        <asp:Label ID="LabelAnatomyKit" runat="server">Anatomy Kit</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownAnatomyKit" runat="server" Width="140px" Height="22px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            <p style="text-align: right; padding: 5px 15px 5px 0;">
                <asp:Button ID="btnDetailSave" CssClass="MasterManagementButton" runat="server" Text="Register"
                    ValidationGroup="RegisterOrderGroup" OnClick="btnDetailSave_Click" />
                <asp:Button ID="btnDetailCancel" CssClass="MasterManagementButton" runat="server"
                    Text="Cancel" OnClick="btnDetailCancel_Click" />
            </p>
            <asp:HiddenField ID="hiddenTooth" runat="server" />
            <asp:HiddenField ID="hiddenBeforeTooth" runat="server" />
            <asp:HiddenField ID="hiddenAfterTooth" runat="server" />
            <asp:HiddenField ID="hiddenOrderSeq" runat="server" />
            <asp:HiddenField ID="hiddenOfficeCd" runat="server" />
            <asp:HiddenField ID="hiddenRegister" runat="server" Value ="" />
    </asp:Panel>
</div>

</asp:Content>

