/**
	* Método estándar para la ejecución de solicitudes tipo AJAX.
	* realiza la validación de errores en la solicitud y al recibir el
	* objeto de regreso, aplica parseo a JSON de ser necesario y envía
	* los errores a la consola del navegador para su depuración.
	*
	* @param object 	url	    Url de petición al servidor.
	* @param object	oParams	Contiene todos los valores que se enviarán
	* 							al método que será ejecutado en el servidor.
	* @param object 	onSuccess Objeto de función que será ejecutado al
	* 							completar la ejecución del servidor de manera
	*                           satisfactoria y que recibirá los resultados 
	*						    del proceso en un único objeto tipo JSON.
	* @param object 	onError Objeto de función que será ejecutado al
	* 							completar la ejecución del servidor de manera
	*                           erronea y que recibirá los resultados 
	*						    del proceso en un único objeto tipo JSON.
	* @param object	oOptions Objeto con opciones adicionales para el
	* 							método ajax de jQuery.
*/
function doAjax(url = '', oParams = null, onSuccess = function () { }, onError = function () { }, oOptions = null) {
	let datos = { };

	//Si el arreglo oParams es distinto de null
	if (typeof oParams === 'object' && !Array.isArray(oParams)) {
		if (oParams != null)
			//Recorremos el objeto y asignamos cada propiedad al objeto a enviar datos
			$.extend(datos, oParams);
	} else {
		console.warn(oParams);
		console.warn('El argumento oParams pasado a la funci&oacute;n no es un objeto v&aacute;lido.');
		return false;
	} //if

	// Parámetros necesarios para el procedimiento AJAX
	let objDefaults = {
		url: url,
		data: datos,
		type: 'POST',
		dataType: "json",
		success: function (objRespuesta) { //Si la llamada es satisfactoria
			try {

				if (typeof objRespuesta !== 'object') objRespuesta = JSON.parse(objRespuesta);
				if (typeof onSuccess === 'function') onSuccess(objRespuesta);


			} catch (error) {
				console.warn(error);
				if (typeof onError === 'function') onError(error);
			} //try
		},
		error: function (xhr, estado, errdata) { //En caso de error de comunicaciones o del sitio
			console.warn(estado);
			console.warn(errdata);
			onError([JSON.stringify(estado), JSON.stringify(errdata)].join());
			return false;
		}
	};

	// Incluye los parámetros opcionales recibidos
	if (typeof oOptions === 'object' && !Array.isArray(oOptions)) {
		if (oOptions != null)
			//Recorremos el objeto y asignamos cada propiedad al objeto a enviar datosRegistro
			$.extend(objDefaults, oOptions);
	} else {
		console.warn(oOptions);
		console.warn('El argumento oOptions pasado a la funci&oacute;n no es un objeto v&aacute;lido.');
		return false;
	} //if

	// Llamado ajax
	return $.ajax(objDefaults);
} //doAjax