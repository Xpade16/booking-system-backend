using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using BookingSystem.Application.Services.Interfaces;

namespace BookingSystem.Infrastructure.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> logger)
    {
        _redis = redis;
        _db = redis.GetDatabase();
        _logger = logger;
    }

    private string GetSlotKey(int classScheduleId) => $"class:{classScheduleId}:slots";

    public async Task<bool> TryDecrementSlotAsync(int classScheduleId)
    {
        var key = GetSlotKey(classScheduleId);
        
        try
        {
            // Lua script for atomic decrement with check
            var script = @"
                local current = redis.call('GET', KEYS[1])
                if current == false then
                    return -1
                end
                current = tonumber(current)
                if current > 0 then
                    redis.call('DECR', KEYS[1])
                    return 1
                else
                    return 0
                end
            ";
            
            var result = (int)await _db.ScriptEvaluateAsync(
                script, 
                new RedisKey[] { key });
            
            if (result == 1)
            {
                _logger.LogInformation("✅ Slot decremented for class {ClassId}", classScheduleId);
                return true;
            }
            else if (result == 0)
            {
                _logger.LogWarning("⚠️ No slots available for class {ClassId}", classScheduleId);
                return false;
            }
            else
            {
                _logger.LogWarning("⚠️ Slot key not found for class {ClassId}", classScheduleId);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Redis error during slot decrement for class {ClassId}", classScheduleId);
            throw;
        }
    }

    public async Task IncrementSlotAsync(int classScheduleId)
    {
        var key = GetSlotKey(classScheduleId);
        
        try
        {
            await _db.StringIncrementAsync(key);
            _logger.LogInformation("✅ Slot incremented for class {ClassId}", classScheduleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Redis error during slot increment for class {ClassId}", classScheduleId);
            throw;
        }
    }

    public async Task<int> GetAvailableSlotsAsync(int classScheduleId)
    {
        var key = GetSlotKey(classScheduleId);
        
        try
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? (int)value : -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Redis error getting slots for class {ClassId}", classScheduleId);
            return -1;
        }
    }

    public async Task SetAvailableSlotsAsync(int classScheduleId, int slots)
    {
        var key = GetSlotKey(classScheduleId);
        
        try
        {
            await _db.StringSetAsync(key, slots);
            _logger.LogInformation("✅ Slots set to {Slots} for class {ClassId}", slots, classScheduleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Redis error setting slots for class {ClassId}", classScheduleId);
            throw;
        }
    }

    public async Task DeleteSlotKeyAsync(int classScheduleId)
    {
        var key = GetSlotKey(classScheduleId);
        
        try
        {
            await _db.KeyDeleteAsync(key);
            _logger.LogInformation("✅ Slot key deleted for class {ClassId}", classScheduleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Redis error deleting slot key for class {ClassId}", classScheduleId);
        }
    }

    public async Task<bool> IsConnectedAsync()
    {
        try
        {
            await _db.PingAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}