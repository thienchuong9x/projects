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
    public partial class ChangePassword : DDVPortalModuleBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ChangePassword).Name);
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
            catch(Exception ex)
            {
                logger.Error("Error Page_Load", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }

        private void Initialize()
        {
            btnSave.Text = GetResource("btnSave.Text");
            lblOldPassWord.Text = GetResource("lblOldPassWord.Text");
            lblNewPassWord.Text = GetResource("lblNewPassWord.Text");
            lblConfirmPassWord.Text = GetResource("lblConfirmPassWord.Text");
            ReqValidatorNewPassWord.ErrorMessage = ReqValidatorPassWord.ErrorMessage = GetResource("ReqValidatorPassWord.Text");
            ReqValidatorPassWordConfirm.ErrorMessage = GetResource("ReqValidatorPassWordConfirm.Text");
            RegValidatorPassNewWordLength.ErrorMessage = RegValidatorPassWordLength.ErrorMessage = GetResource("RegValidatorPassWordLenght.Text");
            CompValidatorPassWord.ErrorMessage = GetResource("CompValidatorPassWord.Text");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipUser u = Membership.GetUser(User.Identity.Name);
                if(u.ChangePassword(txtOldPassWord.Text,txtNewPassWord.Text))
                {
                    txtOldPassWord.Text = txtNewPassWord.Text = txtConfirmPassWord.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetResource("ChangePassSuccess.Text") + "\");", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), GetResource("ChangePassFail.Text")) + "\");", true);
            }
            catch (Exception ex)
            {
                logger.Error("Error Save", ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + GetJSMessage(GetResource("Title_Error.Text"), ex.Message) + "\");", true);
            }
        }
}
}
