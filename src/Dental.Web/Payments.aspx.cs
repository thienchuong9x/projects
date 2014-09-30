using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using log4net;
using Dental.Utilities;
using Dental.Domain;
using System.Data.SqlClient;
using System.Configuration;

public partial class Payments : DDVPortalModuleBase
{   
    #region "Event Handlers"
       
    PaymentsInfo objPaymentInfo = new PaymentsInfo();
    int flag;
    #region SaveCheckOfOtherPage
    private void RemoveViewState()
    {
        if (ViewState["PaymentlistView"] != null)
        {
            ViewState.Remove("PaymentlistView");
        }
    }
    public Hashtable HashCheckArray
    {
        get
        {
            if (this.ViewState["PaymentlistView"] == null)
            {
                return new Hashtable();
            }
            return this.ViewState["PaymentlistView"] as Hashtable;
        }
    }
    private void AddHashCheckArray(Hashtable hash)
    {
        if (ViewState["PaymentlistView"] == null)
        {
            ViewState.Add("PaymentlistView", hash);
        }
        else
        {
            ViewState["PaymentlistView"] = hash;
        }
    }
    private void SaveCheckBox()
    {
        Hashtable hash = this.HashCheckArray;
        int beginItem = grvPayment.PageIndex * grvPayment.PageSize;
        foreach (GridViewRow msgRow in grvPayment.Rows)
        {
            CheckBox ck = (CheckBox)msgRow.FindControl("CheckEdit");
            if (ck.Checked == true)
            {
                if (!hash.Contains(beginItem + msgRow.RowIndex))
                {
                    hash.Add((beginItem + msgRow.RowIndex), GetPaymentamentInfo(msgRow));
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
    private PaymentsInfo GetPaymentamentInfo(GridViewRow msgRow)
    {
        PaymentsInfo obj = new PaymentsInfo();
        //gvOrderList.Rows[msgRow.RowIndex].Cells[ConstantIndex.index_OrderSeq].Text + "_" + gvOrderList.Rows[msgRow.RowIndex].Cells[ConstantIndex.index_DetailSeq].Text;
        obj.OfficeCd = Convert.ToInt32(HiddenFieldOfficeCd.Value);
        obj.SupplierOutsourceCd = int.Parse(grvPayment.Rows[msgRow.RowIndex].Cells[ConstantIndex.index_SupplierOutsourceCd].Text);
        obj.PurchaseSeq = int.Parse(grvPayment.Rows[msgRow.RowIndex].Cells[ConstantIndex.index_PurchaseSeq].Text);
        return obj;
    }
    private void RestoreCheckBox()
    {
        if (this.HashCheckArray != null)
        {
            int beginItem = grvPayment.PageIndex * grvPayment.PageSize;
            foreach (GridViewRow msgRow in grvPayment.Rows)
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("CheckEdit");
                if (this.HashCheckArray.Contains(beginItem + msgRow.RowIndex))
                    ck.Checked = true;
            }
        }
    }
    #endregion
    /// ----------------------------------------------------------------------------- 
    /// <summary> 
    /// Page_Load runs when the control is loaded 
    /// </summary> 
    /// ----------------------------------------------------------------------------- 
    private static readonly ILog logger = LogManager.GetLogger(typeof(Payments).Name);
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            FillCalendar();
            if (!IsPostBack)
            {
                InitLanguage();
                logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
                //HttpCookie ck = Request.Cookies[this.User.Identity.Name]; //Request.Cookies("test");
                //if (ck != null)
                //{
                //    HiddenFieldOfficeCd.Value = ck["OfficeCd"];
                //}
                //else HiddenFieldOfficeCd.Value = "-1";
                HiddenFieldOfficeCd.Value = GetOffice();

                //Get Message Resource                      
                lbSellerCd.Text = GetResource("lbSeller.Text");
                lbBuyDate.Text = GetResource("lbBuyDate.Text");
                //lbPaymentDate.Text = GetResource("lbPaymentDate.Text");
                lbTotalAmount.Text = GetResource("lbTotalAmount.Text");
                lbBankDetails.Text = GetResource("lbBankDetails.Text");
                lbChkCompleted.Text = GetResource("chkCompleted.Text");
                lbOutsoureLab.Text = GetResource("lbOutsourceLab.Text");
                lnMessage.ForeColor = System.Drawing.Color.Red;
                //TextBox1.Visible = false;

                btnSearch.Text = GetResource("btnSearch.Text");
                btnClear.Text = GetResource("btnClear.Text");
                btnRegister.Text = GetResource("btnRegister.Text");

                Val_BuyDateFrom.ErrorMessage = GetResource("DateOnly.Text");
                Val_BuyDateTo.ErrorMessage = GetResource("DateOnly.Text");

                // Fill data to Gridview
                FillDropDownSellerNm();
                FillDropDownBankDetail();
                FillDropDownOutsourceLab();

                List<PaymentsInfo> listPayment = new List<PaymentsInfo>();
                FillDataGridview(listPayment);
                lbMessage.Text = string.Empty;
                RemoveViewState();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    #endregion

    /// <summary>
    /// Fill
    /// </summary>
    /// 
    #region "Fill All"
    private void FillCalendar()
    {
        hplBuyDateFrom.NavigateUrl = Calendar.InvokePopupCal(txtBuyDateFrom);
        hplBuyDateTo.NavigateUrl = Calendar.InvokePopupCal(txtBuyDateTo);       
    }
    /// <summary>
    /// Fill DataGridview 
    /// </summary>
    private void FillDataGridview(List<PaymentsInfo> listPayment)
    {
        flag = 2;
        txtTotalAmount.Text = "";
        grvPayment.Columns[9].Visible = true;
        grvPayment.Columns[11].Visible = true;
        grvPayment.Columns[12].Visible = true;
        grvPayment.Columns[10].Visible = true;
        grvPayment.Columns[13].Visible = true;
        grvPayment.Columns[14].Visible = true;        
        // gridview not Record
        if (listPayment.Count == 0)
        {
            btnRegister.Enabled = false;
            PaymentsInfo objPayment = new PaymentsInfo();
            objPayment.OfficeCd = -1;
            objPayment.SupplierOutsourceCd = -1;
            objPayment.PurchaseSeq = -1;
            objPayment.PayDate = DateTime.Now.ToShortDateString();
            objPayment.PurchaseDate = DateTime.Now.Date;
            listPayment.Add(objPayment);

            grvPayment.DataSource = listPayment;
            grvPayment.DataBind();

            if (listPayment.Count == 1 && listPayment[0].SupplierOutsourceCd == -1)
            {
                grvPayment.Rows[0].Visible = false;
            }
            lbMessage.Text = GetResource("NoRecordFound.Text");
            DropDownListBankDetails.Text = "";
        }
        // Gridview not record
        else if (flag == 1)
        {
            btnRegister.Enabled = false;
            PaymentsInfo objPayment = new PaymentsInfo();
            objPayment.OfficeCd = -1;
            objPayment.SupplierOutsourceCd = -1;
            objPayment.PurchaseSeq = -1;
            objPayment.PayDate = DateTime.Now.ToShortDateString();
            objPayment.PurchaseDate = DateTime.Now.Date;
            listPayment = new List<PaymentsInfo>();
            listPayment.Add(objPayment);
            grvPayment.DataSource = listPayment;
            grvPayment.DataBind();

            if (listPayment.Count == 1 && listPayment[0].SupplierOutsourceCd == -1)
            {
                grvPayment.Rows[0].Visible = false;
            }
            lbMessage.Text = GetResource("NoRecordFound.Text");
            DropDownListBankDetails.Text = "";

        }
        // Load data to Gridview
        else
        {
            btnRegister.Enabled = true;
            grvPayment.DataSource = listPayment;
            grvPayment.DataBind();
            lbMessage.Text = string.Empty;
            foreach (GridViewRow row in grvPayment.Rows)
            {
                if (Common.GetRowString(row.Cells[1].Text) != "")
                    row.Cells[1].Text = SetDateFormat(Convert.ToDateTime(Common.GetRowString(row.Cells[1].Text)).ToShortDateString());
                else
                    row.Cells[1].Text = "";
                //Balance = Amount
                if (Common.GetRowString(row.Cells[6].Text) == "")
                    row.Cells[6].Text = Common.GetRowString(row.Cells[5].Text);

                ListItem item = DropDownListOutsourcelab.Items.FindByValue(Common.GetRowString(row.Cells[11].Text));
                if (item != null)
                    row.Cells[3].Text = item.Text;
                else row.Cells[3].Text = "";

                ListItem item1 = DropDownListSeller.Items.FindByValue(Common.GetRowString(row.Cells[11].Text));
                if (item1 != null)
                    row.Cells[2].Text = item1.Text;
                else row.Cells[2].Text = "";

                TrnOutsource objTrnOutsource = TrnOutsource.GetTrnOutsourceByTrnPurchase(Convert.ToInt32(HiddenFieldOfficeCd.Value), Convert.ToInt32(row.Cells[11].Text), Convert.ToInt32(row.Cells[10].Text));
                if (objTrnOutsource == null)
                    row.Cells[3].Text = "";
                else
                    row.Cells[2].Text = "";

                TextBox txtPayAmount = (TextBox)row.FindControl("txtCurrentPaymentAmount");
                TextBox txtSubtractFee = (TextBox)row.FindControl("txtServiceCharge");

                CompareValidator Val_Amount = (CompareValidator)row.FindControl("Val_Amount");
                CompareValidator Val_ServiceCharge = (CompareValidator)row.FindControl("Val_ServiceCharge");

                Val_Amount.ErrorMessage = GetResource("DoubleOnly.Text");
                Val_ServiceCharge.ErrorMessage = GetResource("DoubleOnly.Text");

                txtPayAmount.ReadOnly = true;
                txtSubtractFee.ReadOnly = true;
            }
            lbMessage.Text = GetResource("lbResult.Text") + listPayment.Count.ToString();
        }
        grvPayment.Columns[9].Visible = false;
        grvPayment.Columns[10].Visible = false;
        grvPayment.Columns[11].Visible = false;
        grvPayment.Columns[12].Visible = false;
        grvPayment.Columns[13].Visible = false;
        grvPayment.Columns[14].Visible = false;        

    }
    /// <summary>
    /// Fill DropDownlist Supplier
    /// </summary>
    /// 
    private void FillDropDownSellerNm()
    {
        DropDownListSeller.Items.Add(new ListItem("", ""));
        List<MasterSupplier> listSupplier = MasterSupplier.GetSupplierMasters(Convert.ToInt32(HiddenFieldOfficeCd.Value));
        foreach (MasterSupplier i in listSupplier)
        {
            DropDownListSeller.Items.Add(new ListItem(i.SupplierNm, i.SupplierCd.ToString()));
        }
    }
    /// <summary>
    /// Fill DropDownlist outsourceLab
    /// </summary>
    /// 
    private void FillDropDownOutsourceLab()
    {
        DropDownListOutsourcelab.Items.Add(new ListItem("", ""));           
        List<MasterOutsourceLab> listOutsourceLab = MasterOutsourceLab.GetOutsourceLabMasters(Convert.ToInt32(HiddenFieldOfficeCd.Value));
        foreach (MasterOutsourceLab i in listOutsourceLab)
        {
            DropDownListOutsourcelab.Items.Add(new ListItem(i.OutsourceNm, i.OutsourceCd.ToString()));
        }
    }
    /// <summary>
    /// Fill DropDown BankDetails
    /// </summary>
    /// <param name="show"></param>
    private void FillDropDownBankDetail()
    {            
        DropDownListBankDetails.Items.Add(new ListItem("", ""));
        List<MasterBank> listBank = MasterBank.GetBankForFlg(Convert.ToInt32(HiddenFieldOfficeCd.Value), true, false);
        foreach (MasterBank i in listBank)
        {
            DropDownListBankDetails.Items.Add(new ListItem(i.BankAccount, i.BankCd.ToString()));
        }
    }
    #endregion

    #region "Text Changed"   
    /// <summary>
    ///  DropDownListSeller Changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownListSeller_SelectedIndexChanged(object sender, EventArgs e)
    {        
        txtSellerCd.Text = DropDownListSeller.SelectedValue;
    }    
    /// <summary>
    ///  DropDownListOutsourcelab Changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DropDownListOutsourcelab_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtOutsoureCd.Text = DropDownListOutsourcelab.SelectedValue;
    }
    #endregion
    /// <summary>
    /// Event Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    #region "Event Click"

    protected void CheckEdit_Checked(object sender, EventArgs e)
    {
        int check = 0;
        double totalPay = 0;
        foreach (GridViewRow row in grvPayment.Rows)
        {
            TextBox txtPayAmount = (TextBox)row.FindControl("txtCurrentPaymentAmount");
            TextBox txtSubtractFee = (TextBox)row.FindControl("txtServiceCharge");

            txtPayAmount.Attributes.Add("onblur", "AddTotal();");

            CheckBox cb = (CheckBox)row.FindControl("CheckEdit");// check CheckBox checked             

            // Checked
            if (cb.Checked)
            {
                check++;
                txtPayAmount.ReadOnly = false;
                txtSubtractFee.ReadOnly = false;

                if (txtPayAmount.Text == "")
                {
                    txtPayAmount.Text = row.Cells[6].Text;
                }
                totalPay = Math.Round(totalPay + Convert.ToDouble(txtPayAmount.Text), 10);
            }
            // not checked
            else
            {
                txtPayAmount.ReadOnly = true;
                txtSubtractFee.ReadOnly = true;
                txtPayAmount.Text = "";
                txtSubtractFee.Text = "";
            }
        }
        txtTotalAmount.Text = totalPay.ToString();

    }
    /// <summary>
    /// Event Search
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            int check;
            DropDownListBankDetails.Text = "";
            List<PaymentsInfo> listPayment = new List<PaymentsInfo>();
            if (chkCompleted.Checked)
                check = 1;
            else
                check = 0;
            //Search   
            listPayment = PaymentsInfo.GetPaymentsInfoSearch(txtSellerCd.Text, txtOutsoureCd.Text, txtBuyDateFrom.Text, txtBuyDateTo.Text, Convert.ToInt32(HiddenFieldOfficeCd.Value), check);
            RemoveViewState();
            ViewState.Add("PaymentlistView", listPayment);
            FillDataGridview((List<PaymentsInfo>)ViewState["PaymentlistView"]);
            DropDownListSeller.SelectedValue = txtSellerCd.Text;
            DropDownListOutsourcelab.SelectedValue = txtOutsoureCd.Text;
        }
        catch (Exception ex)
        {
            logger.Error("Error btnSearch_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    /// <summary>
    /// Event Clear
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtBuyDateFrom.Text = "";
        txtBuyDateTo.Text = "";
        txtSellerCd.Text = "";        
        DropDownListSeller.Text = "";
        txtTotalAmount.Text = "";
        txtOutsoureCd.Text = "";
        DropDownListOutsourcelab.Text = "";
        DropDownListBankDetails.Text = "";
    }
    /// <summary>
    /// Event Register
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {                
            //PaymentsInfo objinfo1 = new PaymentsInfo();
            PaymentsInfo objinfo = new PaymentsInfo();
            List<PaymentsInfo> ListPaymentTemp = new List<PaymentsInfo>();
            int total = 0;
            #region Old
            #region
            foreach (GridViewRow row in grvPayment.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("CheckEdit");
                TextBox txtCurrentPaymentAmount = (TextBox)row.FindControl("txtCurrentPaymentAmount");
                TextBox txtServiceCharge = (TextBox)row.FindControl("txtServiceCharge");
                double d;

                if (cb.Checked)
                {
                    total++;

                    objinfo.OfficeCd = Convert.ToInt32(row.Cells[9].Text);
                    objinfo.PurchaseSeq = Convert.ToInt32(row.Cells[10].Text);
                    objinfo.SupplierOutsourceCd = Convert.ToInt32(row.Cells[11].Text);
                    objinfo.PayDate = DateTime.Now.ToString();
                    objinfo.Note = Common.GetRowString(row.Cells[14].Text).ToString();
                    // Check data in textbox 
                    if (txtCurrentPaymentAmount.Text == "")
                        txtCurrentPaymentAmount.Text = "0";

                    if (!Double.TryParse(txtCurrentPaymentAmount.Text, out d))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("DoubleOnly.Text")) + "\");", true);
                    }
                    else
                    {
                        if (Convert.ToDouble(txtCurrentPaymentAmount.Text) > Convert.ToDouble(row.Cells[6].Text))
                        {
                            lnMessage.Text = GetResource("Current_amount_and_Balance.Text");
                        }
                        else
                        {
                            objinfo.PayPrice = txtCurrentPaymentAmount.Text;
                        }
                    }

                    if (txtServiceCharge.Text == "")
                        txtServiceCharge.Text = "0";

                    objinfo.PayFee = txtServiceCharge.Text;
                    objinfo.PurchasePrice = Common.GetNullableDouble(row.Cells[5].Text);

                    if (Common.GetRowString(row.Cells[13].Text) == "")
                        row.Cells[13].Text = "0";

                    objinfo.PaidMoney = Common.GetNullableDouble(Math.Round(Convert.ToDouble(Common.GetRowString(row.Cells[13].Text)) + Convert.ToDouble(objinfo.PayPrice), 10).ToString());
                    objinfo.Balance = Common.GetNullableDouble(Math.Round(Convert.ToDouble(Common.GetRowString(row.Cells[6].Text)) - Convert.ToDouble(objinfo.PayPrice), 10).ToString());

                    if (DropDownListBankDetails.SelectedItem.Text == "")
                        objinfo.BankCd = null;
                    else
                        objinfo.BankCd = DropDownListBankDetails.SelectedValue;

                    using (DBContext DBContext = new DBContext())
                    {
                        System.Data.Common.DbTransaction tran = DBContext.UseTransaction();                        
                        try
                        { 
                            objinfo.CreateDate = objinfo.ModifiedDate = DateTime.Now;
                            objinfo.CreateAccount = objinfo.ModifiedAccount = this.User.Identity.Name;

                            TrnPurchase objTrnPurchase = new TrnPurchase();
                            TrnPayHistory objTrnPay = new TrnPayHistory();

                            objTrnPurchase.OfficeCd = objinfo.OfficeCd;
                            objTrnPurchase.SupplierOutsourceCd = objinfo.SupplierOutsourceCd;
                            objTrnPurchase.PurchaseSeq = objinfo.PurchaseSeq.Value;
                            objTrnPurchase.PurchaseDate = Common.GetNullableDateTime(row.Cells[1].Text);
                            objTrnPurchase.PurchaseCategory = Common.GetNullableInt(row.Cells[12].Text);
                            objTrnPurchase.PurchaseItems = row.Cells[4].Text;
                            objTrnPurchase.PurchasePrice = objinfo.PurchasePrice;
                            objTrnPurchase.RegularPrice = objinfo.RegularPrice;
                            objTrnPurchase.PurchaseFee = objinfo.PurchaseFee;
                            objTrnPurchase.PaidMoney = objinfo.PaidMoney;
                            objTrnPurchase.Balance = objinfo.Balance;
                            objTrnPurchase.Note = Common.GetRowString(row.Cells[14].Text).ToString();

                            objTrnPurchase.ModifiedAccount = objTrnPay.ModifiedAccount = objinfo.ModifiedAccount;
                            objTrnPurchase.ModifiedDate = objTrnPay.ModifiedDate = objinfo.ModifiedDate;
                            objTrnPurchase.CreateAccount = objTrnPay.CreateAccount = objinfo.CreateAccount;
                            objTrnPurchase.CreateDate = objTrnPay.CreateDate = objinfo.CreateDate;


                            objTrnPay.OfficeCd = objinfo.OfficeCd;
                            objTrnPay.SupplierOutsourceCd = objinfo.SupplierOutsourceCd;
                            objTrnPay.PurchaseSeq = objinfo.PurchaseSeq.Value;
                            objTrnPay.PayDate = DateTime.Now;
                            objTrnPay.PayPrice = Common.GetNullableDouble(objinfo.PayPrice);
                            objTrnPay.PayFee = Common.GetNullableDouble(objinfo.PayFee);
                            objTrnPay.BankCd = Common.GetNullableInt(objinfo.BankCd);

                            objTrnPurchase.Update();
                            objTrnPay.Insert();
                            tran.Commit();                 
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();                            
                            throw ex;
                        }
                    }                    
                }
            }
            #endregion
            #endregion

            #region New
            //foreach (GridViewRow row in grvPayment.Rows)
            //{
            //    CheckBox cb = (CheckBox)(row.FindControl("CheckEdit"));
            //    if (cb.Checked)
            //    {
            //        objinfo1 = objCon.GetPayMentData(row.Cells[11].Text, Convert.ToInt32(txtOfficeCd.Text), row.Cells[10].Text);
            //        ListPaymentTemp.Add(objinfo1);
            //    }
            //}
            //for (int ii = 0; ii < ListPaymentTemp.Count; ii++)
            //{
            //    total++;

            //}
            #endregion
            if (total < 1)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
            }
            else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Success.Text"), "") + "\");", true);
            btnSearch_Click(sender, e);
        }
        catch (Exception ex)
        {
            logger.Error("btnRegister_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    #endregion

    #region "Addrow"
    /// <summary>
    /// Addrow
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvPayment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Payment custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            // Add checkAll
            TableHeaderCell tcCheckBox = new TableHeaderCell();
            CheckBox chkCheckAll = new CheckBox();
            chkCheckAll.ID = "chkCheckAll";
            chkCheckAll.Attributes["onclick"] = "javascript:checkAll(this);";
            chkCheckAll.AutoPostBack = true;
            chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            tcCheckBox.Controls.Add(chkCheckAll);
            tcCheckBox.CssClass = "HeadwithBG";
            oGridViewRow.Cells.Add(tcCheckBox);

            //Add Buydate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbBuyDate.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(90);

            oGridViewRow.Cells.Add(oTableCell);

            //Add SellerNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("SellerNm.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(250);
            oGridViewRow.Cells.Add(oTableCell);

            //Add OutsourceNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOutsourceNm.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(250);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PurchaseItem
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("PurchaseItem.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(250);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Amount
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("Amount_Amount.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add Balance
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("Balance.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add CurrentPaymentAmount.Text
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("CurrentPaymentAmount.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add ServiceCharge.Text
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("ServiceCharge.Text");
            oTableCell.ColumnSpan = 1;
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    #endregion
    /// <summary>
    /// Paging
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvPayment.PageIndex = e.NewPageIndex;
        FillDataGridview((List<PaymentsInfo>)ViewState["PaymentlistView"]);
    }
    /// <summary>
    /// Rowdatabound set event onclick textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grvPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtCurrentPaymentAmount = (TextBox)e.Row.FindControl("txtCurrentPaymentAmount");
            txtCurrentPaymentAmount.Attributes.Add("onblur", "AddTotal();");

            CompareValidator Val_Amount = (CompareValidator)e.Row.FindControl("Val_Amount");
            CompareValidator Val_ServiceCharge = (CompareValidator)e.Row.FindControl("Val_ServiceCharge");

            Val_Amount.ErrorMessage = GetResource("DoubleOnly.Text");
            Val_ServiceCharge.ErrorMessage = GetResource("DoubleOnly.Text");
        }

    }
    protected void chkCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow rows in grvPayment.Rows)
        {
            CheckEdit_Checked(sender, e);
        }
    }
    public string ResourceJavaScript()
    {
        return GetResource("Current_amount_and_Balance.Text");
    }
}
