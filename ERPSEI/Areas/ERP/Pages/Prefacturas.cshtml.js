var table;
var buttonExport;
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
    tableProdServ = $("#tableProductosServicios");
    dlgCFDI = document.getElementById('dlgCFDI');
    dlgCFDIModal = new bootstrap.Modal(dlgCFDI, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlgCFDI.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });

    dlgProdServ = document.getElementById('dlgProdServ');
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlgProdServ.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });

    initTable();

    let btnBuscar = document.getElementById("btnBuscar");
    if (btnBuscar) { btnBuscar.click(); }

    autoCompletar("#inpEmisor", {
        select: function (element, item) { toggleEmisorInfo(item); },
        change: function (element, item) {
            let inpEmisor = document.getElementById('inpEmisor');

            inpEmisor.classList.remove("is-invalid");
            inpEmisor.classList.remove("is-valid");

            if ((inpEmisor.value || "").length <= 0) {
                toggleEmisorInfo();
                inpEmisor.classList.add("is-invalid");
            }
            else {
                inpEmisor.classList.add("is-valid");
            }
        }
    });
    autoCompletar("#inpReceptor", {
        select: function (element, item) { toggleReceptorInfo(item); },
        change: function (element, item) {
            let inpReceptor = document.getElementById('inpReceptor');

            inpReceptor.classList.remove("is-invalid");
            inpReceptor.classList.remove("is-valid");

            if ((inpReceptor.value || "").length <= 0) {
                toggleReceptorInfo();
                inpReceptor.classList.add("is-invalid");
            }
            else {
                inpReceptor.classList.add("is-valid");
            }
        }
    });
    autoCompletar("#inpProductoServicio", {
        select: function (element, item) {
            let inpDescripcion = document.getElementById("inpDescripcion");
            inpDescripcion.value = item.value;
        }
    });
    autoCompletar("#inpUnidad", {
        change: function (element, item) {
            let inpUnidad = document.getElementById('inpUnidad');

            inpUnidad.classList.remove("is-invalid");
            inpUnidad.classList.remove("is-valid");

            if ((inpUnidad.value || "").length <= 0) {
                inpUnidad.classList.add("is-invalid");
            }
            else {
                inpUnidad.classList.add("is-valid");
            }
        }
    });

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
    if ((puedeTodo || puedeAutorizar) && row.estatusId == ESTATUS_SOLICITADA) {
        //Si el usuario tiene permisos para autorizar y la prefactura está en estatus solicitada, busca al usuario por Id en la lista de autorizaciones del elemento.
        let foundUserAuth = row.autorizaciones.find(a => a.userId == window.userId);

        if (foundUserAuth == undefined) {
            //Si no se encontró la autorización del usuario en la lista, entonces todavía no ha autorizado el elemento, por lo tanto, busca si el usuario puede autorizar prefacturas.
            foundUserAuth = window.authUsers.find(u => u == window.userId);

            //Si se encontró al usuario en la lista, entonces agrega icono para autorizar.
            if (foundUserAuth != undefined) { icons.push(`<li><a class=7"dropdown-item pdf" href="#" title="${btnAutorizarTitle}"><i class="bi bi-patch-check"></i> ${btnAutorizarTitle}</a></li>`); }
        }
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
        showPDF(row.id);
    }
}
//Función para agregar cfdis
function onAgregarClick() {
    let oCFDINuevo = createNewCFDI();
    initCFDIDialog(NUEVO, oCFDINuevo);
    dlgCFDIModal.toggle();
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

        // save your data, here just save the current page
        selections = getIdSelections()
        // push or splice the selections if you want to save all data selections
    });
    if (buttonExport) { buttonExport.click(function () { onExportarCFDIClick(selections); }); }
}
//Función para capturar el click de los botones para dar de baja cfdis. Ejecuta una llamada ajax para dar de baja cfdis.
function onDeleteCFDIClick(ids = null) {
    askConfirmation(dlgDeleteTitle, dlgDeleteQuestion, function () {
        let oParams = {};

        if (ids != null) { oParams.ids = ids; }
        else { oParams.ids = [document.getElementById("inpCFDIId").value]; }

        doAjax(
            "/ERP/Prefacturas/Disable",
            oParams,
            function (resp) {
                if (resp.tieneError) {
                    showError(dlgDeleteTitle, resp.mensaje);
                    return;
                }

                table.bootstrapTable('remove', {
                    field: 'id',
                    values: oParams.ids
                });

                if (ids != null) {
                    ids = [];
                    selections = null;
                    buttonExport.prop('disabled', true);
                }

                onBuscarClick();

                showSuccess(dlgDeleteTitle, resp.mensaje);
            }, function (error) {
                showError(dlgDeleteTitle, error);
            },
            postOptions
        );

    });
}
////////////////////////////////

////////////////////////////////
//Funcionalidad Filtrar
////////////////////////////////
//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let btnBuscar = document.getElementById("btnBuscar");
    let inpSerie = document.getElementById("inpFiltroSerie");
    let inpFechaInicio = document.getElementById("inpFiltroFechaInicio");
    let inpFechaFin = document.getElementById("inpFiltroFechaFin");
    let selMoneda = document.getElementById("selFiltroMoneda");
    let selFormaPago = document.getElementById("selFiltroFormaPago");
    let selMetodoPago = document.getElementById("selFiltroMetodoPago");
    let selUsoCFDI = document.getElementById("selFiltroUsoCFDI");

    let oParams = {
        Serie: inpSerie.value,
        FechaInicio: inpFechaInicio.value,
        FechaFin: inpFechaFin.value,
        MonedaId: selMoneda.value == 0 ? null : parseInt(selMoneda.value),
        FormaPagoId: selFormaPago.value == 0 ? null : parseInt(selFormaPago.value),
        MetodoPagoId: selMetodoPago.value == 0 ? null : parseInt(selMetodoPago.value),
        UsoCFDIId: selUsoCFDI.value == 0 ? null : parseInt(selUsoCFDI.value)
    };

    //Resetea el valor de los filtros.
    document.querySelectorAll("#filtros .form-control").forEach(function (e) { e.value = ""; });
    document.querySelectorAll("#filtros .form-select").forEach(function (e) { e.value = 0; });

    doAjax(
        "/ERP/Prefacturas/Filtrar",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    let summary = ``;
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                    summaryContainer.innerHTML += `<ul>${summary}</ul>`;
                }
                showError(btnBuscar.innerHTML, resp.mensaje);
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
//Función para mostrar una prefactura como PDF
function showPDF(idPrefactura) {
    window.open(`/FileViewer?id=${idPrefactura}&module=prefacturas`, "_blank");
}

//Función para crear un nuevo objeto CFDI
function createNewCFDI() {
    let curDate = new Date();
    let year = `${curDate.getFullYear()}`.padStart(4, "0");
    let month = `${curDate.getMonth() + 1}`.padStart(2, "0");
    let day = `${curDate.getDate()}`.padStart(2, "0");
    let hour = `${curDate.getHours()}`.padStart(2, "0");
    let minute = `${curDate.getMinutes()}`.padStart(2, "0");
    let second = `${curDate.getSeconds()}`.padStart(2, "0");

    let strCurDate = `${year}-${month}-${day} ${hour}:${minute}:${second}`

    let oCFDINuevo = {
        id: nuevoRegistro,
        fecha: strCurDate,
        fechaJS: strCurDate,
        tipoComprobanteId: 0,
        serie: "",
        folio: "",
        usoCFDIId: 0,
        formaPagoId: 0,
        metodoPagoId: 0,
        monedaId: 0,
        tipoCambio: "",
        exportacionId: 0,
        numeroOperacion: "",
        emisorId: 0,
        emisor: "",
        receptorId: 0,
        receptor: "",
        conceptos: []
    };

    return oCFDINuevo;
}

//Función para inicializar el cuadro de diálogo
function initCFDIDialog(action, row) {
    let tabGenerales = document.getElementById("tabGenerales");

    let idField = document.getElementById("inpCFDIId");

    let fechaField = document.getElementById("inpFecha"),
        tipoComprobanteField = document.getElementById("selTipoComprobante");

    let serieField = document.getElementById("inpSerie"),
        folioField = document.getElementById("inpFolio"),
        usoField = document.getElementById("selUsoCFDI");

    let formaField = document.getElementById("selFormaPago"),
        metodoField = document.getElementById("selMetodoPago"),
        monedaField = document.getElementById("selMoneda"),
        tipoCambioField = document.getElementById("inpTipoCambio");

    let exportacionField = document.getElementById("selExportacion"),
        numeroOperacionField = document.getElementById("inpNumeroOperacion");

    let emisorField = document.getElementById("inpEmisor"),
        btnInfoEmisor = document.getElementById("btnInfoEmisor"),
        receptorField = document.getElementById("inpReceptor"),
        btnInfoReceptor = document.getElementById("btnInfoReceptor");

    let btnLimpiar = document.getElementById("btnLimpiar"),
        btnGuardar = document.getElementById("dlgCFDIBtnGuardar");

    let dlgTitle = document.getElementById("dlgCFDITitle"),
        summaryContainer = document.getElementById("saveValidationSummary");

    summaryContainer.innerHTML = "";

    dialogMode = action;

    idField.setAttribute("disabled", true);
    switch (action) {
        case NUEVO:
        case EDITAR:
            if (action == NUEVO) {
                dlgTitle.innerHTML = dlgNuevoTitle;
            }
            else {
                dlgTitle.innerHTML = dlgEditarTitle;
            }

            btnLimpiar.hidden = false;
            btnGuardar.hidden = false;

            document.querySelectorAll(".formButton").forEach(function (btn) { btn.classList.remove("disabled"); });
            document.querySelectorAll(".formInput, .formSelect").forEach(function (e) { e.removeAttribute("disabled"); });

            break;
        default:
            dlgTitle.innerHTML = dlgVerTitle;

            btnLimpiar.hidden = true;
            btnGuardar.hidden = true;

            document.querySelectorAll(".formButton").forEach(function (btn) { btn.classList.add("disabled"); });
            document.querySelectorAll(".formInput, .formSelect").forEach(function (e) { e.setAttribute("disabled", true); });
            break;
    }

    idField.value = row.id;

    fechaField.value = row.fechaJS;
    tipoComprobanteField.value = row.tipoComprobanteId;

    serieField.value = row.serie;
    folioField.value = row.folio;
    usoField.value = row.usoCFDIId;

    formaField.value = row.formaPagoId;
    metodoField.value = row.metodoPagoId;
    monedaField.value = row.monedaId;
    tipoCambioField.value = row.tipoCambio;
    if (action != VER) { tipoCambioField.removeAttribute("disabled"); }

    exportacionField.value = row.exportacionId;
    numeroOperacionField.value = row.numeroOperacion;

    emisorField.setAttribute("idselected", row.emisorId);
    emisorField.value = row.emisor;
    btnInfoEmisor.setAttribute("hidden", true);
    receptorField.setAttribute("idselected", row.receptorId);
    receptorField.value = row.receptor;
    btnInfoReceptor.setAttribute("hidden", true);

    tabGenerales.click();

    if (action == NUEVO || (row.hasDatosAdicionales || false)) {
        establecerDatosAdicionales(row);
        dlgCFDIModal.toggle();
        return;
    }

    doAjax(
        "/ERP/Prefacturas/DatosAdicionales",
        { idPrefactura: row.id },
        function (resp) {
            if (resp.tieneError) {
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    let summary = ``;
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                    summaryContainer.innerHTML += `<ul>${summary}</ul>`;
                }
                showError(dlgTitle.innerHTML, resp.mensaje);
                return;
            }

            //Se establece el row con los datos adicionales.
            if (typeof resp.datos == "string" && resp.datos.length >= 1) { resp.datos = JSON.parse(resp.datos); };
            row.conceptos = resp.datos.conceptos || [];
            row.hasDatosAdicionales = true;

            //Actualiza el row para no tener que volver a obtener los datos la próxima vez.
            table.bootstrapTable('updateByUniqueId', { id: row.id, row: row });

            establecerDatosAdicionales(row);
            dlgCFDIModal.toggle();

        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}

//Función para establecer los datos adicionales de la prefactura
function establecerDatosAdicionales(row) {
    //Se establecen los productos y servicios
    let data = [];
    row.conceptos = row.conceptos || [];
    row.conceptos.forEach(function (c) { data.push(c); });
    initTableProdServ(data);
}

//Función para limpiar los campos del cuadro de diálogo al cerrar.
function onCerrarClick() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    //Removes validation summary 
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');
    //Removes danger text from fields
    $(".text-danger").children().remove();
    //Removes is-valid and is-invalid class
    $(".is-valid").removeClass("is-valid");
    $(".is-invalid").removeClass("is-invalid");
}

//Función para el cierre del cuadro de diálogo
function onCerrarDialogoClick() {
    if (dialogMode == VER) {
        onCerrarClick();
        dlgCFDIModal.toggle();
    }
    else {
        askConfirmation(dlgConfirmActionTitle, dlgConfirmActionQuestion, function () {
            onCerrarClick();
            dlgCFDIModal.toggle();
        });
    }
}

//Función para el guardado de información del empleado
function onGuardarClick() {
    //Si las empresas emisora y receptora no son válidas, finaliza el proceso.
    if (!validarEmpresas()) { return; }

    //Ejecuta la validación de los campos
    $("#theForm").validate();

    //Determina los errores. Si la forma no es válida, entonces finaliza.
    if (!$("#theForm").valid()) { return; }

    //Si los conceptos no son válidos, finaliza el proceso.
    if (!validarConceptos()) { return; }

    let dlgTitle = document.getElementById("dlgCFDITitle"),
        summaryContainer = document.getElementById("saveValidationSummary"),
        idField = document.getElementById("inpCFDIId")
        emisorField = $("#inpEmisor"),
        receptorField = $("#inpReceptor"),
        fechaField = document.getElementById("inpFecha"),
        comprobanteField = document.getElementById("selTipoComprobante"),
        serieField = document.getElementById("inpSerie"),
        folioField = document.getElementById("inpFolio"),
        usoField = document.getElementById("selUsoCFDI"),
        formaField = document.getElementById("selFormaPago"),
        metodoField = document.getElementById("selMetodoPago"),
        monedaField = document.getElementById("selMoneda"),
        cambioField = document.getElementById("inpTipoCambio"),
        exportacionField = document.getElementById("selExportacion"),
        operacionField = document.getElementById("inpNumeroOperacion");
        
    summaryContainer.innerHTML = "";

    let aConceptos = tableProdServ.bootstrapTable("getData");

    let total = 0.0,
        limite = 2000000.00; //Dos millones

    aConceptos.forEach(function (c) { total += parseFloat(c.totalCalculado); });

    let oParams = {
        id: idField.value == nuevoRegistro ? 0 : idField.value,
        emisorId: emisorField.data("id"),
        receptorId: receptorField.data("id"),
        serie: serieField.value,
        folio: folioField.value,
        tipoComprobanteId: comprobanteField.value,
        fecha: fechaField.value,
        monedaId: monedaField.value,
        tipoCambio: cambioField.value,
        formaPagoId: formaField.value,
        metodoPagoId: metodoField.value,
        usoCFDIId: usoField.value,
        exportacionId: exportacionField.value,
        numeroOperacion: operacionField.value,
        requiereAutorizacion: total >= limite,
        conceptos: aConceptos
    }

    doAjax(
        "/ERP/Prefacturas/Save",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    let summary = ``;
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                    summaryContainer.innerHTML += `<ul>${summary}</ul>`;
                }
                showError(dlgTitle.innerHTML, resp.mensaje);
                return;
            }

            dlgCFDIModal.toggle();

            onBuscarClick();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}

//Función para dar formato de moneda a los campos numéricos.
function currencyFormatter(value, row, index) {
    return `$ ${numFormatter.format(value)}`;
}

//Función para dar formato de número a los campos numéricos.
function numericFormatter(value, row, index) {
    return numFormatter.format(value);
}

//Función para dar formato a los iconos de operación de los registros de productos y servicios
function operateFormatterProdServ(value, row, index) {
    if (puedeTodo || puedeEliminar) {
        switch (dialogMode) {
            case NUEVO:
            case EDITAR:
                return `<a class="delete" href="#" title="${btnEliminarTitle}"><i class="bi bi-x btn-close formButton"></i></a>`;
                break;
            default:
                return `<a class="delete" href="#" title="${btnEliminarTitle}"><i class="bi bi-x btn-close formButton disabled"></i></a>`;
                break;
        }
    }
    else {
        return `<a class="delete" href="#" title="${btnEliminarTitle}"><i class="bi bi-x btn-close formButton disabled"></i></a>`;
    }
}

//Eventos de los iconos de operación
window.operateProdServEvents = {
    'click .delete': function (e, value, row, index) {
        onEliminarProductoServicioClick(row.id);
    }
}

//Función para inicializar la tabla de productos y servicios
function initTableProdServ(data = null) {
    tableProdServ.bootstrapTable('destroy').bootstrapTable({
        locale: cultureName,
        data: data,
        columns: [
            {
                field: "state",
                checkbox: true,
                align: "center",
                valign: "middle"
            },
            {
                title: colCantidadHeader,
                field: "cantidad",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: numericFormatter
            },
            {
                title: colUnidadHeader,
                field: "unidad",
                align: "center",
                valign: "middle",
                sortable: true,
                width: "200px"
            },
            {
                title: colClaveHeader,
                field: "clave",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colDescripcionHeader,
                field: "descripcion",
                align: "center",
                valign: "middle",
                sortable: true,
                width: "200px"
            },
            {
                title: colUnitarioHeader,
                field: "unitario",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: colSubtotalHeader,
                field: "subtotalCalculado",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: colDescuentoHeader,
                field: "descuento",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: colTrasladoHeader,
                field: "trasladoCalculado",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: colRetencionHeader,
                field: "retencionCalculada",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: colTotalHeader,
                field: "totalCalculado",
                align: "center",
                valign: "middle",
                sortable: true,
                formatter: currencyFormatter
            },
            {
                title: "",
                field: "operate",
                align: "center",
                width: "100px",
                clickToSelect: false,
                events: window.operateProdServEvents,
                formatter: operateFormatterProdServ
            }
        ]
    });
    tableProdServ.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function () {
        if ($("#removeProdServ")) { $("#removeProdServ").prop('disabled', !tableProdServ.bootstrapTable('getSelections').length) }
    });
    if ($("#removeProdServ")) {

        $("#removeProdServ").click(function () {
            askConfirmation(btnEliminarTitle, dlgDeleteElementQuestion, function () {
                let elements = tableProdServ.bootstrapTable('getSelections');
                let data = tableProdServ.bootstrapTable('getData');
                let newData = [];
                data.forEach(function (d) {
                    let foundElement = elements.find(f => f.id == d.id);
                    if (!foundElement) { newData.push(d); }
                });
                initTableProdServ(newData);
                $("#removeProdServ").prop('disabled', true);
            });
        });
    }
}

//Función para mostrar u ocultar la información del emisor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleEmisorInfo(item = null) {
    let divFilledInfoEmisor = document.getElementById("divFilledInfoEmisor");
    let divEmptyInfoEmisor = document.getElementById("divEmptyInfoEmisor");
    let btnInfoEmisor = document.getElementById("btnInfoEmisor");

    let lblRFC = document.getElementById("lblRFCEmisor");
    let lblRazonSocial = document.getElementById("lblRazonSocialEmisor");
    let lblOrigen = document.getElementById("lblOrigenEmisor");
    let lblNivel = document.getElementById("lblNivelEmisor");
    let lblPerfil = document.getElementById("lblPerfilEmisor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaEmisor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalEmisor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialEmisor");

    let inpSerie = document.getElementById("inpSerie");
    let inpFolio = document.getElementById("inpFolio");

    lblNivel.classList.remove("is-invalid");
    lblNivel.classList.remove("is-valid");

    lblActividadEconomica.classList.remove("is-invalid");
    lblActividadEconomica.classList.remove("is-valid");

    lblPerfil.classList.remove("is-invalid");
    lblPerfil.classList.remove("is-valid");

    lblDomicilioFiscal.classList.remove("is-invalid");
    lblDomicilioFiscal.classList.remove("is-valid");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblOrigen.innerHTML = item.origen;
        lblNivel.innerHTML = item.nivel.nombre;
        lblPerfil.innerHTML = item.perfil;

        let listaActividades = [];
        item.actividadesEconomicas.forEach(function (a) {
            listaActividades.push(`<li id="${a.id}" clave="${a.clave}" class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                          <div class="fw-bold">${a.clave}</div>
                                          ${a.nombre}
                                        </div>
                                    </div>
								  </li>`);
        });
        lblActividadEconomica.innerHTML = `<ul class="list-group list-group-flush mt-3">${listaActividades.join("")}</ul>`;

        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        let osParts = item.objetoSocial.split("\n");
        let listParts = [];
        osParts.forEach(function (o) {
            if (o.length >= 1) {
                listParts.push(`<li class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                            <p style="text-align: justify">${o.trim()}</p>
                                        </div>
                                    </div>
				                </li>`);
            }
        });
        lblObjetoSocial.innerHTML = `<ul class="list-group list-group-flush mt-3">${listParts.join("")}</ul>`;

        divEmptyInfoEmisor.classList.remove('d-flex');
        divEmptyInfoEmisor.setAttribute('hidden', true);
        divFilledInfoEmisor.removeAttribute('hidden');

        $("#inpReceptor").data('idempresa', item.id);

        btnInfoEmisor.removeAttribute("hidden");

        let serie = item.rfc.substring(0, 3);
        inpSerie.value = serie;

        inpFolio.value = item.proximoFolio;
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblOrigen.innerHTML = emptyInfo;
        lblNivel.innerHTML = emptyInfo;
        lblPerfil.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;

        divEmptyInfoEmisor.removeAttribute('hidden');
        divFilledInfoEmisor.setAttribute('hidden', true);
        divEmptyInfoEmisor.classList.add('d-flex');

        $("#inpReceptor").data('idempresa', null);

        $("#inpEmisor").data("id", null);
        $("#inpEmisor").data("nivel", null);
        $("#inpEmisor").data("actividadesEconomicas", null);
        $("#inpEmisor").data("perfil", null);
        $("#inpEmisor").data("direccion", null);
        $("#inpEmisor").data("productosServicios", null);
        $("#inpEmisor").data("proximoFolio", null);

        btnInfoEmisor.setAttribute("hidden", true);

        inpSerie.value = "";

        inpFolio.value = "";
    }

}

//Función para mostrar u ocultar la información del receptor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleReceptorInfo(item = null) {
    let divFilledInfoReceptor = document.getElementById("divFilledInfoReceptor");
    let divEmptyInfoReceptor = document.getElementById("divEmptyInfoReceptor");
    let btnInfoReceptor = document.getElementById("btnInfoReceptor");

    let lblRFC = document.getElementById("lblRFCReceptor");
    let lblRazonSocial = document.getElementById("lblRazonSocialReceptor");
    let lblOrigen = document.getElementById("lblOrigenReceptor");
    let lblNivel = document.getElementById("lblNivelReceptor");
    let lblPerfil = document.getElementById("lblPerfilReceptor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaReceptor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalReceptor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialReceptor");

    lblNivel.classList.remove("is-invalid");
    lblNivel.classList.remove("is-valid");

    lblActividadEconomica.classList.remove("is-invalid");
    lblActividadEconomica.classList.remove("is-valid");

    lblPerfil.classList.remove("is-invalid");
    lblPerfil.classList.remove("is-valid");

    lblDomicilioFiscal.classList.remove("is-invalid");
    lblDomicilioFiscal.classList.remove("is-valid");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblOrigen.innerHTML = item.origen;
        lblNivel.innerHTML = item.nivel.nombre;
        lblPerfil.innerHTML = item.perfil;

        let listaActividades = [];
        item.actividadesEconomicas.forEach(function (a) {
            listaActividades.push(`<li id="${a.id}" clave="${a.clave}" class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                          <div class="fw-bold">${a.clave}</div>
                                          ${a.nombre}
                                        </div>
                                    </div>
								  </li>`);
        });
        lblActividadEconomica.innerHTML = `<ul class="list-group list-group-flush mt-3">${listaActividades.join("")}</ul>`;

        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        let osParts = item.objetoSocial.split("\n");
        let listParts = [];
        osParts.forEach(function (o) {
            if (o.length >= 1) {
                listParts.push(`<li class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                            <p style="text-align: justify">${o.trim()}</p>
                                        </div>
                                    </div>
				                </li>`);
            }
        });
        lblObjetoSocial.innerHTML = `<ul class="list-group list-group-flush mt-3">${listParts.join("")}</ul>`;

        divEmptyInfoReceptor.classList.remove('d-flex');
        divEmptyInfoReceptor.setAttribute('hidden', true);
        divFilledInfoReceptor.removeAttribute('hidden');

        $("#inpEmisor").data('idempresa', item.id);

        btnInfoReceptor.removeAttribute("hidden");
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblOrigen.innerHTML = emptyInfo;
        lblNivel.innerHTML = emptyInfo;
        lblPerfil.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;

        divEmptyInfoReceptor.removeAttribute('hidden');
        divFilledInfoReceptor.setAttribute('hidden', true);
        divEmptyInfoReceptor.classList.add('d-flex');

        $("#inpEmisor").data('idempresa', null);

        $("#inpReceptor").data("id", null);
        $("#inpReceptor").data("nivel", null);
        $("#inpReceptor").data("actividadesEconomicas", null);
        $("#inpReceptor").data("perfil", null);
        $("#inpReceptor").data("direccion", null);
        $("#inpReceptor").data("productosServicios", null);

        btnInfoReceptor.setAttribute("hidden", true);
    }

}

//Función para validar los datos de una empresa al ser seleccionada por el usuario.
function validarEmpresaSeleccionada(e, isEmisor, isReceptor) {
    let nivelField = null;
    let actividadesField = null;
    let perfilField = null;

    if (isEmisor) {
        nivelField = document.getElementById("lblNivelEmisor");
        actividadesField = document.getElementById("lblActividadEconomicaEmisor");
        perfilField = document.getElementById("lblPerfilEmisor");
    }
    if (isReceptor) {
        nivelField = document.getElementById("lblNivelReceptor");
        actividadesField = document.getElementById("lblActividadEconomicaReceptor");
        perfilField = document.getElementById("lblPerfilReceptor");
    }

    nivelField.classList.remove("is-invalid");
    nivelField.classList.remove("is-valid");

    actividadesField.classList.remove("is-invalid");
    actividadesField.classList.remove("is-valid");

    perfilField.classList.remove("is-invalid");
    perfilField.classList.remove("is-valid");

    let puedeFacturar = (e.nivel.puedeFacturar.toLowerCase() === 'true');
    if (!puedeFacturar) {
        nivelField.classList.add("is-invalid");
        showAlert(title, nivelNoPuedeFacturar);
        return false;
    }
    nivelField.classList.add("is-valid");

    if ((e.perfil || "").length <= 0) {
        perfilField.classList.add("is-invalid");
        showAlert(title, sinPerfilNoPuedeFacturar);
        return false;
    }
    perfilField.classList.add("is-valid");

    if ((e.actividadesEconomicas || []).length <= 0) {
        nivelField.classList.add("is-invalid");
        showAlert(title, sinActividadesNoPuedeFacturar);
        return false;
    }
    actividadesField.classList.add("is-valid");

    return true;
}

//Función para validar los datos de una empresa al ser seleccionada por el usuario.
function validarFacturacionEntreEmpresas() {
    let emisor = {
            id: $("#inpEmisor").data("id"),
            nivel: $("#inpEmisor").data("nivel"),
            actividadesEconomicas: $("#inpEmisor").data("actividadesEconomicas"),
            perfil: $("#inpEmisor").data("perfil"),
            direccion: $("#inpEmisor").data("domicilioFiscal"),
            productosServicios: $("#inpEmisor").data("productosServicios")
        },
        receptor = {
            id: $("#inpReceptor").data("id"),
            nivel: $("#inpReceptor").data("nivel"),
            actividadesEconomicas: $("#inpReceptor").data("actividadesEconomicas"),
            perfil: $("#inpReceptor").data("perfil"),
            direccion: $("#inpReceptor").data("domicilioFiscal"),
            productosServicios: $("#inpReceptor").data("productosServicios")
        },
        nivelEmisorField = document.getElementById("lblNivelEmisor"),
        direccionEmisorField = document.getElementById("lblDomicilioFiscalEmisor"),
        nivelReceptorField = document.getElementById("lblNivelReceptor"),
        direccionReceptorField = document.getElementById("lblDomicilioFiscalReceptor");

    if (!validarEmpresaSeleccionada(emisor, true, false)) { return false; }
    if (!validarEmpresaSeleccionada(receptor, false, true)) { return false; }

    nivelEmisorField.classList.remove("is-invalid");
    nivelEmisorField.classList.remove("is-valid");

    direccionEmisorField.classList.remove("is-invalid");
    direccionEmisorField.classList.remove("is-valid");

    nivelReceptorField.classList.remove("is-invalid");
    nivelReceptorField.classList.remove("is-valid");

    direccionReceptorField.classList.remove("is-invalid");
    direccionReceptorField.classList.remove("is-valid");

    //Empresas emisoras con nivel menor a nivel del receptor no pueden facturar.
    if (parseInt(emisor.nivel.ordinal || "0") < parseInt(receptor.nivel.ordinal || "0")) {
        nivelEmisorField.classList.add("is-invalid");
        nivelReceptorField.classList.add("is-invalid");
        showAlert(title, nivelMenorNoPuedeFacturar);
        return false;
    }
    if (emisor.nivel.ordinal === receptor.nivel.ordinal) {
        nivelEmisorField.classList.add("is-invalid");
        nivelReceptorField.classList.add("is-invalid");
        showAlert(title, mismoNivelNoPuedeFacturar);
        return false;
    }
    nivelEmisorField.classList.add("is-valid");
    nivelReceptorField.classList.add("is-valid");

    if (emisor.direccion === receptor.direccion) {
        direccionEmisorField.classList.add("is-invalid");
        direccionReceptorField.classList.add("is-invalid");
        showAlert(title, mismaDireccionNoPuedeFacturar);
        return false;
    }
    direccionEmisorField.classList.add("is-valid");
    direccionReceptorField.classList.add("is-valid");

    return true;
}

//Función para validar las empresas
function validarEmpresas() {
    //Se ejecuta validación de facturación entre empresas.
    let inpEmisor = document.getElementById("inpEmisor"),
        inpReceptor = document.getElementById("inpReceptor"),
        hasEmisor = parseInt(inpEmisor.getAttribute("idselected") || "0") >= 1,
        hasReceptor = parseInt(inpReceptor.getAttribute("idselected") || "0") >= 1;

    inpEmisor.classList.remove("is-invalid");
    inpEmisor.classList.remove("is-valid");

    inpReceptor.classList.remove("is-invalid");
    inpReceptor.classList.remove("is-valid");

    if (!hasEmisor && !hasReceptor) {
        inpEmisor.classList.add("is-invalid");
        inpReceptor.classList.add("is-invalid");
        showError(title, faltanEmpresasParaComparar);
        return false;
    }
    else if (!hasEmisor) {
        inpEmisor.classList.add("is-invalid");
        showError(title, faltanEmpresasParaComparar);
        return false;
    }
    else if (!hasReceptor) {
        inpReceptor.classList.add("is-invalid");
        showError(title, faltanEmpresasParaComparar);
        return false;
    }

    return validarFacturacionEntreEmpresas();
}

//Función para validar los conceptos
function validarConceptos() {
    let prodServPermitidos = $("#inpEmisor").data("productosServicios") || [];
    let empresaLabel = emisorLabel.toLowerCase();
    let prodServSelectorField = document.getElementById("inpProductoServicio");
    prodServSelectorField.classList.remove("is-invalid");
    prodServSelectorField.classList.remove("is-valid");

    if (prodServPermitidos.length <= 0) {
        showAlert(title, sinProductosServiciosNoPuedeFacturar);
        return false;
    }

    let prodServsAdded = tableProdServ.bootstrapTable('getData') || [];
    if (prodServsAdded.length <= 0) {
        prodServSelectorField.classList.add("is-invalid");
        showAlert(title, almenosUnProductoServicioNecesario);
        return false;
    }

    let hasError = false;
    let message = "";
    prodServsAdded.forEach(function (ps) {
        //Si el producto o servicio agregado no está en los permitidos, entonces falla la validación.
        let clave = ps.clave;
        let found = prodServPermitidos.find((ps) => ps.clave == clave);
        if (found == undefined) {
            hasError = true;
            message = `${productoServicioConClave} '${clave}' ${noCorresponde} ${empresaLabel} ${noPuedeFacturar}`;
            return false;
        }
    });
    if (hasError) {
        showAlert(title, message);
        return false;
    }

    return true;
}

//Función para capturar el clic del botón limpiar. Limpia y oculta la información del emisor y del receptor.
function onLimpiarClick() {
    askConfirmation(dlgConfirmActionTitle, dlgConfirmActionQuestion, function () {
        let oCFDINuevo = createNewCFDI();

        onCerrarClick();

        initCFDIDialog(NUEVO, oCFDINuevo);

        limpiarEmisor();

        limpiarReceptor();
    });
}

//Función para limpiar los datos del emisor
function limpiarEmisor() {
    let inpEmisor = document.getElementById("inpEmisor");
    inpEmisor.value = null;
    toggleEmisorInfo();
}

//Función para limpiar los datos del receptor
function limpiarReceptor() {
    let inpReceptor = document.getElementById("inpReceptor");
    inpReceptor.value = null;
    toggleReceptorInfo();
}

//Función para agregar un producto o servicio al listado
function onAgregarProductoServicioClick() {
    //Ejecuta la validación de los campos
    $("#theForm").validate();
    //Determina los errores. Si la forma no es válida, entonces finaliza.
    if (!$("#theForm").valid()) { return; }

    let productoServicioField = $(document.getElementById("inpProductoServicio")),
        cantidadField = document.getElementById("inpCantidad"),
        unitarioField = document.getElementById("inpUnitario"),
        descuentoField = document.getElementById("inpDescuento"),
        unidadField = document.getElementById("inpUnidad"),
        descripcionField = document.getElementById("inpDescripcion"),
        objetoImpuesto = document.getElementById("selObjetoImpuesto"),
        trasladoTipoField = document.getElementById("selTrasladoTipoImpuesto"),
        trasladoField = document.getElementById("inpTraslado"),
        retencionField = document.getElementById("inpRetencion"),
        retencionTipoField = document.getElementById("selRetencionTipoImpuesto");

    let cantidad = parseFloat(cantidadField.value || "0"),
        unitario = parseFloat(unitarioField.value || "0"),
        descuento = parseFloat(descuentoField.value || "0"),
        valorTraslado = trasladoField.value,
        valorRetencion = retencionField.value,
        subtotal = (cantidad * unitario) - descuento,
        traslado = subtotal * valorTraslado,
        retencion = subtotal * valorRetencion,
        total = subtotal + traslado - retencion;

    //Si el campo producto / servicio no tiene elemento seleccionado, muestra error.
    if (parseInt(productoServicioField.attr("idselected") || "0") <= 0) {
        showAlert(msgAgregarProductoServicio, sinProductoServicio);
        return;
    }

    let oTraslado = {
        tasaOCuotaId: trasladoTipoField.value||0,
        valor: valorTraslado
    }

    let oRetencion = {
        tasaOCuotaId: retencionTipoField.value||0,
        valor: valorRetencion,
    }

    let oProdServ = {
        id: productoServicioField.attr("idselected"),
        productoServicioId: productoServicioField.attr("idselected"),
        objetoImpuestoId: objetoImpuesto.value,
        cantidad: cantidad,
        unidadId: unidadField.getAttribute("idselected"),
        unidad: unidadField.value,
        clave: productoServicioField.data("clave"),
        descripcion: descripcionField.value,
        unitario: unitario,
        descuento: descuento,
        traslado: oTraslado,
        retencion: oRetencion,
        subtotalCalculado: subtotal,
        trasladoCalculado: traslado,
        retencionCalculada: retencion,
        totalCalculado: total
    };

    tableProdServ.bootstrapTable('prepend', [oProdServ]);

    initDialogProdServ();

    onCerrarClick();
}

//Función para eliminar un producto o servicio del listado
function onEliminarProductoServicioClick(id) {
    askConfirmation(btnEliminarTitle, dlgDeleteElementQuestion, function () {
        tableProdServ.bootstrapTable('removeByUniqueId', id);
    });
}

//Función para detectar el cambio de valor en el campo Moneda
function onMonedaChanged() {
    let selMoneda = document.getElementById("selMoneda"),
        inpTipoCambio = document.getElementById("inpTipoCambio"),
        clave = selMoneda.options[selMoneda.selectedIndex].getAttribute("clave"),
        clavePesoMexicano = "MXN",
        siblings = [...inpTipoCambio.parentElement.children];

    if (clave == clavePesoMexicano) {
        inpTipoCambio.value = "1";
        inpTipoCambio.setAttribute("disabled", true);

        //Removes validation from input
        let ive = siblings.filter(c => c.classList.contains("input-validation-error"));
        ive.forEach(function (e) {
            e.classList.add('input-validation-valid');
            e.classList.remove('input-validation-error');
        });
        //Removes validation message after input
        let fve = siblings.filter(c => c.classList.contains("field-validation-error"));
        fve.forEach(function (e) {
            e.classList.add('field-validation-valid');
            e.classList.remove('field-validation-error');
        });
        //Removes danger text
        let tdn = siblings.filter(c => c.classList.contains("text-danger"));
        tdn.forEach(function (e) {
            e.innerHTML = "";
        });

        inpTipoCambio.classList.remove("is-invalid");
    } else {
        inpTipoCambio.value = "";
        inpTipoCambio.removeAttribute("disabled");
    }

    $("#inpTipoCambio").change();
}
////////////////////////////////

////////////////////////////////
//Funcionalidad Diálogo Productos Servicios
////////////////////////////////
//Función para inicializar el diálogo
function initDialogProdServ() {
    let prodservField = document.getElementById("inpProductoServicio");

    let cantidadField = document.getElementById("inpCantidad"),
        unitarioField = document.getElementById("inpUnitario"),
        descuentoField = document.getElementById("inpDescuento"),
        unidadField = document.getElementById("inpUnidad"),
        descripcionField = document.getElementById("inpDescripcion");

    let objetoImpuesto = document.getElementById("selObjetoImpuesto"),
        selTrasladoTipoImpuesto = document.getElementById("selTrasladoTipoImpuesto"),
        trasladoField = document.getElementById("inpTraslado"),
        divTraslado = document.getElementById("divTraslado"),
        inpTrasladoManual = document.getElementById("inpTrasladoManual"),
        selRetencionTipoImpuesto = document.getElementById("selRetencionTipoImpuesto"),
        retencionField = document.getElementById("inpRetencion"),
        divRetencion = document.getElementById("divRetencion"),
        inpRetencionManual = document.getElementById("inpRetencionManual");

    let totalTrasladoField = document.getElementById("trasladoProducto");
    let totalRetencionField = document.getElementById("retencionProducto");
    let totalField = document.getElementById("totalProducto");

    prodservField.value = "";
    prodservField.setAttribute('idselected', "");

    cantidadField.value = 1;
    unitarioField.value = "";
    descuentoField.value = "";
    unidadField.value = "";
    unidadField.setAttribute('idselected', "");
    descripcionField.value = "";

    objetoImpuesto.value = 0;
    selTrasladoTipoImpuesto.value = 0;
    selTrasladoTipoImpuesto.parentElement.removeAttribute("hidden");
    trasladoField.value = "";
    trasladoField.parentElement.setAttribute("hidden", true);
    inpTrasladoManual.checked = false;
    divTraslado.setAttribute("hidden", true);

    selRetencionTipoImpuesto.value = 0;
    selRetencionTipoImpuesto.parentElement.removeAttribute("hidden");
    retencionField.value = "";
    retencionField.parentElement.setAttribute("hidden", true);
    inpRetencionManual.checked = false;
    divRetencion.setAttribute("hidden", true);

    totalTrasladoField.textContent = 0.00
    totalRetencionField.textContent = 0.00
    totalField.textContent = 0.00
}

//Función para calcular el total de un producto o servicio
function calcularTotal() {
    let inpCantidad = document.getElementById("inpCantidad"),
        inpUnitario = document.getElementById("inpUnitario"),
        inpDescuento = document.getElementById("inpDescuento"),
        inpTraslado = document.getElementById("inpTraslado"),
        inpRetencion = document.getElementById("inpRetencion"),
        spanTraslado = document.getElementById("trasladoProducto"),
        spanRetencion = document.getElementById("retencionProducto"),
        spanTotal = document.getElementById("totalProducto");

    let cantidad = parseFloat(inpCantidad.value || "0"),
        unitario = parseFloat(inpUnitario.value || "0"),
        descuento = parseFloat(inpDescuento.value || "0"),
        subtotal = (cantidad * unitario) - descuento,
        traslado = (subtotal * parseFloat(inpTraslado.value || "0")),
        retencion = (subtotal * parseFloat(inpRetencion.value || "0")),
        total = subtotal + traslado - retencion;

    spanTraslado.textContent = numFormatter.format(traslado);
    spanRetencion.textContent = numFormatter.format(retencion);   
    spanTotal.textContent = numFormatter.format(total);
}

//Función para establecer el comportamiento de los impuestos en base al item seleccionado de objeto impuesto
function onObjetoImpuestoChanged() {
    let selObjetoImpuesto = document.getElementById("selObjetoImpuesto"),
        inpTraslado = document.getElementById("inpTraslado"),
        divTraslado = document.getElementById("divTraslado"),
        inpRetencion = document.getElementById("inpRetencion"),
        divRetencion = document.getElementById("divRetencion"),
        objetoImpuestoSelected = selObjetoImpuesto.options[selObjetoImpuesto.selectedIndex],
        claveNoImpuestos = "01";

    divTraslado.removeAttribute("hidden");
    divRetencion.removeAttribute("hidden");

    inpTraslado.value = 0;
    inpRetencion.value = 0;

    if (objetoImpuestoSelected.getAttribute("clave") == claveNoImpuestos) {
        divTraslado.setAttribute("hidden", true);
        divRetencion.setAttribute("hidden", true);
    }
}

//Función para detectar el cambio de valor del traslado automático
function onTrasladoAutomaticoChanged() {
    let selTrasladoTipoImpuesto = document.getElementById("selTrasladoTipoImpuesto"),
        selectedTipo = selTrasladoTipoImpuesto.options[selTrasladoTipoImpuesto.selectedIndex],
        inpTraslado = document.getElementById("inpTraslado");

    inpTraslado.value = selectedTipo.getAttribute("valorMaximo");

    calcularTotal();
}

//Función para detectar el cambio de valor del traslado manual
function onTrasladoManualChanged() {
    let selTrasladoTipoImpuesto = document.getElementById("selTrasladoTipoImpuesto"),
        inpEsManual = document.getElementById("inpTrasladoManual"),
        inpTraslado = document.getElementById("inpTraslado");

    selTrasladoTipoImpuesto.value = 0;
    inpTraslado.value = 0;

    if (inpEsManual.checked) {
        selTrasladoTipoImpuesto.parentElement.setAttribute("hidden", true);
        inpTraslado.parentElement.removeAttribute("hidden");
    }
    else {
        inpTraslado.parentElement.setAttribute("hidden", true);
        selTrasladoTipoImpuesto.parentElement.removeAttribute("hidden");
    }

    calcularTotal();
}

//Función para detectar el cambio de valor del traslado automático
function onRetencionAutomaticaChanged() {
    let selRetencionTipoImpuesto = document.getElementById("selRetencionTipoImpuesto"),
        selectedTipo = selRetencionTipoImpuesto.options[selRetencionTipoImpuesto.selectedIndex],
        inpRetencion = document.getElementById("inpRetencion");

    inpRetencion.value = selectedTipo.getAttribute("valorMaximo");

    calcularTotal();
}

//Función para detectar el cambio de valor de la retención manual
function onRetencionManualChanged() {
    let selRetencionTipoImpuesto = document.getElementById("selRetencionTipoImpuesto"),
        inpEsManual = document.getElementById("inpRetencionManual"),
        inpRetencion = document.getElementById("inpRetencion");

    selRetencionTipoImpuesto.value = 0;
    inpRetencion.value = 0;

    if (inpEsManual.checked) {
        selRetencionTipoImpuesto.parentElement.setAttribute("hidden", true);
        inpRetencion.parentElement.removeAttribute("hidden");
    }
    else {
        inpRetencion.parentElement.setAttribute("hidden", true);
        selRetencionTipoImpuesto.parentElement.removeAttribute("hidden");
    }

    calcularTotal();
}
/////////////////////