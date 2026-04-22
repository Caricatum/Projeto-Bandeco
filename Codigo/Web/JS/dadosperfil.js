document.getElementById("dadosForm").addEventListener("submit", function (e) {
    document.getElementById("aluno").checked = false;
    document.getElementById("func").checked = false;
    document.getElementById("name").value = "";
    document.getElementById("message").value = "";
    e.preventDefault();
    const url = `http://localhost:8080/user/login/${document.getElementById("username").value}`;


    fetch(url, {
        method: 'GET',
    })
        .then(res => {
            if (!res.ok) { throw new Error("Usuário não encontrado"); }
            return res.json();
        })
        .then(data => {

            // Mostrar os elementos só depois da resposta
            document.getElementById("sectionTipodeUsuario").style.display = "flex";
            document.getElementById("div-nome").style.display = "flex";
            document.getElementById("trocarinfo").style.display = "block";
            document.getElementById("deletar").style.display = "block";
            document.getElementById("trocarinfo").disabled = false;
            document.getElementById("deletar").disabled = false;


            // Exemplo: preencher dados na tela
            if (data.funcionario === true) {
                document.getElementById("func").checked = true;
                document.getElementById("aluno").checked = false;
            } else {
                document.getElementById("aluno").checked = true;
                document.getElementById("func").checked = false;
            }
            document.getElementById("name").value = data.nome;


        })
        .catch(error => {
            console.error("Erro:", error);
            document.getElementById("message").innerText = "Usuário não encontrado.";
        });


});

document.getElementById("trocarinfo").addEventListener("click", function () {

    const url = `http://localhost:8080/user/login/${document.getElementById("username").value}`;

    fetch(url, {
        method: 'GET',
    })
        .then(res => {
            if (!res.ok) { throw new Error("Erro na requisição."); }
            return res.json();
        })
        .then(data => {
            const nomeBanco = data.nome;
            const tipoDeUsuarioBanco = data.funcionario;
            const idBanco = data.id;

            localStorage.setItem("username", document.getElementById("username").value);
            localStorage.setItem("nome", nomeBanco);
            localStorage.setItem("tipo", tipoDeUsuarioBanco);
            localStorage.setItem("id", idBanco);
            window.location.href = "trocarinfo.html";
        })
        .catch(error => {
            console.error("Erro:", error);
            document.getElementById("message").innerText = "Erro na requisição.";
        });


});

document.getElementById("deletar").addEventListener("click", function () {

    const url = `http://localhost:8080/user/login/${document.getElementById("username").value}`;

    console.log("clicou");

    fetch(url, {
        method: 'GET',
    })
        .then(res => {
            if (!res.ok) { throw new Error("Usuário não encontrado"); }
            return res.json();
        })
        .then(data => {

            const id = data.id
            const urld = `http://localhost:8080/user/deletarUser/${id}`;
            console.log("entrou");

            fetch(urld, {
                method: 'DELETE',
            })
                .then(res => {
                    if (!res.ok) throw new Error("Erro na requisição");

                    const contentType = res.headers.get("content-type");

                    if (contentType && contentType.includes("application/json")) {
                        return res.json();
                    } else {
                        return null;
                    }
                })
                .then(data => {
                    console.log("deletou")
                })

                .catch(error => {
                    console.error("Erro:", error);
                    document.getElementById("message").innerText = "Erro na requisição.";
                })
        }) //Acaba o then

        .catch(error => {
            console.error("Erro:", error);
            document.getElementById("message").innerText = "Usuário não encontrado.";
        });

});