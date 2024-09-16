var table;
var buttonRemove;
var tableActividad;
var selections = [];
var dlgConciliacion = null;
var dlgConciliacionModal = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const maxFileSizeInBytes = 5242880; //5mb = (5 * 1024) * 1024;
const oneMegabyteSizeInBytes = 1048576; // 1mb = (1 * 1024) * 1024
const postOptions = {
    headers: {
        "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
    }
};
document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    initTable();

    buttonRemove = $("#remove");

    dlgConciliacion = document.getElementById('dlgConciliacion');
    dlgConciliacionModal = new bootstrap.Modal(dlgConciliacion, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlgConciliacion.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });
    //Función para ejecutar acciones posteriores al mostrado del diálogo.
    dlgConciliacion.addEventListener('shown.bs.modal', function (e) {
        //Este evento es necesario para poder mostrar el text area ajustado al tamaño del contenido, basado en el tamaño del scroll.
        calculateTextAreaHeight(document.querySelectorAll("textarea"));
    })

    let btnBuscar = document.getElementById("btnBuscar");
    btnBuscar.click();
});

//Funcionalidad Tabla
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

//Función para añadir botones a la cinta de botones de la tabla
function additionalButtons() {
    return {
        // Aquí puedes agregar otros botones si los tienes, pero sin el botón de importar
    };
}

//Función para dar formato a los iconos de operación de los registros
function operateFormatter(value, row, index) {
    let icons = [];

    //Icono Ver
    //icons.push(`<li><a class="dropdown-item see" href="#" title="${btnVerTitle}"><i class="bi bi-search"></i> ${btnVerTitle}</a></li>`);
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
        initConciliacionDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initConciliacionDialog(EDITAR, row);
        //table.bootstrapTable('remove', {
        //    field: 'id',
        //    values: [row.id]
        //})
    }
}

function onAgregarClick() {
    initConciliacionDialog(NUEVO, { id: "Nuevo", nombre: "" });
}

function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel'],
        toolbar: '#toolbar', // Asegúrate de que este ID coincida con el elemento HTML donde quieres que aparezcan los botones
        buttons: additionalButtons, // Asegúrate de que `additionalButtons` esté siendo llamado correctamente aquí
        columns: [
            {
                field: "state",
                checkbox: true,
                align: "center",
                valign: "middle"
            },
            {
                title: colIdHeader,
                field: "Id",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFechaHeader,
                field: "Fecha",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colDescripcionHeader,
                field: "Descripcion",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colClienteHeader,
                field: "Cliente",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colTotalHeader,
                field: "Total",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colUsuarioCreadorHeader,
                field: "UsuarioCreador",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colUsuarioModificoHeader,
                field: "UsuarioModificador",
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
                "/ERP/Conciliaciones/DeleteConciliaciones",
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

//Funcionalidad Diálogo Conciliación
function initConciliacionDialog(action, row) {
    // Obtener los campos del formulario
    let idField = document.getElementById("inpConciliacionId");
    let descripcionField = document.getElementById("inpConciliacionDescripcion");
    let totalField = document.getElementById("inpConciliacionTotal");
    let bancoField = document.getElementById("inpConciliacionBanco");
    let clienteField = document.getElementById("inpConciliacionCliente");
    let empresaField = document.getElementById("inpConciliacionEmpresa");
    let fechaField = document.getElementById("inpConciliacionFecha");
    let detallesField = document.getElementById("inpConciliacionDetalles");
    let btnGuardar = document.getElementById("dlgConciliacionBtnGuardar");
    let dlgTitle = document.getElementById("dlgConciliacionTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");

    // Limpiar los mensajes de validación
    summaryContainer.innerHTML = "";

    // Deshabilitar el campo de ID por defecto
    idField.setAttribute("disabled", true);

    // Configuración según la acción
    switch (action) {
        case NUEVO:
            dlgTitle.innerHTML = "Nueva conciliación";
            descripcionField.removeAttribute("disabled");
            totalField.removeAttribute("disabled");
            bancoField.removeAttribute("disabled");
            clienteField.removeAttribute("disabled");
            empresaField.removeAttribute("disabled");
            fechaField.removeAttribute("disabled");
            detallesField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        case EDITAR:
            dlgTitle.innerHTML = "Editar conciliación";
            descripcionField.removeAttribute("disabled");
            totalField.removeAttribute("disabled");
            bancoField.removeAttribute("disabled");
            clienteField.removeAttribute("disabled");
            empresaField.removeAttribute("disabled");
            fechaField.removeAttribute("disabled");
            detallesField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        default:
            dlgTitle.innerHTML = "Consultar conciliación";
            descripcionField.setAttribute("disabled", true);
            totalField.setAttribute("disabled", true);
            bancoField.setAttribute("disabled", true);
            clienteField.setAttribute("disabled", true);
            empresaField.setAttribute("disabled", true);
            fechaField.setAttribute("disabled", true);
            detallesField.setAttribute("disabled", true);
            btnGuardar.setAttribute("disabled", true);
            break;
    }

    // Asignar valores a los campos del diálogo usando los valores de la entidad Conciliacion
    idField.value = row.id || "";
    descripcionField.value = row.descripcion || "";
    totalField.value = row.total || "";
    bancoField.value = row.bancoId || "";
    clienteField.value = row.clienteId || "";
    empresaField.value = row.empresaId || "";
    fechaField.value = row.fecha ? row.fecha.toISOString().split('T')[0] : ""; // Si `Fecha` es una instancia de TimeSpan
    detallesField.value = row.detallesConciliacion || "";

    // Mostrar el modal
    dlgConciliacionModal.toggle();
}
function onGuardarConciliacionClick() {
    // Ejecuta la validación del formulario con el id "theForm"
    $("#theForm").validate();
    // Determina si la validación fue exitosa
    let valid = $("#theForm").valid();
    // Si el formulario no es válido, termina la ejecución
    if (!valid) { return; }

    let btnClose = document.getElementById("dlgConciliacionBtnCancelar");
    let idField = document.getElementById("inpConciliacionId");
    let descripcionField = document.getElementById("inpConciliacionDescripcion");
    let totalField = document.getElementById("inpConciliacionTotal");
    let bancoField = document.getElementById("inpConciliacionBanco");
    let clienteField = document.getElementById("inpConciliacionCliente");
    let empresaField = document.getElementById("inpConciliacionEmpresa");
    let fechaField = document.getElementById("inpConciliacionFecha");
    let detallesField = document.getElementById("inpConciliacionDetalles");
    let dlgTitle = document.getElementById("dlgConciliacionTitle");
    let summaryContainer = document.getElementById("saveValidationSummaryConciliacion");
    summaryContainer.innerHTML = "";

    // Parámetros que se enviarán en la solicitud
    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        descripcion: descripcionField.value,
        total: totalField.value,
        bancoId: bancoField.value,
        clienteId: clienteField.value,
        empresaId: empresaField.value,
        fecha: fechaField.value,
        detallesConciliacion: detallesField.value
    };

    // Llamada AJAX para guardar los datos
    doAjax(
        "/ERP/Conciliaciones/SaveConciliacion",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                // Si hay errores, los muestra en el resumen de validación
                if (Array.isArray(resp.errores) && resp.errores.length >= 1) {
                    let summary = ``;
                    resp.errores.forEach(function (error) {
                        summary += `<li>${error}</li>`;
                    });
                    summaryContainer.innerHTML += `<ul>${summary}</ul>`;
                }
                // Muestra el error general
                showError(dlgTitle.innerHTML, resp.mensaje);
                return;
            }

            // Cierra el diálogo al terminar
            btnClose.click();

            // Actualiza la tabla para reflejar los cambios
            let e = document.querySelector("[name='refresh']");
            e.click();

            // Muestra mensaje de éxito
            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            // Maneja errores de la solicitud AJAX
            showError("Error", error);
        },
        postOptions
    );
}