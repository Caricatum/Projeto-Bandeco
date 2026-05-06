# Projeto Bandeco - Sistema de Gestão de Restaurante Universitário

Este projeto é uma solução multiplataforma desenvolvida para o gerenciamento do restaurante universitário ("Bandeco") do **COTIL/UNICAMP**. O sistema visa facilitar a consulta de cardápios, avaliações de pratos e a comunicação com os usuários.

## 🚀 Tecnologias Utilizadas

O ecossistema do projeto é dividido em quatro frentes principais:

*   **Backend (API):** Java 17 com Spring Boot, Spring Data JPA e Spring Security.
*   **Mobile:** Flutter (Dart) para Android e iOS.
*   **Web:** Interface responsiva com HTML5, CSS3, JavaScript Vanilla e Bootstrap 5.
*   **Desktop:** Aplicação em C# (.NET).
*   **Banco de Dados:** MySQL.

## 📂 Estrutura do Repositório

```text
├── api/                # Servidor Backend (Spring Boot)
├── Codigo/
│   ├── Mobile/         # Aplicativo Flutter
│   ├── Web/            # Interface Web (HTML/JS/CSS)
│   └── Desktop/        # Aplicação C#
├── banco/              # Scripts SQL e Diagramas do Banco de Dados
└── README.md           # Documentação principal
```

## 🛠️ Como Executar o Projeto

### Pré-requisitos
*   Java JDK 17
*   Maven
*   Flutter SDK
*   MySQL Server
*   .NET SDK (para o Desktop)

### 1. Banco de Dados
Importe o script localizado em `banco/RU_cotil.sql` no seu servidor MySQL para criar a estrutura de tabelas necessária.

### 2. Backend (API)
1. Navegue até a pasta da API:
   ```bash
   cd api/apiBandeco
   ```
2. Configure as credenciais do banco em `src/main/resources/application.properties`.
3. Execute o comando:
   ```bash
   ./mvnw spring-boot:run
   ```

### 3. Frontend Web
Basta abrir o arquivo `Codigo/Web/Paginas/login.html` em qualquer navegador moderno. Para evitar problemas de CORS ao realizar requisições à API local, recomenda-se o uso de um servidor local (como a extensão Live Server do VS Code).

### 4. Aplicativo Mobile
1. Navegue até a pasta mobile:
   ```bash
   cd Codigo/Mobile
   ```
2. Instale as dependências e execute:
   ```bash
   flutter pub get
   flutter run
   ```

### 5. Desktop
1. Navegue até a pasta desktop:
   ```bash
   cd Codigo/Desktop/MeuAppCSharp
   ```
2. Execute a aplicação:
   ```bash
   dotnet run
   ```

## 📝 Funcionalidades
*   **Gestão de Cardápios:** Visualização e cadastro de pratos (almoço/jantar, padrão/vegano).
*   **Avaliações:** Feedback dos usuários sobre as refeições.
*   **Notificações:** Alertas sobre avisos importantes do restaurante.
*   **Perfil:** Controle de acesso para alunos e funcionários.

---
*Desenvolvido para o projeto de PI/TCC do COTIL - UNICAMP.*
