using System;
using System.IO;
using System.Linq;

namespace MovieLibraryA4
{
    class Program
    {
        public static String file = "movies.csv"; //in case the path to the file needs to be changed, I only need to change it in one place
        static void Main(string[] args)
        {
            var done = false;
            while(!done)
            {
                var isValid = false;
                int choice = 0;
                while (!isValid)
                {
                    Console.Write("Choose an option:\n1. List all movies on record\n2. Add movie to file\n3. Exit\n>");
                    var response = Console.ReadLine();
                    isValid = Int32.TryParse(response, out choice);
                    if(!isValid)
                    {
                        Console.WriteLine("ERROR: Input was not a number.\n"); //Extra newline to make output cleaner.
                    }
                }
               
                
                switch (choice)
                { 
                    case 1:
                        ReadMovies();
                        break;
                    case 2:
                        AddMovie();
                        break;
                    case 3:
                        done = true;
                        Console.WriteLine("Exiting...\n");
                        break;
                    default:
                        Console.WriteLine("ERROR: Please choose one of the given options.\n");
                        break;
                }
            }
        }
        public static void ReadMovies()
        {
            try
            {
                StreamReader sr = new StreamReader(file);
                var linecount = 0;
                while (!sr.EndOfStream) //I wanted to get all the lines in the file, there are probably cleaner ways to do it but none I found worked.
                {
                    sr.ReadLine();
                    linecount++;
                }
                sr.Close();
                StreamReader sr2 = new StreamReader(file);
                var count = 0;
                var goal = 1000;
                var doneCounting = false;
                while (!sr2.EndOfStream && !doneCounting)
                {
                    if (count < goal)
                    {
                        Console.WriteLine(sr2.ReadLine());
                        count++;
                    }
                    else
                    {
                        Console.WriteLine($"\nRead {goal} of {linecount} lines.");
                        Console.Write("Continue reading? (Y/N)\n>");
                        char response = Char.ToUpper(Console.ReadLine()[0]);
                        if (!response.Equals('Y'))
                        {
                            doneCounting = true;
                        }
                        else
                        {
                            if (goal + 1000 > linecount)
                            {
                                goal = linecount;
                            }
                            else
                            {
                                goal = goal + 1000;
                            }
                        }
                    }
                    if (sr2.EndOfStream)
                    {
                        Console.WriteLine($"\nRead {linecount} of {linecount} lines.");
                    }
                }
                sr2.Close();
            }
            catch(IOException e)
            {
                Console.WriteLine("ERROR: File not found.\n");
            }
        }
        public static bool AddMovie()
        {
            try
            {
                String last = File.ReadLines(file).Last(); //getting the last line in the movie list
                String[] splitmovie = last.Split(',');
                bool canParse = Int32.TryParse(splitmovie[0], out int result); //get the last movie's ID, since it's the highest
                if (!canParse)
                {
                    return false;
                }
                result++;
                Console.Write("Please input the movie's name and year.\n>");
                var name = Console.ReadLine();
                StreamReader sr = new StreamReader(file);
                while (sr != null && !sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    String[] splitline = line.Split(",");
                    if (splitline[1].Equals(name))
                    {
                        Console.WriteLine("ERROR: Movie with that name and year already exists.\n");
                        sr.Close();
                        sr = null;
                        return false;
                    }

                }
                sr.Close();
                Console.Write("Input the movie's genre.\n>");
                var genre = Console.ReadLine();
                String fullmovie = result + "," + name + "," + genre;
                StreamWriter sw = new StreamWriter(file, true);
                sw.WriteLine(fullmovie);
                sw.Close();
                return true;
            }
            catch(IOException e)
            {
                Console.WriteLine("ERROR: File not found.\n");
                return false;
            }
        }
    }
}
