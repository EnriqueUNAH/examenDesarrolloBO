$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/Usuarios';

    // Inicializar DataTable
    $('#usuariosTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idUsuario" },
            { "data": "codigoUsuario" },
            { "data": "nombreUsuario" },
            { "data": "passwordUsuario" },
            { "data": "isActivo", "render": function (data) { return data ? 'Sí' : 'No'; } },
            { "data": "ultimaConexion" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuarioRegistro" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditUsuarioModal(${row.idUsuario})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteUsuarioModal(${row.idUsuario})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditUsuarioModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editUsuarioId').val(data.idUsuario);
                $('#editCodigoUsuario').val(data.codigoUsuario);
                $('#editNombreUsuario').val(data.nombreUsuario);
                $('#editPasswordUsuario').val(data.passwordUsuario);
                $('#editIsActivo').val(data.isActivo ? '1' : '0');
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(new Date().toISOString());
                $('#editIdUsuarioRegistro').val(data.idUsuarioRegistro);
                $('#editUsuarioModal').modal('show');
            }
        });
    };

    window.showDeleteUsuarioModal = function (id) {
        $('#deleteUsuarioId').val(id);
        $('#deleteUsuarioModal').modal('show');
    };

    $('#editUsuarioForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editUsuarioId').val();
        const data = {
            idUsuario: id,
            codigoUsuario: $('#editCodigoUsuario').val(),
            nombreUsuario: $('#editNombreUsuario').val(),
            passwordUsuario: $('#editPasswordUsuario').val(),
            isActivo: $('#editIsActivo').val() === '1',
            ultimaConexion: new Date().toISOString(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: new Date().toISOString(),
            idUsuarioRegistro: $('#editIdUsuarioRegistro').val()
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
                        text: 'Usuario actualizado exitosamente',
                    }).then(() => {
                        $('#editUsuarioModal').modal('hide');
                        $('#usuariosTable').DataTable().ajax.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al actualizar el Usuario: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al actualizar el Usuario:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al actualizar el Usuario',
                });
            });
    });

    $('#deleteUsuarioForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteUsuarioId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Usuario eliminado exitosamente',
                }).then(() => {
                    $('#deleteUsuarioModal').modal('hide');
                    $('#usuariosTable').DataTable().ajax.reload();
                });
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al eliminar el Usuario',
                });
            }
        });
    });

    $('#createUsuarioForm').submit(function (e) {
        e.preventDefault();
        const data = {
            codigoUsuario: $('#createCodigoUsuario').val(),
            nombreUsuario: $('#createNombreUsuario').val(),
            passwordUsuario: $('#createPasswordUsuario').val(),
            isActivo: $('#createIsActivo').val() === '1',
            ultimaConexion: new Date().toISOString(), // Enviar la fecha actual para ultimaConexion
            fechaRegistro: new Date().toISOString(),
            fechaModificado: new Date().toISOString(),
            idUsuarioRegistro: $('#createIdUsuarioRegistro').val()
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
                        text: 'Usuario creado exitosamente',
                    }).then(() => {
                        $('#createUsuarioModal').modal('hide');
                        $('#usuariosTable').DataTable().ajax.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al crear el Usuario: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al crear el Usuario:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear el Usuario',
                });
            });
    });
});
