var table;
var buttonRemove;
var selections = [];
var dlgEmpresa = null;
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
    dlgEmpresa = document.getElementById('dlgEmpresa');
    dlgEmpresaModal = new bootstrap.Modal(dlgEmpresa, null);
    //Función para limpiar el cuadro de diálogo cuando es cerrado
    dlgEmpresa.addEventListener('hidden.bs.modal', function (event) {
        onCerrarClick();
    });
    //Función para ejecutar acciones posteriores al mostrado del diálogo.
    dlgEmpresa.addEventListener('shown.bs.modal', function (e) {
        //Este evento es necesario para poder mostrar el text area ajustado al tamaño del contenido, basado en el tamaño del scroll.
        calculateTextAreaHeight(document.querySelectorAll("textarea"));
    })

    initTable();

    let btnBuscar = document.getElementById("btnBuscar");
    btnBuscar.click();
});
//Función para redimensionar el campo de objeto social cada que cambie el tamaño de pantalla.
window.addEventListener('resize', function (event) {
    calculateTextAreaHeight(document.querySelectorAll("textarea"));
}, true);

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
        origenId: 0,
        nivelId: 0,
        fechaConstitucion: null,
        fechaInicioOperacion: null,
        fechaInicioFacturacion: null,
        fechaInicioAsimilados: null,
        rfc: "",
        domicilioFiscal: "",
        administrador: "",
        accionista: "",
        correoGeneral: "",
        correoBancos: "",
        correoFiscal: "",
        correoFacturacion: "",
        telefono: "",
        actividadEconomicaId: 0,
        objetoSocial: "",
        bancos: [],
        archivos: []
    };

    for (let a in arrTiposDocumentos) {
        oEmpresaNueva.archivos.push({
            nombre: "",
            tipoArchivoId: a,
            extension: "",
            imgSrc: "",
            htmlContainer: "",
            fileSize: 0
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
                title: colActividadEconomicaHeader,
                field: "actividadEconomica",
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
    buttonRemove.click(function () { onDeleteEmpresaClick(selections); });
}
//Función para capturar el click de los botones para dar de baja empresas. Ejecuta una llamada ajax para dar de baja empresas.
function onDeleteEmpresaClick(ids = null) {
    askConfirmation(dlgDeleteTitle, dlgDeleteQuestion, function () {
        let oParams = {};

        if (ids != null) { oParams.ids = ids; }
        else { oParams.ids = [document.getElementById("inpEmpresaId").value]; }

        doAjax(
            "/Catalogos/Empresas/DisableEmpresas",
            oParams,
            function (resp) {
                if (resp.tieneError) {
                    showError(dlgDeleteTitle, resp.mensaje);
                    return;
                }

                table.bootstrapTable('remove', {
                    field: 'id',
                    values: oParams.ids
                });

                if (ids != null) {
                    ids = [];
                    selections = null;
                    buttonRemove.prop('disabled', true);
                }

                onBuscarClick();

                showSuccess(dlgDeleteTitle, resp.mensaje);
            }, function (error) {
                showError(dlgDeleteTitle, error);
            },
            postOptions
        );

    });
}
////////////////////////////////

//Funcionalidad Filtrar

//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let btnBuscar = document.getElementById("btnBuscar");
    let selOrigen = document.getElementById("selFiltroOrigen");
    let selNivel = document.getElementById("selFiltroNivel");
    let selActividadEconomica = document.getElementById("selFiltroActividadEconomica");

    let oParams = {
        origen: selOrigen.value,
        nivel: selNivel.value,
        actividadEconomica: selActividadEconomica.value
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
    $("#theForm .accordion-button").removeClass('collapsed');
    $("#theForm .accordion-button").attr('aria-expanded', true);
    $("#theForm .accordion-collapse").addClass('show');

    let summaryContainer = document.getElementById("saveValidationSummary");
    let idField = document.getElementById("inpEmpresaId");
    let razonSocialField = document.getElementById("inpEmpresaRazonSocial");
    let origenField = document.getElementById("selEmpresaOrigen");
    let nivelField = document.getElementById("selEmpresaNivel");
    let fechaConstitucionField = document.getElementById("inpEmpresaFechaConstitucion");
    let fechaInicioOperacionField = document.getElementById("inpEmpresaFechaInicioOperacion");
    let fechaInicioFacturacionField = document.getElementById("inpEmpresaFechaInicioFacturacion");
    let fechaInicioAsimiladosField = document.getElementById("inpEmpresaFechaInicioAsimilados");
    let rfcField = document.getElementById("inpEmpresaRFC");
    let domicilioFiscalField = document.getElementById("txtEmpresaDomicilioFiscal");
    let administradorField = document.getElementById("inpEmpresaAdministrador");
    let accionistaField = document.getElementById("inpEmpresaAccionista");
    let correoGeneralField = document.getElementById("inpEmpresaCorreoGeneral");
    let correoBancosField = document.getElementById("inpEmpresaCorreoBancos");
    let correoFiscalField = document.getElementById("inpEmpresaCorreoFiscal");
    let correoFacturacionField = document.getElementById("inpEmpresaCorreoFacturacion");
    let telefonoField = document.getElementById("inpEmpresaTelefono");
    let actividadEconomicaField = document.getElementById("selEmpresaActividadEconomica");
    let objetoSocialField = document.getElementById("inpEmpresaObjetoSocial");

    summaryContainer.innerHTML = "";
    idField.setAttribute("disabled", true);
    idField.value = row.id;
    razonSocialField.value = row.razonSocial;
    origenField.value = row.origenId;
    nivelField.value = row.nivelId;
    fechaConstitucionField.value = row.fechaConstitucionJS;
    fechaInicioOperacionField.value = row.fechaInicioOperacionJS;
    fechaInicioFacturacionField.value = row.fechaInicioFacturacionJS;
    fechaInicioAsimiladosField.value = row.fechaInicioAsimiladosJS;
    rfcField.value = row.rfc;
    domicilioFiscalField.value = row.domicilioFiscal;
    administradorField.value = row.administrador;
    accionistaField.value = row.accionista;
    correoGeneralField.value = row.correoGeneral;
    correoBancosField.value = row.correoBancos;
    correoFiscalField.value = row.correoFiscal;
    correoFacturacionField.value = row.correoFacturacion;
    telefonoField.value = row.telefono;
    actividadEconomicaField.value = row.actividadEconomicaId;
    objetoSocialField.value = row.objetoSocial;

    if (action == NUEVO || (row.hasDatosAdicionales || false)) {
        establecerDatosAdicionales(row, action);
        initializeDisableableButtons(false);
        prepareForm(action);
        dlgEmpresaModal.toggle();
        return;
    }

    doAjax(
        "/Catalogos/Empresas/DatosAdicionalesEmpresa",
        {idEmpresa: row.id},
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

            //Se establece el row con los datos adicionales.
            if (typeof resp.datos == "string" && resp.datos.length >= 1) { resp.datos = JSON.parse(resp.datos); };
            row.bancos = resp.datos.bancos || [];
            row.archivos = resp.datos.archivos || [];
            row.hasDatosAdicionales = true;

            //Actualiza el row para no tener que volver a obtener los datos la próxima vez.
            table.bootstrapTable('updateByUniqueId', { id: row.id, row: row });

            establecerDatosAdicionales(row, action);
            initializeDisableableButtons(action == VER);
            prepareForm(action);
            dlgEmpresaModal.toggle();

        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}
//Función para hablitar / deshabilitar el formulario de empresa
function prepareForm(action) {
    let btnDesactivar = document.getElementById("dlgEmpresaBtnDesactivar");
    let dlgTitle = document.getElementById("dlgEmpresaTitle");
    
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
}
//Función para establecer los datos adicionales de la empresa
function establecerDatosAdicionales(row, action) {
    //Se establecen los bancos
    $("#bodyBancos").html("");
    row.bancos = row.bancos || [];
    row.bancos.forEach(function (b) { onAgregarBancoClick(b) });

    //Se establecen los archivos.
    $("#bodyArchivos").html("");
    row.archivos = row.archivos || [];
    let i = 1;
    row.archivos.forEach(function (a) {
        let containerClass = "document-container-empty";
        let iconClass = "opacity-25";
        let nameClass = "opacity-25";
        let nameHTML = `<div class="overflowed-text">${emptySelectItemText}</div>`;
        let editDisabled = action == VER ? "disabled" : "";
        let itemVerHTML = "";

        if (parseInt(a.fileSize) >= 1) {
            //Si el tamaño del archivo es mayor o igual a 1 byte, agrega un archivo al DOM con la información.
            containerClass = "document-container-filled";
            iconClass = "document-icon-filled";
            nameClass = "document-name-filled";
            nameHTML = `<div class="overflowed-text">${a.nombre}</div>.<div>${a.extension}</div>`;
            itemVerHTML = `<li><a class='dropdown-item' onclick='onVerDocumentClick(this);' inputName="selector${a.tipoArchivoId}"><i class='bi bi-search'></i> ${btnVerTitle}</a></li>`;
        }

        $("#bodyArchivos").append(
            `<div class="col-12 col-xl-6">
                <div><b>${arrTiposDocumentos[a.tipoArchivoId]}</b></div>
                <div id="container${a.tipoArchivoId}" class="alert mb-2 mt-2 ${containerClass} row me-0">
                    <div id="fileIcon${a.tipoArchivoId}" class="align-self-center col-1 ${iconClass}"><i class='bi bi-file-image' style='font-size:25px'></i></div>
                    <div id="fileName${a.tipoArchivoId}" class="align-self-center col-10 ${nameClass} p-2" style="display:flex; color:dimgray">${nameHTML}</div>
                    <div class="align-self-center col-1">
                        <input type="file" id="selector${a.tipoArchivoId}" sourceId="${a.id}" sourceName="${a.nombre}.${a.extension}" sourceLength="${a.fileSize}" tipoArchivoId="${a.tipoArchivoId}" containerName="container${a.tipoArchivoId}" fileIconName="fileIcon${a.tipoArchivoId}" fileNameName="fileName${a.tipoArchivoId}" onchange="onDocumentSelectorChanged(this);" accept="image/png, image/jpeg, application/pdf" hidden />
                        <div class="dropdown">
                            <button class="btn" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="bi bi-three-dots-vertical success"></i>
                            </button>
                            <ul class="dropdown-menu">
                                ${itemVerHTML}
                                <li><a class='dropdown-item ${editDisabled}' onclick='onEditDocumentClick(this);' inputName="selector${a.tipoArchivoId}"><i class='bi bi-pencil-fill'></i> ${btnEditarTitle}</a></li>
                                <li><a class="dropdown-item disableable" inputName="selector${a.tipoArchivoId}" onclick="onDeleteClick(this);" sourceId="selector${a.tipoArchivoId}" sourceLength="${a.fileSize}"><i class="bi bi-x-lg"></i> ${btnEliminarTitle}</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>`
        );

        i++;
    });
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
            let sourceLength = (input.getAttribute("b64") || "").length;
            if (sourceLength <= 0) { sourceLength = parseInt(input.getAttribute("sourceLength") || "0"); }
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
//Función para capturar el clic en el botón de ver, que dispara la visualización del archivo.
function onVerDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    let input = document.getElementById(inputName);
    let oParams = {id: input.getAttribute("sourceId")}

    window.open(`/FileViewer?id=${oParams.id}&module=empresas`, "_blank");
}
//Función para capturar el clic en el botón de edición, que dispara la apertura del selector de archivo.
function onEditDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    document.getElementById(inputName).click();
}
//Función para capturar el clic en el botón quitar, que elimina el archivo seleccionado.
function onDeleteClick(button) {
    let inputName = button.getAttribute("inputName") || "";

    if (inputName.length >= 1) {
        let fileInput = document.getElementById(inputName);
        let containerName = fileInput.getAttribute("containerName") || "";
        let fileIconName = fileInput.getAttribute("fileIconName");
        let fileNameName = fileInput.getAttribute("fileNameName");
        let container = document.getElementById(containerName);
        let fileIcon = document.getElementById(fileIconName);
        let fileName = document.getElementById(fileNameName);

        fileInput.value = null;
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
function onAgregarBancoClick(row = {banco: "", responsable: "", firmante: ""}) {
    let btnAgregarBanco = document.getElementById("dlgEmpresaBtnAgregarBanco");
    let currentRows = document.querySelectorAll(".rowBancos").length;

    if (currentRows >= maxBanks) {
        showAlert(btnAgregarBanco.innerHTML, dlgMaxBancosAllowedMessage);
        return;
    }

    currentRows += 1;
    
    let bodyBancos = document.getElementById("bodyBancos");
    bodyBancos.innerHTML += `<div class="card mb-3 shadow">
                                <span class="text-end mt-2" data-effect="fadeOut">
                                    <button type="button" class="btn-close formButton" onclick="onEliminarBancoClick(this);"></button>
                                </span>
                                <div rownumber="${currentRows}" class="col-sm-12 col-md-12 col-lg-12 rowBancos">
								    <div class="row">
									    <h6 class="col-12"><i>${empresaBancoTitle} ${currentRows}</i></h6>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4">
										    <div class="form-floating mb-3">
											    <input id="inpEmpresaBancoNombre${currentRows}" type="text" class="form-control formInput" placeholder="${bancoNombrePlaceholder}" value="${row.banco}" maxlength="40" />
											    <label for="inpEmpresaBancoNombre${currentRows}" class="form-label">${bancoNombrePlaceholder}</label>
										    </div>
									    </div>
                                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4">
										    <div class="form-floating mb-3">
											    <input id="inpEmpresaBancoResponsable${currentRows}" type="text" class="form-control formInput" placeholder="${responsablePlaceholder}" value="${row.responsable}" maxlength="40" />
											    <label for="inpEmpresaBancoResponsable${currentRows}" class="form-label">${responsablePlaceholder}</label>
										    </div>
									    </div>
									    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-4">
										    <div class="form-floating mb-3">
											    <input id="inpEmpresaBancoFirmante${currentRows}" type="text" class="form-control formInput" placeholder="${firmantePlaceholder}" value="${row.firmante}" maxlength="40" />
											    <label for="inpEmpresaBancoFirmante${currentRows}" class="form-label">${firmantePlaceholder}</label>
										    </div>
									    </div>
								    </div>
							    </div>
                            </div>`;
}
//Función para capturar el clic en el botón eliminar banco, que elimina un item de banco del listado.
function onEliminarBancoClick(button) {
    $(button).closest('.card').remove();
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
    let razonSocialField = document.getElementById("inpEmpresaRazonSocial");
    let origenField = document.getElementById("selEmpresaOrigen");
    let nivelField = document.getElementById("selEmpresaNivel");
    let fechaConstitucionField = document.getElementById("inpEmpresaFechaConstitucion");
    let fechaInicioOperacionField = document.getElementById("inpEmpresaFechaInicioOperacion");
    let fechaInicioFacturacionField = document.getElementById("inpEmpresaFechaInicioFacturacion");
    let fechaInicioAsimiladosField = document.getElementById("inpEmpresaFechaInicioAsimilados");
    let rfcField = document.getElementById("inpEmpresaRFC");
    let domicilioFiscalField = document.getElementById("txtEmpresaDomicilioFiscal");
    let administradorField = document.getElementById("inpEmpresaAdministrador");
    let accionistaField = document.getElementById("inpEmpresaAccionista");
    let correoGeneralField = document.getElementById("inpEmpresaCorreoGeneral");
    let correoBancosField = document.getElementById("inpEmpresaCorreoBancos");
    let correoFiscalField = document.getElementById("inpEmpresaCorreoFiscal");
    let correoFacturacionField = document.getElementById("inpEmpresaCorreoFacturacion");
    let telefonoField = document.getElementById("inpEmpresaTelefono");
    let actividadEconomicaField = document.getElementById("selEmpresaActividadEconomica");
    let objetoSocialField = document.getElementById("inpEmpresaObjetoSocial");

    let dlgTitle = document.getElementById("dlgEmpresaTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    let banks = [];
    $("#bodyBancos .rowBancos").each(function (i, b) {
        let row = b.getAttribute("rownumber");
        let bancoNombreField = document.getElementById(`inpEmpresaBancoNombre${row}`);
        let bancoResponsableField = document.getElementById(`inpEmpresaBancoResponsable${row}`);
        let bancoFirmanteField = document.getElementById(`inpEmpresaBancoFirmante${row}`);
        
        banks.push({
            banco: bancoNombreField.value.trim(),
            responsable: bancoResponsableField.value.trim(),
            firmante: bancoFirmanteField.value.trim(),
        });
    });

    let files = [];
    $("#bodyArchivos input").each(function (i, a) {
        let id = a.getAttribute("id");
        let file = getFile(id);
        if (file) { files.push(file); }
    });

    let oParams = {
        id: idField.value == nuevoRegistro ? 0 : idField.value,
        razonSocial: razonSocialField.value.trim(),
        origenId: origenField.value == 0 ? null : parseInt(origenField.value),
        nivelId: nivelField.value == 0 ? null : parseInt(origenField.value),
        fechaConstitucion: fechaConstitucionField.value,
        fechaInicioOperacion: fechaInicioOperacionField.value,
        fechaInicioFacturacion: fechaInicioFacturacionField.value,
        fechaInicioAsimilados: fechaInicioAsimiladosField.value,
        rfc: rfcField.value.trim(),
        domicilioFiscal: domicilioFiscalField.value.trim(),
        administrador: administradorField.value.trim(),
        accionista: accionistaField.value.trim(),
        correoGeneral: correoGeneralField.value.trim(),
        correoBancos: correoBancosField.value.trim(),
        correoFiscal: correoFiscalField.value.trim(),
        correoFacturacion: correoFacturacionField.value.trim(),
        telefono: telefonoField.value.trim(),
        actividadEconomicaId: actividadEconomicaField.value == 0 ? null : parseInt(actividadEconomicaField.value),
        objetoSocial: objetoSocialField.value.trim(),
        bancos: banks,
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
        sourceLength = parseInt(fileField.getAttribute("sourceLength") || "0"),
        sourceId = fileField.getAttribute("sourceId")||"";

    file = fileField != null ? fileField.files : null;
    if (file) { file = file.length > 0 ? file[0] : null; }

    
    if (file) {
        //Si se estableció archivo en pantalla, crea el json con el archivo.
        let fileParts = (file.name||"").split(".");
        return {
            id: sourceId,
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
            id: sourceId,
            nombre: fileParts.length >= 1 ? fileParts[0] : "",
            tipoArchivoId: tipoArchivo,
            extension: fileParts.length >= 2 ? fileParts[1] : "",
            imgSrc: b64
        };
    }
    else {
        //De lo contrario, devuelve un objeto de archvio vacío, indicando solamente el tipo de archvio.
        return {
            id: "",
            nombre: "",
            tipoArchivoId: tipoArchivo,
            extension: "",
            imgSrc: ""
        };
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