// =============================================================
// CARDÁPIO DO DIA
// Adicione este arquivo ao buscaPratos.php:
//   <script src="../JS/cardapioDia.js"></script>
// (depois do buscaPratos.js, pois reaproveita API, todosPratos, etc)
// =============================================================

const btnNovoCardapio   = document.getElementById('btnNovoCardapio');
const modalCardapioDia  = document.getElementById('modalCardapioDia')
    ? new bootstrap.Modal(document.getElementById('modalCardapioDia'))
    : null;

document.addEventListener('DOMContentLoaded', () => {

    // Mostra o botão só para funcionário
    if (typeof isFuncionario !== 'undefined' && isFuncionario && btnNovoCardapio) {
        btnNovoCardapio.classList.remove('d-none');
    }

    if (btnNovoCardapio && modalCardapioDia) {
        btnNovoCardapio.addEventListener('click', () => {
            popularSelectsDePratos();
            modalCardapioDia.show();
        });
    }

    const btnSalvar = document.getElementById('salvarCardapioDia');
    if (btnSalvar) {
        btnSalvar.addEventListener('click', salvarCardapioDia);
    }
});

// =============================================================
// Popula cada <select class="select-prato"> com os pratos da
// categoria correspondente (e filtra vegano quando necessário)
// =============================================================
function popularSelectsDePratos() {
    document.querySelectorAll('.select-prato').forEach(select => {
        const categoriaId = parseInt(select.dataset.categoria);
        const exigeVegano = select.dataset.vegano === 'true';

        // Mantém só a opção "Nenhum"
        select.innerHTML = '<option value="">Nenhum</option>';

        const pratosFiltrados = (todosPratos || []).filter(p => {
            const mesmaCategoria = p.categoria && p.categoria.id === categoriaId;
            if (!mesmaCategoria) return false;
            if (exigeVegano && !p.vegano) return false;
            return true;
        });

        pratosFiltrados.forEach(p => {
            const opt = document.createElement('option');
            opt.value = p.id;
            opt.textContent = p.nome + (p.vegano ? ' 🥦' : '');
            select.appendChild(opt);
        });
    });
}

// =============================================================
// Monta os 4 objetos Cardapio (campos = ids de Pratos) a partir
// dos selects, depois cadastra cada um em /cardapio/cadastrar
// e por fim cadastra o CardapioDia com os 4 ids retornados.
// =============================================================
async function salvarCardapioDia() {
    const msg    = document.getElementById('msgCardapioDia');
    const btn    = document.getElementById('salvarCardapioDia');
    const data   = document.getElementById('dataCardapioDia').value;

    msg.textContent = '';

    if (!data) {
        msg.textContent = '⚠️ Selecione a data do cardápio.';
        return;
    }

    // Estrutura: { padraoAlmoco: {...campos}, veganoAlmoco: {...}, padraoJantar: {...}, veganoJantar: {...} }
    const cardapios = {
        padraoAlmoco: { vegano: false },
        veganoAlmoco: { vegano: true  },
        padraoJantar: { vegano: false },
        veganoJantar: { vegano: true  },
    };

    document.querySelectorAll('.select-prato').forEach(select => {
        const cardapioKey = select.dataset.cardapio; // padraoAlmoco, veganoAlmoco, ...
        const campo       = select.dataset.campo;    // acompanhamento, prato_principal, ...
        const valor       = select.value;

        if (valor) {
            cardapios[cardapioKey][campo] = { id: parseInt(valor) };
        }
    });

    btn.disabled = true;
    btn.textContent = 'Salvando...';

    try {
        // 1) Cadastra cada um dos 4 cardápios em /cardapio/cadastrar
        const idsCardapios = {};

        for (const chave of Object.keys(cardapios)) {
            const res = await fetch(`${API}/cardapio/cadastrar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cardapios[chave])
            });

            if (!res.ok) {
                throw new Error(`Erro ao salvar "${chave}": ${await res.text()}`);
            }

            const cardapioSalvo = await res.json();
            idsCardapios[chave] = cardapioSalvo.id;
        }

        // 2) Cadastra o CardapioDia com a data e os 4 ids + usuário logado
        const cardapioDiaPayload = {
            data: data,
            padraoAlmoco: { id: idsCardapios.padraoAlmoco },
            veganoAlmoco: { id: idsCardapios.veganoAlmoco },
            padraoJantar: { id: idsCardapios.padraoJantar },
            veganoJantar: { id: idsCardapios.veganoJantar },
            user: { id: userId }
        };

        const resDia = await fetch(`${API}/cardapioDia/cadastrar`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(cardapioDiaPayload)
        });

        if (!resDia.ok) {
            throw new Error(await resDia.text());
        }

        msg.style.color = 'green';
        msg.textContent = '✅ Cardápio do dia salvo com sucesso!';

        setTimeout(() => {
            modalCardapioDia.hide();
            msg.textContent = '';
            msg.style.color = '';
            document.getElementById('dataCardapioDia').value = '';
        }, 1200);

    } catch (e) {
        msg.style.color = '';
        msg.textContent = '❌ ' + e.message;
    } finally {
        btn.disabled = false;
        btn.textContent = '💾 Salvar Cardápio do Dia';
    }
}
