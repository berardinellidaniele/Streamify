$(document).ready(function () {
    let isLoading = false;
    let offsets = {};

    // Recupero del genere dalla pagina
    const genere = $('h1.text-center').text().trim();

    // Funzione per caricare i contenuti in base allo scroll
    function loadMoreContent() {
        if (isLoading) return;

        const windowHeight = $(window).height();
        const scrollPosition = $(document).scrollTop();
        const documentHeight = $(document).height();

        if (documentHeight - scrollPosition <= windowHeight + 100) {
            const limit = 10;
            isLoading = true;
            console.log(`Caricamento di ${limit} contenuti per il genere ${genere}...`); //debug

            caricaContenutiDallaCache(genere, offsets[genere] || 0, limit).then(function (contenutiHtml) {
                if (contenutiHtml) {
                    $('.contenuti-grid').append(contenutiHtml);
                    offsets[genere] = (offsets[genere] || 0) + limit;
                }

                isLoading = false;
            }).catch(function (error) {
                console.error("Errore nel caricamento dei contenuti:", error); //debug
                isLoading = false;
            });
        }
    }

    // Evento scroll per caricare i contenuti
    $(window).scroll(function () {
        loadMoreContent();
    });

    // Funzione per caricare i contenuti dalla cache o dalla cache
    function caricaContenutiDallaCache(genere, offset, limit) {
        const cacheKey = `contenuti_${genere}_${offset}_${limit}`;
        const cachedData = localStorage.getItem(cacheKey);

        if (cachedData) {
            const { data, timestamp } = JSON.parse(cachedData);
            const cacheDuration = 3600000;
            if (Date.now() - timestamp < cacheDuration) {
                console.log("Caricamento dati dalla cache."); //debug
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
                    console.log("Risposta AJAX ricevuta:", data); //debug
                    localStorage.setItem(cacheKey, JSON.stringify({ data: data, timestamp: Date.now() }));
                    resolve(data);
                },
                error: function (error) {
                    console.error("Errore nella richiesta AJAX:", error) //debug
                    reject([])
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
                    alert('Contenuto non trovato.'); //migliorare l'allert di errore
                }
            },
            error: function () {
                alert('Errore nel recupero dei dettagli del contenuto.'); //migliorare l'allert di errore
            }
        });
    });

    // Chiusura del popup
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

    loadMoreContent();
});
