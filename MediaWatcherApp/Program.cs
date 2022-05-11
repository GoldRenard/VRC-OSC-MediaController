using System;
using MediaWatcherLib;

namespace MediaWatcherApp {
    class Program {
        static void Main(string[] args) {
            var watcher = new MediaWatcher();
            watcher.Start();
            Console.ReadLine();
            watcher.Shutdown();
        }
    }
}
