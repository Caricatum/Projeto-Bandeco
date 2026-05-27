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

        user.setSenhaHash(encoder.encode(user.getSenhaHash()));
        userRepo.save(user);
    }

    @GetMapping("/validar")//valida Login do usuário
    public ResponseEntity<Boolean> validarUser(@RequestParam String login,
                                               @RequestParam String senhaHash){

        Optional<User> optUser = userRepo.findByLogin(login);
        if (optUser.isEmpty()){
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(false);
        }
        boolean valid = false;

        valid = encoder.matches(senhaHash, optUser.get().getSenhaHash());

        HttpStatus status = (valid) ? HttpStatus.OK : HttpStatus.UNAUTHORIZED;
        if (!valid){

        }
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

        user.setCodigo(codigo);
        user.setExpiracaoCodigo(LocalDateTime.now().plusMinutes(15));

        userRepo.save(user);

        emailService.enviarCodigo(login, codigo);
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

        if(user.getCodigo() == null ||
                !user.getCodigo().equals(codigo)){

            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código inválido");
        }

        if(LocalDateTime.now().isAfter(user.getExpiracaoCodigo())){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Código expirado");
        }

        user.setSenhaHash(encoder.encode(novaSenha));

        user.setCodigo(null);
        user.setExpiracaoCodigo(null);

        userRepo.save(user);
    }
}
