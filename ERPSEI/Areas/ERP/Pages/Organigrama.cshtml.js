var ocs = [];
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

function onCargarOrganigrama() {
    let btnOrganigrama = document.getElementById("btnOrganigrama");
    let selArea = document.getElementById("selFiltroArea");
    let selSubarea = document.getElementById("selFiltroSubarea");
    let divCharts = document.getElementById("divCharts");

    let oParams = {
        AreaId: selArea.value == 0 ? null : parseInt(selArea.value),
        SubareaId: selSubarea.value == 0 ? null : parseInt(selSubarea.value),
    };

    divCharts.innerHTML = "";
    ocs = [];
    doAjax(
        "/ERP/Organigrama/FiltrarEmpleados",
        oParams,
        function (resp) {
            let windowSize = $(window).width();
            let vLevel = undefined;
            if (windowSize > 1200) {
                vLevel = undefined;
            } else {
                vLevel = 2;
            }
            if (resp.tieneError) {
                showError(btnOrganigrama.innerHTML, resp.mensaje);
                return;
            }

            if (typeof resp.datos == "string" && resp.datos.length >= 1) { resp.datos = JSON.parse(resp.datos); }
            divCharts.innerHTML += `<div id="chart${resp.datos[0].id}" class="col-12 orgchart"></div>`;
            ocs.push(
                $(`#chart${resp.datos[0].id}`).orgchart({
                    'data': resp.datos[0],
                    'nodeContent': 'title',
                    'pan': true,
                    'zoom': true,
                    'verticalLevel': vLevel,
                    'createNode': function ($node, data) {
                        /*$node.find('.title').append(`<img class="avatar" src="https://dabeng.github.io/OrgChart/img/avatar/8.jpg" crossorigin="anonymous">`);*/
                        //$node.find('.title').append(`<img class="avatar" src="/img/default_profile_pic.png" crossorigin="anonymous" />`);
                        //$node.find('.content').prepend($node.find('.symbol'));
                        let formattedPhone = data.telefono;
                        if (data.telefono.length >= 10) {
                            formattedPhone = `${data.telefono.slice(0, 2)} ${data.telefono.slice(2, 6)} ${data.telefono.slice(6, 10)}`;
                        }
                        let divEmail = data.email.length >= 1 ? `<div><i class="bi bi-envelope-at-fill"></i> ${data.email}</div>` : ``;
                        let divTelefono = data.telefono.length >= 1 ? `<div><i class="bi bi-telephone-fill"></i> ${formattedPhone}</div>` : ``;
                        let divOficina = data.oficina.length >= 1 ? `<div><i class="bi bi-building-fill"></i> ${data.oficina}</div>` : ``;
                        $node.find('.content').append(`
                                                        <div class="second-menu">
                                                            <img class="avatar" src="/img/default_profile_pic.png">
                                                            ${divEmail}
                                                            ${divTelefono}
                                                            ${divOficina}
                                                        </div>
                                                      `);
                    }
                })
            );
            //resp.datos.forEach(function (e) {
            //    divCharts.innerHTML += `<div id="chart${e.id}" class="col-12 orgchart"></div>`;

            //    ocs.push($(`#chart${e.id}`).orgchart({
            //        'data': e,
            //        'nodeContent': 'title',
            //        'pan': true,
            //        'zoom': true
            //    }));


            //});

            //var nodeTemplate = function (data) {
            //    return `
            //            <div class="title">${data.name} <img class="avatar" src="/img/default_profile_pic.png" crossorigin="anonymous" /></div>
            //            <div class="content"><b>${data.title}</b></div>
            //          `;
            //};

            ocs.forEach(function (oc) {
                oc.$chartContainer.on('touchmove', function (event) {
                    event.preventDefault();
                });
            });

        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
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
        ocs.forEach(function (oc) {
            oc.init({ 'verticalLevel': undefined });
        });
    } else {
        ocs.forEach(function (oc) {
            oc.init({ 'verticalLevel': 2 });
        });
    }
});