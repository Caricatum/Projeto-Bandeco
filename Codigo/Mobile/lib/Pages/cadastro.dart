import 'package:flutter/material.dart';
import 'login.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;
import '../Class/usuarioClass.dart';
import 'confirmarEmail.dart';

class Cadastro extends StatefulWidget {
  const Cadastro({super.key});

  @override
  State<Cadastro> createState() => _CadastroState();
}

class _CadastroState extends State<Cadastro> {
  String tipoUsuario = "Aluno";

  bool _obscurePassword = true;

  InputDecoration customInput(String hint, IconData icon, {Widget? suffix}) {
    return InputDecoration(
      prefixIcon: Icon(icon, color: Colors.black),
      suffixIcon: suffix,
      hintText: hint,
      filled: true,
      fillColor: Colors.grey[200],
      contentPadding: const EdgeInsets.symmetric(vertical: 18),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(30),
        borderSide: BorderSide.none,
      ),
    );
  }

  Future<void> cadastrarUsuario() async {
    final email = loginController.text.trim();

    final url = Uri.parse('http://localhost:8080/user/cadastrar');

    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nome': nomeController.text.trim(),
          'login': email,
          'senhaHash': senhaHashController.text,
          'tipoUsuario': tipoUsuario,
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        // O cadastro não retorna o usuário.
        // Por isso buscamos o usuário pelo login/e-mail.
        final buscarUrl = Uri.parse(
          'http://localhost:8080/user/login/'
          '${Uri.encodeComponent(email)}',
        );

        final usuarioResponse = await http.get(buscarUrl);

        if (usuarioResponse.statusCode == 200) {
          final usuario = jsonDecode(usuarioResponse.body);

          if (!mounted) return;

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Cadastro realizado! Verifique seu e-mail.'),
            ),
          );

          Navigator.pushReplacement(
            context,
            MaterialPageRoute(
              builder: (_) => ConfirmarEmail(email: email, id: usuario['id']),
            ),
          );
        } else {
          if (!mounted) return;

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text(
                'Cadastro realizado, mas não foi possível localizar o usuário.',
              ),
            ),
          );
        }
      } else {
        if (!mounted) return;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao cadastrar: ${response.body}')),
        );
      }
    } catch (e) {
      if (!mounted) return;

      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    }
  }

  TextEditingController nomeController = TextEditingController();
  TextEditingController senhaHashController = TextEditingController();
  TextEditingController loginController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          children: [
            Stack(
              children: [
                Image.asset(
                  'assets/images/bandejao.png',
                  height: 260,
                  width: double.infinity,
                  fit: BoxFit.cover,
                ),

                Container(
                  height: 260,
                  color: Colors.black.withValues(alpha: 0.3),
                ),

                Positioned(
                  left: 20,
                  bottom: 40,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: const [
                      Text(
                        "Bandejão",
                        style: TextStyle(
                          color: Colors.white,
                          fontSize: 40,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 8),
                      Text(
                        "Cadastre-se agora!",
                        style: TextStyle(color: Colors.white, fontSize: 20),
                      ),
                    ],
                  ),
                ),
              ],
            ),

            Container(
              width: double.infinity,
              color: const Color(0xFFE97824),
              padding: const EdgeInsets.all(20),
              child: Column(
                children: [
                  // Nome
                  TextFormField(
                    controller: nomeController,
                    decoration: customInput("* Nome:", Icons.person),
                    validator: (value) {
                      if (value!.isEmpty) {
                        return "Preencha o campo nome";
                      } else {
                        return null;
                      }
                    },
                  ),
                  const SizedBox(height: 15),

                  // Email
                  TextFormField(
                    controller: loginController,
                    keyboardType: TextInputType.emailAddress,
                    decoration: customInput("* Email:", Icons.email),
                    validator: (value) {
                      if (value!.isEmpty) {
                        return "Preencha o campo nome";
                      } else {
                        return null;
                      }
                    },
                  ),
                  const SizedBox(height: 25),

                  // senhaHash
                  TextFormField(
                    controller: senhaHashController,
                    obscureText: _obscurePassword,
                    decoration: customInput(
                      "* Senha:",
                      Icons.lock,
                      suffix: IconButton(
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
                    ),
                    validator: (value) {
                      if (value!.isEmpty) {
                        return "Preencha o campo nome";
                      } else {
                        return null;
                      }
                    },
                  ),
                  const SizedBox(height: 15),

                  Row(
                    children: [
                      Expanded(
                        child: RadioListTile<String>(
                          title: const Text("Aluno"),
                          value: "Aluno",
                          groupValue: tipoUsuario,
                          onChanged: (value) {
                            setState(() {
                              tipoUsuario = value!;
                            });
                          },
                        ),
                      ),

                      Expanded(
                        child: RadioListTile<String>(
                          title: const Text("Funcionário"),
                          value: "Funcionário",
                          groupValue: tipoUsuario,
                          onChanged: (value) {
                            setState(() {
                              tipoUsuario = value!;
                            });
                          },
                        ),
                      ),
                    ],
                  ),

                  ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.grey[300],
                      foregroundColor: Colors.black,
                      padding: const EdgeInsets.symmetric(
                        horizontal: 50,
                        vertical: 15,
                      ),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(30),
                      ),
                    ),
                    onPressed: () async {
                      if (nomeController.text.isEmpty ||
                          loginController.text.isEmpty ||
                          senhaHashController.text.isEmpty) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(
                            content: Text('Preencha todos os campos'),
                          ),
                        );
                        return;
                      }

                      await cadastrarUsuario();
                    },
                    child: const Text(
                      "Cadastrar",
                      style: TextStyle(fontSize: 18),
                    ),
                  ),

                  const SizedBox(height: 15),

                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      const Text("Já tem uma conta? "),
                      GestureDetector(
                        onTap: () {
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => Login()),
                          );
                        },
                        child: const Text(
                          "Entre já!",
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
            ),
          ],
        ),
      ),
    );
  }
}
