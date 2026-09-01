import 'package:flutter/material.dart';
import 'Pages/login.dart';
import 'Pages/CardapioReferent/cardapioPratos.dart';
import 'Pages/principal.dart';
import 'Pages/login.dart';
import 'Pages/cadastro.dart';
import 'Pages/UserReferent/perfil.dart';
import 'Pages/CardapioReferent/cardapioPratos.dart';
import 'Pages/CardapioReferent/cadastroPrato.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
      ),
      home: const Cadastro(),
    );
  }
}