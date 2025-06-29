using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiziVote.Converters
{
    public class RatingToBrushMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int rating && values[1] is int threshold)
            {
                return rating >= threshold ? new SolidColorBrush(Colors.Gold) : new SolidColorBrush(Colors.Gray);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
