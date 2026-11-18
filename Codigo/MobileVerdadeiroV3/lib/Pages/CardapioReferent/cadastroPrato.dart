import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:http_parser/http_parser.dart';
import 'package:http/http.dart' as http;
import 'package:image_picker/image_picker.dart';
import 'package:tcc_flutter/Class/categoriaClass.dart';
import '../../Class/usuarioClass.dart';
import '../menuNavegacao.dart';
import 'cardapio.dart';

class Cadastroprato extends StatefulWidget {
  const Cadastroprato({super.key, required this.usuario});

  final Usuario usuario;

  @override
  State<Cadastroprato> createState() => _CadastropratoState();
}

class _CadastropratoState extends State<Cadastroprato> {
  final TextEditingController nomeController = TextEditingController();
  final TextEditingController descricaoController = TextEditingController();
  final TextEditingController notaController = TextEditingController();
  final ImagePicker _imagePicker = ImagePicker();

  final formKey = GlobalKey<FormState>();

  XFile? imagemSelecionada;
  Uint8List? imagemBytes;
  int categoriaSelecionada = 1;
  bool vegano = false;

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

  // Alertas Nutricionais (Alto teor)
  bool altoAcucarAdicionado = false;
  bool altoGorduraSaturada = false;
  bool altoSodio = false;

  bool salvando = false;

  @override
  void dispose() {
    nomeController.dispose();
    descricaoController.dispose();
    notaController.dispose();
    super.dispose();
  }

  Future<void> selecionarImagem() async {
    try {
      final arquivo = await _imagePicker.pickImage(
        source: ImageSource.gallery,
        imageQuality: 85,
      );
      if (arquivo == null) return;

      final extensao = arquivo.name.split('.').last.toLowerCase();
      if (!{'jpg', 'jpeg', 'png', 'webp'}.contains(extensao)) {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Selecione uma imagem JPG, PNG ou WEBP.'),
          ),
        );
        return;
      }

      final bytes = await arquivo.readAsBytes();
      if (!mounted) return;
      setState(() {
        imagemSelecionada = arquivo;
        imagemBytes = bytes;
      });
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Não foi possível selecionar a imagem: $e')),
      );
    }
  }

  Future<void> cadastrarPrato() async {
    if (!widget.usuario.funcionario) {
      return;
    }

    setState(() {
      salvando = true;
    });

    final url = Uri.parse('http://localhost:8080/pratos');

    try {
      final prato = {
        'nome': nomeController.text.trim(),
        'descricao': descricaoController.text.trim(),
        'vegano': vegano,
        'notaTecnica': notaController.text.trim().isEmpty
            ? null
            : notaController.text.trim(),
        'categoria': {'id': categoriaSelecionada},
        'leite': leite,
        'ovo': ovo,
        'amendoim': amendoim,
        'castanhas': castanhas,
        'frutosDoMar': frutosDoMar,
        'peixes': peixes,
        'trigo': trigo,
        'soja': soja,
        'gergilim': gergilim,
        'altoAcucarAdicionado': altoAcucarAdicionado,
        'altoGorduraSaturada': altoGorduraSaturada,
        'altoSodio': altoSodio,
      };
      final request = http.MultipartRequest('POST', url)
        ..files.add(
          http.MultipartFile.fromString(
            'prato',
            jsonEncode(prato),
            contentType: MediaType('application', 'json'),
          ),
        );

      final arquivo = imagemSelecionada;
      final bytes = imagemBytes;
      if (arquivo != null && bytes != null) {
        final extensao = arquivo.name.split('.').last.toLowerCase();
        request.files.add(
          http.MultipartFile.fromBytes(
            'imagem',
            bytes,
            filename: arquivo.name,
            contentType: MediaType(
              'image',
              extensao == 'jpg' ? 'jpeg' : extensao,
            ),
          ),
        );
      }

      final streamedResponse = await request.send();
      final response = await http.Response.fromStream(streamedResponse);

      if (response.statusCode == 200 || response.statusCode == 201) {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Prato cadastrado com sucesso!')),
        );

        Navigator.pushReplacement(
          context,
          MaterialPageRoute(
            builder: (_) => CardapioPage(usuario: widget.usuario),
          ),
        );
      } else {
        if (!mounted) return;
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao cadastrar: ${response.body}')),
        );
      }
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(SnackBar(content: Text('Erro de conexão: $e')));
    } finally {
      if (mounted) {
        setState(() {
          salvando = false;
        });
      }
    }
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
      appBar: AppBar(
        title: const Text(
          "Cadastro de Prato",
          style: TextStyle(fontWeight: FontWeight.bold, color: Colors.white),
        ),
        centerTitle: true,
        backgroundColor: const Color.fromARGB(255, 211, 47, 47),
        elevation: 0,
      ),
      drawer: MenuNavegacao(usuario: widget.usuario),
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              Colors.red.shade700,
              Colors.orange.shade500,
              Colors.amber.shade300,
            ],
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Form(
            key: formKey,
            child: ListView(
              children: [
                const SizedBox(height: 20),

                const Icon(
                  Icons.restaurant_menu,
                  size: 80,
                  color: Colors.white,
                ),

                const SizedBox(height: 10),

                const Text(
                  "Novo Prato",
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 28,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),

                const SizedBox(height: 25),

                Card(
                  elevation: 10,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Padding(
                    padding: const EdgeInsets.all(20),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        TextFormField(
                          controller: nomeController,
                          decoration: InputDecoration(
                            labelText: "Nome",
                            prefixIcon: const Icon(Icons.fastfood),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                          validator: (value) {
                            if (value == null || value.trim().isEmpty) {
                              return "Informe o nome";
                            }
                            return null;
                          },
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: descricaoController,
                          maxLines: 3,
                          decoration: InputDecoration(
                            labelText: "Descrição",
                            prefixIcon: const Icon(Icons.description),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        ClipRRect(
                          borderRadius: BorderRadius.circular(12),
                          child: SizedBox(
                            height: 220,
                            width: double.infinity,
                            child: imagemBytes == null
                                ? Container(
                                    color: Colors.orange.shade100,
                                    child: const Icon(
                                      Icons.restaurant,
                                      size: 70,
                                      color: Colors.orange,
                                    ),
                                  )
                                : Image.memory(imagemBytes!, fit: BoxFit.cover),
                          ),
                        ),

                        const SizedBox(height: 8),

                        Row(
                          children: [
                            Expanded(
                              child: OutlinedButton.icon(
                                onPressed: salvando ? null : selecionarImagem,
                                icon: const Icon(Icons.upload_file),
                                label: Text(
                                  imagemSelecionada == null
                                      ? 'Selecionar foto'
                                      : 'Trocar foto',
                                ),
                              ),
                            ),
                            if (imagemSelecionada != null) ...[
                              const SizedBox(width: 8),
                              IconButton(
                                tooltip: 'Remover foto',
                                onPressed: salvando
                                    ? null
                                    : () {
                                        setState(() {
                                          imagemSelecionada = null;
                                          imagemBytes = null;
                                        });
                                      },
                                icon: const Icon(Icons.delete_outline),
                              ),
                            ],
                          ],
                        ),

                        const SizedBox(height: 15),

                        TextFormField(
                          controller: notaController,
                          decoration: InputDecoration(
                            labelText: "Nota Técnica",
                            prefixIcon: const Icon(Icons.star),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                        ),

                        const SizedBox(height: 15),

                        DropdownButtonFormField<int>(
                          value: categoriaSelecionada,
                          decoration: InputDecoration(
                            labelText: "Categoria",
                            prefixIcon: const Icon(Icons.category),
                            border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(12),
                            ),
                          ),
                          items: categorias.map((categoria) {
                            return DropdownMenuItem<int>(
                              value: categoria.id,
                              child: Text(categoria.descricao),
                            );
                          }).toList(),
                          onChanged: (value) {
                            setState(() {
                              categoriaSelecionada = value!;
                            });
                          },
                        ),

                        const SizedBox(height: 15),

                        Container(
                          decoration: BoxDecoration(
                            color: Colors.green.shade50,
                            borderRadius: BorderRadius.circular(12),
                          ),
                          child: SwitchListTile(
                            title: const Text(
                              "Prato Vegano",
                              style: TextStyle(fontWeight: FontWeight.bold),
                            ),
                            secondary: const Icon(
                              Icons.eco,
                              color: Colors.green,
                            ),
                            value: vegano,
                            activeColor: Colors.green,
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
                            borderRadius: BorderRadius.circular(12),
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
                                onChanged: (v) =>
                                    setState(() => frutosDoMar = v),
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
                            borderRadius: BorderRadius.circular(12),
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
                          child: ElevatedButton(
                            onPressed: salvando
                                ? null
                                : () {
                                    if (formKey.currentState!.validate()) {
                                      cadastrarPrato();
                                    }
                                  },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: Colors.red.shade700,
                              shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(15),
                              ),
                            ),
                            child: salvando
                                ? const SizedBox(
                                    width: 24,
                                    height: 24,
                                    child: CircularProgressIndicator(
                                      color: Colors.white,
                                      strokeWidth: 3,
                                    ),
                                  )
                                : const Text(
                                    "Cadastrar Prato",
                                    style: TextStyle(
                                      fontSize: 18,
                                      color: Colors.white,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),

                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
