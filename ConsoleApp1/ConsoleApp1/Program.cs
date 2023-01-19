using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

string typeOfUser;
string login;
string haslo;

Random liczba = new Random();
int miasto = liczba.Next(0, 5);

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

string[] lokalizacje = new string[6]
{
    "Gdynia",
    "Karslskrona",
    "Hong Kong",
    "Szczecin",
    "Oslo",
    "Kopenhaga"
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
    if (login == "")
    {
        goto start;
    }

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
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Nieprawidlowe haslo!");
        goto start; // wraca na gore
    };
}

if (typeOfUser == "admin")
{
    Console.WriteLine("Witaj adminie");

panelAdmina:

    Console.WriteLine("1. Podglad calej floty");
    Console.WriteLine("2. Zarzadzanie statkiem");
    Console.WriteLine("3. Zmiana danych uzytkownika");
    Console.WriteLine("4. Wyloguj");

    wybor = Int32.Parse(Console.ReadLine());

    switch (wybor)
    {
        case 1:
            {
                string[] kolumny = new string[12]
                {
                    "Typ Strefy",
                    "Waga Max",
                    "Ilosc Max",
                    "Waga",
                    "Elektronika",
                    "Zabawki",
                    "Jedzenie",
                    "Plastiki",
                    "Chemia",
                    "AGD",
                    "Pojazdy",
                    "Ubrania"
                };
                Console.Write("Podaj numer statku: ");
                int wyborStatku = Int32.Parse(Console.ReadLine()); // 1 lub 2

                int[,] statek = new int[4, 13];

                var lines = File.ReadAllLines("statek" + wyborStatku + ".txt"); // przepisuje plik tekstowy do tabelki
                Console.WriteLine("Statek nr: " + wyborStatku);
                Console.WriteLine("|Typ Strefy|Waga Max|Ilosc Max|Waga|Elektronika|Zabawki|Jedzenie|Plastiki|Chemia|AGD|Pojazdy|Ubrania|");

                for (int x = 0; x < 4; x++)
                {
                    string[] words = lines[x].Split(' ');
                    int i = 0;
                    foreach (var word in words)
                    {
                        Console.Write("|");
                        int dlugosc = kolumny[i].Length;
                        Console.Write(word.PadRight(dlugosc)); //uzupelnia spacjami do ilosci
                        i++;
                    }
                    Console.WriteLine("|");
                };
                Console.WriteLine();
            }
            break;
        case 2:
            {
                Console.Write("Podaj numer statku: ");
                int wyborStatku = int.Parse(Console.ReadLine()); // 1 lub 2

                float[,] statek = new float[4, 13];

                var lines = File.ReadAllLines("statek" + wyborStatku + ".txt"); // przepisuje plik tekstowy do tabelki
                for (int x = 0; x < 4; x++)
                {
                    string[] words = lines[x].Split(' ');

                    int indeks = 0;
                    foreach (var word in words)
                    {
                        statek[x, indeks] = int.Parse(word);
                        indeks++;
                    }
                };

                Console.WriteLine("Wpisz 1 jesli chcesz dodac kontener" + "\n" + "Wpisz 2 jesli chcesz usunac kontener");
                int wybor_zarzadzania = int.Parse(Console.ReadLine());

                if (wybor_zarzadzania == 1)
                {
                    int typKontenera = 0;
                    while (typKontenera != 40 && typKontenera != 60)
                    {
                        Console.Write("Podaj typ kontenera (40 albo 60): ");
                        typKontenera = Int32.Parse(Console.ReadLine()); // 40 lub 60;
                    }

                    int iloscZaladunku = 300;
                    int maxIloscStrefy = 100;
                    int iloscStref = 5;
                    float waga = 0;
                    int[] ilosci = new int[8];
                    if (typKontenera == 60)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerów z zawartością {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }
                            waga += ilosci[j] * wagi[j];
                            if (400 < (statek[2, 5] + statek[3, 5] + ilosci[j]))
                            {
                                Console.WriteLine("Za duża ilość kontenerów");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (waga + statek[2, 3] + statek[3, 3] > 1000)
                            {
                                Console.WriteLine("Zbyt duża waga");
                                waga = 0;
                                goto B;
                            }
                            while (ilosci[j] != 0)
                            {

                                if ((statek[2, j + 4] + 1) < 200)
                                {
                                    statek[2, j + 4] = statek[2, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                    statek[2, 3] = statek[2, 3] + wagi[j];
                                }

                                if ((statek[3, j + 4] + 1) < 200 && ilosci[j] > 0)
                                {
                                    statek[3, j + 4] = statek[3, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                    statek[3, 3] = statek[3, 3] + wagi[j];
                                }
                            }
                        S:
                            waga = 0;

                        }
                    }

                    if (typKontenera == 40)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerów z zawartością {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }
                            waga += ilosci[j] * wagi[j];
                            if (200 < (statek[0, 5] + statek[1, 5] + ilosci[j]))
                            {
                                Console.WriteLine("Za duża ilość kontenerów");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (waga + statek[0, 3] + statek[1, 3] > 600)
                            {
                                Console.WriteLine("Zbyt duża waga");
                                waga = 0;
                                goto B;
                            }
                            while (ilosci[j] != 0)
                            {

                                if ((statek[0, j + 4] + 1) < 100)
                                {
                                    statek[0, j + 4] = statek[0, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                }

                                if ((statek[1, j + 4] + 1) < 100 && ilosci[j] > 0)
                                {
                                    statek[1, j + 4] = statek[1, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                }


                            }
                        S:
                            waga = 0;

                        }
                    }
                }

                else
                {
                    int typKontenera = 0;
                    while (typKontenera != 40 && typKontenera != 60)
                    {
                        Console.Write("Podaj typ kontenera ktory chcesz usunac (40 albo 60): ");
                        typKontenera = Int32.Parse(Console.ReadLine()); // 40 lub 60;
                    }

                    int iloscZaladunku = 300;
                    int maxIloscStrefy = 100;
                    int iloscStref = 5;
                    float waga = 0;
                    int[] ilosci = new int[8];

                    if (typKontenera == 60)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerów z zawartością {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }
                            waga += ilosci[j] * wagi[j];
                            if (400 < (statek[2, 5] + statek[3, 5] + ilosci[j]))
                            {
                                Console.WriteLine("Za duża ilość kontenerów");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (waga + statek[2, 3] + statek[3, 3] > 1000)
                            {
                                Console.WriteLine("Zbyt duża waga");
                                waga = 0;
                                goto B;
                            }
                            while (ilosci[j] != 0)
                            {

                                if ((statek[2, j + 4] + 1) < 200)
                                {
                                    statek[2, j + 4] = statek[2, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                    statek[2, 3] = statek[2, 3] + wagi[j];  
                                }

                                if ((statek[3, j + 4] + 1) < 200 && ilosci[j] > 0)
                                {
                                    statek[3, j + 4] = statek[3, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                    statek[3, 3] = statek[3, 3] + wagi[j];
                                }
                            }
                            statek[3, j + 4] = statek[3, j + 4] + 1;
                        S:
                            waga = 0;

                        }
                    }

                    if (typKontenera == 40)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerów z zawartością {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }
                            waga += ilosci[j] * wagi[j];
                            if (200 < (statek[0, 5] + statek[1, 5] + ilosci[j]))
                            {
                                Console.WriteLine("Za duża ilość kontenerów");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (waga + statek[0, 3] + statek[1, 3] > 600)
                            {
                                Console.WriteLine("Zbyt duża waga");
                                waga = 0;
                                goto B;
                            }
                            while (ilosci[j] != 0)
                            {

                                if ((statek[0, j + 4] + 1) < 100)
                                {
                                    statek[0, j + 4] = statek[0, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                }

                                if ((statek[1, j + 4] + 1) < 100 && ilosci[j] > 0)
                                {
                                    statek[1, j + 4] = statek[1, j + 4] + 1;
                                    ilosci[j] = ilosci[j] - 1;
                                }
                            }
                        S:
                            waga = 0;
                        }
                    }
                }

                //ZROBIĆ - LICZENIE WAG
                //I USUWANIE KONTENEROW

                string[,] bufor = new string[4, 13];
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        bufor[k, l] = statek[k, l].ToString();
                    }
                }
                Console.WriteLine(bufor[0, 12]);

                using (var sw = new StreamWriter("statek" + wyborStatku + ".txt"))
                {
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 12; l++)
                        {
                            if (l <= 10) sw.Write(bufor[k, l] + " ");
                            else if (l == 11) sw.Write(bufor[k, l]);
                        }
                        sw.Write("\n");
                    }
                    sw.Flush();
                    sw.Close();
                }




                /*
                string[] linie = new string[iloscStref];
                for (int i = 0; i < iloscStref; i++)
                {
                    waga = 0; // w tonach


                    for (int j = 0; j < ilosci.Length; j++)
                    {
                        Console.Write($"Podaj ilosc kontenerów z zawartością {nazwy[j]}: ");
                        ilosci[j] = Int32.Parse(Console.ReadLine());

                        if (typKontenera == 40)
                            waga += ilosci[j] * wagi[j] * 0.75f; // dodaj do wagi całkowitej wage kontenera. małe kontenery są 25% lzejsze
                        else
                            waga += ilosci[j] * wagi[j]; // dodaj do wagi całkowitej wage kontenera
                    }

                    string strefa = $"{maxIloscStrefy} {iloscZaladunku} {waga}";

                    for (int j = 0; j < ilosci.Length; j++)
                    {
                        strefa += $" {ilosci[j]}";
                    }

                    Console.WriteLine("Calkowita waga strefy: " + waga);
                    Console.WriteLine("Numer strefy "+ i+1);

                    linie[i] = strefa;
                }

                File.WriteAllLines("statek" + wyborStatku + ".txt", linie); // Tworzy nowy plik tekstowy dla kazdego statku 

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
        case 3:
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
        case 4:
            {
                goto start;
            }
            break;
    }
    goto panelAdmina;
}

else
{
    Console.WriteLine("Witaj uzytkowniku");
K:
    Console.WriteLine("Wpisz 1, jesli sprawdziać lokalizacje kontenera");
    Console.WriteLine("Wpisz 2, jesli chcesz sie wylogować");
    Console.WriteLine();
    wybor = Int32.Parse(Console.ReadLine());
    if (wybor == 1)
    {
        Console.WriteLine(lokalizacje[miasto]);
        goto K;
    }
    else if (wybor == 2)
    {
        goto start;
    }
    else
    {
        Console.WriteLine("Zle wprowadzona liczba");
        Console.WriteLine();
        goto K;
    }
};