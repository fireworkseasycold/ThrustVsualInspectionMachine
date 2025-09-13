using SqlSugar;
using ThrustVsualInspectionMachineProject.Models;

namespace ThrustVsualInspectionMachineProject.Services;

public interface ISampleRepository
{
    Task<int> AddAsync(SampleEntity entity, CancellationToken ct = default);
    Task<SampleEntity?> GetAsync(int id, CancellationToken ct = default);
    Task<List<SampleEntity>> GetAllAsync(CancellationToken ct = default);
}