﻿//Función para el llenado de campos a autocompletar
function autoCompletar(selector, oExtend) {
	const classIcon = typeof oExtend != 'undefined' && typeof oExtend.icon === 'boolean' && oExtend.icon === true ? 'ui-autocomplete-icon' : '';
	selector = selector || 'input[area][module][source]';
	oExtend = $.extend({ select: null, change: null }, oExtend);

	// Muestra la lista de sugerencias
	return $(selector).autocomplete({
		position: { collision: "flip" },
		minLength: 3,
		search: function (event, ui) {
			if ($(event.target).val().trim().length < $(this).autocomplete('option', 'minLength')) {
				$(event.target).autocomplete('close');
				return false;
			} //if
		},
		source: function (request, response) {
			let itemDOM = $(this.element);
			itemDOM.attr({ idselected: '' });

			// url y parametros
			let area = itemDOM.attr("area");
			let module = itemDOM.attr("module");
			let source = itemDOM.attr("source");
			let url = `/${area}/${module}/${source}`;

			// Arma objeto de datos para solicitud AJAX
			let oDatos = { texto: request.term };

			let _filtro = itemDOM.attr('filtro');
			//Verifica si requiere datos adicionales para el filtro
			if (typeof _filtro != 'undefined' && _filtro != null) {
				_filtro = _filtro.split(',');
				if (_filtro.length >= 1) // Agrega valores adicionales de los atributos data del elemento
					_filtro.forEach(dataName => oDatos[dataName] = itemDOM.data(dataName));
			} //if

			// Deshabilita componentes de la UI para evitar interacción del usuario durante la petición
			toDisable('.ui-dialog-titlebar-close, .ui-disabled-on-suggest');
			let objDefaults = {
				url: url,
				data: oDatos,
				headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
				type: 'POST',
				dataType: "json",
				success: function (resp) { //Si la llamada es satisfactoria
					// Habilita componentes de la UI previamente deshabilitados
					toEnable('.ui-dialog-titlebar-close, .ui-disabled-on-suggest');
					try {
						if (resp.tieneError) {
							// Si no hay coincidencias, limpia campos relacionados
							itemDOM.val('').attr({ idselected: '' }).removeClass('ui-autocomplete-loading');

							showError('', resp.mensaje);
							return;
						}
						if (typeof resp.datos == "string" && resp.datos.length >= 1) { resp.datos = JSON.parse(resp.datos); };
						if (resp.datos.length == 0) resp.datos.push({ id: -1, value: 'Sin Coincidencias...', label: 'Sin Coincidencias...' });

						response(resp.datos);
					} catch (error) {
						console.warn(error);
					} //try
				},
				error: function (xhr, estado, errdata) { //En caso de error de comunicaciones o del sitio
					console.warn(estado);
					console.warn(errdata);
					showError([JSON.stringify(estado), JSON.stringify(errdata)].join());
					return false;
				}
			};
			$.post(objDefaults);
		},
		select: function (event, ui) {
			let itemDOM = $(this);
			// En caso de no existir coincidencias
			if (ui.item.id == -1) {
				// Limpia el campo de captura
				itemDOM.val('').attr({ idselected: '' });
				ui.item.value = '';

				return false;
			} //if

			// Recupera el ID del renglon seleccionado
			itemDOM.attr({ idselected: ui.item.id });

			// Asigna atributos 'data' al elemento con atributos del item seleccionado
			itemDOM.data(ui.item);

			// Invoca la función personalizada para procesar el elemento seleccionado
			if (typeof oExtend.select == 'function' || typeof eval(itemDOM.attr('onselect')) == 'function') {
				let exec = typeof oExtend.select == 'function' ? oExtend.select : eval(itemDOM.attr('onselect'));
				let respuesta = exec(itemDOM, ui.item);
				if (respuesta === false)
					return false;
			}
		},
		change: function (event, ui) {
			let itemDOM = $(this);
			// Valida si viene vacío para limpiar control
			if (itemDOM.val().trim() == '' || (itemDOM.attr('idselected') || '') == '' || ui.item == null)
				itemDOM.val('').attr({ idselected: '' });

			// Invoca la función personalizada para procesar el elemento seleccionado
			if (typeof oExtend.change == 'function') oExtend.change(itemDOM, ui.item);
		}
	}).addClass(classIcon);
} //function autoCompletar

/**
* Este método permite deshabilitar componentes de la UI mediante un selector
* el selector podrá ser cualquier identificador válido de CSS o un objeto
* jQuery y en caso de no especificarlo, se buscarán los elementos con la clase
* .oneClick
* A los elementos deshabilitados se les agregará la clase ui-state-disabled
* a fin de mantener el estandar de formato del plug-in UI de jQuery.
*/
function toDisable(selector) {
	selector = selector || '.oneClick';
	$(selector).prop('disabled', true).addClass('ui-state-disabled');

	$(selector).each(function (idx, item) {
		if (item.nodeName != 'IMG' && item.nodeName != 'A') return;
		let pos = $(item).position();
		let index = $(item).zIndex() + 1;
		let height = $(item).outerHeight();
		let width = $(item).outerWidth();

		let overDiv = "<div class='ui-div-to-disabled' style='left:" + pos.left + "px;height:" + height + "px;position:absolute;top:" + pos.top + "px;width:" + width + "px;z-index:" + index + "'></div>";
		$(overDiv).insertAfter(item);
	});
} //function toDisable

/**
* Este método permite habilitar componentes de la UI mediante un selector
* el selector podrá ser cualquier identificador válido de CSS o un objeto
* jQuery y en caso de no especificarlo, se buscarán los elementos con la
* clase .oneClick
* A los elementos habilitados se les removerá la clase ui-state-disabled
* a fin de mantener el estandar de formato del plug-in UI de jQuery.
*/
function toEnable(selector) {
	selector = selector || '.oneClick';
	$(selector).prop('disabled', false).removeClass('ui-state-disabled');
	$(".ui-div-to-disabled").remove();
} //function toEnable