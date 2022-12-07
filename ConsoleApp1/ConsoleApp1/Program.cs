using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;

string typeOfUser = "user";
string login;
string haslo;


Start:
Console.WriteLine("Wpisz 1, jeśli chcesz stworzyć nowego użytkownika");
Console.WriteLine("Wpisz 2, jeśli chcesz się zalogować");
int wybor = Int32.Parse(Console.ReadLine());

if (wybor == 1)
{
    Console.WriteLine("Wpisz login");
    login = Console.ReadLine();
    Console.WriteLine("Wpisz hasło");
    haslo = Console.ReadLine();
    string[] lines =
        {
            login, haslo
        };

    await File.WriteAllLinesAsync("user" + login + ".txt", lines); // Tworzy nowy plik tekstowy dla kazdego uzytkownika 
}

else if (wybor == 2)
{
    Console.WriteLine("Wpisz login");
    login = Console.ReadLine();
    Console.WriteLine(login);
    if(login == "admin")
    {
        typeOfUser = "admin";
    };
    Console.WriteLine("Wpisz hasło");
    haslo = Console.ReadLine();
    string[] test = System.IO.File.ReadAllLines(typeOfUser + login + ".txt");
    if (test[1] == haslo)
    {
        Console.WriteLine("Zalogowano");
    }
    else
    {
        goto Start;
    };
}

if(typeOfUser=="admin")
{

};

if (typeOfUser != "admin")
{

};