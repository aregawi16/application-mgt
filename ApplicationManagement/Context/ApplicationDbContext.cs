using ApplicationManagement.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;

using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public DbSet<ApplicationProgram> Programs { get; set; }
     public DbSet<Candidate> CandidateAnswers { get; set; }

    private readonly string _endpointUri;
    private readonly string _primaryKey;
    private readonly string _databaseName;
    private readonly string _containerName;


    public ApplicationDbContext(string endpointUri, string primaryKey, string databaseName, string containerName)
    {
        _endpointUri = endpointUri;
        _primaryKey = primaryKey;
        _databaseName = databaseName;
        _containerName = containerName;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseCosmos(
                _endpointUri,
                _primaryKey,
                _databaseName,
                options =>
                {
                    options.ConnectionMode(ConnectionMode.Gateway);
                });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure any additional model mappings or relationships here
    }
}