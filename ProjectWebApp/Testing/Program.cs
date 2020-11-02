using System;
using System.Collections.Generic;
using System.Linq;

namespace Testing
{
    class Program
    {
        private static readonly List<Exercise> _exercises = new List<Exercise>();

        static void Main(string[] args)
        {
            var stringList = new List<string>();

            stringList.Add("Benchpress");
            stringList.Add("10");
            stringList.Add("3");

            stringList.Add("Squat");
            stringList.Add("20");
            stringList.Add("4");

            TestCase(stringList);

            foreach (var exercise in _exercises)
            {
                Console.WriteLine($"Name: {exercise.Name}\n Reps: {exercise.Reps} \n Sets: {exercise.Sets} \n");
            }
        }

        private static void TestCase(List<string> list)
        {
            if (list == null || !list.Any()) return;

            var propAmount = typeof(Exercise).GetProperties().Length;
            var propData = list.Take(propAmount).ToList();

            for (var i = 0; i < propAmount; i++)
            {
                list.RemoveAt(0);
            }

            var exercise = new Exercise();

            foreach (var prop in exercise.GetType().GetProperties())
            {
                if (propData.Count.Equals(0)) break;

                if (prop.PropertyType == typeof(int))
                {
                    prop.SetValue(exercise, int.Parse(propData.ElementAt(0)), null);
                }
                else
                {
                    prop.SetValue(exercise, propData.ElementAt(0), null);
                }

                propData.RemoveAt(0);
            }

            _exercises.Add(exercise);

            TestCase(list);
        }

    }

    public class Exercise
    {
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
    }
}
