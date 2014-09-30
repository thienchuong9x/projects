using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;
using System.Data;

public partial class MstProsthesis : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstProsthesis));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
            if (!IsPostBack)
            {
                InitLanguage();
                Initialize();
                FillProcessList(selProcess);
                FillTechPriceList(selTechPrice);
                FillProsthesisTypeList();
                
                List<MasterProcess> listProcess = MasterProcess.GetAll().Where(p=>(p.OfficeCd==int.Parse(HiddenFieldOfficeCd.Value) && p.IsDeleted==false)).ToList();
                selProcessRegister.DataSource = listProcess;
                selProcessRegister.DataBind();
                selProcessRegister.Items.Insert(0, new ListItem("",""));

                List<MasterTech> listTech = MasterTech.GetAll().Where(t => (t.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value) && (t.StartDate <= DateTime.Today || t.StartDate >= DateTime.Today) && (t.EndDate == null || t.EndDate >= DateTime.Today))).ToList();
                selTechPriceEdit.DataSource = listTech;
                selTechPriceEdit.DataBind();
                selTechPriceEdit.Items.Insert(0, new ListItem("", ""));

                Search();
                
            }
        }
        catch(Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    private void Initialize()
    {
        ViewState.Remove("Process");
        ViewState.Remove("Tech");

        HiddenFieldOfficeCd.Value = GetOffice();
        DropDownListProsthesisType.Visible = true;

        btDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        btnDeleteTechPrice.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        //lblModuleTitle.Text = GetResource("lblProsthesis.Text");
        lblProsthesisCd0.Text = lblProsthesisCd1.Text = lblProsthesisCd.Text = GetResource("lblProsthesisCd.Text");
        lblProsthesisAbbNm.Text = GetResource("lblProsthesisAbbNm.Text");
        lblProsthesisNm.Text = GetResource("lblProsthesisNm.Text");
        lblProsthesisType.Text = GetResource("lblProsthesisType.Text");
        lblStump.Text = GetResource("lblStump.Text");
        lblMinimumProcess.Text = GetResource("lblMinimumProcess.Text");
        lblProcessSearch.Text = GetResource("Process.Text");
        lblTechPriceSearch.Text = GetResource("TechPrice.Text");
        LabelProcessCd.Text = GetResource("HEADER_PROCESS_CODE.Text");
        LabelDisplayOrder.Text = GetResource("HEADER_PROCESS_DISPLAYORDER.Text");
        lblProcess.Text = GetResource("ProcessforProsthesis.Text");

        lblNorecordTechPrice.Visible = lblNorecordFoundTech.Visible = lblNoRecord.Visible = lblNoRecordProsthesis.Visible = true;
        lblNorecordTechPrice.Text = lblNorecordFoundTech.Text = lblNoRecord.Text = lblNoRecordProsthesis.Text = GetResource("NoRecordFound.Text");
        lblNorecordTechPrice.Visible = lblNorecordFoundTech.Visible = lblNoRecord.Visible = lblNoRecordProsthesis.Visible = false;

        lblTechPrice.Text = GetResource("lblTechPrice_TechTemplate.Text");
        lblTechPriceCd.Text = GetResource("lblTechPriceCd.Text");
        lblDisplayOrderTech.Text = GetResource("HEADER_PROCESS_DISPLAYORDER.Text");
        lblProsthesisReg.Text = GetResource("lblProsthesisReg.Text");
        LabelTechPrice.Text = GetResource("TechPrice.Text");

        btnSavetechPrice.Text = btnSave.Text = btSave.Text = GetResource("btSave.Text");
        btnDelete.Text = btnDeleteTechPrice.Text = btDelete.Text = GetResource("btDelete.Text");
        btnCancelTechDetail.Text = btnCancel.Text = btnCancelTechPrice.Text = btCancel.Text = GetResource("btCancel.Text");
        btnViewTechPice.Text = GetResource("btnViewTechPice.Text");

        btnRegProcess.Text = btnRegTechPrice.Text = btRegister.Text = GetResource("btRegister.Text");
        btnEditProcess.Text = btnEditTechPrice.Text = btEdit.Text = GetResource("btEdit.Text");

        //valNumbersOnly_ProsthesisCd.ErrorMessage = GetResource("NumberOnly.Text");
        valNumbersOnly_MinimumProcess.ErrorMessage = GetResource("NumberOnly.Text");
        valRequired_MinimumProcess.ErrorMessage = GetResource("valRequired_MinimumProcess.Text");
        valRequired_ProsthesisAbbNm.ErrorMessage = GetResource("valRequired_ProsthesisAbbNm.Text");
        valRequired_ProsthesisNm.ErrorMessage = GetResource("valRequired_ProsthesisNm.ErrorMessage");
        valRequired_ProsthesisType.ErrorMessage = GetResource("valRequired_ProsthesisType.Text");
        valRequired_ProsthesisCd.ErrorMessage = GetResource("CodeEmpty.Text");
        valRequiredInput_ProcessCd.ErrorMessage = GetResource("valRequired_ProcessCd.Text");
        valRequiredNumber_TechCd.Text = GetResource("valRequired_TechCd.Text");
        //valRequiredDisplayoer_Process.ErrorMessage = valRequiredDisplayorder_TechPrice.ErrorMessage = GetResource("valDisplayOrder.Text");
        cbShowIsDeleted.Text = GetResource("cbIsDeleted.Text");

        chkAvailableTechPrice.Text = GetResource("chkAvailableTechPrice.Text");

        gridProsthesis.EmptyDataText = GetResource("NoRecordFound.Text");

        lblCode.Text = GetResource("lblProsthesisCd.Text");
        lblName.Text = GetResource("lblProsthesisNm.Text");
        btSearch.Text = GetResource("btSearch.Text");
        btClear.Text = GetResource("btClear.Text");


    }


    private void FillProsthesisTypeList()
    {
        DropDownListProsthesisType.Items.Add(new ListItem("", ""));
        DropDownListProsthesisType.Items.Add(new ListItem(("STUMP"), "STUMP"));
        DropDownListProsthesisType.Items.Add(new ListItem(("PONTIC"), "PONTIC"));
        DropDownListProsthesisType.Items.Add(new ListItem(("ARCH"), "ARCH"));
    }

    private void FillTechPriceList(params DropDownList[] dropdownlistTech )
    {
        try
        {
            
            List<MasterTech> listTech = new List<MasterTech>();
            listTech = MasterTech.GetAll();
            listTech = listTech.Where(l => (l.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value))).ToList();
            
            foreach (DropDownList d in dropdownlistTech)
            {
                d.Items.Add(new ListItem("", ""));
                foreach (MasterTech p in listTech)
                {
                    d.Items.Add(new ListItem(p.TechNm,p.TechCd.ToString()));
                }
            }
        }
        catch (Exception e)
        {

            logger.Error("Error Fill Dropdownlist Process", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        }
    }

    private void FillProcessList(params DropDownList[] dropdownlistTechPrice)
    {
        try
        {
            List<MasterProcess> listProcess = new List<MasterProcess>();
            listProcess = MasterProcess.GetAll();
            listProcess = listProcess.Where(l => (l.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value))).ToList();
            foreach (DropDownList d in dropdownlistTechPrice)
            {
                
                d.Items.Add(new ListItem("", ""));
                foreach (MasterProcess p in listProcess)
                {
                    d.Items.Add(new ListItem(p.ProcessNm, p.ProcessCd.ToString()));
                }
            }
        }
        catch (Exception e)
        {

            logger.Error("Error Fill Dropdownlist TechPrice", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        }
    }

    private void Search()
    {
        try
        {
            lblNoRecordProsthesis.Visible = false;
            List<ProsthesisSearchInfo> prosInfo = new List<ProsthesisSearchInfo>();
            prosInfo = ProsthesisSearchInfo.GetProsthesisSearch(int.Parse(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text, Common.GetNullableInt(txtProcessSearch.Text), Common.GetNullableInt(txtTechPriceSearch.Text));
            if (prosInfo.Count == 0)
            {
                ProsthesisSearchInfo info = new ProsthesisSearchInfo();
                info.OfficeCd = 9;
                info.ProsthesisCd = -1;
                info.ProsthesisAbbNm = "";
                prosInfo.Add(info);
                gridProsthesis.DataSource = prosInfo;
                gridProsthesis.DataBind();
                if (prosInfo.Count == 1 && prosInfo[0].ProsthesisCd == -1)
                {
                    gridProsthesis.Rows[0].Visible = false;
                }
                lblNoRecordProsthesis.Visible = true;
                return;
            }
            gridProsthesis.DataSource = prosInfo;
            gridProsthesis.DataBind();
            foreach (GridViewRow r in gridProsthesis.Rows)
            {
                if (r.Cells[5].Text == "True")
                    r.Cells[5].Text = "√";
                else
                    r.Cells[5].Text = "";
            }
        }
        catch (Exception e)
        {
            logger.Error("Error Search", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        
        }
    }

    protected void gridProsthesis_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add ProsthesisCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProsthesisCd.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(50);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProsthesisNmAbb
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProsthesisAbbNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProsthesisNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProsthesisNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(300);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProsthesisType
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProsthesisType.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //add StumpFlg
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblStump.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Minimumprocess
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblMinimumProcess.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Process
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("Process.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add TechPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TechPrice.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void gridProcess_RowCreated(object sender, GridViewRowEventArgs e)
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
            oTableCell.Width = Unit.Pixel(80);
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);


            //Add ProcessName
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_NAME.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(300);
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void gridViewTech_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add TechCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_CODE.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(50);
            oGridViewRow.Cells.Add(oTableCell);

            //Add TechNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_NAME.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(200);
            oGridViewRow.Cells.Add(oTableCell);

            //Add TechPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("colStandardTechPrice.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add EditTable
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_EDITTABLE.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(50);
            oGridViewRow.Cells.Add(oTableCell);

            //AddTech Group 
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_GROUP.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Startdate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_START_DATE.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add EndDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_END_DATE.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);


            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void selProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProcessSearch.Text = selProcess.SelectedValue.ToString();
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtName.Text = "";
        txtTechPriceSearch.Text = "";
        txtProcessSearch.Text = "";
        selTechPrice.SelectedValue = "";
        selProcess.SelectedValue = "";
    }
    protected void btRegister_Click(object sender, EventArgs e)
    {
        DataStructureProcess();
        DataStructureTech();

        tbxProsthesisCd.Enabled = true;
        tbxProsthesisCd.Text = string.Empty;
        tbxProsthesisAbbNm.Text = string.Empty;
        tbxProsthesisNm.Text = string.Empty;
        DropDownListProsthesisType.SelectedValue = string.Empty;
        tbxMinimumProcess.Text = string.Empty;
        panelGrid.Visible = false;
        panelEdit.Visible = true;
        gridProcess.DataSource = null;
        gridProcess.DataBind();
        PanelProcessView.Visible = true;
        PanelViewTechPrice.Visible = true;
        btDelete.Enabled = false;
        //btnEditProcess.Enabled = false;
        FillDataToGridProcess();
        FillDataToGridTech();
    }

   
    private void DataStructureProcess()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ProcessCd");
        dt.Columns.Add("ProcessNm");
        dt.Columns.Add("IsDeleted");
        dt.Columns.Add("DisplayOrder");
        ViewState["Process"] = dt;
    }

    private void DataStructureTech()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TechCd");
        dt.Columns.Add("TechNm");
        dt.Columns.Add("StandardTechPrice");
        dt.Columns.Add("Editable");
        dt.Columns.Add("TechGroup");
        dt.Columns.Add("StartDate");
        dt.Columns.Add("EndDate");
        dt.Columns.Add("DisplayOrder");
       
        ViewState["Tech"] = dt; 
    }

    private void FillDataToGridTech()
    {
        try
        {
            List<MasterTech> listTechInfo = new List<MasterTech>();
            listTechInfo = MasterTech.GetTechMasterByProsthesis(int.Parse(HiddenFieldOfficeCd.Value), Common.GetNullableInt(tbxProsthesisCd.Text));
            lblNorecordFoundTech.Visible = false;

            if (listTechInfo.Count == 0)
            {
                MasterTech info = new MasterTech();
                info.OfficeCd = 9;
                info.TechCd = -1;
                info.StartDate = DateTime.Now;
                listTechInfo.Add(info);

                gridViewTech.DataSource = listTechInfo;
                gridViewTech.DataBind();
                if (listTechInfo.Count == 1 && listTechInfo[0].TechCd == -1)
                {
                    gridViewTech.Rows[0].Visible = false;
                }

                lblNorecordFoundTech.Visible = true;

                return;
            }
            if (listTechInfo != null && listTechInfo.Count > 0)
            {
                if (!chkAvailableTechPrice.Checked)
                    listTechInfo = listTechInfo.Where(t => ((t.StartDate <= DateTime.Today || t.StartDate >= DateTime.Today) && (t.EndDate == null || t.EndDate.Value >= DateTime.Today))).ToList();

                if (listTechInfo.Count == 0)
                {
                    MasterTech info = new MasterTech();
                    info.OfficeCd = 9;
                    info.TechCd = -1;
                    info.StartDate = DateTime.Now;
                    listTechInfo.Add(info);

                    gridViewTech.DataSource = listTechInfo;
                    gridViewTech.DataBind();
                    if (listTechInfo.Count == 1 && listTechInfo[0].TechCd == -1)
                    {
                        gridViewTech.Rows[0].Visible = false;
                    }

                    lblNorecordFoundTech.Visible = true;

                    return;
                }

                gridViewTech.DataSource = listTechInfo;
                gridViewTech.DataBind();
                foreach (GridViewRow r in gridViewTech.Rows)
                {
                    if (r.Cells[4].Text == "True")
                        r.Cells[4].Text = "√";
                    else
                        r.Cells[4].Text = "";
                    if (r.Cells[5].Text == "-1")
                        r.Cells[5].Text = "";
                    r.Cells[6].Text = SetDateFormat(r.Cells[6].Text);
                    if (Common.GetRowString(r.Cells[7].Text) != "")
                    {
                        r.Cells[7].Text = SetDateFormat(r.Cells[7].Text);
                    }
                }

            }
        }
        catch (Exception e)
        {
            logger.Error("Error FillDataToGridTech", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        
        }
    }

    private void FillDataToGridProcess()
    {
        try
        {
            List<MasterProcess> listProcess = new List<MasterProcess>();

            listProcess = MasterProcess.GetProcessMasterByProsthesis(int.Parse(HiddenFieldOfficeCd.Value), Common.GetNullableInt(tbxProsthesisCd.Text));
            lblNoRecord.Visible = false;
            if (!cbShowIsDeleted.Checked)
            {
                listProcess = listProcess.Where(p => (p.IsDeleted == false)).ToList();
            }
            if (listProcess.Count == 0)
            {
                MasterProcess info = new MasterProcess();
                info.ProcessCd = -1;
                info.ProcessNm = "";
                listProcess.Add(info);
                gridProcess.DataSource = listProcess;
                gridProcess.DataBind();
                if (listProcess.Count == 1 && listProcess[0].ProcessCd == -1)
                {
                    gridProcess.Rows[0].Visible = false;
                }
                lblNoRecord.Visible = true;
                return;
            }

            gridProcess.DataSource = listProcess;
            gridProcess.DataBind();
            foreach (GridViewRow r in gridProcess.Rows)
            {
                if (r.Cells[3].Text == "True")
                    r.Cells[3].Text = "√";
                else
                    r.Cells[3].Text = "";

            }
        }
        catch (Exception e)
        {
            logger.Error("Error FillDataToGridProcess", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        
        }
    }

    protected void btEdit_Click(object sender, EventArgs e)
    {
        try
        {
            var checkedIDs = (from GridViewRow msgRow in gridProsthesis.Rows
                              where ((CheckBox)msgRow.FindControl("Check")).Checked
                              select int.Parse(gridProsthesis.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();
            if (checkedIDs.Count == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
                return;
            }
            else if (checkedIDs.Count > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
                return;
            }
            else
            {
                var row = MasterProsthesis.GetProsthesis(int.Parse(HiddenFieldOfficeCd.Value), checkedIDs[0]);
                tbxProsthesisCd.Text = row.ProsthesisCd.ToString();
                tbxProsthesisAbbNm.Text = row.ProsthesisAbbNm;
                tbxProsthesisNm.Text = row.ProsthesisNm;
                DropDownListProsthesisType.SelectedValue = row.ProsthesisType;
                if (row.StumpFlg == true)
                    cbStump.Checked = true;
                else
                    cbStump.Checked = false;
                tbxMinimumProcess.Text = row.MinimumProcess.ToString();
                tbxProsthesisCd.Enabled = false;
                panelGrid.Visible = false;
                panelEdit.Visible = true;
                PanelProcessView.Visible = true;
                PanelViewTechPrice.Visible = true;
                btDelete.Enabled = true;
                btnEditProcess.Enabled = true;

                FillDataToGridProcess();
                FillDataToGridTech();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        try
        {
            var row = MasterProsthesis.GetProsthesis(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxProsthesisCd.Text));
            if (row != null)
            {
                if (btDelete.Enabled == true)
                {
                    row.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                    row.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                    row.ProsthesisAbbNm = tbxProsthesisAbbNm.Text;
                    row.ProsthesisNm = tbxProsthesisNm.Text;
                    row.ProsthesisType = DropDownListProsthesisType.SelectedValue;
                    if (cbStump.Checked)
                        row.StumpFlg = true;
                    else
                        row.StumpFlg = false;
                    row.MinimumProcess = int.Parse(tbxMinimumProcess.Text);
                    row.ModifiedAccount = this.User.Identity.Name;
                    row.ModifiedDate = DateTime.Today;

                    row.Update();


                }
                else
                {

                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("Title_Same_Key_ProthesisCode.Text")) + "\");");
                    return;
                }
            }
            else
            {
                MasterProsthesis objProsthesis = new MasterProsthesis();
                objProsthesis.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                objProsthesis.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                objProsthesis.ProsthesisAbbNm = tbxProsthesisAbbNm.Text;
                objProsthesis.ProsthesisNm = tbxProsthesisNm.Text;
                objProsthesis.ProsthesisType = DropDownListProsthesisType.SelectedValue;
                if (cbStump.Checked)
                    objProsthesis.StumpFlg = true;
                else
                    objProsthesis.StumpFlg = false;
                objProsthesis.MinimumProcess = int.Parse(tbxMinimumProcess.Text);
                objProsthesis.CreateAccount = objProsthesis.ModifiedAccount = this.User.Identity.Name;
                objProsthesis.CreateDate = objProsthesis.ModifiedDate = DateTime.Today;
                objProsthesis.Insert();

                #region  save processtemplete
                DataTable dtprocess = (DataTable)ViewState["Process"];
                for (int i = 0; i < dtprocess.Rows.Count; i++)
                {
                    MasterProcessTemplate obj = new MasterProcessTemplate();
                    obj.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                    obj.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                    obj.ProcessCd = int.Parse(dtprocess.Rows[i][0].ToString());
                    obj.DisplayOrder = int.Parse(dtprocess.Rows[i][3].ToString());
                    obj.CreateAccount = obj.ModifiedAccount = this.User.Identity.Name;
                    obj.CreateDate = obj.ModifiedDate = DateTime.Today;
                    obj.Insert();
                }
                #endregion

                #region save techpricetemplete
                DataTable dttech = (DataTable)ViewState["Tech"];

                List<MasterTechPriceTemplate> list = new List<MasterTechPriceTemplate>();
                for (int i = 0; i < dttech.Rows.Count; i++)
                {
                    list.Add(new MasterTechPriceTemplate
                    {
                        OfficeCd = int.Parse(HiddenFieldOfficeCd.Value),
                        ProsthesisCd = int.Parse(tbxProsthesisCd.Text),
                        TechCd = int.Parse(dttech.Rows[i][0].ToString()),
                        DisplayOrder = int.Parse(dttech.Rows[i][7].ToString()),
                        CreateAccount = this.User.Identity.Name,
                        CreateDate = DateTime.Today,
                        ModifiedAccount = this.User.Identity.Name,
                        ModifiedDate = DateTime.Today,
                    });
                }
                list = list.GroupBy(x => x.TechCd).Select(y => y.First()).ToList();
                foreach (MasterTechPriceTemplate obj in list)
                {
                    obj.Insert();
                }
                #endregion

            }

            Search();
            panelGrid.Visible = true;
            panelEdit.Visible = false;
            PanelProcessView.Visible = false;
            PanelViewTechPrice.Visible = false;
            PanelViewTechPriceDetail.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error("Error Save", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        try
        {
            MasterProsthesis mstProsthesis = new MasterProsthesis();
            mstProsthesis.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            mstProsthesis.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);

            foreach (GridViewRow r in gridProcess.Rows)
            {
                if (r.Cells[1].Text != "-1")
                {
                    MasterProcessTemplate obj = new MasterProcessTemplate();
                    obj = MasterProcessTemplate.GetProcessTempalteMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxProsthesisCd.Text), int.Parse(r.Cells[1].Text));
                    if(obj!=null)
                        obj.Delete();
                }
            }

            foreach (GridViewRow r in gridViewTech.Rows)
            {
                if (r.Cells[1].Text != "-1")
                {
                    MasterTechPriceTemplate obj = new MasterTechPriceTemplate();
                    obj = MasterTechPriceTemplate.GetTechPriceTempleteMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxProsthesisCd.Text), int.Parse(r.Cells[1].Text));
                    if (obj != null)
                        obj.Delete();
                }
            }

            mstProsthesis.Delete();
            Search();
            panelGrid.Visible = true;
            panelEdit.Visible = false;
            PanelProcessEdit.Visible = false;
            PanelProcessView.Visible = false;
            PanelViewTechPrice.Visible = false;
            PanelTechPriceEdit.Visible = false;
            PanelViewTechPriceDetail.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error("Error Delete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        ViewState.Remove("Process");
        ViewState.Remove("Tech");

        panelGrid.Visible = true;
        panelEdit.Visible = false;
        PanelProcessView.Visible = false;
        PanelProcessEdit.Visible = false;
        PanelViewTechPrice.Visible = false;
        PanelViewTechPriceDetail.Visible = false;
    }

    protected void btnRegProcess_Click(object sender, EventArgs e)
    {
        txtProcessCd.Text = "";
        selProcessRegister.SelectedValue = "";
        txtDisplayOrder.Text = "";
        tbxProsthesisCd.Enabled = false;
        txtProcessCd.Enabled = true;
        selProcessRegister.Enabled = true;
        selProcessRegister.SelectedValue = "";
        PanelProcessView.Visible = false;
        PanelProcessEdit.Visible = true;
        panelEdit.Enabled = false;
        btnDelete.Enabled = false;
        tbxProsthesisCd0.Text = tbxProsthesisCd.Text;
        tbxProsthesisNm0.Text = tbxProsthesisNm.Text;
    }

    protected void btnEditProcess_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = new CheckBox();
            string displayorder = "";
            int count = 0;
            foreach (GridViewRow r in gridProcess.Rows)
            {
                cb = (CheckBox)r.Cells[0].FindControl("cbProcess");
                if (cb.Checked)
                {
                    count++;
                    if (count == 2) break;
                    else
                    {
                        txtProcessCd.Text = Common.GetRowString(r.Cells[1].Text);
                        displayorder = Common.GetRowString(r.Cells[4].Text);
                        
                    }
                }
            }
            if (count == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
                return;
            }
            else if (count == 2)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
                return;
            }
            else
            {
                txtProcessCd.Enabled = false;
                selProcessRegister.Enabled = false;
                if (btDelete.Enabled == true)
                {
                    txtDisplayOrder.Text = MasterProcessTemplate.GetAll().Where(p => (p.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value) && p.ProsthesisCd == int.Parse(tbxProsthesisCd.Text) && p.ProcessCd == int.Parse(txtProcessCd.Text))).FirstOrDefault().DisplayOrder.ToString();
                }
                else
                {
                    txtDisplayOrder.Text = displayorder;
                }
                selProcessRegister.SelectedValue = txtProcessCd.Text;
                btnDelete.Enabled = true;
                tbxProsthesisCd0.Text = tbxProsthesisCd.Text;
                tbxProsthesisNm0.Text = tbxProsthesisNm.Text;
               
                PanelProcessView.Visible = false;
                PanelProcessEdit.Visible = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit processtemplete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblNoRecord.Visible = false;
            if (btDelete.Enabled == false)
            {
                DataTable dt = (DataTable)ViewState["Process"];
                DataRow r = dt.NewRow();
                r["ProcessCd"] = txtProcessCd.Text;
                r["ProcessNm"]=selProcessRegister.SelectedItem.Text;
                if(txtDisplayOrder.Text == string.Empty) 
                    r["DisplayOrder"]="0";
                else
                    r["DisplayOrder"] = txtDisplayOrder.Text;
                int check = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == txtProcessCd.Text)
                    {
                        if (btnDelete.Enabled == false)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckProcesstemplete.Text")) + "\");");
                            check = 1;
                            return;
                        }
                        else
                        {
                            dt.Rows.RemoveAt(i);
                            r["ProcessCd"] = txtProcessCd.Text;
                            r["ProcessNm"] = selProcessRegister.SelectedItem.Text;
                            if (txtDisplayOrder.Text == string.Empty)
                                r["DisplayOrder"] = "0";
                            else
                                r["DisplayOrder"] = txtDisplayOrder.Text;
                        }
                    }
                }
                if (check == 0)
                {
                    dt.Rows.Add(r);
                    dt.AcceptChanges();
                    gridProcess.Columns[4].Visible = true;
                    gridProcess.DataSource = dt;
                    gridProcess.DataBind();
                    gridProcess.Columns[4].Visible = false;
                   
                }
            }
            else
            {
                MasterProcessTemplate infoProtemp = new MasterProcessTemplate();
                infoProtemp.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                infoProtemp.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                infoProtemp.ProcessCd = int.Parse(txtProcessCd.Text);
                if (txtDisplayOrder.Text == string.Empty)
                    infoProtemp.DisplayOrder = 0;
                else
                    infoProtemp.DisplayOrder = int.Parse(txtDisplayOrder.Text);
                infoProtemp.ModifiedDate = DateTime.Today;
                infoProtemp.ModifiedAccount = this.User.Identity.Name;


                if (txtProcessCd.Enabled == true)
                {
                    infoProtemp.CreateAccount = infoProtemp.ModifiedAccount;
                    infoProtemp.CreateDate = infoProtemp.ModifiedDate;
                    MasterProcessTemplate checkProTemp = MasterProcessTemplate.GetProcessTempalteMaster(int.Parse(HiddenFieldOfficeCd.Value),int.Parse(tbxProsthesisCd.Text),int.Parse(txtProcessCd.Text));
                    if (checkProTemp == null)
                    {
                        infoProtemp.Insert();

                    }
                    if (checkProTemp != null)
                    {

                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckProcesstemplete.Text")) + "\");");
                        return;
                    }

                }
                else
                {
                    infoProtemp.Update();

                }
                FillDataToGridProcess();
               
            }
            PanelProcessEdit.Visible = false;
            PanelProcessView.Visible = true;
            if (btDelete.Enabled == false)
                tbxProsthesisCd.Enabled = true;
            if (PanelProcessView.Enabled == true)
                panelEdit.Enabled = true;
           
        }
        catch (Exception ex)
        {
            logger.Error("Error Save Process Templete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btDelete.Enabled == false)
            {
                DataTable dt = (DataTable)ViewState["Process"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == txtProcessCd.Text)
                        dt.Rows.RemoveAt(i);

                }
                gridProcess.DataSource = dt;
                gridProcess.DataBind();
            }
            else
            {
                MasterProcessTemplate mstProcessTemp = new MasterProcessTemplate();
                mstProcessTemp.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                mstProcessTemp.ProcessCd = int.Parse(txtProcessCd.Text);
                mstProcessTemp.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                mstProcessTemp.Delete();
                FillDataToGridProcess();
            }
            PanelProcessEdit.Visible = false;
            PanelProcessView.Visible = true;
            if (btDelete.Enabled == false)
                tbxProsthesisCd.Enabled = true;
            if (PanelProcessView.Enabled == true)
                panelEdit.Enabled = true;

        }
        catch (Exception ex)
        {
            logger.Error("Error Delete Process Templete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PanelProcessEdit.Visible = false;
        PanelProcessView.Visible = true;
        if (btDelete.Enabled == false)
            tbxProsthesisCd.Enabled = true;
        if (PanelViewTechPrice.Visible == true)
            panelEdit.Enabled = true;
    }

    protected void selProcessRegister_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProcessCd.Text = selProcessRegister.SelectedValue.ToString();
    }

    protected void btnRegTechPrice_Click(object sender, EventArgs e)
    {
        
        txtTechCd.Text = "";
        selTechPriceEdit.SelectedValue = "";
        txtDisplayOrderTech.Text = "";
        tbxProsthesisCd.Enabled = false;
        txtTechCd.Enabled = true;
        selTechPriceEdit.Enabled = true;
        selTechPriceEdit.SelectedValue = "";
        PanelViewTechPrice.Visible = false;
        PanelTechPriceEdit.Visible = true;
        panelEdit.Enabled = false;
        btnDeleteTechPrice.Enabled = false;
        tbxProsthesisCd1.Text = tbxProsthesisCd.Text;
        tbxProsthesisNm1.Text = tbxProsthesisNm.Text;
    }

    protected void btnEditTechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            int countChecked = 0;
            string displayorder = "";
            foreach (GridViewRow r in gridViewTech.Rows)
            {
                CheckBox chkCheck = (CheckBox)r.Cells[0].FindControl("cbTechPrice");
                if (chkCheck.Checked)
                {
                    countChecked++;
                    if (countChecked == 2) break;
                    else
                    {
                        txtTechCd.Text = Common.GetRowString(r.Cells[1].Text);
                        displayorder = Common.GetRowString(r.Cells[8].Text);
                    }
                }
            }

            if (countChecked == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            }
            else if (countChecked > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                txtTechCd.Enabled = false;
                selTechPriceEdit.Enabled = false;
                if (btDelete.Enabled == false)
                    txtDisplayOrderTech.Text = displayorder;
                else
                    txtDisplayOrderTech.Text = MasterTechPriceTemplate.GetTechPriceTempleteMaster(int.Parse(HiddenFieldOfficeCd.Value),int.Parse(tbxProsthesisCd.Text),int.Parse(txtTechCd.Text)).DisplayOrder.ToString();
                selTechPriceEdit.SelectedValue = txtTechCd.Text;
                PanelViewTechPrice.Visible = false;
                PanelTechPriceEdit.Visible = true;
                btnDeleteTechPrice.Enabled = true;
                tbxProsthesisCd1.Text = tbxProsthesisCd.Text;
                tbxProsthesisNm1.Text = tbxProsthesisNm.Text;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit Techprice Templete", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");");
        }
    }

    protected void btnSavetechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            if (btDelete.Enabled == false)
            {
                int check = 0;
                DataTable dt = (DataTable)ViewState["Tech"];
                List<MasterTech> listTech = new List<MasterTech>();
                listTech = MasterTech.GetAll().Where(t => (t.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value) && t.TechCd == int.Parse(txtTechCd.Text) && (t.StartDate <= DateTime.Today || t.StartDate >= DateTime.Today) && (t.EndDate == null || t.EndDate >= DateTime.Today))).ToList();
                
                foreach (GridViewRow r in gridViewTech.Rows)
                {
                    if (r.Cells[1].Text == txtTechCd.Text)
                    {
                        check = 1;
                        if (btnDeleteTechPrice.Enabled == false)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckTechPricetemplete.Text")) + "\");");
                            
                            return;
                        }
                        else
                        {
                            r.Cells[8].Text = string.IsNullOrWhiteSpace(txtDisplayOrderTech.Text) ? "0" : txtDisplayOrderTech.Text;
                        }
                    }
                }
                if (check == 0)
                {

                    foreach (MasterTech tech in listTech)
                    {
                        DataRow r = dt.NewRow();
                        r["TechCd"] = tech.TechCd;
                        r["TechNm"] = tech.TechNm;
                        r["StandardTechPrice"] = tech.StandardTechPrice;
                        r["Editable"] = tech.Editable;
                        r["TechGroup"] = tech.TechGroup;
                        r["StartDate"] = tech.StartDate;
                        r["EndDate"] = tech.EndDate;
                        if (txtDisplayOrderTech.Text == string.Empty)
                            r["DisplayOrder"] = 0;
                        else
                            r["DisplayOrder"] = txtDisplayOrderTech.Text;
                        dt.Rows.Add(r);

                    }

                    dt.AcceptChanges();
                    gridViewTech.Columns[8].Visible = true;
                    gridViewTech.DataSource = dt;
                    gridViewTech.DataBind();
                    gridViewTech.Columns[8].Visible = false;
                    foreach (GridViewRow r in gridViewTech.Rows)
                    {
                        if (r.Cells[4].Text == "True")
                            r.Cells[4].Text = "√";
                        else
                            r.Cells[4].Text = "";
                        if (r.Cells[5].Text == "-1")
                            r.Cells[5].Text = "";
                        r.Cells[6].Text = SetDateFormat(r.Cells[6].Text);
                        if (Common.GetRowString(r.Cells[7].Text) != "")
                        {
                            r.Cells[7].Text = SetDateFormat(r.Cells[7].Text);
                        }
                    }
                }
            }

            else
            {
                MasterTechPriceTemplate infoTechtemp = new MasterTechPriceTemplate();
                infoTechtemp.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                infoTechtemp.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                infoTechtemp.TechCd = int.Parse(txtTechCd.Text);
                if (txtDisplayOrderTech.Text == string.Empty)
                    infoTechtemp.DisplayOrder = 0;
                else infoTechtemp.DisplayOrder = int.Parse(txtDisplayOrderTech.Text);
                infoTechtemp.ModifiedAccount = this.User.Identity.Name;
                infoTechtemp.ModifiedDate = DateTime.Today;

                if (txtTechCd.Enabled == true)
                {

                    MasterTechPriceTemplate checkTechTemp = MasterTechPriceTemplate.GetTechPriceTempleteMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxProsthesisCd.Text), int.Parse(txtTechCd.Text));
                    if (checkTechTemp == null)
                    {
                        infoTechtemp.CreateAccount = infoTechtemp.ModifiedAccount;
                        infoTechtemp.CreateDate = infoTechtemp.ModifiedDate;
                        infoTechtemp.Insert();

                    }
                    if (checkTechTemp != null)
                    {

                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckTechPricetemplete.Text")) + "\");");
                        return;
                    }

                }
                else
                {
                    infoTechtemp.Update();

                }
                FillDataToGridTech();
            }

            PanelTechPriceEdit.Visible = false;
            PanelViewTechPrice.Visible = true;
            if (btDelete.Enabled == false)
                tbxProsthesisCd.Enabled = true;
            if (PanelProcessView.Enabled == true)
                panelEdit.Enabled = true;
            lblNorecordFoundTech.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error("Error Save TechPrice Templete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btnDeleteTechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            if (btDelete.Enabled == false)
            {
                DataTable dt = (DataTable)ViewState["Tech"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == txtTechCd.Text)
                    {
                        dt.Rows.RemoveAt(i);
                        dt.AcceptChanges();
                        i--;
                    }

                }

                gridViewTech.Columns[8].Visible = true;
                gridViewTech.DataSource = dt;
                gridViewTech.DataBind();
                gridViewTech.Columns[8].Visible = false;
                foreach (GridViewRow r in gridViewTech.Rows)
                {
                    if (r.Cells[4].Text == "True")
                        r.Cells[4].Text = "√";
                    else
                        r.Cells[4].Text = "";
                    if (r.Cells[5].Text == "-1")
                        r.Cells[5].Text = "";
                    r.Cells[6].Text = SetDateFormat(r.Cells[6].Text);
                    if (Common.GetRowString(r.Cells[7].Text) != "")
                    {
                        r.Cells[7].Text = SetDateFormat(r.Cells[7].Text);
                    }
                }
            }
            else
            {
                MasterTechPriceTemplate techTemp = new MasterTechPriceTemplate();
                techTemp.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                techTemp.TechCd = int.Parse(txtTechCd.Text);
                techTemp.ProsthesisCd = int.Parse(tbxProsthesisCd.Text);
                techTemp.Delete();
                FillDataToGridTech();
            }
            PanelTechPriceEdit.Visible = false;
            PanelViewTechPrice.Visible = true;
            if (btDelete.Enabled == false)
                tbxProsthesisCd.Enabled = true;
        }
        catch (Exception ex)
        {

            logger.Error("Error delete TechpriceTemplete", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btnCancelTechPrice_Click(object sender, EventArgs e)
    {
        PanelTechPriceEdit.Visible = false;
        PanelViewTechPrice.Visible = true;
        if (btDelete.Enabled == false)
            tbxProsthesisCd.Enabled = true;
        if (PanelProcessView.Visible == true)
            panelEdit.Enabled = true;
    }

    protected void btnViewTechPice_Click(object sender, EventArgs e)
    {
        try
        {
            int countChecked = 0;
            string techcd = ""; DateTime startdate = DateTime.Today;
            foreach (GridViewRow r in gridViewTech.Rows)
            {
                CheckBox chkCheck = (CheckBox)r.Cells[0].FindControl("cbTechPrice");
                if (chkCheck.Checked)
                {
                    countChecked++;
                    if (countChecked == 2) break;
                    else
                    {
                        techcd = Common.GetRowString(r.Cells[1].Text);
                        startdate = DateTime.Parse(Common.GetRowString(r.Cells[6].Text));
                    }
                }
            }

            if (countChecked == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            }
            else if (countChecked > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                List<TechPriceBasedOnTech> techpriceinfo = new List<TechPriceBasedOnTech>();
                techpriceinfo = TechPriceBasedOnTech.GetTechPriceByTechCd(int.Parse(HiddenFieldOfficeCd.Value),int.Parse(techcd),startdate);
                lblNorecordTechPrice.Visible = false;
                if (techpriceinfo.Count == 0)
                {
                    TechPriceBasedOnTech info = new TechPriceBasedOnTech();
                    info.OfficeCd = 9;
                    info.TechCd = -1;
                    info.DentalOfficeCd = 1;
                    info.StartDate = DateTime.Now;
                    techpriceinfo.Add(info);
                    GridViewTechPrice.DataSource = techpriceinfo;
                    GridViewTechPrice.DataBind();
                    if (techpriceinfo.Count == 1 && techpriceinfo[0].TechCd == -1)
                    {
                        GridViewTechPrice.Rows[0].Visible = false;
                    }

                    lblNorecordTechPrice.Visible = true;

                }
                else
                {
                    GridViewTechPrice.DataSource = techpriceinfo;
                    GridViewTechPrice.DataBind();
                    foreach (GridViewRow r in GridViewTechPrice.Rows)
                    {
                        r.Cells[1].Text = Convert.ToDateTime(r.Cells[1].Text).ToShortDateString();
                    }
                }
                PanelViewTechPrice.Visible = false;
                PanelViewTechPriceDetail.Visible = true;

            }
        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() Error.", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");");
        }
    }
    protected void GridViewTechPrice_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();


            //Add TechCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_NAME.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Startdate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_START_DATE.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentalOffice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DentalOffice_Nm_DantalOfficeNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add TechPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_PRICE_TechPrice.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void btnCancelTechDetail_Click(object sender, EventArgs e)
    {
        PanelViewTechPrice.Visible = true;
        PanelViewTechPriceDetail.Visible = false;
    }
    protected void gridProsthesis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridProsthesis.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void gridProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridProcess.PageIndex = e.NewPageIndex;
        if (btDelete.Enabled == false)
        {
            DataTable dt = (DataTable)ViewState["Process"];
            gridProcess.Columns[4].Visible = true;
            gridProcess.DataSource = dt;
            gridProcess.DataBind();
            gridProcess.Columns[4].Visible = false;
        }
        else
        {
            FillDataToGridProcess();
        }
    }
    protected void gridViewTech_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridViewTech.PageIndex = e.NewPageIndex;
        if (btDelete.Enabled == false)
        {
            DataTable dt = (DataTable)ViewState["Tech"];
            gridViewTech.Columns[8].Visible = true;
            gridViewTech.DataSource = dt;
            gridViewTech.DataBind();
            gridViewTech.Columns[8].Visible = false;
            foreach (GridViewRow r in gridViewTech.Rows)
            {
                if (r.Cells[4].Text == "True")
                    r.Cells[4].Text = "√";
                else
                    r.Cells[4].Text = "";
                if (r.Cells[5].Text == "-1")
                    r.Cells[5].Text = "";
                r.Cells[6].Text = SetDateFormat(r.Cells[6].Text);
                if (Common.GetRowString(r.Cells[7].Text) != "")
                {
                    r.Cells[7].Text = SetDateFormat(r.Cells[7].Text);
                }
            }
        }
        else
        {
            FillDataToGridTech();
        }
    }
    protected void GridViewTechPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewTechPrice.PageIndex = e.NewPageIndex;

    }
    protected void txtProcessSearch_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtProcessSearch, selProcess);
    }
    protected void txtTechPriceSearch_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtTechPriceSearch, selTechPrice);
    }
    protected void txtProcessCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtProcessCd, selProcessRegister);
    }
    protected void txtTechCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtTechCd, selTechPriceEdit);
    }
}