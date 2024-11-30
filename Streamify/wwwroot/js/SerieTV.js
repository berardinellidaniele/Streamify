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

        $rowWrapper.animate({ scrollLeft: newScrollPosition }, 100);
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
                url: '/Home/CaricaPiuContenutiSerieTV',
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
    // Gestione apertura popup con dettagli
    $(document).on('click', '.locandina', function () {
        const contentId = $(this).data('id');
        id_contenuto = contentId;
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

                    if (data.like) {
                        like = true;
                        var icon = $('#like-button').find('i');
                        icon.removeClass('far').addClass('fas');
                    }

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

    var like = false;

    $('#like-button').click(function () {
        var $this = $(this);
        var icon = $this.find('i');
        like = !like;
        if (like) {
            $.ajax({
                url: '/Home/Aggiungi_Preferiti',
                method: 'GET',
                data: { id_contenuto: id_contenuto },
                success: function (data) {
                    if (data.success) {
                        icon.removeClass('far').addClass('fas');
                        alert('Contenuto aggiunto ai preferiti!');
                    } else {
                        alert('Effettua il login per poter salvare i contenuti nei preferiti.');
                    }
                },
                error: function () {
                    alert('Errore nell\'aggiunta del contenuto ai preferiti.');
                }
            });
        } else {
            $.ajax({
                url: '/Home/Rimuovi_Preferiti',
                method: 'GET',
                data: { id_contenuto: id_contenuto },
                success: function (data) {
                    if (data.success) {
                        icon.removeClass('fas').addClass('far');
                        alert('Contenuto rimosso dai preferiti!');
                    } else {
                        alert('Errore nella rimozione del contenuto dai preferiti.');
                    }
                },
                error: function () {
                    alert('Errore nella rimozione del contenuto dai preferiti.');
                }
            });
        }
    });

    $('#watch-button').click(function () {
        $.ajax({
            url: '/Home/Aggiungi_Cronologia',
            method: 'GET',
            data: { id_contenuto: id_contenuto },
            success: function (data) {
                if (data.success) {
                    alert('Contenuto guardato!');
                } else {
                    alert('Effettua il login per poter guardare il contenuto');
                }
            },
            error: function () {
                alert('Errore nella visione del contenuto.');
            }
        });
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
