import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'cardapios.dart';

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
          'categoriaId': categoriaController.text,
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
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Form(
          key: formKey,
          child: ListView(
            children: [
              TextFormField(
                controller: nomeController,
                decoration: const InputDecoration(labelText: "Nome"),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return "Informe o nome";
                  }
                  return null;
                },
              ),

              TextFormField(
                controller: descricaoController,
                decoration: const InputDecoration(labelText: "Descrição"),
              ),

              TextFormField(
                controller: imagemController,
                decoration: const InputDecoration(labelText: "URL da Imagem"),
              ),

              TextFormField(
                controller: notaController,
                decoration: const InputDecoration(labelText: "Nota Técnica"),
              ),

              TextFormField(
                controller: categoriaController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(labelText: "ID da Categoria"),
              ),

              SwitchListTile(
                title: const Text("Vegano"),
                value: vegano,
                onChanged: (value) {
                  setState(() {
                    vegano = value;
                  });
                },
              ),

              ElevatedButton(
                onPressed: cadastrarPrato,
                child: const Text("Cadastrar"),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
