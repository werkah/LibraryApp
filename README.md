# LibraryApp


## Description

The application is designed for book rental and library management. Users can borrow books by creating a "borrow" record, where they select the borrowing and return dates, as well as the book. Users can view a list of available books along with their categories and availability to facilitate the borrowing process. They can also display the category with the highest number of books. Administrators have the ability to add and manage users, as well as add new books, categories, and borrow records to the database. Administrators can also search for user borrowings by their name and display the user with the highest number of borrowings. Access to different sections is granted based on user levels, where unauthenticated individuals can only see the homepage and login page. Upon initial launch, an administrator account is automatically created. Users do not have the ability to create new users; only administrators possess that capability.

## Functionalities

- Registration of default 'admin' user
- Login and logout
- Adding/removing/displaying/editing users
- Adding/removing/displaying/editing books
- Adding/removing/displaying/editing categories
- Adding/removing/displaying/editing borrow records
- Searching for a user's borrowings by their name
- Displaying the user with the highest number of borrowings
- Displaying the category with the highest number of books
- Displaying a list of books with their category and availability


## Database

### Tables:
- User
    - UserId: int (primary key)
    - Username: string
    - Password: string
    - Borrows: ICollection<Borrow> (foreign key to the Borrow table)
- Category
    - CategoryId: int (primary key)
    - Name: string
    - Books: ICollection<Book> (foreign key to the Book table)
- Book
    - BookId: int (primary key)
    - Title: string
    - Author: string
    - Available: bool
    - CategoryId: int (foreign key to the Category table)
    - Borrows: ICollection<Borrow> (foreign key to the Borrow table)
- Borrow
    - BorrowId: int (primary key)
    - BorrowDate: DateTime
    - BorrowDue: DateTime
    - isReturned: bool
    - BookId: int (foreign key to the Book table)
    - UserId: int (foreign key to the User table)




