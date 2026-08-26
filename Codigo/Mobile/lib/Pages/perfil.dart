import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../Class/usuarioClass.dart';
import 'editarusuario.dart';

class Perfil extends StatefulWidget {
  final Usuario usuario;

  const Perfil({super.key, required this.usuario});

  @override
  State<Perfil> createState() => _PerfilState();
}

class _PerfilState extends State<Perfil> {
  late Usuario usuario;

  @override
  void initState() {
    super.initState();

    usuario = widget.usuario;
  }

  Future<void> editarUsuario() async {
    final resultado = await Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => EditarUsuario(usuario: usuario)),
    );

    if (resultado != null && resultado is Usuario) {
      setState(() {
        usuario = resultado;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(16.0),

        child: Column(
          children: [
            Column(
              children: [
                const SizedBox(height: 10),

                Text(
                  usuario.nome,
                  style: const TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ],
            ),

            const SizedBox(height: 30),

            Card(
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16),
              ),
              elevation: 4,

              child: Padding(
                padding: const EdgeInsets.all(20),

                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,

                  children: [
                    const Text(
                      "Saldo disponível",
                      style: TextStyle(fontSize: 16),
                    ),

                    const Text(
                      "R\$ (100,00)",
                      style: TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                        color: Colors.green,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 20),

            Expanded(
              child: ListView(
                children: [
                  ListTile(
                    leading: const Icon(Icons.restaurant),
                    title: const Text("Histórico de refeições"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {},
                  ),

                  ListTile(
                    leading: const Icon(Icons.attach_money),
                    title: const Text("Recarregar saldo"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {},
                  ),

                  ListTile(
                    leading: const Icon(Icons.settings),
                    title: const Text("Configurações"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: editarUsuario,
                  ),

                  ListTile(
                    leading: const Icon(Icons.logout),
                    title: const Text("Sair"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {},
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
