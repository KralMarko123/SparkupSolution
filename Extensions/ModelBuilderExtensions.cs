using Bogus;
using Microsoft.EntityFrameworkCore;
using SparkUpSolution.Domain.Entities;
using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Seed players and bonuses during local development
            var playerFaker = new Faker<Player>()
                .CustomInstantiator(f => new Player { Id = Guid.NewGuid(), Username = f.Internet.UserName() });

            var bonusFaker = new Faker<Bonus>()
                .CustomInstantiator(f => new Bonus
                {
                    Id = Guid.NewGuid(),
                    Status = EnumExtensions.RandomEnumValue<BonusStatus>(),
                    Type = EnumExtensions.RandomEnumValue<BonusType>(),
                    Amount = f.Random.Int(10, 1000)
                });

            var players = playerFaker.Generate(5);
            var bonuses = new List<Bonus>();

            players.ForEach(player =>
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
               
            });

            modelBuilder.Entity<Player>().HasData(players);
            modelBuilder.Entity<Bonus>().HasData(bonuses);
        }
    }
}
