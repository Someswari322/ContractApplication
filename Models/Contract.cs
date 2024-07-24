
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos.Table;

namespace ContractApplication.Models
{

    public class Contract : TableEntity
    {

        //public string PartitionKey { get; set; }
        //public string RowKey { get; set; }
        //public DateTimeOffset? Timestamp { get; set; }
        ////public ETag ETag { get; set; }

        [Required]
        public string ContractName { get; set; }
        [Required]
        public string ClientName { get; set; }
        //[Required]
        //public string Age { get; set; }
        //[Required]
        //public string Address { get; set; }
    }
}
