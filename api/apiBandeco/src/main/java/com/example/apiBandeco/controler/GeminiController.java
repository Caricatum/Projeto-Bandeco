package com.example.apiBandeco.controler;

import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.LinkedHashMap;
import java.util.Map;

@RestController
@RequestMapping("/gemini")
@RequiredArgsConstructor
public class GeminiController {

    private final GeminiService geminiService;


    @GetMapping("/test")
    public ResponseEntity<Map<String, Object>> testarGemini(
            @RequestParam(defaultValue = "Olá Gemini, responda confirmando que você está ativo e funcionando!") String prompt) {

        Map<String, Object> resultado = new LinkedHashMap<>();

        try {
            String resposta = geminiService.askGemini(prompt);
            resultado.put("status", "SUCESSO");
            resultado.put("pergunta", prompt);
            resultado.put("resposta", resposta);
            return ResponseEntity.ok(resultado);
        } catch (Exception e) {
            resultado.put("status", "ERRO");
            resultado.put("pergunta", prompt);
            resultado.put("mensagemErro", e.getMessage());
            resultado.put("tipoErro", e.getClass().getSimpleName());
            return ResponseEntity.status(500).body(resultado);
        }
    }
}
