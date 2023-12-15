var selections = [];
var tableComunicados;
var tablePoliticas;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

//Función para inicializar el módulo.
document.addEventListener("DOMContentLoaded", function (event) {
    tableComunicados = $("#tableComunicados");
    tablePoliticas = $("#tablePoliticas");

    initTable(tableComunicados);
    initTable(tablePoliticas);
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
//Función para inicializar la tabla
function initTable(table) {
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
                title: "Columna 1",
                field: "nombre",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Acciones",
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
            // save your data, here just save the current page
            selections = getIdSelections()
            // push or splice the selections if you want to save all data selections
        })
    table.on('all.bs.table', function (e, name, args) {
        console.log(name, args)
    })
}
////////////////////////////////