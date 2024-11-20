# 🎥 **Piattaforma di streaming**

>Descrivere e realizzare un database per la gestione di una piattaforma di streaming, considerando che gli utenti possano visualizzare film o serie TV, tenendo traccia del loro storico di visione e delle loro preferenze

Traccia:
[[Gestione di una Piattaforma di Streaming.pdf]]

---
### 🌟 **Schema E/R**

Il testo ci dice di progettare il database per la piattaforma di streaming, dunque la prima cosa che andiamo a verificare sono le entità e i collegamenti da inserire nello schema E/R (Entità / Relazioni)

**Entità:  
  - 📌 **Utente**  
  - 🎬 **Contenuto**  
  - 📺 **Serie TV**  
  - 🛠️ **Amministratore**  
  - 💡 **Preferenza**  
  - 🎭 **Preferenza_Genere**  
  - 🕒 **Cronologia**

- **Relazioni**:  
  - Un **utente** può avere più **preferenze** _(1-N)_.  
  - Gli **utenti** possono selezionare più **generi preferiti** _(N-N)_.  
  - Gli **utenti** possono tenere traccia della loro **cronologia** _(1-N)_.  
  - Le **preferenze** possono essere di più e sono associate ai **contenuti** della piattaforma _(N-1)_.  
  - La **cronologia** è collegata ai **contenuti** _(N-1)_.  
  - Gli **amministratori** possono gestire più **contenuti** _(1-N)_.
---
## 🛠️ **Entità e Attributi**

#### 📌 **Utente**

Gli utenti sono coloro che interagiscono nella piattaforma

| **Attributo**     | **Tipo** | not null | **Descrizione**             |
| ----------------- | -------- | -------- | --------------------------- |
| `ID_Utente` (PK)  | int      | ✅        | chiave primaria             |
| `Nome`            | varchar  | ✅        | nome dell'utente            |
| `Cognome`         | varchar  | ✅        | cognome dell'utente         |
| `Email`           | varchar  | ✅        | email dell'utente           |
| `Password`        | varchar  | ✅        | password dell'utente        |
| `Data_Iscrizione` | date     | ✅        | data di registrazione       |
| `Data_Nascita`    | date     | ✅        | data di nascita dell'utente |

La data di nascita viene utilizzata per controllare se l'utente è adulto, dato che alcuni contenuti sono per adulti e altri no. 

---

### 💡 **Preferenza**

Un'entità che tiene traccia delle preferenze di un utente verso i contenuti.

| **Attributo**        | **Tipo** | not null | **Descrizione**                |
| -------------------- | -------- | -------- | ------------------------------ |
| `ID_Preferenza` (PK) | int      | ✅        | chiave primaria                |
| `ID_Utente` (FK)     | int      | ✅        | chiave esterna verso utente    |
| `ID_Contenuto` (FK)  | int      | ✅        | chiave esterna verso contenuto |
| `Voto`               | int      | ❌        | Valutazione data dall'utente   |

---

### 🎭 **Preferenza_Genere**

Un'entità che rappresenta i generi preferiti degli utenti

| **Attributo**               | **Tipo** | not null | **Descrizione**           |
| --------------------------- | -------- | -------- | ------------------------- |
| `ID_Preferenza_Genere` (PK) | int      | ✅        | chiave primaria           |
| `Genere`                    | varchar  | ❌        | nome del genere preferito |

---

### 🕒 **Cronologia**

Un'entità per registrare le visioni degli utenti

| **Attributo**        | **Tipo** | not null | **Descrizione**                       |
| -------------------- | -------- | -------- | ------------------------------------- |
| `ID_Cronologia` (PK) | int      | ✅        | chiave primaria                       |
| `ID_Utente` (FK)     | int      | ✅        | chiave esterna rivolta ad utente      |
| `ID_Contenuto` (FK)  | int      | ✅        | chiave esterna rivolta a contenuto    |
| `Data_Inizio`        | datetime | ✅        | data e ora di inizio della visione    |
| `Stato`              | varchar  | ✅        | stato della visione (in corso, visto) |

---

### 🎬 **Contenuto**

Rappresenta i film o le serie TV disponibili sulla piattaforma

| **Attributo**            | **Tipo** | not null | **Descrizione**                              |
| ------------------------ | -------- | -------- | -------------------------------------------- |
| `ID_Contenuto` (PK)      | int      | ✅        | chiave primaria                              |
| `ID_Amministratore` (FK) | int      | ✅        | chiave esterna rivolta ad amministratore     |
| `Nome`                   | varchar  | ✅        | nome del contenuto                           |
| `Tipo`                   | varchar  | ✅        | Film o Serie TV                              |
| `Data_Rilascio`          | date     | ✅        | data di pubblicazione                        |
| `Genere`                 | string   | ✅        | genere del contenuto                         |
| `Locandina`              | nvarchar | ✅        | locandina rappresentativa                    |
| `Descrizione`            | varchar  | ✅        | descrizione del contenuto                    |
| `Rating`                 | float    | ✅        | valutazione media degli utenti presa da IMDB |
| `N_Episodi`              | int      | ✅        | numero di episodi totali                     |
| `Durata`                 | int      | ✅        | durata in minuti                             |
| `Paese_Origine`          | varchar  | ✅        | paese d'origine                              |

Il rating è preso da [IMDB](https://www.imdb.com/it/)

Tutti i contenuti vengono presi da un dataset pubblico su [Kaggle](https://www.kaggle.com/datasets/snehaanbhawal/netflix-tv-shows-and-movie-list)

---
### 🛠️ **Amministratore**

Gli amministratori della piattaforma che gestiscono i contenuti e li moderano.

| **Attributo**            | **Tipo** | not null | **Descrizione**              |
| ------------------------ | -------- | -------- | ---------------------------- |
| `ID_Amministratore` (PK) | int      | ✅        | chiave primaria              |
| `Nome`                   | varchar  | ✅        | nome dell'amministratore     |
| `Cognome`                | varchar  | ✅        | cognome dell'amministratore  |
| `Email`                  | varchar  | ✅        | email dell'amministratore    |
| `Password`               | varchar  | ✅        | password dell'amministratore |

---

## 🔗 **Relazioni**

1. **Utente - Preferenza**: 1-N  
   Ogni utente può esprimere più preferenze su diversi contenuti

2. **Utente - Cronologia**: 1-N  
   Ogni utente mantiene uno storico di visione

3. **Preferenza - Contenuto**: N-1  
   Più preferenze possono essere associate a un contenuto

4. **Cronologia - Contenuto**: N-1  
   Lo storico di visione può contenere riferimenti a più contenuti

5. **Utente - Preferenza_Genere**: N-N  
   Gli utenti possono selezionare più generi preferiti

6. **Amministratore - Contenuto**: 1-N  
   Ogni amministratore può gestire più contenuti
   

--- 
