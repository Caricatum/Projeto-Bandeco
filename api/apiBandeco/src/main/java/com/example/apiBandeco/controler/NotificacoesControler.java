package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Notificacoes;
import com.example.apiBandeco.repository.NotificacoesRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/notificacoes")
public class NotificacoesControler {
    @Autowired
    UserRepository userRepository;
    @Autowired
    NotificacoesRepository notificacoesRepository;

    @GetMapping("/id/{id}")//busca notificacao pelo id
    public Notificacoes buscarPorId(@PathVariable("id") int id){
        return notificacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Notificação não encontrada"
                ));
    }

    @GetMapping("/all")//busca todas as notificações
    public List<Notificacoes> buscarTodasNotificacoes(){return notificacoesRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra uma notificação
    public Notificacoes cadastroNotificacao (@RequestBody @Valid Notificacoes notificacao){
        Integer userId = notificacao.getUser() != null
                ? notificacao.getUser().getId() : null;
        if (userId != null){
            var user = userRepository.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "User não encontrado"));
            notificacao.setUser(user);
        }

        return notificacoesRepository.save(notificacao);
    }

    @PutMapping("/atualizar") //Atualiza Notificação
    public Notificacoes atualizaNotificacao (@RequestBody @Valid Notificacoes notificacao){

        notificacoesRepository.findById(notificacao.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Notificação não encontrada"));

        Integer userId = notificacao.getUser() != null
                ? notificacao.getUser().getId() : null;
        if (userId != null){
            var user = userRepository.findById(userId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "User não encontrado"));
            notificacao.setUser(user);
        }

        return notificacoesRepository.save(notificacao);
    }

    @DeleteMapping("/deletar/{id}") //deleta Notificacao pelo id
    public void deletarNotificacao(@PathVariable(value = "id") int id){
        var notificacao = notificacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Notificação não encontrada"));

        notificacoesRepository.delete(notificacao);
    }
}
