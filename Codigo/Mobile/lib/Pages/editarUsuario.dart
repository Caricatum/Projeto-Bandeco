import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../Class/usuarioClass.dart';

class EditarUsuario extends StatefulWidget {
  final Usuario usuario;

  const EditarUsuario({super.key, required this.usuario});

  @override
  State<EditarUsuario> createState() => _EditarUsuarioState();
}

class _EditarUsuarioState extends State<EditarUsuario> {
  late TextEditingController nomeController;
  late TextEditingController loginController;

  bool carregando = false;

  @override
  void initState() {
    super.initState();

    nomeController = TextEditingController(text: widget.usuario.nome);

    loginController = TextEditingController(text: widget.usuario.login);
  }

  @override
  void dispose() {
    nomeController.dispose();
    loginController.dispose();

    super.dispose();
  }

  Future<void> salvar() async {
    setState(() {
      carregando = true;
    });

    try {
      final response = await http.put(
        Uri.parse('http://localhost:8080/user/atualizar'),

        headers: {'Content-Type': 'application/json'},

        body: jsonEncode({
          'id': widget.usuario.id,

          'nome': nomeController.text,

          'login': loginController.text,
        }),
      );

      if (response.statusCode == 200) {
        final dados = jsonDecode(response.body);

        Usuario usuarioAtualizado = Usuario.fromJson(dados);

        if (!mounted) return;

        Navigator.pop(context, usuarioAtualizado);
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao atualizar: ${response.statusCode}')),
        );
      }
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Editar usuário')),

      body: Padding(
        padding: const EdgeInsets.all(16),

        child: Column(
          children: [
            TextField(
              controller: nomeController,
              decoration: const InputDecoration(labelText: 'Nome'),
            ),

            const SizedBox(height: 16),

            TextField(
              controller: loginController,
              decoration: const InputDecoration(labelText: 'E-mail'),
            ),

            const SizedBox(height: 30),

            ElevatedButton(
              onPressed: carregando ? null : salvar,

              child: carregando
                  ? const CircularProgressIndicator()
                  : const Text('Salvar'),
            ),
          ],
        ),
      ),
    );
  }
}
