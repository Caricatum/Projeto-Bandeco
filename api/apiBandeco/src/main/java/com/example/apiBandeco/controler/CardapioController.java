package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.CardapioDia;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.CardapioDiaRepository;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.CategoriaRepository;
import com.example.apiBandeco.repository.PratosRepository;
import com.example.apiBandeco.service.PdfGeneratorService;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.io.IOException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.Date;
import java.util.List;

@CrossOrigin
@RestController
@RequestMapping("/cardapio")
public class CardapioController {
    @Autowired
    PratosRepository pratosRepository;
    @Autowired
    CardapioRepository cardapioRepository;
    @Autowired
    CardapioDiaRepository cardapioDiaRepository;
    @Autowired
    PdfGeneratorService pdfGeneratorService;

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

    @DeleteMapping("/deletar/{id}")
    public void deletarCardapio(@PathVariable int id) {

        Cardapio cardapio = cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio não encontrado"));

        List<CardapioDia> cardapioDias = cardapioDiaRepository.findAll();

        for (CardapioDia c : cardapioDias) {

            if (c.getPadraoAlmoco() != null &&
                    c.getPadraoAlmoco().getId() == id) {
                c.setPadraoAlmoco(null);
            }

            if (c.getPadraoJantar() != null &&
                    c.getPadraoJantar().getId() == id) {
                c.setPadraoJantar(null);
            }

            if (c.getVeganoAlmoco() != null &&
                    c.getVeganoAlmoco().getId() == id) {
                c.setVeganoAlmoco(null);
            }

            if (c.getVeganoJantar() != null &&
                    c.getVeganoJantar().getId() == id) {
                c.setVeganoJantar(null);
            }
        }

        cardapioDiaRepository.saveAll(cardapioDias);

        cardapioRepository.delete(cardapio);
    }

    @GetMapping("/pdf/{data}/{idNormal}/{idVegano}")
    public void gerarPdf (
            @PathVariable @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate data,
            @PathVariable int idNormal,
            @PathVariable int idVegano,
            HttpServletResponse response) throws IOException {

        Cardapio cardapioNormal = cardapioRepository.findById(idNormal)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio normal não encontrado"
                ));

        Cardapio cardapioVegano = cardapioRepository.findById(idVegano)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio vegano não encontrado"
                ));


        response.setContentType("application/pdf");

        String headerKey = "Content-Disposition";
        String headerValue = "attachment; filename=cardapio_" + data + ".pdf";

        response.setHeader(headerKey, headerValue);

        pdfGeneratorService.export(
                response,
                cardapioNormal,
                cardapioVegano,
                data
        );

    }


}
