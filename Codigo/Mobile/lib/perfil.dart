import 'package:flutter/material.dart';

class Perfil extends StatefulWidget {
  const Perfil({super.key});

  @override
  State<Perfil> createState() => _PerfilState();
}

class _PerfilState extends State<Perfil> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Column(
              children: [
                const CircleAvatar(
                  radius: 50,
                  backgroundImage: NetworkImage(
                    "https://i.pravatar.cc/150?img=3",
                  ),
                ),
                const SizedBox(height: 10),
                Text(
                  "Nome do Usuário",
                  style: const TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                Text(
                  "RA: 123456789",
                  style: TextStyle(color: Colors.grey[600]),
                ),
              ],
            ),

            const SizedBox(height: 30),

            // Card de saldo
            Card(
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(16),
              ),
              elevation: 4,
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    const Text(
                      "Saldo disponível",
                      style: TextStyle(fontSize: 16),
                    ),
                    Text(
                      "R\$ (100,00)",
                      style: const TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                        color: Colors.green,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            const SizedBox(height: 20),

            // Botões de ação
            Expanded(
              child: ListView(
                children: [
                  ListTile(
                    leading: const Icon(Icons.restaurant),
                    title: const Text("Histórico de refeições"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {
                      // Navegar
                    },
                  ),
                  ListTile(
                    leading: const Icon(Icons.attach_money),
                    title: const Text("Recarregar saldo"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {
                      // Navegar
                    },
                  ),
                  ListTile(
                    leading: const Icon(Icons.settings),
                    title: const Text("Configurações"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {},
                  ),
                  ListTile(
                    leading: const Icon(Icons.logout),
                    title: const Text("Sair"),
                    trailing: const Icon(Icons.arrow_forward_ios),
                    onTap: () {},
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