import 'package:flutter/material.dart';
import 'login.dart';
import 'package:tcc_flutter/cardapioPratos.dart';
import 'principal.dart';
import 'login.dart';
import 'cadastro.dart';
import 'perfil.dart';
import 'cardapioPratos.dart';
import 'cadastroPrato.dart';



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
      home: const Principal(),
    );
  }
}