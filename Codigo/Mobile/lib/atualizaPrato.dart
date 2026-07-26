import 'dart:convert';
import 'package:tcc_flutter/categoria.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class AtualizaPrato extends StatefulWidget {
  final Map<String, dynamic> prato;

  const AtualizaPrato({
  super.key,
  required this.prato,
});

  @override
  State<AtualizaPrato> createState() => _AtualizaPratoState();
}

class _AtualizaPratoState extends State<AtualizaPrato> {
  final nomeController = TextEditingController();
  final descricaoController = TextEditingController();
  final imagemController = TextEditingController();
  final notaController = TextEditingController();

  bool vegano = false;
  bool carregando = false;
  Categoria? categoriaSelecionada;

  @override
  void initState() {
    super.initState();

    nomeController.text = widget.prato['nome'] ?? "";
    descricaoController.text = widget.prato['descricao'] ?? "";
    imagemController.text = widget.prato['imagem'] ?? "";
    notaController.text = widget.prato['nota']?.toString() ?? "";
    vegano = widget.prato['vegano'] ?? false;
    final categoriaId = widget.prato["categoria"]?["id"];
    categoriaSelecionada = categorias.firstWhere(
      (c) => c.id == categoriaId,
      orElse: () => categorias.first,
    );
  }

  Future<void> atualizarPrato() async {
    setState(() {
      carregando = true;
    });

    try {
      final response = await http.put(
        Uri.parse("http://localhost:8080/pratos/atualizar"),
        headers: {"Content-Type": "application/json"},
        body: jsonEncode({
          "id": widget.prato["id"],
          "nome": nomeController.text,
          "descricao": descricaoController.text,
          "imagem": imagemController.text,
          "vegano": vegano,
          "nota": double.tryParse(notaController.text),
          "categoria": {"id": categoriaSelecionada!.id,},
        }),
      );

      if (response.statusCode == 200) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Prato atualizado com sucesso!")),
        );
        Navigator.pop(context, true);
      } else {
        throw Exception("Erro ao atualizar prato.");
      }
    } catch (e) {
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text(e.toString())));
    }

    setState(() {
      carregando = false;
    });
  }

  Widget campo({
    required TextEditingController controller,
    required String label,
    required IconData icone,
    int maxLines = 1,
    TextInputType? teclado,
  }) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 18),
      child: TextField(
        controller: controller,
        maxLines: maxLines,
        keyboardType: teclado,
        decoration: InputDecoration(
          filled: true,
          fillColor: const Color(0xFFFFF3E6),
          prefixIcon: Icon(icone, color: const Color(0xFFE76F51)),
          labelText: label,
          labelStyle: const TextStyle(color: Color(0xFFE76F51)),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(15),
            borderSide: BorderSide.none,
          ),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFFFF8F1),

      appBar: AppBar(
        elevation: 0,
        centerTitle: true,
        backgroundColor: const Color(0xFFE76F51),
        title: const Text(
          "Editar Prato",
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
      ),

      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Card(
          elevation: 8,
          color: Colors.white,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(25),
          ),
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              children: [
                /// Imagem
                ClipRRect(
                  borderRadius: BorderRadius.circular(18),
                  child:
                      widget.prato["imagem"] != null &&
                          widget.prato["imagem"].toString().isNotEmpty
                      ? Image.network(
                          widget.prato["imagem"],
                          height: 220,
                          width: double.infinity,
                          fit: BoxFit.cover,
                          errorBuilder: (_, __, ___) {
                            return Container(
                              height: 220,
                              color: Colors.orange.shade100,
                              child: const Center(
                                child: Icon(
                                  Icons.restaurant,
                                  size: 70,
                                  color: Colors.orange,
                                ),
                              ),
                            );
                          },
                        )
                      : Container(
                          height: 220,
                          color: Colors.orange.shade100,
                          child: const Center(
                            child: Icon(
                              Icons.restaurant,
                              size: 70,
                              color: Colors.orange,
                            ),
                          ),
                        ),
                ),

                const SizedBox(height: 25),

                campo(
                  controller: nomeController,
                  label: "Nome do prato",
                  icone: Icons.restaurant_menu,
                ),

                campo(
                  controller: descricaoController,
                  label: "Descrição",
                  icone: Icons.description,
                  maxLines: 4,
                ),

                campo(
                  controller: imagemController,
                  label: "URL da imagem",
                  icone: Icons.image,
                ),

                campo(
                  controller: notaController,
                  label: "Nota",
                  icone: Icons.star,
                  teclado: TextInputType.number,
                ),

                DropdownButtonFormField<Categoria>(
                  value: categoriaSelecionada,
                  decoration: InputDecoration(
                    filled: true,
                    fillColor: const Color(0xFFFFF3E6),
                    prefixIcon: const Icon(
                      Icons.category,
                      color: Color(0xFFE76F51),
                    ),
                    labelText: "Categoria",
                    labelStyle: const TextStyle(color: Color(0xFFE76F51)),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(15),
                      borderSide: BorderSide.none,
                    ),
                  ),
                  items: categorias.map((categoria) {
                    return DropdownMenuItem<Categoria>(
                      value: categoria,
                      child: Text(categoria.descricao),
                    );
                  }).toList(),
                  onChanged: (Categoria? value) {
                    setState(() {
                      categoriaSelecionada = value;
                    });
                  },
                ),

                SwitchListTile(
                  activeColor: const Color(0xFFE76F51),
                  title: const Text(
                    "Prato Vegano",
                    style: TextStyle(fontWeight: FontWeight.bold),
                  ),
                  secondary: const Icon(Icons.eco, color: Colors.green),
                  value: vegano,
                  onChanged: (value) {
                    setState(() {
                      vegano = value;
                    });
                  },
                ),

                const SizedBox(height: 25),

                SizedBox(
                  width: double.infinity,
                  height: 55,
                  child: ElevatedButton.icon(
                    style: ElevatedButton.styleFrom(
                      backgroundColor: const Color(0xFFE76F51),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(18),
                      ),
                    ),
                    onPressed: carregando ? null : atualizarPrato,
                    icon: carregando
                        ? const SizedBox(
                            width: 22,
                            height: 22,
                            child: CircularProgressIndicator(
                              color: Colors.white,
                              strokeWidth: 3,
                            ),
                          )
                        : const Icon(Icons.save, color: Colors.white),
                    label: Text(
                      carregando ? "Salvando..." : "Salvar Alterações",
                      style: const TextStyle(
                        color: Colors.white,
                        fontSize: 17,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),

                const SizedBox(height: 15),

                SizedBox(
                  width: double.infinity,
                  height: 50,
                  child: OutlinedButton.icon(
                    style: OutlinedButton.styleFrom(
                      foregroundColor: const Color(0xFFE76F51),
                      side: const BorderSide(color: Color(0xFFE76F51)),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(18),
                      ),
                    ),
                    onPressed: () {
                      Navigator.pop(context);
                    },
                    icon: const Icon(Icons.arrow_back),
                    label: const Text("Cancelar"),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
