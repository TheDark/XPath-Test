using System;
using System.Windows.Input;

namespace MVVM {
	public class DelegateCommand : ICommand {
		private readonly Action<object> _commandAction;
		private readonly Func<object, bool> _canExecuteFunc;

		public DelegateCommand(Action<object> action, Func<object, bool> canExecute = null) {
			_commandAction = action;
			_canExecuteFunc = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => _canExecuteFunc?.Invoke(parameter) ?? true;

		public void Execute(object parameter) {
			if (CanExecute(parameter)) {
				_commandAction(parameter);
			}
		}

		public void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
	public class ParameterlessDelegateCommand : ICommand {
		private readonly Action _commandAction;
		private readonly Func<bool> _canExecuteFunc;

		public ParameterlessDelegateCommand(Action action, Func<bool> canExecute = null) {
			_commandAction = action;
			_canExecuteFunc = canExecute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => _canExecuteFunc?.Invoke() ?? true;

		public void Execute(object parameter) {
			if (CanExecute(parameter)) {
				_commandAction();
			}
		}

		public void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
