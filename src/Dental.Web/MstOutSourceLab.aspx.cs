using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Dental.Domain;
using Dental.Utilities;

public partial class MstOutSourceLab : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(MstOutSourceLab));
    protected void Page_Load(object sender, EventArgs e)
    {
        HiddenFieldOfficeCd.Value = GetOffice();

        try
        {
            logger.Info(System.Reflection.MethodBase.GetCurrentMethod().Name + "  run!");
            if (!IsPostBack)
            {
                InitLanguage();
                Initailize();
                Search();
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error Page_Load", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    private void FillGridView()
    {
        List<MasterOutsourceLab> listOursource = new List<MasterOutsourceLab>();
        listOursource = MasterOutsourceLab.GetAll();
        listOursource = listOursource.Where(l => (l.OfficeCd == int.Parse(HiddenFieldOfficeCd.Value))).ToList();
        gridOutsource.DataSource = listOursource;
        gridOutsource.DataBind();
    }

    private void Initailize()
    {
        btnDelete.Attributes["onclick"] = "javascript:return confirm(' " + GetResource("msDelete.Text") + " ');";
        //-------------add label & button name----------------
        lblOutsourceCd.Text = GetResource("lblOsCd.Text");
        lblOutsourceNm.Text = GetResource("lblOsNm.Text");
        lblOutsourcePostalCd.Text = GetResource("lblOsPostalCd.Text");
        lblOutsourceAddress1.Text = GetResource("lblOsAddress1.Text");
        lblOoutsourceAddress2.Text = GetResource("lblOsAddress2.Text");
        lblOutsourceTEL.Text = GetResource("lblOsTEL.Text");
        lblOutsourceFAX.Text = GetResource("lblOsFAX.Text");
        lblOutsourceContactPerson.Text = GetResource("lblOsContactPerson.Text");
        lblTitle.Text = GetResource("Title.Text");
        lblCode.Text = GetResource("lblOsCd.Text");
        lblName.Text = GetResource("lblOsNm.Text");
        lblNoRecord.Text = GetResource("lblNoRecord.Text");

        btnCancel.Text = GetResource("btnCancel.Text");
        btnEdit.Text = GetResource("btnEdit.Text");
        btnReg.Text = GetResource("btnReg.Text");
        btnSave.Text = GetResource("btnSave.Text");
        btnDelete.Text = GetResource("btnDel.Text");
        btSearch.Text = GetResource("btnSearch.Text");
        btClear.Text = GetResource("btnClear.Text");
        //------------------------------------------

        //-------add Validation message----
        
        RequiredFieldValidator2.ErrorMessage = GetResource("check_OsNm.Text");
        RegularExpressionValidator1.ErrorMessage = GetResource("check_Input_Number.Text");
        RequiredFieldValidator3.ErrorMessage = GetResource("check_OSCd.Text");

        //---------------------------------
    }

    protected void gridOutsource_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView grid = (GridView)sender;
            GridViewRow gridrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tblcell = new TableCell();

            //add checkbox header
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(20);
            gridrow.Cells.Add(tblcell);

            //add outsource cd
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsCd.Text");
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            //add outsource Name
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsNm.Text");
            tblcell.Width = Unit.Pixel(150);
            gridrow.Cells.Add(tblcell);

            //add outsource PostalCd
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsPostalCd.Text");
            tblcell.Width = Unit.Pixel(200);
            gridrow.Cells.Add(tblcell);

            //add outsuorce address1
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsAddress1.Text");
            tblcell.Width = Unit.Pixel(200);
            gridrow.Cells.Add(tblcell);

            //add outsource address2
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsAddress2.Text");
            tblcell.Width = Unit.Pixel(200);
            gridrow.Cells.Add(tblcell);

            //add outsource tel
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsTEL.Text");
            tblcell.Width = Unit.Pixel(110);
            gridrow.Cells.Add(tblcell);

            //add outsource fax
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsFAX.Text");
            tblcell.Width = Unit.Pixel(110);
            gridrow.Cells.Add(tblcell);

            // add outsource contact
            tblcell = new TableCell();
            tblcell.CssClass = "td_header";
            tblcell.Text = GetResource("lblOsContactPerson.Text");
            tblcell.Width = Unit.Pixel(200);
            gridrow.Cells.Add(tblcell);

            //ad header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }
    protected void btnReg_Click(object sender, EventArgs e)
    {
        btnDelete.Enabled = false;
        PanelShow.Visible = false;
        PanelEdit.Visible = true;
        txtOutsourceCd.Text = ""; txtOutsourceNm.Text = "";
        txtOutsourcePostalCd.Text = "";
        txtOutsourceAddress1.Text = "";
        txtOutsourceAddress2.Text = "";
        txtOutsourceTEL.Text = "";
        txtOutsourceFAX.Text = "";
        txtOutsourceContactPerson.Text = "";
        txtOutsourceCd.Enabled = true;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            btnDelete.Enabled = true;
            int countchecked = 0;
            CheckBox cb = new CheckBox();
            foreach (GridViewRow r in gridOutsource.Rows)
            {
                cb = (CheckBox)r.Cells[0].FindControl("cb");
                if (cb.Checked)
                {
                    countchecked++;
                    if (countchecked == 2) break;
                    else
                    {
                        txtOutsourceCd.Text = Common.GetRowString(r.Cells[1].Text);
                        txtOutsourceNm.Text = Common.GetRowString(r.Cells[2].Text);
                        txtOutsourcePostalCd.Text = Common.GetRowString(r.Cells[3].Text);
                        txtOutsourceAddress1.Text = Common.GetRowString(r.Cells[4].Text);
                        txtOutsourceAddress2.Text = Common.GetRowString(r.Cells[5].Text);
                        txtOutsourceTEL.Text = Common.GetRowString(r.Cells[6].Text);
                        txtOutsourceFAX.Text = Common.GetRowString(r.Cells[7].Text);
                        txtOutsourceContactPerson.Text = Common.GetRowString(r.Cells[8].Text);
                    }
                }
            }
            if (countchecked == 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_NONE_SELECTED_ITEM.Text")) + "\");", true);
                return;
            }
            else
                if (countchecked == 2)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("MSG_MORE_ONE_SELECTED_ITEM.Text")) + "\");", true);
                    return;
                }
                else
                {
                    PanelShow.Visible = false;
                    PanelEdit.Visible = true;
                    txtOutsourceCd.Enabled = false;
                }
        }
        catch (Exception ex)
        {
            logger.Error("Error Edit: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            MasterOutsourceLab objOutsourceInfo = new MasterOutsourceLab();

            objOutsourceInfo.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            objOutsourceInfo.OutsourceCd = int.Parse(txtOutsourceCd.Text);
            objOutsourceInfo.OutsourceNm = txtOutsourceNm.Text;
            objOutsourceInfo.OutsourcePostalCd = txtOutsourcePostalCd.Text;
            objOutsourceInfo.OutsourceAddress1 = txtOutsourceAddress1.Text;
            objOutsourceInfo.OutsourceAddress2 = txtOutsourceAddress2.Text;
            objOutsourceInfo.OutsourceTEL = txtOutsourceTEL.Text;
            objOutsourceInfo.OutsourceFAX = txtOutsourceFAX.Text;
            objOutsourceInfo.OutsourceContactPerson = txtOutsourceContactPerson.Text;
            objOutsourceInfo.ModifiedAccount = this.User.Identity.Name;
            objOutsourceInfo.ModifiedDate = DateTime.Now;

            MasterOutsourceLab outsouceLab = new MasterOutsourceLab();
            outsouceLab = MasterOutsourceLab.GetOutsourceLabMaster(objOutsourceInfo.OfficeCd, objOutsourceInfo.OutsourceCd);
            //add new record
            if (btnDelete.Enabled == false)
            {
                objOutsourceInfo.CreateAccount = this.User.Identity.Name;
                objOutsourceInfo.CreateDate = DateTime.Now;
                if (outsouceLab == null)
                {
                    objOutsourceInfo.Insert();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("check_Duplicate_key_Code.Text")) + "\");", true);
                    return;
                }
            }
            //update record
            else
            {
                objOutsourceInfo.CreateAccount = outsouceLab.CreateAccount;
                objOutsourceInfo.CreateDate = outsouceLab.CreateDate;
                objOutsourceInfo.Update();
            }
            PanelShow.Visible = true;
            PanelEdit.Visible = false;
            Search();
        }
        catch (Exception ex)
        {
            logger.Error("Error Save: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            MasterOutsourceLab outsource = new MasterOutsourceLab();
            outsource.OfficeCd = int.Parse(HiddenFieldOfficeCd.Value);
            outsource.OutsourceCd = int.Parse(txtOutsourceCd.Text);
            outsource.Delete();
            PanelShow.Visible = true;
            PanelEdit.Visible = false;
            Search();
        }
        catch (Exception ex)
        {
            logger.Error("Error Delete: ", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        PanelShow.Visible = true;
        PanelEdit.Visible = false;
        
    }
    protected void btSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    private void Search()
    {
        try
        {
            lblNoRecord.Visible = false;
            List<MasterOutsourceLab> listOutsource = new List<MasterOutsourceLab>();
            listOutsource = MasterOutsourceLab.GetOutSourceLabSearch(int.Parse(HiddenFieldOfficeCd.Value), txtCode.Text, txtName.Text);

            if (listOutsource.Count == 0)
            {
                MasterOutsourceLab objOutsource = new MasterOutsourceLab();

                objOutsource.OutsourceCd = -1;
                objOutsource.OutsourceNm = "";
                objOutsource.OutsourcePostalCd = "";
                objOutsource.OutsourceAddress1 = "";
                objOutsource.OutsourceAddress2 = "";
                listOutsource.Add(objOutsource);
                gridOutsource.DataSource = listOutsource;
                gridOutsource.DataBind();
                if (listOutsource.Count == 1 && listOutsource[0].OutsourceCd == -1)
                {
                    gridOutsource.Rows[0].Visible = false;
                }
                lblNoRecord.Visible = true;
                return;
            }
            else
            {
                gridOutsource.DataSource = listOutsource;
                gridOutsource.DataBind();
            }
        }
        catch (Exception e)
        {
            logger.Error("Error Search: ", e);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), e.Message) + "\");", true);
        }
    }
    protected void btClear_Click(object sender, EventArgs e)
    {
        txtCode.Text = txtName.Text = "";
    }
    protected void gridOutsource_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridOutsource.PageIndex = e.NewPageIndex;
        Search();
    }
}