using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractApplication.Models;

namespace ContractApplication.Services
{
    public class ContractServices
    {

        private readonly TableClient _tableClient;

        public ContractServices(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorageConnectionString");
            _tableClient = new TableClient(connectionString, "cruddemo");
            _tableClient.CreateIfNotExists();
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync(string query = null)
        {
            var entities = new List<Contract>();
            await foreach (var page in _tableClient.QueryAsync<Contract>(filter: query).AsPages())
            {
                entities.AddRange(page.Values);
            }
            return entities;
        }

        public async Task<Contract> GetContractAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<Contract>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task AddContractAsync(Contract contract)
        {
            await _tableClient.AddEntityAsync(contract);
        }

        public async Task UpdateContractAsync(Contract contract)
        {
            await _tableClient.UpdateEntityAsync(contract, contract.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteContractAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
