namespace ApplicationManagement.Services
{
    using Microsoft.Azure.Cosmos;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using ApplicationManagement.Models;

    public class CosmosDbService
    {
        private CosmosClient _cosmosClient;
        private Container _programContainer;
        private Container _candidateAnswerContainer;

        public CosmosDbService(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDb:EndpointUri"];
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var primaryKey = configuration["CosmosDb:Key"];
            var cosmosClientOptions = new CosmosClientOptions
            {
                ApplicationName = databaseName,
                ConnectionMode = ConnectionMode.Gateway,

                //ServerCertificateCustomValidationCallback = (request, certificate, chain) =>
                //{
                //    // Always return true to ignore certificate validation errors
                //    return true; //not for production
                //}
            };
            _cosmosClient = new CosmosClient(connectionString,primaryKey, cosmosClientOptions);
            var database = _cosmosClient.GetDatabase(databaseName);
            _programContainer = database.GetContainer("ApplicationProgram");
            _candidateAnswerContainer = database.GetContainer("CandidateAnswer");
        }

        public async Task AddProgramAsync(ApplicationProgram program)
        {
            await _programContainer.CreateItemAsync(program, new PartitionKey(program.id));
        }
        public async Task<IEnumerable<ApplicationProgram>> GetAllProgramsAsync()
        {
            var query = "SELECT * FROM c";
            var iterator = _programContainer.GetItemQueryIterator<ApplicationProgram>(query);
            var programs = new List<ApplicationProgram>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                programs.AddRange(response.ToList());
            }
            return programs;
        }

        public async Task<ApplicationProgram> GetProgramAsync(string id)
        {
            try
            {
                var response = await _programContainer.ReadItemAsync<ApplicationProgram>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateProgramAsync(string id, ApplicationProgram program)
        {
            await _programContainer.UpsertItemAsync(program, new PartitionKey(id));
        }

        public async Task DeleteProgramAsync(string id)
        {
            await _programContainer.DeleteItemAsync<ApplicationProgram>(id, new PartitionKey(id));
        }




        public async Task<IEnumerable<Candidate>> GetAllCandidateAnswersAsync()
        {
            var query = "SELECT * FROM c";
            var iterator = _programContainer.GetItemQueryIterator<Candidate>(query);
            var programs = new List<Candidate>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                programs.AddRange(response.ToList());
            }
            return programs;
        }

        public async Task AddCandidateAnswerAsync(Candidate candidateAnswer)
        {
            ApplicationProgram program = await GetProgramAsync(candidateAnswer.ProgramId);

            if (program == null)
            {
                // Handle the case where the ApplicationProgram does not exist
                throw new InvalidOperationException("The specified ApplicationProgram does not exist.");
            }
            await _candidateAnswerContainer.CreateItemAsync(candidateAnswer, new PartitionKey(candidateAnswer.id));
        }
        public async Task<Candidate> GetCandidateAnswerAsync(string id)
        {
            try
            {
                var response = await _programContainer.ReadItemAsync<Candidate>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        public async Task<List<Candidate>> GetCandidatesByProgramAsync(string applicationProgramId)
        {
            try
            {
                // Query to get all candidates with the specified ApplicationProgramId
                var query = new QueryDefinition("SELECT * FROM c WHERE c.ApplicationProgramId = @applicationProgramId")
                    .WithParameter("@applicationProgramId", applicationProgramId);

                var iterator = _programContainer.GetItemQueryIterator<Candidate>(query);

                List<Candidate> candidates = new List<Candidate>();
                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    candidates.AddRange(response.Resource);
                }
                return candidates;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Handle not found case, possibly log this depending on your needs
                return null;
            }
        }

        public async Task UpdateCandidateAnswerAsync(string id, Candidate candidate)
        {
            await _programContainer.UpsertItemAsync(candidate, new PartitionKey(id));
        }

        public async Task DeleteCandidateAnswerAsync(string id)
        {
            await _programContainer.DeleteItemAsync<Candidate>(id, new PartitionKey(id));
        }




    }

}
