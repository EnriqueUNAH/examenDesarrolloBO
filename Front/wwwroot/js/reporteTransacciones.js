$(document).ready(function () {
    const apiBaseUrl = 'https://localhost:7106/api/ReporteTransacciones';

    const table = $('#reporteTransaccionesTable').DataTable({
        "ajax": {
            "url": apiBaseUrl,
            "dataSrc": ""
        },
        "columns": [
            { "data": "fechaTransaccion" },
            { "data": "tipoCliente" },
            { "data": "numeroIdentidad" },
            { "data": "nombreCliente" },
            { "data": "codigoAgencia" },
            { "data": "nombreAgencia" },
            { "data": "canalServicio" },
            { "data": "codigoMotivoTransaccion" },
            { "data": "nombreMotivoTransaccion" },
            { "data": "montoTransaccion" }
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

    $('#btnFiltrar').on('click', function () {
        let fechaInicio = $('#fechaInicio').val();
        let fechaFin = $('#fechaFin').val();

        // Reload the table data with the date range filter
        table.ajax.url(apiBaseUrl + `?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`).load();
    });

    // Cargar datos iniciales sin filtro
    table.ajax.reload();
});
