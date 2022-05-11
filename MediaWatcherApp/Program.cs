using System;
using MediaWatcherLib;

namespace MediaWatcherApp {
    class Program {
        static void Main(string[] args) {
            var watcher = new MediaWatcher();
            watcher.MediaChanged += (s, e) => {
                Console.Write($"Playing={e.IsPlaying}");
                if (e.Artist != null && e.Title != null) {
                    Console.Write($": {e.Artist} - {e.Title}");
                }
                Console.WriteLine();
            };
            watcher.Start();
            Console.ReadLine();
            watcher.Shutdown();
        }
    }
}
