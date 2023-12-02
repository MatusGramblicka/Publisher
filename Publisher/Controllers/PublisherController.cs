using AutoMapper;
using Contracts;
using Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Publisher.ActionFilters;
using Publisher.Interface;

namespace Publisher.Controllers;

[Route("api/publisher")]
[ApiController]
public class PublisherController : Controller
{
    private readonly ILogger<PublisherController> _logger;
    private readonly IComputer _computer;
    private readonly IMapper _mapper;

    public PublisherController(ILogger<PublisherController> logger, IComputer computer, IMapper mapper)
    {
        _logger = logger;
        _computer = computer;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("calculation/{key}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<ActionResult<ComputedOutputDto>> Calculate([FromRoute] int key,
        [FromBody] CalculationInputDto calculationInputDto)
    {
        var calculationInput = _mapper.Map<CalculationInput>(calculationInputDto);

        _logger.LogInformation($"Request received with key {key}");

        var computedOutputDto = await _computer.Compute(key, calculationInput.Input);

        return Ok(computedOutputDto);
    }
}