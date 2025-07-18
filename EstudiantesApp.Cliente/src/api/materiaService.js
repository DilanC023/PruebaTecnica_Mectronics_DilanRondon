import api from './http';

export const obtenerMaterias = () =>
  api.get("/materias").then(res => res.data);

export const obtenerMateriaPorId = (id) =>
  api.get(`/materias/${id}`).then(res => res.data);

export const crearMateria = (data) =>
  api.post("/materias", data).then(res => res.data);

export const modificarMateria = (id, data) =>
  api.put(`/materias/modificarmateria/${id}`, data).then(res => res.data);

export const deshabilitarMateria = (id) =>
  api.put(`/materias/deshabilitarmateria/${id}`).then(res => res.data);
