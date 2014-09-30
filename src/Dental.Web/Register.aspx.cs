using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace Dental.Account
{
    public partial class Register : DDVPortalModuleBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Register).Name);
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
            lblUserName.Text = GetResource("lblPopUpUserName.Text");
            lblPassWord.Text = GetResource("lblPopUpPassWord.Text");
            lblConfirmPassWord.Text = GetResource("lblConfirmPassWord.Text");
            btnRegister.Text = GetResource("btnRegister.Text");
            lblEmail.Text = GetResource("Email.Text");
            ReqValidatorUserName.ErrorMessage = GetResource("ReqValidatorUserName.Text");
            ReqValidatorPassWord.ErrorMessage = GetResource("ReqValidatorPassWord.Text");
            CompValidatorPassWord.Text = GetResource("CompValidatorPassWord.Text");
            RegValidatorEmail.ErrorMessage = GetResource("RegValidatorEmail.Text");
            RegValidatorPassWordLength.ErrorMessage = GetResource("RegValidatorPassWordLenght.Text");
            ReqValidatorEmail.ErrorMessage = GetResource("ReqValidatorEmail.Text");
            RegValidatorUserLenght.ErrorMessage = GetResource("RegValidatorUserLenght.Text");
            ReqValidatorPassWordConfirm.ErrorMessage = GetResource("ReqValidatorPassWordConfirm.Text");
        }


        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipCreateStatus status;
                Membership.CreateUser(txtUserName.Text, txtPassWord.Text, txtEmail.Text, null, null, true, out status);
                if (status.ToString() == "Success")
                {
                    txtUserName.Text = txtEmail.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + (GetResource("RegisterUserSuccess.Text") + "\");"), true);
                }
                if (Membership.GetUser(txtUserName.Text).UserName != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("RegisterUserFail.Text")) + "\");", true);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Register", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
    }
}
