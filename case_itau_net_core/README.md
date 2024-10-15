# Atividades realizadas na API(case_itau_net_core)

## Ajustes gerais
1.  Inclusão da referencia ao arquivo dbcaseitau.S3db dentro de appsettings.json
2.  Inclusão arquivo .gitignore

## Correções Bugs
1.  Correção no momento de inserir o Fundo
    O valor do patrimonio estava setado como NULL no POST, e na hora de fazer o GET, uma exceção do tipo null reference era gerada para o respectivo campo

## Correções estruturais ou de segurança
1.  Reestruturação da API com Clean Architecture: Separação das camadas em Application, Domain, e Infrastructure para seguir o padrão de arquitetura limpa.
2.  Utilização do Entity Framework para transações com banco de dados
3.  Centralização do tratamento de exceções: Uso de ExceptionFilter para capturar exceções em nível global e reduzir a necessidade de try-catch em todas as camadas.
4.  Segurança contra SQL Injection
5.  Uso de data annotations para garantir a validação de campos (como tamanho máximo e obrigatoriedade) nas entidades.
6.  Logs customizados: Configuração de logs customizados para registrar apenas erros, centralizando o controle dos registros de logs em um arquivo específico.
7.  Implementação do AutoMapper para mapear automaticamente entre DTOs e entidades, simplificando o código.

## Testes Unitários
1. Implementação de testes unitários utilizando xUnit seguindo o padrão AAA (Arrange, Act, Assert).
2. Cobertura de múltiplos cenários: testes de criação, movimentação, remoção de fundos, verificação de conflitos e retornos de erros apropriados.
3. Simulação de repositórios e uso de Moq para testar isoladamente as funcionalidades de cada camada.

## Design Patterns Utilizados
1.  Injeção de Dependência: Instanciação de objetos através de construtores e interfaces.
2.  Repository Pattern: Separação das responsabilidades de acesso ao banco de dados nas classes de repositórios e interfaces.
3.  Middleware para exceções: Filtro global para centralizar o tratamento de exceções, eliminando a necessidade de try-catch em cada operação.
4.  DTO (Data Transfer Object): Transferência segura de dados entre a entidade e a camada de apresentação.