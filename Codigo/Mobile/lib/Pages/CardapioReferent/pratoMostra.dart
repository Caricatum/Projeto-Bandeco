import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../../Class/avaliacaoClass.dart';
import '../../Class/pratosClass.dart';
import '../../Class/usuarioClass.dart';

class PratoPage extends StatefulWidget {
  final int pratoId;
  final Usuario usuario;

  const PratoPage({
    super.key,
    required this.pratoId,
    required this.usuario,
  });

  @override
  State<PratoPage> createState() => _PratoPageState();
}

class _PratoPageState extends State<PratoPage> {
  // ============================================================
  // VARIÁVEIS
  // ============================================================

  Pratos? prato;

  List<Avaliacao> avaliacoes = [];

  bool favorito = false;

  int? idFavorito;

  bool carregando = true;

  // ============================================================
  // FUNÇÕES
  // ============================================================

  @override
  void initState() {
    super.initState();

    carregarDados();
  }

  Future<void> carregarDados() async {
    await Future.wait([buscarPrato(), buscarAvaliacoes(), verificarFavorito()]);

    if (mounted) {
      setState(() {
        carregando = false;
      });
    }
  }

  Future<void> buscarPrato() async {
    final url = Uri.parse('http://localhost:8080/pratos/id/${widget.pratoId}');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        setState(() {
          prato = Pratos.fromJson(jsonDecode(response.body));
        });
      }
    } catch (e) {
      debugPrint('Erro ao buscar prato: $e');
    }
  }

  Future<void> buscarAvaliacoes() async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        setState(() {
          avaliacoes = dados
              .map((json) => Avaliacao.fromJson(json))
              .where((avaliacao) => avaliacao.pratoId == widget.pratoId)
              .toList();
        });
      }
    } catch (e) {
      debugPrint('Erro ao buscar avaliações: $e');
    }
  }

  Future<void> verificarFavorito() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        for (final item in dados) {
          final int pratoId = item['prato']['id'];

          final int usuarioId = item['user']['id'];

          if (pratoId == widget.pratoId &&
              usuarioId == widget.usuario.id) {
            setState(() {
              favorito = true;

              idFavorito = item['id'];
            });

            break;
          }
        }
      }
    } catch (e) {
      debugPrint('Erro ao verificar favorito: $e');
    }
  }

  Future<void> adicionarFavorito() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos/cadastrar');

    try {
      final response = await http.post(
        url,

        headers: {'Content-Type': 'application/json'},

        body: jsonEncode({
          'user': {'id': widget.usuario.id},

          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        final dados = jsonDecode(response.body);

        setState(() {
          favorito = true;

          idFavorito = dados['id'];
        });

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Prato adicionado aos favoritos!')),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Não foi possível adicionar aos favoritos.')),
        );
      }
    } catch (e) {
      debugPrint('Erro ao adicionar favorito: $e');
    }
  }

  Future<void> removerFavorito() async {
    if (idFavorito == null) {
      return;
    }

    final url = Uri.parse(
      'http://localhost:8080/pratosFavoritos/deletar/$idFavorito',
    );

    try {
      final response = await http.delete(url);

      if (response.statusCode == 200 || response.statusCode == 204) {
        setState(() {
          favorito = false;

          idFavorito = null;
        });

        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Prato removido dos favoritos.')),
        );
      }
    } catch (e) {
      debugPrint('Erro ao remover favorito: $e');
    }
  }

  Future<void> alternarFavorito() async {
    if (favorito) {
      await removerFavorito();
    } else {
      await adicionarFavorito();
    }
  }

  double calcularMedia() {
    if (avaliacoes.isEmpty) {
      return 0;
    }

    int soma = 0;

    for (final avaliacao in avaliacoes) {
      soma += avaliacao.nota;
    }

    return soma / avaliacoes.length;
  }

  // ============================================================
  // BODY
  // ============================================================

  @override
  Widget build(BuildContext context) {
    if (carregando) {
      return const Scaffold(body: Center(child: CircularProgressIndicator()));
    }

    if (prato == null) {
      return const Scaffold(
        body: Center(child: Text('Não foi possível carregar o prato.')),
      );
    }

    return Scaffold(
      appBar: AppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
        title: const Text('Detalhes do prato'),
      ),

      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,

        children: [
          _buildImagem(),

          Padding(
            padding: const EdgeInsets.all(16),

            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,

              children: [
                _buildTitulo(),

                const SizedBox(height: 10),

                _buildDescricao(),

                const SizedBox(height: 16),

                _buildNota(),

                const SizedBox(height: 16),

                _buildFavorito(),

                const SizedBox(height: 24),

                _buildComentarios(),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildImagem() {
    if (prato!.imagem == null || prato!.imagem!.isEmpty) {
      return Container(
        height: 250,

        width: double.infinity,

        color: Colors.grey[300],

        child: const Icon(Icons.restaurant, size: 80),
      );
    }

    return Image.network(
      prato!.imagem!,

      height: 250,

      width: double.infinity,

      fit: BoxFit.cover,

      errorBuilder: (context, error, stackTrace) {
        return Container(
          height: 250,

          width: double.infinity,

          color: Colors.grey[300],

          child: const Icon(Icons.broken_image, size: 80),
        );
      },
    );
  }

  Widget _buildTitulo() {
    return Row(
      children: [
        Expanded(
          child: Text(
            prato!.nome,

            style: const TextStyle(fontSize: 26, fontWeight: FontWeight.bold),
          ),
        ),

        if (prato!.vegano) const Icon(Icons.eco, color: Colors.green),
      ],
    );
  }

  Widget _buildDescricao() {
    return Text(prato!.descricao, style: const TextStyle(fontSize: 16));
  }

  Widget _buildNota() {
    final media = calcularMedia();

    return Row(
      children: [
        const Icon(Icons.star, color: Colors.amber, size: 30),

        const SizedBox(width: 8),

        Text(
          media.toStringAsFixed(1),

          style: const TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
        ),

        const SizedBox(width: 8),

        Text(
          '(${avaliacoes.length} avaliações)',

          style: TextStyle(fontSize: 15, color: Colors.grey[600]),
        ),
      ],
    );
  }

  Widget _buildFavorito() {
    return SizedBox(
      width: double.infinity,

      child: OutlinedButton.icon(
        onPressed: alternarFavorito,

        icon: Icon(
          favorito ? Icons.favorite : Icons.favorite_border,

          color: favorito ? Colors.red : null,
        ),

        label: Text(
          favorito ? 'Remover dos favoritos' : 'Adicionar aos favoritos',
        ),
      ),
    );
  }

  Widget _buildComentarios() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,

      children: [
        const Text(
          'Comentários',

          style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
        ),

        const SizedBox(height: 12),

        if (avaliacoes.isEmpty)
          const Text('Ainda não existem comentários para este prato.'),

        ...avaliacoes.map((avaliacao) => _buildComentario(avaliacao)),
      ],
    );
  }

  Widget _buildComentario(Avaliacao avaliacao) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),

      child: Padding(
        padding: const EdgeInsets.all(14),

        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,

          children: [
            Row(
              children: [
                const Icon(Icons.person),

                const SizedBox(width: 8),

                Expanded(
                  child: Text(
                    avaliacao.nomeUsuario,

                    style: const TextStyle(fontWeight: FontWeight.bold),
                  ),
                ),

                Row(
                  children: List.generate(
                    avaliacao.nota,

                    (index) =>
                        const Icon(Icons.star, size: 18, color: Colors.amber),
                  ),
                ),
              ],
            ),

            if (avaliacao.avaliacao != null && avaliacao.avaliacao!.isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(top: 10),

                child: Text(avaliacao.avaliacao!),
              ),
          ],
        ),
      ),
    );
  }
}
