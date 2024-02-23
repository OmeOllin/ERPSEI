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
});

//Función para mostrar u ocultar la información del emisor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleEmisorInfo(item = null) {
    let divFilledInfoEmisor = document.getElementById("divFilledInfoEmisor");
    let divEmptyInfoEmisor = document.getElementById("divEmptyInfoEmisor");

    let lblRFC = document.getElementById("lblRFCEmisor");
    let lblRazonSocial = document.getElementById("lblRazonSocialEmisor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialEmisor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaEmisor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalEmisor");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblObjetoSocial.innerHTML = item.objetoSocial;
        lblActividadEconomica.innerHTML = item.actividadEconomica;
        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        divEmptyInfoEmisor.classList.remove('d-flex');
        divEmptyInfoEmisor.setAttribute('hidden', true);
        divFilledInfoEmisor.removeAttribute('hidden');

        $("#inpReceptor").data('idempresa', item.id);
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;

        divEmptyInfoEmisor.removeAttribute('hidden');
        divFilledInfoEmisor.setAttribute('hidden', true);
        divEmptyInfoEmisor.classList.add('d-flex');

        $("#inpReceptor").data('idempresa', null);
    }

}

//Función para mostrar u ocultar la información del receptor. Si se establece el parámetro item, se muestra. De lo contrario, se oculta.
function toggleReceptorInfo(item = null) {
    let divFilledInfoReceptor = document.getElementById("divFilledInfoReceptor");
    let divEmptyInfoReceptor = document.getElementById("divEmptyInfoReceptor");

    let lblRFC = document.getElementById("lblRFCReceptor");
    let lblRazonSocial = document.getElementById("lblRazonSocialReceptor");
    let lblObjetoSocial = document.getElementById("lblObjetoSocialReceptor");
    let lblActividadEconomica = document.getElementById("lblActividadEconomicaReceptor");
    let lblDomicilioFiscal = document.getElementById("lblDomicilioFiscalReceptor");

    if (item != null) {
        lblRFC.innerHTML = item.rfc;
        lblRazonSocial.innerHTML = item.razonSocial;
        lblObjetoSocial.innerHTML = item.objetoSocial;
        lblActividadEconomica.innerHTML = item.actividadEconomica;
        lblDomicilioFiscal.innerHTML = item.domicilioFiscal;

        divEmptyInfoReceptor.classList.remove('d-flex');
        divEmptyInfoReceptor.setAttribute('hidden', true);
        divFilledInfoReceptor.removeAttribute('hidden');

        $("#inpEmisor").data('idempresa', item.id);
    }
    else {
        lblRFC.innerHTML = emptyInfo;
        lblRazonSocial.innerHTML = emptyInfo;
        lblObjetoSocial.innerHTML = emptyInfo;
        lblActividadEconomica.innerHTML = emptyInfo;
        lblDomicilioFiscal.innerHTML = emptyInfo;

        divEmptyInfoReceptor.removeAttribute('hidden');
        divFilledInfoReceptor.setAttribute('hidden', true);
        divEmptyInfoReceptor.classList.add('d-flex');

        $("#inpEmisor").data('idempresa', null);
    }

}

//Función para capturar el clic del botón validar. Valida que se pueda efectuar una factura entre el emisor y el receptor.
function onValidarClick() {
   
}

//Función para capturar el clic del botón limpiar. Limpia y oculta la información del emisor y del receptor.
function onLimpiarClick() {
    let inpEmisor = document.getElementById("inpEmisor");
    inpEmisor.value = null;
    toggleEmisorInfo();

    let inpReceptor = document.getElementById("inpReceptor");
    inpReceptor.value = null;
    toggleReceptorInfo();
}