package com.example.apiBandeco.controler;

import com.example.apiBandeco.email.EmailService;
import com.example.apiBandeco.model.Pratos;
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
import java.util.Optional;
import java.util.UUID;

@CrossOrigin
@RestController
@RequestMapping("/user")
public class UserController {
    @Autowired
    UserRepository userRepo;
    private final PasswordEncoder encoder;

    @Autowired
    private EmailService emailService;

    public UserController(PasswordEncoder encoder) {
        this.encoder = encoder;
    }


    @GetMapping("/id/{id}")//busca um usuário pelo id
    public Optional<User> buscarPorId(@PathVariable("id") int id){
        return userRepo.findById(id);
    }

    @GetMapping("/login/{login}")//busca um usuário pelo login
    public Optional<User> buscarPorLogin(@PathVariable("login") String login){
        return userRepo.findByLogin(login);
    }

    @GetMapping("/all")//busca todos os usuários
    public List<User> buscarTodosUsers(){return userRepo.findAll();}

    @PostMapping(value = "/cadastrar",consumes = MediaType.MULTIPART_FORM_DATA_VALUE)//cadastra um usuário
    public void cadastroUser(@RequestPart("user") @Valid User user,
    @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
            throws IOException{
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

        userRepo.save(user);

        emailService.enviarCodigo(
                user,
                codigo
        );
    }

    @PutMapping("/reenviarCodigoCadastro")
    public void reenviarCodigoCadastro(@RequestParam int id){
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"
                ));

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
    }

    @PostMapping("/confirmarEmail")
    public void confirmarEmail(
            @RequestParam String email,
            @RequestParam String codigo){


        User user = userRepo.findByLogin(email)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if(user.isEmailConfirmado()){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "E-mail já confirmado");
        }

        if(user.getCodigoConfirmacao() == null ||
                !user.getCodigoConfirmacao().equals(codigo)){

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
    }

    @GetMapping("/validar")//valida Login do usuário
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

    @GetMapping("/validarFunc")//valida Login do usuário
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



    @DeleteMapping("/deletar/{id}") //deleta user pelo id
    public void deletarUser (@PathVariable(value = "id") int id) throws IOException{
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "User não encontrado"));
        deletarImagem(user.getId());
        userRepo.deleteById(id);
    }

    @DeleteMapping(value = "/excluirImagem/{id}")
    public void excluirImagem(@PathVariable int id) throws IOException{
        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "User não encontrado"));

        deletarImagem(user.getId());
        user.setImagem(null);
        userRepo.save(user);
    }

    @PutMapping(value = "/atualizar",consumes = MediaType.MULTIPART_FORM_DATA_VALUE)//Atualiza User menos a senha
    public User atualizaUser (@RequestPart("user") @Valid User user,
                              @RequestPart(value = "imagem", required = false) MultipartFile arquivo)
    throws IOException{

        User userAtualizado = userRepo.findById(user.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "User não encontrado"));

        if (arquivo != null && !arquivo.isEmpty()) {
            String urlImagem = uploadImagem(arquivo);
            deletarImagem(user.getId());
            userAtualizado.setImagem(urlImagem);
        } else {
            user.setImagem(
                    userAtualizado.getImagem()
            );
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

    @PostMapping("/solicitarResetSenha")
    public void solicitarResetSenha(@RequestParam String login){

        User user = userRepo.findByLogin(login)
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
    }

    @PutMapping("/resetSenha") //resetar senha
    public void resetSenha(
            @RequestParam String login,
            @RequestParam String codigo,
            @RequestParam String novaSenha){

        User user = userRepo.findByLogin(login)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Usuário não encontrado"));

        if(user.getCodigoResetSenha() == null ||
                !user.getCodigoResetSenha().equals(codigo)){

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código inválido");
        }

        if(LocalDateTime.now().isAfter(user.getExpiracaoResetSenha())){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código expirado");
        }

        user.setSenhaHash(encoder.encode(novaSenha));

        user.setCodigoResetSenha(null);
        user.setExpiracaoResetSenha(null);

        userRepo.save(user);
    }

    //envia a imagem e retorna seu link
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

    // Deleta a imagem do prato
    private void deletarImagem(Integer id) throws IOException {

        User user = userRepo.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Prato não encontrado"));

        if (user.getImagem() == null || user.getImagem().isBlank()) {
            return;
        }

        String nomeArquivo = user.getImagem()
                .substring(user.getImagem().lastIndexOf("/") + 1);

        Path caminho = Paths.get("uploads/user", nomeArquivo);

        Files.deleteIfExists(caminho);
    }
}
