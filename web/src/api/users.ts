import api from './axios'
import type { User } from '../types'

export const getUsers = () => api.get<User[]>('/api/users')
export const getUser = (id: string) => api.get<User>(`/api/users/${id}`)
export const createUser = (data: Omit<User, 'id' | 'createdAt' | 'isActive'>) => api.post('/api/users', data)
export const updateUser = (data: User) => api.put('/api/users', data)
export const deleteUser = (id: string) => api.delete(`/api/users/${id}`)
