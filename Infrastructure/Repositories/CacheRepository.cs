using DateJournal.Infrastructure.Models;
using MongoDB.Driver;

namespace DateJournal.Infrastructure.Repositories
{
	public class CacheRepository
	{
		private readonly IMongoCollection<UserCache> _collection;

		public static CacheRepository Create()
		{
			var client = new MongoClient("mongodb://localhost:27017");
			var database = client.GetDatabase("storyDB");
			return new CacheRepository(database.GetCollection<UserCache>("cache"));
		}

		private CacheRepository(IMongoCollection<UserCache> collection) => _collection = collection;

		public async Task<Dictionary<string, UserCache>> GetCache() =>
			(await _collection.Find(_ => true).ToListAsync()).ToDictionary(item => item.UserName);

		public async Task ChangeCache(string user, UserCache cache) =>
			await _collection.FindOneAndReplaceAsync(cache => cache.UserName == user, cache);
	}
}
