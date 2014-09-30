using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dental.Domain;
using Dental.Utilities;
using log4net;

public partial class MstSupplier : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstSupplier));
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitLanguage();
                //Lay gia tri cho textBox txtOfficeCd tu Cookies; Dat o Page_Load;

                HttpCookie ck = Request.Cookies[this.User.Identity.Name]; //Request.Cookies("test");
                if (ck != null)
                {
                    HiddenFieldOfficeCd.Value = ck["OfficeCd"];
                    //Goi method kiem tra quyen; Ko ko co gan textbox = -1
                }
                else
                    HiddenFieldOfficeCd.Value = "-1";

                HiddenFieldOfficeCd.Value = GetOffice();

                btDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

                lblModuleTitle.Text = GetResource("lblSupplier.Text");
                btSave.Text = GetResource("btSave.Text");
                btDelete.Text = GetResource("btDelete.Text");
                btCancel.Text = GetResource("btCancel.Text");

                btRegister.Text = GetResource("btRegister.Text");
                btEdit.Text = GetResource("btEdit.Text");

                lblSupplierCd.Text = GetResource("lblSupplierCd.Text");
                lblSupplierAbbNm.Text = GetResource("lblSupplierAbbNm.Text");
                lblSupplierAddress1.Text = GetResource("lblSupplierAddress1.Text");
                lblSupplierAddress2.Text = GetResource("lblSupplierAddress2.Text");
                lblSupplierFAX.Text = GetResource("lblSupplierFAX.Text");
                lblSupplierNm.Text = GetResource("lblSupplierNm.Text");
                lblSupplierPostalCd.Text = GetResource("lblSupplierPostalCd.Text");
                lblSupplierStaff.Text = GetResource("lblSupplierStaff.Text");
                lblSupplierTEL.Text = GetResource("lblSupplierTEL.Text");

                valNumbersOnly_SupplierCd.ErrorMessage = GetResource("valNumbersOnly_SupplierCd.ErrorMessage");
                valRequired_SupplierCd.ErrorMessage = GetResource("valRequired_SupplierCd.ErrorMessage");
                valRequired_SupplierAbbNm.ErrorMessage = GetResource("valRequired_SupplierAbbNm.ErrorMessage");
                valRequired_SupplierNm.ErrorMessage = GetResource("valRequired_SupplierNm.ErrorMessage");

                gridSupplier.EmptyDataText = GetResource("NoRecordFound.Text");

                lblCode.Text = GetResource("lblCode_Code.Text");
                lblName.Text = GetResource("lblName_Name.Text");
                btSearch.Text = GetResource("btSearch.Text");
                btClear.Text = GetResource("btClear.Text");

                // fill data
                btSearch_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Fill data
    /// </summary>
    /// <param name="listSupplier"></param>
    protected void FillData(List<MasterSupplier> listSupplier)
    {        
        MasterSupplier objSupplierInfo = new MasterSupplier();

        // Fill data to Gridview 
        // Not data
        if (listSupplier.Count == 0)
        {
            objSupplierInfo.OfficeCd = -1;
            objSupplierInfo.SupplierCd = -1;
            objSupplierInfo.SupplierNm = objSupplierInfo.SupplierAbbNm = "";

            listSupplier.Add(objSupplierInfo);
            gridSupplier.DataSource = listSupplier;
            gridSupplier.DataBind();

            if (listSupplier.Count == 1 && listSupplier[0].SupplierCd == -1)
            {
                gridSupplier.Rows[0].Visible = false;
            }
        }
        // Load data 
        else
        {
            gridSupplier.DataSource = listSupplier;
            gridSupplier.DataBind();            
        }        
        panelEdit.Visible = false;
        panelGrid.Visible = true;       
    }
    /// <summary>
    /// Row create
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridSupplier_RowCreated(object sender, GridViewRowEventArgs e)
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
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierAbbNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierPostalCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierAddress1.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierAddress2.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierTEL.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierFAX.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblSupplierStaff.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    #region "Event"
    /// <summary>
    /// Event Register
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btRegister_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = string.Empty;
        tbxSupplierCd.Text = string.Empty;
        tbxSupplierNm.Text = string.Empty;
        tbxSupplierAbbNm.Text = string.Empty;
        tbxSupplierPostalCd.Text = string.Empty;
        tbxSupplierAddress1.Text = string.Empty;
        tbxSupplierAddress2.Text = string.Empty;
        tbxSupplierTEL.Text = string.Empty;
        tbxSupplierFAX.Text = string.Empty;
        tbxSupplierStaff.Text = string.Empty;

        panelGrid.Visible = false;
        panelEdit.Visible = true;
        tbxSupplierCd.Enabled = true;
        btDelete.Enabled = false;
    }
    /// <summary>
    /// Event Edit
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btEdit_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = string.Empty;
        try
        {
            int total = 0;
            string Code = null;
            foreach (GridViewRow row in gridSupplier.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("Check");
                if (cb.Checked)// check CheckBox
                {
                    total++;
                    if (Code == null)
                        Code = row.Cells[1].Text;
                    if (total == 2)
                        break;
                }
            }
            if (total == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            }
            else if (total > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                var row = MasterSupplier.GetSupplierMaster(int.Parse(HiddenFieldOfficeCd.Value), Convert.ToInt32(Code));
                tbxSupplierCd.Text = row.SupplierCd.ToString();
                tbxSupplierNm.Text = row.SupplierNm;
                tbxSupplierAbbNm.Text = row.SupplierAbbNm;
                tbxSupplierPostalCd.Text = row.SupplierPostalCd;
                tbxSupplierAddress1.Text = row.SupplierAddress1;
                tbxSupplierAddress2.Text = row.SupplierAddress2;
                tbxSupplierTEL.Text = row.SupplierTEL;
                tbxSupplierFAX.Text = row.SupplierFAX;
                tbxSupplierStaff.Text = row.SupplierStaff;

                panelGrid.Visible = false;
                panelEdit.Visible = true;
                tbxSupplierCd.Enabled = false;
                btDelete.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btSave_Click(object sender, EventArgs e)
    {
        try
        {
            var row = MasterSupplier.GetSupplierMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxSupplierCd.Text));
            if (row == null)
            {
                row = new MasterSupplier
                {
                    OfficeCd = int.Parse(HiddenFieldOfficeCd.Value),
                    SupplierCd = int.Parse(tbxSupplierCd.Text),
                    SupplierNm = tbxSupplierNm.Text,
                    SupplierAbbNm = tbxSupplierAbbNm.Text,
                    SupplierPostalCd = tbxSupplierPostalCd.Text,
                    SupplierAddress1 = tbxSupplierAddress1.Text,
                    SupplierAddress2 = tbxSupplierAddress2.Text,
                    SupplierTEL = tbxSupplierTEL.Text,
                    SupplierFAX = tbxSupplierFAX.Text,
                    SupplierStaff = tbxSupplierStaff.Text,
                    CreateAccount = this.User.Identity.Name,
                    ModifiedAccount = this.User.Identity.Name
                };
                row.Insert();
            }
            else
            {
                row.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                row.SupplierNm = tbxSupplierNm.Text;
                row.SupplierAbbNm = tbxSupplierAbbNm.Text;
                row.SupplierPostalCd = tbxSupplierPostalCd.Text;
                row.SupplierAddress1 = tbxSupplierAddress1.Text;
                row.SupplierAddress2 = tbxSupplierAddress2.Text;
                row.SupplierTEL = tbxSupplierTEL.Text;
                row.SupplierFAX = tbxSupplierFAX.Text;
                row.SupplierStaff = tbxSupplierStaff.Text;
                row.ModifiedAccount = this.User.Identity.Name;
                row.Update();
            }
            btSearch_Click(sender, e);
            panelEdit.Visible = false;
            panelGrid.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btDelete_Click(object sender, EventArgs e)
    {
        //Check MsStock xem co data ko
        //No data -> Xoa
        //Co data: Amount=0 & Fraction=0 -> Xoa
        //Co data: !=0 -> Ko xoa
        try
        {
            List<MasterStock> list = MasterStock.getStockMasters(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxSupplierCd.Text));

            if (list.Count <= 0)
            {
                MasterSupplier objMasterSupplier = MasterSupplier.GetSupplierMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxSupplierCd.Text));
                objMasterSupplier.Delete();
                btSearch_Click(sender, e);
                panelEdit.Visible = false;
                panelGrid.Visible = true;
            }
            else
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_DELETED_ITEM.Text")) + "\");");
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btCancel_Click(object sender, EventArgs e)
    {
        panelEdit.Visible = false;
        panelGrid.Visible = true;
    }
    /// <summary>
    /// Event Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btSearch_Click(object sender, EventArgs e)
    {
        //FillData();
        List<MasterSupplier> listSupplier = MasterSupplier.GetSupplierMasters(Convert.ToInt32(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text);
        FillData(listSupplier);
    }
    /// <summary>
    /// Event Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtName.Text = "";
    }
    #endregion
    /// <summary>
    /// paging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gridSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridSupplier.PageIndex = e.NewPageIndex;
        btSearch_Click(sender, e);
    }
}
