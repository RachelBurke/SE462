using System;

namespace FerryLegacy
{
    /**
     * The Main Program
     * Edits:
     *   Move all commands to command class
     *   Added instance of commands class 
     *   Use commands class to send a command into the commands class 
     */
    public class Program
    {

        // Main - Start reading commands
        public static void Main()
        {
            ManagementSystem managementSystem = new ManagementSystem();
            CommandLoop();
        }

        // Command Reader - reads in commands from console
        private static void CommandLoop()
        {
            string line = "start";
            while (line != "quit")
            {
                Commands.Command(line);
                line = (Console.ReadLine() ?? "").ToLower();
            }
        }
    }
}
