import api from './http';

export const registrarProfesor = (data) =>
  api.post("/profesores", data).then(res => res.data);

export const obtenerProfesores = () =>
  api.get("/profesores").then(res => res.data);

export const obtenerProfesorPorId = (id) =>
  api.get(`/profesores/${id}`).then(res => res.data);

export const modificarProfesor = (id, data) =>
  api.put(`/profesores/modificarprofesor/${id}`, data).then(res => res.data);

export const deshabilitarProfesor = (id) =>
  api.put(`/profesores/deshabilitarprofesor/${id}`).then(res => res.data);
