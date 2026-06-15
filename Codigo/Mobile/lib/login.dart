import 'dart:convert';

import 'package:flutter/material.dart';
import 'cadastro.dart';
import 'package:http/http.dart' as http;
import 'principal.dart';

class Login extends StatefulWidget {
  const Login({super.key});

  @override
  State<Login> createState() => _LoginState();
}

TextEditingController loginController = TextEditingController();
TextEditingController senhaController = TextEditingController();

class _LoginState extends State<Login> {
  final TextEditingController loginController = TextEditingController();
  final TextEditingController senhaController = TextEditingController();

  Future<void> fazerLogin(BuildContext context) async {
    final url = Uri.parse('http://localhost:8080/user/login' + loginController.text + "&senhaHash=" + senhaController.text,
    );
    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        final usuario = jsonDecode(response.body);

        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Bem-vindo ${usuario["nome"]}')));

        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Principal()),
        );
      } else {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Login ou senha inválidos')));
      }
    } catch (e) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          children: [
            SizedBox(
              width: double.infinity,
              height: 250,
              child: Image.asset(
                "assets/images/cotilEntrada.jpeg",
                fit: BoxFit.cover,
              ),
            ),
            Container(
              width: double.infinity,
              padding: const EdgeInsets.all(24),
              color: Colors.orangeAccent,
            ),
            Column(
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                const Text(
                  "Login",
                  style: TextStyle(fontSize: 36, fontWeight: FontWeight.bold),
                ),

                const SizedBox(height: 10),

                const Text(
                  "Entre ou cadastre-se agora!",
                  textAlign: TextAlign.center,
                  style: TextStyle(fontSize: 18),
                ),
                    TextFormField(
                      controller: loginController,
                      decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.person),
                        hintText: "Email",
                        filled: true,
                        fillColor: Colors.grey[200],
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(30),
                          borderSide: BorderSide.none,
                        ),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Preencha o campo email";
                        } else {
                          return null;
                        }
                      },
                    ),

                    const SizedBox(height: 30),

                    TextFormField(
                      controller: senhaController,
                      obscureText: true,
                      decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.lock),
                        hintText: "Senha",
                        filled: true,
                        fillColor: Colors.grey[200],
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(30),
                          borderSide: BorderSide.none,
                        ),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Preencha o campo senha";
                        } else {
                          return null;
                        }
                      },
                    ),

                    const SizedBox(height: 30),

                    ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 50,
                          vertical: 15,
                        ),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(30),
                        ),
                      ),
                      onPressed: () {
                        fazerLogin(context);
                      },
                      child: const Text("Entrar"),
                    ),

                    const SizedBox(height: 20),

                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        const Text("Não tem uma conta? "),
                        GestureDetector(
                          onTap: () {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) => Cadastro(),
                              ),
                            );
                          },
                          child: const Text(
                            "Crie a sua!",
                            style: TextStyle(
                              color: Colors.blue,
                              decoration: TextDecoration.underline,
                            ),
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ],
            ),
        ),
      );
  }
}
