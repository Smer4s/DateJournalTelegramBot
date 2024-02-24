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

		public async Task SaveCache(IEnumerable<UserCache> cache)
		{
			foreach (var item in cache)
			{
				if (await _collection.Find(x => x.UserName == item.UserName).FirstOrDefaultAsync() is null)
				{
					await _collection.InsertOneAsync(item);
				}
				else
				{
					await _collection.FindOneAndReplaceAsync(x => x.UserName == item.UserName, item);
				}
			}
		}

		public async Task GetCache(IEnumerable<UserCache> userCaches)
		{
			foreach (var cache in userCaches)
			{
				var dbCache = await _collection.Find(x => x.UserName == cache.UserName).FirstAsync();
				cache.Id = dbCache.Id;
				cache.IsStory = dbCache.IsStory;
				cache.CurrentStoryIndex = dbCache.CurrentStoryIndex;
				cache.SessionId = dbCache.SessionId;
			}
		}
	}
}
