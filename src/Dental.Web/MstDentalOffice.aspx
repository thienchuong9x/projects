<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstDentalOffice.aspx.cs" Inherits="MstDentalOffice" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<link  rel="stylesheet" href="Styles/grid.css" type="text/css"/>
<link  rel="stylesheet" href="Styles/form.css" type="text/css"/>
<link rel="Stylesheet" href="Styles/Paging/simplePagination.css" type="text/css" />

<asp:UpdatePanel ID="updatePanelDentalOfficeMaster" runat="server">

    <ContentTemplate>
        <asp:Panel ID="panelHeader" runat="server">
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
                        <asp:Label ID="lbSalesman" runat="server" Text="Salesman"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dlStaff" runat="server" Width="155px" Height="22"/>
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

            <asp:GridView runat="server" ID="gridViewDentalOffice" 
                AutoGenerateColumns="false" style="margin-top:10px"
                DataKeyNames="DentalOfficeCd" ShowHeader="False" OnRowCreated="gridViewDentalOffice_RowCreated"
                CssClass="gridView" AllowPaging="True" 
                onpageindexchanging="gridViewDentalOffice_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DentalOfficeCd">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeAbbNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                     <asp:BoundField DataField="StaffNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficePostalCd">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeAddress1">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeAddress2">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeTEL">
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DentalOfficeFAX">
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TransferDays">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="BillNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="tr_body" />
            </asp:GridView>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btnRegister" CssClass="MasterManagementButton" runat="server" Text="Register"
                    OnClick="btnRegister_Click" />
                <asp:Button ID="btnEdit" CssClass="MasterManagementButton" runat="server" Text="Edit"
                    OnClick="btnEdit_Click" />
            </p>
        </asp:Panel>
        <asp:Panel ID="panelDetail" runat="server" Visible="False">
            <asp:Panel ID="PanelMasterManagement" runat="server">
                <table>
                    <tr>
                        <td style="width: 150px">
                            <asp:Label ID="LabelDentalOfficeCd" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDentalOfficeCd" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="valRequiredInput_DentalOfficeCd" runat="server" ControlToValidate="txtDentalOfficeCd"
                                ForeColor="Red" Display="Dynamic" ValidationGroup = "ValidationDentalOffice" ErrorMessage="*">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="valRequiredNumber_DentalOfficeCd"
                                ControlToValidate="txtDentalOfficeCd" ForeColor="Red" Display="Dynamic" ValidationExpression="^\d+$"  ValidationGroup ="ValidationDentalOffice">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeNm" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextDentalOfficeNm" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="40" Width="300px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="valRequired_DentalOfficeNm" runat="server" ControlToValidate="TextDentalOfficeNm"
                                ForeColor="Red" Display="Dynamic" ValidationGroup ="ValidationDentalOffice">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeAbbNm" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficeAbbNm" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="20" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                      <tr>
                        <td>
                            <asp:Label ID="LabelStaff" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dropDownStaff" runat="server" Width="155px" Height="22"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficePostalCd" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficePostalCd" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="8" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeAddress1" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficeAddress1" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="20" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeAddress2" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficeAddress2" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="20" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeTEL" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficeTEL" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="15" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelDentalOfficeFAX" runat="server"  ></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="TextDentalOfficeFAX" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="15" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelTransferDays" runat="server"  ></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextTransferDays" runat="server" CssClass="MasterManagementTextEntry"
                                MaxLength="10" Width="80px"></asp:TextBox>
                        </td>
                        <td>
                           <asp:RegularExpressionValidator ID="valRequired_TranferDays" runat="server" ControlToValidate="TextTransferDays"
                                ForeColor="Red" ValidationExpression="^[0-9]+$"  ValidationGroup="ValidationDentalOffice">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelBill" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownBill" runat="server" Width="155px" Height="22"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldBill" runat="server" ControlToValidate="DropDownBill"
                                ForeColor="Red" ValidationGroup = "ValidationDentalOffice" InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="ButtonSave" CssClass="MasterManagementButton" runat="server" Text="OK"
                    OnClick="ButtonSave_Click" ValidationGroup ="ValidationDentalOffice" />
                <asp:Button ID="ButtonDelete" CssClass="MasterManagementButton" runat="server" Text="Delete"
                    OnClick="ButtonDelete_Click" CausesValidation="false" />
                <asp:Button ID="ButtonCancel" CssClass="MasterManagementButton" runat="server" Text="Cancel"
                    OnClick="ButtonCancel_Click" CausesValidation="false" />
            </p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:HiddenField ID="hiddenOfficeCd" runat ="server" Visible ="false" /> 


</asp:Content>

