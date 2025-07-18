namespace EstudiantesApp.API.Responses
{
    public class APIResponse<T>
    {
        public bool EsExitosa { get; set; }
        public string Mensaje { get; set; }
        public T Datos { get; set; }
        public List<string> Errores { get; set; } = new List<string>();

        public static APIResponse<T> RespuestaExitosa(T data, string message = "Operación exitosa")
        {
            return new APIResponse<T>
            {
                EsExitosa = true,
                Mensaje = message,
                Datos = data
            };
        }

        public static APIResponse<T> RespuestaError(string errorMessage, List<string> errors = null)
        {
            return new APIResponse<T>
            {
                EsExitosa = false,
                Mensaje = errorMessage,
                Errores = errors ?? new List<string>()
            };
        }
    }
}
