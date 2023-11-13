var table;
var buttonRemove;
var selections = [];
const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener("DOMContentLoaded", function (event) {
    initializeDate();

    table = $("#table");
    buttonRemove = $("#remove");

    initTable();
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
    })
    return res
}
function detailFormatter(index, row) {
    var html = []
    $.each(row, function (key, value) {
        if (key != "state" && key != "empleados") {
            html.push('<p><b>' + key + ':</b> ' + value + '</p>')
        }
    });
    return html.join('')
}
function operateFormatter(value, row, index) {
    return [
        '<a class="see btn" href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#dlgEmpleado" title="' + btnVerTitle + '">',
            '<i class="bi bi-search"></i>',
        '</a>  ',
        '<a class="edit btn" href="javascript:void(0)" data-bs-toggle="modal" data-bs-target="#dlgEmpleado" title="' + btnEditarTitle + '">',
            '<i class="bi bi-pencil-fill"></i>',
        '</a>'
    ].join('')
}
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initEmpleadoDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initEmpleadoDialog(EDITAR, row);
        //table.bootstrapTable('remove', {
        //    field: 'id',
        //    values: [row.id]
        //})
    }
}
function additionalButtons() {
    return {
        btnImport: {
            text: 'Importar',
            icon: 'bi-upload',
            event: function () { },
            attributes: {
                "title": 'Importar datos desde un archivo excel',
                "data-bs-toggle": "modal",
                "data-bs-target": "#dlgImportarExcel"
            }
        }
    }
}


function onAgregarClick() {
    initEmpleadoDialog(NUEVO, {
        id: "Nuevo",
        primerNombre: "",
        segundoNombre: "",
        apellidoPaterno: "",
        apellidoMaterno: "",
        fechaNacimiento: null,
        fechaIngreso: null,
        direccion: "",
        telefono: "",
        email: "",
        generoId: 0,
        estadoCivilId: 0,
        puestoId: 0,
        areaId: 0,
        subareaId: 0,
        oficinaId: 0,
        jefeId: 0,
        nombreContacto1: "",
        telefonoContacto1: "",
        nombreContacto2: "",
        telefonoContacto2: ""
    });
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
                field: "nombreCompleto",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colFechaIngresoHeader,
                field: "fechaIngreso",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colPuestoHeader,
                field: "puesto",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colAreaHeader,
                field: "area",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colTelefonoHeader,
                field: "telefono",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colCorreoHeader,
                field: "email",
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
                "/Catalogos/GestionDeTalento/DeleteEmpleados",
                oParams,
                function (resp) {
                    if (resp.tieneError) {
                        showError("Error", error);
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
                    showError("Error", error);
                },
                postOptions
            );

        });
    })
}
/////////////////////

//Funcionalidad Diálogo Empleado
function initEmpleadoDialog(action, row) {
    let idField = document.getElementById("inpEmpleadoId");
    let primerNombreField = document.getElementById("inpEmpleadoPrimerNombre");
    let apellidoPaternoField = document.getElementById("inpEmpleadoApellidoPaterno");
    let apellidoMaternoField = document.getElementById("inpEmpleadoApellidoMaterno");
    let fechaNacimientoField = document.getElementById("inpEmpleadoFechaNacimiento");
    let telefonoField = document.getElementById("inpEmpleadoTelefono");
    let generoField = document.getElementById("selEmpleadoGeneroId");
    let estadoCivilIdField = document.getElementById("selEmpleadoEstadoCivilId");
    let direccionField = document.getElementById("txtEmpleadoDireccion");
    let puestoField = document.getElementById("selEmpleadoPuestoId");
    let areaField = document.getElementById("selEmpleadoAreaId");
    let subareaField = document.getElementById("selEmpleadoSubareaId");
    let oficinaField = document.getElementById("selEmpleadoOficinaId");
    let jefeField = document.getElementById("selEmpleadoJefeId");
    let fechaIngresoField = document.getElementById("inpEmpleadoFechaIngreso");
    let emailField = document.getElementById("inpEmpleadoEmail");
    let nombreContacto1Field = document.getElementById("inpEmpleadoNombreContacto1");
    let telefonoContacto1Field = document.getElementById("inpEmpleadoTelefonoContacto1");
    let nombreContacto2Field = document.getElementById("inpEmpleadoNombreContacto2");
    let telefonoContacto2Field = document.getElementById("inpEmpleadoTelefonoContacto2");

    let btnDesactivar = document.getElementById("dlgEmpleadoBtnDesactivar");
    let btnGuardar = document.getElementById("dlgEmpleadoBtnGuardar");
    let dlgTitle = document.getElementById("dlgEmpleadoTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    idField.setAttribute("disabled", true);
    btnDesactivar.hidden = true;
    switch (action) {
        case NUEVO:
        case EDITAR:
            if (action == NUEVO) {
                dlgTitle.innerHTML = dlgNuevoTitle;
            }
            else {
                btnDesactivar.hidden = false;
                dlgTitle.innerHTML = dlgEditarTitle;
            }

            primerNombreField.removeAttribute("disabled");
            apellidoPaternoField.removeAttribute("disabled");
            apellidoMaternoField.removeAttribute("disabled");
            fechaNacimientoField.removeAttribute("disabled");
            telefonoField.removeAttribute("disabled");
            generoField.removeAttribute("disabled");
            estadoCivilIdField.removeAttribute("disabled");
            direccionField.removeAttribute("disabled");
            puestoField.removeAttribute("disabled");
            areaField.removeAttribute("disabled");
            subareaField.removeAttribute("disabled");
            oficinaField.removeAttribute("disabled");
            jefeField.removeAttribute("disabled");
            fechaIngresoField.removeAttribute("disabled");
            emailField.removeAttribute("disabled");
            nombreContacto1Field.removeAttribute("disabled");
            telefonoContacto1Field.removeAttribute("disabled");
            nombreContacto2Field.removeAttribute("disabled");
            telefonoContacto2Field.removeAttribute("disabled");
            btnGuardar.removeAttribute("disabled");
            break;
        default:
            dlgTitle.innerHTML = dlgVerTitle;

            primerNombreField.setAttribute("disabled", true);
            apellidoPaternoField.setAttribute("disabled", true);
            apellidoMaternoField.setAttribute("disabled", true);
            fechaNacimientoField.setAttribute("disabled", true);
            telefonoField.setAttribute("disabled", true);
            generoField.setAttribute("disabled", true);
            estadoCivilIdField.setAttribute("disabled", true);
            direccionField.setAttribute("disabled", true);
            puestoField.setAttribute("disabled", true);
            areaField.setAttribute("disabled", true);
            subareaField.setAttribute("disabled", true);
            oficinaField.setAttribute("disabled", true);
            jefeField.setAttribute("disabled", true);
            fechaIngresoField.setAttribute("disabled", true);
            emailField.setAttribute("disabled", true);
            nombreContacto1Field.setAttribute("disabled", true);
            telefonoContacto1Field.setAttribute("disabled", true);
            nombreContacto2Field.setAttribute("disabled", true);
            telefonoContacto2Field.setAttribute("disabled", true);
            btnGuardar.setAttribute("disabled", true);
            break;
    }

    idField.value = row.id;
    primerNombreField.value = row.primerNombre;
    apellidoPaternoField.value = row.apellidoPaterno;
    apellidoMaternoField.value = row.apellidoMaterno;
    fechaNacimientoField.value = row.fechaNacimientoJS;
    telefonoField.value = row.telefono;
    generoField.value = row.generoId;
    estadoCivilIdField.value = row.estadoCivilId;
    direccionField.value = row.direccion;
    puestoField.value = row.puestoId;
    areaField.value = row.areaId;
    subareaField.value = row.subareaId;
    oficinaField.value = row.oficinaId;
    jefeField.value = row.jefeId;
    fechaIngresoField.value = row.fechaIngresoJS;
    emailField.value = row.email;

    row.contactosEmergencia = row.contactosEmergencia || [];
    if (row.contactosEmergencia.length >= 1) {
        nombreContacto1Field.value = row.contactosEmergencia[0].nombre || "";
        telefonoContacto1Field.value = row.contactosEmergencia[0].telefono || "";
    }
    if (row.contactosEmergencia.length >= 2) {
        nombreContacto2Field.value = row.contactosEmergencia[1].nombre || "";
        telefonoContacto2Field.value = row.contactosEmergencia[1].telefono || "";
    }

    $("#bodyArchivos").html("");
    row.archivos = row.archivos || [];
    let i = 1;
    row.archivos.forEach(function (a) {
        //Obtiene el nombre del tipo.
        $("#bodyArchivos").append(
            `<tr valign="middle" align="center">
			    <th scope="row">${a.tipoArchivoId}</th>
			    <td><b>${arrTiposDocumentos[a.tipoArchivoId]}</b></td>
			    <td>
				    <div id="container${i}" class="document-container">
					    ${a.archivo}
				    </div>
			    </td>
			    <td align="left">
				    <div style="width:auto;">
					    <input type="file" id="selector${i}" containerName="container${i}" onchange="onDocumentSelectorChanged(this);" accept="image/png, image/jpeg, application/pdf" hidden />
					    <a class="btn mb-1" onclick="onEditDocumentClick(this);" inputName="selector${i}">
						    <i class="bi bi-pencil-fill"></i>
					    </a>
					    <a class="btn disableable mb-1" asp-page="/FileViewer" asp-route-id="${a.id}" target="_blank" sourceLength="${a.archivo.length}">
						    <i class="bi bi-search"></i>
					    </a>
					    <a class="btn disableable mb-1" onclick="onDeleteClick(this);" sourceId="selector${i}" sourceLength="${a.archivo.length}">
						    <i class="bi bi-x-lg"></i>
					    </a>
				    </div>
			    </td>
		    </tr>`
        );
        i++;
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
function onGuardarClick() {
    //Ejecuta la validación
    $("#theForm").validate();
    //Determina los errores
    let valid = $("#theForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let btnClose = document.getElementById("dlgEmpleadoBtnCancelar");

    let idField = document.getElementById("inpEmpleadoId");
    let primerNombreField = document.getElementById("inpEmpleadoPrimerNombre");
    let segundoNombreField = document.getElementById("inpEmpleadoSegundoNombre");
    let apellidoPaternoField = document.getElementById("inpEmpleadoApellidoPaterno");
    let apellidoMaternoField = document.getElementById("inpEmpleadoApellidoMaterno");
    let fechaNacimientoField = document.getElementById("inpEmpleadoFechaNacimiento");
    let telefonoField = document.getElementById("inpEmpleadoTelefono");
    let generoField = document.getElementById("selEmpleadoGeneroId");
    let estadoCivilIdField = document.getElementById("selEmpleadoEstadoCivilId");
    let direccionField = document.getElementById("txtEmpleadoDireccion");
    let puestoField = document.getElementById("selEmpleadoPuestoId");
    let areaField = document.getElementById("selEmpleadoAreaId");
    let subareaField = document.getElementById("selEmpleadoSubareaId");
    let oficinaField = document.getElementById("selEmpleadoOficinaId");
    let jefeField = document.getElementById("selEmpleadoJefeId");
    let fechaIngresoField = document.getElementById("inpEmpleadoFechaIngreso");
    let emailField = document.getElementById("inpEmpleadoEmail");
    let nombreContacto1Field = document.getElementById("inpEmpleadoNombreContacto1");
    let telefonoContacto1Field = document.getElementById("inpEmpleadoTelefonoContacto1");
    let nombreContacto2Field = document.getElementById("inpEmpleadoNombreContacto2");
    let telefonoContacto2Field = document.getElementById("inpEmpleadoTelefonoContacto2");

    let dlgTitle = document.getElementById("dlgEmpleadoTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        primerNombre: primerNombreField.value,
        segundoNombre: segundoNombreField.value,
        apellidoPaterno: apellidoPaternoField.value,
        apellidoMaterno: apellidoMaternoField.value,
        fechaNacimiento: fechaNacimientoField.value,
        telefono: telefonoField.value,
        generoId: generoField.value == 0 ? null : parseInt(generoField.value),
        estadoCivilId: estadoCivilIdField.value == 0 ? null : parseInt(estadoCivilIdField.value),
        direccion: direccionField.value,
        puestoId: puestoField.value == 0 ? null : parseInt(puestoField.value),
        areaId: areaField.value == 0 ? null : parseInt(areaField.value),
        subareaId: subareaField.value == 0 ? null : parseInt(subareaField.value),
        oficinaId: oficinaField.value == 0 ? null : parseInt(oficinaField.value),
        jefeId: jefeField.value == 0 ? null : parseInt(jefeField.value),
        fechaIngreso: fechaIngresoField.value,
        email: emailField.value,
        nombreContacto1: nombreContacto1Field.value,
        telefonoContacto1: telefonoContacto1Field.value,
        nombreContacto2: nombreContacto2Field.value,
        telefonoContacto2: telefonoContacto2Field.value
    };

    doAjax(
        "/Catalogos/GestionDeTalento/SaveEmpleado",
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
/////////////////////
function initializeDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    let monthStart = now.getFullYear() + "-" + (month) + "-" + "01";
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    document.getElementById("inpFiltroFechaIngresoInicio").setAttribute("value", monthStart);
    document.getElementById("inpFiltroFechaIngresoFin").setAttribute("value", today);
}

//Funcionalidad Diálogo importar
function onImportarClick() {
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
        "/Catalogos/GestionDeTalento/ImportarEmpleados",
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
                return;
            }

            btnClose.click();

            let e = document.querySelector("[name='refresh']");
            e.click();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        },
        function (error) {
            showError("Error", error);
        },
        extendedOptions
    );
}
function onCerrarImportarClick() {
    let fileField = document.getElementById("excelFile");
    fileField.value = null;
}
function onExcelSelectorChanged(input) {
    //Validación para seleccionar archivos excel solamente.
    if (input.files && (input.files.length || 0) >= 1) {
        let docType = input.files[0].type;
        let isExcel = docType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || docType == "application/vnd.ms-excel";

        if(!isExcel){
            input.value = null;
            showAlert(invalidFormatTitle, invalidFormatMsg);
        }
    }
}
/////////////////////