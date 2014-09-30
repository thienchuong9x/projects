using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Dental.Utilities;
using System.IO;
using Dental.Domain;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        HttpCookie cookie = Request.Cookies["CurrentLanguage"];
        if (!IsPostBack && cookie != null && cookie.Value != null)
        {
            if (cookie.Value.IndexOf("en-") >= 0)
            {
                ibtUS.Enabled = false;
                ibtJanpan.Enabled = true;
            }
            else
            {
                ibtUS.Enabled = true;
                ibtJanpan.Enabled = false;
            }
            
           
        }
    
        HttpCookie cookie1 = Request.Cookies["CurrentLanguage"];
        if (cookie1 != null && cookie1.Value != null)
        {
            Page.UICulture = cookie1.Value;
        }
  
        Initialize();
        InitializeMenu();
        InitializeLinkTitle();

        #region check login
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            lblUserName.Visible = true;
            lblUserName.Text = HttpContext.Current.User.Identity.Name;
            lBtnLogout.Visible = true;
            lBtnLogin.Visible = false;

            panelMenuSale.Visible = true;
            panelMenuAccounting.Visible = true;
            panelMenuStock.Visible = true;
            panelMenuMaster.Visible = true;
            panelMenuUsers.Visible = true;

            if (cboOfficeName.Items.Count == 0)
            {
                panelMenuSale.Visible = false;
                panelMenuAccounting.Visible = false;
                panelMenuStock.Visible = false;
                panelMenuMaster.Visible = false;
                panelMenuUsers.Visible = false;
            }

        }
        else
        {
            cboOfficeName.Items.Clear();

            lblUserName.Visible = false;
            lblUserName.Text = string.Empty;
            lBtnLogout.Visible = false;
            lBtnLogin.Visible = true;

            panelMenuSale.Visible = false;
            panelMenuAccounting.Visible = false;
            panelMenuStock.Visible = false;
            panelMenuMaster.Visible = false;
            panelMenuUsers.Visible = false;
        }
        #endregion

        #region User Permission

        int staffCd = MasterStaff.GetStaffCd(HttpContext.Current.User.Identity.Name);
        string permissionNm = DentalPermission.GetPermissionForUser(staffCd, cboOfficeName.SelectedValue); 
        if(string.IsNullOrEmpty(permissionNm)) 
        {
            panelMenuSale.Visible = panelMenuUsers.Visible = false;
            panelMenuMaster.Visible = false;
            panelMenuAccounting.Visible = panelMenuManageStock.Visible = false;

        }
        else if(permissionNm == "OfficeChief") 
        {
            panelMenuUsers.Visible = true;
            panelMenuOffices.Visible = false;
            panelMenuItems.Visible = false;
            panelMenuTax.Visible = false;
            panelMenuSystem.Visible = false;
        }
        else if(permissionNm == "Member")
        {
            panelMenuUsers.Visible = false;
            panelMenuOffices.Visible = false;
            panelMenuItems.Visible = false;
            panelMenuTax.Visible = false;
            panelMenuSystem.Visible = false;
        }

        if (HttpContext.Current.User.Identity.Name == "admin")
        {
            panelMenuSale.Visible = true;
            panelMenuAccounting.Visible = true;
            panelMenuStock.Visible = true;
            panelMenuManageStock.Visible = true;
            panelMenuMaster.Visible = true;
            panelMenuUsers.Visible = true;
        }

        #endregion

        //set Header image
        ImageHeader.ImageUrl = "~/Styles/Flexiweb-DarkBlue/images/top-logo.png";
        
    }

    private void InitializeLinkTitle()
    {
        string title = Path.GetFileNameWithoutExtension(Request.Path);
        if (title == "OrderList") hplLinkTitle.Text = lblMenuOrders.Text;
        if (title == "OrderInput") hplLinkTitle.Text = lblMenuOrderRegistration.Text;
        if (title == "DeliverStatement") hplLinkTitle.Text = lblMenuDeliveryStatements.Text;
        if (title == "BillStatement") hplLinkTitle.Text = lblMenuInvoices.Text;
        if (title == "Deposits") hplLinkTitle.Text = lblMenuDeposits.Text;
        if (title == "Payments") hplLinkTitle.Text = lblMenuPayments.Text;
        if (title == "ManageStock") hplLinkTitle.Text = lblMenuManageStock.Text;
        if (title == "MstDentalOffice") hplLinkTitle.Text = lblMenuDentalClinics.Text;
        if (title == "MstProsthesis") hplLinkTitle.Text = lblMenuProsthetics.Text;
        if (title == "MstMaterial") hplLinkTitle.Text = lblMenuMaterials.Text;
        if (title == "MstSupplier") hplLinkTitle.Text = lblMenuSuppliers.Text;
        if (title == "MstProcess") hplLinkTitle.Text = lblMenuProcesses.Text;
        if (title == "MstBill") hplLinkTitle.Text = lblMenuBillingRecipients.Text;
        if (title == "MstBank") hplLinkTitle.Text = lblMenuBanks.Text;
        if (title == "MstOutSourceLab") hplLinkTitle.Text = lblMenuOutsources.Text;
        if (title == "MstTech") hplLinkTitle.Text = lblMenuTechPrice.Text;
        if (title == "MstOffice") hplLinkTitle.Text = lblMenuOffices.Text;
        if (title == "MstSystem") hplLinkTitle.Text = lblMenuSystem.Text;
        if (title == "MstItem") hplLinkTitle.Text = lblMenuItems.Text;
        if (title == "MstTax") hplLinkTitle.Text = lblMenuTax.Text;
        if (title == "Register") hplLinkTitle.Text = lblMenuRegisterUsers.Text;
        if (title == "Staffs") hplLinkTitle.Text = lblMenuStaffs.Text;
        if (title == "ManageUser") hplLinkTitle.Text = lblMenuManageUsers.Text;
        if (title == "ChangePassword") hplLinkTitle.Text = Common.GetResourceString("ChangePassWord.Text");

        hplLinkTitle.NavigateUrl = Request.Url.AbsoluteUri;
       
    }



    private void InitializeMenu()
    {
        #region MenuSale
        lblMenuSale.Text = Common.GetResourceString("Menu_Sale.Text");
        lblMenuOrders.Text = Common.GetResourceString("Menu_Orders.Text");
        lblMenuOrderRegistration.Text = Common.GetResourceString("Menu_OrderRegistration.Text");
        lblMenuDeliveryStatements.Text = Common.GetResourceString("Menu_DeliveryStatements.Text");
        #endregion

        #region MenuAccounting
        lblMenuAccounting.Text = Common.GetResourceString("Menu_Accounting.Text");
        lblMenuInvoices.Text = Common.GetResourceString("Menu_Invoices.Text");
        lblMenuDeposits.Text = Common.GetResourceString("Menu_Deposits.Text");
        lblMenuPayments.Text = Common.GetResourceString("Menu_Payments.Text");
        #endregion

        #region MemuStock
        lblMenuStock.Text = Common.GetResourceString("Menu_Stock.Text");
        lblMenuManageStock.Text = Common.GetResourceString("Menu_ManageStock.Text");
        #endregion

        #region MenuMaster
        lblMenuMaster.Text = Common.GetResourceString("Menu_Master.Text");
        lblMenuDentalClinics.Text = Common.GetResourceString("Menu_DentalClinics.Text");
        lblMenuProsthetics.Text = Common.GetResourceString("Menu_Prosthetics.Text");
        lblMenuMaterials.Text = Common.GetResourceString("Menu_Materials.Text");
        lblMenuSuppliers.Text = Common.GetResourceString("Menu_Suppliers.Text");
        lblMenuProcesses.Text = Common.GetResourceString("Menu_Processes.Text");
        lblMenuBillingRecipients.Text = Common.GetResourceString("Menu_BillingRecipients.Text");
        lblMenuBanks.Text = Common.GetResourceString("Menu_Banks.Text");
        lblMenuOutsources.Text = Common.GetResourceString("Menu_Outsources.Text");
        lblMenuTechPrice.Text = Common.GetResourceString("Menu_TechPrice.Text");
        lblMenuOffices.Text = Common.GetResourceString("Menu_Offices.Text");
        lblMenuSystem.Text = Common.GetResourceString("Menu_System.Text");
        lblMenuItems.Text = Common.GetResourceString("Menu_Items.Text");
        lblMenuTax.Text = Common.GetResourceString("Menu_Tax.Text");
        #endregion

        #region MenuUser
        lblMenuUser.Text = Common.GetResourceString("Menu_User.Text");
        lblMenuManageUsers.Text = Common.GetResourceString("Menu_ManageUsers.Text");
        lblMenuRegisterUsers.Text = Common.GetResourceString("Menu_RegisterUsers.Text");
        lblMenuStaffs.Text = Common.GetResourceString("Menu_Staffs.Text");
        #endregion
    }

    private void Initialize()
    {
        //lblTiltePopUpLogin.Text = Common.GetResourceString("lblTiltePopUpLogin.Text");
        //lblPopUpUserName.Text = Common.GetResourceString("lblPopUpUserName.Text");
        //lblPopUpPassWord.Text = Common.GetResourceString("lblPopUpPassWord.Text");
        //cbRememberMe.Text = Common.GetResourceString("cbRememberMe.Text");
        //btnPopUpLogin.Text = Common.GetResourceString("btnLogin.Text");
        lBtnLogin.Text = Common.GetResourceString("btnLogin.Text");
        lBtnLogout.Text = Common.GetResourceString("lBtnLogout.Text");
    }

    protected void ibtUS_Click(object sender, ImageClickEventArgs e)
    {
        HttpCookie cookie = new HttpCookie("CurrentLanguage");
        cookie.Value = "en-US";
        Calendar.flag = "en";
        Response.SetCookie(cookie);
        Response.Redirect(Request.RawUrl);
    }
    protected void ibtJanpan_Click(object sender, ImageClickEventArgs e)
    {
        HttpCookie cookie = new HttpCookie("CurrentLanguage");
        cookie.Value = "ja-JP";
        Calendar.flag = "ja";
        Response.SetCookie(cookie);
        Response.Redirect(Request.RawUrl);
    }
    //protected void btnLogin_Click(object sender, EventArgs e)
    //{
    //    if (Membership.ValidateUser(txtPopUpUserName.Text, txtPopUpPassWord.Text))
    //    {
    //        FormsAuthentication.RedirectFromLoginPage(txtPopUpUserName.Text, cbRememberMe.Checked ? true : false);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "alert(\"" + Common.GetResourceString("LoginFail.Text") + "\");", true);
    //    }
    //}
    protected void lBtnLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();

        //delete cookie OfficeCd
        if (Request.Cookies["CurrentOffice"] != null)
        {
            HttpCookie cookie = Request.Cookies["CurrentOffice"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }
        Response.Redirect("~/Default.aspx");
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        FillDropDownOffice();

    }
    private void FillDropDownOffice()
    {
        /*
        cboOfficeName.Items.Clear();
        //Addnew to Roles, Lay ID
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DentalConnectionString"].ToString());
        string s = "SELECT ur.RoleID, r.RoleName FROM UserRoles as ur,Roles as r WHERE ur.UserID = (select StaffCd from [DENTAL_MstStaff] where UserId = @UserID) and ur.RoleID = r.RoleID and ur.RoleID IN (select r.RoleID from Roles as r,RoleGroups as g where r.RoleGroupid=g.RoleGroupid and RoleGroupName='Office_Group')";
        SqlCommand command = new SqlCommand(s, conn); //"DDV_GetOffices1"
        //command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = HttpContext.Current.User.Identity.Name;
        conn.Open();
        SqlDataReader dt = command.ExecuteReader();
        if (dt.HasRows)
        {
            while (dt.Read())
            {
                cboOfficeName.Items.Add(new ListItem(dt.GetString(1), dt.GetInt32(0).ToString()));
            }
        }
        conn.Close();

        if (cboOfficeName.Items.Count == 0)
        {
            panelMenuSale.Visible = false;
            panelMenuAccounting.Visible = false;
            panelMenuStock.Visible = false;
            panelMenuMaster.Visible = false;
            panelMenuUsers.Visible = false;
        }

        */

        //make new table Permission 

        cboOfficeName.Items.Clear();
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            int staffCd = MasterStaff.GetStaffCd(HttpContext.Current.User.Identity.Name);
            var listPermission =  DentalPermission.GetAllPermissionByUserName(staffCd);

            foreach (MasterOffice i in listPermission)
            {
                cboOfficeName.Items.Add(new ListItem(i.OfficeNm, i.OfficeCd.ToString()));
            }

            if (cboOfficeName.Items.Count == 0)
            {
                panelMenuSale.Visible = false;
                panelMenuAccounting.Visible = false;
                panelMenuStock.Visible = false;
                panelMenuMaster.Visible = false;
                panelMenuUsers.Visible = false;
            }

            
            HttpCookie cookie = Request.Cookies["CurrentOffice"];
            if (cookie != null)
            {
                cboOfficeName.SelectedValue = cookie.Value;
            }


        }


    }
    protected void cboOfficeName_SelectedIndexChanged(object sender, EventArgs e)
    {
         HttpCookie cookie = Request.Cookies["CurrentOffice"]; 
         if(cookie == null) 
           cookie = new HttpCookie("CurrentOffice");

         cookie.Value = cboOfficeName.SelectedValue;
         Response.SetCookie(cookie);
         Response.Redirect(Request.RawUrl );
    }
    protected void lBtnLogin_Click(object sender, EventArgs e)
    {
        panelPopUp1.Visible = true;
    }
}
