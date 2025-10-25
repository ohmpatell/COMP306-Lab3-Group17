using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using PodcastManagementSystem.Models;

namespace PodcastManagementSystem.Services
{
    public class CommentService : ICommentService
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName;

        public CommentService(IAmazonDynamoDB dynamoDb, IConfiguration configuration)
        {
            _dynamoDb = dynamoDb;
            _tableName = configuration["AWS:DynamoDB:CommentsTableName"];
        }

        public async Task<List<Comment>> GetCommentsByEpisodeAsync(string episodeId)
        {
            var request = new QueryRequest
            {
                TableName = _tableName,
                KeyConditionExpression = "EpisodeID = :episodeId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":episodeId", new AttributeValue { S = episodeId } }
                }
            };

            var response = await _dynamoDb.QueryAsync(request);
            var comments = new List<Comment>();

            foreach (var item in response.Items)
            {
                comments.Add(new Comment
                {
                    EpisodeID = item["EpisodeID"].S,
                    CommentID = item["CommentID"].S,
                    PodcastID = item.ContainsKey("PodcastID") ? item["PodcastID"].S : "",
                    UserID = item.ContainsKey("UserID") ? item["UserID"].S : "",
                    Username = item.ContainsKey("Username") ? item["Username"].S : "",
                    Text = item.ContainsKey("Text") ? item["Text"].S : "",
                    Timestamp = item.ContainsKey("Timestamp") ? item["Timestamp"].S : ""
                });
            }

            return comments;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            comment.CommentID = Guid.NewGuid().ToString();
            comment.Timestamp = DateTime.UtcNow.ToString("o");

            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "EpisodeID", new AttributeValue { S = comment.EpisodeID } },
                    { "CommentID", new AttributeValue { S = comment.CommentID } },
                    { "PodcastID", new AttributeValue { S = comment.PodcastID } },
                    { "UserID", new AttributeValue { S = comment.UserID } },
                    { "Username", new AttributeValue { S = comment.Username } },
                    { "Text", new AttributeValue { S = comment.Text } },
                    { "Timestamp", new AttributeValue { S = comment.Timestamp } }
                }
            };

            await _dynamoDb.PutItemAsync(request);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "EpisodeID", new AttributeValue { S = comment.EpisodeID } },
                    { "CommentID", new AttributeValue { S = comment.CommentID } },
                    { "PodcastID", new AttributeValue { S = comment.PodcastID } },
                    { "UserID", new AttributeValue { S = comment.UserID } },
                    { "Username", new AttributeValue { S = comment.Username } },
                    { "Text", new AttributeValue { S = comment.Text } },
                    { "Timestamp", new AttributeValue { S = comment.Timestamp } }
                }
            };

            await _dynamoDb.PutItemAsync(request);
        }

        public async Task<Comment> GetCommentAsync(string episodeId, string commentId)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "EpisodeID", new AttributeValue { S = episodeId } },
                    { "CommentID", new AttributeValue { S = commentId } }
                }
            };

            var response = await _dynamoDb.GetItemAsync(request);

            if (response.Item == null || response.Item.Count == 0)
                return null;

            var item = response.Item;
            return new Comment
            {
                EpisodeID = item["EpisodeID"].S,
                CommentID = item["CommentID"].S,
                PodcastID = item.ContainsKey("PodcastID") ? item["PodcastID"].S : "",
                UserID = item.ContainsKey("UserID") ? item["UserID"].S : "",
                Username = item.ContainsKey("Username") ? item["Username"].S : "",
                Text = item.ContainsKey("Text") ? item["Text"].S : "",
                Timestamp = item.ContainsKey("Timestamp") ? item["Timestamp"].S : ""
            };
        }
    }
}