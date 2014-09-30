using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dental.Domain;
using Dental.Utilities;
using System.Web.Security;
using log4net;

public partial class MstMaterial : DDVPortalModuleBase
{
    private void RemoveViewState()
    {
        if (ViewState["listMaterial"] != null)
        {
            ViewState.Remove("listMaterial");
        }        
    }
    private void RemoveViewStateUnit()
    {        
        if (ViewState["listUnit"] != null)
        {
            ViewState.Remove("listUnit");
        }       
    }
    private void RemoveViewStateMaterialPrice()
    {
        if (ViewState["listMaterialPrice"] != null)
        {
            ViewState.Remove("listMaterialPrice");
        }        
    }
    

    private static readonly ILog logger = LogManager.GetLogger(typeof(MstMaterial).Name);

    protected void Page_Load(object sender, System.EventArgs e)
    {
        hplApplyDate.NavigateUrl = Calendar.InvokePopupCal(txtApplyDate);
        hplTerminateDate.NavigateUrl = Calendar.InvokePopupCal(txtTerminateDate);
        hplApplyDate1.NavigateUrl = Calendar.InvokePopupCal(txtApplyDate1);
        hplTerminateDate1.NavigateUrl = Calendar.InvokePopupCal(txtTerminateDate1);

        if (!IsPostBack)
        {
            InitLanguage();
            HttpCookie ck = Request.Cookies[this.User.Identity.Name]; //Request.Cookies("test");
            if (ck != null)
            {
                HiddenFieldOfficeCd.Value = ck["OfficeCd"];
            }
            HiddenFieldOfficeCd.Value = GetOffice();

            btDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

            lblModuleTitle.Text = GetResource("lblMaterial.Text");
            lblMaterialCd.Text = GetResource("lblMaterialCd.Text");
            lblMaterialNm.Text = GetResource("lblMaterialNm.Text");
            lblMaterialCADNm.Text = GetResource("lblMaterialCADNm.Text");
            lblProductMaker.Text = GetResource("lblProductMaker.Text");
            lblProductCd.Text = GetResource("lblProductCd.Text");
            lblProductNm.Text = GetResource("lblProductNm.Text");
            lbLentPrice.Text = GetResource("lblLentPrice.Text");
            lblShadeFlg.Text = GetResource("lblShadeFlg.Text");
            lblUnitNm.Text = GetResource("lblUnitNm.Text");
            lblOtherUnit.Text = GetResource("lblOtherUnit.Text");
            lblApplyDate.Text = GetResource("lblApplyDate.Text");
            lblTerminateDate.Text = GetResource("lblTerminateDate.Text");
            lbShowTerminate.Text = GetResource("lbShowTerminate.Text");

            btCancel.Text = GetResource("btCancel.Text");
            btDelete.Text = GetResource("btDelete.Text");
            btEdit.Text = GetResource("btEdit.Text");
            btRegister.Text = GetResource("btRegister.Text");
            btSave.Text = GetResource("btSave.Text");
            cb.Text = GetResource("cb.Text");

            valNumbersOnly_MaterialCd.ErrorMessage = GetResource("valNumbersOnly.ErrorMessage");
            valRequired_MaterialCd.ErrorMessage = GetResource("valRequired_MaterialCd.ErrorMessage");
            valRequired_MaterialNm.ErrorMessage = GetResource("valRequired_MaterialNm.ErrorMessage");
            valRequired_MaterialCADNm.ErrorMessage = GetResource("valRequired_MaterialCADNm.ErrorMessage");
            valRequired_LentPrice.ErrorMessage = GetResource("valRequired_LentPrice.ErrorMessage");
            valLentPrice.ErrorMessage = GetResource("valNumbersOnly.ErrorMessage");
            gridMaterial.EmptyDataText = GetResource("NoRecordFound.Text");
            Val_ApplyDate.ErrorMessage = GetResource("DateOnly.Text");
            Val_TerminateDate.ErrorMessage = GetResource("DateOnly.Text");

            //Show grid Unit
            btCancel1.Text = GetResource("btCancel.Text");
            btEdit1.Text = GetResource("btEdit.Text");
            btSave1.Text = GetResource("btSave.Text");
            btRegister1.Text = GetResource("btRegister.Text");
            btDelete1.Text = GetResource("btDelete.Text");
            lblUnitCd.Text = GetResource("lblUnitCd.Text");
            lblUnitNm1.Text = GetResource("lblUnitNm.Text");
            lblPriority.Text = GetResource("lblPriority.Text");
            lblAmountByMinimumUnit.Text = GetResource("lblAmountByMinimumUnit.Text");
            lblUnitNote.Text = GetResource("lblUnitNote.Text");
            valNumbersOnly_AmountByMinimumUnit.ErrorMessage = GetResource("NumberOnly.Text");
            lblApplyDate1.Text = GetResource("lblApplyDate.Text");
            lblTerminateDate1.Text = GetResource("lblTerminateDate.Text");

            lblPriceName.Text = GetResource("lblPriceNm.Text");
            lblEndDDate.Text = GetResource("lblEndDate.Text");
            lblMaCd.Text = GetResource("lblMaNm.Text");
            lblStartDate.Text = GetResource("lblStartDate.Text");
            lblPrice.Text = GetResource("lblPrice.Text");
            lblSupCd.Text = GetResource("lblSupNm.Text");
            lblTitle.Text = GetResource("lblMaterialPrice.Text");
            btCancel2.Text = GetResource("btCancel.Text");
            btEdit2.Text = GetResource("btEdit.Text");
            btSave2.Text = GetResource("btSave.Text");
            btRegister2.Text = GetResource("btRegister.Text");
            btDelete2.Text = GetResource("btDelete.Text");


            lblCode.Text = GetResource("lblCode_Code.Text");
            lblName.Text = GetResource("lblName_Name.Text");
            btSearch.Text = GetResource("btSearch.Text");
            btClear.Text = GetResource("btClear.Text");

            //Unit
            req_UnitName.ErrorMessage = GetResource("req_UnitName.ErrorMessage");
            //valNumberOnly_Priority.ErrorMessage = GetResource("valNumbersOnly.ErrorMessage");
            valNumbersOnly_AmountByMinimumUnit.ErrorMessage = GetResource("valNumbersOnly.ErrorMessage");
            rqPrice.ErrorMessage = GetResource("rqPrice.ErrorMessage");
            val_ApplyDate1.ErrorMessage = GetResource("DateOnly.Text");
            val_TerminateDate1.ErrorMessage = GetResource("DateOnly.Text");
            cbShowAllUnit.Text = GetResource("cbShowAllUnit.Text");

            txtPriority.Text = GetResource("MSG_AMOUNT_AND_PRIORITY_TRUE.Text");

            //Material Price
            rqSupCd.ErrorMessage = GetResource("requireField.ErrorMessage");
            rqStartDate.ErrorMessage = GetResource("requireField.ErrorMessage");
            rqPrice.ErrorMessage = GetResource("requireField.ErrorMessage");
            val_StartDate.ErrorMessage = GetResource("DateOnly.Text");
            val_EndDate.ErrorMessage = GetResource("DateOnly.Text");

            List<MasterSupplier> cSup = MasterSupplier.GetSupplierMasters(int.Parse(HiddenFieldOfficeCd.Value));
            DropDownListSup.Items.Clear();
            DropDownListSup.Items.Add("");
            foreach (MasterSupplier si in cSup)
            {
                DropDownListSup.Items.Add(new ListItem(si.SupplierNm, si.SupplierCd.ToString()));
            }           
            //if (UserId == 1) btDelete.Visible = true;            
            btSearch_Click(sender, e);
            RemoveViewStateMaterialPrice();
            RemoveViewStateUnit();
        }
    }
    //****************************************************************************************
    #region Material

    protected void gridMaterial_RowCreated(object sender, GridViewRowEventArgs e)
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

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblMaterialCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblMaterialNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblMaterialCADNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProductMaker.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProductCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblProductNm.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblLentPrice.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblApplyDate.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblTerminateDate.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void gridMaterial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridMaterial.PageIndex = e.NewPageIndex;
        FillListMaterial((List<MasterMaterial>)ViewState["listMaterial"]);
    }

    protected void FillListMaterial(List<MasterMaterial> List)
    {
        if (List.Count == 0)
        {
            //Create List empty
            MasterMaterial blankObj = new MasterMaterial();
            blankObj.MaterialCd = -1;
            blankObj.MaterialNm = "";
            blankObj.ApplyDate = Common.GetNullableDateTime(DateTime.Today.ToString());
            blankObj.TerminateDate = Common.GetNullableDateTime(DateTime.Today.ToString());
            List.Add(blankObj);
            lbMessage.Text = GetResource("NoRecordFound.Text");
            gridMaterial.DataSource = List;
            gridMaterial.DataBind();
            if (List.Count == 1 && List[0].MaterialCd == -1)
            {
                gridMaterial.Rows[0].Visible = false;
            }
            return;
        }
        else
        {
            lbMessage.Text = List.Count.ToString() + " records";
            gridMaterial.DataSource = List;
            gridMaterial.DataBind();
            foreach (GridViewRow row in gridMaterial.Rows)
            {
                if (Common.GetRowString(row.Cells[8].Text) != "")
                    row.Cells[8].Text = SetDateFormat(Convert.ToDateTime(row.Cells[8].Text).ToShortDateString());

                if (Common.GetRowString(row.Cells[9].Text) != "")
                    row.Cells[9].Text = SetDateFormat(Convert.ToDateTime(row.Cells[9].Text).ToShortDateString());
            }
        }
    }

    protected void btSearch_Click(object sender, EventArgs e)
    {
        List<MasterMaterial> List = MasterMaterial.GetAllMstMaterials(int.Parse(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text, cbShowTerminate.Checked);
        RemoveViewState();
        ViewState.Add("listMaterial", List);
        FillListMaterial((List<MasterMaterial>)ViewState["listMaterial"]);
    }
    protected void cbShowTerminate_CheckedChanged(object sender, EventArgs e)
    {
        btSearch_Click(sender, e);
    }

    protected void btRegister_Click(object sender, EventArgs e)
    {
        // area Material
        tbxMaterialCd.Enabled = true;
        tbxMaterialCd.Text = string.Empty;
        tbxMaterialNm.Text = string.Empty;
        tbxMaterialCADNm.Text = string.Empty;
        tbxProductMaker.Text = string.Empty;
        tbxProductCd.Text = string.Empty;
        tbxProductNm.Text = string.Empty;
        tbxLentPrice.Text = string.Empty;
        cbxShadeFlg.Checked = false;
        txtApplyDate.Text = string.Empty;
        txtTerminateDate.Text = string.Empty;
        tbxUnitNm.Text = string.Empty;

        panelGrid.Visible = false;
        panelInput.Visible = true;
        btDelete.Enabled = false;
        lblUnitNm.Visible = true;
        tbxUnitNm.Visible = true;

        ViewState.Remove("listUnitNew");
        ViewState.Remove("listMaterialPriceNew");
        // list Unit
        List<MasterUnit> listUnit = new List<MasterUnit>();
        FillListUnit(listUnit);
        // list MaterialPrice
        List<MasterMaterialPrice> listMaterialPrice = new List<MasterMaterialPrice>();
        FillListMaterialPrice(listMaterialPrice);

        btDelete.Enabled = false;
        //btEdit1.Enabled = false;
        //btEdit2.Enabled = false;
        lblUnitNm.Visible = false;
        tbxUnitNm.Visible = false;
        tbxMaterialCd.Focus();
        btClear_Click(sender, e);
    }

    protected void btEdit_Click(object sender, EventArgs e)
    {
        //UnitPanel.Visible = true;
        var checkedIDs = (from GridViewRow msgRow in gridMaterial.Rows
                          where ((CheckBox)msgRow.FindControl("Check")).Checked
                          select (gridMaterial.DataKeys[msgRow.RowIndex].Value.ToString())).ToList();

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
            var row = MasterMaterial.GetMaterialMaster(int.Parse(HiddenFieldOfficeCd.Value), Convert.ToInt32(checkedIDs[0]));
            tbxMaterialCd.Text = row.MaterialCd.ToString();
            tbxMaterialNm.Text = row.MaterialNm;
            tbxMaterialCADNm.Text = row.MaterialCADNm;
            tbxProductMaker.Text = row.ProductMaker;
            tbxProductCd.Text = row.ProductCd;
            tbxProductNm.Text = row.ProductNm;
            tbxLentPrice.Text = row.LentPrice.ToString();
            cbxShadeFlg.Checked = row.ShadeFlg.Value;
            if (row.ApplyDate == null)
                txtApplyDate.Text = "";
            else
                txtApplyDate.Text = SetDateFormat(row.ApplyDate.Value.ToShortDateString());

            if (row.TerminateDate == null)
                txtTerminateDate.Text = "";
            else
                txtTerminateDate.Text = SetDateFormat((row.TerminateDate.Value.ToShortDateString()));

            ViewState.Remove("listUnitNew");
            ViewState.Remove("listMaterialPriceNew");
            GetUnit();
            //MaterialPrice default ko check
            GetMaterialPrices();

            panelGrid.Visible = false;
            panelInput.Visible = true;
            UnitpanelGrid.Visible = true;
            UnitpanelInput.Visible = false;
            PricePanelGrid.Visible = true;
            PricePanelInput.Visible = false;

            btDelete.Enabled = true;
            btEdit1.Enabled = true;
            btEdit2.Enabled = true;
            tbxMaterialCd.Enabled = false;
            lblUnitNm.Visible = false;
            tbxUnitNm.Visible = false;
            btClear_Click(sender, e);
        }
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        using (DBContext DBContext = new DBContext())
        {
            System.Data.Common.DbTransaction tran = DBContext.UseTransaction();
            try
            {
                if (btDelete.Enabled)//edit mode
                {
                    var row = new MasterMaterial();
                    row.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
                    row.MaterialCd = Convert.ToInt32(tbxMaterialCd.Text);
                    row.MaterialNm = tbxMaterialNm.Text;
                    row.MaterialCADNm = tbxMaterialCADNm.Text;
                    row.ProductMaker = tbxProductMaker.Text;
                    row.ProductCd = tbxProductCd.Text;
                    row.ProductNm = tbxProductNm.Text;
                    row.LentPrice = float.Parse(tbxLentPrice.Text);
                    row.ShadeFlg = cbxShadeFlg.Checked;
                    row.ApplyDate = Common.GetNullableDateTime(txtApplyDate.Text);
                    row.TerminateDate = Common.GetNullableDateTime(txtTerminateDate.Text);
                    row.ModifiedAccount = this.User.Identity.Name;

                    List<MasterUnit> listUnit = (List<MasterUnit>)ViewState["listUnitNew"];
                    List<MasterMaterialPrice> listMPrice = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
                    if (listUnit != null)
                    {
                        foreach (MasterUnit unit in listUnit)
                        {
                            MasterUnit msUnit = MasterUnit.GetMstUnit(unit.OfficeCd, unit.UnitCd);
                            if (msUnit != null)
                                unit.Update();
                            else unit.Insert();
                        }
                    }
                    if (listMPrice != null)
                    {
                        foreach (MasterMaterialPrice mPrice in listMPrice)
                        {
                            MasterMaterialPrice msMPrice = MasterMaterialPrice.GetMaterialPrice(mPrice.OfficeCd, mPrice.MaterialCd, mPrice.StartDate.ToString(), mPrice.SupplierCd);
                            if (msMPrice != null)
                                mPrice.Update();
                            else mPrice.Insert();
                        }
                    }
                    row.Update();
                    tran.Commit();
                }
                else// add mode
                {
                    var row = MasterMaterial.GetMaterialMaster(int.Parse(HiddenFieldOfficeCd.Value), Convert.ToInt32(tbxMaterialCd.Text));
                    if (row != null)
                    {
                        RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MaterialIsExisted.Text")) + "\");");
                        return;
                    }
                    row = new MasterMaterial()
                    {
                        OfficeCd = int.Parse(HiddenFieldOfficeCd.Value),
                        MaterialCd = Convert.ToInt32(tbxMaterialCd.Text),
                        MaterialNm = tbxMaterialNm.Text,
                        MaterialCADNm = tbxMaterialCADNm.Text,
                        ProductMaker = tbxProductMaker.Text,
                        ProductCd = tbxProductCd.Text,
                        ProductNm = tbxProductNm.Text,
                        LentPrice = float.Parse(tbxLentPrice.Text),
                        ShadeFlg = cbxShadeFlg.Checked,
                        CreateAccount = this.User.Identity.Name,
                        ModifiedAccount = this.User.Identity.Name
                    };
                    row.ApplyDate = Common.GetNullableDateTime(txtApplyDate.Text);
                    row.TerminateDate = Common.GetNullableDateTime(txtTerminateDate.Text);

                    List<MasterUnit> listUnit = (List<MasterUnit>)ViewState["listUnitNew"];
                    List<MasterMaterialPrice> listMPrice = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
                    if (listUnit != null)
                    {
                        foreach (MasterUnit unit in listUnit)
                        {
                            MasterUnit msUnit = MasterUnit.GetMstUnit(unit.OfficeCd, unit.UnitCd);
                            if (msUnit != null)
                                unit.Update();
                            else unit.Insert();
                        }
                    }
                    if (listMPrice != null)
                    {
                        foreach (MasterMaterialPrice mPrice in listMPrice)
                        {
                            MasterMaterialPrice msMPrice = MasterMaterialPrice.GetMaterialPrice(mPrice.OfficeCd, mPrice.MaterialCd, mPrice.StartDate.ToString(), mPrice.SupplierCd);
                            if (msMPrice != null)
                                mPrice.Update();
                            else mPrice.Insert();
                        }
                    }
                    row.Insert();
                    tran.Commit();
                }
                panelGrid.Visible = true;
                panelInput.Visible = false;
                btSearch_Click(sender, e);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

    }

    protected void btDelete_Click(object sender, EventArgs e)
    {
        using (DBContext DBContext = new DBContext())
        {
            System.Data.Common.DbTransaction tran = DBContext.UseTransaction();
            try
            {
                //Check Stock  
                List<MasterStock> listStock = MasterStock.GetAmountStock(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text));
                List<MasterUnit> listUnit = (List<MasterUnit>)ViewState["listUnitNew"];
                List<MasterMaterialPrice> listMPrice = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
                if (listUnit.Count > 0)
                {
                    for (int i = 0; i < listUnit.Count; i++)
                    {
                        if (listUnit[i].TerminateDate == null || listUnit[i].TerminateDate.Value.Date > DateTime.Now.Date)
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("UnitUsing.Text")) + "\");");
                            return;
                        }
                    }
                }
                if (listMPrice.Count > 0)
                {
                    for (int i = 0; i < listMPrice.Count; i++)
                    {
                        if (listMPrice[i].EndDate == null || listMPrice[i].EndDate.Value.Date > DateTime.Now.Date )
                        {
                            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("PriceUsing.Text")) + "\");");
                            return;
                        }
                    }
                }

                double Amount =0;
                if(listStock.Count > 0)
                    Amount = listStock.Sum(c=>c.Amount);                
                if (listStock.Count ==0 || Amount == 0)
                {                             
                    if (listUnit != null)
                    {                        
                        foreach (MasterUnit unit in listUnit)
                        {                            
                            MasterUnit msUnit = MasterUnit.GetMstUnit(unit.OfficeCd, unit.UnitCd);                            
                            if (msUnit != null)
                                unit.Update();
                            else unit.Insert();
                        }
                    }
                    if (listMPrice != null)
                    {
                        foreach (MasterMaterialPrice mPrice in listMPrice)
                        {
                            MasterMaterialPrice msMPrice = MasterMaterialPrice.GetMaterialPrice(mPrice.OfficeCd, mPrice.MaterialCd, mPrice.StartDate.ToString(), mPrice.SupplierCd);
                            if (msMPrice != null)
                                mPrice.Update();
                            else mPrice.Insert();
                        }
                    }
                    if (listStock.Count >0 && Amount == 0)
                    {
                        foreach (MasterStock msStock in listStock)
                        {
                            msStock.Delete();
                        }
                    }
                    //Delete Material
                    MasterMaterial mstObject = MasterMaterial.GetMaterialMaster(int.Parse(HiddenFieldOfficeCd.Value), Convert.ToInt32(tbxMaterialCd.Text));
                    mstObject.Delete();
                    tran.Commit();
                }                
                else
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("StockAmount.Text")) + "\");");
                    return;
                }
                panelGrid.Visible = true;
                panelInput.Visible = false;
                //gridMaterial.DataSource = _materialController.GetMstMaterials(int.Parse(txtOffice.Text));
                //gridMaterial.DataBind();
                btSearch_Click(sender, e);

            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
                tran.Rollback();
                throw ex;            
            }
        }
    }

    protected void btCancel_Click(object sender, EventArgs e)
    {
        panelGrid.Visible = true;
        panelInput.Visible = false;
        btSearch_Click(sender, e);
    }

    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtName.Text = "";
    }
    #endregion

    //****************************************************************************************
    #region Unit

    private void GetUnit()
    {

        RemoveViewStateUnit();
        int _all;
        if (cbShowAllUnit.Checked)
            _all = 1;
        else _all = 0;
        List<MasterUnit> info = MasterUnit.GetMstUnitByMaterialCd(_all, int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text));        
        ViewState.Add("listUnit", info);
        ViewState.Add("listUnitNew", info);
        FillListUnit((List<MasterUnit>)ViewState["listUnit"]);
    }
    protected void FillListUnit(List<MasterUnit> List)
    {        
        if (List.Count == 0)
        {
            //Create List empty            
            MasterUnit blankObj = new MasterUnit();
            blankObj.UnitCd = "-1";
            blankObj.UnitNm = "";
            blankObj.ApplyDate = DateTime.Today;
            blankObj.TerminateDate = DateTime.Today;
            List.Add(blankObj);
            gridUnit.DataSource = List;
            gridUnit.DataBind();
            if (List.Count == 1 && List[0].UnitCd == "-1")
                gridUnit.Rows[0].Visible = false;
            List.Remove(blankObj);
        }
        else
        {
            gridUnit.DataSource = List;
            gridUnit.DataBind();
            foreach (GridViewRow row in gridUnit.Rows)
            {
                if (row.Cells[2].Text == "True")
                    row.Cells[2].Text = "√";
                else row.Cells[2].Text = "";
                if (Common.GetRowString(row.Cells[4].Text) != "")
                    row.Cells[4].Text = SetDateFormat(Convert.ToDateTime(row.Cells[4].Text).ToShortDateString());

                if (Common.GetRowString(row.Cells[5].Text) != "")
                    row.Cells[5].Text = SetDateFormat(Convert.ToDateTime(row.Cells[5].Text).ToShortDateString());
            }
        }
    }

    protected void gridUnit_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();

            //Add CheckBoxSelected
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);
           
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblUnitNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(130);
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblPriority.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(40);
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblAmountByMinimumUnit.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(40);
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblApplyDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblTerminateDate.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(80);
            oGridViewRow.Cells.Add(oTableCell);

            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("lblUnitNote.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add custom header            
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }

    protected void btRegister1_Click(object sender, EventArgs e)
    {
        lblCheckNewMaterial.Text = "1";
        try
        {
            string s = MasterUnit.GetUnitCd(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text));
            //tbxUnitCd.Enabled = true;
            if (string.IsNullOrWhiteSpace(s))
                tbxUnitCd.Text = tbxMaterialCd.Text + "-1";
            else
            {
                string s1 = s.Substring(s.IndexOf('-') + 1);
                int i = int.Parse(s1);
                i++;
                tbxUnitCd.Text = tbxMaterialCd.Text + "-" + i.ToString();
            }
        }
        catch
        {
            tbxUnitCd.Text = tbxMaterialCd.Text + "-1";
        }
        tbxUnitNm1.Text = "";
        cbPriority.Checked = false;
        cbPriority.Enabled = true;
        tbxAmountByMinimumUnit.Text = "1";
        tbxAmountByMinimumUnit.Enabled = true;
        txtApplyDate1.Text = "";//txtApplyDate.Text;
        txtTerminateDate1.Text = "";    // txtTerminateDate.Text;
        tbxUnitNote.Text = "";
        UnitpanelGrid.Visible = false;
        UnitpanelInput.Visible = true;
        //MaterialPanelInput.Enabled = false;
        btDelete1.Enabled = false;
        tbxUnitNm1.Focus();        
        btCancel.Enabled = true;
        lbTest.Visible = false;
    }

    protected void btEdit1_Click(object sender, EventArgs e)
    {
        var checkedIDs = (from GridViewRow msgRow in gridUnit.Rows
                            where ((CheckBox)msgRow.FindControl("Check")).Checked
                            select gridUnit.DataKeys[msgRow.RowIndex].Value.ToString()).ToList();
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
            tbxUnitCd.Enabled = false;
            List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
            var row = list.Where(c => c.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value) && c.UnitCd == checkedIDs[0]).ToList();//MasterUnit.GetMstUnit(int.Parse(HiddenFieldOfficeCd.Value), checkedIDs[0]);
            tbxUnitCd.Text = row[0].UnitCd;
            tbxUnitNm1.Text = row[0].UnitNm;
            cbPriority.Checked = row[0].Priority;
            tbxAmountByMinimumUnit.Text = row[0].AmountByMinimumUnit.Value.ToString();
            tbxAmountByMinimumUnit.Enabled = false;
            cbPriority.Enabled = false;
            if (row[0].ApplyDate != null)
                txtApplyDate1.Text = SetDateFormat(row[0].ApplyDate.Value.ToString());
            else
                txtApplyDate1.Text = "";

            if (row[0].TerminateDate != null)
                txtTerminateDate1.Text = SetDateFormat(row[0].TerminateDate.Value.ToString());
            else 
                txtTerminateDate1.Text = "";

            tbxUnitNote.Text = row[0].UnitNote;
            UnitpanelGrid.Visible = false;
            UnitpanelInput.Visible = true;
            btDelete1.Enabled = true;
            //MaterialPanelInput.Enabled = false;
            btCancel.Enabled = true;
            lbTest.Visible = false;
        }
    }

    protected void btSave1_Click(object sender, EventArgs e)
    {        
        if (txtApplyDate1.Text != "") //&& DateTime.Parse(txtApplyDate1.Text) <= DateTime.Today
        {
            if (txtTerminateDate1.Text == "" || DateTime.Parse(txtTerminateDate1.Text) >= DateTime.Today)                
                if ((MasterUnit.GetPriorityOfUnitByMaterial(int.Parse(HiddenFieldOfficeCd.Value), tbxMaterialCd.Text, tbxUnitCd.Text, txtApplyDate1.Text, txtTerminateDate1.Text) > 0) && cbPriority.Checked == true)
                {
                    RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_ONLYONE_PRIORITY_TRUE.Text")) + "\");");
                    return;
                }
        }
        if (cbPriority.Checked == true && tbxAmountByMinimumUnit.Text != "1")
        {
            lbTest.Text = txtPriority.Text;
            lbTest.Visible = true;
            if (tbxAmountByMinimumUnit.Enabled == true)
                tbxAmountByMinimumUnit.Focus();
            return;
        }
        
        //Check Priority = true and EndDate -> Amout = 0
        if (cbPriority.Checked == true && txtTerminateDate1.Text != "")
        {
            if (MasterStock.GetMstStockWithMaterial(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text), tbxUnitCd.Text) > 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_ENDDATE_AND_PRIORITY_TRUE.Text")) + "\");");
                return;
            }
        }

        if (string.IsNullOrWhiteSpace(tbxAmountByMinimumUnit.Text))
            tbxAmountByMinimumUnit.Text = "1";
        MasterUnit unit = new MasterUnit
        {
            OfficeCd = int.Parse(HiddenFieldOfficeCd.Value),
            MaterialCd = int.Parse(tbxMaterialCd.Text),
            UnitCd = tbxUnitCd.Text,  //row.MaterialCd.ToString() + "-1"
            UnitNm = tbxUnitNm1.Text,
            CreateAccount = this.User.Identity.Name,
            ModifiedAccount = this.User.Identity.Name,
            Priority = cbPriority.Checked,
            AmountByMinimumUnit = int.Parse(tbxAmountByMinimumUnit.Text),
            UnitNote = tbxUnitNote.Text
        };

        if (!string.IsNullOrWhiteSpace(txtApplyDate1.Text))
            unit.ApplyDate = Convert.ToDateTime(txtApplyDate1.Text);
        if (!string.IsNullOrWhiteSpace(txtTerminateDate1.Text))
            unit.TerminateDate = Convert.ToDateTime(txtTerminateDate1.Text);

        if (btDelete1.Enabled)// edit
        {
            List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
            if (list == null)
                list = new List<MasterUnit>();
            list = list.Where(c => c.UnitCd != unit.UnitCd).ToList();           
            list.Add(unit);
            list = list.OrderBy(c=>c.UnitCd).ToList();
            ViewState.Remove("listUnitNew");
            ViewState.Add("listUnitNew", list);
        }
        else // register
        {
            List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
            if (list == null)
                list = new List<MasterUnit>();
            list.Add(unit);
            list = list.OrderBy(c => c.UnitCd).ToList();
            ViewState.Remove("listUnitNew");
            ViewState.Add("listUnitNew", list);
        }
        UnitpanelInput.Visible = false;
        UnitpanelGrid.Visible = true;

        List<MasterUnit> listU = (List<MasterUnit>)ViewState["listUnitNew"];
        if (!cbShowAllUnit.Checked)
            listU = listU.Where(c => c.TerminateDate == null || c.TerminateDate.Value.Date >= DateTime.Now.Date).ToList();
        FillListUnit(listU);

        btEdit1.Enabled = true;
        MaterialPanelInput.Enabled = true;
        tbxMaterialCd.Enabled = false;
    }

    protected void btCancel1_Click(object sender, EventArgs e)
    {
        UnitpanelGrid.Visible = true;
        UnitpanelInput.Visible = false;
        MaterialPanelInput.Enabled = true;
        tbxMaterialCd.Enabled = false;
    }

    protected void btDelete1_Click(object sender, EventArgs e)
    {
        //MstStockController msc = new MstStockController();
        //Check Priority = true va EndDate -> Amout = 0
        if (cbPriority.Checked == true)
        {
            if (MasterStock.GetMstStockWithMaterial(int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text), tbxUnitCd.Text) > 0)
            {
                RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_ENDDATE_AND_PRIORITY_TRUE.Text")) + "\");");
                return;
            }
        }

        //MstUnitController _unitController = new MstUnitController();
        //MasterUnit.DeleteMstUnitByEndDate(int.Parse(HiddenFieldOfficeCd.Value), tbxUnitCd.Text);// set termidate = datetime.now
        List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
        var unit = list.Where(c => c.UnitCd == tbxUnitCd.Text).ToList();
        list = list.Where(c => c.UnitCd != tbxUnitCd.Text).ToList();
        unit[0].TerminateDate = DateTime.Now;
        list.AddRange(unit);
        list = list.OrderBy(c => c.UnitCd).ToList();
        ViewState.Remove("listUnitNew");
        ViewState.Add("listUnitNew", list);
        FillListUnit((List<MasterUnit>)ViewState["listUnitNew"]);

        UnitpanelGrid.Visible = true;
        UnitpanelInput.Visible = false;
        MaterialPanelInput.Enabled = true;
        btCancel.Enabled = true;
    }

    protected void cbShowAllUnit_CheckedChanged(object sender, EventArgs e)
    {
        List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
        GetlistUnitNew(list);
        //GetUnit
    }

    protected void gridUnit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridUnit.PageIndex = e.NewPageIndex;
        List<MasterUnit> list = (List<MasterUnit>)ViewState["listUnitNew"];
        GetlistUnitNew(list);
    }

    protected void GetlistUnitNew(List<MasterUnit> list)
    {
        if (list == null)
            list = new List<MasterUnit>();
        if (!cbShowAllUnit.Checked)
            list = list.Where(c => c.TerminateDate == null || c.TerminateDate.Value.Date >= DateTime.Now.Date).ToList();
        FillListUnit(list);
    }

    protected void tbxAmountByMinimumUnit_TextChanged(object sender, EventArgs e)
    {
        if (cbPriority.Checked == true)
        {
            if (tbxAmountByMinimumUnit.Text != "1")
            {

                lbTest.Text = txtPriority.Text;
                lbTest.Visible = true;
                tbxAmountByMinimumUnit.Focus();
            }
            else
            {
                lbTest.Text = "";
                lbTest.Visible = false;
            }

        }
        else
        {
            lbTest.Text = "";
            lbTest.Visible = false;
        }
    }
    #endregion

    //****************************************************************************************
    #region MaterialPrice

    private void GetMaterialPrices()
    {
        RemoveViewStateMaterialPrice();
        int _all;
        if (cb.Checked)
            _all = 1;
        else _all = 0;
        List<MasterMaterialPrice> mpinfo = MasterMaterialPrice.GetMaterialPriceByMaterialCd(_all, int.Parse(HiddenFieldOfficeCd.Value), int.Parse(tbxMaterialCd.Text));
        ViewState.Add("listMaterialPrice", mpinfo);
        ViewState.Add("listMaterialPriceNew", mpinfo); 
        FillListMaterialPrice((List<MasterMaterialPrice>)ViewState["listMaterialPrice"]);
    }    
    protected void FillListMaterialPrice(List<MasterMaterialPrice> List)
    {        
        if (List.Count == 0)
        {           
            //Create List empty
            MasterMaterialPrice blankObj1 = new MasterMaterialPrice();
            blankObj1.MaterialCd = -1;
            blankObj1.SupplierCd = -1;
            blankObj1.StartDate = DateTime.Today;
            blankObj1.EndDate = DateTime.Today;
            List.Add(blankObj1);
            gridMaterialPrice.DataSource = List;
            gridMaterialPrice.DataBind();            
            if (List.Count == 1 && List[0].MaterialCd == -1)
                gridMaterialPrice.Rows[0].Visible = false;
            List.Remove(blankObj1);
        }
        else
        {
            gridMaterialPrice.DataSource = List;
            gridMaterialPrice.DataBind();         
   
            foreach (GridViewRow row in gridMaterialPrice.Rows)
            {
                row.Cells[3].Text = SetDateFormat(row.Cells[3].Text);
                if (Common.GetRowString(row.Cells[4].Text) == "")
                    row.Cells[4].Text = "";
                else
                    row.Cells[4].Text = SetDateFormat(row.Cells[4].Text);                

                ListItem item1 = DropDownListSup.Items.FindByValue(row.Cells[1].Text);
                if (item1 != null)
                    row.Cells[2].Text = item1.Text;
                else row.Cells[2].Text = "";
            }
        }       
    }

    protected void btRegister2_Click(object sender, EventArgs e)
    {
        try
        {
            lblCheckNewMaterialPrice.Text = "1";
            PricePanelGrid.Visible = false;
            PricePanelInput.Visible = true;

            txtMaCd.Enabled = false;
            txtSupCd.Enabled = true;
            DropDownListSup.Enabled = true;
            txtStartDate.Enabled = true;
            hplStartDate.Enabled = true;
            btDelete2.Enabled = false;

            hplStartDate.NavigateUrl = Calendar.InvokePopupCal(txtStartDate);
            hplEndDate.NavigateUrl = Calendar.InvokePopupCal(txtEndDate);

            txtMaCd.Text = tbxMaterialCd.Text;
            txtMaName.Text = tbxMaterialNm.Text;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtSupCd.Text = "";            
            txtPriceName.Text = "";
            txtPrice.Enabled = true;
            txtPrice.Text = "0";
            DropDownListSup.SelectedValue = "";
        }
        catch (Exception) { }
    }

    protected void btEdit2_Click(object sender, EventArgs e)
    {
        int i = 0;

        //gridMaterialPrice.Columns[1].Visible = true;

        foreach (GridViewRow msgRow in gridMaterialPrice.Rows)
        {
            try
            {
                CheckBox ck = (CheckBox)msgRow.FindControl("Check");
                if (ck.Checked == true)
                {
                    i++;
                    txtSupCd.Text = gridMaterialPrice.Rows[msgRow.RowIndex].Cells[1].Text;
                    txtStartDate.Text = gridMaterialPrice.Rows[msgRow.RowIndex].Cells[3].Text;
                    txtEndDate.Text = Common.GetRowString(gridMaterialPrice.Rows[msgRow.RowIndex].Cells[4].Text);
                    txtPriceName.Text = Common.GetRowString(gridMaterialPrice.Rows[msgRow.RowIndex].Cells[5].Text);
                    txtPrice.Text = gridMaterialPrice.Rows[msgRow.RowIndex].Cells[6].Text; 

                }
            }
            catch (Exception) { }
        }
        //gridMaterialPrice.Columns[1].Visible = false;

        if (i == 0)
        {
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");");
        }
        else if (i > 1)
        {
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");");
        }
        else
        {
            PricePanelGrid.Visible = false;
            PricePanelInput.Visible = true;

            lblCheckNewMaterialPrice.Text = "-1";
            //hplStartDate.NavigateUrl = Calendar.InvokePopupCal(txtStartDate);
            hplEndDate.NavigateUrl = Calendar.InvokePopupCal(txtEndDate);

            txtStartDate.Enabled = false;
            hplStartDate.Enabled = false;
            txtSupCd.Enabled = false;
            DropDownListSup.Enabled = false;
            btDelete2.Enabled = true;

            txtMaCd.Text = tbxMaterialCd.Text;
            txtMaName.Text = tbxMaterialNm.Text;

            DropDownListSup.SelectedValue = txtSupCd.Text; //= SupplierCd
            //txtSupName.Text = DropDownListSup.SelectedItem.Text;
            txtPrice.Enabled = false;
            }
    }
        
    protected void btSave2_Click(object sender, EventArgs e)
    {        
        MasterMaterialPrice mappInfo = new MasterMaterialPrice();
        try
        {
            mappInfo.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            mappInfo.StartDate = Convert.ToDateTime(txtStartDate.Text);
            mappInfo.MaterialCd = int.Parse(txtMaCd.Text);
            mappInfo.SupplierCd = int.Parse(txtSupCd.Text);

            mappInfo.PriceNm = txtPriceName.Text;
            mappInfo.Price = int.Parse(txtPrice.Text);

            mappInfo.CreateAccount = this.User.Identity.Name;
            mappInfo.ModifiedAccount = this.User.Identity.Name;

            if (string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                mappInfo.EndDate = null;
            else
            {
                mappInfo.EndDate = Common.GetNullableDateTime(txtEndDate.Text);
            }

            if (lblCheckNewMaterialPrice.Text == "1")
            {
                //Insert  
                //MaterialPriceInfo mapcheck = new MaterialPriceInfo();
                MasterMaterialPrice mapcheck = new MasterMaterialPrice();
                mapcheck = MasterMaterialPrice.GetMaterialPrice(int.Parse(txtMaCd.Text), int.Parse(txtSupCd.Text), txtStartDate.Text, int.Parse(HiddenFieldOfficeCd.Value));
                if (mapcheck != null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("check_Duplicate_key.Text")) + "\");", true);
                    return;
                }
                else
                {
                    //mapConstr.AddMaterialPrice(mappInfo);
                    List<MasterMaterialPrice> list = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
                    if (list == null)
                        list = new List<MasterMaterialPrice>();
                    list.Add(mappInfo);
                    list = list.OrderBy(c =>c.MaterialCd).ThenBy(c=>c.SupplierCd).ThenBy(c=>c.StartDate).ToList();
                    ViewState.Remove("listMaterialPriceNew");
                    ViewState.Add("listMaterialPriceNew", list);
                }
            }
            else
            {
                //Update
                //mapConstr.UpdateMaterialPrice(mappInfo);
                List<MasterMaterialPrice> list = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
                if (list == null)
                    list = new List<MasterMaterialPrice>();
                list.RemoveAll(c =>c.MaterialCd == mappInfo.MaterialCd && c.SupplierCd == mappInfo.SupplierCd && c.StartDate.Date == mappInfo.StartDate.Date);
                //list.RemoveAt()
                list.Add(mappInfo);
                list = list.OrderBy(c => c.MaterialCd).ThenBy(c => c.SupplierCd).ThenBy(c => c.StartDate).ToList();
                ViewState.Remove("listMaterialPriceNew");
                ViewState.Add("listMaterialPriceNew", list);
            }

            //GetMaterialPrices();
            List<MasterMaterialPrice> listMP = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
            if (!cb.Checked)
                listMP = listMP.Where(c => c.EndDate == null || c.EndDate.Value.Date >= DateTime.Now.Date).ToList();
            FillListMaterialPrice(listMP);            
            PricePanelGrid.Visible = true;
            PricePanelInput.Visible = false;

        }
        catch (Exception ex)
        {
            if (logger.IsErrorEnabled) logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + "() error. ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            //Exceptions.ProcessModuleLoadException(this, ex);
        }
    }

    protected void btDelete2_Click(object sender, EventArgs e)
    {
        //MaterialPriceController mapconstr = new MaterialPriceController();
        //MasterMaterialPrice.DeleteMaterialPriceByEndDate(int.Parse(txtMaCd.Text), int.Parse(txtSupCd.Text), DateTime.Parse(txtStartDate.Text), int.Parse(HiddenFieldOfficeCd.Value));
        // Set endDate = datetime.now
        List<MasterMaterialPrice> list = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
        var mPrice = list.Where(c => c.MaterialCd == Convert.ToInt32(tbxMaterialCd.Text) && c.SupplierCd == Convert.ToInt32(txtSupCd.Text) && c.StartDate.Date == Convert.ToDateTime(txtStartDate.Text).Date).ToList();               
        list.RemoveAll(c => c.MaterialCd == Convert.ToInt32(tbxMaterialCd.Text) && c.SupplierCd == Convert.ToInt32(txtSupCd.Text) && c.StartDate.Date == Convert.ToDateTime(txtStartDate.Text).Date);

        mPrice[0].EndDate = DateTime.Now;
        list.AddRange(mPrice);
        list = list.OrderBy(c => c.MaterialCd).ThenBy(c => c.SupplierCd).ThenBy(c => c.StartDate).ToList();
        ViewState.Remove("listMaterialPriceNew");
        ViewState.Add("listMaterialPriceNew", list);
        FillListMaterialPrice((List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"]);        
        PricePanelGrid.Visible = true;
        PricePanelInput.Visible = false;        
    }

    protected void btCancel2_Click(object sender, EventArgs e)
    {
        PricePanelGrid.Visible = true;
        PricePanelInput.Visible = false;
        txtSupCd.Enabled = true;
        DropDownListSup.Enabled = true;
        txtStartDate.Enabled = true;
        hplStartDate.Enabled = true;
    }

    protected void gridMaterialPrice_RowCreated(object sender, GridViewRowEventArgs e)
    {
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

            //add supplierCd
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblSupCd.Text");
            tblcell.Width = Unit.Pixel(40);
            gridrow.Cells.Add(tblcell);


            //add supplier Nm
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblSupNm.Text");
            tblcell.Width = Unit.Pixel(180);
            gridrow.Cells.Add(tblcell);

            //add startDate header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblStartDate.Text");
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            //add end date header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = lblEndDDate.Text = GetResource("lblEndDate.Text");
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            //add price name header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = lblPriceName.Text = GetResource("lblPriceNm.Text");
            tblcell.Width = Unit.Pixel(130);
            gridrow.Cells.Add(tblcell);



            //add price header
            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblPrice.Text");
            tblcell.Width = Unit.Pixel(50);
            gridrow.Cells.Add(tblcell);


            //Add header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }

    protected void gridMaterialPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridMaterialPrice.PageIndex = e.NewPageIndex;
        //GetMaterialPrices();
        List<MasterMaterialPrice> list = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
        GetlistMaPriceNew(list);
    }   

    protected void DropDownListSup_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSupCd.Text = DropDownListSup.SelectedValue;        
    }

    protected void cb_CheckedChanged(object sender, EventArgs e)
    {
        //GetMaterialPrices();
        List<MasterMaterialPrice> list = (List<MasterMaterialPrice>)ViewState["listMaterialPriceNew"];
        GetlistMaPriceNew(list);
    }

    protected void GetlistMaPriceNew(List<MasterMaterialPrice> list)
    {
        if (list == null)
            list = new List<MasterMaterialPrice>();
        if (!cb.Checked)
            list = list.Where(c => c.EndDate == null || c.EndDate.Value.Date >= DateTime.Now.Date).ToList();
        FillListMaterialPrice(list);
    }
    #endregion
}
