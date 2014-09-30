using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Collections;
using Dental.Domain;
using Microsoft.Reporting.WebForms;
using Dental.Utilities;
using System.IO;
using Ionic.Zip;
using System.Text;
using System.Reflection;
using iTextSharp.text.pdf;

public partial class BillStatement : DDVPortalModuleBase
{
    readonly static ILog logger = LogManager.GetLogger(typeof(BillStatement));

    #region ViewState
    private void RemoveViewState()
    {
        if (ViewState["HashCheckArray"] != null)
        {
            ViewState.Remove("HashCheckArray");
        }
    }
    public Hashtable HashCheckArray
    {
        get
        {
            if (this.ViewState["HashCheckArray"] == null)
            {
                return new Hashtable();
            }
            return this.ViewState["HashCheckArray"] as Hashtable;
        }
    }
    private void AddHashCheckArray(Hashtable hash)
    {
        if (ViewState["HashCheckArray"] == null)
        {
            ViewState.Add("HashCheckArray", hash);
        }
        else
        {
            ViewState["HashCheckArray"] = hash;
        }
    }
    private void SaveCheckBox()
    {
        Hashtable hash = this.HashCheckArray;
        int beginItem = gvOrderList.PageIndex * gvOrderList.PageSize;
        foreach (GridViewRow msgRow in gvOrderList.Rows)
        {
            CheckBox ck = (CheckBox)msgRow.FindControl("Check");
            if (ck.Checked == true)
            {
                if (!hash.Contains(beginItem + msgRow.RowIndex))
                {
                    hash.Add((beginItem + msgRow.RowIndex), GetBillStamentInfo(msgRow));
                }
            }
            else
            {
                if (hash.Contains(beginItem + msgRow.RowIndex))
                {
                    hash.Remove((beginItem + msgRow.RowIndex));
                }
            }
        }
        AddHashCheckArray(hash);
    }
    #endregion

    #region "Event Handlers"
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            hplFromDate.NavigateUrl = Calendar.InvokePopupCal(txtFromDate);
            hplToDate.NavigateUrl = Calendar.InvokePopupCal(txtToDate);
            hplDeliveryFromDate.NavigateUrl = Calendar.InvokePopupCal(txtDeliveryFromDate);
            hplDeliveryToDate.NavigateUrl = Calendar.InvokePopupCal(txtDeliveryToDate);
            hplIssueDate.NavigateUrl = Calendar.InvokePopupCal(txtIssueDate);
            if (!IsPostBack)
            {
                InitLanguage();

                hiddenRoundSystem.Value = MasterSystem.GetSystemMaster("PriceRound").Value;
                RemoveViewState();
                hiddenIsMultiBillCd.Value = "";
                AddEnablePrintJS();
                hiddenOfficeCd.Value = GetOffice();
                SetLabelText(lbOrderNo, lbOrderDate, lbDeliveryDate, lbClinicName, lbDeliveryType, lbInvoiceNo, lbYearMonth, lbAdjustedFee, lbBillingAmount,
                   lbComment, lbInvoiceIssueDate, lbInvoiceRecipient, lbTotal, lbType, lblUnInsured, lblInsured, lbRowPerPage, lbSum);
                SetButtonText(btnSearch, btnClear, btnPrint, btnBack);

                rbNewIssue.Text = GetResource("rbNewIssue.Text");
                rbReissueDelete.Text = GetResource("rbReissueDelete.Text");
                rbReissue.Text = GetResource("rbReissue.Text");
                rbDelete.Text = GetResource("rbDelete.Text");
                cbUnInsured.Text = GetResource("cbUnInsured.Text");
                cbInsured.Text = GetResource("cbInsured.Text");
                cbSearch.Text = GetResource("cbSearch.Text");
                gvOrderList.EmptyDataText = GetResource("NoRecordFound.Text");

                RequiredIssueDate.ErrorMessage = RequiredIssueDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), lbInvoiceIssueDate.Text);
                RequiredDateIssueDate.ErrorMessage = RequiredIssueDate.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_DATE_FIELD.Text"), lbInvoiceIssueDate.Text);
                RequiredInvoiceNo.ErrorMessage = RequiredInvoiceNo.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_FIELD.Text"), lbInvoiceNo.Text);
                RequiredAdjustedFee.ErrorMessage = RequiredAdjustedFee.ToolTip = string.Format(GetResource("MSG_REQUIRED_INPUT_NUMBER_FIELD.Text"), lbAdjustedFee.Text);

                //Fill data to dropdownlist 
                FillDentalOffice();
                FillBill();
                FillYear();
                FillMonth();
                //Create List empty
                BindGridView(new List<BillStatementInfo>());
                lbMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");", true);
        }
    }
    private void AddEnablePrintJS()
    {
        this.btnPrint.Enabled = true;
        this.btnPrint.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()) { return ConfirmIssue('" + GetResource("MSG_CONFIRM_ISSUE.Text") + "','');}");
        // this.btnPrint.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()) { if(ConfirmIssue('" + GetResource("MSG_CONFIRM_ISSUE.Text") + "')){this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnPrint, "").ToString() + ";return true;}else{this.disabled=false;return false;}}");
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            logger.Info("Begin btnPrint_Click");
            if (rbNewIssue.Checked)
                NewIssueBillStatement(int.Parse(hiddenOfficeCd.Value), dlYear.SelectedItem.Text, dlmonth.SelectedItem.Text, Convert.ToDateTime(txtIssueDate.Text));
            else if (rbReissueDelete.Checked)
                ReissueDelete(int.Parse(hiddenOfficeCd.Value), dlYear.SelectedItem.Text, dlmonth.SelectedItem.Text);
            logger.Info("End btnPrint_Click");
        }
        catch (Exception ex)
        {
            AddEnablePrintJS();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");", true);

            if (ex.Message.Contains(txtInvoiceNo.Text) || ex.Message.Contains(GetResource("MSG_CANT_DELETE_BILLSTATEMENT.Text")))
                logger.Warn("Warn btnPrint_Click.Message = " + ex.Message);
            else
                logger.Error("Error in btnPrint_Click ", ex);
        }
    }

    #region FillDropDown List
    private void FillDentalOffice()
    {

        List<MasterDentalOffice> listDentalOfficeMaster = MasterDentalOffice.GetDentalOfficeMasters(int.Parse(hiddenOfficeCd.Value));
        dropDownDentalOffice.Items.Add(new ListItem("", ""));
        foreach (MasterDentalOffice i in listDentalOfficeMaster)
        {
            dropDownDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }
    }
    private void FillBill()
    {
        dropDownBill.Items.Add(new ListItem("", ""));

        List<MasterBill> billList = MasterBill.GetBillMasters(int.Parse(hiddenOfficeCd.Value));
        foreach (MasterBill i in billList)
        {
            dropDownBill.Items.Add(new ListItem(i.BillNm, i.BillCd.ToString()));
        }
    }
    private void FillYear()
    {
        for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 5; i--)
        {
            dlYear.Items.Add(i.ToString());
        }
    }
    private void FillMonth()
    {
        int month = DateTime.Now.Month;
        for (int i = 1; i <= 12; i++)
        {
            dlmonth.Items.Add(i.ToString());
        }
        dlmonth.SelectedValue = month.ToString();
    }

    #endregion

    #region "Textchanged and DropdownlistChanged"

    protected void txtDentalOfficeCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtDentalOfficeCd, dropDownDentalOffice);
        dropDownDentalOffice_SelectedIndexChanged(sender, e);
    }
    protected void dropDownDentalOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtDentalOfficeCd.Text = dropDownDentalOffice.SelectedValue;
    }
    protected void txtInvoiceRecipient_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtInvoiceRecipient, dropDownBill);
        dropDownBill_SelectedIndexChanged(sender, e);
    }
    protected void dropDownBill_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtInvoiceRecipient.Text = dropDownBill.SelectedValue;
    }

    #endregion

    #endregion

    #region Search
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hiddenInsured.Value = hiddenUninsured.Value = hiddenIsMultiBillCd.Value = hiddenChooseBillCd.Value = "";
        hiddenRoundSystem.Value = MasterSystem.GetSystemMaster("PriceRound").Value;
        RemoveViewState();
        ResetInputNewIssue();
        //Get module listing

        List<BillStatementInfo> listBillStatmentInfo = GetListFilterByClosingDay(BillStatementInfo.GetBillStatements(GetObjectSearchInfo()));
        logger.Debug("btnSearch_Click listBillStatmentInfo.COUNT = " + listBillStatmentInfo.Count);

        BindGridView(listBillStatmentInfo);
        bool isMultiBillCd = (listBillStatmentInfo.GroupBy(p => p.BillCd)).ToList().Count > 1;
        if (rbNewIssue.Checked)
        {
            txtAdjustedFeeInsured.Enabled = true;
            if (isMultiBillCd)
            {
                txtAdjustedFeeInsured.Enabled = false;
                txtAdjustedFeeInsured.Text = "0";
                hiddenIsMultiBillCd.Value = "True";
                ShowWarningChooseMultiBill();
            }
        }
        dlYear.Enabled = dlmonth.Enabled = panelConditions.Enabled = false;
        btnPrint.Enabled = true;
    }
    private void BindGridView(List<BillStatementInfo> listBillStatmentInfo)
    {
        if (listBillStatmentInfo.Count == 0)
        {
            //Create List empty
            BillStatementInfo objOL = new BillStatementInfo();
            objOL.OrderSeq = -1;
            objOL.OrderNo = "";
            objOL.OrderDate = DateTime.Today;
            objOL.DeliveredDate = DateTime.Today;
            listBillStatmentInfo.Add(objOL);
            gvOrderList.DataSource = listBillStatmentInfo;
            gvOrderList.DataBind();
            gvOrderList.Columns[1].Visible = false;
            if (listBillStatmentInfo.Count == 1 && listBillStatmentInfo[0].OrderSeq == -1)
            {
                gvOrderList.Rows[0].Visible = false;
            }
            lbMessage.Text = GetResource("NoRecordFound.Text");
            return;
        }
        gvOrderList.Columns[1].Visible = gvOrderList.Columns[12].Visible = gvOrderList.Columns[13].Visible = gvOrderList.Columns[14].Visible = gvOrderList.Columns[15].Visible = gvOrderList.Columns[16].Visible = gvOrderList.Columns[17].Visible = gvOrderList.Columns[18].Visible = true;
        gvOrderList.DataSource = listBillStatmentInfo;
        gvOrderList.DataBind();
        lbMessage.Text = string.Format(GetResource("TitleResults.Text"), listBillStatmentInfo.Count); // lbMessage.Text = string.Empty;
        gvOrderList.Columns[1].Visible = gvOrderList.Columns[12].Visible = gvOrderList.Columns[13].Visible = gvOrderList.Columns[14].Visible = gvOrderList.Columns[15].Visible = gvOrderList.Columns[16].Visible = gvOrderList.Columns[17].Visible = gvOrderList.Columns[18].Visible = false;
        if (this.HashCheckArray != null)
        {
            int beginItem = gvOrderList.PageIndex * gvOrderList.PageSize;
            foreach (GridViewRow msgRow in gvOrderList.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                if (this.HashCheckArray.Contains(beginItem + msgRow.RowIndex))
                    ck.Checked = true;
            }
        }
    }
    private void ShowWarningChooseMultiBill()
    {
        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_WARN_MULTI_BILL_CD.Text")) + "\");");
    }
    private BillSearchInfo GetObjectSearchInfo()
    {

        BillSearchInfo objSearch = new BillSearchInfo();
        objSearch.OfficeCd = int.Parse(hiddenOfficeCd.Value);
        if (rbNewIssue.Checked)
        {
            objSearch.OrderNo = txtOrderNo.Text;
            objSearch.OrderDateFrom = txtFromDate.Text;
            objSearch.OrderDateTo = txtToDate.Text;
            objSearch.DeliverDateFrom = txtDeliveryFromDate.Text;
            objSearch.DeliverDateTo = txtDeliveryToDate.Text;
            if (!string.IsNullOrWhiteSpace(txtDentalOfficeCd.Text))
                objSearch.DentalOfficeCd = txtDentalOfficeCd.Text;
            if (!string.IsNullOrWhiteSpace(txtInvoiceRecipient.Text))
                objSearch.BillCd = txtInvoiceRecipient.Text;
            objSearch.Insured = cbInsured.Checked;
            objSearch.UnInsured = cbUnInsured.Checked;
            objSearch.BillYear = dlYear.SelectedValue;
            objSearch.BillMonth = dlmonth.SelectedValue;
        }
        else
        {
            objSearch.OrderDateFrom = objSearch.OrderDateTo = objSearch.DeliverDateFrom = objSearch.DeliverDateTo = objSearch.DentalOfficeCd = objSearch.BillCd = objSearch.BillYear = objSearch.BillMonth = null;
            objSearch.UnInsured = objSearch.Insured = true;
            objSearch.BillStatementNo = txtInvoiceNo.Text;
        }
        return objSearch;
    }
    private void ResetInputNewIssue()
    {
        txtBillingAmountInsured.Text = txtBillingAmountUnInsured.Text = txtSum.Text = txtAdjustedFeeInsured.Text = txtBillingTotalAmount.Text = "";
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        RemoveViewState();
        panelConditions.Enabled = dlmonth.Enabled = dlYear.Enabled = true;
        BindGridView(new List<BillStatementInfo>());
        ResetInputNewIssue();
        lbMessage.Text = "";
        btnPrint.Enabled = false;
    }
    private void ResetCondition()
    {
        txtOrderNo.Text = txtFromDate.Text = txtToDate.Text = txtDeliveryFromDate.Text = txtDeliveryToDate.Text = txtDentalOfficeCd.Text = txtInvoiceRecipient.Text = string.Empty;
        dropDownDentalOffice.SelectedIndex = dropDownBill.SelectedIndex = 0;
        cbInsured.Checked = cbUnInsured.Checked = true;
        dlmonth.SelectedValue = DateTime.Now.Month.ToString();
        dlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected string GetDentalOfficeName(string code)
    {
        ListItem item = dropDownDentalOffice.Items.FindByValue(code);
        if (item != null)
            return item.Text;
        return string.Empty;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        panelReport.Visible = false;
        panelBillStatement.Visible = true;

        RemoveViewState();
        rbNewIssue.Checked = rbReissue.Checked = true;
        rbReissueDelete.Checked = rbDelete.Checked = cbSearch.Checked = false;
        EnableInputRegion(true);
        txtIssueDate.Text = txtComment.Text = txtInvoiceNo.Text = txtAdjustedFeeInsured.Text = "";
        ResetCondition();
        if (gvOrderList.Rows.Count > 1)
        {
            BindGridView(new List<BillStatementInfo>());
            lbMessage.Text = "";
        }
    }
    protected void dlNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvOrderList.PageSize = int.Parse(dlNumber.SelectedValue);

        if (btnPrint.Enabled)
        {
            RemoveViewState();
            ResetInputNewIssue();

            List<BillStatementInfo> listBillStatmentInfo = GetListFilterByClosingDay(BillStatementInfo.GetBillStatements(GetObjectSearchInfo()));
            logger.Debug("listBillStatmentInfo.COUNT = " + listBillStatmentInfo.Count);
            BindGridView(listBillStatmentInfo);
        }
    }
    protected void rbNewIssue_CheckedChanged(object sender, EventArgs e)
    {
        EnableInputRegion(rbNewIssue.Checked);
    }
    private void EnableInputRegion(bool enable)
    {
        cbSearch.Enabled = gvOrderList.Enabled = panelInput.Enabled = RequiredAdjustedFee.Enabled = RequiredDateIssueDate.Enabled = RequiredIssueDate.Enabled = dlYear.Enabled = dlmonth.Enabled = enable;
        panelReIssue.Enabled = RequiredInvoiceNo.Enabled = !enable;
        if (!enable)
        {
            panelSearch.Enabled = panelConditions.Enabled = false;
            RemoveViewState();
            btnPrint.Enabled = true;
            BindGridView(new List<BillStatementInfo>());
            lbMessage.Text = "";
        }
        else
        {
            btnPrint.Enabled = !cbSearch.Checked;
            panelSearch.Enabled = txtAdjustedFeeInsured.Enabled = panelConditions.Enabled = cbSearch.Checked;
        }
    }
    protected void cbSearch_CheckedChanged(object sender, EventArgs e)
    {
        panelSearch.Enabled = txtAdjustedFeeInsured.Enabled = panelConditions.Enabled = cbSearch.Checked;
        btnPrint.Enabled = !cbSearch.Checked;
        if (panelSearch.Enabled == false)
        {
            RemoveViewState();
            ResetCondition();
            BindGridView(new List<BillStatementInfo>());
            lbMessage.Text = "";
            dlYear.Enabled = dlmonth.Enabled = true;
            txtAdjustedFeeInsured.Text = "";
        }
    }

    #endregion

    #region GridView
    protected void gvOrderList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell; //= new TableHeaderCell();

            #region "Make Headers"

            TableHeaderCell tcCheckBox = new TableHeaderCell();
            CheckBox chkCheckAll = new CheckBox();
            chkCheckAll.ID = "chkCheckAll";
            chkCheckAll.Attributes["onclick"] = "javascript:checkAll(this);";
            tcCheckBox.Controls.Add(chkCheckAll);
            tcCheckBox.CssClass = "HeadwithBG";
            oGridViewRow.Cells.Add(tcCheckBox);

            //Add OrderNo
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOrderNo.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(120);
            oGridViewRow.Cells.Add(oTableCell);

            //Add OrderDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOrderDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(65);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliverDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDeliveryDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(65);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentalOfficeCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbClinicName.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(110);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PatientLastNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbPatientNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(120);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DetailNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDetailNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProsthesisNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbProsthesisNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add lbInsured.Text
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_Insured.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(30);
            oGridViewRow.Cells.Add(oTableCell);

            //Add MaterialPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_MaterialPrice.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(75);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProcessPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbProcess.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(75);
            oGridViewRow.Cells.Add(oTableCell);


            #endregion
            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void gvOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            SaveCheckBox();
            gvOrderList.PageIndex = e.NewPageIndex;
            //Get module listing
            List<BillStatementInfo> listBillStatementInfo = GetListFilterByClosingDay(BillStatementInfo.GetBillStatements(GetObjectSearchInfo()));
            BindGridView(listBillStatementInfo);

            Hashtable hash = this.HashCheckArray;
            hiddenChooseBillCd.Value = "";
            if (hash != null && hash.Count > 0)
            {
                List<BillStatementInfo> listBillStatement = hash.Values.OfType<BillStatementInfo>().ToList();
                double insured = listBillStatement.Where(p => p.InsuranceKbn == true).Sum(p => (p.ProcessPrice == null ? 0 : Convert.ToDouble(p.ProcessPrice)) + (p.MaterialPrice == null ? 0 : Convert.ToDouble(p.MaterialPrice)));
                double UnInsured = listBillStatement.Where(p => p.InsuranceKbn == false).Sum(p => (p.ProcessPrice == null ? 0 : Convert.ToDouble(p.ProcessPrice)) + (p.MaterialPrice == null ? 0 : Convert.ToDouble(p.MaterialPrice)));
                hiddenInsured.Value = insured.ToString();
                hiddenUninsured.Value = UnInsured.ToString();

                insured = BillConstantIndex.RoundSystemPrice(insured, hiddenRoundSystem.Value);
                UnInsured = BillConstantIndex.RoundSystemPrice(UnInsured, hiddenRoundSystem.Value);
                txtBillingAmountInsured.Text = insured.ToString();
                txtBillingAmountUnInsured.Text = UnInsured.ToString();
                txtSum.Text = (Convert.ToDouble(txtBillingAmountInsured.Text) + UnInsured).ToString();
                double fee = string.IsNullOrWhiteSpace(txtAdjustedFeeInsured.Text) ? 0 : Convert.ToInt32(txtAdjustedFeeInsured.Text);
                txtBillingTotalAmount.Text = (insured + UnInsured - fee).ToString();

                if (hiddenIsMultiBillCd.Value != "")
                {
                    foreach (BillStatementInfo i in listBillStatement)
                    {
                        if (hiddenChooseBillCd.Value == "")
                            hiddenChooseBillCd.Value = i.BillCd;
                        else if (hiddenChooseBillCd.Value != i.BillCd)
                        {
                            hiddenChooseBillCd.Value = "-1";
                            break;
                        }
                    }
                }
            }
            else
            {
                ResetInputNewIssue();
            }
            logger.Debug("END gvOrderList_PageIndexChanging");
        }
        catch (Exception ex)
        {
            logger.Debug("ERROR= " + ex.Message);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");", true);
        }
    }
    #endregion

    #region NewIssueBillStatement
    private void NewIssueBillStatement(int officeCd, string billYear, string billMonth, DateTime issueDate)
    {
        logger.Debug("Begin NewIssueBillStatement ");

        int feeAdjust = 0;
        List<BillStatementInfo> listSearch = GetListBillStamentInfo(officeCd, ref feeAdjust);
        logger.Debug("listSearch.Count = " + listSearch.Count);
        var listBill = listSearch.OrderBy(p => p.BillCd).Select(p => p.BillCd).Distinct().ToList();
        logger.Debug("list BillCd = " + listBill.Count);
        List<ReportBillStatement> listReport = new List<ReportBillStatement>();
    
        MasterTax objTax = new MasterTax();
        objTax = MasterTax.SearchTax(issueDate);

        string companyName, companyAddress, companyPostalCode, companyTel, companyFax;
        GetCompanyReportHeader(out companyName, out companyAddress, out companyPostalCode, out companyTel, out companyFax);
        int numberPage = 0;
        List<PdfBillStatementInfo> listRangePdf = new List<PdfBillStatementInfo>();

        using (DBContext db = new DBContext())
        {
            using (System.Data.Common.DbTransaction tran = db.UseTransaction())
            {
                try
                {
                    foreach (string billCd in listBill)
                    {
                        var mstBill = MasterBill.GetBillMaster(officeCd, int.Parse(billCd));
                        if (mstBill == null)
                            throw new Exception(string.Format(GetResource("MSG_CAN_NOT_FOUND_MST_BILL.Text"), billCd));
                        logger.Debug("Issue mstBill.SupplierCd = " + mstBill.SupplierCd);

                        MasterBillStatementNo masterBillStatementNo = MasterBillStatementNo.GetBillStatementNo(officeCd, mstBill.BillCd, Convert.ToInt32(billYear));
                        if (masterBillStatementNo == null)
                        {
                            masterBillStatementNo = new MasterBillStatementNo();
                            masterBillStatementNo.OfficeCd = officeCd;
                            masterBillStatementNo.BillCd = mstBill.BillCd;
                            masterBillStatementNo.Year = Convert.ToInt32(billYear);
                            masterBillStatementNo.BlanchNo  = 0;
                            masterBillStatementNo.CreateAccount = masterBillStatementNo.ModifiedAccount  = this.User.Identity.Name;
                        }

                        DateTime deliverFrom, deliverTo;
                        GetDateFromClosingDay(mstBill.BillClosingDay, out deliverFrom, out deliverTo);
                        List<BillStatementInfo> listBillStatement = listSearch.Where(p => p.BillCd == billCd).ToList();
                        if (listBillStatement.Count == 0)
                            continue;

                        #region Get BillHeader

                        var objBillHeader = new TrnBillHeader();
                        objBillHeader.OfficeCd = officeCd;
                        objBillHeader.BillSeq = TrnBillHeader.GetNextBillSeq(officeCd);
                        objBillHeader.BillYear = billYear;
                        objBillHeader.BillMonth = billMonth;
                        objBillHeader.BillPeriod = string.Format("{0} - {1}", deliverFrom.ToString("yyyy/MM/dd"), deliverTo.ToString("yyyy/MM/dd"));
                        objBillHeader.BillStatementNo = string.Format("B{0}-{1}-{2}-{3}", officeCd, billCd, billYear, masterBillStatementNo.BlanchNo + 1 );
                        objBillHeader.BillIssueDate = issueDate;
                        objBillHeader.BillCd = mstBill.BillCd;
                        objBillHeader.BillNm = mstBill.BillNm;
                        objBillHeader.Note = txtComment.Text == "" ? null : txtComment.Text;
                        objBillHeader.CreateAccount = this.User.Identity.Name;
                        //we can use txtBillingAmount 
                        objBillHeader.CurrentInsuredPrice = BillConstantIndex.RoundSystemPrice(listBillStatement.Where(p => p.InsuranceKbn== true).Sum(p => (p.ProcessPrice == null ? 0 : Convert.ToDouble(p.ProcessPrice)) + (p.MaterialPrice == null ? 0 : Convert.ToDouble(p.MaterialPrice))), hiddenRoundSystem.Value);
                        objBillHeader.CurrentUnInsuredPrice = BillConstantIndex.RoundSystemPrice(listBillStatement.Where(p => p.InsuranceKbn == false).Sum(p => (p.ProcessPrice == null ? 0 : Convert.ToDouble(p.ProcessPrice)) + (p.MaterialPrice == null ? 0 : Convert.ToDouble(p.MaterialPrice))), hiddenRoundSystem.Value);
                        objBillHeader.PreviousSumPrice = objBillHeader.CurrentPaidMoney = objBillHeader.CarryOver = 0;
                        var lastBillHeader = TrnBillHeader.GetLastTrnBillHeader(officeCd, mstBill.BillCd);
                        if (lastBillHeader != null && lastBillHeader.Count > 0)
                        {
                            logger.Debug("Found lastBillHeader.count = " + lastBillHeader.Count);
                            objBillHeader.PreviousSumPrice = lastBillHeader.First().CurrentSumPrice;   // lastBillHeader.CurrentSumPrice;
                            objBillHeader.CarryOver = lastBillHeader.Sum(p => p.CurrentSumPrice - p.CurrentPaidMoney);  //PurchaseCarryOver = CurentSumPrice - CurrentPaidmoney from preveous TrnBillHeader.
                        }
                        objBillHeader.CurrentTechPrice = BillConstantIndex.RoundSystemPrice(listBillStatement.Sum(p => p.ProcessPrice == null ? 0 : p.ProcessPrice.Value ), hiddenRoundSystem.Value); //  sum(TechPrice.TrnOrderDetail)
                        objBillHeader.CurrentMaterialPrice = BillConstantIndex.RoundSystemPrice(listBillStatement.Sum(p => p.MaterialPrice == null ? 0 : p.MaterialPrice.Value ), hiddenRoundSystem.Value); //  sum(MaterialPrice.TrnOrderDetail)
                        objBillHeader.CurrentPrice = objBillHeader.CurrentTechPrice + objBillHeader.CurrentMaterialPrice;
                        objBillHeader.CurrentDiscount = feeAdjust;
                        objBillHeader.CurrentTax = (objBillHeader.CurrentPrice - objBillHeader.CurrentDiscount);

                        if (objTax != null)
                        {
                            if (objTax.RoundFraction == 2)
                                objBillHeader.CurrentTax = Math.Floor(Convert.ToDouble(objTax.TaxRate) * objBillHeader.CurrentTax);
                            else if (objTax.RoundFraction == 3)
                                objBillHeader.CurrentTax = Math.Ceiling(Convert.ToDouble(objTax.TaxRate) * objBillHeader.CurrentTax);
                            else
                                objBillHeader.CurrentTax = Math.Floor((Convert.ToDouble(objTax.TaxRate) * objBillHeader.CurrentTax) + 0.5);
                        }
                        else
                            objBillHeader.CurrentTax = 0;
                        objBillHeader.CurrentTax = BillConstantIndex.RoundSystemPrice(objBillHeader.CurrentTax, hiddenRoundSystem.Value);
                        objBillHeader.CurrentSumPrice = objBillHeader.CurrentPrice - objBillHeader.CurrentDiscount + objBillHeader.CurrentTax; //CurrentPrice  - CurrentDiscount + CurrentTax
                        ReportBillStatement item = ReportBillStatement.GetItem(objBillHeader, mstBill);
                        item.CompanyNm = companyName;
                        item.CompanyAddress = companyAddress;
                        item.CompanyPostalCode = companyPostalCode;
                        item.CompanyTel = companyTel;
                        item.CompanyFax = companyFax;
                        #endregion

                        logger.Debug("End get BillHeader , billSeq =" + objBillHeader.BillSeq);

                        #region GetList Material
                        Hashtable hashMaterial = new Hashtable();
                        if (mstBill.SupplierCd != null && mstBill.SupplierCd !=0)
                        {
                            bool isFirstBillCd = TrnBillHeader.isFirstBillCd(masterBillStatementNo.OfficeCd , masterBillStatementNo.BillCd , masterBillStatementNo.Year);
                            List<TrnStockInOut> listBorrowedMaterial = new List<TrnStockInOut>();
                            if (isFirstBillCd)
                            {
                                logger.Debug(" Case First BillCd. Get listBorrowedMaterial ,deliverTo = " + deliverTo);
                                listBorrowedMaterial = TrnStockInOut.GetListBorrowedMaterialStockInOut(officeCd, Convert.ToInt32(mstBill.SupplierCd), mstBill.BillCd, null, deliverTo.AddDays(1));
                            }
                            else
                            {
                                logger.Debug(" Case Not First BillCd. Get listBorrowedMaterial , deliverFrom = " + deliverFrom);
                                listBorrowedMaterial = TrnStockInOut.GetListBorrowedMaterialStockInOut(officeCd, Convert.ToInt32(mstBill.SupplierCd), mstBill.BillCd, deliverFrom, deliverTo);
                            }
                            logger.Debug(" listBorrowedMaterial.Count = " + listBorrowedMaterial.Count);
                            if (listBorrowedMaterial.Count > 0)
                            {
                                var groupMaterial = listBorrowedMaterial.GroupBy(p => p.MaterialCd).ToList();
                                for (int i = 0; i < groupMaterial.Count; i++)
                                {
                                    List<TrnStockInOut> listMaterial = groupMaterial[i].ToList();

                                    TrnBillMaterial objBillMaterial = new TrnBillMaterial();
                                    objBillMaterial.OfficeCd = officeCd;
                                    objBillMaterial.MaterialCd = listMaterial[0].MaterialCd;
                                    objBillMaterial.MaterialNm = listMaterial[0].MaterialNm;
                                    objBillMaterial.MaterialCarryOver = objBillMaterial.MaterialBorrow = 0;
                                    objBillMaterial.MaterialStock = TrnBillMaterial.GetMaterialStock(officeCd, objBillMaterial.MaterialCd, mstBill.BillCd);

                                    logger.Debug("MaterialStock (last) = " + listMaterial[0].MaterialStock);
                                    if (isFirstBillCd || string.IsNullOrEmpty(listMaterial[0].MaterialStock))
                                    {
                                        objBillMaterial.MaterialCarryOver = 0;
                                    }
                                    else
                                    {
                                        logger.Debug(string.Format("Get MaterialCarryOver(Have LastData) to UntilLast month officeCd = {0} SupplierCd = {1} , MaterialCd = {2} ,deliverFrom = {3}", officeCd, mstBill.SupplierCd, objBillMaterial.MaterialCd, deliverFrom));
                                        objBillMaterial.MaterialCarryOver = Convert.ToDouble(listMaterial[0].MaterialStock);
                                        logger.Debug("MaterialCarryOver = " + objBillMaterial.MaterialCarryOver);
                                    }
                                    objBillMaterial.MaterialBorrow = listMaterial.Sum(p => p.InOutKbn == 12 ? p.Amount : -p.Amount);
                                    objBillMaterial.MaterialUsed = listBillStatement.Where(p => (p.SupplierCd == mstBill.SupplierCd) && (p.MaterialCd == objBillMaterial.MaterialCd)).Sum(p => p.Amount == null ? 0 : Convert.ToDouble(p.Amount));

                                    logger.Debug(string.Format("Get (StockInOut) MaterialCd = {0} ,MaterialCarryOver = {1} , MaterialBorrow = {2} , MaterialUsed = {3}", objBillMaterial.MaterialCd, objBillMaterial.MaterialCarryOver, objBillMaterial.MaterialBorrow, objBillMaterial.MaterialUsed));
                                    if (objBillMaterial.MaterialCarryOver != 0 || objBillMaterial.MaterialUsed != 0 || objBillMaterial.MaterialBorrow != 0) //Ignore allMaterial = 0
                                    {
                                        hashMaterial.Add(listMaterial[0].MaterialCd.ToString(), objBillMaterial);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Update Detail
                        logger.Debug("Begin Update DentalOrderDetail for BillStatementNo = " + objBillHeader.BillStatementNo);
                        foreach (BillStatementInfo i in listBillStatement)
                        {
                            if (i.MaterialCd != null &&  !hashMaterial.Contains(i.MaterialCd.ToString()))
                            {
                                TrnBillMaterial objBillMaterial = new TrnBillMaterial();
                                objBillMaterial.MaterialCd = Convert.ToInt32(i.MaterialCd);
                                objBillMaterial.MaterialNm = i.MaterialNm;
                                objBillMaterial.MaterialCarryOver = BillStatementInfo.GetMaterialCarryOver(officeCd, mstBill.BillCd, objBillMaterial.MaterialCd);
                                objBillMaterial.MaterialBorrow = 0;
                                objBillMaterial.MaterialUsed = listBillStatement.Where(p => (p.SupplierCd == mstBill.SupplierCd) && ( p.MaterialCd.Value == objBillMaterial.MaterialCd)).Sum(p => p.Amount == null ? 0 : Convert.ToDouble(p.Amount));

                                if (objBillMaterial.MaterialCarryOver != 0 || objBillMaterial.MaterialUsed != 0) //Ignore allMaterial = 0 
                                {
                                    logger.Debug(string.Format("Get (OrderDetail) MaterialCd = {0} ,MaterialCarryOver = {1} , MaterialBorrow = {2}", objBillMaterial.MaterialCd, objBillMaterial.MaterialCarryOver, objBillMaterial.MaterialBorrow));
                                    hashMaterial.Add(i.MaterialCd.ToString() , objBillMaterial);
                                }
                            }

                            TrnOrderDetail detail = TrnOrderDetail.GetDentalOrderDetail(officeCd, i.OrderSeq , i.DetailSeq );
                            detail.BillStatementNo = objBillHeader.BillStatementNo;
                            detail.ModifiedAccount = this.User.Identity.Name;
                            detail.Update();
                        }

                        #endregion

                        #region Update listMaterial
                        logger.Debug("Update list TrnBillMaterial.count = " + hashMaterial.Count);
                        int totalMaterialReport = 0;
                        item.TotalPage = hashMaterial.Count / BillConstantIndex.TOTAL_MATERIAL_PER_PAGE + (hashMaterial.Count % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE == 0 ? 0 : 1);
                        int beginBillPage = ++numberPage;


                        foreach (string key in hashMaterial.Keys)
                        {
                            TrnBillMaterial objBillMaterial = (TrnBillMaterial)hashMaterial[key];
                            objBillMaterial.BillSeq = objBillHeader.BillSeq;
                            objBillMaterial.DelFlg = false;
                            objBillMaterial.MaterialStock = objBillMaterial.MaterialCarryOver - objBillMaterial.MaterialUsed + objBillMaterial.MaterialBorrow;  //MaterialStock = CarryOver - used Amount(this month) + MaterialBorrow
                            objBillMaterial.CreateAccount = objBillMaterial.ModifiedAccount = this.User.Identity.Name;                            
                            objBillMaterial.Insert();

                            if (totalMaterialReport != 0 && totalMaterialReport % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE == 0)
                                numberPage++;

                            totalMaterialReport++;
                            ReportBillStatement reportItem = ReportBillStatement.GetItem(item, objBillMaterial);
                            reportItem.BillPage = totalMaterialReport / BillConstantIndex.TOTAL_MATERIAL_PER_PAGE + 1;
                            listReport.Add(reportItem);
                        }
                        if (totalMaterialReport == 0 || totalMaterialReport % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE != 0)
                            AddEmptyMaterialReport(totalMaterialReport % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE, item, listReport);

                        #endregion

                        #region AddBillHeader
                        logger.Debug("AddTrnBillHeader for BillSeq = " + objBillHeader.BillSeq);
                        objBillHeader.CreateAccount = objBillHeader.ModifiedAccount = this.User.Identity.Name;
                        objBillHeader.DelFlg = false;
                        objBillHeader.Insert(); 

                        // Insert BillStatementNo 
                        masterBillStatementNo.BlanchNo = masterBillStatementNo.BlanchNo + 1;
                        if (masterBillStatementNo.BlanchNo == 1)
                        {
                            masterBillStatementNo.Insert();
                        }
                        else
                        {
                            masterBillStatementNo.Update();
                        }

                        #endregion

                        listRangePdf.Add(new PdfBillStatementInfo(objBillHeader.BillStatementNo, beginBillPage + "-" + numberPage));
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    logger.Error("Error NewIssueBillStatement", ex);
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        logger.Debug("total listReport.Count = " + listReport.Count);
        if (listReport.Count > 0)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS.Text")) + "\");", true);
            runRptViewer(listReport);
            SplitBillStatementNo(listRangePdf, this.ReportViewer1.LocalReport.DisplayName);
        }
        else
        {
            throw new Exception(GetResource("MSG_CANNOT_FIND_DETAIL.Text"));
           
        }
    }
    private object GetValueParameterCommand(string value)
    {
        if (string.IsNullOrEmpty(value))
            return DBNull.Value;
        return value;
    }
    #endregion

    #region Issue
    private BillStatementInfo GetBillStamentInfo(GridViewRow msgRow)
    {
        BillStatementInfo obj = new BillStatementInfo();
        obj.OfficeCd = Convert.ToInt32(hiddenOfficeCd.Value);
        obj.OrderSeq = double.Parse(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_OrderSeq].Text);
        obj.DetailSeq = int.Parse(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_DetailSeq].Text);
        obj.DentalOfficeCd = int.Parse(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_DentalOfficeCd].Text);
        obj.DeliveredDate = Convert.ToDateTime(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_DeliveredDate].Text);
        obj.BillCd = Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_BillCd].Text);
        obj.MaterialCd = Common.GetNullableInt( Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_MaterialCd].Text));
        obj.SupplierCd = Common.GetNullableInt( Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_SupplierCd].Text));
        obj.MaterialNm = Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_MaterialName].Text);
        obj.InsuranceKbn = bool.Parse(Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_Insurance].Text));
        obj.ProcessPrice = Common.GetNullableDouble( Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_ProcessPrice].Text));
        obj.MaterialPrice = Common.GetNullableDouble(Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_MaterialPrice].Text));
        obj.Amount = Common.GetNullableInt (Common.GetRowString(gvOrderList.Rows[msgRow.RowIndex].Cells[BillConstantIndex.index_Amount].Text));
        return obj;
    }
    private void GetDateFromClosingDay(int closingDay, out DateTime from, out DateTime to)
    {
        from = to = DateTime.Now;
        int billYear = Convert.ToInt32(dlYear.SelectedValue);
        int billMonth = Convert.ToInt32(dlmonth.SelectedValue);
        if (closingDay == 0)
        {
            from = new DateTime(billYear, billMonth, 1);
            to = new DateTime(billYear, billMonth, DateTime.DaysInMonth(billYear, billMonth));
        }
        else
        {
            if (billMonth > 1)
            {
                from = new DateTime(billYear, billMonth - 1, closingDay + 1);
                to = new DateTime(billYear, billMonth, closingDay);
            }
            else if (billMonth == 1)
            {
                from = new DateTime(billYear - 1, 12, closingDay + 1);
                to = new DateTime(billYear, billMonth, closingDay);
            }
        }
    }
    private List<BillStatementInfo> GetListFilterByClosingDay(List<BillStatementInfo> listSearch)
    {
        var listBill = listSearch.Select(p => p.BillCd).Distinct().ToList();
        foreach (string billCd in listBill)
        {
            var objBill = MasterBill.GetBillMaster(int.Parse(hiddenOfficeCd.Value), int.Parse(billCd));
            if (objBill != null)
            {
                DateTime deliverFrom, deliverTo;
                GetDateFromClosingDay(objBill.BillClosingDay, out deliverFrom, out deliverTo);
                listSearch.RemoveAll(p => p.BillCd == billCd && (p.DeliveredDate.Value.Date  < deliverFrom.Date || p.DeliveredDate.Value.Date  > deliverTo.Date));
            }
        }
        return listSearch;
    }
    private List<BillStatementInfo> GetListBillStamentInfo(int officeCd, ref int feeAdjust)
    {
        List<BillStatementInfo> listSearch = new List<BillStatementInfo>();
        if (cbSearch.Checked)
        {
            logger.Debug("Case SEARCH ");
            feeAdjust = txtAdjustedFeeInsured.Text == "" ? 0 : Convert.ToInt32(txtAdjustedFeeInsured.Text);
            foreach (GridViewRow msgRow in gvOrderList.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                if (ck.Checked == true)
                {
                    listSearch.Add(GetBillStamentInfo(msgRow));
                }
            }
            listSearch.AddRange(this.HashCheckArray.Values.OfType<BillStatementInfo>().ToList());
        }
        else
        {
            logger.Debug("Case  NO SEARCH ");
            BillSearchInfo objSearch = new BillSearchInfo();
            objSearch.OfficeCd = officeCd;
            objSearch.OrderNo = "";
            objSearch.BillYear = dlYear.SelectedValue;
            objSearch.BillMonth = dlmonth.SelectedValue;
            objSearch.BillCd = objSearch.OrderDateFrom = objSearch.OrderDateTo = objSearch.DeliverDateFrom = objSearch.DeliverDateTo = objSearch.DentalOfficeCd = null;
            objSearch.UnInsured = objSearch.Insured = true;

            listSearch = GetListFilterByClosingDay(BillStatementInfo.GetBillStatements(objSearch));
        }
        if (listSearch.Count == 0)
        {
            throw new Exception(GetResource("MSG_CANNOT_FIND_DETAIL.Text"));
        }
        return listSearch;
    }
    
    private void ReissueDelete(int officeCd, string billYear, string billMonth)
    {
        List<ReportBillStatement> listReport = new List<ReportBillStatement>();
        List<TrnBillHeader> listBillHeader = TrnBillHeader.GetListTrnBillHeaderSearch(officeCd, txtInvoiceNo.Text).OrderBy(p => p.BillCd).ToList();
        logger.Debug("*** Begin ReissueDelete , listBillHeader.Count = " + listBillHeader.Count);
        if (listBillHeader.Count == 0)
            throw new Exception(string.Format(GetResource("MSG_CAN_NOT_FIND_DATA_FROM_BILL_STATEMENT.Text"), txtInvoiceNo.Text));
        if (rbDelete.Checked && listBillHeader.Any(p => p.CurrentPaidMoney != 0d))
            throw new Exception(GetResource("MSG_CANT_DELETE_BILLSTATEMENT.Text"));

        var listBillCd = listBillHeader.Select(p => p.BillCd).Distinct().ToList();
        string companyName, companyAddress, companyPostalCode, companyTel, companyFax;
        GetCompanyReportHeader(out companyName, out companyAddress, out companyPostalCode, out companyTel, out companyFax);
        foreach (int billCd in listBillCd)
        {
            logger.Debug(" Case BillCd = " + billCd);
            var mstBill = MasterBill.GetBillMaster(officeCd, billCd);
            if (mstBill != null)
            {
                logger.Debug(" Contains BillCd in Bill Master");
                var objBillHeader = listBillHeader.Where(p => p.BillCd == billCd).FirstOrDefault();
                ReportBillStatement item = ReportBillStatement.GetItem(objBillHeader, mstBill);
                item.CompanyNm = companyName;
                item.CompanyAddress = companyAddress;
                item.CompanyPostalCode = companyPostalCode;
                item.CompanyTel = companyTel;
                item.CompanyFax = companyFax;
                var listBillMaterial = TrnBillMaterial.GetListTrnBillMaterialSearch(officeCd, objBillHeader.BillSeq);
                logger.Debug(" listBillMaterial.count = " + listBillMaterial.Count);
                item.TotalPage = listBillMaterial.Count / BillConstantIndex.TOTAL_MATERIAL_PER_PAGE + (listBillMaterial.Count % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE == 0 ? 0 : 1);
                for (int i = 0; i < listBillMaterial.Count; i++)
                {
                    ReportBillStatement reportItem = ReportBillStatement.GetItem(item, listBillMaterial[i]);
                    reportItem.BillPage = i / BillConstantIndex.TOTAL_MATERIAL_PER_PAGE + 1;
                    listReport.Add(reportItem);
                }
                if (listBillMaterial.Count == 0 || listBillMaterial.Count % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE != 0)
                {
                    AddEmptyMaterialReport(listBillMaterial.Count % BillConstantIndex.TOTAL_MATERIAL_PER_PAGE, item, listReport);
                }
            }
        }
        if (rbDelete.Checked)
        {
            logger.Debug("Call DeleteIssueBillStatementNo ");

            TrnBillHeader.DeleteIssueBillStatementNo(officeCd, txtInvoiceNo.Text, this.User.Identity.Name);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS.Text")) + "\");", true);
        }
        logger.Debug("listReport.count =  " + listReport.Count);
        if (listReport.Count > 0)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS.Text")) + "\");", true);
            runRptViewer(listReport);
            SplitBillStatementNo(new List<PdfBillStatementInfo>(), this.ReportViewer1.LocalReport.DisplayName);
        }
        else
        {
            throw new Exception(GetResource("MSG_CANNOT_FIND_DETAIL.Text"));
        }
    }
    private void AddEmptyMaterialReport(int countMaterial, ReportBillStatement item, List<ReportBillStatement> listReport)
    {
        for (int i = countMaterial; i < BillConstantIndex.TOTAL_MATERIAL_PER_PAGE; i++)
        {
            if (i == 0)
            {
                listReport.Add(ReportBillStatement.GetItem(item, null));
            }
            else
                listReport.Add(null);
        }
    }
    private void GetCompanyReportHeader(out string companyName, out string companyAddress, out string companyPostalCode, out string companyTel, out string companyFax)
    {

        companyName = MasterSystem.GetSystemMaster("CompanyName").Value;
        companyAddress = MasterSystem.GetSystemMaster("CompanyAddress").Value;
        companyPostalCode = MasterSystem.GetSystemMaster("CompanyPostalCode").Value;
        companyTel =  MasterSystem.GetSystemMaster("CompanyTEL").Value;
        companyFax = MasterSystem.GetSystemMaster("CompanyFAX").Value;
    }
    #endregion

    #region SplitZip File
    private void runRptViewer(List<ReportBillStatement> listReport)
    {
        string statusTitle = "";
        string titleNm = "";
        if (rbReissueDelete.Checked)
        {
            if (rbDelete.Checked)
            {
                statusTitle = GetResource("REPORT_StatusTitle_DELETE.Text");
                titleNm = GetResource("REPORT_NmDelete.Text");
            }
            else
            {
                statusTitle = GetResource("REPORT_StatusTitle_REISSUE.Text");
                titleNm = GetResource("REPORT_NmReissue.Text");
            }
        }
        panelReport.Visible = true;
        panelBillStatement.Visible = false;
        this.ReportViewer1.Reset();
        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("reportBillStatement.rdlc"); 

        ReportDataSource rds = new ReportDataSource("ReportBillStatement", listReport);
        this.ReportViewer1.LocalReport.DataSources.Clear();
        this.ReportViewer1.LocalReport.DataSources.Add(rds);
        DisableUnwantedExportFormat(ReportViewer1, "WORD");
        DisableUnwantedExportFormat(ReportViewer1, "Excel");
        this.ReportViewer1.LocalReport.EnableExternalImages = true;

        ReportParameter[] param = new ReportParameter[23];

        param[0] = new ReportParameter("AccountStatementTitleCopy", GetResource("REPORT_AccountStatementTitleCopy.Text"), false);
        param[1] = new ReportParameter("ClosingPeriod", "ClosingPeriod", false);
        param[2] = new ReportParameter("BillStatementNo", "BillStatementNo", false);
        param[3] = new ReportParameter("InvoiIssueDateTitle", "InvoiIssueDate", false);
        param[4] = new ReportParameter("InvoiIssueDate", txtIssueDate.Text, false);

        param[5] = new ReportParameter("LastMonth", GetResource("REPORT_LastMonth.Text"), false);
        param[6] = new ReportParameter("CarryOver", GetResource("REPORT_CarryOver.Text"), false);
        param[7] = new ReportParameter("Purchase", GetResource("REPORT_Purchase.Text"), false);
        param[8] = new ReportParameter("DisCount", GetResource("REPORT_Discount.Text"), false);

        param[9] = new ReportParameter("Tax", GetResource("REPORT_Tax.Text"), false);
        param[10] = new ReportParameter("SumPurchase", GetResource("REPORT_SumPurchase.Text"), false);
        param[11] = new ReportParameter("Material", GetResource("REPORT_Material.Text"), false);
        param[12] = new ReportParameter("CarryOverMaterial", GetResource("REPORT_CarryOverMaterial.Text"), false);
        param[13] = new ReportParameter("UserAccount", GetResource("REPORT_UserAccount.Text"), false);
        param[14] = new ReportParameter("RestOfReceivedMaterial", GetResource("REPORT_RestOfReceivedMaterial.Text"), false);

        param[15] = new ReportParameter("TechPrice", GetResource("REPORT_TechPrice.Text"), false);
        param[16] = new ReportParameter("MaterialPrice", GetResource("REPORT_MaterialPrice.Text"), false);
        param[17] = new ReportParameter("BillPage", GetResource("REPORT_BillPage.Text"), false);
        param[18] = new ReportParameter("AccountStatementTitle", GetResource("REPORT_AccountStatementTitle.Text"), false);
        param[19] = new ReportParameter("AdditionReceiveMaterial", GetResource("REPORT_AdditionReceiveMaterial.Text"), false);
        param[20] = new ReportParameter("TotalPage", (listReport.Count / BillConstantIndex.TOTAL_MATERIAL_PER_PAGE).ToString(), false);
        param[21] = new ReportParameter("StatusTitle", statusTitle, false);
        param[22] = new ReportParameter("BankInfomation", GetResource("REPORT_BankInfomation.Text"), false);

        this.ReportViewer1.LocalReport.SetParameters(param);
        this.ReportViewer1.DataBind();
        this.ReportViewer1.LocalReport.Refresh();
        this.ReportViewer1.LocalReport.DisplayName = listReport[0].BillStatementNo + titleNm;
    }
    public void DisableUnwantedExportFormat(ReportViewer ReportViewerID, string strFormatName)
    {
        FieldInfo info;
        foreach (RenderingExtension extension in ReportViewerID.LocalReport.ListRenderingExtensions())
        {
            if (extension.Name == strFormatName)
            {
                info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                info.SetValue(extension, false);
            }
        }
    }
    private void VisibleHyperlinkSaveReport(bool visible)
    {
        //this.hyperlinkSave.Visible = visible;
        this.ReportViewer1.ShowExportControls = !visible;
    }
    private void SplitBillStatementNo(List<PdfBillStatementInfo> listRange, string displayName)
    {
        try
        {
            string fullfolderZip = CreateBillStatementStore(displayName);

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = ReportViewer1.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            string _outMergeFile = "";
            if (listRange.Count > 1)
                _outMergeFile = fullfolderZip + "\\" + displayName + "All.pdf";
            else
                _outMergeFile = fullfolderZip + "\\" + displayName + ".pdf";

            FileStream fs = new FileStream(_outMergeFile, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            if (listRange.Count > 1)
            {
                for (int i = 0; i < listRange.Count; i++)
                {
                    PdfReader readerA = new PdfReader(_outMergeFile);
                    readerA.SelectPages(listRange[i].pageRange);
                    logger.Debug("listRange[i].pageRange = " + listRange[i].pageRange);
                    PdfStamper cc = new PdfStamper(readerA, new FileStream(fullfolderZip + "\\" + listRange[i].billStatementNo + ".pdf", FileMode.CreateNew));
                    cc.Close();
                    readerA.Close();
                }
                logger.Debug("End split file");
                if (File.Exists(_outMergeFile))
                    File.Delete(_outMergeFile);
            }
            //this.hyperlinkSave.NavigateUrl = SaveBillStatementFile(displayName);
            VisibleHyperlinkSaveReport(true);
        }
        catch (Exception ex)
        {
            logger.Warn("Warn SplitBillStatementNo ", ex);
            VisibleHyperlinkSaveReport(false);
        }
    }
    protected string CreateBillStatementStore(string folderStore)
    {
        string folderPath = Server.MapPath("Portal" + "\\" + "BillStatement"); 
        logger.Debug("Begin CreateBillStatementStore in folderPath = " + folderPath);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        else
        {
            try
            {
                List<FileInfo> ListFiles = new System.IO.DirectoryInfo(folderPath).GetFiles().Where(p => p.CreationTime.AddMinutes(10) <= DateTime.Now).ToList();
                logger.Debug("total ListFile delete = " + ListFiles.Count);
                foreach (FileInfo i in ListFiles)
                    i.Delete();

                List<DirectoryInfo> ListFolder = new System.IO.DirectoryInfo(folderPath).GetDirectories().Where(p => p.CreationTime.AddMinutes(10) <= DateTime.Now).ToList();
                logger.Debug("total ListFolder delete = " + ListFiles.Count);
                foreach (DirectoryInfo i in ListFolder)
                    i.Delete(true);
            }
            catch { }
        }
        string folderName = Path.Combine(folderPath, folderStore);
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);
        logger.Debug("End CreateBillStatementStore in folderName = " + folderName);
        return folderName;
    }

    protected string SaveBillStatementFile(string zipFolderDelivery)
    {
        try
        {
            string folderPath = Server.MapPath("Portal" + "\\" + "BillStatement");
            string folderName = Path.Combine(folderPath, zipFolderDelivery);
            if (!Directory.Exists(folderName))
            {
                return "";
            }
            CreateZipFile(folderName, folderPath + "\\" + zipFolderDelivery + ".zip");
            return folderPath + "\\" + zipFolderDelivery + ".zip"; 
        }
        catch (Exception ex)
        {
            logger.Debug("Error SaveBillStatementFile = " + ex.Message);
            return ex.Message;
        }
    }
    private void CreateZipFile(string _folderZip, string _ZipFile)
    {
        if (!Directory.Exists(_folderZip))
        {
            return;
        }
        using (ZipFile zip = new ZipFile(Encoding.UTF8))
        {
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
            zip.TempFileFolder = Server.MapPath("");
            zip.StatusMessageTextWriter = System.Console.Out;
            zip.AddDirectory(_folderZip); // recurses subdirectories
            zip.Save(_ZipFile);
        }
    }
    #endregion 

    protected void ImageSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filepath = SaveBillStatementFile(this.ReportViewer1.LocalReport.DisplayName);
            FileInfo myfile = new FileInfo(filepath);
            if (myfile.Exists)
            {
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + myfile.Name);
                Response.AddHeader("Content-Length", myfile.Length.ToString());
                Response.ContentType = "application/zip";
                Response.TransmitFile(myfile.FullName);
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Save Report", ex);
        }  
    }
}