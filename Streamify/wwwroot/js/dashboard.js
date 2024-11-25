$(document).ready(function () {
    // Gestione scorrimento righe
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

    // Gestione caricamento dinamico contenuti
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
                url: '/Home/CaricaPiuContenuti',
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

    // Gestione apertura popup con dettagli
    $(document).on('click', '.locandina', function () {
        const contentId = $(this).data('id');  // Ottieni l'ID del contenuto cliccato
        const popup = $('#dettagli-contenuto');

        $.ajax({
            url: '/Home/GetContenutoDettagli',
            method: 'GET',
            data: { id: contentId },  // Passa l'ID del contenuto al server
            success: function (data) {
                if (data.success) {
                    // Popola il popup con i dati ricevuti
                    $('#dettagli-titolo').text(data.nome);
                    $('#dettagli-descrizione').text(data.descrizione);

                    // Recupera il trailer usando la funzione esistente
                    const youtubeSearchUrl = `https://www.youtube.com/results?search_query=${encodeURIComponent(data.trailerKeyword)}`;
                    fetchTrailerUrl(youtubeSearchUrl, function (url) {
                        $('#dettagli-trailer').attr('src', url);
                    });

                    popup.show();
                } else {
                    alert('Contenuto non trovato.');
                }
            },
            error: function () {
                alert('Errore nel recupero dei dettagli del contenuto.');
            }
        });
    });

    $('#close-popup').click(function () {
        $('#dettagli-contenuto').hide();
        $('#dettagli-trailer').attr('src', '');
    });

    function fetchTrailerUrl(searchUrl, callback) {
        $.get(searchUrl, function (data) {
            const match = data.match(/watch\?v=([a-zA-Z0-9_-]{11})/);
            if (match && match[1]) {
                const videoUrl = `https://www.youtube.com/embed/${match[1]}`;
                callback(videoUrl);
            } else {
                callback('');
            }
        });
    }
});
