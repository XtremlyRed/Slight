using System.Threading.Tasks;
using System.Windows.Input;

namespace Slight.Core
{
    /// <summary>
    ///  default implementation  <see cref="RelayCommand{TParameter}"/>
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface ICommand<TParameter> : ICommand
    {
        bool CanExecute(TParameter parameter);
        void Execute(TParameter parameter);
    }

    /// <summary>
    ///  default implementation  <see cref="RelayCommandAsync"/>
    /// </summary>
    public interface ICommandAsync : ICommand
    {
        bool CanExecute();
        Task ExecuteAsync();
    }

    /// <summary>
    ///  default implementation  <see cref="RelayCommandAsync{TParameter}"/>
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface ICommandAsync<TParameter> : ICommand
    {
        bool CanExecute(TParameter parameter);
        Task ExecuteAsync(TParameter parameter);
    }
}