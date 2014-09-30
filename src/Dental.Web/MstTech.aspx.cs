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

public partial class MstTech : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstTech));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
            if (!IsPostBack)
            {
                InitLanguage();
                InitCalendar();
                Initialize();
                FillDentalOffice();
                Search();
            }
        }
        catch (Exception ex)
        {

            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }


    private void Initialize()
    {
        ViewState.Remove("dtDatas");
        HiddenFieldOfficeCd.Value = GetOffice();
        btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        btnDeleteTechPrice.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        cbAvailabelTech.Text = GetResource("cbAvailableTech.Text");
        lblTechCd_search.Text = GetResource("colTechCd.Text");
        lblTechNm_Search.Text = GetResource("colTechNm.Text");
        lblStartDate_Search.Text = GetResource("colStartDate.Text");
        LabelNorecord.Visible = lblNorecord.Visible = true;
        LabelNorecord.Text = lblNorecord.Text = GetResource("lblNoRecord.Text");
        LabelNorecord.Visible = lblNorecord.Visible = false;
        lblTechCd.Text = GetResource("colTechCd.Text");
        lblTechNm.Text = GetResource("colTechNm.Text"); ;
        lblStartDate.Text = GetResource("colStartDate.Text");
        lblEndDate.Text = GetResource("colEndDate.Text");
        lblStandardTechPrice.Text = GetResource("colStandardTechPrice.Text");
        lblTechGroup.Text = GetResource("colTechGroup.Text");
        lblEditable.Text = cbEditable.Text = GetResource("colEditable.Text");
        LabelDentalOffice.Text = GetResource("HEADER_DentalOffice_CODE_DentalOfficeCd.Text");
        LabelTechPrice.Text = GetResource("HEADER_TECH_PRICE_TechPrice.Text");
        lblTechTitle.Text = GetResource("lblTechTitle.Text");
        lblTechPriceTitle.Text = GetResource("lblTechPriceTitle.Text");
        

        btnCancelTechPrice.Text = btnCancel.Text = GetResource("btnCancel.Text");
        btnDeleteTechPrice.Text = btnDelete.Text = GetResource("btnDelete.Text");
        btnSearch.Text = GetResource("btnSearch.Text");
        btnClear.Text = GetResource("btnClear.Text");
        btnEditTechPrice.Text = btnEdit.Text = GetResource("btnEdit.Text");
        btnRegisterTechPrice.Text = btnRegister.Text = GetResource("btnRegister.Text");
        btnSaveTechPrice.Text = btnSave.Text = GetResource("btnSave.Text");

        reqTechCd.ErrorMessage = GetResource("reqTechCd.Text");
        reqTechNm.ErrorMessage = GetResource("reqTechNm.Text");
        reqStartDate.ErrorMessage = GetResource("reqStartDate.Text");
        RequiredFieldValidatorTechPrice.ErrorMessage = GetResource("valRequired_TechPrice.Text");
        valRequiredDentalOffice.ErrorMessage = GetResource("valRequiredDentalOffice.Text");
        ReqStandardTechPrice.ErrorMessage = GetResource("ReqStandardTechPrice.Text");

        CompareValidatorStartDate.ErrorMessage = GetResource("compDate.Text");
        CompareValidatorEndDate.ErrorMessage = GetResource("compDate.Text");
    }

    private void InitCalendar()
    {
        hplStartDate1.NavigateUrl = Calendar.InvokePopupCal(txtStartDate1);
        hplStartDate2.NavigateUrl = Calendar.InvokePopupCal(txtStartDate2);
        hplStartDate.NavigateUrl = Calendar.InvokePopupCal(txtStartDate);
        hplEndDate.NavigateUrl = Calendar.InvokePopupCal(txtEndDate);
    }

    private void FillDentalOffice()
    {
        List<MasterDentalOffice> DentalOfficeMasterInfo = new List<MasterDentalOffice>();
        DentalOfficeMasterInfo = MasterDentalOffice.GetAll();
        DentalOfficeMasterInfo = DentalOfficeMasterInfo.Where(d => (d.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value))).ToList();
        selDentalOffice.Items.Add(new ListItem("", ""));
        foreach (var i in DentalOfficeMasterInfo)
        {
            selDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }
    }

    private void Search()
    {
        try
        {
            lblNorecord.Visible = false;

            List<MasterTech> techInfo = new List<MasterTech>();
            techInfo = MasterTech.GetTechMasterSearch(int.Parse(HiddenFieldOfficeCd.Value), txtTechCd_search.Text, txtTechNm_search.Text, txtStartDate1.Text, txtStartDate2.Text);

            if (!cbAvailabelTech.Checked)
            {
                techInfo = techInfo.Where(t => ((t.StartDate <= DateTime.Today || t.StartDate >= DateTime.Today) && (t.EndDate == null || t.EndDate.Value >= DateTime.Today))).ToList();
            }

            if (techInfo.Count == 0)
            {
                var info = new MasterTech();
                info.OfficeCd = 9;
                info.TechCd = -1;
                info.StartDate = Convert.ToDateTime("1/1/1973");
                techInfo.Add(info);
                gridTech.DataSource = techInfo;
                gridTech.DataBind();
                if (techInfo.Count == 1 && techInfo[0].TechCd == -1)
                {
                    gridTech.Rows[0].Visible = false;
                }
                lblNorecord.Visible = true;
                return;
            }

            gridTech.DataSource = techInfo;
            gridTech.DataBind();
            foreach (GridViewRow r in gridTech.Rows)
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
        catch (Exception e)
        {

            logger.Error("Error Search", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        }
    }


    protected void gridTech_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView grid = (GridView)sender;
            GridViewRow gridrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tblcell = new TableHeaderCell();

            //add checkbox header
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(20);
            gridrow.Cells.Add(tblcell);

            //add TechCd header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colTechCd.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);


            //add TechNm header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colTechNm.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(300);
            gridrow.Cells.Add(tblcell);

            //add StandardTechPrice header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colStandardTechPrice.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            //add Editable header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colEditable.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //add TechGroup header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colTechGroup.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);


            //add StartDate header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colStartDate.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //add EndDate header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("colEndDate.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //Add header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }
    protected void gridTech_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridTech.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void cbAvailabelTech_CheckedChanged(object sender, EventArgs e)
    {
        Search();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtTechCd_search.Text = txtTechNm_search.Text = txtStartDate1.Text = txtStartDate2.Text = string.Empty;
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        datastructure();
        btnDelete.Enabled = false;
        cbEditable.Checked = false;
        txtTechCd.Text = txtTechNm.Text = txtStartDate.Text = txtEndDate.Text = txtTechGroup.Text = txtStandardTechPrice.Text = string.Empty;
        txtTechCd.Enabled = txtStartDate.Enabled = hplStartDate.Enabled = txtTechNm.Enabled = txtStandardTechPrice.Enabled = true;
        FillToTechPrice();
        txtStartDate.Text = txtTechCd.Text = string.Empty;
        panelEdit.Visible = true;
        panelShow.Visible = false;
    }

    private void FillToTechPrice()
    {
        if (txtStartDate.Text == string.Empty) txtStartDate.Text = DateTime.Now.ToShortDateString();
        if (txtTechCd.Text == string.Empty) txtTechCd.Text = "-1";

        LabelNorecord.Visible = false;

        List<TechPriceBasedOnTech> techpriceInfo = new List<TechPriceBasedOnTech>();

        techpriceInfo = TechPriceBasedOnTech.GetTechPriceByTechCd(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtTechCd.Text), DateTime.Parse(txtStartDate.Text));

        techpriceInfo = techpriceInfo.Where(t => (t.TechCd == Common.GetNullableInt(txtTechCd.Text) && t.StartDate == Common.GetNullableDateTime(txtStartDate.Text))).ToList();

        if (techpriceInfo.Count == 0)
        {
            TechPriceBasedOnTech info = new TechPriceBasedOnTech();
            info.OfficeCd = 9;
            info.TechCd = -1;
            info.DentalOfficeCd = 1;
            info.StartDate = DateTime.Now;
            
            techpriceInfo.Add(info);
            gridTechPrice.Columns[1].Visible = true;
            gridTechPrice.Columns[3].Visible = true;
            gridTechPrice.Columns[4].Visible = true;
            gridTechPrice.DataSource = techpriceInfo;
            gridTechPrice.DataBind();
            if (techpriceInfo.Count == 1 && techpriceInfo[0].TechCd == -1)
            {
                gridTechPrice.Rows[0].Visible = false;
            }
            gridTechPrice.Columns[1].Visible = false;
            gridTechPrice.Columns[3].Visible = false;
            gridTechPrice.Columns[4].Visible = false;
            LabelNorecord.Visible = true;

            return;

        }
        if (techpriceInfo != null && techpriceInfo.Count > 0)
        {

            gridTechPrice.Columns[1].Visible = true;
            gridTechPrice.Columns[3].Visible = true;
            gridTechPrice.Columns[4].Visible = true;
            gridTechPrice.DataSource = techpriceInfo;
            gridTechPrice.DataBind();
            foreach (GridViewRow r in gridTechPrice.Rows)
            {
                r.Cells[4].Text = Convert.ToDateTime(r.Cells[4].Text).ToShortDateString();

            }
            gridTechPrice.Columns[1].Visible = false;
            gridTechPrice.Columns[3].Visible = false;
            gridTechPrice.Columns[4].Visible = false;
        }
    }

    private void datastructure()
    {
        DataTable dtDatas = new DataTable();
        //add columns to the datatable
        dtDatas.Columns.Add("TechCd");
        dtDatas.Columns.Add("DentalOfficeCd");
        dtDatas.Columns.Add("TechNm");
        dtDatas.Columns.Add("StartDate");
        dtDatas.Columns.Add("DentalOfficeNm");
        dtDatas.Columns.Add("TechPrice");
        //store the state of the datatable into a ViewState object
        ViewState["dtDatas"] = dtDatas; 
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState.Remove("dtDatas");
        
        int countCheck = 0;
        CheckBox cb = new CheckBox();
        foreach (GridViewRow r in gridTech.Rows)
        {
            cb = (CheckBox)r.Cells[0].FindControl("cb");
            if (cb.Checked)
            {
                countCheck++;
                if (countCheck == 2) break;
                else
                {
                    txtTechCd.Text = Common.GetRowString(r.Cells[1].Text);
                    txtStartDate.Text = Common.GetRowString(r.Cells[6].Text);
                    txtEndDate.Text = Common.GetRowString(r.Cells[7].Text);
                    txtTechNm.Text = Common.GetRowString(r.Cells[2].Text);
                    txtStandardTechPrice.Text = Common.GetRowString(r.Cells[3].Text);
                    txtTechGroup.Text = Common.GetRowString(r.Cells[5].Text);
                    if (r.Cells[4].Text == "√")
                    {
                        cbEditable.Checked = true;
                        txtTechNm.Enabled = txtStandardTechPrice.Enabled = true;
                    }
                    else
                    {
                        cbEditable.Checked = false;
                        txtTechNm.Enabled = txtStandardTechPrice.Enabled = false;
                    }
                }
            }

        }
        if (countCheck == 0)
        {
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            //RegisterStartupScript("ShowMessage('" + Localization.GetString("TITLE_ERROR.Text", LocalResourceFile) + "','" + Localization.GetString("MSG_NONE_SELECTED_ITEM.Text", LocalResourceFile) + "');");
            return;
        }
        if (countCheck == 2)
        {
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            return;
        }
        btnDelete.Enabled = true;
        txtTechCd.Enabled = txtStartDate.Enabled = hplStartDate.Enabled = false;
        FillToTechPrice();
        panelEdit.Visible = true;
        panelShow.Visible = false;
    }
    protected void gridTechPrice_RowCreated(object sender, GridViewRowEventArgs e)
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

            //Add DentalOfficeCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DentalOffice_CODE_DentalOfficeCd.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentalOfficeNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DentalOffice_Nm_DantalOfficeNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(200);
            oGridViewRow.Cells.Add(oTableCell);

            //Add TechPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TECH_PRICE_TechPrice.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);


            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterTech techInfo = new MasterTech();

            techInfo.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            techInfo.TechCd = int.Parse(txtTechCd.Text);
            techInfo.StartDate = Convert.ToDateTime(txtStartDate.Text);
            techInfo.EndDate = Common.GetNullableDateTime(txtEndDate.Text);
            techInfo.TechNm = txtTechNm.Text;
            techInfo.StandardTechPrice = int.Parse(txtStandardTechPrice.Text);
            techInfo.TechGroup = Common.GetNullableInt(txtTechGroup.Text);
            if (cbEditable.Checked)
                techInfo.Editable = true;
            else
                techInfo.Editable = false;
            techInfo.ModifiedDate = DateTime.Today;
            techInfo.ModifiedAccount = this.User.Identity.Name;

            MasterTech info = MasterTech.GetTechMaster(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtTechCd.Text), Convert.ToDateTime(txtStartDate.Text));

            if (btnDelete.Enabled == false)// && txtTechCd.Enabled == true)
            {
               
                if (info == null)
                {
                    techInfo.CreateAccount = this.User.Identity.Name;
                    techInfo.CreateDate = DateTime.Today;
                    techInfo.Insert();
                    if (gridTechPrice.Rows.Count > 0 && gridTechPrice.Rows[0].Visible != false)
                    {
                        DataTable dtDatas = (DataTable)ViewState["dtDatas"];
                        foreach (DataRow r in dtDatas.Rows)
                        {
                            MasterTechPrice techpriceInfo = new MasterTechPrice();
                            techpriceInfo.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                            techpriceInfo.TechCd = int.Parse(txtTechCd.Text);
                            techpriceInfo.StartDate = Convert.ToDateTime(txtStartDate.Text);
                            techpriceInfo.DentalOfficeCd = int.Parse(r["DentalOfficeCd"].ToString());
                            techpriceInfo.TechPrice = int.Parse(r["TechPrice"].ToString());
                            techpriceInfo.CreateAccount = this.User.Identity.Name;
                            techpriceInfo.ModifiedAccount = this.User.Identity.Name;
                            techpriceInfo.CreateDate = DateTime.Today;
                            techpriceInfo.ModifiedDate = DateTime.Today;

                            techpriceInfo.Insert();
                            
                        }
                    }
                }
                else
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("Check_Insert_Tech.Text")) + "\");");
                    return;
                }
            }
            else
            {
                techInfo.CreateAccount = info.CreateAccount;
                techInfo.CreateDate = info.CreateDate;
                techInfo.Update();
            }
            Search();
            panelEdit.Visible = false;
            panelShow.Visible = true;
        }
        catch (Exception ex)
        {

            logger.Error("Error Save", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnRegisterTechPrice_Click(object sender, EventArgs e)
    {
        txtDentalOfficeCd.Text = "";
        selDentalOffice.SelectedValue = "";
        txtTechPrice.Text = "";
        selDentalOffice.Enabled = txtDentalOfficeCd.Enabled = true;
        btnDeleteTechPrice.Enabled = false;
        PanelEditTech.Enabled = false;
        PanelTechPrice.Visible = false;
        PanelEditTechPrice.Visible = true; 
    }
    protected void btnEditTechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            int countChecked = 0;
            foreach (GridViewRow r in gridTechPrice.Rows)
            {
                CheckBox chkCheck = (CheckBox)r.Cells[0].FindControl("cb");
                if (chkCheck.Checked)
                {
                    countChecked++;
                    if (countChecked == 2) break;
                    else
                    {

                        txtDentalOfficeCd.Text = Common.GetRowString(r.Cells[2].Text);
                        txtTechPrice.Text = Common.GetRowString(r.Cells[6].Text);
                    }
                }
            }

            if (countChecked == 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
            }
            else if (countChecked > 1)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
            }
            else
            {
                selDentalOffice.SelectedValue = txtDentalOfficeCd.Text;
                selDentalOffice.Enabled = txtDentalOfficeCd.Enabled = false;
                btnDeleteTechPrice.Enabled = true;
                PanelEditTechPrice.Visible = true;
                PanelTechPrice.Visible = false;
                PanelEditTech.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit TechPrice", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
        
    }

    protected void btnSaveTechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            if (!btnDelete.Enabled)
            {


                //take out the source from the viewstate object
                DataTable dtDatas = (DataTable)ViewState["dtDatas"];
                //initialize a new Datarow object
                DataRow drNewRow = dtDatas.NewRow();
                drNewRow["TechCd"] = txtTechCd.Text;
                drNewRow["DentalOfficeCd"] = txtDentalOfficeCd.Text;
                drNewRow["TechNm"] = txtTechNm.Text;
                drNewRow["StartDate"] = txtStartDate.Text;
                drNewRow["DentalOfficeNm"] = selDentalOffice.SelectedItem.Text;
                drNewRow["TechPrice"] = txtTechPrice.Text;
                int check = 0;
                for (int i = 0; i < dtDatas.Rows.Count; i++)
                {
                    if (dtDatas.Rows[i][1].ToString() == txtDentalOfficeCd.Text && !btnDeleteTechPrice.Enabled)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_EXIST_TECH_CD_AND_START_DATE.Text")) + "\");");
                        check = 1;
                        break;
                    }
                    else
                    {
                        if (dtDatas.Rows[i][1].ToString() == txtDentalOfficeCd.Text)
                        {
                            dtDatas.Rows.RemoveAt(i);
                            drNewRow["TechCd"] = txtTechCd.Text;
                            drNewRow["DentalOfficeCd"] = txtDentalOfficeCd.Text;
                            drNewRow["TechNm"] = txtTechNm.Text;
                            drNewRow["StartDate"] = txtStartDate.Text;
                            drNewRow["DentalOfficeNm"] = selDentalOffice.SelectedItem.Text;
                            drNewRow["TechPrice"] = txtTechPrice.Text;
                        }
                    }
                }


                if (check == 0)
                {
                    dtDatas.Rows.Add(drNewRow);
                    dtDatas.AcceptChanges();
                    gridTechPrice.DataSource = dtDatas;
                    gridTechPrice.DataBind();

                    PanelEditTechPrice.Visible = false;
                    PanelTechPrice.Visible = true;
                    PanelEditTech.Enabled = true;
                    LabelNorecord.Visible = false;
                }
            }
            if (btnDelete.Enabled)
            {
                var row = MasterTechPrice.GetMstTechPrice(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtTechCd.Text), Convert.ToDateTime(txtStartDate.Text), int.Parse(txtDentalOfficeCd.Text));
                if (row == null)
                {
                    row = FillTechPriceObj();
                    row.CreateAccount = this.User.Identity.Name;
                    row.CreateDate = DateTime.Today;
                    row.Insert();
                }
                else
                {
                    if (txtDentalOfficeCd.Enabled)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), GetResource("MSG_EXIST_TECH_CD_AND_START_DATE.Text")) + "\");");
                        return;
                    }
                    row = FillTechPriceObj();
                    row.Update();
                }
                FillToTechPrice();
                PanelEditTechPrice.Visible = false;
                PanelTechPrice.Visible = true;
                PanelEditTech.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Save TechPrice", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnDeleteTechPrice_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnDelete.Enabled)
            {
                MasterTechPrice mstTechPrice = new MasterTechPrice();
                mstTechPrice.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                mstTechPrice.TechCd = int.Parse(txtTechCd.Text);
                mstTechPrice.DentalOfficeCd = int.Parse(txtDentalOfficeCd.Text);
                mstTechPrice.StartDate = Convert.ToDateTime(txtStartDate.Text);

                mstTechPrice.Delete();

                FillToTechPrice();

            }
            if (!btnDelete.Enabled)
            {
                DataTable dtDatas = (DataTable)ViewState["dtDatas"];
                for (int i = 0; i < dtDatas.Rows.Count; i++)
                {
                    if (dtDatas.Rows[i][1].ToString() == txtDentalOfficeCd.Text)
                        dtDatas.Rows.RemoveAt(i);
                }
                gridTechPrice.DataSource = dtDatas;
                gridTechPrice.DataBind();
            }
            PanelEditTechPrice.Visible = false;
            PanelTechPrice.Visible = true;
            PanelEditTech.Enabled = true;
        }
        catch (Exception ex)
        {
            logger.Error("Error Delete TechPrice", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        
        }
    }
    protected void btnCancelTechPrice_Click(object sender, EventArgs e)
    {

        PanelEditTechPrice.Visible = false;
        PanelTechPrice.Visible = true;
        PanelEditTech.Enabled = true;
    }

    private MasterTechPrice FillTechPriceObj()
    {
        MasterTechPrice obj = new MasterTechPrice();
        obj.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
        obj.TechCd = int.Parse(txtTechCd.Text);
        obj.DentalOfficeCd = int.Parse(txtDentalOfficeCd.Text);
        obj.StartDate = Convert.ToDateTime(txtStartDate.Text);
        obj.TechPrice = int.Parse(txtTechPrice.Text);
        obj.ModifiedDate = DateTime.Today;
        obj.ModifiedAccount = this.User.Identity.Name;
        return obj;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState.Remove("dtDatas");
        panelEdit.Visible = false;
        panelShow.Visible = true;
    }
    protected void txtDentalOfficeCd_TextChanged(object sender, EventArgs e)
    {
        ListItem item = selDentalOffice.Items.FindByValue(txtDentalOfficeCd.Text);
        if (item != null)
        {
            selDentalOffice.SelectedValue = txtDentalOfficeCd.Text;
        }
        else
        {
            selDentalOffice.SelectedIndex = 0;
            txtDentalOfficeCd.Text = selDentalOffice.SelectedValue;
            txtDentalOfficeCd.Focus();
        }
    }
    protected void gridTechPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridTechPrice.PageIndex = e.NewPageIndex;
        if (!btnDelete.Enabled)
        {
            DataTable dtDatas = (DataTable)ViewState["dtDatas"];
            gridTechPrice.DataSource = dtDatas;
            gridTechPrice.DataBind();
        }
        else
        {
            FillToTechPrice();
        }
    }
}