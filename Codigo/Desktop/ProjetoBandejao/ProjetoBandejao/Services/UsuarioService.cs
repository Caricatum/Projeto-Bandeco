using ProjetoBandejao.Data;
using ProjetoBandejao.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ProjetoBandejao.Services
{
    public class UsuarioService
    {
        private static readonly HttpClient clientEdicao = new() { Timeout = TimeSpan.FromSeconds(20) };
        private readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(2.5) };

        private const string API_URL = "http://localhost:8080/user";

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static string ExtrairMensagemErro(string responseContent, string mensagemPadrao = "Ocorreu um erro na requisição.")
        {
            if (string.IsNullOrWhiteSpace(responseContent))
                return mensagemPadrao;

            try
            {
                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;
                if (root.TryGetProperty("message", out var msgProp) && msgProp.ValueKind == JsonValueKind.String)
                {
                    string msg = msgProp.GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(msg))
                        return msg;
                }
                if (root.TryGetProperty("error", out var errProp) && errProp.ValueKind == JsonValueKind.String)
                {
                    string err = errProp.GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(err))
                        return err;
                }
            }
            catch
            {
                if (!responseContent.Contains("<html", StringComparison.OrdinalIgnoreCase))
                {
                    return responseContent.Trim();
                }
            }

            return mensagemPadrao;
        }

        // Listar Todos os Usuários Assíncrono (Sincronizado com SQLite)
        public async Task<List<Usuario>> ListarTodosAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, jsonOptions);
                    if (usuarios != null && usuarios.Count > 0)
                    {
                        // Sincroniza com o banco local SQLite
                        SQLiteDatabase.SalvarFuncionarios(usuarios);
                        return SQLiteDatabase.ObterFuncionarios();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ListarTodosAsync] Falha na API ({ex.Message}). Carregando do banco local SQLite...");
            }

            return SQLiteDatabase.ObterFuncionarios();
        }

        // Listar Todos os Usuários (Sincronizado com SQLite)
        public List<Usuario> ListarTodos()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(API_URL).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, jsonOptions);
                    if (usuarios != null && usuarios.Count > 0)
                    {
                        // Sincroniza com o banco local SQLite
                        SQLiteDatabase.SalvarFuncionarios(usuarios);
                        return usuarios;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ListarTodos] Falha na API ({ex.Message}). Carregando do banco local SQLite...");
            }

            // Fallback: carrega do banco local SQLite se a API estiver inacessível
            return SQLiteDatabase.ObterFuncionarios();
        }

        // Deletar Usuário (Remove da API e do banco local)
        public bool Deletar(int id)
        {
            if (!PermissoesDesktop.PodeAdministrarContas || PermissoesDesktop.EhContaAtual(id))
                return false;

            try
            {
                if (id < 0)
                    return SQLiteDatabase.DeletarFuncionario(id);

                HttpResponseMessage response = client.DeleteAsync($"{API_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    SQLiteDatabase.DeletarFuncionario(id);
                    return true;
                }

                string msg = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show($"Erro ao excluir usuário na API ({response.StatusCode}):\n{msg}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir usuário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Atualizar Usuário
        public async Task<string?> AtualizarAsync(Usuario usuario, string? caminhoImagem)
        {
            var sessao = UsuarioSession.UsuarioLogado;
            bool ehPropriaConta = PermissoesDesktop.EhContaAtual(usuario.Id);
            if (sessao == null || (!ehPropriaConta && !PermissoesDesktop.PodeAdministrarContas))
                return "Você só pode editar a sua própria conta.";
            if (ehPropriaConta && (usuario.Funcionario != sessao.Funcionario
                || !string.Equals(usuario.Login, sessao.Login, StringComparison.OrdinalIgnoreCase)))
                return "A edição do perfil permite alterar o nome e a foto.";

            try
            {
                if (usuario.Id < 0)
                {
                    if (!SQLiteDatabase.AtualizarFuncionarioPendente(usuario))
                        return "Não foi possível atualizar o cadastro local.";

                    if (ehPropriaConta && UsuarioSession.UsuarioLogado?.Id == usuario.Id)
                        sessao.Nome = usuario.Nome;
                    return null;
                }

                var cadastroAtual = await clientEdicao.GetFromJsonAsync<Usuario>($"{API_URL}/{usuario.Id}", jsonOptions);
                if (cadastroAtual == null || !string.Equals(cadastroAtual.Login, usuario.Login, StringComparison.OrdinalIgnoreCase))
                    return "A conta mudou no servidor. Atualize a tela antes de editar.";

                var dados = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    login = usuario.Login,
                    funcionario = ehPropriaConta ? cadastroAtual.Funcionario : usuario.Funcionario,
                    // O DTO Java exige um valor nesta parte, mas o PUT não grava a senha.
                    senhaHash = "campo-nao-alterado",
                    alergiaLeite = cadastroAtual.AlergiaLeite,
                    alergiaOvo = cadastroAtual.AlergiaOvo,
                    alergiaAmendoim = cadastroAtual.AlergiaAmendoim,
                    alergiaCastanhas = cadastroAtual.AlergiaCastanhas,
                    alergiaFrutosDoMar = cadastroAtual.AlergiaFrutosDoMar,
                    alergiaPeixes = cadastroAtual.AlergiaPeixes,
                    alergiaTrigo = cadastroAtual.AlergiaTrigo,
                    alergiaSoja = cadastroAtual.AlergiaSoja,
                    alergiaGergilim = cadastroAtual.AlergiaGergilim
                };

                using var multipart = new MultipartFormDataContent();
                multipart.Add(new StringContent(JsonSerializer.Serialize(dados), Encoding.UTF8, "application/json"), "user");
                if (caminhoImagem != null)
                {
                    var imagem = new StreamContent(File.OpenRead(caminhoImagem));
                    imagem.Headers.ContentType = new MediaTypeHeaderValue(Path.GetExtension(caminhoImagem).Equals(".png", StringComparison.OrdinalIgnoreCase)
                        ? "image/png" : "image/jpeg");
                    multipart.Add(imagem, "imagem", Path.GetFileName(caminhoImagem));
                }
                using var resposta = await clientEdicao.PutAsync($"{API_URL}/{usuario.Id}", multipart);
                string corpo = await resposta.Content.ReadAsStringAsync();
                if (!resposta.IsSuccessStatusCode)
                    return ExtrairMensagemErro(corpo, $"Não foi possível atualizar o funcionário (HTTP {(int)resposta.StatusCode}).");
                var salvo = JsonSerializer.Deserialize<Usuario>(corpo, jsonOptions);
                if (salvo != null)
                {
                    SQLiteDatabase.SalvarFuncionarios([salvo]);
                    if (UsuarioSession.UsuarioLogado?.Id == salvo.Id) UsuarioSession.UsuarioLogado = salvo;
                }
                return null;
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException or OperationCanceledException or JsonException)
            {
                return "Não foi possível concluir a edição. Confira a conexão com a API e a foto escolhida.";
            }
        }

        // Validar Login (com failover inteligente entre endpoints da API e contingência offline no SQLite)
        public bool ValidarLogin(Usuario usuario, out string mensagemErro, out bool isOffline)
        {
            mensagemErro = string.Empty;
            isOffline = false;

            string loginDigitado = usuario.Login.Trim();
            string senhaDigitada = usuario.Senha;

            // 1. Tenta autenticação online na API se o servidor estiver acessível
            try
            {
                var credenciais = new
                {
                    login = loginDigitado,
                    senha = senhaDigitada
                };

                string json = JsonSerializer.Serialize(credenciais);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Primeiro tenta a rota exclusiva de funcionários
                HttpResponseMessage response = client.PostAsync($"{API_URL}/login-funcionario", content).Result;

                // Compatibilidade com servidores antigos sem a rota de funcionário.
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    try
                    {
                        var contentGeral = new StringContent(json, Encoding.UTF8, "application/json");
                        var responseGeral = client.PostAsync($"{API_URL}/login", contentGeral).Result;
                        if (responseGeral.IsSuccessStatusCode
                            && JsonSerializer.Deserialize<Usuario>(responseGeral.Content.ReadAsStringAsync().Result, jsonOptions)?.Funcionario == true)
                        {
                            response = responseGeral;
                        }
                    }
                    catch { }
                }

                if (response.IsSuccessStatusCode)
                {
                    // Login online bem-sucedido: sincroniza a senha em hash no SQLite local
                    // para que logins futuros sem conexão exijam e validem esta mesma senha.
                    try
                    {
                        string respJson = response.Content.ReadAsStringAsync().Result;
                        var userRetornado = JsonSerializer.Deserialize<Usuario>(respJson, jsonOptions);
                        if (userRetornado != null)
                        {
                            SQLiteDatabase.SalvarFuncionarios(new List<Usuario> { userRetornado });
                        }
                        SQLiteDatabase.AtualizarSenhaHash(loginDigitado, senhaDigitada);
                    }
                    catch { }

                    isOffline = false;
                    return true;
                }
                else
                {
                    if ((int)response.StatusCode >= 500
                        && SQLiteDatabase.ValidarSenhaLocal(loginDigitado, senhaDigitada, out _))
                    {
                        isOffline = true;
                        mensagemErro = string.Empty;
                        return true;
                    }

                    string erroApi = ExtrairMensagemErro(response.Content.ReadAsStringAsync().Result, "Usuário ou senha incorretos.");
                    mensagemErro = erroApi;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ValidarLogin] API indisponível ({ex.Message}). Ativando contingência offline...");

                // 2. API offline -> Valida credenciais no banco local SQLite
                bool senhaCorreta = SQLiteDatabase.ValidarSenhaLocal(loginDigitado, senhaDigitada, out Usuario? userLocal);

                if (senhaCorreta && userLocal != null)
                {
                    isOffline = true;
                    mensagemErro = string.Empty;
                    return true;
                }

                // Verifica se o usuário existe localmente para fornecer mensagem informativa
                string prefixo = loginDigitado.Contains("@") ? loginDigitado.Split('@')[0] : loginDigitado;
                var userExisteLocal = SQLiteDatabase.ObterFuncionarios()
                    .Find(u => string.Equals(u.Login, loginDigitado, StringComparison.OrdinalIgnoreCase) ||
                               string.Equals(u.Login, prefixo, StringComparison.OrdinalIgnoreCase) ||
                               (u.Login.Contains("@") && u.Login.Split('@')[0].Equals(prefixo, StringComparison.OrdinalIgnoreCase)));

                if (userExisteLocal != null)
                {
                    mensagemErro = "A API não está respondendo e a senha informada não coincide com o registro local desta máquina.\n\nCaso seja seu primeiro acesso com este usuário, inicie a API para validar as credenciais online.";
                }
                else
                {
                    mensagemErro = $"Usuário '{loginDigitado}' não encontrado nesta máquina. Conecte-se à API para entrar pela primeira vez.";
                }

                return false;
            }
        }

        public bool ValidarLogin(Usuario usuario, out string mensagemErro)
        {
            return ValidarLogin(usuario, out mensagemErro, out _);
        }

        // Solicitar Reset Senha (REST: POST /user/solicitarResetSenha com JSON body)
        public bool SolicitarResetSenha(string email, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                var payload = new { login = email.Trim() };
                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/solicitarResetSenha", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        mensagemErro = "Nenhum usuário cadastrado com este e-mail.";
                    }
                    else
                    {
                        string body = response.Content.ReadAsStringAsync().Result;
                        mensagemErro = string.IsNullOrWhiteSpace(body) ? $"Erro na solicitação ({response.StatusCode})." : body;
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Não foi possível conectar à API: {ex.Message}";
                return false;
            }
        }

        public bool SolicitarResetSenha(string email)
        {
            return SolicitarResetSenha(email, out _);
        }

        // Resetar Senha (REST: POST /user/resetSenha com JSON body)
        public bool ResetarSenha(string email, string codigo, string novaSenha, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            try
            {
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                var payload = new
                {
                    login = email.Trim(),
                    codigo = codigoLimpo,
                    novaSenha = novaSenha
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync($"{API_URL}/resetSenha", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string body = response.Content.ReadAsStringAsync().Result;
                    mensagemErro = string.IsNullOrWhiteSpace(body) ? "Código inválido ou expirado." : body;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                mensagemErro = $"Erro de conexão com o servidor: {ex.Message}";
                return false;
            }
        }

        public bool ResetarSenha(string email, string codigo, string novaSenha)
        {
            return ResetarSenha(email, codigo, novaSenha, out _);
        }

        // Confirmar Email (REST: POST /user/confirmarEmail com JSON body)
        public bool ConfirmarEmail(string email, string codigo, out string respostaErro)
        {
            respostaErro = string.Empty;
            try
            {
                string codigoLimpo = codigo.Replace(" ", "").Replace("_", "").Trim();

                var payload = new
                {
                    email = email.Trim(),
                    codigo = codigoLimpo
                };

                string json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = client.PostAsync($"{API_URL}/confirmarEmail", content).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    respostaErro = ExtrairMensagemErro(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                        "A API recusou o código. Confira os seis dígitos ou solicite outro.");
                    return false;
                }

                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException)
            {
                respostaErro = "Não foi possível conectar à API para confirmar o e-mail.";
                return false;
            }
        }

        // Obter Usuário por Login
        public Usuario? ObterPorLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login)) return null;

            string loginLimpo = login.Trim();
            try
            {
                string url = $"{API_URL}/login/{Uri.EscapeDataString(loginLimpo)}";
                HttpResponseMessage res = client.GetAsync(url).Result;
                if (res.IsSuccessStatusCode)
                {
                    string json = res.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<Usuario>(json, jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UsuarioService.ObterPorLogin] Erro: {ex.Message}");
            }

            string prefixo = loginLimpo.Contains("@") ? loginLimpo.Split('@')[0] : loginLimpo;
            return SQLiteDatabase.ObterFuncionarios().Find(u =>
                string.Equals(u.Login, loginLimpo, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(u.Login, prefixo, StringComparison.OrdinalIgnoreCase) ||
                (u.Login.Contains("@") && u.Login.Split('@')[0].Equals(prefixo, StringComparison.OrdinalIgnoreCase)));
        }

        // Reenviar Código de Cadastro
        public bool ReenviarCodigoCadastro(string email, out string mensagemRetorno)
        {
            mensagemRetorno = string.Empty;
            try
            {
                string urlReenvio = $"{API_URL}/reenviarCodigoCadastro?email={Uri.EscapeDataString(email.Trim().ToLowerInvariant())}";
                using var request = new HttpRequestMessage(HttpMethod.Put, urlReenvio);
                using var resReenvio = clientEdicao.SendAsync(request).GetAwaiter().GetResult();

                if (resReenvio.IsSuccessStatusCode)
                {
                    mensagemRetorno = "A API aceitou o reenvio. Confira a caixa de entrada e o spam.";
                    return true;
                }

                mensagemRetorno = ExtrairMensagemErro(resReenvio.Content.ReadAsStringAsync().Result, "Não foi possível solicitar o reenvio.");
                return false;
            }
            catch (Exception ex)
            {
                mensagemRetorno = ex.Message;
                return false;
            }
        }

        // Cadastrar (REST: POST /user compatível com multipart/form-data e fallback offline)
        public bool Cadastrar(Usuario usuario, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            usuario.Login = usuario.Login.Trim().ToLowerInvariant();
            if (!System.Net.Mail.MailAddress.TryCreate(usuario.Login, out var endereco) || endereco.Address != usuario.Login)
            {
                mensagemErro = "Informe um endereço de e-mail válido.";
                return false;
            }
            if (SQLiteDatabase.ExisteEmailLocal(usuario.Login))
            {
                mensagemErro = "Este e-mail já está cadastrado.";
                return false;
            }

            try
            {
                using var consulta = client.GetAsync(API_URL).GetAwaiter().GetResult();
                if (!consulta.IsSuccessStatusCode)
                {
                    mensagemErro = "A API não conseguiu verificar se o e-mail já está cadastrado.";
                    return false;
                }
                var contas = JsonSerializer.Deserialize<List<Usuario>>(consulta.Content.ReadAsStringAsync().GetAwaiter().GetResult(), jsonOptions);
                if (contas == null)
                {
                    mensagemErro = "A API não retornou a lista de contas.";
                    return false;
                }
                if (contas.Any(conta => string.Equals(conta.Login?.Trim(), usuario.Login, StringComparison.OrdinalIgnoreCase)))
                {
                    mensagemErro = "Este e-mail já está cadastrado. Se a conta estiver pendente, solicite outro código.";
                    return false;
                }
            }
            catch (HttpRequestException ex) when (ex.HttpRequestError == HttpRequestError.ConnectionError)
            {
                if (SQLiteDatabase.SalvarFuncionarioLocal(usuario, usuario.Senha)) return true;
                mensagemErro = SQLiteDatabase.ExisteEmailLocal(usuario.Login)
                    ? "Este e-mail já está cadastrado." : "Não foi possível salvar o cadastro neste computador.";
                return false;
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException or JsonException)
            {
                mensagemErro = "Não foi possível verificar o e-mail na API. Tente novamente.";
                return false;
            }

            try
            {
                using var multipartContent = new MultipartFormDataContent();

                var dados = new
                {
                    nome = usuario.Nome,
                    login = usuario.Login,
                    senhaHash = usuario.Senha,
                    funcionario = usuario.Funcionario
                };

                string json = JsonSerializer.Serialize(dados);
                var userContent = new StringContent(json, Encoding.UTF8, "application/json");
                userContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                multipartContent.Add(userContent, "user");

                using var response = clientEdicao.PostAsync(API_URL, multipartContent).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    mensagemErro = ExtrairMensagemErro(response.Content.ReadAsStringAsync().Result, "Erro ao cadastrar funcionário na API.");
                    return false;
                }

                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or OperationCanceledException)
            {
                mensagemErro = "A API não confirmou o cadastro. Verifique se a conta já existe antes de tentar novamente.";
                return false;
            }
        }
    }
}
