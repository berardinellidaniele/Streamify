$(document).ready(function () {
    var table = $('#cronologiaTable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.5/i18n/it-IT.json"
        }
    });

    $('#reset').on('click', function () {
        $('#dataInizio').val('');
        $('#dataFine').val('');
        var dataInizio = '1111-11-11'
        var dataFine = new Date().toISOString().slice(0, 10)

        if (dataInizio && dataFine) {
            table.rows().every(function () {
                var data = this.data();
                var dataInizioRiga = data[2];
                if (dataInizioRiga >= dataInizio && dataInizioRiga <= dataFine) {
                    $(this.node()).show();
                } else {
                    $(this.node()).hide();
                }
            });
        }
    });

    $('#filtraDate').on('click', function () {
        var dataInizio = $('#dataInizio').val();
        var dataFine = $('#dataFine').val();

        console.log(dataInizio);

        if (dataInizio && dataFine) {
            table.rows().every(function () {
                var data = this.data();
                var dataInizioRiga = data[2];
                if (dataInizioRiga >= dataInizio && dataInizioRiga <= dataFine) {
                    $(this.node()).show();
                } else {
                    $(this.node()).hide();
                }
            });
        }
    });

    $('#cronologiaTable').on('click', '.finisci', function () {
        var cronologiaId = $(this).data('id');
        $.ajax({
            url: '/Settings/Cambiastatovisione',
            method: 'GET',
            data: { id_cronologia: cronologiaId },
            success: function (data) {
                if (data.success) {
                    alert('Lo stato di visione è stato aggiornato');
                    window.location.reload();
                } else {
                    alert('Impossibile cambiare lo stato di visione, riprova tra poco');
                }
            },
            error: function () {
                alert('Impossibile cambiare lo stato di visione, riprova tra poco');
            }
        });
    });
});
