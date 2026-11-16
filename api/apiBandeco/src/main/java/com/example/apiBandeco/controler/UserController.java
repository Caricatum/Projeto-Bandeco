package com.example.apiBandeco.controler;

import com.example.apiBandeco.email.EmailService;
import com.example.apiBandeco.model.User;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.UUID;

@CrossOrigin
@RestController
@RequestMapping({"/user", "/usuarios", "/api/v1/usuarios", "/api/v1/user"})
public class UserController {
    @Autowired
    UserRepository userRepo;
    private final PasswordEncoder encoder;

    @Autowired
    private EmailService emailService;

    public UserController(PasswordEncoder encoder) {
        this.encoder = encoder;
    }

    // Busca usuário por ID (REST: GET /user/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public User buscarPorId(@PathVariable("id") int id){
        return userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Usuário não encontrado"
                ));
    }

    // Busca usuário pelo login (REST: GET /user/login/{login} ou GET /user?login=...)
    @GetMapping({"/login/{login}"})
    public User buscarPorLogin(@PathVariable("login") String login){
        return userRepo.findByLogin(login)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Usuário não encontrado"
                ));
    }

    // Lista usuários ou filtra por login (REST: GET /user ou GET /user?login=...)
    @GetMapping
    public Object buscarTodosUsers(@RequestParam(required = false) String login){
        if (login != null && !login.isBlank()) {
            return userRepo.findByLogin(login)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Usuário não encontrado"
                    ));
        }
        return userRepo.findAll();
    }

    // Cadastra um usuário (REST: POST /user)
    @PostMapping(value = {"", "/cadastrar"}, consumes = {MediaType.MULTIPART_FORM_DATA_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @ResponseStatus(HttpStatus.CREATED)
    public User cadastroUser(@RequestPart("user") @Valid User user,
                             @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException {
        if(userRepo.findByLogin(user.getLogin()).isPresent()){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "E-mail já cadastrado");
        }

        String codigo = String.valueOf(
                (int)(Math.random() * 900000) + 100000
        );

        user.setSenhaHash(
                encoder.encode(user.getSenhaHash())
        );

        if (arquivo != null && !arquivo.isEmpty()) {
            String urlImagem = uploadImagem(arquivo);
            user.setImagem(urlImagem);
        } else {
            user.setImagem(null);
        }
        user.setEmailConfirmado(false);
        user.setCodigoConfirmacao(codigo);
        user.setExpiracaoConfirmacao(
                LocalDateTime.now().plusMinutes(15)
        );

        User saved = userRepo.save(user);

        emailService.enviarCodigo(
                saved,
                codigo
        );

        return saved;
    }

    // Login seguro padrão REST (POST com JSON no corpo)
    @PostMapping(value = {"/login", "/auth/login"}, consumes = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<User> loginRest(@RequestBody Map<String, String> credenciais) {
        String login = credenciais.get("login");
        String senha = credenciais.containsKey("senha") ? credenciais.get("senha") : credenciais.get("senhaHash");

        if (login == null || senha == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Login e senha são obrigatórios");
        }

        User user = userRepo.findByLogin(login)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Usuário ou senha incorretos"));

        if (!user.isEmailConfirmado()) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "E-mail não confirmado");
        }

        if (!encoder.matches(senha, user.getSenhaHash())) {
            throw new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Usuário ou senha incorretos");
        }

        return ResponseEntity.ok(user);
    }

    // Login exclusivo para funcionários padrão REST (POST com JSON no corpo)
    @PostMapping(value = {"/login-funcionario", "/auth/login-funcionario"}, consumes = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<User> loginFuncionarioRest(@RequestBody Map<String, String> credenciais) {
        String login = credenciais.get("login");
        String senha = credenciais.containsKey("senha") ? credenciais.get("senha") : credenciais.get("senhaHash");

        if (login == null || senha == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Login e senha são obrigatórios");
        }

        User user = userRepo.findByLogin(login)
                .orElseThrow(() -> new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Usuário ou senha incorretos"));

        if (!user.isEmailConfirmado()) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "E-mail não confirmado");
        }

        if (!user.isFuncionario()) {
            throw new ResponseStatusException(HttpStatus.FORBIDDEN, "Usuário não é um funcionário");
        }

        if (!encoder.matches(senha, user.getSenhaHash())) {
            throw new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Usuário ou senha incorretos");
        }

        return ResponseEntity.ok(user);
    }

    // Compatibilidade legada para validar Login
    @GetMapping("/validar")
    public ResponseEntity<Boolean> validarUser(@RequestParam String login,
                                               @RequestParam String senhaHash){
        Optional<User> optUser = userRepo.findByLogin(login);

        if (optUser.isEmpty()){
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body(false);
        }

        User user = optUser.get();

        if(!user.isEmailConfirmado()){
            return ResponseEntity
                    .status(HttpStatus.FORBIDDEN)
                    .body(false);
        }

        boolean valid =
                encoder.matches(senhaHash, user.getSenhaHash());

        HttpStatus status =
                valid ? HttpStatus.OK : HttpStatus.UNAUTHORIZED;

        return ResponseEntity.status(status).body(valid);
    }

    // Compatibilidade legada para validar Login de funcionário
    @GetMapping("/validarFunc")
    public ResponseEntity<Boolean> validarFunc(@RequestParam String login,
                                               @RequestParam String senhaHash){
        Optional<User> optUser = userRepo.findByLogin(login);

        if (optUser.isEmpty()){
            return ResponseEntity
                    .status(HttpStatus.UNAUTHORIZED)
                    .body(false);
        }

        User user = optUser.get();

        if(!user.isEmailConfirmado()){
            return ResponseEntity
                    .status(HttpStatus.FORBIDDEN)
                    .body(false);
        }

        if (!user.isFuncionario()){
            throw new ResponseStatusException(
                    HttpStatus.FORBIDDEN,
                    "User não é um funcionário"
            );
        }

        boolean valid =
                encoder.matches(senhaHash, user.getSenhaHash());

        HttpStatus status =
                valid ? HttpStatus.OK : HttpStatus.UNAUTHORIZED;

        return ResponseEntity.status(status).body(valid);
    }

    // Reenviar código de confirmação
    @RequestMapping(value = {"/reenviarCodigoCadastro", "/reenviar-codigo"}, method = {RequestMethod.PUT, RequestMethod.POST})
    public ResponseEntity<Map<String, String>> reenviarCodigoCadastro(
            @RequestParam(required = false) Integer id,
            @RequestParam(required = false) String email,
            @RequestBody(required = false) Map<String, Object> body){

        Integer userId = id;
        String userEmail = email;

        if (userId == null && body != null && body.containsKey("id")) {
            userId = Integer.parseInt(body.get("id").toString());
        }
        if (userEmail == null && body != null && body.containsKey("email")) {
            userEmail = body.get("email").toString();
        }

        User user;
        if (userId != null) {
            user = userRepo.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Usuário não encontrado"));
        } else if (userEmail != null) {
            user = userRepo.findByLogin(userEmail)
                    .orElseThrow(() -> new ResponseStatusException(HttpStatus.NOT_FOUND, "Usuário não encontrado"));
        } else {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Identificador ou e-mail do usuário não informado");
        }

        if (user.isEmailConfirmado()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "E-mail já confirmado"
            );
        }

        String codigo = String.valueOf(
                (int)(Math.random() * 900000) + 100000
        );

        user.setEmailConfirmado(false);
        user.setCodigoConfirmacao(codigo);
        user.setExpiracaoConfirmacao(
                LocalDateTime.now().plusMinutes(15)
        );

        userRepo.save(user);

        emailService.enviarCodigo(
                user,
                codigo
        );

        return ResponseEntity.ok(Map.of("mensagem", "Código reenviado com sucesso"));
    }

    // Confirmar e-mail
    @PostMapping({"/confirmarEmail", "/confirmar-email"})
    public ResponseEntity<Map<String, String>> confirmarEmail(
            @RequestParam(required = false) String email,
            @RequestParam(required = false) String codigo,
            @RequestBody(required = false) Map<String, String> body){

        String emailVal = email != null ? email : (body != null ? body.get("email") : null);
        String codigoVal = codigo != null ? codigo : (body != null ? body.get("codigo") : null);

        if (emailVal == null || codigoVal == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "E-mail e código são obrigatórios");
        }

        User user = userRepo.findByLogin(emailVal.trim())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if(user.isEmailConfirmado()){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "E-mail já confirmado");
        }

        if(user.getCodigoConfirmacao() == null ||
                !user.getCodigoConfirmacao().equals(codigoVal.trim())){

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código inválido");
        }

        if(LocalDateTime.now().isAfter(
                user.getExpiracaoConfirmacao())){

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código expirado");
        }

        user.setEmailConfirmado(true);
        user.setCodigoConfirmacao(null);
        user.setExpiracaoConfirmacao(null);

        userRepo.save(user);

        return ResponseEntity.ok(Map.of("mensagem", "E-mail confirmado com sucesso"));
    }

    // Solicitar reset de senha
    @PostMapping({"/solicitarResetSenha", "/solicitar-reset-senha"})
    public ResponseEntity<Map<String, String>> solicitarResetSenha(
            @RequestParam(required = false) String login,
            @RequestBody(required = false) Map<String, String> body){

        String loginVal = login != null ? login : (body != null ? body.get("login") : null);
        if (loginVal == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Login/E-mail é obrigatório");
        }

        User user = userRepo.findByLogin(loginVal.trim())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        String codigo = String.valueOf(
                (int)(Math.random() * 900000) + 100000
        );

        user.setCodigoResetSenha(codigo);
        user.setExpiracaoResetSenha(LocalDateTime.now().plusMinutes(15));

        userRepo.save(user);

        emailService.enviarCodigo(user, codigo);

        return ResponseEntity.ok(Map.of("mensagem", "Código de recuperação enviado"));
    }

    // Resetar senha
    @RequestMapping(value = {"/resetSenha", "/reset-senha"}, method = {RequestMethod.PUT, RequestMethod.POST})
    public ResponseEntity<Map<String, String>> resetSenha(
            @RequestParam(required = false) String login,
            @RequestParam(required = false) String codigo,
            @RequestParam(required = false) String novaSenha,
            @RequestBody(required = false) Map<String, String> body){

        String loginVal = login != null ? login : (body != null ? body.get("login") : null);
        String codigoVal = codigo != null ? codigo : (body != null ? body.get("codigo") : null);
        String senhaVal = novaSenha != null ? novaSenha : (body != null ? (body.containsKey("novaSenha") ? body.get("novaSenha") : body.get("senha")) : null);

        if (loginVal == null || codigoVal == null || senhaVal == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Dados incompletos para redefinição de senha");
        }

        User user = userRepo.findByLogin(loginVal.trim())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if(user.getCodigoResetSenha() == null ||
                !user.getCodigoResetSenha().equals(codigoVal.trim())){

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código inválido");
        }

        if(LocalDateTime.now().isAfter(user.getExpiracaoResetSenha())){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código expirado");
        }

        user.setSenhaHash(encoder.encode(senhaVal));

        user.setCodigoResetSenha(null);
        user.setExpiracaoResetSenha(null);

        userRepo.save(user);

        return ResponseEntity.ok(Map.of("mensagem", "Senha alterada com sucesso"));
    }

    // Atualiza usuário (REST: PUT /user/{id} ou PUT /user)
    @PutMapping(value = {"/{id}", "/atualizar", ""}, consumes = {MediaType.MULTIPART_FORM_DATA_VALUE, MediaType.APPLICATION_JSON_VALUE})
    public User atualizaUser (@PathVariable(value = "id", required = false) Integer pathId,
                              @RequestPart("user") @Valid User user,
                              @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException{

        int id = pathId != null ? pathId : user.getId();
        User userAtualizado = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if (arquivo != null && !arquivo.isEmpty()) {
            String urlImagem = uploadImagem(arquivo);
            deletarImagem(userAtualizado.getId());
            userAtualizado.setImagem(urlImagem);
        } else if (user.getImagem() != null && !user.getImagem().isBlank()) {
            userAtualizado.setImagem(user.getImagem());
        }

        userAtualizado.setLogin(user.getLogin());
        userAtualizado.setNome(user.getNome());
        userAtualizado.setFuncionario(user.isFuncionario());

        userAtualizado.setAlergiaLeite(user.isAlergiaLeite());
        userAtualizado.setAlergiaOvo(user.isAlergiaOvo());
        userAtualizado.setAlergiaAmendoim(user.isAlergiaAmendoim());
        userAtualizado.setAlergiaCastanhas(user.isAlergiaCastanhas());
        userAtualizado.setAlergiaFrutosDoMar(user.isAlergiaFrutosDoMar());
        userAtualizado.setAlergiaPeixes(user.isAlergiaPeixes());
        userAtualizado.setAlergiaTrigo(user.isAlergiaTrigo());
        userAtualizado.setAlergiaSoja(user.isAlergiaSoja());
        userAtualizado.setAlergiaGergilim(user.isAlergiaGergilim());

        return userRepo.save(userAtualizado);
    }

    // Deleta usuário (REST: DELETE /user/{id})
    @DeleteMapping({"/{id}", "/deletar/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarUser (@PathVariable(value = "id") int id) throws IOException{
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));
        deletarImagem(user.getId());
        userRepo.deleteById(id);
    }

    // Exclui apenas imagem do usuário (REST: DELETE /user/{id}/imagem)
    @DeleteMapping(value = {"/{id}/imagem", "/excluirImagem/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void excluirImagem(@PathVariable int id) throws IOException{
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        deletarImagem(user.getId());
        user.setImagem(null);
        userRepo.save(user);
    }

    // Envia a imagem e retorna seu link
    private String uploadImagem(MultipartFile arquivo)
            throws IOException {
        if (arquivo.isEmpty()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Nenhum arquivo enviado");
        }

        if (arquivo.getContentType() == null ||
                !arquivo.getContentType().startsWith("image/")) {

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "O arquivo deve ser uma imagem");
        }

        String nomeOriginal = arquivo.getOriginalFilename();
        String extensao = "";

        if (nomeOriginal != null && nomeOriginal.contains(".")) {
            extensao = nomeOriginal.substring(nomeOriginal.lastIndexOf(".") + 1);
        }

        extensao = extensao.toLowerCase();

        List<String> permitidas = List.of("jpg", "jpeg", "png", "webp");

        if (extensao.isBlank()) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Arquivo sem extensão");
        }

        if (!permitidas.contains(extensao)) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Formato inválido");
        }

        String nomeArquivo = UUID.randomUUID() + "." + extensao;

        Path destino = Paths.get("uploads/user", nomeArquivo);

        Files.createDirectories(destino.getParent());

        Files.copy(
                arquivo.getInputStream(),
                destino,
                StandardCopyOption.REPLACE_EXISTING
        );

        return ServletUriComponentsBuilder
                .fromCurrentContextPath()
                .path("/uploads/user/")
                .path(nomeArquivo)
                .toUriString();
    }

    // Deleta a imagem do usuário
    private void deletarImagem(Integer id) throws IOException {
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if (user.getImagem() == null || user.getImagem().isBlank()) {
            return;
        }

        String nomeArquivo = user.getImagem()
                .substring(user.getImagem().lastIndexOf("/") + 1);

        Path caminho = Paths.get("uploads/user", nomeArquivo);

        Files.deleteIfExists(caminho);
    }
}
