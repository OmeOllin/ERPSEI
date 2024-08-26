var table;
var buttonRemove;
var tableActividad;
var selections = [];
var dlgAsistencia = null;
var dlgAsistenciaModal = null;

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

    dlgAsistencia = document.getElementById('dlgAsistencia');
    dlgAsistenciaModal = new bootstrap.Modal(dlgAsistencia, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlgAsistencia.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });
    //Función para ejecutar acciones posteriores al mostrado del diálogo.
    dlgAsistencia.addEventListener('shown.bs.modal', function (e) {
        //Este evento es necesario para poder mostrar el text area ajustado al tamaño del contenido, basado en el tamaño del scroll.
        calculateTextAreaHeight(document.querySelectorAll("textarea"));
    })

    let btnBuscar = document.getElementById("btnBuscar");
    btnBuscar.click();
});

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
        btnImport: {
            text: btnImportarText,
            icon: 'bi-upload',
            event: function () { },
            attributes: {
                "title": btnImportarTitle,
                "data-bs-toggle": "modal",
                "data-bs-target": "#dlgImportarExcel"
            }
        }
    }
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
    'click .edit': function (e, value, row, index) {
        initAsistenciaDialog(EDITAR, row);
        /*table.bootstrapTable('remove', {
            field: 'id',
            values: [row.id]
        })*/
    }
}

$(document).ready(function () {
    $('#btnCalcularAsistencia').on('click', function () {
        $('#dlgAsistenciaModal').modal('show');

        $.ajax({
            url: '/Reportes/Asistencia/AsistenciasCalculo',
            type: 'GET',
            success: function (data) {
                console.log('Datos recibidos:', data);

                let jsonData = data;

                let tableHtml = '<table class="table table-striped">';
                tableHtml += '<thead><tr><th>Nombre</th><th>Retardos</th><th>Omisión/Falta</th><th>Acumulado Ret</th><th>Total Faltas</th></tr></thead>';
                tableHtml += '<tbody>';

                jsonData.forEach(function (item) {
                    tableHtml += `<tr>
                        <td>${item.nombre}</td>
                        <td>${item.retardos}</td>
                        <td>${item.omisionesFaltas}</td>
                        <td>${item.acumuladoRet}</td>
                        <td>${item.totalFaltas}</td>
                    </tr>`;
                });

                tableHtml += '</tbody></table>';

                $('#jtableContainer').html(tableHtml);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching data:', error);
            }
        });
    });

    // Mueve el evento del botón de exportación fuera del AJAX
    $('#btnExportExcel').off('click').on('click', function () {
        // Asumiendo que jsonData ya está definido en el ámbito general
        var jsonData = $('#jtableContainer').find('tr').map(function () {
            return {
                nombre: $(this).find('td').eq(0).text(),
                retardos: $(this).find('td').eq(1).text(),
                omisionesFaltas: $(this).find('td').eq(2).text(),
                acumuladoRet: $(this).find('td').eq(3).text(),
                totalFaltas: $(this).find('td').eq(4).text()
            };
        }).get();

        // Crear un nuevo workbook y worksheet
        var workbook = new ExcelJS.Workbook();
        var worksheet = workbook.addWorksheet('Asistencias');

        // Establecer las columnas
        worksheet.columns = [
            { header: 'Nombre', key: 'nombre', width: 30 },
            { header: 'Retardos', key: 'retardos', width: 15 },
            { header: 'Omisión/Falta', key: 'omisionesFaltas', width: 20 },
            { header: 'Acumulado Ret', key: 'acumuladoRet', width: 20 },
            { header: 'Total Faltas', key: 'totalFaltas', width: 20 }
        ];

        // Establecer estilos para el encabezado
        worksheet.getRow(1).eachCell(function (cell) {
            cell.font = { bold: true };
            cell.alignment = { vertical: 'middle', horizontal: 'center' };
            cell.fill = {
                type: 'pattern',
                pattern: 'solid',
                fgColor: { argb: '5ba9f2' }
            };
        });

        // Agregar los datos
        jsonData.forEach(function (item) {
            if (item.nombre && item.retardos && item.omisionesFaltas && item.acumuladoRet && item.totalFaltas) {
                worksheet.addRow({
                    nombre: item.nombre,
                    retardos: parseInt(item.retardos,10),
                    omisionesFaltas: parseInt(item.omisionesFaltas,10),
                    acumuladoRet: parseInt(item.acumuladoRet,10),
                    totalFaltas: parseInt(item.totalFaltas,10)
                });
            }
        });


        // Establecer alineación de datos
        worksheet.eachRow({ includeEmpty: false }, function (row, rowNumber) {
            row.eachCell({ includeEmpty: false }, function (cell, colNumber) {
                cell.alignment = { vertical: 'middle', horizontal: 'center' };
            });
        });

        // Obtener la fecha actual en formato YYYY-MM-DD
        var currentDate = new Date();
        var formattedDate = currentDate.toISOString().split('T')[0];

        // Exportar el archivo Excel
        workbook.xlsx.writeBuffer().then(function (buffer) {
            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), `Asistencias_${formattedDate}.xlsx`);
        });
    });
});


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
            title: colHorarioHeader,
            field: "Horario",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colNombreEmpleadoHeader,
            field: "NombreEmpleado",
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
            title: colDiaHeader,
            field: "Dia",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colEntradaHeader,
            field: "Entrada",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colResultadoEHeader,
            field: "ResultadoE",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colSalidaHeader,
            field: "Salida",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colResultadoSHeader,
            field: "ResultadoS",
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
    });
}

//Funcionalidad Diálogo
function initAsistenciaDialog(action, row) {
    // Obtener los elementos del modal
    let idField = document.getElementById("inpAsistenciaId");
    let nombreField = document.getElementById("inpAsistenciaNombre");
    let resultadoEField = document.getElementById("inpAsistenciaResultadoE");
    let resultadoSField = document.getElementById("inpAsistenciaResultadoS");
    let btnGuardar = document.getElementById("dlgAsistenciaBtnGuardar");
    let dlgTitle = document.getElementById("dlgAsistenciaTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");

    // Limpiar mensajes de error
    summaryContainer.innerHTML = "";

    // Mostrar título basado en la acción
    switch (action) {
        case EDITAR:
            dlgTitle.innerHTML = dlgEditarTitle;

            // Habilitar campos para edición

            idField.setAttribute("disabled", false);
            nombreField.setAttribute("disabled", true);
            resultadoEField.removeAttribute("disabled");
            resultadoSField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
    }

    // Establecer los valores de los campos
    idField.value = row.Id;
    nombreField.value = row.NombreEmpleado;
    resultadoEField.value = row.ResultadoE;
    resultadoSField.value = row.ResultadoS;

    // Mostrar el diálogo
    dlgAsistenciaModal.toggle();
}

// Función para manejar el click en el botón de búsqueda
function onBuscarClick() {
    let nombreField = document.getElementById("inpFiltroNombreEmpleado").value.trim();
    let fechaInicioField = document.getElementById("inpFiltroFechaIngresoInicio").value.trim();
    let fechaFinField = document.getElementById("inpFiltroFechaIngresoFin").value.trim();
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let oParams = {
        nombreEmpleado: nombreField.length <= 0 ? null : nombreField,
        fechaIngresoInicio: fechaInicioField.length <= 0 ? null : fechaInicioField,
        fechaIngresoFin: fechaFinField.length <= 0 ? null : fechaFinField
    };

    //Resetea el valor de los filtros.
    document.querySelectorAll("#filtros .form-control").forEach(function (e) { e.value = ""; });
    document.querySelectorAll("#filtros .form-select").forEach(function (e) { e.value = 0; });

    doAjax(
        "/Reportes/Asistencia/FiltrarAsistencia",
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

//Función para obtener el archivo de un input
function getFile(inputId) {
    let fileField = document.getElementById(inputId),
        file = null,
        tipoArchivo = fileField.getAttribute("tipoArchivoId"),
        b64 = fileField.getAttribute("b64") || "",
        sourceId = fileField.getAttribute("sourceId") || "",
        actualizar = parseInt(fileField.getAttribute("actualizar") || "0")

    file = fileField != null ? fileField.files : null;
    if (file) { file = file.length > 0 ? file[0] : null; }

    let oFile = {
        id: sourceId,
        tipoArchivoId: tipoArchivo,
        imgSrc: b64,
        nombre: "",
        extension: ""
    }

    if (file) {
        //Si se estableció archivo en pantalla, crea el json con el archivo.
        let fileParts = (file.name || "").split(".");
        oFile.nombre = fileParts.length >= 1 ? fileParts.slice(0, -1).join(".") : "";
        oFile.extension = fileParts.length >= 2 ? fileParts[fileParts.length - 1] : "";
    }

    if (actualizar) { return oFile; }

    return null;
}
//Función para el importado del archivo con información de empresas
function onImportarClick() {
    //Ejecuta la validación
    $("#importForm").validate();
    //Determina los errores
    let valid = $("#importForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let form = new FormData();
    let btnClose = document.getElementById("dlgExcelBtnCancelar");
    let dlgTitle = document.getElementById("dlgExcelTitle");
    let fileField = document.getElementById("excelFile");
    fileField = fileField != null ? fileField.files : null;

    if (fileField) { fileField = fileField.length > 0 ? fileField[0] : null; }

    if (fileField) { form.append("plantilla", fileField); }

    let extendedOptions = {
        headers: postOptions.headers,
        data: form,
        contentType: false,
        processData: false
    }

    doAjax(
        "/Reportes/Asistencia/ImportarAsistencias",
        {},
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

            onBuscarClick();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        },
        function (error) {
            showError("Error", error);
        },
        extendedOptions
    );
}
//Función para el cierre del cuadro de diálogo
function onCerrarImportarClick() {
    let fileField = document.getElementById("excelFile");
    fileField.value = null;
    onCerrarClick();
}
//Función para procesar el cambio de archivo a exportar
function onExcelSelectorChanged(input) {
    //Validación para seleccionar archivos excel solamente.
    if (input.files && (input.files.length || 0) >= 1) {
        let docType = input.files[0].type;
        let isExcel = docType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || docType == "application/vnd.ms-excel";

        if (!isExcel) {
            input.value = null;
            showAlert(invalidFormatTitle, invalidFormatMsg);
        }
    }
}

function onGuardarClick() {
    // Ejecuta la validación
    $("#theForm").validate();
    // Determina los errores
    let valid = $("#theForm").valid();
    // Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let idField = document.getElementById("inpAsistenciaId");
    let btnClose = document.getElementById("dlgAsistenciaBtnCancelar");
    let resultadoEField = document.getElementById("inpAsistenciaResultadoE");
    let resultadoSField = document.getElementById("inpAsistenciaResultadoS");
    let dlgTitle = document.getElementById("dlgAsistenciaTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let oParams = {
        id: idField.value === "Nuevo" ? 0 : parseInt(idField.value, 10),
        resultadoE: resultadoEField.value,
        resultadoS: resultadoSField.value // Corregido: debe ser 'resultadoS'
    };

    doAjax(
        "/Reportes/Asistencia/SaveAsistencia",
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

