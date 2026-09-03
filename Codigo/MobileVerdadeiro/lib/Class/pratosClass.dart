class Pratos {
  int _id;
  String _nome;
  String _descricao;
  bool _vegano;
  String? _imagem;
  String? _notaTecnica;
  String? _descricaoIA;
  int _categoriaId;

  Pratos(
    this._id,
    this._nome,
    this._descricao,
    this._vegano,
    this._imagem,
    this._notaTecnica,
    this._descricaoIA,
    this._categoriaId,
  );

  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  String get descricao => _descricao;
  set descricao(String value) => _descricao = value;

  bool get vegano => _vegano;
  set vegano(bool value) => _vegano = value;

  String? get imagem => _imagem;
  set imagem(String? value) => _imagem = value;

  String? get notaTecnica => _notaTecnica;
  set notaTecnica(String? value) => _notaTecnica = value;

  String? get descricaoIA => _descricaoIA;
  set descricaoIA(String? value) => _descricaoIA = value;

  int get categoriaId => _categoriaId;
  set categoriaId(int value) => _categoriaId = value;

  factory Pratos.fromJson(Map<String, dynamic> json) {
    return Pratos(
      json['id'],
      json['nome'],
      json['descricao'],
      json['vegano'] ?? false,
      json['imagem'],
      json['notaTecnica'],
      json['descricaoIA'],
      json['categoria']['id'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': _id,
      'nome': _nome,
      'descricao': _descricao,
      'vegano': _vegano,
      'imagem': _imagem,
      'notaTecnica': _notaTecnica,
      'descricaoIA': _descricaoIA,
      'categoria': {'id': _categoriaId},
    };
  }
}
