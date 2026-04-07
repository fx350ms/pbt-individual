(function ($) {
    var _orderService = abp.services.app.order,
        l = abp.localization.getSource('Individual'),
        _$waybillModal = $('#create-waybill-modal'),
        _$waybillCreateForm = _$waybillModal.find('form');

    _$waybillCreateForm.on('submit', (e) => {
        debugger;
        e.preventDefault();
        var form = $(e.currentTarget);
        if (!form.valid()) {
            return;
        }
        var dto = form.serializeFormToObject();
        abp.ui.setBusy(_$waybillModal);
        _orderService.createWaybills(dto).done(() => {
            _$waybillModal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            _$waybillCreateForm[0].reset();
        }).always(() => {
            abp.ui.clearBusy(_$waybillModal);
        });
    });

})(jQuery);