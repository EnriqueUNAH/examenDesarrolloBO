$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/Transacciones';
    const motivosApiUrl = 'https://localhost:7106/api/MotivoTransaccion';
    const agenciasApiUrl = 'https://localhost:7106/api/Agencias';
    const clientesApiUrl = 'https://localhost:7106/api/Clientes';

    function populateSelect(url, selectId) {
        $.ajax({
            url: url,
            method: 'GET',
            success: function (data) {
                const select = $(selectId);
                select.empty();
                data.forEach(item => {
                    select.append(new Option(item.nombreMotivoTransaccion || item.nombreAgencia || item.nombreCliente, item.idMotivoTransaccion || item.idAgencia || item.idCliente));
                });
            }
        });
    }

    function populateAllSelects() {
        populateSelect(motivosApiUrl, '#createIdMotivoTransaccion, #editIdMotivoTransaccion');
        populateSelect(agenciasApiUrl, '#createIdAgencia, #editIdAgencia');
        populateSelect(clientesApiUrl, '#createIdCliente, #editIdCliente');
    }

    $('#transaccionesTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idTransaccion" },
            {
                "data": "motivoTransaccion",
                "render": function (data, type, row) {
                    return data && data.nombreMotivoTransaccion ? data.nombreMotivoTransaccion : 'N/A';
                }
            },
            {
                "data": "agencia",
                "render": function (data, type, row) {
                    return data && data.nombreAgencia ? data.nombreAgencia : 'N/A';
                }
            },
            {
                "data": "cliente",
                "render": function (data, type, row) {
                    return data && data.nombreCliente ? data.nombreCliente : 'N/A';
                }
            },
            { "data": "fechaTransaccion" },
            { "data": "montoTransaccion" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditTransaccionModal(${row.idTransaccion})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteTransaccionModal(${row.idTransaccion})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditTransaccionModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editIdTransaccion').val(data.idTransaccion);
                $('#editIdMotivoTransaccion').val(data.idMotivoTransaccion);
                $('#editIdAgencia').val(data.idAgencia);
                $('#editIdCliente').val(data.idCliente);
                $('#editMontoTransaccion').val(data.montoTransaccion);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editTransaccionModal').modal('show');
            }
        });
    };

    window.showDeleteTransaccionModal = function (id) {
        $('#deleteTransaccionId').val(id);
        $('#deleteTransaccionModal').modal('show');
    };

    $('#editTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editIdTransaccion').val();
        const data = {
            idTransaccion: id,
            idMotivoTransaccion: $('#editIdMotivoTransaccion').val(),
            idAgencia: $('#editIdAgencia').val(),
            idCliente: $('#editIdCliente').val(),
            fechaTransaccion: new Date().toISOString(),
            montoTransaccion: $('#editMontoTransaccion').val(),
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
                        text: 'Transacción actualizada exitosamente',
                    });
                    $('#editTransaccionModal').modal('hide');
                    $('#transaccionesTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al actualizar la Transacción: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al actualizar la Transacción:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al actualizar la Transacción',
                });
            });
    });

    $('#deleteTransaccionForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteTransaccionId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Transacción eliminada exitosamente',
                });
                $('#deleteTransaccionModal').modal('hide');
                $('#transaccionesTable').DataTable().ajax.reload();
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al eliminar la Transacción',
                });
            }
        });
    });

    $('#createTransaccionForm').submit(function (e) {
        e.preventDefault();
        const data = {
            idMotivoTransaccion: $('#createIdMotivoTransaccion').val(),
            idAgencia: $('#createIdAgencia').val(),
            idCliente: $('#createIdCliente').val(),
            fechaTransaccion: new Date().toISOString(),
            montoTransaccion: $('#createMontoTransaccion').val(),
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
                        text: 'Transacción creada exitosamente',
                    });
                    $('#createTransaccionModal').modal('hide');
                    $('#transaccionesTable').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Error al crear la Transacción: ' + JSON.stringify(data),
                    });
                }
            })
            .catch(error => {
                console.error('Error al crear la Transacción:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error al crear la Transacción',
                });
            });
    });

    populateAllSelects();
});
