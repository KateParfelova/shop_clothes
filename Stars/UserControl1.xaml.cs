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

namespace Stars
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(double), typeof(RatingStars),
                new PropertyMetadata(0.0, OnRatingChanged));

        public static readonly DependencyProperty MaxRatingProperty =
            DependencyProperty.Register("MaxRating", typeof(int), typeof(RatingStars),
                new PropertyMetadata(5));

        public static readonly DependencyProperty StarSizeProperty =
            DependencyProperty.Register("StarSize", typeof(double), typeof(RatingStars),
                new PropertyMetadata(20.0));

        public static readonly DependencyProperty FilledStarColorProperty =
            DependencyProperty.Register("FilledStarColor", typeof(Brush), typeof(RatingStars),
                new PropertyMetadata(Brushes.Gold));

        public static readonly DependencyProperty EmptyStarColorProperty =
            DependencyProperty.Register("EmptyStarColor", typeof(Brush), typeof(RatingStars),
                new PropertyMetadata(Brushes.LightGray));

        public static readonly DependencyProperty ShowHalfStarsProperty =
            DependencyProperty.Register("ShowHalfStars", typeof(bool), typeof(RatingStars),
                new PropertyMetadata(true));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RatingStars),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ShowRatingValueProperty =
            DependencyProperty.Register("ShowRatingValue", typeof(bool), typeof(RatingStars),
                new PropertyMetadata(false));

        public static readonly DependencyProperty RatingValueFormatProperty =
            DependencyProperty.Register("RatingValueFormat", typeof(string), typeof(RatingStars),
                new PropertyMetadata("{0:F1}"));

        #endregion

        #region Properties

        public double Rating
        {
            get => (double)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public int MaxRating
        {
            get => (int)GetValue(MaxRatingProperty);
            set => SetValue(MaxRatingProperty, value);
        }

        public double StarSize
        {
            get => (double)GetValue(StarSizeProperty);
            set => SetValue(StarSizeProperty, value);
        }

        public Brush FilledStarColor
        {
            get => (Brush)GetValue(FilledStarColorProperty);
            set => SetValue(FilledStarColorProperty, value);
        }

        public Brush EmptyStarColor
        {
            get => (Brush)GetValue(EmptyStarColorProperty);
            set => SetValue(EmptyStarColorProperty, value);
        }

        public bool ShowHalfStars
        {
            get => (bool)GetValue(ShowHalfStarsProperty);
            set => SetValue(ShowHalfStarsProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public bool ShowRatingValue
        {
            get => (bool)GetValue(ShowRatingValueProperty);
            set => SetValue(ShowRatingValueProperty, value);
        }

        public string RatingValueFormat
        {
            get => (string)GetValue(RatingValueFormatProperty);
            set => SetValue(RatingValueFormatProperty, value);
        }

        #endregion

        #region Events

        public static readonly RoutedEvent RatingChangedEvent =
            EventManager.RegisterRoutedEvent("RatingChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<double>), typeof(RatingStars));

        public event RoutedPropertyChangedEventHandler<double> RatingChanged
        {
            add => AddHandler(RatingChangedEvent, value);
            remove => RemoveHandler(RatingChangedEvent, value);
        }

        private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (RatingStars)d;
            var oldValue = (double)e.OldValue;
            var newValue = (double)e.NewValue;

            control.OnRatingChanged(oldValue, newValue);
        }

        protected virtual void OnRatingChanged(double oldValue, double newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue)
            {
                RoutedEvent = RatingChangedEvent
            };
            RaiseEvent(args);
        }

        #endregion
    }
}
