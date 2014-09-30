<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstTax.aspx.cs" Inherits="MstTax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:UpdatePanel ID="UpdateTax" runat="server">
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
                <asp:Label ID="lbDate" runat="server" Text="Label"></asp:Label>                
            </td>
            <td>
                <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplStartDateFrom" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>            
            </td>
        </tr> 
        <tr>
             <td >
                 <asp:Label ID="lbChkAvailable" runat="server"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="ChckAvailable" runat="server" AutoPostBack="True" 
                    oncheckedchanged="ChckAvailable_CheckedChanged" />
            </td>

        </tr> 
        <tr>
            <td></td>        
            <td>
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click"/>
                <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" />
            </td>
        </tr>        
                 
    </table> 
    <asp:GridView ID="grvTax" runat="server" AutoGenerateColumns="False" CssClass="gridView"       
            style="margin-top:10px " CellPadding="4" 
        onrowcreated="grvTax_RowCreated" DataKeyNames="TaxCd"  ShowHeader="False" 
        AllowPaging="True" onpageindexchanging="grvTax_PageIndexChanging">        
        <Columns>     
            <asp:TemplateField> 
                    <ItemTemplate><center>                                          
                        <asp:CheckBox ID="CheckEdit" runat="server" AutoPostBack="false" /> </center>                       
                    </ItemTemplate><ItemStyle Width="20px" HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>         
            <asp:BoundField DataField="TaxCd" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField= "TaxRate" > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="StartDate"> <ItemStyle Width="75px" HorizontalAlign="Center" Font-Size="Small" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="EndDate" > <ItemStyle Width="75px"  HorizontalAlign="Center" Font-Size="Small" ></ItemStyle></asp:BoundField>
            <asp:BoundField DataField="RoundFraction"  > <ItemStyle Width="100px" ></ItemStyle></asp:BoundField>           
        </Columns>
        <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
            <PagerStyle CssClass="pagination" BackColor="White" /> 
        <RowStyle CssClass="tr_body" />
    </asp:GridView>    
    <p style="margin-top:15px; text-align:left">    
    <asp:Button ID="btnRegister" runat="server" onclick="btnRegister_Click"  />
        <asp:Button ID="btnEdit" runat="server" onclick="btnEdit_Click" />
    </p>
    </asp:Panel>
<style type="text/css"> 
    </style>
<asp:Panel ID="PnEdit" runat="server" Visible="False"> 
    <br />     
    <table>
        <tr>            
            <td >
                <asp:Label ID="lbDentalTaxCd" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:TextBox ID="txtDentalTaxCd" runat="server" Width="80px" MaxLength="10" ></asp:TextBox>
                </td>
            <td >
                <asp:CompareValidator ID="val_TaxCd" runat="server" 
                    ControlToValidate="txtDentalTaxCd" 
                    Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ForeColor="Red" 
                    ValidationGroup="Tax"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="valRe_TaxCd" runat="server" 
                    ControlToValidate="txtDentalTaxCd" ForeColor="Red" ValidationGroup="Tax"></asp:RequiredFieldValidator>
            </td>
                                  
        </tr>
        <tr>         
            <td >
                <asp:Label ID="lbDentalTaxRate" runat="server" ></asp:Label>
            </td>            
            <td >
                <asp:TextBox ID="txtDentalTaxRate" runat="server" Width="80px"></asp:TextBox>
            </td>
            <td >
                <asp:CompareValidator ID="Val_TaxRate" runat="server" ControlToValidate="txtDentalTaxRate" 
                    Operator="DataTypeCheck" Type="Double" Display="Dynamic" ForeColor="Red" 
                    ValidationGroup="Tax"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="valRe_TaxRate" runat="server" 
                    ControlToValidate="txtDentalTaxRate" ForeColor="Red" ValidationGroup="Tax"></asp:RequiredFieldValidator>
            </td>
                        
        </tr>
        <tr>           
            <td >
                <asp:Label ID="lbDentalStartDate" runat="server" ></asp:Label>
            </td>            
            <td >
                <asp:TextBox ID="txtDentalStartDate" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplStartDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
            </td>
            <td >
                <asp:CompareValidator ID="Val_StartDate" runat="server" ControlToValidate="txtDentalStartDate" 
                    Operator="DataTypeCheck" Type="Date" Display="Dynamic" ForeColor="Red" 
                    ValidationGroup="Tax"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="valRe_StartDate" runat="server" 
                    ControlToValidate="txtDentalStartDate" ForeColor="Red" ValidationGroup="Tax"></asp:RequiredFieldValidator>
                </td>            
        </tr>
        <tr>         
            <td >
                <asp:Label ID="lbDentalEndDate" runat="server" ></asp:Label>
            </td>
            
            <td>
                <asp:TextBox ID="txtDentalEndDate" runat="server" Width="80px"></asp:TextBox>
                <asp:HyperLink ID="hplEndDate" runat="server" 
                            ImageUrl="~/Styles/images/calendar.png" ToolTip="Select on Calendar">HyperLink</asp:HyperLink>
            </td>
            <td >                
                <asp:CompareValidator ID="Val_EndDate" runat="server" ControlToValidate="txtDentalEndDate" 
                    Operator="DataTypeCheck" Type="Date" ForeColor="Red" ValidationGroup="Tax"></asp:CompareValidator>
                </td>
                  
        </tr>
        <tr>
            <td >
                <asp:Label ID="lbDentalRoundFraction" runat="server" ></asp:Label>
            </td>            
            <td>
                <asp:DropDownList ID="DropDownListTaxRoundFraction" runat="server" 
                    Width="155px" Height="22px">
                </asp:DropDownList>
            </td>
            <td >               
                </td>             
        </tr>       
    </table>  
    <p style="text-align:left; margin-top: 15px;">
        <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" ValidationGroup="Tax"/>
        <asp:Button ID="btnDelete" runat="server"  onclick="btnDelete_Click"  />
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />  
    </p>     
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

