import 'package:flutter/material.dart';
import 'package:tcc_flutter/cardapios.dart';
import 'principal.dart';
import 'login.dart';
import 'cadastro.dart';
import 'perfil.dart';
import 'cardapios.dart';
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
        colorScheme: .fromSeed(seedColor: Colors.deepPurple),
      ),
<<<<<<< HEAD
      home: Principal(),
=======
      home: const Principal(),
>>>>>>> 994247aba882dc8ba4aa4ec73b6918298ff6d26e
    );
  }
}