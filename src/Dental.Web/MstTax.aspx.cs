using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dental.Domain;
using Dental.Utilities;
using log4net;

public partial class MstTax : DDVPortalModuleBase
{

    #region "Event Handlers"

    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// Page_Load runs when the control is loaded 
    /// </summary> 
    /// ----------------------------------------------------------------------------- 
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstTax).Name);

    protected void Page_Load(object sender, System.EventArgs e)
    {
        FillCalendar();
        if (!IsPostBack)
        {
            try
            {
                InitLanguage();
                logger.Info("Begin Page_Load");
                btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

                // getMessage Resource 
                lbCode.Text = lbDentalTaxCd.Text = GetResource("lbDentalTaxCd.Text");
                lbChkAvailable.Text = GetResource("lbChkAvailable.Text");
                lbDentalTaxRate.Text = GetResource("lbDentalTaxRate.Text");
                lbDate.Text = GetResource("lbDate.Text");
                lbDentalStartDate.Text = GetResource("lbDentalStartDate.Text");
                lbDentalEndDate.Text = GetResource("lbDentalEndDate.Text");
                lbDentalRoundFraction.Text = GetResource("lbDentalRoundFraction.Text");

                btnRegister.Text = GetResource("btnRegister.Text");
                btnEdit.Text = GetResource("btnEdit.Text");
                btnSave.Text = GetResource("btnSave.Text");
                btnDelete.Text = GetResource("btnDelete.Text");
                btnCancel.Text = GetResource("btnCancel.Text");
                btnClear.Text = GetResource("btnClear.Text");
                btnSearch.Text = GetResource("btnSearch.Text");
                lblModuleTitle.Text = GetResource("lblModuleTitle.Text");

                val_TaxCd.ErrorMessage = GetResource("NumberOnly.Text");
                valRe_TaxCd.ErrorMessage = GetResource("TaxCodeEmpty.Text");
                Val_TaxRate.ErrorMessage = GetResource("TaxRateDoubleOnly.Text");
                valRe_TaxRate.ErrorMessage = GetResource("TaxRateEmpty.Text");
                Val_StartDate.ErrorMessage = GetResource("DateOnly.Text");
                valRe_StartDate.ErrorMessage = GetResource("StartDateEmpty.Text");
                Val_EndDate.ErrorMessage = GetResource("DateOnly.Text");

                // load data

                btnSearch_Click(sender, e);
                FillDropDownListTaxRoundFraction();
            }

            catch (Exception ex)
            {
                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
    }
    #endregion

    #region Fill All
    protected void FillData(List<MasterTax> ListTax)
    {
        MasterTax objTaxInfo = new MasterTax();
        //ListTax not Record 
        if (ListTax.Count == 0)
        {
            objTaxInfo.TaxCd = -1;
            objTaxInfo.TaxRate = -1;
            objTaxInfo.StartDate = DateTime.Now;
            objTaxInfo.EndDate = DateTime.Now;
            objTaxInfo.RoundFraction = null;
            ListTax.Add(objTaxInfo);
            grvTax.DataSource = ListTax;
            grvTax.DataBind();
            if (ListTax.Count == 1 && ListTax[0].TaxCd == -1)
            {
                grvTax.Rows[0].Visible = false;
            }
        }
        // Load data to gridview
        else
        {
            grvTax.DataSource = ListTax;
            grvTax.DataBind();
            foreach (GridViewRow row in grvTax.Rows)
            {
                row.Cells[3].Text = SetDateFormat(Convert.ToDateTime(row.Cells[3].Text).ToShortDateString());

                //row.Cells[4].Text = Common.null(row.Cells[4].Text).ToShortDateString();
                if (Common.GetRowString(row.Cells[4].Text) != "")
                    row.Cells[4].Text = SetDateFormat(Convert.ToDateTime(row.Cells[4].Text).ToShortDateString());

                if (row.Cells[5].Text == "1")
                    row.Cells[5].Text = GetResource("TitleRounded.Text");
                else if (row.Cells[5].Text == "2")
                    row.Cells[5].Text = GetResource("TitleTruncation.Text");
                else if (row.Cells[5].Text == "3")
                    row.Cells[5].Text = GetResource("TitleConclusion.Text");
                else row.Cells[5].Text = "";
            }
        }

        PnView.Visible = true;
        PnEdit.Visible = false;
    }
    /// <summary>
    /// fill Calendar
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected void FillCalendar()
    {
        hplStartDate.NavigateUrl = Calendar.InvokePopupCal(txtDentalStartDate);
        hplEndDate.NavigateUrl = Calendar.InvokePopupCal(txtDentalEndDate);
        hplStartDateFrom.NavigateUrl = Calendar.InvokePopupCal(txtDate);
    }

    protected void FillDropDownListTaxRoundFraction()
    {
        DropDownListTaxRoundFraction.Items.Add(new ListItem("", ""));
        DropDownListTaxRoundFraction.Items.Add(new ListItem(GetResource("TitleRounded.Text"), "1"));
        DropDownListTaxRoundFraction.Items.Add(new ListItem(GetResource("TitleTruncation.Text"), "2"));
        DropDownListTaxRoundFraction.Items.Add(new ListItem(GetResource("TitleConclusion.Text"), "3"));
    }
    #endregion

    #region AddRow GridView
    /// <summary>
    /// Addrow 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTax_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add TaxCode
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalTaxCd.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add TaxRate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalTaxRate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add StartDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalStartDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add EndDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalEndDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add RoundFraction
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentalRoundFraction.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    #endregion

    #region Event Click
    /// <summary>
    /// Event Register
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtDate.Text = string.Empty;
        txtDentalTaxCd.Text = "";
        txtDentalTaxRate.Text = "";
        txtDentalStartDate.Text = "";
        txtDentalEndDate.Text = "";
        DropDownListTaxRoundFraction.Text = "";
        txtDentalTaxCd.ReadOnly = false;
        btnDelete.Enabled = false;
        PnEdit.Visible = true;
        PnView.Visible = false;
        txtDentalTaxCd.Enabled = true;
    }
    /// <summary>
    /// Event Edit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtDate.Text = string.Empty;
        try
        {
            string Code = null;
            int total = 0;
            // event Checked
            foreach (GridViewRow gvr in grvTax.Rows)
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

            //no check
            if (total == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            // more check
            else if (total == 2)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            // checked
            else
            {
                MasterTax objTaxInfo = new MasterTax();
                txtDentalTaxCd.Text = Code.ToString();
                objTaxInfo = MasterTax.GetTaxMaster(Convert.ToInt32(txtDentalTaxCd.Text));
                txtDentalTaxRate.Text = objTaxInfo.TaxRate.ToString();
                txtDentalStartDate.Text = objTaxInfo.StartDate.ToShortDateString();

                if (objTaxInfo.EndDate.ToString() != "")
                    txtDentalEndDate.Text = Convert.ToDateTime(objTaxInfo.EndDate.ToString()).ToShortDateString();
                else
                    txtDentalEndDate.Text = objTaxInfo.EndDate.ToString();

                DropDownListTaxRoundFraction.SelectedValue = objTaxInfo.RoundFraction.ToString();

                txtDentalTaxCd.Enabled = false;
                btnDelete.Enabled = true;
                PnEdit.Visible = true;
                PnView.Visible = false;
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
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo.TaxCd = Convert.ToInt32(txtDentalTaxCd.Text);
            objTaxInfo.TaxRate = Convert.ToDouble(txtDentalTaxRate.Text);

            if (txtDentalStartDate.Text != "")
                objTaxInfo.StartDate = DateTime.Parse(txtDentalStartDate.Text);

            if (txtDentalStartDate.Text == "")
                objTaxInfo.StartDate = DateTime.Now;
            
            objTaxInfo.EndDate = Common.GetNullableDateTime(txtDentalEndDate.Text);            
            objTaxInfo.RoundFraction = Common.GetNullableInt(DropDownListTaxRoundFraction.SelectedValue);

            objTaxInfo.ModifiedDate = DateTime.Now;
            objTaxInfo.ModifiedAccount = this.User.Identity.Name;

            MasterTax objTax = MasterTax.GetTaxMaster(Convert.ToInt32(txtDentalTaxCd.Text));

            if (!string.IsNullOrWhiteSpace(txtDentalEndDate.Text) && (objTaxInfo.StartDate.Date > Convert.ToDateTime(objTaxInfo.EndDate).Date))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error_Date.Text"), "") + "\");", true);
                return;
            }
            else
            {
                // Add new record
                if (!btnDelete.Enabled)
                {
                    if (objTax == null)
                    {
                        objTaxInfo.CreateDate = DateTime.Now;
                        objTaxInfo.CreateAccount = this.User.Identity.Name;
                        objTaxInfo.Insert();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("Title_Same_Key.Text")) + "\");", true);
                        return;
                    }
                }
                // Update record
                else
                {
                    objTaxInfo.CreateDate = objTax.CreateDate;
                    objTaxInfo.CreateAccount = objTax.CreateAccount;
                    objTaxInfo.Update();
                }
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
        MasterTax objTax = MasterTax.GetTaxMaster(Convert.ToInt32(txtDentalTaxCd.Text));
        objTax.Delete();
        btnSearch_Click(sender, e);
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
        int check = 0;
        if (ChckAvailable.Checked)
            check = 1;
        else check = 0;

        List<MasterTax> ListTax = new List<MasterTax>();
        ListTax = MasterTax.GetTaxMasterSearch(txtCode.Text, txtDate.Text, check);
        FillData(ListTax);
    }
    /// <summary>
    /// Event Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtDate.Text = "";
        txtCode.Text = "";
        ChckAvailable.Checked = false;
    }
    #endregion
    /// <summary>
    /// Paging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvTax.PageIndex = e.NewPageIndex;
        btnSearch_Click(sender, e);
    }  

    protected void ChckAvailable_CheckedChanged(object sender, EventArgs e)
    {
        btnSearch_Click(sender, e);
    }
}