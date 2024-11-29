$(document).ready(function () {
    $('.scroller-left, .scroller-right').click(function () {
        const isLeft = $(this).hasClass('scroller-left');
        const targetGenere = $(this).data('target');
        const $rowWrapper = $(`.row-wrapper[data-genere='${targetGenere}']`);
        const scrollAmount = 1000;

        const newScrollPosition = isLeft
            ? $rowWrapper.scrollLeft() - scrollAmount
            : $rowWrapper.scrollLeft() + scrollAmount;

        $rowWrapper.animate({ scrollLeft: newScrollPosition }, 300); 
    });

    let isLoading = false;
    let offsets = {};

    $('.row-wrapper').on('scroll', function () {
        if (isLoading) return;

        let $this = $(this);
        const genere = $this.data('genere');

        if (!offsets.hasOwnProperty(genere)) {
            offsets[genere] = 10;
        }

        if ($this[0].scrollWidth - $this.scrollLeft() <= $this.outerWidth() + 100) {
            const limit = 10;
            isLoading = true;

            $.ajax({
                url: '/Home/CaricaPiuContenutiGeneri',
                method: 'GET',
                data: { genere: genere, offset: offsets[genere], limit: limit },
                success: function (data) {
                    $this.append(data);
                    offsets[genere] += limit;
                    isLoading = false;
                },
                error: function () {
                    isLoading = false;
                }
            });
        }
    });

    $(document).on('click', '.locandina', function () {
        const contenutoId = $(this).data('id');
        const popup = $('#dettagli-contenuto');

        $.ajax({
            url: '/Home/GetContenutoDettagli',
            method: 'GET',
            data: { id: contenutoId },
            success: function (data) {
                if (data.success) {
                    $('#dettagli-titolo').text(data.nome);
                    $('#descrizione-dettagli').text(data.descrizione);
                    $('#trailer-dettagli').attr('src', data.trailerUrl || '').toggle(!!data.trailerUrl);
                    popup.show();
                } else {
                    alert('Contenuto non trovato.');
                }
            },
            error: function () {
                alert('Errore nel caricamento dei dettagli.');
            }
        });
    });

    $('#chiudi-popup').click(function () {
        $('#dettagli-contenuto').hide();
        $('#trailer-dettagli').attr('src', '');
    });
});
