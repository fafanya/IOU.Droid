using System.Globalization;

namespace IOU.Droid
{
    public static class Convertors
    {
        public static double StringToDouble(string input)
        {
            input = input.Replace(',', '.');

            double output;
            double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out output);
            return output;
        }

        public static int StringToInt(string input)
        {
            input = input.Replace(',', '.');

            int output;
            int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out output);
            return output;
        }

        public static string DoubleToString(double input)
        {
            string output = input.ToString("0.00", CultureInfo.InvariantCulture);
            return output;
        }
    }
}