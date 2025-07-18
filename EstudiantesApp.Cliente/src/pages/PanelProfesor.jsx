import { useEffect, useState } from "react";
import { obtenerMaterias } from "../api/materiaService";
import Layout from "../components/Layout";

const PanelProfesor = () => {
  const [materias, setMaterias] = useState([]);

  useEffect(() => {
    obtenerMaterias().then(res => {
      if (res.esExitosa) setMaterias(res.datos);
    });
  }, []);

  return (
    <Layout title="Panel del Profesor">
      <div className="max-w-2xl mx-auto mt-10">
        <h2 className="text-xl font-bold mb-4">Materias que Impartes</h2>
        <ul className="list-disc ml-6 space-y-2">
          {materias.map(m => (
            <li key={m.id}>
              {m.nombre} - {m.creditos} créditos
            </li>
          ))}
        </ul>
      </div>

    </Layout>
  );
};

export default PanelProfesor;
