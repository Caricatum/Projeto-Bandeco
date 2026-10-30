import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../../Class/avaliacaoClass.dart';
import '../../Class/categoriaClass.dart';
import '../../Class/pratosClass.dart';
import '../../Class/usuarioClass.dart';
import '../../Class/valorNutricionalClass.dart';
import '../../Widgets/imagemPrato.dart';

class PratoPage extends StatefulWidget {
  final int pratoId;
  final Usuario usuario;

  const PratoPage({super.key, required this.pratoId, required this.usuario});

  @override
  State<PratoPage> createState() => _PratoPageState();
}

class _PratoPageState extends State<PratoPage> {
  Pratos? prato;
  ValorNutricional? valorNutricional;

  List<Avaliacao> avaliacoes = [];

  Avaliacao? minhaAvaliacao;

  bool favorito = false;
  int? idFavorito;

  bool carregando = true;
  bool enviandoAvaliacao = false;

  @override
  void initState() {
    super.initState();
    carregarDados();
  }

  // ============================================================
  // CARREGAMENTO INICIAL
  // ============================================================

  Future<void> carregarDados() async {
    await Future.wait([
      buscarPrato(),
      buscarAvaliacoes(),
      verificarFavorito(),
      buscarValorNutricional(),
    ]);

    if (mounted) {
      setState(() {
        carregando = false;
      });
    }
  }

  // ============================================================
  // BUSCAR PRATO
  // ============================================================

  Future<void> buscarPrato() async {
    final url = Uri.parse('http://localhost:8080/pratos/id/${widget.pratoId}');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        if (mounted) {
          setState(() {
            prato = Pratos.fromJson(jsonDecode(response.body));
          });
        }
      } else {
        debugPrint('Erro ao buscar prato. Código: ${response.statusCode}');
      }
    } catch (e) {
      debugPrint('Erro ao buscar prato: $e');
    }
  }

  // ============================================================
  // BUSCAR VALOR NUTRICIONAL
  // ============================================================

  Future<void> buscarValorNutricional() async {
    final url = Uri.parse('http://localhost:8080/valorNutricional/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        for (final item in dados) {
          if (item['prato'] != null && item['prato']['id'] == widget.pratoId) {
            if (mounted) {
              setState(() {
                valorNutricional = ValorNutricional.fromJson(item);
              });
            }
            break;
          }
        }
      }
    } catch (e) {
      debugPrint('Erro ao buscar valor nutricional: $e');
    }
  }

  // ============================================================
  // BUSCAR AVALIAÇÕES DO PRATO
  // ============================================================

  Future<void> buscarAvaliacoes() async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        final List<Avaliacao> lista = dados
            .map((json) => Avaliacao.fromJson(json))
            .where((avaliacao) => avaliacao.pratoId == widget.pratoId)
            .toList();

        Avaliacao? avaliacaoDoUsuario;

        for (final avaliacao in lista) {
          if (avaliacao.usuarioId == widget.usuario.id) {
            avaliacaoDoUsuario = avaliacao;
            break;
          }
        }

        if (mounted) {
          setState(() {
            avaliacoes = lista;
            minhaAvaliacao = avaliacaoDoUsuario;
          });
        }
      } else {
        debugPrint('Erro ao buscar avaliações. Código: ${response.statusCode}');
      }
    } catch (e) {
      debugPrint('Erro ao buscar avaliações: $e');
    }
  }

  // ============================================================
  // VERIFICAR FAVORITO
  // ============================================================

  Future<void> verificarFavorito() async {
    final url = Uri.parse('http://localhost:8080/pratosFavoritos/all');

    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);

        for (final item in dados) {
          final int pratoId = item['prato']['id'];
          final int usuarioId = item['user']['id'];

          if (pratoId == widget.pratoId && usuarioId == widget.usuario.id) {
            if (mounted) {
              setState(() {
                favorito = true;
                idFavorito = item['id'];
              });
            }

            break;
          }
        }
      }
    } catch (e) {
      debugPrint('Erro ao verificar favorito: $e');
    }
  }

  // ============================================================
  // ADICIONAR FAVORITO
  // ============================================================

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

        if (mounted) {
          setState(() {
            favorito = true;
            idFavorito = dados['id'];
          });

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Prato adicionado aos favoritos!')),
          );
        }
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(
              content: Text('Não foi possível adicionar aos favoritos.'),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao adicionar favorito: $e');
    }
  }

  // ============================================================
  // REMOVER FAVORITO
  // ============================================================

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
        if (mounted) {
          setState(() {
            favorito = false;
            idFavorito = null;
          });

          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Prato removido dos favoritos.')),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao remover favorito: $e');
    }
  }

  // ============================================================
  // ALTERNAR FAVORITO
  // ============================================================

  Future<void> alternarFavorito() async {
    if (favorito) {
      await removerFavorito();
    } else {
      await adicionarFavorito();
    }
  }

  // ============================================================
  // CADASTRAR AVALIAÇÃO
  // ============================================================

  Future<void> cadastrarAvaliacao(int nota, String comentario) async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/cadastrar');

    try {
      setState(() {
        enviandoAvaliacao = true;
      });

      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nota': nota,
          'avaliacao': comentario.trim().isEmpty ? null : comentario.trim(),
          'user': {'id': widget.usuario.id},
          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação enviada com sucesso!')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível enviar a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao cadastrar avaliação: $e');

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Erro de conexão com o servidor.')),
        );
      }
    } finally {
      if (mounted) {
        setState(() {
          enviandoAvaliacao = false;
        });
      }
    }
  }

  // ============================================================
  // ATUALIZAR AVALIAÇÃO
  // ============================================================

  Future<void> atualizarAvaliacao(
    Avaliacao avaliacao,
    int nota,
    String comentario,
  ) async {
    final url = Uri.parse('http://localhost:8080/avaliacoes/atualizar');

    try {
      setState(() {
        enviandoAvaliacao = true;
      });

      final response = await http.put(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'id': avaliacao.id,
          'nota': nota,
          'avaliacao': comentario.trim().isEmpty ? null : comentario.trim(),
          'user': {'id': widget.usuario.id},
          'prato': {'id': widget.pratoId},
        }),
      );

      if (response.statusCode == 200) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação atualizada com sucesso!')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível atualizar a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao atualizar avaliação: $e');
    } finally {
      if (mounted) {
        setState(() {
          enviandoAvaliacao = false;
        });
      }
    }
  }

  // ============================================================
  // EXCLUIR AVALIAÇÃO
  // ============================================================

  Future<void> excluirAvaliacao(Avaliacao avaliacao) async {
    final url = Uri.parse(
      'http://localhost:8080/avaliacoes/deletar/${avaliacao.id}',
    );

    try {
      final response = await http.delete(url);

      if (response.statusCode == 200 || response.statusCode == 204) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Avaliação excluída com sucesso.')),
          );
        }

        await buscarAvaliacoes();
        await buscarPrato();
      } else {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text(
                'Não foi possível excluir a avaliação. '
                'Código: ${response.statusCode}',
              ),
            ),
          );
        }
      }
    } catch (e) {
      debugPrint('Erro ao excluir avaliação: $e');
    }
  }

  // ============================================================
  // CONFIRMAR EXCLUSÃO
  // ============================================================

  void confirmarExclusao(Avaliacao avaliacao) {
    showDialog(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          title: const Text('Excluir avaliação'),
          content: const Text('Tem certeza que deseja excluir sua avaliação?'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(dialogContext);
              },
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              onPressed: () async {
                Navigator.pop(dialogContext);

                await excluirAvaliacao(avaliacao);
              },
              child: const Text('Excluir'),
            ),
          ],
        );
      },
    );
  }

  // ============================================================
  // ABRIR FORMULÁRIO DE AVALIAÇÃO
  // ============================================================

  void abrirDialogAvaliacao({Avaliacao? avaliacaoExistente}) {
    int nota = avaliacaoExistente?.nota ?? 5;

    final comentarioController = TextEditingController(
      text: avaliacaoExistente?.avaliacao ?? '',
    );

    final bool editando = avaliacaoExistente != null;

    showDialog(
      context: context,
      builder: (dialogContext) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              backgroundColor: Colors.white,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(20),
              ),
              title: Text(
                editando ? 'Editar avaliação' : 'Avaliar prato',
                style: const TextStyle(
                  color: Color(0xFF9C3D2A),
                  fontWeight: FontWeight.bold,
                ),
              ),
              content: SingleChildScrollView(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    const Text(
                      'Escolha sua nota:',
                      style: TextStyle(fontWeight: FontWeight.w600),
                    ),

                    const SizedBox(height: 8),

                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: List.generate(5, (index) {
                        final valor = index + 1;

                        return IconButton(
                          onPressed: () {
                            setDialogState(() {
                              nota = valor;
                            });
                          },
                          icon: Icon(
                            valor <= nota ? Icons.star : Icons.star_border,
                            color: Colors.amber,
                            size: 32,
                          ),
                        );
                      }),
                    ),

                    const SizedBox(height: 10),

                    TextField(
                      controller: comentarioController,
                      maxLines: 4,
                      decoration: InputDecoration(
                        labelText: 'Comentário',
                        labelStyle: const TextStyle(color: Color(0xFF9C3D2A)),
                        hintText: 'O que você achou do prato?',
                        filled: true,
                        fillColor: const Color(0xFFFFF8F1),
                        border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(14),
                          borderSide: const BorderSide(color: Color(0xFFF4A261)),
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(14),
                          borderSide: const BorderSide(
                            color: Color(0xFFE76F51),
                            width: 2,
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              actions: [
                TextButton(
                  style: TextButton.styleFrom(
                    foregroundColor: Colors.grey.shade700,
                  ),
                  onPressed: () {
                    Navigator.pop(dialogContext);
                  },
                  child: const Text('Cancelar'),
                ),

                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFE76F51),
                    foregroundColor: Colors.white,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                  ),
                  onPressed: () async {
                    Navigator.pop(dialogContext);

                    if (editando) {
                      await atualizarAvaliacao(
                        avaliacaoExistente,
                        nota,
                        comentarioController.text,
                      );
                    } else {
                      await cadastrarAvaliacao(nota, comentarioController.text);
                    }
                  },
                  child: Text(editando ? 'Salvar' : 'Enviar'),
                ),
              ],
            );
          },
        );
      },
    ).then((_) {
      comentarioController.dispose();
    });
  }

  // ============================================================
  // CALCULAR MÉDIA
  // ============================================================

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
  // BUILD
  // ============================================================

  @override
  Widget build(BuildContext context) {
    if (carregando) {
      return const Scaffold(
        backgroundColor: Color(0xFFFFF8F1),
        body: Center(
          child: CircularProgressIndicator(color: Color(0xFFE76F51)),
        ),
      );
    }

    if (prato == null) {
      return Scaffold(
        backgroundColor: const Color(0xFFFFF8F1),
        appBar: AppBar(
          backgroundColor: const Color(0xFFE76F51),
          foregroundColor: Colors.white,
          title: const Text('Detalhes do prato'),
        ),
        body: const Center(
          child: Text(
            'Não foi possível carregar o prato.',
            style: TextStyle(color: Color(0xFF9C3D2A)),
          ),
        ),
      );
    }

    return Scaffold(
      backgroundColor: const Color(0xFFFFF8F1),
      appBar: AppBar(
        backgroundColor: const Color(0xFFE76F51),
        foregroundColor: Colors.white,
        elevation: 0,
        centerTitle: true,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: Colors.white),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
        title: const Text(
          '🍽️ Detalhes do Prato',
          style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
        ),
      ),
      body: _buildBody(),
    );
  }

  // ============================================================
  // BODY
  // ============================================================

  Widget _buildBody() {
    return RefreshIndicator(
      color: const Color(0xFFE76F51),
      onRefresh: carregarDados,
      child: SingleChildScrollView(
        physics: const AlwaysScrollableScrollPhysics(),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _buildImagem(),

            Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildAlertaAlergias(),

                  _buildTitulo(),

                  const SizedBox(height: 16),

                  _buildDescricao(),

                  const SizedBox(height: 16),

                  _buildNotaTecnica(),

                  const SizedBox(height: 16),

                  _buildDescricaoIA(),

                  const SizedBox(height: 16),

                  _buildTabelaNutricional(),

                  const SizedBox(height: 16),

                  _buildNota(),

                  const SizedBox(height: 16),

                  _buildFavorito(),

                  const SizedBox(height: 24),

                  _buildAvaliacaoUsuario(),

                  const SizedBox(height: 24),

                  _buildComentarios(),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // IMAGEM
  // ============================================================

  Widget _buildImagem() {
    return ImagemPrato(
      imagem: prato!.imagem,
      height: 250,
      width: double.infinity,
      placeholderColor: Colors.orange.shade100,
      placeholderIcon: Icons.restaurant,
      placeholderIconSize: 80,
    );
  }

  // ============================================================
  // ALERTA DE ALERGIAS DO USUÁRIO
  // ============================================================

  Widget _buildAlertaAlergias() {
    if (prato == null) return const SizedBox.shrink();
    final conflitos = widget.usuario.verificarConflitoAlergias(prato!);
    if (conflitos.isEmpty) return const SizedBox.shrink();

    return Container(
      margin: const EdgeInsets.only(bottom: 16),
      padding: const EdgeInsets.all(14),
      decoration: BoxDecoration(
        color: const Color(0xFFFEF2F2),
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: const Color(0xFFF87171), width: 1.5),
        boxShadow: [
          BoxShadow(
            color: Colors.red.withValues(alpha: 0.08),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Icon(
            Icons.warning_amber_rounded,
            color: Color(0xFFDC2626),
            size: 28,
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const Text(
                  'ATENÇÃO: ALERTA DE ALERGIA!',
                  style: TextStyle(
                    color: Color(0xFF991B1B),
                    fontWeight: FontWeight.bold,
                    fontSize: 15,
                  ),
                ),
                const SizedBox(height: 4),
                Text(
                  'Este prato contém alérgenos que você cadastrou: ${conflitos.join(', ')}.',
                  style: const TextStyle(
                    color: Color(0xFFB91C1C),
                    fontSize: 13,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  // ============================================================
  // BADGE HELPER
  // ============================================================

  Widget _buildBadge({
    required String texto,
    required Color corFundo,
    required Color corTexto,
    Border? border,
  }) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
      decoration: BoxDecoration(
        color: corFundo,
        borderRadius: BorderRadius.circular(14),
        border: border,
      ),
      child: Text(
        texto,
        style: TextStyle(
          color: corTexto,
          fontWeight: FontWeight.bold,
          fontSize: 12,
        ),
      ),
    );
  }

  // ============================================================
  // TÍTULO E BADGES
  // ============================================================

  Widget _buildTitulo() {
    final List<Widget> badges = [];

    // Categoria
    final cat = categorias.firstWhere(
      (c) => c.id == prato!.categoriaId,
      orElse: () => Categoria(
        id: prato!.categoriaId,
        descricao: prato!.categoriaDescricao ?? "Categoria",
      ),
    );
    badges.add(
      _buildBadge(
        texto: "📁 ${cat.descricao}",
        corFundo: Colors.blue.shade50,
        corTexto: Colors.blue.shade800,
        border: Border.all(color: Colors.blue.shade200),
      ),
    );

    // Vegano
    if (prato!.vegano) {
      badges.add(
        _buildBadge(
          texto: "🥦 Vegano",
          corFundo: const Color(0xFFF1F8E9),
          corTexto: Colors.green.shade800,
          border: Border.all(color: Colors.green.shade200),
        ),
      );
    }

    // Alérgenos
    if (prato!.leite) {
      badges.add(
        _buildBadge(
          texto: "🥛 Leite / Lactose",
          corFundo: const Color(0xFFE0F2FE),
          corTexto: const Color(0xFF0369A1),
          border: Border.all(color: const Color(0xFFBAE6FD)),
        ),
      );
    }
    if (prato!.ovo) {
      badges.add(
        _buildBadge(
          texto: "🥚 Ovo",
          corFundo: const Color(0xFFFEF3C7),
          corTexto: const Color(0xFFB45309),
          border: Border.all(color: const Color(0xFFFDE68A)),
        ),
      );
    }
    if (prato!.amendoim) {
      badges.add(
        _buildBadge(
          texto: "🥜 Amendoim",
          corFundo: const Color(0xFFFEF3C7),
          corTexto: const Color(0xFF92400E),
          border: Border.all(color: const Color(0xFFFCD34D)),
        ),
      );
    }
    if (prato!.castanhas) {
      badges.add(
        _buildBadge(
          texto: "🌰 Castanhas",
          corFundo: const Color(0xFFFFEDD5),
          corTexto: const Color(0xFFC2410C),
          border: Border.all(color: const Color(0xFFFED7AA)),
        ),
      );
    }
    if (prato!.frutosDoMar) {
      badges.add(
        _buildBadge(
          texto: "🦐 Frutos do Mar",
          corFundo: const Color(0xFFCFFAFE),
          corTexto: const Color(0xFF0E7490),
          border: Border.all(color: const Color(0xFFA5F3FC)),
        ),
      );
    }
    if (prato!.peixes) {
      badges.add(
        _buildBadge(
          texto: "🐟 Peixes",
          corFundo: const Color(0xFFE0F2FE),
          corTexto: const Color(0xFF0284C7),
          border: Border.all(color: const Color(0xFFBAE6FD)),
        ),
      );
    }
    if (prato!.trigo) {
      badges.add(
        _buildBadge(
          texto: "🌾 Trigo / Glúten",
          corFundo: const Color(0xFFFEF9C3),
          corTexto: const Color(0xFF854D0E),
          border: Border.all(color: const Color(0xFFFEF08A)),
        ),
      );
    }
    if (prato!.soja) {
      badges.add(
        _buildBadge(
          texto: "🌱 Soja",
          corFundo: const Color(0xFFDCFCE7),
          corTexto: const Color(0xFF15803D),
          border: Border.all(color: const Color(0xFFBBF7D0)),
        ),
      );
    }
    if (prato!.gergilim) {
      badges.add(
        _buildBadge(
          texto: "⚪ Gergelim",
          corFundo: const Color(0xFFF3F4F6),
          corTexto: const Color(0xFF374151),
          border: Border.all(color: const Color(0xFFE5E7EB)),
        ),
      );
    }

    // Rótulos Nutricionais (Alto Teor)
    if (prato!.altoAcucarAdicionado) {
      badges.add(
        _buildBadge(
          texto: "⚠️ Alto em Açúcar",
          corFundo: const Color(0xFF18181B),
          corTexto: const Color(0xFFFACC15),
          border: Border.all(color: const Color(0xFF52525B)),
        ),
      );
    }
    if (prato!.altoGorduraSaturada) {
      badges.add(
        _buildBadge(
          texto: "⚠️ Alto em Gord. Sat.",
          corFundo: const Color(0xFF18181B),
          corTexto: const Color(0xFFFACC15),
          border: Border.all(color: const Color(0xFF52525B)),
        ),
      );
    }
    if (prato!.altoSodio) {
      badges.add(
        _buildBadge(
          texto: "⚠️ Alto em Sódio",
          corFundo: const Color(0xFF18181B),
          corTexto: const Color(0xFFFACC15),
          border: Border.all(color: const Color(0xFF52525B)),
        ),
      );
    }

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          prato!.nome,
          style: const TextStyle(
            fontSize: 26,
            fontWeight: FontWeight.bold,
            color: Color(0xFF9C3D2A),
          ),
        ),
        const SizedBox(height: 10),
        Wrap(
          spacing: 8,
          runSpacing: 8,
          children: badges,
        ),
      ],
    );
  }

  // ============================================================
  // DESCRIÇÃO
  // ============================================================

  Widget _buildDescricao() {
    return Card(
      elevation: 2,
      color: Colors.white,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Descrição',
              style: TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: Color(0xFF9C3D2A),
              ),
            ),
            const SizedBox(height: 8),
            Text(
              prato!.descricao,
              style: TextStyle(
                fontSize: 15,
                color: Colors.grey.shade800,
                height: 1.4,
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // NOTA TÉCNICA (FUNCIONÁRIO)
  // ============================================================

  Widget _buildNotaTecnica() {
    if (!widget.usuario.funcionario ||
        prato!.notaTecnica == null ||
        prato!.notaTecnica!.trim().isEmpty) {
      return const SizedBox.shrink();
    }

    return Card(
      elevation: 2,
      color: const Color(0xFFF3E5F5),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: BorderSide(color: Colors.purple.shade200),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: const [
                Icon(Icons.admin_panel_settings, color: Colors.purple),
                SizedBox(width: 8),
                Text(
                  'Nota Técnica (Exclusivo da Equipe)',
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: Colors.purple,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 8),
            Text(
              prato!.notaTecnica!,
              style: const TextStyle(
                fontSize: 14,
                color: Color(0xFF4A148C),
                height: 1.4,
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // DESCRIÇÃO DA IA
  // ============================================================

  Widget _buildDescricaoIA() {
    if (prato!.descricaoIA == null || prato!.descricaoIA!.trim().isEmpty) {
      return const SizedBox.shrink();
    }

    return Card(
      elevation: 2,
      color: const Color(0xFFFFE8D6),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
        side: BorderSide(
          color: const Color(0xFFF4A261).withValues(alpha: 0.5),
        ),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: const [
                Icon(Icons.auto_awesome, color: Color(0xFFE76F51)),
                SizedBox(width: 8),
                Text(
                  'Opinião geral (IA)',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 10),

            Text(
              prato!.descricaoIA!,
              style: const TextStyle(
                fontSize: 15,
                color: Color(0xFF4A3B32),
                height: 1.4,
              ),
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // TABELA NUTRICIONAL
  // ============================================================

  Widget _buildTabelaNutricional() {
    return Card(
      elevation: 2,
      color: Colors.white,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: const [
                Icon(Icons.monitor_weight_outlined, color: Color(0xFFE76F51)),
                SizedBox(width: 8),
                Text(
                  'Informação Nutricional',
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Color(0xFF9C3D2A),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            if (valorNutricional == null)
              const Text(
                'Informações nutricionais ainda não cadastradas para este prato.',
                style: TextStyle(color: Colors.grey, fontSize: 14),
              )
            else ...[
              if (valorNutricional!.medida.isNotEmpty)
                Padding(
                  padding: const EdgeInsets.only(bottom: 12),
                  child: Text(
                    'Porção de referência: ${valorNutricional!.medida}',
                    style: TextStyle(
                      fontSize: 13,
                      fontStyle: FontStyle.italic,
                      color: Colors.grey.shade700,
                    ),
                  ),
                ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceAround,
                children: [
                  _itemNutricional(
                    'Calorias',
                    '${valorNutricional!.kcal.toStringAsFixed(0)} kcal',
                    Colors.deepOrange,
                  ),
                  _itemNutricional(
                    'Carboidratos',
                    '${valorNutricional!.carboidratos.toStringAsFixed(1)} g',
                    Colors.amber.shade800,
                  ),
                  _itemNutricional(
                    'Proteínas',
                    '${valorNutricional!.proteinas.toStringAsFixed(1)} g',
                    Colors.blue.shade700,
                  ),
                  _itemNutricional(
                    'Lipídios',
                    '${valorNutricional!.lipidios.toStringAsFixed(1)} g',
                    Colors.green.shade700,
                  ),
                ],
              ),
            ],
          ],
        ),
      ),
    );
  }

  Widget _itemNutricional(String titulo, String valor, Color cor) {
    return Column(
      children: [
        Text(
          valor,
          style: TextStyle(
            fontSize: 15,
            fontWeight: FontWeight.bold,
            color: cor,
          ),
        ),
        const SizedBox(height: 4),
        Text(
          titulo,
          style: TextStyle(
            fontSize: 12,
            color: Colors.grey.shade600,
          ),
        ),
      ],
    );
  }

  // ============================================================
  // NOTA
  // ============================================================

  Widget _buildNota() {
    final media = calcularMedia();

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
      decoration: BoxDecoration(
        color: const Color(0xFFFFE8D6),
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          const Icon(Icons.star, color: Colors.amber, size: 28),

          const SizedBox(width: 8),

          Text(
            media.toStringAsFixed(1),
            style: const TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Color(0xFF9C3D2A),
            ),
          ),

          const SizedBox(width: 8),

          Text(
            '(${avaliacoes.length} avaliações)',
            style: const TextStyle(
              fontSize: 14,
              color: Color(0xFF4A3B32),
            ),
          ),
        ],
      ),
    );
  }

  // ============================================================
  // FAVORITO
  // ============================================================

  Widget _buildFavorito() {
    return SizedBox(
      width: double.infinity,
      child: OutlinedButton.icon(
        style: OutlinedButton.styleFrom(
          foregroundColor: favorito ? Colors.red : const Color(0xFFE76F51),
          side: BorderSide(
            color: favorito ? Colors.red : const Color(0xFFE76F51),
            width: 1.5,
          ),
          padding: const EdgeInsets.symmetric(vertical: 14),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(14),
          ),
        ),
        onPressed: alternarFavorito,
        icon: Icon(
          favorito ? Icons.favorite : Icons.favorite_border,
          color: favorito ? Colors.red : const Color(0xFFE76F51),
        ),
        label: Text(
          favorito ? 'Remover dos favoritos' : 'Adicionar aos favoritos',
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
      ),
    );
  }

  // ============================================================
  // MINHA AVALIAÇÃO / BOTÃO AVALIAR
  // ============================================================

  Widget _buildAvaliacaoUsuario() {
    if (minhaAvaliacao == null) {
      return SizedBox(
        width: double.infinity,
        child: ElevatedButton.icon(
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color(0xFFE76F51),
            foregroundColor: Colors.white,
            padding: const EdgeInsets.symmetric(vertical: 14),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(14),
            ),
            elevation: 2,
          ),
          onPressed: enviandoAvaliacao
              ? null
              : () {
                  abrirDialogAvaliacao();
                },
          icon: const Icon(Icons.star),
          label: Text(
            enviandoAvaliacao ? 'Enviando...' : 'Avaliar prato',
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
        ),
      );
    }

    return _buildMinhaAvaliacao();
  }

  // ============================================================
  // MINHA AVALIAÇÃO
  // ============================================================

  Widget _buildMinhaAvaliacao() {
    final avaliacao = minhaAvaliacao!;

    return Card(
      elevation: 3,
      color: Colors.white,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(16),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Sua avaliação',
              style: TextStyle(
                fontSize: 18,
                fontWeight: FontWeight.bold,
                color: Color(0xFF9C3D2A),
              ),
            ),

            const SizedBox(height: 8),

            Row(
              children: List.generate(5, (index) {
                return Icon(
                  index < avaliacao.nota ? Icons.star : Icons.star_border,
                  color: Colors.amber,
                );
              }),
            ),

            if (avaliacao.avaliacao != null &&
                avaliacao.avaliacao!.trim().isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text(
                  avaliacao.avaliacao!,
                  style: const TextStyle(fontSize: 15),
                ),
              ),

            const SizedBox(height: 12),

            Row(
              children: [
                Expanded(
                  child: OutlinedButton.icon(
                    style: OutlinedButton.styleFrom(
                      foregroundColor: const Color(0xFFE76F51),
                      side: const BorderSide(color: Color(0xFFE76F51)),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                    ),
                    onPressed: enviandoAvaliacao
                        ? null
                        : () {
                            abrirDialogAvaliacao(avaliacaoExistente: avaliacao);
                          },
                    icon: const Icon(Icons.edit),
                    label: const Text('Editar'),
                  ),
                ),

                const SizedBox(width: 10),

                Expanded(
                  child: OutlinedButton.icon(
                    style: OutlinedButton.styleFrom(
                      foregroundColor: Colors.red,
                      side: const BorderSide(color: Colors.red),
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                    ),
                    onPressed: enviandoAvaliacao
                        ? null
                        : () {
                            confirmarExclusao(avaliacao);
                          },
                    icon: const Icon(Icons.delete_outline),
                    label: const Text('Excluir'),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  // ============================================================
  // COMENTÁRIOS
  // ============================================================

  Widget _buildComentarios() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Text(
          'Comentários',
          style: TextStyle(
            fontSize: 22,
            fontWeight: FontWeight.bold,
            color: Color(0xFF9C3D2A),
          ),
        ),

        const SizedBox(height: 12),

        if (avaliacoes.isEmpty)
          const Text('Ainda não existem avaliações para este prato.'),

        ...avaliacoes.map((avaliacao) => _buildComentario(avaliacao)),
      ],
    );
  }

  // ============================================================
  // COMENTÁRIO INDIVIDUAL
  // ============================================================

  Widget _buildComentario(Avaliacao avaliacao) {
    final bool ehMinha = avaliacao.usuarioId == widget.usuario.id;

    return Card(
      elevation: 2,
      color: Colors.white,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(14),
      ),
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                CircleAvatar(
                  backgroundColor: const Color(0xFFFFE8D6),
                  child: const Icon(Icons.person, color: Color(0xFFE76F51)),
                ),

                const SizedBox(width: 10),

                Expanded(
                  child: Text(
                    ehMinha ? 'Você' : avaliacao.nomeUsuario,
                    style: const TextStyle(
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF9C3D2A),
                    ),
                  ),
                ),
              ],
            ),

            const SizedBox(height: 8),

            Row(
              children: List.generate(5, (index) {
                return Icon(
                  index < avaliacao.nota ? Icons.star : Icons.star_border,
                  size: 18,
                  color: Colors.amber,
                );
              }),
            ),

            if (avaliacao.avaliacao != null &&
                avaliacao.avaliacao!.trim().isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(top: 10),
                child: Text(
                  avaliacao.avaliacao!,
                  style: const TextStyle(fontSize: 15),
                ),
              ),
          ],
        ),
      ),
    );
  }
}
