using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class OrderList : DDVPortalModuleBase
{
    private OrderSearchInfo objSearch = new OrderSearchInfo();
    private string mMale = "Male";
    private string mFemale = "Female";
    private string mUnknow = "unknow";
    readonly static ILog logger = LogManager.GetLogger(typeof(OrderList));

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            hplFromDate.NavigateUrl = Calendar.InvokePopupCal(txtFromDate);
            hplToDate.NavigateUrl = Calendar.InvokePopupCal(txtToDate);
            hplDeliveryFromDate.NavigateUrl = Calendar.InvokePopupCal(txtDeliveryFromDate);
            hplDeliveryToDate.NavigateUrl = Calendar.InvokePopupCal(txtDeliveryToDate);
            //btlCad.Attributes["onclick"] = "javascript:return showFolder();";

            if (!IsPostBack)
            {
                InitLanguage();
                hiddenOfficeCd.Value = GetOffice();
                //Page.Title = GetResource("OrderList.String");
                logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
                Initiation(sender, e);

                //Get Staff
                FillStaffDropDownList();

                //Get DentalOffice
                FillDentalOfficeDropDownList();
                txtClinicName.Text = "";
                txtClinicName1.Text = "";

                //Get Prosthesis
                FillProsthesisDropDownList();

                //Get Contractor
                FillOutsourceLabDropDownList();

                //Create List empty
                OrderListInfo objOL = new OrderListInfo();
                objOL.OrderSeq = -1;
                objOL.OrderNo = " ";
                objOL.OfficeCd = 9;
                objOL.OrderDate = DateTime.Today;
                objOL.DeliverDate = DateTime.Today;
                objOL.SetDate = DateTime.Today;
                objOL.DentalOfficeCd = 1;
                List<OrderListInfo> colOrderList = new List<OrderListInfo>();
                colOrderList.Add(objOL);

                gvOrderList.DataSource = colOrderList;
                gvOrderList.DataBind();

                if (colOrderList.Count == 1 && colOrderList[0].OrderSeq == -1)
                {
                    gvOrderList.Rows[0].Visible = false;
                }

                //Lay gia tri cho textBox Search tu Cookies; Dat o Page_Load;
                HttpCookie ck1 = Request.Cookies[User.Identity.Name + "_orderlist"];
                if (ck1 != null)
                {
                    txtOrderNo.Text = ck1["OrderNo"];
                    txtFromDate.Text = ck1["FromDate"];
                    txtToDate.Text = ck1["ToDate"];
                    txtDeliveryFromDate.Text = ck1["DeliveryFromDate"];
                    txtDeliveryToDate.Text = ck1["DeliveryToDate"];
                    txtSalesman.Text = ck1["StaffCd"];
                    if (!string.IsNullOrWhiteSpace(txtSalesman.Text))
                        txtSalesman_TextChanged(sender, e);
                    txtClinicName.Text = ck1["DentalOfficeCd"];
                    if (!string.IsNullOrWhiteSpace(txtClinicName.Text))
                        txtClinicName_TextChanged(sender, e);
                    txtPatient.Text = ck1["Patient"];
                    txtProstheticType.Text = ck1["ProstheticType"];
                    if (!string.IsNullOrWhiteSpace(txtProstheticType.Text))
                        txtProstheticType_TextChanged(sender, e);
                    txtContractorId.Text = ck1["ContractorId"];
                    if (!string.IsNullOrWhiteSpace(txtContractorId.Text))
                        txtContractorId_TextChanged(sender, e);

                    if (ck1["CompletedBill"] == "1")
                        cbxCompletedBill.Checked = true;
                    else cbxCompletedBill.Checked = false;

                    if (ck1["CompletedDelivery"] == "1")
                        cbxCompletedDelivery.Checked = true;
                    else cbxCompletedDelivery.Checked = false;

                    if (ck1["OnlyTrialOrder"] == "1")
                        cbxOnlyTrialOrder.Checked = true;
                    else cbxOnlyTrialOrder.Checked = false;

                    if (ck1["OnlyRemanufacture"] == "1")
                        cbxOnlyRemanufacture.Checked = true;
                    else cbxOnlyRemanufacture.Checked = false;

                    dlNumber.SelectedValue = ck1["PageSize"];
                    gvOrderList.PageSize = int.Parse(dlNumber.SelectedValue);

                    btOk_Click(sender, e);
                }


            }
        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");");
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        //CheckBox1.Checked = true;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        //CheckBox1.Checked = true;
    }

    protected void txtDeliveryFromDate_TextChanged(object sender, EventArgs e)
    {
        //CheckBox1.Checked = true;
    }
    protected void txtDeliveryToDate_TextChanged(object sender, EventArgs e)
    {
        //CheckBox1.Checked = true;
    }

     
    protected void btOk_Click(object sender, EventArgs e)
    {
        SetValueForObjectSearch();

        List<OrderListInfo> colOrderList;
        colOrderList = OrderListInfo.GetOrderListSearch(objSearch); 

        if (colOrderList.Count == 0)
        {
            //Create List empty
            OrderListInfo objOL = new OrderListInfo();
            objOL.OrderSeq = -1;
            objOL.OrderNo = " ";
            objOL.OrderDate = DateTime.Today;
            objOL.DeliverDate = DateTime.Today;
            objOL.SetDate = DateTime.Today;

            colOrderList.Add(objOL);

            gvOrderList.DataSource = colOrderList;
            gvOrderList.DataBind();

            gvOrderList.Columns[1].Visible = false;

            if (colOrderList.Count == 1 && colOrderList[0].OrderSeq == -1)
            {
                gvOrderList.Rows[0].Visible = false;
            }

            lbMessage.Text = GetResource("NoRecordFound.Text");
            return;
        }


        gvOrderList.DataSource = colOrderList;
        gvOrderList.DataBind();

        foreach (GridViewRow row in gvOrderList.Rows)
        {
            if (Common.GetRowString(row.Cells[3].Text) != "")
                row.Cells[3].Text = getStaffName(row.Cells[3].Text);

            if (Common.GetRowString(row.Cells[4].Text) != "")
                row.Cells[4].Text = Convert.ToDateTime(row.Cells[4].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[5].Text) != "")
                row.Cells[5].Text = Convert.ToDateTime(row.Cells[5].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[6].Text) != "")
                row.Cells[6].Text = Convert.ToDateTime(row.Cells[6].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[7].Text) != "")
                row.Cells[7].Text = getOfficeName(row.Cells[7].Text);

            if (row.Cells[10].Text == "1")
                row.Cells[10].Text = mMale; //GetResource("Insurance.Text");
            else
                if (row.Cells[10].Text == "2")
                    row.Cells[10].Text = mFemale;
                else
                    row.Cells[10].Text = mUnknow;
        }


        gvOrderList.Columns[1].Visible = false;
        lbMessage.Text = GetResource("lbResult.Text") + " " + colOrderList.Count.ToString(); // +" results"; //string.Empty;


        //Luu cookie ve cac thong so search
        HttpCookie ck1 = Request.Cookies[User.Identity.Name + "_orderlist"];  //UserInfo.Username
        if (ck1 == null)
        {
            ck1 = new HttpCookie(User.Identity.Name + "_orderlist"); //UserInfo.Username
        }
        ck1["OrderNo"] = txtOrderNo.Text;
        ck1["FromDate"] = txtFromDate.Text;
        ck1["ToDate"] = txtToDate.Text;
        ck1["DeliveryFromDate"] = txtDeliveryFromDate.Text;
        ck1["DeliveryToDate"] = txtDeliveryToDate.Text;
        ck1["StaffCd"] = txtSalesman.Text;
        ck1["DentalOfficeCd"] = txtClinicName.Text;
        ck1["Patient"] = txtPatient.Text;
        ck1["ProstheticType"] = txtProstheticType.Text;
        ck1["ContractorId"] = txtContractorId.Text;
        if (cbxCompletedBill.Checked)
            ck1["CompletedBill"] = "1";
        else ck1["CompletedBill"] = "0";
        if (cbxCompletedDelivery.Checked)
            ck1["CompletedDelivery"] = "1";
        else ck1["CompletedDelivery"] = "0";
        if (cbxOnlyTrialOrder.Checked)
            ck1["OnlyTrialOrder"] = "1";
        else ck1["OnlyTrialOrder"] = "0";
        if (cbxOnlyRemanufacture.Checked)
            ck1["OnlyRemanufacture"] = "1";
        else ck1["OnlyRemanufacture"] = "0";
        ck1["PageSize"] = dlNumber.SelectedValue;
        ck1.Expires = DateTime.Now.AddDays(15);
        Response.Cookies.Add(ck1);

        //btHideSearch_Click(sender, e);

    }

    protected void btShowSearch_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        //btShowSearch.Visible = false;
        //btHideSearch.Visible = true;

        btlShowSearch.Visible = false;
        btlHideSearch.Visible = true;

    }

    protected void btHideSearch_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
        //btShowSearch.Visible = true;
        //btHideSearch.Visible = false;

        btlShowSearch.Visible = true;
        btlHideSearch.Visible = false;

    }

    private void SetValueForObjectSearch()
    {
        try
        {
            //objSearch.OfficeCd = int.Parse(txtOffice.Text);
            objSearch.OfficeCd = int.Parse(hiddenOfficeCd.Value);
        }
        catch
        {
            objSearch.OfficeCd = -1;
            //CHo dai 1 gia tri, Se ko ra ket qua nao dau 
        }
        objSearch.OrderNo = txtOrderNo.Text;
        objSearch.OrderDateFrom = txtFromDate.Text;
        objSearch.OrderDateTo = txtToDate.Text;
        objSearch.DeliverDateFrom = txtDeliveryFromDate.Text;
        objSearch.DeliverDateTo = txtDeliveryToDate.Text;
        objSearch.StaffCd = txtSalesman.Text;
        objSearch.DentalOfficeCd = txtClinicName.Text;
        objSearch.PatientLastNm = txtPatient.Text;
        objSearch.ProsthesisCd = txtProstheticType.Text;
        objSearch.ContractorCd = txtContractorId.Text;
        objSearch.BillCompleteFlg = cbxCompletedBill.Checked;
        objSearch.DeliveryCompleteFlg = cbxCompletedDelivery.Checked;
        objSearch.OnlyTrialOrderFlg = cbxOnlyTrialOrder.Checked;
        objSearch.OnlyReManufactureFlg = cbxOnlyRemanufacture.Checked;
    }

    private void GetValueFromCookie()
    {
        try
        {
            //objSearch.OfficeCd = int.Parse(txtOffice.Text);
            objSearch.OfficeCd = int.Parse(hiddenOfficeCd.Value);
        }
        catch
        {
            objSearch.OfficeCd = -1;
            //CHo dai 1 gia tri, Se ko ra ket qua nao dau 
        }

        //Lay gia tri cho textBox Search tu Cookies; Dat o Page_Load;
        HttpCookie ck1 = Request.Cookies[User.Identity.Name + "_orderlist"];
        if (ck1 != null)
        {
            objSearch.OrderNo = ck1["OrderNo"];
            objSearch.OrderDateFrom = ck1["FromDate"];
            objSearch.OrderDateTo = ck1["ToDate"];
            objSearch.DeliverDateFrom = ck1["DeliveryFromDate"];
            objSearch.DeliverDateTo = ck1["DeliveryToDate"];
            objSearch.StaffCd = ck1["StaffCd"];
            //if (!string.IsNullOrWhiteSpace(txtSalesman.Text))
            //    txtSalesman_TextChanged(sender,e);
            objSearch.DentalOfficeCd = ck1["DentalOfficeCd"];
            //if (!string.IsNullOrWhiteSpace(txtClinicName.Text))
            //    txtClinicName_TextChanged(sender, e);
            objSearch.PatientLastNm = ck1["Patient"];
            objSearch.ProsthesisCd = ck1["ProstheticType"];
            //if (!string.IsNullOrWhiteSpace(txtProstheticType.Text))
            //    txtProstheticType_TextChanged(sender, e);
            objSearch.ContractorCd = ck1["ContractorId"];
            //if (!string.IsNullOrWhiteSpace(txtContractorId.Text))
            //    txtContractorId_TextChanged(sender, e);

            if (ck1["CompletedBill"] == "1")
                objSearch.BillCompleteFlg = true;
            else objSearch.BillCompleteFlg = false;

            if (ck1["CompletedDelivery"] == "1")
                objSearch.DeliveryCompleteFlg = true;
            else objSearch.DeliveryCompleteFlg = false;

            if (ck1["OnlyTrialOrder"] == "1")
                objSearch.OnlyTrialOrderFlg = true;
            else objSearch.OnlyTrialOrderFlg = false;

            if (ck1["OnlyRemanufacture"] == "1")
                objSearch.OnlyReManufactureFlg = true;
            else objSearch.OnlyReManufactureFlg = false;

            //dlNumber.SelectedValue = ck1["PageSize"];
            //gvOrderList.PageSize = int.Parse(dlNumber.SelectedValue);
        }
    }

    protected void gvOrderList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            InitLanguage();

            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            #region "Make Headers"
            //Add CheckBoxSelected
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(20);
            oGridViewRow.Cells.Add(oTableCell);

            //Add OrderNo
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOrderNo.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(90);
            oGridViewRow.Cells.Add(oTableCell);

            //Add StaffCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbSalesman.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add OrderDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbOrderDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DeliverDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDeliveryDate_deliveryDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add SetDate
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbSetDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentalOfficeCd
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbClinicName.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(150);
            oGridViewRow.Cells.Add(oTableCell);

            //Add DentistNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbDentistNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PatientLastNm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbPatientNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(100);
            oGridViewRow.Cells.Add(oTableCell);

            ////Add PatientFirstNm
            //oTableCell = new TableHeaderCell();
            //oTableCell.Text = GetResource("lbPatientFirstNm.Text");
            //oTableCell.CssClass = "td_header";
            //oTableCell.Width = Unit.Pixel(70);
            //oGridViewRow.Cells.Add(oTableCell);

            //Add PatientSex
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbPatientSex.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(60);
            oGridViewRow.Cells.Add(oTableCell);

            //Add PatientAge
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lbPatientAge.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(60);
            oGridViewRow.Cells.Add(oTableCell);

            ////Add Note
            //oTableCell = new TableHeaderCell();
            //oTableCell.Text = GetResource("lbNote.Text");
            //oTableCell.CssClass = "td_header";
            //oTableCell.Width = Unit.Pixel(100);
            //oGridViewRow.Cells.Add(oTableCell);
            #endregion
            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void gvOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrderList.PageIndex = e.NewPageIndex;

        //Get module listing
        //OrderListController oController1 = new OrderListController();
        List<OrderListInfo> colOrderList;

        //SetValueForObjectSearch();
        GetValueFromCookie();

        colOrderList = OrderListInfo.GetOrderListSearch(objSearch); //oController1.GetOrderLists(objSearch);
        gvOrderList.DataSource = colOrderList;
        gvOrderList.DataBind();

        foreach (GridViewRow row in gvOrderList.Rows)
        {
            if (Common.GetRowString(row.Cells[3].Text) != "")
                row.Cells[3].Text = getStaffName(row.Cells[3].Text);

            if (Common.GetRowString(row.Cells[4].Text) != "")
                row.Cells[4].Text = Convert.ToDateTime(row.Cells[4].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[5].Text) != "")
                row.Cells[5].Text = Convert.ToDateTime(row.Cells[5].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[6].Text) != "")
                row.Cells[6].Text = Convert.ToDateTime(row.Cells[6].Text).ToShortDateString();

            if (Common.GetRowString(row.Cells[7].Text) != "")
                row.Cells[7].Text = getOfficeName(row.Cells[7].Text);

            if (row.Cells[10].Text == "1")
                row.Cells[10].Text = mMale; //GetResource("Insurance.Text");
            else
                if (row.Cells[10].Text == "2")
                    row.Cells[10].Text = mFemale;
                else
                    row.Cells[10].Text = mUnknow;
        }

        gvOrderList.Columns[1].Visible = false;
        //lbMessage.Text = "";
        lbMessage.Text = colOrderList.Count.ToString() + " results"; //string.Empty;
    }


    //protected void dlContractor_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtContractorId.Text = dlContractor.SelectedValue;
    //    txtContractorName.Text = dlContractor.SelectedItem.Text;
    //}

    //protected void txtContractorId_TextChanged(object sender, EventArgs e)
    //{
    //    Common.GetName(txtContractorId.Text, txtContractorName, dlContractor);
    //    dlContractor_SelectedIndexChanged(sender, e);
    //}

    protected void dlDentalOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtClinicName.Text = dlDentalOffice.SelectedValue;
        txtClinicName1.Text = dlDentalOffice.SelectedItem.Text;
    }

    protected void txtClinicName_TextChanged(object sender, EventArgs e)
    {
        GetName(txtClinicName.Text, txtClinicName1, dlDentalOffice);
        dlDentalOffice_SelectedIndexChanged(sender, e);
    }

    protected void dlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSalesman.Text = dlStaff.SelectedValue;
        txtSalesman1.Text = dlStaff.SelectedItem.Text;

        //Get DentalOffice with Staff
        dlDentalOffice.Items.Clear();
        dlDentalOffice.Items.Add(new ListItem("", ""));
        List<MasterDentalOffice> list = MasterDentalOffice.GetDentalOfficeMasterSearch(int.Parse(hiddenOfficeCd.Value), string.Empty, string.Empty, txtSalesman.Text);
        foreach (MasterDentalOffice i in list)
        {
            dlDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }

        txtClinicName.Text = "";
        txtClinicName1.Text = "";
    }

    protected void txtSalesman_TextChanged(object sender, EventArgs e)
    {
        GetName(txtSalesman.Text, txtSalesman1, dlStaff);
        dlStaff_SelectedIndexChanged(sender, e);

        //Get DentalOffice with Staff
        dlDentalOffice.Items.Clear();
        dlDentalOffice.Items.Add(new ListItem("", ""));
        List<MasterDentalOffice> list = MasterDentalOffice.GetDentalOfficeMasterSearch(int.Parse(hiddenOfficeCd.Value), string.Empty, string.Empty, txtSalesman.Text);
        foreach (MasterDentalOffice i in list)
        {
            dlDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }

        txtClinicName.Text = "";
        txtClinicName1.Text = "";
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        //txtDivision.Text = string.Empty;
        txtOrderNo.Text = string.Empty;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtDeliveryFromDate.Text = string.Empty;
        txtDeliveryToDate.Text = string.Empty;
        txtSalesman.Text = string.Empty;
        txtSalesman1.Text = string.Empty;
        txtClinicName.Text = string.Empty;
        txtClinicName1.Text = string.Empty;
        txtPatient.Text = string.Empty;
        txtProstheticType.Text = string.Empty;
        txtProstheticType1.Text = string.Empty;
        txtContractorId.Text = string.Empty;
        txtContractorName.Text = string.Empty;

        dlDentalOffice.SelectedIndex = -1;
        dlStaff.SelectedIndex = -1;
        dlProstheticType.SelectedIndex = -1;
        dlContractor.SelectedIndex = -1;

    }

    protected void btNewOrder_Click(object sender, EventArgs e)
    {
        //int tabId1 = GetTabIdByName(PortalId, "Order");
        //Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tabId1, string.Empty, string.Empty, string.Empty, "?office=" + mOfficeCd.ToString()));
        Response.Redirect("OrderInput.aspx");
    }

    protected void btEdit_Click(object sender, EventArgs e)
    {
        var checkedIDs = (from GridViewRow msgRow in gvOrderList.Rows
                          where ((CheckBox)msgRow.FindControl("Check")).Checked
                          select Convert.ToInt32(gvOrderList.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();

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
            Response.Redirect("OrderInput.aspx" + "?id=" + checkedIDs[0].ToString());
        }
    }

    protected string getStaffName(string code)
    {
        ListItem item = dlStaff.Items.FindByValue(code);
        if (item != null)
            return item.Text;
        return string.Empty;
    }


    protected string getOfficeName(string code)
    {
        ListItem item = dlDentalOffice.Items.FindByValue(code);
        if (item != null)
            return item.Text;
        return string.Empty;
    }

    protected string showCompleteDate(string dt, string DOcode)
    {
        string s = string.Empty;
        if (dt.Contains("0001")) return string.Empty;

        ////Get module listing
        //var oController2 = new DentalOfficeMaster.DentalOfficeMasterController();
        //var dental = oController2.GetDentalOfficeMaster(int.Parse(txtOffice.Text), int.Parse(DOcode));

        MasterDentalOffice dental = MasterDentalOffice.GetDentalOfficeMaster(int.Parse(hiddenOfficeCd.Value), int.Parse(DOcode));

        if (dental == null)
            return string.Empty;

        int TransDay = dental.TransferDays == null ? 0 : dental.TransferDays.Value; 

        DateTime dlDate = DateTime.Parse(dt);
        DateTime compDate = dlDate.AddDays(-TransDay);
        s = compDate.ToString();
        s = s.Split(' ')[0];
        return s;
    }

    protected string showSex(string s)
    {
        //mMale, mFemale, mUnknow;
        if (s == "1") return mMale;
        if (s == "2") return mFemale;
        return mUnknow;
    }

    protected string showAge(string s)
    {
        //-1-> blank;                 <asp:BoundField DataField="PatientAge" />
        if (s == "-1")
            return "";
        else
            return s;
    }


    protected void dlProstheticType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProstheticType.Text = dlProstheticType.SelectedValue;
        txtProstheticType1.Text = dlProstheticType.SelectedItem.Text;
    }

    protected void txtProstheticType_TextChanged(object sender, EventArgs e)
    {
        GetName(txtProstheticType.Text, txtProstheticType1, dlProstheticType);
        dlProstheticType_SelectedIndexChanged(sender, e);
    }

    protected void btProcess_Click(object sender, EventArgs e)
    {
        var checkedIDs = (from GridViewRow msgRow in gvOrderList.Rows
                          where ((CheckBox)msgRow.FindControl("Check")).Checked
                          select Convert.ToInt32(gvOrderList.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();

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
            Response.Redirect("OperationProcess.aspx" + "?id=" + checkedIDs[0].ToString());
        }

    }

    protected void btMaterial_Click(object sender, EventArgs e)
    {
        var checkedIDs = (from GridViewRow msgRow in gvOrderList.Rows
                          where ((CheckBox)msgRow.FindControl("Check")).Checked
                          select Convert.ToInt32(gvOrderList.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();

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
            Response.Redirect("OperationTechPrice.aspx" + "?id=" + checkedIDs[0].ToString());
        }
    }

    protected void dlContractor_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtContractorId.Text = dlContractor.SelectedValue;
        txtContractorName.Text = dlContractor.SelectedItem.Text;
    }

    protected void txtContractorId_TextChanged(object sender, EventArgs e)
    {
        GetName(txtContractorId.Text, txtContractorName, dlContractor);
        dlContractor_SelectedIndexChanged(sender, e);
    }

    protected void dlNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvOrderList.PageSize = int.Parse(dlNumber.SelectedValue);
        btOk_Click(sender, e);

        ////Get module listing
        ////OrderListController oController1 = new OrderListController();
        //List<OrderListInfo> colOrderList;

        ////SetValueForObjectSearch();
        //GetValueFromCookie();

        //gvOrderList.Columns[1].Visible = true;
        
        //colOrderList = OrderListInfo.GetOrderListSearch(objSearch); //colOrderList = oController1.GetOrderLists(objSearch);

        //if (colOrderList.Count == 0)
        //{
        //    //Create List empty
        //    OrderListInfo objOL = new OrderListInfo();
        //    objOL.OrderSeq = -1;
        //    objOL.OrderNo = " ";
        //    objOL.OrderDate = DateTime.Today;
        //    objOL.DeliverDate = DateTime.Today;
        //    objOL.SetDate = DateTime.Today;

        //    colOrderList.Add(objOL);

        //    gvOrderList.DataSource = colOrderList;
        //    gvOrderList.DataBind();

        //    gvOrderList.Columns[1].Visible = false;

        //    if (colOrderList.Count == 1 && colOrderList[0].OrderSeq == -1)
        //    {
        //        gvOrderList.Rows[0].Visible = false;
        //    }

        //    lbMessage.Text = GetResource("NoRecordFound.Text");
        //    return;
        //}
        //else
        //{
        //    gvOrderList.DataSource = colOrderList;
        //    gvOrderList.DataBind();
        //    gvOrderList.Columns[1].Visible = false;
        //    //lbMessage.Text = "";
        //    lbMessage.Text = colOrderList.Count.ToString() + " results"; //string.Empty;
        //}
  
    }

    //////////////////////////////////////////////////////////

    private void FillStaffDropDownList()
    {
        dlStaff.Items.Clear();
        dlStaff.Items.Add(new ListItem("", ""));

        List<MasterStaff> list = MasterStaff.GetStaffwiOffice(int.Parse(hiddenOfficeCd.Value));

        foreach (MasterStaff i in list)
        {
            if (i.SalesFlg.HasValue && i.SalesFlg.Value == true)
            {
                //dropDownStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
                dlStaff.Items.Add(new ListItem(i.StaffNm, i.StaffCd.ToString()));
            }
        }
    }

    private void FillDentalOfficeDropDownList()
    {
        dlDentalOffice.Items.Clear();
        dlDentalOffice.Items.Add(new ListItem("", ""));

        List<MasterDentalOffice> list = MasterDentalOffice.GetDentalOfficeMasterSearch(int.Parse(hiddenOfficeCd.Value), string.Empty, string.Empty, string.Empty);

        foreach (MasterDentalOffice i in list)
        {
            dlDentalOffice.Items.Add(new ListItem(i.DentalOfficeNm, i.DentalOfficeCd.ToString()));
        }
    }

    private void FillProsthesisDropDownList()
    {
        dlProstheticType.Items.Clear();
        dlProstheticType.Items.Add(new ListItem("", ""));

        List<MasterProsthesis> list = MasterProsthesis.GetProsthesiss(int.Parse(hiddenOfficeCd.Value));

        foreach (MasterProsthesis i in list)
        {
            dlProstheticType.Items.Add(new ListItem(i.ProsthesisNm, i.ProsthesisCd.ToString()));
        }
    }

    private void FillOutsourceLabDropDownList()
    {
        dlContractor.Items.Clear();
        dlContractor.Items.Add(new ListItem("", ""));

        List<MasterOutsourceLab> list = MasterOutsourceLab.GetOutsourceLabMasters(int.Parse(hiddenOfficeCd.Value));

        foreach (MasterOutsourceLab i in list)
        {
            dlContractor.Items.Add(new ListItem(i.OutsourceNm, i.OutsourceCd.ToString()));
        }
    }

    private void Initiation(object sender, EventArgs e)
    {
        //lblCode.Text = GetResource("lblCode.Text");
        //lblName.Text = GetResource("lblName.Text");
        //btSearch.Text = GetResource("btSearch.Text");
        //btClear.Text = GetResource("btClear.Text");
        ////lblModuleTitle.Text = GetResource("lblDentalOffice.Text");
        //LabelDentalOfficeCd.Text = GetResource("HEADER_OFFICE_DentalOfficeCd.Text");
        //LabelDentalOfficeNm.Text = GetResource("HEADER_OFFICE_DentalOfficeNm.Text");
        //LabelDentalOfficePostalCd.Text = GetResource("HEADER_OFFICE_DentalOfficePostalCd.Text");
        //LabelDentalOfficeAddress1.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress1.Text");
        //LabelDentalOfficeAddress2.Text = GetResource("HEADER_OFFICE_DentalOfficeAddress2.Text");
        //LabelDentalOfficeFAX.Text = GetResource("HEADER_OFFICE_DentalOfficeFAX.Text");
        //LabelDentalOfficeTEL.Text = GetResource("HEADER_OFFICE_DentalOfficeTEL.Text");
        //valRequiredInput_DentalOfficeCd.ErrorMessage = valRequiredInput_DentalOfficeCd.ToolTip = valRequiredNumber_DentalOfficeCd.ErrorMessage = valRequiredNumber_DentalOfficeCd.ToolTip = GetResource("valRequired_DentalOfficeCd.Text");
        //valRequired_DentalOfficeNm.ErrorMessage = valRequired_DentalOfficeNm.ErrorMessage = GetResource("valRequired_DentalOfficeNm.Text");

        //gridViewOffice.EmptyDataText = GetResource("NoRecordFound.Text");
        //SetButtonText(btnRegister, btnEdit, ButtonSave, ButtonDelete, ButtonCancel);


        lbOrderNo.Text = GetResource("lbOrderNo.Text");
        lbOrderDate.Text = GetResource("lbOrderDate.Text");
        lbDeliveryDate.Text = GetResource("lbDeliveryDate.Text");
        lbClinicName.Text = GetResource("lbClinicName.Text");
        lbPatient.Text = GetResource("lbPatient.Text");
        lbSalesman.Text = GetResource("lbSalesman.Text");
        lbProstheticType.Text = GetResource("lbProstheticType.Text");
        lbContractor.Text = GetResource("lbContractor.Text");
        lbRowPerPage.Text = GetResource("lbRowsPage.Text");

        cbxCompletedBill.Text = GetResource("cbxCompletedBill.Text");
        cbxCompletedDelivery.Text = GetResource("cbxCompletedDelivery.Text");
        cbxOnlyTrialOrder.Text = GetResource("cbxOnlyTrialOrder.Text");
        cbxOnlyRemanufacture.Text = GetResource("cbxOnlyRemanufacture.Text");

        hiddenCadCamPopupTitle.Value = GetResource("hiddenCadCamPopupTitle.Value");

        btOk.Text = GetResource("btOk.Text");
        btCancel.Text = GetResource("btCancel.Text");
        gvOrderList.EmptyDataText = GetResource("NoRecordFound.Text");

        btNewOrder.Text = GetResource("btNewOrder.Text");
        btEdit.Text = GetResource("btEdit.Text");
        btProcess.Text = GetResource("btProcess.Text");
        btMaterial.Text = GetResource("btMaterial.Text");
        btlHideSearch.Text = GetResource("btHideSearch.Text");
        btlShowSearch.Text = GetResource("btShowSearch.Text");
        btlHideSearch.Text = GetResource("btHideSearch.Text");
        lblCadCam.Text = btlCad.Text = GetResource("btlCad.Text");
        btShowSearch_Click(sender, e);

        btlHideSearch.Visible = true;
        btlShowSearch.Visible = false;

        mMale = GetResource("lbMale.Text");
        mFemale = GetResource("lbFemale.Text");
        mUnknow = GetResource("lbUnknow.Text");
    }
    protected void btlCad_Click(object sender, EventArgs e)
    {
        panelPopUp.Visible = true;
       
    }
    protected void btnClose_Click(object sender, ImageClickEventArgs e)
    {
        panelPopUp.Visible = false;
    }
}




//  var context = new DBContext();
            
//            list = (from item in context.GetTable<MasterProcessTemplate>()
//                    join pet in context.GetTable<MasterProcess>() on item.ProcessCd equals pet.ProcessCd
// // nhieu dieu kien ket
//on new { p.FirstName, p.LastName } equals new { w.FirstName, w.LastName }