<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Deposits.aspx.cs" Inherits="Deposits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <script language="javascript" type="text/javascript">

        function AddTotal() {

            var grid = document.getElementById('<%=grvMoneyReceive.ClientID %>');
            var col1;
            var totalcol1 = 0;
            var col2;
            var totalcol2 = 0;
            var col3;
            var col4;

            for (i = 0; i < grid.rows.length; i++) {
                col1 = grid.rows[i].cells[7];
                col2 = grid.rows[i].cells[8];
                col3 = grid.rows[i].cells[9];
                col4 = grid.rows[i].cells[6];

                for (j = 0; j < col1.childNodes.length; j++) {
                    if (col1.childNodes[j].type == "text") {
                        if (isNaN(col1.childNodes[j].value) && isNaN(col2.childNodes[j].value) || (!parseFloat(col1.childNodes[j].value) && !parseFloat(col2.childNodes[j].value))) {
                            col3.childNodes[j].value = "";
                        }
                        else {
                            if (parseFloat(col1.childNodes[j].value) > parseFloat(col4.innerHTML)) {
                                col1.childNodes[j].value = "0";
                                var lblObj = document.getElementById('<%= TextBox1.ClientID %>').value;
                                lblObj = lblObj.replace('@', "\n");
                                alert(lblObj);
                            }
                            if (!parseFloat(col1.childNodes[j].value) && parseFloat(col2.childNodes[j].value)) {
                                col3.childNodes[j].value = parseFloat(col2.childNodes[j].value);
                            }
                            else if (parseFloat(col1.childNodes[j].value) && !parseFloat(col2.childNodes[j].value)) {
                                col3.childNodes[j].value = parseFloat(col1.childNodes[j].value);
                            }
                            else if (parseFloat(col1.childNodes[j].value) && parseFloat(col2.childNodes[j].value)) {
                                col3.childNodes[j].value = parseFloat((parseFloat(col1.childNodes[j].value) + parseFloat(col2.childNodes[j].value)).toFixed(10));
                            }
                            if (!isNaN(col2.childNodes[j].value) && col2.childNodes[j].value != "")
                                totalcol2 = parseFloat((parseFloat(totalcol2) + parseFloat(col2.childNodes[j].value)).toFixed(10));
                            if (!isNaN(col1.childNodes[j].value) && col1.childNodes[j].value != "")
                                totalcol1 = parseFloat((parseFloat(totalcol1) + parseFloat(col1.childNodes[j].value)).toFixed(10));
                        }
                    }
                }
                document.getElementById('<%= txtPaymentAmount.ClientID %>').value = totalcol1;
                document.getElementById('<%= txtServiceCharge.ClientID %>').value = totalcol2;
                document.getElementById('<%= txtTotalPayment.ClientID %>').value = parseFloat((totalcol1 + totalcol2).toFixed(10));
            }
            document.getElementById('<%= txtPaymentAmount.ClientID %>').value = totalcol1;
            document.getElementById('<%= txtServiceCharge.ClientID %>').value = totalcol2;
            document.getElementById('<%= txtTotalPayment.ClientID %>').value = parseFloat((totalcol1 + totalcol2).toFixed(10));
        }

        function focusobj(value) {
            document.getElementById(value).style.backgroundColor = "";
        }

        function blurobj(value) {
            document.getElementById(value).style.backgroundColor = "";
        }

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


        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            var GridView = document.getElementById("<%=grvMoneyReceive.ClientID %>");
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
            var GridView = document.getElementById("<%=grvMoneyReceive.ClientID %>");
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
    
   </script>   
    
<style type="text/css">
    .style1
    {
        height: 36px;
    }
</style>
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input" 
        Visible="False" ></asp:Label>    
      <asp:Panel ID="Panel1" runat ="server" >
            <asp:Panel ID="pnSearch" runat="server" BorderColor="Silver" BorderWidth="0px">
                <table>  
                    <tr >                        
                        <td >
                            <asp:Label ID="lblBillStatementNo" runat="server"></asp:Label>
                        </td>                        
                        <td>                          
                            <asp:TextBox ID="txtBillStatementNo" runat="server" Width="100px"></asp:TextBox>
                        </td>                        
                    </tr>                          
                    <tr>
                        <td>
                            <asp:Label ID="lbIssueDate" runat="server">Issue Date</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIssueDateFrom" runat="server" TabIndex="1" Width="80px"></asp:TextBox>
                            <asp:HyperLink ID="hplIssueDateFrom" runat="server" 
                                ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:Label ID="lb" runat="server" Font-Bold="True" Font-Size="Medium">~</asp:Label>
                            <asp:TextBox ID="txtIssueDateTo" runat="server" TabIndex="2" Width="80px"></asp:TextBox>
                            <asp:HyperLink ID="hplIssueDateTo" runat="server" 
                                ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="Val_IssueDateFrom" runat="server" 
                                ControlToValidate="txtIssueDateFrom" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="Search"></asp:CompareValidator>
                            <asp:CompareValidator ID="Val_IssueDateTo" runat="server" 
                                ControlToValidate="txtIssueDateTo" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="Search"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>                        
                        <td >
                            <asp:Label ID="lbPayerCd" runat="server">Payer</asp:Label>
                        </td>                        
                        <td>
                            <asp:TextBox ID="txtPayerCd" runat="server" Width="80px" 
                                TabIndex="3" AutoPostBack="True" ontextchanged="txtPayerCd_TextChanged" ></asp:TextBox>
                            <asp:DropDownList ID="DropDownPayer" runat="server" AutoPostBack="True" 
                                Width="180px" onselectedindexchanged="DropDownPayer_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>                        
                    </tr>
                    <tr >                        
                        <td >
                            <asp:Label ID="lbPayMentDate" runat="server">PayMent Date</asp:Label>
                        </td>                        
                        <td >
                            <asp:TextBox ID="txtPayDateFrom" runat="server" Width="80px" TabIndex="4"></asp:TextBox>
                                <asp:HyperLink ID="hplPayMentDateFrom" runat="server" 
                                ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:Label ID="lb0" runat="server" Font-Bold="True" Font-Size="Medium">~</asp:Label>
                            <asp:TextBox ID="txtPayDateTo"   runat="server" Width="80px" TabIndex="5"></asp:TextBox>
                                <asp:HyperLink ID="hplPaymentDateTo" runat="server"  
                                ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="Val_PayDateFrom" runat="server" 
                                ControlToValidate="txtPayDateFrom" ForeColor="Red" Operator="DataTypeCheck" 
                                Type="Date" ValidationGroup="PaySearch" Display="Dynamic"></asp:CompareValidator>
                            <asp:CompareValidator ID="Val_PayDateTo" runat="server" 
                                ControlToValidate="txtPayDateTo" ForeColor="Red" Operator="DataTypeCheck" 
                                Type="Date" ValidationGroup="PaySearch" Display="Dynamic"></asp:CompareValidator>
                        </td>                       
                    </tr>
                    <tr>
                    <td >
                        <asp:Label ID="lbChkCompleted" runat="server"></asp:Label>
                    </td>
                    <td><asp:CheckBox ID="chkCompleted" runat="server" TabIndex="6" /></td>
                </tr>
                    <tr >
                        <td >
                            &nbsp;&nbsp;</td>                        
                        <td >
                            <asp:Button ID="btnSearch" runat="server" 
                                TabIndex="7" Text="Search" ValidationGroup="PaySearch" 
                                onclick="btnSearch_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear"  TabIndex="8" 
                                onclick="btnClear_Click" />
                            <asp:Label ID="Label1" runat="server">&nbsp;&nbsp;</asp:Label>
                        </td>                       
                    </tr>                    
            </table> 
          </asp:Panel>          
              <asp:GridView ID="grvMoneyReceive" runat="server" AutoGenerateColumns="False"                        
                style="margin-top:15px" CssClass="gridView" AllowPaging="True" ShowHeader="False" 
                FooterStyle-BorderColor="Blue" FooterStyle-BackColor="white" 
                FooterStyle-ForeColor="#003399" onrowcreated="grvMoneyReceive_RowCreated" 
                onrowdatabound="grvMoneyReceive_RowDataBound" 
                onpageindexchanging="grvMoneyReceive_PageIndexChanging" >
                 <Columns> 
                        <asp:TemplateField>
                        <ItemTemplate>                                          
                            <center><asp:CheckBox ID="CheckEdit" onclick="Check_Click(this);" runat="server" 
                                    AutoPostBack="true" Text=" " oncheckedchanged="CheckEdit_Checked"/></center>                        
                        </ItemTemplate><ItemStyle Width="20px" HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>       
                        <asp:BoundField DataField="DentalOfficeNm"  > <ItemStyle Width="160px" HorizontalAlign=Left></ItemStyle></asp:BoundField>    
                        <asp:BoundField DataField="BillStatementNo"  > <ItemStyle Width="160px" HorizontalAlign=Left></ItemStyle></asp:BoundField>
                        <asp:BoundField DataField="BillIssueDate"  > <ItemStyle Width="100px" HorizontalAlign=Center></ItemStyle></asp:BoundField>                    
                        <asp:BoundField DataField="LastPayDate"  > <ItemStyle Width="150px" HorizontalAlign=Center></ItemStyle></asp:BoundField>                        
                        <asp:BoundField DataField="CurrentSumPrice"  > <ItemStyle Width="100px" HorizontalAlign=Right></ItemStyle></asp:BoundField> 
                        <asp:BoundField DataField="Balance"   > <ItemStyle Width="100px" HorizontalAlign=Right></ItemStyle></asp:BoundField> 
                        <asp:TemplateField >
                          <ItemTemplate>
                            <asp:TextBox ID="txtPayAmount" runat="server" AutoPostBack="false"  onkeyup="AddTotal()" onblur="blurobj(this.id)"
                                 Width="80px" TabIndex="5" style="text-align: right" ></asp:TextBox>                            
                            <asp:CompareValidator ID="Val_Amount" runat="server" 
                                ControlToValidate="txtPayAmount" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Double" ValidationGroup="Register"></asp:CompareValidator>                                        
                          </ItemTemplate>                         
                         <ItemStyle Width="120px" />                         
                        </asp:TemplateField>
                        <asp:TemplateField >
                          <ItemTemplate>
                            <asp:TextBox ID="txtSubtractFee" runat="server" AutoPostBack="false"  onkeyup="AddTotal()" onblur="blurobj(this.id)"
                                 Width="80px" TabIndex="5" style="text-align: right" ></asp:TextBox>                            
                            <asp:CompareValidator ID="Val_ServiceCharge" runat="server" 
                                ControlToValidate="txtSubtractFee" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Double" ValidationGroup="Register"></asp:CompareValidator>                                        
                          </ItemTemplate>
                         <ItemStyle Width="120px" />                         
                        </asp:TemplateField>
                        <asp:TemplateField >
                          <ItemTemplate>
                            <asp:TextBox ID="txtSumPayAmount" runat="server" AutoPostBack="false"  onfocus="focusobj(this.id)" onblur="blurobj(this.id)"
                                 Width="80px"  style="text-align: right" ReadOnly="true" ></asp:TextBox> 
                          </ItemTemplate>                          
                         <ItemStyle Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current payment date">
                        <ItemTemplate >
                            <asp:TextBox ID="txtCurrentPaymentDate" runat="server" Width="80px" TabIndex="11" style="text-align:center"></asp:TextBox>
                            <asp:HyperLink ID="hplCurrentPaymentDate" runat="server" 
                                ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
                            <asp:CompareValidator ID="Val_CurrentpayDate" runat="server" 
                                ControlToValidate="txtCurrentPaymentDate" Display="Dynamic" ForeColor="Red" 
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="Register"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="Val_ReqDateTime" runat="server" 
                                ControlToValidate="txtCurrentPaymentDate" Display="Dynamic" ForeColor="Red" 
                                ValidationGroup="Register"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <ItemStyle Width="200px" />
                    </asp:TemplateField>
                         <asp:BoundField DataField="BillSeq"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField> 
                         <asp:BoundField DataField="BillCd"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField>
                         <asp:BoundField DataField="BankCd"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField> 
                         <asp:BoundField DataField="Note"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField>  
                         <asp:BoundField DataField ="CurrentBillSumPrice" > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField> 
                         <asp:BoundField DataField="PayAmount"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField> 
                         <asp:BoundField DataField="SubtractFee"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField>                         
                         <asp:BoundField DataField="OfficeCd"  > <ItemStyle Width="10px" ></ItemStyle></asp:BoundField>
                         <asp:BoundField DataField="PayDateTime"  > <ItemStyle Width="80px" ></ItemStyle></asp:BoundField>  
                         <asp:BoundField  > <ItemStyle Width="80px" ></ItemStyle></asp:BoundField>                          
                        <asp:BoundField DataField="PaidMoney" />
                 </Columns>
                  <FooterStyle BackColor="White" BorderColor="Blue" ForeColor="#003399" />
                 <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                 Mode="NumericFirstLast" PageButtonCount="5" />
                 <PagerStyle CssClass="pagination" BackColor="White" /> 
                <RowStyle CssClass="tr_body" />
          </asp:GridView>          
          <asp:Label ID="lbMessage" runat="server"></asp:Label>  
                <div style="text-align: right; margin-top:15px; margin-left: 1px;" >                
                        <asp:TextBox id="txtPaymentAmount" onkeyup ="ValidateText(this);" 
                             runat="server" Width="90px"  BackColor="Silver" 
                            ReadOnly="True" style="text-align: right" ></asp:TextBox> 
                             
                        <asp:TextBox type="text" ID="txtServiceCharge" onkeyup ="ValidateText(this);" 
                             runat="server" Width="90px" BackColor="Silver" 
                            ReadOnly="True" style="text-align: right" ></asp:TextBox> 
                      
                        <asp:TextBox id ="txtTotalPayment" onfocus="ghi()"  readonly="true" runat="server"
                            Width="90px"  BackColor="Silver" style="text-align: right" ></asp:TextBox>                    
                        <asp:Label ID="Label2" runat="server" style="text-align: right;" Width="125px"></asp:Label>
                </div>  
                    <table>
                    <tr >                        
                        <td class="style1" >
                            &nbsp;<asp:Label ID="lbDepositAccount" runat="server">Deposit Account</asp:Label>
                        </td>
                        <td class="style1">
                            <asp:DropDownList ID="DropDownListAccount" runat="server"                             
                                TabIndex="7" Width="180px">
                            </asp:DropDownList>
                        </td>
                         <td class="style1" >
                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lbComment" runat="server">Comment</asp:Label>
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtComment" runat="server" MaxLength="99" 
                                TabIndex="8" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" Width="0px" 
                                Font-Size="0pt" ForeColor="White"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="Val_RegMaxlength" runat="server" 
                                ControlToValidate="txtComment" Display="Dynamic" ForeColor="Red" 
                                ValidationExpression="^[\s\S]{0,100}" ValidationGroup="Register"></asp:RegularExpressionValidator>
                        </td> 
                    </tr>                   
               </table>     
              <div  style="text-align: right; margin-top:15px" >                  
              <asp:Button ID="btnRegister" runat="server" TabIndex="9"
                       style="text-align: right;" Text="Register" ValidationGroup="Register" 
                      onclick="btnRegister_Click" />     
                  <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
              </div>   
 </asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

