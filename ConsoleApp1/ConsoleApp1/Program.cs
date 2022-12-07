using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;

string typeOfUser = "user";
string login;
string haslo;


Start:
Console.WriteLine("Wpisz 1, jesli chcesz stworzyc nowego uzytkownika");
Console.WriteLine("Wpisz 2, jesli chcesz sie zalogowac");
int wybor = Int32.Parse(Console.ReadLine());

if (wybor == 1)
{
    Console.WriteLine("Wpisz login");
    login = Console.ReadLine(); // ZROBIC: JEZELI ISTNIEJE JUZ UZYTKOWNIK TO NIE ROBIC NOWEGO
    Console.WriteLine("Wpisz haslo");
    haslo = Console.ReadLine();
    string[] lines =
        {
            login, haslo
        };

    await File.WriteAllLinesAsync("user" + login + ".txt", lines); // Tworzy nowy plik tekstowy dla kazdego uzytkownika 
    goto Start;
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
    Console.WriteLine("Wpisz haslo");
    haslo = Console.ReadLine();
    string[] test = System.IO.File.ReadAllLines(typeOfUser + login + ".txt");  // Uzytkownicy oraz admin znajduja sie jako pliki tekstowe w folderze z programem
    if (test[1] == haslo)
    {
        Console.WriteLine("Zalogowano"); 
    }
    else
    {
        goto Start; // wraca na gore
    };
}

if(typeOfUser=="admin")
{
    Console.WriteLine("Witaj adminie");
    wybor = Int32.Parse(Console.ReadLine());

};

if (typeOfUser != "admin")
{
    Console.WriteLine("Witaj uzytkowiku");
    wybor = Int32.Parse(Console.ReadLine());
};