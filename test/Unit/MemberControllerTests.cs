using LoyaltySystem.Controllers;
using LoyaltySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestProject1
{

    public class MemberControllerTests
    {
        private readonly DbContextOptions<Context> _dbContextOptions;

        private readonly Member _member1 = new Member
        {
            Name = "John Doe",
            PurchasesTotalAmount = 100
        };

        private readonly Member _member2 = new Member
        {
            Name = "Jane Smith",
            PurchasesTotalAmount = 25
        };

        public MemberControllerTests()
        {
            // We set up the in-memory database context for the test.
            _dbContextOptions = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;
        }

        [Fact]
        public async Task GetAllMembers_ReturnsAllMembers()
        {
            //  Create a new in-memory database context instance for the test
            using (var context = new Context(_dbContextOptions))
            {
                // Añadimos los Members creados previamente al contexto de base de datos en memoria
                context.Database.EnsureDeleted();
                context.Member.AddRange(_member1, _member2);
                await context.SaveChangesAsync();

                // Add the previously created Members to the in-memory database context
                var controller = new MemberController(context);

                // Execute the GetAllMembers method and save the result.
                var result = await controller.GetAllMembers();

                // Check that the result is an ActionResult object containing a collection of Members
                Assert.IsType<ActionResult<IEnumerable<Member>>>(result);
                var members = Assert.IsAssignableFrom<IEnumerable<Member>>(result.Value);

                // Check that the collection contains the Members we have previously added.
                Assert.Equal(2, members.Count());
            }
        }

        [Fact]
        public async Task PutMember_ReturnsNoContentResult()
        {
            // create a new Member
            var newMember = new Member
            {
                Name = "John Doe",
                PurchasesTotalAmount = 100
            };

            // Create a new in-memory database context instance for the test
            using (var context = new Context(_dbContextOptions))
            {
                // Add the Member to the in-memory database context
                context.Member.Add(newMember);
                await context.SaveChangesAsync();

                // We create a new instance of the Member driver and pass it the in-memory database context
                var controller = new MemberController(context);

                // Create a Member instance with the updated data
                var updatedMember = new Member
                {
                    Id = newMember.Id,
                    Name = "Jane Doe",
                };

                // Execute the PutMember method and save the result
                var result = await controller.PutMember(newMember.Id, updatedMember);

                // We check that the result is a NoContentResult object.
                Assert.IsType<NoContentResult>(result);

                // We get the updated Member from the in-memory database and check that the data is correct.
                var member = await context.Member.FindAsync(newMember.Id);
                Assert.Equal(updatedMember.Name, member!.Name);
            }
        }
    }
}