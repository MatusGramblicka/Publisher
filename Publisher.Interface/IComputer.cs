using Contracts;
using Contracts.Dto;

namespace Publisher.Interface;

public interface IComputer
{
    Task<ComputedOutputDto> Compute(int key, decimal input);
}