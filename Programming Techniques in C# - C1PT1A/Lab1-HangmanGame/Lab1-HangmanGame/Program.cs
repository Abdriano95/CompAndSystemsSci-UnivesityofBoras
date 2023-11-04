/* Hängagubbe spel
Det här programmet implementerar ett enkelt konsolbaserat Hängagubbe-spel i C#.
Användaren uppmanas att gissa bokstäver för att komma på ett dolt ord.
Spelet fortsätter tills användaren bestämmer sig för att avsluta.*/
bool playGame = true;
string[] wordList = {"Vispgrädde",
      "Ukulele",
      "Innebandyspelare",
      "Flaggstång",
      "Yxa",
      "Havsfiske",
      "Prisma",
      "Landsbygd",
      "Generositet",
      "Lyckosam",
      "Perrong",
      "Samarbeta",
      "Välartad"};
string[] meaningList = {"Uppvispad grädde",
      "Ett fyrsträngat instrument med ursprung i Portugal",
      "En person som spelar sporten innebandy",
      "En mast man hissar upp en flagga på",
      "Verktyg för att hugga ved",
      "Fiske till havs",
      "Ett transparent optiskt element som bryter ljuset vid plana ytor",
      "Geografiskt område med lantlig bebyggelse",
      "En personlig egenskap där man vill dela med sig av det man har",
      "Att man ofta, eller för stunden har tur",
      "Den upphöjda yta som passagerare väntar på eller stiger på/av ett spårfordon",
      "Att arbeta tillsamans mot ett gemensamt mål",
      "Att någon är väluppfostrad, skötsam, eller lovande"};
Random random = new Random(); // Random objekt som används för att generera slumpmässiga index för att välja ord från ordlistan

(string word, string meaning) GetRandomWordAndMeaning(string[] words, string[] meanings) // Genererar ett slumpmässigt index och returnerar en tuppel som innehåller ordet och dess betydelse från motsvarande arrayer
{
    int index = random.Next(words.Length);
    return (word: words[index].ToLower(), meaning: meanings[index]);
}

void PlayAgain() // Ber användaren att bestämma om han vill spela igen. Ställer in playGame-flaggan baserat på användarens svar
{
    bool again = true;
    while (again)
    {
        Console.Write("Vill du spela igen? (y/n): ");
        string? answer = Console.ReadLine();

        if (string.IsNullOrEmpty(answer) || answer.Length != 1 || !char.IsLetter(answer[0]))
        {
            Console.WriteLine("Ogiltig inmatning! (Skriv antingen Y eller N)");
            continue;
        }
        else if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            playGame = true;
            again = false;
        }
        else if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Tack för att du spelade!");
            playGame = false;
            again = false;
        }
    }
}

void DisplayGameState(string correctWord, string usedLetters, int remainingGuesses)// Visar det aktuella tillståndet för spelet, inklusive delvis avslöjat ord, använda bokstäver och återstående gissningar.
{
    Console.WriteLine($"\nSökt ord: {correctWord}");
    Console.WriteLine($"Använda bokstäver: {usedLetters.ToUpper()}");
    Console.WriteLine($"{remainingGuesses}x gissningar kvar");
}

string GetUserInput()// Ber användaren om input och validerar den.Frågar användaren igen tills en giltig enstaka bokstav skrivs in.
{
    while (true)
    {
        Console.Write("Gissa bokstaven: ");
        string? input = Console.ReadLine(); // input[]

        if (!string.IsNullOrEmpty(input) && input.Length == 1 && char.IsLetter(input[0]))
        {
            return input;
        }
        Console.WriteLine("Ogiltig inmatning. Var vänlig ange EN bokstav.");
    }
}

void HangMan()// Main spelloopen. Implementerar spellogiken, inklusive användarinmatning, spelstatusuppdateringar och vinst/förlustvillkor.
{
    while (playGame)
    {
        Console.Clear();
        // Initiera spelvariabler för den nya omgången. 
        var (randomWord, randomMeaning) = GetRandomWordAndMeaning(wordList, meaningList);
        string correctWord = new string('_', randomWord.Length);
        string usedLetters = "";
        int remainingGuesses = 10;

        Console.WriteLine("Hänga gubbe!");

        while (!correctWord.Equals(randomWord, StringComparison.OrdinalIgnoreCase) && remainingGuesses > 0)
        {
            DisplayGameState(correctWord, usedLetters, remainingGuesses);
            string input = GetUserInput();

            if (usedLetters.Contains(input, StringComparison.OrdinalIgnoreCase))// Kontrollera om den gissade bokstaven finns i ordet.Uppdatera den korrekta Word-strängen och usedLetters därefter.
            {
                Console.WriteLine($"Du har REDAN gissat bokstaven {input.ToUpper()}. Använd en annan bokstav!");
                continue;
            }

            usedLetters += input;

            if (randomWord.Contains(input, StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < randomWord.Length; i++)
                {
                    if (randomWord[i].ToString().Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        correctWord = correctWord.Remove(i, 1).Insert(i, input);
                    }
                }
            }
            else
            {
                remainingGuesses--;
            }
        }

        if (correctWord.Equals(randomWord, StringComparison.OrdinalIgnoreCase))  // Kontrollera om spelets slutförhållanden är och visa lämpliga meddelanden.Be användaren att spela igen eller avsluta.
        {
            Console.WriteLine($"\nDu vann!\nOrdet var {randomWord} ({randomMeaning})");
        }
        else
        {
            Console.WriteLine($"Du har slut på gissningar! Korrekta ordet var {randomWord} ({randomMeaning})");
        }

        //Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
        //Console.ReadKey();

        PlayAgain();
    }
}

HangMan();
