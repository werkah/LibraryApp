# BIBLIOTEKA


## Opis aplikacji 

Aplikacja służy do wypożyczania książek i zarządzania biblioteką. Użytkownik może wypożyczyć książkę przez utworzenie "borrow", gdzie wybiera datę wypożyczenia i zwrotu oraz książkę. Użytkownik może wyświetlić listę dostępnych książek razem z kategorią i ich dostępnością w celu ułatwienia procesu wypożyczania oraz może wyświetlić kategorię z największą liczbą książek. Administrator ma możliwość dodawania użytkowników i zarządzania nimi, jak również dodawania nowych książek do bazy, nowych kategorii i nowych wypożyczeń. Administrator może również wyszukiwać wypożyczenia użytkownika po jego nazwie i wyświetlić użytkownika z największą liczbą wypożyczeń. Dostęp do poszczególnych sekcji jest dostępny z różnych poziomów, osoba niezalogowana widzi tylko stronę główną i logowanie. Przy pierwszym uruchomieniu jest automatycznie tworzone konto administratora. Użytkownik nie ma możliwości tworzenia użytkowników, tylko administrator posiada taką możliwość. 

## Funkcjonalności
- Zarejestrowanie użytkownika domyślnego 'admin'
- Logowanie i wylogowywanie
- Dodawanie/usuwanie/wyświetlanie/edycja użytkowników
- Dodawanie/usuwanie/wyświetlanie/edycja książek
- Dodawanie/usuwanie/wyświetlanie/edycja kategorii
- Dodawanie/usuwanie/wyświetlanie/edycja wypożyczeń
- Wyszukiwanie po nazwie użytkownika jego wypożyczeń
- Wyświetlenie użytkownika z największą liczbą wypożyczeń
- Wyświetlenie kategorii z największą liczbą książek
- Wyświetlenie listy książek wraz z ich kategorią i dostepnością


## Baza danych

### Tabele:
- User
    - UserId: int
    - Username: string
    - Password: string
    - Borrows: ICollection<Borrow> (klucz obcy do tabeli Borrow)
- Category
    - CategoryId: int
    - Name: string
    - Books: ICollection<Book> (klucz obcy do tabeli Book)
- Book
    - BookId: int (klucz główny)
    - Title: string
    - Author: string
    - Available: bool
    - CategoryId: int (klucz obcy do tabeli Category)
    - Borrows: ICollection<Borrow> (klucz obcy do tabeli Borrow)
- Borrow
    - BorrowId: int (klucz główny)
    - BorrowDate: DateTime
    - BorrowDue: DateTime
    - isReturned: bool
    - BookId: int (klucz obcy do tabeli Book)
    - UserId: int (klucz obcy do tabeli User)




