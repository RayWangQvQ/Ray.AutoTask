window.onload = function () {

};

(function (hangfire) {
    //periodic job 列表页：
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

        //点击新增/编辑按钮事件
        $(this).on('click', '.js-periodic-jobs-list-edit', function (e) {
            var $this = $(this);
            $this.prop('disabled');
            var id = $this.data('id') || ' ';

            $('.modal-dialog').load(`periodic/edit?id=${id}`, function () {
                $("#btnPeriodicSubmit").off("click");
                $('#btnPeriodicSubmit').one('click', function (e) {
                    periodicSubmit($(this).data('url'));
                    e.preventDefault();
                });
            });

            e.preventDefault();
        });
    });

    //提交 periodic 作业
    var periodicSubmit = function (url) {
        //取值
        var id = $("#modal_Id").val();
        var cron = $(".cronInsideInput").val();//用Id取不到
        var queue = $("#modal_Queue").val();
        var classFullName = $("#modal_ClassFullName").val();
        var methodName = $("#modal_Method").val();

        var timeZoneSelectOption = $("#modal_TimeZone option:selected");
        var timeZoneId = timeZoneSelectOption.val();

        //提交
        $.post(url, {
            "Id": id,
            "Cron": cron,
            "TimeZoneId": timeZoneId,
            "Queue": queue,
            "ClassFullName": classFullName,
            "MethodName": methodName
        }, function (data, status, xhr) {
            console.log(data)
            console.log(status)
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
        }).fail(function (response) {
            console.log('异常：' + JSON.stringify(response));
            alert('异常：' + JSON.stringify(response));
        });
        //清空文本框
        function clearInput() {
            $("#modal_Id").val('');
            $("#modal_Cron").val('');
            $("#modal_TimeZone").val('');
            $("#modal_Queue").val('');
            $("#modal_Class").val('');
            $("#modal_Method").val('');
        }
    };
})(window.Hangfire = window.Hangfire || {});
