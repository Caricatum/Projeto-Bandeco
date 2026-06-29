import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:tcc_flutter/atualizaPrato.dart';
import 'package:tcc_flutter/categoria.dart';
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

  //Deletar prato
  Future<void> deletarPrato(int id) async {
    final response = await http.delete(
      Uri.parse("http://localhost:8080/pratos/deletar/$id"),
    );

    if (response.statusCode != 200) {
      throw Exception("Erro ao excluir prato.");
    }
  }

  //Corfirma pra deletar o prato
  Future<void> confirmarExclusao(int id) async {
    final bool? confirmar = await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Excluir prato"),
          content: const Text("Tem certeza que deseja excluir este prato?"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(context, false);
              },
              child: const Text("Cancelar"),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
              onPressed: () {
                Navigator.pop(context, true);
              },
              child: const Text("Excluir"),
            ),
          ],
        );
      },
    );

    if (confirmar == true) {
      try {
        await deletarPrato(id);

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Prato removido com sucesso!")),
        );

        setState(() {});
      } catch (e) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text(e.toString())));
      }
    }
  }

  TextEditingController idController = TextEditingController();

  Map<String, dynamic>? pratoEncontrado;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFFFF8F1),
      appBar: AppBar(
        backgroundColor: const Color(0xFFE76F51),
        elevation: 0,
        centerTitle: true,
        title: const Text(
          '🍽️ Cardápio Completo',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.add_circle_outline, color: Colors.white),
            tooltip: "Cadastrar prato",
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
          Container(
            margin: const EdgeInsets.all(16),
            padding: const EdgeInsets.all(16),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(18),
              boxShadow: [
                BoxShadow(
                  color: Colors.orange.withOpacity(0.15),
                  blurRadius: 12,
                  offset: const Offset(0, 5),
                ),
              ],
            ),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: idController,
                    keyboardType: TextInputType.number,
                    decoration: InputDecoration(
                      filled: true,
                      fillColor: const Color(0xFFFFF3E6),
                      prefixIcon: const Icon(
                        Icons.search,
                        color: Color(0xFFE76F51),
                      ),
                      labelText: "Buscar prato por ID",
                      labelStyle: const TextStyle(color: Color(0xFFE76F51)),
                      border: OutlineInputBorder(
                        borderRadius: BorderRadius.circular(15),
                        borderSide: BorderSide.none,
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 12),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFE76F51),
                    foregroundColor: Colors.white,
                    padding: const EdgeInsets.all(16),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(14),
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
                      ScaffoldMessenger.of(context).showSnackBar(
                        SnackBar(
                          backgroundColor: Colors.red,
                          content: Text(e.toString()),
                        ),
                      );
                    }
                  },
                  child: const Icon(Icons.search),
                ),
              ],
            ),
          ),

          if (pratoEncontrado != null)
            Container(
              margin: const EdgeInsets.symmetric(horizontal: 16),
              child: Card(
                color: const Color(0xFFFFE8D6),
                elevation: 4,
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(18),
                ),
                child: ListTile(
                  leading: const CircleAvatar(
                    backgroundColor: Color(0xFFE76F51),
                    child: Icon(Icons.restaurant_menu, color: Colors.white),
                  ),
                  title: Text(
                    pratoEncontrado!['nome'],
                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                  subtitle: Column(
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(pratoEncontrado!['descricao'] ?? "Sem descrição"),
                      const SizedBox(height: 15),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          // Botão Editar
                          OutlinedButton.icon(
                            icon: const Icon(Icons.edit),
                            label: const Text("Editar"),
                            onPressed: () async {
                              await Navigator.push(
                                context,
                                MaterialPageRoute(
                                  builder: (_) => const Cadastroprato(),
                                ),
                              );

                              setState(() {});
                            },
                          ),

                          const SizedBox(width: 10),

                          // Botão Excluir
                          ElevatedButton.icon(
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.red,
                              foregroundColor: Colors.white,
                            ),
                            icon: const Icon(Icons.delete),
                            label: const Text("Excluir"),
                            onPressed: () async {
                              bool? confirmar = await showDialog(
                                context: context,
                                builder: (_) => AlertDialog(
                                  title: const Text("Excluir prato"),
                                  content: Text(
                                    "Deseja realmente excluir '${pratoEncontrado!['nome']}'?",
                                  ),
                                  actions: [
                                    TextButton(
                                      onPressed: () {
                                        Navigator.pop(context, false);
                                      },
                                      child: const Text("Cancelar"),
                                    ),

                                    ElevatedButton(
                                      style: ElevatedButton.styleFrom(
                                        backgroundColor: Colors.red,
                                      ),
                                      onPressed: () {
                                        Navigator.pop(context, true);
                                      },
                                      child: const Text("Excluir"),
                                    ),
                                  ],
                                ),
                              );

                              if (confirmar == true) {
                                try {
                                  await deletarPrato(pratoEncontrado!['id']);

                                  ScaffoldMessenger.of(context).showSnackBar(
                                    const SnackBar(
                                      content: Text(
                                        "Prato excluído com sucesso.",
                                      ),
                                    ),
                                  );

                                  setState(() {});
                                } catch (e) {
                                  ScaffoldMessenger.of(context).showSnackBar(
                                    SnackBar(content: Text(e.toString())),
                                  );
                                }
                              }
                            },
                          ),
                        ],
                      ),
                    ],
                  ),

                  trailing: Chip(
                    backgroundColor: const Color(0xFFE76F51),
                    label: Text(
                      "ID ${pratoEncontrado!['id']}",
                      style: const TextStyle(color: Colors.white),
                    ),
                  ),
                ),
              ),
            ),

          Expanded(
            child: FutureBuilder<List<dynamic>>(
              future: listarPratos(),
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.waiting) {
                  return const Center(
                    child: CircularProgressIndicator(color: Color(0xFFE76F51)),
                  );
                }

                if (snapshot.hasError) {
                  return Center(
                    child: Text(
                      "Erro: ${snapshot.error}",
                      style: const TextStyle(color: Colors.red),
                    ),
                  );
                }

                if (!snapshot.hasData || snapshot.data!.isEmpty) {
                  return const Center(
                    child: Text(
                      "Nenhum prato cadastrado.",
                      style: TextStyle(fontSize: 18),
                    ),
                  );
                }

                final pratos = snapshot.data!;

                return ListView.builder(
                  padding: const EdgeInsets.all(16),
                  itemCount: pratos.length,
                  itemBuilder: (context, index) {
                    final prato = pratos[index];

                    return Card(
                      elevation: 8,
                      color: Colors.white,
                      margin: const EdgeInsets.only(bottom: 18),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(22),
                      ),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Stack(
                            children: [
                              ClipRRect(
                                borderRadius: const BorderRadius.vertical(
                                  top: Radius.circular(22),
                                ),
                                child:
                                    prato['imagem'] != null &&
                                        prato['imagem'].toString().isNotEmpty
                                    ? Image.network(
                                        prato['imagem'],
                                        height: 220,
                                        width: double.infinity,
                                        fit: BoxFit.cover,
                                        errorBuilder:
                                            (context, error, stackTrace) {
                                              return Container(
                                                height: 220,
                                                color: Colors.orange.shade100,
                                                child: const Center(
                                                  child: Icon(
                                                    Icons.restaurant,
                                                    size: 70,
                                                    color: Colors.orange,
                                                  ),
                                                ),
                                              );
                                            },
                                      )
                                    : Container(
                                        height: 220,
                                        color: Colors.orange.shade100,
                                        child: const Center(
                                          child: Icon(
                                            Icons.restaurant,
                                            size: 70,
                                            color: Colors.orange,
                                          ),
                                        ),
                                      ),
                              ),

                              if (prato['nota'] != null)
                                Positioned(
                                  top: 12,
                                  right: 12,
                                  child: Container(
                                    padding: const EdgeInsets.symmetric(
                                      horizontal: 10,
                                      vertical: 6,
                                    ),
                                    decoration: BoxDecoration(
                                      color: Colors.white,
                                      borderRadius: BorderRadius.circular(20),
                                    ),
                                    child: Row(
                                      children: [
                                        const Icon(
                                          Icons.star,
                                          color: Colors.amber,
                                          size: 18,
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
                                  ),
                                ),
                            ],
                          ),

                          Padding(
                            padding: const EdgeInsets.all(18),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  prato['nome'] ?? "Sem nome",
                                  style: const TextStyle(
                                    fontSize: 22,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xFF9C3D2A),
                                  ),
                                ),
                                const SizedBox(height: 10),

                                Text(
                                  prato['descricao'] ?? "Sem descrição.",
                                  style: TextStyle(
                                    color: Colors.grey.shade700,
                                    height: 1.5,
                                  ),
                                ),

                                const SizedBox(height: 16),

                                Row(
                                  children: [
                                    if (prato['vegano'] == true)
                                      Container(
                                        padding: const EdgeInsets.symmetric(
                                          horizontal: 12,
                                          vertical: 7,
                                        ),
                                        decoration: BoxDecoration(
                                          color: Colors.green.shade100,
                                          borderRadius: BorderRadius.circular(
                                            20,
                                          ),
                                        ),
                                        child: Row(
                                          children: const [
                                            Icon(
                                              Icons.eco,
                                              color: Colors.green,
                                              size: 18,
                                            ),
                                            SizedBox(width: 5),
                                            Text(
                                              "Vegano",
                                              style: TextStyle(
                                                color: Colors.green,
                                                fontWeight: FontWeight.bold,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),

                                    const Spacer(),

                                    Container(
                                      padding: const EdgeInsets.symmetric(
                                        horizontal: 12,
                                        vertical: 7,
                                      ),
                                      decoration: BoxDecoration(
                                        color: const Color(0xFFFFE8D6),
                                        borderRadius: BorderRadius.circular(20),
                                      ),
                                      child: Text(
                                        "ID ${prato['id']}",
                                        style: const TextStyle(
                                          color: Color(0xFFE76F51),
                                          fontWeight: FontWeight.bold,
                                        ),
                                      ),
                                    ),
                                  ],
                                ),

                                const SizedBox(height: 16),

                                Row(
                                  mainAxisAlignment: MainAxisAlignment.end,
                                  children: [
                                    OutlinedButton.icon(
                                      icon: const Icon(Icons.edit),
                                      label: const Text("Editar"),
                                      onPressed: () async {
                                        await Navigator.push(
                                          context,
                                          MaterialPageRoute(
                                            builder: (_) =>
                                                AtualizaPrato(prato: prato),
                                          ),
                                        );

                                        setState(() {});
                                      },
                                    ),

                                    const SizedBox(width: 10),

                                    ElevatedButton.icon(
                                      style: ElevatedButton.styleFrom(
                                        backgroundColor: Colors.red,
                                        foregroundColor: Colors.white,
                                      ),
                                      icon: const Icon(Icons.delete),
                                      label: const Text("Excluir"),
                                      onPressed: () async {
                                        await confirmarExclusao(prato['id']);
                                      },
                                    ),
                                  ],
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
