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

        public static void CrearFichero(Cuenta CuentaModificar, string fichero)
        {

            StreamWriter Escritor = File.CreateText(fichero);
            Escritor.WriteLine(CuentaModificar.Titular);
            Escritor.WriteLine(CuentaModificar.Cantidad);
            Escritor.WriteLine(CuentaModificar.FechaNacimiento.ToString());
            Escritor.WriteLine(CuentaModificar.NumCuenta);

            Escritor.Close();
        }




        public static void GuardarCuentaNueva(Cuenta nuevaCuenta)
        {
            // Recursos
            string fichero = PATH;


            if (nuevaCuenta == null) throw new ArgumentNullException("Crear Cuenta: La Cuenta es Null");

            // Generación del nombre del fichero
            fichero += $"\\{GenerarNombreFichero(nuevaCuenta)}";


            CrearFichero(nuevaCuenta, fichero);
        }

        public static string ConsultarFichero(Cuenta CuentaModificar)
        {
            // Recursos
            StreamReader lector;
            string cadena = "";
            string fichero = PATH;

            // Obtener nombre
            fichero += $"\\{GenerarNombreFichero(CuentaModificar)}";

            // Obtener contenido
            lector = File.OpenText($"{fichero}");
            cadena = lector.ReadToEnd();
            lector.Close();


            return cadena;
        }

        public static Cuenta[] ObtenerListaCuentas()
        {
            // Recursos Locales
            Cuenta[] listaCuentas = null;   // Lista de cuentas a devolver
            string[] listaFicheros = null;  // Lista de los ficheros almacenados

            // Lista de los ficheros almacenados
            listaFicheros = Directory.GetFiles(PATH); // c:/.../CuentasBancarias/(Ficheros.jov/oro/pla) Lo que devuelve
            // Validar
            if (listaFicheros == null) throw new ArgumentNullException("Obtener Modificar: No hay Cuentas Generadas");

            // 1. Para cada fichero generar la cuenta y agregarla al array
            listaCuentas = new Cuenta[listaFicheros.Length]; // Instanciar el array


            for (int indice = 0; indice < listaFicheros.Length; indice++)
            {


                /*
                 * nuevaCuenta = GeneraarCuenta(ListaFicheros(indice))
                 * listaCuentas(indice) = nuevaCuenta;
                 */

                listaCuentas[indice] = GenerarCuenta(listaFicheros[indice]);
            }
  

            return listaCuentas;
        }




        #endregion




        #region Métodos Privados

        private static Cuenta GenerarCuenta(string rutaFichero)
        {
            //  Recursos Locales
            Cuenta nuevaCuenta = null;
            string titular = "";
            string numCuenta = "";
            double cantidad = 0;
            DateTime fecha = new DateTime();
            string tipoCuenta = "";
            StreamReader lector = null;

            string auxFichero = null;
            // Obtención del número de cuenta y del tipo de cuenta


            auxFichero = rutaFichero.Substring(rutaFichero.LastIndexOf('\\') + 1);

            numCuenta = auxFichero.Split('.')[0];

            tipoCuenta = auxFichero.Split('.')[1];

            // Obtener los Datos del Fichero
            lector = File.OpenText(rutaFichero);
            titular = lector.ReadLine();
            cantidad = Convert.ToDouble(lector.ReadLine());
            // fecha = DateTime.ParseExact(lector.ReadLine(), "dd/MM/yyyy H:mm:ss", null);
            fecha = DateTime.Parse(lector.ReadLine());

            lector.Close();

            // Generación de la Cuenta
            switch (tipoCuenta)
            {
                case "jov":
                    nuevaCuenta = new CuentaJoven(titular, cantidad, fecha, numCuenta);
                    break;
                case "oro":
                    nuevaCuenta = new CuentaOro(titular, cantidad, fecha, numCuenta);
                    break;
                case "pla":
                    nuevaCuenta = new CuentaPlatino(titular, cantidad, fecha, numCuenta);
                    break;
            }


            return nuevaCuenta;
        }



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
