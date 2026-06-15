class Pratos {
  int _id;
  String _nome;
  String _descricao;
  bool _vegano;
  String _imagem;
  String _notaTecnica;
  int _categoriaId;

  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  String get descricao => _descricao;
  set descricao(String value) => _descricao = value;

  bool get vegano => _vegano;
  set vegano(bool value) => _vegano = value;

  String get imagem => _imagem;
  set imagem(String value) => _imagem = value;

  String get notaTecnica => _notaTecnica;
  set notaTecnica(String value) => _notaTecnica = value;

  int get categoriaId => _categoriaId;
  set categoriaId(int value) => _categoriaId = value;

  Pratos(this._id, this._nome, this._descricao, this._vegano, this._imagem,
      this._notaTecnica, this._categoriaId, {required String notaTecnica, required String descricao, required String nome, required String imagem, required int categoriaId, required bool vegano});
}