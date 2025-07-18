import { useState } from "react";
import { registrarEstudiante } from "../api/estudianteService";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";

const RegistroEstudiante = () => {
  const [form, setForm] = useState({ nombre: "", email: "", clave: "" });
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await registrarEstudiante(form);
    if (res.esExitosa) {
      alert("Estudiante registrado correctamente");
      navigate("/");
    } else {
      alert(res.mensaje);
    }
  };

  return (
    <Layout title="Registro de Estudiante">
      <div className="max-w-md mx-auto mt-10">
        <h2 className="text-xl font-bold mb-4">Registro de Estudiante</h2>
        <form onSubmit={handleSubmit} className="flex flex-col gap-3">
          <input type="text" placeholder="Nombre" required
           className="border rounded p-2 w-full"
            value={form.nombre}
            onChange={(e) => setForm({ ...form, nombre: e.target.value })}
          />
          <input type="email" placeholder="Email" required
           className="border rounded p-2 w-full"
            value={form.email}
            onChange={(e) => setForm({ ...form, email: e.target.value })}
          />
          <input type="password" placeholder="Clave" required
            className="border rounded p-2 w-full"
            value={form.clave}
            onChange={(e) => setForm({ ...form, clave: e.target.value })}
          />
          <button className="bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700" type="submit">
            Registrarse
          </button>
        </form>
      </div>

    </Layout>
  );
};

export default RegistroEstudiante;
