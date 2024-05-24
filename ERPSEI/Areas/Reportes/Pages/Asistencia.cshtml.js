var table;
var buttonRemove;
var selections = [];
var dlg = null;
var dlgModal = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");
    dlg = document.getElementById('dlgPuesto');
    dlgModal = new bootstrap.Modal(dlg, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlg.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });

    initTable();
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
        initPuestoDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initPuestoDialog(EDITAR, row);
        //table.bootstrapTable('remove', {
        //    field: 'id',
        //    values: [row.id]
        //})
    }
}
function onAgregarClick() {
    initPuestoDialog(NUEVO, { id: "Nuevo", nombre: "" });
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
                title: "Nombre",
                field: "nombre",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Fecha",
                field: "fecha",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Hora Entrada",
                field: "hora_entrada",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Hora Salida",
                field: "hora_salida",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Retardo",
                field: "retardo",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Total",
                field: "total",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Faltas",
                field: "faltas",
                align: "center",
                valign: "middle",
                sortable: true
            },
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
                "/Catalogos/Puestos/DeletePuestos",
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
function initAsistenciaDialog(action, row) {
    let idField = document.getElementById("inpAsistenciaId");
    let nombreField = document.getElementById("inpAsistenciaNombre");
    let fechaField = document.getElementById("inpAsistenciaFecha");
    let horaEntradaField = document.getElementById("inpAsistenciaHoraEntrada");
    let horaSalidaField = document.getElementById("inpAsistenciaHoraSalida");
    let retardoField = document.getElementById("inpAsistenciaRetardo");
    let totalField = document.getElementById("inpAsistenciaTotal");
    let faltasField = document.getElementById("inpAsistenciaFaltas");
    let btnGuardar = document.getElementById("dlgAsistenciaBtnGuardar");
    let dlgTitle = document.getElementById("dlgAsistenciaTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    idField.setAttribute("disabled", true);

    switch (action) {
        case NUEVO:
            dlgTitle.innerHTML = dlgNuevoTitle;

            nombreField.removeAttribute("disabled");
            fechaField.removeAttribute("disabled");
            horaEntradaField.removeAttribute("disabled");
            horaSalidaField.removeAttribute("disabled");
            retardoField.removeAttribute("disabled");
            totalField.removeAttribute("disabled");
            faltasField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        case EDITAR:
            dlgTitle.innerHTML = dlgEditarTitle;

            nombreField.removeAttribute("disabled");
            fechaField.removeAttribute("disabled");
            horaEntradaField.removeAttribute("disabled");
            horaSalidaField.removeAttribute("disabled");
            retardoField.removeAttribute("disabled");
            totalField.removeAttribute("disabled");
            faltasField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        default:
            dlgTitle.innerHTML = dlgVerTitle;

            nombreField.setAttribute("disabled", true);
            fechaField.setAttribute("disabled", true);
            horaEntradaField.setAttribute("disabled", true);
            horaSalidaField.setAttribute("disabled", true);
            retardoField.setAttribute("disabled", true);
            totalField.setAttribute("disabled", true);
            faltasField.setAttribute("disabled", true);
            btnGuardar.setAttribute("disabled", true);
            break;
    }

    idField.value = row.id;
    nombreField.value = row.nombre;
    fechaField.value = row.fecha;
    horaEntradaField.value = row.hora_entrada;
    horaSalidaField.value = row.hora_salida;
    retardoField.value = row.retardo;
    totalField.value = row.total;
    faltasField.value = row.faltas;

    dlgModal.toggle();
}

