import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'cadastroPrato.dart';

//Aviso, o código abaixo está como placeholder, mostrando apenas os pratos cadastrados e não os cardapios inteiros,
// prncipalmente porque não deu tempo de fazer o resto das classes pra fazer o cardapio completo.

class Cardapios extends StatefulWidget {
  const Cardapios({super.key});

  @override
  State<Cardapios> createState() => _CardapiosState();
}

class _CardapiosState extends State<Cardapios> {
  //Mostra tudo
  Future<List<dynamic>> listarPratos() async {
    final url = Uri.parse('http://localhost:8080/pratos');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      } else {
        throw Exception('Falha ao carregar os pratos');
      }
    } catch (e) {
      throw Exception('Erro de conexão: $e');
    }
  }

  // Busca por ID
  Future<Map<String, dynamic>> buscarPratoId(int id) async {
    Uri url = Uri.parse('http://localhost:8080/pratos/id/$id');

    try {
      http.Response response = await http.get(url);

      if (response.statusCode == 200) {
        return jsonDecode(response.body);
      } else {
        throw Exception('Prato não encontrado');
      }
    } catch (e) {
      throw Exception('Erro de conexão: $e');
    }
  }

  TextEditingController idController = TextEditingController();

  Map<String, dynamic>? pratoEncontrado;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Cardápio Completo'),
        actions: [
          // Botão para a tela de cadastro
          IconButton(
            icon: const Icon(Icons.add),
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const Cadastroprato()),
              ).then((_) {
                setState(() {});
              });
            },
          ),
        ],
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(10),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: idController,
                    keyboardType: TextInputType.number,
                    decoration: const InputDecoration(
                      labelText: 'Buscar prato por ID',
                      border: OutlineInputBorder(),
                    ),
                  ),
                ),
                const SizedBox(width: 10),
                ElevatedButton(
                  onPressed: () async {
                    try {
                      Map<String, dynamic> resultado = await buscarPratoId(
                        int.parse(idController.text),
                      );

                      setState(() {
                        pratoEncontrado = resultado;
                      });
                    } catch (e) {
                      ScaffoldMessenger.of(
                        context,
                      ).showSnackBar(SnackBar(content: Text(e.toString())));
                    }
                  },
                  child: const Icon(Icons.search),
                ),
              ],
            ),
          ),

          if (pratoEncontrado != null)
            Card(
              margin: const EdgeInsets.all(12),
              child: ListTile(
                title: Text(pratoEncontrado!['nome']),
                subtitle: Text(
                  pratoEncontrado!['descricao'] ?? 'Sem descrição',
                ),
                trailing: Text("ID: ${pratoEncontrado!['id']}"),
              ),
            ),

          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: listarPratos(),
              builder: (context, snapshot) {
                // Enquanto carrega os dados da API
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(child: CircularProgressIndicator());
                }

                // Caso de erro na requisição
                if (snapshot.hasError) {
                  return Center(
                    child: Text(
                      'Erro: ${snapshot.error}',
                      style: const TextStyle(color: Colors.red),
                    ),
                  );
                }

                // Lista vazia
                if (!snapshot.hasData || snapshot.data!.isEmpty) {
                  return const Center(
                    child: Text('Nenhum prato cadastrado no momento.'),
                  );
                }

                final pratos = snapshot.data!;

                // Lista normal
                return ListView.builder(
                  padding: const EdgeInsets.all(12),
                  itemCount: pratos.length,
                  itemBuilder: (context, index) {
                    final prato = pratos[index];

                    return Card(
                      elevation: 4,
                      margin: const EdgeInsets.symmetric(vertical: 8),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          // Exibição da Imagem (Trata se a URL for inválida ou vazia)
                          ClipRRect(
                            borderRadius: const BorderRadius.vertical(
                              top: Radius.circular(12),
                            ),
                            child:
                                prato['imagem'] != null &&
                                    prato['imagem'].toString().isNotEmpty
                                ? Image.network(
                                    prato['imagem'],
                                    height: 180,
                                    width: double.infinity,
                                    fit: BoxFit.cover,
                                    errorBuilder: (context, error, stackTrace) {
                                      return const SizedBox(
                                        height: 180,
                                        child: Center(
                                          child: Icon(
                                            Icons.broken_image,
                                            size: 50,
                                          ),
                                        ),
                                      );
                                    },
                                  )
                                : const SizedBox(
                                    height: 150,
                                    child: Center(
                                      child: Icon(Icons.restaurant, size: 50),
                                    ),
                                  ),
                          ),
                          Padding(
                            padding: const EdgeInsets.all(12),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    // Nome do Prato
                                    Expanded(
                                      child: Text(
                                        prato['nome'] ?? 'Sem nome',
                                        style: const TextStyle(
                                          fontSize: 18,
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                    // Nota do Prato
                                    if (prato['nota'] != null)
                                      Row(
                                        children: [
                                          const Icon(
                                            Icons.star,
                                            color: Colors.amber,
                                            size: 20,
                                          ),
                                          const SizedBox(width: 4),
                                          Text(
                                            prato['nota'].toString(),
                                            style: const TextStyle(
                                              fontWeight: FontWeight.bold,
                                            ),
                                          ),
                                        ],
                                      ),
                                  ],
                                ),
                                const SizedBox(height: 8),
                                // Descrição
                                Text(
                                  prato['descricao'] ?? 'Sem descrição.',
                                  style: TextStyle(color: Colors.grey[700]),
                                ),
                                const SizedBox(height: 8),
                                // Selo para Vegano
                                if (prato['vegano'] == true)
                                  Container(
                                    padding: const EdgeInsets.symmetric(
                                      horizontal: 8,
                                      vertical: 4,
                                    ),
                                    decoration: BoxDecoration(
                                      color: Colors.green[100],
                                      borderRadius: BorderRadius.circular(8),
                                    ),
                                    child: Text(
                                      'Vegano',
                                      style: TextStyle(
                                        color: Colors.green[800],
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    );
                  },
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
