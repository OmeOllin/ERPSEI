var table;
var buttonRemove;
var selections = [];
const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
//5mb = (5 * 1024) * 1024;
var maxFileSizeInBytes = 5242880;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

//Función para inicializar el módulo.
document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");

    initTable();

    let btnBuscar = document.getElementById("btnBuscar");
    btnBuscar.click();
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
    if (typeof res == "string" && res.length >= 1) {
        res = JSON.parse(res);
    }
    $.each(res, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1
    });



    return res
}
//Función para dar formato al detalle de empleado
function detailFormatter(index, row) {
    var html = []
    $.each(row, function (key, value) {
        if (key != "state" && key != "empleados") {
            html.push('<p><b>' + key + ':</b> ' + value + '</p>')
        }
    });
    return html.join('')
}
//Función para dar formato a los iconos de operación de los registros
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
//Eventos de los iconos de operación
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
//Función para añadir botones a la cinta de botones de la tabla
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
//Función para agregar empleados
function onAgregarClick() {
    let oEmpleadoNuevo = {
        id: "Nuevo",
        nombre: "",
        nombrePreferido: "",
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
        curp: "",
        rfc: "",
        nss: "",
        nombreContacto1: "",
        telefonoContacto1: "",
        nombreContacto2: "",
        telefonoContacto2: "",
        contactosEmergencia: [],
        archivos: []
    };

    for (let a in arrTiposDocumentos) {
        oEmpleadoNuevo.archivos.push({
            nombre: "",
            tipoArchivoId: a,
            extension: "",
            imgSrc: "",
            htmlContainer: ""

        });
    }

    initEmpleadoDialog(NUEVO, oEmpleadoNuevo);
}
//Función para inicializar la tabla
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
                        showError(dlgDeleteTitle, resp.mensaje);
                        return;
                    }

                    table.bootstrapTable('remove', {
                        field: 'id',
                        values: selections
                    })
                    selections = [];
                    buttonRemove.prop('disabled', true);

                    onBuscarClick();

                    showSuccess(dlgDeleteTitle, resp.mensaje);
                }, function (error) {
                    showError(dlgDeleteTitle, error);
                },
                postOptions
            );

        });
    })
}
////////////////////////////////

//Funcionalidad Filtrar

//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let btnBuscar = document.getElementById("btnBuscar");
    let inpFechaIngresoIni = document.getElementById("inpFiltroFechaIngresoInicio");
    let inpFechaIngresoFin = document.getElementById("inpFiltroFechaIngresoFin");
    let inpFechaNacimientoIni = document.getElementById("inpFiltroFechaNacimientoInicio");
    let inpFechaNacimientoFin = document.getElementById("inpFiltroFechaNacimientoFin");
    let selPuesto = document.getElementById("selFiltroPuesto");
    let selArea = document.getElementById("selFiltroArea");
    let selSubarea = document.getElementById("selFiltroSubarea");
    let selOficina = document.getElementById("selFiltroOficina");

    let oParams = {
        FechaIngresoInicio: inpFechaIngresoIni.value,
        FechaIngresoFin: inpFechaIngresoFin.value,
        FechaNacimientoInicio: inpFechaNacimientoIni.value,
        FechaNacimientoFin: inpFechaNacimientoFin.value,
        PuestoId: selPuesto.value == 0 ? null : parseInt(selPuesto.value),
        AreaId: selArea.value == 0 ? null : parseInt(selArea.value),
        SubareaId: selSubarea.value == 0 ? null : parseInt(selSubarea.value),
        OficinaId: selOficina.value == 0 ? null : parseInt(selOficina.value)
    };

    //Resetea el valor de los filtros.
    document.querySelectorAll("#filtros .form-control").forEach(function (e) { e.value = ""; });
    document.querySelectorAll("#filtros .form-select").forEach(function (e) { e.value = 0; });

    doAjax(
        "/Catalogos/GestionDeTalento/FiltrarEmpleados",
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
////////////////////////////////

////////////////////////////////
//Funcionalidad Diálogo Empleado
////////////////////////////////
//Función para inicializar el cuadro de diálogo
function initEmpleadoDialog(action, row) {
    let idField = document.getElementById("inpEmpleadoId");
    let picField = document.getElementById("profilePicContainer");
    let picSelector = document.getElementById("profilePicSelector");
    let primerNombreField = document.getElementById("inpEmpleadoPrimerNombre");
    let nombrePreferidoField = document.getElementById("inpEmpleadoNombrePreferido");
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
    let curpField = document.getElementById("inpEmpleadoCURP");
    let rfcField = document.getElementById("inpEmpleadoRFC");
    let nssField = document.getElementById("inpEmpleadoNSS");
    let nombreContacto1Field = document.getElementById("inpEmpleadoNombreContacto1");
    let telefonoContacto1Field = document.getElementById("inpEmpleadoTelefonoContacto1");
    let nombreContacto2Field = document.getElementById("inpEmpleadoNombreContacto2");
    let telefonoContacto2Field = document.getElementById("inpEmpleadoTelefonoContacto2");

    let btnDesactivar = document.getElementById("dlgEmpleadoBtnDesactivar");
    let dlgTitle = document.getElementById("dlgEmpleadoTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    idField.setAttribute("disabled", true);
    switch (action) {
        case NUEVO:
        case EDITAR:
            if (action == NUEVO) {
                btnDesactivar.hidden = true;
                dlgTitle.innerHTML = dlgNuevoTitle;
            }
            else {
                btnDesactivar.hidden = false;
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

    idField.value = row.id;
    primerNombreField.value = row.nombre;
    nombrePreferidoField.value = row.nombrePreferido;
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
    curpField.value = row.curp;
    rfcField.value = row.rfc;
    nssField.value = row.nss;

    row.contactosEmergencia = row.contactosEmergencia || [];
    nombreContacto1Field.value = "";
    telefonoContacto1Field.value = "";
    nombreContacto2Field.value = "";
    telefonoContacto2Field.value = "";
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
        let srcElements = (a.imgSrc || "").split(",");
        let b64 = srcElements.length >= 1 ? srcElements[1]||"" : "";

        if (a.tipoArchivoId == 1) {
            //Si el tipo de archivo es la foto de perfil, se establece en el contenedor directamente.
            if ((a.imgSrc||"").length <= 0) { a.imgSrc = "/img/default_profile_pic.png"; }
            picField.setAttribute('src', a.imgSrc);
            picSelector.setAttribute('sourceLength', a.imgSrc.length);
            picSelector.setAttribute('sourceName', `${a.nombre}.${a.extension}`);
            picSelector.setAttribute('b64', b64);
        }
        else {
            let containerClass = "document-container-empty";
            let iconClass = "opacity-25";
            let nameClass = "opacity-25";
            let nameHTML = `<div class="overflowed-text">Seleccione...</div>`;
            let editDisabled = action == VER ? "disabled" : "";

            if (b64.length >= 1) {
                //Si trae base64, agrega un archivo al DOM con la información.
                containerClass = "document-container-filled";
                iconClass = "document-icon-filled";
                nameClass = "document-name-filled";
                nameHTML = `<div class="overflowed-text">${a.nombre}</div>.<div>${a.extension}</div>`;
            }

            $("#bodyArchivos").append(
                `<div class="col-12 col-xl-6">
                    <div><b>${arrTiposDocumentos[a.tipoArchivoId]}</b></div>
                    <div id="container${a.tipoArchivoId}" class="alert mb-2 mt-2 ${containerClass} row me-0">
                        <div id="fileIcon${a.tipoArchivoId}" class="align-self-center col-1 ${iconClass}"><i class='bi bi-file-image' style='font-size:25px'></i></div>
                        <div id="fileName${a.tipoArchivoId}" class="align-self-center col-10 ${nameClass} p-2" style="display:flex; color:dimgray">${nameHTML}</div>
                        <div class="align-self-center col-1">
                            <input type="file" id="selector${a.tipoArchivoId}" b64="${b64}" sourceName="${a.nombre}.${a.extension}" sourceLength="${(b64||"").length}" tipoArchivoId="${a.tipoArchivoId}" containerName="container${a.tipoArchivoId}" fileIconName="fileIcon${a.tipoArchivoId}" fileNameName="fileName${a.tipoArchivoId}" onchange="onDocumentSelectorChanged(this);" accept="image/png, image/jpeg, application/pdf" hidden />
                            <a class='btn btn-sm btn-primary ${editDisabled} mb-1' onclick='onEditDocumentClick(this);' inputName="selector${a.tipoArchivoId}"><i class='bi bi-pencil-fill'></i></a>
                            <a class="btn btn-sm btn-primary disableable mb-1" inputName="selector${a.tipoArchivoId}" onclick="onDeleteClick(this);" sourceId="selector${a.tipoArchivoId}" sourceLength="${(a.archivo || []).length}"><i class="bi bi-x-lg"></i></a>
                        </div>
                    </div>
                </div>`
            );
        }
        i++;
    });

    initializeDisableableButtons(action == VER);
}
//Función para habilitar/deshabilitar los botones de visualización en base a si existe contenido o no para visualizar.
function initializeDisableableButtons(isConsulta = false) {
    //Botones de acción de editar y eliminar
    let buttons = document.getElementsByClassName("disableable");

    //Se inicializan los botones de edición
    if (buttons.length >= 1) {
        for (let i = 0; i < buttons.length; i++) {
            let button = buttons[i];
            let inputName = button.getAttribute("inputName");
            let input = document.getElementById(inputName);
            let sourceLength = (input.getAttribute("b64")||"").length;
            let hasFile = sourceLength >= 1;
            if (hasFile && !isConsulta) {
                button.classList.remove("disabled");
            }
            else {
                button.classList.add("disabled");
            }
        }
    }
}
//Función para capturar el clic en el botón de edición, que dispara la apertura del selector de archivo.
function onEditDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    document.getElementById(inputName).click();
}
//Función para capturar el clic en el botón quitar, que elimina el archivo seleccionado.
function onDeleteClick(button) {
    let sourceId = button.getAttribute("sourceId") || "";

    if (sourceId.length >= 1) {
        let fileInput = document.getElementById(sourceId);
        let containerName = fileInput.getAttribute("containerName") || "";
        let fileIconName = fileInput.getAttribute("fileIconName");
        let fileNameName = fileInput.getAttribute("fileNameName");
        let container = document.getElementById(containerName);
        let fileIcon = document.getElementById(fileIconName);
        let fileName = document.getElementById(fileNameName);

        fileInput.files = null;
        fileInput.setAttribute("b64", "");

        container.classList.remove("document-container-filled");
        container.classList.add("document-container-empty");

        fileIcon.classList.remove("document-icon-filled");
        fileIcon.classList.add("opacity-25");

        fileName.classList.remove("document-name-filled");
        fileName.classList.add("opacity-25");
        fileName.innerHTML = `<div class="overflowed-text">Seleccione...</div>`;
    }

    initializeDisableableButtons();
}
//Función para mostrar la foto de perfil seleccionada.
function onProfilePicSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            input.value = null;
            showAlert(
                "Tama&ntilde;o de archivo inv&aacute;lido",
                `El tama&ntilde;o del archivo no debe superar ${maxFileSizeInBytes / 1000000}Mb. Por favor elija otro archivo.`
            );
            return;
        }
        let imgType = input.files[0].type;
        if (imgType == "image/png" || imgType == "image/jpg" || imgType == "image/jpeg") {
            document.getElementById("profilePicContainer").src = window.URL.createObjectURL(input.files[0]);
            var reader = new FileReader();
            reader.onload = function () {

                var arrayBuffer = this.result,
                    binary = '',
                    bytes = new Uint8Array(arrayBuffer),
                    len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                document.getElementById("profilePicSelector").setAttribute("b64", window.btoa(binary));
                document.getElementById("profilePicSelector").setAttribute("sourceLength", "0");
            }
            reader.readAsArrayBuffer(input.files[0]);
        }
        else {
            showAlert(
                "Formato de archivo inv&aacute;lido",
                `El formato del archivo no es permitido. Por favor elija archivos en formato png, jpg o jpeg.`
            );
        }
    }
}
//Función para mostrar cualquiera de los documentos seleccionados.
function onDocumentSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            input.value = null;
            showAlert(
                "Tama&ntilde;o de archivo inv&aacute;lido",
                `El tama&ntilde;o del archivo no debe superar ${maxFileSizeInBytes / 1000000}Mb. Por favor elija otro archivo.`
            );
            return;
        }
        let docType = input.files[0].type;
        let docParts = input.files[0].name.split(".");
        let fName = docParts.length >= 1 ? docParts[0] || "" : "";
        let fExt = docParts.length >= 2 ? docParts[1] || "" : "";
        /*let fileSize = (input.files[0].size / 1000000).toFixed(2);*/
        let isImg = docType == "image/png" || docType == "image/jpg" || docType == "image/jpeg";
        let isPDF = docType == "application/pdf";
        let containerName = input.getAttribute("containerName");
        let fileIconName = input.getAttribute("fileIconName");
        let fileNameName = input.getAttribute("fileNameName");
        /*let showerName = containerName + "_children";*/
        let container = document.getElementById(containerName);
        let fileIcon = document.getElementById(fileIconName);
        let fileName = document.getElementById(fileNameName);

        if (isImg || isPDF) {
            var reader = new FileReader();
            reader.onload = function () {

                var arrayBuffer = this.result,
                    binary = '',
                    bytes = new Uint8Array(arrayBuffer),
                    len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                input.setAttribute("b64", window.btoa(binary));
                input.setAttribute("sourceLength", "0");

                initializeDisableableButtons();
            }
            reader.readAsArrayBuffer(input.files[0]);

            container.classList.remove("document-container-empty");
            container.classList.add("document-container-filled");

            fileIcon.classList.remove("opacity-25");
            fileIcon.classList.add("document-icon-filled");

            fileName.classList.remove("opacity-25");
            fileName.classList.add("document-name-filled");
            fileName.innerHTML = `<div class="overflowed-text">${fName}</div>.<div>${fExt}</div>`;
        }
        else {
            input.value = null;
            showAlert(
                "Formato de archivo inv&aacute;lido",
                `El formato del archivo debe ser .pdf, .jpg, .jpeg o .png. Por favor elija otro archivo.`
            );

            return;
        }

        //if (isImg) {
        //    let src = window.URL.createObjectURL(input.files[0]);
        //    container.innerHTML = `<img id="${showerName}" src="${src}" style="max-height: 200px;" />`;
        //}
        //else if (isPDF) {
        //    showLoading();
        //    container.innerHTML = `<canvas id="${showerName}" class="canvaspdf"></canvas>`;
        //    await loadPDFFromFileAsync(input.files[0], showerName);
        //    hideLoading();
        //}
        //else {
        //    input.value = null;
        //    showAlert(
        //        "Formato de archivo inv&aacute;lido",
        //        `El formato del archivo debe ser .pdf, .jpg, .jpeg o .png. Por favor elija otro archivo.`
        //    );
        //}
    }
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
//Función para el guardado de información del empleado
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
    let nombrePreferidoField = document.getElementById("inpEmpleadoNombrePreferido");
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
    let curpField = document.getElementById("inpEmpleadoCURP");
    let rfcField = document.getElementById("inpEmpleadoRFC");
    let nssField = document.getElementById("inpEmpleadoNSS");
    let nombreContacto1Field = document.getElementById("inpEmpleadoNombreContacto1");
    let telefonoContacto1Field = document.getElementById("inpEmpleadoTelefonoContacto1");
    let nombreContacto2Field = document.getElementById("inpEmpleadoNombreContacto2");
    let telefonoContacto2Field = document.getElementById("inpEmpleadoTelefonoContacto2");

    let dlgTitle = document.getElementById("dlgEmpleadoTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let files = [];
    let profilePic = getFile("profilePicSelector");
    if (profilePic) { files.push(profilePic); }
    $("#bodyArchivos input").each(function (i, a) {
        let id = a.getAttribute("id");
        let file = getFile(id);
        if (file) { files.push(file); }
    });

    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        nombre: primerNombreField.value.trim(),
        nombrePreferido: nombrePreferidoField.value.trim(),
        apellidoPaterno: apellidoPaternoField.value.trim(),
        apellidoMaterno: apellidoMaternoField.value.trim(),
        fechaNacimiento: fechaNacimientoField.value,
        telefono: telefonoField.value.trim(),
        generoId: generoField.value == 0 ? null : parseInt(generoField.value),
        estadoCivilId: estadoCivilIdField.value == 0 ? null : parseInt(estadoCivilIdField.value),
        direccion: direccionField.value.trim(),
        puestoId: puestoField.value == 0 ? null : parseInt(puestoField.value),
        areaId: areaField.value == 0 ? null : parseInt(areaField.value),
        subareaId: subareaField.value == 0 ? null : parseInt(subareaField.value),
        oficinaId: oficinaField.value == 0 ? null : parseInt(oficinaField.value),
        jefeId: jefeField.value == 0 ? null : parseInt(jefeField.value),
        fechaIngreso: fechaIngresoField.value,
        email: emailField.value.trim(),
        curp: curpField.value.trim(),
        rfc: rfcField.value.trim(),
        nss: nssField.value.trim(),
        nombreContacto1: nombreContacto1Field.value.trim(),
        telefonoContacto1: telefonoContacto1Field.value.trim(),
        nombreContacto2: nombreContacto2Field.value.trim(),
        telefonoContacto2: telefonoContacto2Field.value.trim(),
        archivos: files
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
                showError(dlgTitle.innerHTML, resp.mensaje);
                return;
            }

            btnClose.click();

            onBuscarClick();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
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
        b64 = fileField.getAttribute("b64")||"",
        sourceLength = parseInt(fileField.getAttribute("sourceLength") || "0");

    file = fileField != null ? fileField.files : null;
    if (file) { file = file.length > 0 ? file[0] : null; }

    
    if (file) {
        //Si se estableció archivo en pantalla, crea el json con el archivo.
        let fileParts = (file.name||"").split(".");
        return {
            nombre: fileParts.length >= 1 ? fileParts[0] : "",
            tipoArchivoId: tipoArchivo,
            extension: fileParts.length >= 2 ? fileParts[1] : "",
            imgSrc: b64
        };
    }
    else if (sourceLength >= 1) {
        //De lo contrario, verifica si ya venía archivo guardado en base y construye el json con dichos datos.
        let fileParts = (fileField.getAttribute("sourceName") || "").split(".");
        return {
            nombre: fileParts.length >= 1 ? fileParts[0] : "",
            tipoArchivoId: tipoArchivo,
            extension: fileParts.length >= 2 ? fileParts[1] : "",
            imgSrc: b64
        };
    }
    else {
        //De lo contrario, devuelve un objeto de archvio vacío, indicando solamente el tipo de archvio.
        return {
            nombre: "",
            tipoArchivoId: tipoArchivo,
            extension: "",
            imgSrc: ""
        }
;
    }
}
////////////////////////////////

////////////////////////////////
//Funcionalidad Diálogo Importar
////////////////////////////////
//Función para el importado del archivo con información de empleados
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

        if(!isExcel){
            input.value = null;
            showAlert(invalidFormatTitle, invalidFormatMsg);
        }
    }
}
////////////////////////////////