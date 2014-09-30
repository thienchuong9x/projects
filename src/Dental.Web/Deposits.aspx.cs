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

public partial class Deposits : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(Deposits));
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitLanguage();
                InitCalendar();
                Initialize();

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
        //get Message Resource
        HiddenFieldOfficeCd.Value = GetOffice();
        lbIssueDate.Text = GetResource("TitleBillIssueDate.Text");
        lbPayerCd.Text = GetResource("lbPayerCd.Text");
        lbPayMentDate.Text = GetResource("TitlePayDate.Text");
        lbDepositAccount.Text = GetResource("lbDepositAccount.Text");
        lbComment.Text = GetResource("lbComment.Text");
        lblModuleTitle.Text = GetResource("lblModuleTitle.Text");
        lbChkCompleted.Text = GetResource("lbChkCompleted.Text");
        lblBillStatementNo.Text = GetResource("TitleBillStataMentNo.Text");

        string str = GetResource("Title_Error.Text") + "@" + GetResource("Title_PayMentAmount_Less_Than_Billing_Amount.Text");
        //str = str.Replace("@", Environment.NewLine);
        TextBox1.Text = str;

        btnSearch.Text = GetResource("btnSearch.Text");
        btnClear.Text = GetResource("btnClear.Text");
        btnRegister.Text = GetResource("btnRegister.Text");

        Val_IssueDateFrom.ErrorMessage = Val_IssueDateTo.ErrorMessage =
        Val_PayDateFrom.ErrorMessage = Val_PayDateTo.ErrorMessage = GetResource("DateOnly.Text");
        Val_RegMaxlength.ErrorMessage = GetResource("Val_RegMaxlength.Text");
        // Fill data
        FillDropDownPayer();
        FillDropAccount();
        FillDataGridviewOnStartup();

    }

    private void FillDataGridviewOnStartup()
    {
        List<MoneyReceiveSearchInfo> list = new List<MoneyReceiveSearchInfo>();
        btnRegister.Enabled = false;
        MoneyReceiveSearchInfo objMoneyReceiveInfo = new MoneyReceiveSearchInfo();
        objMoneyReceiveInfo.DentalOfficeCd = -1;
        objMoneyReceiveInfo.OfficeCd = -1;
        objMoneyReceiveInfo.BillSeq = -1;
        objMoneyReceiveInfo.BillCd = -1;
        objMoneyReceiveInfo.BillIssueDate = DateTime.Now;
        objMoneyReceiveInfo.PayDate = DateTime.Now;

        list.Add(objMoneyReceiveInfo);

        grvMoneyReceive.DataSource = list;
        grvMoneyReceive.DataBind();
        if (list.Count == 1 && list[0].OfficeCd == -1)
        {
            grvMoneyReceive.Rows[0].Visible = false;
        }
        lbMessage.Text = GetResource("NoRecordFound.Text");
        DropDownListAccount.Text = "";
        txtComment.Text = "";
            
    }

    private void FillDropAccount()
    {
        List<MasterBank> listBank = new List<MasterBank>();
        DropDownListAccount.Items.Add(new ListItem("", ""));
        listBank = MasterBank.GetBankForFlg(int.Parse(HiddenFieldOfficeCd.Value), false, true);
        foreach (MasterBank i in listBank)
        {
            DropDownListAccount.Items.Add(new ListItem(i.BankAccount, i.BankCd.ToString()));
        }
    }

    private void FillDropDownPayer()
    {
        DropDownPayer.Items.Add(new ListItem("", ""));
        List<MasterBill> billInfo = MasterBill.GetBillMasters(int.Parse(HiddenFieldOfficeCd.Value));
        foreach (MasterBill b in billInfo)
        {
            DropDownPayer.Items.Add(new ListItem(b.BillNm, b.BillCd.ToString()));
        }
    }

    private void InitCalendar()
    {
        hplIssueDateFrom.NavigateUrl = Calendar.InvokePopupCal(txtIssueDateFrom);
        hplIssueDateTo.NavigateUrl = Calendar.InvokePopupCal(txtIssueDateTo);
        hplPayMentDateFrom.NavigateUrl = Calendar.InvokePopupCal(txtPayDateFrom);
        hplPaymentDateTo.NavigateUrl = Calendar.InvokePopupCal(txtPayDateTo);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            FillDataGridview();

        }
        catch (Exception ex)
        {
            logger.Error("Error Search", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);

        }
    }

    private void FillDataGridview()
    {
        try
        {
            DropDownListAccount.Text = "";
            txtComment.Text = "";
            int check;
            List<MoneyReceiveSearchInfo> ListMoneyReceiveInfo = new List<MoneyReceiveSearchInfo>();

            if (chkCompleted.Checked)
                check = 1;
            else
                check = 0;

            #region "Input"
            if (txtIssueDateFrom.Text == "")
                txtIssueDateFrom.Text = "1/1/2000";

            if (txtIssueDateTo.Text == "")
                txtIssueDateTo.Text = "1/1/4000";

            if (txtPayDateFrom.Text == "")
                txtPayDateFrom.Text = "1/1/2000";

            if (txtPayDateTo.Text == "")
                txtPayDateTo.Text = "1/1/4000";

            if (txtPayerCd.Text == "")
                txtPayerCd.Text = "-1";
            #endregion

            ListMoneyReceiveInfo = MoneyReceiveSearchInfo.GetMoneyReciveSearch(txtPayerCd.Text, txtBillStatementNo.Text, Convert.ToDateTime(txtIssueDateFrom.Text), Convert.ToDateTime(txtIssueDateTo.Text + " 23:59:59"), Convert.ToDateTime(txtPayDateFrom.Text), Convert.ToDateTime(txtPayDateTo.Text + " 23:59:59"), Convert.ToInt32(HiddenFieldOfficeCd.Value), check);
            if (!string.IsNullOrWhiteSpace(txtBillStatementNo.Text))
            {
                ListMoneyReceiveInfo = ListMoneyReceiveInfo.Where(m => (m.BillStatementNo == txtBillStatementNo.Text.Trim())).ToList();
            }

            #region "Clear Input"
            if (txtIssueDateFrom.Text == "1/1/2000")
                txtIssueDateFrom.Text = "";

            if (txtIssueDateTo.Text == "1/1/4000")
                txtIssueDateTo.Text = "";

            if (txtPayDateFrom.Text == "1/1/2000")
                txtPayDateFrom.Text = "";

            if (txtPayDateTo.Text == "1/1/4000")
                txtPayDateTo.Text = "";

            if (txtPayerCd.Text == "-1")
                txtPayerCd.Text = "";

            #endregion
            
            grvMoneyReceive.Columns[12].Visible = true;
            grvMoneyReceive.Columns[13].Visible = true;
            grvMoneyReceive.Columns[14].Visible = true;
            grvMoneyReceive.Columns[15].Visible = true;
            grvMoneyReceive.Columns[16].Visible = true;
            grvMoneyReceive.Columns[17].Visible = true;
            grvMoneyReceive.Columns[18].Visible = true;
            grvMoneyReceive.Columns[19].Visible = true;
            grvMoneyReceive.Columns[20].Visible = true;
            grvMoneyReceive.Columns[11].Visible = true;
            grvMoneyReceive.Columns[21].Visible = true;

            txtServiceCharge.Text = "";
            txtTotalPayment.Text = "";
            txtPaymentAmount.Text = "";

            // gridview not record
            if (ListMoneyReceiveInfo.Count == 0)
            {
                List<MoneyReceiveSearchInfo> list = new List<MoneyReceiveSearchInfo>();
                btnRegister.Enabled = false;
                MoneyReceiveSearchInfo objMoneyReceiveInfo = new MoneyReceiveSearchInfo();
                objMoneyReceiveInfo.DentalOfficeCd = -1;
                objMoneyReceiveInfo.OfficeCd = -1;
                objMoneyReceiveInfo.BillSeq = -1;
                objMoneyReceiveInfo.BillCd = -1;
                objMoneyReceiveInfo.BillIssueDate = DateTime.Now;
                objMoneyReceiveInfo.PayDate = DateTime.Now;

                list.Add(objMoneyReceiveInfo);

                grvMoneyReceive.DataSource = list;
                grvMoneyReceive.DataBind();
                if (list.Count == 1 && list[0].OfficeCd == -1)
                {
                    grvMoneyReceive.Rows[0].Visible = false;
                }
                lbMessage.Text = GetResource("NoRecordFound.Text");
                DropDownListAccount.Text = "";
                txtComment.Text = "";
            }
            // Fill data to gridview
            else
            {
                btnRegister.Enabled = true;
                grvMoneyReceive.DataSource = ListMoneyReceiveInfo;
                grvMoneyReceive.DataBind();
                lbMessage.Text = string.Empty;
                foreach (GridViewRow row in grvMoneyReceive.Rows)
                {
                    if (Common.GetRowString(row.Cells[3].Text) != "")
                        row.Cells[3].Text = Convert.ToDateTime(Common.GetRowString(row.Cells[3].Text)).ToShortDateString();

                    if (Common.GetRowString(row.Cells[4].Text) != "")
                        row.Cells[4].Text = Convert.ToDateTime(Common.GetRowString(row.Cells[4].Text)).ToShortDateString();


                    if (Common.GetRowString(row.Cells[21].Text) == "0")
                        row.Cells[4].Text = "";//create by CHUONG


                    TextBox txtPayAmount = (TextBox)row.FindControl("txtPayAmount");
                    TextBox txtSubtractFee = (TextBox)row.FindControl("txtSubtractFee");
                    TextBox txtCurrentPaymentDate = (TextBox)row.FindControl("txtCurrentPaymentDate");

                    txtCurrentPaymentDate.Text = DateTime.Now.ToShortDateString();
                    HyperLink hpl = (HyperLink)row.FindControl("hplCurrentPaymentDate");
                    hpl.NavigateUrl = Calendar.InvokePopupCal(txtCurrentPaymentDate);

                    CompareValidator Val_Amount = (CompareValidator)row.FindControl("Val_Amount");
                    CompareValidator Val_ServiceCharge = (CompareValidator)row.FindControl("Val_ServiceCharge");
                    CompareValidator Val_CurrentpayDate = (CompareValidator)row.FindControl("Val_CurrentpayDate");

                    Val_Amount.ErrorMessage = GetResource("DoubleOnly.Text");
                    Val_ServiceCharge.ErrorMessage = GetResource("DoubleOnly.Text");
                    Val_CurrentpayDate.ErrorMessage = GetResource("DateOnly.Text");

                    //Previuos PayAmount 
                    if (Common.GetRowString(row.Cells[16].Text) == "")
                        row.Cells[16].Text = "0";
                    // Previous SubtractFee
                    if (Common.GetRowString(row.Cells[17].Text) == "")
                        row.Cells[17].Text = "0";
                    // Balance = SumPrice - PayAmount 
                    // row.Cells[6].Text = Math.Round(Convert.ToDouble(row.Cells[15].Text) - Convert.ToDouble(row.Cells[16].Text), 10).ToString();
                    if (Common.GetRowString(row.Cells[6].Text) == "")
                        row.Cells[6].Text = row.Cells[5].Text;

                    //if (float.Parse(row.Cells[6].Text)<0)//row.Cells[6].Text == "-3.402823E+38")
                    //    row.Cells[6].Text = row.Cells[5].Text;

                    if (Common.GetRowString(row.Cells[21].Text) == string.Empty)//row.Cells[21].Text == "-3.402823E+38")
                        row.Cells[21].Text = "0";
                    // Getname Payer
                    ListItem item = DropDownPayer.Items.FindByValue(row.Cells[12].Text);
                    if (item != null)
                        row.Cells[1].Text = item.Text;
                    else row.Cells[1].Text = "";

                    txtPayAmount.ReadOnly = true;
                    txtSubtractFee.ReadOnly = true;
                    txtCurrentPaymentDate.Text = SetDateFormat(txtCurrentPaymentDate.Text);
                    if (Common.GetRowString(row.Cells[3].Text) != string.Empty)
                    {
                        row.Cells[3].Text = SetDateFormat(Common.GetRowString(row.Cells[3].Text));
                    }
                    if (Common.GetRowString(row.Cells[4].Text) != string.Empty)
                    {
                        row.Cells[4].Text = SetDateFormat(Common.GetRowString(row.Cells[4].Text));
                    }
                }
                lbMessage.Text = GetResource("TitleResults.Text") + " : " + grvMoneyReceive.Rows.Count;
            }
            grvMoneyReceive.Columns[12].Visible = false;
            grvMoneyReceive.Columns[13].Visible = false;
            grvMoneyReceive.Columns[14].Visible = false;
            grvMoneyReceive.Columns[15].Visible = false;
            grvMoneyReceive.Columns[16].Visible = false;
            grvMoneyReceive.Columns[17].Visible = false;
            grvMoneyReceive.Columns[18].Visible = false;
            grvMoneyReceive.Columns[19].Visible = false;
            grvMoneyReceive.Columns[20].Visible = false;
            grvMoneyReceive.Columns[11].Visible = false;
            grvMoneyReceive.Columns[21].Visible = false;
        }
        catch (Exception ex)
        {

            logger.Error("Error Fill Grid", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);

        }
    }
    protected void grvMoneyReceive_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Payment custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add CheckAll
            TableHeaderCell tcCheckBox = new TableHeaderCell();
            CheckBox chkCheckAll = new CheckBox();
            chkCheckAll.ID = "chkCheckAll";
            chkCheckAll.Attributes["onclick"] = "javascript:checkAll(this);";
            chkCheckAll.AutoPostBack = true;
            chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            tcCheckBox.Controls.Add(chkCheckAll);
            tcCheckBox.CssClass = "HeadwithBG";
            oGridViewRow.Cells.Add(tcCheckBox);

            ////Add CheckBoxSelected
            //oTableCell.CssClass = "td_header";
            //oTableCell.Width = Unit.Pixel(20);
            //oGridViewRow.Cells.Add(oTableCell);


            //Add DentalOfficeNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleDentalOfficeNm.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(160);
            oGridViewRow.Cells.Add(oTableCell);

            //Add BillStatementNo
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleBillStataMentNo.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(160);
            oGridViewRow.Cells.Add(oTableCell);


            //Add IssuaDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleBillIssueDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(160);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PayDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitlePayDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(180);
            oGridViewRow.Cells.Add(oTableCell);

            //Add CurrentBillSumPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleCurrentBillSumPrice.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add CurrentBillSumPrice
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbBalance.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PayAmount
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitlePayAmount.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add SubtractFee
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleSubtractFee.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add SumPayAmount
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleSumPayAmount.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add SumPayAmount
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("TitleCurrentPayDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(200);
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header     
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
            //Add Footer                
        }
    }
    protected void chkCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow rows in grvMoneyReceive.Rows)
        {
            CheckEdit_Checked(sender, e);
        }
    }
    protected void CheckEdit_Checked(object sender, EventArgs e)
    {
        int check = 0;
        double totalPay = 0;
        double totalser = 0;
        foreach (GridViewRow row in grvMoneyReceive.Rows)
        {
            TextBox txtPayAmount = (TextBox)row.FindControl("txtPayAmount");
            TextBox txtSubtractFee = (TextBox)row.FindControl("txtSubtractFee");
            TextBox txtSumPayAmount = (TextBox)row.FindControl("txtSumPayAmount");

            txtPayAmount.Attributes.Add("onblur", "AddTotal();");
            txtSubtractFee.Attributes.Add("onblur", "AddTotal();");
            txtSumPayAmount.Attributes.Add("onblur", "AddTotal();");

            CheckBox cb = (CheckBox)row.FindControl("CheckEdit");// check CheckBox checked             

            // Checked
            if (cb.Checked)
            {
                check++;
                txtPayAmount.ReadOnly = false;
                txtSubtractFee.ReadOnly = false;
                if (Common.GetRowString(row.Cells[13].Text) != "")
                    DropDownListAccount.SelectedValue = row.Cells[13].Text;
                else
                    DropDownListAccount.SelectedValue = null;

                if (Common.GetRowString(row.Cells[14].Text) != "")
                    txtComment.Text = row.Cells[14].Text;
                else txtComment.Text = "";

                if (txtPayAmount.Text == "")
                    txtPayAmount.Text = row.Cells[6].Text;

                if (txtPayAmount.Text != "" && txtSubtractFee.Text == "")
                    txtSumPayAmount.Text = txtPayAmount.Text;

                if (txtPayAmount.Text == "" && txtSubtractFee.Text != "")
                    txtSumPayAmount.Text = txtSubtractFee.Text;

                if (txtPayAmount.Text != "" && txtSubtractFee.Text != "")
                    txtSumPayAmount.Text = Math.Round(Convert.ToDouble(txtPayAmount.Text) + Convert.ToDouble(txtSubtractFee.Text), 10).ToString();

                if (txtPayAmount.Text != "")
                    totalPay = Math.Round(totalPay + Convert.ToDouble(txtPayAmount.Text), 10);
                else totalPay += 0;

                if (txtSubtractFee.Text != "")
                    totalser = Math.Round(totalser + Convert.ToDouble(txtSubtractFee.Text), 10);
                else totalser += 0;

            }
            // not checked
            else
            {
                txtPayAmount.ReadOnly = true;
                txtSubtractFee.ReadOnly = true;
                txtPayAmount.Text = "";
                txtSubtractFee.Text = "";
                txtSumPayAmount.Text = "";
            }
            if (check < 1)
            {
                DropDownListAccount.SelectedValue = null;
                txtComment.Text = "";
            }
        }
        txtPaymentAmount.Text = totalPay.ToString();
        txtServiceCharge.Text = totalser.ToString();
        txtTotalPayment.Text = Math.Round(totalPay + totalser, 10).ToString();

    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            int total = 0;
            int Error = 0;
            MoneyReceiveSearchInfo objinfoMoneyReceive = new MoneyReceiveSearchInfo();
            MasterBill objBill1 = new MasterBill();
            foreach (GridViewRow row in grvMoneyReceive.Rows)
            {
                TextBox txtPayAmount = (TextBox)row.FindControl("txtPayAmount");
                TextBox txtSubtractFee = (TextBox)row.FindControl("txtSubtractFee");
                TextBox txtCurrentPaymentDate = (TextBox)row.FindControl("txtCurrentPaymentDate");

                CheckBox cb = (CheckBox)row.FindControl("CheckEdit");// check CheckBox checked
                //SetPaydateTime                 

                // checked
                if (cb.Checked)
                {
                    total++;
                    // set data
                    if (Common.GetRowString(row.Cells[19].Text) == "")
                        row.Cells[19].Text = txtCurrentPaymentDate.Text; //create by chuong

                    objinfoMoneyReceive.OfficeCd = Convert.ToInt32(HiddenFieldOfficeCd.Value);

                    if (Common.GetRowString(row.Cells[4].Text) != string.Empty)
                        objinfoMoneyReceive.PayDateTime = Common.GetNullableDateTime(Common.GetRowString(row.Cells[19].Text));//chuong
                    else
                        objinfoMoneyReceive.PayDateTime = Common.GetNullableDateTime(txtCurrentPaymentDate.Text);

                    objinfoMoneyReceive.BillCd = Convert.ToInt32(row.Cells[12].Text);

                    objBill1 = MasterBill.GetBillMaster(Convert.ToInt32(HiddenFieldOfficeCd.Value), objinfoMoneyReceive.BillCd);
                    objinfoMoneyReceive.BillNm = objBill1.BillNm;

                    objinfoMoneyReceive.PayDate = Convert.ToDateTime(txtCurrentPaymentDate.Text) + DateTime.Now.TimeOfDay;

                    objinfoMoneyReceive.BillSeq = Convert.ToInt32(Common.GetRowString(row.Cells[11].Text));

                    if (txtPayAmount.Text == "")
                        txtPayAmount.Text = "0";
                    if (txtSubtractFee.Text == "")
                        txtSubtractFee.Text = "0";

                    
                    objinfoMoneyReceive.PayAmount = Common.GetNullableDouble(txtPayAmount.Text);

                    
                    objinfoMoneyReceive.SubtractFee = Common.GetNullableDouble(txtSubtractFee.Text);

                    objinfoMoneyReceive.SumPayAmountCurrent = (Convert.ToDouble(txtPayAmount.Text) + Convert.ToDouble(txtSubtractFee.Text)).ToString();
                    objinfoMoneyReceive.SumPayAmount = (Convert.ToDouble(txtPayAmount.Text) + Convert.ToDouble(row.Cells[21].Text) + Convert.ToDouble(txtSubtractFee.Text));

                    objinfoMoneyReceive.Balance = (double.Parse(row.Cells[6].Text) - (double.Parse(txtPayAmount.Text) + double.Parse(txtSubtractFee.Text)));
                    if (objinfoMoneyReceive.Balance < 0)
                        objinfoMoneyReceive.Balance = 0;

                    if (DropDownListAccount.SelectedItem.Text == "")
                        objinfoMoneyReceive.BankCd =  null;
                    else
                        objinfoMoneyReceive.BankCd = Common.GetNullableInt(DropDownListAccount.SelectedValue);

                    if (txtComment.Text == "")
                        objinfoMoneyReceive.Note =  null;
                    else objinfoMoneyReceive.Note = txtComment.Text;

                    if (Convert.ToDouble(txtPayAmount.Text) > Convert.ToDouble(row.Cells[6].Text))
                    {
                        Error = total;
                    }
                    else
                    {
                        if ((Convert.ToDouble(txtPayAmount.Text) + Convert.ToDouble(txtSubtractFee.Text)) == Convert.ToDouble(row.Cells[6].Text))
                            objinfoMoneyReceive.PayCompleteFlg = 1;
                        else if (Convert.ToDouble(txtPayAmount.Text) < Convert.ToDouble(row.Cells[6].Text))
                            objinfoMoneyReceive.PayCompleteFlg =2;
                        else
                            objinfoMoneyReceive.PayCompleteFlg = null;


                        if (Convert.ToDouble(row.Cells[6].Text) - (Convert.ToDouble(txtPayAmount.Text) + Convert.ToDouble(txtSubtractFee.Text)) == Convert.ToDouble(row.Cells[5].Text) && row.Cells[5].Text != "0")
                        {
                            objinfoMoneyReceive.PayCompleteFlg = null;
                        }

                         objinfoMoneyReceive.ModifiedAccount = this.User.Identity.Name;
                         objinfoMoneyReceive.CreateAccount = this.User.Identity.Name;

                        objinfoMoneyReceive.CreateDate = objinfoMoneyReceive.ModifiedDate = DateTime.Now;
                        objinfoMoneyReceive.Count = 0;
                        objinfoMoneyReceive.Message = GetResource("Title_Error.Text");

                        MoneyReceiveSearchInfo.TransactionMoneyRecieve(objinfoMoneyReceive);

                    }
                }
            }
            // not check
            if (total < 1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
            }
            if (Error != 0)
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(Localization.GetString("Title_Error.Text", LocalResourceFile) + " " + GetResource("Titleresults.Text") + " " + Error, Localization.GetString("Title_PayMentAmount_Less_Than_Billing_Amount.Text", LocalResourceFile)) + "\");", true);
            }
            btnSearch_Click(sender, e);
        }
        catch (Exception ex)
        {
             logger.Error("Error Register", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        
        }
    }
    protected void grvMoneyReceive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtPayAmount = (TextBox)e.Row.FindControl("txtPayAmount");
            TextBox txtSubtractFee = (TextBox)e.Row.FindControl("txtSubtractFee");
            TextBox txtSumPayAmount = (TextBox)e.Row.FindControl("txtSumPayAmount");
            TextBox txtCurrentPaymentDate = (TextBox)e.Row.FindControl("txtCurrentPaymentDate");

            txtPayAmount.Attributes.Add("onblur", "AddTotal();");
            txtSubtractFee.Attributes.Add("onblur", "AddTotal();");
            txtSumPayAmount.Attributes.Add("onblur", "AddTotal();");

            CompareValidator Val_Amount = (CompareValidator)e.Row.FindControl("Val_Amount");
            CompareValidator Val_ServiceCharge = (CompareValidator)e.Row.FindControl("Val_ServiceCharge");
            CompareValidator Val_CurrentpayDate = (CompareValidator)e.Row.FindControl("Val_CurrentpayDate");
            RequiredFieldValidator Val_ReqDateTime = (RequiredFieldValidator)e.Row.FindControl("Val_ReqDateTime");

            Val_Amount.ErrorMessage = GetResource("DoubleOnly.Text");
            Val_ServiceCharge.ErrorMessage = GetResource("DoubleOnly.Text");
            Val_CurrentpayDate.ErrorMessage = GetResource("DateOnly.Text");
            Val_ReqDateTime.ErrorMessage = GetResource("Val_ReqDateTime.Text");
        }
    }
    protected void grvMoneyReceive_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvMoneyReceive.PageIndex = e.NewPageIndex;
        FillDataGridview();
    }
    protected void txtPayerCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtPayerCd, DropDownPayer);
        DropDownPayer_SelectedIndexChanged(sender, e);
    }
    protected void DropDownPayer_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPayerCd.Text = DropDownPayer.SelectedValue;   
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtPayerCd.Text = "";
        DropDownPayer.Text = "";
        txtIssueDateFrom.Text = "";
        txtIssueDateTo.Text = "";
        txtPayDateFrom.Text = "";
        txtPayDateTo.Text = "";
        txtComment.Text = "";
        DropDownListAccount.Text = "";
        txtServiceCharge.Text = "";
        txtTotalPayment.Text = "";
        txtPaymentAmount.Text = "";
        txtBillStatementNo.Text = "";
    }
}