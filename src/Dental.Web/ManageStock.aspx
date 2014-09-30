<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ManageStock.aspx.cs" Inherits="ManageStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        function ValidateTextDate(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d\/]+/g, '');
            }
        }
    </script>
    <script type="text/javascript">
        function ValidateTextNumber(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d]+/g, '');
            }
        }
    </script>

    <script type="text/javascript">
        function ValidateText(i) {
            if (i.value.length > 0) {
                i.value = i.value.replace(/[^\d\.]+/g, '');
            }
        }

    </script>

    <style type="text/css">
        .gridViewHeader {
            padding: 6px 0 6px 0;
            font-size: 12px;
            font-weight: bold;
            color: #ffffff;
            background-color: #003399;
            text-align: center;
            width: 30px;
            height: 40px;
            color: #CCCCFF;
        }

        .style1 {
            height: 28px;
        }
    </style>
    <asp:UpdatePanel ID="pnup" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelShow" runat="server">

                <asp:Label ID="lblTitle" runat="server" Text="Label" Visible="False"></asp:Label>
                <asp:Panel ID="PanelSearch" runat="server">
                    <table class="auto">
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblSearchMaCd" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto">&nbsp;</td>
                            <td class="auto" valign="middle">
                                <asp:TextBox ID="txtSearchMaCd" runat="server" Width="80px" AutoPostBack="True" 
                                    ontextchanged="txtSearchMaCd_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlMa" runat="server" Width="195px" AutoPostBack="True" 
                                    onselectedindexchanged="ddlMa_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="auto" valign="middle">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Text="~"></asp:Label>
                            </td>
                            <td class="auto" valign="middle">
                                <asp:TextBox ID="txtSearchMaCd0" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtSearchMaCd0_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlMa0" runat="server" AutoPostBack="True" Width="195px" 
                                    onselectedindexchanged="ddlMa0_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblSearchBorrower" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto">&nbsp;</td>
                            <td class="auto">
                                <asp:TextBox ID="txtSearchBorower" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtSearchBorower_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlBorrower" runat="server" AutoPostBack="True" 
                                    Width="195px" onselectedindexchanged="ddlBorrower_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="auto">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Text="~"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtSearchBorower0" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtSearchBorower0_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlBorrower0" runat="server" AutoPostBack="True" 
                                    Width="195px" onselectedindexchanged="ddlBorrower0_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:CheckBox ID="cbBorrower" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="cbBorrower_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblSearchLender" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td class="auto">&nbsp;</td>
                            <td class="auto">
                                <asp:TextBox ID="txtSearchLender" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtSearchLender_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlLender" runat="server" AutoPostBack="True" 
                                    Width="195px" onselectedindexchanged="ddlLender_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="auto">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Text="~"></asp:Label>
                            </td>
                            <td class="auto">
                                <asp:TextBox ID="txtSearchLender0" runat="server" AutoPostBack="True" 
                                    Width="80px" ontextchanged="txtSearchLender0_TextChanged"></asp:TextBox>
                                <asp:DropDownList ID="ddlLender0" runat="server" AutoPostBack="True" 
                                    Width="195px" onselectedindexchanged="ddlLender0_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:CheckBox ID="cbLender" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="cbLender_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="auto">
                                <asp:Label ID="lblOwnStock" runat="server" Text="OwnStock"></asp:Label>
                            </td>
                            <td class="auto">&nbsp;</td>
                            <td class="auto">
                                <asp:CheckBox ID="cbOwnStock" runat="server" />
                            </td>
                            <td class="auto">&nbsp;</td>
                            <td class="auto">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" 
                                    onclick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server"
                                    Text="Clear" onclick="btnClear_Click" />
                            </td>
                            <td></td>
                            <td style="text-align: right">
                                <asp:DropDownList ID="DropdownSelectRows" runat="server" AutoPostBack="True" 
                                    Width="50px" 
                                    onselectedindexchanged="DropdownSelectRows_SelectedIndexChanged">
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="lblRowsPage" runat="server" Text="rows/page"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                    AllowPaging="True"
                    CssClass="gridView" EmptyDataText="No Data!" ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowcreated="GridView1_RowCreated">
                    <Columns>
                        <asp:BoundField DataField="MaterialCd" HeaderText="Material Cd">
                            <ItemStyle Width="50" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MaterialNm" HeaderText="Material Name">
                            <ItemStyle Width="300" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Maker" HeaderText="Maker">
                            <ItemStyle Width="300" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit">
                            <ItemStyle Width="50" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Qty" HeaderText="Qty">
                            <ItemStyle Width="100" />
                        </asp:BoundField>

                        <asp:BoundField HeaderText="Borrower" DataField="Borrower">
                            <ItemStyle Width="300" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Borrowed Qty" DataField="BorrowedQty">
                            <ItemStyle Width="100" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Lender" DataField="Lender">
                            <ItemStyle Width="300" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LendedQty" HeaderText="Lended Qty">
                            <ItemStyle Width="100" />
                        </asp:BoundField>
                    </Columns>
                    
                     <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" 
                        PreviousPageText="Prev" PageButtonCount="5" />
                    <PagerStyle CssClass="pagination" />
                    <RowStyle  CssClass="tr_body" />

                </asp:GridView>
                <asp:Label ID="lblNoRecord" runat="server" Visible="False"></asp:Label>
                <asp:HiddenField ID="HiddenFieldOfficeCd" runat="server" />
                <p style="text-align: left; margin-top: 15px">

                    <asp:Button ID="btnRegister" runat="server" Text="Register" 
                        CausesValidation="False" onclick="btnRegister_Click" />

                </p>
            </asp:Panel>

            <asp:Panel ID="PanelReg" runat="server" Visible="False">
                <asp:Label ID="lblTitle2" runat="server" Text="Label"></asp:Label>
                <br />
                <br />
                <br />
                <table class="auto" style="width: auto">
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblRegDate" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtRegdate" runat="server" Width="80px"
                                onkeyup="ValidateTextDate(this)"></asp:TextBox>
                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                ImageUrl="~/Styles/images/calendar.png">hplCalendar</asp:HyperLink>
                            <asp:CompareValidator ID="compareRegDate" runat="server"
                                ControlToValidate="txtRegdate" Display="Dynamic" ForeColor="Red"
                                Operator="DataTypeCheck" Type="Date" ValidationGroup="checkError"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="reqValidatorRegDate" runat="server"
                                ControlToValidate="txtRegdate" ForeColor="Red"
                                ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblMaterialType" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto"></td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtMaCd" runat="server" AutoPostBack="True" Enabled="true" 
                                Width="80px" ontextchanged="txtMaCd_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="DropDownListMa" runat="server" AutoPostBack="True" 
                                Width="300px" onselectedindexchanged="DropDownListMa_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqValidationMaCd" runat="server"
                                ControlToValidate="txtMaCd" Display="Dynamic" ForeColor="Red"
                                ValidationGroup="checkError"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regValidationMaCd" runat="server"
                                ControlToValidate="txtMaCd" Display="Dynamic" ForeColor="Red"
                                ValidationExpression="^[0-9]*(?:\.[0-9]+)?$" ValidationGroup="checkError"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:Label ID="lblCurrentStock" runat="server" Text="Label"></asp:Label>
                            &nbsp;:<asp:Label ID="LabelCurStock" runat="server"></asp:Label>
                            &nbsp;
                <asp:Label ID="lblTotalStock" runat="server" Text="Label" Visible="False"></asp:Label>
                            &nbsp;<asp:Label ID="LabelTotal" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="LabelTotalUnit" runat="server" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto"></td>
                        <td class="auto"></td>
                        <td class="auto"></td>
                        <td class="auto"></td>
                        <td class="auto"></td>
                        <td class="auto"></td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblAction" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto"></td>
                        <td class="auto" style="text-align: left" width="30">
                            <asp:Label ID="lblIn" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto" colspan="3" style="text-align: left">
                            <asp:RadioButton ID="rbtnBuy" runat="server" AutoPostBack="True"
                                GroupName="checkAction" Text="Buy"
                                Width="120px" oncheckedchanged="rbtnBuy_CheckedChanged" />
                            <asp:RadioButton ID="rbtnBorrow" runat="server" AutoPostBack="True"
                                GroupName="checkAction"
                                Text="Borrow" Width="120px" oncheckedchanged="rbtnBorrow_CheckedChanged" />
                            <asp:RadioButton ID="rbtnLendReturn" runat="server" AutoPostBack="True"
                                GroupName="checkAction"
                                Text="Lend return" Width="120px" 
                                oncheckedchanged="rbtnLendReturn_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">&nbsp;</td>
                        <td class="auto"></td>
                        <td class="auto" width="30">
                            <asp:Label ID="lblOut" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto" colspan="3">
                            <asp:RadioButton ID="rbtnLeave" runat="server" AutoPostBack="True"
                                GroupName="checkAction"
                                Text="Leave" Width="120px" oncheckedchanged="rbtnLeave_CheckedChanged" />
                            <asp:RadioButton ID="rbtnLend" runat="server" AutoPostBack="True"
                                GroupName="checkAction" Text="Lend"
                                Width="120px" oncheckedchanged="rbtnLend_CheckedChanged" />
                            <asp:RadioButton ID="rbtnBorrowReturn" runat="server" AutoPostBack="True"
                                GroupName="checkAction"
                                Text="Borrow return" Width="120px" 
                                oncheckedchanged="rbtnBorrowReturn_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblQty" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtQty" runat="server" Width="78px" onkeyup="ValidateText(this)"></asp:TextBox>
                            <asp:DropDownList ID="DropDownListQtyUnit" runat="server" Width="78px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqValidationQty" runat="server"
                                ControlToValidate="txtQty" Display="Dynamic" ForeColor="Red"
                                ValidationGroup="checkError"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegExQty" runat="server"
                                ControlToValidate="txtQty" Display="Dynamic" ForeColor="Red"
                                ValidationExpression="\d*\.?\d+" ValidationGroup="checkError"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="reqValidationUnit" runat="server"
                                ControlToValidate="DropDownListQtyUnit" ForeColor="Red" Display="Dynamic"
                                ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblPrice" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtPrice" runat="server" Width="78px" onkeyup="ValidateText(this)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqValidatorPrice" runat="server"
                                ControlToValidate="txtPrice" ForeColor="Red" ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblFee" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtFee" runat="server" Width="78px" onkeyup="ValidateText(this)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblSupllier" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtSupCd" runat="server" AutoPostBack="True" Width="80px" 
                                ontextchanged="txtSupCd_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="DropDownListSup" runat="server" AutoPostBack="True"
                                Width="300px" 
                                onselectedindexchanged="DropDownListSup_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqValidatorSupplier" runat="server"
                                ControlToValidate="txtSupCd" ForeColor="Red" ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label ID="lblBorrower" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="style1"></td>
                        <td class="style1" colspan="4">
                            <asp:TextBox ID="txtOsCd" runat="server" AutoPostBack="True" Width="80px" 
                                ontextchanged="txtOsCd_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="DropDownListOS" runat="server" AutoPostBack="True" 
                                Width="300px" onselectedindexchanged="DropDownListOS_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqValidatorOS" runat="server"
                                ControlToValidate="txtOsCd" ForeColor="Red" ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblLender" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">
                            <asp:TextBox ID="txtLender" runat="server" AutoPostBack="True" Width="80px" 
                                ontextchanged="txtLender_TextChanged"></asp:TextBox>
                            <asp:DropDownList ID="DropDownListLender" runat="server" AutoPostBack="True" 
                                Width="300px" 
                                onselectedindexchanged="DropDownListLender_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqValidatorLender" runat="server"
                                ControlToValidate="txtLender" ForeColor="Red" ValidationGroup="checkError"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">
                            <asp:Label ID="lblComment" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4" rowspan="2">
                            <asp:TextBox ID="txtCmt" runat="server" TextMode="MultiLine" Width="383px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regExpMaxCmt" runat="server"
                                ControlToValidate="txtCmt" Display="Dynamic" ForeColor="Red"
                                ValidationExpression="^[\s\S]{0,100}" ValidationGroup="checkError"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto">&nbsp;</td>
                        <td class="auto">&nbsp;</td>
                        <td class="auto" colspan="4">&nbsp;</td>
                    </tr>
                </table>
                <asp:Button ID="btnReg" runat="server"
                    ValidationGroup="checkError" onclick="btnReg_Click" />
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" 
                    Text="Cancel" onclick="btnCancel_Click" />
                <asp:Label ID="Label7" runat="server" Text="Label" Visible="False"></asp:Label>
                <asp:Label ID="Label8" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />


            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

