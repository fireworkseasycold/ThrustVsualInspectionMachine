using NLog;
using SqlSugar;
using ThrustVsualInspectionMachineProject.Models;

namespace ThrustVsualInspectionMachineProject.Services;

public class SampleRepository : ISampleRepository
{
    private readonly SqlSugarClient _db;
    private readonly ILogger _logger;

    public SampleRepository(SqlSugarClient db, ILogger logger)
    {
        _db = db;
        _logger = logger;
        try
        {
            // 自动初始化表结构
            _db.CodeFirst.InitTables(typeof(SampleEntity));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Init table failed");
        }
    }

    public async Task<int> AddAsync(SampleEntity entity, CancellationToken ct = default)
    {
        try
        {
            return await _db.Insertable(entity).ExecuteReturnIdentityAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Insert SampleEntity failed");
            throw;
        }
    }

    public async Task<SampleEntity?> GetAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _db.Queryable<SampleEntity>().InSingleAsync(id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Query SampleEntity failed");
            throw;
        }
    }

    public async Task<List<SampleEntity>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            return await _db.Queryable<SampleEntity>().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Query all SampleEntity failed");
            throw;
        }
    }
}