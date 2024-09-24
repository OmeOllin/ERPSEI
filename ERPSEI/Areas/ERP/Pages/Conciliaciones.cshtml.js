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
    buttonRemove = $("#remove");
    dlgConciliacion = document.getElementById('dlgConciliacion');
    dlgConciliacionModal = new bootstrap.Modal(dlgConciliacion, {});
    dlgConciliacion.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });
    initTable();
    // Asignar el evento de clic al botón de importación
    document.getElementById('fileUpload').addEventListener('change', onImportarMovimientosBancariosClick);

    autoCompletar("#inpConciliacionClienteId");
});

async function onImportarMovimientosBancariosClick(event) {
    const file = event.target.files[0];
    if (file) {
        try {
            // Verificar que el archivo sea un PDF o Excel
            const validTypes = ['application/pdf', 'application/vnd.ms-excel', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'];
            if (!validTypes.includes(file.type)) {
                alert('El archivo seleccionado no es un PDF ni un archivo de Excel.');
                return;
            }

            // Lógica adicional para manejar el archivo (lectura, procesamiento, etc.)
            console.log('Archivo válido seleccionado:', file.name);

        } catch (error) {
            console.error('Error al leer el archivo:', error);
        }
    }
}



/*async function onImportarMovimientosBancariosClick(event) {
    const file = event.target.files[0];
    if (file) {
        try {
            // Verificar que el archivo sea un PDF
            if (file.type !== 'application/pdf') {
                alert('El archivo seleccionado no es un PDF.');
                return;
            }

            // Leer el archivo PDF
            const reader = new FileReader();
            reader.onload = async function () {
                const typedArray = new Uint8Array(this.result);

                try {
                    // Cargar el PDF usando pdfjs-dist
                    const pdf = await pdfjsLib.getDocument({ data: typedArray }).promise;
                    const pageNumber = 33;

                    if (pageNumber <= pdf.numPages) {
                        const page = await pdf.getPage(pageNumber);
                        const textContent = await page.getTextContent();
                        const text = textContent.items.map(item => item.str).join(' ');

                        // Obtener el nombre del archivo PDF sin la extensión
                        const fileName = file.name.replace(/\.[^/.]+$/, "");
                        // Obtener la fecha actual en formato YYYYMMDD
                        const date = new Date();
                        const formattedDate = date.toISOString().split('T')[0].replace(/-/g, '');
                        // Crear el nombre del archivo Excel
                        const excelFileName = `${fileName}_${formattedDate}.xlsx`;

                        exportTextToExcel(text, excelFileName);

                    } else {
                        console.error(`El PDF solo tiene ${pdf.numPages} páginas.`);
                    }
                } catch (error) {
                    console.error('Error al procesar el archivo PDF:', error);
                }
            };
            reader.readAsArrayBuffer(file);
        } catch (error) {
            console.error('Error al leer el archivo PDF:', error);
        }
    }
}*/

/*
function exportTextToExcel(text, fileName) {
    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.aoa_to_sheet([[text]]);
    XLSX.utils.book_append_sheet(wb, ws, "Datos del PDF");
    // Usar el nombre de archivo proporcionado
    XLSX.writeFile(wb, fileName);
}*/

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
        initConciliacionDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initConciliacionDialog(EDITAR, row);
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
        toolbar: '#toolbar',
        buttons: additionalButtons,
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
    });

    // Aquí gestionamos la selección y deselección de registros
    table.on('check.bs.table uncheck.bs.table ' +
        'check-all.bs.table uncheck-all.bs.table', function () {

            // Obtener si hay registros seleccionados
            const hasSelections = table.bootstrapTable('getSelections').length > 0;

            // Habilitar o deshabilitar el botón en base a las selecciones
        $('#dlgConsultarBtnGuardar').prop('disabled', !hasSelections);

            // Guardar selecciones
            selections = getIdSelections();

            // Deshabilitar o habilitar el botón de eliminación
            buttonRemove.prop('disabled', !hasSelections);
        });

    // **Verificar si hay selecciones al iniciar**
    const initialSelections = table.bootstrapTable('getSelections').length > 0;
    $('#dlgConsultarBtnGuardar').prop('disabled', !initialSelections);
    buttonRemove.prop('disabled', !initialSelections);


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
                    });
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
    });
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
    let fechaField = document.getElementById("inpConciliacionFecha");
    let clienteIdField = document.getElementById("inpConciliacionClienteId");
    let descripcionField = document.getElementById("inpConciliacionDescripcion");
    let dlgTitle = document.getElementById("dlgConciliacionTitle");

    //Botones
    let btnGuardar = document.getElementById("dlgConciliacionBtnGuardar");
    let btnCancelar = document.getElementById("dlgConciliacionBtnCancelar");
    let btnComprobantesFechas = document.getElementById("dlgConciliacionBtnFechas");
    let btnBtnMovimientosImportar = document.getElementById("dlgConciliacionBtnMovimientos");
    //let btnComprobantes = document.getElementById("dlgConciliacionBtnGuardar");
    let summaryContainer = document.getElementById("saveValidationSummary");

    // Limpiar los mensajes de validación
    summaryContainer.innerHTML = "";

    // Configuración según la acción
    switch (action) {
        case NUEVO:
            dlgTitle.innerHTML = dlgNuevoTitle;

            idField.setAttribute("disabled", true);
            fechaField.removeAttribute("disabled");
            clienteIdField.removeAttribute("disabled");
            descripcionField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        case EDITAR:
            dlgTitle.innerHTML = dlgEditarTitle;

            fechaField.removeAttribute("disabled");
            clienteIdField.removeAttribute("disabled");
            descripcionField.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        case VER:
            dlgTitle.innerHTML = dlgVerTitle;

            idField.setAttribute("disabled", true);
            fechaField.setAttribute("disabled", true);
            clienteIdField.setAttribute("disabled", true);
            descripcionField.setAttribute("disabled", true);

            //btnComprobantesFechas.setAttribute("disabled", true);
            //btnBtnMovimientosImportar.setAttribute("disabled", true);
            btnGuardar.setAttribute("disabled", true);
            break;
    }

    // Asignar valores a los campos del diálogo usando los valores de la entidad Conciliacion
    idField.value = row.id || "";
    fechaField.value = row.fecha || "";
    clienteIdField.value = row.clienteId || "";
    descripcionField.value = row.descripcion || "";

    // Mostrar el modal
    dlgConciliacionModal.show(); 
}
function onGuardarConciliacionClick() {
    // Ejecuta la validación del formulario con el id "theForm"
    $("#theForm").validate();
    // Determina si la validación fue exitosa
    let valid = $("#theForm").valid();
    // Si el formulario no es válido, termina la ejecución
    if (!valid) { return; }

    let btnClose = document.getElementById("dlgConciliacionBtnCancelar");
    let fechaField = document.getElementById("inpConciliacionFecha");
    let clienteIdField = document.getElementById("inpConciliacionClienteId");
    let descripcionField = document.getElementById("inpConciliacionDescripcion");
    let idField = document.getElementById("inpConciliacionId"); // Añadido: Obtener el idField
    let dlgTitle = document.getElementById("dlgConciliacionTitle");
    let summaryContainer = document.getElementById("saveValidationSummaryConciliacion");
    summaryContainer.innerHTML = "";

    // Parámetros que se enviarán en la solicitud
    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        descripcion: descripcionField.value,
        fecha: fechaField.value,
        clienteId: clienteIdField.value
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

//Funciones para el cardview del lado izquierdo del principal modal
function actionFormatter(value, row, index) {
    return `
        <button class="btn btn-primary btn-sm" onclick="eliminarRegistro(${row.id})">
            <i class="bi bi-paperclip rotate-clip"></i> Conciliar/Reconciliar
        </button>
    `;
}
function eliminarRegistro(id) {
    if (confirm('¿Estás seguro de eliminar el registro con ID: ' + id + '?')) {
        alert('Registro eliminado con éxito.');
        // Lógica para eliminar el registro
    }
}



