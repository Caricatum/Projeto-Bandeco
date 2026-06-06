class Usuario {
  int _id;
  String _nome;
  bool _funcionario;
  String _senha;
  String _login;

  Usuario(this._id, this._nome, this._funcionario, this._senha, this._login);

  int get id => _id;
  set id(int value) => _id = value;

  String get nome => _nome;
  set nome(String value) => _nome = value;

  bool get funcionario => _funcionario;
  set funcionario(bool value) => _funcionario = value;

  String get senha => _senha;
  set senha(String value) => _senha = value;

  String get login => this._login;
  set login(String value) => _login = value;
}