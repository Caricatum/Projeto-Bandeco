import 'dart:convert';
import 'package:tcc_flutter/Class/categoriaClass.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http_parser/http_parser.dart';
import '../../Class/usuarioClass.dart';
import '../../Widgets/imagemPrato.dart';

class AtualizaPrato extends StatefulWidget {
  final Map<String, dynamic> prato;
  final Usuario usuario;

  const AtualizaPrato({super.key, required this.prato, required this.usuario});

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

  // Alérgenos
  bool leite = false;
  bool ovo = false;
  bool amendoim = false;
  bool castanhas = false;
  bool frutosDoMar = false;
  bool peixes = false;
  bool trigo = false;
  bool soja = false;
  bool gergilim = false;

  // Alertas Nutricionais
  bool altoAcucarAdicionado = false;
  bool altoGorduraSaturada = false;
  bool altoSodio = false;

  @override
  void initState() {
    super.initState();

    nomeController.text = widget.prato['nome'] ?? "";
    descricaoController.text = widget.prato['descricao'] ?? "";
    imagemController.text = widget.prato['imagem'] ?? "";
    notaController.text =
        widget.prato['notaTecnica'] ?? widget.prato['nota']?.toString() ?? "";
    vegano = widget.prato['vegano'] ?? false;

    leite = widget.prato['leite'] ?? false;
    ovo = widget.prato['ovo'] ?? false;
    amendoim = widget.prato['amendoim'] ?? false;
    castanhas = widget.prato['castanhas'] ?? false;
    frutosDoMar = widget.prato['frutosDoMar'] ?? false;
    peixes = widget.prato['peixes'] ?? false;
    trigo = widget.prato['trigo'] ?? false;
    soja = widget.prato['soja'] ?? false;
    gergilim = widget.prato['gergilim'] ?? false;

    altoAcucarAdicionado = widget.prato['altoAcucarAdicionado'] ?? false;
    altoGorduraSaturada = widget.prato['altoGorduraSaturada'] ?? false;
    altoSodio = widget.prato['altoSodio'] ?? false;

    final categoriaId = widget.prato["categoria"]?["id"];
    categoriaSelecionada = categorias.firstWhere(
      (c) => c.id == categoriaId,
      orElse: () => categorias.first,
    );
  }

  @override
  void dispose() {
    nomeController.dispose();
    descricaoController.dispose();
    imagemController.dispose();
    notaController.dispose();
    super.dispose();
  }

  Future<void> atualizarPrato() async {
    if (!widget.usuario.funcionario) {
      return;
    }

    setState(() {
      carregando = true;
    });

    try {
      final request = http.MultipartRequest(
        'PUT',
        Uri.parse("http://localhost:8080/pratos/${widget.prato["id"]}"),
      );
      request.files.add(
        http.MultipartFile.fromString(
          'prato',
          jsonEncode({
          "id": widget.prato["id"],
          "nome": nomeController.text.trim(),
          "descricao": descricaoController.text.trim(),
          "imagem": imagemController.text.trim().isEmpty
              ? null
              : imagemController.text.trim(),
          "vegano": vegano,
          "notaTecnica": notaController.text.trim().isEmpty
              ? null
              : notaController.text.trim(),
          "categoria": {"id": categoriaSelecionada!.id},
          "leite": leite,
          "ovo": ovo,
          "amendoim": amendoim,
          "castanhas": castanhas,
          "frutosDoMar": frutosDoMar,
          "peixes": peixes,
          "trigo": trigo,
          "soja": soja,
          "gergilim": gergilim,
          "altoAcucarAdicionado": altoAcucarAdicionado,
          "altoGorduraSaturada": altoGorduraSaturada,
          "altoSodio": altoSodio,
          }),
          contentType: MediaType('application', 'json'),
        ),
      );
      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);

      if (response.statusCode == 200) {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Prato atualizado com sucesso!")),
        );
        Navigator.pop(context, true);
      } else {
        throw Exception(
          "Erro ao atualizar prato. Código: ${response.statusCode}",
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text(e.toString())));
      }
    } finally {
      if (mounted) {
        setState(() {
          carregando = false;
        });
      }
    }
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

  Widget _buildSwitchItem({
    required String titulo,
    required String emoji,
    required bool valor,
    required ValueChanged<bool> onChanged,
    Color corAtiva = const Color(0xFFE76F51),
  }) {
    return SwitchListTile(
      dense: true,
      contentPadding: const EdgeInsets.symmetric(horizontal: 12),
      activeColor: corAtiva,
      title: Row(
        children: [
          Text(emoji, style: const TextStyle(fontSize: 18)),
          const SizedBox(width: 8),
          Expanded(
            child: Text(
              titulo,
              style: const TextStyle(fontSize: 14, fontWeight: FontWeight.w600),
            ),
          ),
        ],
      ),
      value: valor,
      onChanged: onChanged,
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
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                /// Imagem
                ClipRRect(
                  borderRadius: BorderRadius.circular(18),
                  child:
                      widget.prato["imagem"] != null &&
                          widget.prato["imagem"].toString().isNotEmpty
                      ? ImagemPrato(
                          imagem: widget.prato["imagem"].toString(),
                          height: 220,
                          width: double.infinity,
                          fit: BoxFit.cover,
                          placeholderColor: Colors.orange.shade100,
                          placeholderIcon: Icons.restaurant,
                          placeholderIconColor: Colors.orange,
                          placeholderIconSize: 70,
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
                  label: "Nota Técnica",
                  icone: Icons.note_alt_outlined,
                  maxLines: 2,
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

                const SizedBox(height: 12),

                Container(
                  decoration: BoxDecoration(
                    color: Colors.green.shade50,
                    borderRadius: BorderRadius.circular(15),
                  ),
                  child: SwitchListTile(
                    activeColor: Colors.green,
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
                ),

                const SizedBox(height: 20),

                // SEÇÃO DE ALÉRGENOS
                const Text(
                  "⚠️ Alérgenos presentes",
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
                const SizedBox(height: 8),

                Container(
                  decoration: BoxDecoration(
                    color: const Color(0xFFFFF3E6),
                    borderRadius: BorderRadius.circular(15),
                    border: Border.all(color: Colors.orange.shade200),
                  ),
                  child: Column(
                    children: [
                      _buildSwitchItem(
                        titulo: "Leite / Lactose",
                        emoji: "🥛",
                        valor: leite,
                        onChanged: (v) => setState(() => leite = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Ovo",
                        emoji: "🥚",
                        valor: ovo,
                        onChanged: (v) => setState(() => ovo = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Amendoim",
                        emoji: "🥜",
                        valor: amendoim,
                        onChanged: (v) => setState(() => amendoim = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Castanhas / Nozes",
                        emoji: "🌰",
                        valor: castanhas,
                        onChanged: (v) => setState(() => castanhas = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Frutos do Mar",
                        emoji: "🦐",
                        valor: frutosDoMar,
                        onChanged: (v) => setState(() => frutosDoMar = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Peixes",
                        emoji: "🐟",
                        valor: peixes,
                        onChanged: (v) => setState(() => peixes = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Trigo / Glúten",
                        emoji: "🌾",
                        valor: trigo,
                        onChanged: (v) => setState(() => trigo = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Soja",
                        emoji: "🌱",
                        valor: soja,
                        onChanged: (v) => setState(() => soja = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Gergelim",
                        emoji: "⚪",
                        valor: gergilim,
                        onChanged: (v) => setState(() => gergilim = v),
                      ),
                    ],
                  ),
                ),

                const SizedBox(height: 20),

                // SEÇÃO DE RÓTULOS NUTRICIONAIS (ALTO TEOR)
                const Text(
                  "🏷️ Rótulos de Alto Teor (Anvisa)",
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
                const SizedBox(height: 8),

                Container(
                  decoration: BoxDecoration(
                    color: const Color(0xFFFFFBEB),
                    borderRadius: BorderRadius.circular(15),
                    border: Border.all(color: Colors.amber.shade300),
                  ),
                  child: Column(
                    children: [
                      _buildSwitchItem(
                        titulo: "Alto em Açúcar Adicionado",
                        emoji: "🍬",
                        valor: altoAcucarAdicionado,
                        corAtiva: Colors.amber.shade800,
                        onChanged: (v) =>
                            setState(() => altoAcucarAdicionado = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Alto em Gordura Saturada",
                        emoji: "🥩",
                        valor: altoGorduraSaturada,
                        corAtiva: Colors.amber.shade800,
                        onChanged: (v) =>
                            setState(() => altoGorduraSaturada = v),
                      ),
                      const Divider(height: 1),
                      _buildSwitchItem(
                        titulo: "Alto em Sódio",
                        emoji: "🧂",
                        valor: altoSodio,
                        corAtiva: Colors.amber.shade800,
                        onChanged: (v) => setState(() => altoSodio = v),
                      ),
                    ],
                  ),
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
