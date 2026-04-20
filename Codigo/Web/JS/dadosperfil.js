document.getElementById("dadosForm").addEventListener("submit", function(e){
    e.preventDefault();
    const url = `http://localhost:8080/user/login/${document.getElementById("username").value}`;
    const userDigitado = document.getElementById("username").value;

    fetch(url, {
        method: 'GET',
    })
    .then(res=>{
        if (!res.ok) {throw new Error("Usuário não encontrado");}
        return res.json();
    })
    .then(data => {
        console.log(data);

        // Mostrar os elementos só depois da resposta
        document.getElementById("sectionTipodeUsuario").style.display = "flex";
        document.getElementById("div-nome").style.display = "flex";

        // Exemplo: preencher dados na tela
        document.getElementById("sectionTipodeUsuario").value = data.funcionario;
        document.getElementById("name").value = data.nome;
    })
    .catch(error => {
        console.error("Erro:", error);
    });
    document.getElementById("sectionTipodeUsuario").style.display = "flex";
    document.getElementById("div-nome").style.display = "flex";
    
    

}); 