// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('.dataTable').each(function () {
        $(this).DataTable({
            responsive: true,
            lengthChange: true,
            autoWidth: false,
            dom:
                '<"row"<"col-6 p-0 d-flex"f><"col-6 d-flex justify-content-end"B>>' +
                '<"row"<"col-12"rt>>' +
                '<"row"<"col-6 d-flex align-items-center"l><"col-6 d-flex justify-content-end align-items-center"<"align-items-center mb-3"i><"ml-3"p>>>',
            buttons: ["copy", "csv", "excel", "pdf", "print", "colvis"],
            language: {
                lengthMenu: '_MENU_ <span class="ml-2">entries</span>',
                search: ""
            },
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
            initComplete: function () {
                var $searchInput = $(this).closest('.dataTables_wrapper').find('.dataTables_filter input');

                // Custom placeholder only for #tableUsers
                if ($(this).attr('id') === 'tableUsers') {
                    $searchInput.attr('placeholder', 'Search for users...');
                }

                var $wrapper = $('#tableUsers_filter');
                if (!$wrapper.find('.icon').length) {
                    $wrapper.append('<i class="fa fa-search icon"></i>');
                }
 
            }
        });
    });
});



$('.selectpicker').selectpicker('refresh');