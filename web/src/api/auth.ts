import api from './axios'
import type { User } from '../types'

export const loginApi = (email: string, password: string) =>
  api.post<User>('/api/login', { email, password })
