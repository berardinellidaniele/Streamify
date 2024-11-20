CREATE TABLE Utente (
    ID_Utente INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(50) NOT NULL,
    Cognome NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Data_Iscrizione DATE NOT NULL,
    Data_Nascita DATE NOT NULL
);

CREATE TABLE Preferenza_Genere (
    ID_Preferenza_Genere INT PRIMARY KEY IDENTITY(1,1),
    Genere NVARCHAR(50) NOT NULL
);

CREATE TABLE Immette (
    ID_Utente INT NOT NULL,
    ID_Preferenza_Genere INT NOT NULL,
    PRIMARY KEY (ID_Utente, ID_Preferenza_Genere),
    FOREIGN KEY (ID_Utente) REFERENCES Utente(ID_Utente),
    FOREIGN KEY (ID_Preferenza_Genere) REFERENCES Preferenza_Genere(ID_Preferenza_Genere)
);

CREATE TABLE Amministratore (
    ID_Amministratore INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(50) NOT NULL,
    Cognome NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL
);

CREATE TABLE Contenuto (
    ID_Contenuto INT PRIMARY KEY IDENTITY(1,1),
    ID_Amministratore INT NOT NULL,
    Nome NVARCHAR(100) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    Data_Rilascio DATE NOT NULL,
    Genere NVARCHAR(50) NOT NULL,
    Locandina NVARCHAR(MAX) NOT NULL,
    Descrizione NVARCHAR(MAX) NOT NULL,
    Rating FLOAT NOT NULL,
    N_Episodi INT NOT NULL,
    Durata INT NOT NULL,
    FOREIGN KEY (ID_Amministratore) REFERENCES Amministratore(ID_Amministratore)
);

CREATE TABLE Preferenza (
    ID_Preferenza INT PRIMARY KEY IDENTITY(1,1),
    ID_Utente INT NOT NULL,
    ID_Contenuto INT NOT NULL,
    Voto INT NOT NULL,
    FOREIGN KEY (ID_Utente) REFERENCES Utente(ID_Utente),
    FOREIGN KEY (ID_Contenuto) REFERENCES Contenuto(ID_Contenuto)
);

CREATE TABLE Cronologia (
    ID_Cronologia INT PRIMARY KEY IDENTITY(1,1),
    ID_Utente INT NOT NULL,
    ID_Contenuto INT NOT NULL,
    Data_Inizio DATE NOT NULL,
    Stato NVARCHAR(50) NOT NULL,
    FOREIGN KEY (ID_Utente) REFERENCES Utente(ID_Utente),
    FOREIGN KEY (ID_Contenuto) REFERENCES Contenuto(ID_Contenuto)
);
