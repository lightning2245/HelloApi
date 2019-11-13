using System.Collections.Generic;
using Hello.Common;
using Hello.Common.Models;
using Hello.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HelloAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IActionResultTypeMapper _typeMapper;

        public MessageController(IMessageRepository messageRepository, IActionResultTypeMapper typeMapper)
        {
            _messageRepository = Validators.ThrowArgNullExcIfNull(messageRepository, nameof(messageRepository));
            _typeMapper = Validators.ThrowArgNullExcIfNull(typeMapper, nameof(typeMapper));
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return _typeMapper.Convert(_messageRepository.GetMessages(), typeof(IEnumerable<IMessage>));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return _typeMapper.Convert(_messageRepository.GetMessageById(id), typeof(IMessage));            
        }        
    }
}
