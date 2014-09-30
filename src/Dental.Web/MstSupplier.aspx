<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstSupplier.aspx.cs" Inherits="MstSupplier" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<script type="text/javascript">
//    $(document).ready(function () {
//        $('input[type="checkbox"]').dnnCheckbox();
//    });
</script>

<asp:UpdatePanel ID="updatePanelSupplier" runat="server">
    <ContentTemplate>
        <asp:Panel ID="panelGrid" runat="server">
            <asp:Label ID="lblModuleTitle" runat="server"   
                Visible="False"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCode" runat="server"  />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCode" runat="server" MaxLength="10" Width="80px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server"  />
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" MaxLength="40" Width="300px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" OnClick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" OnClick="btClear_Click" />
                    </td>
                </tr>
            </table>

            <asp:GridView ID="gridSupplier" runat="server" ShowHeader="false" style="margin-top:10px"
             EnableModelValidation="True" DataKeyNames="SupplierCd" CssClass="gridView"
                AutoGenerateColumns="False" onrowcreated="gridSupplier_RowCreated" 
                AllowPaging="True" onpageindexchanging="gridSupplier_PageIndexChanging" >
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="SupplierCd" HeaderText="Prosthesis Code">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierNm" HeaderText="Prosthesis Name">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierAbbNm" HeaderText="Minimum Process">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierPostalCd" HeaderText="Minimum Process">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierAddress1" HeaderText="Minimum Process">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierAddress2" HeaderText="Minimum Process">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierTEL" HeaderText="Minimum Process">
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierFAX" HeaderText="Minimum Process">
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SupplierStaff" HeaderText="Minimum Process">
                        <ItemStyle Width="100px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="tr_body" />
               <PagerStyle CssClass="pagination" BackColor="White" /> 
            </asp:GridView>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btRegister" runat="server" OnClick="btRegister_Click" />
                <asp:Button ID="btEdit" runat="server" onclick="btEdit_Click" />
            </p>
        </asp:Panel>
        <asp:Panel ID="panelEdit" runat="server" Visible="False">
            <table>
                <tr>
                    <td style="width:150px">
                        <asp:Label ID="lblSupplierCd" runat="server"  ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxSupplierCd" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly_SupplierCd" ControlToValidate="tbxSupplierCd" ForeColor="Red"
                            Display="Dynamic" ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)">
                        </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="valRequired_SupplierCd" runat="server" ControlToValidate="tbxSupplierCd"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierNm" runat="server"  ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxSupplierNm" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="valRequired_SupplierNm" runat="server" ControlToValidate="tbxSupplierNm"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierAbbNm" runat="server"  ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxSupplierAbbNm" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="valRequired_SupplierAbbNm" runat="server" ControlToValidate="tbxSupplierAbbNm"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierPostalCd" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierPostalCd" runat="server" MaxLength="8" Width="80px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierAddress1" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierAddress1" runat="server" MaxLength="20" 
                            Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierAddress2" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierAddress2" runat="server" MaxLength="20" 
                            Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierTEL" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierTEL" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierFAX" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierFAX" runat="server" MaxLength="15" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplierStaff" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="tbxSupplierStaff" runat="server" MaxLength="10" Width="150px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btSave" runat="server" OnClick="btSave_Click" />
                <asp:Button ID="btDelete" runat="server" onclick="btDelete_Click" />
                <asp:Button ID="btCancel" runat="server" OnClick="btCancel_Click" CausesValidation="false" />
            </p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
</asp:Content>

