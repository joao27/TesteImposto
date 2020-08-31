# TesteImposto

Refactor realizado com o objetivo de realizar a persistência de dados de Notas Fiscais em uma base de dados, bem como identificar melhorias no projeto e realizar a correção de bugs.

### Considerações Importantes

* As adições de novas **Dll's** seguem a versão do projeto inicial - *.NET Framework 4.5* - pensando-se em manter a compatibilidade do sistema.
* Utilizado a IDE *Visual Studio 2019* para desenvolvimento.
* Os testes foram implementados somente para a camada de negócio.
* O arquivo contendo as considerações sobre o projeto, bem como as atividades desenvolvidas, encontra-se na pasta `TesteImposto`, no arquivo `consideracoes.pdf`.

### Setup das alterações realizadas

Para realizar o teste das atividades desenvolvidas é necessário realizar os seguintes passos abaixo listados:
* Já existindo o banco de dados do projeto inicial criado execute os seguintes scripts localizados na pasta `TesteImposto/SQL/ATUALIZACOES` **ALTERAR_TB_NOTA_FISCAL_ITEM**, **ALTERAR_P_NOTA_FISCAL_ITEM** e **P_CONSULTA_NOTA_FISCAL_ITEM**, na respectiva ordem.
* Definir o diretório para a gravação dos XML's das notas fiscais emitidas. A definição desse diretório deverá ser feita no arquivo `App.config`, localizado na pasta `TesteImposto`, sendo que o caminho do diretório de gravação deverá conter uma contra barra `(\)` em seu final `(ex.: C:\Diretorio-Notas-Emitidas\)`.
* Definir a string de conexão para o banco de dados no arquivo `App.config`, localizado no diretório já mencionado no item acima. Para o caso de o banco de dados de teste estar configurado localmente com o SQLEXPRESS, então não há necessidade de alterações.

### Execução

Defina **Set as Startup Projetc** o projeto *TesteImposto*, localizado na pasta `TesteImposto`.
