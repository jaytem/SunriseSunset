<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="SunriseSunset.WebDemo.Demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div style="margin-bottom: 10px;">
                Add
                <asp:TextBox ID="txtPlusHours" runat="server" Text="0" Style="width: 20px" />
            </div>
            <div style="margin-bottom: 10px">
                <asp:Button ID="btnGetnSunriseSunsetData" runat="server" Text="Get Sunrise/Sunset data" OnClick="btnGetnSunriseSunsetData_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
            </div>

            <div>
                <asp:Literal ID="litoutput" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
