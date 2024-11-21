$(document).ready(function () {
    let isLoading = false;

    $(window).on('scroll', function () {
        if (isLoading) return;

        $('.row-wrapper').each(function () {
            let $this = $(this);

            if ($this[0].getBoundingClientRect().bottom <= window.innerHeight && !isLoading) {
                const genere = $this.data('genere');
                const offset = $this.data('offset');
                const limit = 40;

                isLoading = true;

                // Richiesta AJAX per ottenere nuovi contenuti
                $.ajax({
                    url: '/Dashboard/GetContenuti',
                    method: 'GET',
                    data: { genere: genere, offset: offset, limit: limit },
                    success: function (data) {
                        // Aggiunge i contenuti alla pagina
                        $this.append(data);
                        // Aumenta l'offset per la prossima richiesta
                        $this.data('offset', offset + limit);
                        isLoading = false;
                    },
                    error: function () {
                        isLoading = false;
                    }
                });
            }
        });
    });
});
