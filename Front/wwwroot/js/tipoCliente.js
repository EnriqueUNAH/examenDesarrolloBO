﻿$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/TipoCliente';

    // Inicializar DataTable
    $('#tipoClienteTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idTipoCliente" },
            { "data": "codigoTipoCliente" },
            { "data": "nombreTipoCliente" },
            { "data": "fechaRegistro" },
            { "data": "fechaModificado" },
            { "data": "idUsuario" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return `
                        <button class="edit-btn" onclick="showEditTipoClienteModal(${row.idTipoCliente})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteTipoClienteModal(${row.idTipoCliente})"><i class="fas fa-trash-alt"></i></button>
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

    window.showEditTipoClienteModal = function (id) {
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editTipoClienteId').val(data.idTipoCliente);
                $('#editCodigoTipoCliente').val(data.codigoTipoCliente);
                $('#editNombreTipoCliente').val(data.nombreTipoCliente);
                $('#editFechaRegistro').val(data.fechaRegistro);
                $('#editFechaModificado').val(data.fechaModificado);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editTipoClienteModal').modal('show');
            }
        });
    };

    window.showDeleteTipoClienteModal = function (id) {
        $('#deleteTipoClienteId').val(id);
        $('#deleteTipoClienteModal').modal('show');
    };

    $('#editTipoClienteForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editTipoClienteId').val();
        const data = {
            idTipoCliente: id,
            codigoTipoCliente: $('#editCodigoTipoCliente').val(),
            nombreTipoCliente: $('#editNombreTipoCliente').val(),
            fechaRegistro: $('#editFechaRegistro').val(),
            fechaModificado: new Date().toISOString(),
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
                    alert(data.message);
                    $('#editTipoClienteModal').modal('hide');
                    $('#tipoClienteTable').DataTable().ajax.reload();
                } else {
                    alert('Error al actualizar el Tipo Cliente');
                }
            })
            .catch(error => {
                console.error('Error al actualizar el Tipo Cliente:', error);
                alert('Error al actualizar el Tipo Cliente');
            });
    });

    $('#deleteTipoClienteForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteTipoClienteId').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function () {
                $('#deleteTipoClienteModal').modal('hide');
                $('#tipoClienteTable').DataTable().ajax.reload();
            }
        });
    });

    $('#createTipoClienteForm').submit(function (e) {
        e.preventDefault();
        const data = {
            codigoTipoCliente: $('#createCodigoTipoCliente').val(),
            nombreTipoCliente: $('#createNombreTipoCliente').val(),
            fechaRegistro: new Date().toISOString(),
            fechaModificado: new Date().toISOString(),
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
                    alert(data.message);
                    $('#createTipoClienteModal').modal('hide');
                    $('#tipoClienteTable').DataTable().ajax.reload();
                } else {
                    alert('Error al crear el Tipo Cliente');
                }
            })
            .catch(error => {
                console.error('Error al crear el Tipo Cliente:', error);
                alert('Error al crear el Tipo Cliente');
            });
    });
});