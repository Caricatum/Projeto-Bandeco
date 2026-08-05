import 'package:flutter/material.dart';
import 'login.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;

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
    final url = Uri.parse('http://localhost:8080/user/cadastrar');

    try {
      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nome': nomeController.text,
          'login': loginController.text,
          'senhaHash': senhaController.text,
          'tipoUsuario': tipoUsuario,
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Usuário cadastrado com sucesso!')),
        );

        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (_) => Login()),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao cadastrar: ${response.body}')),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    }
  }

  TextEditingController nomeController = TextEditingController();
  TextEditingController senhaController = TextEditingController();
  TextEditingController loginController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Column(
          children: [
            // 🔝 IMAGEM COM TEXTO SOBREPOSTO
            Stack(
              children: [
                Image.asset(
                  'assets/images/bandejao.png',
                  height: 260,
                  width: double.infinity,
                  fit: BoxFit.cover,
                ),

                Container(height: 260, color: Colors.black.withValues(alpha: 0.3)),

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

            // FORMULÁRIO
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

                  // Senha
                  TextFormField(
                    controller: senhaController,
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

                  // BOTÃO
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
                          senhaController.text.isEmpty) {
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

                  // LINK LOGIN
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
