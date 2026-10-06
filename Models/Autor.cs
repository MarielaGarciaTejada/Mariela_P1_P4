namespace WebApiAutores.Models;

public record AutoresGet(int IdAutor, string Nombre, string Nacionalidad, DateTime FechaNacimiento, double Sueldo)
{
    public AutoresGet() : this(0, "", "", default, 0) { }
}

public record AutoresSet(int IdAutor, string Nombre, string Nacionalidad, DateTime FechaNacimiento, double Sueldo);