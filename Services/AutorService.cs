
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;
using WebApiAutores.Models;

namespace WebApiAutores.Services
{
    public class AutorService(IConfiguration configuration){
        private readonly string _connectionString = configuration.GetConnectionString("sqliteConnection")
        ?? throw new InvalidCastException("No se encontró la cadena SqliteConnection");

        private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

        public async Task InitializeAsync()
        {
            const string consulta = @"
                CREATE TABLE IF NOT EXISTS Autores
                (
                    IdAutor INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Nacionalidad REAL NOT NULL,
                    FechaNacimiento TEXT NOT NULL,
                    Sueldo DOUBLE NOT NULL
                 );";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(consulta);    
        }

        public async Task<IEnumerable<AutoresGet>> GetListAsync()
        {
            const string consulta = @"
                SELECT IdAutor, Nombre, Nacionalidad, FechaNacimiento, Sueldo 
                FROM Autores;
            ";
            using var connection = CreateConnection();
            return await connection.QueryAsync<AutoresGet>(consulta);
        }

        public async Task<AutoresGet?> GetByIdAsync(int idAutor)
        {
            const string consulta = @"
            SELECT IdAutor, Nombre, Nacionalidad, FechaNacimiento, Sueldo
            FROM Autores
            WHERE IdAutor = @IdAutor;
            ";

            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<AutoresGet>(consulta, new { IdAutor = idAutor });
        }
        
        public async Task<int> SaveAsync(AutoresSet crear)
        {
            const string consulta = @"
            INSERT INTO Autores (Nombre, Nacionalidad, FechaNacimiento, Sueldo)
            VALUES (@Nombre, @Nacionalidad, @FechaNacimiento, @Sueldo);
            SELECT last_insert_rowid();";

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(consulta, crear);

        }

        public async Task<bool> UpdateAsync(int idAutor, AutoresSet autoresSet)
        {
            const string consulta = @"
            UPDATE Autores
            SET Nombre = @Nombre,
            Nacionalidad = @Nacionalidad,
            FechaNacimiento = @FechaNacimiento,
            Sueldo = @Sueldo
            WHERE IdAutor = @IdAutor;
            ";

            using var connection = CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(consulta, new
            {
                IdAutor = idAutor,
                autoresSet.Nombre,
                autoresSet.Nacionalidad,
                autoresSet.FechaNacimiento,
                autoresSet.Sueldo
            });
            return filasAfectadas > 0;
        }

        public async Task<bool> DeleteAsync(int idAutor)
        {
            const string consulta = @"
            DELETE FROM Autores
            WHERE IdAutor = @IdAutor;
            ";

            using var connection = CreateConnection();
            var filasAfectadas = await connection.ExecuteAsync(consulta, new { IdAutor = idAutor });
            return filasAfectadas > 0;
        }

    }
   
}