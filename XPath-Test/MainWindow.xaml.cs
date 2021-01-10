using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MVVM;

namespace XPath_Test {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			DataContext = new ViewModel(this);
		}
	}

	class ViewModel : MVVM.ViewModelBase<MainWindow> {
		public ParameterlessDelegateCommand RunTest { get; }


		private string bs_XPathExpression;
		public string XPathExpression {
			get => bs_XPathExpression;
			set {
				if (value == bs_XPathExpression) { return; }

				bs_XPathExpression = value;

				OnPropertyChanged();
			}
		}

		private string bs_InputXml;
		public string InputXml {
			get => bs_InputXml;
			set {
				if (value == bs_InputXml) { return; }

				bs_InputXml = value;

				OnPropertyChanged();
			}
		}

		private string bs_OutputResult;
		public string OutputResult {
			get => bs_OutputResult;
			set {
				if (value == bs_OutputResult) { return; }

				bs_OutputResult = value;

				OnPropertyChanged();
			}
		}

		public ViewModel(MainWindow window) : base(window) {
			RunTest = new ParameterlessDelegateCommand(DoRunTest, CanRunTest);

			PropertyChanged += (s, ea) => {
				switch (ea.PropertyName) {
					case nameof(InputXml):
					case nameof(XPathExpression):
						RunTest.UpdateCanExecute();
						break;
				}
			};
		}

		void DoRunTest() {
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(InputXml);

			try {
				Regex rx = new Regex("(.*)/@([A-Za-z_][A-Za-z0-9_.-]*)");
				string xpath = XPathExpression;
				string attrib = null;

				var rxMatch = rx.Match(XPathExpression);

				if (rxMatch.Success) {
					xpath = rxMatch.Groups[1].Value;
					attrib = rxMatch.Groups[2].Value;
				}

				var nodes = doc.DocumentElement?.SelectNodes(xpath);
				if (nodes != null) {
					var sb = new StringBuilder();
					foreach (XmlNode node in nodes) {
						if (attrib == null) {
							sb.AppendLine(node.OuterXml);
						} else {
							if (node is XmlElement elem) {
								var attribValue = elem.Attributes[attrib];
								if (attribValue != null) {
									sb.AppendLine(attribValue.Value);
								}
							}
						}
					}

					OutputResult = sb.ToString();
				}
			} catch (Exception ex) {
				MessageBox.Show($"An error ocurred when running test: {ex.Message}");
			}
		}

		bool CanRunTest() => !string.IsNullOrWhiteSpace(bs_InputXml) && !string.IsNullOrWhiteSpace(bs_XPathExpression);
	}
}
