$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/Agencias';

    $('#agenciasTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idAgencia" },
            { "data": "idCanalServicio" },
            { "data": "codigoAgencia" },
            { "data": "nombreAgencia" },
            { "data": "direccionAgencia" },
            { "data": "telefonoAgencia" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditUserModal(${row.idAgencia})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteUserModal(${row.idAgencia})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditUserModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editAgenciaId').val(data.idAgencia);
                $('#editIdCanalServicio').val(data.idCanalServicio);
                $('#editCodigoAgencia').val(data.codigoAgencia);
                $('#editNombreAgencia').val(data.nombreAgencia);
                $('#editDireccionAgencia').val(data.direccionAgencia);
                $('#editTelefonoAgencia').val(data.telefonoAgencia);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(new Date().toISOString()); // Fecha actual para fechaModificado
                $('#editIdUsuario').val(data.idUsuario);
                $('#editAgenciaModal').modal('show');
            }
        });
    };

    window.showDeleteUserModal = function (id) {
        $('#deleteIdAgencia').val(id);
        $('#deleteAgenciaModal').modal('show');
    };

    $('#editAgenciaForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editAgenciaId').val();
        const data = {
            idAgencia: id,
            idCanalServicio: $('#editIdCanalServicio').val(),
            codigoAgencia: $('#editCodigoAgencia').val(),
            nombreAgencia: $('#editNombreAgencia').val(),
            direccionAgencia: $('#editDireccionAgencia').val(),
            telefonoAgencia: $('#editTelefonoAgencia').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: $('#editFechaModificado').val(),
            idUsuario: $('#editIdUsuario').val()
        };

        fetch(`${apiBaseUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.ok) {
                    alert('Agencia actualizada exitosamente');
                    window.location.reload();
                } else {
                    throw new Error('Error al actualizar la Agencia');
                }
            })
            .catch(error => {
                console.error('Error al actualizar la Agencia:', error);
                alert('Error al actualizar la Agencia');
            });
    });

    $('#deleteAgenciaForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteIdAgencia').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                $('#deleteAgenciaModal').modal('hide');
                $('#agenciasTable').DataTable().ajax.reload();
            }
        });
    });

    $('#createAgenciaForm').submit(function (e) {
        e.preventDefault();
        const data = {
            idCanalServicio: $('#createIdCanalServicio').val(),
            codigoAgencia: $('#createCodigoAgencia').val(),
            nombreAgencia: $('#createNombreAgencia').val(),
            direccionAgencia: $('#createDireccionAgencia').val(),
            telefonoAgencia: $('#createTelefonoAgencia').val(),
            idUsuario: $('#createIdUsuario').val(),
            fechaRegistro: new Date().toISOString(), // Fecha actual para fechaRegistro
            fechaModificado: new Date().toISOString() // Fecha actual para fechaModificado
        };

        fetch(apiBaseUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.ok) {
                    alert('Agencia creada exitosamente');
                    window.location.reload();
                } else {
                    throw new Error('Error al crear la Agencia');
                }
            })
            .catch(error => {
                console.error('Error al crear la Agencia:', error);
                alert('Error al crear la Agencia');
            });
    });
});
