using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ProjetoBandejao.Data;
using ProjetoBandejao.Models;

namespace ProjetoBandejao.Services
{
    public class AvisoService
    {
        private static readonly HttpClient cliente = new() { Timeout = TimeSpan.FromSeconds(6) };
        private const string Url = "http://localhost:8080/avisos";

        public async Task<List<Aviso>> ListarAsync()
        {
            try
            {
                var remotos = await cliente.GetFromJsonAsync<List<Aviso>>(Url);
                if (remotos != null) SQLiteDatabase.SalvarAvisosApi(remotos);
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                System.Diagnostics.Debug.WriteLine($"Mural: {ex.Message}");
            }
            return SQLiteDatabase.ObterAvisos();
        }

        public async Task<(bool Sucesso, string Mensagem)> SalvarAsync(Aviso aviso)
        {
            if (aviso.Id < 0)
                return SQLiteDatabase.AtualizarAvisoPendente(aviso)
                    ? (true, "Aviso atualizado no SQLite. Envio pendente.")
                    : (false, "Este aviso já saiu da fila local. Atualize o mural.");
            if (aviso.User?.Id is not > 0)
                return (false, "Entre com uma conta já sincronizada para publicar no mural.");

            var dados = new
            {
                titulo = aviso.Titulo,
                descricao = aviso.Descricao,
                model = aviso.Model,
                data_inicial = aviso.DataInicial?.ToString("yyyy-MM-dd"),
                data_final = aviso.DataFinal?.ToString("yyyy-MM-dd"),
                user = new { id = aviso.User.Id }
            };
            try
            {
                using var resposta = aviso.Id > 0
                    ? await cliente.PutAsJsonAsync($"{Url}/{aviso.Id}", dados)
                    : await cliente.PostAsJsonAsync(Url, dados);
                if (!resposta.IsSuccessStatusCode)
                    return (false, UsuarioService.ExtrairMensagemErro(await resposta.Content.ReadAsStringAsync(),
                        $"A API recusou o aviso (HTTP {(int)resposta.StatusCode})."));

                var salvo = await resposta.Content.ReadFromJsonAsync<Aviso>();
                if (salvo != null) SQLiteDatabase.SalvarAvisosApi([salvo], listaCompleta: false);
                return (true, aviso.Id > 0 ? "Aviso atualizado." : "Aviso publicado.");
            }
            catch (HttpRequestException ex) when (aviso.Id == 0 && ex.HttpRequestError == HttpRequestError.ConnectionError)
            {
                return SQLiteDatabase.SalvarAvisoLocal(aviso) > 0
                    ? (true, "Aviso salvo no SQLite. Envio pendente.")
                    : (false, "Não foi possível gravar o aviso neste computador.");
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                return (false, "A API não confirmou a operação. Atualize o mural antes de tentar novamente.");
            }
        }

        public async Task<string?> DeletarAsync(int id)
        {
            if (id < 0)
                return SQLiteDatabase.DeletarAviso(id) ? null : "O aviso não está mais na fila local.";
            try
            {
                using var resposta = await cliente.DeleteAsync($"{Url}/{id}");
                if (resposta.StatusCode == HttpStatusCode.MethodNotAllowed)
                    return "A API atual não oferece exclusão de aviso por ID.";
                if (!resposta.IsSuccessStatusCode)
                    return $"A API recusou a exclusão (HTTP {(int)resposta.StatusCode}).";
                SQLiteDatabase.DeletarAviso(id);
                return null;
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException)
            {
                return "A API está indisponível. O aviso continua no mural.";
            }
        }
    }
}
