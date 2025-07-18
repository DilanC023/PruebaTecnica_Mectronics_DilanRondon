import api from './http';

export const registrarEstudiante = (data) =>
  api.post("/estudiantes", data).then(res => res.data);

export const obtenerEstudiantes = () =>
  api.get("/estudiantes").then(res => res.data);

export const obtenerEstudiantePorId = (id) =>
  api.get(`/estudiantes/${id}`).then(res => res.data);

export const modificarEstudiante = (id, data) =>
  api.put(`/estudiantes/modificarestudiante/${id}`, data).then(res => res.data);

export const deshabilitarEstudiante = (id) =>
  api.put(`/estudiantes/deshabilitarestudiante/${id}`).then(res => res.data);

export const inscribirMateria = (data) =>
  api.post(`/estudiantes/inscribir`, data).then(res => res.data);

export const obtenerCompanerosClase = (estudianteId) =>
  api.get(`/estudiantes/companeros/${estudianteId}`).then(res => res.data);
