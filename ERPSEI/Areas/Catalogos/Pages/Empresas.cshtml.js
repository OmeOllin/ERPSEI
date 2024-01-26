var table;
var buttonRemove;
var selections = [];
var dlgEmpresaModal = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const maxFileSizeInBytes = 5242880; //5mb = (5 * 1024) * 1024
const oneMegabyteSizeInBytes = 1048576; // 1mb = (1 * 1024) * 1024
const maxBanks = 5; // Máximo 5 registros de bancos.
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

//Función para inicializar el módulo.
document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");
    dlgEmpresaModal = new bootstrap.Modal(document.getElementById('dlgEmpresa'), null);

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
//Función para dar formato al detalle de empresa
function detailFormatter(index, row) {
    let h = `<div class="container alert alert-primary">
                <div class="row">
                    
                </div>
            </div>`;
    return h;
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
//Eventos de los iconos de operación
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initEmpresaDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initEmpresaDialog(EDITAR, row);
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
//Función para agregar empresas
function onAgregarClick() {
    let oEmpresaNueva = {
        id: nuevoRegistro,
        razonSocial: "",
        rfc: "",
        origen: "",
        nivel: "",
        administrador: "",
        accionista: "",
        domicilioFiscal: "",
        correoGeneral: "",
        correoBancos: "",
        correoFiscal: "",
        telefono: "",
        fechaInicioOperacion: null,
        bancos: [],
        archivos: []
    };

    for (let a in arrTiposDocumentos) {
        oEmpresaNueva.archivos.push({
            nombre: "",
            tipoArchivoId: a,
            extension: "",
            imgSrc: "",
            htmlContainer: ""
        });
    }

    initEmpresaDialog(NUEVO, oEmpresaNueva);
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
                field: "razonSocial",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colOrigenHeader,
                field: "origen",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colNivelHeader,
                field: "nivel",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colAdministradorHeader,
                field: "administrador",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colAccionistaHeader,
                field: "accionista",
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
                "/Catalogos/Empresas/DeleteEmpresa",
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
    let inpOrigen = document.getElementById("inpFiltroOrigen");
    let inpNivel = document.getElementById("inpFiltroNivel");
    let inpAdministrador = document.getElementById("inpFiltroAdministrador");
    let inpAccionista = document.getElementById("inpFiltroAccionista");

    let oParams = {
        origen: inpOrigen.value,
        nivel: inpNivel.value,
        administrador: inpAdministrador.value,
        accionista: inpAccionista.value
    };

    //Resetea el valor de los filtros.
    document.querySelectorAll("#filtros .form-control").forEach(function (e) { e.value = ""; });
    document.querySelectorAll("#filtros .form-select").forEach(function (e) { e.value = 0; });

    doAjax(
        "/Catalogos/Empresas/FiltrarEmpresas",
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
//Funcionalidad Diálogo Empresa
////////////////////////////////
//Función para inicializar el cuadro de diálogo
function initEmpresaDialog(action, row) {
    let idField = document.getElementById("inpEmpresaId");
    let razonSocialField = document.getElementById("inpEmpresaRazonSocial");
    let rfcField = document.getElementById("inpEmpresaRFC");
    let origenField = document.getElementById("inpEmpresaOrigen");
    let nivelField = document.getElementById("inpEmpresaNivel");
    let administradorField = document.getElementById("inpEmpresaAdministrador");
    let accionistaField = document.getElementById("inpEmpresaAccionista");
    let domicilioFiscalField = document.getElementById("txtEmpresaDomicilioFiscal");
    let correoGeneralField = document.getElementById("inpEmpresaCorreoGeneral");
    let correoBancosField = document.getElementById("inpEmpresaCorreoBancos");
    let correoFiscalField = document.getElementById("inpEmpresaCorreoFiscal");
    let telefonoField = document.getElementById("inpEmpresaTelefono");
    let fechaInicioOperacionField = document.getElementById("inpEmpresaFechaInicioOperacion");

    let btnDesactivar = document.getElementById("dlgEmpresaBtnDesactivar");
    let dlgTitle = document.getElementById("dlgEmpresaTitle");
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
    razonSocialField.value = row.razonSocial;
    rfcField.value = row.rfc;
    origenField.value = row.origen;
    nivelField.value = row.nivel;
    administradorField.value = row.administrador;
    accionistaField.value = row.accionista;
    domicilioFiscalField.value = row.domicilioFiscal;
    correoGeneralField.value = row.correoGeneral;
    correoBancosField.value = row.correoBancos;
    correoFiscalField.value = row.correoFiscal;
    telefonoField.value = row.telefono;
    fechaInicioOperacionField.value = row.fechaInicioOperacionJS;

    $("#bodyBancos").html("");
    row.bancos = row.bancos || [];
    let j = 1;
    row.bancos.forEach(function (b) {
        $("#bodyBancos").append(
            `<div class="col-sm-12 col-md-12 col-lg-6 rowBancos">
				<div class="row">
					<h6 class="col-12"><i>${empresaBancoTitle} ${j}</i></h6>
					<div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
						<div class="form-floating mb-3">
							<input id="inpEmpresaBancoTitular${j}" type="text" class="form-control formInput" placeholder="${titularPlaceholder}" />
							<label for="inpEmpresaBancoTitular${j}" class="form-label">${titularPlaceholder}</label>
							<span class="text-danger"></span>
						</div>
					</div>
					<div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
						<div class="form-floating mb-3">
							<input id="inpEmpresaBancoResponsable${j}" type="text" class="form-control formInput" placeholder="${responsablePlaceholder}" />
							<label for="inpEmpresaBancoResponsable${j}" class="form-label">${responsablePlaceholder}</label>
							<span class="text-danger"></span>
						</div>
					</div>
				</div>
			</div>`
        );

        j++;
    });

    $("#bodyArchivos").html("");
    row.archivos = row.archivos || [];
    let i = 1;
    row.archivos.forEach(function (a) {
        let srcElements = (a.imgSrc || "").split(",");
        let b64 = srcElements.length >= 1 ? srcElements[1]||"" : "";

        let containerClass = "document-container-empty";
        let iconClass = "opacity-25";
        let nameClass = "opacity-25";
        let nameHTML = `<div class="overflowed-text">${emptySelectItemText}</div>`;
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

        i++;
    });

    initializeDisableableButtons(action == VER);
    dlgEmpleadoModal.toggle();
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
        fileName.innerHTML = `<div class="overflowed-text">${emptySelectItemText}</div>`;
    }

    initializeDisableableButtons();
}
//Función para capturar el clic en el botón agregar banco, que añade un item de banco para capturar datos.
function onAgregarBancoClick() {
    let btnAgregarBanco = document.getElementById("dlgEmpresaBtnAgregarBanco");
    let currentRows = document.querySelectorAll(".rowBancos").length;

    if (currentRows >= maxBanks) {
        showAlert(btnAgregarBanco.innerHTML, dlgMaxBancosAllowedMessage);
        return;
    }

    currentRows += 1;
    
    let bodyBancos = document.getElementById("bodyBancos");
    bodyBancos.innerHTML += `<div class="col-sm-12 col-md-12 col-lg-6 rowBancos">
								<div class="row">
									<h6 class="col-12"><i>${empresaBancoTitle} ${currentRows}</i></h6>
									<div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
										<div class="form-floating mb-3">
											<input id="inpEmpresaBancoTitular${currentRows}" type="text" class="form-control formInput" placeholder="${titularPlaceholder}" />
											<label for="inpEmpresaBancoTitular${currentRows}" class="form-label">${titularPlaceholder}</label>
											<span class="text-danger"></span>
										</div>
									</div>
									<div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
										<div class="form-floating mb-3">
											<input id="inpEmpresaBancoResponsable${currentRows}" type="text" class="form-control formInput" placeholder="${responsablePlaceholder}" />
											<label for="inpEmpresaBancoResponsable${currentRows}" class="form-label">${responsablePlaceholder}</label>
											<span class="text-danger"></span>
										</div>
									</div>
								</div>
							</div>`;
}

//Función para mostrar cualquiera de los documentos seleccionados.
function onDocumentSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            input.value = null;
            showAlert(maxFileSizeTitle, `${maxFileSizeMessage} ${maxFileSizeInBytes / oneMegabyteSizeInBytes}Mb`);
            return;
        }
        let docType = input.files[0].type;
        let docParts = input.files[0].name.split(".");
        let fName = docParts.length >= 1 ? docParts[0] || "" : "";
        let fExt = docParts.length >= 2 ? docParts[1] || "" : "";
        let isImg = docType == "image/png" || docType == "image/jpg" || docType == "image/jpeg";
        let isPDF = docType == "application/pdf";
        let containerName = input.getAttribute("containerName");
        let fileIconName = input.getAttribute("fileIconName");
        let fileNameName = input.getAttribute("fileNameName");
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
            showAlert(fileFormatTitle, fileFormatMessage);

            return;
        }
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
//Función para el guardado de información de la empresa
function onGuardarClick() {
    //Ejecuta la validación
    $("#theForm").validate();
    //Determina los errores
    let valid = $("#theForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let btnClose = document.getElementById("dlgEmpresaBtnCancelar");

    let idField = document.getElementById("inpEmpresaId");
    let rfcField = document.getElementById("inpEmpresaRFC");

    let dlgTitle = document.getElementById("dlgEmpresaTitle");
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
        id: idField.value == nuevoRegistro ? 0 : idField.value,
        rfc: rfcField.value.trim(),
        archivos: files
    };

    doAjax(
        "/Catalogos/Empresas/SaveEmpresa",
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
        "/Catalogos/Empresas/ImportarEmpresas",
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

        if(!isExcel){
            input.value = null;
            showAlert(invalidFormatTitle, invalidFormatMsg);
        }
    }
}
////////////////////////////////