package com.example.apiBandeco.controler;

import com.example.apiBandeco.email.EmailService;
import com.example.apiBandeco.model.User;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

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

    @PostMapping("/cadastrar")//cadastra um usuário
    public void cadastroUser (@RequestBody @Valid User user){
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
    public void deletarUser (@PathVariable(value = "id") int user){
        userRepo.deleteById(user);
    }

    @PutMapping("/atualizar") //Atualiza User menos a senha
    public User atualizaUser (@RequestBody @Valid User user){

        User userAtualizado = userRepo.findById(user.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "User não encontrado"));

        userAtualizado.setLogin(user.getLogin());
        userAtualizado.setNome(user.getNome());
        userAtualizado.setFuncionario(user.isFuncionario());

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
}
