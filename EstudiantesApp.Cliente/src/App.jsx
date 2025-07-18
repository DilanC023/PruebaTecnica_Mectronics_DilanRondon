import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Login from "./pages/Login";
import RegistroEstudiante from "./pages/RegistroEstudiante";
import RegistroProfesor from "./pages/RegistroProfesor";
import MateriasEstudiante from "./pages/MateriasEstudiante";
import CompanerosClase from "./pages/CompanerosClase";
import PanelProfesor from "./pages/PanelProfesor";
import ProtectedRoute from "./components/ProtectedRoute";

function App() {
  return (
    <>
      <Navbar />
      <Routes>
        {/* Ruta por defecto */}
        <Route path="/" element={<Login />} />

        {/* Rutas públicas */}
        <Route path="/registro-estudiante" element={<RegistroEstudiante />} />
        <Route path="/registro-profesor" element={<RegistroProfesor />} />

        {/* Rutas protegidas */}
        <Route
          path="/materias"
          element={
            <ProtectedRoute rol="Estudiante">
              <MateriasEstudiante />
            </ProtectedRoute>
          }
        />
        <Route
          path="/companeros"
          element={
            <ProtectedRoute rol="Estudiante">
              <CompanerosClase />
            </ProtectedRoute>
          }
        />
        <Route
          path="/panel-profesor"
          element={
            <ProtectedRoute rol="Profesor">
              <PanelProfesor />
            </ProtectedRoute>
          }
        />
      </Routes>
    </>
  );
}

export default App;
