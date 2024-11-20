-- dato che alcuni campi hanno solo un "-" al posto del contenuto reale 
-- cancelliamo tutti le righe in cui non sono presenti i campi con valori reali
-- così facendo si sono eliminate circa ~200 record ma data la grandezza del database non ci interessa molto (:

DELETE FROM Contenuto
WHERE 
    CAST(Nome AS VARCHAR(MAX)) = '-' OR
    CAST(Tipo AS VARCHAR(MAX)) = '-' OR
    CAST(Genere AS VARCHAR(MAX)) = '-' OR
    CAST(Locandina AS VARCHAR(MAX)) = '-' OR
    CAST(Descrizione AS VARCHAR(MAX)) = '-' OR
    CAST(Rating AS VARCHAR(MAX)) = '-' OR
    CAST(N_Episodi AS VARCHAR(MAX)) = '-' OR
    CAST(Durata AS VARCHAR(MAX)) = '-' OR
    CAST(Data_Rilascio AS VARCHAR(MAX)) = '-';

-- Si esegue il cast dato che si utilizzano anche attributi con TEXT
