<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="CrearCompañia.aspx.cs" Inherits="AdBoostDashboard.CrearCompañia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="row wrapper border-bottom white-bg page-heading">
        <div class="col-sm-4">
            <h2>Agregar Compañia</h2>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Compañias</a>
                </li>
                <li class="active">
                    <strong>Dashboard</strong>
                </li>
            </ol>
        </div>
        <div class="col-sm-8">
            <div class="title-action">
                <a href="Default.aspx" class="btn btn-primary">Volver</a>
            </div>
        </div>
    </div>
    <div class="wrapper wrapper-content">
        <div class="wrapper wrapper-content animated fadeInRight">
            <div class="row">

                <div class="col-lg-8">
                    <div class="ibox float-e-margins">
                        <div class="ibox-title">
                            <h5>Agregue Compañia</h5>
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
                                    <label class="col-lg-2 control-label">Compañia Id</label>
                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtIdCompany" class="form-control" placeholder="Id Compañia DFP-Google" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Nombre</label>

                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtNombre" class="form-control" placeholder="Nombre" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Contacto</label>

                                    <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtContacto" class="form-control" placeholder="Contacto" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Email</label>

                                     <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtEmail" class="form-control" placeholder="Email" required="" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="form-group">
                                    <label class="col-lg-2 control-label">Teléfono</label>

                                     <div class="col-lg-10">                                        
                                        <asp:TextBox ID="TxtTelefono" class="form-control" placeholder="N° Teléfono" required="" runat="server"></asp:TextBox>
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
        </div>
    </div>
</asp:Content>
