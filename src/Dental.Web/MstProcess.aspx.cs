using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstProcess : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstProcess));
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            if (!IsPostBack)
            {
                InitLanguage();
                Initialize();
                Search();
            }
        }
        catch (Exception ex)
        {
            
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            
        }
    }

    private void Search()
    {
        try
        {
            lblNoRecord.Visible = false;
            List<MasterProcess> listProcess = new List<MasterProcess>();

            listProcess = MasterProcess.GetProcessmasterSearch(int.Parse(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text);
            if (!cbIsDeleted.Checked)
                listProcess = listProcess.Where(p => (p.IsDeleted == false)).ToList();

            if (listProcess.Count == 0)
            {
                MasterProcess info = new MasterProcess();
                info.ProcessCd = -1;
                info.ProcessNm = "";
                listProcess.Add(info);
                gridViewProcess.DataSource = listProcess;
                gridViewProcess.DataBind();
                if (listProcess.Count == 1 && listProcess[0].ProcessCd == -1)
                {
                    gridViewProcess.Rows[0].Visible = false;
                }
                lblNoRecord.Visible = true;
                return;
            }
            else
            {
                gridViewProcess.DataSource = listProcess;
                gridViewProcess.DataBind();
                foreach (GridViewRow r in gridViewProcess.Rows)
                {
                    if (r.Cells[3].Text == "True")
                        r.Cells[3].Text = "√";
                    else
                        r.Cells[3].Text = "";
                }
            }
        }
        catch (Exception e)
        {

            logger.Error("Error Page_Load", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
            
        }
    }


    private void Initialize()
    {

        HiddenFieldOfficeCd.Value = GetOffice();
        ButtonDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

        LabelProcessCd.Text = GetResource("HEADER_PROCESS_CODE.Text");
        LabelProcessNm.Text = GetResource("HEADER_PROCESS_NAME.Text");
        lblCode.Text = GetResource("lblCode_Code.Text");
        lblName.Text = GetResource("lblName_Name.Text");
        lblNoRecord.Visible = true;
        lblNoRecord.Text = GetResource("lblNoRecord.Text");
        lblNoRecord.Visible = false;
        cbIsDeleted.Text = GetResource("cbIsDeleted_processes.Text");

        valRequiredInput_ProcessCd.ErrorMessage = valRequiredInput_ProcessCd.ToolTip = valRequiredNumber_ProcessCd.ErrorMessage = valRequiredNumber_ProcessCd.ToolTip = GetResource("valRequired_ProcessCd.Text");
        valRequired_ProcessNm.ErrorMessage = valRequired_ProcessNm.ToolTip = GetResource("valRequired_ProcessNm.Text");


        gridViewProcess.EmptyDataText = GetResource("NoRecordFound.Text");
        SetButtonText(btnRegister, btnEdit, ButtonSave, ButtonDelete, ButtonCancel, btSearch, btClear);
    }
    protected void gridViewProcess_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add ProcessCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_CODE.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);


            //Add ProcessName
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_NAME.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Isdelete
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_IS_DELETE.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
       
    }
    protected void gridViewProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridViewProcess.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        panelHeader.Visible = false;
        panelDetail.Visible = true;
        TextProcessNm.Text = txtProcessCd.Text = string.Empty;
        txtProcessCd.Enabled = true;
        ButtonDelete.Enabled = false;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            MasterProcess objProcess = new MasterProcess();
            string ProcessCd = "", ProcessName = "";
            ButtonDelete.Enabled = true;
            txtProcessCd.Enabled = false;
            int countChecked = 0;
            CheckBox cb = new CheckBox();
            foreach (GridViewRow r in gridViewProcess.Rows)
            {
                cb = (CheckBox)r.Cells[0].FindControl("Check");
                if (cb.Checked)
                {
                    countChecked++;
                    if (countChecked == 2) break;
                    else
                    {
                        ProcessCd = Common.GetRowString(r.Cells[1].Text);
                        ProcessName = Common.GetRowString(r.Cells[2].Text);
                    }
                }

            }
            if (countChecked == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            else
                if (countChecked == 2)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");", true);
                    return;
                }
                else
                {
                    txtProcessCd.Text = ProcessCd;
                    TextProcessNm.Text = ProcessName;
                    panelHeader.Visible = false;
                    panelDetail.Visible = true;
                }
        }
        catch (Exception ex)
        {

            logger.Error("Error Edit", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterProcess objProcess = new MasterProcess();
            objProcess.ProcessCd = int.Parse(txtProcessCd.Text);
            objProcess.ProcessNm = TextProcessNm.Text;
            objProcess.IsDeleted = false;
            objProcess.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            objProcess.ModifiedAccount = this.User.Identity.Name;
            objProcess.ModifiedDate = DateTime.Now;

            MasterProcess process = new MasterProcess();
            process = MasterProcess.GetProcessMaster(objProcess.OfficeCd,objProcess.ProcessCd);
            //add new record
            if (ButtonDelete.Enabled == false)
            {
                objProcess.CreateAccount = objProcess.ModifiedAccount;
                objProcess.CreateDate = objProcess.ModifiedDate;
                if (process == null)
                {
                    objProcess.Insert();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_EXIST_PROCESS_CD.Text")) + "\");", true);
                    return;
                }
            }
            //update record
            else
            {
                objProcess.CreateAccount = process.CreateAccount;
                objProcess.CreateDate = process.CreateDate;
                objProcess.Update();
            }
            panelHeader.Visible = true;
            panelDetail.Visible = false;
            Search();

        }
        catch (Exception ex)
        {
            logger.Error("Error Save", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void ButtonDelete_Click(object sender, EventArgs e)
    {
        try
        {
            MasterProcess objProcess = new MasterProcess();
            objProcess.ProcessCd = int.Parse(txtProcessCd.Text);
            objProcess.ProcessNm = TextProcessNm.Text;
            objProcess.IsDeleted = true;
            objProcess.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            objProcess.ModifiedAccount = this.User.Identity.Name;
            objProcess.ModifiedDate = DateTime.Now;
            objProcess.Update();
            panelHeader.Visible = true;
            panelDetail.Visible = false;
            Search();
        }
        catch (Exception ex)
        {
            logger.Error("Error Delete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        panelDetail.Visible = false;
        panelHeader.Visible = true;
    }
    protected void btSearch_Click(object sender, EventArgs e)
    {
        Search();
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = string.Empty;
    }
    protected void cbIsDeleted_CheckedChanged(object sender, EventArgs e)
    {
        Search();
    }
}