$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/TipoTransaccion';

    // Inicializar DataTable
    $('#tipoTransaccionTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idTipoTransaccion" },
            { "data": "codigoTipoMovimiento" },
            { "data": "codigoTipoTransaccion" },
            { "data": "nombreTipoTransaccion" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificacion" },  // Asegúrate de que el nombre coincida con los datos del servidor
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditTipoTransaccionModal(${row.idTipoTransaccion})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteTipoTransaccionModal(${row.idTipoTransaccion})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditTipoTransaccionModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editTipoTransaccionId').val(data.idTipoTransaccion);
                $('#editCodigoTipoMovimiento').val(data.codigoTipoMovimiento);
                $('#editCodigoTipoTransaccion').val(data.codigoTipoTransaccion);
                $('#editNombreTipoTransaccion').val(data.nombreTipoTransaccion);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificacion').val(new Date().toISOString());
                $('#editIdUsuario').val(data.idUsuario);
                $('#editTipoTransaccionModal').modal('show');
            }
        });
    };

    window.showDeleteTipoTransaccionModal = function (id) {
        $('#deleteTipoTransaccionId').val(id);
        $('#deleteTipoTransaccionModal').modal('show');
    };

    $('#editTipoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editTipoTransaccionId').val();
        const data = {
            idTipoTransaccion: id,
            codigoTipoMovimiento: $('#editCodigoTipoMovimiento').val(),
            codigoTipoTransaccion: $('#editCodigoTipoTransaccion').val(),
            nombreTipoTransaccion: $('#editNombreTipoTransaccion').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificacion: new Date().toISOString(),
            idUsuario: $('#editIdUsuario').val()
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
                if (data.message) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: 'Tipo Transacción actualizado exitosamente',
                    });
                    $('#editTipoTransaccionModal').modal('hide');
                    $('#tipoTransaccionTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al actualizar el Tipo Transacción: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al actualizar el Tipo Transacción:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al actualizar el Tipo Transacción',
                });
            });
    });

    $('#deleteTipoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteTipoTransaccionId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Tipo Transacción eliminado exitosamente',
                });
                $('#deleteTipoTransaccionModal').modal('hide');
                $('#tipoTransaccionTable').DataTable().ajax.reload();
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al eliminar el Tipo Transacción',
                });
            }
        });
    });

    $('#createTipoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const data = {
            codigoTipoMovimiento: $('#createCodigoTipoMovimiento').val(),
            codigoTipoTransaccion: $('#createCodigoTipoTransaccion').val(),
            nombreTipoTransaccion: $('#createNombreTipoTransaccion').val(),
            fechaRegistro: new Date().toISOString(),
            fechaModificacion: new Date().toISOString(),
            idUsuario: $('#createIdUsuario').val()
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
                if (data.message) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Éxito',
                        text: 'Tipo Transacción creado exitosamente',
                    });
                    $('#createTipoTransaccionModal').modal('hide');
                    $('#tipoTransaccionTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al crear el Tipo Transacción: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al crear el Tipo Transacción:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear el Tipo Transacción',
                });
            });
    });
});
