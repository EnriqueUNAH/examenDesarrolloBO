$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/CanalServicio';

    $('#canalServicioTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idCanalServicio" },
            { "data": "codigoCanalServicio" },
            { "data": "nombreCanalServicio" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditCanalServicioModal(${row.idCanalServicio})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteCanalServicioModal(${row.idCanalServicio})"><i class="fas fa-trash-alt"></i></button>
                    `;
                }
            }
        ],
        "language": {
            "lengthMenu": "Mostrar _MENU_ entradas",
            "zeroRecords": "No se encontraron resultados",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            "infoEmpty": "Mostrando 0 a 0 de 0 entradas",
            "infoFiltered": "(filtrado de _MAX_ entradas totales)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });

    window.showEditCanalServicioModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editCanalServicioId').val(data.idCanalServicio);
                $('#editCodigoCanalServicio').val(data.codigoCanalServicio);
                $('#editNombreCanalServicio').val(data.nombreCanalServicio);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(new Date().toISOString());
                $('#editCanalServicioModal').modal('show');
            }
        });
    };

    window.showDeleteCanalServicioModal = function (id) {
        $('#deleteCanalServicioId').val(id);
        $('#deleteCanalServicioModal').modal('show');
    };

    $('#editCanalServicioForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editCanalServicioId').val();
        const data = {
            idCanalServicio: id,
            codigoCanalServicio: $('#editCodigoCanalServicio').val(),
            nombreCanalServicio: $('#editNombreCanalServicio').val(),
            idUsuario: $('#editIdUsuario').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: new Date().toISOString()
        };

        fetch(`${apiBaseUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.message.includes('actualizado exitosamente')) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: 'Canal Servicio actualizado exitosamente',
                    });
                    $('#editCanalServicioModal').modal('hide');
                    $('#canalServicioTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al actualizar el Canal Servicio: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al actualizar el Canal Servicio:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al actualizar el Canal Servicio',
                });
            });
    });

    $('#deleteCanalServicioForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteCanalServicioId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                $('#deleteCanalServicioModal').modal('hide');
                $('#canalServicioTable').DataTable().ajax.reload();
            }
        });
    });

    $('#createCanalServicioForm').submit(function (e) {
        e.preventDefault();
        const data = {
            codigoCanalServicio: $('#createCodigoCanalServicio').val(),
            nombreCanalServicio: $('#createNombreCanalServicio').val(),
            idUsuario: $('#createIdUsuario').val(),
            fechaRegistro: new Date().toISOString(),
            fechaModificado: new Date().toISOString()
        };

        fetch(apiBaseUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.message.includes('creado exitosamente')) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: 'Canal Servicio creado exitosamente',
                    });
                    $('#createCanalServicioModal').modal('hide');
                    $('#canalServicioTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al crear el Canal Servicio: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al crear el Canal Servicio:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear el Canal Servicio',
                });
            });
    });
});
