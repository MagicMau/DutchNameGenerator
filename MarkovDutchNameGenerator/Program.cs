using System;

namespace MarkovDutchNameGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            var firstNameGenerator = new MarkovNameGenerator(6, 10);
            firstNameGenerator.FeedFile("FirstNames.txt");

            var lastNameGenerator = new MarkovNameGenerator(6, 20);
            lastNameGenerator.FeedFile("LastNames.txt");

            for (int i = 0; i < 100; i++)
            {
                string firstName = firstNameGenerator.Generate();
                string lastName = lastNameGenerator.Generate();
                string name = firstName + " " + lastName;

                Console.WriteLine(name);
            }

            Console.ReadLine();
        }
    }
}