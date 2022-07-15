Napisz program konsolowy do wyświetlania plików JSON w formie tabeli.

Funkcjonalności
1. Zapytaj o ścieżkę do pliku, który ma zostać wyświetlony.
2. Pobierz plik wybrany przez użytkownika.
3. Sprawdź jego poprawność: Plik powinien być poprawnym plikiem JSON. Plik nie powinien zawierać zagnieżdżeń obiektów, ani tablic w danej właściwości (uproszczenie ze względu na wymaganie do wyświetlenia tabeli).
4. Wyświetl plik w postaci tabeli (patrz przykład).
5. Zapytaj, czy wczytać kolejny plik.

Przykład
Po podaniu ścieżki do pliku C:/temp/example.json
Wyświetl tabelę jak poniżej
```
--------------------------------------------------------------------------
|   IMIĘ   |   NAZWISKO   |   ZAWÓD   |   WIEK   |   MIEJSCE-URODZENIA   |
--------------------------------------------------------------------------
| Kajetan  |  Duszyński   |Programista|    33    |      Lublin           |
--------------------------------------------------------------------------
|   Jan    |    Kowalski  | Marynarz  |    37    |      Sopot            |
--------------------------------------------------------------------------
```

UWAGA
Jeżeli coś się będzie krzywo wyświetlało to się nie przejmuj 😉