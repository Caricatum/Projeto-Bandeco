import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';
import 'package:http/http.dart' as http;

import '../Class/avisos.dart';
import '../Class/cardapioClass.dart';
import '../Class/cardapioDiaClass.dart';
import '../Class/pratosClass.dart';

enum TipoNotificacao { aviso, pratoFavorito, geral }

class ItemNotificacao {
  final int id;
  final String titulo;
  final String corpo;
  final DateTime dataHora;
  final TipoNotificacao tipo;
  final dynamic dadosExtras;

  ItemNotificacao({
    required this.id,
    required this.titulo,
    required this.corpo,
    required this.dataHora,
    required this.tipo,
    this.dadosExtras,
  });
}

class _PratoRefeicao {
  final Pratos prato;
  final String refeicao;
  final String categoria;

  _PratoRefeicao(this.prato, this.refeicao, this.categoria);
}

class NotificacaoService {
  static final NotificacaoService instance = NotificacaoService._internal();

  NotificacaoService._internal();

  final FlutterLocalNotificationsPlugin _notificationsPlugin =
      FlutterLocalNotificationsPlugin();

  bool _inicializado = false;
  final List<ItemNotificacao> _historico = [];
  final ValueNotifier<int> naoLidasNotifier = ValueNotifier<int>(0);

  final Set<int> _avisosNotificados = {};
  final Set<String> _pratosFavoritosNotificados = {};
  String _ultimaDataCardapioVerificada = '';

  List<Pratos> pratosFavoritosHoje = [];

  List<ItemNotificacao> get historico => List.unmodifiable(_historico);

  Future<void> inicializar() async {
    if (_inicializado) return;

    try {
      const AndroidInitializationSettings androidSettings =
          AndroidInitializationSettings('@mipmap/ic_launcher');

      const DarwinInitializationSettings darwinSettings =
          DarwinInitializationSettings(
        requestAlertPermission: true,
        requestBadgePermission: true,
        requestSoundPermission: true,
      );

      const LinuxInitializationSettings linuxSettings =
          LinuxInitializationSettings(defaultActionName: 'Open');

      const InitializationSettings initSettings = InitializationSettings(
        android: androidSettings,
        iOS: darwinSettings,
        macOS: darwinSettings,
        linux: linuxSettings,
      );

      await _notificationsPlugin.initialize(
        settings: initSettings,
        onDidReceiveNotificationResponse: (NotificationResponse response) {
          debugPrint('Notificação tocada: ${response.payload}');
        },
      );

      // Solicitar permissão no Android 13+
      if (!kIsWeb && defaultTargetPlatform == TargetPlatform.android) {
        final androidImplementation = _notificationsPlugin
            .resolvePlatformSpecificImplementation<
                AndroidFlutterLocalNotificationsPlugin>();
        await androidImplementation?.requestNotificationsPermission();
      }

      _inicializado = true;
    } catch (e) {
      debugPrint('Erro ao inicializar NotificacaoService: $e');
    }
  }

  Future<void> mostrarNotificacao({
    required int id,
    required String titulo,
    required String corpo,
    String? payload,
    TipoNotificacao tipo = TipoNotificacao.geral,
    dynamic dadosExtras,
  }) async {
    final item = ItemNotificacao(
      id: id,
      titulo: titulo,
      corpo: corpo,
      dataHora: DateTime.now(),
      tipo: tipo,
      dadosExtras: dadosExtras,
    );

    _historico.insert(0, item);
    naoLidasNotifier.value++;

    try {
      const AndroidNotificationDetails androidDetails =
          AndroidNotificationDetails(
        'bandeco_notifications_channel',
        'Notificações RU Cotil',
        channelDescription:
            'Alertas de novos avisos e pratos favoritos no cardápio',
        importance: Importance.max,
        priority: Priority.high,
        showWhen: true,
        icon: '@mipmap/ic_launcher',
      );

      const DarwinNotificationDetails darwinDetails = DarwinNotificationDetails(
        presentAlert: true,
        presentBadge: true,
        presentSound: true,
      );

      const NotificationDetails platformDetails = NotificationDetails(
        android: androidDetails,
        iOS: darwinDetails,
        macOS: darwinDetails,
      );

      await _notificationsPlugin.show(
        id: id,
        title: titulo,
        body: corpo,
        notificationDetails: platformDetails,
        payload: payload ?? '$id',
      );
    } catch (e) {
      debugPrint('Erro ao exibir notificação nativa: $e');
    }
  }

  Future<void> notificarNovoAviso(
    Avisos aviso, {
    bool verificarDuplicado = true,
  }) async {
    if (verificarDuplicado && _avisosNotificados.contains(aviso.id)) {
      return;
    }

    _avisosNotificados.add(aviso.id);

    final String corpo =
        (aviso.descricao != null && aviso.descricao!.trim().isNotEmpty)
            ? aviso.descricao!
            : 'Novo aviso publicado no Mural do RU.';

    await mostrarNotificacao(
      id: aviso.id,
      titulo: '📢 Novo Aviso: ${aviso.titulo}',
      corpo: corpo,
      payload: 'aviso_${aviso.id}',
      tipo: TipoNotificacao.aviso,
      dadosExtras: aviso,
    );
  }

  Future<void> verificarNovosAvisos(
    List<Avisos> avisos, {
    bool primeiraCarga = false,
  }) async {
    if (avisos.isEmpty) return;

    if (primeiraCarga && _avisosNotificados.isEmpty) {
      for (final a in avisos) {
        _avisosNotificados.add(a.id);
      }
      return;
    }

    for (final aviso in avisos) {
      if (!_avisosNotificados.contains(aviso.id)) {
        await notificarNovoAviso(aviso, verificarDuplicado: true);
      }
    }
  }

  Future<List<Map<String, dynamic>>> verificarCardapioHojeEFavoritos({
    required int usuarioId,
    String baseUrl = 'http://localhost:8080',
  }) async {
    final String dataHoje = DateTime.now().toIso8601String().split('T').first;

    if (_ultimaDataCardapioVerificada != dataHoje) {
      _pratosFavoritosNotificados.clear();
      pratosFavoritosHoje.clear();
      _ultimaDataCardapioVerificada = dataHoje;
    }

    final List<Map<String, dynamic>> correspondencias = [];

    try {
      final favUrl = Uri.parse('$baseUrl/pratosFavoritos/all');
      final favResponse = await http.get(favUrl);

      if (favResponse.statusCode != 200) {
        return correspondencias;
      }

      final List<dynamic> favDados = jsonDecode(favResponse.body);
      final List<Pratos> favoritosDoUsuario = [];

      for (final item in favDados) {
        if (item is Map &&
            item['user'] is Map &&
            item['user']['id'] == usuarioId &&
            item['prato'] != null) {
          favoritosDoUsuario.add(Pratos.fromJson(item['prato']));
        }
      }

      if (favoritosDoUsuario.isEmpty) {
        return correspondencias;
      }

      final menuUrl = Uri.parse('$baseUrl/cardapioDia/data/$dataHoje');
      final menuResponse = await http.get(menuUrl);

      if (menuResponse.statusCode != 200) {
        return correspondencias;
      }

      final cardapioHoje = CardapioDia.fromJson(jsonDecode(menuResponse.body));

      final List<_PratoRefeicao> pratosHoje = [];

      void extrair(Cardapio? c, String refeicao) {
        if (c == null) return;
        if (c.pratoPrincipal != null) {
          pratosHoje.add(
            _PratoRefeicao(c.pratoPrincipal!, refeicao, 'Prato Principal'),
          );
        }
        if (c.acompanhamento != null) {
          pratosHoje.add(
            _PratoRefeicao(c.acompanhamento!, refeicao, 'Acompanhamento'),
          );
        }
        if (c.guarnicao != null) {
          pratosHoje.add(
            _PratoRefeicao(c.guarnicao!, refeicao, 'Guarnição'),
          );
        }
        if (c.salada != null) {
          pratosHoje.add(
            _PratoRefeicao(c.salada!, refeicao, 'Salada'),
          );
        }
        if (c.sobremesa != null) {
          pratosHoje.add(
            _PratoRefeicao(c.sobremesa!, refeicao, 'Sobremesa'),
          );
        }
        if (c.refresco != null) {
          pratosHoje.add(
            _PratoRefeicao(c.refresco!, refeicao, 'Refresco'),
          );
        }
      }

      extrair(cardapioHoje.padraoAlmoco, 'Almoço Padrão');
      extrair(cardapioHoje.veganoAlmoco, 'Almoço Vegano');
      extrair(cardapioHoje.padraoJantar, 'Jantar Padrão');
      extrair(cardapioHoje.veganoJantar, 'Jantar Vegano');

      final List<Pratos> favoritosEncontrados = [];

      for (final pRef in pratosHoje) {
        for (final fav in favoritosDoUsuario) {
          final bool mesmoId = fav.id != 0 && fav.id == pRef.prato.id;
          final bool mesmoNome = fav.nome.trim().isNotEmpty &&
              fav.nome.trim().toLowerCase() ==
                  pRef.prato.nome.trim().toLowerCase();

          if (mesmoId || mesmoNome) {
            favoritosEncontrados.add(pRef.prato);
            correspondencias.add({
              'prato': pRef.prato,
              'refeicao': pRef.refeicao,
              'categoria': pRef.categoria,
            });

            final chaveNotificacao =
                '${dataHoje}_${fav.id != 0 ? fav.id : fav.nome}_${pRef.refeicao}';

            if (!_pratosFavoritosNotificados.contains(chaveNotificacao)) {
              _pratosFavoritosNotificados.add(chaveNotificacao);

              final int notifId =
                  (fav.id * 1000 + pRef.refeicao.hashCode).abs() % 100000;

              await mostrarNotificacao(
                id: notifId,
                titulo: '🍽️ Prato Favorito Hoje!',
                corpo:
                    'Seu prato favorito "${pRef.prato.nome}" está no cardápio de hoje (${pRef.refeicao})!',
                payload: 'prato_${pRef.prato.id}',
                tipo: TipoNotificacao.pratoFavorito,
                dadosExtras: pRef.prato,
              );
            }
          }
        }
      }

      pratosFavoritosHoje = favoritosEncontrados;
    } catch (e) {
      debugPrint('Erro ao verificar favoritos no cardápio de hoje: $e');
    }

    return correspondencias;
  }

  void limparNaoLidas() {
    naoLidasNotifier.value = 0;
  }

  void limparHistorico() {
    _historico.clear();
    naoLidasNotifier.value = 0;
  }
}
