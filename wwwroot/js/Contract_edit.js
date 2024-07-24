$(function () {
    $('#editButton').click(function (e) {
        e.preventDefault();
        var contract = {
            PartitionKey: $('#PartitionKey').val(),
            RowKey: $('#RowKey').val(),
            ContractName: $('#ContractName').val(),
            ClientName: $('#ClientName').val()
        };

        $.ajax({
            url: '/Contract/Edit',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(contract),
            success: function () {
                alert('Contract updated successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('Error updating contract: ' + textStatus + ' - ' + errorThrown);
                alert('Error updating contract.');
            }
        });
    });
});
