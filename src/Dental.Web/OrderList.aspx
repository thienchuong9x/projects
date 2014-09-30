<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="OrderList" %>
<%@ Register src="~/UCCadCamFiles.ascx" tagname="CadCam" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

        <asp:Panel ID="Panel1" runat="server" BorderWidth="0" BorderColor="#C6C6C6">
            <table>
                <tr>
                    <td >
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
                        <asp:TextBox ID="txtFromDate" runat="server" Width="80px" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                        <asp:HyperLink ID="hplFromDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtToDate" runat="server" Width="80px" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:HyperLink ID="hplToDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>
                    <td>
                        <asp:Label ID="lbDeliveryDate" runat="server" Text="Delivery Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeliveryFromDate" runat="server" Width="80px" OnTextChanged="txtDeliveryFromDate_TextChanged"></asp:TextBox>
                        <asp:HyperLink  ID="hplDeliveryFromDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text=" ~ "></asp:Label>
                        <asp:TextBox ID="txtDeliveryToDate" runat="server" Width="80px" OnTextChanged="txtDeliveryToDate_TextChanged"></asp:TextBox>
                        <asp:HyperLink ID="hplDeliveryToDate" runat="server" ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbSalesman" runat="server" Text="Salesman"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSalesman" runat="server" OnTextChanged="txtSalesman_TextChanged"
                            Width="50px" AutoPostBack="True"></asp:TextBox>
                        <asp:TextBox ID="txtSalesman1" runat="server" Width="16px" ReadOnly="True" 
                            Visible="False"></asp:TextBox>
                        <asp:DropDownList ID="dlStaff" runat="server" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="dlStaff_SelectedIndexChanged"
                            Width="200px">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly2" ControlToValidate="txtSalesman"
                                ForeColor="Red" Display="Dynamic" 
                            ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>
    
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lbClinicName" runat="server" Text="Clinic Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClinicName" runat="server" OnTextChanged="txtClinicName_TextChanged"
                            Width="50px" AutoPostBack="True"></asp:TextBox>
                        <asp:TextBox ID="txtClinicName1" runat="server" Width="16px" ReadOnly="True" 
                            Visible="False"></asp:TextBox>
                        <asp:DropDownList ID="dlDentalOffice" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="dlDentalOffice_SelectedIndexChanged" Width="200px">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly3" ControlToValidate="txtClinicName"
                                ForeColor="Red" Display="Dynamic" 
                            ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>

                    </td>
                    <td>
                        <asp:Label ID="lbPatient" runat="server" Text="Patient"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPatient" runat="server" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbProstheticType" runat="server" Text="Prosthetic Type"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProstheticType" runat="server" Width="50px" OnTextChanged="txtProstheticType_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:TextBox ID="txtProstheticType1" runat="server" Width="16px" 
                            Visible="False"></asp:TextBox>
                        <asp:DropDownList ID="dlProstheticType" runat="server" AutoPostBack="True" Height="22px"
                            OnSelectedIndexChanged="dlProstheticType_SelectedIndexChanged" 
                            Width="200px">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly4" ControlToValidate="txtProstheticType"
                                ForeColor="Red" Display="Dynamic" 
                            ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>
                        </td>



                    <td>
                        <asp:Label ID="lbContractor" runat="server" Text="Contractor"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtContractorId" runat="server" Width="50px" 
                            AutoPostBack="True" ontextchanged="txtContractorId_TextChanged"></asp:TextBox>
                        <asp:TextBox ID="txtContractorName" runat="server" Width="16px" Visible="False"></asp:TextBox>
                        <asp:DropDownList ID="dlContractor" runat="server" AutoPostBack="True" 
                            Height="22px" Width="200px" 
                            onselectedindexchanged="dlContractor_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly5" ControlToValidate="txtContractorId"
                                ForeColor="Red" Display="Dynamic" 
                            ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">*</asp:RegularExpressionValidator>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbxCompletedDelivery" runat="server"  Text="Include Completed Delivery" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="cbxCompletedBill" runat="server"    Text=" Include Completed Bill" />
                    </td>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="cbxOnlyTrialOrder" runat="server" Text="Only Trial Order" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="cbxOnlyRemanufacture" runat="server" Text="Only Re-manufacture" />
                    </td>
                </tr>

                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btOk" runat="server" Text="Search" Width="64px" OnClick="btOk_Click" />
                        <asp:Button ID="btCancel" runat="server" Text="Clear" Width="64px" OnClick="btCancel_Click" />
                    </td>

                    <td></td>
                    <td></td>

                </tr>
            </table>
        <br />
        </asp:Panel>

        <table border="0" width ="100%">
                <tr>
                    <td>
                        <asp:Button ID="btNewOrder" runat="server" Text="New Order" OnClick="btNewOrder_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btEdit" runat="server" Text="Edit" OnClick="btEdit_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btProcess" runat="server" Text="Process" onclick="btProcess_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btMaterial" runat="server" Text="TechPrice" onclick="btMaterial_Click"  />

                        <%--style="padding-left: 10px; padding-right: 10px"--%>
                    </td>

                    <td align="right">
<%--                        <asp:Button ID="btShowSearch" runat="server" Text="ShowSearch" onclick="btShowSearch_Click" />
                        <asp:Button ID="btHideSearch" runat="server" Text="HideSearch" onclick="btHideSearch_Click" />
--%>                        
                        <asp:LinkButton ID="btlShowSearch" runat="server" onclick="btShowSearch_Click">Show</asp:LinkButton>
                        <asp:LinkButton ID="btlHideSearch" runat="server" onclick="btHideSearch_Click">Hide</asp:LinkButton>
                        &nbsp; 

                         <asp:DropDownList ID="dlNumber" runat="server" 
                            onselectedindexchanged="dlNumber_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbRowPerPage" runat="server" Text="rows/page"></asp:Label> 
                    </td>

                    <td align="right">
<%--                        <input type="button" value="CAD/CAM" onclick="javascript:showFolder();" />--%> 
                       <asp:LinkButton ID="btlCad" runat= "server" onclick="btlCad_Click">CAD Order files</asp:LinkButton>

                    </td>

                </tr>
        </table>
        <br />
        <asp:GridView ID="gvOrderList" runat="server" BackColor="White" ShowHeader="False" CssClass="gridView" DataKeyNames="OrderSeq"
            EnableModelValidation="True" OnRowCreated="gvOrderList_RowCreated" AutoGenerateColumns="False"
            OnPageIndexChanging="gvOrderList_PageIndexChanging" AllowPaging="True">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="Check" runat="server" />
                        <asp:HiddenField ID="HiddenOrderSeq" runat="server" Value='<%# Eval("OrderSeq").ToString()%>' />
                    </ItemTemplate>
                    <ItemStyle Width="20px"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="OrderSeq" HeaderStyle-Width="0px"></asp:BoundField>
                <asp:BoundField DataField="OrderNo"/>
                <asp:BoundField DataField="StaffCd"> <ItemStyle Font-Size="Small"></ItemStyle>  </asp:BoundField>
                <asp:BoundField DataField="OrderDate"> <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>  </asp:BoundField>
                <asp:BoundField DataField="DeliverDate"> <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>  </asp:BoundField>

<%--                 <asp:BoundField DataField="DeliverDate"> <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>  </asp:BoundField>
--%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# showCompleteDate(Eval("DeliverDate").ToString(), Eval("DentalOfficeCd").ToString())%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>
                </asp:TemplateField>

                <asp:BoundField DataField="DentalOfficeCd"> <ItemStyle Font-Size="Small"></ItemStyle>  </asp:BoundField>
                <asp:BoundField DataField="DentistNm"> <ItemStyle Font-Size="Small" Width="100px"></ItemStyle>  </asp:BoundField>
                <asp:BoundField DataField="PatientLastNm"> <ItemStyle Font-Size="Small" Width="100px"></ItemStyle> </asp:BoundField>

<%--                <asp:BoundField DataField="PatientFirstNm">
                     <ItemStyle HorizontalAlign="Center" Font-Size="Small" Width="100px"></ItemStyle>
                </asp:BoundField>--%>

                <asp:BoundField DataField="PatientSex"> <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>  </asp:BoundField>
                <asp:BoundField DataField="PatientAge"> <ItemStyle HorizontalAlign="Center" Font-Size="Small"></ItemStyle>  </asp:BoundField>

            </Columns>
            <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" />
            <RowStyle  CssClass="tr_body" />
        </asp:GridView>
        <asp:Label ID="lbMessage" runat="server"></asp:Label>
        <asp:TextBox ID="txtOffice" runat="server" Visible="False"></asp:TextBox>
        
         <asp:Panel ID="panelPopUp" runat="server" Visible=false 
            ScrollBars="Auto" HorizontalAlign="Center">
          <div id="toPopup" style="display:block">
          	<div class="close">
                <asp:ImageButton Id="btnClose" runat ="server" 
                     onclick="btnClose_Click" ImageUrl="~/Styles/PopupLogin/img/closebox.png"/></div>
           
		    <div id="popup_content"> <!--your content start-->
            <h1><asp:Label ID="lblCadCam" runat="server"></asp:Label></h1>
            <hr />
                
               <uc1:CadCam ID="uc1" HomeFolder="~/Portal/XMLOrder" runat="server"/>
            </div> <!--your content end-->
    
        </div> <!--toPopup end-->
    </asp:Panel>


<style type="text/css">
    .dir
    {
        background: url("<%= Page.ResolveUrl("~/DesktopModules/OrderList/images/directory.png") %>") no-repeat scroll left top transparent;
    }
    
    .file
    {
        background: url("<%= Page.ResolveUrl("~/DesktopModules/OrderList/images/file.png") %>") no-repeat scroll left top transparent;
    }
    
    li.dir, li.file
    {
        list-style: none outside none;
        padding: 0 0 0 20px;
    }
</style>
<div id="divFolder" style="display: none; margin-top: 5px">
    <img id="imgUp" onclick="javascript:backup();" src="<%= Page.ResolveUrl("~/DesktopModules/OrderList/images/folder_up.png")%>"
        style="display: none; cursor: pointer">
    <ul id="divFilesContainer">
    </ul>
</div>
<asp:HiddenField ID="hiddenCadCamPopupTitle" runat="server" />
<script id="tmpl" type="text/html">
    <li 
        {{if Type == 0}}
            class="dir"
        {{else}}
            class="file"
        {{/if}} 
    >
    {{if Type == 0}}
        <a style="text-decoration:none; cursor:pointer" onclick="javascript:getFileAndDirectory('${Name}', '${Path}', ${Type});">${Name}</a>
    {{else}}
        <a href="${DownloadLink}">${Name}</a>
    {{/if}} 
        
    </li>
</script>
<script type="text/javascript">
    var fileViewAPIUrl = '<%= Page.ResolveUrl("~/DesktopModules/OrderList/API/FileViewAPI/GetFilesAndFolder")%>';
    var currDir;
    function showFolder() {
        $("#divFolder").dialog({
            dialogClass: $.dnnAlert.defaultOptions.dialogClass,
            width: 300,
            height: 400,
            title: "<%= hiddenCadCamPopupTitle.Value %>",
            open: function () {
                $.getJSON(fileViewAPIUrl, { root: "" }, function (data) {
                    $("#tmpl").tmpl(data).appendTo($("#divFilesContainer").empty());
                });
            }
        }).show();
    }

    function getFileAndDirectory(name, path, type) {
        $.getJSON(fileViewAPIUrl, { root: path + "/" + name }, function (data) {
            $("#tmpl").tmpl(data).appendTo($("#divFilesContainer").empty());
            currDir = path + "/" + name;
            $("#imgUp").show();
        });
    }
    function backup() {
        currDir = currDir.substr(0, currDir.lastIndexOf("/"));
        $.getJSON(fileViewAPIUrl, { root: currDir }, function (data) {
            $("#tmpl").tmpl(data).appendTo($("#divFilesContainer").empty());
            var arr = currDir.split("/");
            var parent = "";
            for (var i = 0; i < arr.length - 1; ++i) {
                if (parent == "") {
                    parent += arr[i];
                }
                else {
                    parent += "/" + arr[i];
                }
            }
            if (currDir == "~/Portals/XMLOrder") {
                $("#imgUp").hide();
            }
        });
    }
</script>

<asp:HiddenField ID="hiddenOfficeCd" runat ="server" Visible ="false" /> 

</asp:Content>

