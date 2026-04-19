package com.example.apiBandeco.controler;


import com.example.apiBandeco.model.Avaliacoes;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.AvaliacoesRepository;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;
@CrossOrigin
@RestController
@RequestMapping("/avaliacoes")
public class AvaliacoesControler {
    @Autowired
    AvaliacoesRepository avaliacoesRepository;
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    UserRepository userRepository;

    @GetMapping("/id/{id}")//busca avaliacao pelo id
    public Avaliacoes buscarPorId(@PathVariable("id") int id){
        return avaliacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Avaliação não encontrada"
                ));
    }

    @GetMapping("/all")//busca todas as avaliações
    public List<Avaliacoes> buscarTodasAvaliacoes(){return avaliacoesRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra uma avaliação
    public Avaliacoes cadastroAvaliacoes (@RequestBody @Valid Avaliacoes avaliacao){
        int pratoId = avaliacao.getPrato().getId();
        int userId = avaliacao.getUser().getId();

        var prato = pratosRepository.findById(pratoId)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não existe"));

        var user = userRepository.findById(userId)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "User não existe"));

        avaliacao.setPrato(prato);
        avaliacao.setUser(user);

        boolean jaExiste = avaliacoesRepository
                .existsByUserIdAndPratoId(userId, pratoId);

        if (jaExiste) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST, "Avaliação já feita");
        }

        return avaliacoesRepository.save(avaliacao);
    }

    @PutMapping("/atualizar")
    public Avaliacoes atualizaAvaliacao (@RequestBody @Valid Avaliacoes avaliacao){

        avaliacoesRepository.findById(avaliacao.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Avaliação não encontrada"));

        int pratoId = avaliacao.getPrato().getId();
        int userId = avaliacao.getUser().getId();

        boolean jaExiste = avaliacoesRepository
                .existsByUserIdAndPratoIdAndIdNot(userId, pratoId, avaliacao.getId());

        if (jaExiste) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST, "Avaliação já feita para este prato");
        }

        var prato = pratosRepository.findById(pratoId)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Prato não existe"));

        var user = userRepository.findById(userId)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "User não existe"));


        avaliacao.setPrato(prato);
        avaliacao.setUser(user);

        return avaliacoesRepository.save(avaliacao);
    }

    @DeleteMapping("/deletar/{id}") //deleta Avaliação pelo id
    public void deletarAvaliacao(@PathVariable(value = "id") int id){
        var avaliacao = avaliacoesRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Avaliação não encontrada"));

        avaliacoesRepository.delete(avaliacao);
    }
}
