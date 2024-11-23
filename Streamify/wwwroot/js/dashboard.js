$(document).ready(function () {
    let isLoading = false;
    let offsets = {};


    $('.row-wrapper').on('scroll', function () {
        if (isLoading) return;

        let $this = $(this);
        const genere = $this.data('genere');

        if (!offsets.hasOwnProperty(genere)) {
            offsets[genere] = 10; // Inizializzo l'offset a 10 se non esiste già il genere
        }


        if ($this[0].scrollWidth - $this.scrollLeft() <= $this.outerWidth() + 100) { // Verifica se l'utente ha raggiunto quasi la fine dello scroll
            const limit = 10; // Numero di elementi da caricare

            isLoading = true;

            // Chiamata ajax per caricare altri elementi
            $.ajax({
                url: '/Home/LoadMoreContent',
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
});
