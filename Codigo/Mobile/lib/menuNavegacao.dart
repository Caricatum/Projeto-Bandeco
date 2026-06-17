import 'package:flutter/material.dart';
import 'principal.dart';
import 'cadastroprato.dart';
import 'cardapios.dart';

class MenuNavegacao extends StatelessWidget {
  const MenuNavegacao({super.key});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [

          const DrawerHeader(
            decoration: BoxDecoration(
              color: Colors.green,
            ),
            child: Center(
              child: Text(
                'Restaurante Universitário do Cotil',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 22,
                ),
              ),
            ),
          ),

          ListTile(
            leading: const Icon(Icons.home),
            title: const Text('Início'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Principal(),
                ),
              );
            },
          ),

          ListTile(
            leading: const Icon(Icons.restaurant_menu),
            title: const Text('Cardápios'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Cardapios(),
                ),
              );
            },
          ),

          ListTile(
            leading: const Icon(Icons.add_circle),
            title: const Text('Novo Prato'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Cadastroprato(),
                ),
              );
            },
          ),
        ],
      ),
    );
  }
}