document.addEventListener('DOMContentLoaded', function () {
    const inputpasswordicone = document.querySelectorAll('.toggle-password');

    inputpasswordicone.forEach(function (icon) {
        icon.addEventListener('click', function () {
            if (this.classList.contains('disabled')) return;

            const input = document.querySelector(`#${this.dataset.toggle}`);
            const tipo = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', tipo);
            this.classList.toggle('fa-eye');
            this.classList.toggle('fa-eye-slash');
            this.classList.toggle('active');

            this.classList.add('disabled');
            setTimeout(() => {
                this.classList.remove('disabled');
            }, 500);
        });
    });
});
