namespace SparkUpSolution.Middlewares
{
    public class CurrentOperatorAccessor : ICurrentOperatorAccessor
    {
        private readonly IHttpContextAccessor context;

        public CurrentOperatorAccessor(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public string Id => context.HttpContext?.User?.FindFirst("id")?.Value ?? "unknown";

        public string Name => context.HttpContext?.User?.FindFirst("name")?.Value ?? "unknown";
    }
}
