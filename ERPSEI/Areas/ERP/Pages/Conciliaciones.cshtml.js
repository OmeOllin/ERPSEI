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
    //document.getElementById('fileUpload').addEventListener('change', onImportarMovimientosBancariosClick);

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

//Método para importar la información del excel y pdf
function onImportarMovimientosBancariosClick() {
    var fileUpload = document.getElementById('fileUpload');
    var selectedBank = $('#selFiltroBanco option:selected').text(); // Obtener el texto del banco seleccionado

    if (fileUpload.files.length === 0) {
        alert('Por favor selecciona un archivo.');
        return;
    }

    if (selectedBank === 'Seleccione...') {
        alert('Por favor selecciona un banco.');
        return;
    }

    var fileType = fileUpload.files[0].name.split('.').pop().toLowerCase(); // Obtener extensión del archivo

    if (fileType === 'xlsx' || fileType === 'xls') {
        importarMovimientosDesdeExcel(fileUpload.files[0], selectedBank);
    } else if (fileType === 'pdf') {
        importarMovimientosDesdePDF(fileUpload.files[0], selectedBank);
    } else {
        alert('Por favor selecciona un archivo Excel o PDF.');
    }
}

function importarMovimientosDesdeExcel(file, selectedBank) {
    var reader = new FileReader();
    reader.onload = function (e) {
        var data = new Uint8Array(e.target.result);
        var workbook = XLSX.read(data, { type: 'array' });

        // Leer la primera hoja del archivo Excel
        var firstSheet = workbook.Sheets[workbook.SheetNames[0]];
        var excelRows = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

        // Limpiar la tabla antes de insertar nuevos datos
        $('#tableCardMovimientos').bootstrapTable('removeAll');

        // Función para convertir los seriales de Excel a fechas en formato DD/MM/YYYY
        function excelDateToJSDate(serial) {
            var utc_days = Math.floor(serial - 25569); // Fecha base 1900
            var utc_value = utc_days * 86400; // Convertir días a segundos
            var date_info = new Date(utc_value * 1000); // Crear la fecha

            // Obtener día, mes y año
            var day = date_info.getUTCDate().toString().padStart(2, '0');
            var month = (date_info.getUTCMonth() + 1).toString().padStart(2, '0'); // Meses empiezan desde 0
            var year = date_info.getUTCFullYear();

            return `${day}/${month}/${year}`; // Formato DD/MM/YYYY
        }

        // Crear un array para almacenar las filas a insertar
        var rows = [];

        // Iterar sobre las filas del Excel, empezando desde la segunda fila
        for (var i = 1; i < excelRows.length; i++) {
            var row = excelRows[i]; // Obtener la fila actual

            // Verificar si el valor de la fecha es un número y convertirlo a fecha
            var fecha = row[0];
            if (!isNaN(fecha)) {
                fecha = excelDateToJSDate(fecha); // Convertir si es un número serial
            }

            // Agregar la fila al array de filas
            rows.push({
                Fecha: fecha || '',
                Banco: selectedBank, // Usar el valor seleccionado del banco
                Descripción: row[1] || '',
                Cargos: row[2] || '',
                Abonos: row[3] || ''
            });
        }

        // Agregar los datos a la tabla usando Bootstrap Table
        $('#tableCardMovimientos').bootstrapTable('append', rows);

        // Cerrar el modal después de que los datos se hayan agregado
        $('#ImportarMovimientosModal').modal('hide');
    };

    // Leer el archivo Excel
    reader.readAsArrayBuffer(file);
}

function importarMovimientosDesdePDF(file, selectedBank) {
    var reader = new FileReader();

    reader.onload = function (e) {
        var typedArray = new Uint8Array(e.target.result);

        // Utilizar pdfjs-dist para leer el archivo PDF
        pdfjsLib.getDocument(typedArray).promise.then(function (pdf) {
            var numPages = pdf.numPages;
            var extractedText = '';

            // Leer todas las páginas del PDF
            var promises = [];
            for (var i = 1; i <= numPages; i++) {
                promises.push(pdf.getPage(i).then(function (page) {
                    return page.getTextContent().then(function (textContent) {
                        textContent.items.forEach(function (item) {
                            extractedText += item.str + ' '; // Concatenar el texto extraído
                        });
                    });
                }));
            }

            // Esperar a que todas las páginas se hayan procesado
            Promise.all(promises).then(function () {
                // Detectar el banco en el texto extraído
                var bancoDetectado = detectarBanco(extractedText);

                // Comparar el banco detectado con el banco seleccionado
                if (bancoDetectado.toLowerCase() === selectedBank.toLowerCase()) {
                    alert(`Banco detectado y seleccionado: ${bancoDetectado}`);
                } else {
                    alert(`Banco detectado: ${bancoDetectado}, pero seleccionaste: ${selectedBank}. \nFavor de seleccionar el correcto.`);
                }

                // Aquí puedes continuar con el procesamiento del PDF si se detecta el banco.
                console.log(extractedText); // Mostrar el texto extraído para depuración
            });
        });
    };

    reader.readAsArrayBuffer(file); // Leer el archivo PDF como ArrayBuffer
}

function detectarBanco(extractedText) {
    // Diccionario de bancos y sus palabras clave
    var bancoKeywords = {
        "Banregio": ["BANREGIO", "BANCO REGIONAL", "Banregio"],
        "BBVA": ["BBVA", "BANCO BBVA"],
        "Alquimia": ["Alquimia", "ALQUIMIA", "Alquimia Digital", "alquimiapay"]
    };

    // Recorrer cada banco y sus palabras clave
    for (var banco in bancoKeywords) {
        var keywords = bancoKeywords[banco];
        // Comprobar si alguna de las palabras clave está en el texto extraído
        for (var i = 0; i < keywords.length; i++) {
            if (extractedText.toLowerCase().includes(keywords[i].toLowerCase())) {
                return banco; // Retorna el banco detectado
            }
        }
    }

    return "Banco no identificado"; // Retorna esto si no se detecta ningún banco
}