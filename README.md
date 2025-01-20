# Balcão UFF

## Visão Geral
O **Balcão UFF** é um sistema projetado para fomentar a colaboração, sustentabilidade e economia circular no ambiente universitário da Universidade Federal Fluminense (UFF). A plataforma permite que alunos, professores e funcionários troquem, vendam ou doem bens e serviços de forma segura e eficiente, promovendo o senso de comunidade e aproveitando recursos de forma inteligente.

Este projeto foi desenvolvido como trabalho da matéria de Gerência de Projetos e Manutenção de Software.

## Tecnologias Utilizadas
- **Frontend**: React
- **Backend**: .NET Core 8.0

## Funcionalidades Principais
### Autenticação
- Os usuários se autenticam utilizando suas credenciais. Os primeiros usuários do banco têm senha 123.

### Criação de Anúncio
- Formulário com campos para:
  - Descrição detalhada
  - Fotos do item ou serviço
  - Tipo de anúncio (oferta ou busca)
  - Preço
  - Informações de contato
  - Localização
  - Categoria (ex.: livros, roupas, móveis, aulas particulares)

### Busca de Anúncio
- Filtros de pesquisa:
  - Categoria
  - Localização
  - Faixa de preço
- Exibição de informações detalhadas do anúncio e do anunciante.

### Comunicação
- Sistema de mensagens integrado para contatos entre compradores e vendedores.
- Privacidade garantida para ambos os lados.

### Perfil
- Visualização de informações pessoais como:
  - Nome
  - Reputação (baseada em avaliações de transações anteriores)
  - Histórico de participações

### Meus Anúncios
- Visualização de todos os anúncios criados pelo usuário.
- Opção de acessar os chats relacionados aos anúncios ativos.
- Avaliação de anúncios finalizados.

## Instruções de Instalação
### Requisitos
- React para o frontend
- .NET Core 8.0 para o backend
- Banco de dados SQLServer

### Configuração do Frontend
1. Navegue até o diretório do frontend.
2. Execute `npm install` para instalar as dependências.
3. Inicie o servidor de desenvolvimento com `npm start`.

### Configuração do Backend
1. Navegue até o diretório do backend.
2. Execute `docker-compose up` para iniciar o backend.

## Licença
Este projeto está licenciado sob os termos da [Licença MIT](LICENSE).

