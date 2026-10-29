import 'package:flutter/material.dart';
import 'package:tcc_flutter/Class/pratosClass.dart';

class Cardapio {
  int? _id;

  Pratos? _acompanhamento;
  Pratos? _pratoPrincipal;
  Pratos? _guarnicao;
  Pratos? _salada;
  Pratos? _sobremesa;
  Pratos? _refresco;

  bool _vegano;

  Cardapio({
    int? id,
    Pratos? acompanhamento,
    Pratos? pratoPrincipal,
    Pratos? guarnicao,
    Pratos? salada,
    Pratos? sobremesa,
    Pratos? refresco,
    bool vegano = false,
  })  : _id = id,
        _acompanhamento = acompanhamento,
        _pratoPrincipal = pratoPrincipal,
        _guarnicao = guarnicao,
        _salada = salada,
        _sobremesa = sobremesa,
        _refresco = refresco,
        _vegano = vegano;

  int? get id => _id;
  set id(int? value) => _id = value;

  Pratos? get acompanhamento => _acompanhamento;
  set acompanhamento(Pratos? value) => _acompanhamento = value;

  Pratos? get pratoPrincipal => _pratoPrincipal;
  set pratoPrincipal(Pratos? value) => _pratoPrincipal = value;

  Pratos? get guarnicao => _guarnicao;
  set guarnicao(Pratos? value) => _guarnicao = value;

  Pratos? get salada => _salada;
  set salada(Pratos? value) => _salada = value;

  Pratos? get sobremesa => _sobremesa;
  set sobremesa(Pratos? value) => _sobremesa = value;

  Pratos? get refresco => _refresco;
  set refresco(Pratos? value) => _refresco = value;

  bool get vegano => _vegano;
  set vegano(bool value) => _vegano = value;

  factory Cardapio.fromJson(Map<String, dynamic> json) {
    return Cardapio(
      id: json['id'],

      acompanhamento: json['acompanhamento'] != null
          ? Pratos.fromJson(json['acompanhamento'])
          : null,

      pratoPrincipal: json['prato_principal'] != null
          ? Pratos.fromJson(json['prato_principal'])
          : null,

      guarnicao: json['guarnicao'] != null
          ? Pratos.fromJson(json['guarnicao'])
          : null,

      salada: json['salada'] != null
          ? Pratos.fromJson(json['salada'])
          : null,

      sobremesa: json['sobremesa'] != null
          ? Pratos.fromJson(json['sobremesa'])
          : null,

      refresco: json['refresco'] != null
          ? Pratos.fromJson(json['refresco'])
          : null,

      vegano: json['vegano'] ?? false,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      if (_id != null)
        'id': _id,

      'acompanhamento': _acompanhamento != null
          ? {'id': _acompanhamento!.id}
          : null,

      'prato_principal': _pratoPrincipal != null
          ? {'id': _pratoPrincipal!.id}
          : null,

      'guarnicao': _guarnicao != null
          ? {'id': _guarnicao!.id}
          : null,

      'salada': _salada != null
          ? {'id': _salada!.id}
          : null,

      'sobremesa': _sobremesa != null
          ? {'id': _sobremesa!.id}
          : null,

      'refresco': _refresco != null
          ? {'id': _refresco!.id}
          : null,

      'vegano': _vegano,
    };
  }
}