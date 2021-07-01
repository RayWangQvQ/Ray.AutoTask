using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using Ray.BiliBiliToolPro.AdminWeb.Controllers;
using Ray.BiliBiliToolPro.AdminWeb.ViewModel.SchoolVMs;
using Ray.BiliBiliToolPro.AdminWeb.Model;
using Ray.BiliBiliToolPro.AdminWeb.DataAccess;


namespace Ray.BiliBiliToolPro.AdminWeb.Test
{
    [TestClass]
    public class SchoolApiTest
    {
        private SchoolController _controller;
        private string _seed;

        public SchoolApiTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateApi<SchoolController>(new DataContext(_seed, DBTypeEnum.Memory), "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            ContentResult rv = _controller.Search(new SchoolSearcher()) as ContentResult;
            Assert.IsTrue(string.IsNullOrEmpty(rv.Content)==false);
        }

        [TestMethod]
        public void CreateTest()
        {
            SchoolVM vm = _controller.Wtm.CreateVM<SchoolVM>();
            School v = new School();
            
            v.SchoolCode = "G15";
            v.SchoolName = "I8KN8vsf2";
            v.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PUB;
            v.Remark = "cRS";
            vm.Entity = v;
            var rv = _controller.Add(vm);
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<School>().Find(v.ID);
                
                Assert.AreEqual(data.SchoolCode, "G15");
                Assert.AreEqual(data.SchoolName, "I8KN8vsf2");
                Assert.AreEqual(data.SchoolType, Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PUB);
                Assert.AreEqual(data.Remark, "cRS");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }
        }

        [TestMethod]
        public void EditTest()
        {
            School v = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.SchoolCode = "G15";
                v.SchoolName = "I8KN8vsf2";
                v.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PUB;
                v.Remark = "cRS";
                context.Set<School>().Add(v);
                context.SaveChanges();
            }

            SchoolVM vm = _controller.Wtm.CreateVM<SchoolVM>();
            var oldID = v.ID;
            v = new School();
            v.ID = oldID;
       		
            v.SchoolCode = "c4Z";
            v.SchoolName = "Fw9Zg";
            v.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PRI;
            v.Remark = "LwU";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.SchoolCode", "");
            vm.FC.Add("Entity.SchoolName", "");
            vm.FC.Add("Entity.SchoolType", "");
            vm.FC.Add("Entity.Remark", "");
            var rv = _controller.Edit(vm);
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<School>().Find(v.ID);
 				
                Assert.AreEqual(data.SchoolCode, "c4Z");
                Assert.AreEqual(data.SchoolName, "Fw9Zg");
                Assert.AreEqual(data.SchoolType, Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PRI);
                Assert.AreEqual(data.Remark, "LwU");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }

		[TestMethod]
        public void GetTest()
        {
            School v = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.SchoolCode = "G15";
                v.SchoolName = "I8KN8vsf2";
                v.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PUB;
                v.Remark = "cRS";
                context.Set<School>().Add(v);
                context.SaveChanges();
            }
            var rv = _controller.Get(v.ID.ToString());
            Assert.IsNotNull(rv);
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            School v1 = new School();
            School v2 = new School();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.SchoolCode = "G15";
                v1.SchoolName = "I8KN8vsf2";
                v1.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PUB;
                v1.Remark = "cRS";
                v2.SchoolCode = "c4Z";
                v2.SchoolName = "Fw9Zg";
                v2.SchoolType = Ray.BiliBiliToolPro.AdminWeb.Model.SchoolTypeEnum.PRI;
                v2.Remark = "LwU";
                context.Set<School>().Add(v1);
                context.Set<School>().Add(v2);
                context.SaveChanges();
            }

            var rv = _controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv, typeof(OkObjectResult));

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data1 = context.Set<School>().Find(v1.ID);
                var data2 = context.Set<School>().Find(v2.ID);
                Assert.AreEqual(data1, null);
            Assert.AreEqual(data2, null);
            }

            rv = _controller.BatchDelete(new string[] {});
            Assert.IsInstanceOfType(rv, typeof(OkResult));

        }


    }
}
