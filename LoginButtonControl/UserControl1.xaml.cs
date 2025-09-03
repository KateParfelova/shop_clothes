using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LoginButtonControl
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            // Привязка команд внутренней кнопки к свойствам UserControl
            loginButton.SetBinding(Button.CommandProperty, new Binding("Command") { Source = this });
            loginButton.SetBinding(Button.CommandParameterProperty, new Binding("CommandParameter") { Source = this });
        }

        #region Dependency Properties

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(UserControl1),
                new PropertyMetadata("Войти", OnButtonTextChanged));

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(UserControl1),
                new PropertyMetadata(105.0, OnButtonSizeChanged));

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(double), typeof(UserControl1),
                new PropertyMetadata(35.0, OnButtonSizeChanged));

        public static readonly DependencyProperty ButtonStyleProperty =
    DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(UserControl1),
        new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsRender,
            OnButtonStyleChanged));
        // Свойство Command
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(UserControl1),
                new PropertyMetadata(null, OnCommandChanged));

        // Свойство CommandParameter
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(UserControl1),
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
            if (d is UserControl1 control && control.loginButton != null)
            {
                control.loginButton.Content = e.NewValue;
            }
        }
        private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl1 control && control.loginButton != null)
            {
                control.loginButton.Style = e.NewValue as Style;
            }
        }
        private static void OnButtonSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl1 control && control.loginButton != null)
            {
                control.loginButton.Width = control.ButtonWidth;
                control.loginButton.Height = control.ButtonHeight;
            }
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl1 control && control.loginButton != null)
            {
                control.loginButton.Command = e.NewValue as ICommand;
            }
        }

        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserControl1 control && control.loginButton != null)
            {
                control.loginButton.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region Events

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UserControl1));

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