package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Cardapio;
import com.example.apiBandeco.model.CardapioDia;
import com.example.apiBandeco.repository.CardapioDiaRepository;
import com.example.apiBandeco.repository.CardapioRepository;
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
import java.time.LocalDate;
import java.util.List;

@CrossOrigin
@RestController
@RequestMapping({"/cardapio", "/cardapios", "/api/v1/cardapios"})
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

    // Busca cardápio por ID (REST: GET /cardapios/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public Cardapio buscarPorId(@PathVariable("id") int id){
        return cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"
                ));
    }

    // Listagem de cardápios (REST: GET /cardapios)
    @GetMapping
    public List<Cardapio> buscarTodosCardapios(){
        return cardapioRepository.findAll();
    }

    // Cadastro de cardápio (REST: POST /cardapios)
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public Cardapio cadastroCardapio (@RequestBody @Valid Cardapio cardapio){
        validaPratos(cardapio);
        return cardapioRepository.save(cardapio);
    }

    // Atualização de cardápio (REST: PUT /cardapios/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public Cardapio atualizaCardapio (@PathVariable(value = "id", required = false) Integer pathId,
                                      @RequestBody @Valid Cardapio cardapio){
        int id = pathId != null ? pathId : cardapio.getId();
        cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));
        validaPratos(cardapio);
        return cardapioRepository.save(cardapio);
    }

    // Exclusão de cardápio (REST: DELETE /cardapios/{id})
    @DeleteMapping({"/{id}", "/deletar/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarCardapio(@PathVariable int id) {

        Cardapio cardapio = cardapioRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio não encontrado"));

        List<CardapioDia> cardapioDias = cardapioDiaRepository.findAll();

        for (CardapioDia c : cardapioDias) {
            if (c.getPadraoAlmoco() != null && c.getPadraoAlmoco().getId() == id) {
                c.setPadraoAlmoco(null);
            }
            if (c.getPadraoJantar() != null && c.getPadraoJantar().getId() == id) {
                c.setPadraoJantar(null);
            }
            if (c.getVeganoAlmoco() != null && c.getVeganoAlmoco().getId() == id) {
                c.setVeganoAlmoco(null);
            }
            if (c.getVeganoJantar() != null && c.getVeganoJantar().getId() == id) {
                c.setVeganoJantar(null);
            }
        }

        cardapioDiaRepository.saveAll(cardapioDias);
        cardapioRepository.delete(cardapio);
    }

    // Geração de PDF (REST: GET /cardapios/export/pdf?data=... ou legado)
    @GetMapping({"/export/pdf", "/pdf", "/pdf/{data}/{idNormal}/{idVegano}"})
    public void gerarPdf (
            @PathVariable(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate data,
            @PathVariable(required = false) Integer idNormal,
            @PathVariable(required = false) Integer idVegano,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dataQuery,
            @RequestParam(required = false) Integer padraoId,
            @RequestParam(required = false) Integer veganoId,
            HttpServletResponse response) throws IOException {

        LocalDate targetData = data != null ? data : dataQuery;
        Integer targetNormal = idNormal != null ? idNormal : padraoId;
        Integer targetVegano = idVegano != null ? idVegano : veganoId;

        if (targetData == null || targetNormal == null || targetVegano == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Parâmetros data, normal e vegano são obrigatórios");
        }

        Cardapio cardapioNormal = cardapioRepository.findById(targetNormal)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio normal não encontrado"
                ));

        Cardapio cardapioVegano = cardapioRepository.findById(targetVegano)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND,
                        "Cardápio vegano não encontrado"
                ));

        response.setContentType("application/pdf");

        String headerKey = "Content-Disposition";
        String headerValue = "attachment; filename=cardapio_" + targetData + ".pdf";

        response.setHeader(headerKey, headerValue);

        pdfGeneratorService.export(
                response,
                cardapioNormal,
                cardapioVegano,
                targetData
        );
    }
}
