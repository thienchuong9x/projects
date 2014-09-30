using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Globalization;

/// <summary>
/// Summary description for Calendar
/// </summary>
/// 
public class Calendar : System.Web.UI.Page
{
    public static string flag;
    public static string stringFormat;

    /// <summary>
    /// Opens a popup Calendar
    /// </summary>
    /// <param name="Field">TextBox to return the date value</param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    public static string InvokePopupCal(TextBox Field)
    {
        // Define character array to trim from language strings
        char[] TrimChars = new char[] { ',', ' ' };

        // Get culture array of month names and convert to string for
        // passing to the popup calendar
        string MonthNameString = "";
        // Get culture array of day names and convert to string for
        // passing to the popup calendar
        string DayNameString = "";
        string strToday = "";
        string strClose = "";
        string strCalendar = "";
        //string FormatString = DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString(); 
        if (Calendar.flag == "en")
        {
            // Get month
            MonthNameString = "January,February,March,April,May,June,July,August,September,October,November,December";
            // Get day
            DayNameString = "Sun,Mon,Tue,Wed,Thu,Fri,Sat";
            Calendar.stringFormat = "MM/dd/yyyy";
            strToday = "Today";
            strClose = "Close";
            strCalendar = "Calendar";
        }
        else if (Calendar.flag == "ja")
        {
            // Get month
            MonthNameString = "一月,二月,三月,四月,五月,六月,七月,八月,九月,十月,十一月,十二月";
            // Get Day
            DayNameString = "日,月,火,水,木,金,土";
            Calendar.stringFormat = "yyyy/MM/dd";
            strToday = "今日";
            strClose = "閉じる";
            strCalendar = "カレンダー";
        }
        else
        {
            // Get month
            foreach (string Month in DateTimeFormatInfo.CurrentInfo.MonthNames)
            {
                MonthNameString += Month + ",";
            }
            MonthNameString = MonthNameString.TrimEnd(TrimChars);

            // Get Day
            foreach (string Day in DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames)
            {
                DayNameString += Day + ",";
            }
            DayNameString = DayNameString.TrimEnd(TrimChars);

            // Format calendar
            Calendar.stringFormat = DateTimeFormatInfo.CurrentInfo.ShortDatePattern.ToString();
            strToday = "Today";
            strClose = "Close";
            strCalendar = "Calendar";
        }

        //int a =(int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

        if (!ClientAPI.IsClientScriptBlockRegistered(Field.Page, "PopupCalendar.js"))
        {
            ClientAPI.RegisterClientScriptBlock(Field.Page, "PopupCalendar.js", "<script src=\"" + ClientAPI.ScriptPath + "PopupCalendar.js\"></script>");
        }       
        return "javascript:popupCal('Cal','" + Field.ClientID + "','" + stringFormat + "','" + MonthNameString + "','" + DayNameString + "','" + strToday + "','" + strClose + "','" + strCalendar + "'," + (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + ");";
    }
}

public class ClientAPI
{
    #region Private Shared Members

    private static string m_sScriptPath;
    private static string m_ClientAPIDisabled = string.Empty;

    #endregion

    #region Public Shared Properties
    //  ScriptPath 
    public static string ScriptPath
    {
        get
        {
            string script = "";
            if (m_sScriptPath != null)
            {
                script = m_sScriptPath;
            }
            else if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                {
                    script = HttpContext.Current.Request.ApplicationPath + "js/";
                }
                else
                {
                    script = HttpContext.Current.Request.ApplicationPath + "/js/";
                }
            }
            return script;
        }
        set
        {
            m_sScriptPath = value;
        }
    }
    #endregion

    #region Public Shared Methods
    // ClientScriptBlockRegistered
    public static bool IsClientScriptBlockRegistered(Page objPage, string key)
    {
        return objPage.ClientScript.IsClientScriptBlockRegistered(key);
    }
    // ClientScriptBlock
    public static void RegisterClientScriptBlock(Page objPage, string key, string strScript)
    {
        objPage.ClientScript.RegisterClientScriptBlock(objPage.GetType(), key, strScript);
    }

    #endregion
}