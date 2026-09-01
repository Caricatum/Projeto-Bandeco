import 'package:tcc_flutter/Class/cardapioClass.dart';
import 'package:tcc_flutter/Class/usuarioClass.dart';

class CardapioDia {
  int? _id;
  DateTime _data;

  Cardapio? _padraoAlmoco;
  Cardapio? _veganoAlmoco;
  Cardapio? _padraoJantar;
  Cardapio? _veganoJantar;

  Usuario? _user;

  CardapioDia({
    int? id,
    required DateTime data,
    Cardapio? padraoAlmoco,
    Cardapio? veganoAlmoco,
    Cardapio? padraoJantar,
    Cardapio? veganoJantar,
    Usuario? user,
  }) : _id = id,
       _data = data,
       _padraoAlmoco = padraoAlmoco,
       _veganoAlmoco = veganoAlmoco,
       _padraoJantar = padraoJantar,
       _veganoJantar = veganoJantar,
       _user = user;

  int? get id => _id;
  set id(int? value) => _id = value;

  DateTime get data => _data;
  set data(DateTime value) => _data = value;

  Cardapio? get padraoAlmoco => _padraoAlmoco;
  set padraoAlmoco(Cardapio? value) => _padraoAlmoco = value;

  Cardapio? get veganoAlmoco => _veganoAlmoco;
  set veganoAlmoco(Cardapio? value) => _veganoAlmoco = value;

  Cardapio? get padraoJantar => _padraoJantar;
  set padraoJantar(Cardapio? value) => _padraoJantar = value;

  Cardapio? get veganoJantar => _veganoJantar;
  set veganoJantar(Cardapio? value) => _veganoJantar = value;

  Usuario? get user => _user;
  set user(Usuario? value) => _user = value;

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

      user: json['user'] != null ? Usuario.fromJson(json['user']) : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      if (_id != null) 'id': _id,

      'data': _data.toIso8601String().split('T').first,

      'padraoAlmoco': _padraoAlmoco != null ? {'id': _padraoAlmoco!.id} : null,

      'veganoAlmoco': _veganoAlmoco != null ? {'id': _veganoAlmoco!.id} : null,

      'padraoJantar': _padraoJantar != null ? {'id': _padraoJantar!.id} : null,

      'veganoJantar': _veganoJantar != null ? {'id': _veganoJantar!.id} : null,

      'user': _user != null ? {'id': _user!.id} : null,
    };
  }
}
