import 'package:flutter/material.dart';
import 'cardapios.dart';
import 'cadastroPrato.dart';
import 'perfil.dart';

class Principal extends StatefulWidget {
  const Principal({super.key});

  @override
  State<Principal> createState() => _PrincipalState();
}

class _PrincipalState extends State<Principal> {
  int indexAtual = 0;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          "Bem Vindo (nome do user)!",
          style: TextStyle(fontSize: 30),
        ), //Colocar nome do Usuario no Bem Vindo
        backgroundColor: Colors.orangeAccent,
        actions: [
          IconButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Perfil()),
              );
            },
            icon: Icon(Icons.account_circle),
          ),
        ],
      ),
      body: IndexedStack(
        index: indexAtual,
        children: [
          // Página Início
          Container(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                Image.asset(
                  'assets/images/cotil_unicamp.png',
                  width: 300,
                  height: 200,
                ),
              ],
            ),
          ),
          Cardapios(),
          Cadastroprato(),
        ],
      ),

      bottomNavigationBar: BottomNavigationBar(
        onTap: (index) {
          setState(() {
            indexAtual = index;
          });
        },
        currentIndex: indexAtual,
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: "Início"),
          BottomNavigationBarItem(
            icon: Icon(Icons.menu_book),
            label: "Cardápios",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.new_label),
            label: "Novo Prato",
          ),
        ],
      ),
    );
  }
}
