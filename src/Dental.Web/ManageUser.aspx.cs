using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using log4net;
using System.Net.Mail;
using Dental.Utilities;
using Dental.Domain;

public partial class ManageUser : DDVPortalModuleBase
{
    private static readonly ILog logger = LogManager.GetLogger(typeof(ManageUser));
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                InitLanguage();
                Initialize();
                Search();
            }
            catch (Exception ex)
            {

                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
    }

    private void Initialize()
    {
        lblNoRecord.Text = GetResource("lblNoRecord.Text");
        btnSearch.Text = GetResource("btnSearch.Text");
        lblUserName.Text = GetResource("lblPopUpUserName.Text");
        cbxOnlineUser.Text = GetResource("cbxOnlineUser.Text");
        
    }

    private void Search()
    {
        try
        {
            List<MembershipUser> listUser = new List<MembershipUser>();
            foreach (MembershipUser m in Membership.GetAllUsers())
            {
                listUser.Add(m);
            }
            listUser = listUser.Where(l => (l.UserName.Contains(txtUserName.Text))).ToList();
            List<MembershipUser> onlineUser = new List<MembershipUser>();
            foreach (MembershipUser u in listUser)
            {
                if (u.IsOnline) onlineUser.Add(u);
            }

            if (cbxOnlineUser.Checked)
            {
                if (onlineUser.Count == 0)
                {
                    lblNoRecord.Visible = true;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("IsLockedOut");
                    DataRow r = dt.NewRow();
                    r["UserName"] = string.Empty;
                    r["Email"] = string.Empty;
                    r["IsApproved"] = true;
                    dt.Rows.Add(r);
                    grvUser.DataSource = dt;
                    grvUser.DataBind();
                    grvUser.Rows[0].Visible = false;
                }
                else
                {
                    lblNoRecord.Visible = false;
                    grvUser.DataSource = onlineUser;
                    grvUser.DataBind();
                }

            }
            else
            {
                if (listUser.Count == 0)
                {
                    lblNoRecord.Visible = true;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("UserName");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("IsApproved");
                    DataRow r = dt.NewRow();
                    r["UserName"] = string.Empty;
                    r["Email"] = string.Empty;
                    r["IsApproved"] = true;
                    dt.Rows.Add(r);

                    grvUser.DataSource = dt;
                    grvUser.DataBind();
                    grvUser.Rows[0].Visible = false;
                }
                else
                {
                    lblNoRecord.Visible = false;
                    grvUser.DataSource = listUser;
                    grvUser.DataBind();
                }
            }


            LinkButton lBtn = new LinkButton();
            LinkButton lBtnResetPass = new LinkButton();
            foreach (GridViewRow r in grvUser.Rows)
            {
                lBtn = (LinkButton)r.Cells[3].FindControl("lBtnLockUnLock");
                lBtnResetPass = (LinkButton)r.Cells[4].FindControl("lBtnResetPassword");
                if (r.Cells[2].Text == "False")
                {
                    r.Cells[2].Text = string.Empty;
                    lBtn.Text = GetResource("Approve.Text");
                }
                else
                {
                    r.Cells[2].Text = "√";
                    lBtn.Text = GetResource("Disapprove.Text");
                }
                if (r.Cells[0].Text == this.User.Identity.Name) lBtn.Visible = false;

                lBtnResetPass.Text = GetResource("ResetPassword.Text");

                lBtn.Attributes["onclick"] = "javascript:return confirm('" + string.Format(GetResource("msDisapproveAccount.Text"), lBtn.Text, Common.GetRowString(r.Cells[0].Text)) + "');";
                lBtnResetPass.Attributes["onclick"] = "javascript:return confirm('" + string.Format(GetResource("msResetPassword.Text"), Common.GetRowString(r.Cells[0].Text)) + "');";
            }
        }
        catch (Exception ex)
        {

            logger.Error("Error Search", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void grvUser_RowCreated(object sender, GridViewRowEventArgs e)
    {
        InitLanguage();
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView grid = (GridView)sender;
            GridViewRow gridrow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tblcell = new TableHeaderCell();

           

            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("lblPopUpUserName.Text"); ;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);

            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("Email.Text"); ;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(350);
            gridrow.Cells.Add(tblcell);

            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.Text = GetResource("Aprroved.Text");
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(80);
            gridrow.Cells.Add(tblcell);

            tblcell = new TableHeaderCell();
            tblcell.ColumnSpan = 1;
            tblcell.CssClass = "td_header";
            tblcell.Width = Unit.Pixel(100);
            gridrow.Cells.Add(tblcell);
            
            //Add header
            grid.Controls[0].Controls.AddAt(0, gridrow);
        }
    }
    protected void cbxOnlineUser_CheckedChanged(object sender, EventArgs e)
    {
        Search();
    }
    protected void lBtnLockUnLock_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow r = (GridViewRow)((LinkButton)sender).NamingContainer;
            MembershipUser u = Membership.GetUser(r.Cells[0].Text);
            if (r.Cells[2].Text != string.Empty)
            {
                u.IsApproved = false;
                Membership.UpdateUser(u);
            }
            else
            {
                u.IsApproved = true;
                Membership.UpdateUser(u);
            }
            Search();
        }
        catch(Exception ex)
        {

            logger.Error("Error lBtnLockUnLock_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }
    protected void lBtnResetPassword_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow r = (GridViewRow)((LinkButton)sender).NamingContainer;
            MembershipUser u = Membership.GetUser(r.Cells[0].Text);
            string newPassword = string.Empty;
            newPassword = u.ResetPassword();
            if (newPassword != string.Empty)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("msResetPasswordSuccess.Text"), string.Format(GetResource("msResetPasswordDetail.Text"), Common.GetRowString(r.Cells[0].Text),newPassword)) + "\");", true);
                SendMail(Common.GetRowString(r.Cells[1].Text), newPassword);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("msResetPasswordFail.Text")) + "\");", true);
            }
        }
        catch (Exception ex)
        {
            logger.Error("Error lBtnResetPassword_Click", ex);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
        }
    }

    private void SendMail(string mailAddress,string newPassword)
    {
        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = Common.AppSettingKey(Constant.MAIL_SERVER);
        smtpClient.Port = Convert.ToInt32(Common.AppSettingKey(Constant.MAIL_PORT));
        smtpClient.Credentials = new System.Net.NetworkCredential(Common.AppSettingKey(Constant.MAIL_USER), Common.AppSettingKey(Constant.MAIL_PWD));
        smtpClient.EnableSsl = Common.AppSettingKey(Constant.MAIL_ENABLE_SSL).ToLower() == "false"?false:true;
        
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(Common.AppSettingKey(Constant.MAIL_USER));
        mail.To.Add(new MailAddress(mailAddress));
        mail.Subject = Common.AppSettingKey(Constant.MAIL_SUBJECT);
        mail.Body = Common.AppSettingKey(Constant.MAIL_BODY) +newPassword;
        smtpClient.Send(mail);
       

    }

}