﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ray.BiliBiliTool.Application.Contracts
{
    /// <summary>
    /// 每日自动任务
    /// </summary>
    [Description("Test")]
    public interface ITestAppService : IAppService, IAutoTask
    {
    }
}
