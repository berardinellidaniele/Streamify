using Microsoft.Data.SqlClient;
using Streamify.Models;
using Dapper;

public class Database
{
    private readonly string _conn;

    public Database(IConfiguration configuration)
    {
        _conn = configuration.GetConnectionString("Default");
    }
    private SqlConnection CreateConnection()
    {
        return new SqlConnection(_conn);
    }

    public bool CreaUtente(Utente user, string hashedPassword)
    {
        const string query = @"
            INSERT INTO Utente (Nome, Cognome, Email, Username, PasswordHash, Data_Iscrizione, Data_Nascita) 
            VALUES (@Nome, @Cognome, @Email, @Username, @PasswordHash, @Data_Iscrizione, @Data_Nascita)";
        using var db = CreateConnection();
        var result = db.Execute(query, new
        {
            user.Nome,
            user.Cognome,
            user.Email,
            user.Username,
            PasswordHash = hashedPassword,
            user.Data_Iscrizione,
            user.Data_Nascita
        });
        return result > 0;
    }

    
    public Utente OttieniUtenteDaEmail(string email)
    {
        const string query = "SELECT * FROM Utente WHERE Email = @Email";
        using var db = CreateConnection();
        return db.QuerySingleOrDefault<Utente>(query, new { Email = email });
    }

    
    public bool ModificaPassword(string email, string hashedPassword)
    {
        const string query = "UPDATE Utente SET PasswordHash = @PasswordHash WHERE Email = @Email";
        using var db = CreateConnection();
        var result = db.Execute(query, new { PasswordHash = hashedPassword, Email = email });
        return result > 0;
    }

   
    public bool ValidazioneUtente(string email, string passwordHash)
    {
        const string query = "SELECT 1 FROM Utente WHERE Email = @Email AND PasswordHash = @PasswordHash";
        using var db = CreateConnection();
        return db.QueryFirstOrDefault<int>(query, new { Email = email, PasswordHash = passwordHash }) == 1;
    }

    public List<Contenuto> GetContenutiPerGenere(string genere, int offset, int limit)
    {
        const string query = @"
            SELECT * 
            FROM Contenuto 
            WHERE Genere = @Genere 
            ORDER BY ID_Contenuto 
            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";
        using var db = CreateConnection();
        return db.Query<Contenuto>(query, new { Genere = genere, Offset = offset, Limit = limit }).AsList();
    }

    public bool AggiungiContenuto(Contenuto contenuto)
    {
        const string query = @"
            INSERT INTO Contenuto (ID_Amministratore, Nome, Tipo, Data_Rilascio, Genere, Locandina, Descrizione, Rating, N_Episodi, Durata)
            VALUES (@ID_Amministratore, @Nome, @Tipo, @Data_Rilascio, @Genere, @Locandina, @Descrizione, @Rating, @N_Episodi, @Durata)";
        using var db = CreateConnection();
        var result = db.Execute(query, contenuto);
        return result > 0;
    }

    public List<Utente> GetAllUsers()
    {
        const string query = "SELECT * FROM Utente";
        using var db = CreateConnection();
        return db.Query<Utente>(query).AsList();
    }

    
    public bool EliminaUtente(int userId)
    {
        const string query = "DELETE FROM Utente WHERE ID_Utente = @ID_Utente";
        using var db = CreateConnection();
        var result = db.Execute(query, new { ID_Utente = userId });
        return result > 0;
    }

    
    public bool AggiornaUtente(Utente user)
    {
        const string query = @"
            UPDATE Utente
            SET Nome = @Nome, Cognome = @Cognome, Email = @Email, Username = @Username, Data_Nascita = @Data_Nascita
            WHERE ID_Utente = @ID_Utente";
        using var db = CreateConnection();
        var result = db.Execute(query, user);
        return result > 0;
    }
}
