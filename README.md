# 🏁 Apex Newsletter

> Uma plataforma de newsletter especializada em automobilismo — agregando notícias, calendários de corridas e envio de conteúdo diretamente para seus assinantes.

---

## 👥 Identificação do Projeto

| Campo | Informação |
|---|---|
| **Projeto** | Apex Newsletter |
| **Curso** | Tópicos Especiais em Sistemas |
| **Turma** | N2 |

**Integrantes:**
- Israel Wendell Cardoso Costa
- Douglas David do Amaral Menegotti
- Eduarda Luiz

---

## 📋 Resumo

A **Apex Newsletter** é uma plataforma web voltada ao universo do automobilismo profissional, desenvolvida como projeto final da disciplina de Tópicos Especiais em Sistemas. O sistema realiza a coleta automatizada de notícias por meio de *web scraping*, agregando conteúdo de diversas fontes relacionadas às principais categorias do esporte a motor — como Fórmula 1, Fórmula E, MotoGP, Stock Car, entre outras. Essas notícias são organizadas e exibidas em um blog integrado, sempre com referência à fonte original e à data de publicação. Além disso, a plataforma gerencia uma base de assinantes e realiza o envio automatizado de e-mails em massa com as principais notícias e o calendário atualizado de corridas, proporcionando uma experiência completa e centralizada para os fãs do automobilismo.

---

## ⚡ Funcionalidades

- **Web Scraping de Notícias** — coleta automática de notícias de fontes especializadas em automobilismo
- **Blog de Notícias** — exibição organizada das notícias com data, categoria e link para a fonte original
- **Calendário de Corridas** — listagem de eventos por categoria, com datas, circuitos e informações relevantes
- **Gestão de Assinantes (CRUD)** — cadastro, edição, listagem e remoção de assinantes da newsletter
- **Envio de E-mail em Massa** — disparo de newsletters para todos os assinantes com resumo das notícias recentes
- **Categorização por Modalidade** — filtragem de notícias e eventos por categoria do automobilismo (F1, MotoGP, Stock Car, etc.)
- **Relacionamento entre entidades** — notícias vinculadas a categorias previamente cadastradas no sistema

---

## 🔍 Descrição das Funcionalidades

### 🕷️ Web Scraping de Notícias
O sistema conta com um módulo de *web scraping* responsável por coletar automaticamente notícias de portais especializados em automobilismo. Cada notícia é armazenada com informações como título, resumo, data de publicação, URL da fonte original e categoria correspondente. Isso garante que o conteúdo exibido na plataforma seja sempre atual e devidamente referenciado, respeitando a autoria das fontes.

### 📰 Blog de Notícias
As notícias coletadas são exibidas em um blog interno da plataforma, organizado de forma cronológica e por categoria. Cada postagem apresenta o título, a data, um breve resumo e o link para a matéria original, permitindo que os leitores acessem o conteúdo completo diretamente na fonte. A interface é simples e objetiva, priorizando a leitura rápida e a navegação por interesse.

### 📅 Calendário de Corridas
A plataforma mantém um calendário atualizado com todas as corridas e etapas das principais categorias do automobilismo mundial. Os eventos são listados com data, local, circuito e categoria, permitindo que os assinantes acompanhem a temporada completa em um único lugar. O calendário pode ser filtrado por modalidade para facilitar a consulta.

### 👤 Gestão de Assinantes (CRUD completo)
O sistema permite o gerenciamento completo dos assinantes da newsletter. É possível cadastrar novos assinantes informando nome e e-mail, editar os dados de assinantes existentes, listar todos os inscritos e remover assinantes que solicitarem o cancelamento. Esta é a entidade principal do sistema e conta com CRUD totalmente funcional.

### 📧 Envio de E-mail em Massa
Com base na lista de assinantes cadastrados e nas notícias mais recentes coletadas pelo scraping, o sistema realiza o disparo de newsletters por e-mail. O conteúdo do e-mail inclui as principais manchetes do período, com links para as matérias completas e o próximo evento do calendário de corridas. O envio é acionado manualmente ou de forma agendada.

### 🏎️ Categorização por Modalidade
Todas as notícias e eventos são vinculados a uma categoria de automobilismo previamente cadastrada no sistema (ex: Fórmula 1, MotoGP, Endurance, Rally, Stock Car). Essa relação entre entidades permite filtrar o conteúdo exibido no blog e nas newsletters de acordo com o interesse do assinante, tornando a experiência mais personalizada.

---

## 🗂️ Repositório

O projeto é hospedado no GitHub e desenvolvido de forma colaborativa entre os integrantes da equipe.

> 🔗 **Link do repositório:** *(inserir URL do repositório aqui)*

A colaboração pode ser verificada por meio do histórico de commits, uso de branches por funcionalidade e revisões de código realizadas entre os membros.

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Uso |
|---|---|
| **C# (.NET 8 – Minimal API)** | Backend / API REST |
| **Entity Framework Core** | ORM para acesso ao banco de dados |
| **SQLite** | Banco de dados |
| **HTML/CSS + JavaScript** | Interface frontend do blog |
| **GitHub** | Versionamento e colaboração |

---

## 🤖 Uso de IA

### Ferramenta Utilizada
**Claude (Anthropic)** — modelo Claude Sonnet, acessado via [claude.ai](https://claude.ai).

### Forma de Uso
A inteligência artificial foi utilizada para apoiar a redação de toda a documentação presente neste README. Os prompts fornecidos descreviam o contexto do projeto — incluindo o tema (automobilismo), as funcionalidades planejadas (web scraping, blog, newsletter, CRUD de assinantes, calendário de corridas) e os requisitos acadêmicos da disciplina. A IA gerou os textos do resumo, a listagem de funcionalidades e as descrições detalhadas de cada funcionalidade com base nessas informações.

### Revisões Realizadas pela Equipe
Após a geração dos textos pela IA, a equipe realizou as seguintes revisões:
- Verificação da coerência técnica com o sistema real desenvolvido
- Ajuste de termos e nomes para refletir com precisão as decisões de implementação da equipe
- Adaptação do conteúdo ao formato e às diretrizes exigidas pela disciplina
- Conferência das informações dos integrantes, nome do projeto e dados do curso

### 💬 Base de Prompts Utilizada

Abaixo estão os prompts enviados à IA durante a conversa que originou esta documentação, em ordem cronológica:

---

**Prompt 1 — Contexto e geração inicial do README:**

```
📑 Documentação do Projeto (README do repositório)
* Entrega da documentação: 19/04/2026
* A documentação do projeto deverá ser realizada diretamente no README do repositório.
[requisitos completos da disciplina incluindo estrutura esperada, tecnologias e critérios de avaliação]

Nomes dos participantes:
- Israel Wendell Cardoso Costa
- Douglas David do Amaral Menegotti
- Eduarda Luiz

Nome do projeto: Apex Newsletter

O projeto será uma newsletter sobre corridas envolvendo todo o automobilismo profissional,
fazendo web scraping para trazer notícias no blog e envio de e-mail em massa.
A base das notícias virá do web scraping, referenciando a fonte, com datas de todas
as corridas e todas as categorias.
```

> **Resultado:** A IA gerou a versão inicial completa do README com todas as seções exigidas — identificação, resumo, funcionalidades, descrição detalhada, repositório, tecnologias e uso de IA.

---

**Prompt 2 — Ajuste de turma e adição dos prompts:**

```
reajuste a parte de turma N2, e adicione nossa base de prompt no readme para realizar nossa conversa
```

> **Resultado:** A IA atualizou a turma para N2 e adicionou esta seção de histórico de prompts ao README.

---

<p align="center">Feito com 🏎️ e ☕ pela equipe Apex Newsletter · 2026</p>
