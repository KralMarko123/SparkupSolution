namespace SparkUpSolution.Extensions
{
    public static class EnumExtensions
    {
        public static T RandomEnumValue<T>()
        {
            var random = new Random();
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(random.Next(values.Length));
        }
    }
}
