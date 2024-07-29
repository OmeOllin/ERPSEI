var table;
var selections = [];
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

function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel'],
        columns: [{
            title: colIdHeader,
            field: "Id",
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
            title: colFechaHoraHeader,
            field: "FechaHora",
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
            title: colHoraHeader,
            field: "Hora",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colDireccionHeader,
            field: "Direccion",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colNombreDispositivoHeader,
            field: "NombreDispositivo",
            align: "center",
            valign: "middle",
            sortable: true
        },
        {
            title: colSerialDispositivoHeader,
            field: "SerialDispositivo",
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
