$(function () {
  var
    _packageService = abp.services.app.package,
    l = abp.localization.getSource('Individual'),
    _$table = $('#orders-table');

  debugger;
  const packageDeliveryStatusDescriptions = {
    "0": { text: l('MissingInfo'), color: 'danger' },
    "1": { text: l('Initiate'), color: 'secondary' },
    "2": { text: l('InTransit'), color: 'info' },
    "3": { text: l('WaitingForShipping'), color: 'warning' },
    "4": { text: l('Shipping'), color: 'primary' },
    "5": { text: l('InWarehouseVN'), color: 'info' },
    "6": { text: l('WaitingForDelivery'), color: 'warning' },
    "7": { text: l('DeliveryRequest'), color: 'info' },
    "8": { text: l('DeliveryInProgress'), color: 'info' },
    "9": { text: l('Delivered'), color: 'success' },
    "10": { text: l('Completed'), color: 'success' },
    "11": { text: l('WaitingForReturn'), color: 'warning' },
    "13": { text: l('WarehouseTransfer'), color: 'info' },
  };
  var _$packageTable = _$table.DataTable({
    paging: true,
    serverSide: true,
    sortable: false,
    listAction: {
      ajaxFunction: _packageService.getPagedByCustomer
    },
    buttons: [
      {
        name: 'refresh',
        text: '<i class="fas fa-redo-alt"></i>',
        action: () => _$packageTable.draw(false)
      }
    ],
    language: {
      "info": l('Display') + " _START_ - _END_ | " + l('Total') + " _TOTAL_ " + l('Record'),
      "lengthMenu": l('Display') + " _MENU_ " + l('Record'),
      "emptyTable": l('EmptyTable'),
      "zeroRecords": l('ZeroRecords')
    },
    responsive: {
      details: {
        type: 'column'
      }
    },
    columnDefs: [
      {
        targets: 0,
        width: 35,
        sortable: false,
        className: 'text-center',
        render: function (data, type, row, meta) {
          return meta.row + meta.settings._iDisplayStart + 1;
        }
      },

      {
        targets: 1,
        data: 'waybillNumber',
        sortable: false

      },
      {
        targets: 2,
        data: 'packageNumber',
        sortable: false

      },
      {
        targets: 3,
        sortable: false,
        data: 'status',
        render: function (data, type, row, meta) {
          var status = packageDeliveryStatusDescriptions[row.shippingStatus];
          if (status) {
            return '<span class="badge badge-' + status.color + '">' + status.text + '</span>';
          }
          return '';
        }
      },

      {
        targets: 4,
        width: 90,
        sortable: false,
        className: 'text-right',
        data: 'warehouseName',
      }
    ]
  });
});
