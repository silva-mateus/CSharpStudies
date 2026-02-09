using System;

namespace Coding.Exercise
{
    public class Dog
    {
        string Name;
        string Breed;
        int Weight;


        public Dog(string name, string breed, int weight)
        {
            Name = name;
            Breed = breed;
            Weight = weight;
        }

        public Dog(string name, int weight)
        {
            Name = name;
            Weight = weight;
            Breed = "mixed-breed";
        }

        public string Describe()
        {
            string weightDescription;

            if (Weight < 5)
            {
                weightDescription = "tiny";
            }
            else if (Weight >= 5 && Weight < 30)
            {
                weightDescription = "medium";
            }
            else
            {
                weightDescription = "large";
            }

            return $"This dog is named {Name}, it's a {Breed}, and it weighs {Weight} kilograms, so it's a {weightDescription} dog.";
        }
    }
}
