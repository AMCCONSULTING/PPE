$(document).ready(function () {
    $(".select2").select2({
        placeholder: "Select an option",
        allowClear: true,
        width: '100%',
    });
    
    $('.select2-tags').select2({
        tags: true,
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
                type: 'GET',
                dataSrc: 'data.$values',
                data: function (d) {
                    //console.log(d)
                    d.filters = {};
                    $('[data-filter]').each(function () {
                        let filterInput = $(this);
                        let filterType = filterInput.data('filter');
                        d.filters[filterType] = filterInput.val();
                    });
                    return d;
                },
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
        
        $('[data-filter]').on('change', function () {
            dataTable.draw();
        });
    });
    
    // employee select change event
    $('.employee-select').on('change', function () {
        const id = $(this).val();
        $.ajax({
            url: '/api/employees/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                const htmlDocument =
                    `<ul class="list-group">
                                    <li class="list-group-item">
                                        <strong>Full Name: </strong>
                                        <span>${data.fullName}</span>
                                    </li> 
                                    <li class="list-group-item">
                                        <strong>Phone Number</strong>
                                        <span>${data.phone}</span>
                                    </li>
                                    <li class="list-group-item">
                                        <strong>Project | Function</strong>
                                        <span>${data.project.title} | ${data.function.title}</span>
                                    </li>
                                    <li class="list-group-item">
                                        <strong>Size | Shoe Size</strong>
                                        <span>${data.size} | ${data.shoeSize}</span>    
                                    </li>
                            </ul>`;
                $('.placeholder').html(htmlDocument);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })
    
    // getPPesByCategoryId
    $('.category-select-in-stock').on('change', function () {
        const id = $(this).val();
        $.ajax({
            url: '/api/getPpes/InStock/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                console.log(data)
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.title}</option>`;
                });
                $('.ppe-select-in-stock').html(htmlDocument);
                $('.value-select-in-stock').html('<option></option>');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })
   
    // api/categoryAttributeValue/{id}
    $('.ppe-select-in-stock').on('change', function () {
        const id = $(this).val();
        $.ajax({
            url: '/api/categoryAttributeValue/inStock/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.title} (${item.currentStock})
                    </option>`;
                });
                $('.value-select-in-stock').html(htmlDocument);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })
    
    // select value change event append to .value-list-container
    $('.value-select-in-stock').on('change', function () {
        const id = $(this).val();
        const ppeTitle = $('.ppe-select-in-stock option:selected').text();
        if ($(`.value-list-container li[data-id="${id}"]`).length > 0) {
            SweetAlert.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Value already exist! Please select another value. ' +
                    'Or modify the existing quantity. Thank you!',
            })
            return;
        }
        // show submit-btn
        $('.submit-btn').removeClass('d-none')
        const text = $(this).find('option:selected').text();
        const htmlDocument = `<li data-id="${id}" class="list-group-item">
                        <div class="row align-items-center">
                            <span class="col">${ppeTitle} - ${text}</span> 
                            <input type="number" class="form-control col qte-input" name="quantities[]">
                            <input type="hidden" value="${id}" name="articles[]">
                            <button type="button" class="btn btn-danger col-1 mx-1" onclick="removeItem(this)">X</button>
                           </div>
                        </li>`;
        $('.value-list-container').append(htmlDocument);
        $('.value-select').val('');
    })

    $('.category-select-in-stock-project').on('change', function () {
        const id = $(this).val();
        const projectID = $('#projectId').val();
        //alert(projectID)
        $.ajax({
            url: '/api/getPpes/inStock/project/' + id + '/' + projectID,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                console.log(data)
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.title}</option>`;
                });
                $('.ppe-select-in-stock-project').html(htmlDocument);
                $('.value-select-in-stock-project').html('<option></option>');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })

    // api/categoryAttributeValue/{id}
    $('.ppe-select-in-stock-project').on('change', function () {
        const id = $(this).val();
        const projectID = $('#projectId').val();
        $.ajax({
            url: '/api/categoryAttributeValue/inStock/project/' + id + '/' + projectID,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.title} (${item.currentStock})
                    </option>`;
                });
                $('.value-select-in-stock-project').html(htmlDocument);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })

    // select value change event append to .value-list-container
    $('.value-select-in-stock-project').on('change', function () {
        const id = $(this).val();
        const projectID = $('#projectId').val();
        const ppeTitle = $('.ppe-select-in-stock-project option:selected').text();
        if ($(`.value-list-container-dotation li[data-id="${id}"]`).length > 0) {
            SweetAlert.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Value already exist! Please select another value. ' +
                    'Or modify the existing quantity. Thank you!',
            })
            return;
        }
        // show submit-btn
        $('.submit-btn').removeClass('d-none')
        const text = $(this).find('option:selected').text();
        const htmlDocument = `<li data-id="${id}" class="list-group-item">
                        <div class="row align-items-center">
                            <span class="col">${ppeTitle} - ${text}</span> 
                            <input type="number" class="form-control col qte-input-employee" name="quantities[]">
                            <input type="hidden" value="${id}" name="articles[]">
                            <button type="button" class="btn btn-danger col-1 mx-1" onclick="removeItem(this)">X</button>
                           </div>
                        </li>`;
        $('.value-list-container-dotation').append(htmlDocument);
        $('.value-select-in-stock-project').val('');
    })

    // getPPesByCategoryId
    $('.category-select').on('change', function () {
        const id = $(this).val();
        $.ajax({
            url: '/api/getPpes/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                console.log(data)
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.title}</option>`;
                });
                $('.ppe-select').html(htmlDocument);
                $('.value-select').html('<option></option>');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })

    // api/categoryAttributeValue/{id}
    $('.ppe-select').on('change', function () {
        const id = $(this).val();
        $.ajax({
            url: '/api/categoryAttributeValue/' + id,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                let htmlDocument = '<option></option>';
                data.forEach(function (item) {
                    htmlDocument += `<option value="${item.id}">${item.attributeValueAttributeCategory.attributeValue.value.text}</option>`;
                });
                $('.value-select').html(htmlDocument);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.log(jqXHR.responseText); // Log the response text for more details
            }
        });
    })

    // select value change event append to .value-list-container
    $('.value-select').on('change', function () {
        const id = $(this).val();
        const ppeTitle = $('.ppe-select option:selected').text();
        if ($(`.value-list-container li[data-id="${id}"]`).length > 0) {
            SweetAlert.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Value already exist! Please select another value. ' +
                    'Or modify the existing quantity. Thank you!',
            })
            return;
        }
        // show submit-btn
        $('.submit-btn').removeClass('d-none')
        const text = $(this).find('option:selected').text();
        const htmlDocument = `<li data-id="${id}" class="list-group-item">
                    <div class="row align-items-center">
                        <span class="col">${ppeTitle} - ${text}</span> 
                        <input type="number" class="form-control col" name="quantities[]">
                        <input type="hidden" value="${id}" name="articles[]">
                        <button type="button" class="btn btn-danger col-1 mx-1" onclick="removeItem(this)">X</button>
                    </div>
                </li>`;
        $('.value-list-container').append(htmlDocument);
        $('.value-select').val('');
    })
    
    $('.value-list-container-dotation').on('change', '.qte-input-employee', function () {
        //const ppeQuantity = $(this).closest('li').find('input[type="hidden"]').val();
        const qte = $(this).val();
        /*alert(JSON.stringify({
            qte: qte,
            //ppeQuantity: ppeQuantity
        }))*/
        if (qte > 2) {
            SweetAlert.fire({
                type: 'error',
                title: 'Oops...',
                text: 'You can not give more than 2 item of a ppe!',
            })
            $(this).val(2);
        }
    })
});

function removeItem(btn) {
    $(btn).closest('li').remove();
}

const Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    /*didOpen: (toast) => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
    }*/
});
function confirmDelete(url, id) {
    let btn = $(this);
    let dataTable = btn.closest('table').DataTable();
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'question',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.value) {
            console.log({
                id: id,
                _token: $('input[name="_token"]').val(),
                url: url
            })
            $.ajax({
                url: url + '/' + id,
                type: 'GET',
                success: function (result) {
                    if (result.success) {
                        Toast.fire({
                            type: 'success',
                            title: 'Deleted!',
                            text: 'Your file has been deleted.',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        $('table[data-column]').each(function () {
                            let table = $(this);
                            table.DataTable().ajax.reload();
                        })
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Toast.fire({
                        type: 'error',
                        title: 'Error deleting data',
                        text: 'Something went wrong!',
                    })
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.log(jqXHR.responseText); // Log the response text for more details
                }
            });
        }
    })
}

function submitBtn() {
    const btn = this.event.target;
    btn.disabled = true;
    btn.value = "Please wait...";
    btn.classList.remove('btn-primary');
    btn.classList.add('btn-secondary');
    if (btn.disabled) {
        btn.form.submit();
    }
}

function returnPpe(data) {
    //alert(JSON.stringify(data))
    let alertMessage = ''
    const btn = this.event.target
    switch (data.type) {
        case 1: 
            alertMessage = 'Are you sure you want to return this ppe'
            break
        case 2: 
            alertMessage = 'Are you sure you want to change this ppe'
            break
        case 3: 
            alertMessage = 'Are you sure you want to reassign this ppe'
            break
        default: 
            alertMessage = ''
            break
    }
    Swal.fire({
        text: alertMessage,
        type: 'question',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    })
        .then(r => {
            appendAndSubmitForm({
                type: data.type,
                articleId: data.articleId,
                employeeId: data.employeeId
            });
        })
        .catch(e => e)
}

function appendAndSubmitForm(inputs) {
    // Iterate over the keys in the inputs object
    for (const key in inputs) {
        if (inputs.hasOwnProperty(key)) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = key;
            input.value = inputs[key];

            document.getElementById("returnPpeForm").appendChild(input);
        }
    }
    
    document.getElementById("returnPpeForm").submit();
}

function removeItemFromStock(btn, id, dotationId) {
    $.ajax({
        url: `/api/remove/article/${id}/dotation/${dotationId}`,
        type: 'DELETE',
        dataType: 'json',
        success: function (data) {
            console.log(data)
            Toast.fire({
                type: 'success',
                title: 'Deleted!',
                text: data.message,
                showConfirmButton: false,
                timer: 3000
            })
            $(btn).closest('li[data-id="' + id + '"]').remove();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Toast.fire({
                type: 'error',
                title: 'Error deleting data',
                text: 'Something went wrong!',
            })
            console.error("AJAX Error:", textStatus, errorThrown);
            console.log(jqXHR.responseText); // Log the response text for more details
        }
    });
}

