import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'cardapios.dart';
import 'menuNavegacao.dart';

class Cadastroprato extends StatefulWidget {
  const Cadastroprato({super.key});

  @override
  State<Cadastroprato> createState() => _CadastropratoState();
}

class _CadastropratoState extends State<Cadastroprato> {
  Future<void> cadastrarPrato() async {
    final url = Uri.parse('http://localhost:8080/pratos/cadastrar');

    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nome': nomeController.text,
          'imagem': imagemController.text,
          'descricao': descricaoController.text,
          'nota': notaController.text,
          'vegano': vegano,
          'categoria': {'id': int.tryParse(categoriaController.text)},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Prato cadastrado com sucesso!')),
        );

        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (_) => Cardapios()),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao cadastrar: ${response.body}')),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    }
  }

  TextEditingController nomeController = TextEditingController();
  TextEditingController descricaoController = TextEditingController();
  TextEditingController imagemController = TextEditingController();
  TextEditingController notaController = TextEditingController();
  TextEditingController categoriaController = TextEditingController();
  bool vegano = false;
  final formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          "Cadastro de Prato",
          style: TextStyle(fontWeight: FontWeight.bold, color: Colors.white),
        ),
        centerTitle: true,
        backgroundColor: Colors.red.shade700,
        elevation: 0,
      ),
      drawer: const MenuNavegacao(),
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              Colors.red.shade700,
              Colors.orange.shade500,
              Colors.amber.shade300,
            ],
          ),
        ),

        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Form(
            key: formKey,
            child: ListView(
              children: [
                const SizedBox(height: 20),

                const Icon(
                  Icons.restaurant_menu,
                  size: 80,
                  color: Colors.white,
                ),

                const SizedBox(height: 10),

                const Text(
                  "Novo Prato",
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 28,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),

                const SizedBox(height: 25),

                Card(
                  elevation: 10,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.all(20),
                    child: Column(
                      children: [
                        TextFormField(
                          controller: nomeController,
                          decoration: InputDecoration(
                            labelText: "Nome",
                            prefixIcon: const Icon(Icons.fastfood),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return "Informe o nome";
                            }
                            return null;
                          },
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: descricaoController,
                          maxLines: 3,
                          decoration: InputDecoration(
                            labelText: "Descrição",
                            prefixIcon: const Icon(Icons.description),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: imagemController,
                          decoration: InputDecoration(
                            labelText: "URL da Imagem",
                            prefixIcon: const Icon(Icons.image),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: notaController,
                          decoration: InputDecoration(
                            labelText: "Nota Técnica",
                            prefixIcon: const Icon(Icons.star),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: categoriaController,
                          keyboardType: TextInputType.number,
                          decoration: InputDecoration(
                            labelText: "ID da Categoria",
                            prefixIcon: const Icon(Icons.category),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        Container(
                          decoration: BoxDecoration(
                            color: Colors.green.shade50,
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: SwitchListTile(
                            title: const Text(
                              "Prato Vegano",
                              style: TextStyle(fontWeight: FontWeight.bold),
                            ),
                            secondary: const Icon(
                              Icons.eco,
                              color: Colors.green,
                            ),
                            value: vegano,
                            activeColor: Colors.green,
                            onChanged: (value) {
                              setState(() {
                                vegano = value;
                              });
                            },
                          ),
                        ),

                        const SizedBox(height: 25),

                        SizedBox(
                          width: double.infinity,
                          height: 55,
                          child: ElevatedButton(
                            onPressed: () {
                              if (formKey.currentState!.validate()) {
                                cadastrarPrato();
                              }
                            },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.red.shade700,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(15),
                              ),
                            ),
                            child: const Text(
                              "Cadastrar Prato",
                              style: TextStyle(
                                fontSize: 18,
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),

                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
