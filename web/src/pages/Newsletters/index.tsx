import { useState, useEffect } from 'react'
import {
  getNewsletters,
  createNewsletter,
  updateNewsletter,
  deleteNewsletter,
} from '../../api/newsletters'
import type { Newsletter } from '../../types'
import Modal from '../../components/Modal'
import Toast from '../../components/Toast'

interface FormData {
  title: string
  subTitle: string
  content: string
}

const emptyForm: FormData = { title: '', subTitle: '', content: '' }

export default function Newsletters() {
  const [newsletters, setNewsletters] = useState<Newsletter[]>([])
  const [loading, setLoading] = useState(true)
  const [modalOpen, setModalOpen] = useState(false)
  const [editing, setEditing] = useState<Newsletter | null>(null)
  const [form, setForm] = useState<FormData>(emptyForm)
  const [errors, setErrors] = useState<Partial<FormData>>({})
  const [saving, setSaving] = useState(false)
  const [toast, setToast] = useState<{ message: string; type: 'success' | 'error' } | null>(null)

  useEffect(() => {
    load()
  }, [])

  async function load() {
    try {
      const r = await getNewsletters()
      setNewsletters(r.data ?? [])
    } catch {
      setNewsletters([])
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

  function openEdit(n: Newsletter) {
    setEditing(n)
    setForm({ title: n.title, subTitle: n.subTitle ?? '', content: n.content })
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
    if (!form.title.trim()) e.title = 'Título é obrigatório'
    if (!form.content.trim()) e.content = 'Conteúdo é obrigatório'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    if (!validate()) return
    setSaving(true)
    try {
      if (editing) {
        await updateNewsletter({ ...editing, ...form })
        setToast({ message: 'Newsletter atualizada com sucesso!', type: 'success' })
      } else {
        await createNewsletter(form)
        setToast({ message: 'Newsletter criada com sucesso!', type: 'success' })
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
    if (!confirm('Deseja excluir esta newsletter?')) return
    try {
      await deleteNewsletter(id)
      setToast({ message: 'Newsletter removida!', type: 'success' })
      load()
    } catch {
      setToast({ message: 'Erro ao remover newsletter.', type: 'error' })
    }
  }

  return (
    <main className="page">
      <div className="page-header">
        <h1>Newsletters</h1>
        <button className="btn btn-primary" onClick={openCreate}>
          + Nova Newsletter
        </button>
      </div>

      {loading ? (
        <div className="loading-center">
          <div className="spinner" />
        </div>
      ) : newsletters.length === 0 ? (
        <p className="empty">Nenhuma newsletter cadastrada.</p>
      ) : (
        <div className="table-wrapper">
          <table>
            <thead>
              <tr>
                <th>Título</th>
                <th>Subtítulo</th>
                <th>Criado em</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {newsletters.map(n => (
                <tr key={n.id}>
                  <td>{n.title}</td>
                  <td>{n.subTitle || '—'}</td>
                  <td>{new Date(n.createdAt).toLocaleDateString('pt-BR')}</td>
                  <td>
                    <div className="td-actions">
                      <button className="btn btn-ghost" onClick={() => openEdit(n)}>
                        Editar
                      </button>
                      <button className="btn btn-danger" onClick={() => handleDelete(n.id)}>
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
        title={editing ? 'Editar Newsletter' : 'Nova Newsletter'}
      >
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Título *</label>
            <input
              value={form.title}
              onChange={e => setForm(f => ({ ...f, title: e.target.value }))}
              placeholder="Título da newsletter"
              maxLength={150}
            />
            {errors.title && <span className="form-error">{errors.title}</span>}
          </div>

          <div className="form-group">
            <label>Subtítulo</label>
            <input
              value={form.subTitle}
              onChange={e => setForm(f => ({ ...f, subTitle: e.target.value }))}
              placeholder="Subtítulo (opcional)"
              maxLength={250}
            />
          </div>

          <div className="form-group">
            <label>Conteúdo *</label>
            <textarea
              value={form.content}
              onChange={e => setForm(f => ({ ...f, content: e.target.value }))}
              placeholder="Conteúdo da newsletter"
              rows={6}
            />
            {errors.content && <span className="form-error">{errors.content}</span>}
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
