using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistrationButtonControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
            // Привязка команд внутренней кнопки к свойствам UserControl
            regButton.SetBinding(Button.CommandProperty, new Binding("Command") { Source = this });
            regButton.SetBinding(Button.CommandParameterProperty, new Binding("CommandParameter") { Source = this });
        }
        #region Dependency Properties

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(UserControl2),
                new PropertyMetadata("Регистрация", OnButtonTextChanged));

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(UserControl2),
                new PropertyMetadata(105.0, OnButtonSizeChanged));

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(double), typeof(UserControl2),
                new PropertyMetadata(35.0, OnButtonSizeChanged));

        public static readonly DependencyProperty ButtonStyleProperty =
    DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(UserControl2),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsRender,
            OnButtonStyleChanged));
        // Свойство Command
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(UserControl2),
                new PropertyMetadata(null, OnCommandChanged));

        // Свойство CommandParameter
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(UserControl2),
                new PropertyMetadata(null, OnCommandParameterChanged));

        #endregion

        #region Properties

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public double ButtonWidth
        {
            get => (double)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public double ButtonHeight
        {
            get => (double)GetValue(ButtonHeightProperty);
            set => SetValue(ButtonHeightProperty, value);
        }

        // Свойство Command
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Свойство CommandParameter
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        public Style ButtonStyle
        {
            get => (Style)GetValue(ButtonStyleProperty);
            set => SetValue(ButtonStyleProperty, value);
        }
        #endregion

        #region Property Changed Handlers

        private static void OnButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl2 control && control.regButton != null)
            {
                control.regButton.Content = e.NewValue;
            }
        }
        private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl2 control && control.regButton != null)
            {
                control.regButton.Style = e.NewValue as Style;
            }
        }
        private static void OnButtonSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl2 control && control.regButton != null)
            {
                control.regButton.Width = control.ButtonWidth;
                control.regButton.Height = control.ButtonHeight;
            }
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl2 control && control.regButton != null)
            {
                control.regButton.Command = e.NewValue as ICommand;
            }
        }

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl2 control && control.regButton != null)
            {
                control.regButton.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region Events

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserControl2));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Пробрасываем событие клика
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }

        #endregion
    }
}