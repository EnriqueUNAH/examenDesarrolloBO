$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api';

    $('#motivoTransaccionTable').DataTable({
        "ajax": {
            "url": `${apiBaseUrl}/MotivoTransaccion`,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idMotivoTransaccion" },
            { "data": "idTipoTransaccion" },
            { "data": "codigoMotivoTransaccion" },
            { "data": "nombreMotivoTransaccion" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditMotivoTransaccionModal(${row.idMotivoTransaccion})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteMotivoTransaccionModal(${row.idMotivoTransaccion})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditMotivoTransaccionModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/MotivoTransaccion/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editMotivoTransaccionId').val(data.idMotivoTransaccion);
                $('#editIdTipoTransaccion').val(data.idTipoTransaccion);
                $('#editCodigoMotivoTransaccion').val(data.codigoMotivoTransaccion);
                $('#editNombreMotivoTransaccion').val(data.nombreMotivoTransaccion);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(data.fechaModificado);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editMotivoTransaccionModal').modal('show');
            }
        });
    };

    window.showDeleteMotivoTransaccionModal = function (id) {
        $('#deleteMotivoTransaccionId').val(id);
        $('#deleteMotivoTransaccionModal').modal('show');
    };

    $('#editMotivoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editMotivoTransaccionId').val();
        const data = {
            idMotivoTransaccion: id,
            idTipoTransaccion: $('#editIdTipoTransaccion').val(),
            codigoMotivoTransaccion: $('#editCodigoMotivoTransaccion').val(),
            nombreMotivoTransaccion: $('#editNombreMotivoTransaccion').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: new Date().toISOString(),
            idUsuario: $('#editIdUsuario').val()
        };

        fetch(`${apiBaseUrl}/MotivoTransaccion/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.isSuccess) {
                    alert('Motivo de transacción actualizado exitosamente');
                    $('#editMotivoTransaccionModal').modal('hide');
                    $('#motivoTransaccionTable').DataTable().ajax.reload();
                } else {
                    alert('Error al actualizar el motivo de transacción');
                }
            })
            .catch(error => {
                console.error('Error al actualizar el motivo de transacción:', error);
                alert('Error al actualizar el motivo de transacción');
            });
    });

    $('#deleteMotivoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteMotivoTransaccionId').val();

        $.ajax({
            url: `${apiBaseUrl}/MotivoTransaccion/${id}`,
            method: 'DELETE',
            success: function () {
                $('#deleteMotivoTransaccionModal').modal('hide');
                $('#motivoTransaccionTable').DataTable().ajax.reload();
            }
        });
    });

    $('#createMotivoTransaccionForm').submit(function (e) {
        e.preventDefault();
        const data = {
            idTipoTransaccion: $('#createIdTipoTransaccion').val(),
            codigoMotivoTransaccion: $('#createCodigoMotivoTransaccion').val(),
            nombreMotivoTransaccion: $('#createNombreMotivoTransaccion').val(),
            fechaRegistro: new Date().toISOString(),
            fechaModificado: new Date().toISOString(),
            idUsuario: $('#createIdUsuario').val()
        };

        fetch(`${apiBaseUrl}/MotivoTransaccion`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.isSuccess) {
                    alert('Motivo de transacción creado exitosamente');
                    $('#createMotivoTransaccionModal').modal('hide');
                    $('#motivoTransaccionTable').DataTable().ajax.reload();
                } else {
                    alert('Error al crear el motivo de transacción');
                }
            })
            .catch(error => {
                console.error('Error al crear el motivo de transacción:', error);
                alert('Error al crear el motivo de transacción');
            });
    });
});
