package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.PratosRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/cardapio")
public class CardapioController {
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CardapioRepository cardapioRepository;

    private void validaPratos(Cardapio cardapio){
        Integer acompanhamentoId = cardapio.getAcompanhamento() != null
                ? cardapio.getAcompanhamento().getId() : null;
        Integer prato_principalId = cardapio.getPrato_principal() != null
                ? cardapio.getPrato_principal().getId() : null;
        Integer guarnicaoId = cardapio.getGuarnicao() != null
                ? cardapio.getGuarnicao().getId() : null;
        Integer saladaId = cardapio.getSalada() != null
                ? cardapio.getSalada().getId() : null;
        Integer sobremesaId = cardapio.getSobremesa() != null
                ? cardapio.getSobremesa().getId() : null;
        Integer refrescoId = cardapio.getRefresco() != null
                ? cardapio.getRefresco().getId() : null;

        if (acompanhamentoId != null){
            var acompanhamento = pratosRepository.findById(acompanhamentoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Acompanhamento não encontrado"));
            cardapio.setAcompanhamento(acompanhamento);
        }
        if (prato_principalId != null){
            var prato_principal = pratosRepository.findById(prato_principalId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Prato principal não encontrado"));
            cardapio.setPrato_principal(prato_principal);
        }
        if (guarnicaoId != null){
            var guarnicao = pratosRepository.findById(guarnicaoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Guarnicao não encontrada"));
            cardapio.setGuarnicao(guarnicao);
        }
        if (saladaId != null){
            var salada = pratosRepository.findById(saladaId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Salada não encontrada"));
            cardapio.setSalada(salada);
        }
        if (sobremesaId != null){
            var sobremesa = pratosRepository.findById(sobremesaId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Sobremesa não encontrada"));
            cardapio.setSobremesa(sobremesa);
        }
        if (refrescoId != null){
            var refresco = pratosRepository.findById(refrescoId)
                    .orElseThrow(() -> new ResponseStatusException(
                            HttpStatus.NOT_FOUND, "Refresco não encontrado"));
            cardapio.setRefresco(refresco);
        }
    }

    @GetMapping("/id/{id}")//busca cardapio pelo id
    public Cardapio buscarPorId(@PathVariable("id") int id){
        return cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"
                ));
    }

    @GetMapping("/all")//busca todos os cardápios
    public List<Cardapio> buscarTodosCardapios(){return cardapioRepository.findAll();}

    @PostMapping("/cadastrar")//cadastra um cardápio
    public Cardapio cadastroCardapio (@RequestBody @Valid Cardapio cardapio){
        validaPratos(cardapio);
        return cardapioRepository.save(cardapio);
    }

    @PutMapping("/atualizar")
    public Cardapio atualizaCardapio (@RequestBody @Valid Cardapio cardapio){
        cardapioRepository.findById(cardapio.getId())
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));
        validaPratos(cardapio);
        return cardapioRepository.save(cardapio);
    }

    @DeleteMapping("/deletar/{id}") //deleta Cardápio pelo id
    public void deletarCardapio(@PathVariable(value = "id") int id){
        var cardapio = cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));

        cardapioRepository.delete(cardapio);
    }
}
