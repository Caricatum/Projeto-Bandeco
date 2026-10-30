import 'package:flutter/material.dart';
import 'Pages/cadastro.dart';
import 'Services/notificacaoService.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await NotificacaoService.instance.inicializar();
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