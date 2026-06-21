import { useState, useEffect } from 'react'
import { getNewsletters, scrapeNews } from '../../api/newsletters'
import type { Newsletter } from '../../types'
import Toast from '../../components/Toast'

export default function Home() {
  const [newsletters, setNewsletters] = useState<Newsletter[]>([])
  const [loading, setLoading] = useState(true)
  const [scraping, setScraping] = useState(false)
  const [toast, setToast] = useState<{ message: string; type: 'success' | 'error' } | null>(null)

  useEffect(() => {
    getNewsletters()
      .then(r => setNewsletters(r.data ?? []))
      .catch(() => setNewsletters([]))
      .finally(() => setLoading(false))
  }, [])

  async function handleScrape() {
    setScraping(true)
    try {
      await scrapeNews()
      const r = await getNewsletters()
      setNewsletters(r.data ?? [])
      setToast({ message: 'Notícias raspadas e salvas com sucesso!', type: 'success' })
    } catch (err: any) {
      const data = err?.response?.data
      const msg = data?.mensagem
        ? `${data.mensagem} Próxima raspagem às ${data.proximaRaspagem}.`
        : 'Erro ao raspar notícias. Verifique a API.'
      setToast({ message: msg, type: 'error' })
    } finally {
      setScraping(false)
    }
  }

  return (
    <main className="page">
      <div className="hero">
        <h1 className="hero__title">
          Apex <span>Newsletter</span>
        </h1>
        <p className="hero__desc">
          Resumos de notícias do automobilismo gerados por Inteligência Artificial
        </p>
        <button className="btn btn-primary" onClick={handleScrape} disabled={scraping}>
          {scraping ? 'Raspando notícias...' : 'Raspar Notícias'}
        </button>
      </div>

      <div className="page-header">
        <h2 className="section-title">Últimas Newsletters</h2>
      </div>

      {loading ? (
        <div className="loading-center">
          <div className="spinner" />
        </div>
      ) : newsletters.length === 0 ? (
        <p className="empty">
          Nenhuma newsletter encontrada. Clique em "Raspar Notícias" para gerar as primeiras.
        </p>
      ) : (
        <div className="cards-grid">
          {newsletters.slice(0, 6).map(n => (
            <div key={n.id} className="card">
              <h3 className="card__title">{n.title}</h3>
              {n.subTitle && <p className="card__subtitle">{n.subTitle}</p>}
              <p className="card__content">{n.content}</p>
              <p className="card__date">
                {new Date(n.createdAt).toLocaleDateString('pt-BR')}
              </p>
            </div>
          ))}
        </div>
      )}

      {toast && (
        <Toast message={toast.message} type={toast.type} onClose={() => setToast(null)} />
      )}
    </main>
  )
}
