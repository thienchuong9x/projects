using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dental.Domain;

public partial class CalendarDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hplDate.NavigateUrl = Calendar.InvokePopupCal(txtDate);
    }
}