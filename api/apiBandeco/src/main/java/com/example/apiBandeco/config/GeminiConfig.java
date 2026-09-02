package com.example.apiBandeco.config;

import com.google.genai.Client;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class GeminiConfig {

    // Substitua pelo valor da sua chave da API do Gemini
    private static final String GEMINI_API_KEY = "AQ.Ab8RN6Ko5ZrkNqaq1tQjEdDAshuq6lfStBsQWMuuEOXAYRr4Sw";

    @Bean
    public Client geminiClient() {
        return Client.builder()
                .apiKey(GEMINI_API_KEY)
                .build();
    }
}
