using Quartz;

namespace QuartzDemo.Jobs
{
    public class SimpleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var message = $"Simple executed at ${DateTime.UtcNow.ToString()}";
            Console.WriteLine(message);
        }
    }
}
