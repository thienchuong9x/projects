using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstSystem : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstSystem));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
            if (!IsPostBack)
            {

                InitLanguage();
                Initialize();
                FillGridView();
            }
        }
        catch (Exception ex)
        {

            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    private void FillGridView()
    {
        List<MasterSystem> listSystem = new List<MasterSystem>();
        listSystem = MasterSystem.GetAll();
        gridSystem.DataSource = listSystem;
        gridSystem.DataBind();
    }

    private void Initialize()
    {
        btDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        btnRegister.Text = GetResource("btnRegister.Text");
        btCancel.Text = GetResource("btCancel.Text");
        btEdit.Text = GetResource("btEdit.Text");
        btSave.Text = GetResource("btSave.Text");
        btDelete.Text = GetResource("btDelete.Text");

        lblParameter.Text = GetResource("lblParameter.Text");
        lblValue.Text = GetResource("lblValue.Text");
        valRequired_Parameter.ErrorMessage = GetResource("valRequired_Parameter.ErrorMessage");
        valRequired_Value.ErrorMessage = GetResource("valRequired_Value.ErrorMessage");
    }
    protected void gridSystem_RowCreated(object sender, GridViewRowEventArgs e)
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
            oTableCell.Text = GetResource("lblParameter.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblValue.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        tbxParameter.Text = string.Empty;
        tbxValue.Text = string.Empty;
        panelGrid.Visible = false;
        panelInput.Visible = true;
        btDelete.Enabled = false;
        tbxParameter.Enabled = true;
    }
    protected void btEdit_Click(object sender, EventArgs e)
    {
        try
        {
            var checkedIDs = (from GridViewRow msgRow in gridSystem.Rows
                              where ((CheckBox)msgRow.FindControl("Check")).Checked
                              select gridSystem.DataKeys[msgRow.RowIndex].Value.ToString()).ToList();
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
                var row = MasterSystem.GetSystemMaster(checkedIDs[0]);
                tbxParameter.Text = row.Parameter;
                tbxValue.Text = row.Value;
                tbxParameter.Enabled = false;

                panelGrid.Visible = false;
                panelInput.Visible = true;
                btDelete.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btSave_Click(object sender, EventArgs e)
    {
        try
        {
            var row = MasterSystem.GetSystemMaster(tbxParameter.Text.Trim());
            if (row == null)
            {
                row = new MasterSystem
                {
                    Parameter = tbxParameter.Text,
                    Value = tbxValue.Text,
                    CreateAccount = this.User.Identity.Name,
                    ModifiedAccount = this.User.Identity.Name
                };
                row.Insert();
            }
            else
            {
                row.Value = tbxValue.Text;
                row.ModifiedAccount = this.User.Identity.Name;
                row.Update();
            }
            panelGrid.Visible = true;
            panelInput.Visible = false;
            FillGridView();
        }
        catch (Exception ex)
        {
            logger.Error("Error Save: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btDelete_Click(object sender, EventArgs e)
    {
        try
        {
            var row = new MasterSystem();
            row.Parameter = tbxParameter.Text.Trim();
            row.Delete();
            panelGrid.Visible = true;
            panelInput.Visible = false;
            FillGridView();
        }
        catch (Exception ex)
        {
            logger.Error("Error Delete: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btCancel_Click(object sender, EventArgs e)
    {
        panelGrid.Visible = true;
        panelInput.Visible = false;
    }
    protected void gridSystem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridSystem.PageIndex = e.NewPageIndex;
        FillGridView();
    }
}