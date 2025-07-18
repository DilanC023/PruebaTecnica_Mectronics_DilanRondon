import { createContext, useContext, useState } from "react";
import { login as loginService } from "../api/authService";

// Esto estaba faltando antes
export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState(() => {
    const stored = localStorage.getItem("auth");
    return stored ? JSON.parse(stored) : null;
  });

  const login = async (credenciales) => {
    try {
      const response = await loginService(credenciales);
      if (response.esExitosa) {
        setAuth(response.datos);
        localStorage.setItem("auth", JSON.stringify(response.datos));
        localStorage.setItem("token", response.datos.token);
      }
      return response;
    } catch (error) {
      return {
        esExitosa: false,
        mensaje: "Error en la autenticación",
      };
    }
  };

  const logout = () => {
    setAuth(null);
    localStorage.removeItem("auth");
    localStorage.removeItem("token");
  };

  return (
    <AuthContext.Provider value={{ auth, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
