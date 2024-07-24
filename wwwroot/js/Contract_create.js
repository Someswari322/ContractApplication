// wwwroot/js/contract.js

$(function () {
    debugger;
    $('#createButton').click(function (e) {
        e.preventDefault();

        var contractName = $('#ContractName').val();
        var clientName = $('#ClientName').val();

        if (!contractName || !clientName) {
            alert('Please fill in all fields.');
            return;
        }

        var contract = {
            PartitionKey: "Contracts",
            RowKey: new Date().getTime().toString(),
            ContractName: contractName,
            ClientName: clientName
        };

        $.ajax({
            url: '/Contract/Create',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(contract),
            success: function () {
                alert('Contract created successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function () {
                alert('Error creating contract.');
            }
        });
    });
});
