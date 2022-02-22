# ASP.NET Core Trabalho Prático

## Estilo de código e estrutura do projeto

É o típico código OOP com estrutura MVC, com dois tipos de Models um de Dados relacionados diretamente com a Tabela da DB e um de Dados Transacionais entre Cliente e Servidor, Views e dois grupos de Controllers, um de controlo interno para as Views e outro de controlo externo para a API.

### Tarefas a concluir

Visto que os dois casos de uso para mim escolhidos (dos quatro possíveis, estabelecidos no trabalho investigativo anterior), com suporte unânime do grupo/par, foram:

+ **Consultar receitas e ingredients**: para consultar as receitas e os ingredients que estão associados a essas receitas.
+ **Gestão de ingredientes e receitas**: para criar, editar ou eliminar ingredientes e receitas.

Conseguimos determinar que temos de facto, os quatro principais métodos HTTP (GET, POST, PUT, DELETE), os quais uma REST API usa (em conjunto com as tecnologias de notação JSON e XML) para comunicar. Tendo assim uma boa base de estudo e trabalho prático.

Em suma, este trabalho tem de ser feito com o objetivo de aprender a usar a linguagem de programação ASP.NET Core, de forma a criar um sistema de gestão de receitas e ingredientes, com a possibilidade de criar, editar ou eliminar receitas e ingredientes via API RESTful e ainda com interface de gestão de receitas e ingredientes via Web.

### Modelos de Dados

O modelo de dados é um conjunto de classes que representam a estrutura de dados da aplicação. Logo, os modelos principais são os da diretoria Models, que representam as tabelas da base de dados.

No entanto, existem também modelos que representam dados que não estão associados a uma tabela, mas sim servem para facilitar a utilização e comunicação nos controladores entre os clientes e o servidor. Estes são os modelos da subdiretoria Models/Compound, pois são modelos compostos, que são modelos que contêm dados de outros modelos.

### Controladores

Um controller é um conjunto de métodos que manipulam dados e são chamados pelo cliente. Os controladores "principais" são os da diretoria Controllers, que representam os grupos de métodos que manipulam dados da aplicação de forma visual. Ou seja, manipulam dados diretamente para as Views que eles geram.

Já para a API, os controladores são os da diretoria API/Controllers, que manipulam dados para a API (RESTful), em formato JSON, com a formatação referente ao modelo de dados composto (Compound).

### Visualizações

As Views são as páginas HTML que apresentam os dados para o cliente. As Views principais são as da diretoria Views, que representam as páginas HTML que apresentam dados para o cliente.

Estas são retornadas pelos controladores "principais", que são os controladores da diretoria Controllers, que manipulam dados para as Views.

## API

A API RESTful para comunicação externa foi a primeira tarefa a ser desenvolvida. A sua conclusão delinearia a estrutura do projeto e encaminhava a forma como os dados são tratados e como os reimplementamos nos controladores com Views.

Para uma API ser RESTful, deve ser possível a comunicação entre o cliente e o servidor, via os métodos HTTP (GET, POST, PUT, DELETE, etc), e com a formatação referente ao modelo de dados num formato de notação de objetos (JSON ou XML). Esta deve também usar um standard de nomenclatura para comunicar os erros. Esse standard usa cinco trios de números, dos quais os que começam por 1 são mensagens informativas, os que começam por 2 são mensagens de sucesso, os que começam por 3 são mensagens de redirecionamento, os que começam por 4 são mensagens de erro de cliente e os que começam por 5 são mensagens de erro de servidor.

Neste caso específico, a API RESTful usa o modelo de dados composto (Compound), que é um modelo de dados que contém dados de outros modelos, em formato de notação JSON, para via os quatro principais métodos HTTP (GET, POST, PUT, DELETE).

### Gestão de ingredientes

Para a gestão de ingredientes, a API RESTful usa dois métodos HTTP GET, um POST, um PUT e um DELETE. Os quais são usados para consultar todos os ingredientes, consultar um ingrediente específico, criar um ingrediente, editar um ingrediente ou eliminar um ingrediente.

Estes URI são:

+ _/api/ingredients/list_: **GET** para consultar todos os ingredientes.
+ _/api/ingredients/view/{id}_: **GET** para consultar um ingrediente específico.
+ _/api/ingredients/add_: **POST** para criar um ingrediente. Usamos no _body_ no formato JSON para enviar os dados, com o formato do IngredienteModel (que é um modelo composto).
+ _/api/ingredients/edit/_: **PUT** para editar um ingrediente. Usamos no _body_ no formato JSON para enviar os dados, com o formato do IngredienteModel (que é um modelo composto).
+ _/api/ingredients/remove/{id}_: **DELETE** para eliminar um ingrediente.

O modelo de dados composto, IngredienteModel, contém os seguintes campos:

+ **IId**: identificador do ingrediente.
+ **Name**: nome do ingrediente.
+ **Quantity**: quantidade do ingrediente.

Estes campos são obrigatórios.

### Gestão de receitas

Na gestão de receitas, a API RESTful usa dois métodos HTTP GET, um POST, um PUT e um DELETE. Os quais são usados para consultar todas as receitas, consultar uma receita específica, criar uma receita, editar uma receita ou eliminar uma receita.

Os URI são:

+ _/api/recipes/list_: **GET** para consultar todas as receitas.
+ _/api/recipes/view/{id}_: **GET** para consultar uma receita específica.
+ _/api/recipes/add_: **POST** para criar uma receita. Usamos no _body_ no formato JSON para enviar os dados, com o formato do RecipeModel (que é um modelo composto).
+ _/api/recipes/edit/_: **PUT** para editar uma receita. Usamos no _body_ no formato JSON para enviar os dados, com o formato do RecipeModel (que é um modelo composto).
+ _/api/recipes/remove/{id}_: **DELETE** para eliminar uma receita.

O RecipeModel, que é um modelo composto, contém os seguintes campos:

+ **RId**: identificador da receita.
+ **Name**: nome da receita.
+ **Ingredients**: ingredientes da receita.

Este campo Ingredients é um IEnumerable, que é um tipo de dados que permite a criação de listas de dados. Esta lista é composta por objetos do tipo RecipeModel.Ingredient, que é um objeto interno do modelo RecipeModel. Este objeto é composto pelos seguintes campos:

+ **IId**: identificador do ingrediente.
+ **Quantity**: quantidade do ingrediente.

### Error Handling

Para a gestão de erros, a API está rodeada de blocos try-catch e faz vários checks if para verificar se ocorreu algum erro. Se ocorreu, o erro é tratado e retornado ao cliente um código de erro e uma mensagem de erro.

Os códigos de erro são:

+ **400**: Bad Request. Ocorre quando o cliente envia dados mal formatados.
+ **404**: Not Found. Ocorre quando o recurso não foi encontrado.
+ **500**: Internal Server Error. Ocorre quando ocorre um erro no servidor.

Na versão rescrita da API, como existe o uso de um login básico (inseguro, mas funcional) com nome e hash no _body_, a API pode ainda retornar:

+ **401**: Unauthorized. Ocorre quando o cliente não está autenticado.
+ **403**: Forbidden. Ocorre quando o cliente não tem permissão para aceder ao recurso.

### Implementação de Autenticação (básica e insegura)

Foi criada uma classe privada herdeira do modelo composto adequado ao controlador, onde se adiciona o campo _UserName_ e _Passhash_. Estes campos são usados para autenticar o cliente. A qual vamos buscar a role do usuário se o mesmo estiver autenticado, para verificar se o cliente tem permissão para aceder ao recurso. Esta verificação é feita no método inicio do método responsável por executar a ação pedida, onde retorna o código de erro 403 (Forbidden) se o cliente não tiver permissão para aceder ao recurso, ou o código de erro 401 (Unauthorized) se o cliente não estiver autenticado ou a hash não corresponder à hash do utilizador.

## Webapp

A aplicação web é constituída por um conjunto de páginas HTML, que são carregadas pelo servidor. Estas usam um roteamento diferente, que é na prática uma API interna, que é acessada pelo cliente através dos formulários nas Views.

### Gestão de ingredientes

Para a gestão de ingredientes, a Webapp usa quatro métodos HTTP GET e dois métodos HTTP POST. Os quais são usados para consultar todos os ingredientes, consultar um ingrediente específico, criar um ingrediente e editar um ingrediente. Não existe a utilização de métodos PUT e DELETE, visto que os formulários HTML e Razor não suportam estes métodos.

Estes URI são:

+ _/api/ingredient/all_: **GET** para consultar todos os ingredientes.
+ _/api/ingredient/view/{id}_: **GET** para consultar um ingrediente específico.
+ _/api/ingredient/add_: **GET** para receber o formulário para criar um ingrediente.
+ _/api/ingredient/edit/{id}_: **GET** para receber o formulário para editar um ingrediente.
+ _/api/ingredient/delete/{id}_: **GET** para eliminar um ingrediente.

Os quais comunicam com as URI:

+ _/api/ingredient/applyadd_: **POST** para criar um ingrediente.
+ _/api/ingredient/applyedit_: **POST** para editar um ingrediente.

O modelo de dados composto, IngredienteModel, contém os seguintes campos:

+ **IId**: identificador do ingrediente.
+ **Name**: nome do ingrediente.
+ **Quantity**: quantidade do ingrediente.

Estes campos são obrigatórios.

### Gestão de receitas

Na gestão de receitas, a API RESTful usa sete métodos HTTP GET e quatro métodos HTTP POST. Os quais são usados para consultar todas as receitas, consultar uma receita específica, criar uma receita e editar uma receita. Não existe a utilização de métodos PUT e DELETE, visto que os formulários HTML e Razor não suportam estes métodos.

Os URI são:

+ _/api/recipe/all_: **GET** para consultar todas as receitas.
+ _/api/recipe/view/{id}_: **GET** para consultar uma receita específica.
+ _/api/recipe/add_: **GET** para receber o formulário para criar uma receita.
+ _/api/recipe/addingredient/{id}_: **GET** para receber o formulário para adicionar um ingrediente à receita.
+ _/api/recipe/editname/{id}_: **GET** para receber o formulário para editar o nome de uma receita.
+ _/api/recipe/editingredients/{id}_: **GET** para receber o formulário para ver os ingredientes a editar de uma receita.
+ _/api/recipe/editingredient/{id}_: **GET** para receber o formulário para editar um ingrediente de uma receita.
+ _/api/recipe/delete/{id}_: **GET** para eliminar uma receita.
+ _/api/recipe/deleteingredient/{id}_: **GET** para eliminar um ingrediente de uma receita.

Os quais comunicam com as URI:

+ _/api/recipe/applyadd_: **POST** para criar uma receita.
+ _/api/recipe/applyaddingredient_: **POST** para adicionar um ingrediente à receita.
+ _/api/recipe/applynameedit_: **POST** para editar o nome de uma receita.
+ _/api/recipe/applyingredientedit_: **POST** para editar um ingrediente de uma receita.

O RecipeVM, que é um ViewModel, contém os seguintes campos:

+ **RId**: identificador da receita.
+ **Name**: nome da receita.

O RecipeAddVM, que é um ViewModel, contém os seguintes campos:

+ **RId**: identificador da receita.
+ **RilId**: identificador do ingrediente-receita.
+ **IId**: identificador do ingrediente.
+ **Name**: nome da receita.
+ **Quantity**: quantidade do ingrediente.

O RecIngListsVM, que é um ViewModel, contém os seguintes campos:  

+ **RId**: identificador da receita.
+ **IId**: identificador do ingrediente.
+ **RilId**: identificador do ingrediente-receita.
+ **Quantity**: quantidade do ingrediente.

### Error Handling

Para a gestão de erros, a API está rodeada de blocos try-catch e faz vários checks if para verificar se ocorreu algum erro. Se ocorreu, o erro é tratado e retornado ao cliente um código de erro e uma mensagem de erro.

Os códigos de erro são:

+ **400**: Bad Request. Ocorre quando o cliente envia dados mal formatados.
+ **404**: Not Found. Ocorre quando o recurso não foi encontrado.
+ **500**: Internal Server Error. Ocorre quando ocorre um erro no servidor.

Na versão rescrita da API, como existe o uso de um login básico (inseguro, mas funcional) com nome e hash no _body_, a API pode ainda retornar:

+ **401**: Unauthorized. Ocorre quando o cliente não está autenticado.
+ **403**: Forbidden. Ocorre quando o cliente não tem permissão para aceder ao recurso.
