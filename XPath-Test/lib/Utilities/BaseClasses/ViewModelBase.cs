using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM {
	public abstract class ViewModelBase<TModel> : INotifyPropertyChanged {
		public TModel Model { get; }

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		#endregion

		protected ViewModelBase(TModel model) => Model = model;
	}
}
