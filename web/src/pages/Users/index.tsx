import { useState, useEffect } from 'react'
import { getUsers, createUser, updateUser, deleteUser } from '../../api/users'
import type { User } from '../../types'
import Modal from '../../components/Modal'
import Toast from '../../components/Toast'

interface FormData {
  name: string
  email: string
  passwordHash: string
}

const emptyForm: FormData = { name: '', email: '', passwordHash: '' }

export default function Users() {
  const [users, setUsers] = useState<User[]>([])
  const [loading, setLoading] = useState(true)
  const [modalOpen, setModalOpen] = useState(false)
  const [editing, setEditing] = useState<User | null>(null)
  const [form, setForm] = useState<FormData>(emptyForm)
  const [errors, setErrors] = useState<Partial<FormData>>({})
  const [saving, setSaving] = useState(false)
  const [toast, setToast] = useState<{ message: string; type: 'success' | 'error' } | null>(null)

  useEffect(() => {
    load()
  }, [])

  async function load() {
    try {
      const r = await getUsers()
      setUsers(r.data ?? [])
    } catch {
      setUsers([])
    } finally {
      setLoading(false)
    }
  }

  function openCreate() {
    setEditing(null)
    setForm(emptyForm)
    setErrors({})
    setModalOpen(true)
  }

  function openEdit(u: User) {
    setEditing(u)
    setForm({ name: u.name, email: u.email, passwordHash: u.passwordHash })
    setErrors({})
    setModalOpen(true)
  }

  function closeModal() {
    setModalOpen(false)
    setEditing(null)
    setForm(emptyForm)
    setErrors({})
  }

  function validate(): boolean {
    const e: Partial<FormData> = {}
    if (!form.name.trim()) e.name = 'Nome é obrigatório'
    if (!form.email.trim()) e.email = 'E-mail é obrigatório'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!validate()) return
    setSaving(true)
    try {
      if (editing) {
        await updateUser({ ...editing, ...form })
        setToast({ message: 'Usuário atualizado com sucesso!', type: 'success' })
      } else {
        await createUser(form)
        setToast({ message: 'Usuário criado com sucesso!', type: 'success' })
      }
      closeModal()
      load()
    } catch {
      setToast({ message: 'Erro ao salvar. Tente novamente.', type: 'error' })
    } finally {
      setSaving(false)
    }
  }

  async function handleDelete(id: string) {
    if (!confirm('Deseja excluir este usuário?')) return
    try {
      await deleteUser(id)
      setToast({ message: 'Usuário removido!', type: 'success' })
      load()
    } catch {
      setToast({ message: 'Erro ao remover usuário.', type: 'error' })
    }
  }

  return (
    <main className="page">
      <div className="page-header">
        <h1>Usuários</h1>
        <button className="btn btn-primary" onClick={openCreate}>
          + Novo Usuário
        </button>
      </div>

      {loading ? (
        <div className="loading-center">
          <div className="spinner" />
        </div>
      ) : users.length === 0 ? (
        <p className="empty">Nenhum usuário cadastrado.</p>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Nome</th>
                <th>E-mail</th>
                <th>Status</th>
                <th>Criado em</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {users.map(u => (
                <tr key={u.id}>
                  <td>{u.name}</td>
                  <td>{u.email}</td>
                  <td>
                    <span className={`badge badge--${u.isActive ? 'active' : 'inactive'}`}>
                      {u.isActive ? 'Ativo' : 'Inativo'}
                    </span>
                  </td>
                  <td>{new Date(u.createdAt).toLocaleDateString('pt-BR')}</td>
                  <td>
                    <div className="td-actions">
                      <button className="btn btn-ghost" onClick={() => openEdit(u)}>
                        Editar
                      </button>
                      <button className="btn btn-danger" onClick={() => handleDelete(u.id)}>
                        Excluir
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Modal
        isOpen={modalOpen}
        onClose={closeModal}
        title={editing ? 'Editar Usuário' : 'Novo Usuário'}
      >
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Nome *</label>
            <input
              value={form.name}
              onChange={e => setForm(f => ({ ...f, name: e.target.value }))}
              placeholder="Nome completo"
            />
            {errors.name && <span className="form-error">{errors.name}</span>}
          </div>

          <div className="form-group">
            <label>E-mail *</label>
            <input
              type="email"
              value={form.email}
              onChange={e => setForm(f => ({ ...f, email: e.target.value }))}
              placeholder="email@exemplo.com"
            />
            {errors.email && <span className="form-error">{errors.email}</span>}
          </div>

          <div className="form-group">
            <label>Senha</label>
            <input
              type="password"
              value={form.passwordHash}
              onChange={e => setForm(f => ({ ...f, passwordHash: e.target.value }))}
              placeholder="Senha"
            />
          </div>

          <div className="form-actions">
            <button type="button" className="btn btn-ghost" onClick={closeModal}>
              Cancelar
            </button>
            <button type="submit" className="btn btn-success" disabled={saving}>
              {saving ? 'Salvando...' : 'Salvar'}
            </button>
          </div>
        </form>
      </Modal>

      {toast && (
        <Toast message={toast.message} type={toast.type} onClose={() => setToast(null)} />
      )}
    </main>
  )
}
