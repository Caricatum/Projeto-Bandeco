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
@RequestMapping({"/notificacoes", "/api/v1/notificacoes"})
public class NotificacoesController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    NotificacoesRepository notificacoesRepository;

    // Busca notificação por ID (REST: GET /notificacoes/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public Notificacoes buscarPorId(@PathVariable("id") int id){
        return notificacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Notificação não encontrada"
                ));
    }

    // Listagem de notificações (REST: GET /notificacoes)
    @GetMapping
    public List<Notificacoes> buscarTodasNotificacoes(){
        return notificacoesRepository.findAll();
    }

    // Cadastro de notificação (REST: POST /notificacoes)
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
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

    // Atualização de notificação (REST: PUT /notificacoes/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public Notificacoes atualizaNotificacao (@PathVariable(value = "id", required = false) Integer pathId,
                                             @RequestBody @Valid Notificacoes notificacao){
        int id = pathId != null ? pathId : notificacao.getId();
        notificacoesRepository.findById(id)
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
        notificacao.setId(id);
        return notificacoesRepository.save(notificacao);
    }

    // Exclusão de notificação (REST: DELETE /notificacoes/{id})
    @DeleteMapping({"/{id}", "/deletar/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarNotificacao(@PathVariable(value = "id") int id){
        var notificacao = notificacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Notificação não encontrada"));

        notificacoesRepository.delete(notificacao);
    }
}
