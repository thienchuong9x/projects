<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Payments.aspx.cs" Inherits="Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

 <script language="javascript" type="text/javascript">
     function ValidateText(i) {
         if (i.value.length > 0) {
             i.value = i.value.replace(/[^\d\.]+/g, '');
         }
     }

     function ValidateTextNumber(i) {
         if (i.value.length > 0) {
             i.value = i.value.replace(/[^\d]+/g, '');
         }
     }
     function ValidateTextDate(i) {
         if (i.value.length > 0) {
             i.value = i.value.replace(/[^\d\/]+/g, '');
         }
     }

     function ChangedSellerCd() {
         var text = document.getElementById('<%=txtSellerCd.ClientID %>');
         var drop = document.getElementById('<%=DropDownListSeller.ClientID %>');         
         drop.value = text.value;
         if (drop.value == "") {
             text.value = "";
             text.focus();
         }       
     }
     function ChangedOutsoureCd() {
         var text = document.getElementById('<%=txtOutsoureCd.ClientID %>');
         var drop = document.getElementById('<%=DropDownListOutsourcelab.ClientID %>');         
         drop.value = text.value;
         if (drop.value == "") {
             text.value = "";
             text.focus();             
         }         
     }
     
     function AddTotal() {
         //alert('came');
         var grid = document.getElementById('<%=grvPayment.ClientID %>');
         var col1;
         var col2;
         var Total = 0;
         var total1 = 0;
         for (i = 0; i < grid.rows.length; i++) {
             col1 = grid.rows[i].cells[6];
             col2 = grid.rows[i].cells[7];

             for (j = 0; j < col2.childNodes.length; j++) {
                 if (col2.childNodes[j].type == "text") {
                     if (isNaN(col2.childNodes[j].value) || !parseFloat(col2.childNodes[j].value)) {
                         document.getElementById('<%= txtTotalAmount.ClientID %>').value = "";
                     }
                     else {
                         if (parseFloat(col2.childNodes[j].value) > parseFloat(col1.innerHTML)) {
                             total1++;
                             var a = "<%=ResourceJavaScript()%>";
                             document.getElementById('<%= lnMessage.ClientID %>').innerHTML = a;
                             document.getElementById('<%=btnRegister.ClientID %>').disabled = true;
                         }
                         if (total1 == 0) {
                             document.getElementById('<%= lnMessage.ClientID %>').innerHTML = "";
                             document.getElementById('<%=btnRegister.ClientID %>').disabled = false;
                         }
                         Total = parseFloat((parseFloat(Total) + parseFloat(col2.childNodes[j].value)).toFixed(10));
                     }
                 }
             }
             document.getElementById('<%= txtTotalAmount.ClientID %>').value = Total;
         }
         document.getElementById('<%= txtTotalAmount.ClientID %>').value = Total;
     }

     function focusobj(value) {
         document.getElementById(value).style.backgroundColor = "";
     }

     function blurobj(value) {
         document.getElementById(value).style.backgroundColor = "";
     }

     function Check_Click(objRef) {
         //Get the Row based on checkbox
         var row = objRef.parentNode.parentNode;
         var GridView = document.getElementById("<%=grvPayment.ClientID %>");
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
         //AddTotal();     
     }
     function checkAll(objRef) {
         var GridView = document.getElementById("<%=grvPayment.ClientID %>");
         var inputList = GridView.getElementsByTagName("input");
         for (var i = 0; i < inputList.length; i++) {
             var row = inputList[i].parentNode.parentNode;
             if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                 if (objRef.checked) {
                     if (inputList[i].checked == false) {
                         inputList[i].checked = true;
                         //AddTotal();             
                     }
                 }
                 else {
                     if (inputList[i].checked == true) {
                         inputList[i].checked = false;
                         //AddTotal();                                    
                     }
                 }
             }
         }
     }
</script>  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate> 
<asp:Label ID="lblModuleTitle" runat="server"  Visible="False"></asp:Label>
    <asp:Panel ID="PnSearch" runat="server">      
        <table>               
                <tr>                    
                    <td >
                        <asp:Label ID="lbBuyDate" runat="server"></asp:Label>
                    </td>
                    <td>                         
                        <asp:TextBox ID="txtBuyDateFrom"  runat="server" Width="80px" TabIndex="1" ></asp:TextBox>
                            <asp:HyperLink ID="hplBuyDateFrom" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                        <asp:Label ID="lb" runat="server"  Font-Bold="True" Font-Size="Medium" >~</asp:Label>
                        <asp:TextBox ID="txtBuyDateTo" runat="server" Width="80px"   TabIndex="2" ></asp:TextBox>
                            <asp:HyperLink ID="hplBuyDateTo" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                        <asp:CompareValidator ID="Val_BuyDateFrom" runat="server" 
                            ControlToValidate="txtBuyDateFrom" Display="Dynamic" ForeColor="Red" 
                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Search"></asp:CompareValidator>
                        <asp:CompareValidator ID="Val_BuyDateTo" runat="server" 
                            ControlToValidate="txtBuyDateTo" Display="Dynamic" ForeColor="Red" 
                            Operator="DataTypeCheck" Type="Date" ValidationGroup="Search"></asp:CompareValidator>
                    
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbSellerCd" runat="server"></asp:Label>
                    </td>
                    <td>                   
                        <asp:TextBox ID="txtSellerCd" runat="server" 
                           Width="80px" TabIndex="3" onkeyup="ChangedSellerCd(this.id)"></asp:TextBox>
                        <asp:DropDownList ID="DropDownListSeller" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownListSeller_SelectedIndexChanged" 
                            Visible="True" Width="180px" >
                        </asp:DropDownList>                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbOutsoureLab" runat="server"></asp:Label>
                    </td>
                    <td>                   
                        <asp:TextBox ID="txtOutsoureCd" runat="server"
                           Width="80px" TabIndex="3" onkeyup="ChangedOutsoureCd(this.id)"></asp:TextBox>
                        <asp:DropDownList ID="DropDownListOutsourcelab" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownListOutsourcelab_SelectedIndexChanged" 
                            Visible="True" Width="180px" >
                        </asp:DropDownList>                        
                    </td>
                </tr>                
                <tr>
                    <td >
                        <asp:Label ID="lbChkCompleted" runat="server"></asp:Label>
                    </td>
                    <td><asp:CheckBox ID="chkCompleted" runat="server" TabIndex="6" /></td>
                </tr>
                <tr>
                <td style="text-align: left" class="style31"></td>
                <td><asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" 
                    Text="" ValidationGroup="Search" TabIndex="7" />
                    <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" Text="" TabIndex="8" />
                    <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
                </td>
                </tr>
        </table>         
        </asp:Panel>
        <asp:Panel ID="PnPayment" runat="server">
            <asp:GridView ID="grvPayment" runat="server" AutoGenerateColumns="False"                        
                style="margin-top:15px" CssClass="gridView" onrowcreated="grvPayment_RowCreated" 
                ShowHeader="False" AllowPaging="True"  
            onpageindexchanging="grvPayment_PageIndexChanging" CellPadding="8" 
            onrowdatabound="grvPayment_RowDataBound" >
                <Columns>                    
                    <asp:TemplateField><%--cell 0--%>
                        <ItemTemplate>                                          
                            <center><asp:CheckBox ID="CheckEdit" runat="server" onclick = "Check_Click(this);"  AutoPostBack="true" OnCheckedChanged="CheckEdit_Checked"  Text=" "/></center>                        
                        </ItemTemplate><ItemStyle Width="20px" HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField> 
                     
                    <asp:BoundField DataField="PurchaseDate" HeaderText="Buy Date"><%--cell 1--%>
                    <ItemStyle Width="90px" HorizontalAlign="Center"  /></asp:BoundField> 

                    <asp:BoundField  DataField ="" HeaderText="SupplierNm" ><%--cell 2--%>
                    <ItemStyle Width="250px"  HorizontalAlign="Left"  /></asp:BoundField>   

                    <asp:BoundField  DataField ="" HeaderText="OutsourceNm" ><%--cell 3--%>
                    <ItemStyle Width="250px"  HorizontalAlign="Left"  /></asp:BoundField>  
                    
                    <asp:BoundField DataField="PurchaseItems" HeaderText="PurchaseItem"><%--cell 4--%>
                    <ItemStyle Width="250px" HorizontalAlign="Left" /></asp:BoundField>                    

                    <asp:BoundField DataField="PurchasePrice" HeaderText="Amount"><%--cell 5--%>
                    <ItemStyle Width="80px" HorizontalAlign="Right" /></asp:BoundField>

                    <asp:BoundField DataField="Balance" HeaderText="Balance" ><%--cell 6--%>
                    <ItemStyle Width="80px" HorizontalAlign="Right" /></asp:BoundField>

                    <asp:TemplateField HeaderText="Current payment Amount"><%--cell 7--%>
                        <ItemTemplate>
                            <asp:TextBox ID="txtCurrentPaymentAmount" runat="server" AutoPostBack="false" onkeyup="AddTotal()"
                                 Width="80px" TabIndex="9" style="text-align:right"  ></asp:TextBox>                           
                            <asp:CompareValidator ID="Val_Amount" runat="server" 
                                ControlToValidate="txtCurrentPaymentAmount" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Double" ValidationGroup="Register"></asp:CompareValidator>                                                                     
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Service charge"><%--cell 8--%>
                        <ItemTemplate>
                            <asp:TextBox ID="txtServiceCharge" runat="server"  Width="70px" TabIndex="9" style="text-align:right" ></asp:TextBox>
                            <asp:CompareValidator ID="Val_ServiceCharge" runat="server" 
                                ControlToValidate="txtServiceCharge" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Double" ValidationGroup="Register"></asp:CompareValidator>                                                                  
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>                    

                    <asp:BoundField DataField="OfficeCd" HeaderText="Office Code cell 9"> <%--cell 9--%>
                    <ItemStyle Width="40px" Font-Size="Smaller" />
                    </asp:BoundField>

                    <asp:BoundField DataField="PurchaseSeq" HeaderText="Purchase Seq cell 10"><%--cell 10--%>
                    <ItemStyle Width="100px" Font-Size="Smaller" />
                    </asp:BoundField> 
                                        
                    <asp:BoundField DataField="SupplierOutsourceCd" HeaderText="Seller Code cell 11"><%--cell 11--%>
                    <ItemStyle Width="100px"  /></asp:BoundField>

                    <asp:BoundField DataField="PurchaseCategory" HeaderText="Seller Code cell 12"><%--cell 12--%>
                    <ItemStyle Width="100px"  /></asp:BoundField>

                    <asp:BoundField DataField="PaidMoney"></asp:BoundField><%--cell 13--%>
                    <asp:BoundField DataField="Note"></asp:BoundField><%--cell 14--%>
                </Columns>  
                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" />            
               <RowStyle CssClass="tr_body" />
            </asp:GridView>            
            
            <table style="margin-top:15px;">
                <tr>
                    <td>
                    <asp:Label ID="lbMessage" runat="server"></asp:Label> 
                    </td>
                    <td style="text-align: right; width:450px;" align="right">
                        <asp:Label ID="lnMessage" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr style="width: 910px;">
                    <td style="text-align: left; width:450px;" align="left" >
                    <div style="text-align: left; width:450px;"><asp:Label ID="lbBankDetails" runat="server" align="left"></asp:Label>
                        <asp:DropDownList ID="DropDownListBankDetails" runat="server" TabIndex="12" 
                           Width="280px" align="left">
                        </asp:DropDownList>
                    </div>
                    </td>                                                  
                    <td style="text-align: right; width:450px;" align="right">
                    <div style="text-align: right;">
                        <asp:Label ID="lbTotalAmount" runat="server" style="text-align: right;" 
                            ></asp:Label>
                        <asp:TextBox ID="txtTotalAmount" style="text-align: right;" runat="server" BackColor="#CCCCCC" 
                            ReadOnly="true" Width="80px"></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" style="text-align: right;" 
                        Width="45px"></asp:Label> 
                    </div>
                    </td>
                 </tr>
             </table>                      
                <p style="text-align:right; margin-top: 15px;">  
                    <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click" 
                        TabIndex="13" Text="" ValidationGroup="Register" />
                </p>
            
       </asp:Panel>
 </ContentTemplate>
 </asp:UpdatePanel> 
 </asp:Content>