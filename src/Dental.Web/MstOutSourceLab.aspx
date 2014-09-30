<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstOutSourceLab.aspx.cs" Inherits="MstOutSourceLab" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<asp:UpdatePanel ID="upn" runat=server>
<ContentTemplate>

<asp:Panel ID="PanelShow" runat="server">
    <asp:Label ID="lblTitle" runat="server" Text="Label" Visible="False"></asp:Label>
          <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCode" runat="server"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server"/>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="200px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                    
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" onclick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" onclick="btClear_Click" />

                    </td>
                </tr>
            </table>
   
    <asp:GridView ID="gridOutsource" runat="server" AutoGenerateColumns="False" 
       ShowHeader="False" style="margin-top:10px"
        onrowcreated="gridOutsource_RowCreated" BorderColor="#3366CC" 
        EmptyDataText="No Data!" 
        ShowHeaderWhenEmpty="True"  CssClass="gridView" AllowPaging="True" 
        onpageindexchanging="gridOutsource_PageIndexChanging">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="cb" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OutsourceCd" HeaderText="Outsource Cd" >
                <ItemStyle Width="100px"/>
            </asp:BoundField>
            <asp:BoundField DataField="OutsourceNm" HeaderText="Outsource Nm" >
             <ItemStyle Width="150px"/>
            </asp:BoundField>
            <asp:BoundField DataField="OutsourcePostalCd" HeaderText="OutsourcePostalCd" />
            <asp:BoundField DataField="OutsourceAddress1" HeaderText="OutsourceAddress1" />
            <asp:BoundField DataField="OutsourceAddress2" HeaderText="OutsourceAddress2" />
            <asp:BoundField DataField="OutsourceTEL" HeaderText="OutsourceTEL" />
            <asp:BoundField DataField="OutsourceFAX" HeaderText="OutsourceFAX" />
            <asp:BoundField DataField="OutsourceContactPerson" 
                HeaderText="OutsourceContactPerson" />
        </Columns>
         <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev"
                Mode="NumericFirstLast" PageButtonCount="5" />
      <PagerStyle CssClass="pagination" BackColor="White" />
      <RowStyle  CssClass="tr_body" />
    </asp:GridView>
    <asp:Label ID="lblNoRecord" runat="server" Text="Label" Visible="False"></asp:Label>
    <p style="text-align:left;margin-top:15px">
        
        <asp:Button ID="btnReg" runat="server" Text="Register" onclick="btnReg_Click" CausesValidation=false/>
        <asp:Button ID="btnEdit" runat="server" Text="Edit" onclick="btnEdit_Click" CausesValidation=false/>
        
    </p>
</asp:Panel>
<asp:Panel ID="PanelEdit" runat="server" Visible="False">
    <table>
        <tr>
            <td class=auto style="height: 29px">
                <asp:Label ID="lblOutsourceCd" runat="server" Text="Label"></asp:Label>
            </td>
            <td class=auto style="height: 29px">
                </td>
            <td class=auto style="height: 29px">
                <asp:TextBox ID="txtOutsourceCd" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtOutsourceCd" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtOutsourceCd" Display="Dynamic" ForeColor="Red" 
                    ValidationExpression="\d+"></asp:RegularExpressionValidator>
            </td>
            <td style="height: 29px">
                &nbsp;</td>
        </tr>
        <tr>
            <td class=auto>
                <asp:Label ID="lblOutsourceNm" runat="server" Text="Label"></asp:Label>
            </td>
            <td class=auto>
                &nbsp;</td>
            <td class=auto>
                <asp:TextBox ID="txtOutsourceNm" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtOutsourceNm" 
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOutsourcePostalCd" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourcePostalCd" runat="server" MaxLength="8" 
                    Width="80px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOutsourceAddress1" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourceAddress1" runat="server" MaxLength="20" 
                    Width="150px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOoutsourceAddress2" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourceAddress2" runat="server" MaxLength="20" 
                    Width="150px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOutsourceTEL" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourceTEL" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOutsourceFAX" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourceFAX" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto">
                <asp:Label ID="lblOutsourceContactPerson" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto">
                &nbsp;</td>
            <td class="auto">
                <asp:TextBox ID="txtOutsourceContactPerson" runat="server" MaxLength="20" 
                    Width="150px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <p style="text-align:left; margin-top:15px">
    
    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
            Text="Delete" CausesValidation=false />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" CausesValidation=false/>
    
</p>


</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>

</asp:Content>

