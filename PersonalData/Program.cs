namespace PersonalData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Person
{
    public string FullName { get; set; }
    public string PersonalID { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    public Person(string fullName, string personalID, DateTime dateOfBirth, string gender)
    {
        FullName = fullName;
        PersonalID = personalID;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }
}
public class Program
{
    public static void Main()
    {
        string inputFilePath = "People.txt";
        string outputFilePath = "People_Full_Data.txt";
        List<Person> people = new List<Person>();
        try
        {
            string[] lines = File.ReadAllLines(inputFilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 2)
                {
                    string fullName = parts[0].Trim();
                    string personalID = parts[1].Trim();
                    DateTime dateOfBirth;
                    string gender;

                    if (ValidateAndExtractInfo(personalID, out dateOfBirth, out gender))
                    {
                        Person person = new Person(fullName, personalID, dateOfBirth, gender);
                        people.Add(person);
                    }
                }
            }
            List<Person> orderedPeople = people.OrderBy(p => p.FullName).ThenBy(p => p.DateOfBirth).ToList();
            foreach (Person person in orderedPeople)
            {
                Console.WriteLine($"{person.FullName}, {person.PersonalID}, {person.DateOfBirth:yyyy-MM-dd}, {person.Gender}");
            }
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (Person person in orderedPeople)
                {
                    writer.WriteLine($"{person.FullName}, {person.PersonalID}, {person.DateOfBirth:yyyy-MM-dd}, {person.Gender}");
                }
            }
            Console.WriteLine($"Data processed successfully. Results saved in {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private static bool ValidateAndExtractInfo(string personalID, out DateTime dateOfBirth, out string gender)
    {
        dateOfBirth = DateTime.MinValue;
        gender = string.Empty;
        if (personalID.Length != 10 || !long.TryParse(personalID, out long _))
        {
            return false;
        }
        int year = int.Parse(personalID.Substring(0, 2));
        int month = int.Parse(personalID.Substring(2, 2));
        int day = int.Parse(personalID.Substring(4, 2));
        if (month > 40)
        {
            year += 2000;
            month -= 40;
        }
        else if (month > 20)
        {
            year += 1800;
            month -= 20;
        }
        else
        {
            year += 1900;
        }
        try
        {
            dateOfBirth = new DateTime(year, month, day);
        }
        catch
        {
            return false;
        }
        if (personalID[8] == '0' || personalID[8] == '2' || personalID[8] == '4' || personalID[8] == '6' || personalID[8] == '8')
        {
            gender = "Male";
        }
        else
        {
            gender = "Female";
        }
        return true;
    }
}
