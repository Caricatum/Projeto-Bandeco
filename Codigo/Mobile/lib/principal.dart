import 'package:flutter/material.dart';
import 'package:tcc_flutter/cadastroPrato.dart';
import 'package:tcc_flutter/cardapios.dart';
import 'package:tcc_flutter/perfil.dart';

class Principal extends StatefulWidget {
  const Principal({super.key});

  @override
  State<Principal> createState() => _PrincipalState();
}

class _PrincipalState extends State<Principal> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Bem Vindo!", style: TextStyle(fontSize: 30)), //Colocar nome do Usuario no Bem Vindo
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
      body: Container(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Image.asset(
              'assets/images/cotil_unicamp.png',
              width: 300,
              height: 200,
            ),
            TextField(
              decoration: InputDecoration(
                label: Text("Buscar", style: TextStyle(fontSize: 20)),
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(50),
                ),
                suffixIcon: Icon(Icons.search),
              ),
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(50),
                    ),
                  ),
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => Cardapios()),
                    );
                  },
                  child: Text("Cardápios"),
                ),

                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(50),
                    ),
                  ),
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => Cadastroprato()),
                    );
                  },
                  child: Text("Cadastrar Prato"),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
