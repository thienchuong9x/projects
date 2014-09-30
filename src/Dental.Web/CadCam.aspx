<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CadCam.aspx.cs" Inherits="CadCam" %>
<%@ Register src="~/UCCadCamFiles.ascx" tagname="CadCam" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="Styles/PopupLogin/style/style.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="toPopup" style="display:block">
    <div id="popup_content">
        <uc1:CadCam ID="cc1" HomeFolder="~/Portal/XMLOrder" runat="server" />
        </div>
    </div>
    </form>

</body>
</html>
