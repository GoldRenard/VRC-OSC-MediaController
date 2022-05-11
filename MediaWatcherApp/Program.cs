using System;
using MediaWatcherLib;

namespace MediaWatcherApp {
    class Program {
        static void Main(string[] args) {
            var watcher = new MediaWatcher();
            watcher.MediaChanged += (s, e) => {
                if (e.IsPlaying) {
                    Console.Write("Playing");
                } else {
                    Console.Write("Stopped/Paused");
                }
                if (e.Artist != null && e.Title != null) {
                    Console.Write($": {e.Artist} - {e.Title}");
                }
                Console.WriteLine();
            };
            watcher.Start();
            Console.WriteLine("Media state polling started...");
            Console.ReadLine();
            watcher.Shutdown();
        }
    }
}
