namespace Contracts.Configuration;

/// <summary>
///     Configuration for caching.
/// </summary>
public class CachingConfiguration
{
    /// <summary>
    ///     Sets expiration of cache values in hours.
    /// </summary>
    public int CacheExpirationInHours { get; set; }
}