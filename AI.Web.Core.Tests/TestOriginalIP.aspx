<%@ Page Language="C#" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title></title>    
</head>
<body>
    <form id="form1" runat="server">   
        <p>
            Host Address: <% Response.Write(Request.UserHostAddress); %>
        </p>
        <br />
        <p>
            Header: <% Response.Write(Request.Headers["X-Forwarded-For"]); %>
        </p>        
    </form>
</body>
</html>
