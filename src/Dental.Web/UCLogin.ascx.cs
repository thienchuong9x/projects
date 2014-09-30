using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Dental.Utilities;

public partial class UCLogin : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPopUpUserName.Focus();
        
        lblTiltePopUpLogin.Text = Common.GetResourceString("lblTiltePopUpLogin.Text");
        lblPopUpUserName.Text = Common.GetResourceString("lblPopUpUserName.Text");
        lblPopUpPassWord.Text = Common.GetResourceString("lblPopUpPassWord.Text");
        cbRememberMe.Text = Common.GetResourceString("cbRememberMe.Text");
        btnPopUpLogin.Text = Common.GetResourceString("btnLogin.Text");
        
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Membership.ValidateUser(txtPopUpUserName.Text, txtPopUpPassWord.Text))
        {
            FormsAuthentication.RedirectFromLoginPage(txtPopUpUserName.Text, cbRememberMe.Checked ? true : false);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + Common.GetResourceString("LoginFail.Text") + "\");", true);
        }
        
    }
}