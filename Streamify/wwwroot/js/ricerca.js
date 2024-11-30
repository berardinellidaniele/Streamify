$(document).ready(function () {
    const barraRicerca = $('.search-input');

    barraRicerca.on('keydown', function (evento) {
        if (evento.key === "Enter") {
            evento.preventDefault();

            const query = barraRicerca.val().trim();

            window.location.replace(`/Home/CercaContenutoOgenere?query=${query}`);
        }
    });

});
