import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Navbar from './components/Navbar'
import Home from './pages/Home'
import Newsletters from './pages/Newsletters'
import Users from './pages/Users'
import './global.css'

export default function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/newsletters" element={<Newsletters />} />
        <Route path="/usuarios" element={<Users />} />
      </Routes>
    </BrowserRouter>
  )
}
