import 'package:flutter/material.dart';
import 'menuNavegacao.dart';

class Cardapio extends StatefulWidget {
  const Cardapio({super.key});

  @override
  State<Cardapio> createState() => _CardapioState();
}

class _CardapioState extends State<Cardapio> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: const Color(0xFFE76F51),
        elevation: 0,
        centerTitle: true,
        title: const Text(
          '🍽️ Cardápio Completo',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
      ),
      drawer: const MenuNavegacao(),
      body: const Center(child: Text('Conteúdo do Cardápio')),
    );
  }
}
