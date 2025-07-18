import { Navigate } from "react-router-dom";
import { useAuth } from "../auth/useAuth";

const ProtectedRoute = ({ children, rol }) => {
  const { auth } = useAuth();

  if (!auth) {
    return <Navigate to="/" replace />;
  }

  if (rol && auth.rol !== rol) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default ProtectedRoute;
