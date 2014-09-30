<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstBank.aspx.cs" Inherits="MstBank" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:UpdatePanel ID="UpdateBank" runat="server">
<ContentTemplate>
<asp:Panel ID="PnView" runat="server">
    <asp:Label ID="lblModuleTitle" runat="server" CssClass="lable_input" 
        Visible="False"></asp:Label>    
    <table>
        <tr>
            <td>
                <asp:Label ID="lbCode" runat="server" Text=""></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbAccount" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAccount" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
            </td>
        </tr>  
        <tr>
            <td>                
                </td>        
            <td>
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click"  Text=""/>
                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click"  Text="" />
            </td>
        </tr>        
                 
    </table>               
 
    
     
    <asp:GridView ID="grvBank" runat="server" AutoGenerateColumns="False" 
        style="margin-top:10px" DataKeyNames="BankCd" CssClass="gridView" 
        CellPadding="4" onrowcreated="grvBank_RowCreated" 
            ShowHeader="False" AllowPaging="True" 
        onpageindexchanging="grvBank_PageIndexChanging" >        
        <Columns>     
            <asp:TemplateField>
                    <ItemTemplate>                                          
                        <center><asp:CheckBox ID="CheckEdit" runat="server" AutoPostBack="false" Text=" "/></center>                        
                    </ItemTemplate><ItemStyle Width="20px" HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>         
            <asp:BoundField DataField="BankCd"  > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="BankAccount"  > <ItemStyle Width="250px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="AccountOwner"  > <ItemStyle Width="250px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="ForReceiveFlg"  > <ItemStyle Width="100px"  HorizontalAlign="Center"></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="ForpayFlg"  > <ItemStyle Width="100px"  HorizontalAlign="Center"></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="OfficeCd"  > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>           
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" /> 
        <RowStyle CssClass="tr_body" />
    </asp:GridView>    
    <p style="text-align: left; margin-top: 15px;">    
    <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click" />        
    <asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" />        
        <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
    </p>
    </asp:Panel>
<style type="text/css">    
    
    </style>
<asp:Panel ID="PnEdit" runat="server" Visible="False"> 
    <table>
        <tr>            
            <td >
                <asp:Label ID="lbDentalBankCd" runat="server"></asp:Label>
            </td>            
            <td >
                <asp:TextBox ID="txtDentalBankCd" runat="server" TabIndex="1" Width="80px" 
                    MaxLength="10" ></asp:TextBox>
                </td>
            <td >
                <asp:CompareValidator ID="valNumbersOnly_BankCd" runat="server" 
                    ControlToValidate="txtDentalBankCd" 
                    Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ForeColor="Red" 
                    ValidationGroup="BankAccount"></asp:CompareValidator>
                &nbsp;<asp:RequiredFieldValidator ID="valRequired_BankCd" runat="server" 
                    ControlToValidate="txtDentalBankCd" ForeColor="Red" 
                    ValidationGroup="BankAccount"></asp:RequiredFieldValidator>
            </td>
                                  
        </tr>
        <tr>             
            <td >
                <asp:Label ID="lbDentalBankAccount" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalBankAccount" runat="server" TabIndex="2" 
                    Width="200px" MaxLength="30"></asp:TextBox>
            </td>
            <td>
                &nbsp;<asp:RequiredFieldValidator ID="valRequired_BankAccount" runat="server" 
                    ControlToValidate="txtDentalBankAccount" ForeColor="Red" 
                    ValidationGroup="BankAccount"></asp:RequiredFieldValidator>
                    </td>
                        
        </tr>
         <tr>                
            <td>
                <asp:Label ID="lbDentalAccountOwner" runat="server" ></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDentalAccountOwner" runat="server" TabIndex="3" 
                    Width="200px" MaxLength="30"></asp:TextBox>
            </td>
            <td>
                &nbsp;<asp:RequiredFieldValidator ID="ValRe_AccountOwner" runat="server" ForeColor="Red" 
                    ValidationGroup="BankAccount" ControlToValidate="txtDentalAccountOwner"></asp:RequiredFieldValidator>
                    </td>
                        
        </tr>
        <tr>                       
            <td>
                <asp:Label ID="lbDentalForReceiveFlg" runat="server"></asp:Label>
            </td>            
            <td>
                <asp:CheckBox ID="ChkReceive" runat="server" AutoPostBack="false" 
                    TabIndex="4" />
            </td>
            <td >               
                <%--<asp:CustomValidator ID="CustomValidator1" runat="server" 
                    ErrorMessage="Input string &quot;True&quot; or &quot;False&quot;" 
                    onservervalidate="CustomValidator1_ServerValidate"></asp:CustomValidator>--%>
                </td>
            
        </tr>
        <tr>                       
            <td>
                <asp:Label ID="lbDentalForpayFlg" runat="server"></asp:Label>
            </td>            
            <td>
                <asp:CheckBox ID="ChkPay" runat="server" AutoPostBack="false" TabIndex="5" />
            </td>
            <td>                
                <%--<asp:CustomValidator ID="CustomValidator2" runat="server" 
                    ControlToValidate="txtDentalForpayFlg" 
                    ErrorMessage="Input string &quot;True&quot; or &quot;False&quot;" 
                    onservervalidate="CustomValidator2_ServerValidate"></asp:CustomValidator>--%>
                </td>
                       
        </tr>       
    </table>  
    <p style="text-align: left; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" ValidationGroup="BankAccount" TabIndex="6"/>
        <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" TabIndex="7" />        
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" TabIndex="8" />  
    </p>     
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>


