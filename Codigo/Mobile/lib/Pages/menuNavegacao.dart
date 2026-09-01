import 'package:flutter/material.dart';
import 'package:tcc_flutter/Pages/CardapioReferent/cadastraCardapioDia.dart';
import 'CardapioReferent/cadastraCardapio.dart';
import 'CardapioReferent/cardapio.dart';
import 'package:tcc_flutter/Pages/UserReferent/favoritos.dart';
import 'principal.dart';
import '../Pages/CardapioReferent/cadastroPrato.dart';
import 'CardapioReferent/cardapioPratos.dart';
import '../Class/usuarioClass.dart';

class MenuNavegacao extends StatelessWidget {
  final Usuario usuario;

  const MenuNavegacao({super.key, required this.usuario});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [
          const DrawerHeader(
            decoration: BoxDecoration(color: Colors.green),
            child: Center(
              child: Text(
                'Restaurante Universitário do Cotil',
                style: TextStyle(color: Colors.white, fontSize: 22),
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
                  builder: (context) => Principal(usuario: usuario),
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
                  builder: (context) => CardapioPratos(usuario: usuario),
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
                  builder: (context) => Cadastroprato(usuario: usuario),
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
                  builder: (context) => Favoritos(usuario: usuario),
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
                MaterialPageRoute(builder: (context) => const CardapioPage()),
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
                  builder: (context) => const CadastraCardapio(),
                ),
              );
            },
          ),
          ListTile(
            leading: const Icon(Icons.logout),
            title: const Text('Cardápios do Dia'),
            onTap: () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                  builder: (context) => CardapioDiaPage(usuario: usuario),
                ),
              );
            },
          ),
        ],
      ),
    );
  }
}
