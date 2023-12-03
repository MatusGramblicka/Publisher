using Contracts.Dto;

namespace Publisher.Interface;

public interface IDataProcessor
{
    Task<ComputedOutputDto> ProcessData(int key, decimal input);
}