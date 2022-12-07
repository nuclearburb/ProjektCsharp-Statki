using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;

string login = null;
string haslo = null;

Start:
Console.WriteLine("Wpisz 1, jeśli chcesz stworzyć nowego użytkownika");
Console.WriteLine("Wpisz 2, jeśli chcesz się zalogować");
int wybor = Int32.Parse(Console.ReadLine());

        Console.WriteLine("Wpisz login");
        login = Console.ReadLine();
        Console.WriteLine(login);
        Console.WriteLine("Wpisz hasło");
        haslo = Console.ReadLine();
        Console.WriteLine(login);
    string[] lines =
        {
            "User "+login,haslo, login
        };

    await File.WriteAllLinesAsync("WriteLines.txt", lines);




    
