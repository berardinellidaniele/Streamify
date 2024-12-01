document.addEventListener('DOMContentLoaded', () => {
    const iconeEdit = document.querySelectorAll('.edit-icon');
    const bottoneSalva = document.getElementById('save-button');
    const usernameNavbar = document.querySelector('.navbar .ms-2'); 

    iconeEdit.forEach(icona => {
        icona.addEventListener('click', () => {
            const campo = icona.previousElementSibling;
            campo.setAttribute('contenteditable', 'true');
            campo.focus();
            campo.addEventListener('keydown', e => {
                if (e.key === "Enter") {
                    e.preventDefault();
                    campo.setAttribute('contenteditable', 'false');
                    campo.textContent = valoreOriginale;
                }
            });
            campo.addEventListener('blur', () => campo.setAttribute('contenteditable', 'false'));
            bottoneSalva.style.display = 'block';
        });
    });

    bottoneSalva.addEventListener('click', () => {
        const campi = document.querySelectorAll('.editable-field');
        const datiModificati = {};
        campi.forEach(campo => {
            const valoreOriginale = campo.getAttribute('data-original');
            const valoreCorrente = campo.textContent.trim();
            if (valoreCorrente !== valoreOriginale) {
                datiModificati[campo.dataset.field] = valoreCorrente;
                campo.setAttribute('data-original', valoreCorrente);
            }

        });
        if (Object.keys(datiModificati).length === 0) {
            alert("Nessuna modifica da salvare.");
            return;
        }
        fetch('/Settings/SalvaDati', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(datiModificati)
        })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    alert('Dati salvati con successo!');
                    bottoneSalva.style.display = 'none';

                    if (datiModificati.Nome && usernameNavbar) {
                        usernameNavbar.textContent = datiModificati.Nome;
                    }
                } else alert(data.message || 'Errore nel salvataggio dei dati.');
            })
            .catch(() => alert('errore nella comunicazione col server.'));
    });
});
