using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

namespace WpfApp.Commands
{
    
    // defines and contains static commands  
    
    public static class ActionCommands
    {
        // static routed commands (for menuing)

        public static RoutedUICommand LoginCommand { private set; get; }
        public static RoutedUICommand LogoutCommand { private set; get; }
        public static RoutedUICommand ExitCommand { private set; get; }

        public static RoutedUICommand AddCommand { private set; get; }
        public static RoutedUICommand EditCommand { private set; get; }
        public static RoutedUICommand DeleteCommand { private set; get; }
        public static RoutedUICommand ViewOrdersCommand { private set; get; }

        public static RoutedUICommand HowDoICommand { private set; get; }
        public static RoutedUICommand IndexCommand { private set; get; }
        public static RoutedUICommand AboutCommand { private set; get; }

        // creates several Routed UI commands with and without shortcut keys.
        
        static ActionCommands()
        {
            LoginCommand = MakeRoutedUiCommand("Login", Key.I, "Ctrl+I");
            LogoutCommand = MakeRoutedUiCommand("Logout", Key.O, "Ctrl+O");
            ExitCommand = MakeRoutedUiCommand("Exit");

            AddCommand = MakeRoutedUiCommand("Add", Key.A, "Ctrl+A");
            EditCommand = MakeRoutedUiCommand("Edit", Key.E, "Ctrl+E");
            DeleteCommand = MakeRoutedUiCommand("Delete", Key.Delete, "Del");

            ViewOrdersCommand = MakeRoutedUiCommand("View Orders");

            HowDoICommand = MakeRoutedUiCommand("How Do I", Key.H, "Ctrl+D");
            IndexCommand = MakeRoutedUiCommand("Index", Key.N, "Ctrl+N");
            AboutCommand = MakeRoutedUiCommand("About");
        }

        
        // creates a routed command instance without shortcut key

        private static RoutedUICommand MakeRoutedUiCommand(string name)
        {
            return new RoutedUICommand(name, name, typeof(ActionCommands));
        }

        
        // creates a routed command instance with a shortcut key

        private static RoutedUICommand MakeRoutedUiCommand(string name, Key key, string displayString)
        {
            var gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(key, ModifierKeys.Control, displayString));

            return new RoutedUICommand(name, name, typeof(ActionCommands), gestures);
        }
    }
}
