using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly IConsumer<Null, string> _consumer;

        public ConsumerController(IConsumer<Null, string> consumer)
        {
            _consumer = consumer;
            _consumer.Subscribe("test-topic");
        }

        [HttpGet("receive")]
        public IActionResult Receive()
        {
            var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
            if (consumeResult == null)
                return NotFound(new { Message = "No messages available" });

            return Ok(new
            {
                Status = "Received",
                Topic = consumeResult.Topic,
                Partition = consumeResult.Partition.Value,
                Offset = consumeResult.Offset.Value,
                SizeInKB = consumeResult.Message.Value.Length / 1024,
                Timestamp = consumeResult.Message.Timestamp.UtcDateTime
            });
        }
    }
}

//using Confluent.Kafka;
//using Microsoft.AspNetCore.Mvc;

//namespace WebApplication1.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ConsumerController : ControllerBase
//    {
//        private readonly IConsumer<Null, string> _consumer;

//        public ConsumerController(IConsumer<Null, string> consumer)
//        {
//            _consumer = consumer;
//        }

//        [HttpGet("receive")]
//        public IActionResult Receive()
//        {
//            _consumer.Subscribe("test-topic");

//            var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
//            if (consumeResult == null)
//                return NotFound("No messages available");

//            return Ok(new
//            {
//                Message = consumeResult.Message.Value,
//                SizeInKB = consumeResult.Message.Value.Length / 1024
//            });
//        }
//    }
//}
