﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Ray.BiliBiliToolPro.AdminWeb.Model;


namespace Ray.BiliBiliToolPro.AdminWeb.ViewModel.SchoolVMs
{
    public partial class SchoolSearcher : BaseSearcher
    {
        [Display(Name = "学校编码")]
        public String SchoolCode { get; set; }
        [Display(Name = "学校名称")]
        public String SchoolName { get; set; }
        [Display(Name = "学校类型")]
        public SchoolTypeEnum? SchoolType { get; set; }

        protected override void InitVM()
        {
        }

    }
}
