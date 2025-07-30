Perfeito! Vou elaborar um README detalhado dividido em duas seções:

1. **Como rodar o projeto localmente**, incluindo Docker, variáveis de ambiente e autenticação.
2. **Decisões técnicas adotadas no projeto**, com base na arquitetura DDD, uso de CQRS, Rebus, MongoDB, etc.

Vou começar agora e te aviso quando estiver pronto para revisão.


# SalesSystem.API

## Como rodar o projeto localmente

Para executar o **SalesSystem.API** localmente, certifique-se de ter o Docker e o Docker Compose instalados em sua máquina. Siga os passos abaixo:

1. **Clonar o repositório:** Faça o clone do projeto `SalesSystem.API` em seu ambiente local. Navegue até o diretório raiz do projeto.
2. **Configurar variáveis de ambiente:** Antes de iniciar, defina as variáveis de ambiente necessárias. Em particular, é preciso configurar o **segredo JWT (JWT Secret)** utilizado para assinar tokens. Você pode fazer isso criando um arquivo `.env` na raiz do projeto ou definindo a variável de ambiente diretamente. (Por exemplo: `JwtExtension__Secret` com um valor seguro). Se houver outras variáveis, como strings de conexão para banco de dados PostgreSQL ou credenciais do RabbitMQ, configure-as conforme necessário. *Dica:* Em .NET Core, o algoritmo recomendado para hash de senhas é o PBKDF2 – o projeto utiliza esse padrão, então mantenha o segredo JWT protegido.
3. **Subir os serviços com Docker Compose:** No diretório do projeto, execute o comando:

   ```bash
   docker-compose up --build
   ```

   Esse comando irá **construir** as imagens Docker do projeto e iniciar os containers definidos. O Compose irá levantar o container da API .NET junto com os serviços de suporte:

   * **PostgreSQL:** banco de dados relacional para armazenar informações (como dados de usuários/autenticação e outros dados do domínio).
   * **MongoDB:** banco NoSQL utilizado para **snapshots** (instantâneos de estado do domínio) e possivelmente para consultas otimizadas.
   * **RabbitMQ:** broker de mensageria para processamento de eventos assíncronos (usado em conjunto com a biblioteca Rebus).
     Esses serviços são iniciados automaticamente em uma rede Docker interna, permitindo que a API comunique-se com eles pelos nomes de host dos containers (e.g. `postgres` para o PostgreSQL, `mongo` para o MongoDB, `rabbitmq` para o RabbitMQ).
4. **Aguardar inicialização:** Aguarde até que todos os containers estejam de pé. O banco PostgreSQL geralmente roda na porta padrão 5432, o MongoDB na 27017 e o RabbitMQ na 5672 (com painel de controle possivelmente na 15672). O container da API deve expor a aplicação (verifique no log a porta – por padrão pode ser a 8080 ou 5000/5001). Assim que a API indicar que está ouvindo requisições HTTP, o sistema estará pronto.
5. **Testar a API e autenticação:** Você pode acessar os endpoints da API (por exemplo, via *Swagger* se habilitado ou via ferramentas como *Postman*). Para testar a **autenticação**, utilize as rotas de registro e login fornecidas:

   * **Registrar novo usuário:** `POST /api/v1/registers` com os dados do usuário (nome, email, senha etc) para criar uma conta.
   * **Login (obter JWT):** `POST /api/v1/registers/signin` com credenciais (email e senha) para realizar login e receber um token JWT de acesso.
   * Se o projeto definir um usuário padrão (credenciais mock) para testes, essa informação estará na documentação ou configurações. Caso exista um usuário administrativo pré-configurado, utilize essas credenciais para fazer login. *(Por exemplo, pode haver um usuário admin com senha default para ambiente de desenvolvimento.*)
   * Após obter o token JWT, inclua-o no header **Authorization** como `Bearer {token}` em chamadas subsequentes para acessar endpoints protegidos.
6. **Verificar comunicação entre serviços:** Ao acionar certas funcionalidades (como operações de domínio que disparam eventos), a API publicará eventos no RabbitMQ. Isso acontece de forma transparente – por exemplo, um evento de domínio é enviado via **Rebus** ao RabbitMQ sem travar o fluxo (estilo *fire-and-forget*). Você pode inspecionar as filas no RabbitMQ (via painel web na porta 15672, credenciais default *guest/guest* se não alteradas) para verificar os eventos publicados.

> **Observação:** O processo de inicialização via Docker Compose cuida de criar os containers e redes necessários. Caso precise personalizar alguma porta ou credencial, ajuste o arquivo `docker-compose.yml`. Certifique-se também de que as migracões de banco (se houver) sejam aplicadas – possivelmente o projeto aplica migracões automaticamente ao subir, dada a prática recomendada de configurar isso no startup.

Seguindo esses passos, o SalesSystem.API deverá estar rodando localmente e integrado aos serviços de suporte. Você poderá então consumir os endpoints REST conforme necessário, testando as funcionalidades de cadastro, autenticação, cadastro de produtos/vendas, etc., de acordo com as endpoints fornecidas pela API.

## Decisões técnicas

O projeto adota diversas decisões de arquitetura e tecnologia para garantir escalabilidade, manutenibilidade e adesão a boas práticas de design de software. Abaixo estão os principais pontos técnicos e as justificativas para cada decisão:

### Arquitetura em camadas (DDD)

A solução foi estruturada seguindo os princípios de **Domain-Driven Design (DDD)** e uma arquitetura em camadas bem definida. O código fonte está organizado em projetos separados por responsabilidade, tipicamente:

* **Domain (Domínio):** contém as entidades de negócio, agregados, Value Objects, interfaces de repositório e regras de negócio. Esta camada não depende de nenhuma outra – é o núcleo da aplicação.
* **Application (Aplicação):** contém os casos de uso/serviços de aplicação, implementações de **CQRS** (Commands, Queries e seus **Handlers**), além de interfaces para comunicação com a camada de Domínio. Orquestra as operações de negócio combinando entidades do domínio, mas sem implementar regras de negócio complexas diretamente.
* **Infrastructure (Infraestrutura):** fornece implementações concretas para as interfaces definidas no domínio (por exemplo, repositórios concretos usando EF Core com PostgreSQL, implementação do serviço de mensageria Rebus, acesso ao MongoDB, envio de email etc.). Essa camada conecta o mundo da aplicação aos recursos externos.
* **API (Apresentação):** o projeto Web (SalesSystem.API) que expõe os serviços via HTTP/REST. Contém os **Controllers**, configuração de dependência (IoC/DI container), configuração do ASP.NET Core (Middleware, autenticação JWT, Swagger, etc).

Essa separação segue o princípio de dependência invertida: as camadas superiores (como API ou Application) dependem das abstrações definidas no Domain, e não de implementações concretas. Isso torna o domínio da aplicação isolado e facilmente testável, além de permitir trocar detalhes de infraestrutura sem impactar a lógica de negócio. Em outras palavras, *“o projeto DDD é totalmente orientado ao domínio; a principal diferença dessa solução é proteger os códigos concretos (Serviços de Aplicação, Infraestrutura) e forçar as camadas de apresentação a usar apenas as interfaces e entidades do domínio”*. Assim, garante-se baixo acoplamento e alta coesão, seguindo princípios **SOLID** e facilitando a evolução do software.

### CQRS e Mediator.NET

Para implementar a comunicação interna entre as camadas e separar as responsabilidades de leitura e escrita, o projeto adota o padrão **CQRS (Command Query Responsibility Segregation)**. Isso significa que operações de **consulta** (queries) e **comando** (modificação de estado) são tratadas de formas distintas, geralmente até mesmo com modelos de dados separados.

Cada ação de escrita, como criar um registro de venda ou cadastrar um novo cliente, é modelada como um **Command** que é enviado para um **Handler** específico. Da mesma forma, cada operação de leitura de dados complexa pode ser modelada como uma **Query** com seu respectivo handler. Para viabilizar esse despacho de comandos/queries de maneira desacoplada, o projeto utiliza um mediador – no caso, a biblioteca **Mediator.NET** (uma implementação do padrão *Mediator* para .NET). O mediador atua como um orquestrador central: em vez de o Controller chamar serviços ou repositórios diretamente, ele envia um Command ou Query ao mediador (por meio de uma interface como `IMediatorHandler`), e este se encarrega de localizar e executar o handler apropriado.

Essa abordagem traz vários benefícios:

* **Desacoplamento:** O Controller não precisa conhecer os detalhes da lógica – apenas qual comando ou consulta disparar. Novos handlers podem ser adicionados sem modificar os Controllers.
* **Organização:** Centraliza a lógica de cada caso de uso em classes separadas (handlers), facilitando manutenção e testes isolados.
* **Pipelines e comportamentos:** O Mediator.NET suporta pipelines, permitindo implementar comportamentos transversais (como logging, validação, cache) envolta dos handlers de forma consistente.

Em resumo, o uso de **CQRS + Mediator** melhora a clareza e manutenção do código. Cada comando/consulta representa uma intenção clara e é processado por um único trecho de código, seguindo princípios de responsabilidade única.

### Event Sourcing e Snapshots (MongoDB)

Para algumas partes do domínio, especialmente aquelas mais críticas ou com necessidade de histórico de mudanças, o projeto adota conceitos de **Event Sourcing**. Em vez de simplesmente persistir o estado atual, as mudanças de estado (eventos de domínio) são armazenadas sequencialmente. Com o tempo, o conjunto de eventos pode crescer, e reconstruir o estado de um agregado a partir de todos os eventos pode se tornar custoso.

Para mitigar isso, o SalesSystem implementa **snapshots** – essencialmente *instantâneos* periódicos do estado do agregado ou de visões de leitura. Os snapshots capturam o estado completo em determinado ponto no tempo, permitindo reidratar um agregado rapidamente aplicando apenas eventos novos após o snapshot, em vez de todos os eventos desde o início.

A escolha técnica aqui foi usar o **MongoDB** como repositório desses snapshots. O MongoDB, por ser um banco orientado a documentos, é adequado para armazenar estruturas flexíveis de estado agregado (JSON/BSON) de forma eficiente. Assim, periodicamente (por exemplo, a cada N eventos ou intervalo de tempo), o sistema armazena um documento no MongoDB representando o estado atual do agregado. Em consultas, pode-se ler diretamente do Mongo (snapshot mais recente) ao invés de reconstruir tudo do zero. Conforme apontado por especialistas, *“não há nada de errado em gerar snapshots assincronamente e armazená-los fora do stream de eventos”* – ou seja, manter os snapshots em armazenamento separado é uma prática válida em event sourcing, focada em desempenho e não interfere na confiabilidade dos eventos.

Em resumo, **MongoDB** atua como o *snapshot store* do sistema, garantindo consultas mais rápidas e aliviando a carga de reconstrução. Enquanto isso, o log de eventos (provavelmente mantido no PostgreSQL ou outro meio via módulo EventSourcing) continua sendo a fonte da verdade imutável. Os snapshots são uma otimização de leitura, mantendo uma **consistência eventual**: podem não refletir imediatamente o último evento, mas são atualizados periodicamente ou sob demanda conforme configurado.

### Integração assíncrona com Rebus e RabbitMQ

No contexto de uma arquitetura orientada a domínio e potencialmente de microservices, o SalesSystem.API precisa propagar eventos de domínio e realizar comunicações assíncronas de forma confiável. Para isso, ele utiliza a combinação **Rebus + RabbitMQ**.

O **RabbitMQ** é um servidor de filas (*message broker*) amplamente usado que implementa o protocolo AMQP, garantindo entrega de mensagens, roteamento flexível e suporte a padrões de publicação/assinatura. Já o **Rebus** é uma biblioteca .NET que funciona como um *service bus* leve, facilitando o uso de filas e transporte de mensagens dentro das aplicações .NET. Segundo a documentação, *“Rebus é uma implementação lean de service bus para .NET, semelhante ao NServiceBus e MassTransit, porém mais enxuta”*. Isso o torna ideal para integrar o sistema com o RabbitMQ de forma simples, sem a complexidade de frameworks de mensageria mais pesados.

No SalesSystem, o Rebus é configurado para usar o RabbitMQ como **transport**. Assim, quando determinados eventos de domínio ocorrem (por exemplo, “VendaEfetuada” ou “PagamentoRecebido”), esses eventos são publicados via Rebus, que os envia ao RabbitMQ. Outros serviços ou partes do sistema podem estar inscritos nessas filas para reagir aos eventos. Importante destacar que esse envio é **fire-and-forget**, ou seja, a API não aguarda resposta ao publicar um evento – o objetivo é desacoplar e permitir processamento assíncrono em background. Por exemplo, após uma venda ser registrada no sistema principal (transação persistida), um evento pode ser publicado para que um microsserviço de faturamento gere a nota fiscal, ou para que o módulo de pagamentos cobre o cliente, sem bloquear o fluxo da API.

Essa decisão técnica promove uma arquitetura **event-driven** e escalável. Novos consumidores podem ser adicionados à fila RabbitMQ sem modificar o código da API (basta assinar os tópicos/eventos relevantes). O uso do RabbitMQ garante confiabilidade na entrega dos eventos, enquanto o Rebus simplifica a codificação desse mecanismo dentro do .NET.

### Autenticação JWT e hash de senhas (PBKDF2)

Em termos de segurança, o projeto implementa autenticação baseada em **JWT (JSON Web Tokens)** para proteger a API e **PBKDF2** para hashing de senhas de usuários.

* **JWT:** Ao efetuar login (endpoint de SignIn), o usuário recebe um token JWT assinado com uma chave secreta conhecida pelo servidor. Esse token contém as claims do usuário (identificação, roles, etc.) e uma assinatura HMAC que garante a integridade. A API utiliza autenticação do tipo Bearer – ou seja, os clientes devem enviar o token no header Authorization. O token JWT permite autenticação **stateless** no servidor (não é necessário manter sessão no backend), e pode incluir tempo de expiração e outras políticas de segurança. A configuração do JWT é feita definindo-se o emissor, audiência válida e a chave secreta de assinatura nos settings do projeto. Conforme configurado no projeto, o JWT Bearer Authentication exige uma chave secreta (definida via configuração `JwtExtension:Secret`) e valida emissor/audiência conforme valores especificados em `JwtExtension:Issuer` e `JwtExtension:ValidAt`. Isso significa que apenas tokens emitidos pelo próprio sistema e destinados ao domínio da aplicação serão aceitos.

* **PBKDF2 para senhas:** Em vez de armazenar senhas em texto plano ou usar hashing simples, o sistema segue as melhores práticas utilizando um algoritmo de derivação de chave resistente a força bruta. O PBKDF2 (Password-Based Key Derivation Function 2) é atualmente um dos algoritmos recomendados para hash de senha em .NET. De fato, a própria Identity da Microsoft usa PBKDF2 por padrão. Esse algoritmo aplica milhares de iterações de um hash (HMAC-SHA256, por exemplo) com sal (salt) para produzir um hash seguro da senha. Isso torna extremamente lento e impraticável obter a senha original mesmo que o hash seja comprometido, comparado a algoritmos simples. Segundo a OWASP, *“no .NET Core, o algoritmo mais forte para hash de senhas é o PBKDF2, implementado em Microsoft.AspNetCore.Cryptography.KeyDerivation.Pbkdf2”* – o que justifica sua utilização. No contexto do SalesSystem, quando um usuário se registra ou altera sua senha, o sistema provavelmente utiliza PBKDF2 para gerar o hash armazenado no banco. No login, o hash da senha fornecida é comparado ao hash armazenado (realizando o mesmo procedimento de derivação) para verificar se coincidem, ao invés de comparar senhas diretamente.

Com essa combinação, a aplicação garante um **alto nível de segurança**: senhas protegidas contra ataques de dicionário/rainbow-tables graças ao PBKDF2, e autenticação robusta e sem estado via tokens JWT (facilitando escalabilidade horizontal do backend e integração entre serviços).

### Mapeamento de objetos com AutoMapper

O projeto faz uso do **AutoMapper** para simplificar a conversão de objetos entre as camadas. Em uma arquitetura em camadas e com CQRS, é comum termos diferentes modelos:

* Entidades de Domínio (ricas em comportamento).
* DTOs (Data Transfer Objects) expostos nas APIs ou utilizados nas queries/comandos (geralmente modelos mais simples, apenas com dados necessários).
* ViewModels ou contratos de entrada/saída das controllers.

Ao invés de escrever manualmente código para copiar propriedades de um objeto para outro, o **AutoMapper** realiza esse mapeamento de forma automática, baseada em convenções. Ele permite definir *profiles* de mapeamento e então transformar, por exemplo, uma entidade `Product` em um `ProductDto` com uma única chamada. O AutoMapper é descrito como *“um mapeador objeto-objeto baseado em convenções: mapeia automaticamente de modelos complexos para destinos simples e planos (DTOs), sem necessidade de configuração adicional além das convenções de nomenclatura”*. Isso reduz drasticamente o código boilerplate na camada de aplicação e apresentação.

No SalesSystem.API, o AutoMapper provavelmente é configurado durante a inicialização (registrando os profiles de mapeamento), e então injetado nos handlers ou controllers que precisam converter dados. Por exemplo, ao retornar os dados de um registro recém-criado, um handler pode usar AutoMapper para converter a entidade de domínio para um DTO de resposta que será serializado em JSON. Isso melhora a **manutenção** (menos código repetitivo para atualizar se um campo for adicionado, por exemplo) e diminui erros, pois as conversões seguem um padrão centralizado.

### Organização modular por contexto de negócio

Notavelmente, o projeto está organizado não apenas por camadas, mas também por **contextos de negócio (bounded contexts)**. Pela estrutura de pastas/projetos, podemos identificar módulos como *Catalog* (catalogação de produtos), *Sales* (vendas), *Payments* (pagamentos), *Registers* (cadastro de usuários/clientes), entre outros, cada um com suas subcamadas (Domain, Application, Infrastructure próprias). Essa modularização interna alinha-se ao DDD, onde cada contexto de domínio é relativamente isolado. Inclusive, existe um projeto *Payments.ACL* – ACL refere-se a **Anti-Corruption Layer**, sugerindo integração com um sistema externo ou adaptação entre contextos (por exemplo, se o contexto de Pagamentos precisar consumir algo de outro contexto de forma isolada para não corromper suas regras, ele passa por uma camada anticorrupção).

Os **BuildingBlocks** (blocos de construção) como *SalesSystem.EventSourcing* e *SalesSystem.Email* contêm funcionalidades reutilizáveis entre contextos. Por exemplo, *EventSourcing* pode fornecer infraestrutura genérica para qualquer agregado usar (publicar eventos, armazenar eventos, aplicar eventos), enquanto *Email* provavelmente fornece serviço de envio de emails (talvez usado em casos de envio de confirmação, recuperação de senha, etc).

Essa organização de pastas/projetos reflete uma arquitetura **monolítica modular** – tudo dentro de um mesmo deploy (monólito), porém bem dividido em camadas e módulos de domínio. Isso traz o melhor dos dois mundos: durante o desenvolvimento, é tudo parte de uma solução única (facilitando transações entre contextos e compartilhamento de código comum), mas conceitualmente cada contexto é independente e **pode evoluir ou até ser extraído para um microserviço separado futuramente** sem grandes impactos. De fato, uma das vantagens apontadas para esse tipo de arquitetura é possibilitar transição para microsserviços se necessário, ou simplesmente manter um monólito organizado. Conforme documentação de projetos similares, essa arquitetura é adequada tanto para *monólitos quanto para microservices*, bastando, no caso de microserviços, separar cada contexto em sua própria solução/deploy.

### Estratégias de testes e extensão futura

A adoção das práticas acima naturalmente melhora a testabilidade do sistema. Com camadas bem separadas e dependências invertidas, é possível escrever **testes unitários** para as regras de negócio no Domínio (sem depender de banco ou detalhes externos, usando repositórios em memória ou mocks) e para os **handlers** da Application (testando a lógica de aplicação em isolamento, simulando dependências). O projeto inclusive contém projetos dedicados a testes (unitários e de integração), indicando a preocupação em cobrir o comportamento com testes automatizados. A arquitetura baseada em interfaces permite usar mocks ou stubs nos testes para emular, por exemplo, resultados de repositórios ou envio de mensagens.

Para testes mais abrangentes, a presença de **IntegrationTests** sugere cenários onde o sistema todo é executado (possivelmente em memória ou usando banco de teste) para validar casos de uso completos – por exemplo, garantindo que ao chamar a API de criar pedido, o registro seja persistido, o evento publicado no RabbitMQ, etc.

Como a solução força as camadas de apresentação a dependerem apenas do projeto de Domain (e possivelmente de um projeto IoC/DI), é possível rodar testes do domínio sem carregar a API ou Infra. Isso facilita um estilo **TDD** na camada de negócio. Segundo um guia de arquitetura similar, essa organização suporta bem *“TDD ou testes de unidade (basta adicionar um projeto de UnitTest que referencie apenas Domain e IoC)”*, garantindo que os testes focam nas regras e contratos, não em detalhes de implementação.

Em termos de **extensibilidade futura**, o sistema está preparado para crescer de maneira sustentável. Novos módulos de negócio podem ser adicionados seguindo o mesmo padrão (por exemplo, adicionar um contexto **Logística** com Domain/Application/Infrastructure), sem necessidade de alterar os existentes – basta plugá-lo na API e configurar DI. Da mesma forma, trocar detalhes tecnológicos é mais fácil: se no futuro optar-se por outro broker de mensageria ou outro provedor de banco, as mudanças ficam isoladas na camada de Infrastructure, sem quebrar o restante. A compatibilidade com padrões de mercado (CQRS, DDD, Clean Architecture) também torna mais fácil a entrada de novos desenvolvedores no projeto.

Por fim, a arquitetura adotada é adequada tanto para permanecer como um monólito bem estruturado quanto, se as demandas crescerem, evoluir para uma arquitetura distribuída. A separação por contexto significa que cada parte poderia, em tese, virar um microsserviço independente – já usando RabbitMQ para comunicação e com seus próprios bancos (PostgreSQL/Mongo). Essa flexibilidade é intencional no design. Como mencionado anteriormente, a solução é **sustentável para monólitos ou microserviços**, suportando autenticação JWT, princípios SOLID, e padrões como CQRS desde o início. Em suma, as decisões técnicas tomadas no SalesSystem.API visam construir um sistema robusto, de fácil manutenção e pronto para evoluir conforme as necessidades de negócio.

**Fontes**:

* Hernaski, *Domain-Driven Design Solution (.NET 5)* – Estrutura de camadas protegendo domínio e dependendo de interfaces
* Awesome .NET – Definições de **Mediator.Net** e **Rebus**
* *Stack Overflow* – Uso de snapshots assíncronos fora do event stream (Event Sourcing)
* OWASP Cheat Sheets – Recomendação do PBKDF2 para hash de senhas em .NET
* Documentação AutoMapper – Mapeamento objeto-objeto por convenções (DTOs)
* Trechos do código do projeto (SalesSystem.API) – Definição de endpoints de autenticação (SignUp/SignIn), configuração JWT no projeto.
