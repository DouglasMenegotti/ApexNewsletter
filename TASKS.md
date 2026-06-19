# Tarefas — Frontend ApexNewsletter

> React + TypeScript (Vite) · SPA simples · API: `http://localhost:5216`
> Setup, CORS e estrutura de pastas já concluídos.

---

## Douglas — Componentes base + Home

- [ ] `Navbar` com links para Home, Newsletters, Usuários (estilizada)
- [ ] `Modal` simples (overlay + div centralizada) para formulários
- [ ] `Toast` para feedback de sucesso/erro da API
- [ ] Página **Home** (`/`)
  - [ ] Banner com nome do projeto e descrição curta
  - [ ] Botão "Raspar Notícias" → `GET /raspar-noticias`, loading + resultado
  - [ ] Cards com as últimas newsletters salvas

---

## Israel — Página Newsletters (CRUD completo)

- [ ] Tabela com todas as newsletters (`GET /api/newsletters`)
- [ ] Botão "Nova Newsletter" → abre Modal com formulário (Title, SubTitle, Content)
- [ ] Botão "Editar" por linha → Modal preenchido → `PUT /api/newsletters`
- [ ] Botão "Excluir" por linha → confirmação → `DELETE /api/newsletters/{id}`
- [ ] Validação: Title e Content obrigatórios antes de enviar
- [ ] Feedback de sucesso/erro via Toast

---

## Eduarda — Página Usuários (CRUD completo) + CSS global

- [ ] Tabela com todos os usuários (`GET /api/users`)
- [ ] Botão "Novo Usuário" → Modal com formulário (Name, Email, PasswordHash)
- [ ] Botão "Editar" por linha → `PUT /api/users`
- [ ] Botão "Excluir" por linha → `DELETE /api/users/{id}`
- [ ] Validação: Name e Email obrigatórios
- [ ] Feedback de sucesso/erro via Toast
- [ ] `global.css`: reset, fontes, paleta de cores (tema escuro/racing)
- [ ] Tabelas com linhas alternadas
- [ ] Formulários com botões diferenciados (salvar = verde, excluir = vermelho)
- [ ] Loading spinner para chamadas à API

---

## Endpoints da API

| Método | Rota | Uso |
|--------|------|-----|
| GET | `/raspar-noticias` | Scraping + resumo IA |
| GET | `/api/newsletters` | Listar newsletters |
| POST | `/api/newsletters` | Criar newsletter |
| PUT | `/api/newsletters` | Editar newsletter |
| DELETE | `/api/newsletters/{id}` | Remover newsletter |
| GET | `/api/users` | Listar usuários |
| POST | `/api/users` | Criar usuário |
| PUT | `/api/users` | Editar usuário |
| DELETE | `/api/users/{id}` | Remover usuário |
