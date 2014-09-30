using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstItem : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstItem));

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
            if (!IsPostBack)
            {
                InitLanguage();
                Initailize();
                FillGridView();
            }
        }
        catch(Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    private void Initailize()
    {

        HiddenFieldOfficeCd.Value = GetOffice();
        btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";

        lblItemCathegory.Text = GetResource("lblItemCa.Text");
        lblItemNo.Text = GetResource("lblItemNo.Text");
        lblItemNm.Text = GetResource("lblItemNm.Text");
        lblItemValue.Text = GetResource("lblItemVa.Text");
        lblViewOrder.Text = GetResource("lblVo.Text");
        lblTitle.Text = GetResource("Title.Text");
        lblNoRecord.Visible = true;
        lblNoRecord.Text = GetResource("lblNoRecord.Text");
        lblNoRecord.Visible = false;
        lblCode.Text = GetResource("lblItemCa.Text");
        lblName.Text = GetResource("lblItemNm.Text");
        cbIsDeleted.Text = GetResource("cbIsDeleted_items.Text");

        btnCancel.Text = GetResource("btnCancel.Text");
        btnDelete.Text = GetResource("btnDel.Text");
        btnReg.Text = GetResource("btnReg.Text");
        btnEdit.Text = GetResource("btnEdit.Text");
        btnSave.Text = GetResource("btnSave.Text");
        btnSearch.Text = GetResource("btnSearch.Text");
        btnClear.Text = GetResource("btnClear.Text");

        //----add validation erro message----

        RequiredFieldValidatorICa.ErrorMessage = GetResource("check_ItemCd.Text");
        RequiredFieldValidatorIno.ErrorMessage = GetResource("check_ItemNo.Text");
        RegularExpressionValidatorICa.Text = GetResource("check_ItemNo_input.Text");
        RegularExpressionValidatorIvo.Text = GetResource("check_ItemViewOrder.Text");

        //-----------------------------------
        

    }

    private void FillGridView()
    {

        List<MasterItem> listItems = new List<MasterItem>();
        listItems = MasterItem.GetAll();

        if (!cbIsDeleted.Checked)
        {
            listItems = listItems.Where(i => (i.IsDeleted == false)).ToList();
        }

        if (listItems.Count == 0)
        {
            MasterItem iinfo = new MasterItem();
            iinfo.ItemCathegory = "";
            iinfo.ItemNo = -1;
            iinfo.ItemNm = "";
            listItems.Add(iinfo);
            GridViewItem.DataSource = listItems;
            GridViewItem.DataBind();
            if (listItems.Count == 1 && listItems[0].ItemNo == -1)
            {
                GridViewItem.Rows[0].Visible = false;
            }
            lblNoRecord.Visible = true;
            return;
        }
        else
        {

            GridViewItem.DataSource = listItems;
            GridViewItem.DataBind();
            foreach (GridViewRow r in GridViewItem.Rows)
            {
                if (r.Cells[6].Text == "True")
                    r.Cells[6].Text = "√";
                else
                    r.Cells[6].Text = "";
            }
        }
    }

    private void Search()
    {
        try
        {
            List<MasterItem> listItems = new List<MasterItem>();
            listItems = MasterItem.GetItemMasterSearch(txtCode.Text, txtName.Text);

            if (!cbIsDeleted.Checked)
            {
                listItems = listItems.Where(i => (i.IsDeleted == false)).ToList();
            }

            if (listItems.Count == 0)
            {
                MasterItem iinfo = new MasterItem();
                iinfo.ItemCathegory = "";
                iinfo.ItemNo = -1;
                iinfo.ItemNm = "";
                listItems.Add(iinfo);
                GridViewItem.DataSource = listItems;
                GridViewItem.DataBind();
                if (listItems.Count == 1 && listItems[0].ItemNo == -1)
                {
                    GridViewItem.Rows[0].Visible = false;
                }
                lblNoRecord.Visible = true;
                return;
            }
            else
            {

                GridViewItem.DataSource = listItems;
                GridViewItem.DataBind();
                foreach (GridViewRow r in GridViewItem.Rows)
                {
                    if (r.Cells[6].Text == "True")
                        r.Cells[6].Text = "√";
                    else
                        r.Cells[6].Text = "";
                }
            }
        }
        catch (Exception e)
        {
            logger.Error("Error Search() fuction", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        }
    }

    protected void GridViewItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView grid = (GridView)sender;
            GridViewRow gridrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tblCell = new TableHeaderCell();
            //Add checkbox header
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Width = Unit.Pixel(20);
            gridrow.Cells.Add(tblCell);

            //Add ItemCathegory header
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("lblItemCa.Text" );
            tblCell.Width = Unit.Pixel(250);
            gridrow.Cells.Add(tblCell);

            //Add ItemNo
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("lblItemNo.Text" );
            tblCell.Width = Unit.Pixel(50);
            gridrow.Cells.Add(tblCell);

            //Add ItemNm
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("lblItemNm.Text" );
            tblCell.Width = Unit.Pixel(250);
            gridrow.Cells.Add(tblCell);

            //Add ItemValue
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("lblItemVa.Text" );
            tblCell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblCell);

            //Add ViewOrder
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("lblVo.Text" );
            tblCell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblCell);
                
            //Add isDeleted
            tblCell = new TableHeaderCell();
            tblCell.ColumnSpan = 1;
            tblCell.CssClass = "td_header";
            tblCell.Text = GetResource("colDeleted.Text");
            tblCell.Width = Unit.Pixel(60);
            gridrow.Cells.Add(tblCell);

            //Add header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

        txtCode.Text = txtName.Text = string.Empty;
        
    }
    protected void cbIsDeleted_CheckedChanged(object sender, EventArgs e)
    {
        Search();
    }
    protected void GridViewItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewItem.PageIndex = e.NewPageIndex;
        Search();
    }
    protected void btnReg_Click(object sender, EventArgs e)
    {
        PanelSow.Visible = false;
        PanelEdit.Visible = true;

        txtItemCathegory.Text = ""; txtItemNm.Text = ""; txtItemNo.Text = ""; txtItemValue.Text = txtViewOrder.Text="";
        txtItemCathegory.Enabled = true; txtItemNo.Enabled = true;
        btnDelete.Enabled = false;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            MasterItem item = new MasterItem();
            btnDelete.Enabled = true;
            Label1.Text = "0";
            int countChecked = 0;
            string itemCa = null; string itemNo = null;
            CheckBox cb = new CheckBox();
            foreach (GridViewRow r in GridViewItem.Rows)
            {
                cb = (CheckBox)r.Cells[0].FindControl("cb");
                if (cb.Checked)
                {
                    countChecked++;
                    if (countChecked == 2) break;
                    else
                    {
                        itemCa =Common.GetRowString(r.Cells[1].Text);
                        itemNo = Common.GetRowString(r.Cells[2].Text);
                    }
                }

            }
            if (countChecked == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            else
                if (countChecked == 2)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");", true);
                    return;
                }
                else
                {

                    item = MasterItem.GetItemMaster(itemCa, Convert.ToInt32(itemNo));
                    txtItemCathegory.Text = itemCa;
                    txtItemNo.Text = itemNo;
                    txtItemNm.Text = item.ItemNm;
                    txtItemValue.Text = item.ItemValue;
                    txtViewOrder.Text = Convert.ToString(item.ViewOrder);
                    txtItemCathegory.Enabled = false;
                    txtItemNo.Enabled = false;

                    PanelSow.Visible = false;
                    PanelEdit.Visible = true;
                }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit item!", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PanelSow.Visible = true;
        PanelEdit.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterItem objItemInfo = new MasterItem();
            objItemInfo.ItemCathegory = txtItemCathegory.Text;
            objItemInfo.ItemNo = int.Parse(txtItemNo.Text);
            objItemInfo.ItemNm = txtItemNm.Text;
            objItemInfo.ItemValue = txtItemValue.Text;
            objItemInfo.ViewOrder = Common.GetNullableInt(txtViewOrder.Text);
            objItemInfo.IsDeleted = false;
            objItemInfo.ModifiedDate = DateTime.Now;
            objItemInfo.ModifiedAccount = this.User.Identity.Name;

            MasterItem objItem = new MasterItem();
            objItem = MasterItem.GetItemMaster(objItemInfo.ItemCathegory, objItemInfo.ItemNo);

            //add new record
            if (btnDelete.Enabled == false)
            {
                objItemInfo.CreateDate = DateTime.Now;
                objItemInfo.CreateAccount = this.User.Identity.Name;
                if (objItem == null)
                {
                    objItemInfo.Insert();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("check_Duplicate_key_item.Text")) + "\");", true);
                    return;
                }
            }
            //update record
            else
            {
                objItemInfo.CreateDate = objItem.CreateDate;
                objItemInfo.CreateAccount = objItem.CreateAccount; ;
                objItemInfo.Update();
 
            }
          
            Search();
            PanelSow.Visible = true;
            PanelEdit.Visible = false;
        }
        catch (Exception ex)
        {
            logger.Error("Error Save!", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            MasterItem objItemInfo = new MasterItem();
            objItemInfo.ItemCathegory = txtItemCathegory.Text;
            objItemInfo.ItemNo = int.Parse(txtItemNo.Text);
            objItemInfo.ItemNm = txtItemNm.Text;
            objItemInfo.ItemValue = txtItemValue.Text;
            objItemInfo.ViewOrder = Common.GetNullableInt(txtViewOrder.Text);
            objItemInfo.IsDeleted = true;
            objItemInfo.ModifiedDate = DateTime.Now;
            objItemInfo.ModifiedAccount = this.User.Identity.Name;
            objItemInfo.Update();
            Search();
            PanelSow.Visible = true;
            PanelEdit.Visible = false;
        }
        catch(Exception ex)
        {
            logger.Error("Error Delete!", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
}