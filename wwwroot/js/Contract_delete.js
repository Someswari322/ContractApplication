$(document).ready(function () {
    $('#deleteButton').click(function (e) {
        e.preventDefault();
        var contract = {
            PartitionKey: $('#PartitionKey').val(),
            RowKey: $('#RowKey').val()
        };

        $.ajax({
            url: '/Contract/Delete',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(contract),
            success: function () {
                alert('Contract deleted successfully.');
                window.location.href = '/Contract/Index';
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('Error deleting contract: ' + textStatus + ' - ' + errorThrown);
                alert('Error deleting contract.');
            }
        });
    });
});