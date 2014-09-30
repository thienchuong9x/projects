using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstBank : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstBank).Name);

    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {          
                
                InitLanguage();
                HiddenFieldOfficeCd.Value = GetOffice();

                //Get message resource
                btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
                btnRegister.Text = GetResource("btnRegister.Text");
                btnEdit.Text = GetResource("btnEdit.Text");
                btnSave.Text = GetResource("btnSave.Text");
                btnDelete.Text = GetResource("btnDelete.Text");
                btnCancel.Text = GetResource("btnCancel.Text");
                btnSearch.Text = GetResource("btnSearch.Text");
                btnClear.Text = GetResource("btnClear.Text");

                lbCode.Text = GetResource("lbDentalBankCd.Text");
                lbDentalBankCd.Text = GetResource("lbDentalBankCd.Text");
                lbAccount.Text = lbDentalBankAccount.Text = GetResource("lbDentalBankAccount.Text");
                lbDentalAccountOwner.Text = GetResource("lbDentalAccountOwner.Text");
                lbDentalForReceiveFlg.Text = GetResource("lbDentalForReceiveFlg.Text");
                lbDentalForpayFlg.Text = GetResource("lbDentalForpayFlg.Text");
                lblModuleTitle.Text = GetResource("lblModuleTitle.Text");

                valNumbersOnly_BankCd.ErrorMessage = GetResource("NumberOnly.Text");
                valRequired_BankCd.ErrorMessage = GetResource("BankCodeEmpty.Text");
                valRequired_BankAccount.ErrorMessage = GetResource("BankAccountEmpty.Text");
                ValRe_AccountOwner.ErrorMessage = GetResource("AccountOwnerEmpty.Text");

                // Fill Data
                btnSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }

    }

    #region FillAll
    /// <summary>
    /// Fill Data
    /// </summary>
    protected void FillData(List<MasterBank> listBank)
    {
        grvBank.Columns[6].Visible = true;
        MasterBank objBanksInfo = new MasterBank();

        // Fill data to Gridview 
        // Not data
        if (listBank.Count == 0)
        {
            objBanksInfo.OfficeCd = -1;
            objBanksInfo.BankCd = -1;
            objBanksInfo.BankAccount = "";
            objBanksInfo.ForPayFlg = false;
            objBanksInfo.ForReceiveFlg = false;

            listBank.Add(objBanksInfo);
            grvBank.DataSource = listBank;
            grvBank.DataBind();

            if (listBank.Count == 1 && listBank[0].BankCd == -1)
            {
                grvBank.Rows[0].Visible = false;
            }
        }
        // Load data 
        else
        {
            grvBank.DataSource = listBank;
            grvBank.DataBind();
            foreach (GridViewRow row in grvBank.Rows)
            {
                if (row.Cells[4].Text == "True")
                    row.Cells[4].Text = "√";
                else row.Cells[4].Text = "";

                if (row.Cells[5].Text == "True")
                    row.Cells[5].Text = "√";
                else row.Cells[5].Text = "";
            }
        }
        grvBank.Columns[6].Visible = false;
        PnEdit.Visible = false;
        PnView.Visible = true;
    }
    /// <summary>
    /// Fill Check
    /// </summary>        
    #endregion

    #region Event Click
    /// <summary>
    /// Event Register
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtAccount.Text = string.Empty;
        txtDentalBankCd.Text = "";
        txtDentalBankAccount.Text = "";
        txtDentalAccountOwner.Text = "";
        ChkPay.Checked = false;
        ChkReceive.Checked = false;
        PnEdit.Visible = true;
        PnView.Visible = false;
        btnDelete.Enabled = false;
        txtDentalBankCd.Enabled = true;

    }
    /// <summary>
    /// Event Edit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtAccount.Text = string.Empty;
        try
        {
            int total = 0;
            string Code = null;
            foreach (GridViewRow row in grvBank.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckEdit");
                if (cb.Checked)// check CheckBox
                {
                    total++;
                    if (Code == null)
                        Code = row.Cells[1].Text;
                    if (total == 2)
                        break;
                }
            }
            if (total == 0)// not CheckBox checked
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
                MasterBank objBankInfo = new MasterBank();
                txtDentalBankCd.Text = Code.ToString();
                objBankInfo = MasterBank.GetBankMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBankCd.Text));
                txtDentalBankAccount.Text = objBankInfo.BankAccount;
                txtDentalAccountOwner.Text = objBankInfo.AccountOwner;

                if (objBankInfo.ForReceiveFlg.ToString() == "True")
                    ChkReceive.Checked = true;

                if (objBankInfo.ForReceiveFlg.ToString() == "False")
                    ChkReceive.Checked = false;

                if (objBankInfo.ForPayFlg.ToString() == "True")
                    ChkPay.Checked = true;

                if (objBankInfo.ForPayFlg.ToString() == "False")
                    ChkPay.Checked = false;

                PnEdit.Visible = true;
                PnView.Visible = false;
                txtDentalBankCd.Enabled = false;
                btnDelete.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            logger.Error("End btnEdit_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }

    }

    /// <summary>
    /// Event Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            MasterBank objBankInfo = new MasterBank();

            objBankInfo.OfficeCd = Convert.ToInt32(HiddenFieldOfficeCd.Value);
            objBankInfo.BankCd = Convert.ToInt32(txtDentalBankCd.Text);
            objBankInfo.BankAccount = txtDentalBankAccount.Text;
            objBankInfo.AccountOwner = txtDentalAccountOwner.Text;

            if (ChkReceive.Checked)
                objBankInfo.ForReceiveFlg = Convert.ToBoolean(1);

            if (!ChkReceive.Checked)
                objBankInfo.ForReceiveFlg = Convert.ToBoolean(0);

            if (ChkPay.Checked)
                objBankInfo.ForPayFlg = Convert.ToBoolean(1);

            if (!ChkPay.Checked)
                objBankInfo.ForPayFlg = Convert.ToBoolean(0);

            objBankInfo.CreateDate = objBankInfo.ModifiedDate = DateTime.Now;
            objBankInfo.CreateAccount = objBankInfo.ModifiedAccount = this.User.Identity.Name;

            MasterBank objBank = new MasterBank();
            objBank = MasterBank.GetBankMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBankCd.Text));

            // Add new record
            if (btnDelete.Enabled == false)
            {
                if (objBank == null)
                {
                    objBankInfo.Insert();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("Title_Same_Key.Text")) + "\");", true);
                    return;
                }
            }
            // Update Record
            else
            {
                objBankInfo.Update();
            }
            // load page
            btnSearch_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error("End btnSave_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            MasterBank objBank = MasterBank.GetBankMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(txtDentalBankCd.Text));
            objBank.Delete();
            btnSearch_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error("End btnDelete_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
    /// <summary>
    /// Event Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        List<MasterBank> listBank = new List<MasterBank>();
        listBank = MasterBank.GetBankMasterSearch(Convert.ToInt32(HiddenFieldOfficeCd.Value), txtCode.Text, txtAccount.Text);
        FillData(listBank);
    }
    /// <summary>
    /// Event Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtAccount.Text = "";
    }
    #endregion

    #region Addrow Gridview
    /// <summary>
    /// AddRow Gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvBank_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add CheckBoxSelected
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(20);
            oGridViewRow.Cells.Add(oTableCell);

            //Add BankCode
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBankCd.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add BankAccount
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalBankAccount.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(250);
            oGridViewRow.Cells.Add(oTableCell);

            //Add AccountOwner
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalAccountOwner.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(250);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ForReceiveFlg
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalForReceiveFlg.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ForpayFlg
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalForpayFlg.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    #endregion
    /// <summary>
    /// event Paging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    protected void grvBank_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvBank.PageIndex = e.NewPageIndex;
        btnSearch_Click(sender, e);
    }
}
