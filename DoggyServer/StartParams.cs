using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggyServer
{
    class StartParams
    {
        public const string firstname = "firstname";
        public static void firstname_help() { output("HELP"); }

        public const string lastname = "lastname";
        public static void lastname_help() { output("HELP"); }

        public const string password = "password";
        public const string passwort = "passwort";
        public static void password_help() { output("HELP"); }

        public const string joingroup = "joingroup";
        public static void joingroup_help() { output("HELP"); }

        public const string randomSim = "randomsim";
        public static void randomSim_help() { output("HELP"); }

        public const string getallSims = "getallsims";
        public static void getallSims_help() { output("HELP"); }

        public const string noHomeTP = "nohometp";
        public static void noHomeTP_help() { output("HELP"); }

        public const string timer = "timer";
        public static void timer_help() { output("HELP"); }

        public const string endtimer = "endtimer";
        public static void endtimer_help() { output("HELP"); }

        public const string fromdatebase = "fromdatabase";
        public static void fromdatebase_help() { output("HELP"); }

        public const string loginsim = "loginsim";
        public static void loginsim_help() { output("HELP"); }

        public const string reloadSimDatabase = "reloadsims";
        public static void reloadSimDatabase_help() { output("HELP"); }

        public const string follow = "follow";
        public static void follow_help() { output("HELP"); }

        public const string chatfollow = "chatfollow";
        public static void chatfollow_help() { output("HELP"); }

        public const string public_ = "public";
        public static void public_help() { output("HELP"); }

        public const string scan = "scan";
        public static void scan_help() { output("HELP"); }

        public const string rip = "rip";
        public static void rip_help() { output("HELP"); }

        public const string update = "update";
        public static void update_help() { output("HELP"); }

        public const string findobjekt = "findobject";
        public static void findobjekt_help() { output("HELP"); }

        public const string groupinvide = "groupinvide";
        public static void groupinvide_help() { output("HELP"); }

        public const string dotraffic = "dotraffic";
        public static void dotraffic_help() { output("HELP"); }

        public const string checkonline = "checkonline";
        public static void checkonline_help() { output("HELP"); }

        public const string last = "last";
        public static void last_help() { output("HELP"); }

        public const string home = "home";
        public static void home_help() { output("HELP"); }

        public const string deletepid = "deletepid";
        public static void deletepid_help() { output("HELP"); }

        public const string homeset = "homeset";
        public static void homeset_help() { output("HELP"); }

        public const string marketplace = "marketplace";
        public static void marketplace_help() { output("HELP"); }

        public const string showfriends = "showfriends";
        public static void showfriends_help() { output("HELP"); }

        public const string lookdatabase = "lookdatabase";
        public static void lookdatabase_help() { output("HELP"); }

        public const string noFetchInventory = "nofetchinvetory";
        public static void noFetchInventory_help() { output("HELP"); }

        public const string choiceava = "menu";
        public static void choiceava_help() { output("Not included"); }

        public const string alice_chat = "alice_chat";
        public static void alice_chat_help() { output("Not included"); }

        public const string alice_im = "alice_im";
        public static void alice_im_help() { output("Not included"); }

        public const string alice_groupim = "alice_groupim";
        public static void alice_groupim_help() { output("Not included"); }

        public const string use_remoting = "MCP";
        public static void use_remoting_help() { output("Not included"); }

        public const string mcp_restart = "MCP_restart";
        public static void mcp_restart_help() { output("Not included"); }

        public const string rake = "rake";
        public static void rake_help() { output("Not included"); }

        public const string grid = "grid";
        public static void grid_help() { output("Not included"); }

        public const string avatype = "avatype";
        public static void avatype_help() { output("Not included"); }

        private static void output(string text)
        {
            Console.WriteLine("\n" + text);

            Environment.Exit(0);

            Console.ReadLine();
        }

       
    }

    class Testo
    {
        public static void all()
        {
            
            
        }
    }
}
