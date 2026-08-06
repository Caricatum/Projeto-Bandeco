import 'package:flutter/material.dart';
import 'menuNavegacao.dart';

class Cadastracardapio extends StatefulWidget {
  const Cadastracardapio({super.key});

  @override
  State<Cadastracardapio> createState() => _CadastracardapioState();
}

class _CadastracardapioState extends State<Cadastracardapio> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color.fromARGB(255, 255, 255, 255),
      appBar: AppBar(
        backgroundColor: const Color.fromARGB(255, 211, 47, 47),
        elevation: 0,
        centerTitle: true,
        title: const Text(
          '🍽️ Criar Cardápio',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
      ),
      
      drawer: const MenuNavegacao(),

    );
  }
}