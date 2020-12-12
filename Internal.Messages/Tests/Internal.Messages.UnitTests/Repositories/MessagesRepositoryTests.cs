using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internal.Messages.Core.Models.Domain;
using Internal.Messages.Core.Models.ResourceParameters;
using Internal.Messages.Repository.Data;
using Internal.Messages.Repository.Repositories;
using Internal.Messages.TestUtilities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Internal.Messages.UnitTests.Repositories
{
    [TestFixture]
    public class MessagesRepositoryTests
    {
        public static IEnumerable<TestCaseData> NotFoundSearchTestCases
        {
            get
            {
                yield return new TestCaseData(new MessagesResourceParameters
                {
                    ChannelId = 3,
                    SearchQuery = null
                });
                yield return new TestCaseData(new MessagesResourceParameters
                {
                    ChannelId = 1,
                    SearchQuery = "Three"
                });
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetMessagesAsync_NullParameters_ReturnsArgumentNullException()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<InternalMessagesContext>();

            using (var context = new InternalMessagesContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                // Assert
                Assert.ThrowsAsync<ArgumentNullException>(async () => await messagesRepository.GetMessagesAsync(null));
            }
        }

        [Test]
        public async Task GetMessagesAsync_AllPropertiesNull_ReturnsAllMessages()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<InternalMessagesContext>();

            var user = new User()
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            var messageOne = new Message()
            {
                Text = "Test Message One",
                ChannelId = 1,
            };
            var messageTwo = new Message()
            {
                Text = "Test Message Two",
                ChannelId = 1,
            };

            using (var context = new InternalMessagesContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the messages
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                messageOne.UserId = user.Id;
                messageTwo.UserId = user.Id;

                // Add the messages
                await messagesRepository.CreateAsync(messageOne);
                await messagesRepository.CreateAsync(messageTwo);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var parameters = new MessagesResourceParameters { ChannelId = 0, SearchQuery = null };

                // Get messages with null properties for parameters
                var results = await messagesRepository.GetMessagesAsync(parameters);

                // Assert
                Assert.AreEqual(results.Count(), 2);
                Assert.IsTrue(results.FirstOrDefault(x => x.Text == messageOne.Text) != null);
                Assert.IsTrue(results.FirstOrDefault(x => x.Text == messageTwo.Text) != null);
            }
        }

        [Test]
        public async Task GetMessagesAsync_ChannelId1_ReturnsMessage()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<InternalMessagesContext>();

            var user = new User()
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            var messageOne = new Message()
            {
                Text = "Test Message One",
                ChannelId = 1,
            };
            var messageTwo = new Message()
            {
                Text = "Test Message Two",
                ChannelId = 2,
            };

            using (var context = new InternalMessagesContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the messages
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                messageOne.UserId = user.Id;
                messageTwo.UserId = user.Id;

                // Add the messages
                await messagesRepository.CreateAsync(messageOne);
                await messagesRepository.CreateAsync(messageTwo);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var parameters = new MessagesResourceParameters { ChannelId = 1, SearchQuery = null };

                // Get messages with null properties for parameters
                var results = await messagesRepository.GetMessagesAsync(parameters);

                // Assert
                Assert.AreEqual(results.Count(), 1);
                Assert.IsTrue(results.FirstOrDefault(x => x.Text == messageOne.Text) != null);
            }
        }

        [Test]
        public async Task GetMessagesAsync_SearchQueryTwo_ReturnsMessage()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<InternalMessagesContext>();

            var user = new User()
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            var messageOne = new Message()
            {
                Text = "Test Message One",
                ChannelId = 1,
            };
            var messageTwo = new Message()
            {
                Text = "Test Message Two",
                ChannelId = 2,
            };

            using (var context = new InternalMessagesContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the messages
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                messageOne.UserId = user.Id;
                messageTwo.UserId = user.Id;

                // Add the messages
                await messagesRepository.CreateAsync(messageOne);
                await messagesRepository.CreateAsync(messageTwo);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var parameters = new MessagesResourceParameters { ChannelId = 0, SearchQuery = "Two" };

                // Get messages with null properties for parameters
                var results = await messagesRepository.GetMessagesAsync(parameters);

                // Assert
                Assert.AreEqual(results.Count(), 1);
                Assert.IsTrue(results.FirstOrDefault(x => x.Text == messageTwo.Text) != null);
            }
        }

        [TestCaseSource(nameof(NotFoundSearchTestCases))]
        public async Task GetMessagesAsync_ChannelId3_ReturnsEmptyList(MessagesResourceParameters messagesResourceParameters)
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<InternalMessagesContext>();

            var user = new User()
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };

            var messageOne = new Message()
            {
                Text = "Test Message One",
                ChannelId = 1,
            };
            var messageTwo = new Message()
            {
                Text = "Test Message Two",
                ChannelId = 2,
            };

            using (var context = new InternalMessagesContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the messages
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                messageOne.UserId = user.Id;
                messageTwo.UserId = user.Id;

                // Add the messages
                await messagesRepository.CreateAsync(messageOne);
                await messagesRepository.CreateAsync(messageTwo);
            }

            using (var context = new InternalMessagesContext(options))
            {
                var messagesRepository = new MessagesRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var results = await messagesRepository.GetMessagesAsync(messagesResourceParameters);

                // Assert
                Assert.AreEqual(results.Count(), 0);
            }
        }
    }
}
