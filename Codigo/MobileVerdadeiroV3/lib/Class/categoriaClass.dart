class Categoria {
  int _id;
  String _descricao;

  Categoria({
    required int id,
    required String descricao,
  })  : _id = id,
        _descricao = descricao;

  int get id => _id;
  String get descricao => _descricao;

  set id(int value) {_id = value;}
  set descricao(String value) {_descricao = value;}
}

List<Categoria> categorias = [
  Categoria(id: 1, descricao: "Acompanhamento"),
  Categoria(id: 2, descricao: "Guarnição"),
  Categoria(id: 3, descricao: "Prato Principal"),
  Categoria(id: 4, descricao: "Refresco"),
  Categoria(id: 5, descricao: "Salada"),
  Categoria(id: 6, descricao: "Sobremesa"),
];
