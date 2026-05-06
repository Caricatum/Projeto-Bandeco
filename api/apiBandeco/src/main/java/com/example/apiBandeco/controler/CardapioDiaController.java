package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.CardapioDia;
import com.example.apiBandeco.repository.CardapioDiaRepository;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/cardapioDia")
public class CardapioDiaController {
    @Autowired
    CardapioDiaRepository cardapioDiaRepository;
    @Autowired
    CardapioRepository cardapioRepository;
    @Autowired
    UserRepository userRepository;

    private void validaCardapios(CardapioDia cardapioDia){
        Integer padraoAlmocoId = cardapioDia.getPadraoAlmoco() != null
                ? cardapioDia.getPadraoAlmoco().getId() : null;
        Integer veganoAlmocoId = cardapioDia.getVeganoAlmoco() != null
                ? cardapioDia.getVeganoAlmoco().getId() : null;
        Integer padraoJantarId = cardapioDia.getPadraoJantar() != null
                ? cardapioDia.getPadraoJantar().getId() : null;
        Integer veganoJantarId = cardapioDia.getVeganoJantar() != null
                ? cardapioDia.getVeganoJantar().getId() : null;
        if (cardapioDia.getUser() == null) {
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST, "Usuário deve ser informado");
        }
        int userId = cardapioDia.getUser().getId();


        var user = userRepository.findById(userId)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "User não encontrado"));

        cardapioDia.setUser(user);
        if (padraoAlmocoId != null){
            var padraoAlmoco = cardapioRepository.findById(padraoAlmocoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Cardápio padrão de almoço não encontrado"));
            cardapioDia.setPadraoAlmoco(padraoAlmoco);
        }
        if (veganoAlmocoId != null){
            var veganoAlmoco = cardapioRepository.findById(veganoAlmocoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Cardápio vegano de almoço não encontrado"));
            cardapioDia.setVeganoAlmoco(veganoAlmoco);
        }
        if (padraoJantarId != null){
            var padraoJantar = cardapioRepository.findById(padraoJantarId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Cardápio padrão do jantar não encontrado"));
            cardapioDia.setPadraoJantar(padraoJantar);
        }
        if (veganoJantarId != null){
            var veganoJantar = cardapioRepository.findById(veganoJantarId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Cardápio vegano do jantar não encontrado"));
            cardapioDia.setVeganoJantar(veganoJantar);
        }
    }


    @GetMapping("/id/{id}")//busca cardapio do dia pelo id
    public CardapioDia buscarPorId(@PathVariable("id") int id){
        return cardapioDiaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio do dia não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os cardápios de todos os dias
    public List<CardapioDia> buscarTodosCardapiosDias(){return cardapioDiaRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra um cardápio de um dia
    public CardapioDia cadastroCardapioDia (@RequestBody @Valid CardapioDia cardapioDia){
        if(cardapioDiaRepository.existsByData(cardapioDia.getData())){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Já existe cardápio cadastrado para esta data"
            );
        }
        validaCardapios(cardapioDia);
        return cardapioDiaRepository.save(cardapioDia);
    }

    @PutMapping("/atualizar")
    public CardapioDia atualizaCardapioDia (@RequestBody @Valid CardapioDia cardapioDia){
        cardapioDiaRepository.findById(cardapioDia.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));
        if(cardapioDiaRepository.existsByDataAndIdNot(cardapioDia.getData(), cardapioDia.getId())){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Já existe cardápio cadastrado para esta data"
            );
        }
        validaCardapios(cardapioDia);
        return cardapioDiaRepository.save(cardapioDia);
    }

    @DeleteMapping("/deletar/{id}") //deleta Cardápio de um dia pelo id
    public void deletarCardapioDia(@PathVariable(value = "id") int id){
        var cardapioDia = cardapioDiaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));

        cardapioDiaRepository.delete(cardapioDia);
    }


}
