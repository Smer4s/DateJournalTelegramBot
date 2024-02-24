using DateJournal.Infrastructure.Models;
using MongoDB.Driver;

namespace DateJournal.Infrastructure.Repositories
{
	public class StoryRepository
	{
		private readonly IMongoCollection<Story> _collection;

		public static StoryRepository Create()
		{
			var client = new MongoClient("mongodb://localhost:27017");
			var database = client.GetDatabase("storyDB");
			return new StoryRepository(database.GetCollection<Story>("stories"));
		}

		private StoryRepository(IMongoCollection<Story> collection) =>
			_collection = collection;

		//public async Task<Story?> GetStory(DateOnly date) =>
		//	await _collection.Find(story => story.Created == date).FirstOrDefaultAsync();

		public async Task Create(Story story)
			=> await _collection.InsertOneAsync(story);

		public async Task<List<Story>> GetStories() =>
			await _collection.Find(_ => true).ToListAsync();
	}
}
