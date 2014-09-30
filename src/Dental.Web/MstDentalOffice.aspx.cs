using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstDentalOffice : DDVPortalModuleBase 
{
    readonly static ILog logger = LogManager.GetLogger(typeof(MstDentalOffice));

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InitLanguage();
                    hiddenOfficeCd.Value = GetOffice();

                    ButtonDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
                    Initiation();
                    FillStaffDropDownList();
                    FillBillDropDownList();
                    FillDataToGridView();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Page_Load ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }


        protected void gridViewDentalOffice_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                InitLanguage();
                //Build custom header.
                GridView oGridView = (GridView)sender;
                GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableCell oTableCell = new TableHeaderCell();


                //Add CheckBoxSelected
                oTableCell.CssClass = "td_header";
                oTableCell.Width = Unit.Pixel(20);
                oGridViewRow.Cells.Add(oTableCell);

                //Add DentalOfficeCode
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeCd.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);


                //Add DentalOfficeName
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeNm.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add DentalOfficeAbbNm
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeAbbNm.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add StaffName
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeStaff.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add DentalOfficePostalCd
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficePostalCd.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add DentalOfficeAddress1
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress1.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add DentalOfficeAddress2
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress2.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add HEADER_DentalOfficeTEL
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeTEL.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add HEADER_DentalOfficeFAX
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_DentalOfficeFAX.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add HEADER_TransferDays
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_TransferDays.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add HEADER_Bill
                oTableCell = new TableHeaderCell();
                oTableCell.Text = GetResource("HEADER_OFFICE_Bill_Name.Text");
                oTableCell.CssClass = "td_header";
                oGridViewRow.Cells.Add(oTableCell);

                //Add custom header            
                oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var checkedIDs = (from GridViewRow msgRow in gridViewDentalOffice.Rows
                                  where ((CheckBox)msgRow.FindControl("Check")).Checked
                                  select Convert.ToInt32(gridViewDentalOffice.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();
                if (checkedIDs.Count == 0)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
                }
                else if (checkedIDs.Count > 1)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
                }
                else
                {
                    txtDentalOfficeCd.Text = checkedIDs[0].ToString();
                    txtDentalOfficeCd.Enabled = false;

                    MasterDentalOffice info = new MasterDentalOffice();
                    info.OfficeCd = int.Parse(hiddenOfficeCd.Value )  ;
                    info.DentalOfficeCd = Convert.ToInt32(checkedIDs[0].ToString());
                    info = info.GetByPrimaryKey(); 
                    SetDentalOfficeMaster(info);
                    SetVisibleHeader(false);
                    ButtonDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            txtDentalOfficeCd.Enabled = true;
            ResetDentalOfficeMaster();
            SetVisibleHeader(false);
            ButtonDelete.Enabled = false;
        }

        private void Initiation()
        {
            lblCode.Text = GetResource("HEADER_DentalOffice_CODE_DentalOfficeCd.Text");
            lblName.Text = GetResource("HEADER_DentalOffice_Nm_DantalOfficeNm.Text");
            btSearch.Text = GetResource("btSearch.Text");
            btClear.Text = GetResource("btClear.Text");
            //lblModuleTitle.Text = GetResource("lblDentalOffice.Text");
            LabelDentalOfficeCd.Text = GetResource("HEADER_OFFICE_DentalOfficeCd.Text");
            LabelDentalOfficeNm.Text = GetResource("HEADER_OFFICE_DentalOfficeNm.Text");
            LabelDentalOfficeAbbNm.Text = GetResource("HEADER_OFFICE_DentalOfficeAbbNm.Text");
            LabelDentalOfficePostalCd.Text = GetResource("HEADER_OFFICE_DentalOfficePostalCd.Text");
            LabelDentalOfficeAddress1.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress1.Text");
            LabelDentalOfficeAddress2.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress2.Text");
            LabelDentalOfficeFAX.Text = GetResource("HEADER_OFFICE_DentalOfficeFAX.Text");
            LabelDentalOfficeTEL.Text = GetResource("HEADER_OFFICE_DentalOfficeTEL.Text");
            LabelTransferDays.Text = GetResource("HEADER_OFFICE_TransferDays.Text");
            LabelStaff.Text = GetResource("HEADER_OFFICE_DentalOfficeStaff.Text");
            lbSalesman.Text = GetResource("HEADER_OFFICE_DentalOfficeStaff.Text");
            LabelBill.Text = GetResource("HEADER_OFFICE_Bill_Name.Text");
            valRequiredInput_DentalOfficeCd.ErrorMessage = valRequiredInput_DentalOfficeCd.ToolTip = valRequiredNumber_DentalOfficeCd.ErrorMessage = valRequiredNumber_DentalOfficeCd.ToolTip = GetResource("valRequired_DentalOfficeCd.Text");
            valRequired_DentalOfficeNm.ErrorMessage = valRequired_DentalOfficeNm.ErrorMessage = GetResource("valRequired_DentalOfficeNm.Text");
            valRequired_TranferDays.ErrorMessage = valRequired_TranferDays.ToolTip = GetResource("valRequired_TranferDays.Text");
            RequiredFieldBill.ErrorMessage = RequiredFieldBill.ToolTip = GetResource("valRequired_BillCd.Text");

            gridViewDentalOffice.EmptyDataText = GetResource("NoRecordFound.Text");
            SetButtonText(btnRegister, btnEdit, ButtonSave, ButtonDelete, ButtonCancel);
        }
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                MasterDentalOffice row = MasterDentalOffice.GetDentalOfficeMaster(int.Parse(hiddenOfficeCd.Value), Convert.ToInt32(txtDentalOfficeCd.Text));
                if (ButtonDelete.Enabled == false)
                {
                    if (row == null)
                    {
                        row = GetDentalOfficeMaster();
                        row.Insert();
                    }
                    else
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_EXIST_DENTAL_OFFICE_CD.Text")) + "\");");
                        return;
                    }
                }
                else
                {
                    row = GetDentalOfficeMaster();
                    row.Update();
                }
                FillDataToGridView();
                SetVisibleHeader(true);
            }
            catch (Exception ex)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
                logger.Error("Error ButtonSave_Click ", ex);
            }
        }
        public static string GetNameFromCode(List<CodeName> list, string code)
        {
            try
            {
                CodeName item = list.FirstOrDefault(p=>p.Code == code);
                if (item != null)
                    return item.Name;
                return "";
            }
            catch
            {
                return "";
            }
        }
        private void FillDataToGridView()
        {
            List<MasterDentalOffice> colDentalOfficeMasters = MasterDentalOffice.GetDentalOfficeMasterSearch(int.Parse(hiddenOfficeCd.Value ), txtCode.Text, txtName.Text, dlStaff.SelectedValue);
            if (colDentalOfficeMasters != null && colDentalOfficeMasters.Count > 0)
            {


                foreach (MasterDentalOffice i in colDentalOfficeMasters)
                {
                    if (i.StaffCd != null)
                    {
                        var staffInfo = MasterStaff.GetStaff(Convert.ToInt32(i.StaffCd));
                        if (staffInfo != null)
                        {
                            i.StaffNm = staffInfo.StaffNm;
                        }
                        else
                        {
                            i.StaffNm = "?";
                        }
                    }
                    else
                        i.StaffNm = "?";


                    var billInfo = MasterBill.GetBillMaster(int.Parse(hiddenOfficeCd.Value), i.BillCd);
                    if (billInfo != null)
                    {
                           i.BillNm = billInfo.BillNm;
                    }
                    else
                    {
                            i.BillNm = "?";
                    }

                }

                gridViewDentalOffice.DataSource = colDentalOfficeMasters;
                gridViewDentalOffice.DataBind();
            }
            else
            {
                colDentalOfficeMasters = new List<MasterDentalOffice>();
                colDentalOfficeMasters.Add(new MasterDentalOffice());
                gridViewDentalOffice.DataSource = colDentalOfficeMasters;
                gridViewDentalOffice.DataBind();
                gridViewDentalOffice.Rows[0].Visible = false;
            }
        }
        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtDentalOfficeCd.Enabled)
                {
                    MasterDentalOffice item = MasterDentalOffice.GetDentalOfficeMaster(int.Parse(hiddenOfficeCd.Value), Convert.ToInt32(txtDentalOfficeCd.Text));
                    item.Delete();
                }
                else
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_NOT_CASE_DELETE.Text")) + "\");");
                }
                FillDataToGridView();
                SetVisibleHeader(true);
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            }
        }
        private MasterDentalOffice GetDentalOfficeMaster()
        {
            MasterDentalOffice dental = new MasterDentalOffice();
            dental.OfficeCd = int.Parse(hiddenOfficeCd.Value);
            dental.DentalOfficeCd = Convert.ToInt32(txtDentalOfficeCd.Text);
            dental.DentalOfficeNm = TextDentalOfficeNm.Text;
            dental.DentalOfficeAbbNm = TextDentalOfficeAbbNm.Text;
            dental.DentalOfficePostalCd = TextDentalOfficePostalCd.Text;
            dental.DentalOfficeAddress1 = TextDentalOfficeAddress1.Text;
            dental.DentalOfficeAddress2 = TextDentalOfficeAddress2.Text;
            dental.DentalOfficeFAX = TextDentalOfficeFAX.Text;
            dental.DentalOfficeTEL = TextDentalOfficeTEL.Text;
            dental.TransferDays = Common.GetNullableInt(TextTransferDays.Text);
            dental.CreateAccount =  dental.ModifiedAccount = this.User.Identity.Name ;
            dental.StaffCd = Common.GetNullableInt(dropDownStaff.SelectedValue); 
            dental.BillCd = Convert.ToInt32(DropDownBill.SelectedValue);
            return dental;
        }
        private void SetDentalOfficeMaster(MasterDentalOffice dental)
        {
            txtDentalOfficeCd.Text = dental.DentalOfficeCd.ToString();
            TextDentalOfficeNm.Text = dental.DentalOfficeNm;
            TextDentalOfficeAbbNm.Text = dental.DentalOfficeAbbNm;
            TextDentalOfficePostalCd.Text = dental.DentalOfficePostalCd;
            TextDentalOfficeAddress1.Text = dental.DentalOfficeAddress1;
            TextDentalOfficeAddress2.Text = dental.DentalOfficeAddress2;
            TextDentalOfficeFAX.Text = dental.DentalOfficeFAX;
            TextDentalOfficeTEL.Text = dental.DentalOfficeTEL;
            TextTransferDays.Text  = Common.GetNullableString(dental.TransferDays);
            
            SetDropDownValue(dropDownStaff,  Common.GetNullableString(dental.StaffCd));
            SetDropDownValue(DropDownBill, dental.BillCd.ToString());    
        }
        private void SetDropDownValue(DropDownList dropdown, string code)
        {
            try
            {
                ListItem item = dropdown.Items.FindByValue(code);
                if (item != null)
                {
                    dropdown.SelectedValue = code;
                }
                else
                {
                    dropdown.Text = "";
                }
            }
            catch { }
        }
        private void ResetDentalOfficeMaster()
        {
            txtDentalOfficeCd.Text = "";
            TextDentalOfficeNm.Text = "";
            TextDentalOfficeAbbNm.Text = "";
            TextDentalOfficePostalCd.Text = "";
            TextDentalOfficeAddress1.Text = "";
            TextDentalOfficeAddress2.Text = "";
            TextDentalOfficeFAX.Text = "";
            TextDentalOfficeTEL.Text = "";
            TextTransferDays.Text = "";
            dropDownStaff.Text = "";
        }
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            SetVisibleHeader(true);
        }
        private void SetVisibleHeader(bool show)
        {
            panelHeader.Visible = show;
            panelDetail.Visible = !show;
        }
        private void FillStaffDropDownList()
        {
            dropDownStaff.Items.Clear();
            dropDownStaff.Items.Add(new ListItem("", ""));

            List<MasterStaff> list = MasterStaff.GetStaffwiOffice(int.Parse(hiddenOfficeCd.Value));
            dlStaff.Items.Add("");

            foreach (MasterStaff i in list)
            {
                if (i.SalesFlg.HasValue && i.SalesFlg.Value == true)
                {
                    dropDownStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
                    dlStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
                }
            }
        }
        private void FillBillDropDownList()
        {
            logger.Debug("FillBillDropDownList");
            DropDownBill.Items.Clear();

            List<MasterBill> listBill = MasterBill.GetBillMasters(int.Parse(hiddenOfficeCd.Value));
            logger.Debug("listbill.count = " + listBill.Count);
            foreach (MasterBill i in listBill)
            {
                DropDownBill.Items.Add(new ListItem(i.BillNm, i.BillCd.ToString()));
            }
        }

        protected void gridViewDentalOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewDentalOffice.PageIndex = e.NewPageIndex;
            FillDataToGridView();
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            FillDataToGridView();
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtName.Text = "";
            dlStaff.SelectedIndex = 0;
        }
    
}
