<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MstOffice.aspx.cs" Inherits="MstOffice" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<link  rel="stylesheet" href="Styles/grid.css" type="text/css"/>
<link  rel="stylesheet" href="Styles/form.css" type="text/css"/>
<link rel="Stylesheet" href="Styles/Paging/simplePagination.css"/>
<%--
<asp:ScriptManager ID="ScriptManager1" runat="server">
 </asp:ScriptManager>--%>

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
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" OnClick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" OnClick="btClear_Click" />
                    </td>
                </tr>
            </table>

            <asp:GridView runat="server" ID="gridViewOffice" 
                AutoGenerateColumns="False" style="margin-top:10px"
                DataKeyNames="OfficeCd" ShowHeader="False" OnRowCreated="gridViewOffice_RowCreated"
                CssClass="gridView" AllowPaging="True" 
                onpageindexchanging="gridViewOffice_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OfficeCd">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficeNm">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficePostalCd">
                        <ItemStyle />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficeAddress1">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficeAddress2">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficeTEL">
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OfficeFAX">
                        <ItemStyle Width="90px" />
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


