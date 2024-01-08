var oc = null;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

function onCargarOrganigrama() {
    let btnOrganigrama = document.getElementById("btnOrganigrama");
    let selPuesto = document.getElementById("selFiltroPuesto");
    let selArea = document.getElementById("selFiltroArea");
    let selSubarea = document.getElementById("selFiltroSubarea");
    let divCharts = document.getElementById("divCharts");
    let btnExportar = document.getElementById("btnExportar");

    let oParams = {
        PuestoId: selPuesto.value == 0 ? null : parseInt(selPuesto.value),
        AreaId: selArea.value == 0 ? null : parseInt(selArea.value),
        SubareaId: selSubarea.value == 0 ? null : parseInt(selSubarea.value),
    };

    let errors = validarParametros(oParams)||"";

    if (errors.length >= 1) {
        showError(btnOrganigrama.innerHTML, errors);
        return;
    }

    divCharts.innerHTML = `<div class="col-12 opacity-25" style="height:150px;">
		                        <div>
			                        <img class="indeximage" src="${emptyOrgchartURL}" />
			                        <div class="col-12 h2">
				                        <span>${orgchartInstruction}</span>
			                        </div>
		                        </div>
	                        </div>`;
    oc = null;
    btnExportar.classList.add("disabled");
    doAjax(
        "/ERP/Organigrama/FiltrarEmpleados",
        oParams,
        function (resp) {
            if (resp.tieneError) {
                showError(btnOrganigrama.innerHTML, resp.mensaje);
                return;
            }

            if (typeof resp.datos == "string" && resp.datos.length >= 1) { resp.datos = JSON.parse(resp.datos); }

            if (resp.datos.length <= 0) {
                showAlert(btnOrganigrama.innerHTML, msgSinResultados);
                return;
            }

            divCharts.innerHTML = `<div id="chart${resp.datos[0].id}" class="col-12 orgchart"></div>`;
            oc = $(`#chart${resp.datos[0].id}`).orgchart({
                'data': resp.datos[0],
                'nodeContent': 'title',
                'pan': true,
                'zoom': true,
                'createNode': function ($node, data) {
                    let formattedPhone = data.telefono;
                    if (data.telefono.length >= 10) {
                        formattedPhone = `${data.telefono.slice(0, 2)} ${data.telefono.slice(2, 6)} ${data.telefono.slice(6, 10)}`;
                    }
                    let divNombre = data.nombreCompleto.length >= 1 ? `<div><i class="bi bi-person-fill"></i> <b>${data.nombreCompleto}</b></div>` : ``;
                    let divEmail = data.email.length >= 1 ? `<div><i class="bi bi-envelope-at-fill"></i> ${data.email}</div>` : ``;
                    let divTelefono = data.telefono.length >= 1 ? `<div><i class="bi bi-telephone-fill"></i> ${formattedPhone}</div>` : ``;
                    let divOficina = data.oficina.length >= 1 ? `<div><i class="bi bi-building-fill"></i> ${data.oficina}</div>` : ``;
                    data.profilePic = data.profilePic || "/img/default_profile_pic.png";
                    $node.find('.content').append(`
                                                        <div class="second-menu">
                                                            <img class="avatar" src="${data.profilePic}">
                                                            ${divNombre}
                                                            <hr />
                                                            ${divEmail}
                                                            ${divTelefono}
                                                            ${divOficina}
                                                        </div>
                                                        `);
                }
            });

            btnExportar.classList.remove("disabled");
            
            oc.$chartContainer.on('touchmove', function (event) {
                event.preventDefault();
            });

            var $container = oc.$chartContainer;
            var $chart = oc.$chart;

            //Si el ancho del chart es mayor al ancho del contenedor, entonces escala el diagrama para mostrarlo completo.
            if ($container.width() < $chart.outerWidth(true)) {
                var scale = $container.width() / $chart.outerWidth(true);
                var x = ($container.width() - $chart.outerWidth(true)) / 2 * (1 / scale);
                var y = ($container.height() - $chart.outerHeight(true)) / 2 * (1 + scale);
                oc.setChartScale($chart, scale);
                var val = $chart.css('transform');
                $chart.css('transform', val + ' translate(' + x + 'px,' + y + 'px)');
            }
            else {
                oc.$chart.css('transform', 'none');
            }

        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}
function validarParametros(oParams) {
    //Si no se seleccionó ningún área, marca el error.
    //if (oParams.AreaId == null) { return msgAreaIdRequerida; }

    //Obtiene las opciones que correspondan al área seleccionada.
    subareaOptions = document.querySelectorAll(`#selFiltroSubarea option[areaid='${oParams.AreaId}']`);

    return "";
}

function onExportarOrganigrama() {
    if (oc === null) { return; }

    oc.export("orgchart", "png");
}

function onAreaChanged() {
    let areaField = document.getElementById("selFiltroArea");
    let subareaField = document.getElementById("selFiltroSubarea");

    //Establece la selección por default.
    subareaField.value = 0;

    //Oculta todas las opciones, excepto la opción "Seleccione..."
    let subareaOptions = document.querySelectorAll(`#selFiltroSubarea option`);
    subareaOptions.forEach(function (o) { if (o.value >= 1) { o.style.display = 'none'; } });

    //Deshabilita el campo de selección
    subareaField.setAttribute('disabled', true);

    //Muestra solo las opciones que correspondan al área seleccionada.
    subareaOptions = document.querySelectorAll(`#selFiltroSubarea option[areaid='${areaField.value}']`);
    //Si hay subareas...
    if (subareaOptions.length >= 1) {
        //Muestra las subareas
        subareaOptions.forEach(function (o) { o.style.display = 'block'; });

        //Habilita el campo de selección
        subareaField.removeAttribute('disabled');
    }
}

$(document).on("click", ".node", function (e) {
    $(document).find(".second-menu:visible").toggle();
    $(this).find('.second-menu').toggle();
    e.stopPropagation();
});
$(document).on("click", function (e) {
    $(document).find(".second-menu:visible").toggle();
});

$(window).resize(function () {
    var width = $(window).width();
    if (width > 1200) {
        if(oc != null) oc.init({ 'verticalLevel': undefined });
    } else {
        if (oc != null) oc.init({ 'verticalLevel': 2 });
    }
});