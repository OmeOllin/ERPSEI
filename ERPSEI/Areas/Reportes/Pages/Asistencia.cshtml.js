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
    dlg = document.getElementById('dlgAsistencia');
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
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel'],
        columns: [
            {
                title: colNombreHeader,
                field: "nombre",
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
                title: colHoraEntradaHeader,
                field: "horaEntrada",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colHoraSalidaHeader,
                field: "horaSalida",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colRetardoHeader,
                field: "retardo",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colTotalHeader,
                field: "total",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFaltasHeader,
                field: "faltas",
                align: "center",
                valign: "middle",
                sortable: true
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
                "/Catalogos/Asistencias/DeleteAsistencias",
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
function onGuardarClick() {
    // Ejecuta la validación
    $("#theForm").validate();
    // Determina los errores
    let valid = $("#theForm").valid();
    // Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }
    
    let idField = document.getElementById("inpAsistenciaId");
    let nombreField = document.getElementById("inpAsistenciaNombre");
    let fechaField = document.getElementById("inpAsistenciaFecha");
    let horaEntradaField = document.getElementById("inpAsistenciaHoraEntrada");
    let horaSalidaField = document.getElementById("inpAsistenciaHoraSalida");
    let retardoField = document.getElementById("inpAsistenciaRetardo");
    let totalField = document.getElementById("inpAsistenciaTotal");
    let faltasField = document.getElementById("inpAsistenciaFaltas");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        nombre: nombreField.value,
        fecha: fechaField.value,
        horaEntrada: horaEntradaField.value,
        horaSalida: horaSalidaField.value,
        retardo: retardoField.value,
        total: totalField.value,
        faltas: faltasField.value
    };

    doAjax(
        "/Catalogos/Asistencias/SaveAsistencia",
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