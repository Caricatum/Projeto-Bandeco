import 'package:flutter/material.dart';
import 'login.dart';

class Cadastro extends StatefulWidget {
  const Cadastro({super.key});

  @override
  State<Cadastro> createState() => _CadastroState();
}

class _CadastroState extends State<Cadastro> {

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
                  TextField(decoration: customInput("* Nome:", Icons.person)),
                  const SizedBox(height: 15),

                  // Senha
                  TextField(
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

                  // Telefone
                  TextField(
                    keyboardType: TextInputType.phone,
                    decoration: customInput("Telefone:", Icons.phone),
                  ),
                  const SizedBox(height: 15),

                  // CPF
                  TextField(
                    keyboardType: TextInputType.number,
                    decoration: customInput("* CPF:", Icons.badge),
                  ),
                  const SizedBox(height: 15),

                  // Email
                  TextField(
                    keyboardType: TextInputType.emailAddress,
                    decoration: customInput("* Email:", Icons.email),
                  ),
                  const SizedBox(height: 25),

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