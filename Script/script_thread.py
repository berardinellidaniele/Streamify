import pandas as pd
import pyodbc
from tqdm import tqdm
from concurrent.futures import ThreadPoolExecutor

file_path = 'dati.csv'
df = pd.read_csv(file_path)

# converte i nomi delle colonne del CSV al nome degli attributi del DB
df['Data_Rilascio'] = pd.to_datetime(df['YEAR'], format='%Y').dt.date
df['N_Episodi'] = pd.to_numeric(df['EPISODES'], errors='coerce').fillna(0).astype(int)
df['Durata'] = pd.to_numeric(df['RUNTIME'], errors='coerce').fillna(0).astype(int)


# stringa di connessione presa da somee.com
cxn = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=tcp:Streamify.mssql.somee.com,1433;"
    "Database=Streamify;"
    "Uid=Berard10_SQLLogin_2;"
    "Pwd=grlp6ikuyk;"
    "Encrypt=no;" # crittografia disabilitata
    "TrustServerCertificate=yes;"
    "Connection Timeout=60;" 
)

def data_row_singola(row):
    try:
        conn = pyodbc.connect(cxn)
        cursor = conn.cursor()
        query = """
        INSERT INTO Contenuto (
            ID_Amministratore, Nome, Tipo, Data_Rilascio, Genere,
            Locandina, Descrizione, Rating, N_Episodi, Durata
        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """
        data = (
            1,  
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
        cursor.execute(query, data)
        conn.commit()
        conn.close()
    except Exception as e:
        print(f"errore inserimento della riga {row['TITLE']}: {e}")

def multithread(df):
    with ThreadPoolExecutor(max_workers=5) as executor:  
        futures = []
        for _, row in tqdm(df.iterrows(), total=len(df), desc="inserimento in corso"):
            futures.append(executor.submit(data_row_singola, row))
            
        for future in futures:
            future.result()

try:
    multithread(df)
except Exception as e:
    print(f"errore inserimento: {e}")
