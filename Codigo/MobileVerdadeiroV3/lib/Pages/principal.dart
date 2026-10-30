import 'dart:async';
import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import 'CardapioReferent/cardapioDia.dart';
import 'UserReferent/perfil.dart';
import 'menuNavegacao.dart';

import '../Class/usuarioClass.dart';
import '../Class/avisos.dart';
import '../Services/notificacaoService.dart';

class Principal extends StatefulWidget {
  final Usuario usuario;

  const Principal({super.key, required this.usuario});

  @override
  State<Principal> createState() => _PrincipalState();
}

class _PrincipalState extends State<Principal> {
  // ==================== STATE VARIABLES ====================
  final String baseUrl = 'http://localhost:8080';
  List<Avisos> avisos = [];
  bool carregando = true;
  bool _primeiraCargaAvisos = true;
  Timer? _timerVerificacao;
  List<Map<String, dynamic>> _favoritosHoje = [];

  // ==================== LIFECYCLE ====================
  @override
  void initState() {
    super.initState();
    _buscarAvisos();
    _verificarFavoritosNoCardapio();

    // Verificação periódica automática de novos avisos e cardápio
    _timerVerificacao = Timer.periodic(const Duration(seconds: 40), (_) {
      if (mounted) {
        _buscarAvisosSilencioso();
        _verificarFavoritosNoCardapio();
      }
    });
  }

  @override
  void dispose() {
    _timerVerificacao?.cancel();
    super.dispose();
  }

  // ==================== API METHODS ====================
  Future<void> _buscarAvisos() async {
    setState(() {
      carregando = true;
    });

    try {
      final url = Uri.parse('$baseUrl/avisos/all');

      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);
        final lista = dados.map((json) => Avisos.fromJson(json)).toList();

        setState(() {
          avisos = lista;
          carregando = false;
        });

        await NotificacaoService.instance.verificarNovosAvisos(
          lista,
          primeiraCarga: _primeiraCargaAvisos,
        );
        _primeiraCargaAvisos = false;
      } else {
        setState(() {
          carregando = false;
        });

        _mostrarMensagem('Erro ao carregar os avisos.');
      }
    } catch (e) {
      setState(() {
        carregando = false;
      });

      _mostrarMensagem('Não foi possível conectar à API.');
    }
  }

  Future<void> _buscarAvisosSilencioso() async {
    try {
      final url = Uri.parse('$baseUrl/avisos/all');
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final List<dynamic> dados = jsonDecode(response.body);
        final lista = dados.map((json) => Avisos.fromJson(json)).toList();

        if (mounted) {
          setState(() {
            avisos = lista;
          });
        }

        await NotificacaoService.instance.verificarNovosAvisos(
          lista,
          primeiraCarga: _primeiraCargaAvisos,
        );
        _primeiraCargaAvisos = false;
      }
    } catch (_) {
      // Ignorar falhas silenciosas de polling em background
    }
  }

  Future<void> _verificarFavoritosNoCardapio() async {
    final matches = await NotificacaoService.instance
        .verificarCardapioHojeEFavoritos(
          usuarioId: widget.usuario.id,
          baseUrl: baseUrl,
        );

    if (mounted) {
      setState(() {
        _favoritosHoje = matches;
      });
    }
  }

  Future<void> _adicionarAviso({
    required String titulo,
    required String descricao,
    required String model,
    required DateTime dataInicial,
    required DateTime dataFinal,
  }) async {
    try {
      final url = Uri.parse('$baseUrl/avisos/cadastrar');

      final response = await http.post(
        url,
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'titulo': titulo,
          'descricao': descricao,
          'model': model,
          'data_inicial': dataInicial.toIso8601String().split('T')[0],
          'data_final': dataFinal.toIso8601String().split('T')[0],
          'user': {'id': widget.usuario.id},
        }),
      );

      if (response.statusCode == 200 || response.statusCode == 201) {
        final dados = jsonDecode(response.body);

        final novoAviso = Avisos.fromJson(dados);

        setState(() {
          avisos.insert(0, novoAviso);
        });

        // Notificar imediatamente o novo aviso criado
        await NotificacaoService.instance.notificarNovoAviso(
          novoAviso,
          verificarDuplicado: false,
        );

        _mostrarMensagem('Aviso cadastrado com sucesso!');
      } else {
        _mostrarMensagem(
          'Erro ao cadastrar aviso.\n'
          'Código: ${response.statusCode}',
        );
      }
    } catch (e) {
      _mostrarMensagem('Não foi possível conectar à API.');
    }
  }

  Future<void> _deletarAviso(Avisos aviso) async {
    try {
      final url = Uri.parse('$baseUrl/avisos/deletar/${aviso.id}');

      final response = await http.delete(url);

      if (response.statusCode == 200 || response.statusCode == 204) {
        setState(() {
          avisos.removeWhere((item) => item.id == aviso.id);
        });

        _mostrarMensagem('Aviso excluído com sucesso!');
      } else {
        _mostrarMensagem('Erro ao excluir aviso.');
      }
    } catch (e) {
      _mostrarMensagem('Não foi possível conectar à API.');
    }
  }

  // ==================== DIALOGS & USER INTERACTIONS ====================
  void _confirmarExclusao(Avisos aviso) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Excluir aviso'),

          content: Text(
            'Deseja realmente excluir o aviso '
            '"${aviso.titulo}"?',
          ),

          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(context);
              },
              child: const Text('Cancelar'),
            ),

            ElevatedButton(
              onPressed: () {
                Navigator.pop(context);

                _deletarAviso(aviso);
              },
              child: const Text('Excluir'),
            ),
          ],
        );
      },
    );
  }

  void _mostrarDialogoAdicionarAviso() {
    final tituloController = TextEditingController();

    final descricaoController = TextEditingController();

    final modelController = TextEditingController();

    DateTime? dataInicial;
    DateTime? dataFinal;

    showDialog(
      context: context,
      builder: (dialogContext) {
        return StatefulBuilder(
          builder: (context, setDialogState) {
            return AlertDialog(
              title: const Text('Novo aviso'),

              content: SingleChildScrollView(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    TextField(
                      controller: tituloController,
                      decoration: const InputDecoration(labelText: 'Título'),
                    ),

                    const SizedBox(height: 12),

                    TextField(
                      controller: descricaoController,
                      maxLines: 3,
                      decoration: const InputDecoration(labelText: 'Descrição'),
                    ),

                    const SizedBox(height: 12),

                    TextField(
                      controller: modelController,
                      decoration: const InputDecoration(labelText: 'Modelo'),
                    ),

                    const SizedBox(height: 16),

                    // DATA INICIAL
                    ListTile(
                      contentPadding: EdgeInsets.zero,

                      leading: const Icon(Icons.calendar_today),

                      title: Text(
                        dataInicial == null
                            ? 'Data inicial'
                            : 'Data inicial: '
                                  '${_formatarData(dataInicial!)}',
                      ),

                      onTap: () async {
                        final data = await showDatePicker(
                          context: context,
                          initialDate: DateTime.now(),
                          firstDate: DateTime(2020),
                          lastDate: DateTime(2100),
                        );

                        if (data != null) {
                          setDialogState(() {
                            dataInicial = data;
                          });
                        }
                      },
                    ),

                    // DATA FINAL
                    ListTile(
                      contentPadding: EdgeInsets.zero,

                      leading: const Icon(Icons.calendar_today),

                      title: Text(
                        dataFinal == null
                            ? 'Data final'
                            : 'Data final: '
                                  '${_formatarData(dataFinal!)}',
                      ),

                      onTap: () async {
                        final data = await showDatePicker(
                          context: context,
                          initialDate: dataInicial ?? DateTime.now(),
                          firstDate: DateTime(2020),
                          lastDate: DateTime(2100),
                        );

                        if (data != null) {
                          setDialogState(() {
                            dataFinal = data;
                          });
                        }
                      },
                    ),
                  ],
                ),
              ),

              actions: [
                TextButton(
                  onPressed: () {
                    Navigator.pop(dialogContext);
                  },
                  child: const Text('Cancelar'),
                ),

                ElevatedButton(
                  onPressed: () async {
                    final titulo = tituloController.text.trim();

                    final descricao = descricaoController.text.trim();

                    final model = modelController.text.trim();

                    if (titulo.isEmpty ||
                        model.isEmpty ||
                        dataInicial == null ||
                        dataFinal == null) {
                      _mostrarMensagem(
                        'Preencha todos os campos obrigatórios.',
                      );

                      return;
                    }

                    if (dataFinal!.isBefore(dataInicial!)) {
                      _mostrarMensagem(
                        'A data final não pode ser anterior à data inicial.',
                      );

                      return;
                    }

                    Navigator.pop(dialogContext);

                    await _adicionarAviso(
                      titulo: titulo,
                      descricao: descricao,
                      model: model,
                      dataInicial: dataInicial!,
                      dataFinal: dataFinal!,
                    );
                  },
                  child: const Text('Adicionar'),
                ),
              ],
            );
          },
        );
      },
    );
  }

  // ==================== HELPER METHODS ====================
  String _formatarData(DateTime data) {
    final dia = data.day.toString().padLeft(2, '0');

    final mes = data.month.toString().padLeft(2, '0');

    final ano = data.year.toString();

    return '$dia/$mes/$ano';
  }

  void _mostrarMensagem(String mensagem) {
    if (!mounted) return;

    ScaffoldMessenger.of(
      context,
    ).showSnackBar(SnackBar(content: Text(mensagem)));
  }

  void _abrirCentralNotificacoes() {
    NotificacaoService.instance.limparNaoLidas();

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (bottomContext) {
        return StatefulBuilder(
          builder: (context, setModalState) {
            final lista = NotificacaoService.instance.historico;

            return Container(
              height: MediaQuery.of(context).size.height * 0.75,
              decoration: const BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.vertical(top: Radius.circular(24)),
              ),
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
              child: Column(
                children: [
                  Container(
                    width: 40,
                    height: 4,
                    decoration: BoxDecoration(
                      color: Colors.grey.shade300,
                      borderRadius: BorderRadius.circular(2),
                    ),
                  ),
                  const SizedBox(height: 12),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Row(
                        children: [
                          Icon(
                            Icons.notifications_active,
                            color: Colors.orange,
                          ),
                          SizedBox(width: 8),
                          Text(
                            'Central de Notificações',
                            style: TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ],
                      ),
                      if (lista.isNotEmpty)
                        TextButton.icon(
                          onPressed: () {
                            NotificacaoService.instance.limparHistorico();
                            setModalState(() {});
                          },
                          icon: const Icon(Icons.clear_all, size: 18),
                          label: const Text('Limpar'),
                        ),
                    ],
                  ),
                  const Divider(),
                  // AÇÃO DE VERIFICAÇÃO MANUAL
                  SingleChildScrollView(
                    scrollDirection: Axis.horizontal,
                    child: Row(
                      children: [
                        OutlinedButton.icon(
                          icon: const Icon(Icons.sync, size: 16),
                          label: const Text('Verificar Agora'),
                          onPressed: () async {
                            await _buscarAvisosSilencioso();
                            await _verificarFavoritosNoCardapio();
                            setModalState(() {});
                            if (context.mounted) {
                              ScaffoldMessenger.of(context).showSnackBar(
                                const SnackBar(
                                  content: Text(
                                    'Verificação de avisos e cardápio concluída.',
                                  ),
                                  duration: Duration(seconds: 2),
                                ),
                              );
                            }
                          },
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(height: 10),
                  Expanded(
                    child: lista.isEmpty
                        ? const Center(
                            child: Column(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Icon(
                                  Icons.notifications_none,
                                  size: 56,
                                  color: Colors.grey,
                                ),
                                SizedBox(height: 12),
                                Text(
                                  'Nenhuma notificação registrada ainda.',
                                  style: TextStyle(
                                    color: Colors.grey,
                                    fontSize: 15,
                                  ),
                                ),
                                SizedBox(height: 4),
                                Text(
                                  'Novos avisos e pratos favoritos aparecerão aqui.',
                                  style: TextStyle(
                                    color: Colors.grey,
                                    fontSize: 12,
                                  ),
                                ),
                              ],
                            ),
                          )
                        : ListView.separated(
                            itemCount: lista.length,
                            separatorBuilder: (context, index) =>
                                const Divider(height: 1),
                            itemBuilder: (context, index) {
                              final item = lista[index];
                              final isAviso =
                                  item.tipo == TipoNotificacao.aviso;
                              final hora =
                                  '${item.dataHora.hour.toString().padLeft(2, '0')}:${item.dataHora.minute.toString().padLeft(2, '0')}';

                              return ListTile(
                                contentPadding: const EdgeInsets.symmetric(
                                  horizontal: 4,
                                  vertical: 4,
                                ),
                                leading: CircleAvatar(
                                  backgroundColor: isAviso
                                      ? Colors.orange.shade100
                                      : Colors.red.shade100,
                                  child: Icon(
                                    isAviso ? Icons.campaign : Icons.favorite,
                                    color: isAviso ? Colors.orange : Colors.red,
                                  ),
                                ),
                                title: Text(
                                  item.titulo,
                                  style: const TextStyle(
                                    fontWeight: FontWeight.bold,
                                    fontSize: 14,
                                  ),
                                ),
                                subtitle: Text(
                                  item.corpo,
                                  style: const TextStyle(fontSize: 13),
                                ),
                                trailing: Text(
                                  hora,
                                  style: const TextStyle(
                                    color: Colors.grey,
                                    fontSize: 11,
                                  ),
                                ),
                              );
                            },
                          ),
                  ),
                ],
              ),
            );
          },
        );
      },
    );
  }

  // ==================== UI BUILD METHODS ====================
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F5F5),

      appBar: AppBar(
        elevation: 0,
        backgroundColor: Colors.orange,

        title: Column(
          crossAxisAlignment: CrossAxisAlignment.start,

          children: [
            Text(
              'Olá ${widget.usuario.nome}!',
              style: const TextStyle(fontSize: 14, fontWeight: FontWeight.w400),
            ),

            const Text(
              'Bem-vindo ao RU',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
            ),
          ],
        ),

        actions: [
          // BOTÃO CENTRAL DE NOTIFICAÇÕES COM BADGE
          ValueListenableBuilder<int>(
            valueListenable: NotificacaoService.instance.naoLidasNotifier,
            builder: (context, totalNaoLidas, _) {
              return Stack(
                alignment: Alignment.center,
                children: [
                  IconButton(
                    icon: const Icon(Icons.notifications_outlined, size: 28),
                    tooltip: 'Notificações',
                    onPressed: _abrirCentralNotificacoes,
                  ),
                  if (totalNaoLidas > 0)
                    Positioned(
                      right: 6,
                      top: 8,
                      child: Container(
                        padding: const EdgeInsets.all(3),
                        decoration: const BoxDecoration(
                          color: Colors.red,
                          shape: BoxShape.circle,
                        ),
                        constraints: const BoxConstraints(
                          minWidth: 18,
                          minHeight: 18,
                        ),
                        child: Text(
                          totalNaoLidas > 9 ? '9+' : '$totalNaoLidas',
                          style: const TextStyle(
                            color: Colors.white,
                            fontSize: 10,
                            fontWeight: FontWeight.bold,
                          ),
                          textAlign: TextAlign.center,
                        ),
                      ),
                    ),
                ],
              );
            },
          ),
          Padding(
            padding: const EdgeInsets.only(right: 8),

            child: IconButton(
              icon: const Icon(Icons.account_circle, size: 32),

              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => Perfil(usuario: widget.usuario),
                  ),
                );
              },
            ),
          ),
        ],
      ),

      drawer: MenuNavegacao(usuario: widget.usuario),

      body: Padding(
        padding: const EdgeInsets.all(16),

        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,

          children: [
            Container(
              width: double.infinity,

              padding: const EdgeInsets.all(16),

              decoration: BoxDecoration(
                color: Colors.orange,
                borderRadius: BorderRadius.circular(20),
              ),

              child: const Column(
                crossAxisAlignment: CrossAxisAlignment.start,

                children: [
                  Text(
                    '🍽 Restaurante Universitário',

                    style: TextStyle(
                      color: Colors.white,
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                    ),
                  ),

                  SizedBox(height: 8),

                  Text(
                    'Confira avisos, cardápio e horários atualizados.',

                    style: TextStyle(color: Colors.white70),
                  ),
                ],
              ),
            ),

            // BANNER DE PRATO FAVORITO NO CARDÁPIO DE HOJE
            if (_favoritosHoje.isNotEmpty) ...[
              const SizedBox(height: 16),
              Container(
                width: double.infinity,
                padding: const EdgeInsets.all(14),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [
                      Colors.orange.shade700,
                      Colors.deepOrange.shade600,
                    ],
                  ),
                  borderRadius: BorderRadius.circular(16),
                  boxShadow: [
                    BoxShadow(
                      color: Colors.deepOrange.withValues(alpha: 0.25),
                      blurRadius: 8,
                      offset: const Offset(0, 3),
                    ),
                  ],
                ),
                child: InkWell(
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) =>
                            CardapioMostra(usuario: widget.usuario),
                      ),
                    );
                  },
                  child: Row(
                    children: [
                      const CircleAvatar(
                        backgroundColor: Colors.white,
                        radius: 20,
                        child: Icon(Icons.favorite, color: Colors.deepOrange),
                      ),
                      const SizedBox(width: 12),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const Text(
                              '🍽️ Seu favorito está no cardápio de hoje!',
                              style: TextStyle(
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                                fontSize: 14,
                              ),
                            ),
                            const SizedBox(height: 4),
                            Text(
                              _favoritosHoje
                                  .map(
                                    (f) =>
                                        '${f['prato'].nome} (${f['refeicao']})',
                                  )
                                  .join(', '),
                              style: const TextStyle(
                                color: Colors.white,
                                fontSize: 12,
                              ),
                              maxLines: 2,
                              overflow: TextOverflow.ellipsis,
                            ),
                          ],
                        ),
                      ),
                      const Icon(
                        Icons.arrow_forward_ios,
                        color: Colors.white,
                        size: 16,
                      ),
                    ],
                  ),
                ),
              ),
            ],

            const SizedBox(height: 24),
            const Row(
              children: [
                Icon(Icons.campaign, color: Colors.orange),

                SizedBox(width: 8),

                Text(
                  'Mural de Avisos',

                  style: TextStyle(fontSize: 22, fontWeight: FontWeight.bold),
                ),
              ],
            ),

            const SizedBox(height: 16),
            Expanded(child: _buildListaAvisos()),
          ],
        ),
      ),

      floatingActionButton: FloatingActionButton.extended(
        backgroundColor: Colors.orange,

        onPressed: _mostrarDialogoAdicionarAviso,

        icon: const Icon(Icons.add),

        label: const Text('Novo Aviso'),
      ),
    );
  }

  Widget _buildListaAvisos() {
    if (carregando) {
      return const Center(child: CircularProgressIndicator());
    }

    if (avisos.isEmpty) {
      return const Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,

          children: [
            Icon(
              Icons.notifications_off_outlined,
              size: 70,
              color: Colors.grey,
            ),

            SizedBox(height: 12),

            Text(
              'Nenhum aviso disponível',

              style: TextStyle(fontSize: 16, color: Colors.grey),
            ),
          ],
        ),
      );
    }

    return RefreshIndicator(
      onRefresh: _buscarAvisos,

      child: ListView.builder(
        itemCount: avisos.length,

        itemBuilder: (context, index) {
          final aviso = avisos[index];

          return Card(
            elevation: 3,

            margin: const EdgeInsets.only(bottom: 12),

            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(16),
            ),

            child: Padding(
              padding: const EdgeInsets.all(16),

              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,

                children: [
                  // TÍTULO + LIXEIRA
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,

                    children: [
                      Container(
                        padding: const EdgeInsets.all(10),

                        decoration: BoxDecoration(
                          color: Colors.orange.shade100,

                          borderRadius: BorderRadius.circular(12),
                        ),

                        child: const Icon(
                          Icons.announcement,
                          color: Colors.orange,
                        ),
                      ),

                      const SizedBox(width: 12),

                      Expanded(
                        child: Text(
                          aviso.titulo,

                          style: const TextStyle(
                            fontSize: 18,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                      ),

                      IconButton(
                        icon: const Icon(
                          Icons.delete_outline,
                          color: Colors.red,
                        ),

                        onPressed: () {
                          _confirmarExclusao(aviso);
                        },
                      ),
                    ],
                  ),

                  const SizedBox(height: 12),

                  // DESCRIÇÃO
                  if (aviso.descricao != null && aviso.descricao!.isNotEmpty)
                    Text(
                      aviso.descricao!,

                      style: const TextStyle(fontSize: 15),
                    ),

                  const SizedBox(height: 10),

                  // MODELO
                  Text(
                    'Modelo: ${aviso.model}',

                    style: const TextStyle(color: Colors.grey, fontSize: 13),
                  ),

                  const SizedBox(height: 6),

                  // DATAS
                  Row(
                    children: [
                      const Icon(
                        Icons.calendar_today,
                        size: 16,
                        color: Colors.grey,
                      ),

                      const SizedBox(width: 6),

                      Text(
                        '${_formatarData(aviso.dataInicial)}'
                        ' até '
                        '${_formatarData(aviso.dataFinal)}',

                        style: const TextStyle(
                          color: Colors.grey,
                          fontSize: 13,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
