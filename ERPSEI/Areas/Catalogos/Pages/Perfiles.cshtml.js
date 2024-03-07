var table;
var buttonRemove;
var selections = [];
var dlg = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");
    dlg = document.getElementById('dlg');
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlg.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });

    initTable();

    autoCompletar("#inpProductoServicio", {
        select: function (element, item) {
            let btnAdd = document.getElementById("dlgBtnAgregarProductoServicio");
            btnAdd.classList.remove("disabled");
        },
        change: function (element, item) {
            let actividadField = document.getElementById("inpProductoServicio");
            if (parseInt(actividadField.getAttribute("idselected") || "0") <= 0) {
                let btnAdd = document.getElementById("dlgBtnAgregarProductoServicio");
                btnAdd.classList.add("disabled");
            }
        }
    });
});

//Funcionalidad Tabla
function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}
function responseHandler(res) {
    $.each(res, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1
    })
    return res
}
function operateFormatter(value, row, index) {
    return [
        '<a class="see btn" href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#dlg" title="' + btnVerTitle + '">',
            '<i class="bi bi-search"></i>',
        '</a>  ',
        '<a class="edit btn" href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#dlg" title="' + btnEditarTitle + '">',
            '<i class="bi bi-pencil-fill"></i>',
        '</a>'
    ].join('')
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
    initDialog(NUEVO, { id: "Nuevo", nombre: "" });
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
    table.on('all.bs.table', function (e, name, args) {
        console.log(name, args)
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

//Funcionalidad Diálogo
function initDialog(action, row) {
    let idField = document.getElementById("inpId");
    let nombreField = document.getElementById("inpNombre");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    idField.setAttribute("disabled", true);

    prepareForm(action);

    idField.value = row.id;
    nombreField.value = row.nombre;

    //Se establecen los productos y servicios
    $("#listProductosServicios").html("");
    row.productosServicios = row.productosServicios || [];
    row.productosServicios.forEach(function (p) { agregarProductoServicio(p.id, p.clave, p.descripcion); });
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
    $("#listProductosServicios li").each(function (i, a) {
        let id = a.getAttribute("id");

        prodserv.push({ id: id });
    });

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

//Función para agregar un producto o servicio al listado
function onAgregarProductoServicioClick() {
    let productoServicioField = $(document.getElementById("inpProductoServicio"));
    //Si el campo producto / servicio no tiene elemento seleccionado, muestra error.
    if (parseInt(productoServicioField.attr("idselected") || "0") <= 0) {
        showAlert(msgAgregarProductoServicio, sinProductoServicio);
        return;
    }

    let listItem = document.querySelector(`li[clave='${productoServicioField.data("clave")}']`);
    //Si el elemento ya existe, muestra error.
    if (listItem) {
        showAlert(msgAgregarProductoServicio, productoServicioRepetido);
        return;
    }

    agregarProductoServicio(productoServicioField.attr("idselected"), productoServicioField.data("clave"), productoServicioField.data("value"));

    let btnAdd = document.getElementById("dlgBtnAgregarProductoServicio");
    btnAdd.classList.add("disabled");

    productoServicioField.val("");
    productoServicioField.attr("idselected", 0);
}
//Función para añadir un elemento al listado de productos y servicios
function agregarProductoServicio(id, clave, descripcion) {
    let listProductosServicios = document.getElementById("listProductosServicios");

    listProductosServicios.innerHTML += `<li id="${id}" clave="${clave}" class="list-group-item">
                                    <div class="row">
                                        <div class="col-11 border-end">
                                          <div class="fw-bold">${clave}</div>
                                          ${descripcion}
                                        </div>
                                        <div class="col-1 align-items-center d-flex justify-content-center">
									        <button type="button" class="btn-close formButton" onclick="onEliminarProductoServicioClick(${clave});"></button>
                                        </div>
                                    </div>
								  </li>`;
}
//Función para eliminar un producto o servicio del listado
function onEliminarProductoServicioClick(clave) {
    let listItem = document.querySelector(`li[clave='${clave}']`);
    listItem.remove();
}
/////////////////////