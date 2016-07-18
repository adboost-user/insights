<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="CrearUSuario.aspx.cs" Inherits="AdBoostDashboard.CrearUSuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row wrapper border-bottom white-bg page-heading">
        <div class="col-sm-4">
            <h2>Agregar Usuario</h2>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Usuarios</a>
                </li>
                <li class="active">
                    <strong>Dashboard</strong>
                </li>
            </ol>
        </div>
        <div class="col-sm-8">
            <div class="title-action">
                <a href="#" class="btn btn-primary">Volver</a>
            </div>
        </div>
    </div>
    <div class="wrapper wrapper-content">
        <div class="wrapper wrapper-content animated fadeInRight">
            <div class="row">

                <div class="col-lg-5">
                    <div class="ibox float-e-margins">
                        <div class="ibox-title">
                            <h5>Agregue Usuario</h5>
                            <div class="ibox-tools">
                                <a class="collapse-link">
                                    <i class="fa fa-chevron-up"></i>
                                </a>
                                <a class="close-link">
                                    <i class="fa fa-times"></i>
                                </a>
                            </div>
                        </div>
                        <div class="ibox-content">
                            <form class="form-horizontal" runat="server">
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Usuario</label>
                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtUser" class="form-control" placeholder="Usuario" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Password</label>

                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtPassword" class="form-control" placeholder="Password" required="" TextMode="Password" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Nombre</label>

                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtNombre" class="form-control" placeholder="Nombre" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Privilegio</label>

                                    <div class="col-lg-10">
                                        <select name="privilegio" class="form-control m-b">
                                            <option value="9000">Administrador</option>
                                            <option value="1000">Operador</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Compañia</label>
                                    <div class="col-lg-10">
                                        <select id="CompanyId" name="Company" class="form-control m-b">
                                            <asp:Literal ID="LitOptions" runat="server"></asp:Literal>
                                            <%--<option selected>Seleccione una opción</option>
                                            <option value="1">Adboost</option>
                                            <option value="59769708">Loto</option>--%>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-lg-offset-2 col-lg-10">                                        
                                        <asp:LinkButton ID="LbInsertUsuario" runat="server" class="btn btn-sm btn-primary pull-right m-t-n-xs" OnClick="LbSubmit_Click"><strong>Crear</strong></asp:LinkButton>                                        
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

        <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5>Usuarios</h5>
                    <div class="ibox-tools">
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                       
                        <a class="close-link">
                                <i class="fa fa-times"></i>
                            </a>
                    </div>
                </div>
                <div class="ibox-content">

                    <div class="table-responsive">
                        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <table class="table table-striped table-bordered table-hover dataTables-example dataTable" id="jsonTable" aria-describedby="DataTables_Table_0_info" role="grid">
                                <asp:Literal ID="LitTblH" runat="server"></asp:Literal>
                                <asp:Literal ID="LitTblB" runat="server"></asp:Literal>
                            </table>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

                
        </div>
    </div>



</asp:Content>
