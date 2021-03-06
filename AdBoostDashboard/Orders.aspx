﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="AdBoostDashboard.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row wrapper border-bottom white-bg page-heading">
                <div class="col-lg-10">
                    <h2>Órdenes</h2>
                   <%-- <ol class="breadcrumb">
                        <li>
                            <a href="index.html">Home</a>
                        </li>
                        <li>
                            <a>Tables</a>
                        </li>
                        <li class="active">
                            <strong>Data Tables</strong>
                        </li>
                    </ol>--%>
                    <ol class="breadcrumb">
                    <li>  
           <%--         <form class="m-t" role="form" action="#" runat="server">                      
                        <div class="form-group">
                        <asp:TextBox ID="txtCompany" class="form-control" placeholder="Company" required="" runat="server"></asp:TextBox>
                        </div>                  
                        <div class="form-group">       
                        <asp:LinkButton ID="Lkbtn" class="btn btn-primary block full-width m-b" runat="server" onclick="LbGetOrder">Obtener Orden</asp:LinkButton>
                        </div>
                        <div class="form-group">       
                        <asp:LinkButton ID="LinkButton1" class="btn btn-primary block full-width m-b" runat="server" onclick="GetLineItems">Obtener Orden-LineItem</asp:LinkButton>                                      
                        </div>
                    </form>--%>
                    </li></ol>                    
                    <h1><asp:Literal ID="LitOrders" runat="server"></asp:Literal></h1>
                    <h2><br><asp:Literal ID="LitLineItem" runat="server"></asp:Literal></h2>

                </div>
                <div class="col-lg-2">

                </div>
            </div>
    <div class="wrapper wrapper-content animated fadeInRight">
        
            <div class="row">
                <div class="col-lg-12">
                <div class="ibox float-e-margins">
                    <div class="ibox-title">
                        <h5>Órdenes</h5>
                        <div class="ibox-tools">
                            <a class="collapse-link">
                                <i class="fa fa-chevron-up"></i>
                            </a>
                            <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                                <i class="fa fa-wrench"></i>
                            </a>
                            <%--<ul class="dropdown-menu dropdown-user">
                                <li><a href="#">Config option 1</a>
                                </li>
                                <li><a href="#">Config option 2</a>
                                </li>
                            </ul>--%>
                            <%--<a class="close-link">
                                <i class="fa fa-times"></i>
                            </a>--%>
                        </div>
                    </div>
                    <div class="ibox-content">

                    <div class="table-responsive">
                    <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                        <div class="html5buttons">
                            <div class="dt-buttons btn-group">
                                <%--<a class="btn btn-default buttons-copy buttons-html5" tabindex="0" aria-controls="DataTables_Table_0"><span>Copy</span></a>
                                <a class="btn btn-default buttons-csv buttons-html5" tabindex="0" aria-controls="DataTables_Table_0"><span>CSV</span></a>
                                <a class="btn btn-default buttons-excel buttons-html5" tabindex="0" aria-controls="DataTables_Table_0"><span>Excel</span></a>
                                <a class="btn btn-default buttons-pdf buttons-html5" tabindex="0" aria-controls="DataTables_Table_0"><span>PDF</span></a>
                                <a class="btn btn-default buttons-print" tabindex="0" aria-controls="DataTables_Table_0"><span>Print</span></a>--%>
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
                    
                    </div>
                        </div>

                    </div>
                </div>
            </div>
            </div>
            
            
          <%--  <div class="row">
            <div class="col-lg-12">
            <div class="ibox float-e-margins">
            <div class="ibox-title">
                <h5>Editable Table in- combination with jEditable</h5>
                <div class="ibox-tools">
                    <a class="collapse-link">
                        <i class="fa fa-chevron-up"></i>
                    </a>
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="fa fa-wrench"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-user">
                        <li><a href="#">Config option 1</a>
                        </li>
                        <li><a href="#">Config option 2</a>
                        </li>
                    </ul>
                    <a class="close-link">
                        <i class="fa fa-times"></i>
                    </a>
                </div>
            </div>
            <div class="ibox-content">
            <div class="">
            <a onclick="fnClickAddRow();" href="javascript:void(0);" class="btn btn-primary ">Add a new row</a>
            </div>
            <div id="editable_wrapper" class="dataTables_wrapper form-inline dt-bootstrap"><div class="row"><div class="col-sm-6"><div class="dataTables_length" id="editable_length"><label>Show <select name="editable_length" aria-controls="editable" class="form-control input-sm"><option value="10">10</option><option value="25">25</option><option value="50">50</option><option value="100">100</option></select> entries</label></div></div><div class="col-sm-6"><div id="editable_filter" class="dataTables_filter"><label>Search:<input type="search" class="form-control input-sm" placeholder="" aria-controls="editable"></label></div></div></div><div class="row"><div class="col-sm-12"><table class="table table-striped table-bordered table-hover  dataTable" id="editable" role="grid" aria-describedby="editable_info">
            <thead>
            <tr role="row"><th class="sorting_asc" tabindex="0" aria-controls="editable" rowspan="1" colspan="1" aria-sort="ascending" aria-label="Rendering engine: activate to sort column descending" style="width: 175px;">Rendering engine</th><th class="sorting" tabindex="0" aria-controls="editable" rowspan="1" colspan="1" aria-label="Browser: activate to sort column ascending" style="width: 218px;">Browser</th><th class="sorting" tabindex="0" aria-controls="editable" rowspan="1" colspan="1" aria-label="Platform(s): activate to sort column ascending" style="width: 196px;">Platform(s)</th><th class="sorting" tabindex="0" aria-controls="editable" rowspan="1" colspan="1" aria-label="Engine version: activate to sort column ascending" style="width: 149px;">Engine version</th><th class="sorting" tabindex="0" aria-controls="editable" rowspan="1" colspan="1" aria-label="CSS grade: activate to sort column ascending" style="width: 105px;">CSS grade</th></tr>
            </thead>
            <tbody>
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            <tr class="gradeA odd" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Firefox 1.0</td>
                <td>Win 98+ / OSX.2+</td>
                <td class="center">1.7</td>
                <td class="center">A</td>
            </tr><tr class="gradeA even" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Firefox 1.5</td>
                <td>Win 98+ / OSX.2+</td>
                <td class="center">1.8</td>
                <td class="center">A</td>
            </tr><tr class="gradeA odd" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Firefox 2.0</td>
                <td>Win 98+ / OSX.2+</td>
                <td class="center">1.8</td>
                <td class="center">A</td>
            </tr><tr class="gradeA even" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Firefox 3.0</td>
                <td>Win 2k+ / OSX.3+</td>
                <td class="center">1.9</td>
                <td class="center">A</td>
            </tr><tr class="gradeA odd" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Camino 1.0</td>
                <td>OSX.2+</td>
                <td class="center">1.8</td>
                <td class="center">A</td>
            </tr><tr class="gradeA even" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Camino 1.5</td>
                <td>OSX.3+</td>
                <td class="center">1.8</td>
                <td class="center">A</td>
            </tr><tr class="gradeA odd" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Netscape 7.2</td>
                <td>Win 95+ / Mac OS 8.6-9.2</td>
                <td class="center">1.7</td>
                <td class="center">A</td>
            </tr><tr class="gradeA even" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Netscape Browser 8</td>
                <td>Win 98SE+</td>
                <td class="center">1.7</td>
                <td class="center">A</td>
            </tr><tr class="gradeA odd" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Netscape Navigator 9</td>
                <td>Win 98+ / OSX.2+</td>
                <td class="center">1.8</td>
                <td class="center">A</td>
            </tr><tr class="gradeA even" role="row">
                <td class="sorting_1">Gecko</td>
                <td>Mozilla 1.0</td>
                <td>Win 95+ / OSX.1+</td>
                <td class="center">1</td>
                <td class="center">A</td>
            </tr></tbody>
            <tfoot>
            <tr><th rowspan="1" colspan="1">Rendering engine</th><th rowspan="1" colspan="1">Browser</th><th rowspan="1" colspan="1">Platform(s)</th><th rowspan="1" colspan="1">Engine version</th><th rowspan="1" colspan="1">CSS grade</th></tr>
            </tfoot>
            </table></div></div><div class="row"><div class="col-sm-5"><div class="dataTables_info" id="editable_info" role="status" aria-live="polite">Showing 1 to 10 of 57 entries</div></div><div class="col-sm-7"><div class="dataTables_paginate paging_simple_numbers" id="editable_paginate"><ul class="pagination"><li class="paginate_button previous disabled" id="editable_previous"><a href="#" aria-controls="editable" data-dt-idx="0" tabindex="0">Previous</a></li><li class="paginate_button active"><a href="#" aria-controls="editable" data-dt-idx="1" tabindex="0">1</a></li><li class="paginate_button "><a href="#" aria-controls="editable" data-dt-idx="2" tabindex="0">2</a></li><li class="paginate_button "><a href="#" aria-controls="editable" data-dt-idx="3" tabindex="0">3</a></li><li class="paginate_button "><a href="#" aria-controls="editable" data-dt-idx="4" tabindex="0">4</a></li><li class="paginate_button "><a href="#" aria-controls="editable" data-dt-idx="5" tabindex="0">5</a></li><li class="paginate_button "><a href="#" aria-controls="editable" data-dt-idx="6" tabindex="0">6</a></li><li class="paginate_button next" id="editable_next"><a href="#" aria-controls="editable" data-dt-idx="7" tabindex="0">Next</a></li></ul></div></div></div></div>

            </div>
            </div>
            </div>
            </div>--%>
        </div>



    <!-- Mainly scripts -->
    
    <script src="js/plugins/jeditable/jquery.jeditable.js"></script>
    <script src="js/plugins/dataTables/datatables.min.js"></script>

    <!-- Custom and plugin javascript -->
    

    <!-- Page-Level Scripts -->
    <script>
        $(document).ready(function(){
            $('.dataTables-example').DataTable({
                dom: '<"html5buttons"B>lTfgitp',
                buttons: [
                    { extend: 'copy'},
                    {extend: 'csv'},
                    {extend: 'excel', title: 'ExampleFile'},
                    {extend: 'pdf', title: 'ExampleFile'},

                    {extend: 'print',
                     customize: function (win){
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                    .addClass('compact')
                                    .css('font-size', 'inherit');
                    }
                    }
                ]

            });

            /* Init DataTables */
            var oTable = $('#editable').DataTable();

            /* Apply the jEditable handlers to the table */
            oTable.$('td').editable( '../example_ajax.php', {
                "callback": function( sValue, y ) {
                    var aPos = oTable.fnGetPosition( this );
                    oTable.fnUpdate( sValue, aPos[0], aPos[1] );
                },
                "submitdata": function ( value, settings ) {
                    return {
                        "row_id": this.parentNode.getAttribute('id'),
                        "column": oTable.fnGetPosition( this )[2]
                    };
                },

                "width": "90%",
                "height": "100%"
            } );


        });

        function fnClickAddRow() {
            $('#editable').dataTable().fnAddData( [
                "Custom row",
                "New row",
                "New row",
                "New row",
                "New row" ] );

        }
    </script>

   

</asp:Content>
