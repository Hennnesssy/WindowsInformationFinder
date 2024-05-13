using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsInformationFinder
{
    public partial class Form1 : Form
    {
        System.Threading.Timer timer;

        //Mail Class
        string mailSubject;
        string mailBody;
        public Form1()
        {
            InitializeComponent();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(@"C:\", "*.*");
            watcher.EnableRaisingEvents = true;
            watcher.SynchronizingObject = this;
            watcher.IncludeSubdirectories = true;

            watcher.Created += new FileSystemEventHandler(Watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(Watcher_Deleted);
            watcher.Renamed += new RenamedEventHandler(Watcher_Renamed);

            StartTimer();
        }
        void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string path = @"C:\Users\Administrator\Desktop\statistic.txt";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
                streamWriter.WriteLine($"file created \"{e.Name}\", {DateTime.Now}\n");
            updateSendStatistic();
        }
        void Watcher_Renamed(object sender, FileSystemEventArgs e)
        {
            string path = @"C:\Users\Administrator\Desktop\statistic.txt";
            string nameFile = e.Name;
            using (StreamWriter streamWriter = new StreamWriter(path, true))
                streamWriter.WriteLine($"file \"{nameFile}\" renamed \"{e.Name}\", {DateTime.Now}\n");
            updateSendStatistic();
        }
        void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string path = @"C:\Users\Administrator\Desktop\statistic.txt";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
                streamWriter.WriteLine($"file Deleted \"{e.Name}\", {DateTime.Now}\n");
            updateSendStatistic();
        }

        void SendToMail(string mailSubject, string mailBody, string attachmentFilePath)
        {
            Mail mail = new Mail();
            mail.SendEmailNotification(mailSubject, mailBody, @"C:\Users\Administrator\Desktop\statistic.txt");
        }
        void updateSendStatistic()
        {
            mailSubject = $"Statistki {DateTime.Now}";
            mailBody = File.ReadAllText(@"C:\Users\Administrator\Desktop\statistic.txt");
        }
        void StartTimer()
        {
            timer = new System.Threading.Timer((e) =>
            {
                updateSendStatistic();
                SendToMail(mailSubject, mailBody, @"C:\Users\Administrator\Desktop\statistic.txt");
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }
    }
}

