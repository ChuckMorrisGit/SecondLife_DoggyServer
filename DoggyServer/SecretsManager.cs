using System;
using Microsoft.Extensions.Configuration;

namespace DoggyServer
{
    public static class SecretsManager
    {
        private static IConfiguration _config;

        public static string MySqlServer { get; private set; }
        public static string MySqlDatabase { get; private set; }
        public static string MySqlUserId { get; private set; }
        public static string MySqlPassword { get; private set; }
        public static bool MySqlPooling { get; private set; }

        public static string OpenSimServer { get; private set; }
        public static string OpenSimDatabase { get; private set; }
        public static string OpenSimUserId { get; private set; }
        public static string OpenSimPassword { get; private set; }

        public static string DefaultAvatarPassword { get; private set; }
        public static string BackupAvatarPassword { get; private set; }
        public static string SecondaryAvatarPassword { get; private set; }

        public static string SmtpFromAddress { get; private set; }
        public static string SmtpPassword { get; private set; }
        public static string SmtpServer { get; private set; }
        public static int SmtpPort { get; private set; }

        public static string LogPath { get; private set; }
        public static bool DebugMode { get; private set; }

        public static string GetMySqlConnectionString() =>
            $"Server={MySqlServer};Database={MySqlDatabase};User ID={MySqlUserId};Password={MySqlPassword};Pooling={MySqlPooling};";
        public static string GetOpenSimConnectionString() =>
            $"Server={OpenSimServer};Database={OpenSimDatabase};User ID={OpenSimUserId};Password={OpenSimPassword};Pooling={MySqlPooling};";

        public static void Initialize()
        {
            try
            {
                _config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
                    .Build();

                MySqlServer = Get("MySqlServer", "localhost");
                MySqlDatabase = Get("MySqlDatabase", "SecondLife");
                MySqlUserId = Get("MySqlUserId", "md");
                MySqlPassword = Get("MySqlPassword", "");
                MySqlPooling = GetBool("MySqlPooling", false);

                OpenSimServer = Get("OpenSimServer", "localhost");
                OpenSimDatabase = Get("OpenSimDatabase", "opensim_temp");
                OpenSimUserId = Get("OpenSimUserId", "opensimuser");
                OpenSimPassword = Get("OpenSimPassword", "");

                DefaultAvatarPassword = Get("DefaultAvatarPassword", "");
                BackupAvatarPassword = Get("BackupAvatarPassword", "");
                SecondaryAvatarPassword = Get("SecondaryAvatarPassword", "");

                SmtpFromAddress = Get("SmtpFromAddress", "");
                SmtpPassword = Get("SmtpPassword", "");
                SmtpServer = Get("SmtpServer", "smtp.gmail.com");
                SmtpPort = GetInt("SmtpPort", 587);

                LogPath = Get("LogPath", "./logs");
                DebugMode = GetBool("DebugMode", false);

                Console.WriteLine("[SecretsManager] Secrets erfolgreich geladen");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SecretsManager] Fehler: {ex.Message}");
                throw;
            }
        }

        private static string Get(string key, string defaultValue) =>
            _config[key] ?? defaultValue;

        private static bool GetBool(string key, bool defaultValue) =>
            bool.TryParse(_config[key], out var result) ? result : defaultValue;

        private static int GetInt(string key, int defaultValue) =>
            int.TryParse(_config[key], out var result) ? result : defaultValue;
    }
}
