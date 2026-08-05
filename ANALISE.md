# Análise Técnica do Projeto - Sistema Bandeco (COTIL/UNICAMP)

Esta análise avalia a implementação de boas práticas de desenvolvimento, segurança e validação de dados em todas as frentes do projeto (Backend, Web, Mobile e Banco de Dados).

## 1. Validação de Dados

### Backend (Java/Spring Boot)
*   **Bean Validation:** Uso correto de `@NotBlank`, `@NotNull` e `@Valid` nas entidades e controladores. Isso garante que a integridade dos dados seja verificada no momento da entrada.
*   **Tratamento de Exceções:** Implementação de `@RestControllerAdvice` na classe `TratadorErros`, o que permite uma resposta padronizada para erros de validação (HTTP 400), facilitando a integração com o frontend.

### Banco de Dados (MySQL)
*   **Integridade Referencial:** O script SQL define corretamente chaves estrangeiras (`FOREIGN KEY`), garantindo que não existam registros órfãos.
*   **Restrições de Unicidade:** Uso de `UNIQUE KEY` para campos críticos como `login` na tabela `user`, prevenindo duplicidade.
*   **Tipagem:** Uso adequado de tipos como `BIT(1)` para booleanos e `DATETIME` para registros temporais.

### Frontends (Web e Mobile)
*   **Ponto de Melhoria:** A validação é predominantemente delegada ao servidor. Recomenda-se implementar validações preventivas no cliente (JavaScript no Web e `validator` no Flutter) para reduzir o tráfego desnecessário e melhorar o tempo de resposta percebido pelo usuário.

## 2. Segurança

*   **Criptografia de Senhas:** O uso de `PasswordEncoder` no `UserControler` demonstra maturidade em segurança, evitando o armazenamento de senhas em texto puro.
*   **Cross-Origin Resource Sharing (CORS):** A anotação `@CrossOrigin` está presente, permitindo a comunicação entre o frontend e a API de forma controlada.

## 3. Arquitetura e Boas Práticas

### Pontos Positivos
*   **Padrão MVC:** Clara separação entre Modelos, Repositórios e Controladores.
*   **Organização de Arquivos:** Frontends bem estruturados com separação de responsabilidades (CSS, JS, Assets).

### Oportunidades de Melhoria
1.  **Camada de DTO (Data Transfer Objects):** Atualmente, as entidades de banco de dados são expostas diretamente nos controladores. Recomenda-se o uso de DTOs para desacoplar o contrato da API do modelo de dados.
2.  **Convenções de Código (Java):**
    *   Substituir *snake_case* (`senha_hash`) por *camelCase* (`senhaHash`) para seguir o padrão idiomático da linguagem.
    *   Corrigir o erro de digitação no nome do pacote: de `controler` para `controller`.
3.  **Refatoração Mobile:** Migrar de `TextField` para `TextFormField` no Flutter para aproveitar as ferramentas nativas de validação de formulários.
4.  **Feedback de Erro:** Implementar avisos visuais no frontend (ex: Toasts ou Modais) para exibir as mensagens de erro retornadas pela API.

---
*Análise gerada em 6 de maio de 2026.*
