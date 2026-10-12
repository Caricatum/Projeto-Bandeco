package com.example.apiBandeco.config;

import com.google.genai.Client;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class GeminiConfig {
    //AO COMMITAR, REMOVA A CHAVE!!!
    // Substitua pelo valor da sua chave da API do Gemini
<<<<<<< HEAD
    private static final String GEMINI_API_KEY = "***REMOVIDO***";
=======
    private static final String GEMINI_API_KEY = "";
>>>>>>> origin/main

    @Bean
    public Client geminiClient() {
        return Client.builder()
                .apiKey(GEMINI_API_KEY)
                .build();
    }
}
