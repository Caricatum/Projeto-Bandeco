package com.example.apiBandeco.controler;

import com.example.apiBandeco.model.Avaliacoes;
import com.example.apiBandeco.model.Pratos;
import com.example.apiBandeco.repository.AvaliacoesRepository;
import com.example.apiBandeco.repository.PratosRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class GeminiAvaliacaoService {

    private final AvaliacoesRepository avaliacoesRepository;
    private final PratosRepository pratosRepository;
    private final GeminiService geminiService;

    @Async
    public void atualizarDescricaoIA(Pratos prato) {

        try {

            List<Avaliacoes> avaliacoes =
                    avaliacoesRepository.findByPratoId(prato.getId());

            // Segurança: só gera com pelo menos 3 avaliações
            if (avaliacoes.size() < 3) {
                return;
            }

            StringBuilder textoAvaliacoes =
                    new StringBuilder();

            for (Avaliacoes avaliacao : avaliacoes) {

                textoAvaliacoes
                        .append("Nota: ")
                        .append(avaliacao.getNota())
                        .append("\n");

                textoAvaliacoes
                        .append("Comentário: ")
                        .append(avaliacao.getAvaliacao())
                        .append("\n\n");
            }

            String prompt =
                    "Você é responsável por resumir avaliações de pratos "
                            + "de um restaurante universitário.\n\n"

                            + "Analise as informações do prato e suas avaliações "
                            + "e produza uma única opinião geral sobre o prato.\n\n"

                            + "REGRAS:\n"
                            + "- Baseie-se exclusivamente nas informações fornecidas.\n"
                            + "- Não invente características do prato.\n"
                            + "- Não mencione usuários individualmente.\n"
                            + "- Não diga quantas pessoas avaliaram.\n"
                            + "- Não use linguagem excessivamente informal.\n"
                            + "- Seja imparcial.\n"
                            + "- Considere tanto elogios quanto críticas recorrentes.\n"
                            + "- Se houver opiniões divergentes, mencione isso de forma equilibrada.\n"
                            + "- Escreva no máximo 3 frases.\n"
                            + "- Retorne somente a opinião geral, sem título.\n\n"

                            + "NOME DO PRATO:\n"
                            + prato.getNome()
                            + "\n\n"

                            + "DESCRIÇÃO DO PRATO:\n"
                            + prato.getDescricao()
                            + "\n\n"

                            + "AVALIAÇÕES:\n"
                            + textoAvaliacoes;

            String resposta =
                    geminiService.askGemini(prompt);

            // Busca novamente para garantir que estamos
            // trabalhando com a entidade atualizada
            Pratos pratoAtualizado =
                    pratosRepository.findById(prato.getId())
                            .orElse(null);

            if (pratoAtualizado != null) {

                pratoAtualizado.setDescricaoIA(resposta);

                pratosRepository.save(pratoAtualizado);
            }

        } catch (Exception e) {

            System.out.println(
                    "Erro ao gerar descrição com IA: "
                            + e.getMessage()
            );

            e.printStackTrace();
        }
    }
}