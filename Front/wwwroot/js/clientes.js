$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/Clientes';
    const tipoClienteApiUrl = 'https://localhost:7106/api/TipoCliente'; // API para obtener los tipos de cliente

    // Inicializar DataTable
    $('#clientesTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idCliente" },
            { "data": "nombreCliente" },
            { "data": "numeroIdentidad" },
            { "data": "codigoCliente" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditClienteModal(${row.idCliente})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteClienteModal(${row.idCliente})"><i class="fas fa-trash-alt"></i></button>
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

    // Obtener y llenar los tipos de cliente
    function fillTipoClienteSelect() {
        $.ajax({
            url: tipoClienteApiUrl,
            method: 'GET',
            success: function (data) {
                const tipoClienteSelects = ['#createIdTipoCliente', '#editIdTipoCliente'];
                tipoClienteSelects.forEach(selectId => {
                    const select = $(selectId);
                    select.empty();
                    data.forEach(tipoCliente => {
                        select.append(new Option(tipoCliente.nombreTipoCliente, tipoCliente.idTipoCliente));
                    });
                });
            }
        });
    }

    fillTipoClienteSelect(); // Llenar los selects al cargar la página

    window.showEditClienteModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editClienteId').val(data.idCliente);
                $('#editNumeroIdentidad').val(data.numeroIdentidad);
                $('#editNombreCliente').val(data.nombreCliente);
                $('#editIdTipoCliente').val(data.idTipoCliente); // Select de tipo cliente
                $('#editCodigoCliente').val(data.codigoCliente);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(new Date().toISOString());
                $('#editClienteModal').modal('show');
            }
        });
    };

    window.showDeleteClienteModal = function (id) {
        $('#deleteClienteId').val(id);
        $('#deleteClienteModal').modal('show');
    };

    $('#editClienteForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editClienteId').val();
        const data = {
            idCliente: id,
            idTipoCliente: $('#editIdTipoCliente').val(),
            codigoCliente: $('#editCodigoCliente').val(),
            numeroIdentidad: $('#editNumeroIdentidad').val(),
            nombreCliente: $('#editNombreCliente').val(),
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
                if (data.message && data.message.includes('actualizado exitosamente')) {
                    alert('Cliente actualizado exitosamente');
                    $('#editClienteModal').modal('hide');
                    $('#clientesTable').DataTable().ajax.reload();
                } else {
                    alert('Error al actualizar el Cliente: ' + JSON.stringify(data));
                }
            })
            .catch(error => {
                console.error('Error al actualizar el Cliente:', error);
                alert('Error al actualizar el Cliente');
            });
    });

    $('#deleteClienteForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteClienteId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                $('#deleteClienteModal').modal('hide');
                $('#clientesTable').DataTable().ajax.reload();
            }
        });
    });

    $('#createClienteForm').submit(function (e) {
        e.preventDefault();
        const data = {
            idTipoCliente: $('#createIdTipoCliente').val(),
            codigoCliente: $('#createCodigoCliente').val(),
            numeroIdentidad: $('#createNumeroIdentidad').val(),
            nombreCliente: $('#createNombreCliente').val(),
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
                if (data.message && data.message.includes('creado exitosamente')) {
                    alert('Cliente creado exitosamente');
                    $('#createClienteModal').modal('hide');
                    $('#clientesTable').DataTable().ajax.reload();
                } else {
                    alert('Error al crear el Cliente: ' + JSON.stringify(data));
                }
            })
            .catch(error => {
                console.error('Error al crear el Cliente:', error);
                alert('Error al crear el Cliente');
            });
    });
});
