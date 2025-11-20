namespace SparkUpSolution.Middlewares
{
    public interface ICurrentOperatorAccessor
    {
        public string Id { get; }
        public string Name { get; }
    }
}
