package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.User;
import com.example.apiBandeco.repository.UserRepository;
import org.apache.catalina.LifecycleState;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@CrossOrigin
@RestController
@RequestMapping("/user")
public class UserControler {
    @Autowired
    UserRepository userRepo;
    private final PasswordEncoder encoder;

    public UserControler(PasswordEncoder encoder) {
        this.encoder = encoder;
    }


    @GetMapping("/id/{id}")//busca um usuário pelo id
    public Optional<User> buscarPorId(@PathVariable("id") int id){
        return userRepo.findById(id);
    }

    @GetMapping("/all")//busca todos os usuários
    public List<User> buscarTodosUsers(){return userRepo.findAll();}

    @PostMapping("/cadastrarUser")//cadastra um usuário
    public void cadastroUser (@RequestBody User user){

        user.setSenha_hash(encoder.encode(user.getSenha_hash()));
        userRepo.save(user);
    }

    @GetMapping("/validarUser")//valida Login do usuário
    public ResponseEntity<Boolean> validarUser(@RequestParam String login,
                                               @RequestParam String senha){

        Optional<User> optUser = userRepo.findByLogin(login);
        if (optUser.isEmpty()){
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body(false);
        }
        boolean valid = false;

        valid = encoder.matches(senha, optUser.get().getSenha_hash());

        HttpStatus status = (valid) ? HttpStatus.OK : HttpStatus.UNAUTHORIZED;
        if (!valid){

        }
        return ResponseEntity.status(status).body(valid);
    }

    @DeleteMapping("/deletarUser/{id}") //deleta user pelo id
    public void deletarUser (@PathVariable(value = "id") int user){
        userRepo.deleteById(user);
    }

    @PutMapping("/atualizarUser") //Atualiza User
    public void atualizaUser (@RequestBody User user){
        userRepo.save(user);
    }


}
