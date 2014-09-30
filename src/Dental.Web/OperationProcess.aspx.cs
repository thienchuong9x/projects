using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class OperationProcess : DDVPortalModuleBase 
{
    readonly static ILog logger = LogManager.GetLogger(typeof(OperationProcess));

    #region ViewState
    public objDropDownList DropDownSource
    {
        get
        {
            if (this.ViewState["DropDownSource"] == null)
            {
                this.ViewState["DropDownSource"] = new objDropDownList(Convert.ToInt32(hiddenOfficeCd.Value));
            }
            return this.ViewState["DropDownSource"] as objDropDownList;
        }
    }
    public List<OperationProcessInfo> listDetail
    {
        get
        {
            if (this.ViewState["OperationProcess"] == null)
            {
                this.ViewState["OperationProcess"] = OperationProcessInfo.GetOperationProcessInfo(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value), Convert.ToDateTime(txtOrderDate.Text));
            }
            return this.ViewState["OperationProcess"] as List<OperationProcessInfo>;
        }
    }

    private List<CustomProcess> CustomProcess
    {
        get
        {
            if (this.ViewState["CurrentProcess"] == null)
            {
                int row = gridViewOrderDetail.SelectedRow.RowIndex;
                this.ViewState["CurrentProcess"] = this.listDetail[row].listCustomProcess;
            }
            return this.ViewState["CurrentProcess"] as List<CustomProcess>;
        }
    }

    private void RemoveViewState(params string[] nameState)
    {
        // CurrentDetail0.
        foreach (string state in nameState)
        {
            if (ViewState[state] != null)
            {
                ViewState.Remove(state);
            }
        }
    }
    #endregion

    #region Fill GridView
    private void BindGridView()
    {
        BindGridView(this.CustomProcess);
    }
    private void BindGridView(List<CustomProcess> list)
    {
        this.gridViewProcess.DataSource = list;
        this.gridViewProcess.DataBind();

        if (Convert.ToInt32(((DataBoundLiteralControl)this.gridViewProcess.Rows[0].Cells[4].Controls[0]).Text) == 0)
        {
            this.gridViewProcess.Rows[0].Visible = false;
        }
        this.gridViewProcess.Columns[5].Visible = false;
        this.gridViewProcess.Columns[6].Visible = false;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InitCalendar();
            if (!IsPostBack)
            {
                InitLanguage();

                this.buttonUpRow.Attributes.Add("onclick", "javascript:return UpDownDeleteRow('" + GetResource("MSG_CHOOSE_ROW_IN_GRIDVIEW.Text") + "','');");
                this.buttonDownRow.Attributes.Add("onclick", "javascript:return UpDownDeleteRow('" + GetResource("MSG_CHOOSE_ROW_IN_GRIDVIEW.Text") + "','');");
                this.buttonDeleteRow.Attributes.Add("onclick", "javascript:return UpDownDeleteRow('" + GetResource("MSG_CHOOSE_ROW_IN_GRIDVIEW.Text") + "','" + GetResource("MSG_CONFIRM_DELETE.Text") + "');");
                this.btnGoTechPrice.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_TECH_PRICE.Text") + "');");
                this.btnGoOrder.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_ORDER.Text") + "');");
                this.btnRegister.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()){ this.disabled=true;}" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString());
                RemoveViewState("CurrentProcess", "OperationProcess", "DropDownSource");
                InitLabel();
                InitHeader();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    private void InitCalendar()
    {
        hplOrderDate.NavigateUrl = Calendar.InvokePopupCal(txtOrderDate);
        hplDueDate.NavigateUrl = Calendar.InvokePopupCal(txtDueDate);
        hplCompleteDate.NavigateUrl = Calendar.InvokePopupCal(txtCompleteDate);

        hplOutsourceDate.NavigateUrl = Calendar.InvokePopupCal(txtOutsourceDate);
        hplReceiveEstimateDate.NavigateUrl = Calendar.InvokePopupCal(txtReceiveEstimateDate);
        hplReceiveDate.NavigateUrl = Calendar.InvokePopupCal(txtReceiveDate);
    }
    private void InitLabel()
    {
        optInternal.Text = GetResource("optInternal.Text");
        optOutsource.Text = GetResource("optOutsource.Text");

        SetLabelText(lblOrderNo, lblOrderDate, lblDueDate, lblClinicName, lblPatient, lblBorrowPart, lblComments); //Header
        SetLabelText(lblMaterial, lblStock, lblMaterialPrice, lblGridProcess, lblTotalWorkTime, lblFullWorkTime, lblCompleteDate, lblInspectionStaff, lblManufactureStaff); //Internal 
        lblTechPriceUnit.Text = lblUnitPrice.Text = GetResource("lblUnitPrice.Text");

        lblTechPrice.Text = GetResource("lblTechPrice_TechPrice.Text");
        SetLabelText(lblOutsourceLab, lblOutsourceDate, lblReceiveDate, lblReceiveEstimateDate); //OutSource

        //Init Button 
        SetButtonText(buttonAddRow, buttonDownRow, buttonDeleteRow, buttonUpRow, btnRegister, btnCancel, btnGoOrder, btnGoTechPrice);
        RequiredDateCompleteDate.ErrorMessage = RequiredDateCompleteDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), lblCompleteDate.Text);

        RequiredDateOutsource.ErrorMessage = RequiredDateOutsource.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), lblOutsourceDate.Text);
        RequiredDateReceiveDate.ErrorMessage = RequiredDateReceiveDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), lblReceiveDate.Text);
        RequiredDateReceiveEstimate.ErrorMessage = RequiredDateReceiveEstimate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), lblReceiveEstimateDate.Text);
    }

    private void InitHeader()
    {
        string orderSeq = Request.QueryString["id"];
       

        hiddenOfficeCd.Value = GetOffice();
        hiddenOrderSeq.Value = orderSeq;

        if (!string.IsNullOrEmpty(orderSeq))
        {
            TrnOrderHeader header = TrnOrderHeader.GetTrnOrderHeader(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(orderSeq));
            SetOrderInputItem(header);


            this.gridViewOrderDetail.DataSource = this.listDetail.Select(p => p.detail).ToList();
            this.gridViewOrderDetail.DataBind();

            if (this.listDetail.Count > 0)
            {
                this.gridViewOrderDetail.SelectedIndex = 0;

                FillDropDownList(this.DropDownMaterial, true, CodeName.GetListMaterialByOrderDate(Convert.ToInt32(hiddenOfficeCd.Value), txtOrderDate.Text));
                FillDropDownList(this.DropDownOutsourceCompany, true, CodeName.GetMasterListCodeName(DropDownOutsourceCompany.ID, Convert.ToInt32(hiddenOfficeCd.Value)));
                List<CodeName> listTechnicalStaff = CodeName.GetTechnicalStaffListCodeName(Convert.ToInt32(hiddenOfficeCd.Value));
                FillDropDownList(this.DropDownStaff, true, listTechnicalStaff);
                FillDropDownList(this.DropDownManufactureStaff, true, listTechnicalStaff);
                SetOperationProcess();
                if (this.listDetail.FirstOrDefault(p => string.IsNullOrEmpty(p.detail.BillStatementNo)) == null)
                {
                    panelDetail.Enabled = panelProcess.Enabled = btnRegister.Enabled = false;
                }
            }
            else
            {
                this.btnRegister.Enabled = this.buttonAddRow.Enabled = false;
            }
        }
    }
    private void SetOrderInputItem(TrnOrderHeader header)
    {
        txtOrderNo.Text = header.OrderNo;
        txtOrderDate.Text = showOnlyDate(header.OrderDate.ToString());
        txtDueDate.Text = showOnlyDate(header.DeliverDate.ToString());
        txtPatientNm.Text = header.PatientLastNm + " " + header.PatientFirstNm;
        txtComments.Text = header.Note;
        txtBorrowPart.Text = header.BorrowParts;
        MasterDentalOffice itemDentalOffice = MasterDentalOffice.GetDentalOfficeMaster(header.OfficeCd, header.DentalOfficeCd);
        hiddenSupplierCd.Value = "";
        if (itemDentalOffice != null)
        {
            txtClinicNm.Text = itemDentalOffice.DentalOfficeNm;
            MasterBill itemBill = MasterBill.GetBillMaster(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToInt32(itemDentalOffice.BillCd));
            if (itemBill != null)
                hiddenSupplierCd.Value = Common.GetNullableString(itemBill.SupplierCd);
        }
    }

    #region DETAIL
    protected void gridViewOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add BridgedID
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_BridgeID.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(60);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DetailNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_NAME.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Tooth Number
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_TOOTH.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(65);
            oGridViewRow.Cells.Add(oTableCell);

            //Add CHILD
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_CHILD.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add GAP
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_GAP.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add prosthetic name
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_ProstheticName.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(200);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Material name
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_MaterialName.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Shape
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_Shape.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Shade
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_Shade.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //add
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void gridViewOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gridViewOrderDetail, "Select$" + e.Row.RowIndex);
        }
    }
    protected void gridViewOrderDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Update Before Value First
            SaveGridViewProcess(hiddenBeforeRowDetail.Value);
            if (this.ViewState["CurrentProcess"] != null)
            {
                this.ViewState.Remove("CurrentProcess");
            }
            gridViewOrderDetail.SelectedRow.Focus();
            //Show Process In here
            ResetOperationProcess();
            SetOperationProcess();
        }
        catch (Exception ex)
        {
            logger.Error("Error gridViewOrderDetail_SelectedIndexChanged ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }
    private void ResetOperationProcess()
    {
        //Common
        txtAmount.Text = txtMaterialPrice.Text = txtCompleteDate.Text = txtStaffCd.Text = txtManufactureStaff.Text = "";
        DropDownManufactureStaff.SelectedIndex = DropDownStaff.SelectedIndex = 0;
        //OutSource
        DropDownOutsourceCompany.SelectedIndex = 0;
        txtOutsourceLabCd.Text = DropDownOutsourceCompany.SelectedValue;
        txtOutsourceDate.Text = txtReceiveDate.Text = txtReceiveDate.Text = txtReceiveEstimateDate.Text = txtTechPrice.Text = string.Empty;
    }
    private void SaveGridViewProcess(string selectedRow)
    {
        if (!string.IsNullOrEmpty(selectedRow))
        {
            int rowIndex = Convert.ToInt32(selectedRow);
            OperationProcessInfo info = this.listDetail[rowIndex];

            #region Save Process List
            List<CustomProcess> currentList = new List<CustomProcess>();
            if (this.ViewState["CurrentProcess"] == null)
            {
                foreach (GridViewRow r in this.gridViewProcess.Rows)
                {
                    string ProcessNm = Convert.ToString(((DataBoundLiteralControl)r.Cells[1].Controls[0]).Text.Trim());
                    string StaffNm = ((DataBoundLiteralControl)r.Cells[2].Controls[0]).Text.Trim();
                    string ProcessTime = ((DataBoundLiteralControl)r.Cells[3].Controls[0]).Text.Trim();
                    string ID = ((DataBoundLiteralControl)r.Cells[4].Controls[0]).Text.Trim();
                    string ProcessCd = Common.GetRowString(r.Cells[5].Text.Trim());
                    string StaffCd = Common.GetRowString(r.Cells[6].Text.Trim());

                    logger.Debug(string.Format("first i.ProcessName ={0} , i.StaffNm = {1} , i.ProcessCd = {2}, i.StaffCd= {3} , i.ID  = {4}", ProcessNm, StaffNm, ProcessCd, StaffCd, ID));
                    if (string.IsNullOrEmpty(ProcessCd))
                    {
                        ProcessCd = "0";
                        if (!string.IsNullOrEmpty(ProcessNm))
                        {
                            //find ProcessCd from ProcessNm
                            CodeName temp = this.DropDownSource.listProcess.First(p => p.Name == ProcessNm);
                            ProcessCd = temp.Code;
                        }
                    }
                    if (!string.IsNullOrEmpty(StaffNm) && string.IsNullOrEmpty(StaffCd))
                    {
                        CodeName temp = this.DropDownSource.listStaff.First(p => p.Name == StaffNm);
                        StaffCd = temp.Code;
                    }

                    logger.Debug(string.Format("Change. i.ProcessName ={0} , i.StaffNm = {1} , i.ProcessCd = {2}, i.StaffCd= {3} , i.ID  = {4}", ProcessNm, StaffNm, ProcessCd, StaffCd, ID));
                    if (!string.IsNullOrEmpty(ProcessCd))
                        currentList.Add(new CustomProcess(Convert.ToInt32(ID), Convert.ToInt32(ProcessCd), ProcessNm, StaffCd, StaffNm, ProcessTime));
                }
            }
            else
            {
                currentList = this.ViewState["CurrentProcess"] as List<CustomProcess>;
            }
            logger.Debug("End SaveGridViewProcess currentList.count = " + currentList.Count);

            info.listCustomProcess = currentList;
            #endregion

            #region Save OutSource
            info.OutSource = GetTrnOutsource(info.OutSource);
            #endregion

            #region Save Common
            info.useOutsource = optOutsource.Checked;
            info.detailChanged = 1; // Changed
            //Save Detail
            info.detail.MaterialCd = Common.GetNullableInt(txtMaterialCd.Text);
            info.detail.MaterialNm = DropDownMaterial.SelectedItem.Text;

            info.detail.Amount = Common.GetNullableDouble (txtAmount.Text);
            info.detail.UnitCd = dropDownUnit.SelectedValue;
            info.detail.SupplierCd =Common.GetNullableInt(dropDownStock.SelectedValue);
            info.detail.MaterialPrice =Common.GetNullableDouble(txtMaterialPrice.Text); //issue 

            info.detail.CompleteDate = Common.GetNullableDateTime(txtCompleteDate.Text);
            info.detail.InspectionStaff = Common.GetNullableInt(txtStaffCd.Text);
            info.detail.ManufactureStaff = Common.GetNullableInt(txtManufactureStaff.Text);
            //Common for all bridgeID 
            if (info.detail.BridgedID!=null)
            {
                List<OperationProcessInfo> list = this.listDetail.Where(p => p.detail.BridgedID == info.detail.BridgedID && p.detail.DetailSeq != info.detail.DetailSeq).ToList();
                foreach (OperationProcessInfo i in list)
                {
                    i.detail.CompleteDate = Common.GetNullableDateTime(txtCompleteDate.Text);
                    i.detail.InspectionStaff = Common.GetNullableInt(txtStaffCd.Text);
                    i.detail.ManufactureStaff = Common.GetNullableInt(txtManufactureStaff.Text);
                    i.detailChanged = 1; //changed
                    //Set OutSource , except TechPrice
                    if (i.OutSource == null)
                        i.OutSource = new TrnOutsource();
                    i.OutSource.OutsourceCd = info.OutSource.OutsourceCd;
                    i.OutSource.OutsourceDate = info.OutSource.OutsourceDate;
                    i.OutSource.ReceiveEstimateDate = info.OutSource.ReceiveEstimateDate;
                    i.OutSource.ReceiveDate = info.OutSource.ReceiveDate;
                    //Except TechPrice
                    i.useOutsource = info.useOutsource;
                }
            }
            #endregion
        }
    }
    private void SetOperationProcess()
    {
        OperationProcessInfo info = this.listDetail[this.gridViewOrderDetail.SelectedRow.RowIndex];
        //check IssuedDetail 
        if (!string.IsNullOrEmpty(info.detail.BillStatementNo))
        {
            optInternal.Enabled = optOutsource.Enabled = panelOutsource.Enabled = panelProcess.Enabled = panelDetail.Enabled = false;
        }
        else
        {
            optInternal.Enabled = optOutsource.Enabled = panelOutsource.Enabled = panelProcess.Enabled = panelDetail.Enabled = true;
            EnableRegionProcess(!info.useOutsource);
        }
        optOutsource.Checked = info.useOutsource;
        optInternal.Checked = !info.useOutsource;

        //Set Common
        txtMaterialCd.Text = DropDownMaterial.SelectedValue = Common.GetNullableString(info.detail.MaterialCd);
        FillUnitAndStock(info);
        //dropDownUnit.SelectedValue = detail.UnitCd;
        //dropDownStock.SelectedValue = detail.SupplierCd;
        if (info.detail.Amount!=null)
            txtAmount.Text = info.detail.Amount.ToString();
        if (info.detail.MaterialPrice!=null)
            txtMaterialPrice.Text = info.detail.MaterialPrice.Value.ToString();
        if (info.detail.CompleteDate!=null)
            txtCompleteDate.Text = showOnlyDate(info.detail.CompleteDate.Value.ToString());

        txtStaffCd.Text = DropDownStaff.SelectedValue = Common.GetNullableString(info.detail.InspectionStaff);
        txtManufactureStaff.Text = DropDownManufactureStaff.SelectedValue = Common.GetNullableString(info.detail.ManufactureStaff);
        //Set Process List
        txtTotalWorkTime.Text = info.listCustomProcess.Where(p => (p.ProcessTime != null && p.ProcessTime.Trim() != "")).Sum(p => Convert.ToInt32(p.ProcessTime)).ToString();
        txtFullWorkTime.Text = this.listDetail.Where(x => x.listCustomProcess.Any()).Select(x => x.listCustomProcess.Where(y => !string.IsNullOrEmpty(y.ProcessTime)).Sum(y => Convert.ToInt32(y.ProcessTime))).Sum().ToString();
        this.BindGridView(info.listCustomProcess);

        //Set OutSource Region
        SetTrnOutsource(info.OutSource);

        hiddenBeforeRowDetail.Value = gridViewOrderDetail.SelectedRow.RowIndex.ToString();
    }
    private void SetTrnOutsource(TrnOutsource obj)
    {
        if (obj != null)
        {
            if (obj.OutsourceCd != -1)
                txtOutsourceLabCd.Text = obj.OutsourceCd.ToString();
            GetAutomaticDropDownList(txtOutsourceLabCd, DropDownOutsourceCompany);
            if (obj.OutsourceDate != null)
                txtOutsourceDate.Text = showOnlyDate(obj.OutsourceDate.Value.ToString());
            if (obj.ReceiveDate != null)
                txtReceiveDate.Text = showOnlyDate(obj.ReceiveDate.ToString());
            if (obj.ReceiveEstimateDate != null)
                txtReceiveEstimateDate.Text = showOnlyDate(obj.ReceiveEstimateDate.ToString());
            if (obj.TechPrice != null)
                txtTechPrice.Text = obj.TechPrice.Value.ToString();
        }
    }
    private TrnOutsource GetTrnOutsource(TrnOutsource obj)
    {
        if (obj == null)
            obj = new TrnOutsource();
        if (txtOutsourceLabCd.Text != "")
            obj.OutsourceCd = Convert.ToInt32(DropDownOutsourceCompany.SelectedValue);
        else
            obj.OutsourceCd = -1;
        obj.OutsourceDate = Common.GetNullableDateTime(txtOutsourceDate.Text);
        obj.ReceiveEstimateDate = Common.GetNullableDateTime(txtReceiveEstimateDate.Text);

        obj.ReceiveDate = Common.GetNullableDateTime(txtReceiveDate.Text);
        obj.TechPrice = Common.GetNullableDouble (txtTechPrice.Text); 
        return obj;
    }

   
    #endregion

    #region PROCESS
    protected void gridViewProcess_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add Action
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Type
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_ProcessNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add SalesMan
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_SalesMan.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add WorkTime
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_PROCESS_WorkTime.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(90);
            oGridViewRow.Cells.Add(oTableCell);
            //add
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void gridViewProcess_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            // e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = "javascript:ClickRow(this);";
        }
        if (e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dropDownProcess = (DropDownList)e.Row.FindControl("DropDownProcess");

            if (dropDownProcess != null)
            {
                if (DropDownSource.listProcess.Count == 0)
                {
                    buttonAddRow.Enabled = false;
                    dropDownProcess.Items.Add(new ListItem("", ""));
                }
                else
                {
                    if (optInternal.Checked && optInternal.Enabled)
                        buttonAddRow.Enabled = true;
                    FillDropDownList(dropDownProcess, false, DropDownSource.listProcess);
                }
            }
            DropDownList dropDownStaff = (DropDownList)e.Row.FindControl("DropDownStaff");
            if (dropDownStaff != null)
            {
                FillDropDownList(dropDownStaff, true, DropDownSource.listStaff);
            }
            //Only Case Footer Set Default
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtProcess = (TextBox)e.Row.FindControl("txtProcess");
                txtProcess.Text = dropDownProcess.Items[0].Value;
            }
            else //case Row 
            {
                TextBox txtProcess = (TextBox)e.Row.FindControl("txtProcess");
                TextBox txtStaff = (TextBox)e.Row.FindControl("txtStaff");
                if (txtProcess != null && txtStaff != null)
                {
                    GetAutomaticDropDownList(txtProcess, dropDownProcess);
                    GetAutomaticDropDownList(txtStaff, dropDownStaff);
                }
            }

        }
    }
    protected void dropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropDown = (DropDownList)sender;
        GridViewRow gridrow = (GridViewRow)dropDown.NamingContainer;

        if (dropDown.ID == "DropDownStaff")
        {
            TextBox txtStaff = (TextBox)gridrow.FindControl("txtStaff");
            if (txtStaff != null)
                txtStaff.Text = dropDown.SelectedItem.Value;
        }
        else
        {
            TextBox txtProcess = (TextBox)gridrow.FindControl("txtProcess");
            if (txtProcess != null)
                txtProcess.Text = dropDown.SelectedItem.Value;
        }
    }
    protected void textBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        try
        {
            GridViewRow gridrow = (GridViewRow)textBox.NamingContainer;
            if (textBox.ID == "txtProcess")
            {
                DropDownList dropDownProcess = (DropDownList)gridrow.FindControl("DropDownProcess");
                GetAutomaticDropDownList(textBox, dropDownProcess);
            }
            else
            {
                DropDownList dropDownStaff = (DropDownList)gridrow.FindControl("DropDownStaff");
                GetAutomaticDropDownList(textBox, dropDownStaff);
                TextBox txtProcessTime = (TextBox)gridrow.FindControl("txtProcessTime");
                txtProcessTime.Focus();
            }
        }
        catch 
        {
            textBox.Text = "";
            textBox.Focus();
            //RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void gridViewProcess_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridViewProcess.EditIndex = e.NewEditIndex;
        this.BindGridView();
    }

    protected void gridViewProcess_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridViewProcess.EditIndex = -1;
        this.BindGridView();
    }

    protected void gridViewProcess_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = this.gridViewProcess.Rows[e.RowIndex];
            int ID = Convert.ToInt32(((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text);

            TextBox txtProcess = row.FindControl("txtProcess") as TextBox;
            TextBox txtStaff = row.FindControl("txtStaff") as TextBox;
            TextBox txtProcessTime = row.FindControl("txtProcessTime") as TextBox;
            DropDownList dropdownProcess = row.FindControl("DropDownProcess") as DropDownList;
            DropDownList dropdownStaff = row.FindControl("DropDownStaff") as DropDownList;

            if ((txtProcess != null) && (txtStaff != null) && (txtProcessTime != null))
            {
                CustomProcess customer = this.CustomProcess.Find(c => c.ID == ID);

                customer.ProcessCd = Convert.ToInt32(txtProcess.Text.Trim());
                customer.ProcessName = dropdownProcess.SelectedItem.Text;
                if (customer.ProcessName == "")
                    throw new Exception(GetResource("MSG_INPUT_PROCESS_CODE_NOT_VALID.Text"));

                customer.StaffCd = txtStaff.Text.Trim();
                customer.StaffNm = dropdownStaff.SelectedItem.Text;

                int oldWorkTime = (customer.ProcessTime == string.Empty) ? 0 : Convert.ToInt32(customer.ProcessTime);
                int newWorktime = (txtProcessTime.Text.Trim() == string.Empty) ? 0 : Convert.ToInt32(txtProcessTime.Text.Trim());

                int currentTotalWT = txtTotalWorkTime.Text == "" ? 0 : Convert.ToInt32(txtTotalWorkTime.Text);
                txtTotalWorkTime.Text = (currentTotalWT - oldWorkTime + newWorktime).ToString();
                txtFullWorkTime.Text = ((txtFullWorkTime.Text == "") ? 0 : Convert.ToInt32(txtFullWorkTime.Text) - oldWorkTime + newWorktime).ToString();

                customer.ProcessTime = txtProcessTime.Text.Trim();

                this.gridViewProcess.EditIndex = -1;
                this.BindGridView();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error gridViewProcess_RowUpdating ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void gridViewProcess_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = gridViewProcess.Rows[e.RowIndex];
        int ID = Convert.ToInt32(((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text);

        CustomProcess customer = this.CustomProcess.Find(c => c.ID == ID);
        this.CustomProcess.Remove(customer);

        this.BindGridView();
    }

    #endregion

    #region ADD ,DELETE , UP , DOWN
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = this.gridViewProcess.FooterRow;
            TextBox txtProcess = row.FindControl("txtProcess") as TextBox;
            TextBox txtStaff = row.FindControl("txtStaff") as TextBox;
            TextBox txtProcessTime = row.FindControl("txtProcessTime") as TextBox;
            DropDownList dropdownProcess = row.FindControl("DropDownProcess") as DropDownList;
            DropDownList dropdownStaff = row.FindControl("DropDownStaff") as DropDownList;
            if ((txtProcess != null) && (txtStaff != null) && (txtProcessTime != null))
            {
                if (string.IsNullOrEmpty(txtProcess.Text))
                    throw new Exception(GetResource("MSG_INPUT_PROCESS_REQUIRED.Text"));

                if (!string.IsNullOrEmpty(txtProcessTime.Text))
                {
                    int currentTotalWT = txtTotalWorkTime.Text == "" ? 0 : Convert.ToInt32(txtTotalWorkTime.Text);
                    txtTotalWorkTime.Text = (currentTotalWT + Convert.ToInt32(txtProcessTime.Text)).ToString();
                    txtFullWorkTime.Text = ((txtFullWorkTime.Text == "") ? 0 : Convert.ToInt32(txtFullWorkTime.Text) + Convert.ToInt32(txtProcessTime.Text)).ToString();
                }

                CustomProcess customer = new CustomProcess
                {
                    ID = this.CustomProcess.Max(c => c.ID) + 1,
                    ProcessName = dropdownProcess.SelectedItem.Text,
                    StaffNm = dropdownStaff.SelectedItem.Text,
                    ProcessTime = txtProcessTime.Text.Trim(),
                    ProcessCd = Convert.ToInt32(txtProcess.Text),
                    StaffCd = txtStaff.Text
                };
                this.CustomProcess.Add(customer);
                this.BindGridView();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error btnAdd_Click ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        UpDownDataRow(true);
    }
    protected void btnDown_Click(object sender, EventArgs e)
    {
        UpDownDataRow(false);
    }
    private void UpDownDataRow(bool up)
    {
        try
        {
            int rowIndex = Convert.ToInt32(hiddenSelectedRowIndex.Value);

            GridViewRow row = (GridViewRow)this.gridViewProcess.Rows[rowIndex];
            var rows = this.gridViewProcess.Rows.Cast<GridViewRow>().Where(a => a != row).ToList();
            if (up)
            {
                if (row.RowIndex.Equals(1))
                {
                    return;
                    //   rows.Add(row);
                }
                else
                    rows.Insert(row.RowIndex - 1, row);
            }
            else
            {
                if (row.RowIndex.Equals(this.gridViewProcess.Rows.Count - 1))
                {
                    return;
                    //   rows.Insert(0, row);
                }
                else
                    rows.Insert(row.RowIndex + 1, row);
            }
            this.gridViewProcess.DataSource = rows.Select(a => new
            {
                ID = ((DataBoundLiteralControl)a.Cells[4].Controls[0]).Text,
                ProcessName = ((DataBoundLiteralControl)a.Cells[1].Controls[0]).Text,
                StaffNm = ((DataBoundLiteralControl)a.Cells[2].Controls[0]).Text,
                ProcessTime = ((DataBoundLiteralControl)a.Cells[3].Controls[0]).Text,
                ProcessCd = a.Cells[5].Text,
                StaffCd = a.Cells[6].Text
            }).ToList();


            this.gridViewProcess.DataBind();
            if (Convert.ToInt32(((DataBoundLiteralControl)this.gridViewProcess.Rows[0].Cells[4].Controls[0]).Text) == 0)
            {
                this.gridViewProcess.Rows[0].Visible = false;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error UpDownDataRow ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = Convert.ToInt32(hiddenSelectedRowIndex.Value);

            GridViewRow row = (GridViewRow)this.gridViewProcess.Rows[rowIndex];
            string processTime = (((DataBoundLiteralControl)row.Cells[3].Controls[0]).Text.Trim());

            this.gridViewProcess.DeleteRow(rowIndex);

            txtTotalWorkTime.Text = ((txtTotalWorkTime.Text == "" ? 0 : Convert.ToInt32(txtTotalWorkTime.Text)) - (processTime == "" ? 0 : Convert.ToDouble(processTime))).ToString();
            txtFullWorkTime.Text = ((txtFullWorkTime.Text == "" ? 0 : Convert.ToInt32(txtFullWorkTime.Text)) - (processTime == "" ? 0 : Convert.ToDouble(processTime))).ToString();

            List<CustomProcess> list = (List<CustomProcess>)this.gridViewProcess.DataSource;
            BindGridView(list);


        }
        catch (Exception ex)
        {
            logger.Error("Error btnDelete_Click ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }
    #endregion

    #region COMMON
    private void EnableRegionProcess(bool bProcess)
    {
        panelProcess.Enabled = bProcess;
        panelOutsource.Enabled = !bProcess;
    }
    private void FillDropDownList(DropDownList dropDown, bool bFirstEmptyItem, List<CodeName> listCodeName)
    {
        dropDown.Items.Clear();
        if (bFirstEmptyItem)
            dropDown.Items.Add(new ListItem("", ""));

        foreach (CodeName i in listCodeName)
        {
            dropDown.Items.Add(new ListItem(i.Name, i.Code));
        }
    }
    private void FillDropDownListUnit(List<MasterUnit> listUnit)
    {
        dropDownUnit.Items.Clear();
        dropDownUnit.Items.Add(new ListItem("", ""));
        foreach (MasterUnit i in listUnit)
        {
            dropDownUnit.Items.Add(new ListItem(i.UnitNm, i.UnitCd));
        }
    }
    private void GetSelectedDropDownListValue(DropDownList dropDownList, string value)
    {
        try
        {
            if (value == null) value = "0";
            dropDownList.SelectedValue = value;
        }
        catch { }
    }
    #endregion

    #region Register Process
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            logger.Info("Begin btnRegister_Click");
            if (gridViewOrderDetail.SelectedRow == null)
                throw new Exception(GetResource("MSG_NOT_CHOOSE_BRIDGE_GROUP.Text"));

            SaveGridViewProcess(gridViewOrderDetail.SelectedRow.RowIndex.ToString());
            
            OperationProcessInfo.RegisterOperationProcess(this.listDetail, Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value), this.User.Identity.Name);

            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS_Full.Text")) + "\");");
            //RemoveViewState("CurrentProcess", "OperationProcess", "DropDownSource");
        }
        catch (Exception ex)
        {
            logger.Error("Error btnRegister_Click ", ex);
            btnRegister.Enabled = true;
            this.btnRegister.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString());
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentProcess", "OperationProcess", "DropDownSource");
        Response.Redirect("OrderList.aspx");
    }
    protected void btnGoTechPrice_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentProcess", "OperationProcess", "listStaff");
        Response.Redirect("OperationTechPrice.aspx?id=" + hiddenOrderSeq.Value);
    }

    protected void btnGoOrder_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentProcess", "OperationProcess", "listStaff");
        Response.Redirect("OrderInput.aspx?id=" + hiddenOrderSeq.Value);
    }
    protected void optInternal_CheckedChanged(object sender, EventArgs e)
    {
        EnableRegionProcess(true);
    }

    protected void optOutsource_CheckedChanged(object sender, EventArgs e)
    {
        EnableRegionProcess(false);
    }

    #endregion

    #region CalculateMaterialPrice
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        CalculateMaterialPrice();
    }
    protected void dropDownUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateMaterialPrice();
    }

    protected void dropDownStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateMaterialPrice();
    }
    #endregion

    #region OutsourceLab Changed
    protected void OutsourceLabCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtOutsourceLabCd, DropDownOutsourceCompany);
    }
    protected void DropDownOutsourceCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtOutsourceLabCd.Text = DropDownOutsourceCompany.SelectedItem.Value;
        //CalculateMaterialPrice();
    }
    #endregion

    protected void txtStaffCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtStaffCd, DropDownStaff);
    }
    protected void DropDownStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtStaffCd.Text = DropDownStaff.SelectedItem.Value;
    }
    protected void txtManufactureStaff_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtManufactureStaff, DropDownManufactureStaff);
    }
    protected void DropDownManufactureStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtManufactureStaff.Text = DropDownManufactureStaff.SelectedItem.Value;
    }
    protected void txtMaterialCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtMaterialCd, DropDownMaterial);
        gridViewOrderDetail.Rows[this.gridViewOrderDetail.SelectedRow.RowIndex].Cells[7].Text = DropDownMaterial.SelectedItem.Text;
    }
    protected void DropDownMaterial_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMaterialCd.Text = DropDownMaterial.SelectedItem.Value;
        gridViewOrderDetail.Rows[this.gridViewOrderDetail.SelectedRow.RowIndex].Cells[7].Text = DropDownMaterial.SelectedItem.Text;
        FillUnitAndStock(this.listDetail[this.gridViewOrderDetail.SelectedRow.RowIndex]);
    }
    private void FillUnitAndStock(OperationProcessInfo info) //using old detail
    {
        logger.Debug("Begin FillUnitAndStock detail.materialCd = " + txtMaterialCd.Text);
        List<MasterUnit> listUnit = MasterUnit.GetListUnitByMaterialCd(Convert.ToInt32(hiddenOfficeCd.Value), Common.GetNullableInt(txtMaterialCd.Text), Convert.ToDateTime(txtOrderDate.Text));
        FillDropDownListUnit(listUnit);
        MasterUnit currentUnit = null;
        if (listUnit != null)
            currentUnit = listUnit.FirstOrDefault(p => p.UnitCd == info.detail.UnitCd);
        if (currentUnit != null)
            dropDownUnit.SelectedValue = currentUnit.UnitCd;
        else
            dropDownUnit.SelectedIndex = 0;
        //set listStock 
        List<CodeName> listStock = CodeName.GetMasterStockListCodeNamePrice(Convert.ToInt32(hiddenOfficeCd.Value), txtMaterialCd.Text, Convert.ToDateTime(txtOrderDate.Text));
        listStock.RemoveAll(p => p.Code != "0" && p.Code != hiddenSupplierCd.Value);
        listStock = listStock.GroupBy(p => p.Code).Select(p => p.First()).ToList();

        FillDropDownList(dropDownStock, true, listStock);

        CodeName currentStock = null;
        if (listStock != null)
            currentStock = listStock.Where(p => p.Code == Common.GetNullableString(info.detail.SupplierCd)).FirstOrDefault();
        if (currentStock != null)
            dropDownStock.SelectedValue = currentStock.Code;
        else
            dropDownStock.SelectedIndex = 0;

        CalculateMaterialPrice(info, currentUnit, currentStock);
    }
    private void CalculateMaterialPrice() //using current 
    {
        List<MasterUnit> listUnit = MasterUnit.GetListUnitByMaterialCd(Convert.ToInt32(hiddenOfficeCd.Value), Common.GetNullableInt(txtMaterialCd.Text), Convert.ToDateTime(txtOrderDate.Text));
        List<CodeName> listStock = CodeName.GetMasterStockListCodeNamePrice(Convert.ToInt32(hiddenOfficeCd.Value), txtMaterialCd.Text, Convert.ToDateTime(txtOrderDate.Text));

        //Save internal , Outsource 
        OperationProcessInfo detail = this.listDetail[gridViewOrderDetail.SelectedIndex];
        MasterUnit currentUnit = null;
        if (listUnit != null)
            currentUnit = listUnit.FirstOrDefault(p => p.UnitCd == dropDownUnit.SelectedValue);
        CodeName currentStock = null;
        logger.DebugFormat("listStock.count = {0}", listStock.Count);
        if (listStock != null)
            currentStock = listStock.Where(p => p.Code == dropDownStock.SelectedValue).FirstOrDefault();
        CalculateMaterialPrice(detail, currentUnit, currentStock);
    }
    private void CalculateMaterialPrice(OperationProcessInfo info, MasterUnit currentUnit, CodeName currentStock)
    {
        try
        {
            double materialPrice = 0;
            logger.DebugFormat("Begin CalculateMaterialPrice txtAmount.Text = {0} , MaterialCd = {1}", txtAmount.Text, txtMaterialCd.Text);
            int outSourceLabCd = 0;
            info.detail.Price = null;
            if (txtAmount.Text != "" && txtMaterialCd.Text != "" && currentUnit != null && currentStock != null)
            {
                logger.DebugFormat("Case Calculate GetAmountInMstStock outSourceLabCd = {0} ,Stock = {1} , Unit = {2}", outSourceLabCd, currentStock.Code, currentUnit.UnitCd);

                double? amountInStock = MasterStock.GetAmountInMstStock(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToInt32(txtMaterialCd.Text), Convert.ToInt32(dropDownStock.SelectedValue), outSourceLabCd, dropDownUnit.SelectedValue);
                if (amountInStock == null)
                {
                    txtAmount.Text = txtMaterialPrice.Text = "";
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_NO_STOCK_TO_USE.Text")) + "\");");
                    return;
                }
                if (Convert.ToDouble(txtAmount.Text) > amountInStock +   (info.detail.Amount == null ? 0 : Convert.ToDouble(info.detail.Amount)))
                {
                    txtAmount.Text = txtMaterialPrice.Text = "";
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_NOT_ENOUGH_STOCK.Text")) + "\");");
                    return;
                }
                info.detail.Price = Common.GetNullableDouble(currentStock.HiddenValue);
                if (currentUnit.AmountByMinimumUnit!=null && info.detail.Price!=null)
                {
                    logger.DebugFormat("Get MaterialPrice txtAmount.Text = {0} , Unit = {1} , Price = {2} ", txtAmount.Text, currentUnit.AmountByMinimumUnit, info.detail.Price);
                    materialPrice = Convert.ToDouble(txtAmount.Text) * Convert.ToDouble(currentUnit.AmountByMinimumUnit) * Convert.ToDouble(info.detail.Price);
                    logger.Debug("materialPrice = " + materialPrice);
                }
                if (materialPrice == 0)
                    txtMaterialPrice.Text = "";
                else
                {
                    txtMaterialPrice.Text = materialPrice.ToString();
                }
            }
            else
            {
                txtAmount.Text = txtMaterialPrice.Text = "";
            }
            logger.Debug("End CalculateMaterialPrice");
        }
        catch (Exception ex)
        {
            txtAmount.Text = txtMaterialPrice.Text = "";
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }
    protected string ShowCheckedStringValue(string bCheck)
    {
        if (bCheck == "True")
        {
            return GetResource("CHECKED_STRING.Text");
        }
        else
            return "";
    }
}