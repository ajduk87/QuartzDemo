using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzDemo.Jobs;

namespace QuartzDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IScheduler _scheduler;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        [HttpPost(Name = "StartSimpleJob")]
        public async Task<IActionResult> StartSimpleJob()
        {
            //trigger our simple job
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                .WithIdentity("simplejob", "quartzexample")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("testtrigger", "quartzexample")
                .StartNow()
                .WithSimpleSchedule(t => t.WithIntervalInSeconds(5).WithRepeatCount(5))
                .Build();

            await _scheduler.ScheduleJob(job, trigger);

            return RedirectToAction("Index");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
