using BDAS2_Restaurace.Controller;
using BDAS2_Restaurace.Model;
using BDAS2_Restaurace.ViewModel;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

namespace BDAS2_Restaurace.Views
{
	/// <summary>
	/// Interakční logika pro TableView.xaml
	/// </summary>
	public partial class TableView : UserControl
	{

		private TableViewModel viewModel = new TableViewModel(typeof(Customer));
		public TableView()
		{
			InitializeComponent();
			CreateNavigation();

			var model = viewModel.Data;
			CreateForm(model);
			
		}

		private void CreateNavigation()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();

			var classTypes = assembly.GetTypes().Where(type => String.Equals(type.Namespace, "BDAS2_Restaurace.Model") && type.IsClass && !type.IsAbstract);
			foreach (var classType in classTypes)
			{
				var btn = new Button();
				btn.Content = classType.Name;
				btn.Click += (sender, e) =>
				{
					viewModel = new TableViewModel(classType);
					CreateForm(viewModel.Data);
				};
				Navigation.Children.Add(btn);
			}


		}

		private void CreateForm(object data)
		{
			Form.Children.Clear();
			var type = data.GetType();
			var properties = type.GetProperties();

			foreach (var property in properties)
			{
				if (!property.CanWrite)
				{
					Form.Children.Add(CreateLabel(property));
					continue;
				}

				if (property.PropertyType == typeof(string) || property.PropertyType.IsPrimitive)
					Form.Children.Add(CreateInput(property));
				else if (property.PropertyType == typeof(DateTime))
					Form.Children.Add(CreateDatePicker(property));
				else if (property.PropertyType == typeof(BitmapImage))
					Form.Children.Add(CreateFilePicker(property));
				else
					Form.Children.Add(CreateComboBox(property));

			}
		}

		private StackPanel CreateLabel(PropertyInfo property)
		{
			var stackPanel = new StackPanel();
			var label = new Label();
			var value = new Label();

			label.Content = property.Name + " " + property.PropertyType;

			var binding = new Binding(property.Name);
			value.SetBinding(Label.ContentProperty, binding);

			stackPanel.Children.Add(label);
			stackPanel.Children.Add(value);

			return stackPanel;
		}

		private StackPanel CreateFilePicker(PropertyInfo property)
		{
			var stackPanel = new StackPanel();
			var label = new Label();
			var input = new Button();

			label.Content = property.Name + " " + property.PropertyType;

			input.Content = "Vybrat soubor";
			input.Click += (sender, e) =>
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.ShowDialog();
			};

			stackPanel.Children.Add(label);
			stackPanel.Children.Add(input);

			return stackPanel;
		}

		private StackPanel CreateDatePicker(PropertyInfo property)
		{
			var stackPanel = new StackPanel();
			var label = new Label();
			var input = new DatePicker();

			label.Content = property.Name + " " + property.PropertyType;

			stackPanel.Children.Add(label);
			stackPanel.Children.Add(input);

			return stackPanel;
		}

		private StackPanel CreateComboBox(PropertyInfo property)
		{
			var stackPanel = new StackPanel();
			var label = new Label();
			var input = new ComboBox();

			label.Content = property.Name + " " + property.PropertyType.Name;

			stackPanel.Children.Add(label);
			stackPanel.Children.Add(input);

			return stackPanel;
		}

		private StackPanel CreateInput(PropertyInfo property)
		{
			var stackPanel = new StackPanel();
			var label = new Label();
			var input = new TextBox();

			var binding = new Binding(property.Name);
			// binding.Source = viewModel;
			input.SetBinding(TextBox.TextProperty, binding);

			label.Content = property.Name + " " + property.PropertyType;

			stackPanel.Children.Add(label);
			stackPanel.Children.Add(input);

			return stackPanel;
		}
	}
}
