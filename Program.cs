using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using Npgsql;
using Npgsql.Internal;


class Program
{
    static void Main()
    {
        MostrarMenu();
    }

    static void MostrarMenu()
    {
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("Selecciona una opción:");
            Console.WriteLine("1. Usuario ");
            Console.WriteLine("2. Administrador ");
            Console.WriteLine("3. Salir");
            Console.Write("Opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    using (var conexionPG = new NpgsqlConnection("Host=sasa-mile-7238.g8z.gcp-us-east1.cockroachlabs.cloud;Port=26257; Database=parcial; Username=usuario;Password=12345"))
                    {
                        try
                        {
                            conexionPG.Open();
                            MostrarMenu(conexionPG);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        finally
                        {
                            conexionPG.Close();
                        }
                    }
                    break;
                case "2":
                    using (var conexionMY = new NpgsqlConnection("Host=sasa-mile-7238.g8z.gcp-us-east1.cockroachlabs.cloud;Port=26257; Database=parcial; Username=santiago;Password=5EDx-kkb-B6ngBGgVP-fmw"))
                    {
                        try
                        {
                            conexionMY.Open();
                            MostrarMenu(conexionMY);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        finally
                        {
                            conexionMY.Close();
                        }
                    }
                    break;
                case "3":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void MostrarMenu(NpgsqlConnection conn)
    {
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("Selecciona una opción:");
            Console.WriteLine("1. Crear registro");
            Console.WriteLine("2. Leer registros");
            Console.WriteLine("3. Actualizar registro");
            Console.WriteLine("4. Eliminar registro");
            Console.WriteLine("5. Volver al menú inicial");
            Console.Write("Opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    CrearRegistro(conn);
                    break;
                case "2":
                    LeerRegistros(conn);
                    break;
                case "3":
                    ActualizarRegistro(conn);
                    break;
                case "4":
                    EliminarRegistro(conn);
                    break;
                case "5":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void CrearRegistro(NpgsqlConnection conn)
    {
        Console.Clear();
        Console.WriteLine("CREAR REGISTRO");

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Edad: ");
        int edad = Convert.ToInt32(Console.ReadLine());

        Console.Write("Email: ");
        string email = Console.ReadLine();

        var sql = "INSERT INTO Personas (Nombre, Edad, Email) VALUES (@nombre, @edad, @email)";
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            try
            {
                cmd.Parameters.AddWithValue("nombre", nombre);
                cmd.Parameters.AddWithValue("edad", edad);
                cmd.Parameters.AddWithValue("email", email);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Registro creado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }


    }

    static void LeerRegistros(NpgsqlConnection conn)
    {
        Console.Clear();
        Console.WriteLine("LEER REGISTROS");

        var sql = "SELECT * FROM Vista_Personas_Otra_Tabla";
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            try
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id: {reader.GetInt64(0)}, Nombre: {reader.GetString(1)}, Edad: {reader.GetInt32(2)}, Email: {reader.GetString(3)},Columna1:{reader.GetString(4)},Columna2:{reader.GetInt32(5)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    static void ActualizarRegistro(NpgsqlConnection conn)
    {
        Console.Clear();
        Console.WriteLine("ACTUALIZAR REGISTRO");

        Console.Write("Ingrese el Id del registro que desea actualizar: ");
        long id = Convert.ToInt64(Console.ReadLine());

        Console.Write("Nuevo Nombre: ");
        string nuevoNombre = Console.ReadLine();

        Console.Write("Nueva Edad: ");
        int nuevaEdad = Convert.ToInt32(Console.ReadLine());

        Console.Write("Nuevo Email: ");
        string nuevoEmail = Console.ReadLine();

        var sql = "UPDATE Personas SET Nombre = @nombre, Edad = @edad, Email = @email WHERE Id = @id";
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            try
            {
                cmd.Parameters.AddWithValue("nombre", nuevoNombre);
                cmd.Parameters.AddWithValue("edad", nuevaEdad);
                cmd.Parameters.AddWithValue("email", nuevoEmail);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        Console.WriteLine("Registro actualizado con éxito.");
    }

    static void EliminarRegistro(NpgsqlConnection conn)
    {
        Console.Clear();
        Console.WriteLine("ELIMINAR REGISTRO");

        Console.Write("Ingrese el Id del registro que desea eliminar: ");
        long id = Convert.ToInt64(Console.ReadLine());

        var sql = "DELETE FROM Personas WHERE Id = @id";
        using (var cmd = new NpgsqlCommand(sql, conn))
        {
            try
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        Console.WriteLine("Registro eliminado con éxito.");
    }
}