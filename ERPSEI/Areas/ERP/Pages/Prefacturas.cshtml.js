const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

document.addEventListener('DOMContentLoaded', function () {
    autoCompletar("#inpEmisor", {
        select: function (element, item) { toggleEmisorInfo(item); },
        change: function (element, item) {
            let inpEmisor = document.getElementById('inpEmisor');
            if ((inpEmisor.value || "").length <= 0) { toggleEmisorInfo(); }
        }
    });
    autoCompletar("#inpReceptor", {
        select: function (element, item) { toggleReceptorInfo(item); },
        change: function (element, item) {
            let inpReceptor = document.getElementById('inpReceptor');
            if ((inpReceptor.value || "").length <= 0) { toggleReceptorInfo(); }
        }
    });
    autoCompletar("#inpProductoServicioEmisor", {
        select: function (element, item) {
            let btnAdd = document.getElementById("dlgBtnAgregarProductoServicioEmisor");
            btnAdd.classList.remove("disabled");
            let prodServSelectorField = document.getElementById("inpProductoServicioEmisor");
            prodServSelectorField.classList.remove("is-invalid");
            prodServSelectorField.classList.remove("is-valid");
        },
        change: function (element, item) {
            let prodServSelectorField = document.getElementById("inpProductoServicioEmisor");
            if (parseInt(prodServSelectorField.getAttribute("idselected") || "0") <= 0) {
                let btnAdd = document.getElementById("dlgBtnAgregarProductoServicioEmisor");
                btnAdd.classList.add("disabled");
            }
            prodServSelectorField.classList.remove("is-invalid");
            prodServSelectorField.classList.remove("is-valid");
        }
    });
});

//Función para mostrar u ocultar la información del emisor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleEmisorInfo(item = null) {
    let divFilledInfoEmisor = document.getElementById("divFilledInfoEmisor");
    let divEmptyInfoEmisor = document.getElementById("divEmptyInfoEmisor");

    let lblRFC = document.getElementById("lblRFCEmisor");
    let lblRazonSocial = document.getElementById("lblRazonSocialEmisor");
    let lblOrigen = document.getElementById("lblOrigenEmisor");
    let lblNivel = document.getElementById("lblNivelEmisor");
    let lblPerfil = document.getElementById("lblPerfilEmisor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaEmisor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalEmisor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialEmisor");
    let listaDOM = document.getElementById("listProductosServiciosEmisor");

    listaDOM.innerHTML = "";
    listaDOM.classList.remove("form-control");
    listaDOM.classList.remove("is-invalid");
    listaDOM.classList.remove("is-valid");

    lblNivel.classList.remove("is-invalid");
    lblNivel.classList.remove("is-valid");

    lblActividadEconomica.classList.remove("is-invalid");
    lblActividadEconomica.classList.remove("is-valid");

    lblPerfil.classList.remove("is-invalid");
    lblPerfil.classList.remove("is-valid");

    lblDomicilioFiscal.classList.remove("is-invalid");
    lblDomicilioFiscal.classList.remove("is-valid");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblOrigen.innerHTML = item.origen;
        lblNivel.innerHTML = item.nivel.nombre;
        lblPerfil.innerHTML = item.perfil;

        let listaActividades = [];
        item.actividadesEconomicas.forEach(function (a) {
            listaActividades.push(`<li id="${a.id}" clave="${a.clave}" class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                          <div class="fw-bold">${a.clave}</div>
                                          ${a.nombre}
                                        </div>
                                    </div>
								  </li>`);
        });
        lblActividadEconomica.innerHTML = `<ul class="list-group list-group-flush mt-3">${listaActividades.join("")}</ul>`;

        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        let osParts = item.objetoSocial.split("\n");
        let listParts = [];
        osParts.forEach(function (o) {
            if (o.length >= 1) {
                listParts.push(`<li class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                            <p style="text-align: justify">${o.trim()}</p>
                                        </div>
                                    </div>
				                </li>`);
            }
        });
        lblObjetoSocial.innerHTML = `<ul class="list-group list-group-flush mt-3">${listParts.join("")}</ul>`;

        divEmptyInfoEmisor.classList.remove('d-flex');
        divEmptyInfoEmisor.setAttribute('hidden', true);
        divFilledInfoEmisor.removeAttribute('hidden');

        $("#inpReceptor").data('idempresa', item.id);
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblOrigen.innerHTML = emptyInfo;
        lblNivel.innerHTML = emptyInfo;
        lblPerfil.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;

        divEmptyInfoEmisor.removeAttribute('hidden');
        divFilledInfoEmisor.setAttribute('hidden', true);
        divEmptyInfoEmisor.classList.add('d-flex');

        $("#inpReceptor").data('idempresa', null);

        $("#inpEmisor").data("id", null);
        $("#inpEmisor").data("nivel", null);
        $("#inpEmisor").data("actividadesEconomicas", null);
        $("#inpEmisor").data("perfil", null);
        $("#inpEmisor").data("direccion", null);
        $("#inpEmisor").data("productosServicios", null);
    }

}

//Función para mostrar u ocultar la información del receptor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleReceptorInfo(item = null) {
    let divFilledInfoReceptor = document.getElementById("divFilledInfoReceptor");
    let divEmptyInfoReceptor = document.getElementById("divEmptyInfoReceptor");

    let lblRFC = document.getElementById("lblRFCReceptor");
    let lblRazonSocial = document.getElementById("lblRazonSocialReceptor");
    let lblOrigen = document.getElementById("lblOrigenReceptor");
    let lblNivel = document.getElementById("lblNivelReceptor");
    let lblPerfil = document.getElementById("lblPerfilReceptor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaReceptor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalReceptor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialReceptor");

    lblNivel.classList.remove("is-invalid");
    lblNivel.classList.remove("is-valid");

    lblActividadEconomica.classList.remove("is-invalid");
    lblActividadEconomica.classList.remove("is-valid");

    lblPerfil.classList.remove("is-invalid");
    lblPerfil.classList.remove("is-valid");

    lblDomicilioFiscal.classList.remove("is-invalid");
    lblDomicilioFiscal.classList.remove("is-valid");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblOrigen.innerHTML = item.origen;
        lblNivel.innerHTML = item.nivel.nombre;
        lblPerfil.innerHTML = item.perfil;

        let listaActividades = [];
        item.actividadesEconomicas.forEach(function (a) {
            listaActividades.push(`<li id="${a.id}" clave="${a.clave}" class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                          <div class="fw-bold">${a.clave}</div>
                                          ${a.nombre}
                                        </div>
                                    </div>
								  </li>`);
        });
        lblActividadEconomica.innerHTML = `<ul class="list-group list-group-flush mt-3">${listaActividades.join("")}</ul>`;

        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        let osParts = item.objetoSocial.split("\n");
        let listParts = [];
        osParts.forEach(function (o) {
            if (o.length >= 1) {
                listParts.push(`<li class="list-group-item border-bottom-0 bg-transparent">
                                    <div class="row">
                                        <div class="col-12">
                                            <p style="text-align: justify">${o.trim()}</p>
                                        </div>
                                    </div>
				                </li>`);
            }
        });
        lblObjetoSocial.innerHTML = `<ul class="list-group list-group-flush mt-3">${listParts.join("")}</ul>`;

        divEmptyInfoReceptor.classList.remove('d-flex');
        divEmptyInfoReceptor.setAttribute('hidden', true);
        divFilledInfoReceptor.removeAttribute('hidden');

        $("#inpEmisor").data('idempresa', item.id);
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblOrigen.innerHTML = emptyInfo;
        lblNivel.innerHTML = emptyInfo;
        lblPerfil.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;

        divEmptyInfoReceptor.removeAttribute('hidden');
        divFilledInfoReceptor.setAttribute('hidden', true);
        divEmptyInfoReceptor.classList.add('d-flex');

        $("#inpEmisor").data('idempresa', null);

        $("#inpReceptor").data("id", null);
        $("#inpReceptor").data("nivel", null);
        $("#inpReceptor").data("actividadesEconomicas", null);
        $("#inpReceptor").data("perfil", null);
        $("#inpReceptor").data("direccion", null);
        $("#inpReceptor").data("productosServicios", null);
    }

}

//Función para validar los datos de una empresa al ser seleccionada por el usuario.
function validarEmpresaSeleccionada(e, isEmisor, isReceptor) {

    //let tabAField = null;
    //let tabBField = null;
    let empresaLabel = "";
    let nivelField = null;
    let actividadesField = null;
    let perfilField = null;
    let prodServField = null;
    let prodServSelectorField = null;
    if (isEmisor) {
        empresaLabel = emisorLabel.toLowerCase();
        //tabAField = document.getElementById("tabAEmisor");
        //tabBField = document.getElementById("tabBEmisor");
        nivelField = document.getElementById("lblNivelEmisor");
        actividadesField = document.getElementById("lblActividadEconomicaEmisor");
        perfilField = document.getElementById("lblPerfilEmisor");
        prodServField = document.getElementById("listProductosServiciosEmisor");
        prodServSelectorField = document.getElementById("inpProductoServicioEmisor");
    }
    if (isReceptor) {
        empresaLabel = receptorLabel.toLowerCase();
        //tabAField = document.getElementById("tabAReceptor");
        //tabBField = document.getElementById("tabBReceptor");
        nivelField = document.getElementById("lblNivelReceptor");
        actividadesField = document.getElementById("lblActividadEconomicaReceptor");
        perfilField = document.getElementById("lblPerfilReceptor");
    }

    //tabAField.classList.remove("bg-");
    //tabAField.classList.remove("is-valid");

    //tabBField.classList.remove("is-invalid");
    //tabBField.classList.remove("is-valid");

    nivelField.classList.remove("is-invalid");
    nivelField.classList.remove("is-valid");

    actividadesField.classList.remove("is-invalid");
    actividadesField.classList.remove("is-valid");

    perfilField.classList.remove("is-invalid");
    perfilField.classList.remove("is-valid");

    if (isEmisor) {

        prodServField.classList.remove("form-control");
        prodServField.classList.remove("is-invalid");
        prodServField.classList.remove("is-valid");

        prodServSelectorField.classList.remove("is-invalid");
        prodServSelectorField.classList.remove("is-valid");

        let prodServsAllowed = (e.productosServicios || []);
        if (prodServsAllowed.length <= 0) {
            perfilField.classList.add("is-invalid");
            showAlert(title, sinProductosServiciosNoPuedeFacturar);
            return false;
        }

        let idProdServField = prodServField.getAttribute("id");
        let prodServsAdded = (document.querySelectorAll(`#${idProdServField} li[clave]`) || []);
        if (prodServsAdded.length <= 0) {
            prodServSelectorField.classList.add("is-invalid");
            showAlert(title, almenosUnProductoServicioNecesario);
            return false;
        }

        let hasError = false;
        let message = "";
        prodServsAdded.forEach(function (ps) {
            //Si el producto o servicio agregado no está en los permitidos, entonces falla la validación.
            let clave = ps.getAttribute("clave");
            let found = prodServsAllowed.find((ps) => ps.clave == clave);
            if (found == undefined) {
                hasError = true;
                message = `${productoServicioConClave} '${clave}' ${noCorresponde} ${empresaLabel} ${noPuedeFacturar}`;
                return false;
            }
        });
        if (hasError) {
            prodServField.classList.add("form-control");
            prodServField.classList.add("is-invalid");
            showAlert(title, message);
            return false;
        }
        else {
            if (prodServsAdded.length >= 1) {
                prodServField.classList.add("form-control");
                prodServField.classList.add("is-valid");
            }
        }
    }

    let puedeFacturar = (e.nivel.puedeFacturar.toLowerCase() === 'true');
    if (!puedeFacturar) {
        nivelField.classList.add("is-invalid");
        /*tabAField.classList.add("is-invalid");*/
        showAlert(title, nivelNoPuedeFacturar);
        return false;
    }
    nivelField.classList.add("is-valid");

    if ((e.perfil || "").length <= 0) {
        perfilField.classList.add("is-invalid");
        showAlert(title, sinPerfilNoPuedeFacturar);
        return false;
    }
    perfilField.classList.add("is-valid");
    /*tabAField.classList.add("is-valid");*/

    if ((e.actividadesEconomicas || []).length <= 0) {
        nivelField.classList.add("is-invalid");
        /*tabBField.classList.add("is-invalid");*/
        showAlert(title, sinActividadesNoPuedeFacturar);
        return false;
    }
    actividadesField.classList.add("is-valid");
    /*tabBField.classList.add("is-valid");*/


    return true;
}

//Función para validar los datos de una empresa al ser seleccionada por el usuario.
function validarFacturacionEntreEmpresas(emisor, receptor) {

    //let tabAEmisorField = document.getElementById("tabAEmisor");
    //let tabBEmisorField = document.getElementById("tabBEmisor");

    //let tabAReceptorField = document.getElementById("tabAReceptor");
    //let tabBReceptorField = document.getElementById("tabBReceptor");

    let nivelEmisorField = document.getElementById("lblNivelEmisor");
    let direccionEmisorField = document.getElementById("lblDomicilioFiscalEmisor");

    let nivelReceptorField = document.getElementById("lblNivelReceptor");
    let direccionReceptorField = document.getElementById("lblDomicilioFiscalReceptor");

    let hasEmisor = parseInt(emisor.id || "0") >= 1;
    let hasReceptor = parseInt(receptor.id || "0") >= 1;

    if (hasEmisor) { if (!validarEmpresaSeleccionada(emisor, true, false)) { return; } }
    if (hasReceptor) { if (!validarEmpresaSeleccionada(receptor, false, true)) { return; } }

    if (!hasEmisor && !hasReceptor) {
        showMessage(title, faltanEmpresasParaComparar);
        return false;
    }
    else if (!hasEmisor || !hasReceptor) {
        showMessage(title, faltaEmpresaParaComparar);
        return false;
    }

    //tabAEmisorField.classList.remove("is-invalid");
    //tabBEmisorField.classList.remove("is-valid");

    nivelEmisorField.classList.remove("is-invalid");
    nivelEmisorField.classList.remove("is-valid");

    direccionEmisorField.classList.remove("is-invalid");
    direccionEmisorField.classList.remove("is-valid");

    //tabAReceptorField.classList.remove("is-invalid");
    //tabBReceptorField.classList.remove("is-valid");

    nivelReceptorField.classList.remove("is-invalid");
    nivelReceptorField.classList.remove("is-valid");

    direccionReceptorField.classList.remove("is-invalid");
    direccionReceptorField.classList.remove("is-valid");

    //Empresas emisoras con nivel menor a nivel del receptor no pueden facturar.
    if (parseInt(emisor.nivel.ordinal || "0") < parseInt(receptor.nivel.ordinal || "0")) {
        nivelEmisorField.classList.add("is-invalid");
        nivelReceptorField.classList.add("is-invalid");
        //tabAEmisorField.classList.add("is-invalid");
        //tabAReceptorField.classList.add("is-invalid");
        showAlert(title, nivelMenorNoPuedeFacturar);
        return false;
    }
    if (emisor.nivel.ordinal === receptor.nivel.ordinal) {
        nivelEmisorField.classList.add("is-invalid");
        nivelReceptorField.classList.add("is-invalid");
        //tabAEmisorField.classList.add("is-invalid");
        //tabAReceptorField.classList.add("is-invalid");
        showAlert(title, mismoNivelNoPuedeFacturar);
        return false;
    }
    nivelEmisorField.classList.add("is-valid");
    nivelReceptorField.classList.add("is-valid");

    if (emisor.direccion === receptor.direccion) {
        direccionEmisorField.classList.add("is-invalid");
        direccionReceptorField.classList.add("is-invalid");
        //tabAEmisorField.classList.add("is-invalid");
        //tabAReceptorField.classList.add("is-invalid");
        showAlert(title, mismaDireccionNoPuedeFacturar);
        return false;
    }
    direccionEmisorField.classList.add("is-valid");
    direccionReceptorField.classList.add("is-valid");

    //tabAEmisorField.classList.add("is-valid");
    //tabAReceptorField.classList.add("is-valid");
    return true;
}

//Función para capturar el clic del botón validar. Valida que se pueda efectuar una factura entre el emisor y el receptor.
function onValidarClick() {
    let emisor = {
        id: $("#inpEmisor").data("id"),
        nivel: $("#inpEmisor").data("nivel"),
        actividadesEconomicas: $("#inpEmisor").data("actividadesEconomicas"),
        perfil: $("#inpEmisor").data("perfil"),
        direccion: $("#inpEmisor").data("domicilioFiscal"),
        productosServicios: $("#inpEmisor").data("productosServicios")
    };

    let receptor = {
        id: $("#inpReceptor").data("id"),
        nivel: $("#inpReceptor").data("nivel"),
        actividadesEconomicas: $("#inpReceptor").data("actividadesEconomicas"),
        perfil: $("#inpReceptor").data("perfil"),
        direccion: $("#inpReceptor").data("domicilioFiscal"),
        productosServicios: $("#inpReceptor").data("productosServicios")
    };

    if (validarFacturacionEntreEmpresas(emisor, receptor)) { showSuccess(title, puedenFacturar); }
}

//Función para capturar el clic del botón limpiar. Limpia y oculta la información del emisor y del receptor.
function onLimpiarClick() {
    limpiarEmisor();

    limpiarReceptor();
}

//Función para limpiar los datos del emisor
function limpiarEmisor() {
    let inpEmisor = document.getElementById("inpEmisor");
    inpEmisor.value = null;
    toggleEmisorInfo();
}

//Función para limpiar los datos del receptor
function limpiarReceptor() {
    let inpReceptor = document.getElementById("inpReceptor");
    inpReceptor.value = null;
    toggleReceptorInfo();
}

//Función para agregar un producto o servicio al listado
function onAgregarProductoServicioClick() {
    let productoServicioField = null;
    let btnAdd = null;
    let idLista = "";

    btnAdd = document.getElementById("dlgBtnAgregarProductoServicioEmisor");
    productoServicioField = $(document.getElementById("inpProductoServicioEmisor"));
    idLista = "listProductosServiciosEmisor";

    //Si el campo producto / servicio no tiene elemento seleccionado, muestra error.
    if (parseInt(productoServicioField.attr("idselected") || "0") <= 0) {
        showAlert(msgAgregarProductoServicio, sinProductoServicio);
        return;
    }

    let listItem = document.querySelector(`#${idLista} li[clave='${productoServicioField.data("clave")}']`);
    //Si el elemento ya existe, muestra error.
    if (listItem) {
        showAlert(msgAgregarProductoServicio, productoServicioRepetido);
        return;
    }

    agregarProductoServicio(productoServicioField.attr("idselected"), productoServicioField.data("clave"), productoServicioField.data("value"), idLista);
    
    btnAdd.classList.add("disabled");

    productoServicioField.val("");
    productoServicioField.attr("idselected", 0);
}

//Función para añadir un elemento al listado de productos y servicios
function agregarProductoServicio(id, clave, descripcion, idListaDOM) {
    let listProductosServicios = document.getElementById(idListaDOM);

    listProductosServicios.innerHTML += `<li id="${id}" clave="${clave}" class="list-group-item">
                                            <div class="row">
                                                <div class="col-11 border-end">
                                                  <div class="fw-bold">${clave}</div>
                                                  ${descripcion}
                                                </div>
                                                <div class="col-1 align-items-center d-flex justify-content-center">
									                <button type="button" class="btn-close formButton" onclick="onEliminarProductoServicioClick('${clave}', '${idListaDOM}');"></button>
                                                </div>
                                            </div>
								          </li>`;
}

//Función para eliminar un producto o servicio del listado
function onEliminarProductoServicioClick(clave, idListaDOM) {
    let listaDOM = document.getElementById(idListaDOM);
    let listItem = document.querySelector(`#${idListaDOM} li[clave='${clave}']`);

    listItem.remove();
    listaDOM.classList.remove("form-control");
    listaDOM.classList.remove("is-invalid");
    listaDOM.classList.remove("is-valid");
}