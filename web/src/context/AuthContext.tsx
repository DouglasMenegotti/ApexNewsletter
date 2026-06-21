import { createContext, useContext, useState } from 'react'
import type { User } from '../types'

interface AuthContextType {
  user: User | null
  login: (user: User) => void
  logout: () => void
}

const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(() => {
    const stored = localStorage.getItem('apex_user')
    return stored ? JSON.parse(stored) : null
  })

  function login(user: User) {
    setUser(user)
    localStorage.setItem('apex_user', JSON.stringify(user))
  }

  function logout() {
    setUser(null)
    localStorage.removeItem('apex_user')
  }

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth precisa estar dentro do AuthProvider')
  return ctx
}
