﻿using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;

string typeOfUser;
string login;
string haslo;

string[] nazwy = new string[8]
{
    "Elektronika",
    "Zabawki",
    "Jedzenie",
    "Plastiki",
    "Chemia",
    "AGD",
    "Pojazdy",
    "Ubrania"
};

float[] wagi = new float[8]
{
    3f,
    1f,
    1f,
    3f,
    5f,
    10f,
    20f,
    0.5f
};


start:
typeOfUser = "user";
login = "";
haslo = "";

Console.WriteLine("Wpisz 1, jesli chcesz stworzyc nowego uzytkownika");
Console.WriteLine("Wpisz 2, jesli chcesz sie zalogowac");
int wybor = Int32.Parse(Console.ReadLine());

if (wybor == 1)
{
    Console.Write("Wpisz login: ");
    login = Console.ReadLine();

    string sciezkaPliku = "user" + login + ".txt";
    if (File.Exists(sciezkaPliku)) // sprawdzanie czy dane konto juz istnieje
    {
        Console.WriteLine("Dany użytkownik już istnieje");
        goto start;
    }

    Console.Write("Wpisz haslo: ");
    haslo = Console.ReadLine();
    string[] lines =
        {
            login, haslo
        };

    File.WriteAllLines("user" + login + ".txt", lines); // Tworzy nowy plik tekstowy dla kazdego uzytkownika 
    goto start;
}

else if (wybor == 2)
{
    Console.Write("Wpisz login: ");
    login = Console.ReadLine();
    if (login == "admin")
    {
        typeOfUser = "admin";
    };
    Console.Write("Wpisz haslo: ");
    haslo = Console.ReadLine();
    string[] test = System.IO.File.ReadAllLines(typeOfUser + login + ".txt");  // Uzytkownicy oraz admin znajduja sie jako pliki tekstowe w folderze z programem
    if (test[1] == haslo)
    {
        Console.WriteLine("Zalogowano");
    }
    else
    {
        Console.Write("Nieprawidlowe haslo!");
        goto start; // wraca na gore
    };
}

if (typeOfUser == "admin")
{
    Console.WriteLine("Witaj adminie");

    panelAdmina:

    Console.WriteLine("1. Podglad calej floty");
    Console.WriteLine("2. Podglad zawartosci statkow, lokalizacja");
    Console.WriteLine("3. Dodaj kontener");
    Console.WriteLine("4. Zarzadzanie statkiem");
    Console.WriteLine("5. Zmiana danych uzytkownika");
    Console.WriteLine("6. Wyloguj");

    wybor = Int32.Parse(Console.ReadLine());

    switch (wybor)
    {
        case 1:
            {
                
            }
            break;
        case 2:
            {

            }
            break;
        case 3:
            {
                Console.WriteLine("Podaj numer statku: "); // 1 lub 2
                int wyborStatku = Int32.Parse(Console.ReadLine());

                int typStrefy = 0; // 0 lub 1;
                int maxIloscStrefy = 0;
                int iloscZaladunku = 0;
                float waga = 0; // w tonach

                int[] ilosci = new int[8];

                for(int i = 0; i < ilosci.Length; i++)
                {
                    Console.Write($"Podaj ilosc do kontenera z zawartością {nazwy[i]}: ");
                    ilosci[i] = Int32.Parse(Console.ReadLine());

                    waga += ilosci[i] * wagi[i]; // dodaj do wagi całkowitej wage kontenera
                }

                Console.WriteLine("Calkowita waga: " + waga);


                /*
                int rodzaj = 0;
                Console.WriteLine("Podaj rodzaj kontenera (40/60)");
                rodzaj = int.Parse(Console.ReadLine());
                if (rodzaj == 40)
                {
                    Console.WriteLine("cos40");
                }
                else if (rodzaj == 60)
                {
                    Console.WriteLine("cos60");
                }
                Console.ReadKey(true);
                */
            }
            break;
        case 4:
            {

            }
            break;
        case 5:
            {
                Console.Write("Podaj uzytkownika: ");
                login = Console.ReadLine();

                string sciezkaPliku = "user" + login + ".txt";
                if (!File.Exists(sciezkaPliku)) // sprawdzanie czy dane konto juz istnieje
                {
                    Console.WriteLine("Dany użytkownik nie istnieje");
                    break;
                }

                Console.Write("Nowe haslo: ");
                haslo = Console.ReadLine();
                string[] lines =
                    {
                        login, haslo
                    };

                File.WriteAllLines("user" + login + ".txt", lines); // Tworzy nowy plik tekstowy dla kazdego uzytkownika 
            }
            break;
        case 6:
            {
                goto start;
            }
            break;
    }

    goto panelAdmina;
}

else
{
    Console.WriteLine("Witaj uzytkowiku");
    wybor = Int32.Parse(Console.ReadLine());





};