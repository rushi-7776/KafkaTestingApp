using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly IProducer<Null, string> _producer;

        public ProducerController(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        [RequestSizeLimit(104857600)]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] object data)
        {
            string message = data.ToString();

            var result = await _producer.ProduceAsync("test-topic", new Message<Null, string> { Value = message });

            return Ok(new
            {
                Status = "Sent",
                Topic = result.Topic,
                Partition = result.Partition.Value,
                Offset = result.Offset.Value,
                SizeInKB = message.Length / 1024
            });
        }
    }
}
