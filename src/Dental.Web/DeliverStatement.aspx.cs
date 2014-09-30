using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;
using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.Collections;
using System.IO;
using Ionic.Zip;
using System.Text;
using iTextSharp.text.pdf;

partial class DeliverStatement : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(DeliverStatement).Name);      
   // private OrderSearchInfo objSearch = new OrderSearchInfo();        
    //public int mOfficeCd;
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
                logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
                this.btPrint.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btPrint, "").ToString());   
                //btPrint.Attributes["onclick"] = "javascript:return consur();";
                HttpCookie ck = Request.Cookies[this.User.Identity.Name]; //Request.Cookies("test");
                if (ck != null)
                {
                    HiddenFieldOfficeCd.Value = ck["OfficeCd"];
                }
                else HiddenFieldOfficeCd.Value = "-1";
                HiddenFieldOfficeCd.Value = GetOffice();

                lbOrderNo.Text = GetResource("lbOrderNo.Text");
                lbOrderDate.Text = GetResource("lbOrderDate.Text");
                lbDeliveryDate.Text = GetResource("lbDueDate.Text");
                lbClinicName.Text = GetResource("lbClinicName.Text");
                lbSalesman.Text = GetResource("lbSalesman.Text");
                lblIncludeCompleteOrder.Text = GetResource("lblIncludeCompleteOrder.Text");
                lbRowPerPage.Text = GetResource("lbRowsPage.Text");

                btOk.Text = GetResource("btOk.Text");
                btCancel.Text = GetResource("btCancel.Text");
                gvOrderList.EmptyDataText = GetResource("NoRecordFound.Text");
                val_IssueDate.ErrorMessage = GetResource("DateOnly.Text");
                valRequired_IssueDate.ErrorMessage = GetResource("valRequired_IssueDate.ErrorMessage");
                btPrint.Text = GetResource("btPrint.Text");
                btBack.Text = GetResource("btBack.Text");
                lbPrintPerOrder.Text = GetResource("lbPrintPerOrder.Text");
                lbIssueDate.Text = GetResource("lbIssueDate.Text");

                //Get Staff
                fillSalesMan();
                //Get DentalOffice                
                fillClinic();
                //fill Data
                List<DeliveryStatementInfo> listDeliveryStatement = new List<DeliveryStatementInfo>();
                FillData(listDeliveryStatement);
                RemoveViewState();               
                LinkDentalFont.Text = GetResource("DentalFont.Text");
                //hyperlinkDentalFont.Text = GetResource("DentalFont.Text");
                //hyperlinkDentalFont.NavigateUrl = GetDentalFont();              
            }
        }
        catch (Exception exc)
        {
            logger.Error("Error Page_Load", exc);
        }
    }
    /// <summary>
    /// Sales Man
    /// </summary>
    protected void fillSalesMan()
    {        
        List<MasterStaff> colStaffList = MasterStaff.GetStaffwiOffice(int.Parse(HiddenFieldOfficeCd.Value)); ;
        dlStaff.Items.Add("");
        foreach (MasterStaff i in colStaffList)
        {
            if (i.SalesFlg.HasValue && i.SalesFlg.Value == true)
                dlStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
        }
    }
    /// <summary>
    /// Clinic
    /// </summary>
    protected void fillClinic()
    {
        List<MasterDentalOffice>  colDentalOfficeMasterList = MasterDentalOffice.GetDentalOfficeMasters(int.Parse(HiddenFieldOfficeCd.Value));
        dlDentalOffice.Items.Clear();
        dlDentalOffice.Items.Add("");
        foreach (MasterDentalOffice i in colDentalOfficeMasterList)
        {
            dlDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }        
    }
    /// <summary>
    /// Fill data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FillData(List<DeliveryStatementInfo> listDeliveryStatement)
    {
        gvOrderList.Columns[1].Visible = true;
        gvOrderList.Columns[2].Visible = true;
        gvOrderList.Columns[13].Visible = true;
        gvOrderList.Columns[14].Visible = true;   
        
        if (listDeliveryStatement.Count == 0)
        {            
            DeliveryStatementInfo objOL = new DeliveryStatementInfo();
            objOL.OrderSeq = -1;
            objOL.OrderNo = " ";
            objOL.OrderDate = DateTime.Today;
            objOL.DeliverDate = DateTime.Today;
            objOL.SetDate = DateTime.Today;

            listDeliveryStatement.Add(objOL);

            gvOrderList.DataSource = listDeliveryStatement;
            gvOrderList.DataBind();
            gvOrderList.Columns[1].Visible = false;
            gvOrderList.Columns[2].Visible = false;
            gvOrderList.Columns[13].Visible = false;
            gvOrderList.Columns[14].Visible = false;   

            if (listDeliveryStatement.Count == 1 && listDeliveryStatement[0].OrderSeq == -1)
            {
                gvOrderList.Rows[0].Visible = false;
            }
            lbMessage.Text = GetResource("NoRecordFound.Text");
            return;
        }
        else
        {
            gvOrderList.DataSource = listDeliveryStatement; 
            gvOrderList.DataBind();
            foreach (GridViewRow row in gvOrderList.Rows)
            {
                if (Common.GetRowString(row.Cells[4].Text) != "")
                    row.Cells[4].Text = SetDateFormat(Convert.ToDateTime(row.Cells[4].Text).ToShortDateString());

                if (Common.GetRowString(row.Cells[5].Text) != "")
                    row.Cells[5].Text = SetDateFormat(Convert.ToDateTime(row.Cells[5].Text).ToShortDateString());

                if (Common.GetRowString(row.Cells[6].Text) != "")
                    row.Cells[6].Text = SetDateFormat(Convert.ToDateTime(row.Cells[6].Text).ToShortDateString());

                ListItem item = dlDentalOffice.Items.FindByValue(Common.GetRowString(row.Cells[7].Text));
                if (item != null)
                    row.Cells[7].Text = item.Text;
                else row.Cells[7].Text = "";

                if (row.Cells[11].Text == "True")
                    row.Cells[11].Text = GetResource("Insurance.Text");
                else
                    row.Cells[11].Text = GetResource("UnInsured.Text");

                if (Common.GetRowString(row.Cells[13].Text) == "True" || Common.GetRowString(row.Cells[13].Text) == "1" )
                    row.Cells[12].Text = GetResource("Trial.Text");
                else  if (Common.GetRowString(row.Cells[14].Text) == "True" || Common.GetRowString(row.Cells[14].Text) == "1")
                    row.Cells[12].Text = GetResource("Remake.Text");
                else
                    row.Cells[12].Text = GetResource("Finish.Text");
            }
            gvOrderList.Columns[1].Visible = false;
            gvOrderList.Columns[2].Visible = false;
            gvOrderList.Columns[13].Visible = false;
            gvOrderList.Columns[14].Visible = false;
            lbMessage.Text = GetResource("lbResult.Text") + listDeliveryStatement.Count.ToString(); //string.Empty;    
        }
    }
    /// <summary>
    /// Event Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btOk_Click(object sender, EventArgs e)
    {        
        bool DepositCompleteFlg = false;
        if(cbxDepositCompleteFlg.Checked)
            DepositCompleteFlg = true;
        List<DeliveryStatementInfo> listDeliveryStatement = DeliveryStatementInfo.GetDeliveryStatement(txtFromDate.Text, txtToDate.Text, txtDeliveryFromDate.Text, txtDeliveryToDate.Text, Convert.ToInt32(HiddenFieldOfficeCd.Value), txtOrderNo.Text, txtClinicName.Text, txtSalesman.Text, DepositCompleteFlg);
        RemoveViewState();
        ViewState["Listsearch"] = listDeliveryStatement;
        FillData(listDeliveryStatement);
        InputEnable(false);    
    }

    private void InputEnable(bool enable)
    {
        txtOrderNo.Enabled = txtDeliveryFromDate.Enabled = txtDeliveryToDate.Enabled = txtFromDate.Enabled = txtToDate.Enabled = txtSalesman.Enabled = dlStaff.Enabled = 
        txtClinicName.Enabled = dlDentalOffice.Enabled =  btOk.Enabled = cbxDepositCompleteFlg.Enabled = enable;
    }   
    /// <summary>
    /// Row create
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvOrderList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell;// = new TableHeaderCell();

            #region "Make Headers"
            //Add CheckBoxSelected

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
            oTableCell.Width = Unit.Pixel(90);
            oGridViewRow.Cells.Add(oTableCell);

            //Add OrderDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOrderDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliverDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDueDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliverDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDeliveredDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentalOfficeCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbClinicName.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(120);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PatientLastNm
            oTableCell = new TableHeaderCell();
            //oTableCell.Text = GetResource("lbPatientLastNm.Text");
            oTableCell.Text = GetResource("lbPatientNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(110);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ProsthesisNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbProsthesisNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(170);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ToothNumber
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbToothNumber.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(60);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliveryType_Insurance 
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDeliveryType_Insurance.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliveryType
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbType.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);
            #endregion
            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    /// <summary>
    /// gvOrderList_PageIndexChanging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckBox();
        gvOrderList.PageIndex = e.NewPageIndex;        
        FillData((List<DeliveryStatementInfo>)ViewState["Listsearch"]);        
        lbMessage.Text = GetResource("lbResult.Text") + ((List<DeliveryStatementInfo>)ViewState["Listsearch"]).Count.ToString();
        RestoreCheckBox();
    }
    /// <summary>
    /// dlDentalOffice_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dlDentalOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dlDentalOffice.SelectedIndex == 0)
        {
            txtClinicName.Text = "";            
            return;
        }
        txtClinicName.Text = dlDentalOffice.SelectedValue.ToString();        
    }    
    /// <summary>
    /// dlStaff_SelectedIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSalesman.Text = dlStaff.SelectedValue;        
        //Get DentalOffice with Staff        
        GetDentalOfficeWithStaff();        
    }    
    /// <summary>
    /// GetDentalOffice With Staff
    /// </summary>
    protected void GetDentalOfficeWithStaff()
    {
        List<MasterDentalOffice> colDentalOfficeMasterList;
        if (string.IsNullOrWhiteSpace(txtSalesman.Text))
            colDentalOfficeMasterList = MasterDentalOffice.GetDentalOfficeMasters(int.Parse(HiddenFieldOfficeCd.Value));
        else
            colDentalOfficeMasterList = MasterDentalOffice.GetDentalOfficeMasterSearch(int.Parse(HiddenFieldOfficeCd.Value), string.Empty, string.Empty, txtSalesman.Text);
        dlDentalOffice.Items.Clear();
        dlDentalOffice.Items.Add("");
        foreach (MasterDentalOffice i in colDentalOfficeMasterList)
        {
            dlDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }
        txtClinicName.Text = "";        
    }
    /// <summary>
    /// btCancel_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btCancel_Click(object sender, EventArgs e)
    {
        txtOrderNo.Text = txtFromDate.Text = txtToDate.Text = txtDeliveryFromDate.Text = txtDeliveryToDate.Text = txtClinicName.Text = txtSalesman.Text = lbMessage.Text = string.Empty;
        dlDentalOffice.SelectedIndex = -1;
        dlStaff.SelectedIndex = -1;
        cbxDepositCompleteFlg.Checked = false;
        InputEnable(true);
        fillClinic();
        List<DeliveryStatementInfo> colOrderList = new List<DeliveryStatementInfo>();
        FillData(colOrderList);
    }    
    /// <summary>
    /// getOfficeName
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    protected string getOfficeName(string code)
    {
        ListItem item = dlDentalOffice.Items.FindByValue(code);
        if (item != null)
            return item.Text;
        return string.Empty;
    }     
    /// <summary>
    /// btPrint_Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btPrint_Click(object sender, EventArgs e)
    {       
        int flag = 0;
        double ToTalPage = 0;
        double PageNumber = 0; 
        double Tax =0;
        DateTime dd;            
        string Sum = "";

        try
        {
            if (string.IsNullOrWhiteSpace(txtIssueDate.Text))
                return;
            else
                dd = DateTime.Parse(txtIssueDate.Text);
            
            List<DeliveryStatementInfo> listAll = new List<DeliveryStatementInfo>();            
            DeliveryStatementInfo colOrderList = new DeliveryStatementInfo();
            List<DeliveryStatementInfo> colOrderListAll = new List<DeliveryStatementInfo>();
            List<DeliveryStatementInfo> colOrderListAllTemp = new List<DeliveryStatementInfo>();                
            
            List<string> listOldTemp = this.HashCheckArray.Values.OfType<string>().ToList();
            for (int i = 0; i < listOldTemp.Count; i++)
            {
                string[] s = listOldTemp[i].Split('_');
                colOrderList = DeliveryStatementInfo.GetOrder(Convert.ToDouble(s[0].ToString()), Convert.ToInt32(s[1].ToString()), Convert.ToInt32(HiddenFieldOfficeCd.Value));                
                colOrderListAllTemp.Add(colOrderList);
            }                
            foreach (GridViewRow row in gvOrderList.Rows)
            {
                CheckBox cb = (CheckBox)(row.FindControl("Check"));
                if (cb.Checked)
                { 
                    colOrderList = DeliveryStatementInfo.GetOrder(Convert.ToDouble(row.Cells[1].Text), Convert.ToInt32(row.Cells[2].Text), Convert.ToInt32(HiddenFieldOfficeCd.Value)); //oController2                   
                    colOrderListAllTemp.Add(colOrderList);
                }
            }

            //int tem = 0;            
            if (colOrderListAllTemp.Count < 1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            #region "Create ListNew"
            colOrderListAllTemp = colOrderListAllTemp.OrderBy(c => c.OrderSeq).ThenBy(c => c.ToothNumber).ThenBy(c => c.ProsthesisCd).ToList();

            var colOrderListAllNewTemp = colOrderListAllTemp.Where(c => string.IsNullOrEmpty(c.ToothNumberStr)).ToList();
            var colOrderListAllNewTemp1 = colOrderListAllTemp.Where(c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 1).OrderByDescending(c => c.ToothNumberStr).ToList();
            var colOrderListAllNewTemp2 = colOrderListAllTemp.Where(c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 2).OrderBy(c => c.ToothNumberStr).ToList();
            var colOrderListAllNewTemp4 = colOrderListAllTemp.Where(c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 4).OrderByDescending(c => c.ToothNumberStr).ToList();
            var colOrderListAllNewTemp3 = colOrderListAllTemp.Where(c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 3).OrderBy(c => c.ToothNumberStr).ToList();

            List<DeliveryStatementInfo> colOrderListAllNew = new List<DeliveryStatementInfo>();
            colOrderListAllNew.AddRange(colOrderListAllNewTemp1);
            colOrderListAllNew.AddRange(colOrderListAllNewTemp2);
            colOrderListAllNew.AddRange(colOrderListAllNewTemp4);
            colOrderListAllNew.AddRange(colOrderListAllNewTemp3);
            colOrderListAllNew.AddRange(colOrderListAllNewTemp);

            List<DeliveryStatementInfo> listNew = new List<DeliveryStatementInfo>();            
            List<DeliveryStatementInfo> listNewTemp = new List<DeliveryStatementInfo>();
            #endregion

            // Event
            GroupPart(colOrderListAllNew, colOrderListAll, listAll, listNew, listNewTemp, cbxPrintPerOrder, Sum, Tax);           

            //Hide panel search
            viewReport.Visible = true;
            viewSearch.Visible = false;

            // Set Page Number
            SetPageNumber(flag, listAll, ToTalPage, PageNumber);            
            // Fill ReportViewer
            FillReportViewer(listAll, Tax, ToTalPage);
            // Split Page
            SplitPage(flag, listAll);
        }
        catch (Exception exc)
        {
            //Module failed to load 
            logger.Error("Error Issue Delivery ", exc);                
            this.btPrint.Attributes.Add("onclick", "this.disabled=false;" + Page.ClientScript.GetPostBackEventReference(btPrint, "").ToString());
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), exc.Message) + "\");");
        }
    }
    /// <summary>
    /// Event Back
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btBack_Click(object sender, EventArgs e)
    {
        viewReport.Visible = false;
        viewSearch.Visible = true;
        InputEnable(true);
        //btOk_Click(sender, e);
        //Create List empty
        DeliveryStatementInfo objOL = new DeliveryStatementInfo();
        objOL.OrderSeq = -1;
        objOL.OrderNo = " ";
        objOL.OrderDate = DateTime.Today;
        objOL.DeliverDate = DateTime.Today;
        objOL.SetDate = DateTime.Today;
        objOL.InsuranceKbn = null;
        objOL.InsuranceKbnReport = "";

        txtOrderNo.Text = txtDeliveryFromDate.Text = txtDeliveryToDate.Text = txtFromDate.Text = txtToDate.Text = txtSalesman.Text = txtClinicName.Text 
        = lbMessage.Text = dlDentalOffice.Text = dlStaff.Text = txtIssueDate.Text = "";
        cbxDepositCompleteFlg.Checked = cbxPrintPerOrder.Checked = false;
        List<DeliveryStatementInfo> colOrderList = new List<DeliveryStatementInfo>();
        colOrderList.Add(objOL);

        gvOrderList.DataSource = colOrderList;
        gvOrderList.DataBind();

        if (colOrderList.Count == 1 && colOrderList[0].OrderSeq == -1)
        {
            gvOrderList.Rows[0].Visible = false;
        }
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

    protected void dlNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvOrderList.PageSize = Convert.ToInt32(dlNumber.SelectedValue);
        if(btOk.Enabled ==false)
            btOk_Click(sender, e);
    }
    
    #region SaveCheckOfOtherPage

    private void RemoveViewState()
    {
        if (ViewState["HashCheckArray"] != null)
        {
            ViewState.Remove("HashCheckArray");
            ViewState.Remove("Listsearch");
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
                    hash.Add((beginItem + msgRow.RowIndex), GetDeliveryStamentInfo(msgRow)); // GetDeliveryStamentInfo(msgRow)); == null;
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

    private string GetDeliveryStamentInfo(GridViewRow msgRow)
    {
        return gvOrderList.Rows[msgRow.RowIndex].Cells[ConstantIndexDelivelyStatement.index_OrderSeq].Text + "_" + gvOrderList.Rows[msgRow.RowIndex].Cells[ConstantIndexDelivelyStatement.index_DetailSeq].Text;
    }

    private void RestoreCheckBox()
    {
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
    #endregion        

    protected string CreateDeliveryStatementStore(string folderStore)
    {
        try
        {            
            //string folderPath = this.PortalSettings.HomeDirectoryMapPath + "DeliveryStatement";      
            string folderPath = Server.MapPath("Portal" + "\\" + "DeliveryStatement");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            else
            {
                try
                {
                    List<FileInfo> ListFiles = new System.IO.DirectoryInfo(folderPath).GetFiles().Where(p => p.CreationTime.AddMinutes(10) <= DateTime.Now).ToList();                        
                    foreach (FileInfo i in ListFiles)
                        i.Delete();

                    List<DirectoryInfo> ListFolder = new System.IO.DirectoryInfo(folderPath).GetDirectories().Where(p => p.CreationTime.AddMinutes(10) <= DateTime.Now).ToList();                        
                    foreach (DirectoryInfo i in ListFolder)
                        i.Delete(true);
                }
                catch { }
            }
            string folderName = Path.Combine(folderPath, folderStore);
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);                
            return folderName;
        }
        catch (Exception ex)
        {
            logger.Error("Error CreateDeliveryStatementStore = " + ex.Message);
            return "";
        } 
    }

    protected string SaveDeliveryStatementFile(string zipFolderDelivery)
    {
        try
        {
            //string folderPath = this.PortalSettings.HomeDirectoryMapPath + "DeliveryStatement";
            string folderPath = Server.MapPath("Portal" + "\\" + "DeliveryStatement");
            string folderName = Path.Combine(folderPath, zipFolderDelivery);
            if (!Directory.Exists(folderName))
            {
                return "";
            }
            //zip folderName                 
            CreateZipFile(folderName, folderPath + "\\" + zipFolderDelivery + ".zip");
            //return this.PortalSettings.HomeDirectory + "DeliveryStatement/" + zipFolderDelivery + ".zip"; 
            
            return folderPath + "\\" + zipFolderDelivery + ".zip"; 
        }
        catch (Exception ex)
        {
            logger.Error("Error SaveDeliveryStatementFile = " + ex.Message);
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
            zip.TempFileFolder = Server.MapPath("");//this.PortalSettings.HomeDirectoryMapPath;
            zip.StatusMessageTextWriter = System.Console.Out;
            zip.AddDirectory(_folderZip); // recurses subdirectories
            zip.Save(_ZipFile);
        }
    }

    private string GetDentalFont()
    {
        string fileDentalFont = Server.MapPath("~/cpeu.zip");//this.PortalSettings.HomeDirectory + "cpeu.zip";        
        return fileDentalFont;
    }   

    protected void GroupPart(List<DeliveryStatementInfo> colOrderListAllNew, List<DeliveryStatementInfo> colOrderListAll, List<DeliveryStatementInfo> listAll, List<DeliveryStatementInfo> listNew, List<DeliveryStatementInfo> listNewTemp, CheckBox cbxPrintPerOrder, string Sum, double Tax)
    {        
        // Group Parts   
        if(cbxPrintPerOrder.Checked)         
            colOrderListAllNew = colOrderListAllNew.OrderBy(c => c.OrderSeq).ToList();
        else
            colOrderListAllNew = colOrderListAllNew.OrderBy(c=>c.DentalOfficeCd).ThenBy(c => c.OrderSeq).ToList();
        // "Event Gruop"
        for (int ii = 0; ii < colOrderListAllNew.Count; )
        {
            var temlist = new List<DeliveryStatementInfo>();
            if(cbxPrintPerOrder.Checked)
            temlist = colOrderListAllNew.Where(c => (c.OrderSeq == colOrderListAllNew[ii].OrderSeq)).ToList();
            else
                temlist = colOrderListAllNew.Where(c => (c.DentalOfficeCd == colOrderListAllNew[ii].DentalOfficeCd && c.OrderSeq == colOrderListAllNew[ii].OrderSeq)).ToList();

            var temlist1 = temlist.Where((c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 1)).ToList();
            var temlist2 = temlist.Where((c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 2)).ToList();
            var temlist4 = temlist.Where((c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 4)).ToList();
            var temlist3 = temlist.Where((c => !string.IsNullOrEmpty(c.ToothNumberStr) && Convert.ToInt32(c.ToothNumberStr) / 10 == 3)).ToList();
            var temlist0 = temlist.Where((c => string.IsNullOrEmpty(c.ToothNumberStr))).ToList();
            ii += temlist.Count;

            if (temlist0.Count >= 1)
            {
                SetValue(temlist0, Sum, Tax);
            }
            if (temlist1.Count + temlist0.Count == temlist.Count)// only Tooth 18-11 
                SetToothRoot_1_4(temlist1, Sum, Tax);

            else if (temlist4.Count + temlist0.Count == temlist.Count) // only Tooth 48-41
                SetToothRoot_1_4(temlist4, Sum, Tax);

            else if (temlist2.Count + temlist0.Count == temlist.Count)// only Tooth 21-28                    
                SetToothRoot_2_3(temlist2, Sum, Tax);

            else if (temlist3.Count + temlist0.Count == temlist.Count) // only Tooth 31-38
                SetToothRoot_2_3(temlist3, Sum, Tax);
            else
            {
                if (temlist1.Count >= 1 && temlist2.Count >= 1 && temlist3.Count >= 1 && temlist4.Count >= 1) //all 1,2,3,4
                {
                    SetToothRoot_1_4N(temlist1, Sum, Tax);
                    SetToothRoot_2_3N(temlist2, Sum, Tax);
                    SetToothRoot_2_3N(temlist3, Sum, Tax);
                    SetToothRoot_1_4N(temlist4, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count >= 1 && temlist3.Count < 1 && temlist4.Count >= 1) // 1,2,4
                {
                    SetToothRoot_1_4N(temlist1, Sum, Tax);
                    SetToothRoot_2_3N(temlist2, Sum, Tax);
                    SetToothRoot_1_4(temlist4, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count >= 1 && temlist3.Count >= 1 && temlist4.Count < 1)//1,2,3
                {
                    SetToothRoot_1_4N(temlist1, Sum, Tax);
                    SetToothRoot_2_3N(temlist2, Sum, Tax);
                    SetToothRoot_2_3(temlist3, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count < 1 && temlist3.Count >= 1 && temlist4.Count < 1) // 1, 3
                {
                    SetToothRoot_1_4(temlist1, Sum, Tax);
                    SetToothRoot_2_3(temlist3, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count < 1 && temlist3.Count < 1 && temlist4.Count >= 1) // 1, 4
                {
                    SetToothRoot_1_4(temlist1, Sum, Tax);
                    SetToothRoot_1_4(temlist4, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count >= 1 && temlist3.Count < 1 && temlist4.Count < 1) // 1, 2
                {
                    SetToothRoot_1_4N(temlist1, Sum, Tax);
                    SetToothRoot_2_3N(temlist2, Sum, Tax);
                }
                else if (temlist1.Count >= 1 && temlist2.Count < 1 && temlist3.Count >= 1 && temlist4.Count >= 1) // 1, 3, 4
                {
                    SetToothRoot_1_4(temlist1, Sum, Tax);
                    SetToothRoot_2_3N(temlist3, Sum, Tax);
                    SetToothRoot_1_4N(temlist4, Sum, Tax);
                }
                else if (temlist1.Count < 1 && temlist2.Count >= 1 && temlist3.Count >= 1 && temlist4.Count >= 1) // 2, 3, 4
                {
                    SetToothRoot_2_3(temlist2, Sum, Tax);
                    SetToothRoot_2_3N(temlist3, Sum, Tax);
                    SetToothRoot_1_4N(temlist4, Sum, Tax);
                }
                else if (temlist1.Count < 1 && temlist2.Count >= 1 && temlist3.Count >= 1 && temlist4.Count < 1) // 2, 3
                {
                    SetToothRoot_2_3(temlist2, Sum, Tax);
                    SetToothRoot_2_3(temlist3, Sum, Tax);
                }
                else if (temlist1.Count < 1 && temlist2.Count >= 1 && temlist3.Count < 1 && temlist4.Count >= 1) // 2, 4
                {
                    SetToothRoot_2_3(temlist2, Sum, Tax);
                    SetToothRoot_1_4(temlist4, Sum, Tax);
                }
                else if (temlist1.Count < 1 && temlist2.Count < 1 && temlist3.Count >= 1 && temlist4.Count >= 1) // 3, 4
                {
                    SetToothRoot_2_3N(temlist3, Sum, Tax);
                    SetToothRoot_1_4N(temlist4, Sum, Tax);
                }
            }
            colOrderListAll = new List<DeliveryStatementInfo>();
            colOrderListAll.AddRange(temlist1);
            colOrderListAll.AddRange(temlist2);
            colOrderListAll.AddRange(temlist4);
            colOrderListAll.AddRange(temlist3);
            colOrderListAll.AddRange(temlist0);
            // Set DeliveryStatementNo 
            SetDeliveryStatementNo(colOrderListAll);
            // Get GroupToothNumber
            GetGroupToothNumber(colOrderListAll);
            // add list Material
            AddListMaterial(colOrderListAll, listNewTemp);
            // Group Prosthesis
            colOrderListAll = GroupProsthesis(colOrderListAll, cbxPrintPerOrder);
            listNew.AddRange(colOrderListAll);
        }

        // Group Material
        listNewTemp = GroupMaterial(listNewTemp, cbxPrintPerOrder);
        listNew.AddRange(listNewTemp);
        // Add reow emptry
        if (cbxPrintPerOrder.Checked)
            AddRowEmptyOrderSeq(listNew, listAll);
        else
            AddRowEmptyDentalOffice(listNew, listAll);
    }

    protected void GetGroupToothNumber(List<DeliveryStatementInfo> listDetail)
    {
        try
        {
            int firsti = 0;
            int div10f = 0;
            int div10i = 0;
            int div10j = 0;
            int mod10f = 0;
            int mod10i = 0;
            int mod10j = 0;
            int count = 0;
            int count1 = 0;
            if (listDetail.Count > 1)
            {
                for (int i = 1; i < listDetail.Count; i++)
                {
                    if (listDetail[i] != null)
                    {
                        if (listDetail[firsti].OrderSeq == listDetail[i].OrderSeq)
                        {
                            if (!string.IsNullOrEmpty(listDetail[firsti].ToothNumberStr) && !string.IsNullOrEmpty(listDetail[i].ToothNumberStr))
                            {
                                div10f = Convert.ToInt32(listDetail[firsti].ToothNumberStr) / 10;
                                mod10f = Convert.ToInt32(listDetail[firsti].ToothNumberStr) % 10;
                                div10i = Convert.ToInt32(listDetail[i].ToothNumberStr) / 10;
                                mod10i = Convert.ToInt32(listDetail[i].ToothNumberStr) % 10;
                                div10j = Convert.ToInt32(listDetail[i - 1].ToothNumberStr) / 10;
                                mod10j = Convert.ToInt32(listDetail[i - 1].ToothNumberStr) % 10;                                

                                if ((div10f == 2 || div10f == 3) && div10f == div10i && (mod10j + 1 == mod10i || mod10j == mod10i))
                                    listDetail[firsti].ToothNumberReport += listDetail[i].ToothNumberReport;

                                else if ((div10f == 2 || div10f == 3) && div10f == div10i && mod10j + 2 == mod10i)
                                {
                                    if (div10f == 2)
                                        listDetail[firsti].ToothNumberReport += GetResource("DownNotSquential.Text") + listDetail[i].ToothNumberReport;
                                    if (div10f == 3)
                                        listDetail[firsti].ToothNumberReport += GetResource("UPNotSquential.Text") + listDetail[i].ToothNumberReport;
                                }

                                else if ((div10f == 2 || div10f == 3) && div10f == div10i && mod10j + 2 < mod10i)
                                {
                                    if (div10f == 2)
                                        listDetail[firsti].ToothNumberReport += "＿" + listDetail[i].ToothNumberReport;
                                    if (div10f == 3)
                                        listDetail[firsti].ToothNumberReport += "￣" + listDetail[i].ToothNumberReport;
                                }

                                else if ((div10f == 1 || div10f == 4) && div10f == div10i && (mod10j - 1 == mod10i || mod10j == mod10i))
                                    listDetail[firsti].ToothNumberReport += listDetail[i].ToothNumberReport;

                                else if ((div10f == 1 || div10f == 4) && div10f == div10i && mod10j - 2 == mod10i)
                                {
                                    if (div10f == 1)
                                        listDetail[firsti].ToothNumberReport += GetResource("DownNotSquential.Text") + listDetail[i].ToothNumberReport;
                                    if (div10f == 4)
                                        listDetail[firsti].ToothNumberReport += GetResource("UPNotSquential.Text") + listDetail[i].ToothNumberReport;
                                }

                                else if ((div10f == 1 || div10f == 4) && div10f == div10i && mod10j - 2 > mod10i)
                                {
                                    if (div10f == 1)
                                        listDetail[firsti].ToothNumberReport += "＿" + listDetail[i].ToothNumberReport;
                                    if (div10f == 4)
                                        listDetail[firsti].ToothNumberReport += "￣" + listDetail[i].ToothNumberReport;
                                }

                                else if (div10f == 1 && div10i == 2)
                                {
                                    count++;
                                    if (count < 2)
                                    {                                        
                                        listDetail[firsti].ToothNumberReport += GetResource("DifferentToothNumber1_2.Text") + listDetail[i].ToothNumberReport;
                                        listDetail[i].ToothNumberReport = listDetail[firsti].ToothNumberReport;
                                    }
                                    else
                                    {
                                        if (div10j == 2 && (mod10j + 1 == mod10i || mod10j == mod10i))
                                            listDetail[firsti].ToothNumberReport += listDetail[i].ToothNumberReport;
                                        if (div10j == 2 && mod10j + 2 == mod10i)
                                            listDetail[firsti].ToothNumberReport += GetResource("DownNotSquential.Text") + listDetail[i].ToothNumberReport;
                                        if (div10j == 2 && mod10j + 2 < mod10i)
                                            listDetail[firsti].ToothNumberReport += "＿" + listDetail[i].ToothNumberReport;
                                    }
                                }
                                else if (div10f == 4 && div10i == 3)
                                {
                                    count1++;
                                    if (count1 < 2)
                                    {                                        
                                        listDetail[firsti].ToothNumberReport += GetResource("DifferentToothNumber3_4.Text") + listDetail[i].ToothNumberReport;
                                        listDetail[i].ToothNumberReport = listDetail[firsti].ToothNumberReport;
                                    }
                                    else
                                    {
                                        if (div10j == 3 && (mod10j + 1 == mod10i || mod10j == mod10i))
                                            listDetail[firsti].ToothNumberReport += listDetail[i].ToothNumberReport;
                                        if (div10j == 3 && mod10j + 2 == mod10i)
                                            listDetail[firsti].ToothNumberReport += GetResource("UPNotSquential.Text") + listDetail[i].ToothNumberReport;
                                        if (div10j == 3 && mod10j + 2 < mod10i)
                                            listDetail[firsti].ToothNumberReport += "￣" + listDetail[i].ToothNumberReport;
                                    }
                                }
                                else if ((div10f == 1 || div10f == 2) && (div10i == 3 || div10i == 4))
                                {
                                    firsti = i;
                                }
                            }
                            else
                                listDetail[firsti].ToothNumberReport += listDetail[i].ToothNumberReport;

                            if (firsti != i)
                            {
                                listDetail[i].ToothNumberReport = listDetail[firsti].ToothNumberReport;
                                listDetail[i].OrderSeq = listDetail[firsti].OrderSeq;
                                listDetail[i].PatientFirstNm = listDetail[firsti].PatientFirstNm;
                                listDetail[i].PatientLastNm = listDetail[firsti].PatientLastNm;
                                listDetail[i].Area = listDetail[firsti].Area;
                                listDetail[i - 1].ToothNumberReport = listDetail[i].ToothNumberReport;
                                listDetail[i - 1].OrderSeq = listDetail[i].OrderSeq;
                                listDetail[i - 1].PatientFirstNm = listDetail[i].PatientFirstNm;
                                listDetail[i - 1].PatientLastNm = listDetail[i].PatientLastNm;
                                listDetail[i - 1].Area = listDetail[i].Area;
                            }
                        }
                        else
                            firsti = i;
                    }
                }
            }
        }
        catch (Exception exc)
        {
            logger.Error("Grounp tooth", exc);
        }
    }
   
    protected void SetToothRoot_1_4(List<DeliveryStatementInfo> temlist,string Sum, double Tax)
    {
        int div10 = 0;
        int mod10 = 0;
        string OrderDate = "";
        #region Set Font    
        for (int i = 0; i < temlist.Count; i++)
        {
            string resourceName = "";
            string resource = "";
            string resourcegap = "";
            div10 = Convert.ToInt32(temlist[i].ToothNumberStr) / 10;
            mod10 = Convert.ToInt32(temlist[i].ToothNumberStr) % 10;            

            if (div10 == 1 || div10 == 4)
                resourceName += "Right";
            if (div10 == 1)
            {
                resourceName += "Down";
                resource += "Down";
                resourcegap = "Down";
            }
            if (div10 == 4)
            {
                resourceName += "Up";
                resource += "Up";
                resourcegap = "Up";
            }
            if (temlist[i].ChildFlg == true && temlist[i].GapFlg != true && temlist[i].StumpFlg != "True")
            {
                if (mod10 == 1)
                {
                    resourceName += "A";
                    resource += "A";
                }
                if (mod10 == 2)
                {
                    resourceName += "B";
                    resource += "B";
                }
                if (mod10 == 3)
                {
                    resourceName += "C";
                    resource += "C";
                }
                if (mod10 == 4)
                {
                    resourceName += "D";
                    resource += "D";
                }
                if (mod10 == 5)
                {
                    resourceName += "E";
                    resource += "E";
                }
            }
            if (temlist[i].GapFlg == true)
                temlist[i].ToothNumberReport = GetResource("GapFlg" + resourcegap + ".Text");
            else if(i != temlist.Count -1)
            {
                resourcegap = "";
                if (temlist[i].StumpFlg == "True" && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + "StumpFlg" + mod10 + ".Text"); 
               
                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + ".Text");
                
                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 1 || div10 == 4))
                {
                    resource += mod10;
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + ".Text");
                }                
            }
            else
            {
                resourcegap = "";
                if (temlist[i].StumpFlg == "True" && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resourceName + "StumpFlg" + mod10 + ".Text");                

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resourceName + ".Text");
                
                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 1 || div10 == 4))
                {
                    resourceName += mod10;
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resourceName + ".Text");
                }                
            }

            #region Set Value
            if (temlist[i].ProcessPrice == null)
                temlist[i].ProcessPrice = 0;
            if (temlist[i].MaterialPrice == null)
                temlist[i].MaterialPrice = 0;
            if (temlist[i].Amount == null)
                temlist[i].Amount = 0;

            temlist[i].DeliverDate = Convert.ToDateTime(txtIssueDate.Text);
            //StaffController objStaffCon = new StaffController();
            MasterStaff objStaffInfo = new MasterStaff();
            if (temlist[i].StaffCd != null && temlist[i].StaffCd != -1)
            {
                objStaffInfo = MasterStaff.GetStaff(Convert.ToInt32(temlist[i].StaffCd));
                if (objStaffInfo != null)
                    temlist[i].StaffNm = objStaffInfo.StaffNm;
                else
                    temlist[i].StaffNm = "";
            }
            else
                temlist[i].StaffNm = "";

            MasterDentalOffice objdentalofficeinfo = MasterDentalOffice.GetDentalOfficeMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(temlist[i].DentalOfficeCd));
            temlist[i].DentalOfficeNm = objdentalofficeinfo.DentalOfficeNm;

            if (temlist[i].TrialOrderFlg.Value)
                temlist[i].Area = GetResource("Trial.Text");
            else if (temlist[i].RemanufactureFlg.Value)
                temlist[i].Area = GetResource("Remake.Text");
            else
                temlist[i].Area = GetResource("Finish.Text");

            if (temlist[i].InsuranceKbn.Value.ToString() == "True" || temlist[i].InsuranceKbn.Value.ToString() == "1")
                temlist[i].InsuranceKbnReport = GetResource("Insurance.Text");
            else if (temlist[i].InsuranceKbn.Value.ToString() == "False" || temlist[i].InsuranceKbn.Value.ToString() == "0")
                temlist[i].InsuranceKbnReport = GetResource("UnInsured.Text");
            else
                temlist[i].InsuranceKbnReport = null;

            if (temlist[i].Price == null)
                temlist[i].Price = 0;

            if (temlist[i].Amount.Value > 0 && temlist[i].Price.Value > 0)
                Sum = (Convert.ToDouble(temlist[i].Amount.Value) * Convert.ToDouble(temlist[i].Price.Value)).ToString();
            else Sum = "";

            OrderDate = temlist[i].OrderDate.ToShortDateString();
            //TaxController objTaxCon = new TaxController();
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo = MasterTax.SearchTax(Convert.ToDateTime(OrderDate));

            Tax = temlist[i].MaterialPrice.Value + temlist[i].ProcessPrice.Value;
            if (objTaxInfo != null)
            {
                if (objTaxInfo.RoundFraction == 2)
                    temlist[i].TaxRate = Math.Floor(objTaxInfo.TaxRate * Tax);
                else if (objTaxInfo.RoundFraction == 3)
                    temlist[i].TaxRate = Math.Ceiling(objTaxInfo.TaxRate * Tax);
                else
                    temlist[i].TaxRate = (long)(Math.Floor(objTaxInfo.TaxRate * Tax + 0.5));
            }
            #endregion
        }        
        #endregion        
    }

    protected void SetToothRoot_1_4N(List<DeliveryStatementInfo> temlist, string Sum, double Tax)
    {
        int div10 = 0;
        int mod10 = 0;  
        string OrderDate = "";        
        #region Set Font
        for (int i = 0; i < temlist.Count; i++)
        {
            string resourceName = "";
            string resource = "";
            string resourcegap = "";

            div10 = Convert.ToInt32(temlist[i].ToothNumberStr) / 10;
            mod10 = Convert.ToInt32(temlist[i].ToothNumberStr) % 10;
            if (div10 == 1 || div10 == 4)
                resourceName += "Right";
            if (div10 == 1)
            {
                resourceName += "Down";
                resource += "Down";
                resourcegap = "Down";
            }
            if (div10 == 4)
            {
                resourceName += "Up";
                resource += "Up";
                resourcegap = "Up";
            }
            if (temlist[i].ChildFlg == true && temlist[i].GapFlg != true && temlist[i].StumpFlg != "True")
            {
                if (mod10 == 1)
                {
                    resourceName += "A";
                    resource += "A";
                }
                if (mod10 == 2)
                {
                    resourceName += "B";
                    resource += "B";
                }
                if (mod10 == 3)
                {
                    resourceName += "C";
                    resource += "C";
                }
                if (mod10 == 4)
                {
                    resourceName += "D";
                    resource += "D";
                }
                if (mod10 == 5)
                {
                    resourceName += "E";
                    resource += "E";
                }
            }
            if (temlist[i].GapFlg == true)
                temlist[i].ToothNumberReport = GetResource("GapFlg" + resourcegap + ".Text");
            else
            {
                resourcegap = "";
                if (temlist[i].StumpFlg == "True" && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + "StumpFlg" + mod10 + ".Text");

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 1 || div10 == 4))
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + ".Text");

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 1 || div10 == 4))
                {
                    resource += mod10;
                    temlist[i].ToothNumberReport = resourcegap + GetResource(resource + ".Text");
                }
            }            
            #region Set Value
            if (temlist[i].ProcessPrice == null)
                temlist[i].ProcessPrice = 0;
            if (temlist[i].MaterialPrice == null)
                temlist[i].MaterialPrice = 0;
            if (temlist[i].Amount == null)
                temlist[i].Amount = 0;

            temlist[i].DeliverDate = Convert.ToDateTime(txtIssueDate.Text);
            //StaffController objStaffCon = new StaffController();
            MasterStaff objStaffInfo = new MasterStaff();
            if (temlist[i].StaffCd != null && temlist[i].StaffCd != -1)
            {
                objStaffInfo = MasterStaff.GetStaff(Convert.ToInt32(temlist[i].StaffCd));
                if (objStaffInfo != null)
                    temlist[i].StaffNm = objStaffInfo.StaffNm;
                else
                    temlist[i].StaffNm = "";
            }
            else
                temlist[i].StaffNm = "";

            MasterDentalOffice objdentalofficeinfo = MasterDentalOffice.GetDentalOfficeMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(temlist[i].DentalOfficeCd));
            temlist[i].DentalOfficeNm = objdentalofficeinfo.DentalOfficeNm;

            if (temlist[i].TrialOrderFlg.Value)
                temlist[i].Area = GetResource("Trial.Text");
            else if (temlist[i].RemanufactureFlg.Value)
                temlist[i].Area = GetResource("Remake.Text");
            else
                temlist[i].Area = GetResource("Finish.Text");

            if (temlist[i].InsuranceKbn.Value.ToString() == "True" || temlist[i].InsuranceKbn.Value.ToString() == "1")
                temlist[i].InsuranceKbnReport = GetResource("Insurance.Text");
            else if (temlist[i].InsuranceKbn.Value.ToString() == "False" || temlist[i].InsuranceKbn.Value.ToString() == "0")
                temlist[i].InsuranceKbnReport = GetResource("UnInsured.Text");
            else
                temlist[i].InsuranceKbnReport = null;

            if (temlist[i].Price == null)
                temlist[i].Price = 0;

            if (temlist[i].Amount.Value > 0 && temlist[i].Price.Value > 0)
                Sum = (Convert.ToDouble(temlist[i].Amount.Value) * Convert.ToDouble(temlist[i].Price.Value)).ToString();
            else Sum = "";

            OrderDate = temlist[i].OrderDate.ToShortDateString();
            //TaxController objTaxCon = new TaxController();
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo = MasterTax.SearchTax(Convert.ToDateTime(OrderDate));

            Tax = temlist[i].MaterialPrice.Value + temlist[i].ProcessPrice.Value;
            if (objTaxInfo != null)
            {
                if (objTaxInfo.RoundFraction == 2)
                    temlist[i].TaxRate = Math.Floor(objTaxInfo.TaxRate * Tax);
                else if (objTaxInfo.RoundFraction == 3)
                    temlist[i].TaxRate = Math.Ceiling(objTaxInfo.TaxRate * Tax);
                else
                    temlist[i].TaxRate = (long)(Math.Floor(objTaxInfo.TaxRate * Tax + 0.5));
            }
            #endregion
        }
        #endregion
    }

    protected void SetToothRoot_2_3(List<DeliveryStatementInfo> temlist, string Sum, double Tax)
    {
        int div10 = 0;
        int mod10 = 0; 
        string OrderDate = "";
        #region Set Font
        for (int i = 0; i < temlist.Count; i++)
        {
            string resourceName = "";
            string resource = "";
            string resourcegap = "";
            div10 = Convert.ToInt32(temlist[i].ToothNumberStr) / 10;
            mod10 = Convert.ToInt32(temlist[i].ToothNumberStr) % 10;
            
            if (div10 == 2 || div10 == 3)
                resourceName += "Left";            
            if (div10 == 2)
            {
                resourceName += "Down";
                resource += "Down";
                resourcegap = "Down";
            }
            if (div10 == 3)
            {
                resourceName += "Up";
                resource += "Up";
                resourcegap = "Up";
            }

            if (temlist[i].ChildFlg == true && temlist[i].GapFlg != true && temlist[i].StumpFlg != "True")
            {
                if (mod10 == 1)
                {
                    resourceName += "A";
                    resource += "A";
                }
                if (mod10 == 2)
                {
                    resourceName += "B";
                    resource += "B";
                }
                if (mod10 == 3)
                {
                    resourceName += "C";
                    resource += "C";
                }
                if (mod10 == 4)
                {
                    resourceName += "D";
                    resource += "D";
                }
                if (mod10 == 5)
                {
                    resourceName += "E";
                    resource += "E";
                }
            }
            if (temlist[i].GapFlg == true)
                temlist[i].ToothNumberReport = GetResource("GapFlg" + resourcegap + ".Text");
            else if (i != 0)
            {
                resourcegap = "";
                if (temlist[i].StumpFlg == "True" && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resource + "StumpFlg" + mod10 + ".Text") + resourcegap;
              
                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resource + ".Text") + resourcegap;
               
                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 2 || div10 == 3))
                {
                    resource += mod10;
                    temlist[i].ToothNumberReport = GetResource(resource + ".Text") + resourcegap;
                }
            }
            else
            {
                resourcegap = "";                
                if (temlist[i].StumpFlg == "True" && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resourceName + "StumpFlg" + mod10 + ".Text") + resourcegap;

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resourceName + ".Text") + resourcegap;

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 2 || div10 == 3))
                {
                    resourceName += mod10;
                    temlist[i].ToothNumberReport = GetResource(resourceName + ".Text") + resourcegap;
                }
            }
            #region Set Value
            if (temlist[i].ProcessPrice == null)
                temlist[i].ProcessPrice = 0;
            if (temlist[i].MaterialPrice == null)
                temlist[i].MaterialPrice = 0;
            if (temlist[i].Amount == null)
                temlist[i].Amount = 0;

            temlist[i].DeliverDate = Convert.ToDateTime(txtIssueDate.Text);
            //StaffController objStaffCon = new StaffController();
            MasterStaff objStaffInfo = new MasterStaff();
            if (temlist[i].StaffCd != null && temlist[i].StaffCd != -1)
            {
                objStaffInfo = MasterStaff.GetStaff(Convert.ToInt32(temlist[i].StaffCd));
                if (objStaffInfo != null)
                    temlist[i].StaffNm = objStaffInfo.StaffNm;
                else
                    temlist[i].StaffNm = "";
            }
            else
                temlist[i].StaffNm = "";

            MasterDentalOffice objdentalofficeinfo = MasterDentalOffice.GetDentalOfficeMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(temlist[i].DentalOfficeCd));
            temlist[i].DentalOfficeNm = objdentalofficeinfo.DentalOfficeNm;

            if (temlist[i].TrialOrderFlg.Value)
                temlist[i].Area = GetResource("Trial.Text");
            else if (temlist[i].RemanufactureFlg.Value)
                temlist[i].Area = GetResource("Remake.Text");
            else
                temlist[i].Area = GetResource("Finish.Text");

            if (temlist[i].InsuranceKbn.Value.ToString() == "True" || temlist[i].InsuranceKbn.Value.ToString() == "1")
                temlist[i].InsuranceKbnReport = GetResource("Insurance.Text");
            else if (temlist[i].InsuranceKbn.Value.ToString() == "False" || temlist[i].InsuranceKbn.Value.ToString() == "0")
                temlist[i].InsuranceKbnReport = GetResource("UnInsured.Text");
            else
                temlist[i].InsuranceKbnReport = null;

            if (temlist[i].Price == null)
                temlist[i].Price = 0;

            if (temlist[i].Amount.Value > 0 && temlist[i].Price.Value > 0)
                Sum = (Convert.ToDouble(temlist[i].Amount.Value) * Convert.ToDouble(temlist[i].Price.Value)).ToString();
            else Sum = "";

            OrderDate = temlist[i].OrderDate.ToShortDateString();
            //TaxController objTaxCon = new TaxController();
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo = MasterTax.SearchTax(Convert.ToDateTime(OrderDate));

            Tax = temlist[i].MaterialPrice.Value + temlist[i].ProcessPrice.Value;
            if (objTaxInfo != null)
            {
                if (objTaxInfo.RoundFraction == 2)
                    temlist[i].TaxRate = Math.Floor(objTaxInfo.TaxRate * Tax);
                else if (objTaxInfo.RoundFraction == 3)
                    temlist[i].TaxRate = Math.Ceiling(objTaxInfo.TaxRate * Tax);
                else
                    temlist[i].TaxRate = (long)(Math.Floor(objTaxInfo.TaxRate * Tax + 0.5));
            }
            #endregion
        }
        #endregion
    }

    protected void SetToothRoot_2_3N(List<DeliveryStatementInfo> temlist, string Sum, double Tax)
    {
        int div10 = 0;
        int mod10 = 0; 
        string OrderDate = "";       
        #region Set Font
        for (int i = 0; i < temlist.Count; i++)
        {
            string resourceName = "";
            string resource = "";
            string resourcegap = "";
            div10 = Convert.ToInt32(temlist[i].ToothNumberStr) / 10;
            mod10 = Convert.ToInt32(temlist[i].ToothNumberStr) % 10;

            if (div10 == 2 || div10 == 3)
                resourceName += "Left";
            if (div10 == 2)
            {
                resourceName += "Down";
                resource += "Down";
                resourcegap = "Down";
            }
            if (div10 == 3)
            {
                resourceName += "Up";
                resource += "Up";
                resourcegap = "Up";
            }

            if (temlist[i].ChildFlg == true && temlist[i].GapFlg != true && temlist[i].StumpFlg != "True")
            {
                if (mod10 == 1)
                {
                    resourceName += "A";
                    resource += "A";
                }
                if (mod10 == 2)
                {
                    resourceName += "B";
                    resource += "B";
                }
                if (mod10 == 3)
                {
                    resourceName += "C";
                    resource += "C";
                }
                if (mod10 == 4)
                {
                    resourceName += "D";
                    resource += "D";
                }
                if (mod10 == 5)
                {
                    resourceName += "E";
                    resource += "E";
                }
            }
            if (temlist[i].GapFlg == true)
                temlist[i].ToothNumberReport = GetResource("GapFlg" + resourcegap + ".Text");
            else 
            {
                resourcegap = "";
                if (temlist[i].StumpFlg == "True" && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resource + "StumpFlg" + mod10 + ".Text") + resourcegap;

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg == true && (div10 == 2 || div10 == 3))
                    temlist[i].ToothNumberReport = GetResource(resource + ".Text") + resourcegap;

                if (temlist[i].StumpFlg != "True" && temlist[i].ChildFlg != true && (div10 == 2 || div10 == 3))
                {
                    resource += mod10;
                    temlist[i].ToothNumberReport = GetResource(resource + ".Text") + resourcegap;
                }
            }            
            #region Set Value
            if (temlist[i].ProcessPrice == null)
                temlist[i].ProcessPrice = 0;
            if (temlist[i].MaterialPrice == null)
                temlist[i].MaterialPrice = 0;
            if (temlist[i].Amount == null)
                temlist[i].Amount = 0;

            temlist[i].DeliverDate = Convert.ToDateTime(txtIssueDate.Text);
            //StaffController objStaffCon = new StaffController();
            MasterStaff objStaffInfo = new MasterStaff();
            if (temlist[i].StaffCd != null && temlist[i].StaffCd != -1)
            {
                objStaffInfo = MasterStaff.GetStaff(Convert.ToInt32(temlist[i].StaffCd));
                if (objStaffInfo != null)
                    temlist[i].StaffNm = objStaffInfo.StaffNm;
                else
                    temlist[i].StaffNm = "";
            }
            else
                temlist[i].StaffNm = "";

            MasterDentalOffice objdentalofficeinfo = MasterDentalOffice.GetDentalOfficeMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(temlist[i].DentalOfficeCd));
            temlist[i].DentalOfficeNm = objdentalofficeinfo.DentalOfficeNm;

            if (temlist[i].TrialOrderFlg.Value)
                temlist[i].Area = GetResource("Trial.Text");
            else if (temlist[i].RemanufactureFlg.Value)
                temlist[i].Area = GetResource("Remake.Text");
            else
                temlist[i].Area = GetResource("Finish.Text");

            if (temlist[i].InsuranceKbn.Value.ToString() == "True" || temlist[i].InsuranceKbn.Value.ToString() == "1")
                temlist[i].InsuranceKbnReport = GetResource("Insurance.Text");
            else if (temlist[i].InsuranceKbn.Value.ToString() == "False" || temlist[i].InsuranceKbn.Value.ToString() == "0")
                temlist[i].InsuranceKbnReport = GetResource("UnInsured.Text");
            else
                temlist[i].InsuranceKbnReport = null;

            if (temlist[i].Price == null)
                temlist[i].Price = 0;

            if (temlist[i].Amount.Value > 0 && temlist[i].Price.Value > 0)
                Sum = (Convert.ToDouble(temlist[i].Amount.Value) * Convert.ToDouble(temlist[i].Price.Value)).ToString();
            else Sum = "";

            OrderDate = temlist[i].OrderDate.ToShortDateString();
            //TaxController objTaxCon = new TaxController();
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo = MasterTax.SearchTax(Convert.ToDateTime(OrderDate));

            Tax = temlist[i].MaterialPrice.Value + temlist[i].ProcessPrice.Value;
            if (objTaxInfo != null)
            {
                if (objTaxInfo.RoundFraction == 2)
                    temlist[i].TaxRate = Math.Floor(objTaxInfo.TaxRate * Tax);
                else if (objTaxInfo.RoundFraction == 3)
                    temlist[i].TaxRate = Math.Ceiling(objTaxInfo.TaxRate * Tax);
                else
                    temlist[i].TaxRate = (long)(Math.Floor(objTaxInfo.TaxRate * Tax + 0.5));
            }
            #endregion
        }
        #endregion
    }

    protected void SetValue(List<DeliveryStatementInfo> temlist, string Sum, double Tax)
    {        
        string OrderDate = "";        
        for (int i = 0; i < temlist.Count; i++)
        {
            #region Set Value
            if (temlist[i].ProcessPrice == null)
                temlist[i].ProcessPrice = 0;
            if (temlist[i].MaterialPrice == null)
                temlist[i].MaterialPrice = 0;
            if (temlist[i].Amount == null)
                temlist[i].Amount = 0;

            temlist[i].DeliverDate = Convert.ToDateTime(txtIssueDate.Text);
            //StaffController objStaffCon = new StaffController();
            MasterStaff objStaffInfo = new MasterStaff();
            if (temlist[i].StaffCd != null && temlist[i].StaffCd != -1)
            {
                objStaffInfo = MasterStaff.GetStaff(Convert.ToInt32(temlist[i].StaffCd));
                if (objStaffInfo != null)
                    temlist[i].StaffNm = objStaffInfo.StaffNm;
                else
                    temlist[i].StaffNm = "";
            }
            else
                temlist[i].StaffNm = "";

            MasterDentalOffice objdentalofficeinfo = MasterDentalOffice.GetDentalOfficeMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(temlist[i].DentalOfficeCd));
            temlist[i].DentalOfficeNm = objdentalofficeinfo.DentalOfficeNm;

            if (temlist[i].TrialOrderFlg.Value)
                temlist[i].Area = GetResource("Trial.Text");
            else if (temlist[i].RemanufactureFlg.Value)
                temlist[i].Area = GetResource("Remake.Text");
            else
                temlist[i].Area = GetResource("Finish.Text");

            if (temlist[i].InsuranceKbn.Value.ToString() == "True" || temlist[i].InsuranceKbn.Value.ToString() == "1")
                temlist[i].InsuranceKbnReport = GetResource("Insurance.Text");
            else if (temlist[i].InsuranceKbn.Value.ToString() == "False" || temlist[i].InsuranceKbn.Value.ToString() == "0")
                temlist[i].InsuranceKbnReport = GetResource("UnInsured.Text");
            else
                temlist[i].InsuranceKbnReport = null;

            if (temlist[i].Price == null)
                temlist[i].Price = 0;

            if (temlist[i].Amount.Value > 0 && temlist[i].Price.Value > 0)
                Sum = (Convert.ToDouble(temlist[i].Amount.Value) * Convert.ToDouble(temlist[i].Price.Value)).ToString();
            else Sum = "";

            OrderDate = temlist[i].OrderDate.ToShortDateString();
            //TaxController objTaxCon = new TaxController();
            MasterTax objTaxInfo = new MasterTax();
            objTaxInfo = MasterTax.SearchTax(Convert.ToDateTime(OrderDate));

            Tax = temlist[i].MaterialPrice.Value + temlist[i].ProcessPrice.Value;
            if (objTaxInfo != null)
            {
                if (objTaxInfo.RoundFraction == 2)
                    temlist[i].TaxRate = Math.Floor(objTaxInfo.TaxRate * Tax);
                else if (objTaxInfo.RoundFraction == 3)
                    temlist[i].TaxRate = Math.Ceiling(objTaxInfo.TaxRate * Tax);
                else
                    temlist[i].TaxRate = (long)(Math.Floor(objTaxInfo.TaxRate * Tax + 0.5));
            }
            #endregion
        }    
    }

    protected void SetDeliveryStatementNo(List<DeliveryStatementInfo> colOrderListAll)
    { 
        #region //Set DeliveryStatementNo
        using (DBContext DB = new DBContext())
        {
            using (System.Data.Common.DbTransaction tran = DB.UseTransaction())
            {
                try
                {

                    string strBranch = "";
                    DateTime dd;
                    int temp = 0;
                    if (string.IsNullOrWhiteSpace(txtIssueDate.Text))
                        return;
                    else
                        dd = DateTime.Parse(txtIssueDate.Text);

                    for (int i = 0; i < colOrderListAll.Count; i++)
                    {
                        if (colOrderListAll[temp].OrderSeq == colOrderListAll[i].OrderSeq)
                        {
                            //Set DeliveryStatementNo                            
                            if (strBranch == "")
                            {
                                MasterDeliveryStatementNo objMasterDeliveryStatementNo = MasterDeliveryStatementNo.GetDeliveryStatementNo(colOrderListAll[i].OfficeCd, colOrderListAll[i].DentalOfficeCd, dd.Year);
                                if (objMasterDeliveryStatementNo != null)
                                {
                                    if (objMasterDeliveryStatementNo.BlanchNo == 0)
                                    {
                                        strBranch = "01";
                                        objMasterDeliveryStatementNo.BlanchNo = 1;
                                    }
                                    else
                                    {
                                        int tmpBranch = objMasterDeliveryStatementNo.BlanchNo;
                                        tmpBranch++;
                                        strBranch = tmpBranch.ToString();
                                        if (strBranch.Length == 1) strBranch = "0" + strBranch;
                                        objMasterDeliveryStatementNo.BlanchNo += 1;
                                    }
                                    objMasterDeliveryStatementNo.OfficeCd = colOrderListAll[i].OfficeCd;
                                    objMasterDeliveryStatementNo.DentalOfficeCd = colOrderListAll[i].DentalOfficeCd;
                                    objMasterDeliveryStatementNo.Year = dd.Year;
                                    objMasterDeliveryStatementNo.Update();
                                }
                                else
                                {
                                    strBranch = "01";
                                    objMasterDeliveryStatementNo = new MasterDeliveryStatementNo();
                                    objMasterDeliveryStatementNo.OfficeCd = colOrderListAll[i].OfficeCd;
                                    objMasterDeliveryStatementNo.DentalOfficeCd = colOrderListAll[i].DentalOfficeCd;
                                    objMasterDeliveryStatementNo.Year = dd.Year;
                                    objMasterDeliveryStatementNo.BlanchNo = 1;
                                    objMasterDeliveryStatementNo.CreateAccount = objMasterDeliveryStatementNo.ModifiedAccount = this.User.Identity.Name;
                                    objMasterDeliveryStatementNo.CreateDate = objMasterDeliveryStatementNo.ModifiedDate = DateTime.Now;
                                    objMasterDeliveryStatementNo.Insert();
                                }
                            }
                            colOrderListAll[i].DeliveryStatementNo = "D" + HiddenFieldOfficeCd.Value + "-" + colOrderListAll[i].DentalOfficeCd.ToString() + "-" + dd.Year.ToString() + "-" + strBranch;

                            TrnOrderDetail objTrnOrderDetailUpdate = TrnOrderDetail.GetDentalOrderDetail(colOrderListAll[i].OfficeCd, colOrderListAll[i].OrderSeq, colOrderListAll[i].DetailSeq);
                            if (objTrnOrderDetailUpdate != null)
                            {
                                objTrnOrderDetailUpdate.DeliveryStatementNo = colOrderListAll[i].DeliveryStatementNo;
                                objTrnOrderDetailUpdate.DeliveredDate = Common.GetMinableDateTime(txtIssueDate.Text);
                                objTrnOrderDetailUpdate.ModifiedAccount = this.User.Identity.Name;
                                objTrnOrderDetailUpdate.Update();
                            }
                            //tran.Commit();
                        }
                        else
                        {
                            MasterDeliveryStatementNo objMasterDeliveryStatementNo = MasterDeliveryStatementNo.GetDeliveryStatementNo(colOrderListAll[i].OfficeCd, colOrderListAll[i].DentalOfficeCd, dd.Year);
                            if (objMasterDeliveryStatementNo != null)
                            {
                                if (objMasterDeliveryStatementNo.BlanchNo == 0)
                                {
                                    strBranch = "01";
                                    objMasterDeliveryStatementNo.BlanchNo = 1;
                                }
                                else
                                {
                                    int tmpBranch = objMasterDeliveryStatementNo.BlanchNo;
                                    tmpBranch++;
                                    strBranch = tmpBranch.ToString();
                                    if (strBranch.Length == 1) strBranch = "0" + strBranch;
                                    objMasterDeliveryStatementNo.BlanchNo += 1;
                                }
                                objMasterDeliveryStatementNo.OfficeCd = colOrderListAll[i].OfficeCd;
                                objMasterDeliveryStatementNo.DentalOfficeCd = colOrderListAll[i].DentalOfficeCd;
                                objMasterDeliveryStatementNo.Year = dd.Year;
                                objMasterDeliveryStatementNo.Update();
                            }
                            else
                            {
                                strBranch = "01";
                                objMasterDeliveryStatementNo = new MasterDeliveryStatementNo();
                                objMasterDeliveryStatementNo.OfficeCd = colOrderListAll[i].OfficeCd;
                                objMasterDeliveryStatementNo.DentalOfficeCd = colOrderListAll[i].DentalOfficeCd;
                                objMasterDeliveryStatementNo.Year = dd.Year;
                                objMasterDeliveryStatementNo.BlanchNo = 1;
                                objMasterDeliveryStatementNo.CreateAccount = objMasterDeliveryStatementNo.ModifiedAccount = this.User.Identity.Name;
                                objMasterDeliveryStatementNo.CreateDate = objMasterDeliveryStatementNo.ModifiedDate = DateTime.Now;
                                objMasterDeliveryStatementNo.Insert();
                            }
                            colOrderListAll[i].DeliveryStatementNo = "D" + HiddenFieldOfficeCd.Value + "-" + colOrderListAll[i].DentalOfficeCd.ToString() + "-" + dd.Year.ToString() + "-" + strBranch;
                            TrnOrderDetail objTrnOrderDetailUpdate = TrnOrderDetail.GetDentalOrderDetail(colOrderListAll[i].OfficeCd, colOrderListAll[i].OrderSeq, colOrderListAll[i].DetailSeq);
                            if (objTrnOrderDetailUpdate != null)
                            {
                                objTrnOrderDetailUpdate.DeliveryStatementNo = colOrderListAll[i].DeliveryStatementNo;
                                objTrnOrderDetailUpdate.DeliveredDate = Common.GetMinableDateTime(txtIssueDate.Text);
                                objTrnOrderDetailUpdate.ModifiedAccount = this.User.Identity.Name;
                                objTrnOrderDetailUpdate.Update();
                                temp = i;
                            }
                            //tran.Commit();
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    logger.Error("Set DeliveryStatementNo", ex);
                }
            }
        }
        #endregion
    }

    protected static List<DeliveryStatementInfo> GroupProsthesis(List<DeliveryStatementInfo> listNewTemp, CheckBox cbxPrintPerOrder)
    {       
        try
        {
            for (int i = 0; i < listNewTemp.Count; i++)
            {
                listNewTemp[i].Amount = 1;
                listNewTemp[i].MaterialPrice = 0;
                listNewTemp[i].Price = listNewTemp[i].ProcessPrice;                        
            }
            if (!cbxPrintPerOrder.Checked)
                listNewTemp = listNewTemp.OrderBy(c => c.DentalOfficeCd).ThenBy(c => c.OrderSeq).ThenBy(c=>c.ProsthesisCd).ThenBy(c => c.InsuranceKbn).ThenBy(c => c.Price).ToList();
            else
                listNewTemp = listNewTemp.OrderBy(c => c.OrderSeq).ThenBy(c => c.ProsthesisCd).ThenBy(c => c.InsuranceKbn).ThenBy(c => c.Price).ToList();

            int div10i = 0;
            int div10l = 0;
            int l = 0;
            listNewTemp[0].TechPrice = listNewTemp[0].Price;
            for (int i = 1; i < listNewTemp.Count; i++)
            {                            
                if (listNewTemp[i] != null)
                {                                
                    if (listNewTemp[l].OrderSeq == listNewTemp[i].OrderSeq)
                    {                                   
                        if (!string.IsNullOrEmpty(listNewTemp[l].ToothNumberStr) && !string.IsNullOrEmpty(listNewTemp[i].ToothNumberStr))
                        {
                            div10l = Convert.ToInt32(listNewTemp[l].ToothNumberStr) / 10;
                            div10i = Convert.ToInt32(listNewTemp[i].ToothNumberStr) / 10;
                        }
                        if (listNewTemp[l].ProsthesisCd == listNewTemp[i].ProsthesisCd && listNewTemp[l].Price == listNewTemp[i].Price && listNewTemp[l].InsuranceKbn == listNewTemp[i].InsuranceKbn)
                        {
                            if ((div10l == 1 || div10l == 2) && (div10i == 3 || div10i == 4))
                            {
                                listNewTemp[i].TechPrice = listNewTemp[i].Amount * listNewTemp[i].Price;                                
                                l = i;
                            }
                            else
                            {
                                listNewTemp[l].Amount += 1;
                                listNewTemp[l].TechPrice = listNewTemp[l].Amount * listNewTemp[l].Price;
                                listNewTemp[i] = listNewTemp[l];
                            }
                        }
                        else
                        {                                        
                            listNewTemp[i].TechPrice = listNewTemp[i].Amount * listNewTemp[i].Price;                            
                            l = i;
                        }
                    }
                    else
                    {                                   
                        listNewTemp[l].TechPrice = listNewTemp[l].Amount * listNewTemp[l].Price;
                        listNewTemp[i].TechPrice = listNewTemp[i].Amount * listNewTemp[i].Price;
                        l = i;
                    }
                }
            }
            listNewTemp = listNewTemp.Distinct().ToList();
            var list1 = listNewTemp.Where(c => c.ToothNumber != null && Convert.ToInt32(c.ToothNumber) / 10 == 1).OrderByDescending(c => c.ToothNumber).ToList();
            var list2 = listNewTemp.Where(c => c.ToothNumber != null && Convert.ToInt32(c.ToothNumber) / 10 == 2).OrderBy(c => c.ToothNumber).ToList();
            var list3 = listNewTemp.Where(c => c.ToothNumber != null && Convert.ToInt32(c.ToothNumber) / 10 == 3).OrderBy(c => c.ToothNumber).ToList();
            var list4 = listNewTemp.Where(c => c.ToothNumber != null && Convert.ToInt32(c.ToothNumber) / 10 == 4).OrderByDescending(c => c.ToothNumber).ToList();
            var list0 = listNewTemp.Where(c => c.ToothNumber == null).ToList();
            listNewTemp = new List<DeliveryStatementInfo>();
            listNewTemp.AddRange(list1);
            listNewTemp.AddRange(list2);
            listNewTemp.AddRange(list4);
            listNewTemp.AddRange(list3);
            listNewTemp.AddRange(list0);
            for (int i = 0; i < listNewTemp.Count; i++)
            {
                if (i < listNewTemp.Count - 1)
                    listNewTemp[i + 1].PatientFirstNm = listNewTemp[i + 1].PatientLastNm = listNewTemp[i + 1].OrderNo = listNewTemp[i + 1].Area = "";
                if (i < listNewTemp.Count - 1 && listNewTemp[i].ToothNumberReport == listNewTemp[i + 1].ToothNumberReport)
                    listNewTemp[i + 1].ToothNumberReport = "";
            }
        }
        catch(Exception ex)
        {
            logger.Error("GroupProthesis", ex);
        }
        return listNewTemp;        
    }

    protected void AddListMaterial(List<DeliveryStatementInfo> colOrderListAll, List<DeliveryStatementInfo> listNewTemp)
    {
        try
        {
            for (int i = 0; i < colOrderListAll.Count; i++)
            {
                DeliveryStatementInfo listobjdelivery = DeliveryStatementInfo.GetOrder(colOrderListAll[i].OrderSeq, colOrderListAll[i].DetailSeq, Convert.ToInt32(HiddenFieldOfficeCd.Value));
                if (listobjdelivery.MaterialCd != null)
                {
                    if (listobjdelivery.Amount == null)
                        listobjdelivery.Amount = 0;
                    if (listobjdelivery.Price == null)
                        listobjdelivery.Price = 0;
                    if (listobjdelivery.ProcessPrice == null)
                        listobjdelivery.ProcessPrice = 0;
                    if (listobjdelivery.MaterialPrice == null)
                        listobjdelivery.MaterialPrice = 0;
                    listobjdelivery.ProsthesisNm = listobjdelivery.MaterialNm;
                    listobjdelivery.ToothNumberReport = null;
                    listobjdelivery.OrderNo = listobjdelivery.PatientFirstNm = listobjdelivery.PatientLastNm = "";
                    listobjdelivery.InsuranceKbn = true; //"*"
                    listobjdelivery.InsuranceKbnReport = "*";
                    listNewTemp.Add(listobjdelivery);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error("ListMaterial", ex);
        }        
    }

    protected static List<DeliveryStatementInfo> GroupMaterial(List<DeliveryStatementInfo> listNewTemp, CheckBox cbxPrintPerOrder)
    {
        List<DeliveryStatementInfo> listNew = new List<DeliveryStatementInfo>();
        try
        {            
            if (listNewTemp.Count > 0)
            {
                for (int i = 0; i < listNewTemp.Count; i++)
                {
                    listNewTemp[i].TechPrice = 0;
                }
                listNewTemp[0].MaterialPrice = listNewTemp[0].Amount * listNewTemp[0].Price;

                if(!cbxPrintPerOrder.Checked)
                    listNewTemp = listNewTemp.OrderBy(c => c.DentalOfficeCd).ThenBy(c => c.OrderSeq).ThenBy(c => c.MaterialCd).ToList();
                else
                    listNewTemp = listNewTemp.OrderBy(c => c.OrderSeq).ThenBy(c => c.MaterialCd).ToList();

                int k = 0;
                double amount = 0;
                if (listNewTemp.Count > 1)
                {
                    for (int i = 1; i < listNewTemp.Count; i++)
                    {
                        if (listNewTemp[0].Amount == null)
                            listNewTemp[0].Amount = 0;
                        if (listNewTemp[i].Amount == null)
                            listNewTemp[i].Amount = 0;
                        if (listNewTemp[i] != null)
                        {
                            if (listNewTemp[k].OrderSeq == listNewTemp[i].OrderSeq)
                            {
                                if (listNewTemp[k].MaterialCd == listNewTemp[i].MaterialCd)
                                {
                                    amount = Convert.ToDouble(listNewTemp[k].Amount);
                                    amount += Convert.ToDouble(listNewTemp[i].Amount);
                                    listNewTemp[k].Amount = amount;
                                    listNewTemp[k].MaterialPrice = listNewTemp[k].Amount * listNewTemp[k].Price;
                                    listNewTemp[i] = listNewTemp[k];
                                }
                                else
                                {
                                    listNewTemp[k].MaterialPrice = listNewTemp[k].Amount * listNewTemp[k].Price;
                                    k = i;
                                }
                            }
                            else
                            {
                                listNewTemp[k].MaterialPrice = listNewTemp[k].Amount * listNewTemp[k].Price;
                                k = i;
                            }
                        }
                        listNewTemp[i].MaterialPrice = listNewTemp[i].Amount * listNewTemp[i].Price;
                    }
                }
                listNew = listNewTemp.Distinct().ToList();
            }            
        }
        catch (Exception ex)
        {
            logger.Error("Group Material", ex);
        }
        return listNew;        
    }
    
    protected static List<DeliveryStatementInfo> AddRowEmptyDentalOffice(List<DeliveryStatementInfo> listNew, List<DeliveryStatementInfo> listAll)
    {
        #region add Empty row        
        DeliveryStatementInfo colOrderList = new DeliveryStatementInfo();        
        int total = 0;
        listNew = listNew.Distinct().ToList();
        listNew = listNew.OrderBy(c => c.DentalOfficeCd).ThenBy(c => c.OrderSeq).ToList();
        int tempsearch = listNew[0].DentalOfficeCd;
        try
        {
            int firts = 0;
            int temTaxRate = 0;
            
            if (listNew.Count == 1)
            {
                listNew[0].PageNumber = listNew[0].TotalPage = "1";
            }
            for (int i = 1; i < listNew.Count; i++)
            {
                if (listNew[temTaxRate].DentalOfficeCd == listNew[i].DentalOfficeCd)
                {
                    //listCount++;
                    listNew[temTaxRate].TaxRate += listNew[i].TaxRate;
                    listNew[i].TaxRate = listNew[temTaxRate].TaxRate;
                    listNew[temTaxRate].TechPrice += listNew[i].TechPrice;
                    listNew[i].TechPrice = listNew[temTaxRate].TechPrice;
                    listNew[temTaxRate].MaterialPrice += listNew[i].MaterialPrice;
                    listNew[i].MaterialPrice = listNew[temTaxRate].MaterialPrice;
                }
                else
                {                    
                    temTaxRate = i;                    
                }
            }
            //temTaxRate = 0;
            //listCount = 0;
            for (int i = 1; i < listNew.Count; i++)
            {
                if (listNew[temTaxRate].DentalOfficeCd == listNew[i].DentalOfficeCd)
                {
                    listNew[i].TaxRate = listNew[temTaxRate].TaxRate;
                    listNew[i].TechPrice = listNew[temTaxRate].TechPrice;
                    listNew[i].MaterialPrice = listNew[temTaxRate].MaterialPrice;
                }
                else                
                    temTaxRate = i;                
            }            
            for (int i = 0; i < listNew.Count; i++)
            {
                if (tempsearch == listNew[i].DentalOfficeCd)
                {
                    listAll.Add(listNew[i]);
                    total++;
                }
                else
                {
                    if (total % 7 != 0)
                    {
                        for (int j = 0; j < 7 - total % 7; j++)
                        {
                            colOrderList = null;
                            listAll.Add(colOrderList);
                        }
                    }
                    tempsearch = listNew[i].DentalOfficeCd;
                    listAll.Add(listNew[i]);
                    firts = i;
                    total = 1;
                }
            }
            if (total % 7 != 0)
            {
                for (int j = 0; j < 7 - total % 7; j++)
                {
                    colOrderList = null;
                    listAll.Add(colOrderList);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error("Add empty row", ex);
        }
        return listAll;
        #endregion               
    }

    protected static List<DeliveryStatementInfo> AddRowEmptyOrderSeq(List<DeliveryStatementInfo> listNew, List<DeliveryStatementInfo> listAll)
    {
        DeliveryStatementInfo colOrderList = new DeliveryStatementInfo();       
        int total = 0;
        listNew = listNew.Distinct().ToList();
        listNew = listNew.OrderBy(c => c.OrderSeq).ToList();    
        double tempsearch = listNew[0].OrderSeq;

        try
        {
            int temTaxRate = 0;
            for (int i = 1; i < listNew.Count; i++)
            {
                if (listNew[temTaxRate].OrderSeq == listNew[i].OrderSeq)
                {
                    listNew[temTaxRate].TaxRate += listNew[i].TaxRate;
                    listNew[i].TaxRate = listNew[temTaxRate].TaxRate;
                    listNew[temTaxRate].TechPrice += listNew[i].TechPrice;
                    listNew[i].TechPrice = listNew[temTaxRate].TechPrice;
                    listNew[temTaxRate].MaterialPrice += listNew[i].MaterialPrice;
                    listNew[i].MaterialPrice = listNew[temTaxRate].MaterialPrice;
                }
                else
                    temTaxRate = i;
            }
            for (int i = 1; i < listNew.Count; i++)
            {
                if (listNew[temTaxRate].OrderSeq == listNew[i].OrderSeq)
                {
                    listNew[i].TaxRate = listNew[temTaxRate].TaxRate;
                    listNew[i].TechPrice = listNew[temTaxRate].TechPrice;
                    listNew[i].MaterialPrice = listNew[temTaxRate].MaterialPrice;
                }
                else
                    temTaxRate = i;
            }
            for (int i = 0; i < listNew.Count; i++)
            {
                if (tempsearch == listNew[i].OrderSeq)
                {
                    listAll.Add(listNew[i]);
                    total++;
                }
                else
                {
                    if (total % 7 != 0)
                    {
                        for (int j = 0; j < 7 - total % 7; j++)
                        {
                            colOrderList = null;
                            listAll.Add(colOrderList);
                        }
                    }
                    tempsearch = listNew[i].OrderSeq;
                    listAll.Add(listNew[i]);
                    total = 1;
                }
            }
            // Add page last
            if (total % 7 != 0)
            {
                for (int j = 0; j < 7 - total % 7; j++)
                {
                    colOrderList = null;
                    listAll.Add(colOrderList);
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error("Break List and add empty list", ex);
        }
        return listAll;
    }

    protected void SetPageNumber(int flag, List<DeliveryStatementInfo> listAll, double ToTalPage, double PageNumber)
    {        
        try
        {
            int tempCount = 0;
            int iii = 0;
            while (iii < listAll.Count)
            {
                flag++;
                var temp = listAll.Where(c => c != null && c.DeliveryStatementNo == listAll[tempCount].DeliveryStatementNo).ToList();
                ToTalPage = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(temp.Count) / 7));
                if (ToTalPage < 1)
                    ToTalPage = 1;
                for (int j = 1; j <= ToTalPage; j++)
                {
                    PageNumber++;
                    for (int k = tempCount + 7 * (j - 1); k < tempCount + 7 * j; k++)
                    {
                        if (listAll[k] != null)
                        {
                            listAll[k].PageNumber = PageNumber.ToString();
                            listAll[k].TotalPage = ToTalPage.ToString();
                        }
                    }
                }
                iii = tempCount + 7 * Convert.ToInt32(ToTalPage);
                tempCount = iii;
                PageNumber = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Error("Set Page", ex);
        }       
    }

    protected void FillReportViewer(List<DeliveryStatementInfo> listAll, double Tax, double ToTalPage)
    {
        try
        {
            this.ReportViewer1.Reset();
            if(Calendar.flag == "ja")
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReportDeliveryJP.rdlc");
            else
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReportDelivery.rdlc");

            //this.ReportViewer1.LocalReport.ReportEmbeddedResource = "Dental.Web.ReportDelivery.rdlc";
            ReportDataSource rds = new ReportDataSource("DataSet1", listAll);
            this.ReportViewer1.LocalReport.DataSources.Add(rds);
            this.ReportViewer1.LocalReport.EnableExternalImages = true;

            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DeliveryStatementCopy", GetResource("DeliveryStatementCopy.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("BillStatement", GetResource("BillStatement.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DeliveryStatement", GetResource("DeliveryStatement.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Date", GetResource("Date.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DentalOfficeNm", GetResource("DentalOfficeName.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SaleMan", GetResource("lbSalesman.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DeliveryStatementNo", GetResource("DeliveryStatementNo.Text")));

            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ParameterDate", DateTime.Now.ToShortDateString()));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Area", GetResource("Area.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("No", GetResource("No.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Parts", GetResource("Parts.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Prosthesis", GetResource("Prosthesis.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Insurance", GetResource("lbInsurance.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Amount", GetResource("Amount_Amount.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Price", GetResource("Price.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Sum", GetResource("Sum.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ProcessPrice", GetResource("ProcessPrice.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Patient", GetResource("Patient.Text")));

            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Note", GetResource("NoteRe.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("TechnicalPrice", GetResource("TechnicalPrice.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SumMaterialPrice", GetResource("MaterialPrice.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Tax", GetResource("Tax.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("TaxSum", Tax.ToString()));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SumSum", GetResource("Sum.Text")));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ToTalPage", ToTalPage.ToString()));
            this.ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("MaterialNm", GetResource("lbMaterialNm.Text")));

            this.ReportViewer1.LocalReport.DisplayName = listAll[0].DeliveryStatementNo;
            DisableUnwantedExportFormat(ReportViewer1, "WORD");
            DisableUnwantedExportFormat(ReportViewer1, "Excel");
            this.ReportViewer1.DataBind();
            this.ReportViewer1.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            logger.Error("Fill Report Viewer", ex);
        }  
    }

    protected void SplitPage(int flag, List<DeliveryStatementInfo> listAll)
    {        
        try
        {
            string fullfolderZip = CreateDeliveryStatementStore(this.ReportViewer1.LocalReport.DisplayName);
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = ReportViewer1.LocalReport.Render("Pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);
            string _outMergeFile = "";

            if (flag == 1)
            {
                _outMergeFile = fullfolderZip + "\\" + this.ReportViewer1.LocalReport.DisplayName + ".pdf";
                FileStream fs = new FileStream(_outMergeFile, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }
            else
            {
                _outMergeFile = fullfolderZip + "\\" + this.ReportViewer1.LocalReport.DisplayName + "All.pdf";
                FileStream fs = new FileStream(_outMergeFile, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                #region// Set Page Number
                int tempCount1 = 0;
                int iii1 = 0;
                double ToTalPage1 = 0;
                string Nameaaa = "";
                while (iii1 < listAll.Count)
                {
                    var temp = listAll.Where(c => c != null && c.DeliveryStatementNo == listAll[tempCount1].DeliveryStatementNo).ToList();
                    ToTalPage1 = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(temp.Count) / 7));
                    if (ToTalPage1 < 1)
                        ToTalPage1 = 1;
                    Nameaaa = listAll[tempCount1].DeliveryStatementNo;
                    PdfReader readerA = new PdfReader(_outMergeFile);
                    readerA.SelectPages((tempCount1 / 7 + 1).ToString() + "-" + (ToTalPage1 + tempCount1 / 7).ToString());
                    PdfStamper cc = new PdfStamper(readerA, new FileStream(fullfolderZip + "\\" + Nameaaa + ".pdf", FileMode.CreateNew));
                    cc.Close();
                    readerA.Close();
                    iii1 = tempCount1 + 7 * Convert.ToInt32(ToTalPage1);
                    tempCount1 = iii1;
                }
                #endregion
            }
            if (flag != 1)
                File.Delete(fullfolderZip + "\\" + this.ReportViewer1.LocalReport.DisplayName + "All.pdf");
            //this.hyperlinkSave.Visible = true;
            //this.hyperlinkSave.NavigateUrl = SaveDeliveryStatementFile(this.ReportViewer1.LocalReport.DisplayName);
        }
        catch (Exception ex)
        {
            logger.Error("Split pdf", ex);
            //this.hyperlinkSave.Visible = true;
            //this.hyperlinkSave.NavigateUrl = SaveDeliveryStatementFile(this.ReportViewer1.LocalReport.DisplayName);
        }       
    }

    protected void LinkDentalFont_Click(object sender, EventArgs e)
    {
        try
        {
            string filepath = Server.MapPath("Portal" + "\\" + "cpeu.zip");
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
            logger.Error("Download font", ex);            
        }   
    }

    protected void ImageSave_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filepath = SaveDeliveryStatementFile(this.ReportViewer1.LocalReport.DisplayName);
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