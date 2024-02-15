using MvcNetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace MvcNetCoreLinqToSql.Repositories

{
    public class RepositoryEnfermos
    {
        //CONSULTAS DE SELECCION 

        private DataTable tablaEnfermos;

        //CONSULTAS DE ACCION 

        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITALES;Persist Security Info=True;User ID=SA;Password=MCSD2023";

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;


            string sql = "select * from ENFERMO";
            SqlDataAdapter adEmp = new SqlDataAdapter(sql, connectionString);
            //INSTANCIAMOS NUESTRO DATATABLE
            this.tablaEnfermos = new DataTable();
            //TRAEMOS LOS DATOS
            adEmp.Fill(tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            //LA CONSULTA LINQ SE ALMACENA EN VARIABLES DE TIPO var
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            //LO QUE TENEMOS ALMACENADO EN CONSULTA ES UN CONJUNTO DE 
            //OBJETOS DataRow, QUE SON LOS OBJETOS QUE CONTIENE LA 
            //CLASE DataTable
            //DEBEMOS CONVERTIR DICHOS OBJETOS DataRow EN EMPLEADOS
            List<Enfermo> enfermos = new List<Enfermo>();
            //RECORREMOS CADA FILA DE LA consulta
            foreach (var row in consulta)
            {
                //PARA EXTRAER LOS DATOS DE UNA FILA DataRow
                //  fila.Field<TIPO>("COLUMNA")
                Enfermo enf = new Enfermo();
                enf.Inscripcion = row.Field<string>("INSCRIPCION");
                enf.Apellido = row.Field<string>("APELLIDO");
                enf.Direccion = row.Field<string>("DIRECCION");
                enf.Sexo = row.Field<string>("S");
                enf.NSS = row.Field<string>("NSS");
                enfermos.Add(enf);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo(string inscripcion)
        {
            //EL ALIAS datos REPRESENTA CADA OBJETO DENTRO DEL CONJUNTO
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<string>("INSCRIPCION") == inscripcion
                           select datos;
            //NOSOTROS SABEMOS QUE DEVUELVE SOLO UNA FILA, PERO 
            //consulta SIEMPRE SERA UNA COLECCION
            //consulta CONTIENE UNA SERIE DE METODOS Lambda PARA INDICAR 
            //CIERTAS FILAS O ACCIONES NECESARIAS
            //TENEMOS UN METODO LLAMADO First() QUE NOS DEVUELVE LA PRIMERA FILA
            var row = consulta.First();
            Enfermo enfermo = new Enfermo();
            enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
            enfermo.Apellido = row.Field<string>("APELLIDO");
            enfermo.Direccion = row.Field<string>("DIRECCION");
            enfermo.Sexo = row.Field<string>("S");
            enfermo.NSS = row.Field<string>("NSS");
            return enfermo;
        }


        public void DeleteEnfermo(string inscripcion)

        {

            string sql = "delete from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;

            this.cn.Open();
            int af = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

        }

    }

}
