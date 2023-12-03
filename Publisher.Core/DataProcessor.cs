using AutoMapper;
using Contracts;
using Contracts.Configuration;
using Contracts.Dto;
using Messaging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Publisher.Interface;

namespace Publisher.Core;

public class DataProcessor : CacheProvider, IDataProcessor
{
    private readonly ILogger<DataProcessor> _logger;
    private readonly IMessageProducer _messageProducer;
    private readonly ITimeHelper _timeHelper;
    private readonly IMapper _mapper;

    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    private const double StorageValueConst = 2;
    private const int MaxAgeSeconds = 15;

    private double? _computedValue;
    private double? _previousValue;

    public DataProcessor(IMemoryCache memoryCache,
        ILogger<DataProcessor> logger, IOptions<CachingConfiguration> cachingConfiguration,
        IMessageProducer messageProducer, ITimeHelper timeHelper, IMapper mapper) : base(
        memoryCache, cachingConfiguration)
    {

        _logger = logger;
        _messageProducer = messageProducer;
        _timeHelper = timeHelper;
        _mapper = mapper;
    }

    public async Task<ComputedOutputDto> ProcessData(int key, decimal input)
    {
        _computedValue = null;
        _previousValue = null;

        var cachedValue = GetValue(key);

        if (cachedValue is ObjectForCache objectForCache)
        {
            _logger.LogInformation("Key/Value found in cache");
            var seconds = _timeHelper.GetSecondsDifferenceFromNow(objectForCache.DateCreated);
            _logger.LogDebug($"Key/Value pair is {seconds} seconds old");

            _previousValue = objectForCache.StorageValue;

            if (seconds > MaxAgeSeconds)
            {
                _logger.LogInformation($"Cached object is older than {MaxAgeSeconds}");
                await SetKeyValueToCache(key, StorageValueConst);
            }
            else
            {
                _computedValue = Math.Cbrt(Math.Log((double) input)) / _previousValue;
                await SetKeyValueToCache(key, _computedValue.Value);
            }
        }
        else
        {
            _logger.LogInformation("Key/Value was not found in cache");
            await SetKeyValueToCache(key, StorageValueConst);
        }

        _logger.LogDebug($"computedValue {_computedValue}, input {input}, previousValue {_previousValue}");

        var computedOutput = new ComputedOutput
        {
            ComputedValue = _computedValue,
            InputValue = input,
            PreviousValue = _previousValue
        };

        var computedOutputDto = _mapper.Map<ComputedOutputDto>(computedOutput);

        _messageProducer.SendMessage(computedOutputDto);
        
        return computedOutputDto;
    }

    private async Task SetKeyValueToCache(int key, double value)
    {
        try
        {
            await Semaphore.WaitAsync();
            SetValue(key, new ObjectForCache
            {
                StorageValue = value,
                DateCreated = _timeHelper.GetNow()
            });
        }
        finally
        {
            Semaphore.Release();
        }
    }
}