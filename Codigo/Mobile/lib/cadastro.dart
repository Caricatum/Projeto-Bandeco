import 'package:flutter/material.dart';
import 'login.dart';

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

  TextEditingController nomeController = new TextEditingController();
  TextEditingController senhaController = new TextEditingController();
  TextEditingController loginController = new TextEditingController();

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

                Container(height: 260, color: Colors.black.withOpacity(0.3)),

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

            // 🟧 FORMULÁRIO
            Container(
              width: double.infinity,
              color: const Color(0xFFE97824),
              padding: const EdgeInsets.all(20),
              child: Column(
                children: [
                  // Nome
                  TextField(
                    controller: nomeController,
                    decoration: customInput("* Nome:", Icons.person),
                  ),
                  const SizedBox(height: 15),

                  // Senha
                  TextField(
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
                  ),
                  const SizedBox(height: 15),

                  // Email
                  TextField(
                    controller: loginController,
                    keyboardType: TextInputType.emailAddress,
                    decoration: customInput("* Email:", Icons.email),
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
                    onPressed: () {},
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
