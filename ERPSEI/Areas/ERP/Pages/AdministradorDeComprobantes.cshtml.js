var table;
var buttonExport;
var buttonCancel;
var tableProdServ;
var selections = [];
var dlgProdServ = null;
var dlgCFDI = null;
var dlgCFDIModal = null;
const ESTATUS_SOLICITADA = 1;
const ESTATUS_AUTORIZADA = 2;
const ESTATUS_FINALIZADA = 3;

var numFormatter = null;
var dialogMode = null;
const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener('DOMContentLoaded', function () {
    numFormatter = new Intl.NumberFormat(cultureName);

    table = $("#table");
    buttonExport = $("#btnExportar");
    buttonCancel = $("#btnCancelar");

    initTable();

    autoCompletar("#inpEmisor");
    autoCompletar("#inpReceptor");

    jQuery.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("is-invalid").removeClass("is-valid");
        },
        unhighlight: function (element, errorClass, validClass) {
            if ($(element).hasClass("is-invalid")) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            }
        }
    });

});

////////////////////////////////
//Funcionalidad Tabla
////////////////////////////////
//Función para obtener los identificadores de los registros seleccionados
function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}

//Función para procesar la respuesta del servidor al consultar datos
function responseHandler(res) {
    if (typeof res == "string" && res.length >= 1) {
        res = JSON.parse(res);
    }
    $.each(res, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1
    });

    return res
}

//Función para dar formato a los iconos de operación de los registros
function operateFormatter(value, row, index) {
    let icons = [];
    
    //Icono Ver
    if (puedeTodo || puedeConsultar || puedeEditar || puedeEliminar) { icons.push(`<li><a class="dropdown-item see" href="#" title="${btnVerTitle}"><i class="bi bi-search"></i> ${btnVerTitle}</a></li>`); }
    //Icono PDF
    if (puedeTodo || puedeConsultar || puedeEditar || puedeEliminar) { icons.push(`<li><a class="dropdown-item pdf" href="#" title="${btnPDFTitle}"><i class="bi bi-file-pdf"></i> ${btnPDFTitle}</a></li>`); }
    //Icono Autorizar
    if ((puedeTodo || puedeAutorizar) && row.estatusId == ESTATUS_SOLICITADA && row.requiereAutorizacion == "True") {
        //Si el usuario tiene permisos para autorizar y además la prefactura está en estatus solicitada y además requiere autorización... 
        //Busca al usuario por Id en la lista de autorizaciones del elemento.
        let foundUserAuth = row.autorizaciones.find(a => a.userId == window.userId);

        if (foundUserAuth == undefined) {
            //Si no se encontró la autorización del usuario en la lista, entonces todavía no ha autorizado el elemento, por lo tanto, busca si el usuario puede autorizar prefacturas.
            foundUserAuth = window.authUsers.find(u => u == window.userId);

            //Si se encontró al usuario en la lista, entonces agrega icono para autorizar.
            if (foundUserAuth != undefined) { icons.push(`<li><a class="dropdown-item auth" href="#" title="${btnAutorizarTitle}"><i class="bi bi-patch-check"></i> ${btnAutorizarTitle}</a></li>`); }
        }
    }
    //Icono Timbrar
    if ((puedeTodo || puedeEditar) && (row.estatusId == ESTATUS_AUTORIZADA || row.estatusId == ESTATUS_SOLICITADA)) {
        icons.push(`<li><a class="dropdown-item stamp" href="#" title="${btnTimbrarTitle}"><i class="bi bi-postage"></i> ${btnTimbrarTitle}</a></li>`);
    }

    if (icons.length >= 1) {

        return `<div class="dropdown">
                  <button class="btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="bi bi-three-dots-vertical success"></i>
                  </button>
                  <ul class="dropdown-menu">${icons.join("")}</ul>
                </div>`;
    }
    else {
        return '';
    }
}

//Eventos de los iconos de operación
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initCFDIDialog(VER, row);
        dlgCFDIModal.toggle();
    },
    'click .pdf': function (e, value, row, index) {
        onShowPDF(row.safeL);
    },
    'click .cancel': function (e, value, row, index) {
        onCancelarPrefactura(row.id);
    }
}

//Función para cancelar cfdis
function onCancelarCFDIClick(ids = null) {
    let oParams = {};

    if (ids != null) { oParams.ids = ids; }
    else { oParams.ids = [document.getElementById("inpCFDIId").value]; }

    doAjax(
        "/ERP/AdministradorDeComprobantes/Cancelar",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                showError(dlgExportTitle, resp.mensaje);
                return;
            }

            if (ids != null) {
                ids = [];
                selections = null;
                if (buttonExport) { buttonExport.prop('disabled', true); }
                table.bootstrapTable('uncheckAll');
            }

            let fileLink = document.getElementById("downloadFileLink");
            fileLink.click();

            showSuccess(dlgExportTitle, resp.mensaje);
        }, function (error) {
            showError(dlgExportTitle, error);
        },
        postOptions
    );
}

//Función para exportar cfdis
function onExportarCFDIClick(ids = null) {
    let oParams = {};

    if (ids != null) { oParams.ids = ids; }
    else { oParams.ids = [document.getElementById("inpCFDIId").value]; }  

    doAjax(
        "/ERP/Prefacturas/ExportExcel",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                showError(dlgExportTitle, resp.mensaje);
                return;
            }

            if (ids != null) {
                ids = [];
                selections = null;
                if (buttonExport) { buttonExport.prop('disabled', true); }
                table.bootstrapTable('uncheckAll');
            }

            let fileLink = document.getElementById("downloadFileLink");
            fileLink.click();

            showSuccess(dlgExportTitle, resp.mensaje);
        }, function (error) {
            showError(dlgExportTitle, error);
        },
        postOptions
    );
}

//Función para inicializar la tabla
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        columns: [
            {
                field: "state",
                checkbox: true,
                align: "center",
                valign: "middle"
            },
            {
                title: "Id",
                field: "id",
                align: "center",
                valign: "middle",
                sortable: true,
                width: "80px"
            },
            {
                title: colSerieHeader,
                field: "serie",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFolioHeader,
                field: "folio",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFechaHeader,
                field: "fecha",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colMonedaHeader,
                field: "moneda",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFormaPagoHeader,
                field: "formaPago",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colMetodoPagoHeader,
                field: "metodoPago",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colUsoCFDIHeader,
                field: "usoCFDI",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colEstatusHeader,
                field: "estatus",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colAccionesHeader,
                field: "operate",
                align: "center",
                width: "100px",
                clickToSelect: false,
                events: window.operateEvents,
                formatter: operateFormatter
            }
        ]
    })
    table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function () {
        if (buttonExport) { buttonExport.prop('disabled', !table.bootstrapTable('getSelections').length) }
        if (buttonCancel) { buttonCancel.prop('disabled', !table.bootstrapTable('getSelections').length) }

        // save your data, here just save the current page
        selections = getIdSelections()
        // push or splice the selections if you want to save all data selections
    });
    if (buttonExport) { buttonExport.click(function () { onExportarCFDIClick(selections); }); }
    if (buttonCancel) { buttonCancel.click(function () { onTimbrarCFDIClick(selections); }); }
}

//Función para cancelar una prefactura
function onCancelarComprobante(idComprobante) {
    let oParams = { idComprobante: idComprobante }

    doAjax(
        "/ERP/AdministradorDeComprobantes/Cancelar",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                let summary = ``;
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                }
                showError(dlgTitle.innerHTML, `<ul>${summary}</ul>`);
                return;
            }

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}

//Función para mostrar una prefactura como PDF
function onShowPDF(safeL) {
    if (safeL.length >= 1) { window.open(`/FileViewer?safeL=${encodeURIComponent(safeL)}`, "_blank"); }
}
////////////////////////////////

////////////////////////////////
//Funcionalidad Filtrar
////////////////////////////////
//Función para detectar el cambio de valor en el campo Tipo
function onTipoChanged() {
    if ($("#selFiltroTipo").val() == "0") {
        $("#inpFiltroEmisor").parent().parent().hide();
        $("#inpFiltroReceptor").parent().parent().show();
    }
    else {
        $("#inpFiltroReceptor").parent().parent().hide();
        $("#inpFiltroEmisor").parent().parent().show();
    }
}
//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let oParams = {
        Periodo: $("#selFiltroPeriodo").val(),
        EstatusId: $("#selFiltroEstatus").val() == 0 ? null : parseInt($("#selFiltroEstatus").val()),
        TipoId: $("#selFiltroTipo").val() == 0 ? null : parseInt($("#selFiltroTipo").val()),
        FormaPagoId: $("#selFiltroFormaPago").val() == 0 ? null : parseInt($("#selFiltroFormaPago").val()),
        MetodoPagoId: $("#selFiltroMetodoPago").val() == 0 ? null : parseInt($("#selFiltroMetodoPago").val()),
        UsoCFDIId: $("#selFiltroUsoCFDI").val() == 0 ? null : parseInt($("#selFiltroUsoCFDI").val()),
        EmisorId: ($("#inpFiltroEmisor").attr("idselected") || "0") == "0" ? null : parseInt($("#inpFiltroEmisor").attr("idselected")),
        ReceptorId: ($("#inpFiltroReceptor").attr("idselected") || "0") == "0" ? null : parseInt($("#inpFiltroReceptor").attr("idselected"))
    };

    //Resetea el valor de los filtros.
    document.querySelectorAll("#filtros .form-control").forEach(function (e) { e.value = ""; if (e.hasAttribute("idselected")) { e.setAttribute("idselected", ""); } });
    document.querySelectorAll("#filtros .form-select").forEach(function (e) { e.value = 0; });

    doAjax(
        "/ERP/AdministradorDeComprobantes/Filtrar",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                let summary = ``;
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                }
                showError($("#btnBuscar").text(), resp.mensaje + " " + summary);
                return;
            }

            table.bootstrapTable('load', responseHandler(resp.datos));
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}
////////////////////////////////

////////////////////////////////
//Funcionalidad Diálogo CFDI
////////////////////////////////

//Función para dar formato de moneda a los campos numéricos.
function currencyFormatter(value, row, index) {
    return `$ ${numFormatter.format(value)}`;
}

//Función para dar formato de número a los campos numéricos.
function numericFormatter(value, row, index) {
    return numFormatter.format(value);
}
////////////////////////////////