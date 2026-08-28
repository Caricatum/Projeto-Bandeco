import 'package:flutter/material.dart';

class Esquecisenha extends StatefulWidget {
  const Esquecisenha({super.key});

  @override
  State<Esquecisenha> createState() => _EsquecisenhaState();
}

class _EsquecisenhaState extends State<Esquecisenha> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Esqueci a Senha'),
      ),
      body: const Padding(
        padding: EdgeInsets.all(16.0),
        child: Text('Página de recuperação de senha.'),
      ),
    );
  }
}