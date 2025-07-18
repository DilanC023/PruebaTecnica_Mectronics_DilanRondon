import api from './http';

export const login = (data) => api.post('/auth/iniciarsesion', data)
                                  .then(res => res.data);
