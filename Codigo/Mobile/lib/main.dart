import 'package:flutter/material.dart';
<<<<<<< HEAD
import 'login.dart';
=======
import 'package:tcc_flutter/cardapios.dart';
import 'principal.dart';
import 'login.dart';
import 'cadastro.dart';
import 'perfil.dart';
import 'cardapios.dart';
import 'cadastroPrato.dart';
>>>>>>> 2b32ebda870f19b304906a4e8d78f2288ff3cc81


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