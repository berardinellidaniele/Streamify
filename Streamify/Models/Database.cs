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
            INSERT INTO Utente (Nome, Cognome, Email, Password, Data_Iscrizione, Data_Nascita) 
            VALUES (@Nome, @Cognome, @Email, @Password, @Data_Iscrizione, @Data_Nascita)";
        using var db = CreateConnection();
        var result = db.Execute(query, new
        {
            user.Nome,
            user.Cognome,
            user.Email,
            Password = hashedPassword,
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
        const string query = "UPDATE Utente SET Password = @PasswordHash WHERE Email = @Email";
        using var db = CreateConnection();
        var result = db.Execute(query, new { PasswordHash = hashedPassword, Email = email });
        return result > 0;
    }

    public bool ValidazioneUtente(string email, string passwordHash)
    {
        const string query = "SELECT 1 FROM Utente WHERE Email = @Email AND Password = @PasswordHash";
        using var db = CreateConnection();
        return db.QueryFirstOrDefault<int>(query, new { Email = email, PasswordHash = passwordHash }) == 1;
    }

    public List<Contenuto> GetContenutiPerGenere(string genere, int offset, int limit)
    {
        using var db = CreateConnection();
        var result = db.Query<Contenuto>($"SELECT * FROM Contenuto WHERE Genere LIKE '%{genere}%' ORDER BY ID_Contenuto OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY").AsList();

        foreach (var contenuto in result)
        {
            if (contenuto != null && !string.IsNullOrEmpty(contenuto.Descrizione))
            {
                var words = contenuto.Descrizione.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 73)
                {
                    contenuto.Descrizione = string.Join(" ", words.Take(73)) + "...";
                }
            }
        }

        return result;
    }

    public List<Contenuto> GetContenutiPerGenerePreferiti(string genere, int id_utente, int offset, int limit)
    {
        using var db = CreateConnection();
        var result = db.Query<Contenuto>($@"
                                        SELECT C.* 
                                        FROM Contenuto C
                                        JOIN Preferenza P ON P.ID_Contenuto = C.ID_Contenuto
                                        WHERE P.ID_Utente = {id_utente}
                                        AND C.Genere LIKE '%{genere}%' 
                                        ORDER BY C.ID_Contenuto
                                        OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY
        ").AsList();

        foreach (var contenuto in result)
        {
            if (contenuto != null && !string.IsNullOrEmpty(contenuto.Descrizione))
            {
                var words = contenuto.Descrizione.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 73)
                {
                    contenuto.Descrizione = string.Join(" ", words.Take(73)) + "...";
                }
            }
        }

        return result;
    }

    public List<Contenuto> GetContenutiPerGenere(string genere, string tipo, int offset, int limit)
    {
        using var db = CreateConnection();
        var result = db.Query<Contenuto>($"SELECT * FROM Contenuto WHERE Genere LIKE '%{genere}%' AND Tipo='{tipo}' ORDER BY ID_Contenuto OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY").AsList();

        foreach (var contenuto in result)
        {
            if (contenuto != null && !string.IsNullOrEmpty(contenuto.Descrizione))
            {
                var words = contenuto.Descrizione.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 73)
                {
                    contenuto.Descrizione = string.Join(" ", words.Take(73)) + "...";
                }
            }
        }

        return result;
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
            SET Nome = @Nome, Cognome = @Cognome, Email = @Email, Data_Nascita = @Data_Nascita
            WHERE ID_Utente = @ID_Utente";
        using var db = CreateConnection();
        var result = db.Execute(query, user);
        return result > 0;
    }

    public List<string> GetGeneriUnici()
    {
        const string query = "SELECT Genere FROM Contenuto";
        using var db = CreateConnection();
        var generi = db.Query<string>(query).ToList();
        return generi
            .Where(g => !string.IsNullOrEmpty(g))
            .SelectMany(g => g.Split(',').Select(genere => genere.Trim()))
            .Distinct()
            .OrderBy(g => g)
            .ToList();
    }

    public List<Contenuto> CercaContenutoOGeneri(string search)
    {
        const string query = @"
        SELECT DISTINCT ID_Contenuto, Nome, Locandina, Descrizione
        FROM Contenuto
        WHERE LOWER(Nome) LIKE @Search
            OR LOWER(Genere) LIKE @Search
        ORDER BY ID_Contenuto";

        using var db = CreateConnection();
        var result = db.Query<Contenuto>(query, new
        {
            Search = $"%{search}%"
        }).ToList();

        foreach (var contenuto in result)
        {
            if (contenuto != null && !string.IsNullOrEmpty(contenuto.Descrizione))
            {
                var words = contenuto.Descrizione.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 73)
                {
                    contenuto.Descrizione = string.Join(" ", words.Take(73)) + "...";
                }
            }
        }

        return result;
    }


    public Contenuto GetContenuto(int id)
    {
        const string query = "SELECT * FROM Contenuto WHERE ID_Contenuto = @ID_Contenuto";
        using var db = CreateConnection();
        Contenuto result = db.QueryFirstOrDefault<Contenuto>(query, new { ID_Contenuto = id });

        if (result != null)
        {
            var parole = result.Descrizione.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parole.Length > 73)
            {
               result.Descrizione = string.Join(" ", parole.Take(73)) + "...";
            }
        }

        return result;
    }



    public (string Nome, string Cognome) NomeCognome(string email)
    {
        const string query = "SELECT Nome, Cognome from Utente WHERE Email = @Email";
        using var db = CreateConnection();
        var result = db.QuerySingleOrDefault<(string Nome, string Cognome)>(query, new { Email = email });

        if (result.Equals(default((string Nome, string Cognome))))
        {
            return ("Nome non trovato", "Cognome non trovato");
        }

        return result;
    }
}