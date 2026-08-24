class Usuario {
  int _id;
  String _nome;
  bool _funcionario;
  String _login;
  bool _emailConfirmado;

  Usuario(
    this._id,
    this._nome,
    this._funcionario,
    this._login,
    this._emailConfirmado,
  );

  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  bool get funcionario => _funcionario;
  set funcionario(bool value) => _funcionario = value;

  String get login => _login;
  set login(String value) => _login = value;

  bool get emailConfirmado => _emailConfirmado;
  set emailConfirmado(bool value) => _emailConfirmado = value;
}