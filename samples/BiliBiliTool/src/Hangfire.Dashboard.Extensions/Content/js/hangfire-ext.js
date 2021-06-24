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

            $this.prop('disabled');

            var id = $this.data('id') || ' ';
            $('.modal-dialog').load(`periodic/edit?id=${id}`);
        });
    });

    $(document).on('click', '#btnPeriodicSubmit', function (e) {
        //取值
        var id = $("#modal_Id").val();

        //提交

        //隐藏
        $("#myModal").modal("hide");

        //清空文本框

        //刷新页面
        window.location.reload();
        alert(id);
    });
})();
