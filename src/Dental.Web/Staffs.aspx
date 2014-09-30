<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Staffs.aspx.cs" Inherits="Staffs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <link  rel="stylesheet" href="Styles/grid.css" type="text/css"/>
<link  rel="stylesheet" href="Styles/form.css" type="text/css"/>
<link rel="Stylesheet" href="Styles/Paging/simplePagination.css" type="text/css" />


<asp:UpdatePanel ID="updatePanelStaff" runat="server" ViewStateMode="Enabled">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" Visible="False">
            <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbUserId" runat="server" />
                        </td>
                        <td>
                             <asp:DropDownList ID="cbNewUser" runat="server" Height="22px" 
                                onselectedindexchanged="cbNewUser_SelectedIndexChanged" Width="155px" 
                                AutoPostBack="True" TabIndex="1">
                            </asp:DropDownList>

                        </td>
                        <td>
                            <asp:Label ID="lbWrongUserId" runat="server" ForeColor="Red"></asp:Label>
                    </tr>

                <tr>
                    <td>
                        <asp:Label ID="lbStaffNm" runat="server"  ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStaffNm" runat="server" MaxLength="30" Width="200px" 
                            TabIndex="2"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="valRequired_StaffNm" runat="server" ControlToValidate="txtStaffNm"
                            ForeColor="Red" ValidationGroup="Staff"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbStaffNmKana" runat="server"  ></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtStaffNmKana" runat="server" MaxLength="30" Width="200px" 
                            TabIndex="3"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lbStaffCd" runat="server"  ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtStaffCd" runat="server" MaxLength="10" Width="80px" 
                            TabIndex="4"></asp:TextBox>
                        <asp:Label ID="lbStaffCdExist" runat="server" ForeColor="Red" 
                            Text="StaffCd is existed !" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="valRequired_StaffCd" runat="server" ControlToValidate="txtStaffCd"
                            ForeColor="Red" ValidationGroup="Staff"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="valNumbersOnly_StaffCd" ControlToValidate="txtStaffCd"
                            ForeColor="Red" 
                            ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" ValidationGroup="Staff"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbSalesman" runat="server"  ></asp:Label>
                    </td>
                    <td >
                        <asp:CheckBox ID="cbSalesman" runat="server" TabIndex="7" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbTechnician" runat="server"  ></asp:Label>
                    </td>
                    <td >
                        <asp:CheckBox ID="cbTechnician" runat="server" TabIndex="8" />
                    </td>
                </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lbOfficeCd" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:GridView ID="gvOffice" runat="server" AutoGenerateColumns="False" 
                                CssClass="gridView" DataKeyNames="OfficeCd" 
                                ShowHeader="false" style="margin-top:10px" TabIndex="9">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OfficeNm">
                                    <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="OfficeCd">
                                    <ItemStyle Width="0px" Height="0px" ForeColor="White"/>
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="tr_body" />
                            </asp:GridView>
                        </td>
                        <td>
                             &nbsp;</td>
                    </tr>

                    <tr>
                    
                        <td>
                                <asp:Label ID="lbLogin" runat="server" />
                        </td>
                        <td>
                               <asp:CheckBox ID="cbLogin" runat="server" Text="Allow Login" 
                                   AutoPostBack="True" ForeColor="Black" TabIndex="10" 
                                   oncheckedchanged="cbLogin_CheckedChanged" Enabled="False" Checked="True"/>
                                  <%-- onClick="enableAU()" --%>
                        </td>
                    
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="lbAuthority" runat="server" />
                        </td>
                        <td>
                            <%--<asp:CheckBox ID="cbChief" runat="server"/>--%>
                            <asp:DropDownList ID="cbAuthority" runat="server" Width="155px" Height="22px" 
                                ForeColor="Black" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbIsDeleted" runat="server" Visible="False" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsDeleted" runat="server" Visible="False" />
                        </td>
                    </tr>
                </table>

            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btSave" runat="server" OnClick="btSave_Click" 
                    ValidationGroup="Staff" TabIndex="5" />
                <asp:Button ID="btDelete" runat="server" CausesValidation="false" 
                    OnClick="btDelete_Click" Text="Delete" TabIndex="5" />
                <asp:Button ID="btCancel" runat="server" CausesValidation="false" 
                    OnClick="btCancel_Click" TabIndex="6" />

                <asp:TextBox ID="txtUserName" runat="server" MaxLength="40"  Width="138px" 
                    BorderStyle="None" ForeColor="White"/>

                 <%--onkeyup="CheckUserName()"  />  Width="0px" BorderStyle="None" ForeColor="White"   --%>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server">
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
                        <asp:TextBox ID="txtName" runat="server" MaxLength="30" Width="200px"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbOffice" runat="server"/>
                    </td>
                    <td>
                         <asp:DropDownList ID="cbOffice" runat="server" Width="155px" />
                    </td>
                
                </tr>
                <tr>
                    <td>
                    
                    </td>
                    <td>
                    
                        <asp:CheckBox ID="cbIncludeIsDeleted" runat="server" Text="Include IsDeleted" />
                    
                    </td>
                </tr>

                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btSearch" runat="server" OnClick="btSearch_Click" />
                        <asp:Button ID="btClear" runat="server" OnClick="btClear_Click" />
                    </td>
                    <td></td>

                    <td align="right">
                         <asp:DropDownList ID="dlNumber" runat="server" 
                            onselectedindexchanged="dlNumber_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbRowPerPage" runat="server" Text="rows/page"></asp:Label>            

                    </td>
                </tr>
            </table>

            <asp:GridView ID="lstContent" runat="server" ShowHeader="false" 
                DataKeyNames="StaffCd" style="margin-top:10px"
                CssClass="gridView" EnableModelValidation="True" AutoGenerateColumns="False"
                OnRowCreated="lstContent_RowCreated" AllowPaging="True" 
                onpageindexchanging="lstContent_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="Check" runat="server" AutoPostBack="False" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserId">
                        <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StaffCd">
                        <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StaffNm">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StaffNmKana">
                        <ItemStyle Width="170px"></ItemStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="SalesFlg">
                        <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TechFlg">
                        <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UserId">
                        <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UserId">
                        <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UserId">
                        <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IsDeleted">
                        <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>

                </Columns>
                <RowStyle CssClass="tr_body" />
            </asp:GridView>
            <p style="text-align: left; margin-top: 15px;">
                <asp:Button ID="btRegister" runat="server" OnClick="btRegister_Click"/>
                
                <asp:Button ID="btEdit" runat="server" OnClick="btEdit_Click" />

                &nbsp;</p>
        </asp:Panel>
    <asp:TextBox ID="txtFlag" runat="server" BorderStyle="None" ForeColor="White" Width="0px" Height="0px"/>


    </ContentTemplate>
</asp:UpdatePanel>

<asp:HiddenField ID="hiddenOfficeCd" runat ="server" Visible ="false" /> 
  
</asp:Content>

