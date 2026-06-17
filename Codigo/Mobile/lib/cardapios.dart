import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'cadastroPrato.dart';
import 'menuNavegacao.dart';

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
    final url = Uri.parse('http://localhost:8080/pratos/all');

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
        elevation: 0,
        flexibleSpace: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [Color(0xFFD32F2F), Color(0xFFF57C00)],
            ),
          ),
        ),
        title: const Text(
          'Cardápio',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.add, color: Colors.white),
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
      drawer: const MenuNavegacao(),
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
                    decoration: InputDecoration(
                      labelText: 'Buscar prato por ID',
                      labelStyle: const TextStyle(color: Color(0xFFF57C00)),
                      filled: true,
                      fillColor: Colors.white,
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                        borderSide: const BorderSide(color: Color(0xFFF57C00)),
                      ),
                      enabledBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                        borderSide: const BorderSide(color: Color(0xFFF57C00)),
                      ),
                      focusedBorder: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(12),
                        borderSide: const BorderSide(
                          color: Color(0xFFD32F2F),
                          width: 2,
                        ),
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 10),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFFFC107),
                    foregroundColor: Colors.white,
                    padding: const EdgeInsets.all(15),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
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
              color: Colors.white,
              elevation: 5,
              shadowColor: Colors.orange.withOpacity(0.3),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(15),
              ),
              margin: const EdgeInsets.all(12),
              child: ListTile(
                leading: const CircleAvatar(
                  backgroundColor: Color(0xFFF57C00),
                  child: Icon(Icons.restaurant, color: Colors.white),
                ),
                title: Text(
                  pratoEncontrado!['nome'],
                  style: const TextStyle(
                    color: Color(0xFFD32F2F),
                    fontWeight: FontWeight.bold,
                  ),
                ),
                subtitle: Text(
                  pratoEncontrado!['descricao'] ?? 'Sem descrição',
                ),
                trailing: Text(
                  "ID: ${pratoEncontrado!['id']}",
                  style: const TextStyle(
                    color: Color(0xFFF57C00),
                    fontWeight: FontWeight.bold,
                  ),
                ),
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
                      color: Colors.white,
                      elevation: 6,
                      shadowColor: Colors.orange.withOpacity(0.25),
                      margin: const EdgeInsets.symmetric(vertical: 10),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(18),
                        side: const BorderSide(
                          color: Color(0xFFFFE0B2),
                          width: 1,
                        ),
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          ClipRRect(
                            borderRadius: const BorderRadius.vertical(
                              top: Radius.circular(18),
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
                                      return Container(
                                        height: 180,
                                        color: const Color(0xFFFFF3E0),
                                        child: const Center(
                                          child: Icon(
                                            Icons.broken_image,
                                            size: 60,
                                            color: Color(0xFFF57C00),
                                          ),
                                        ),
                                      );
                                    },
                                  )
                                : Container(
                                    height: 180,
                                    color: const Color(0xFFFFF3E0),
                                    child: const Center(
                                      child: Icon(
                                        Icons.restaurant_menu,
                                        size: 70,
                                        color: Color(0xFFF57C00),
                                      ),
                                    ),
                                  ),
                          ),

                          Padding(
                            padding: const EdgeInsets.all(14),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    Expanded(
                                      child: Text(
                                        prato['nome'] ?? 'Sem nome',
                                        style: const TextStyle(
                                          fontSize: 20,
                                          fontWeight: FontWeight.bold,
                                          color: Color(0xFFD32F2F),
                                        ),
                                      ),
                                    ),

                                    if (prato['nota'] != null)
                                      Container(
                                        padding: const EdgeInsets.symmetric(
                                          horizontal: 8,
                                          vertical: 4,
                                        ),
                                        decoration: BoxDecoration(
                                          color: const Color(0xFFFFF8E1),
                                          borderRadius: BorderRadius.circular(
                                            12,
                                          ),
                                        ),
                                        child: Row(
                                          children: [
                                            const Icon(
                                              Icons.star_rounded,
                                              color: Color(0xFFFFC107),
                                              size: 22,
                                            ),
                                            const SizedBox(width: 4),
                                            Text(
                                              prato['nota'].toString(),
                                              style: const TextStyle(
                                                fontWeight: FontWeight.bold,
                                                color: Color(0xFF5D4037),
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                  ],
                                ),

                                const SizedBox(height: 10),

                                Text(
                                  prato['descricao'] ?? 'Sem descrição.',
                                  style: TextStyle(
                                    color: Colors.grey[700],
                                    fontSize: 14,
                                    height: 1.5,
                                  ),
                                ),

                                const SizedBox(height: 12),

                                if (prato['vegano'] == true)
                                  Container(
                                    padding: const EdgeInsets.symmetric(
                                      horizontal: 10,
                                      vertical: 6,
                                    ),
                                    decoration: BoxDecoration(
                                      color: const Color(0xFFFFF3CD),
                                      borderRadius: BorderRadius.circular(20),
                                      border: Border.all(
                                        color: const Color(0xFFFFC107),
                                      ),
                                    ),
                                    child: const Text(
                                      '🌱 Vegano',
                                      style: TextStyle(
                                        color: Color(0xFFF57C00),
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
