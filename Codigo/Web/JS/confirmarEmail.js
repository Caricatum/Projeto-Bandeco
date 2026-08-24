// Pré-preenche o e-mail se veio do cadastro
document.addEventListener('DOMContentLoaded', () => {
    const emailSalvo = sessionStorage.getItem('emailParaConfirmar');
    if (emailSalvo) {
        document.getElementById('email').value = emailSalvo;
        document.getElementById('emailMostrado').textContent = emailSalvo;
    }
});

document.getElementById('confirmarForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const email   = document.getElementById('email').value.trim();
    const codigo  = document.getElementById('codigo').value.trim();
    const message = document.getElementById('message');

    message.style.color = '';
    message.innerText = 'Confirmando...';

    fetchAPI(`/user/confirmarEmail?email=${encodeURIComponent(email)}&codigo=${encodeURIComponent(codigo)}`, { method: 'POST' })
        .then(async res => {
            if (!res.ok) {
                const texto = await res.text();
                throw new Error(texto || 'Não foi possível confirmar o e-mail.');
            }
            return true;
        })
        .then(() => {
            message.style.color = 'green';
            message.innerText = 'E-mail confirmado com sucesso! Redirecionando para o login...';
            sessionStorage.removeItem('emailParaConfirmar');
            setTimeout(() => {
                window.location.href = 'login.php';
            }, 1800);
        })
        .catch(err => {
            message.style.color = 'red';
            message.innerText = err.message;
        });
});
