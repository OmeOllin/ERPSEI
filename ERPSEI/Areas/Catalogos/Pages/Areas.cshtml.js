var table;
var buttonRemove;
var selections = [];

document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");

    initTable();
});

function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}
function responseHandler(res) {
    $.each(res, function (i, row) {
        let oRow = JSON.parse(row);
        row.state = $.inArray(oRow.id, selections) !== -1
        res[i] = oRow;
    })
    return res
}

function detailFormatter(index, row) {
    var html = []
    $.each(row, function (key, value) {
        if (key != "state") {
            html.push('<p><b>' + key + ':</b> ' + value + '</p>')
        }
    });
    return html.join('')
}
function operateFormatter(value, row, index) {
    return [
        '<a class="see" href="javascript:void(0)" title="' + btnVerTitle + '">',
            '<i class="bi bi-search"></i>',
        '</a>  ',
        '<a class="edit" href="javascript:void(0)" title="' + btnEditarTitle + '">',
            '<i class="bi bi-pencil-fill"></i>',
        '</a>'
    ].join('')
}
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        let stringJSON = JSON.stringify(row);
        showInfo("Ver", `Diste clic para ver a: ${row.nombre}`);
    },
    'click .edit': function (e, value, row, index) {
        let stringJSON = JSON.stringify(row);
        showInfo("Ver", `Diste clic para editar a: ${row.nombre}`);
        //table.bootstrapTable('remove', {
        //    field: 'id',
        //    values: [row.id]
        //})
    }
}
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel', 'pdf'],
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
                sortable: true
            },
            {
                title: "Nombre",
                field: "nombre",
                align: "center",
                valign: "middle",
                sortable: true
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
        askConfirmation("Eliminar registros", "¿Está seguro que desea eliminar los registros seleccionados?", function () {
            var ids = getIdSelections()
            table.bootstrapTable('remove', {
                field: 'id',
                values: ids
            })
            buttonRemove.prop('disabled', true);
        });
    })
}