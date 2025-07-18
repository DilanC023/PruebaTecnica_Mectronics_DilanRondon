import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

const Navbar = () => {
  const { auth, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <nav className="bg-blue-600 text-white px-4 py-3 flex justify-between items-center">
      <div className="text-xl font-bold">EstudiantesApp</div>

      {auth ? (
        <div className="flex gap-4 items-center">
          <span>{auth.nombre}</span>

          {auth.rol === "Estudiante" && (
            <>
              <Link to="/materias" className="hover:underline">Materias</Link>
              <Link to="/companeros" className="hover:underline">Compañeros</Link>
            </>
          )}

          {auth.rol === "Profesor" && (
            <>
              <Link to="/panel-profesor" className="hover:underline">Mis Materias</Link>
            </>
          )}

          <button onClick={handleLogout} className="bg-red-500 px-3 py-1 rounded hover:bg-red-700">
            Cerrar sesión
          </button>
        </div>
      ) : (
        <div className="flex gap-4">
          <Link to="/" className="hover:underline">Iniciar Sesión</Link>
          <Link to="/registro-estudiante" className="hover:underline">Registro</Link>
        </div>
      )}
    </nav>
  );
};

export default Navbar;
