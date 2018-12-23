using System;
using System.Collections.Generic;

namespace MagazineAPI
{
    class Program
    {
        private static readonly ApiClient client = new ApiClient();
        private static List<string> Categories;
        private static List<Subscriber> Subscribers;
        private static Dictionary<string, List<Magazine>> MagazinesByCategory = new Dictionary<string, List<Magazine>>();

        static void Main(string[] args)
        {
            Categories = client.GetCategoriesAsync().Result;
            Subscribers = client.GetSubscribersAsync().Result;

            foreach (var category in Categories)
            {
                List<Magazine> magazines = client.GetMagazinesWithCategoryAsync(category).Result;
                MagazinesByCategory[category] = magazines;
            }

            List<Subscriber> subscribersToAllCategories = Subscribers.FindAll(SubscribesToAllCategories);
            List<string> subscriberIds = new List<string>();

            foreach (var subscriber in subscribersToAllCategories)
                subscriberIds.Add(subscriber.id);

            PostResult resultObj = client.SubmitAnswerAsync(subscriberIds).Result;

            Console.WriteLine($"Answer correct: {resultObj.answerCorrect}");
            Console.WriteLine($"Execution time: {resultObj.totalTime}");
        }

        private static bool SubscribesToAllCategories(Subscriber s)
        {
            int numCategories = 0;

            foreach (var category in Categories)
            {
                if (s.magazineIds.Exists((int id) => MagazinesByCategory[category].Exists((Magazine m) => m.id == id)))
                    numCategories++;

            }

            return numCategories == Categories.Count;
        }
    }
}
