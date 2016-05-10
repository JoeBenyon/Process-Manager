using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager
{
    class Program
    {
        static bool isRunning = true;
        static void Main(string[] args)
        {

            while (isRunning)
            {
                Console.WriteLine("What would you like to do?\n1. Start Process\n2. Kill Process\n3. Check Process");
                var choice = Console.ReadKey().Key;
                switch (choice)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        startProcess();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        killProcess();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        checkProcess();
                        break;
                    case ConsoleKey.Escape:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine(choice);
                        Console.WriteLine("Please choose a valid option");
                        break;
                }
            }
        }

        //Array containing search results
        private static Process[] results;

        //Start a new process
        private static void startProcess()
        {
            Console.WriteLine("Enter a process name/file: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter any arguements: ");
            string args = Console.ReadLine();

            try
            {
                Process.Start(name, args);
                Console.WriteLine("Process Started");
            }
            catch (Exception e)
            {
                Console.WriteLine("{ERROR} Process can't be started");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Escape to quit or press any key to continue");
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                isRunning = false;
            }
            Console.Clear();

        }

        //End a process
        private static void killProcess()
        {
            Console.WriteLine("Enter a process name (LIST to list processes):");
            string request = Console.ReadLine();
            if (request == "LIST")
            {
                Console.Clear();
                processList();
                killProcess();
            }
            else
            {
                try
                {
                    Console.Clear();
                    processRequest(request);
                    Console.WriteLine("\nSelect a process:");
                    string select = Console.ReadLine();
                    Process p = processSelect(select);

                    Console.Clear();
                    p.Kill();
                    Console.WriteLine("Process Killed");
                }
                catch (System.FormatException ex)
                {
                    Console.WriteLine("{ERROR} Enter a valid process");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{ERROR} Process can't be killed");
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("Escape to quit or press any key to continue");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
                Console.Clear();
            }
        }

        //Check process info
        private static void checkProcess()
        {
            Console.WriteLine("Enter a process name (LIST to list processes):");
            string request = Console.ReadLine();
            if (request == "LIST")
            {
                Console.Clear();
                processList();
                checkProcess();
            }
            else
            {
                Console.Clear();
                processRequest(request);
                Console.WriteLine("\nSelect a process:");
                string select = Console.ReadLine();


                try
                {
                    Process p = processSelect(select);

                    Console.Clear();
                    Console.WriteLine("Process Started at : " + p.StartTime);
                    Console.WriteLine("Session ID: " + p.SessionId);
                    Console.WriteLine("Is it responding?: " + p.Responding);
                    Console.WriteLine("Process ID: " + p.Id);
                }
                catch (System.FormatException ex)
                {
                    Console.WriteLine("{ERROR} Enter a valid process");
                    Console.WriteLine(ex.Message);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    Console.WriteLine("{ERROR} You can't access this process");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{ERROR} A problem occured");
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("Escape to quit or press any key to continue");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
                Console.Clear();
            }
        }

        //List all the processes running
        private static void processList()
        {
            Array processes = Process.GetProcesses().ToArray<Process>();

            foreach (Process x in processes)
            {
                Console.WriteLine(x.ProcessName);
            }
            Console.WriteLine("");
        }

        //List processes matching that name
        private static void processRequest(string processName)
        {
            results = Process.GetProcessesByName(processName);

            int x = 0;
            while (results.GetLength(0) > x)
            {
                Console.WriteLine(x + ". " + results[x].ProcessName);
                x++;
            }
        }

        //Select the specific process to interact with
        private static Process processSelect(string select)
        {
            int x = Int16.Parse(select);

            Process selected = results[x];
            return selected;

        }
    }
}
