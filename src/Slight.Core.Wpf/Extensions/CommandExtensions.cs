using System.Windows;
using System.Windows.Input;

namespace Slight.Core
{
    public static class CommandExtensions
    {
        public static void TryExecute(this ICommand command, object commandParameter = null, IInputElement inputElement = null)
        {
            if (command == null)
            {
                return;
            }

            if (command is RoutedCommand routedCommand)
            {
                if (routedCommand.CanExecute(commandParameter, inputElement))
                {
                    routedCommand.Execute(commandParameter, inputElement);
                }

                return;
            }

            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
    }
}
