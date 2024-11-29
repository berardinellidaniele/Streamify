$(document).ready(function () {
    $('.scroller-left, .scroller-right').click(function () {
        const isLeft = $(this).hasClass('scroller-left');
        const targetGenere = $(this).data('target');
        const $rowWrapper = $(`.row-wrapper[data-genere='${targetGenere}']`);
        const scrollAmount = 1000;

        const newScrollPosition = isLeft
            ? $rowWrapper.scrollLeft() - scrollAmount
            : $rowWrapper.scrollLeft() + scrollAmount;

        $rowWrapper.animate({ scrollLeft: newScrollPosition }, 100);
    });

    let isLoading = false;
    let offsets = {};

    const isGeneriPage = window.location.pathname.includes('/Home/Generi');

    // Gestione caricamento dinamico dei contenuti
    $('.row-wrapper').on('scroll', function () {
        if (isLoading) return;

        const $this = $(this);
        const genere = $this.data('genere');

        if (!offsets.hasOwnProperty(genere)) {
            offsets[genere] = 10;
        }

        if ($this[0].scrollWidth - $this.scrollLeft() <= $this.outerWidth() + 100) {
            const limit = 10;
            isLoading = true;

            caricaContenuti(genere, offsets[genere], limit).then(function (contenuti) {
                $this.append(contenuti);
                offsets[genere] += limit;
                isLoading = false;
            }).catch(function () {
                isLoading = false;
            });
        }
    });

    function caricaContenuti(genere, offset, limit) {
        const url = isGeneriPage ? '/Home/CaricaPiuContenutiGeneri' : '/Home/CaricaPiuContenuti';

        return new Promise((resolve, reject) => {
            $.ajax({
                url: url,
                method: 'GET',
                data: { genere: genere, offset: offset, limit: limit },
                success: function (data) {
                    resolve(data);
                },
                error: function () {
                    reject([]);
                }
            });
        });
    }

    // Gestione apertura popup con dettagli
    $(document).on('click', '.locandina', function () {
        const contentId = $(this).data('id');
        const popup = $('#dettagli-contenuto');

        $.ajax({
            url: '/Home/GetContenutoDettagli',
            method: 'GET',
            data: { id: contentId },
            success: function (data) {
                if (data.success) {
                    $('#dettagli-titolo').text(data.nome);
                    $('#dettagli-descrizione').text(data.descrizione);
                    $('#dettagli-trailer').attr('src', '').hide();

                    const trailerKeyword = encodeURIComponent(data.nome);

                    fetchTrailerUrl(trailerKeyword, function (url) {
                        if (url) {
                            $('#dettagli-trailer').attr('src', url + "?autoplay=1").show();
                        }
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
        $('#dettagli-trailer').attr('src', '').hide();
    });

    function fetchTrailerUrl(query, callback) {
        $.ajax({
            url: '/Home/GetTrailerUrl',
            method: 'GET',
            data: { trailerKeyword: query },
            success: function (data) {
                if (data.success) {
                    callback(data.trailerUrl);
                } else {
                    callback('');
                }
            },
            error: function () {
                callback('');
            }
        });
    }
});
