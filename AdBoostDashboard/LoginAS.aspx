<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginAS.aspx.cs" Inherits="AdBoostDashboard.LoginAS" %>

<!DOCTYPE html>

<html>

<head>

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>AdBoostDashboard | Login</title>

    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet">

    <link href="css/animate.css" rel="stylesheet">
    <link href="css/style.css" rel="stylesheet">
</head>

<body class="gray-bg">

    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <div>

                <h1 class="logo-name">+</h1>

            </div>
            <h3>Bienvenido</h3>
            <p>Ingrese su Usuario</p>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="LbSubmit">
                <form class="m-t" role="form" action="#" runat="server">
                    <div class="form-group">
                        <asp:TextBox ID="TxUsername" class="form-control" placeholder="Username" required="" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TxPassword" class="form-control" placeholder="Password" required="" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                    <div class="form-group">                       
                        <asp:DropDownList ID="CompanyId" name="Company" class="form-control m-b" runat="server">
                            <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="form-group">
                    <asp:LinkButton ID="LbSubmit" class="btn btn-primary block full-width m-b" runat="server" OnClick="LbSubmit_Click">Ingresar</asp:LinkButton>
                    </div>

                    <small>
                        <asp:Label ID="LbError" runat="server" Text="Label"></asp:Label></small>
                </form>
            </asp:Panel>
            <p class="m-t"><small>Adboost &copy; 2016</small> </p>
        </div>
    </div>

    <!-- Mainly scripts -->
    <script src="js/jquery-2.1.1.js"></script>
    <script src="js/bootstrap.min.js"></script>

</body>

</html>

