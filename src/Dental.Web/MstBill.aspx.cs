using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstBill : DDVPortalModuleBase
{

    private static readonly ILog logger = LogManager.GetLogger(typeof(MstBill));

    protected void Page_Load(object sender, System.EventArgs e)
    {        
        if (!IsPostBack)
        {
            try
            {
                InitLanguage();
                logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
                
                HiddenFieldOfficeCd.Value = GetOffice();

                btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

                // Get Message resource                   
                lbCode.Text = lbDentalBillCd.Text = GetResource("lbDentalBillCd.Text");
                lbName.Text = lbDentalBillNm.Text = GetResource("lbDentalBillNm.Text");
                lbDentalBillStatementNm.Text = GetResource("lbDentalBillStatementNm.Text");
                lbDentalCreditLimit.Text = GetResource("lbDentalCreditLimit.Text");
                lbDentalBillFlg.Text = GetResource("lbDentalBillFlg.Text");
                lbBillClosingDay.Text = GetResource("lbBillClosingDay.Text");
                lbBankAccount.Text = GetResource("lbDentalBankAccount.Text");
                lbSupplierNm.Text = GetResource("lbSupplierNm.Text");

                btnRegister.Text = GetResource("btnRegister.Text");
                btnEdit.Text = GetResource("btnEdit.Text");
                btnSave.Text = GetResource("btnSave.Text");
                btnDelete.Text = GetResource("btnDelete.Text");
                btnCancel.Text = GetResource("btnCancel.Text");
                btnSearch.Text = GetResource("btnSearch.Text");
                btnClear.Text = GetResource("btnClear.Text");

                lblModuleTitle.Text = GetResource("lblModuleTitle.Text");
                lbDentalBillPostalCd.Text = GetResource("lbDentalBillPostalCd.Text");
                lbDentalBillAddress1.Text = GetResource("lbDentalBillAddress1.Text");
                lbDentalBillAddress2.Text = GetResource("lbDentalBillAddress2.Text");
                lbDentalBillTel.Text = GetResource("lbDentalBillTel.Text");
                lbDentalBillFax.Text = GetResource("lbDentalBillFax.Text");
                lbBillContactPerson.Text = GetResource("lbBillContactPerson.Text");

                Val_BilCd.ErrorMessage = GetResource("NumberOnly.Text");
                ValRe_BillCd.ErrorMessage = GetResource("BillCodeEmpty.Text");
                ValRe_BillNm.ErrorMessage = GetResource("BillNameEmpty.Text");
                Val_CreditLimit.ErrorMessage = GetResource("CreditLimit.Text");
                ValRe_BillClosingDay.ErrorMessage = GetResource("BillClosingDayEmpty.Text");
                REV_PostalCd.ErrorMessage = GetResource("PostalCd.Text");
                // Load Data                     
                FillDropdownlist();
                FillDropdownlistClosingDay();
                FillDropdownlistSupplier();
                FillDropdownlistBankAccount();
                btnSearch_Click(sender, e);
            }

            catch (Exception ex)
            {
                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
    }

    #region Fill All
    /// <summary>
    /// Fill data
    /// </summary>
    protected void FillData(List<MasterBill> ListBill)
    {
        // ListBill not Record    

        grvBill.Columns[3].Visible = true;
        grvBill.Columns[10].Visible = true;
        grvBill.Columns[11].Visible = true;
        grvBill.Columns[13].Visible = true;
        grvBill.Columns[14].Visible = true;
        grvBill.Columns[15].Visible = true;
        grvBill.Columns[16].Visible = true;
        if (ListBill.Count == 0)
        {
            MasterBill objBillInFo = new MasterBill();
            objBillInFo.OfficeCd = -1;
            objBillInFo.BillCd = -1;
            objBillInFo.BillNm = "";
            objBillInFo.BillStatementNm = "";
            objBillInFo.BillFlg = null;
            objBillInFo.CreditLimit = null;

            ListBill.Add(objBillInFo);
            grvBill.DataSource = ListBill;
            grvBill.DataBind();

            if (ListBill.Count == 1 && ListBill[0].BillCd == -1)
            {
                grvBill.Rows[0].Visible = false;
            }
        }
        // load data to Gridview
        else
        {
            grvBill.DataSource = ListBill;
            grvBill.DataBind();
            foreach (GridViewRow row in grvBill.Rows)
            {
                if (row.Cells[11].Text == "1")
                    row.Cells[11].Text = GetResource("FlgTogether.Text");
                if (row.Cells[11].Text == "2")
                    row.Cells[11].Text = GetResource("FlgSeparate.Text");
                else row.Cells[11].Text = row.Cells[11].Text;

                if (row.Cells[12].Text == "25")
                    row.Cells[12].Text = GetResource("ClosingDay25th.Text");
                else if (row.Cells[12].Text == "20")
                    row.Cells[12].Text = GetResource("ClosingDay20th.Text");
                else row.Cells[12].Text = GetResource("ClosingDayLastDay.Text");

                ListItem item1 = DropDownListBankAccount.Items.FindByValue(row.Cells[15].Text);
                if (item1 != null)
                    row.Cells[13].Text = item1.Text;
                else row.Cells[13].Text = "";

                ListItem item = DropDownListSupplier.Items.FindByValue(row.Cells[16].Text);
                if (item != null)
                    row.Cells[14].Text = item.Text;
                else row.Cells[14].Text = "";
            }
        }
        grvBill.Columns[3].Visible = false;
        grvBill.Columns[10].Visible = false;
        grvBill.Columns[11].Visible = false;
        grvBill.Columns[13].Visible = false;
        grvBill.Columns[14].Visible = false;
        grvBill.Columns[15].Visible = false;
        grvBill.Columns[16].Visible = false;

        PnView.Visible = true;
        PnEdit.Visible = false;
    }
    /// <summary>
    /// FillDropdownlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void FillDropdownlist()
    {
        DropDownListBillFlg.Items.Add(new ListItem("", ""));
        DropDownListBillFlg.Items.Add(new ListItem(GetResource("FlgTogether.Text"), "1"));
        DropDownListBillFlg.Items.Add(new ListItem(GetResource("FlgSeparate.Text"), "2"));
    }
    /// <summary>
    /// FillDropdownlistClosingDay
    /// </summary>
    protected void FillDropdownlistClosingDay()
    {
        DropDownListBillClosingDay.Items.Add(new ListItem(GetResource("ClosingDayLastDay.Text"), "0"));
        DropDownListBillClosingDay.Items.Add(new ListItem(GetResource("ClosingDay25th.Text"), "25"));
        DropDownListBillClosingDay.Items.Add(new ListItem(GetResource("ClosingDay20th.Text"), "20"));
    }
    ///
    /// <summary>
    /// FillDropdownlistClosingDay
    /// </summary>
    protected void FillDropdownlistBankAccount()
    {
        List<MasterBank> listbank = new List<MasterBank>();
        listbank = MasterBank.GetBankForFlg(Convert.ToInt32(HiddenFieldOfficeCd.Value), false, true);
        DropDownListBankAccount.Items.Add(new ListItem("", ""));
        foreach (MasterBank i in listbank)
        {
            DropDownListBankAccount.Items.Add(new ListItem(i.BankAccount, i.BankCd.ToString()));
        }

    }
    /// <summary>
    /// FillDropdownlistSupplier
    /// </summary>
    protected void FillDropdownlistSupplier()
    {
        //PayMentController objPaymentCon = new PayMentController();
        List<MasterSupplier> listSupplier = new List<MasterSupplier>();
        listSupplier = MasterSupplier.GetSupplierMasters(Convert.ToInt32(HiddenFieldOfficeCd.Value));
        DropDownListSupplier.Items.Add(new ListItem("", ""));
        foreach (MasterSupplier i in listSupplier)
        {
            DropDownListSupplier.Items.Add(new ListItem(i.SupplierNm, i.SupplierCd.ToString()));
        }

    }
    #endregion

    #region Event Click
    /// <summary>
    /// Event register Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = string.Empty;
        txtDentalBillCd.Enabled = true;
        txtDentalBillCd.Text = "";
        txtDentalBillNm.Text = "";
        txtDentalBillStatementNm.Text = "";
        txtDentalBillPostalCd.Text = "";
        txtDentalBillAddress1.Text = "";
        txtDentalBillAddress2.Text = "";
        txtDentalBillTel.Text = "";
        txtDentalBillFax.Text = "";
        txtBillContactPerson.Text = "";
        txtDentalCreditLimit.Text = "";
        DropDownListBillFlg.Text = "";
        DropDownListBankAccount.Text = "";
        DropDownListBillClosingDay.SelectedValue = "0";
        DropDownListSupplier.Text = "";
        btnDelete.Enabled = false;
        PnEdit.Visible = true;
        PnView.Visible = false;

    }
    /// <summary>
    /// Event Edit Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = string.Empty;
        try
        {
            string Code = null;
            int total = 0;

            foreach (GridViewRow gvr in grvBill.Rows)
            {
                CheckBox cb = (CheckBox)gvr.FindControl("CheckEdit");
                if (cb.Checked)// Check CheckBox checked
                {
                    total++;
                    if (Code == null)
                        Code = gvr.Cells[1].Text;
                    if (total == 2)
                        break;
                }
            }

            if (total == 0) // not CheckBox checked
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }

            else if (total == 2) // more CheckBox checked
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }

            else
            {
                MasterBill objBillInfo = new MasterBill();
                txtDentalBillCd.Text = Code.ToString();
                objBillInfo = MasterBill.GetBillMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBillCd.Text));
                txtDentalBillNm.Text = objBillInfo.BillNm;
                txtDentalBillStatementNm.Text = objBillInfo.BillStatementNm;
                txtDentalBillPostalCd.Text = objBillInfo.BillPostalCd;
                txtDentalBillAddress1.Text = objBillInfo.BillAddress1;
                txtDentalBillAddress2.Text = objBillInfo.BillAddress2;
                txtDentalBillTel.Text = objBillInfo.BillTEL;
                txtDentalBillFax.Text = objBillInfo.BillFAX;
                txtBillContactPerson.Text = objBillInfo.BillContactPerson;
                txtDentalCreditLimit.Text = objBillInfo.CreditLimit.ToString();

                if (objBillInfo.BillFlg == null)
                    DropDownListBillFlg.SelectedValue = "";
                else
                    DropDownListBillFlg.SelectedValue = objBillInfo.BillFlg.ToString();

                DropDownListBillClosingDay.SelectedValue = objBillInfo.BillClosingDay.ToString();
                if (objBillInfo.BankCd == null)
                    DropDownListBankAccount.SelectedValue = "";
                else
                    DropDownListBankAccount.SelectedValue = objBillInfo.BankCd.ToString();
                if (objBillInfo.SupplierCd == null)
                    DropDownListSupplier.SelectedValue = "";
                DropDownListSupplier.SelectedValue = objBillInfo.SupplierCd.ToString();

                txtDentalBillCd.Enabled = false;
                btnDelete.Enabled = true;
                PnEdit.Visible = true;
                PnView.Visible = false;

            }

        }

        catch (Exception ex)
        {
            logger.Error("Error btnEdit_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }

    }
    /// <summary>
    /// Save Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            MasterBill objBillInfo = new MasterBill();
            objBillInfo.OfficeCd = Convert.ToInt32(HiddenFieldOfficeCd.Value);
            objBillInfo.BillCd = Convert.ToInt32(txtDentalBillCd.Text);
            objBillInfo.BillNm = txtDentalBillNm.Text;
            objBillInfo.BillStatementNm = txtDentalBillStatementNm.Text;
            objBillInfo.BillPostalCd = txtDentalBillPostalCd.Text;
            objBillInfo.BillAddress1 = txtDentalBillAddress1.Text;
            objBillInfo.BillAddress2 = txtDentalBillAddress2.Text;
            objBillInfo.BillTEL = txtDentalBillTel.Text;
            objBillInfo.BillFAX = txtDentalBillFax.Text;
            objBillInfo.BillContactPerson = txtBillContactPerson.Text;

            objBillInfo.CreditLimit = Common.GetNullableDouble(txtDentalCreditLimit.Text);
            objBillInfo.BillFlg = Common.GetNullableInt( DropDownListBillFlg.SelectedValue);


            if (DropDownListBillClosingDay.SelectedValue == "25")
                objBillInfo.BillClosingDay = 25;
            else if (DropDownListBillClosingDay.SelectedValue == "20")
                objBillInfo.BillClosingDay = 20;
            else
                objBillInfo.BillClosingDay = 0;

            if (DropDownListBankAccount.SelectedValue == "")
                objBillInfo.BankCd = null;
            else objBillInfo.BankCd = Convert.ToInt32(DropDownListBankAccount.SelectedValue);

            if (DropDownListSupplier.SelectedValue == "")
                objBillInfo.SupplierCd = null;
            else objBillInfo.SupplierCd = Convert.ToInt32(DropDownListSupplier.SelectedValue);

            objBillInfo.CreateDate = objBillInfo.ModifiedDate = DateTime.Now;
            objBillInfo.CreateAccount = objBillInfo.ModifiedAccount = this.User.Identity.Name;

            MasterBill objBill = new MasterBill();
            objBill = MasterBill.GetBillMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBillCd.Text));

            // Add new record
            if (btnDelete.Enabled == false)
            {
                if (objBill == null)
                {
                    objBillInfo.Insert();
                }
                else
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_EXIST_DENTAL_OFFICE_CD.Text")) + "\");");
                    return;
                }
            }
            // Update record
            else
            {
                objBillInfo.Update();
            }

            btnSearch_Click(sender, e);
        }

        catch (Exception ex)
        {
            logger.Error("Error btnSave_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }


    }
    /// <summary>
    /// Event Delete Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //objBillCon.DeleteBill1(Convert.ToInt32(txtOfficeCd.Text), Convert.ToInt32(txtDentalBillCd.Text));
            MasterBill objBill = MasterBill.GetBillMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBillCd.Text));
            objBill.Delete();
            btnSearch_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error("Error btnDelete_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Cancel Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    /// <summary>
    /// Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        List<MasterBill> ListBill = new List<MasterBill>();
        ListBill = MasterBill.GetBillMasterSearch(Convert.ToInt32(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text);
        FillData(ListBill);
    }
    /// <summary>
    /// Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtName.Text = "";
    }
    #endregion


    #region AddRow gridview
    /// <summary>
    /// AddRow Gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvBill_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add CheckBoxSelected
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(20);
            oGridViewRow.Cells.Add(oTableCell);

            //Add BankCode
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillCd.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillName
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillNm.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillPostalCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillPostalCd.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillAddress1
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillAddress1.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillAddress2
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillAddress2.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillTel
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillTel.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillFax
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBillFax.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Bill Contact Person
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbBillContactPerson.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillClosingDay
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbBillClosingDay.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    #endregion
    /// <summary>
    /// Paging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBill.PageIndex = e.NewPageIndex;
        btnSearch_Click(sender, e);
    }
    
}

