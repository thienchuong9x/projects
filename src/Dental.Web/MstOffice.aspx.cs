using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstOffice : DDVPortalModuleBase
{
    readonly static ILog logger = LogManager.GetLogger(typeof(MstOffice));

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
                //FillStaffDropDownList();
                //FillBillDropDownList();
                FillDataToGridView();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }


    protected void gridViewOffice_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            var checkedIDs = (from GridViewRow msgRow in gridViewOffice.Rows
                              where ((CheckBox)msgRow.FindControl("Check")).Checked
                              select Convert.ToInt32(gridViewOffice.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();
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

                MasterOffice info = new MasterOffice();
                //info.OfficeCd = int.Parse(hiddenOfficeCd.Value);
                info.OfficeCd = Convert.ToInt32(checkedIDs[0].ToString());
                info = info.GetByPrimaryKey();
                SetOfficeMaster(info);
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
        ResetOfficeMaster();
        SetVisibleHeader(false);
        ButtonDelete.Enabled = false;
    }

    private void Initiation()
    {
        lblCode.Text = GetResource("lblCode.Text");
        lblName.Text = GetResource("lblName.Text");
        btSearch.Text = GetResource("btSearch.Text");
        btClear.Text = GetResource("btClear.Text");

        //lblModuleTitle.Text = GetResource("lblDentalOffice.Text");
        LabelDentalOfficeCd.Text = GetResource("HEADER_OFFICE_DentalOfficeCd.Text");
        LabelDentalOfficeNm.Text = GetResource("HEADER_OFFICE_DentalOfficeNm.Text");
        LabelDentalOfficePostalCd.Text = GetResource("HEADER_OFFICE_DentalOfficePostalCd.Text");
        LabelDentalOfficeAddress1.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress1.Text");
        LabelDentalOfficeAddress2.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress2.Text");
        LabelDentalOfficeFAX.Text = GetResource("HEADER_OFFICE_DentalOfficeFAX.Text");
        LabelDentalOfficeTEL.Text = GetResource("HEADER_OFFICE_DentalOfficeTEL.Text");
        valRequiredInput_DentalOfficeCd.ErrorMessage = valRequiredInput_DentalOfficeCd.ToolTip = valRequiredNumber_DentalOfficeCd.ErrorMessage = valRequiredNumber_DentalOfficeCd.ToolTip = GetResource("valRequired_DentalOfficeCd.Text");
        valRequired_DentalOfficeNm.ErrorMessage = valRequired_DentalOfficeNm.ErrorMessage = GetResource("valRequired_DentalOfficeNm.Text");

        gridViewOffice.EmptyDataText = GetResource("NoRecordFound.Text");
        SetButtonText(btnRegister, btnEdit, ButtonSave, ButtonDelete, ButtonCancel);
    }
    
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterOffice row = MasterOffice.GetOfficeMaster(Convert.ToInt32(txtDentalOfficeCd.Text));
            //MasterRoles row1 = MasterRoles.GetRoleMaster(Convert.ToInt32(txtDentalOfficeCd.Text));
            if (ButtonDelete.Enabled == false)
            {
                if (row == null)
                {
                    row = GetOfficeMaster();
                    row.Insert();
                    //row1 = GetRoleInfo();
                    //row1.Insert();

                }
                else
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_EXIST_DENTAL_OFFICE_CD.Text")) + "\");");
                    return;
                }
            }
            else
            {
                row = GetOfficeMaster();
                row.Update();
                //row1 = GetRoleInfo();
                //row1.Update();
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
            CodeName item = list.FirstOrDefault(p => p.Code == code);
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
        List<MasterOffice> colOfficeMasters = MasterOffice.GetOfficeMasterSearch(txtCode.Text, txtName.Text);
        if (colOfficeMasters != null && colOfficeMasters.Count > 0)
        {
            //foreach (MasterOffice i in colOfficeMasters)
            //{

            //}

            gridViewOffice.DataSource = colOfficeMasters;
            gridViewOffice.DataBind();
        }
        else
        {
            colOfficeMasters = new List<MasterOffice>();
            colOfficeMasters.Add(new MasterOffice());
            gridViewOffice.DataSource = colOfficeMasters;
            gridViewOffice.DataBind();
            gridViewOffice.Rows[0].Visible = false;
        }
    }
    
    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (!txtDentalOfficeCd.Enabled)
            {
                //MasterUserRoles.RemoveUserRole(Convert.ToInt32(txtDentalOfficeCd.Text));

                MasterOffice item = MasterOffice.GetOfficeMaster(Convert.ToInt32(txtDentalOfficeCd.Text));
                item.Delete();
                //MasterRoles item1 = MasterRoles.GetRoleMaster(Convert.ToInt32(txtDentalOfficeCd.Text));
                //item1.Delete();

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

    private MasterRoles GetRoleInfo()
    {
        MasterRoles item = new MasterRoles();
        //item.OfficeCd = int.Parse(hiddenOfficeCd.Value);
        item.RoleID = Convert.ToInt32(txtDentalOfficeCd.Text);
        item.RoleName = TextDentalOfficeNm.Text;
        item.RoleGroupID = 3;
        item.LastModifiedOnDate = item.CreatedOnDate = DateTime.Now;
        //item.CreatedByUserID = item.LastModifiedByUserID = this.User.Identity.ToString();


        return item;
    }

    private MasterOffice GetOfficeMaster()
    {
        MasterOffice item = new MasterOffice();
        //item.OfficeCd = int.Parse(hiddenOfficeCd.Value);
        item.OfficeCd = Convert.ToInt32(txtDentalOfficeCd.Text);
        item.OfficeNm = TextDentalOfficeNm.Text;
        //item.OfficeAbbNm = TextDentalOfficeAbbNm.Text;
        item.OfficePostalCd = TextDentalOfficePostalCd.Text;
        item.OfficeAddress1 = TextDentalOfficeAddress1.Text;
        item.OfficeAddress2 = TextDentalOfficeAddress2.Text;
        item.OfficeFAX = TextDentalOfficeFAX.Text;
        item.OfficeTEL = TextDentalOfficeTEL.Text;
        //dental.TransferDays = Common.GetNullableInt(TextTransferDays.Text);
        item.CreateAccount = item.ModifiedAccount = this.User.Identity.Name;
        //item.StaffCd = Common.GetNullableInt(dropDownStaff.SelectedValue);
        //item.BillCd = Convert.ToInt32(DropDownBill.SelectedValue);
        return item;
    }
    
    private void SetOfficeMaster(MasterOffice dental)
    {
        txtDentalOfficeCd.Text = dental.OfficeCd.ToString();
        TextDentalOfficeNm.Text = dental.OfficeNm;
        //TextDentalOfficeAbbNm.Text = dental.DentalOfficeAbbNm;
        TextDentalOfficePostalCd.Text = dental.OfficePostalCd;
        TextDentalOfficeAddress1.Text = dental.OfficeAddress1;
        TextDentalOfficeAddress2.Text = dental.OfficeAddress2;
        TextDentalOfficeFAX.Text = dental.OfficeFAX;
        TextDentalOfficeTEL.Text = dental.OfficeTEL;
        //TextTransferDays.Text = Common.GetNullableString(dental.TransferDays);
        //SetDropDownValue(dropDownStaff, Common.GetNullableString(dental.StaffCd));
        //SetDropDownValue(DropDownBill, dental.BillCd.ToString());
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
    
    private void ResetOfficeMaster()
    {
        txtDentalOfficeCd.Text = "";
        TextDentalOfficeNm.Text = "";
        //TextDentalOfficeAbbNm.Text = "";
        TextDentalOfficePostalCd.Text = "";
        TextDentalOfficeAddress1.Text = "";
        TextDentalOfficeAddress2.Text = "";
        TextDentalOfficeFAX.Text = "";
        TextDentalOfficeTEL.Text = "";
        //TextTransferDays.Text = "";
        //dropDownStaff.Text = "";
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

    protected void gridViewOffice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridViewOffice.PageIndex = e.NewPageIndex;
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
        //dlStaff.SelectedIndex = 0;
    }


    //private void FillStaffDropDownList()
    //{
    //    dropDownStaff.Items.Clear();
    //    dropDownStaff.Items.Add(new ListItem("", ""));

    //    List<MasterStaff> list = MasterStaff.GetStaffwiOffice(int.Parse(hiddenOfficeCd.Value));
    //    dlStaff.Items.Add("");

    //    foreach (MasterStaff i in list)
    //    {
    //        if (i.SalesFlg.HasValue && i.SalesFlg.Value == true)
    //        {
    //            dropDownStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
    //            dlStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
    //        }
    //    }
    //}
    //private void FillBillDropDownList()
    //{
    //    logger.Debug("FillBillDropDownList");
    //    DropDownBill.Items.Clear();

    //    List<MasterBill> listBill = MasterBill.GetBillMasters(int.Parse(hiddenOfficeCd.Value));
    //    logger.Debug("listbill.count = " + listBill.Count);
    //    foreach (MasterBill i in listBill)
    //    {
    //        DropDownBill.Items.Add(new ListItem(i.BillNm, i.BillCd.ToString()));
    //    }
    //}

    //protected void cbOffice_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtOffice.Text = cbOffice.SelectedValue;

    //    //Luu cookie
    //    HttpCookie ck = Request.Cookies[UserInfo.Username];
    //    if (ck == null)
    //    {
    //        //Response.Cookies.Remove("OfficeCd");
    //        ck = new HttpCookie(UserInfo.Username);
    //    }

    //    ck["OfficeCd"] = cbOffice.SelectedValue;
    //    ck.Expires = DateTime.Now.AddDays(15);
    //    Response.Cookies.Add(ck);

    //    Response.Redirect(Request.RawUrl);   
    //}


}
