# Artes Bujuju's — Sistema de Gestão de Pedidos

Sistema web para a gestão de pedidos de artigos personalizados (canecas, camisetas,
almofadas, chaveiros, copos, taças e caixas) da empresa **Artes Bujuju's**, em
substituição ao controle feito por planilha.

> Projeto integrador (4º semestre) — em desenvolvimento.

## Tecnologias
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core com MariaDB (provedor Microting.EntityFrameworkCore.MySql)
- HTML, CSS e JavaScript
- Protótipo de telas no Figma

## O que já funciona
- [x] Tela de boas-vindas com carrossel automático
- [x] Layout compartilhado (barra do topo e menu do usuário)
- [x] Cadastro de cliente, gravando no banco
- [x] Login com três perfis: cliente, funcionário e gerente
- [x] Painel por perfil: o cliente vê os próprios pedidos, o funcionário vê a fila
      de produção e o gerente vê todos os pedidos
- [x] Cadastro de funcionário, restrito ao gerente
- [ ] Visual final dos painéis (conforme o protótipo do Figma)
- [ ] Catálogo de produtos
- [ ] Orçamentos e fluxo de pagamento (por enquanto, conferido manualmente)

## Regras de negócio e segurança
- O **gerente é único** e criado direto no banco; nenhuma tela do sistema cria gerente.
- Somente o **gerente cadastra funcionários**. A trava é feita no servidor
  (`[Authorize(Roles = "gerente")]`), e não apenas escondendo o botão.
- O cadastro público cria **sempre** um cliente: o perfil é decidido no servidor
  e nunca vem do formulário.
- As senhas dos usuários são guardadas como hash (SHA-256; melhoria prevista: BCrypt).
- A senha do banco fica fora do código, nos *User Secrets* do .NET.
- O cadastro valida e-mail e CPF repetidos, senha mínima e confirmação de senha.
- Usuário e cliente (ou funcionário) são gravados em uma única transação.

## Estrutura do projeto
```
banco/                         Scripts do banco de dados (MariaDB)
PrototipoSiteGestaoArtesBujujus/
├── Controllers/               Login, Cadastro, Painel e Home
├── Data/                      AppDbContext (Entity Framework)
├── Entidades/                 Classes geradas a partir das tabelas
├── Models/                    ViewModels das telas
├── Servicos/                  Hash de senha e utilitários
├── Views/                     Telas (Razor) e layout compartilhado
└── wwwroot/                   CSS, JavaScript e imagens
```

## Como executar
1. Instalar o **.NET 10 SDK** e o **MariaDB**.
2. Importar o banco: `source banco/banco_completo.sql`
3. Criar o gerente: trocar a senha em `banco/criar_gerente.sql` e executar o script.
4. Na pasta do projeto, guardar a conexão (fora do código):
   ```
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:Padrao" "Server=127.0.0.1;Port=3306;Database=item_personalizados;User=SEU_USUARIO;Password=SUA_SENHA"
   ```
5. Executar: `dotnet run`

**Contas de teste:** os clientes do script (por exemplo, `maria.silva@email.com` /
`senha1`). O gerente é o criado no passo 3, e ele cadastra os funcionários pelo sistema.

## Equipe
- [nomes do grupo]