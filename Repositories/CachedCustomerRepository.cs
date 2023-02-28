using Microsoft.Extensions.Caching.Memory;

using FastDeliveryApi.Entity;
using FastDeliveryApi.Repositories.Interfaces;

namespace FastDeliveryApi.Repositories;

public class CachedCustomerRepository : ICustomerRepository
{
    private readonly ICustomerRepository _decorated;
    private readonly IMemoryCache _memoryCache;
    public CachedCustomerRepository(ICustomerRepository decorated, IMemoryCache memoryCache)
    {
        _decorated = decorated;
        _memoryCache = memoryCache;
    }
    
    public void Add(Customer customer)
    {
        string key = "GetAllCustomers";
        _memoryCache.Remove(key);
        _decorated.Add(customer);
    }

    public void Delete(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Customer>> GetAll()
    {
        string key = "GetAllCustomers";
        return await _memoryCache.GetOrCreateAsync(
            key, 
            entry => {
                return _decorated.GetAll();
            });
    }

    public Task<Customer?> GetCustomerById(int id, CancellationToken cancellationToken = default)
    {
        string key = $"customer-{id}";
        return _memoryCache.GetOrCreateAsync(
            key,
            entry => {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return _decorated.GetCustomerById(id, cancellationToken);
            });
    }

    public void Update(Customer customer) =>
        _decorated.Update(customer);
}