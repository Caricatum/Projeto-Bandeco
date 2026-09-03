import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:tcc_flutter/Class/usuarioClass.dart';
import 'package:tcc_flutter/Pages/UserReferent/confirmarEmail.dart';
import 'package:tcc_flutter/Pages/UserReferent/esqueciSenha.dart';
import 'cadastro.dart';
import 'package:http/http.dart' as http;
import 'principal.dart';

class Login extends StatefulWidget {
  const Login({super.key});

  @override
  State<Login> createState() => _LoginState();
}

class _LoginState extends State<Login> {
  final TextEditingController loginController = TextEditingController();
  final TextEditingController senhaHashController = TextEditingController();
  bool _obscurePassword = true;

  final _formKey = GlobalKey<FormState>();

  Future<void> fazerLogin(BuildContext context) async {
    final login = loginController.text.trim();
    final senha = senhaHashController.text;

    if (login.isEmpty || senha.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Preencha o e-mail e a senha')),
      );
      return;
    }

    final url = Uri.parse(
      'http://localhost:8080/user/validar'
      '?login=${Uri.encodeComponent(login)}'
      '&senhaHash=${Uri.encodeComponent(senha)}',
    );

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final usuarioUrl = Uri.parse(
          'http://localhost:8080/user/login/'
          '${Uri.encodeComponent(login)}',
        );

        final usuarioResponse = await http.get(usuarioUrl);

        if (usuarioResponse.statusCode == 200) {
          if (!context.mounted) return;

          final usuario = Usuario.fromJson(jsonDecode(usuarioResponse.body));

          Navigator.pushReplacement(
            context,
            MaterialPageRoute(builder: (_) => Principal(usuario: usuario)),
          );
        } else {
          if (!context.mounted) return;

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text(
                'Login realizado, mas não foi possível carregar os dados do usuário.',
              ),
            ),
          );
        }

      } else if (response.statusCode == 403) {
        final usuarioUrl = Uri.parse(
          'http://localhost:8080/user/login/'
          '${Uri.encodeComponent(login)}',
        );

        final usuarioResponse = await http.get(usuarioUrl);

        if (usuarioResponse.statusCode == 200) {
          final usuario = jsonDecode(usuarioResponse.body);

          if (!context.mounted) return;

          Navigator.push(
            context,
            MaterialPageRoute(
              builder: (_) => ConfirmarEmail(email: login, id: usuario['id']),
            ),
          );
        } else {
          if (!context.mounted) return;

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Não foi possível localizar o usuário.'),
            ),
          );
        }

      } else if (response.statusCode == 401) {
        if (!context.mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('E-mail ou senha inválidos')),
        );

      } else {
        if (!context.mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Erro ao fazer login: ${response.statusCode}'),
          ),
        );
      }
    } catch (e) {
      if (!context.mounted) return;

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

                Form(
                  key: _formKey,
                  child: Column(
                    children: [
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
                        controller: senhaHashController,
                        obscureText: _obscurePassword,
                        decoration: InputDecoration(
                          prefixIcon: const Icon(Icons.lock),
                          hintText: "Senha",
                          filled: true,
                          fillColor: Colors.grey[200],
                          suffixIcon: IconButton(
                            icon: Icon(
                              _obscurePassword
                                  ? Icons.visibility_off
                                  : Icons.visibility,
                            ),
                            onPressed: () {
                              setState(() {
                                _obscurePassword = !_obscurePassword;
                              });
                            },
                          ),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(30),
                            borderSide: BorderSide.none,
                          ),
                        ),
                      ),
                      const SizedBox(height: 30),
                      TextButton(
                        onPressed: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(
                              builder: (context) => Esquecisenha(email: loginController.text),
                            ),
                          );
                        },
                        child: const Text("Esqueceu a senha?"),
                      ),
                    ],
                  ),
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
                    if (_formKey.currentState!.validate()) {
                      fazerLogin(context);
                    }
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
                          MaterialPageRoute(builder: (context) => Cadastro()),
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
