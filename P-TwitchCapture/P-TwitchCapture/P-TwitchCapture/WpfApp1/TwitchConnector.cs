using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;


namespace PTwitchCapture
{

    class Bot
    {
        //https://github.com/TwitchLib/TwitchLib
        //https://twitchtokengenerator.com/

        //public static string twitch_username = "MarionetteMaster";
        //public static string access_token = "onbyl2dwuc0cmi8m5mv4m6vi3fuv47";
        //public static string refresh_token = "858rb6q9dtg8h8ga8q5srwnc4kwgmwi8em78ibb4fsh8v69n7l";
        //public static string client_id = "gp762nuuoqcoxypju8c569th9wz7q5";
        //public static string channel = "icelabuki";

        /*public static string twitch_username = "MarionetteMaster";
        public static string access_token = "w28vhioyr1srjk0fb96cb74oudwjfo";
        public static string refresh_token = "8iahdoptpt8io4zjidf2or4pnaus3rw3ycsk2uwsikfz51bdjk";
        public static string client_id = "gp762nuuoqcoxypju8c569th9wz7q5";
        public static string channel = "icelabtwitch";*/

        public static string twitch_username = "ligoligo12";
        public static string access_token = "8jdu5hk9pklsxyioggnx3k08nyafls";
        public static string refresh_token = "q0rootdyymy6cvujeqjblkc0sy5buxjxkbi1l8blm7zmmo6rg2";
        public static string client_id = "gp762nuuoqcoxypju8c569th9wz7q5";
        public static string channel = "ligoligo12";

        TwitchClient client;
        MainWindow main;
        public Bot(MainWindow m)
        {
            main = m;
            //ConnectionCredentials credentials = new ConnectionCredentials("twitch_username", "access_token");
            ConnectionCredentials credentials = new ConnectionCredentials(twitch_username, access_token);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, channel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Hey guys! I am a moderator AI");
            client.SendMessage(e.Channel, "Hey guys! I am a moderator AI");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("badword"))
                client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");

            //Console.WriteLine($"NewMsg " + e.ChatMessage.Message);
            main.getMsg(e.ChatMessage.Username, e.ChatMessage.Message);
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
                client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
    }



}