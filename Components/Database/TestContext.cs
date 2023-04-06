using Microsoft.EntityFrameworkCore;

namespace concurrency.Components.Database;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options) : base(options)
    {
    }

    public DbSet<TestState> TestStates => Set<TestState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TestState>();

        entity.HasKey(x => x.CorrelationId);
        entity
            .Property(x => x.CorrelationId)
            .ValueGeneratedNever();

        entity
            .Property(x => x.RowVersion)
            .IsRowVersion();

    }
}
