$(document).ready(function () {
    const apiBaseUrl = "https://localhost:7106/api/Agencias";

    // Inicializar DataTable
    $('#agenciasTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "idAgencia", "visible": false },
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
                        <button class="edit-btn" onclick="showEditAgenciaModal(${row.idAgencia})"><i class="fas fa-edit"></i></button>
                        <button class="delete-btn" onclick="showDeleteAgenciaModal(${row.idAgencia})"><i class="fas fa-trash-alt"></i></button>
                    `;
                }
            }
        ],
        "columnDefs": [
            { "width": "15%", "targets": 1 },
            { "width": "15%", "targets": 2 },
            { "width": "10%", "targets": 3 },
            { "width": "10%", "targets": 4 },
            { "width": "10%", "targets": 5 },
            { "width": "10%", "targets": 6 },
            { "width": "10%", "targets": 7 },
            { "width": "10%", "targets": 8 },
            { "width": "5%", "targets": 9 }
        ],
        "responsive": true,
        "autoWidth": false,
        "scrollX": true,
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

    // Función para mostrar el modal de editar agencia
    window.showEditAgenciaModal = function (id) {
        // Lógica para obtener los datos de la agencia y llenar el formulario
        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'GET',
            success: function (data) {
                $('#editIdAgencia').val(data.idAgencia);
                $('#editIdCanalServicio').val(data.idCanalServicio);
                $('#editCodigoAgencia').val(data.codigoAgencia);
                $('#editNombreAgencia').val(data.nombreAgencia);
                $('#editDireccionAgencia').val(data.direccionAgencia);
                $('#editTelefonoAgencia').val(data.telefonoAgencia);
                $('#editIdUsuario').val(data.idUsuario);
                $('#editAgenciaModal').modal('show');
            }
        });
    };

    // Función para mostrar el modal de eliminar agencia
    window.showDeleteAgenciaModal = function (id) {
        $('#deleteIdAgencia').val(id);
        $('#deleteAgenciaModal').modal('show');
    };

    // Enviar datos de creación
    $('#createAgenciaForm').submit(function (e) {
        e.preventDefault();
        const formData = $(this).serializeArray().reduce((obj, item) => {
            obj[item.name] = item.value;
            return obj;
        }, {});

        $.ajax({
            url: apiBaseUrl,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function () {
                $('#agenciasTable').DataTable().ajax.reload();
                $('#createAgenciaModal').modal('hide');
            },
            error: function () {
                alert('Error al crear la agencia.');
            }
        });
    });

    // Enviar datos de edición
    $('#editAgenciaForm').submit(function (e) {
        e.preventDefault();
        const id = $('#editIdAgencia').val();
        const formData = $(this).serializeArray().reduce((obj, item) => {
            obj[item.name] = item.value;
            return obj;
        }, {});

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function () {
                $('#agenciasTable').DataTable().ajax.reload();
                $('#editAgenciaModal').modal('hide');
            },
            error: function () {
                alert('Error al actualizar la agencia.');
            }
        });
    });

    // Confirmar eliminación de agencia
    $('#deleteAgenciaForm').submit(function (e) {
        e.preventDefault();
        const id = $('#deleteIdAgencia').val();

        $.ajax({
            url: `${apiBaseUrl}/${id}`,
            method: 'DELETE',
            success: function (response) {
                $('#deleteAgenciaModal').modal('hide');
                $('#agenciasTable').DataTable().ajax.reload();
            },
            error: function () {
                alert('Error al eliminar la agencia.');
            }
        });
    });
});
