<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="AdBoostDashboard.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row wrapper border-bottom white-bg page-heading">
        <div class="col-sm-4">
            <h2>Generar Reporte</h2>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Reportes</a>
                </li>
                <li class="active">
                    <strong>Descargar</strong>
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
        <div class="wrapper wrapper-content animated fadeInLeft">
            <div class="row">
                <div class="col-lg-10">
                    <div class="ibox float-e-margins">
                        <div class="ibox-title">
                            <h5>Reportes</h5>
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
                                <div class="form-group" runat="server" id="divCompanyId">
                                    <label class="col-lg-2 control-label">Compañia</label>
                                    <div class="col-lg-5">
                                        <select id="CompanyId" name="Company" class="form-control m-b">
                                            <asp:Literal ID="LitOptions" runat="server"></asp:Literal>
                                            <%--<option selected>Seleccione una opción</option>
                                            <option value="1">Adboost</option>
                                            <option value="59769708">Loto</option>--%>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Opción</label>
                                    <div class="col-lg-6">
                                        <asp:DropDownList ID="OpcionReporte" runat="server" class="form-control m-b">
                                            <asp:ListItem Text="Seleccione una opción" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Duración Total" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Ayer" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Hoy" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Últimos 7 días" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Últimos 14 días" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Últimos 30 días" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="El mes pasado" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Este mes" Value="8"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Anuncio|Campaña</label>
                                    <div class="col-lg-6">
                                        <div class="tabs-container">
                                            <ul class="nav nav-tabs">
                                                <li class="active"><a href="#tab-1" data-toggle="tab" aria-expanded="true">Anuncio</a></li>
                                                <li class=""><a href="#tab-2" data-toggle="tab" aria-expanded="false"></a></li>
                                            </ul>
                                            <div class="tab-content">
                                                <div class="tab-pane active" id="tab-1">
                                                    <div class="panel-body">
                                                        <div class="col-lg-10">
                                                            <select id="selectAnuncio" name="Linea" class="form-control m-b">
                                                                <asp:Literal ID="LitLineaAnuncios" runat="server"></asp:Literal>
                                                            </select>
                                                        </div>
                                                    </div>

                                                </div>
                                                <%--    <div class="tab-pane" id="tab-2" >
                                                    <div class="panel-body">
                                                        <div class="col-lg-10">
                                                            <select id="selectCamp" name="Order" class="form-control m-b">
                                                                <asp:Literal ID="LitCamp" runat="server"></asp:Literal>                                         
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">Tipo de reportes</label>
                                    <div class="col-lg-6">
                                        <div class="radio radio-info radio-inline">
                                            <%--<input type="radio" id="inlineRadio1" value="option1" name="radioInline" checked="">--%>
                                            <asp:RadioButton ID="rbExcel" name="rbExcel" runat="server" GroupName="opcionDescarga" />
                                            <label for="inlineRadio1">Excel</label>
                                        </div>
                                        <div class="radio radio-inline">
                                            <%--<input type="radio" id="inlineRadio2" value="option2" name="radioInline">--%>
                                            <asp:RadioButton ID="rbPdf" name="rbPdf" runat="server" GroupName="opcionDescarga" />
                                            <label for="inlineRadio2">PDF</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-lg-offset-2 col-lg-10">
                                        <asp:LinkButton ID="LbInsertUsuario" runat="server" class="btn btn-sm btn-primary pull-left m-t-n-xs" OnClick="LbSubmit_Click"><strong>Generar</strong></asp:LinkButton>
                                    </div>
                                </div>
                                <small>
                                    <asp:Label ID="LbError" runat="server" Text="Label"></asp:Label></small>
                            </form>
                        </div>
                    </div>
                </div>
            </div>




        </div>
    </div>



</asp:Content>
