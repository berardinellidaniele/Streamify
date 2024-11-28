$(document).ready(function () {
    const barraRicerca = $('.search-bar .search-input');
    const contenutoPrincipale = $('#main-content');
    const contenitoreRisultati = $('#search-results');
    const contenitoreRisultatiRicerca = $('#search-results-container');
    const titoloRisultati = $('#results-title');

    function mostraRisultati(query, visibile) {
        contenutoPrincipale.toggle(!visibile);
        contenitoreRisultatiRicerca.css('display', visibile ? 'block' : 'none');
        titoloRisultati.text(visibile ? `Risultati trovati per: ${query}` : '');
    }

    barraRicerca.on('keydown', function (evento) {
        if (evento.key === "Enter") {
            evento.preventDefault();
            const query = $(this).val().trim();

            if (query.length > 0) {
                $.ajax({
                    url: '/Home/Search',
                    method: 'GET',
                    data: { search: query },
                    success: function (risultati) {
                        if (risultati.length > 0) {
                            contenitoreRisultati.empty();
                            risultati.forEach(elemento => {
                                const html = `
                                    <div class="locandina-container" style="margin: 10px;">
                                        <p style="color: white; font-size: 16px; text-align: center;">
                                            <strong>${elemento.nome || 'Titolo sconosciuto'}</strong>
                                        </p>
                                        <p style="color: gray; font-size: 14px; text-align: center;">
                                            ${elemento.descrizione || 'Descrizione non disponibile'}
                                        </p>
                                    </div>`;
                                contenitoreRisultati.append(html);
                            });
                            mostraRisultati(query, true);
                        } else {
                            contenitoreRisultati.html('<p style="color: white;">Nessun risultato trovato</p>');
                            mostraRisultati(query, true);
                        }
                    },
                    error: function () {
                        alert('Errore durante la ricerca.');
                    }
                });
            } else {
                mostraRisultati('', false);
            }
        }
    });

    barraRicerca.on('input', function () {
        if ($(this).val().trim().length === 0) {
            mostraRisultati('', false);
        }
    });
});
