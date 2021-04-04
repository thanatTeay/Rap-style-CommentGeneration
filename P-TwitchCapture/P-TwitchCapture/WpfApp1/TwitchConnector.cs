using System;
using System.Collections.Generic;
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

        public static string twitch_username = "ligoligo12";
        public static string access_token = "3ft0m2ciq6t950fwix9i9v3q12a70r";
        public static string refresh_token = "9bu7oqgwkhbdz895dx6qu4bdzak6xc63nf9iofxds0jr0f9o0m";
        public static string client_id = "gp762nuuoqcoxypju8c569th9wz7q5";
        public static string channel = "ligoligo12";

        public TwitchClient client;
        MainWindow main;
        public Bot(MainWindow m)
        {
            main = m;
            //createClient();
        }

        public void createClient()
        {
            //ConnectionCredentials credentials = new ConnectionCredentials("twitch_username", "access_token");
            ConnectionCredentials credentials = new ConnectionCredentials(twitch_username, access_token);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,

                ThrottlingPeriod = TimeSpan.FromSeconds(30),
                //SendDelay = 1000

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
            //client_g.Connect();
        }


        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");

            client_g = client;
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            //Console.WriteLine("Hey guys! I am a moderator AI" + ii);ii++;
            //Console.WriteLine("Hey guys! I am a moderator AI");
            //client.SendMessage(e.Channel, "Hey guys! I am a moderator AI");
            botSpeak(e.Channel, "Hey guys! I am a moderator AI");
        }



        int ii = 0;
        DateTime nextMsg = DateTime.Now;
        public void botSpeak(string sender,string txt)
        {
            DateTime timeA = DateTime.Now;
            if (timeA > nextMsg)
            {
                Console.WriteLine(txt);
                //client.SendMessage(sender, txt);
                client.SendMessage(sender, txt + "_" + ii); ii++;

                //client.SendMessage(sender, txt + "_" + ii, true); ii++;
                //client.SendMessage(sender, txt + "X" + ii, false); ii++;
                //client.SendRaw(txt + "R" + ii); ii++;
                //client.SendQueuedItem(txt + "Q" + ii); ii++;

                //public void SendMessage(string channel, string message, bool dryRun = false);
                //public void SendMessage(JoinedChannel channel, string message, bool dryRun = false);
                //public void SendQueuedItem(string message);
                //public void SendRaw(string message);


                nextMsg = timeA.AddMilliseconds(2000);
            }
        }


        public static TwitchClient client_g;
        public static int ii_g = 0;
        public static DateTime nextMsg_g = DateTime.Now;
        public static void botSpeakGlobal(string sender, string txt)
        {
            DateTime timeA = DateTime.Now;
            if (timeA > nextMsg_g)
            {
                Console.WriteLine(txt);
                //client.SendMessage(sender, txt);
                client_g.SendMessage(sender, txt + "_" + ii_g); ii_g++;
                nextMsg_g = timeA.AddMilliseconds(2000);
            }
        }

        //PREVENT DUPLICATED MESSAGES BY MULTIPLE BOT
        public static void botSpeakListGlobal(string sender, List<string> ltxt)
        {
            DateTime timeA = DateTime.Now;
            if (timeA > nextMsg_g)
            {
                foreach(string txt in ltxt)
                {
                    Console.WriteLine(txt);
                    //client.SendMessage(sender, txt);
                    client_g.SendMessage(sender, txt );
                }
                nextMsg_g = timeA.AddMilliseconds(2000);
            }
        }



        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            //if (e.ChatMessage.Message.Contains("badword"))
            //    client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");

            //Console.WriteLine($"NewMsg " + e.ChatMessage.Message);
           if(e.ChatMessage.Username != "ligoligo12")
           {
                main.getMsg(e.ChatMessage.Username, e.ChatMessage.Message);
           }
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