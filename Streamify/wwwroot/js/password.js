document.addEventListener('DOMContentLoaded', () => {
    const toggleIcons = document.querySelectorAll('.toggle-password');
    const salvaPasswordBtn = document.getElementById('salva-password-btn');

    toggleIcons.forEach(icon => {
        icon.addEventListener('click', () => {
            const targetId = icon.getAttribute('data-target');
            const inputField = document.getElementById(targetId);

            if (inputField.type === 'password') {
                inputField.type = 'text';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            } else {
                inputField.type = 'password';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            }
        });
    });

    salvaPasswordBtn.addEventListener('click', (event) => {
        event.preventDefault();

        const vecchiaPassword = document.getElementById('vecchiaPassword').value.trim();
        const nuovaPassword = document.getElementById('nuovaPassword').value.trim();
        const confermaNuovaPassword = document.getElementById('confermaNuovaPassword').value.trim();

        if (!vecchiaPassword || !nuovaPassword || !confermaNuovaPassword) {
            alert('Tutti i campi sono obbligatori.');
            return;
        }

        const formData = new URLSearchParams();
        formData.append('VecchiaPassword', vecchiaPassword);
        formData.append('NuovaPassword', nuovaPassword);
        formData.append('ConfermaNuovaPassword', confermaNuovaPassword);

        fetch('/Account/CambiaPassword', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: formData.toString(),
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Password aggiornata con successo!');
                    document.getElementById('vecchiaPassword').value = '';
                    document.getElementById('nuovaPassword').value = '';
                    document.getElementById('confermaNuovaPassword').value = '';
                } else {
                    const errorList = data.errors ? data.errors.join('\n') : data.message;
                    alert(`Errore:\n${errorList}`);
                }
            })
            .catch(error => {
                console.error('Errore:', error);
                alert('Errore nella comunicazione col server.');
            });
    });
});