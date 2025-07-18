import Layout from "../components/Layout";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

const Login = () => {
  const { login, auth } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    const response = await login({ email, password });
    if (response.esExitosa) {
      const rol = response.datos.rol;
      if (rol === "Estudiante") navigate("/materias");
      else if (rol === "Profesor") navigate("/panel-profesor");
    } else {
      alert(response.mensaje);
    }
  };

  return (
    <Layout title="Iniciar Sesión">
      <form onSubmit={handleSubmit} className="flex flex-col gap-4">
        <input
          type="email"
          placeholder="Correo"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          className="border rounded p-2 w-full"
        />
        <input
          type="password"
          placeholder="Clave"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
         className="border rounded p-2 w-full"
        />
        <button type="submit" className="bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700">
          Ingresar
        </button>
      </form>
    </Layout>
  );
};

export default Login;
