const MSG_TYPE_ALERT = 0;
const MSG_TYPE_INFO = 1;
const MSG_TYPE_QUESTION = 2;
const MSG_TYPE_OK = 3;

var messageDialog = null;

const alertIcon = `<svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" class="bi flex-shrink-0 me-3" width="32" height="32" aria-label="Alerta:" viewBox="0 0 16 16">
                        <path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/>
                   </svg>`;
const questionIcon = `<svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" class="bi flex-shrink-0 me-3" width="32" height="32"aria-label="Pregunta:" viewBox="0 0 16 16">
                           <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.496 6.033h.825c.138 0 .248-.113.266-.25.09-.656.54-1.134 1.342-1.134.686 0 1.314.343 1.314 1.168 0 .635-.374.927-.965 1.371-.673.489-1.206 1.06-1.168 1.987l.003.217a.25.25 0 0 0 .25.246h.811a.25.25 0 0 0 .25-.25v-.105c0-.718.273-.927 1.01-1.486.609-.463 1.244-.977 1.244-2.056 0-1.511-1.276-2.241-2.673-2.241-1.267 0-2.655.59-2.75 2.286a.237.237 0 0 0 .241.247zm2.325 6.443c.61 0 1.029-.394 1.029-.927 0-.552-.42-.94-1.029-.94-.584 0-1.009.388-1.009.94 0 .533.425.927 1.01.927z"/>
                      </svg>`;
const infoIcon = `<svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" class="bi flex-shrink-0 me-3" width="32" height="32"aria-label="Información:" viewBox="0 0 16 16">
                       <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z" />
                  </svg>`;
const okIcon = `<svg xmlns="http://www.w3.org/2000/svg" fill="currentColor" class="bi flex-shrink-0 me-3" width="32" height="32"aria-label="Información:" viewBox="0 0 16 16">
                       <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                  </svg>`;

const messageDialogHTML = `<div id="messageDialog" class="modal fade" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
                         <div class="modal-dialog modal-dialog-centered">
                           <div class="modal-content">
                             <div class="modal-header">
                               <h5 id="titleContainer" class="modal-title"></h5>
                               <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                             </div>
                             <div id="modalBody" class="modal-body alert d-flex align-items-center"></div>
                             <div id="buttonsContainer" class="modal-footer">
                             </div>
                           </div>
                         </div>
                     </div>`;

document.addEventListener("DOMContentLoaded", async function (event) {
    let messageDialogDOM = document.getElementById('messageDialog');
    if (messageDialogDOM == null) {
        document.body.innerHTML += messageDialogHTML;
        messageDialogDOM = document.getElementById('messageDialog');
    }
    messageDialog = new bootstrap.Modal(messageDialogDOM);
});

function showMessage(title, message, type, funcOK, funcCancel) {
    let titleContainer = document.getElementById('titleContainer');
    let modalBody = document.getElementById('modalBody');
    let icon = "";
    let buttonsContainer = document.getElementById('buttonsContainer');

    titleContainer.setHTML(title, { sanitizer: new Sanitizer() });

    let cancelbutton = null;
    let color = "";

    switch (type) {
        case MSG_TYPE_ALERT:
            color = "alert-warning";
            icon = alertIcon;
            break;
        case MSG_TYPE_OK:
            color = "alert-success";
            icon = okIcon;
            break;
        case MSG_TYPE_QUESTION:
            color = "alert-primary";
            icon = questionIcon;
            break;
        default:
            color = "alert-info";
            icon = infoIcon;
            break;
    };

    modalBody.innerHTML = `<div class="w-100 alert align-items-center d-flex ${color}">${icon}<div>${message}</div></div>`;

    if (type == MSG_TYPE_QUESTION) {
        buttonsContainer.innerHTML = `<button id="cancelbutton" type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                      <button id="okbutton" type="button" class="btn btn-primary" data-bs-dismiss="modal">Confirmar</button>`;
    }
    else {
        buttonsContainer.innerHTML = `<button id="okbutton" type="button" class="btn btn-primary" data-bs-dismiss="modal">Ok</button>`;
    }

    let okbutton = document.getElementById('okbutton');

    if (typeof funcOK === 'function' && okbutton != null) {
        okbutton.addEventListener('click', funcOK);
    }
    if (typeof funcCancel === 'function' && cancelbutton != null) {
        cancelbutton.addEventListener('click', funcCancel);
    }
    

    messageDialog.show();
}

function hideMessage() {
    let titleContainer = document.getElementById('titleContainer');
    let modalBody = document.getElementById('modalBody');
    let buttonsContainer = document.getElementById('buttonsContainer');

    messageDialog.hide();

    titleContainer.innerHTML = '';
    modalBody.innerHTML = '';
    buttonsContainer.innerHTML = '';

    setTimeout(function () {
        let modal = document.getElementById('messageDialog');
        if (modal.classList.contains('show')) {
            hideMessage();
        }
    }, 200);
}