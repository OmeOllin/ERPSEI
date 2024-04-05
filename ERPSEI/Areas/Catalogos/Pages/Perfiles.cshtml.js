var table;
var buttonRemove;
var tableProdServ;
var selections = [];
var dlg = null;
var dlgModal = null;

var dialogMode = null;
const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");
    tableProdServ = $("#tableProductosServicios");
    dlg = document.getElementById('dlg');
    dlgModal = new bootstrap.Modal(dlg, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlg.addEventListener('hidden.bs.modal', function (event) { onCerrarClick(); });

    initTable();

    autoCompletar("#inpProductoServicio");
});

//Funcionalidad Tabla
function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}
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
    icons.push(`<li><a class="dropdown-item see" href="#" title="${btnVerTitle}"><i class="bi bi-search"></i> ${btnVerTitle}</a></li>`);
    //Icono Editar
    icons.push(`<li><a class="dropdown-item edit" href="#" title="${btnEditarTitle}"><i class="bi bi-pencil-fill"></i> ${btnEditarTitle}</a></li>`);

    return `<div class="dropdown">
              <button class="btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                <i class="bi bi-three-dots-vertical success"></i>
              </button>
              <ul class="dropdown-menu">${icons.join("")}</ul>
            </div>`;
}
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initDialog(EDITAR, row);
    }
}
function onAgregarClick() {
    initDialog(NUEVO, { id: "Nuevo", nombre: "", productosServicios: [] });
}
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel'],
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
                title: colNombreHeader,
                field: "nombre",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colAccionesHeader,
                field: "operate",
                align: 'center',
                width: "100px",
                clickToSelect: false,
                events: window.operateEvents,
                formatter: operateFormatter
            }
        ]
    })
    table.on('check.bs.table uncheck.bs.table ' +
        'check-all.bs.table uncheck-all.bs.table',
        function () {
            buttonRemove.prop('disabled', !table.bootstrapTable('getSelections').length)

            // save your data, here just save the current page
            selections = getIdSelections()
            // push or splice the selections if you want to save all data selections
        })
    buttonRemove.click(function () {
        askConfirmation(dlgDeleteTitle, dlgDeleteQuestion, function () {
            let oParams = { ids: selections };

            doAjax(
                "/Catalogos/Perfiles/Delete",
                oParams,
                function (resp) {
                    if (resp.tieneError) {
                        showError(dlgDeleteTitle, resp.mensaje);
                        return;
                    }

                    table.bootstrapTable('remove', {
                        field: 'id',
                        values: selections
                    })
                    selections = [];
                    buttonRemove.prop('disabled', true);

                    let e = document.querySelector("[name='refresh']");
                    e.click();

                    showSuccess(dlgDeleteTitle, resp.mensaje);
                }, function (error) {
                    showError(dlgDeleteTitle, error);
                },
                postOptions
            );
        });
    })
}
/////////////////////

//Funcionalidad Diálogo perfil
function initDialog(action, row) {
    let idField = document.getElementById("inpId");
    let nombreField = document.getElementById("inpNombre");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    dialogMode = action;

    idField.setAttribute("disabled", true);

    idField.value = row.id;
    nombreField.value = row.nombre;

    //Se establecen los productos y servicios
    let data = [];
    row.productosServicios = row.productosServicios || [];
    row.productosServicios.forEach(function (p) { data.push(p); });
    initTableProdServ(data);

    prepareForm(action);

    dlgModal.toggle();
}
//Función para hablitar / deshabilitar el formulario de empresa
function prepareForm(action) {
    let dlgTitle = document.getElementById("dlgTitle");

    switch (action) {
        case NUEVO:
        case EDITAR:
            if (action == NUEVO) {
                dlgTitle.innerHTML = dlgNuevoTitle;
            }
            else {
                dlgTitle.innerHTML = dlgEditarTitle;
            }

            document.querySelectorAll(".formButton").forEach(function (btn) { btn.classList.remove("disabled"); });
            document.querySelectorAll(".formInput, .formSelect").forEach(function (e) { e.removeAttribute("disabled"); });


            break;
        default:
            dlgTitle.innerHTML = dlgVerTitle;

            document.querySelectorAll(".formButton").forEach(function (btn) { btn.classList.add("disabled"); });
            document.querySelectorAll(".formInput, .formSelect").forEach(function (e) { e.setAttribute("disabled", true); });
            break;
    }
}
//Función para cerrar el formulario de empresa
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
    $(".text-danger").children().remove()
}
//Función para guardar la información de una empresa
function onGuardarClick() {
    //Ejecuta la validación
    $("#theForm").validate();
    //Determina los errores
    let valid = $("#theForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let btnClose = document.getElementById("dlgBtnCancelar");
    let idField = document.getElementById("inpId");
    let nombreField = document.getElementById("inpNombre");
    let dlgTitle = document.getElementById("dlgTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let prodserv = [];
    let data = tableProdServ.bootstrapTable('getData');
    data.forEach(function (e) { prodserv.push(e.id); });

    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        nombre: nombreField.value,
        productosServicios: prodserv
    };

    doAjax(
        "/Catalogos/Perfiles/Save",
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

            btnClose.click();

            let e = document.querySelector("[name='refresh']");
            e.click();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}
//Función para dar formato a la descripción de los registros de productos y servicios
function prodServDescFormatter(value, row, index) {
    return `<div>
                <div class="fw-bold">${row.clave}</div>
                ${row.descripcion}
            </div>`;
}
//Función para dar formato a los iconos de operación de los registros de productos y servicios
function operateFormatterProdServ(value, row, index) {
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
                title: colDescripcionHeader,
                field: "descripcion",
                align: "left",
                valign: "middle",
                sortable: true,
                formatter: prodServDescFormatter
            },
            {
                title: colAccionesHeader,
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
        $("#removeProdServ").prop('disabled', !tableProdServ.bootstrapTable('getSelections').length)
    });
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
//Función para agregar un producto o servicio al listado
function onAgregarProductoServicioClick() {
    let productoServicioField = $(document.getElementById("inpProductoServicio"));
    //Si el campo producto / servicio no tiene elemento seleccionado, muestra error.
    if (parseInt(productoServicioField.attr("idselected") || "0") <= 0) {
        showAlert(msgAgregarProductoServicio, sinProductoServicio);
        return;
    }

    let data = tableProdServ.bootstrapTable('getData');
    let listItem = data.find(function (e) { return e.clave == productoServicioField.data("clave") });
    //Si el elemento ya existe, muestra error.
    if (listItem) {
        showAlert(msgAgregarProductoServicio, productoServicioRepetido);
        return;
    }

    agregarProductoServicio(productoServicioField.attr("idselected"), productoServicioField.data("clave"), productoServicioField.data("value"));

    productoServicioField.val("");
    productoServicioField.attr("idselected", 0);
}
//Función para añadir un elemento al listado de productos y servicios
function agregarProductoServicio(id, clave, descripcion) {
    tableProdServ.bootstrapTable('prepend', [{
        id: id,
        clave: clave,
        descripcion: descripcion
    }]);
}
//Función para eliminar un producto o servicio del listado
function onEliminarProductoServicioClick(id) {
    askConfirmation(btnEliminarTitle, dlgDeleteElementQuestion, function () {
        tableProdServ.bootstrapTable('removeByUniqueId', id);
    });
}
/////////////////////

//Funcionalidad Diálogo Productos Servicios
function initDialogProdServ() {
    let prodservField = document.getElementById("inpProductoServicio");

    prodservField.value = "";
    prodservField.setAttribute('idselected', "");
}
/////////////////////