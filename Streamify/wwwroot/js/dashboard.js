// Desc: Script per la dashboard dell'utente

// Funzione per la gestione dello scroll orizzontale delle righe di contenuti
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

    // Gestione caricamento dinamico dei contenuti
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

            caricaContenutiDallaCache(genere, offsets[genere], limit).then(function (contenuti) {
                $this.append(contenuti);
                offsets[genere] += limit;
                isLoading = false;
            }).catch(function () {
                isLoading = false;
            });
        }
    });

    // Funzione per caricare i contenuti nella cache o dalla cache
    function caricaContenutiDallaCache(genere, offset, limit) {
        const cacheKey = `contenuti_${genere}_${offset}_${limit}`;
        const cachedData = localStorage.getItem(cacheKey);


        if (cachedData) {
            const { data, timestamp } = JSON.parse(cachedData);
            const cacheDuration = 3600000;
            if (Date.now() - timestamp < cacheDuration) {
                return Promise.resolve(data);
            } else {
                localStorage.removeItem(cacheKey);
            }
        }

        return new Promise((resolve, reject) => {
            $.ajax({
                url: '/Home/CaricaPiuContenuti',
                method: 'GET',
                data: { genere: genere, offset: offset, limit: limit },
                success: function (data) {
                    localStorage.setItem(cacheKey, JSON.stringify({ data: data, timestamp: Date.now() }));
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
                    $('#dettagli-genere').text(data.genere);
                    $('#dettagli-rating').text(data.rating);
                    $('#dettagli-durata').text(data.durata);
                    $('#dettagli-episodi').text(data.episodi);
                    $('#dettagli-locandina').attr('src', data.locandina).show();

                    const trailerKeyword = encodeURIComponent(data.nome);

                    fetchTrailerUrl(trailerKeyword, function (url) {
                        if (url) {
                            $('#dettagli-trailer').attr('src', url + "?autoplay=1").show();
                        } else {
                            $('#dettagli-trailer').hide();
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

    // Funzione per recuperare l'URL del trailer di un contenuto
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
