var table;
var buttonRemove;
var selections = [];
var dlgEmpleado = null;
var dlgEmpleadoModal = null;

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

    let btnBuscar = document.getElementById("btnBuscar");
    btnBuscar.addEventListener("click", onBuscarClick);
});

function responseHandler(res) {
    if (typeof res === "string" && res.length >= 1) {
        res = JSON.parse(res);
    }
    $.each(res, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1;
    });
    return res;
}
//Función para dar formato al detalle de empleado
function detailFormatter(index, row) {
    return `<div class="container-fluid alert alert-primary mb-0">
                <div class="row">
                    <div class="col-sm-12 col-md-6 col-lg-3 mb-2">
						<i class="bi bi-cake2-fill"></i> <span><b>${colFechaNacimientoHeader}: </b>${row.fechaNacimiento}</span>
					</div>
					<div class="col-sm-12 col-md-6 col-lg-3 mb-2">
						<i class="bi bi-telephone-fill"> </i><span><b>${colTelefonoHeader}: </b>${row.telefono}</span>
					</div>
					<div class="col-sm-12 col-md-6 col-lg-3 mb-2">
						<i class="bi bi-building-fill"></i> <span><b>${colOficinaHeader}: </b>${row.oficina}</span>
					</div>
					<div class="col-sm-12 col-md-6 col-lg-3 mb-2">
						<i class="bi bi-yin-yang"></i> <span><b>${colEstadoCivilHeader}: </b>${row.estadoCivil}</span>
					</div>
					<div class="col-12 mb-2">
						<i class="bi bi-house-door-fill"> </i><span><b>${colDireccionHeader}: </b>${row.direccion}</span>
					</div>
                </div>
            </div>`;
}

//Función para dar formato a los iconos de operación de los registros
function operateFormatter(value, row, index) {
    let icons = [];
    //¿El empleado tiene id de usuario?
    let hasUserId = (row.usuarioId || "").length >= 1;
    //El empleado si tiene usuario, pero ¿ese usuario es válido?
    let hasUsuarioValido = parseInt((row.usuarioValido || "0")) >= 1;
    //Si el empleado no tiene id de usuario o tiene un usuario inválido, entonces se habilita la opción invitar.
    let canInvite = !hasUserId || !hasUsuarioValido;

    //Icono Ver
    icons.push(`<li><a class="dropdown-item see" href="#" title="${btnVerTitle}"><i class="bi bi-search"></i> ${btnVerTitle}</a></li>`);
    //Icono Editar
    icons.push(`<li><a class="dropdown-item edit" href="#" title="${btnEditarTitle}"><i class="bi bi-pencil-fill"></i> ${btnEditarTitle}</a></li>`);

    if (canInvite) {
        //Icono Invitar
        icons.push(`<li><a class="dropdown-item invite" href="#" title="${btnInvitarTitle}"><i class="bi bi-person-fill-add"></i> ${btnInvitarTitle}</a></li>`);
    }

    return `<div class="dropdown">
              <button class="btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                <i class="bi bi-three-dots-vertical success"></i>
              </button>
              <ul class="dropdown-menu">${icons.join("")}</ul>
            </div>`;
}

//Eventos de los iconos de operación
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initEmpleadoDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initEmpleadoDialog(EDITAR, row);
    },
    'click .invite': function (e, value, row, index) {
        invitarEmpleado(row.id);
    }
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
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel'],
        toolbar: '#toolbar', // Asegúrate de que este ID coincida con el elemento HTML donde quieres que aparezcan los botones
        buttons: additionalButtons, // Asegúrate de que `additionalButtons` esté siendo llamado correctamente aquí
        columns: [{
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
            field: "Día",
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
            field: "Resultado",
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
            field: "Resultado",
            align: "center",
            valign: "middle",
            sortable: true
        }
        ]
    });
}

// Función para manejar el click en el botón de búsqueda
function onBuscarClick() {
    let nombreField = document.getElementById("inpFiltroNombreEmpleado").value.trim();
    let fechaInicioField = document.getElementById("inpFiltroFechaIngresoInicio").value.trim();
    let fechaFinField = document.getElementById("inpFiltroFechaIngresoFin").value.trim();
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let oParams = {
        nombreEmpleado: nombreField,
        fechaIngresoInicio: fechaInicioField,
        fechaIngresoFin: fechaFinField
    };

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
        },
        function (error) {
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
        "/Reportes/Asistencia/ImportarAsistencia",
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
// Función para realizar la llamada AJAX
function doAjax(url, data, successCallback, errorCallback, options) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json",
        headers: options.headers,
        success: function (response) {
            if (successCallback) successCallback(response);
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", error);
            if (errorCallback) errorCallback(error);
        }
    });
}
