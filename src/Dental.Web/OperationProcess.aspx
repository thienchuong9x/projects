<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OperationProcess.aspx.cs" Inherits="OperationProcess"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


 <script language="javascript" type="text/javascript">
     var oldgridSelectedColor;
     var seletedRow;
     $(document).ready(function () {
         EnableButton(true);

     });
     function setMouseOverColor(element) {
         oldgridSelectedColor = element.style.backgroundColor;
         element.style.cursor = 'hand';
         element.style.textDecoration = 'underline';
     }

     function setMouseOutColor(element) {
         element.style.backgroundColor = oldgridSelectedColor;
         element.style.textDecoration = 'none';
     }

     function ClickRow(objRef) {
         if (seletedRow == null) {
             EnableButton(false);
         }
         if (objRef == seletedRow)
             return;
         else if (seletedRow != null)
             seletedRow.style.backgroundColor = "#ffffff";

         objRef.style.backgroundColor = '#ADD8E6';
         objRef.focus();

         seletedRow = objRef;

         var hiddenSelect = document.querySelector("input[id*=hiddenSelectedRowIndex]");
         hiddenSelect.value = seletedRow.rowIndex;
     }
     function UpDownDeleteRow(msg, msgConfirm) {
         if (seletedRow != null) {
             if (msgConfirm == "" || confirm(msgConfirm) == true) {
                 seletedRow = null;
                 return true;
             }
             return false;
         }
         else {
             alert(msg);
             return false;
         }
     }
     function ValidateText(i) {
         if (i.value.length > 0) {
             i.value = i.value.replace(/[^\d]+/g, '');
         }
     }
     function ValidateFloat(i) {
         if (i.value.length > 0) {
             i.value = i.value.replace(/[^\d.]+/g, '');
         }
     }
     function EnableButton(enable) {
         var enableAddRow = document.getElementById('<%=buttonAddRow.ClientID%>').disabled;
         if (enableAddRow == false) {
             var btnUp = document.getElementById('<%=buttonUpRow.ClientID%>');
             btnUp.disabled = enable;

             var btnDown = document.getElementById('<%=buttonDownRow.ClientID%>');
             btnDown.disabled = enable;

             var btnDelete = document.getElementById('<%=buttonDeleteRow.ClientID%>');
             btnDelete.disabled = enable;
         }
     }
     function ConfirmGoPage(msgConfirm) {
         return confirm(msgConfirm);
     }
</script>

<div id="divPageFirst">
    <asp:UpdatePanel ID="updatePanelOrderInput" runat="server">
        <ContentTemplate>
            <table style=" padding-bottom: 5px;">
                <tr>
                    <td style="width: 80px">
                        <asp:Label ID="lblOrderNo" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server" Style="width: 120px" Enabled = "false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblOrderDate" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderDate" runat="server" Style="width: 120px" Enabled = "false"></asp:TextBox>
                        <asp:HyperLink ID="hplOrderDate" runat="server" ImageUrl="~/Styles/images/calendar.png"
                            ToolTip="Select on Calendar" Enabled = "False"></asp:HyperLink>
                    </td>

                    <td>
                        <asp:Label ID="lblDueDate" runat="server" CssClass="lable_input"></asp:Label>
                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtDueDate" runat="server" Style="width: 120px" Enabled = "false"></asp:TextBox>
                        <asp:HyperLink ID="hplDueDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" Enabled = "False"></asp:HyperLink> 
                    </td>
                   
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblClinicName" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                          <asp:TextBox ID="txtClinicNm" runat="server"  Style="width: 200px" Enabled = "false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblPatient" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPatientNm" runat="server"  Style="width: 200px" Enabled = "false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblBorrowPart" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBorrowPart" runat="server" TextMode="MultiLine" Width="200px"  Height="60px" Enabled = "false"></asp:TextBox>
                    </td>
                    <td valign="top">
                        <asp:Label ID="lblComments" runat="server" CssClass="lable_input"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComments" runat="server" CssClass="OrderInputTextEntry" TextMode="MultiLine"
                            Height="60px" Width="200px" Enabled = "false"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <asp:GridView runat="server" ID="gridViewOrderDetail" 
                AutoGenerateColumns="false" CssClass="gridView"
                ShowHeader="false" OnRowCreated="gridViewOrderDetail_RowCreated"   
                OnRowDataBound="gridViewOrderDetail_RowDataBound" 
                OnSelectedIndexChanged="gridViewOrderDetail_SelectedIndexChanged"  
                Width="870px">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" Visible="False" />
                    <asp:BoundField DataField="BridgedID"></asp:BoundField>
                    <asp:BoundField DataField="DetailNm"></asp:BoundField>
                    <asp:BoundField DataField="ToothNumber"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                                <%#ShowCheckedStringValue(Eval("ChildFlg").ToString())%>
                            </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" > </ItemStyle>
                     </asp:TemplateField>

                     <asp:TemplateField>
                        <ItemTemplate>
                                <%#ShowCheckedStringValue(Eval("GapFlg").ToString())%>
                            </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" > </ItemStyle>
                     </asp:TemplateField>
                      
                    <asp:BoundField DataField="ProsthesisNm"></asp:BoundField>
                    <asp:BoundField DataField="MaterialNm"></asp:BoundField>
                    <asp:BoundField DataField="Shape"></asp:BoundField>
                    <asp:BoundField DataField="Shade"></asp:BoundField>
                </Columns>
                <RowStyle CssClass="tr_body" />
                  <SelectedRowStyle BackColor="#CCCCFF" BorderColor="#003399" />
            </asp:GridView>
            <asp:Panel ID="panelDetail" runat="server"> 
                   <table style="padding-top: 10px;">
                          <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblMaterial" runat="server" CssClass="lable_input"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMaterialCd" runat="server" Style="width: 45px" 
                                        AutoPostBack="True" ontextchanged="txtMaterialCd_TextChanged"></asp:TextBox>
                                    <asp:DropDownList ID="DropDownMaterial" runat="server" Width="200px" Height="22px" AutoPostBack="True"  onselectedindexchanged="DropDownMaterial_SelectedIndexChanged"> </asp:DropDownList>
                                </td>
                           </tr>

                          <tr>
                                <td>
                                    <asp:Label ID="lblStock" runat="server" CssClass="lable_input"></asp:Label>
                                </td>
                                <td>
                                   <asp:DropDownList ID="dropDownStock" runat="server" Style="width: 252px" 
                                       Height="22px" AutoPostBack="True" onselectedindexchanged="dropDownStock_SelectedIndexChanged">
                                      </asp:DropDownList>
                                </td>
                                <td> 
                                     <asp:DropDownList ID="dropDownUnit" runat="server" Style="width: 70px" Height="22px"
                                          AutoPostBack="True" onselectedindexchanged="dropDownUnit_SelectedIndexChanged">
                                      </asp:DropDownList>
                                      <asp:TextBox ID="txtAmount" runat="server" Style="width: 70px"  AutoPostBack="True" ontextchanged="txtAmount_TextChanged"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="RequiredFloatNumber_Amount"
                                ControlToValidate="txtAmount" ForeColor="Red" ValidationExpression="[0-9]*\.?[0-9]*" ValidationGroup="RegisterGroup" ErrorMessage="*">
                               </asp:RegularExpressionValidator>
                                </td>

                                <td>
                                   <asp:Label ID="lblMaterialPrice" runat="server" CssClass="lable_input"></asp:Label>
                                   <asp:TextBox ID="txtMaterialPrice" runat="server" Style="width: 115px" 
                                        Enabled="False"></asp:TextBox>
                                      <asp:Label ID="lblUnitPrice" runat="server" CssClass="lable_input"></asp:Label>
                               </td>
                        
                           </tr>

                             <tr>
                            <td>
                             <asp:Label ID="lblManufactureStaff" runat="server" CssClass="lable_input" 
                Text="Manufacture Staff"></asp:Label>
                            </td>
                            <td>
                              <asp:TextBox ID="txtManufactureStaff" runat="server" Width="45px" AutoPostBack="True" 
                                    OnTextChanged="txtManufactureStaff_TextChanged"></asp:TextBox>
                              <asp:DropDownList ID="DropDownManufactureStaff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownManufactureStaff_SelectedIndexChanged"
                                    Width="155px" Height="22px">
                                </asp:DropDownList>
                            </td>
                        </tr>

                           <tr>
                            <td>
                                <asp:Label ID="lblCompleteDate" runat="server" CssClass="lable_input"></asp:Label>  
                            </td>
                             <td>
                                  <asp:TextBox ID="txtCompleteDate" runat="server" Width="120px"></asp:TextBox>
                                  <asp:HyperLink ID="hplCompleteDate" runat="server" 
                                      ImageUrl="~/Styles/images/calendar.png"></asp:HyperLink>
                                  <asp:CompareValidator ID="RequiredDateCompleteDate" runat="server" ControlToValidate="txtCompleteDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">*</asp:CompareValidator>
                            </td>

                            <td style="text-align:right" >
                             <asp:Label ID="lblInspectionStaff" runat="server" CssClass="lable_input" Text="Inspection Staff"></asp:Label>
                            </td>
                            <td>
                              <asp:TextBox ID="txtStaffCd" runat="server" Width="45px" AutoPostBack="True" 
                                    OnTextChanged="txtStaffCd_TextChanged"></asp:TextBox>
                              <asp:DropDownList ID="DropDownStaff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownStaff_SelectedIndexChanged"
                                    Width="155px" Height="22px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                  </table>
            </asp:Panel>

            <div style="padding-top: 5px;"> 
                <asp:RadioButton ID="optInternal" runat="server" 
                    GroupName="optInternalOutsource"  AutoPostBack="True" 
                    oncheckedchanged="optInternal_CheckedChanged" Checked="True"/>
                 <asp:Panel id="panelProcess" runat="server">
                     <table style="padding-left: 20px;">
                          <tr> 
                              <td>
                                   <asp:Label ID="lblGridProcess" runat="server" CssClass="lable_input"></asp:Label>
                              </td> 
                              <td colspan="2">
                                  <asp:gridview id="gridViewProcess" CssClass="gridView" runat="server" autogeneratecolumns="false" ShowHeader="false" showfooter="true"
                                    OnRowCreated = "gridViewProcess_RowCreated"
                                    onrowcancelingedit="gridViewProcess_RowCancelingEdit"
                                    onrowediting="gridViewProcess_RowEditing"
                                    onrowdeleting="gridViewProcess_RowDeleting"
                                    onrowupdating="gridViewProcess_RowUpdating" 
                                    OnRowDataBound="gridViewProcess_RowDataBound">
		                            <columns>
			                             <asp:templatefield>
				                             <itemtemplate>
				                            	<asp:linkbutton id="btnEdit" runat="server" causesvalidation="false" commandname="Edit" text='<%# GetResource("ActionEdit.Text") %>' />
				                            </itemtemplate>
				                            <edititemtemplate>
					                           <asp:linkbutton id="btnUpdate" runat="server" commandname="Update" text='<%# GetResource("ActionUpdate.Text") %>'/>
					                           <asp:linkbutton id="btnCancel" runat="server" causesvalidation="false" commandname="Cancel" text='<%# GetResource("ActionCancel.Text") %>'  />
				                            </edititemtemplate>
			                            </asp:templatefield>

			                             <asp:templatefield headertext="プロセス名">
				                          <itemtemplate><%# Eval("ProcessName") %>
				                          </itemtemplate>
				                          <edititemtemplate>
				                            	<asp:textbox id="txtProcess" runat="server" text='<%# Bind("ProcessCd") %>' Width="60px" OnTextChanged="textBox_OnTextChanged"  AutoPostBack="true"/>
                                                <asp:DropDownList ID="DropDownProcess" runat="server" Width="180px" Height="22px" TabIndex="-1" OnSelectedIndexChanged ="dropDown_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
				                          </edititemtemplate>
                                         <footertemplate>
					                            <asp:textbox id="txtProcess" runat="server" text='<%# Bind("ProcessCd") %>' Width="60px" OnTextChanged="textBox_OnTextChanged"  AutoPostBack="true"/>
                                                <asp:DropDownList ID="DropDownProcess" runat="server" Width="180px" Height="22px" OnSelectedIndexChanged ="dropDown_SelectedIndexChanged"  AutoPostBack="true" TabIndex="-1"></asp:DropDownList>
				                          </footertemplate>
			                           </asp:templatefield>

			                             <asp:templatefield headertext="担当者">
				                            <itemtemplate>
					                            <%# Eval("StaffNm") %>
				                            </itemtemplate>
				                            <edititemtemplate>
					                            <asp:textbox id="txtStaff" runat="server" text='<%# Bind("StaffCd") %>' Width="60px" OnTextChanged="textBox_OnTextChanged" AutoPostBack="true"/>
                                                <asp:DropDownList ID="DropDownStaff" runat="server"  Width="180px" Height="22px" OnSelectedIndexChanged ="dropDown_SelectedIndexChanged" AutoPostBack="true" TabIndex="-1"></asp:DropDownList>
				                            </edititemtemplate>
				                           <footertemplate>
					                            <asp:textbox id="txtStaff" runat="server" text='<%# Bind("StaffCd") %>' Width="60px" OnTextChanged="textBox_OnTextChanged" AutoPostBack="true"/>
                                                <asp:DropDownList ID="DropDownStaff" runat="server" Width="180px" Height="22px" OnSelectedIndexChanged ="dropDown_SelectedIndexChanged"  AutoPostBack="true" TabIndex="-1"></asp:DropDownList>
				                            </footertemplate>
			                           </asp:templatefield>
                                         
			                             <asp:templatefield headertext="作業時間">
				                            <itemtemplate>
					                            <%# Eval("ProcessTime")%>
				                            </itemtemplate>
				                            <edititemtemplate>
					                            <asp:textbox id="txtProcessTime" runat="server" text='<%# Bind("ProcessTime") %>' onkeyup ="ValidateText(this);" Width="80px"/>
				                            </edititemtemplate>
				                            <footertemplate>
					                            <asp:textbox id="txtProcessTime" runat="server" text='<%# Bind("ProcessTime") %>' onkeyup ="ValidateText(this);" Width="80px"/>
				                            </footertemplate>
			                            </asp:templatefield>

                                         <asp:templatefield headertext="ID" Visible="false">
				                            <itemtemplate>
					                            <%# Eval("ID")%>
				                            </itemtemplate>
                                        </asp:templatefield>

                                         <asp:BoundField DataField="ProcessCd"></asp:BoundField>
                                         <asp:BoundField DataField="StaffCd"></asp:BoundField>
		                            </columns>

                                    <RowStyle CssClass="tr_body" />
                                    <SelectedRowStyle BackColor="#33CC33" BorderColor="#FF6600" />
                                    <FooterStyle Height="50px" />
                                    <HeaderStyle CssClass="td_header" />
	                              </asp:gridview>
                              </td>
                              <td rowspan ="4" colspan="1">
                                <asp:Button ID="buttonAddRow" runat="server" Text="Add" onclick="btnAdd_Click"  style="margin-bottom:4px;"></asp:Button>
                                  <br />
                                <asp:Button ID="buttonUpRow" runat="server" Text="Up" onclick="btnUp_Click"  style="margin-bottom:4px;"></asp:Button>
                                  <br />
                                <asp:Button ID="buttonDownRow" runat="server" Text="Down"  onclick="btnDown_Click" style="margin-bottom:4px;" ></asp:Button>
                                  <br />
                                <asp:Button ID="buttonDeleteRow" runat="server" Text="Delete" onclick="btnDelete_Click" style="margin-bottom:4px;" />
                              </td>
                         </tr>

                          <tr>
                                <td>
                                    <asp:Label ID="lblTotalWorkTime" runat="server" CssClass="lable_input"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalWorkTime" runat="server"
                                        Enabled = "false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblFullWorkTime" runat="server" CssClass="lable_input"></asp:Label>
                                    <asp:TextBox ID="txtFullWorkTime" runat="server" Enabled = "false"></asp:TextBox>
                                </td>
                        </tr>
                </table>

                 </asp:Panel>

                 <asp:RadioButton ID="optOutsource" runat="server" 
                    GroupName="optInternalOutsource"  
                    oncheckedchanged="optOutsource_CheckedChanged" AutoPostBack="True" />
             
                   <asp:Panel id="panelOutsource" runat="server" Enabled="false">
                     <table style="padding-left: 20px;">
                         <tr>
                            <td>
                              <asp:Label ID="lblOutsourceLab" runat="server" CssClass="lable_input"></asp:Label>
                            </td>
                             <td>
                                <asp:TextBox ID="txtOutsourceLabCd" runat="server" Width="45px" AutoPostBack="True" 
                                     OnTextChanged="OutsourceLabCd_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="DropDownOutsourceCompany" runat="server"  Width="180px" Height="22px"
                                    AutoPostBack="true" 
                                    OnSelectedIndexChanged ="DropDownOutsourceCompany_SelectedIndexChanged"> </asp:DropDownList>
                            </td>
                       </tr>

                         <tr>
                            <td>
                                <asp:Label ID="lblOutsourceDate" runat="server" CssClass="lable_input"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOutsourceDate" runat="server" Style="width: 120px"></asp:TextBox>
                                  <asp:HyperLink ID="hplOutsourceDate" runat="server" ImageUrl="~/Styles/images/calendar.png"
                                    ToolTip="Select on Calendar"></asp:HyperLink>
                                 <asp:CompareValidator ID="RequiredDateOutsource" runat="server" ControlToValidate="txtOutsourceDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">*</asp:CompareValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblReceiveEstimateDate" runat="server" CssClass="lable_input"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiveEstimateDate" runat="server" Style="width: 120px"></asp:TextBox>
                                  <asp:HyperLink ID="hplReceiveEstimateDate" runat="server" ImageUrl="~/Styles/images/calendar.png"
                                    ToolTip="Select on Calendar"></asp:HyperLink>
                                  <asp:CompareValidator ID="RequiredDateReceiveEstimate" runat="server" ControlToValidate="txtReceiveEstimateDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">*</asp:CompareValidator>
                            </td>
                        </tr>

                         <tr>
                            <td>
                              <asp:Label ID="lblReceiveDate" runat="server" CssClass="lable_input"></asp:Label>
                            </td>
                            <td>
                              <asp:TextBox ID="txtReceiveDate" runat="server" Style="width: 120px"></asp:TextBox>
                              <asp:HyperLink ID="hplReceiveDate" runat="server" ImageUrl="~/Styles/images/calendar.png"
                            ToolTip="Select on Calendar"></asp:HyperLink>
                             <asp:CompareValidator ID="RequiredDateReceiveDate" runat="server" ControlToValidate="txtReceiveDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" ValidationGroup="RegisterGroup">*</asp:CompareValidator>
                           </td>

                            <td>
                              <asp:Label ID="lblTechPrice" runat="server" CssClass="lable_input"></asp:Label>
                            </td>
                           <td>
                             <asp:TextBox ID="txtTechPrice" runat="server" Style="width: 120px"></asp:TextBox>
                              <asp:Label ID="lblTechPriceUnit" runat="server" CssClass="lable_input"></asp:Label>
                           </td>
                       </tr>
                      </table>
                   </asp:Panel>
            </div>

            <asp:HiddenField ID="hiddenOrderSeq" runat="server" />
            <asp:HiddenField ID="hiddenOfficeCd" runat="server" />
            <asp:HiddenField ID="hiddenSupplierCd" runat="server" />
            <asp:HiddenField ID="hiddenProsthesisCd" runat="server" Value="" />
            <asp:HiddenField ID="hiddenSelectedRowIndex" runat="server" Value="" />
            <asp:HiddenField ID="hiddenBeforeRowDetail" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
   
    <p>   
        &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red"  ValidationGroup="RegisterGroup"/>
        <div style="text-align: right; padding: 5px 15px 25px 0;">

                <asp:Button ID="btnRegister" CssClass="MasterManagementButton" runat="server" Text="Register"
                  ValidationGroup="RegisterGroup"  OnClick="btnRegister_Click"  />
                <asp:Button ID="btnGoTechPrice" CssClass="MasterManagementButton" 
                    runat="server" Text="Go TechPrice" onclick="btnGoTechPrice_Click" />
                <asp:Button ID="btnGoOrder" CssClass="MasterManagementButton" 
                    runat="server" Text="Go Order" onclick="btnGoOrder_Click" />
                <asp:Button ID="btnCancel" CssClass="MasterManagementButton" runat="server"
                    Text="Cancel" OnClick="btnBack_Click"  />
        </div>
</div>
</asp:Content>

