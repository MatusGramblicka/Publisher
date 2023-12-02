namespace Publisher.Interface;

public interface ICacheProvider
{
    /// <summary>
    ///     Gets the object from cache
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    object? GetValue(object key);

    /// <summary>
    ///     Sets the object/value to the cache
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="cacheEntry">The cache entry.</param>
    void SetValue(object key, object cacheEntry);

    /// <summary>
    ///     Remove the value from the cache
    /// </summary>
    /// <param name="key"></param>
    void RemoveValue(object key);
}