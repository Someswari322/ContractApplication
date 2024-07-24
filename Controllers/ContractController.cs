// Controllers/ContractController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContractApplication.Models;
using Azure.Data.Tables;
//using System.Diagnostics.Contracts;


public class ContractController : Controller
{
    private CloudTable GetCloudTable()
    {
        string storageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"; // Replace with your Azure Storage connection string
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        CloudTable table = tableClient.GetTableReference("Contractsalary"); // Replace with your table name
        table.CreateIfNotExistsAsync();
        return table;
    }

    public async Task<IActionResult> Index()
    {
        //CloudTable table = GetCloudTable();
        //TableQuery<Contract> query = new TableQuery<Contract>();
        //var contracts = await table.ExecuteQuerySegmentedAsync(
        //    query, null);
        //return View(contracts.Results);
        CloudTable table = GetCloudTable();
        TableQuery<Contract> query = new TableQuery<Contract>();
        List<Contract> contracts = new List<Contract>();
        TableContinuationToken token = null;
        do
        {
            TableQuerySegment<Contract> segment = await table.ExecuteQuerySegmentedAsync(query, token);
            token = segment.ContinuationToken;
            contracts.AddRange(segment.Results);
        } while (token != null);

        return View(contracts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Contract contract)
    {
        //CloudTable table = GetCloudTable();
        //TableOperation insertOperation = TableOperation.Insert(contract);
        //await table.ExecuteAsync(insertOperation);
        //return RedirectToAction("Index");
        try
        {
            CloudTable table = GetCloudTable();
            TableOperation insertOperation = TableOperation.Insert(contract);
            TableResult result = await table.ExecuteAsync(insertOperation);
            return Ok(); // Return success status
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating contract: {ex.Message}");
            return StatusCode(500, "Internal server error"); // Return error status
        }
    }

    public async Task<IActionResult> Edit(string partitionKey, string rowKey)
    {
        CloudTable table = GetCloudTable();
        TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
        TableResult result = await table.ExecuteAsync(retrieveOperation);
        Contract contract = result.Result as Contract;

        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody]Contract contract)
    {
        try
        {
            CloudTable table = GetCloudTable();

            // Retrieve the existing entity
            TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(contract.PartitionKey, contract.RowKey);
            TableResult retrieveResult = await table.ExecuteAsync(retrieveOperation);
            Contract existingContract = retrieveResult.Result as Contract;

            if (existingContract == null)
            {
                return NotFound();
            }

            // Set the ETag of the existing entity to the contract to be replaced
            contract.ETag = existingContract.ETag;

            // Replace the entity
            TableOperation replaceOperation = TableOperation.Replace(contract);
            await table.ExecuteAsync(replaceOperation);

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating contract: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
        //try
        //{
        //    CloudTable table = GetCloudTable();
        //    TableOperation replaceOperation = TableOperation.Replace(contract);
        //    await table.ExecuteAsync(replaceOperation);
        //    return Ok();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error updating contract: {ex.Message}");
        //    return StatusCode(500, "Internal server error");
        //}
    }
    public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    {
        CloudTable table = GetCloudTable();
        TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
        TableResult result = await table.ExecuteAsync(retrieveOperation);
        Contract contract = result.Result as Contract;

        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }
    // [HttpPost]
    //public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    //{
    //    try
    //    {
    //        CloudTable table = GetCloudTable();
    //        TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
    //        TableResult result = await table.ExecuteAsync(retrieveOperation);
    //        Contract exisContract = result.Result as Contract;

    //        if (exisContract == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(exisContract);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error deleting contract: {ex.Message}");
    //        return StatusCode(500, "Internal server error");
    //    }
    //}


    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed([FromBody] Contract contract)
    {
        try
        {
            CloudTable table = GetCloudTable();
            TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(contract.PartitionKey, contract.RowKey);
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            Contract delcontract = result.Result as Contract;

            if (delcontract == null)
            {
                return NotFound();
            }

            TableOperation deleteOperation = TableOperation.Delete(delcontract);
            await table.ExecuteAsync(deleteOperation);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting contract: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    public async Task<IActionResult> Details(string partitionKey, string rowKey)
    {
        CloudTable table = GetCloudTable();
        TableOperation retrieveOperation = TableOperation.Retrieve<Contract>(partitionKey, rowKey);
        TableResult result = await table.ExecuteAsync(retrieveOperation);
        Contract contract = result.Result as Contract;

        if (contract == null)
        {
            return NotFound();
        }

        return View(contract);
    }
}
