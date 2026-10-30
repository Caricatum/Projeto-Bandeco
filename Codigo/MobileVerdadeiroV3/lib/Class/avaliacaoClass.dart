class Avaliacao {
  int _id;
  String? _avaliacao;
  int _nota;
  int _usuarioId;
  String _nomeUsuario;
  int _pratoId;

  Avaliacao(
    this._id,
    this._avaliacao,
    this._nota,
    this._usuarioId,
    this._nomeUsuario,
    this._pratoId,
  );

  int get id => _id;
  set id(int value) => _id = value;

  String? get avaliacao => _avaliacao;
  set avaliacao(String? value) => _avaliacao = value;

  int get nota => _nota;
  set nota(int value) => _nota = value;

  int get usuarioId => _usuarioId;
  set usuarioId(int value) => _usuarioId = value;

  String get nomeUsuario => _nomeUsuario;
  set nomeUsuario(String value) => _nomeUsuario = value;

  int get pratoId => _pratoId;
  set pratoId(int value) => _pratoId = value;

  factory Avaliacao.fromJson(Map<String, dynamic> json) {
    return Avaliacao(
      json['id'],
      json['avaliacao'],
      json['nota'],
      json['user']['id'],
      json['user']['nome'] ?? 'Usuário',
      json['prato']['id'],
    );
  }
}
