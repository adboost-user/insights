<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="OrderItem.aspx.cs" Inherits="AdBoostDashboard.OrderItem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="row">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5><asp:Literal ID="LitTitle" runat="server"></asp:Literal></h5>
                    <div class="ibox-tools">
                        <a href="Reports.aspx" class="btn btn-primary">
                            <span>
                                <i class="fa fa-download" aria-hidden="true" style="color: white; font-size: 20px; vertical-align: middle;"></i>
                            </span>Descargar Reportes</a>
                    </div>
                </div>
            </div>
        </div>   
    </div>
    <div runat="server" id="HideDivFiltro" style="display: none;"></div>
    <div class="row" id="TopInfo" runat="server">
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <span class="label label-orange pull-right">Campaña
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                    </span>
                    <h5>Impresiones</h5>
                </div>
                <div class="ibox-content">
                    <%--<h1 class="no-margins">40 886,200</h1>--%>
                    <h1 class="no-margins">
                        <asp:Literal ID="LitImpresiones" runat="server"></asp:Literal></h1>
                    <%--<div class="stat-percent font-bold text-success">98% <i class="fa fa-bolt"></i></div>--%>
                    <small>Impresiones</small>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <span class="label label-blue pull-right">Campaña
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                    </span>
                    <h5>Clics</h5>
                </div>
                <div class="ibox-content">
                    <%--<h1 class="no-margins">275,800</h1>--%>
                    <h1 class="no-margins">
                        <asp:Literal ID="LitClics" runat="server"></asp:Literal></h1>
                    <%--<div class="stat-percent font-bold text-info">20% <i class="fa fa-level-up"></i></div>--%>
                    <small>Clics</small>
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <span class="label label-green pull-right">Campaña
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                    </span>
                    <h5>CTR</h5>
                </div>
                <div class="ibox-content">
                    <%--<h1 class="no-margins">106,120</h1>--%>
                    <h1 class="no-margins">
                        <asp:Literal ID="LitCTR" runat="server"></asp:Literal></h1>
                    <%--<div class="stat-percent font-bold text-navy">44% <i class="fa fa-level-up"></i></div>--%>
                    <small>CTR</small>
                </div>
            </div>
        </div>

    </div>
    <div class="row" id="GraficoInfo" runat="server">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <h5 style="margin: 5px;""><asp:Literal ID="LitTitulo" runat="server"></asp:Literal></h5>
                    <div class="pull-left">
                        <div class="btn-group">
                            <%-- <button type="button" class="btn btn-xs btn-white active">Today</button>
                                        <button type="button" class="btn btn-xs btn-white">Monthly</button>
                                        <button type="button" class="btn btn-xs btn-white">Annual</button>--%>
                            
                        </div>
                    </div>
                    <div class="ibox-tools">
                              <form runat="server">
                            <label>
                                Mostrar                                 
                            </label>
                            <asp:DropDownList ID="GraficoLista" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="itemSelected">
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
                            <a class="collapse-link">
                                <i class="fa fa-chevron-up"></i>
                            </a>
                            <a class="close-link">
                                    <i class="fa fa-times"></i>
                                </a>
                        </form>


                    </div>
                </div>
                <div class="ibox-content">
                    <div class="row">
                     <table id="LabelTit"></table></div>
                    <div class="row">
                        <div class="col-lg-9">
                            <h4><asp:Literal ID="LitGrafInfo" runat="server"></asp:Literal></h4>
                            <div class="flot-chart">
                                <div class="flot-chart-content" id="flot-dashboard-chart"></div>                                
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <ul class="stat-list">
                                        <li>
                                            <h2 class="no-margins"><asp:Literal ID="LitImpGrafico" runat="server"></asp:Literal></h2>
                                            <small>Total Impresiones</small>
                                            <div class="stat-percent"> <%--<i class="fa fa-level-up text-navy"></i>--%></div>
                                             <div class="progress progress-mini" style="background-color: #f5a04a;">
                                        <div style="width: 100%;" class="progress-bar-danger"></div>
                                    </div>
                                        </li>
                                        <li>
                                            <h2 class="no-margins "><asp:Literal ID="LitClicsGrafico" runat="server"></asp:Literal></h2>
                                            <small>Clics</small>
                                            <div class="stat-percent"><%--<i class="fa fa-level-down text-navy"></i>--%></div>
                                            <div class="progress progress-mini" style="background-color: #469fd9;">
                                                <div style="width: 100%;" class="progress-bar"></div>
                                            </div>
                                        </li>
                                        <li>
                                            <h2 class="no-margins "><asp:Literal ID="LitCTRgrafico" runat="server"></asp:Literal></h2>
                                            <small>CTR</small>
                                            <div class="stat-percent"><%--<i class="fa fa-bolt text-navy"></i>--%></div>
                                            <div class="progress progress-mini" style="background-color: #07c6c9;">
                                                <div style="width: 100%;" class="progress-bar-danger"></div>
                                            </div>
                                        </li>
                                        </ul>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="row" id="OrderDetalleInfo" runat="server">
        <div class="col-lg-12">
            <div class="ibox float-e-margins">
                <div class="ibox-title">
                    <%--<h5>Órdenes</h5>--%>
                    <h5>Campañas</h5>
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
                            <div class="html5buttons">
                                <div class="dt-buttons btn-group">
                                </div>
                            </div>
                            <div class="dataTables_length" id="DataTables_Table_0_length">
                                <%--<label>Mostrar 
                                <select name="DataTables_Table_0_length" aria-controls="DataTables_Table_0" class="form-control input-sm">
                                <option value="10">10</option><option value="25">25</option>
                                <option value="50">50</option><option value="100">100</option>
                                </select>
                            </label>--%>
                            </div>
                            <div id="DataTables_Table_0_filter" class="dataTables_filter">
                                <%--<label>Search:<input type="search" class="form-control input-sm" placeholder="" aria-controls="DataTables_Table_0"></label>--%>
                            </div>
                            <%--<div class="dataTables_info" id="DataTables_Table_0_info" role="status" aria-live="polite">Showing 1 to 10 of 57 entries</div>--%>
                            <table class="table table-striped table-bordered table-hover dataTables-example dataTable" id="jsonTable" aria-describedby="DataTables_Table_0_info" role="grid">
                            </table>
                            <table class="table table-striped table-bordered table-hover dataTables-example dataTable" id="jsonTableOrders" aria-describedby="DataTables_Table_0_info" role="grid">
                            </table>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

                    <asp:Label ID="lblData2" runat="server" Text="Label" style="display: none;"></asp:Label>                                             
                    <asp:Label ID="lblData3" runat="server" Text="Label" style="display: none;"></asp:Label>
      <!-- Mainly scripts -->
    <script src="js/jquery-2.1.1.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/plugins/metisMenu/jquery.metisMenu.js"></script>
    <script src="js/plugins/slimscroll/jquery.slimscroll.min.js"></script>

        <!-- Flot -->
    <script src="js/plugins/flot/jquery.flot.js"></script>
    <script src="js/plugins/flot/jquery.flot.tooltip.min.js"></script>
    <script src="js/plugins/flot/jquery.flot.spline.js"></script>
    <script src="js/plugins/flot/jquery.flot.resize.js"></script>
    <script src="js/plugins/flot/jquery.flot.pie.js"></script>
    <script src="js/plugins/flot/jquery.flot.symbol.js"></script>
    <script src="js/plugins/flot/jquery.flot.time.js"></script>
        <script>
            $(document).ready(function () {

                /*var data2 = [
                [1459209600000, 100], [1459296000000, 120], [1459382400000, 50], [1459468800000, 220],
                [1459555200000, 25], [1459641600000, 15], [1459728000000, 120], [1459814400000, 60]
                ];

                var data3 = [
                    [1459209600000, 900], [1459296000000, 1200], [1459382400000, 500], [1459468800000, 2200],
                [1459555200000, 250], [1459641600000, 150], [1459728000000, 1201], [1459814400000, 602]
                ];*/

                var data1 = $("#<%=lblData2.ClientID %>").html();
                var data2 = $.parseJSON(data1);
                var data4 = $("#<%=lblData3.ClientID %>").html();
                var data3 = $.parseJSON(data4);
                //var data4 = '[[1325376000000, 800], [1325484000000, 500], [1325570400000, 600], [1325656800000, 700], [1325743200000, 500], [1325829600000, 456], [1325916000000, 800], [1326002400000, 589], [1326088800000, 467], [1326175200000, 876], [1326261600000, 689], [1326348000000, 700], [1326434400000, 500], [1326520800000, 600], [1326607200000, 700], [1326693600000, 786], [1326780000000, 345], [1326866400000, 888], [1326952800000, 888], [1327039200000, 888], [1327125600000, 987], [1327212000000, 444], [1327298400000, 999], [1327384800000, 567], [1327471200000, 786], [1327557600000, 666], [1327644000000, 888], [1327730400000, 900], [1327816800000, 178], [1327903200000, 555], [1327989600000, 993]]';             



                /* console.log(typeof data2);
                 console.log(data2);
                 console.log(typeof data3);
                 console.log(data3);*/

                var dataset = [
                    {
                        label: "Número de Impresiones",
                        data: data3,
                        color: "#df7420",
                        bars: {
                            show: true,
                            align: "center",
                            barWidth: 24 * 60 * 60 * 600,
                            lineWidth: 0,
                            fill: true,
                            fillColor: { colors: [{ opacity: 0.8 }, { opacity: 0.1 }] }
                        }

                    }, {
                        label: "Clics",
                        data: data2,
                        yaxis: 2,
                        color: "#1C84C6",
                        lines: {
                            lineWidth: 1,
                            show: true,
                            fill: true,
                            fillColor: {
                                colors: [{
                                    opacity: 0.4
                                }, {
                                    brightness: 0.6,
                                    opacity: 0.2
                                }]
                            }
                        },
                        splines: {
                            show: false,
                            tension: 0.6,
                            lineWidth: 1,
                            fill: 0.1
                        },
                    }
                ];

                var posicion = document.getElementById("LabelTit");
                var options = {
                    xaxis: {
                        mode: "time",
                        tickSize: [7, "day"],
                        tickLength: 0,
                        axisLabel: "Date",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: 'Arial',
                        axisLabelPadding: 10,
                        color: "#d5d5d5",
                        monthNames: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Nov", "Oct", "Dic"]
                    },
                    yaxes: [{
                        position: "left",
                        color: "#d5d5d5",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: 'Arial',
                        axisLabelPadding: 3
                    }, {
                        position: "right",
                        clolor: "#d5d5d5",
                        axisLabelUseCanvas: true,
                        axisLabelFontSizePixels: 12,
                        axisLabelFontFamily: ' Arial',
                        axisLabelPadding: 67
                    }
                    ],
                    legend: {
                        noColumns: 1,
                        labelBoxBorderColor: "#000000",
                        position: "nw",
                        container: posicion//posicionado en una tabla fuera del contenedor del grafico-- para ver la leyendo fuera de las barras
                    },
                    grid: {
                        hoverable: true,
                        borderWidth: 0
                    },
                    tooltip: true,
                    tooltipOpts: {
                        content: "x: %x, y: %y"
                    }
                };

                function gd(year, month, day) {
                    return new Date(year, month - 1, day).getTime();
                }

                var previousPoint = null, previousLabel = null;

                $.plot($("#flot-dashboard-chart"), dataset, options);
            });
    </script>


</asp:Content>
