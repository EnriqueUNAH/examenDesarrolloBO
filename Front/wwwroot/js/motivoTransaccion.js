$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/MotivoTransaccion';

    $('#motivoTransaccionTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
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
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editIdMotivoTransaccion').val(data.idMotivoTransaccion);
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
        $('#deleteIdMotivoTransaccion').val(id);
        $('#deleteMotivoTransaccionModal').modal('show');
    };

    $('#createMotivoTransaccionForm').submit(function (event) {
        event.preventDefault();
        const now = new Date().toISOString();
        const motivoTransaccion = {
            idTipoTransaccion: parseInt($('#createIdTipoTransaccion').val(), 10),
            codigoMotivoTransaccion: $('#createCodigoMotivoTransaccion').val(),
            nombreMotivoTransaccion: $('#createNombreMotivoTransaccion').val(),
            idUsuario: parseInt($('#createIdUsuario').val(), 10),
            fechaRegistro: now,
            fechaModificado: now
        };

        $.ajax({
            url: apiBaseUrl,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(motivoTransaccion),
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Motivo de transacción creado correctamente',
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#createMotivoTransaccionModal').modal('hide');
                $('#motivoTransaccionTable').DataTable().ajax.reload();
            },
            error: function () {
                alert('Error al crear el motivo de transacción');
            }
        });
    });

    $('#editMotivoTransaccionForm').submit(function (event) {
        event.preventDefault();
        const id = $('#editIdMotivoTransaccion').val();
        const motivoTransaccion = {
            idMotivoTransaccion: parseInt(id, 10),
            idTipoTransaccion: parseInt($('#editIdTipoTransaccion').val(), 10),
            codigoMotivoTransaccion: $('#editCodigoMotivoTransaccion').val(),
            nombreMotivoTransaccion: $('#editNombreMotivoTransaccion').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: new Date().toISOString(),
            idUsuario: parseInt($('#editIdUsuario').val(), 10)
        };

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(motivoTransaccion),
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Motivo de transacción actualizado correctamente',
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#editMotivoTransaccionModal').modal('hide');
                $('#motivoTransaccionTable').DataTable().ajax.reload();
            },
            error: function () {
                alert('Error al actualizar el motivo de transacción');
            }
        });
    });

    $('#deleteMotivoTransaccionForm').submit(function (event) {
        event.preventDefault();
        const id = $('#deleteIdMotivoTransaccion').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Motivo de transacción eliminado correctamente',
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#deleteMotivoTransaccionModal').modal('hide');
                $('#motivoTransaccionTable').DataTable().ajax.reload();
            },
            error: function () {
                alert('Error al eliminar el motivo de transacción');
            }
        });
    });
});
