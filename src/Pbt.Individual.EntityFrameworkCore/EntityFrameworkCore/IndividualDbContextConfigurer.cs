using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Pbt.Individual.EntityFrameworkCore;

public static class IndividualDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<IndividualDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<IndividualDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
