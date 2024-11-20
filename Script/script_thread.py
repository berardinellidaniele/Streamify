import pandas as pd
import pyodbc
from tqdm import tqdm
from concurrent.futures import ThreadPoolExecutor

# Percorso del file CSV
file_path = 'FILM_SERIETV_NORMALIZZATO.csv'
df = pd.read_csv(file_path)

# Pre-elaborazione dei dati
df['Data_Rilascio'] = pd.to_datetime(df['YEAR'], format='%Y').dt.date
df['N_Episodi'] = pd.to_numeric(df['EPISODES'], errors='coerce').fillna(0).astype(int)
df['Durata'] = pd.to_numeric(df['RUNTIME'], errors='coerce').fillna(0).astype(int)


# Stringa di connessione al database
connection_string = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=tcp:Streamify.mssql.somee.com,1433;"
    "Database=Streamify;"
    "Uid=Berard10_SQLLogin_2;"
    "Pwd=grlp6ikuyk;"
    "Encrypt=no;"
    "TrustServerCertificate=yes;"
    "Connection Timeout=60;"  # Aumentato il timeout
)

def insert_data_single_row(row):
    """Inserisce una singola riga nel database."""
    try:
        # Crea una nuova connessione per ogni thread
        conn = pyodbc.connect(connection_string)
        cursor = conn.cursor()
        insert_query = """
        INSERT INTO Contenuto (
            ID_Amministratore, Nome, Tipo, Data_Rilascio, Genere,
            Locandina, Descrizione, Rating, N_Episodi, Durata
        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """
        data = (
            1,  # ID fisso
            row['TITLE'],
            row['TYPE'],
            row['Data_Rilascio'],
            row['genres'],
            row['image_url'],
            row['summary'],
            row['RATING'],
            row['N_Episodi'],
            row['Durata']
        )
        cursor.execute(insert_query, data)
        conn.commit()
        conn.close()
    except Exception as e:
        print(f"Errore durante l'inserimento della riga {row['TITLE']}: {e}")

def insert_data_with_progress_multithreaded(df):
    """Inserisce i dati nel database utilizzando i thread con barra di progresso."""
    with ThreadPoolExecutor(max_workers=5) as executor:  # Ridotto il numero di thread
        futures = []
        for _, row in tqdm(df.iterrows(), total=len(df), desc="Inserimento dati con thread"):
            futures.append(executor.submit(insert_data_single_row, row))

        # Attende che tutti i thread terminino
        for future in futures:
            future.result()

# Connessione e inserimento dati
try:
    print("Inserimento dati in corso...")
    insert_data_with_progress_multithreaded(df)
    print("Processo completato!")
except Exception as e:
    print(f"Errore nell'inserimento: {e}")
