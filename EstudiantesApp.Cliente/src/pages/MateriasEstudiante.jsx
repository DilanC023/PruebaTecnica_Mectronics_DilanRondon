import { useEffect, useState } from "react";
import { obtenerMaterias } from "../api/materiaService";
import { inscribirMateria } from "../api/estudianteService";
import { useAuth } from "../auth/useAuth";
import Layout from "../components/Layout";

const MateriasEstudiante = () => {
  const [materias, setMaterias] = useState([]);
  const [seleccionadas, setSeleccionadas] = useState([]);
  const { auth } = useAuth();

  useEffect(() => {
    obtenerMaterias().then(res => {
      if (res.esExitosa) setMaterias(res.datos);
    });
  }, []);

  const toggleSeleccion = (id) => {
    setSeleccionadas(prev =>
      prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]
    );
  };

  const handleInscribir = async () => {
    if (seleccionadas.length !== 3) {
      alert("Debes seleccionar exactamente 3 materias.");
      return;
    }

    for (let materiaId of seleccionadas) {
      const res = await inscribirMateria({ estudianteId: auth.usuarioId, materiaId });
      if (!res.esExitosa) {
        alert(res.mensaje);
        return;
      }
    }

    alert("Inscripción exitosa.");
  };

  return (
    <Layout title="Inscripción de Materias">
      <div className="max-w-2xl mx-auto mt-8">
        <h2 className="text-xl font-bold mb-4">Inscripción de Materias</h2>
        <ul className="space-y-2">
          {materias.map(m => (
            <li key={m.id}>
              <label className="flex items-center gap-2">
                <input
                  type="checkbox"
                  disabled={!seleccionadas.includes(m.id) && seleccionadas.length >= 3}
                  checked={seleccionadas.includes(m.id)}
                  onChange={() => toggleSeleccion(m.id)}
                />
                {m.nombre} - {m.profesor?.nombre}
              </label>
            </li>
          ))}
        </ul>
        <button onClick={handleInscribir} className="mt-4 bg-blue-600 text-white px-4 py-2 rounded">
          Inscribirse
        </button>
      </div>
    </Layout>

  );
};

export default MateriasEstudiante;
