
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dental.Utilities;

/// <summary>
/// Summary description for DDVPortalModuleBase
/// </summary>
public class DDVPortalModuleBase : System.Web.UI.Page
{   
    protected string GetResource(string resourceName)
    {
        return Common.GetResourceString(resourceName);
    }
    protected string GetOffice()
    {
        DropDownList cboOfficeCd = new DropDownList();
        cboOfficeCd = (DropDownList)Master.FindControl("cboOfficeName");
        if (string.IsNullOrWhiteSpace(cboOfficeCd.SelectedValue))
            return "-1";
        return cboOfficeCd.SelectedValue; 
    }

    protected void SetLabelText(params Label[] ctrl)
    {
        foreach (Label i in ctrl)
        {
            i.Text = GetResource(i.ID + ".Text");
        }
    }
    protected void SetButtonText(params Button[] ctrl)
    {
        foreach (Button i in ctrl)
        {
            i.Text = GetResource(i.ID + ".Text");
        }
    }

    protected void RegisterStartupScript(string script)
    {

        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < 10; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        var key = builder.ToString() + DateTime.Now.Millisecond.ToString();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, script, true);
    }

    protected string GetJSMessage(string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title)) title = string.Empty;
        if (string.IsNullOrWhiteSpace(message)) message = string.Empty;
        return title + "\\n" + ChangeBreakLine((message.Replace('\r', ' ')).Replace("\n", "\\n"));
    }

    protected string ChangeBreakLine(string msg)
    {
        return msg.Replace('\r', ' ').Replace("\n", "\\n").Replace("\\\"", "\"").Replace("\"", "\\\"");
    }

    protected string showOnlyDate(string dt)
    {
        string s = string.Empty;
        s = dt.Split(' ')[0];

        if (s.Contains("0001")) return string.Empty;

        return s;
    }
    protected void InitLanguage()
    {
        HttpCookie cookie = Request.Cookies["CurrentLanguage"];
        if (cookie != null && cookie.Value != null)
        {
            Page.UICulture = cookie.Value;
        }
    }

    protected string SetDateFormat(string datetime)
    {

        if (Page.UICulture == "Japanese (Japan)")
        {
            return Convert.ToDateTime(datetime).ToString("yyyy/MM/dd");
        }
        else
        {
            return Convert.ToDateTime(datetime).ToShortDateString();
        }
    }

    protected void GetName(string value, TextBox textName, DropDownList dropDownSource)
    {

        ListItem item = dropDownSource.Items.FindByValue(value);
        if (item != null)
        {
            textName.Text = item.Text;
            dropDownSource.SelectedValue = value;
        }

    }

    protected void GetAutomaticDropDownList(TextBox textCode, DropDownList dropDownSource)
    {

        ListItem item = dropDownSource.Items.FindByValue(textCode.Text);
        if (item != null)
        {
            dropDownSource.SelectedValue = textCode.Text;
        }
        else
        {
            dropDownSource.SelectedIndex = 0;
            textCode.Text = dropDownSource.SelectedValue;
            textCode.Focus();
        }

    }




}