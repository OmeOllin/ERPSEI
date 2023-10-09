// Loaded via <script> tag, create shortcut to access PDF.js exports.
var pdfjsLib = window['pdfjs-dist/build/pdf'];

// The workerSrc property shall be specified.
pdfjsLib.GlobalWorkerOptions.workerSrc = '../../../lib/pdfjs/pdf.worker.js';

var maxFileSizeInBytes = 1000000;
document.addEventListener("DOMContentLoaded", async function (event) {
    showLoading();

    await renderFilesAsync();
    await initializeDisableableButtonsAsync();

    hideLoading()
});

//Función para capturar el clic en el botón de edición, que dispara la apertura del selector de archivo.
function onEditDocumentClick(button) {
    let inputName = button.getAttribute("inputName");
    document.getElementById(inputName).click();
}

//Función para mostrar la foto de perfil seleccionada.
function onProfilePicSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) { return; }
        let imgType = input.files[0].type;
        if (imgType == "image/png" || imgType == "image/jpg" || imgType == "image/jpeg") {
            document.getElementById("profilePicContainer").src = window.URL.createObjectURL(input.files[0]);
        }
    }
}

//Función para mostrar cualquiera de los documentos seleccionados.
async function onDocumentSelectorChanged(input) {
    if (input.files && (input.files.length || 0) >= 1) {
        if (input.files[0].size >= maxFileSizeInBytes) {
            showMessage("Tama&ntilde;o de archivo inv&aacute;lido", `El tama&ntilde;o del archivo no debe superar ${maxFileSizeInBytes / 1000000}Mb. Por favor elija otro archivo.`);
            return;
        }
        let docType = input.files[0].type;
        let isImg = docType == "image/png" || docType == "image/jpg" || docType == "image/jpeg";
        let isPDF = docType == "application/pdf";
        let containerName = input.getAttribute("containerName");
        let showerName = containerName + "_children";
        let container = document.getElementById(containerName);

        if (isImg) {
            let src = window.URL.createObjectURL(input.files[0]);
            container.setHTML(`<img id="${showerName}" src="${src}" style="width:100%; height: auto; max-height: 200px;" />`, { sanitizer: new Sanitizer() });
        }
        else if (isPDF) {
            showLoading();
            container.setHTML(`<canvas id="${showerName}" class="canvaspdf"></canvas>`, { sanitizer: new Sanitizer() });
            await loadPDFFromFileAsync(input.files[0], showerName);
            hideLoading();
        }
        else {
            showMessage("Formato de archivo inv&aacute;lido", `El formato del archivo debe ser .jpg, .jpeg o .png. Por favor elija otro archivo.`);
        }
    }
}

//Función para la carga y render de archivos pdf
async function loadPDFAsync(pdfData, showerName) {
    // Using DocumentInitParameters object to load binary data.
    let desiredWidth = 200;
    var loadingTask = pdfjsLib.getDocument({ data: pdfData });

    loadingTask.promise.then(async function (pdf) {
        // Fetch the first page
        var pageNumber = 1;

        pdf.getPage(pageNumber).then(async function (page) {
            var scale = 1;
            var viewport = page.getViewport({ scale: scale });
            scale = desiredWidth / viewport.width;
            viewport = page.getViewport({ scale: scale });

            // Prepare canvas using PDF page dimensions
            var canvas = document.getElementById(showerName);
            var context = canvas.getContext('2d');
            canvas.height = 200;
            canvas.width = viewport.width;

            // Render PDF page into canvas context
            var renderContext = {
                canvasContext: context,
                viewport: viewport
            };
            page.render(renderContext);
        });
    }, function (reason) {
        // PDF loading error
        console.error(reason);
    });
}

//Función para la carga y render de archivos pdf a partir de un archivo seleccionado.
async function loadPDFFromFileAsync(file, showerName) {
    await new Promise((resolve, reject) => {
        var fr = new FileReader();
        fr.onload = () => { resolve(fr.result) };
        fr.onerror = reject;
        fr.readAsArrayBuffer(file);
    }).then(async function(result){
        var pdfData = new Uint8Array(result);
        await loadPDFAsync(pdfData, showerName)
    }, function (error) {
        console.log(error);
    });
}

//Función para renderizar los archivos en formato pdf.
async function renderFilesAsync() {
    let elements = document.getElementsByClassName("canvaspdf");
    if (elements.length <= 0) { return; }

    for (let i = 0; i < elements.length; i++) {
        let element = elements[i];
        let id = element.getAttribute("id");
        let b64 = element.getAttribute("b64");
        let pdfData = atob(b64);
        await loadPDFAsync(pdfData, id);
    }
}

//Función para habilitar/deshabilitar los botones de visualización en base a si existe contenido o no para visualizar.
async function initializeDisableableButtonsAsync() {
    //Botones de acción de editar y eliminar
    let buttons = document.getElementsByClassName("disableable");

    //Se inicializan los botones de edición
    if (buttons.length >= 1) {
        for (let i = 0; i < buttons.length; i++) {
            let button = buttons[i];
            let sourceLength = parseInt(button.getAttribute("sourceLength") || "0");
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

function onDeleteClick(button) {
    let sourceId = button.getAttribute("sourceId") || "";

    if (sourceId.length >= 1) {
        let fileInput = document.getElementById(sourceId);
        let containerName = fileInput.getAttribute("containerName") || "";
        fileInput.files = null;
        if (containerName.length >= 1) {
            let container = document.getElementById(containerName);
            container.innerHTML = "";
        }
    }
}