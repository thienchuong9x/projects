using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;
using System.Web.Security;

public partial class Staffs : DDVPortalModuleBase 
{
    readonly static ILog logger = LogManager.GetLogger(typeof(Staffs));

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                InitLanguage();
                hiddenOfficeCd.Value = GetOffice();

                btDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
                Initiation();
                FillOfficeDropDownList();
                FillDataToGridView();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    private void Initiation()
    {

        lstContent.EmptyDataText = GetResource("NoRecordFound.Text");
        SetButtonText(btRegister, btEdit, btSave, btDelete, btCancel);


        lbUserId.Text = GetResource("lbUserId.Text");
        lbStaffCd.Text = GetResource("lbStaffCd.Text");
        lbStaffNm.Text = GetResource("lbStaffNm.Text");
        lbStaffNmKana.Text = GetResource("lbStaffNmKana.Text");
        lbOfficeCd.Text = GetResource("lbOfficeCd.Text");
        lbOffice.Text = GetResource("lbOfficeCd.Text");

        lbSalesman.Text = GetResource("lbSalesman.Text");
        lbTechnician.Text = GetResource("lbTechnician.Text");
        lbAuthority.Text = GetResource("lbAuthority.Text");
        lbIsDeleted.Text = GetResource("lbIsDeleted.Text");
        lbLogin.Text = GetResource("lbLogin.Text");
        cbLogin.Text = GetResource("cbLogin.Text");
        btRegister.Text = GetResource("btRegister.Text");
        btCancel.Text = GetResource("btCancel.Text");
        btEdit.Text = GetResource("btEdit.Text");
        btSave.Text = GetResource("btSave.Text");
        btDelete.Text = GetResource("btDelete.Text");
        //btResetUser.Text = GetResource("btResetUser.Text");

        lstContent.EmptyDataText = GetResource("NoRecordFound.Text");
        valNumbersOnly_StaffCd.ErrorMessage = GetResource("NumberOnly.Text");
        valRequired_StaffNm.ErrorMessage = GetResource("StaffNameIsRequired.Text");

        valRequired_StaffCd.ErrorMessage = GetResource("StaffCodeEmpty.Text");
        //rqUserId.ErrorMessage = GetResource("requireField.ErrorMessage");
        //lbWrongUserId.Text = GetResource("WrongUser.Text");
        lstContent.EmptyDataText = GetResource("NoRecordFound.Text");
        lbStaffCdExist.Text = GetResource("MSG_STAFFCDEXIST.Text");

        lblCode.Text = GetResource("lblCode_Code.Text");
        lblName.Text = GetResource("lblName_Name.Text");
        cbIncludeIsDeleted.Text = GetResource("cbIncludeIsDeleted.Text");
        btSearch.Text = GetResource("btSearch.Text");
        btClear.Text = GetResource("btClear.Text");
        lbRowPerPage.Text = GetResource("lbRowsPage.Text");
        //cbAuthority.Items.Add(new ListItem("", ""));
        cbAuthority.Items.Add(new ListItem("Member", "Member"));  //GetResource("lbMember.Text")
        cbAuthority.Items.Add(new ListItem("OfficeChief", "OfficeChief")); //GetResource("lbOfficeChief.Text")

    }

    private void FillOfficeDropDownList()
    {
        cbOffice.Items.Clear();
        cbOffice.Items.Add(new ListItem("", ""));

        List<MasterOffice> list = MasterOffice.GetOfficeMasters();

        foreach (MasterOffice i in list)
        {
            cbOffice.Items.Add(new ListItem(i.OfficeNm, i.OfficeCd.ToString()));
        }
    }

    private void FillDataToGridView()
    {
        lstContent.DataSource = MasterStaff.GetStaffMasterSearch(txtCode.Text, txtName.Text, cbOffice.SelectedValue, cbIncludeIsDeleted.Checked);
        lstContent.Columns[1].Visible = true;
        lstContent.DataBind();

        foreach (GridViewRow row in lstContent.Rows)
        {
            //if (row.Cells[2].Text == "-1")
            //    row.Cells[2].Text = "";

            if (row.Cells[5].Text == "True")
                row.Cells[5].Text = "√";
            else row.Cells[5].Text = "";

            if (row.Cells[6].Text == "True")
                row.Cells[6].Text = "√";
            else row.Cells[6].Text = "";

            if (row.Cells[8].Text == "True" || row.Cells[7].Text == "datadesign")
                row.Cells[8].Text = "√";
            else row.Cells[8].Text = "";

            if (row.Cells[9].Text == "True")
                row.Cells[9].Text = "√";
            else row.Cells[9].Text = "";

            if (row.Cells[10].Text == "True")
                row.Cells[10].Text = "√";
            else row.Cells[10].Text = "";

        }
        lstContent.Columns[1].Visible = false;

    }

//===================================

    protected void cbNewUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtNewUserId.Text = cbNewUser.SelectedValue;
        //txtUserName.Text = cbNewUser.SelectedItem.Text;

        //if (txtStaffNm.Text == "" || txtFlag.Text == "10")
        //    txtStaffNm.Text = _staffController.GetDisplayName(txtUserName.Text);
        //if (txtFlag.Text == "10" || txtFlag.Text == "12")
        //    txtUserId.Text = txtNewUserId.Text;

        //if (txtUserName.Text == "")
        //{
        //    //lbWrongUserId.Visible = false;
        //    txtNewUserId.Text = "";

        //    gvOffice.Enabled = false;
        //    cbLogin.Enabled = false;
        //}
        //else
        //{
        //    gvOffice.Enabled = true;
        //    cbLogin.Enabled = true;
        //}

        //gvOffice.Columns[2].Visible = true;
        ////Xoa Grid Office
        //MstOfficeController ocon = new MstOfficeController();
        //gvOffice.DataSource = ocon.GetMstOffices();
        //gvOffice.DataBind();
        //gvOffice.Columns[2].Visible = false;

        //cbLogin.Checked = false;
        //cbAuthority.Enabled = false;
        //cbAuthority.SelectedIndex = 0;

        ////Load lai Office
        //if (txtFlag.Text == "11" && txtNewUserId.Text == txtUserId.Text)
        //{
        //    StaffController cStaff = new StaffController();

        //    StaffInfo objStaff = cStaff.GetStaff1(int.Parse(txtUserId.Text));
        //    cbLogin.Checked = objStaff.AllowLogin;
        //    if (cbLogin.Checked == false)
        //        cbAuthority.Enabled = false;
        //    else
        //    {
        //        if (cStaff.GetCountRole(int.Parse(txtUserId.Text), 24) > 0)
        //        {
        //            cbAuthority.SelectedIndex = 2;
        //        }
        //        else
        //            cbAuthority.SelectedIndex = 1;
        //    }

        //    gvOffice.Columns[2].Visible = true;
        //    string tmp = null;
        //    foreach (GridViewRow msgRow in gvOffice.Rows)
        //    {
        //        CheckBox ck = (CheckBox)msgRow.FindControl("Check");
        //        tmp = gvOffice.Rows[msgRow.RowIndex].Cells[2].Text;
        //        WriteLog("Time= " + DateTime.Now.ToString() + ":" + "Check: " + ck.Checked.ToString() + ":" + "Off: " + tmp);
        //        if (!string.IsNullOrWhiteSpace(tmp))
        //            if (cStaff.GetCountRole(int.Parse(txtUserId.Text), int.Parse(tmp)) > 0)
        //            {
        //                ck.Checked = true;
        //            }
        //    }
        //    gvOffice.Columns[2].Visible = false;

        //}

    }

    protected void cbLogin_CheckedChanged(object sender, EventArgs e)
    {
        if (cbLogin.Checked)
        {
            cbAuthority.Enabled = true;
            cbAuthority.SelectedIndex = 1;
        }
        else
        {
            cbAuthority.Enabled = false;
            cbAuthority.SelectedIndex = 0;
        }
    }

    protected void btRegister_Click(object sender, EventArgs e)
    {

        txtStaffCd.Text = string.Empty;
        txtStaffNm.Text = string.Empty;
        txtStaffNmKana.Text = string.Empty;

        cbSalesman.Checked = false;
        cbTechnician.Checked = false;

        gvOffice.Columns[2].Visible = true;

        //Add New Office code
        gvOffice.DataSource = MasterOffice.GetOfficeMasters();
        gvOffice.DataBind();

        //Add combobox New User Name
        cbNewUser.Items.Clear();
        cbNewUser.Items.Add(new ListItem(""));
        var sList = Membership.GetAllUsers();
        foreach (MembershipUser si in sList)
        {
            if (MasterStaff.countUser(si.UserName) <= 0)
                cbNewUser.Items.Add(new ListItem(si.UserName));
        }
        cbNewUser.SelectedIndex = 0;

        txtStaffCd.Enabled = true;
        txtStaffNm.Enabled = true;

        cbLogin.Checked = false;
        //cbAuthority.Enabled = false;
        cbAuthority.SelectedIndex = 0;
        btDelete.Enabled = false;
        cbLogin.Enabled = false;

        //Hide collumn code
        gvOffice.Columns[2].Visible = false;

        Panel1.Visible = true;
        Panel2.Visible = false;
        txtFlag.Text = "10";
    }

    protected void btEdit_Click(object sender, EventArgs e)
    {
        string tmp0 = null;
        string tmp1 = null;
        int i = 0;
        foreach (GridViewRow msgRow in lstContent.Rows)
        {
            CheckBox ck = (CheckBox)msgRow.FindControl("Check");
            if (ck.Checked == true)
            {
                tmp0 = lstContent.Rows[msgRow.RowIndex].Cells[1].Text;
                tmp1 = lstContent.Rows[msgRow.RowIndex].Cells[2].Text;
                i++;
            }
        }

        try
        {
            if (i == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            }
            else if (i > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                //Show column code
                gvOffice.Columns[2].Visible = true;
                MasterStaff objStaff = new MasterStaff();

                txtFlag.Text = "11"; // Ko dung den nua
                txtStaffCd.Text = tmp1;

                objStaff = MasterStaff.GetStaff(int.Parse(txtStaffCd.Text));
                txtStaffCd.Text = objStaff.StaffCd.ToString();
                txtStaffNm.Text = objStaff.StaffNm;
                txtStaffNmKana.Text = objStaff.StaffNmKana;
                
                if (!objStaff.SalesFlg.HasValue)
                    cbSalesman.Checked = false;
                else
                {
                    if ((bool)objStaff.SalesFlg)
                        cbSalesman.Checked = true;
                    else
                        cbSalesman.Checked = false;
                }

                if (!objStaff.TechFlg.HasValue)
                    cbTechnician.Checked = false;
                else
                {
                    if ((bool)objStaff.TechFlg)
                        cbTechnician.Checked = true;
                    else
                        cbTechnician.Checked = false;
                }

                
                //cbSalesman.Checked = objStaff.SalesFlg;
                //cbTechnician.Checked = objStaff.TechFlg;
            //    cbLogin.Checked = objStaff.AllowLogin;
                //if (txtUserName.Text == "datadesign") cbLogin.Checked = true;
                //if (cbLogin.Checked == false)
                //{
                //    cbAuthority.Enabled = false;
                //    cbAuthority.SelectedIndex = 0;
                //}

                cbIsDeleted.Checked = objStaff.IsDeleted;
                btDelete.Enabled = true;
                //cbLogin.Enabled = true;
                txtStaffCd.Enabled = false;

                //Add combobox New User Name
                cbNewUser.Items.Clear();
                cbNewUser.Items.Add(new ListItem(""));
                if (!string.IsNullOrWhiteSpace(objStaff.UserId))
                    cbNewUser.Items.Add(objStaff.UserId);

                var sList = Membership.GetAllUsers();
                foreach (MembershipUser si in sList)
                {
                    if (MasterStaff.countUser(si.UserName) <= 0)
                        cbNewUser.Items.Add(new ListItem(si.UserName));
                }
                if (!string.IsNullOrWhiteSpace(objStaff.UserId))
                    cbNewUser.SelectedIndex = 1;
                else
                    cbNewUser.SelectedIndex = 0;

                //Edit Office code
                List<MasterOffice> list = MasterOffice.GetOfficeMasters();
                gvOffice.DataSource = list;
                gvOffice.DataBind();

                string mOffice = "";
                foreach (GridViewRow msgRow in gvOffice.Rows)
                {
                    CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                    if (DentalPermission.HasRole(int.Parse(txtStaffCd.Text), int.Parse(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text)) > 0)
                    {
                        ck.Checked = true;
                    }
                    mOffice = gvOffice.Rows[msgRow.RowIndex].Cells[2].Text;
                }

                cbAuthority.SelectedIndex = DentalPermission.HasAuthority(int.Parse(txtStaffCd.Text), int.Parse(mOffice));

                //    //Fix code cho Group Sales = 12; Co the goi qua GetFunctions
                //    //StaffController cStaff = new StaffController();
                //    //List<RoleInfo> lStaff = cStaff.GetFunctions(UserId);
                //    //Get RoleId while RoleName="Salesman"

                    //if (MasterUserRoles.isSalesman(int.Parse(txtStaffCd.Text)) > 0)
                    //{
                    //    cbSalesman.Checked = true;
                    //}

                //    //Fix code cho Group Tech = 13; Co the goi qua GetFunctions: 
                //    //StaffController cStaff = new StaffController();
                //    //List<RoleInfo> lStaff = cStaff.GetFunctions(UserId);
                //    //Get RoleId while RoleName="Technician"

                    //if (MasterUserRoles.isTechnician(int.Parse(txtStaffCd.Text)) > 0)
                    //{
                    //    cbTechnician.Checked = true;
                    //}

                //    //Future: Foreach cbAuthority, roi kiem tra GetCountRole, roi set item tuong ung
                //    if (cbLogin.Checked == true)
                //    {
                //        if (cStaff.GetCountRole(int.Parse(txtUserId.Text), 24) > 0)
                //        {
                //            cbAuthority.SelectedIndex = 2;
                //        }
                //        else
                //            cbAuthority.SelectedIndex = 1;
                //    }


                //Hide collumn code
                gvOffice.Columns[2].Visible = false;

                Panel1.Visible = true;
                Panel2.Visible = false;
                //btDelete.Enabled = true;


            }
        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");");
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterStaff row = new MasterStaff();
            row.UserId = cbNewUser.SelectedValue;
            row.StaffCd = int.Parse(txtStaffCd.Text);
            row.StaffNm = txtStaffNm.Text;
            row.StaffNmKana = txtStaffNmKana.Text;
            if (cbSalesman.Checked) row.SalesFlg = true;
            if (cbTechnician.Checked) row.TechFlg = true;
            //if (cbLogin.Checked) row.AllowLogin = true;

            if (txtFlag.Text == "10") //New Staff
            {
                //Check StaffCd is Unike
                if (MasterStaff.GetStaff(int.Parse(txtStaffCd.Text)) != null)
                {
                    //RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_STAFFCDEXIST.Text")) + "\");");
                    lbStaffCdExist.Visible = true;
                    txtStaffCd.Focus();
                    return;
                }
                else
                    lbStaffCdExist.Visible = false;

                row.CreateAccount = User.Identity.Name;
                row.ModifiedDate = row.CreateDate = DateTime.Now;
                row.Insert();
            }

            if (txtFlag.Text == "11" || txtFlag.Text == "12") //Edit with record of Staff
            {
                //if (cbIsDeleted.Checked) row.IsDeleted = true;   else row.IsDeleted = false;
                row.ModifiedAccount = User.Identity.Name;
                row.ModifiedDate = DateTime.Now;
                row.Update();
            }

            string auth="";
            foreach (GridViewRow msgRow in gvOffice.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");

                if (cbAuthority.SelectedIndex == 1)
                    auth = "OfficeChief";
                else
                    auth = "Member";
                if (!string.IsNullOrWhiteSpace(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text))
                    AddRemoveRole(int.Parse(txtStaffCd.Text), int.Parse(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text), auth, ck.Checked);
            }

        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
            //WriteLog("Error UpdateRoles = " + ex.Message);
            //RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
        finally
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            FillDataToGridView();
            btDelete.Enabled = true;
        }
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        //FillData();
        Panel1.Visible = false;
        Panel2.Visible = true;
    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        //StaffController objStaffs = new StaffController();
        //objStaffs.DeleteStaff(txtUserId.Text, SafeCast.AsInt32(txtStaffCd.Text), UserInfo.Username);

        //// get the content from the Staff table 
        //FillData();

        Panel1.Visible = false;
        Panel2.Visible = true;
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        FillDataToGridView();
    }

    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtName.Text = "";
        cbOffice.SelectedIndex = 0;
    }

    protected void dlNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstContent.PageSize = int.Parse(dlNumber.SelectedValue);
        btSearch_Click(sender, e);
    }

    protected void lstContent_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();


            //Add CheckBoxSelected
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            ////Add UserId
            //oTableCell = new TableHeaderCell();
            //oTableCell.Text = GetResource("lbUserId.Text");
            //oTableCell.CssClass = "td_header";
            //oGridViewRow.Cells.Add(oTableCell);

            //Add StaffCode
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbStaffCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add StaffName
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbStaffNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add StaffName Kana
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbStaffNmKana.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Salesman
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbSalesman.Text");
            if (oTableCell.Text == "") oTableCell.Text = "Sales";
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Technician
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbTechnician.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add UserName
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbUserId.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Allow Login
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("cbLogin.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Chief
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbChief.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add IsDeleted
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbIsDeleted.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);


            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void btCreateFolder_Click(object sender, EventArgs e)
    {
        //if (!Directory.Exists(this.PortalSettings.HomeDirectoryMapPath + "\\XMLOrder"))
        //    Directory.CreateDirectory(this.PortalSettings.HomeDirectoryMapPath + "\\XMLOrder");

        //if (!Directory.Exists(this.PortalSettings.HomeDirectoryMapPath + "\\XMLOrder\\sub"))
        //    Directory.CreateDirectory(this.PortalSettings.HomeDirectoryMapPath + "\\XMLOrder\\sub");

    }

    protected void lstContent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lstContent.PageIndex = e.NewPageIndex;
        FillDataToGridView();
    }

    protected void RemoveRoles(string txtUserId_)
    {
        try
        {
            MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 12, false);
            MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 13, false);
            //Xoa Chef
            //MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 24, false);

            //Xoa UserRole
            gvOffice.Enabled = true;
            gvOffice.Columns[2].Visible = true;

            foreach (GridViewRow msgRow in gvOffice.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                logger.Info("Time= " + DateTime.Now.ToString() + ":" + "U= " + txtUserId_ + ":" + "L= " + ck.Checked.ToString() + ":" + "R= " + gvOffice.Rows[msgRow.RowIndex].Cells[2].Text);

                if (!string.IsNullOrWhiteSpace(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text))//(ck.Checked)
                    MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), int.Parse(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text), false);
            }
            gvOffice.Columns[2].Visible = false;

        }
        catch (Exception ex)
        {
            //RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            //WriteLog(DateTime.Now.ToString() + ":Error RemoveRoles: " + ex.Message);
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);

        }
    }

    protected void UpdateRoles(string txtUserId_)
    {
        try
        {
            //Membership.GetAllUsers

            MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 12, cbSalesman.Checked);

            MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 13, cbTechnician.Checked);

            //if (cbAuthority.SelectedIndex == 2)
            //    MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 24, true);
            //else
            //    MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), 24, false);

            //Luu vao table UserRole
            gvOffice.Enabled = true;
            gvOffice.Columns[2].Visible = true;

            foreach (GridViewRow msgRow in gvOffice.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                logger.Info("Time= " + DateTime.Now.ToString() + ":" + "U= " + txtUserId_ + ":" + "L= " + ck.Checked.ToString() + ":" + "R= " + gvOffice.Rows[msgRow.RowIndex].Cells[2].Text);
                if (!string.IsNullOrWhiteSpace(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text))//(ck.Checked)
                    MasterUserRoles.AddRemoveUserRole(int.Parse(txtUserId_), int.Parse(gvOffice.Rows[msgRow.RowIndex].Cells[2].Text), ck.Checked);
            }
            gvOffice.Columns[2].Visible = false;

        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
            //RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
            //WriteLog(DateTime.Now.ToString() + ": Error UpdateRoles: " + ex.Message);
        }
    }
    public static void AddRemoveRole(int StaffCd, int OfficeCd, string permission, Boolean isAdd)
    {
        DentalPermission tmp = DentalPermission.GetRole(StaffCd, OfficeCd);

        if (tmp != null)
        {
            if (!isAdd)
                tmp.Delete();
            else
            {
                tmp.StaffCd = StaffCd;
                tmp.OfficeCd = OfficeCd;
                tmp.Permission = permission;
                tmp.ModifiedDate = DateTime.Now;
                tmp.ModifiedAccount = HttpContext.Current.User.Identity.Name;
                tmp.Update();

            }
        }
        else
        {
            if (isAdd)
            {
                tmp = new DentalPermission();
                tmp.StaffCd = StaffCd;
                tmp.OfficeCd = OfficeCd;
                tmp.Permission = permission;
                tmp.CreateDate = DateTime.Now;
                tmp.CreateAccount = HttpContext.Current.User.Identity.Name;
                tmp.Insert();
            }
        }
    }

}


//MembershipUser m = Membership.GetUser(HttpContext.Current.User.Identity.Name);
//string UserID =  m.ProviderUserKey.ToString();