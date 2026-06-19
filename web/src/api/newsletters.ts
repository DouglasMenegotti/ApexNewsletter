import api from './axios'
import type { Newsletter } from '../types'

export const getNewsletters = () => api.get<Newsletter[]>('/api/newsletters')
export const getNewsletter = (id: string) => api.get<Newsletter>(`/api/newsletters/${id}`)
export const createNewsletter = (data: Omit<Newsletter, 'id' | 'createdAt'>) => api.post('/api/newsletters', data)
export const updateNewsletter = (data: Newsletter) => api.put('/api/newsletters', data)
export const deleteNewsletter = (id: string) => api.delete(`/api/newsletters/${id}`)
export const scrapeNews = () => api.get('/raspar-noticias')
