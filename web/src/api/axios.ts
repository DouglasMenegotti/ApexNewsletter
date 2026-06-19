import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5216',
})

export default api
