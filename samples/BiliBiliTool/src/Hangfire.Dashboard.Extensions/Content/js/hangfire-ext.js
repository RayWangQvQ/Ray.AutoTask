(function () {
    $('.js-jobs-list').each(function () {
        var container = this;

        $(this).on('click', '.js-period-jobs-list-command', function (e) {
            var $this = $(this);

            $this.prop('disabled');
            var loadingDelay = setTimeout(function () {
                $this.button('loading');
            }, 100);

            $.post($this.data('url'), {}, function () {
                clearTimeout(loadingDelay);
                window.location.reload();
            });

            e.preventDefault();
        });

        $(this).on('click', '.js-periodic-jobs-list-edit', function (e) {
            var $this = $(this);

            /*
            $("#modal_Id").val($this.data('id'));
            $("#modal_Cron").val($this.data('cron'));
            $("#modal_Queue").val($this.data('queue'));
            $("#modal_Class").val($this.data('class'));
            $("#modal_Method").val($this.data('method'));
            $("#modal_TimeZoneId").val($this.data('timezoneid'));
            */

            $('.modal-dialog').load(`periodic/edit?id=${$this.data('id')}`);
        });
    })
})();
