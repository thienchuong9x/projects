using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;

namespace Dental.Account
{
    public partial class Login : DDVPortalModuleBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Login).Name);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                InitLanguage();
                Initialize();
                HyperLink hpl = new HyperLink();
                LinkButton lbtn = new LinkButton();
                hpl = (HyperLink)Master.FindControl("hplLinkTitle");
                hpl.Text = btnLogin.Text;
                txtUserName.Focus();
            }
            catch (Exception ex)
            {
                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }

        private void Initialize()
        {
            lblUserName.Text = GetResource("lblPopUpUserName.Text");
            lblPassWord.Text = GetResource("lblPopUpPassWord.Text");
            cbRememberMe.Text = GetResource("cbRememberMe.Text");
            btnLogin.Text = GetResource("btnLogin.Text");
            
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (Membership.ValidateUser(txtUserName.Text, txtPassWord.Text))
                {
                    FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, cbRememberMe.Checked ? true : false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("LoginFail.Text")) + "\");", true);
                }
            }
            catch(Exception ex)
            {
                logger.Error("Error Login", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
}
}
