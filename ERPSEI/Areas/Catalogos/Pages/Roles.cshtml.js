var table;
var selections = [];
var dlgDetalleModal = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

//Función para inicializar el módulo.
document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");

    dlgDetalleModal = new bootstrap.Modal(document.getElementById('dlgDetalle'), null);

    initTable();

    onBuscarClick();
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
    if (typeof res == "string" && res.length >= 1) { res = JSON.parse(res); }
    $.each(res, function (i, row) { row.state = $.inArray(row.id, selections) !== -1 });
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

    return icons.join('');
}
//Eventos de los iconos de operación
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initDetallesDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initDetallesDialog(EDITAR, row);
    }
}
//Función para agregar cfdis
function onAgregarClick() {
    let aModulos = [];
    moduleList.forEach(function (m) {
        aModulos.push({
            nombre: m,
            puedeTodo: false,
            puedeConsultar: false,
            puedeEditar: false,
            puedeEliminar: false,
            puedeAutorizar: false
        });
    });

    let oRol = { id: "Nuevo", rol: "", modulos: aModulos }
    initDetallesDialog(NUEVO, oRol);
    dlgDetalleModal.toggle();
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
                title: colNombreHeader,
                field: "rol",
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
    table.on('check.bs.table uncheck.bs.table ' +
        'check-all.bs.table uncheck-all.bs.table',
        function () {
            // save your data, here just save the current page
            selections = getIdSelections()
            // push or splice the selections if you want to save all data selections
        }
    );
    table.on('all.bs.table', function (e, name, args) {
        console.log(name, args)
    });
}
////////////////////////////////

/////////////////////////////////
//Funcionalidad Filtrar
/////////////////////////////////
//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let oParams = {};

    doAjax(
        "/Catalogos/Roles/Filtrar",
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
                showError(btnBuscarTitle, resp.mensaje);
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
//Funcionalidad Diálogo
////////////////////////////////
//Función para inicializar el cuadro de diálogo de los detalles del registro
function initDetallesDialog(action, row) {
    let inpRolNombre = document.getElementById("inpRolNombre");
    let dlgTitle = document.getElementById("dlgDetalleTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    switch (action) {
        case EDITAR:
            dlgTitle.innerHTML = dlgEditarTitle;        

            inpRolNombre.removeAttribute("disabled");
            break;
        default:
            dlgTitle.innerHTML = dlgVerTitle;

            inpRolNombre.setAttribute("disabled", true);
            break;
    }

    inpRolNombre.value = row.rol;

    row.modulos.forEach(function (m) {
        let inpPuedeTodo = document.getElementById(`inpPuedeTodo${m.nombre}`);
        let inpPuedeConsultar = document.getElementById(`inpPuedeConsultar${m.nombre}`);
        let inpPuedeEditar = document.getElementById(`inpPuedeEditar${m.nombre}`);
        let inpPuedeEliminar = document.getElementById(`inpPuedeEliminar${m.nombre}`);
        let inpPuedeAutorizar = document.getElementById(`inpPuedeAutorizar${m.nombre}`);

        inpPuedeTodo.checked = (m.puedeTodo || "False") == "True";
        inpPuedeConsultar.checked = (m.puedeConsultar || "False") == "True";
        inpPuedeEditar.checked = (m.puedeEditar || "False") == "True";
        inpPuedeEliminar.checked = (m.puedeEliminar || "False") == "True";
        inpPuedeAutorizar.checked = (m.puedeAutorizar || "False") == "True";
    });



    dlgDetalleModal.toggle();
}

//Función para el cierre del cuadro de diálogo
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
//Función para el guardado de información
function onGuardarClick() {
    //Ejecuta la validación
    $("#theForm").validate();
    //Determina los errores
    let valid = $("#theForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let idField = document.getElementById("inpUsuarioId");
    let selRolEmpleadoField = document.getElementById("selEmpleadoRol");
    let dlgTitle = document.getElementById("dlgDetalleTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");

    summaryContainer.innerHTML = "";

    let oParams = {
        id: idField.value,
        rolId: selRolEmpleadoField.value
    };

    doAjax(
        "/Catalogos/Usuarios/Save",
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

            onCerrarClick();

            dlgDetalleModal.toggle();

            onBuscarClick();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}
////////////////////////////////