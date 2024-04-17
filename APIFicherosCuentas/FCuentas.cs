using R24_JesusCG_V1;

namespace APIFicherosCuentas
{
    public static class FCuentas
    {

        private const string PATH = "CuentasBancarias";

        #region Métodos Públicos
        
        
        
        public static void EliminarCuenta(Cuenta CuentaAEliminar)
        {
            // RECURSOS LOCALES
            string fichero;

            // ENTRADA
            if (CuentaAEliminar == null) throw new ArgumentNullException("Eliminar Cuenta: La cuenta es Nula");

            // Obtener el nombre del fichero
            fichero = PATH;
            fichero += $"\\{GenerarNombreFichero(CuentaAEliminar)}";
            
            // Comprobar si existe
            if (!File.Exists(fichero)) throw new FileNotFoundException("El fichero a eliminar no existe");
           
            // Eliminación del fichero
            File.Delete(fichero);
        }

        public static void ModificarCuenta(Cuenta CuentaModificar)
        {
            // RECURSOS
            string fichero = PATH;


            // VALIDAR ENTRADA
            if (CuentaModificar == null) throw new ArgumentNullException("Modificar Cuenta: es Null");
            fichero += $"\\{GenerarNombreFichero(CuentaModificar)}";

            if (!File.Exists(fichero)) throw new FileNotFoundException("El fichero a modificar no existe");

            // Modificar el Ficjero

            CrearFichero(CuentaModificar, fichero);
        }

        private static void CrearFichero(Cuenta CuentaModificar, string fichero)
        {

            StreamWriter Escritor = File.CreateText(fichero);
            Escritor.WriteLine(CuentaModificar.Titular);
            Escritor.WriteLine(CuentaModificar.Cantidad);
            Escritor.WriteLine(CuentaModificar.FechaNacimiento.ToString());
            Escritor.WriteLine(CuentaModificar.NumCuenta);

            Escritor.Close();
        }

        #endregion




        #region Métodos Privados



        /// <summary>
        /// Generar el nombre de nuestro fichero con el numero de cuenta
        /// </summary>
        /// <param name="cuenta"></param>
        /// <returns></returns>
        private static string GenerarNombreFichero(Cuenta cuenta)
        {
            string nombreFichero = null;

            nombreFichero = cuenta.NumCuenta + ".";


            if (cuenta is CuentaJoven) nombreFichero += "jov";
            if (cuenta is CuentaOro) nombreFichero += "oro";
            if (cuenta is CuentaPlatino) nombreFichero += "pla";

            return nombreFichero;
        }

        #endregion
    }
}
