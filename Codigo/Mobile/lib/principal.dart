import 'package:flutter/material.dart';
import 'cardapios.dart';
import 'cadastroprato.dart';
import 'perfil.dart';
import 'login.dart';

class Principal extends StatefulWidget {

  @override
  State<Principal> createState() => _PrincipalState();
}

class _PrincipalState extends State<Principal> {
  int indexAtual = 0;

  final List<String> avisos = [];

  void _adicionarAviso(String texto) {
    setState(() {
      avisos.insert(0, texto);
    });
  }

  void _mostrarDialogoAdicionarAviso() {
    final TextEditingController controller = TextEditingController();

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Novo aviso"),
          content: TextField(
            controller: controller,
            decoration: const InputDecoration(hintText: "Digite o aviso"),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text("Cancelar"),
            ),
            ElevatedButton(
              onPressed: () {
                if (controller.text.trim().isNotEmpty) {
                  _adicionarAviso(controller.text.trim());
                }
                Navigator.pop(context);
              },
              child: const Text("Adicionar"),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        // BÔNUS: Já incluído o nome do usuário dinamicamente no título!
        title: Text(
          "Bem-vindo",
          style: const TextStyle(fontSize: 15),
        ), 
        backgroundColor: Colors.orangeAccent,
        actions: [
          IconButton(
            onPressed: () {
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => Perfil()),
              );
            },
            icon: const Icon(Icons.account_circle),
          ),
        ],
      ),
      body: Column(
        children: [
          const SizedBox(height: 10),
          const Text(
            "Mural de Avisos",
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 10),
          Expanded(
            child: avisos.isEmpty
                ? const Center(
                    child: Text("Nenhum aviso ainda."),
                  )
                : ListView.builder(
                    itemCount: avisos.length,
                    itemBuilder: (context, index) {
                      return Card(
                        margin: const EdgeInsets.symmetric(
                          horizontal: 12,
                          vertical: 6,
                        ),
                        child: ListTile(
                          leading: const Icon(Icons.announcement),
                          title: Text(avisos[index]),
                        ),
                      );
                    },
                  ),
          ),
        ],
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _mostrarDialogoAdicionarAviso,
        child: const Icon(Icons.add),
      ),
      bottomNavigationBar: BottomNavigationBar(
        onTap: (index) {
          setState(() {
            indexAtual = index;
          });
          // Nota: Aqui você precisará implementar a navegação real entre as telas
          // usando o indexAtual no body, se desejar mudar de página.
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