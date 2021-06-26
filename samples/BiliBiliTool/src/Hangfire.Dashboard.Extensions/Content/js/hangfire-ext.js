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

            e.preventDefault();
        });
    });

    $('#periodicModal').on('click', '#btnPeriodicSubmit', function (e) {
        var $this = $(this);
        //取值
        var id = $("#modal_Id").val();
        var cron = $("#modal_Cron").val();
        var timeZone = $("#modal_TimeZoneId").val();
        var queue = $("#modal_Queue").val();
        var classFullName = $("#modal_ClassFullName").val();
        var methodName = $("#modal_Method").val();

        //提交
        $.post($this.data('url'), {
            "Id": id,
            "Cron": cron,
            "TimeZoneId": timeZone,
            "Queue": queue,
            "ClassFullName": classFullName,
            "MethodName": methodName
        }, function (data, status, xhr) {
            if (status == 'success') {
                alert('提交成功,点击返回列表页');
                //隐藏
                $("#myModal").modal("hide");
                //清空文本框
                clearInput();
                //刷新页面
                window.location.reload();
            }
            else {
                alert('异常：' + status + data);
            }
        });
        e.preventDefault();

        //清空文本框
        function clearInput() {
            $("#modal_Id").val('');
            $("#modal_Cron").val('');
            $("#modal_TimeZoneId").val('');
            $("#modal_Queue").val('');
            $("#modal_Class").val('');
            $("#modal_Method").val('');
        }
    });
})();
