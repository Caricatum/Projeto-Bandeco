package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.CardapioDia;
import com.example.apiBandeco.repository.CardapioDiaRepository;
import com.example.apiBandeco.repository.CardapioRepository;
import com.example.apiBandeco.repository.UserRepository;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.time.LocalDate;
import java.util.List;

@CrossOrigin
@RestController
@RequestMapping({"/cardapioDia", "/cardapios-dia", "/api/v1/cardapios-dia"})
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

    // Busca cardápio do dia por ID (REST: GET /cardapios-dia/{id})
    @GetMapping({"/{id}", "/id/{id}"})
    public CardapioDia buscarPorId(@PathVariable("id") int id){
        return cardapioDiaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio do dia não encontrado"
                ));
    }

    // Busca cardápio do dia por data (REST: GET /cardapios-dia/data/{data})
    @GetMapping({"/data/{data}", "/data"})
    public CardapioDia buscarPorData(
            @PathVariable(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate data,
            @RequestParam(required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate dataParam){
        LocalDate target = data != null ? data : dataParam;
        if (target == null) {
            throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Data não informada");
        }
        return cardapioDiaRepository.findByData(target)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio do dia não encontrado"
                ));
    }

    // Busca os cardápios da semana atual (REST: GET /cardapios-dia/semana)
    @GetMapping("/semana")
    public List<CardapioDia> buscarCardapiosSemana() {
        LocalDate hoje = LocalDate.now();

        int diasDesdeDomingo = hoje.getDayOfWeek().getValue() % 7;

        LocalDate inicioSemana = hoje.minusDays(diasDesdeDomingo);
        LocalDate fimSemana = inicioSemana.plusDays(7);

        return cardapioDiaRepository.findByDataBetween(
                inicioSemana,
                fimSemana
        );
    }

    // Listagem geral (REST: GET /cardapios-dia)
    @GetMapping
    public List<CardapioDia> buscarTodosCardapiosDias(){
        return cardapioDiaRepository.findAll();
    }

    // Cadastro de cardápio do dia (REST: POST /cardapios-dia)
    @PostMapping({"", "/cadastrar"})
    @ResponseStatus(HttpStatus.CREATED)
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

    // Atualização de cardápio do dia (REST: PUT /cardapios-dia/{id})
    @PutMapping({"/{id}", "/atualizar", ""})
    public CardapioDia atualizaCardapioDia (@PathVariable(value = "id", required = false) Integer pathId,
                                            @RequestBody @Valid CardapioDia cardapioDia){
        int id = pathId != null ? pathId : cardapioDia.getId();
        cardapioDiaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));

        if(cardapioDiaRepository.existsByDataAndIdNot(cardapioDia.getData(), id)){
            throw new ResponseStatusException(
                    HttpStatus.BAD_REQUEST,
                    "Já existe cardápio cadastrado para esta data"
            );
        }
        validaCardapios(cardapioDia);
        return cardapioDiaRepository.save(cardapioDia);
    }

    // Exclusão de cardápio do dia (REST: DELETE /cardapios-dia/{id})
    @DeleteMapping({"/{id}", "/deletar/{id}"})
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deletarCardapioDia(@PathVariable(value = "id") int id){
        var cardapioDia = cardapioDiaRepository.findById(id)
                .orElseThrow(() -> new ResponseStatusException(
                        HttpStatus.NOT_FOUND, "Cardápio não encontrado"));

        cardapioDiaRepository.delete(cardapioDia);
    }
}
