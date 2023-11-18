var tableHdr = null;
var IdRecord = 0;

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        IdRecord = 0;
        NewRecord();
    });

    $('#btnguardar').on('click', function (e) {
        e.preventDefault();
        Guardar();
    });

    $('#dt-records').on('click', 'button.btn-edit', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        loadDtl(data);
        IdRecord = data.CreditCardID;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.CreditCardID;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/CreditCard/Lista",
        order: [],
        columns: [
            { "data": "CardType" },
            { "data": "CardNumber" },
            { "data": "ExpMonth" },
            { "data": "ExpYear" }
        ],
        processing: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                width: "40%",
                targets: 0,
                data: "CardType"
            },
            {
                width: "40%",
                targets: 1,
                data: "CardNumber"
            },
            {
                width: "9%",
                targets: 2,
                data: "ExpMonth"
            },
            {
                width: "9%",
                targets: 3,
                data: "ExpYear"
            },
            {
                width: "1%",
                targets: 4,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 5,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Crear Tarjeta");

    $('#txtCardType').val('');
    $('#txtCardNumber').val('');
    $('#txtExpMonth').val('');
    $('#txtExpYear').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Tarjeta");

    $('#txtCardType').val(data.CardType);
    $('#txtCardNumber').val(data.CardNumber);
    $("#txtExpMonth").val(data.ExpMonth);
    $("#txtExpYear").val(data.ExpYear);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'CreditCardID':" + IdRecord;
    record += ",'CardType':'" + $.trim($('#txtCardType').val()) + "'";
    record += ",'CardNumber':'" + $.trim($('#txtCardNumber').val()) + "'";
    record += ",'ExpMonth':'" + $.trim($('#txtExpMonth').val()) + "'";
    record += ",'ExpYear':" + $.trim($('#txtExpYear').val());

    $.ajax({
        type: 'POST',
        url: '/CreditCard/Guardar',
        data: eval('({' + record + '})'),
        success: function (response) {
            if (response.success) {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    $.ajax({
        type: 'POST',
        url: '/CreditCard/Eliminar/?CreditCardID=' + IdRecord,
        success: function (response) {
            if (response.success) {
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}