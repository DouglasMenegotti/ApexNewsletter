export interface Newsletter {
  id: string
  title: string
  subTitle: string
  content: string
  createdAt: string
}

export interface User {
  id: string
  name: string
  email: string
  passwordHash: string
  createdAt: string
  isActive: boolean
}
