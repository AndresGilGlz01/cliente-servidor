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
    actividad.addEventListener('click', async (e) => {
        actividadDetails.style.animationName = "open-details";

        let id = actividad.getAttribute('data-id');

        let url = `https://sga.api.labsystec.net/api/actividades/${id}`;

        let request = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        let response = await request.json();

        console.log(response);

        let descripcion = response.descripcion;
        let estado = response.estado;
        let fechaActualizacion = response.fechaActualizacion;
        let fechaCreacion = response.fechaCreacion;
        let fechaRealizacion = response.fechaRealizacion;
        let titulo = response.titulo;

        let detailsTitle = document.querySelector('.details-title');

        let template = `
        ${titulo} <br>
                <a class="details-dpto" href="javascript:">
                    Depto. Ingeniería
                    Sistemas Computacionales
                </a>
                <span class="details-date details-date-create">Creado el ${fechaCreacion}</span>
                <span class="details-date">
                    &middot; Ultima
                    actualizacion el ${fechaActualizacion}
                </span>
                <span class="details-date"> &middot; Realizado el ${fechaRealizacion}</span>
        `;

        let detailsDescription = document.querySelector('#mytextarea');
        let image = document.querySelector('.details-img');

        image.src = `https://sga.api.labsystec.net/images/${id}.png`;

        tinymce.activeEditor.setContent(descripcion);

        detailsTitle.innerHTML = template;
    });
});

if (actividadDetails) {
    btnCloseDetails.addEventListener('click', (e) => {
        actividadDetails.style.animationName = "close-details";
    });
}