import 'package:flutter/material.dart';
import 'package:tcc_flutter/cadastraCardapio.dart';
import 'package:tcc_flutter/cardapio.dart';
import 'package:tcc_flutter/favoritos.dart';
import 'principal.dart';
import 'cadastroprato.dart';
import 'cardapioPratos.dart';

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
            leading: const Icon(Icons.book),
            title: const Text('Pratos'),
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

          ListTile(
            leading: const Icon(Icons.star),
            title: const Text('Favoritos'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Favoritos(),
                ),
              );
            },
          ),

          ListTile(
            leading: const Icon(Icons.restaurant_menu),
            title: const Text('Cardapios'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Cardapio(),
                ),
              );
            },
          ),

          ListTile(
            leading: const Icon(Icons.playlist_add),
            title: const Text('Criar Cardápio'),
            onTap: () {
              Navigator.push(
                context,
                MaterialPageRoute(
                  builder: (context) => const Cadastracardapio(),
                ),
              );
            },
          ),
        ],
      ),
    );
  }
}