using MathGame.glaxxie;

Console.CursorVisible = false;

bool menuSelection = true;
string greeting = "Hello and welcome to the Math game!";

Console.WriteLine("~".PadRight(greeting.Length, '~'));
Console.WriteLine(greeting);
Console.WriteLine("~".PadRight(greeting.Length, '~'));
Console.WriteLine(
@" 
   +             x   x  
   +              x x      O
+++++++  ------    x    =======
   +              x x      O
   +             x   x 
");
Console.WriteLine("-".PadRight(greeting.Length, '-'));
Thread.Sleep(1000);

do
{
    Menu.MainMenu();
} while (menuSelection);

// TODO: Add modulo mode