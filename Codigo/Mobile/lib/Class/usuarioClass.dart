class Usuario {
  int _id;
  String _nome;
  String _senhaHash;
  bool _funcionario;
  String _login;
  bool _emailConfirmado;

  Usuario(
    this._id,
    this._nome,
    this._senhaHash,
    this._funcionario,
    this._login,
    this._emailConfirmado,
  );

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      json['id'],
      json['nome'],
      json['senhaHash'],
      json['funcionario'],
      json['login'],
      json['emailConfirmado'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'nome': nome,
      'senhaHash': _senhaHash,
      'funcionario': funcionario,
      'login': login,
      'emailConfirmado': emailConfirmado,
    };
  }

  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  String get senhaHash => _senhaHash;
  set senhaHash(String value) => _senhaHash = value;

  bool get funcionario => _funcionario;
  set funcionario(bool value) => _funcionario = value;

  String get login => _login;
  set login(String value) => _login = value;

  bool get emailConfirmado => _emailConfirmado;
  set emailConfirmado(bool value) => _emailConfirmado = value;
}