// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $(".select2").select2({
        placeholder: "Select an option",
        allowClear: true,
        width: '100%',
    });
    
    // initialize datatable 
    $('table[data-column]').each(function () {
        let table = $(this);
        let columns = table.data('column').split(',');
        let url = table.data('url');
        const dataTable = table.DataTable({
            processing: true,
            serverSide: true,
            responsive: true,
            ajax: {
                url: url,
                type: 'POST',
                dataSrc: 'data.$values',
                data: function (d) {
                    
                    // Include filters in the AJAX request
                    d.filters = {};
                    // Iterate over columns and get filter values
                    columns.forEach(function (col) {
                        let filterInput = $('#filter-' + col);
                        if (filterInput.length) {
                            d.filters[col] = filterInput.val();
                        }
                    });
                    //console.log(d)
                    return d;
                    
                }, // Pass filters as data to your server
            },
            columns: columns.map(function (col) {
                return {
                    data: col, name: col, autoWidth: true,  // Render each column
                }
            }),
            order: [[0, 'desc']]
        });

        columns.forEach(function (col) {
            $('#filter-' + col).on('change', function () {
                dataTable.draw();
            });
        });
    });
});

function confirmDelete(url, id) {
    let btn = $(this);
    let dataTable = btn.closest('table').DataTable();
    $.confirm({
        title: 'Confirm!',
        content: 'Are you sure you want to delete this record?',
        theme: 'supervan',
        animation: 'zoom',
        closeIcon: true,
        animationSpeed: 500,
        backgroundDismiss: true,
        backgroundDismissAnimation: 'glow',
        escapeKey: true,
        rtl: false,
        buttons: {
            confirm: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: { id: id },
                        success: function (result) {
                            if (result.success) {
                                window.location.reload()
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                        console.error("AJAX Error:", textStatus, errorThrown);
                        console.log(jqXHR.responseText); // Log the response text for more details
                    }
                    
                    });
                    
                }
            },
            cancel: {
                text: 'Cancel',
                btnClass: 'btn-blue',
                action: function () {
                    
                }
            }
        }
    })
}

