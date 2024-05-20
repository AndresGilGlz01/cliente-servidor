let body = document.querySelector('body');
let btnUp = document.querySelector('#up-button');
let frmFiltrosForm = document.querySelector("#aside");
let btnCloseDetails = document.querySelector('#details-close');
let actividades = document.querySelectorAll('.actividad');
let actividadDetails = document.querySelector('.actividad-details');
let userIdDepartamento = parseInt(document.getElementById("userIdDepartamento").dataset.userId || "0");
let roleDiv = document.getElementById('role');
let userrole = roleDiv.getAttribute('data-user-role');

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

        let url = `https://sga.api.labsystec.net/api/actividad/${id}`;

        let request = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        let response = await request.json();

        console.log(response);

        let descripcion = response.descripcion;
        let departamento = response.departamento;
        let estado = response.estado;
        let fechaActualizacion = response.fechaActualizacion;
        let fechaCreacion = response.fechaCreacion;
        let fechaRealizacion = response.fechaRealizacion;
        let titulo = response.titulo;
        let idepa = response.idDepartamento;


        // formatear fechas
        let fechaCreacionDate = new Date(fechaCreacion);
        let fechaActualizacionDate = new Date(fechaActualizacion);
        let fechaRealizacionDate = new Date(fechaRealizacion);

        fechaCreacion = fechaCreacionDate.toLocaleDateString();
        fechaActualizacion = fechaActualizacionDate.toLocaleDateString();
        fechaRealizacion = fechaRealizacionDate.toLocaleDateString();

        let detailsTitle = document.querySelector('.details-title');

        let template = `
        ${titulo} <br>
                <a class="details-dpto" href="javascript:">
                    ${departamento}
                </a> <br>
                <span class="details-date details-date-create">Creado el ${fechaCreacion}</span>
                <span class="details-date">
                    &middot; Ultima
                    actualizacion el ${fechaActualizacion}
                </span>
                <span class="details-date"> &middot; Realizado el ${fechaRealizacion}</span>
        `;
        let image = document.querySelector('.details-img');

        image.src = `https://sga.api.labsystec.net/images/${id}.png`;

        image.onerror = function() {
            image.src = `https://sga.api.labsystec.net/images/0.png`;
        }

        let btnModificar = document.querySelector('.details-modify');
        let btnEliminar = document.querySelector('.details-delete');

        btnModificar.href = `/${userrole}/home/editar/${id}`;
        btnEliminar.href = `/${userrole}/home/eliminar/${id}`;

        if (idepa != userIdDepartamento) {
            btnModificar.style.display = "none";
            btnEliminar.style.display = "none";
        } else {
            btnModificar.style.display = "block";
            btnEliminar.style.display = "block";
            btnModificar.href = `/${userrole}/home/editar/${id}`;
            btnEliminar.href = `/${userrole}/home/eliminar/${id}`;
        }

        document.querySelector('.details-body-description').innerHTML = descripcion;

        detailsTitle.innerHTML = template;
    });
});

if (actividadDetails) {
    btnCloseDetails.addEventListener('click', (e) => {
        actividadDetails.style.animationName = "close-details";
    });
}