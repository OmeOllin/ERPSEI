var table;
var buttonRemove;
var selections = [];
var data = {
    total: 2,
    totalNotFiltered: 2,
    rows: [
        {
            "id": 1,
            "nombre": "Luis Alberto Linares Hernández",
            "fechaIngreso": "07/05/1991",
            "puesto": "Desarrollador de software",
            "area": "Desarrollo",
            "telefono": "5529300993",
            "correo": "luis_linares75@hotmail.com"
        },
        {
            "id": 2,
            "nombre": "Pablo Suarez Nájera",
            "fechaIngreso": "12/12/1985",
            "puesto": "Tester",
            "area": "Desarrollo",
            "telefono": "5531330467",
            "correo": "pablo_suarez333@hotmail.com"
        }
    ]
};
document.addEventListener("DOMContentLoaded", function (event) {
    initializeDate();

    table = $("#table");
    buttonRemove = $("#remove");

    initTable();
});

function additionalButtons() {
    return {
        btnImport: {
            text: 'Importar',
            icon: 'bi-upload',
            event: function () {
                alert("Importar datos");
            },
            attributes: {
                title: 'Importar datos desde un archivo excel'
            }
        }
    }
}
function getIdSelections() {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id
    })
}
function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1
    })
    return res
}

function detailFormatter(index, row) {
    var html = []
    $.each(row, function (key, value) {
        html.push('<p><b>' + key + ':</b> ' + value + '</p>')
    })
    return html.join('')
}
function operateFormatter(value, row, index) {
    return [
        '<a class="like" href="javascript:void(0)" title="Like">',
            '<i class="bi bi-heart"></i>',
        '</a>  ',
        '<a class="remove" href="javascript:void(0)" title="Remove">',
            '<i class="bi bi-trash"></i>',
        '</a>'
    ].join('')
}
window.operateEvents = {
    'click .like': function (e, value, row, index) {
        alert('You click like action, row: ' + JSON.stringify(row))
    },
    'click .remove': function (e, value, row, index) {
        table.bootstrapTable('remove', {
            field: 'id',
            values: [row.id]
        })
    }
}
function totalTextFormatter(data) {
    return 'Total'
}
function totalNameFormatter(data) {
    return data.length
}
function totalPriceFormatter(data) {
    var field = this.field
    return '$' + data.map(function (row) {
        return +row[field].substring(1)
    }).reduce(function (sum, i) {
        return sum + i
    }, 0)
}

function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        exportDataType: 'all',
        exportTypes: ['excel', 'pdf'],
        data: data,
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
            },
            {
                title: "Fecha de Ingreso",
                field: "fechaIngreso",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Puesto",
                field: "puesto",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Área",
                field: "area",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Teléfono",
                field: "telefono",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                title: "Correo Electrónico",
                field: "correo",
                align: "center",
                valign: "middle",
                sortable: true
            },
            {
                field: "operate",
                title: "Acciones",
                align: 'center',
                clickToSelect: false,
                events: window.operateEvents,
                formatter: operateFormatter
            }
        ]
        //columns: [
        //    [{
        //        field: 'state',
        //        checkbox: true,
        //        rowspan: 2,
        //        align: 'center',
        //        valign: 'middle'
        //    }, {
        //        title: 'Item ID',
        //        field: 'id',
        //        rowspan: 2,
        //        align: 'center',
        //        valign: 'middle',
        //        sortable: true,
        //        footerFormatter: totalTextFormatter
        //    }, {
        //        title: 'Item Detail',
        //        colspan: 3,
        //        align: 'center'
        //    }],
        //    [{
        //        field: 'name',
        //        title: 'Item Name',
        //        sortable: true,
        //        footerFormatter: totalNameFormatter,
        //        align: 'center'
        //    }, {
        //        field: 'price',
        //        title: 'Item Price',
        //        sortable: true,
        //        align: 'center',
        //        footerFormatter: totalPriceFormatter
        //    }, {
        //        field: 'operate',
        //        title: 'Item Operate',
        //        align: 'center',
        //        clickToSelect: false,
        //        events: window.operateEvents,
        //        formatter: operateFormatter
        //    }]
        //]
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
        var ids = getIdSelections()
        table.bootstrapTable('remove', {
            field: 'id',
            values: ids
        })
        buttonRemove.prop('disabled', true);
    })
}

function initializeDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    let monthStart = now.getFullYear() + "-" + (month) + "-" + "01";
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    document.getElementById("inpFiltroFechaIngresoInicio").setAttribute("value", monthStart);
    document.getElementById("inpFiltroFechaIngresoFin").setAttribute("value", today);
}