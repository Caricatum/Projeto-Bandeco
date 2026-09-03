import 'package:tcc_flutter/Class/cardapioClass.dart';

class CardapioDia {
  int? id;
  DateTime data;
  Cardapio? padraoAlmoco;
  Cardapio? veganoAlmoco;
  Cardapio? padraoJantar;
  Cardapio? veganoJantar;
  int userId;

  CardapioDia({
    this.id,
    required this.data,
    this.padraoAlmoco,
    this.veganoAlmoco,
    this.padraoJantar,
    this.veganoJantar,
    required this.userId,
  });

  Map<String, dynamic> toJson() {
    return {
      if (id != null) 'id': id,

      'data':
          '${data.year.toString().padLeft(4, '0')}-'
          '${data.month.toString().padLeft(2, '0')}-'
          '${data.day.toString().padLeft(2, '0')}',

      'padraoAlmoco': padraoAlmoco != null
          ? {'id': padraoAlmoco!.id}
          : null,

      'veganoAlmoco': veganoAlmoco != null
          ? {'id': veganoAlmoco!.id}
          : null,

      'padraoJantar': padraoJantar != null
          ? {'id': padraoJantar!.id}
          : null,

      'veganoJantar': veganoJantar != null
          ? {'id': veganoJantar!.id}
          : null,

      'user': {
        'id': userId,
      },
    };
  }

  factory CardapioDia.fromJson(Map<String, dynamic> json) {
    return CardapioDia(
      id: json['id'],
      data: DateTime.parse(json['data']),
      padraoAlmoco: json['padraoAlmoco'] != null
          ? Cardapio.fromJson(json['padraoAlmoco'])
          : null,
      veganoAlmoco: json['veganoAlmoco'] != null
          ? Cardapio.fromJson(json['veganoAlmoco'])
          : null,
      padraoJantar: json['padraoJantar'] != null
          ? Cardapio.fromJson(json['padraoJantar'])
          : null,
      veganoJantar: json['veganoJantar'] != null
          ? Cardapio.fromJson(json['veganoJantar'])
          : null,
      userId: json['user']['id'],
    );
  }
}