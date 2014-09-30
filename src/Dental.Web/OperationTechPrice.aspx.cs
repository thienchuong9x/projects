using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;


public partial class OperationTechPrice : DDVPortalModuleBase
{
    readonly static ILog logger = LogManager.GetLogger(typeof(OperationTechPrice));

    #region ViewState
    public List<MasterTechPrice> listMstTechPrice
    {
        get
        {
            if (this.ViewState["listMstTechPrice"] == null)
            {
                this.ViewState["listMstTechPrice"] = MasterTechPrice.GetListMasterTechPriceByOrderDate(Convert.ToInt32(hiddenOfficeCd.Value), txtOrderDate.Text, Convert.ToInt32(hiddenDentalOfficeCd.Value));
            }
            return this.ViewState["listMstTechPrice"] as List<MasterTechPrice>;
        }
    }
    public List<OperationTechPriceInfo> listDetail
    {
        get
        {
            if (this.ViewState["OperationMaterial"] == null)
            {
                this.ViewState["OperationMaterial"] = OperationTechPriceInfo.GetOperationTechPriceInfo(Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value), txtOrderDate.Text, Convert.ToInt32(hiddenDentalOfficeCd.Value));
            }
            return this.ViewState["OperationMaterial"] as List<OperationTechPriceInfo>;
        }
    }

    private List<TrnTechPrice> listTechPrice
    {
        get
        {
            if (this.ViewState["CurrentTechPrice"] == null)
            {
                int row = gridViewOrderDetail.SelectedRow.RowIndex;
                this.ViewState["CurrentTechPrice"] = this.listDetail[row].listTechPrice;
            }
            return this.ViewState["CurrentTechPrice"] as List<TrnTechPrice>;
        }
    }

    private void RemoveViewState(params string[] nameState)
    {
        // CurrentDetail
        foreach (string state in nameState)
        {
            if (ViewState[state] != null)
            {
                ViewState.Remove(state);
            }
        }
    }
    #endregion

    #region FillGridView
    private void BindGridView()
    {
        BindGridView(this.listTechPrice);
    }
    private void BindGridView(List<TrnTechPrice> list)
    {
        this.gridViewTechPrice.DataSource = list;
        this.gridViewTechPrice.DataBind();
        if (Convert.ToInt32(((DataBoundLiteralControl)this.gridViewTechPrice.Rows[0].Cells[4].Controls[0]).Text) == 0)
        {
            this.gridViewTechPrice.Rows[0].Visible = false;
        }
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
                this.btnGoProcess.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_PROCESS.Text") + "');");
                this.btnGoOrder.Attributes.Add("onclick", "javascript:return ConfirmGoPage('" + GetResource("MSG_CONFIRM_GO_ORDER.Text") + "');");
                this.btnRegister.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString());
                RemoveViewState("CurrentTechPrice", "OperationMaterial", "listMstTechPrice");
                
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
    }
    private void InitLabel()
    {
        SetLabelText(lblOrderNo, lblOrderDate, lblDueDate, lblClinicName, lblPatient, lblBorrowPart, lblComments, lblGridViewTechPrice, lblTotalCost, lblFullCost, lblUnitPrice);
        lblFullUnitPrice.Text = lblUnitPrice.Text;
        SetButtonText(buttonAddRow, buttonDownRow, buttonDeleteRow, buttonUpRow, btnRegister, btnCancel, btnGoOrder, btnGoProcess);
    }

    private void InitHeader()
    {
        string orderSeq = Request.QueryString["id"];
        string officeCd = GetOffice();
        hiddenOfficeCd.Value = officeCd;
        hiddenOrderSeq.Value = orderSeq;

        if (!string.IsNullOrEmpty(orderSeq))
        {
            TrnOrderHeader header = TrnOrderHeader.GetTrnOrderHeader(Convert.ToInt32(officeCd), Convert.ToDouble(orderSeq));
            SetOrderInputItem(header);
            hiddenDentalOfficeCd.Value = header.DentalOfficeCd.ToString();

            this.gridViewOrderDetail.DataSource = this.listDetail.Select(p => p.detail).ToList();
            this.gridViewOrderDetail.DataBind();

            if (this.listDetail.Count > 0)
            {
                this.gridViewOrderDetail.SelectedIndex = 0;
                SetOperationTechPrice();

                if (this.listDetail.FirstOrDefault(p => string.IsNullOrEmpty(p.detail.BillStatementNo)) == null)
                {
                    panelTechPrice.Enabled = btnRegister.Enabled = false;
                }
            }
            else
            {
                this.buttonAddRow.Enabled = this.btnRegister.Enabled = false;
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
        txtClinicNm.Text = header.DentalOfficeCd.ToString();
        List<CodeName> listDentalOffice = CodeName.GetListCodeNameDentalOfficeMasterByStaff(header.OfficeCd, null);
        txtClinicNm.Text = listDentalOffice.Where(p => p.Code == header.DentalOfficeCd.ToString()).FirstOrDefault().Name;
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

            //Add Detail Nm
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_DETAIL_NAME.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Tooth 
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

            if (this.ViewState["CurrentTechPrice"] != null)
            {
                this.ViewState.Remove("CurrentTechPrice");
            }

            gridViewOrderDetail.SelectedRow.Focus();
            //Show Process In here
            SetOperationTechPrice();
        }
        catch (Exception ex)
        {
            logger.Error("Error gridViewOrderDetail_SelectedIndexChanged ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }
    private void SaveGridViewProcess(string selectedRow)
    {
        if (!string.IsNullOrEmpty(selectedRow))
        {
            int rowIndex = Convert.ToInt32(selectedRow);
            OperationTechPriceInfo info = this.listDetail[rowIndex];

            #region Save Process List
            List<TrnTechPrice> currentList = new List<TrnTechPrice>();
            if (this.ViewState["CurrentTechPrice"] == null)
            {
                logger.Debug(" case ViewState = NULL");
                foreach (GridViewRow r in this.gridViewTechPrice.Rows)
                {
                    string TechCd = Convert.ToString(((DataBoundLiteralControl)r.Cells[1].Controls[0]).Text.Trim());
                    string TechNm = ((DataBoundLiteralControl)r.Cells[2].Controls[0]).Text.Trim();
                    string TechPrice = ((DataBoundLiteralControl)r.Cells[3].Controls[0]).Text.Trim();
                    string ID = ((DataBoundLiteralControl)r.Cells[4].Controls[0]).Text.Trim();

                    logger.Debug(string.Format("first i.TechCd ={0} , i.TechNm = {1} , i.TechPrice = {2}, i.ID= {3} ", TechCd, TechNm, TechPrice, ID));

                    currentList.Add(new TrnTechPrice(Convert.ToInt32(ID), TechCd, TechNm, TechPrice));
                }
            }
            else
            {
                logger.Debug(" case ViewState != NULL");
                currentList = this.ViewState["CurrentTechPrice"] as List<TrnTechPrice>;
            }


            info.listTechPrice = currentList;
            info.DetailChange = 1; //Changed
            #endregion

        }
    }
    private void SetOperationTechPrice()
    {
        OperationTechPriceInfo item = this.listDetail[this.gridViewOrderDetail.SelectedRow.RowIndex];
        if (!string.IsNullOrEmpty(item.detail.BillStatementNo))
            panelTechPrice.Enabled = false;
        else
            panelTechPrice.Enabled = true;
        txtTotalCost.Text = item.listTechPrice.Where(p => (p.TechPrice != null )).Sum(p => Convert.ToDouble(p.TechPrice)).ToString();
        txtFullCost.Text = this.listDetail.Where(x => x.listTechPrice.Any()).Select(x => x.listTechPrice.Where(y => y.TechPrice!=null).Sum(y => Convert.ToDouble(y.TechPrice))).Sum().ToString();
        this.BindGridView(item.listTechPrice);
        hiddenBeforeRowDetail.Value = gridViewOrderDetail.SelectedRow.RowIndex.ToString();
    }

    #endregion

    #region TechPrice Grid
    protected void gridViewTechPrice_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell oTableCell = new TableHeaderCell();
            //Add Action
            oTableCell = new TableHeaderCell();
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add Type
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TechPrice_TechCd.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);

            //Add SalesMan
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TechPrice_TechNm.Text");
            oTableCell.CssClass = "td_header";
            oTableCell.Width = Unit.Pixel(200);
            oGridViewRow.Cells.Add(oTableCell);

            //Add WorkTime
            oTableCell = new TableHeaderCell();
            oTableCell.Text = GetResource("HEADER_TechPrice_TechPrice.Text");
            oTableCell.CssClass = "td_header";
            oGridViewRow.Cells.Add(oTableCell);
            //add
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        }
    }
    protected void gridViewTechPrice_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            // e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            e.Row.Attributes["onclick"] = "javascript:ClickRow(this);";
        }
        if (e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList dropDownTech = (DropDownList)e.Row.FindControl("DropDownTech");
            if (dropDownTech != null)
            {
                FillDropDownListTechPrice(dropDownTech, this.listMstTechPrice);
            }
            if (e.Row.RowType == DataControlRowType.DataRow) //case Row 
            {
                TextBox txtTechCd = (TextBox)e.Row.FindControl("txtTechCd");
                if (txtTechCd != null)
                {
                    GetAutomaticDropDownList(txtTechCd, dropDownTech);
                }
            }
        }
    }
    protected void gridViewTechPrice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gridViewTechPrice.EditIndex = e.NewEditIndex;
        this.BindGridView();
        //Find HC Code

        GridViewRow row = this.gridViewTechPrice.Rows[e.NewEditIndex];
        int ID = Convert.ToInt32(((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text);

        TextBox txtTechCd = row.FindControl("txtTechCd") as TextBox;
        DropDownList DropDownTech = row.FindControl("DropDownTech") as DropDownList;
        TextBox txtTechPrice = row.FindControl("txtTechPrice") as TextBox;
        MasterTechPrice item = this.listMstTechPrice.Where(p => p.TechCd.ToString() == txtTechCd.Text).FirstOrDefault();
        if (item != null && !item.Editable)
        {
            txtTechCd.Enabled = DropDownTech.Enabled = txtTechPrice.Enabled = false;
        }
        else
        {
            txtTechCd.Enabled = DropDownTech.Enabled = txtTechPrice.Enabled = true;
        }
    }

    protected void gridViewTechPrice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gridViewTechPrice.EditIndex = -1;
        this.BindGridView();
    }

    protected void gridViewTechPrice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = this.gridViewTechPrice.Rows[e.RowIndex];
            int ID = Convert.ToInt32(((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text);

            TextBox txtTechCd = row.FindControl("txtTechCd") as TextBox;
            DropDownList DropDownTech = row.FindControl("DropDownTech") as DropDownList;
            TextBox txtTechPrice = row.FindControl("txtTechPrice") as TextBox;

            if (txtTechCd != null && txtTechPrice != null)
            {
                TrnTechPrice obj = this.listTechPrice.Find(c => c.TechDetailNo == ID);

                obj.TechCd = Common.GetNullableInt(txtTechCd.Text);
                if (obj.TechCd == null)
                    obj.TechNm = null;
                else
                {
                    obj.TechNm = DropDownTech.SelectedItem.Text;
                    if (obj.TechNm == "")
                        throw new Exception(GetResource("MSG_ERROR_INPUT_TECH_CD.Text"));
                }
                double oldTechPrice = (obj.TechPrice== null) ? 0 : obj.TechPrice.Value;
                double newTechPrice = 0;
                try
                {
                    newTechPrice = (txtTechPrice.Text.Trim() == string.Empty) ? 0 : Convert.ToDouble(txtTechPrice.Text.Trim());
                }
                catch
                {
                    throw new Exception(GetResource("MSG_ERROR_INPUT_FORMAT_TechPrice.Text"));
                }
                double currentTotalWT = txtTotalCost.Text == "" ? 0 : Convert.ToDouble(txtTotalCost.Text);
                txtTotalCost.Text = (currentTotalWT - oldTechPrice + newTechPrice).ToString();

                //get Full Cost
                txtFullCost.Text = (txtFullCost.Text == "" ? 0 : Convert.ToDouble(txtFullCost.Text) - oldTechPrice + newTechPrice).ToString();

                obj.TechPrice = Common.GetNullableDouble(txtTechPrice.Text);

                this.gridViewTechPrice.EditIndex = -1;
                this.BindGridView();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error gridViewTechPrice_RowUpdating ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void gridViewTechPrice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int customerID = Convert.ToInt32(gridViewProcess.DataKeys[e.RowIndex]["ID"]);
        GridViewRow row = gridViewTechPrice.Rows[e.RowIndex];
        int ID = Convert.ToInt32(((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text);

        TrnTechPrice customer = this.listTechPrice.Find(c => c.TechDetailNo == ID);
        this.listTechPrice.Remove(customer);

        this.BindGridView();
    }
    protected void textBox_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        try
        {
            GridViewRow gridrow = (GridViewRow)textBox.NamingContainer;
            if (textBox.ID == "txtTechCd")
            {
                SetEnableTechPrice(textBox.Text, gridrow);
            }
        }
        catch
        {
            textBox.Text = "";
            textBox.Focus();
        }
    }
    protected void DropDownTech_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropDown = (DropDownList)sender;
        GridViewRow gridrow = (GridViewRow)dropDown.NamingContainer;

        if (dropDown.ID == "DropDownTech")
        {
            SetEnableTechPrice(dropDown.SelectedItem.Value, gridrow);
        }
    }

    private void SetEnableTechPrice(string TechCd, GridViewRow gridrow)
    {
        TextBox txtTechCd = (TextBox)gridrow.FindControl("txtTechCd");
        DropDownList DropDownTech = (DropDownList)gridrow.FindControl("DropDownTech");
        TextBox txtTechPrice = (TextBox)gridrow.FindControl("txtTechPrice");
        MasterTechPrice item = this.listMstTechPrice.Where(p => p.TechCd.ToString() == TechCd).FirstOrDefault();
        if (item != null)
        {
            txtTechCd.Text = DropDownTech.SelectedValue = TechCd;
            txtTechPrice.Text = item.TechPrice.ToString();
            if (item.Editable)
            {
                txtTechPrice.Enabled = true;
                txtTechPrice.Focus();
            }
            else
            {
                txtTechPrice.Enabled = false;
            }
        }
        else
        {
            DropDownTech.SelectedIndex = 0;
            txtTechCd.Text = DropDownTech.SelectedValue;
            txtTechPrice.Text = "";
            txtTechCd.Focus();
        }
    }

    #region Edit Update
    protected void textBoxRow_OnTextChanged(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        GridViewRow gridrow = (GridViewRow)textBox.NamingContainer;
        if (textBox.ID == "txtTechCd")
        {
            SetEnableTechPrice(textBox.Text, gridrow);
        }
    }
    protected void DropDownTechRow_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropDown = (DropDownList)sender;
        GridViewRow gridrow = (GridViewRow)dropDown.NamingContainer;

        if (dropDown.ID == "DropDownTech")
        {
            SetEnableTechPrice(dropDown.SelectedItem.Value, gridrow);
        }
    }
    #endregion

    #endregion

    #region ADD ,DELETE , UP , DOWN
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = this.gridViewTechPrice.FooterRow;
            TextBox txtTechCd = row.FindControl("txtTechCd") as TextBox;
            DropDownList dropDownTech = row.FindControl("DropDownTech") as DropDownList;
            TextBox txtTechPrice = row.FindControl("txtTechPrice") as TextBox;

            if ((txtTechCd != null) && (txtTechPrice != null))
            {
                if (!string.IsNullOrEmpty(txtTechPrice.Text))
                {
                    double currentTotalWT = txtTotalCost.Text == "" ? 0 : Convert.ToDouble(txtTotalCost.Text);
                    try
                    {
                        txtTotalCost.Text = (currentTotalWT + Convert.ToDouble(txtTechPrice.Text)).ToString();
                    }
                    catch
                    {
                        throw new Exception(GetResource("MSG_ERROR_INPUT_FORMAT_TechPrice.Text"));
                    }
                    txtFullCost.Text = (txtFullCost.Text == "" ? 0 : Convert.ToDouble(txtFullCost.Text) + Convert.ToDouble(txtTechPrice.Text)).ToString();
                }

                TrnTechPrice obj = new TrnTechPrice
                {
                    TechDetailNo = this.listTechPrice.Max(c => c.TechDetailNo) + 1,
                    TechCd = Common.GetNullableInt(txtTechCd.Text),
                    TechNm = dropDownTech.SelectedItem.Text,
                    TechPrice = Common.GetNullableDouble(txtTechPrice.Text)
                };
                this.listTechPrice.Add(obj);
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

            GridViewRow row = (GridViewRow)this.gridViewTechPrice.Rows[rowIndex];
            var rows = this.gridViewTechPrice.Rows.Cast<GridViewRow>().Where(a => a != row).ToList();
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
                if (row.RowIndex.Equals(this.gridViewTechPrice.Rows.Count - 1))
                {
                    return;
                    //   rows.Insert(0, row);
                }
                else
                    rows.Insert(row.RowIndex + 1, row);
            }
            this.gridViewTechPrice.DataSource = rows.Select(a => new
            {
                TechCd = Convert.ToString(((DataBoundLiteralControl)a.Cells[1].Controls[0]).Text.Trim()),
                TechNm = ((DataBoundLiteralControl)a.Cells[2].Controls[0]).Text.Trim(),
                TechPrice = ((DataBoundLiteralControl)a.Cells[3].Controls[0]).Text.Trim(),
                TechDetailNo = ((DataBoundLiteralControl)a.Cells[4].Controls[0]).Text.Trim()
            }).ToList();


            this.gridViewTechPrice.DataBind();
            if (Convert.ToInt32(((DataBoundLiteralControl)this.gridViewTechPrice.Rows[0].Cells[4].Controls[0]).Text) == 0)
            {
                this.gridViewTechPrice.Rows[0].Visible = false;
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
            logger.Debug(" btnDelete_Click value = " + hiddenSelectedRowIndex.Value);
            int rowIndex = Convert.ToInt32(hiddenSelectedRowIndex.Value);
            //Delete from ViewState 

            GridViewRow row = (GridViewRow)this.gridViewTechPrice.Rows[rowIndex];
            string TechPrice = (((DataBoundLiteralControl)row.Cells[3].Controls[0]).Text.Trim());


            this.gridViewTechPrice.DeleteRow(rowIndex);
            double currentTechPrice = (TechPrice == "") ? 0 : Convert.ToDouble(TechPrice);

            txtTotalCost.Text = (txtTotalCost.Text == "" ? 0 : Convert.ToDouble(txtTotalCost.Text) - currentTechPrice).ToString();
            txtFullCost.Text = (txtFullCost.Text == "" ? 0 : Convert.ToDouble(txtFullCost.Text) - currentTechPrice).ToString();

            List<TrnTechPrice> list = (List<TrnTechPrice>)this.gridViewTechPrice.DataSource;
            BindGridView(list);
        }
        catch (Exception ex)
        {
            logger.Error("Error btnDelete_Click ", ex);
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }
    #endregion

    #region Common
    private void FillDropDownListTechPrice(DropDownList dropDown, List<MasterTechPrice> listCodeName)
    {
        dropDown.Items.Clear();
        dropDown.Items.Add(new ListItem("", ""));
        foreach (MasterTechPrice i in listCodeName)
        {
            dropDown.Items.Add(new ListItem(i.TechNm, i.TechCd.ToString()));
        }
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

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            logger.Info("Begin btnRegister_Click");
            if (gridViewOrderDetail.SelectedRow == null)
                throw new Exception(GetResource("MSG_NOT_CHOOSE_BRIDGE_GROUP.Text"));

            SaveGridViewProcess(gridViewOrderDetail.SelectedRow.RowIndex.ToString());

            OperationTechPriceInfo.RegisterOperationTechPrice(this.listDetail, Convert.ToInt32(hiddenOfficeCd.Value), Convert.ToDouble(hiddenOrderSeq.Value), this.User.Identity.Name);

            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_INFO.Text"), GetResource("MSG_UPDATE_SUCESS_Full.Text")) + "\");");
            //RemoveViewState("CurrentTechPrice", "OperationMaterial", "listMstTechPrice");
            logger.Info("End btnRegister_Click");
        }
        catch (Exception ex)
        {
            logger.Error("Error btnRegister_Click ", ex);
            this.btnRegister.Enabled = true;
            this.btnRegister.Attributes.Add("onclick", "this.disabled=true;" + Page.ClientScript.GetPostBackEventReference(btnRegister, "").ToString());
            RegisterStartupScript("alert(\"" + GetJSMessage(GetResource("TITLE_ERROR.Text"), ex.Message) + "\");");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentTechPrice", "OperationMaterial", "listMstTechPrice");
        Response.Redirect("OrderList.aspx");
    }

    protected void btnGoProcess_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentTechPrice", "OperationMaterial", "listMstTechPrice");
        Response.Redirect("OperationProcess.aspx?id=" + hiddenOrderSeq.Value);
    }
    protected void btnGoOrder_Click(object sender, EventArgs e)
    {
        RemoveViewState("CurrentTechPrice", "OperationMaterial", "listMstTechPrice");
        Response.Redirect("OrderInput.aspx?id=" + hiddenOrderSeq.Value);
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