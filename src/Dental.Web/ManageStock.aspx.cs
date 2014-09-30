using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;


public partial class ManageStock : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(ManageStock));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                InitLanguage();
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

        HiddenFieldOfficeCd.Value = GetOffice();
        this.btnReg.Attributes.Add("onclick", "javascript:if (Page_ClientValidate()){ this.disabled=true;}" + Page.ClientScript.GetPostBackEventReference(btnReg, "").ToString());
        txtRegdate.Text = SetDateFormat(DateTime.Today.ToShortDateString());
       
        txtOsCd.Enabled = false;
        txtLender.Enabled = false;
        reqValidatorOS.Visible = reqValidatorLender.Visible = false;
        HyperLink1.NavigateUrl = Calendar.InvokePopupCal(txtRegdate);


        //-----add label name-----------

        HiddenFieldOfficeCd.Value = GetOffice();
        lblTitle.Text = GetResource("title.Text");
        lblTitle2.Text = GetResource("Title_InventoryRegister.Text");
        lblRegDate.Text = GetResource("lblRegDate.Text");
        lblMaterialType.Text = GetResource("lblMaterialType.Text");
        lblCurrentStock.Text = GetResource("lblCurrentStock.Text");
        lblTotalStock.Text = GetResource("lblTotalStock.Text");
        lblAction.Text = GetResource("lblAction.Text");
        lblIn.Text = GetResource("lblIn.Text");
        lblOut.Text = GetResource("lblOut.Text");
        lblQty.Text = GetResource("lblQty.Text");
        lblPrice.Text = GetResource("lblPrice.Text");
        lblSupllier.Text = GetResource("lblSupplier.Text");
        lblBorrower.Text = GetResource("lblBorrower.Text");
        lblLender.Text = GetResource("lblLender.Text");
        lblComment.Text = GetResource("lblComment.Text");
        lblFee.Text = GetResource("lblFee.Text");
        lblSearchMaCd.Text = lblMaterialType.Text;
        lblSearchBorrower.Text = lblBorrower.Text;
        lblSearchLender.Text = lblLender.Text;
        lblNoRecord.Text = GetResource("mesNoRecord.Text");
        lblOwnStock.Text = GetResource("CheckOwnStock_Own_Stock.Text");
        lblRowsPage.Text = GetResource("lblRowsPage.Text");

        //------add radio button name---
        rbtnBuy.Text = GetResource("rbtnBuy.Text");
        rbtnBorrow.Text = GetResource("rbtnBorrow.Text");
        rbtnLendReturn.Text = GetResource("rbtnLendReturn.Text");
        rbtnLeave.Text = GetResource("rbtnLeave.Text");
        rbtnLend.Text = GetResource("rbtnLend.Text");
        rbtnBorrowReturn.Text = GetResource("rbtnBorrowReturn.Text");

        //--------add button name-------
        btnReg.Text = GetResource("btnReg.Text");
        btnRegister.Text = GetResource("btnReg.Text");
        btnCancel.Text = GetResource("btBack.Text");
        btnClear.Text = GetResource("btnClear.Text");
        btnSearch.Text = GetResource("btnSearch.Text");

        //------------add Validator-------------------
        reqValidationMaCd.ErrorMessage = GetResource("reVaMaterialCd.Text");
        RegExQty.ErrorMessage = reqValidationQty.ErrorMessage = GetResource("reVaQty.Text");
        reqValidatorPrice.ErrorMessage = GetResource("mesPrice.Text");
        reqValidatorSupplier.ErrorMessage = GetResource("mesSupplier.Text");
        reqValidatorOS.ErrorMessage = GetResource("mesBorrower.Text");
        reqValidatorLender.ErrorMessage = GetResource("mesLender.Text");
        compareRegDate.ErrorMessage = GetResource("CheckDateFormat.Text");
        reqValidatorRegDate.ErrorMessage = GetResource("checkDateInput.Text");
        reqValidationUnit.ErrorMessage = GetResource("checkUnit.Text");
        regExpMaxCmt.ErrorMessage = GetResource("regExpCmt.Text");

        //-------------CheckBox Search--------------------
        cbOwnStock.Text = GetResource("CheckBorrowerSearch.Text");
        cbBorrower.Text = GetResource("CheckBorrowerSearch.Text");
        cbLender.Text = GetResource("CheckSupplierSearch.Text");
        
        //-------Fill Material------------
        List<MasterMaterial> listMaterial = new List<MasterMaterial>();
        listMaterial = MasterMaterial.GetAll().Where(m => (m.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value))).ToList();
        DropDownListMa.Items.Add("");
        ddlMa.Items.Add("");
        ddlMa0.Items.Add("");
        foreach (MasterMaterial m in listMaterial)
        {
            DropDownListMa.Items.Add(new ListItem(m.MaterialNm, Convert.ToString(m.MaterialCd)));
            ddlMa.Items.Add(new ListItem(m.MaterialNm, Convert.ToString(m.MaterialCd)));
            ddlMa0.Items.Add(new ListItem(m.MaterialNm, Convert.ToString(m.MaterialCd)));
        }

        //-------Fill Outsource-----------
        List<MasterOutsourceLab> listOutSource = new List<MasterOutsourceLab>();
        listOutSource = MasterOutsourceLab.GetOutsourceLabMasters(int.Parse(HiddenFieldOfficeCd.Value));
        DropDownListOS.Items.Add("");
        ddlBorrower.Items.Add("");
        ddlBorrower0.Items.Add("");
        foreach (MasterOutsourceLab o in listOutSource)
        {
            DropDownListOS.Items.Add(new ListItem(o.OutsourceNm, Convert.ToString(o.OutsourceCd)));
            ddlBorrower.Items.Add(new ListItem(o.OutsourceNm, Convert.ToString(o.OutsourceCd)));
            ddlBorrower0.Items.Add(new ListItem(o.OutsourceNm, Convert.ToString(o.OutsourceCd)));
        }

        //--------Fill Supplier----------
        List<MasterSupplier> listSupplier = new List<MasterSupplier>();
        listSupplier = MasterSupplier.GetSupplierMasters(int.Parse(HiddenFieldOfficeCd.Value));
        //-----load supplier(for search and lender) info----
        DropDownListLender.Items.Add("");
        ddlLender.Items.Add("");
        ddlLender0.Items.Add("");
        foreach (MasterSupplier s in listSupplier)
        {
            DropDownListLender.Items.Add(new ListItem(s.SupplierNm, Convert.ToString(s.SupplierCd)));
            ddlLender.Items.Add(new ListItem(s.SupplierNm, Convert.ToString(s.SupplierCd)));
            ddlLender0.Items.Add(new ListItem(s.SupplierNm, Convert.ToString(s.SupplierCd)));
        }
        //--------------------------------

        //-------load Supplier info-------
        DropDownListSup.Items.Add("");
        foreach (MasterSupplier s in listSupplier)
            DropDownListSup.Items.Add(new ListItem(s.SupplierNm, Convert.ToString(s.SupplierCd)));

        //-----load stock list------------
        Search();

    }

    #region Stock List

    private void Search()
    {
        try
        {

            lblNoRecord.Visible = false;

            int material1 = 0, material2 = 0, outsource1 = 0, outsource2 = 0, supplier1 = 0, supplier2 = 0;
            List<StockListInfo> stinfo = new List<StockListInfo>();
           
            #region "search"
            if (txtSearchMaCd.Text == string.Empty)
                material1 = 0;
            else
                material1 = int.Parse(txtSearchMaCd.Text);
            if (txtSearchMaCd0.Text == string.Empty)
                material2 = int.MaxValue;
            else
                material2 = int.Parse(txtSearchMaCd0.Text);



            if (txtSearchBorower.Text == string.Empty)
                outsource1 = 1;
            else
                outsource1 = int.Parse(txtSearchBorower.Text);
            if (txtSearchBorower0.Text == string.Empty)
                outsource2 = int.MaxValue;
            else
                outsource2 = int.Parse(txtSearchBorower0.Text);


            if (txtSearchLender.Text == string.Empty)
                supplier1 = 1;
            else
                supplier1 = int.Parse(txtSearchLender.Text);
            if (txtSearchLender0.Text == string.Empty)
                supplier2 = int.MaxValue;
            else
                supplier2 = int.Parse(txtSearchLender0.Text);


            stinfo = StockListInfo.GetStockListSeach(int.Parse(HiddenFieldOfficeCd.Value), material1, material2, supplier1, supplier2, outsource1, outsource2);

                        
            if (cbBorrower.Checked || (outsource1>outsource2))
            {
                stinfo = stinfo.Where(s => (s.OutsourceCd == 0)).ToList();
            }
            if (cbLender.Checked || (supplier1>supplier2))
            {
                stinfo = stinfo.Where(s => (s.SupplierCd == 0)).ToList();
            }
            if (cbOwnStock.Checked)
            {
                stinfo = stinfo.Where(s => (s.OutsourceCd != 0 || s.SupplierCd != 0)).ToList();
            }

            for (int i = stinfo.Count - 1; i >= 0; i--)
            {
                if ((stinfo[i].SupplierCd != 0 && stinfo[i].Lender == null) || (stinfo[i].OutsourceCd != 0 && stinfo[i].Borrower == null))
                    stinfo.RemoveAt(i);
            }

            #endregion

            //show header when empty
            if (stinfo.Count == 0)
            {
                StockListInfo info = new StockListInfo();
                info.MaterialCd = -1;
                info.MaterialNm = "";
                info.Qty = 0;
                info.Unit = "";
                info.Maker = "";
                stinfo.Add(info);
                GridView1.DataSource = stinfo;
                GridView1.DataBind();
                if (stinfo.Count == 1 && stinfo[0].MaterialCd == -1)
                {
                    GridView1.Rows[0].Visible = false;
                }
                lblNoRecord.Visible = true;
                return;
            }

            GridView1.DataSource = stinfo;
            GridView1.DataBind();
            foreach (GridViewRow r in GridView1.Rows)
            {
                if (Common.GetRowString(r.Cells[5].Text) == string.Empty && Common.GetRowString(r.Cells[7].Text) == string.Empty)
                {
                    r.Cells[6].Text = string.Empty;
                    r.Cells[8].Text = string.Empty;

                }
                if (Common.GetRowString(r.Cells[5].Text) == string.Empty)
                {
                   
                    r.Cells[6].Text = string.Empty;
                }
                if (Common.GetRowString(r.Cells[7].Text) == string.Empty)
                {
                   
                    r.Cells[8].Text = string.Empty;
                }
                if (Common.GetRowString(r.Cells[7].Text) != string.Empty && Common.GetRowString(r.Cells[8].Text) != string.Empty)
                {
                    r.Cells[4].Text = string.Empty;
                }
                if (Common.GetRowString(r.Cells[5].Text) != string.Empty && Common.GetRowString(r.Cells[6].Text) != string.Empty)
                {
                    r.Cells[4].Text = string.Empty;
                }

            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Search", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView grid = (GridView)sender;
            GridViewRow gridrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tblcell = new TableCell();

            //----add material Cd------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colMaterialCd.Text");
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //-----add material Name-----
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colMateriaNml.Text");
            tblcell.Width = Unit.Pixel(300);
            gridrow.Cells.Add(tblcell);

            //---------add Maker---------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colMaker.Text");
            tblcell.Width = Unit.Pixel(300);
            gridrow.Cells.Add(tblcell);

            //--------add unit------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colUnit.Text");
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            //-------add Qty-------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colQty.Text");
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //-------add Borrower------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colBorrower.Text");
            tblcell.Width = Unit.Pixel(300);
            gridrow.Cells.Add(tblcell);

            //-------add borrowedqty------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colQty.Text");
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //-------add lender------
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colLender.Text");
            tblcell.Width = Unit.Pixel(300);
            gridrow.Cells.Add(tblcell);

            //-------add lendedqty ----
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("colQty.Text");
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);



            //-------------------------------
            //Add header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }

    #region "TextChanged for Region Search"

    protected void txtSearchMaCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchMaCd, ddlMa);
    }

    protected void txtSearchBorower_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchBorower, ddlBorrower);
    }

    protected void txtSearchLender_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchLender, ddlLender);
    }

    protected void txtSearchMaCd0_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchMaCd0, ddlMa0);
    }

    protected void txtSearchBorower0_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchBorower0, ddlBorrower0);
    }

    protected void txtSearchLender0_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSearchLender0, ddlLender0);
    }

    protected void ddlMa_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlMa, txtSearchMaCd);
    }

    protected void ddlBorrower_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlBorrower, txtSearchBorower);
    }

    protected void ddlLender_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlLender, txtSearchLender);
    }

    protected void ddlMa0_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlMa0, txtSearchMaCd0);
    }

    protected void ddlBorrower0_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlBorrower0, txtSearchBorower0);
    }

    protected void ddlLender0_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlChanged(ddlLender0, txtSearchLender0);
    }

    private void ddlChanged(DropDownList ddl, TextBox txt)
    {
        txt.Text = ddl.SelectedValue;
    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Search();
        }
        catch (Exception ex)
        {
            logger.Error("Error BtnSearch", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    
    protected void cbBorrower_CheckedChanged(object sender, EventArgs e)
    {

        if (cbBorrower.Checked)
            txtSearchBorower.Enabled = txtSearchBorower0.Enabled = ddlBorrower.Enabled = ddlBorrower0.Enabled = false;
        else
            txtSearchBorower.Enabled = txtSearchBorower0.Enabled = ddlBorrower.Enabled = ddlBorrower0.Enabled = true;
    }

    protected void cbLender_CheckedChanged(object sender, EventArgs e)
    {
        if (cbLender.Checked)
            txtSearchLender.Enabled = txtSearchLender0.Enabled = ddlLender0.Enabled = ddlLender.Enabled = false;
        else
            txtSearchLender.Enabled = txtSearchLender0.Enabled = ddlLender0.Enabled = ddlLender.Enabled = true;
    }

   
    protected void DropdownSelectRows_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.PageSize = int.Parse(DropdownSelectRows.SelectedValue);
        Search();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearchBorower.Text = "";
        txtSearchBorower0.Text = "";
        txtSearchLender.Text = "";
        txtSearchLender0.Text = "";
        txtSearchMaCd.Text = "";
        txtSearchMaCd0.Text = "";
        ddlBorrower.SelectedValue = ""; ddlBorrower0.SelectedValue = "";
        ddlLender.SelectedValue = ""; ddlLender0.SelectedValue = "";
        ddlMa.SelectedValue = ""; ddlMa0.SelectedValue = "";
        cbOwnStock.Checked = cbBorrower.Checked = cbLender.Checked = false;
        txtSearchBorower.Enabled = txtSearchBorower0.Enabled = ddlBorrower.Enabled = ddlBorrower0.Enabled = true;
        txtSearchLender.Enabled = txtSearchLender0.Enabled = ddlLender0.Enabled = ddlLender.Enabled = true;
    }
    #endregion


    protected void btnRegister_Click(object sender, EventArgs e)
    {
        PanelShow.Visible = false;
        PanelReg.Visible = true;
        rbtnBuy.Checked = true;
        rbtnBorrow.Checked = false;
        rbtnLend.Checked = false;
        rbtnLendReturn.Checked = false;
        rbtnBorrowReturn.Checked = false;
        rbtnLeave.Checked = false;

        reqValidatorOS.Visible = false;
        reqValidatorLender.Visible = false;
        reqValidatorSupplier.Visible = true;
        reqValidatorPrice.Visible = true;

        //--------------------
        DropDownListSup.Enabled = true;
        DropDownListOS.Enabled = false;
        txtOsCd.Enabled = false;

        DropDownListLender.Enabled = false;
        txtLender.Enabled = false;
        txtSupCd.Enabled = true;
        txtPrice.Enabled = true;
        txtFee.Enabled = true;
        //--------------------------

        if (DropDownListMa.Items.Count > 0)
        {
            DropDownListOS.SelectedValue = "";
            DropDownListSup.SelectedValue = "";
            DropDownListMa.SelectedValue = "";
            DropDownListLender.SelectedValue = "";
            DropDownListQtyUnit.Items.Clear();
            txtCmt.Text = "";
            txtLender.Text = "";
            txtFee.Text = "";
            txtMaCd.Text = DropDownListMa.SelectedValue;
            txtOsCd.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            txtSupCd.Text = "";
            LabelCurStock.Text = LabelTotal.Text = LabelTotalUnit.Text = string.Empty;
            txtRegdate.Text = SetDateFormat(DateTime.Today.ToShortDateString());
        }
    }

    protected void txtMaCd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownListQtyUnit.Items.Clear();
            List<MasterUnit> uinfo;
          
            DateTime check;
            if (!DateTime.TryParse(txtRegdate.Text, out check))
            {
                return;
            }
            GetAutomaticDropDownList(txtMaCd, DropDownListMa);


            uinfo = MasterUnit.GetListUnitByMaterialCd(int.Parse(HiddenFieldOfficeCd.Value),Common.GetNullableInt(txtMaCd.Text), Convert.ToDateTime(txtRegdate.Text));
            uinfo = uinfo.OrderBy(x => (x.Priority)).ToList();
            if (uinfo.Count == 0)
            {
                return;
            }
            else
            {
                foreach (MasterUnit u in uinfo)
                {
                    DropDownListQtyUnit.Items.Add(new ListItem(u.UnitNm, u.UnitCd));
                }
                //--------add currrent stock---------

            }
            LoadCurrentStock();
          
        }
        catch (Exception ex)
        {
            logger.Error("Input Material code fail!",ex);
           
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
      
    protected void DropDownListMa_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownListQtyUnit.Items.Clear();
            txtMaCd.Text = DropDownListMa.SelectedValue;

            DropDownListQtyUnit.Items.Clear();
            List<MasterUnit> uinfo;
            
            DateTime check;
            if (!DateTime.TryParse(txtRegdate.Text, out check))
            {
                return;
            }
            if (DropDownListMa.SelectedValue == "")
            {
                LabelCurStock.Text = LabelTotal.Text = LabelTotalUnit.Text = "";
                return;
            }
            uinfo = MasterUnit.GetListUnitByMaterialCd(int.Parse(HiddenFieldOfficeCd.Value), Common.GetNullableInt(txtMaCd.Text), Convert.ToDateTime(txtRegdate.Text));
            uinfo = uinfo.OrderBy(x => (x.Priority ? 0:1)).ToList();
            if (uinfo.Count == 0)
            {
                return;
            }
            else
            {
                foreach (MasterUnit u in uinfo)
                {
                    DropDownListQtyUnit.Items.Add(new ListItem(u.UnitNm, u.UnitCd));
                }
            }

            //--------get currrent stock---------

            LoadCurrentStock();

        }
        catch (Exception ex)
        {
            logger.Error("select material fail!");

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    private void LoadCurrentStock()
    {
        try
        {
            LabelCurStock.Text = "";
            List<CurrentStock> curinfo;

            curinfo = CurrentStock.GetCurrentStock(int.Parse(txtMaCd.Text), int.Parse(HiddenFieldOfficeCd.Value));
            if (curinfo.Count == 0)
                LabelCurStock.Text = "";
            else
                foreach (CurrentStock c in curinfo)
                {
                    LabelCurStock.Text += c.Amount + " " + c.UnitNm + "  ";
                }
        }
        catch (Exception ex)
        {
            logger.Error("Load current stock fail!",ex);
            
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void rbtnBuy_CheckedChanged(object sender, EventArgs e)
    {
        txtOsCd.Enabled = false; txtOsCd.Text = "";
        txtLender.Enabled = false; txtLender.Text = "";
        txtSupCd.Enabled = true; txtSupCd.Text = "";
        txtPrice.Enabled = true;
        txtFee.Enabled = true;
        DropDownListOS.Enabled = false; DropDownListOS.SelectedValue = "";
        DropDownListLender.Enabled = false; DropDownListLender.SelectedValue = "";
        DropDownListSup.Enabled = true;

        reqValidatorPrice.Visible = true;
        reqValidatorSupplier.Visible = true;
        reqValidatorOS.Visible = false;
        reqValidatorLender.Visible = false;

    }

    protected void rbtnBorrow_CheckedChanged(object sender, EventArgs e)
    {

        txtLender.Enabled = true;
        txtSupCd.Enabled = false; txtSupCd.Text = "";
        txtPrice.Enabled = false; txtPrice.Text = "";
        txtOsCd.Enabled = false; txtOsCd.Text = "";
        txtFee.Enabled = false; txtFee.Text = "";
        DropDownListSup.Enabled = false; DropDownListSup.SelectedValue = "";
        DropDownListOS.Enabled = false; DropDownListOS.SelectedValue = "";
        DropDownListLender.Enabled = true;

        reqValidatorPrice.Visible = false;
        reqValidatorSupplier.Visible = false;
        reqValidatorOS.Visible = false;
        reqValidatorLender.Visible = true;

    }

    protected void rbtnLend_CheckedChanged(object sender, EventArgs e)
    {
        txtOsCd.Enabled = true;
        txtSupCd.Enabled = false;
        txtSupCd.Text = "";
        txtPrice.Enabled = false; txtPrice.Text = "";
        txtFee.Enabled = false; txtFee.Text = "";
        txtLender.Enabled = false; txtLender.Text = "";
        DropDownListLender.Enabled = false; DropDownListLender.SelectedValue = "";
        DropDownListOS.Enabled = true;
        DropDownListSup.Enabled = false; DropDownListSup.SelectedValue = "";

        reqValidatorPrice.Visible = false;
        reqValidatorSupplier.Visible = false;
        reqValidatorOS.Visible = true;
        reqValidatorLender.Visible = false;
    }

    protected void rbtnLeave_CheckedChanged(object sender, EventArgs e)
    {

        txtPrice.Enabled = false; txtPrice.Text = "";
        txtFee.Enabled = false; txtFee.Text = "";
        txtSupCd.Enabled = false; txtSupCd.Text = "";
        txtOsCd.Enabled = false; txtOsCd.Text = "";
        txtLender.Enabled = false; txtLender.Text = "";
        DropDownListLender.Enabled = false; DropDownListLender.SelectedValue = "";
        DropDownListOS.Enabled = false; DropDownListOS.SelectedValue = "";
        DropDownListSup.Enabled = false; DropDownListSup.SelectedValue = "";

        reqValidatorPrice.Visible = false;
        reqValidatorSupplier.Visible = false;
        reqValidatorOS.Visible = false;
        reqValidatorLender.Visible = false;
    }

    protected void rbtnLendReturn_CheckedChanged(object sender, EventArgs e)
    {
        txtOsCd.Enabled = true;
        txtSupCd.Enabled = false; txtSupCd.Text = "";
        txtPrice.Enabled = false; txtPrice.Text = "";
        txtFee.Enabled = false; txtFee.Text = "";

        txtLender.Enabled = false; txtLender.Text = "";
        DropDownListLender.Enabled = false; DropDownListLender.SelectedValue = "";
        DropDownListOS.Enabled = true;
        DropDownListSup.Enabled = false; DropDownListSup.SelectedValue = "";

        reqValidatorPrice.Visible = false;
        reqValidatorSupplier.Visible = false;
        reqValidatorOS.Visible = true;
        reqValidatorLender.Visible = false;
    }

    protected void rbtnBorrowReturn_CheckedChanged(object sender, EventArgs e)
    {
        txtSupCd.Enabled = false; txtSupCd.Text = "";
        txtLender.Enabled = true;
        txtPrice.Enabled = false; txtPrice.Text = "";
        txtFee.Enabled = false; txtFee.Text = "";
        txtOsCd.Enabled = false; txtOsCd.Text = "";
        DropDownListLender.Enabled = true;
        DropDownListOS.Enabled = false; DropDownListOS.SelectedValue = "";
        DropDownListSup.Enabled = false; DropDownListSup.Text = "";

        reqValidatorPrice.Visible = false;
        reqValidatorSupplier.Visible = false;
        reqValidatorOS.Visible = false;
        reqValidatorLender.Visible = true;

    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        try
        {

            if (DropDownListQtyUnit.Items.Count == 0 && txtMaCd.Text != string.Empty)// &&(!string.IsNullOrWhiteSpace(txtSupCd.Text) ||!string.IsNullOrWhiteSpace(txtOsCd.Text)||!string.IsNullOrWhiteSpace(txtLender.Text)))
            {
                return;
            }

            DateTime check;
            if (string.IsNullOrWhiteSpace(DropDownListMa.SelectedValue) || string.IsNullOrWhiteSpace(txtQty.Text) || string.IsNullOrWhiteSpace(txtRegdate.Text) || !DateTime.TryParse(txtRegdate.Text, out check))
            {
                return;
            }


            #region "Data declaration"
            MasterStock stinfo = new MasterStock();
            TrnPurchase purchase = new TrnPurchase();
            TrnStockInOut stockinout = new TrnStockInOut();
            List<MasterUnit> unit = new List<MasterUnit>();
            Nullable<int> AmountByMiniMumUnit = 0;
            unit = MasterUnit.GetListUnitByMaterialCd(int.Parse(HiddenFieldOfficeCd.Value),Common.GetNullableInt(txtMaCd.Text), Convert.ToDateTime(txtRegdate.Text));

            string unitCd = string.Empty;
            try
            {
                unitCd = unit.Where(u => (u.UnitCd == DropDownListQtyUnit.SelectedValue)).FirstOrDefault().UnitCd;
            }
            catch { }

            if (!string.IsNullOrEmpty(unitCd))
                AmountByMiniMumUnit = unit.Where(u => (u.UnitCd == DropDownListQtyUnit.SelectedValue)).FirstOrDefault().AmountByMinimumUnit;
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckUnitAvailable.Text")) + "\");", true);
                return;
            }

            string priorityUnit = StockListInfo.GetPriorityUnit(int.Parse(txtMaCd.Text), DateTime.Parse(txtRegdate.Text), int.Parse(HiddenFieldOfficeCd.Value));
            Nullable<int> checkAvailable = null;

            //---------------Stockinout info---------------------
            stockinout.RegisterDate = DateTime.Parse(txtRegdate.Text);
            stockinout.Amount = Convert.ToDouble(txtQty.Text);
            stockinout.UnitCd = DropDownListQtyUnit.SelectedValue;
            stockinout.Comment = txtCmt.Text;
            stockinout.CreateAccount = this.User.Identity.Name;
            stockinout.ModifiedAccount = this.User.Identity.Name;
            stockinout.MaterialCd = int.Parse(txtMaCd.Text);
            stockinout.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            stockinout.CreateDate = DateTime.Today;
            stockinout.ModifiedDate = DateTime.Today;
            //----------------MstStockInfo---------------------
            stinfo.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            stinfo.MaterialCd = int.Parse(txtMaCd.Text);
            stinfo.CreateAccount = this.User.Identity.Name;
            stinfo.ModifiedAccount = this.User.Identity.Name;
            #endregion
            //----------------------------------------------------
            #region Buy

            if (rbtnBuy.Checked)
            {
                if (txtPrice.Text == string.Empty || txtSupCd.Text == string.Empty)
                {
                    return;
                }

                if (int.Parse(txtSupCd.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckOwnStock_Own_Stock.Text") + "(" + DropDownListSup.SelectedItem.Text + ")." + GetResource("CheckOwnStock_Another_Code.Text")) + "\");", true);
                    return;
                }
                //-------------purchase info-----------------------
                purchase.SupplierOutsourceCd = int.Parse(txtSupCd.Text);
                purchase.PurchaseDate = DateTime.Parse(txtRegdate.Text);
                purchase.PurchaseCategory = int.Parse(txtMaCd.Text);
                purchase.PurchaseItems = DropDownListMa.SelectedItem.Text;
                purchase.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                purchase.PurchasePrice = Convert.ToDouble(txtPrice.Text);
                purchase.PurchaseFee = Common.GetNullableDouble(txtFee.Text);
                purchase.Note = txtCmt.Text;
                purchase.CreateAccount = this.User.Identity.Name;
                purchase.ModifiedAccount = this.User.Identity.Name;

                //----------------stock inout info--------------------
                stockinout.SumPrice =  stockinout.Price = Convert.ToDouble(txtPrice.Text);
                stockinout.InOutKbn = 11;
                stockinout.SupplierCd = int.Parse(txtSupCd.Text);
                stockinout.OutsourceLabCd = 0;
                //----------------------------------------------------
                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 1, 0, 0, priorityUnit);
                if (checkAvailable == 0)
                {
                    stinfo.Amount = Convert.ToDouble(txtQty.Text) * (int)AmountByMiniMumUnit;
                    stinfo.UnitCd = priorityUnit;
                    StockListInfo.TransactionManageStock(stinfo, 0, 0, int.Parse(HiddenFieldOfficeCd.Value), 0, stockinout,purchase, 1);
       
                }
                else
                {
                    Nullable<double> currentAmount = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, 0, priorityUnit);
                    stinfo.Amount = (Convert.ToDouble(txtQty.Text) * (int)AmountByMiniMumUnit) + (double)currentAmount;
                    stinfo.UnitCd = priorityUnit;
                    StockListInfo.TransactionManageStock(stinfo, 0, 0, int.Parse(HiddenFieldOfficeCd.Value), 0, stockinout,purchase, 2);
                   

                }

                LoadCurrentStock();
            }

            #endregion

            #region Borrow

            if (rbtnBorrow.Checked)
            {
                string amountBorrow = "";
                if (txtLender.Text == "")
                {
                    return;
                }
                if (int.Parse(txtLender.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckOwnStock_Own_Stock.Text") + "(" + DropDownListLender.SelectedItem.Text + ")." + GetResource("CheckOwnStock_Another_Code.Text")) + "\");", true);
                    return;
                }
                stockinout.InOutKbn = 12;
                stockinout.SupplierCd = int.Parse(txtLender.Text);
                stockinout.OutsourceLabCd = 0;

                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 2, int.Parse(txtLender.Text), 0, priorityUnit);
                if (checkAvailable == 0)
                {
                    stinfo.Amount = double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                    stinfo.UnitCd = priorityUnit;
                    StockListInfo.TransactionManageStock(stinfo, int.Parse(txtLender.Text), 0, int.Parse(HiddenFieldOfficeCd.Value), 0, stockinout,purchase, 1);
                }
                else
                {
                    Nullable<double> currentBorrowed = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), int.Parse(txtLender.Text), 0, priorityUnit);
                    stinfo.Amount = double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit + (double)currentBorrowed;
                    stinfo.UnitCd = priorityUnit;
                    StockListInfo.TransactionManageStock(stinfo, int.Parse(txtLender.Text), 0, int.Parse(HiddenFieldOfficeCd.Value), 0,stockinout,purchase, 2);
                }
                
                amountBorrow = GetResource("colLender.Text") + " " + DropDownListLender.SelectedItem.Text + ": " + txtQty.Text + DropDownListQtyUnit.SelectedItem.Text;
                LoadCurrentStock();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("mesSuccess.Text"), lblCurrentStock.Text + ":" + Common.GetRowString(LabelCurStock.Text)) + "\\n" + amountBorrow + "\");", true);
                
            }

            #endregion

            #region "Borrow return"
            if (rbtnBorrowReturn.Checked)
            {
                Nullable<double> AmountBorrowBf = 0;
                if (txtLender.Text == "")
                {
                    return;
                }
                if (int.Parse(txtLender.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckOwnStock_Own_Stock.Text") + "(" + DropDownListLender.SelectedItem.Text + ")." + GetResource("CheckOwnStock_Another_Code.Text")) + "\");", true);
                    return;
                }
                stockinout.InOutKbn = 23;
                stockinout.SupplierCd = int.Parse(txtLender.Text);
                stockinout.OutsourceLabCd = 0;

                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 2, int.Parse(txtLender.Text), 0, priorityUnit);
                if (checkAvailable == 0)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesBorrowreturn.Text")) + "\");");
                    return;
                }
                else
                {
                    AmountBorrowBf = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), int.Parse(txtLender.Text), 0, priorityUnit);
                    stinfo.Amount = (double)AmountBorrowBf - double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                    stinfo.UnitCd = priorityUnit;
                    if (stinfo.Amount < 0)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesCheckBorrowreturn.Text")) + "\");");
                        return;
                    }
                    StockListInfo.TransactionManageStock(stinfo, int.Parse(txtLender.Text), 0, int.Parse(HiddenFieldOfficeCd.Value), 0, stockinout, purchase,2);
                }

                string amountBorrowReturn = DropDownListLender.SelectedItem.Text + " " + GetResource("rbtnBorrowReturn.Text") + ": " + txtQty.Text + DropDownListQtyUnit.SelectedItem.Text;
                LoadCurrentStock();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("mesSuccess.Text"), lblCurrentStock.Text + ":" + Common.GetRowString(LabelCurStock.Text)) + "\\n" + amountBorrowReturn + "\");", true);
                
            }
            #endregion 

            #region "Leave"
            if (rbtnLeave.Checked)
            {
                if (String.IsNullOrWhiteSpace(txtQty.Text))
                    return;

                stockinout.InOutKbn = 21;
                stockinout.SupplierCd = 0;
                stockinout.OutsourceLabCd = 0;

                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 1, 0, 0, priorityUnit);
                if (checkAvailable == 0)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesLeave.Text")) + "\");");
                    return;
                }
                else
                {
                    Nullable<double> amount = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, 0, priorityUnit);
                    stinfo.Amount = (double)amount - (double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit);
                    stinfo.UnitCd = priorityUnit;
                    if (stinfo.Amount < 0)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesCheckLeave.Text")) + "\");");
                        return;
                    }
                    StockListInfo.TransactionManageStock(stinfo, 0, 0, int.Parse(HiddenFieldOfficeCd.Value), 0, stockinout, purchase,2);
                }

                LoadCurrentStock();
            }
            #endregion

            #region "Lend"
            if (rbtnLend.Checked)
            {
                Nullable<double> AmountOwn = 0;
                double AmountLend = 0;
                Nullable<double> AmountLendBefore = 0;
                if (txtOsCd.Text == "")
                {
                    return;
                }
                if (int.Parse(txtOsCd.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckOwnStock_Own_Stock.Text") + "(" + DropDownListOS.SelectedItem.Text + ")." + GetResource("CheckOwnStock_Another_Code.Text")) + "\");", true);
                    return;
                }
                stockinout.InOutKbn = 22;
                stockinout.SupplierCd = 0;
                stockinout.OutsourceLabCd = int.Parse(txtOsCd.Text);

                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 1, 0, 0, priorityUnit);
                if (checkAvailable == 0)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesLend.Text")) + "\");");
                    return;
                }
                else
                {
                    AmountOwn = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, 0, priorityUnit);
                    if (StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 3, 0, int.Parse(txtOsCd.Text), priorityUnit) == 0)
                    {

                        stinfo.Amount = (double)AmountOwn - double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                        stinfo.UnitCd = priorityUnit;
                        AmountLend = double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                        if (stinfo.Amount < 0)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesCheckLend.Text")) + "\");");
                            return;
                        }
                        StockListInfo.TransactionManageStock(stinfo, 0, int.Parse(txtOsCd.Text), int.Parse(HiddenFieldOfficeCd.Value), AmountLend, stockinout,purchase, 3);
                    }
                    else
                    {
                        AmountLendBefore = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, int.Parse(txtOsCd.Text), priorityUnit);
                        stinfo.Amount = (double)AmountOwn - double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                        stinfo.UnitCd = priorityUnit;
                        AmountLend = double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit + (double)AmountLendBefore;
                        if (stinfo.Amount < 0)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesCheckLend.Text")) + "\");");
                            return;
                        }
                        StockListInfo.TransactionManageStock(stinfo, 0, int.Parse(txtOsCd.Text), int.Parse(HiddenFieldOfficeCd.Value), AmountLend, stockinout,purchase, 4);
                    }

                }

                string amountLend = DropDownListOS.SelectedItem.Text + ": " + txtQty.Text + DropDownListQtyUnit.SelectedItem.Text;
                LoadCurrentStock();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("mesSuccess.Text"), lblCurrentStock.Text + ":" + Common.GetRowString(LabelCurStock.Text)) + "\\n" + GetResource("colBorrower.Text") + " " + amountLend + "\");", true);
      
            }
            #endregion

            #region "Lend return"
            if (rbtnLendReturn.Checked)
            {
                double AmountLendReturn = 0;
                Nullable<double> AmountOwn = 0;
                Nullable<double> AmountLendBefore = 0;
                if (txtOsCd.Text == "")
                {
                    return;
                }
                if (int.Parse(txtOsCd.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("CheckOwnStock_Own_Stock.Text") + "(" + DropDownListOS.SelectedItem.Text + ")." + GetResource("CheckOwnStock_Another_Code.Text")) + "\");", true);
                    return;
                }
                stockinout.InOutKbn = 13;
                stockinout.SupplierCd = 0;
                stockinout.OutsourceLabCd = int.Parse(txtOsCd.Text);


                if (priorityUnit == null)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("checkPriorityUnit.Text")) + "\");");
                    return;
                }
                checkAvailable = StockListInfo.CheckAvailable(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 3, 0, int.Parse(txtOsCd.Text), priorityUnit);
                if (checkAvailable == 0)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesLendreturn.Text")) + "\");");
                    return;
                }
                else
                {
                    AmountOwn = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, 0, priorityUnit);
                    AmountLendBefore = StockListInfo.GetStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(txtMaCd.Text), 0, int.Parse(txtOsCd.Text), priorityUnit);
                    stinfo.Amount = (double)AmountOwn + double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                    stinfo.UnitCd = priorityUnit;
                    AmountLendReturn = (double)AmountLendBefore - double.Parse(txtQty.Text) * (double)AmountByMiniMumUnit;
                    if (AmountLendReturn < 0)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("mesCheckLendreturn.Text")) + "\");");
                        return;
                    }
                    StockListInfo.TransactionManageStock(stinfo, 0, int.Parse(txtOsCd.Text), int.Parse(HiddenFieldOfficeCd.Value), AmountLendReturn, stockinout,purchase, 4);
                }

                string amountlendreturn = DropDownListOS.SelectedItem.Text + " " + GetResource("rbtnLendReturn.Text") + ": " + txtQty.Text + DropDownListQtyUnit.SelectedItem.Text;
                LoadCurrentStock();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("mesSuccess.Text"), lblCurrentStock.Text + ":" + Common.GetRowString(LabelCurStock.Text)) + "\\n" + amountlendreturn + "\");", true);

            }

            #endregion


            if (rbtnBuy.Checked || rbtnLeave.Checked)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("mesSuccess.Text"), lblCurrentStock.Text + ":" + Common.GetRowString(LabelCurStock.Text)) + "\");", true);
            }

            #region "set default display"
            //--------------------------
            DropDownListOS.SelectedValue = "";
            DropDownListSup.SelectedValue = "";
            DropDownListMa.SelectedValue = "";
            DropDownListLender.SelectedValue = "";
            DropDownListQtyUnit.Items.Clear();
            txtCmt.Text = "";
            txtLender.Text = "";
            txtFee.Text = "";
            txtMaCd.Text = DropDownListMa.SelectedValue;
            txtOsCd.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            txtSupCd.Text = "";
            LabelCurStock.Text = LabelTotal.Text = LabelTotalUnit.Text = string.Empty;
            #endregion
        }
        catch (Exception ex)
        {
            logger.Error("Error Register", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void DropDownListSup_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSupCd.Text = DropDownListSup.SelectedValue;

    }

    protected void DropDownListOS_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtOsCd.Text = DropDownListOS.SelectedValue;

    }

    protected void DropDownListDo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLender.Text = DropDownListLender.SelectedValue;

    }

    protected void txtSupCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtSupCd, DropDownListSup);

    }

    protected void txtOsCd_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtOsCd, DropDownListOS);

    }

    protected void txtLender_TextChanged(object sender, EventArgs e)
    {
        GetAutomaticDropDownList(txtLender, DropDownListLender);

    }
   
    protected void DropDownListLender_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLender.Text = DropDownListLender.SelectedValue;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PanelShow.Visible = true;
        PanelReg.Visible = false;
        btnSearch_Click(sender, e);
    }
}