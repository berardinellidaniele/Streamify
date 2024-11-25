$(document).ready(function () {

    //Gestione "ingrandimento" barra di ricerca
    $('.search-bar .search-button').click(function () {
        $('.search-bar .search-input').focus();
    });

    // Evento che cattura la pressione del tasto "Enter" nella barra di ricerca
    $('.search-bar .search-input').keydown(function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            const searchText = $(this).val();

            console.log(searchText);
        }
    });
});
