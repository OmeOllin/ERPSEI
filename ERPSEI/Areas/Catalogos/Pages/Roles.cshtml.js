var table;
var buttonRemove;
var selections = [];
var dlgDetalleModal = null;

const NUEVO = 0;
const EDITAR = 1;
const VER = 2;
const postOptions = { headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() } }

//Función para inicializar el módulo.
document.addEventListener("DOMContentLoaded", function (event) {
    table = $("#table");
    buttonRemove = $("#remove");
    dlgDetalleModal = new bootstrap.Modal(document.getElementById('dlgDetalle'), null);

    initTable();

    onBuscarClick();
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
    if (typeof res == "string" && res.length >= 1) { res = JSON.parse(res); }
    $.each(res, function (i, row) { row.state = $.inArray(row.id, selections) !== -1 });
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

    return icons.join('');
}
//Eventos de los iconos de operación
window.operateEvents = {
    'click .see': function (e, value, row, index) {
        initDetallesDialog(VER, row);
    },
    'click .edit': function (e, value, row, index) {
        initDetallesDialog(EDITAR, row);
    }
}
//Función para agregar cfdis
function onAgregarClick() {
    let aModulos = [];
    moduleList.forEach(function (m) {
        aModulos.push({
            nombre: m,
            puedeTodo: false,
            puedeConsultar: false,
            puedeEditar: false,
            puedeEliminar: false,
            puedeAutorizar: false
        });
    });

    let oRol = { id: "Nuevo", rol: "", modulos: aModulos }
    initDetallesDialog(NUEVO, oRol);
    dlgDetalleModal.toggle();
}

//Función para inicializar la tabla
function initTable() {
    table.bootstrapTable('destroy').bootstrapTable({
        height: 550,
        locale: cultureName,
        columns: [
            {
                field: "state",
                checkbox: true,
                align: "center",
                valign: "middle"
            },
            {
                title: colNombreHeader,
                field: "rol",
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
            buttonRemove.prop('disabled', !table.bootstrapTable('getSelections').length);
            // save your data, here just save the current page
            selections = getIdSelections()
            // push or splice the selections if you want to save all data selections
        }
    );
    table.on('all.bs.table', function (e, name, args) {
        console.log(name, args)
    });
    buttonRemove.click(function () { onDeleteRolClick(selections); });
}
//Función para capturar el click de los botones para dar de baja roles. Ejecuta una llamada ajax para dar de baja roles.
function onDeleteRolClick(ids = null) {
    askConfirmation(dlgDeleteTitle, dlgDeleteQuestion, function () {
        let oParams = {};

        if (ids != null) { oParams.ids = ids; }
        else { oParams.ids = [document.getElementById("inpRolId").value]; }

        doAjax(
            "/Catalogos/Roles/DeleteRoles",
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

/////////////////////////////////
//Funcionalidad Filtrar
/////////////////////////////////
//Función para filtrar los datos de la tabla.
function onBuscarClick() {
    let oParams = {};

    doAjax(
        "/Catalogos/Roles/Filtrar",
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
                showError(btnBuscarTitle, resp.mensaje);
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
//Funcionalidad Diálogo
////////////////////////////////
//Función para inicializar el cuadro de diálogo de los detalles del registro
function initDetallesDialog(action, row) {
    let inpRolId = document.getElementById("inpRolId");
    let inpRolNombre = document.getElementById("inpRolNombre");
    let dlgTitle = document.getElementById("dlgDetalleTitle");
    let inpPuedeTodoTodos = document.getElementById("inpPuedeTodoTodos");
    let inpPuedeConsultarTodos = document.getElementById("inpPuedeConsultarTodos");
    let inpPuedeEditarTodos = document.getElementById("inpPuedeEditarTodos");
    let inpPuedeEliminarTodos = document.getElementById("inpPuedeEliminarTodos");
    let inpPuedeAutorizarTodos = document.getElementById("inpPuedeAutorizarTodos");
    let summaryContainer = document.getElementById("saveValidationSummary");
    summaryContainer.innerHTML = "";

    switch (action) {
        case NUEVO:
        case EDITAR:
            if (action == NUEVO) {
                dlgTitle.innerHTML = dlgNuevoTitle;
            }
            else {
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

    inpRolId.value = row.id;
    inpRolNombre.value = row.rol;
    inpPuedeTodoTodos.checked = false;
    inpPuedeConsultarTodos.checked = false;
    inpPuedeEditarTodos.checked = false;
    inpPuedeEliminarTodos.checked = false;
    inpPuedeAutorizarTodos.checked = false;

    row.modulos.forEach(function (m) {
        let inpPuedeTodo = document.getElementById(`inpPuedeTodo${m.nombre}`);
        let inpPuedeConsultar = document.getElementById(`inpPuedeConsultar${m.nombre}`);
        let inpPuedeEditar = document.getElementById(`inpPuedeEditar${m.nombre}`);
        let inpPuedeEliminar = document.getElementById(`inpPuedeEliminar${m.nombre}`);
        let inpPuedeAutorizar = document.getElementById(`inpPuedeAutorizar${m.nombre}`);

        inpPuedeTodo.checked = (m.puedeTodo || "False") == "True";
        inpPuedeConsultar.checked = (m.puedeConsultar || "False") == "True";
        inpPuedeEditar.checked = (m.puedeEditar || "False") == "True";
        inpPuedeEliminar.checked = (m.puedeEliminar || "False") == "True";
        if (inpPuedeAutorizar) { inpPuedeAutorizar.checked = (m.puedeAutorizar || "False") == "True"; }

        if (inpPuedeAutorizar && inpPuedeAutorizar.checked) { onModuloPuedeAutorizarClick(m.id); }
        if (inpPuedeEliminar && inpPuedeEliminar.checked) { onModuloPuedeEliminarClick(m.id); }
        if (inpPuedeEditar && inpPuedeEditar.checked) { onModuloPuedeEditarClick(m.id); }
        if (inpPuedeConsultar && inpPuedeConsultar.checked) { onModuloPuedeConsultarClick(m.id); }

        /*onModuloPuedeTodoClick(m.id, false);*/
    });

    dlgDetalleModal.toggle();
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
//Función para el guardado de información
function onGuardarClick() {
    //Ejecuta la validación
    $("#theForm").validate();
    //Determina los errores
    let valid = $("#theForm").valid();
    //Si la forma no es válida, entonces finaliza.
    if (!valid) { return; }

    let idField = document.getElementById("inpRolId");
    let inpRolNombre = document.getElementById("inpRolNombre");
    let filasModulos = document.querySelectorAll(".filaModulo");
    let dlgTitle = document.getElementById("dlgDetalleTitle");
    let summaryContainer = document.getElementById("saveValidationSummary");

    summaryContainer.innerHTML = "";

    let modulos = [];

    filasModulos.forEach(function (row) {
        let nombreModulo = row.querySelector(".nombreModulo");
        let permisoTodo = row.querySelector(".permisoTodo");
        let permisoConsultar = row.querySelector(".permisoConsultar");
        let permisoEditar = row.querySelector(".permisoEditar");
        let permisoEliminar = row.querySelector(".permisoEliminar");
        let permisoAutorizar = row.querySelector(".permisoAutorizar");
        modulos.push({
            moduloId: nombreModulo.getAttribute("idModulo"),
            nombre: nombreModulo.getAttribute("nombreNormalizado"),
            puedeTodo: permisoTodo.checked,
            puedeConsultar: permisoConsultar.checked,
            puedeEditar: permisoEditar.checked,
            puedeEliminar: permisoEliminar.checked,
            puedeAutorizar: permisoAutorizar != null ? permisoAutorizar.checked || false : false
        });
    });

    let oParams = {
        id: idField.value == "Nuevo" ? 0 : idField.value,
        nombreRol: inpRolNombre.value,
        modulos: modulos
        
    };

    doAjax(
        "/Catalogos/Roles/Save",
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

            onCerrarClick();

            dlgDetalleModal.toggle();

            onBuscarClick();

            showSuccess(dlgTitle.innerHTML, resp.mensaje);
        }, function (error) {
            showError("Error", error);
        },
        postOptions
    );
}

//Función para checar/deschecar todos los permisos de "todo" en todos los módulos
function onPuedeTodoTodosClick() {
    let inpPuedeTodoTodos = document.getElementById("inpPuedeTodoTodos");
    let inpPuedeConsultarTodos = document.getElementById("inpPuedeConsultarTodos");
    let inpPuedeEditarTodos = document.getElementById("inpPuedeEditarTodos");
    let inpPuedeEliminarTodos = document.getElementById("inpPuedeEliminarTodos");
    let inpPuedeAutorizarTodos = document.getElementById("inpPuedeAutorizarTodos");

    inpPuedeConsultarTodos.checked = inpPuedeTodoTodos.checked;
    inpPuedeEditarTodos.checked = inpPuedeTodoTodos.checked;
    inpPuedeEliminarTodos.checked = inpPuedeTodoTodos.checked;
    inpPuedeAutorizarTodos.checked = inpPuedeTodoTodos.checked;

    let filasModulos = document.querySelectorAll(".filaModulo");
    filasModulos.forEach(function (row) {
        let permisoTodo = row.querySelector(".permisoTodo");
        if (permisoTodo) {
            permisoTodo.checked = inpPuedeTodoTodos.checked;
            onModuloPuedeTodoClick(permisoTodo.getAttribute("idModulo"), false);
        }
    });
}
//Función para checar/deschecar todos los permisos de "consultar" en todos los módulos
function onPuedeConsultarTodosClick() {
    let inpPuedeConsultarTodos = document.getElementById("inpPuedeConsultarTodos");
    let filasModulos = document.querySelectorAll(".filaModulo");
    filasModulos.forEach(function (row) {
        let permisoConsultar = row.querySelector(".permisoConsultar");
        if (permisoConsultar) {
            permisoConsultar.checked = inpPuedeConsultarTodos.checked;
            verificarPermisosModulo(permisoConsultar.getAttribute("idModulo"));
        }
    });
}
//Función para checar/deschecar todos los permisos de "editar" en todos los módulos
function onPuedeEditarTodosClick() {
    let inpPuedeEditarTodos = document.getElementById("inpPuedeEditarTodos");
    let filasModulos = document.querySelectorAll(".filaModulo");
    filasModulos.forEach(function (row) {
        let permisoEditar = row.querySelector(".permisoEditar");
        if (permisoEditar) {
            permisoEditar.checked = inpPuedeEditarTodos.checked;
            verificarPermisosModulo(permisoEditar.getAttribute("idModulo"));
        }
    });
}
//Función para checar/deschecar todos los permisos de "elimiar" en todos los módulos
function onPuedeEliminarTodosClick() {
    let inpPuedeEliminarTodos = document.getElementById("inpPuedeEliminarTodos");
    let filasModulos = document.querySelectorAll(".filaModulo");
    filasModulos.forEach(function (row) {
        let permisoEliminar = row.querySelector(".permisoEliminar");
        if (permisoEliminar) {
            permisoEliminar.checked = inpPuedeEliminarTodos.checked;
            verificarPermisosModulo(permisoEliminar.getAttribute("idModulo"));
        }
    });
}
//Función para checar/deschecar todos los permisos de "autorizar" en todos los módulos
function onPuedeAutorizarTodosClick() {
    let inpPuedeAutorizarTodos = document.getElementById("inpPuedeAutorizarTodos");
    let filasModulos = document.querySelectorAll(".filaModulo");
    filasModulos.forEach(function (row) {
        let permisoAutorizar = row.querySelector(".permisoAutorizar");
        if (permisoAutorizar) {
            permisoAutorizar.checked = inpPuedeAutorizarTodos.checked;
            verificarPermisosModulo(permisoAutorizar.getAttribute("idModulo"));
        }
    });
}

//Función para checar/deschecar todos los permisos de "todo" en un solo módulo
function onModuloPuedeTodoClick(idModulo, triggeredByUser) {
    let permisoTodo = document.querySelector(`.permisoTodo[idModulo='${idModulo}']`);
    let permisoConsultar = document.querySelector(`.permisoConsultar[idModulo='${idModulo}']`);
    let permisoEditar = document.querySelector(`.permisoEditar[idModulo='${idModulo}']`);
    let permisoEliminar = document.querySelector(`.permisoEliminar[idModulo='${idModulo}']`);
    let permisoAutorizar = document.querySelector(`.permisoAutorizar[idModulo='${idModulo}']`);

    if (permisoConsultar) { permisoConsultar.checked = permisoTodo.checked; }
    if (permisoEditar) { permisoEditar.checked = permisoTodo.checked; }
    if (permisoEliminar) { permisoEliminar.checked = permisoTodo.checked; }
    if (permisoAutorizar) { permisoAutorizar.checked = permisoTodo.checked; }

    if (triggeredByUser) {
        let inputsTodo = document.querySelectorAll(`.permisoTodo`);
        let inpPuedeTodoTodos = document.getElementById("inpPuedeTodoTodos");
        let inputsTodoChecados = document.querySelectorAll(`.permisoTodo:checked`);

        inpPuedeTodoTodos.checked = inputsTodo.length === inputsTodoChecados.length;
        onModuloPuedeConsultarClick(idModulo);
        onModuloPuedeEditarClick(idModulo);
        onModuloPuedeEliminarClick(idModulo);
        onModuloPuedeAutorizarClick(idModulo);
    }
}
//Función para checar/deschecar el permiso de "Consultar" en un solo módulo
function onModuloPuedeConsultarClick(idModulo) {
    let inputsConsultar = document.querySelectorAll(`.permisoConsultar`);
    let inpPuedeConsultarTodos = document.getElementById("inpPuedeConsultarTodos");
    let inputsConsultarChecados = document.querySelectorAll(`.permisoConsultar:checked`);

    inpPuedeConsultarTodos.checked = inputsConsultar.length === inputsConsultarChecados.length;

    verificarPermisosModulo(idModulo);
}
//Función para checar/deschecar el permiso de "Editar" en un solo módulo
function onModuloPuedeEditarClick(idModulo) {
    let inputsEditar = document.querySelectorAll(`.permisoEditar`);
    let inpPuedeEditarTodos = document.getElementById("inpPuedeEditarTodos");
    let inputsEditarChecados = document.querySelectorAll(`.permisoEditar:checked`);

    inpPuedeEditarTodos.checked = inputsEditar.length === inputsEditarChecados.length;

    verificarPermisosModulo(idModulo);
}
//Función para checar/deschecar el permiso de "Eliminar" en un solo módulo
function onModuloPuedeEliminarClick(idModulo) {
    let inputsEliminar = document.querySelectorAll(`.permisoEliminar`);
    let inpPuedeEliminarTodos = document.getElementById("inpPuedeEliminarTodos");
    let inputsEliminarChecados = document.querySelectorAll(`.permisoEliminar:checked`);

    inpPuedeEliminarTodos.checked = inputsEliminar.length === inputsEliminarChecados.length;

    verificarPermisosModulo(idModulo);
}
//Función para checar/deschecar el permiso de "Autorizar" en un solo módulo
function onModuloPuedeAutorizarClick(idModulo) {
    let inputsAutorizar = document.querySelectorAll(`.permisoAutorizar`);
    let inpPuedeAutorizarTodos = document.getElementById("inpPuedeAutorizarTodos");
    let inputsAutorizarChecados = document.querySelectorAll(`.permisoAutorizar:checked`);

    inpPuedeAutorizarTodos.checked = inputsAutorizar.length === inputsAutorizarChecados.length;

    verificarPermisosModulo(idModulo);
}

function verificarPermisosModulo(idModulo) {
    let permisoTodo = document.querySelector(`.permisoTodo[idModulo='${idModulo}']`);
    let permisoConsultar = document.querySelector(`.permisoConsultar[idModulo='${idModulo}']`);
    let permisoEditar = document.querySelector(`.permisoEditar[idModulo='${idModulo}']`);
    let permisoEliminar = document.querySelector(`.permisoEliminar[idModulo='${idModulo}']`);
    let permisoAutorizar = document.querySelector(`.permisoAutorizar[idModulo='${idModulo}']`);

    let puedeConsultar = permisoConsultar ? (permisoConsultar.checked) : true;
    let puedeEditar = permisoEditar ? (permisoEditar.checked) : true;
    let puedeEliminar = permisoEliminar ? (permisoEliminar.checked) : true;
    let puedeAutorizar = permisoAutorizar ? (permisoAutorizar.checked) : true;

    permisoTodo.checked = puedeConsultar && puedeEditar && puedeEliminar && puedeAutorizar;

    let inputsTodo = document.querySelectorAll(`.permisoTodo`);
    let inpPuedeTodoTodos = document.getElementById("inpPuedeTodoTodos");
    let inputsTodoChecados = document.querySelectorAll(`.permisoTodo:checked`);

    inpPuedeTodoTodos.checked = inputsTodo.length === inputsTodoChecados.length;
}

////////////////////////////////