var table;
var selections = [];

document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    initTable();
});

function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id;
    });
}

function responseHandler(res) {
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
        columns: [
            {
                title: colIdHeader,
                field: "Id",
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
            },
            {
                title: colNombreEmpleadoHeader,
                field: "NombreEmpleado",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: colNoTarjetaHeader,
                field: "NoTarjeta",
                align: "center",
                valign: "middle",
                sortable: true
            }
        ]
    })
    table.on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', function () {
        selections = getIdSelections();
    });

    table.on('all.bs.table', function (e, name, args) {
        console.log(name, args);
    });
}