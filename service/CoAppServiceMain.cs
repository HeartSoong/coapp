﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2011 Garrett Serack. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace CoApp.Service {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Toolkit.Engine;
    using Toolkit.Exceptions;
    using Toolkit.Extensions;
    using Toolkit.Win32;

    internal class CoAppServiceMain {
        public static bool UseUserAccount;
        private bool _start;
        private bool _stop;
        private bool _install;
        private bool _status;
        private bool _interactive;
        private bool _uninstall;
        private string _username;
        private string _password;

        private const string help =
            @"
Usage:
-------

CoApp.Service [options] 
    
    Options:
    --------
    --help                      this help
    --load-config=<file>        loads configuration from <file>

    --start                     starts the service if not running 
                                (implies --install)

    --restart                   stops and starts the service 
                                (implies --install, --stop, --start)

    --stop                      stops the service if running

    --install                   installs the service 

    --status                    prints the status of the service

    --interactive               runs the CoApp Service as an interactive process.
                                (use ctrl-c to stop)

    --user=<userid>             sets the userid when installing the service
                                (defaults to localsystem)

    --password=<password>       sets the password of the account when 
                                installing the service

    --uninstall                 uninstalls the service
";

        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        private static int Main(string[] args) {
            try {
                return new CoAppServiceMain().main(args);
            } catch (ConsoleException failure) {
                return Fail("\r\n{0}\r\n", failure.Message);
            }
            catch (Exception failure) {
                return Fail("\r\n{0}\r\n{1}", failure.Message, failure.StackTrace);
            }
        }

        private static void RequiresAdmin(string operation) {
            if (!AdminPrivilege.IsRunAsAdmin) {
                throw new ConsoleException("The operation '{0}' requires administrator priviliges.", operation);
            }
        }

        private int main(IEnumerable<string> args) {
            try {
                var options = args.Switches();
                var parameters = args.Parameters();

                Console.CancelKeyPress += (x, y) => {
                    Console.WriteLine("Stopping CoAppService.");
                    EngineService.Stop();
                };

                #region Parse Options

                foreach (var arg in from arg in options.Keys select arg) {
                    var argumentParameters = options[arg];
                    switch (arg) {
                        case "load-config":
                            break;

                        case "auto-install":
                            RequiresAdmin("--auto-install");
                            Environment.Exit(CoAppService.AutoInstall());
                            break;

                        case "start":
                            _start = true;
                            _install = true;
                            break;

                        case "restart":
                            _stop = true;
                            _start = true;
                            _install = true;
                            break;

                        case "stop":
                            _stop = true;
                            break;

                        case "install":
                            _install = true;
                            break;

                        case "uninstall":
                            _stop = true;
                            _uninstall = true;
                            break;

                        case "username":
                            UseUserAccount = true;
                            _username = argumentParameters.LastOrDefault();
                            break;

                        case "password":
                            _password = argumentParameters.LastOrDefault();
                            break;

                        case "status":
                            _status = true;
                            break;

                        case "interactive":
                            _interactive = true;
                            break;

                        case "help":
                            return Help();

                        default:
                            Fail("Unrecognized switch [--{0}]", arg);
                            return Help();
                    }
                }

                #endregion

                Logo();

                if (_interactive) {
                    RequiresAdmin("--interactive");

                    if (CoAppService.IsRunning) {
                        throw new ConsoleException(
                            "The CoApp Service can not be running.\r\nYou must stop it with --stop before using the service interactively.");
                    }
                    Console.WriteLine("Launching CoApp Service interactively.\r\nUse ctrl-c to stop.");

                    var task = EngineService.Start(true);

                    Console.WriteLine("[CoApp Interactive -- Press escape to stop.]");

                    // wait for user to cancel task, or when it's actually closed
                    while (!task.Wait(1000)) {
                        Console.Write(".");
                        while (Console.KeyAvailable) {
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape) {
                                EngineService.Stop();
                            }
                        }
                    }
                    return 0;
                }

                if (_stop) {
                    RequiresAdmin("--stop");
                    Console.Write("Stopping service:");
                    CoAppService.StopService();

                    while (EngineServiceManager.IsServiceRunning) {
                        Console.Write(".");
                        Thread.Sleep(100);
                    }
                    Console.WriteLine(" [Stopped]");
                }

                if (_uninstall) {
                    RequiresAdmin("--uninstall");
                    CoAppService.Uninstall();
                    return 0;
                }

                if (_install) {
                    RequiresAdmin("--install");
                    CoAppService.Install(_username, _password);
                }

                if (_start) {
                    RequiresAdmin("--start");

                    if (CoAppService.IsInstalled) {
                        Console.Write("Starting service:");
                        CoAppService.StartService();

                        while (!EngineServiceManager.IsServiceRunning) {
                            Console.Write(".");
                            Thread.Sleep(100);
                        }
                        Console.WriteLine(" [Started]");
                    } else {
                        throw new ConsoleException("CoApp.Service is not installed.");
                    }
                }

                if (!options.Any() && CoAppService.IsInstalled && parameters.FirstOrDefault() == null) {
                    // this lets us run the service 
                    CoAppService.RunThisProcessAsService();
                    return 0;
                }

                if (_status) {
                    Console.WriteLine("Service installed: {0}", CoAppService.IsInstalled);
                    Console.WriteLine("Service running: {0}", CoAppService.IsRunning);
                    return 0;
                }

                if (!options.Any()) {
                    throw new ConsoleException("Missing CoApp.Service command. Use --help for information");
                }

             
            } catch(ConsoleException e) {
                return Fail(e.Message);
            }
             catch(Exception ex) {
                 return Fail("{0}\r\n{1}",ex.Message,ex.StackTrace);
             }
            return 0;
        }

        #region fail/help/logo

        public static int Fail(string text, params object[] par) {
            Logo();
            Console.WriteLine("Error:{0}", text.format(par));
            return 1;
        }

        private static int Help() {
            Logo();
            help.Print();
            return 0;
        }

        private static void Logo() {
        }

        #endregion
    }
}