let loadingHTML = `<div id="modal" class="modal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
                       <div class="modal-dialog modal-sm modal-dialog-centered">
                           <span class="spinner-border text-info position-fixed" style="left: 50%;"></span>
                       </div>
                   </div>
                   <button id="showLoading" type="button" data-bs-toggle="modal" data-bs-target="#modal" hidden></button>
                   <button id="hideLoading" type="button" data-bs-dismiss="modal" data-bs-target="#modal" hidden></button>`;

document.addEventListener("DOMContentLoaded", async function (event) {
    let modal = document.getElementById('modal');
    if (modal == null) { document.body.innerHTML += loadingHTML; }

    $(document).ajaxStart(showLoading).ajaxStop(hideLoading);
});

function showLoading() {
    let button = document.getElementById('showLoading');
    button.click();
}

function hideLoading() {
    let button = document.getElementById('hideLoading');
    button.click();
    setTimeout(function () {
        let modal = document.getElementById('modal');
        if (modal.classList.contains('show')) {
            hideLoading();
        }
    }, 200);
}