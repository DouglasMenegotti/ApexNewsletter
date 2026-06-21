import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Navbar() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  function handleLogout() {
    logout()
    navigate('/login')
  }

  return (
    <nav className="navbar">
      <NavLink to="/" className="navbar__brand">
        Apex Newsletter
      </NavLink>
      <ul className="navbar__links">
        <li>
          <NavLink to="/" end className={({ isActive }) => isActive ? 'active' : ''}>
            Home
          </NavLink>
        </li>
        <li>
          <NavLink to="/newsletters" className={({ isActive }) => isActive ? 'active' : ''}>
            Newsletters
          </NavLink>
        </li>
        <li>
          <NavLink to="/usuarios" className={({ isActive }) => isActive ? 'active' : ''}>
            Usuários
          </NavLink>
        </li>
      </ul>
      <div className="navbar__user">
        <span className="navbar__username">{user?.name}</span>
        <button className="btn btn-ghost" onClick={handleLogout}>Sair</button>
      </div>
    </nav>
  )
}
