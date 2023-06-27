# ~ MyBookPlanner API 📖

Esta é uma API de catálogo de livros desenvolvida com ASP.NET, Entity Framework Core e SQL Server. A API permite que os usuários criem contas, façam login e adicionem livros disponíveis no catálogo às suas contas pessoais. Os livros salvos pelos usuários são associados a uma nota e a um status de leitura, que pode ser "lido", "lendo" ou "desejo ler".

## Tecnologias utilizadas

- ASP.NET: um framework para desenvolvimento de aplicativos web usando a plataforma .NET.
- Entity Framework Core: um framework de mapeamento objeto-relacional (ORM) que permite o acesso e manipulação de dados em um banco de dados relacional usando objetos.
- SQL Server: um sistema de gerenciamento de banco de dados relacional usado para armazenar os dados do catálogo de livros e informações dos usuários.

## Funcionalidades

1. Autenticação de Usuário:
   - Os usuários podem criar uma conta fornecendo um nome, email e senha.
   - Os usuários podem fazer login usando suas credenciais de conta.

2. Adicionar Livros:
   - Os usuários autenticados podem adicionar livros disponíveis no catálogo à sua conta pessoal.
   - Cada livro adicionado pode ter uma nota e um status de leitura associados a ele.

3. Gerenciar Perfil:
   - Os usuários podem alterar informações do perfil, como nome de usuário, biografia e endereço de e-mail.

4. Estatísticas do Catálogo de Livros:
   - Os usuários podem acessar estatísticas sobre os livros que adicionaram, como a quantidade de livros que estão lendo, que já leram ou que desejam ler.
   - Os usuários podem ver o seu melhor livro, que é o livro do usuário com a maior nota.

## API Endpoints

A API oferece os seguintes principais endpoints:

- POST /user/register
  - Cria uma nova conta de usuário.

- POST /user/login
  - Retorna o token do usuário ao receber as credenciais corretas.

- GET /user/{userId}
  - Obtém informações de um usuário.

- PUT /user/{userId}
  - Atualiza as informações de um usuário.
    
- DELETE /user/{userId}
  - Deleta um usuário.
 
- GET /user-book/all-books/{userId}
  - Retorna todos os livros do usuário.

- POST /user-book/add-book
  - Adiciona um livro na conta do usuário.

- DELETE /user-book/delete-book/{idUser}/{idBook}
  - Deleta um livro da conta do usuário.
    
- PUT /user-book/update-book
  - Atualiza um livro já existente na conta do usuário.

- GET /books?page=0&pageSize=10)
  - Retorna livros disponíveis no catálogo usando paginação de dados. Além disso, os livros já vem em ordem para rankings, sendo a ordem da maior nota para a menor nota.

- GET /user-book/{userId}/statistics
  - Obtém as estatísticas do catálogo de livros do usuário.

- GET /user-book/{userId}/best-book
  - Obtém o livro com a maior nota dada pelo usuário.

- GET /user-book/wish-to-read/{userId}
  - Retorna todos os livros do usuário que estão definidos como "desejo".

- GET /user-book/reading/{userId}
  - Retorna todos os livros do usuário que estão definidos como "lendo".

- GET /user-book/readed/{userId}
  - Retorna todos os livros do usuário que estão definidos como "lido".
    

## Contribuição

Contribuições são bem-vindas! Se você quiser melhorar este projeto, sinta-se à vontade para enviar pull requests ou abrir issues.

## Licença

Este projeto está licenciado sob a Licença MIT. Consulte o arquivo LICENSE para obter mais informações.
