import { useEffect, useState } from "react";
import { obtenerCompanerosClase } from "../api/estudianteService";
import { useAuth } from "../auth/useAuth";
import Layout from "../components/Layout";

const CompanerosClase = () => {
  const [data, setData] = useState([]);
  const { auth } = useAuth();

  useEffect(() => {
    obtenerCompanerosClase(auth.usuarioId).then(res => {
      if (res.esExitosa) setData(res.datos);
    });
  }, []);

  return (
    <Layout title="Compañeros de Clase">
      <div className="max-w-3xl mx-auto mt-8">
        <h2 className="text-xl font-bold mb-4">Compañeros por Materia</h2>
        {data.map(grupo => (
          <div key={grupo.materiaId} className="mb-6">
            <h3 className="font-semibold">{grupo.materiaNombre}</h3>
            <ul className="ml-4 list-disc">
              {grupo.compañeros.map(c => (
                <li key={c.id}>{c.nombre}</li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </Layout>
    
  );
};

export default CompanerosClase;
