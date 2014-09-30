<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OperationTechPrice.aspx.cs" Inherits="OperationTechPrice" %>

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
             //  i.value = i.value.replace(/[^\d.]+/g, '');
             i.value = i.value.replace(/[^-\d.]+/g, '');
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
          
            <asp:Panel ID ="panelTechPrice" runat="server" style="padding-top: 5px;"> 
                     <table>
                     
                          <tr> 
                              <td>
                                   <asp:Label ID="lblGridViewTechPrice" runat="server" CssClass="lable_input"></asp:Label>
                              </td> 
                              <td>
                                  <asp:gridview id="gridViewTechPrice" CssClass="gridView" runat="server" autogeneratecolumns="false" ShowHeader="false" showfooter="true" style="margin-top: 20px;" 
                                    OnRowCreated = "gridViewTechPrice_RowCreated"
                                    onrowcancelingedit="gridViewTechPrice_RowCancelingEdit"
                                    onrowediting="gridViewTechPrice_RowEditing"
                                    onrowdeleting="gridViewTechPrice_RowDeleting" 
                                    onrowupdating="gridViewTechPrice_RowUpdating" 
                                    OnRowDataBound="gridViewTechPrice_RowDataBound">
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

			                             <asp:templatefield headertext="加工コード">
				                          <itemtemplate><%# Eval("TechCd")%>
				                          </itemtemplate>
				                          <edititemtemplate>
                                                <asp:textbox id="txtTechCd" runat="server" text='<%# Bind("TechCd") %>' Width="80px"  OnTextChanged="textBoxRow_OnTextChanged" AutoPostBack="true"/>     
				                          </edititemtemplate>
                                         <footertemplate>
					                            <asp:textbox id="txtTechCd" runat="server" text='<%# Bind("TechCd") %>' Width="80px"  OnTextChanged="textBox_OnTextChanged" AutoPostBack="true"/>     
				                          </footertemplate>
			                           </asp:templatefield>

			                             <asp:templatefield headertext="加工名" >
				                            <itemtemplate>
					                            <%# Eval("TechNm")%>
				                            </itemtemplate>
				                            <edititemtemplate>
                                                <asp:DropDownList ID="DropDownTech" runat="server" Width="250px" Height="22px" OnSelectedIndexChanged ="DropDownTechRow_SelectedIndexChanged"  AutoPostBack="true" TabIndex="-1"></asp:DropDownList>
				                            </edititemtemplate>
				                           <footertemplate>
                                                <asp:DropDownList ID="DropDownTech" runat="server" Width="250px" Height="22px" OnSelectedIndexChanged ="DropDownTech_SelectedIndexChanged"  AutoPostBack="true" TabIndex="-1"></asp:DropDownList>
				                            </footertemplate>
			                           </asp:templatefield>

			                             <asp:templatefield headertext="価格">
				                            <itemtemplate>
					                            <%# Eval("TechPrice")%>
				                            </itemtemplate>
				                            <edititemtemplate>
					                            <asp:textbox id="txtTechPrice" runat="server" text='<%# Bind("TechPrice") %>'   onkeyup ="ValidateFloat(this);" Width="100px"/>
                                                  <asp:RegularExpressionValidator runat="server" ID="RequiredFloatNumber_TechPrice"
                                ControlToValidate="txtTechPrice" ForeColor="Red" ValidationExpression="-?[0-9]*\.?[0-9]*" ValidationGroup="RegisterGroup" ErrorMessage="*">
                               </asp:RegularExpressionValidator>
				                            </edititemtemplate>
				                            <footertemplate>
					                            <asp:textbox id="txtTechPrice" runat="server" text='<%# Bind("TechPrice") %>' onkeyup ="ValidateFloat(this);" Width="100px"/>
                                                  <asp:RegularExpressionValidator runat="server" ID="RequiredFloatNumber_TechPrice"
                                ControlToValidate="txtTechPrice" ForeColor="Red" ValidationExpression="-?[0-9]*\.?[0-9]*" ErrorMessage="*">
                               </asp:RegularExpressionValidator>
				                            </footertemplate>
			                            </asp:templatefield>

                                         <asp:templatefield headertext="TechDetailNo" Visible="false">
				                            <itemtemplate>
					                            <%# Eval("TechDetailNo")%>
				                            </itemtemplate>
                                        </asp:templatefield>

		                            </columns>

                                    <RowStyle CssClass="tr_body" />
                                    <SelectedRowStyle BackColor="#33CC33" BorderColor="#FF6600" />
                                    <FooterStyle Height="50px" />
                                    <HeaderStyle CssClass="td_header" />
	                              </asp:gridview>
                              </td>
                              <td rowspan ="4" colspan="1">
                                <asp:Button ID="buttonAddRow" runat="server" Text="Add" onclick="btnAdd_Click" style="margin-bottom:4px;"></asp:Button>
                                  <br />
                                <asp:Button ID="buttonUpRow" runat="server" Text="Up" onclick="btnUp_Click"  style="margin-bottom:4px;"></asp:Button>
                                  <br />
                                <asp:Button ID="buttonDownRow" runat="server" Text="Down"  onclick="btnDown_Click" style="margin-bottom:4px;" ></asp:Button>
                                  <br />
                                <asp:Button ID="buttonDeleteRow" runat="server" Text="Delete" onclick="btnDelete_Click" style="margin-bottom:4px;" />
                              </td>
                         </tr>

                          <tr>
                               <td></td>
                                <td align="right">
                                    <asp:Label ID="lblTotalCost" runat="server" CssClass="lable_input"></asp:Label>
                                    <asp:TextBox ID="txtTotalCost" runat="server" Enabled = "false"></asp:TextBox>
                                     <asp:Label ID="lblUnitPrice" runat="server" CssClass="lable_input"></asp:Label>
                                </td>
                           </tr>
                            <tr>
                               <td></td>
                                <td align="right">
                                    <asp:Label ID="lblFullCost" runat="server" CssClass="lable_input"></asp:Label>
                                    <asp:TextBox ID="txtFullCost" runat="server" Enabled = "false"></asp:TextBox>
                                    <asp:Label ID="lblFullUnitPrice" runat="server" CssClass="lable_input"></asp:Label>
                                </td>
                           </tr>
                </table>
            </asp:Panel>

            <asp:HiddenField ID="hiddenOrderSeq" runat="server" />
            <asp:HiddenField ID="hiddenOfficeCd" runat="server" />
            <asp:HiddenField ID="hiddenDentalOfficeCd" runat="server" />
            <asp:HiddenField ID="hiddenSelectedRowIndex" runat="server" Value="" />
            <asp:HiddenField ID="hiddenBeforeRowDetail" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
   


        <p style="text-align: right; padding: 5px 15px 5px 0;">
                <asp:Button ID="btnRegister" CssClass="MasterManagementButton" runat="server" Text="Register"
                    OnClick="btnRegister_Click"  ValidationGroup="RegisterGroup"/>
                 <asp:Button ID="btnGoProcess" CssClass="MasterManagementButton" runat="server" Text="Go Process"
                      onclick="btnGoProcess_Click"/>
                <asp:Button ID="btnGoOrder" CssClass="MasterManagementButton" 
                    runat="server" Text="Go Order" onclick="btnGoOrder_Click" />
                <asp:Button ID="btnCancel" CssClass="MasterManagementButton" runat="server"
                    Text="Cancel" OnClick="btnBack_Click" />
        </p>
    


</div>
</asp:Content>

