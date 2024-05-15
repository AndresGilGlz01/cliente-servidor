let body = document.querySelector('body');
let btnUp = document.querySelector('#up-button');
let frmFiltrosForm = document.querySelector("#aside");
let btnCloseDetails = document.querySelector('#details-close');
let actividades = document.querySelectorAll('.actividad');
let actividadDetails = document.querySelector('.actividad-details');

if (frmFiltrosForm) {
    let btnOpenFiltrosForm = document.querySelector('#aside-open');
    let btnCloseFiltrosForm = document.querySelector('#aside-close');
    btnOpenFiltrosForm.addEventListener('click', (e) => {
        frmFiltrosForm.style.animationName = "open-aside";
    });
    
    btnCloseFiltrosForm.addEventListener('click', (e) => {
        frmFiltrosForm.style.animationName = "close-aside";
    });
}

btnUp.addEventListener('click', (e) => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

actividades.forEach((actividad) => {
    actividad.addEventListener('click', (e) => {
        actividadDetails.style.animationName = "open-details";

        let img = actividadDetails.querySelector('.actividad-details img');
        console.log(img);
    });
});

if (actividadDetails) {
    btnCloseDetails.addEventListener('click', (e) => {
        actividadDetails.style.animationName = "close-details";
    });
}