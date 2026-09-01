// =============================================
// CONFIGURAÇÃO CENTRALIZADA DA API E AMBIENTE
// =============================================
const API_BASE_URL = 'http://localhost:8080';

/**
 * Função helper para realizar requisições HTTP seguras com tratamento de erro padronizado.
 * Diagnostica automaticamente se a API estiver inacessível (ERR_CONNECTION_REFUSED / offline).
 */
async function fetchAPI(endpoint, options = {}) {
    const url = endpoint.startsWith('http') ? endpoint : `${API_BASE_URL}${endpoint}`;
    try {
        const response = await fetch(url, options);
        return response;
    } catch (error) {
        console.error(`[API Error] Falha na conexão com ${url}:`, error);
        throw new Error(`Servidor inacessível (porta 8080). Verifique se a API Spring Boot está iniciada.`);
    }
}
