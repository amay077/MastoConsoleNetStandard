using System;
using Mastonet;
using Mastonet.Entities;

namespace MastoConsoleStandard
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("email?");
            var email = Console.ReadLine();
            Console.WriteLine("password?");
            var password = Console.ReadLine();
            Console.WriteLine("Start receive federated timeline. Exit for press enter.");

            RunAsync(email, password);

            Console.ReadLine();
        }

        static async void RunAsync(string email, string password)
        {
            var instanceUrl = "mastodon.cloud";

            // MastodonClient.CreateApp で得たものを保存しとく必要があるけど面倒だから UnitTest(MastodonClientTests)のを使わせてもらった
            //var appRegistration = await MastodonClient.CreateApp(instanceUrl, "MastoConsoleStandard", Scope.Read);
            var appRegistration = new AppRegistration 
            {
                ClientId = "9766a3780217ee179c18dfb9aa234566ff3dfb3dd495f0d3669acfadd9a696d1",
                ClientSecret = "b38cebc9235edb15db12320866c4258606f0172ce5aafa6768f4fb0a4973bf61"
            };

            var client = new MastodonClient(instanceUrl, appRegistration);
            var auth = await client.Connect(email, password);
            var streaming = client.GetPublicStreaming();

            // Register events
            streaming.OnUpdate += (_, e) =>
            {
                Console.WriteLine("--");
                Console.WriteLine($"{e.Status.CreatedAt.ToLocalTime().ToString()} - {e.Status.Account.AccountName}:");
                Console.WriteLine(e.Status.Content);
            };

            // Start streaming
            streaming.Start();
        }
    }
}