using Bogus;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;
using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task Seed(this AppDbContext appDbContext)
        {
            if (appDbContext.Players.Any())
            {
                return;
            }

            // Seed players and bonuses during local development
            var playerFaker = new Faker<Player>()
                .CustomInstantiator(f => new Player 
                { 
                    Id = Guid.NewGuid(),
                    Username = f.Internet.UserName() 
                });

            var bonusFaker = new Faker<Bonus>()
                .CustomInstantiator(f => new Bonus
                {
                    Id = Guid.NewGuid(),
                    Status = EnumExtensions.RandomEnumValue<BonusStatus>(),
                    Type = EnumExtensions.RandomEnumValue<BonusType>(),
                    Amount = f.Random.Int(10, 1000)
                });

            var players = playerFaker.Generate(3);
            var bonuses = new List<Bonus>();

            foreach ( var player in players) 
            {
                var givenBonuses = 0;

                while(givenBonuses != 3)
                {
                    var bonus = bonusFaker.Generate();

                    if(player.Bonuses.Any(b => b.Type == bonus.Type && b.IsActive))
                    {
                        continue;
                    }

                    bonus.PlayerId = player.Id;
                    bonuses.Add(bonus);
                    givenBonuses++;
                }
               
            };

            await appDbContext.Players.AddRangeAsync(players);
            await appDbContext.Bonuses.AddRangeAsync(bonuses);
            await appDbContext.SaveChangesAsync();
        }
    }
}
