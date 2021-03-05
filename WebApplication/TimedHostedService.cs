using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;

namespace WebApplication
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly ExceptionService _exceptionService;
        private Timer _timer;
        private readonly IServiceScopeFactory scopeFactory;
        public TimedHostedService(ILogger<TimedHostedService> logger,IServiceScopeFactory scopeFactory, ExceptionService exceptionService)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
            _exceptionService = exceptionService;
        }
        
    
        public  Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            
                _timer = new Timer(DoWork, null, TimeSpan.Zero,
                    TimeSpan.FromSeconds(5));
            
            
            
    
            return Task.CompletedTask;
        }
    
        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<OrderContext>();
                _logger.LogInformation(
                    "Timed Hosted Service is working. Count: {Count} {time}", count,DateTime.Now);
                Order order = _context.Orders.FirstOrDefault(t =>  t.converted_order ==null);
                if (order != null)
                {
                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(order.source_order);
                    List<Product> converted = new List<Product>();
                    try
                    {
                        Assembly asm = Assembly.LoadFrom("Executer.dll");
                        Type t = asm.GetType("Executer." + order.system_type, true, true);
                        object obj = Activator.CreateInstance(t);
                        MethodInfo method = t.GetMethod("Execute");
                        foreach (var tmp in products)
                        {
                            converted.Add((Product) method.Invoke(obj, new object?[] {tmp}));
                        }
                    }
                    catch (Exception e)
                    {
                        _exceptionService.Error(e).ConfigureAwait(true);
                    }
                    _logger.LogInformation(order.Id + "converted");
                    order.converted_order = JsonSerializer.Serialize(converted);
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                }
                    


            }
            
            
        }
    
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
    
            _timer?.Change(Timeout.Infinite, 0);
    
            return Task.CompletedTask;
        }
    
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    
}