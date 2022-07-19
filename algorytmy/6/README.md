Zastanawialiście się kiedyś jak działa SkyScanner? No to spróbujmy sobie to wyobrazić i zaimplementować.

Masz podaną olbrzymią listę biletów lotniczych. Każdy element tej listy przychodzi w takim samym formacie:
```
(miasto_początkowe, miasto_końcowe, cena)
```

Stwórz aplikację konsolową, która będzie zawierała bazę takich połączeń i na podstawie danych od użytkownika o miejscu początkowym, miejscu końcowym i maksymalnej liczbie przesiadek na jakie się zgadza wyliczy minimalną cenę za bilety.

Przykład

Input:
```
[
  ('JFK', 'ATL', 150),
  ('ATL', 'SFO', 400),
  ('ORD', 'LAX', 200),
  ('LAX', 'DFW', 80),
  ('JFK', 'HKG', 800),
  ('ATL', 'ORD', 90),
  ('JFK', 'LAX', 500),
]
```

Request:
Klient chce przelecieć z JFK na LAX przy maksymalnie 3 połączeniach.


Output:
```
JFK -> ATL -> ORD -> LAX
Cena: 440
```
