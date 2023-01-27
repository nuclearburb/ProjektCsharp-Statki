using System;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading;

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
typeOfUser = "";
login = "";
haslo = "";

Console.Clear();
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
    }
    else
    {
        typeOfUser = "user";
    }
    Console.Write("Wpisz haslo: ");
    haslo = Console.ReadLine();
    if (!File.Exists(typeOfUser + login + ".txt"))
    {
        string temp = "TESTTESTTEST";
        string[] lines =
           {
            login, temp
        };
        File.WriteAllLines("user" + login + ".txt", lines);
    };
    string[] test = System.IO.File.ReadAllLines(typeOfUser + login + ".txt");
    if (test[1] == haslo)
    {
        Console.Clear();
        Console.WriteLine("Zalogowano");
        Thread.Sleep(500);
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Nieprawidlowe haslo!");
        Thread.Sleep(500);
        goto start; // wraca na gore
    };
}
Console.Clear();

if (typeOfUser == "admin")
{
    Console.WriteLine("-------------Witaj adminie----------------");
panelAdmina:
    Console.WriteLine("1. Podglad calej floty");
    Console.WriteLine("2. Zarzadzanie statkiem");
    Console.WriteLine("3. Zmiana danych uzytkownika");
    Console.WriteLine("4. Wyloguj");
    Console.WriteLine("5. Pokaz zlecenia");
    Console.WriteLine();

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
                    "Waga   ",
                    "Elektronika",
                    "Zabawki",
                    "Jedzenie",
                    "Plastiki",
                    "Chemia",
                    "AGD",
                    "Pojazdy",
                    "Ubrania"
                };

                string[] dirs = Directory.GetFiles(".", "statek*"); //przeszukiwanie folderu 
                Console.WriteLine("Statki: ", dirs.Length);
                foreach (string dir in dirs)
                {
                    string str = dir;
                    str = dir.Remove(dir.Length - 4, 4);
                    str = str.Remove(0, 2);
                    Console.WriteLine(str);
                }
            L:
                Console.Write("Podaj numer statku: ");
                int wyborStatku = Int32.Parse(Console.ReadLine()); // 1 lub 2
                Console.WriteLine();
                if (wyborStatku!=1 && wyborStatku!=2)
                {
                    goto L;
                }
                int[,] statek = new int[4, 13];

                var lines = File.ReadAllLines("statek" + wyborStatku + ".txt"); // przepisuje plik tekstowy do tabelki
                Console.WriteLine("Statek nr: " + wyborStatku);
                Console.WriteLine("|Typ Strefy|Waga Max|Ilosc Max|Waga   |Elektronika|Zabawki|Jedzenie|Plastiki|Chemia|AGD|Pojazdy|Ubrania|");

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
                GA:
                Console.Write("Podaj numer statku: ");
                int wyborStatku = int.Parse(Console.ReadLine()); // 1 lub 2
                    if (wyborStatku != 1 && wyborStatku != 2)
                    {
                        goto GA;
                    }


                float[,] statek = new float[4, 13];

                var lines = File.ReadAllLines("statek" + wyborStatku + ".txt"); // przepisuje plik tekstowy do tabelki
                for (int x = 0; x < 4; x++)
                {
                    string[] words = lines[x].Split(' ');

                    int indeks = 0;
                    foreach (var word in words)
                    {
                        statek[x, indeks] = float.Parse(word);
                        indeks++;
                    }
                };

                Console.WriteLine("Wpisz 1 jesli chcesz dodac kontener" + "\n" + "Wpisz 2 jesli chcesz usunac kontener");
                int wybor_zarzadzania = int.Parse(Console.ReadLine());

                if (wybor_zarzadzania == 1) //dodawanie
                {
                    int typKontenera = 0;
                    while (typKontenera != 40 && typKontenera != 60)
                    {
                        Console.Write("Podaj typ kontenera (40 albo 60): ");
                        typKontenera = Int32.Parse(Console.ReadLine()); // 40 lub 60;
                    }
                    Console.WriteLine();
                    int iloscZaladunku = 300;
                    int maxIloscStrefy = 100;
                    int iloscStref = 5;
                    float waga = 0;
                    int[] ilosci = new int[8];
                    if (typKontenera == 60) // dodawanie dla 60tek
                    {
                        int n = 0;
                        for (int m = 1; m < 9; m++)
                        {
                            Console.WriteLine($"{m} " + $"{nazwy[n]}");
                            n++;
                        }
                        Console.WriteLine();

                    wybortypu:
                        int wybor_typu = 0;
                        Console.Write("Podaj jaki chcesz dodac kontener: ");
                        int temp = 0;
                        temp = int.Parse(Console.ReadLine());
                        if ((temp > 0) && (temp <= 8))
                        {
                            wybor_typu = temp;
                        }
                        else
                        {
                            goto wybortypu;
                        };
                            for (int j = wybor_typu - 1; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerow z zawartoscia {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            Console.WriteLine();
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
                                if (statek[2, j + 4] < statek[3, j + 4])
                                {
                                    if ((statek[2, j + 4] + 1) < 200)
                                    {
                                        statek[2, j + 4] = statek[2, j + 4] + 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[2, 3] = statek[2, 3] + wagi[j];
                                    }
                                }
                                else
                                {
                                    if ((statek[3, j + 4] + 1) < 200 && ilosci[j] > 0)
                                    {
                                        statek[3, j + 4] = statek[3, j + 4] + 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[3, 3] = statek[3, 3] + wagi[j];
                                    }
                                }
                            }
                        S:
                            waga = 0;
                            break;
                        }
                    M:
                        Console.WriteLine("Wpisz 1 jesli chcesz dodac kolejne kontenery" + "\n" + "Wpisz 2 jesli chcesz wyjsc z zarzadzania");
                        int wybor_wyjscia = int.Parse(Console.ReadLine());
                        if (wybor_wyjscia == 1)
                        {
                            goto wybortypu;
                        }
                        else if (wybor_wyjscia == 2)
                        {
                            Console.WriteLine();
                            goto koniec;
                        }
                        else
                        {
                            Console.WriteLine("Zle wprowadzona liczba");
                            Console.WriteLine();
                            goto M;
                        }
                        goto wybortypu;
                    } // dodawanie dla 60tek

                    if (typKontenera == 40) // dodawanie dla 40tek
                    {
                        int n = 0;
                        for (int m = 1; m < 9; m++)
                        {
                            Console.WriteLine($"{m} " + $"{nazwy[n]}");
                            n++;
                        }
                        Console.WriteLine();

                    wybortypu:
                        int wybor_typu = 0;
                        Console.Write("Podaj jaki chcesz dodac kontener: ");
                        int temp = 0;
                        temp = int.Parse(Console.ReadLine());
                        if ((temp > 0) && (temp <= 8))
                        {
                            wybor_typu = temp;
                        }
                        else
                        {
                            goto wybortypu;
                        };
                        for (int j = wybor_typu - 1; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerow z zawartoscia {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            Console.WriteLine();
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
                                if (statek[0, j + 4] < statek[1, j + 4])
                                {
                                    if ((statek[0, j + 4] + 1) < 100)
                                    {
                                        statek[0, j + 4] = statek[0, j + 4] + 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[0, 3] = statek[0, 3] + (wagi[j] * 0.75f);
                                    }
                                }
                                else
                                {
                                    if ((statek[1, j + 4] + 1) < 100 && ilosci[j] > 0)
                                    {
                                        statek[1, j + 4] = statek[1, j + 4] + 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[1, 3] = statek[1, 3] + (wagi[j] * 0.75f);
                                    }
                                }
                            }
                        S:
                            waga = 0;
                            break;
                        }
                    M:
                        Console.WriteLine("Wpisz 1 jesli chcesz dodac kolejne kontenery" + "\n" + "Wpisz 2 jesli chcesz wyjsc z zarzadzania");
                        int wybor_wyjscia = int.Parse(Console.ReadLine());
                        if (wybor_wyjscia == 1)
                        {
                            goto wybortypu;
                        }
                        else if (wybor_wyjscia == 2)
                        {
                            Console.WriteLine();
                            goto koniec;
                        }
                        else
                        {
                            Console.WriteLine("Zle wprowadzona liczba");
                            Console.WriteLine();
                            goto M;
                        }
                        goto wybortypu;
                    }
                    //dodawanie dla 40tek
                } //dodawanie
                else if (wybor_zarzadzania == 2) //usuwanie
                {
                    int typKontenera = 0;
                    while (typKontenera != 40 && typKontenera != 60)
                    {
                        Console.Write("Podaj typ kontenera (40 albo 60): ");
                        typKontenera = Int32.Parse(Console.ReadLine()); // 40 lub 60;
                    }
                    Console.WriteLine();
                    int iloscZaladunku = 300;
                    int maxIloscStrefy = 100;
                    int iloscStref = 5;
                    float waga = 0;
                    int[] ilosci = new int[8];
                    if (typKontenera == 60) // usuwanie dla 60tek
                    {
                        int n = 0;
                        for (int m = 1; m < 9; m++)
                        {
                            Console.WriteLine($"{m} " + $"{nazwy[n]}");
                            n++;
                        }
                        Console.WriteLine();
                    wybortypu:
                        int wybor_typu = 0;
                        Console.Write("Podaj jaki chcesz usunac kontener: ");
                        int temp = 0;
                        temp = int.Parse(Console.ReadLine());
                        if ((temp > 0) && (temp <= 8))
                        {
                            wybor_typu = temp;
                        }
                        else
                        {
                            goto wybortypu;
                        };
                        for (int j = wybor_typu - 1; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerow do usunieca z zawartoscia {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            Console.WriteLine();
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }
                            waga += ilosci[j] * wagi[j];
                            if (0 < (statek[2, j + 4] + statek[3, j + 4] - ilosci[j]))
                            {
                                Console.WriteLine("Brak kontenerow na strefie");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (ilosci[j] == statek[2, j + 4] + statek[3, j + 4])
                            {
                                statek[2, j + 4] = 0;
                                statek[3, j + 4] = 0;
                                statek[2, 3] -= (statek[2, j + 4] * wagi[j]);
                                statek[3, 3] -= (statek[3, j + 4] * wagi[j]);
                                goto S;
                            }
                            while (ilosci[j] != 0)
                            {
                                if (statek[2, j + 4] > statek[3, j + 4])
                                {
                                    if ((statek[2, j + 4] - 1) > 0)
                                    {
                                        statek[2, j + 4] = statek[2, j + 4] - 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[2, 3] = statek[2, 3] - wagi[j];
                                    }
                                }
                                else
                                {
                                    if ((statek[3, j + 4] + 1) > 0 && ilosci[j] > 0)
                                    {
                                        statek[3, j + 4] = statek[3, j + 4] - 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[3, 3] = statek[3, 3] - wagi[j];
                                    }
                                }
                            }
                        S:
                            waga = 0;
                            break;
                        }
                    M:
                        Console.WriteLine("Wpisz 1 jesli chcesz usunac kolejne kontenery" + "\n" + "Wpisz 2 jesli chcesz wyjsc z zarzadzania");
                        int wybor_wyjscia = int.Parse(Console.ReadLine());
                        if (wybor_wyjscia == 1)
                        {
                            goto wybortypu;
                        }
                        else if (wybor_wyjscia == 2)
                        {
                            Console.WriteLine();
                            goto koniec;
                        }
                        else
                        {
                            Console.WriteLine("Zle wprowadzona liczba");
                            Console.WriteLine();
                            goto M;
                        }
                        goto wybortypu;
                    } // usuwanie 60tek


                    else if (typKontenera == 40) // usuwanie 40tek
                    {
                        int n = 0;
                        for (int m = 1; m < 9; m++)
                        {
                            Console.WriteLine($"{m} " + $"{nazwy[n]}");
                            n++;
                        }
                        Console.WriteLine();
                    wybortypu:
                        int wybor_typu = 0;
                        Console.Write("Podaj jaki chcesz usunac kontener: ");
                        int temp = 0;
                        temp = int.Parse(Console.ReadLine());
                        if ((temp > 0) && (temp <= 8))
                        {
                            wybor_typu = temp;
                        }
                        else
                        {
                            goto wybortypu;
                        };
                        for (int j = wybor_typu - 1; j < 8; j++)
                        {
                        B:
                            Console.Write($"Podaj ilosc kontenerow do usunieca z zawartoscia {nazwy[j]}: ");
                            ilosci[j] = Int32.Parse(Console.ReadLine());
                            Console.WriteLine();
                   
                            if (ilosci[j] == 0)
                            {
                                goto S;
                            }

                            waga += ilosci[j] * wagi[j];
                            if (0 < (statek[0, j + 4] + statek[1, j + 4] - ilosci[j]))
                            {
                                Console.WriteLine("Brak kontenerow na strefie");
                                ilosci[j] = 0;
                                goto B;
                            }
                            if (ilosci[j] == statek[0, j + 4] + statek[1, j + 4])
                            {
                                statek[0, j + 4] = 0;
                                statek[1, j + 4] = 0;
                                statek[0, 3] -= (ilosci[j] * (wagi[j] * 0.75f));
                                statek[1, 3] -= (ilosci[j] * (wagi[j] * 0.75f));
                                goto S;
                            }
                            while (ilosci[j] != 0)
                            {
                                if (statek[0, j + 4] > statek[1, j + 4])
                                {
                                    if ((statek[0, j + 4] - 1) > 0)
                                    {
                                        statek[0, j + 4] = statek[0, j + 4] - 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[0, 3] = statek[0, 3] - (wagi[j] * 0.75f);
                                    }
                                }
                                else
                                {
                                    if ((statek[1, j + 4] + 1) > 0 && ilosci[j] > 0)
                                    {
                                        statek[1, j + 4] = statek[1, j + 4] - 1;
                                        ilosci[j] = ilosci[j] - 1;
                                        statek[1, 3] = statek[1, 3] - (wagi[j] * 0.75f);
                                    }
                                }
                            }
                        S:
                            waga = 0;
                            break;
                        }
                    M:
                        Console.WriteLine("Wpisz 1 jesli chcesz usunac kolejne kontenery" + "\n" + "Wpisz 2 jesli chcesz wyjsc z zarzadzania");
                        int wybor_wyjscia = int.Parse(Console.ReadLine());
                        if (wybor_wyjscia == 1)
                        {
                            goto wybortypu;
                        }
                        else if (wybor_wyjscia == 2)
                        {
                            goto koniec;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Zle wprowadzona liczba");
                            Console.WriteLine();
                            goto M;
                        }
                        goto wybortypu;
                    } // usuwanie 40tek
                }
            koniec: //wpisywanie do pliku

                string[,] bufor = new string[4, 13];
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        bufor[k, l] = statek[k, l].ToString();
                    }
                }

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
            }
            break;
        case 3:
            {
                string[] dirs = Directory.GetFiles(".", "user*");
                Console.WriteLine("Użytkownicy: ", dirs.Length);
                foreach (string dir in dirs)
                {
                    string str = dir;
                    str = dir.Remove(dir.Length - 4, 4);
                    str = str.Remove(0, 6);
                    Console.WriteLine(str);
                }
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
        case 5:
            {

                for (int i = 1; true; i++)
                {
                    if (File.Exists(i + "_" + "Zlecenie.txt"))
                    {
                        Console.WriteLine("Zlecenie:" + i);
                        StreamReader sr = File.OpenText(i + "_" + "Zlecenie.txt");
                        string s = "";
                        while ((s = sr.ReadLine()) != null) { Console.WriteLine(s); }
                        sr.Close();
                    }
                    else
                    {
                        break;
                    }

                }
            }
            break;
    }
    goto panelAdmina;
}

else if (typeOfUser == "user")
{
K:
    Console.Clear();
    Console.WriteLine("-------------Witaj uzytkowiku-------------");
    Console.WriteLine("Wpisz 1 aby dodac zlecenie");
    Console.WriteLine("Wpisz 2 aby wyswietlic lokalizacje kontenera");
    Console.WriteLine("Wpisz 3 aby sie wylogowac");
    Console.WriteLine("Wpisz 4 aby wylaczyc aplikacje");
    int user_menu = int.Parse(Console.ReadLine());
    Console.Clear();
    switch (user_menu)
    {
        //------------------------------Dodanie zlecenia które admin musi wpisać
        case 1:
            {
                int zlecenie_nr = 1;
                for (int i = 0; true; i++)
                {
                    if (File.Exists(zlecenie_nr + "_" + "Zlecenie.txt"))
                    {
                        zlecenie_nr += 1;
                    }
                    else break;
                }
                int[] u_ilosci = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                StreamWriter su = new StreamWriter(zlecenie_nr + "_" + "Zlecenie.txt");
                su.WriteLine("--------" + login + "--------");
                Console.WriteLine("------------Dodawanie zlecenia------------");
                //------------------------------Wpisanie wartości zlecenia
                int co = 1;
                while (true)
                {
                m:
                    Console.WriteLine("Wybierz 1 jesli chcesz dodac Elektronike");
                    Console.WriteLine("Wybierz 2 jesli chcesz dodac Zabawki");
                    Console.WriteLine("Wybierz 3 jesli chcesz dodac Jedzenie");
                    Console.WriteLine("Wybierz 4 jesli chcesz dodac Plastik");
                    Console.WriteLine("Wybierz 5 jesli chcesz dodac Chemie");
                    Console.WriteLine("Wybierz 6 jesli chcesz dodac AGD");
                    Console.WriteLine("Wybierz 7 jesli chcesz dodac Pojazdy");
                    Console.WriteLine("Wybierz 8 jesli chcesz dodac Ubrania");
                    Console.WriteLine("Jeśli chcesz wyjsc wybierz 0");

                    co = int.Parse(Console.ReadLine());
                    if (co == 0)
                    {
                        break;
                    }
                    if ((co < 0) || (co >= 8)) { 
                        Console.WriteLine("Zla liczba");
                        goto m; 
                    }
                    else
                    {
                        Console.Write($"Podaj ilosc kontenerow z zawartoscia {nazwy[co-1]}: ");
                        u_ilosci[co-1] = int.Parse(Console.ReadLine());
                        su.Write(nazwy[co+1] + ": ");
                        su.WriteLine(u_ilosci[co]);
                    }
                }
            
                su.Close();
                //Otworzenie pliku i wypisanie go w konsoli
                StreamReader sr = File.OpenText(zlecenie_nr + "_" + "Zlecenie.txt");
                string s = "";
                Console.Clear();
                Console.WriteLine("Twoje zlecenie");
                while ((s = sr.ReadLine()) != null) { Console.WriteLine(s); }
                Console.WriteLine("Nacisnij 1 aby wroic: ");
                int user_p = int.Parse(Console.ReadLine());
                if (user_p == 1) { goto K; }
                su.Close();
                Console.ReadKey();

                break;
            }
        //------------------------------Pokazanie lokalizacji statku
        case 2:
            {
                Console.Clear();
                Console.WriteLine("-------------Lokalizacja statku-------------");
                Console.WriteLine("Lokalizacja: " + lokalizacje[miasto]);
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Wcisnij dowolny przycisk zeby wrocic");
                Console.ReadKey();
                goto K;
            }
        //------------------------------Wylogowywanie
        case 3:
            { goto start; }
        //------------------------------Wyłączenie programu
        case 4:
            { break; }

    };
}

else
{
    Console.WriteLine("Zle wprowadzona liczba");
    Console.WriteLine();
    goto start;
}