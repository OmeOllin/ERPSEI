const maxFileSizeInBytes = 5242880; //5mb = (5 * 1024) * 1024;
const oneMegabyteSizeInBytes = 1048576; // 1mb = (1 * 1024) * 1024

document.addEventListener("DOMContentLoaded", async function (event) {
    showLoading();

    initializeDisableableButtons();

    hideLoading()

    //Este evento es necesario para poder mostrar el text area ajustado al tamaño del contenido, basado en el tamaño del scroll.
    calculateTextAreaHeight(document.querySelectorAll("textarea"));
});
//Función para redimensionar los textareas cada que cambie el tamaño de pantalla.
window.addEventListener('resize', function (event) {
    calculateTextAreaHeight(document.querySelectorAll("textarea"));
}, true);

//Functión para capturar el clic del botón guardar
function onUpdateProfileButtonClick() {
    if ($('#profile-form').valid()) { showLoading(); }
}

//Función para capturar el clic en el botón de edición, que dispara la apertura del selector de archivo.
function onEditDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    document.getElementById(inputName).click();
}

//Función para mostrar la foto de perfil seleccionada.
function onProfilePicSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            input.value = null;
            showAlert(maxFileSizeTitle, `${maxFileSizeMessage} ${maxFileSizeInBytes / oneMegabyteSizeInBytes}Mb`);
            return;
        }
        let imgType = input.files[0].type;
        if (imgType == "image/png" || imgType == "image/jpg" || imgType == "image/jpeg") {
            document.getElementById("profilePicContainer").src = window.URL.createObjectURL(input.files[0]);
        }
        else {
            showAlert(fileFormatTitle, fileFormatMessage);
        }
    }
}

//Función para mostrar cualquiera de los documentos seleccionados.
function onDocumentSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            input.value = null;
            showAlert(maxFileSizeTitle, `${maxFileSizeMessage} ${maxFileSizeInBytes / oneMegabyteSizeInBytes}Mb`);
            return;
        }
        let docType = input.files[0].type;
        let docParts = input.files[0].name.split(".");
        let fName = docParts.length >= 1 ? docParts.slice(0, -1).join(".") || "" : "";
        let fExt = docParts.length >= 2 ? docParts[docParts.length - 1] || "" : "";
        let isImg = docType == "image/png" || docType == "image/jpg" || docType == "image/jpeg";
        let isPDF = docType == "application/pdf";
        let containerName = input.getAttribute("containerName");
        let fileIconName = input.getAttribute("fileIconName");
        let fileNameName = input.getAttribute("fileNameName");
        let container = document.getElementById(containerName);
        let fileIcon = document.getElementById(fileIconName);
        let fileName = document.getElementById(fileNameName);

        if (isImg || isPDF) {
            var reader = new FileReader();
            reader.onload = function () {

                var arrayBuffer = this.result,
                    binary = '',
                    bytes = new Uint8Array(arrayBuffer),
                    len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                input.setAttribute("b64", window.btoa(binary));
                input.setAttribute("sourceLength", "0");

                //Se elimina menú para ver el documento, en caso de haber uno anterior.
                let inputName = input.getAttribute("id");
                let menuVer = document.querySelector(`a.dropdown-item.see[inputName='${inputName}']`);
                if (menuVer != null) { menuVer.parentElement.remove(); }

                initializeDisableableButtons();
            }
            reader.readAsArrayBuffer(input.files[0]);

            container.classList.remove("document-container-empty");
            container.classList.add("document-container-filled");

            fileIcon.classList.remove("opacity-25");
            fileIcon.classList.add("document-icon-filled");

            fileName.classList.remove("opacity-25");
            fileName.classList.add("document-name-filled");
            fileName.innerHTML = `<div class="overflowed-text">${fName}</div>.<div>${fExt}</div>`;
        }
        else {
            input.value = null;
            showAlert(fileFormatTitle, fileFormatMessage);
        }
    }
}

//Función para habilitar/deshabilitar los botones de visualización en base a si existe contenido o no para visualizar.
function initializeDisableableButtons() {
    //Botones de acción de editar y eliminar
    let buttons = document.getElementsByClassName("disableable");

    //Se inicializan los botones de edición
    if (buttons.length >= 1) {
        for (let i = 0; i < buttons.length; i++) {
            let button = buttons[i];
            let inputName = button.getAttribute("inputName");
            let input = document.getElementById(inputName);
            let sourceLength = (input.getAttribute("b64") || "").length;
            if (sourceLength <= 0) { sourceLength = parseInt(input.getAttribute("sourceLength") || "0"); }
            let hasFile = sourceLength >= 1;
            if (hasFile) {
                button.classList.remove("disabled");
            }
            else {
                button.classList.add("disabled");
            }
        }
    }
}

//Función para capturar el clic en el botón quitar, que elimina el archivo seleccionado.
function onDeleteClick(button) {
    let sourceId = button.getAttribute("sourceId") || "";

    if (sourceId.length >= 1) {
        let fileInput = document.getElementById(sourceId);
        let containerName = fileInput.getAttribute("containerName") || "";
        let fileIconName = fileInput.getAttribute("fileIconName");
        let fileNameName = fileInput.getAttribute("fileNameName");
        let sourceName = fileInput.getAttribute("sourceName");

        let container = document.getElementById(containerName);
        let fileIcon = document.getElementById(fileIconName);
        let fileName = document.getElementById(fileNameName);
        let fileSizeInput = document.querySelector(`input[name='FilesFromGet[${sourceName}].FileSize']`);

        //Se elimina menú para ver el documento.
        let menuVer = document.querySelector(`a.dropdown-item.see[inputName='${sourceId}']`);
        if (menuVer != null) { menuVer.parentElement.remove(); }

        fileInput.value = null;
        fileInput.setAttribute("b64", "");
        fileSizeInput.setAttribute("value", 0);

        container.classList.remove("document-container-filled");
        container.classList.add("document-container-empty");

        fileIcon.classList.remove("document-icon-filled");
        fileIcon.classList.add("opacity-25");

        fileName.classList.remove("document-name-filled");
        fileName.classList.add("opacity-25");
        fileName.innerHTML = `<div class="overflowed-text">${emptySelectItemText}</div>`;
    }

    initializeDisableableButtons();
}
//Función para capturar el clic en el botón de ver, que dispara la visualización del archivo.
function onVerDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    let input = document.getElementById(inputName);
    let oParams = { safeL: input.getAttribute("safeL") }

    if (oParams.safeL.length >= 1) { window.open(`/FileViewer?safeL=${encodeURIComponent(oParams.safeL)}`, "_blank"); }
}