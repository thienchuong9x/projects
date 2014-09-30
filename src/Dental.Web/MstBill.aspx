<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstBill.aspx.cs" Inherits="MstBill" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:UpdatePanel ID="UpdateBank" runat="server">
<ContentTemplate>
<asp:Panel ID="PnView" runat="server">
    <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input" 
        Visible="False"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="lbCode" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbName" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td></td>        
            <td>
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click"    Text="" />
                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click"      Text="" />               
            </td>
        </tr>
    </table>

    <asp:GridView ID="grvBill" runat="server" AutoGenerateColumns="False"  CssClass="gridView"       
            style="margin-top:10px" BCellPadding="4" 
        onrowcreated="grvBill_RowCreated" showHeader="False" DataKeyNames="BillCd" 
        AllowPaging="True" onpageindexchanging="grvBill_PageIndexChanging" >        
        <Columns>     
            <asp:TemplateField>
                    <ItemTemplate><center>                                          
                        <asp:CheckBox ID="CheckEdit" runat="server" AutoPostBack="false" /> </center>                       
                    </ItemTemplate><ItemStyle Width="20px" HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>         
            <asp:BoundField DataField="BillCd"> <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillNm"  > <ItemStyle Width="200px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillStatementNm" ></asp:BoundField>
            <asp:BoundField DataField="BillPostalCd" > <ItemStyle Width="80px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillAddress1" > <ItemStyle Width="200px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillAddress2" > <ItemStyle Width="200px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillTEL" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillFAX" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BillContactPerson" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="CreditLimit" ></asp:BoundField>
            <asp:BoundField DataField="BillFlg" ></asp:BoundField>   
            <asp:BoundField DataField="BillClosingDay" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField> 
            <asp:BoundField > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField> 
            <asp:BoundField > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField> 
            <asp:BoundField DataField="BankCd" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>  
            <asp:BoundField DataField="SupplierCd" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>                  
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" /> 
        <RowStyle CssClass="tr_body" />
    </asp:GridView>    
    <p style="margin-top: 15px; text-align: left">    
    <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click" />        
        <asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" />       
        <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
    </p>
   </asp:Panel>
<asp:Panel ID="PnEdit" runat="server" Visible="False" >  
    <table>
        <tr>
            <td>
                <asp:Label ID="lbDentalBillCd" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillCd" runat="server" Width="80px" MaxLength="10" ></asp:TextBox>
                </td>
            <td >
                &nbsp;<asp:CompareValidator ID="Val_BilCd" runat="server" 
                    ControlToValidate="txtDentalBillCd" ForeColor="Red" Operator="DataTypeCheck" 
                    Type="Integer" ValidationGroup="Bill" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="ValRe_BillCd" runat="server" 
                    ControlToValidate="txtDentalBillCd" ForeColor="Red" ValidationGroup="Bill"></asp:RequiredFieldValidator>
            </td>
                                  
        </tr>
        <tr>          
            <td >
                <asp:Label ID="lbDentalBillNm" runat="server" ></asp:Label>
            </td>            
            <td >
                <asp:TextBox ID="txtDentalBillNm" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
            </td>
            <td >
                &nbsp;<asp:RequiredFieldValidator ID="ValRe_BillNm" runat="server" 
                    ControlToValidate="txtDentalBillNm" ForeColor="Red" ValidationGroup="Bill"></asp:RequiredFieldValidator>
            </td>
                        
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillStatementNm" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillStatementNm" runat="server" Width="300px" 
                    MaxLength="40"></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillPostalCd" runat="server"  ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillPostalCd" runat="server" Width="80px" 
                    MaxLength="8"></asp:TextBox>
            </td>
            <td >
                <asp:RegularExpressionValidator ID="REV_PostalCd" runat="server" 
                    ControlToValidate="txtDentalBillPostalCd" Display="Dynamic" ForeColor="Red" 
                    ValidationExpression="\d{3}(-(\d{4}))?" ValidationGroup="Bill"></asp:RegularExpressionValidator>
            </td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillAddress1" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillAddress1" runat="server" MaxLength="20" 
                    Width="300px" ></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillAddress2" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillAddress2" runat="server" MaxLength="20" 
                    Width="300px" ></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillTel" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillTel" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalBillFax" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBillFax" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbBillContactPerson" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtBillContactPerson" runat="server" MaxLength="20" 
                    Width="150px"></asp:TextBox>
            </td>
            <td >
                &nbsp;</td>
            
        </tr>        
        <tr>       
            <td >
                <asp:Label ID="lbDentalCreditLimit" runat="server" ></asp:Label>
            </td>            
            <td >
                <asp:TextBox ID="txtDentalCreditLimit" runat="server" Width="150px"></asp:TextBox>
            </td>
            <td >                
                &nbsp;<asp:CompareValidator ID="Val_CreditLimit" runat="server" 
                    ControlToValidate="txtDentalCreditLimit" ForeColor="Red" 
                    Operator="DataTypeCheck" Type="Double" ValidationGroup="Bill"></asp:CompareValidator>
                </td>
                       
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbDentalBillFlg" runat="server" ></asp:Label>
            </td>           
            <td >
                <asp:DropDownList ID="DropDownListBillFlg" runat="server" Width="155px" 
                    Height="22px">
                </asp:DropDownList>
            </td> 
            <td >
                &nbsp;</td>
                         
        </tr>        
        <tr>
            <td>
                <asp:Label ID="lbBillClosingDay" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListBillClosingDay" runat="server" Width="155px" 
                    Height="22px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="ValRe_BillClosingDay" runat="server" 
                    ControlToValidate="DropDownListBillClosingDay" ForeColor="Red" 
                    ValidationGroup="Bill" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
         <tr>
            <td>
                <asp:Label ID="lbBankAccount" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListBankAccount" runat="server" Width="155px" 
                    Height="22px">
                </asp:DropDownList>
            </td>            
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbSupplierNm" runat="server"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListSupplier" runat="server" Width="155px" 
                    Height="22px">
                </asp:DropDownList>
            </td>            
        </tr>
    </table>
    <p style="text-align: left; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click"  ValidationGroup="Bill"/>
        <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" />       
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />  
    </p>   
    
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>




