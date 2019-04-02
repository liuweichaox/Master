﻿using Autofac.Extras.IocManager;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virgo.Domain.Uow;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Dapper.Tests
{
    public class UnifOfWork_Tests : TestBaseWithIocBuilder
    {
        public UnifOfWork_Tests()
        {
            Building(build =>
            {
                build.UseUnitOfWorkInterceptor();
                build.RegisterServices(r =>
                {
                    r.Register<IInterceptor, UnitOfWorkInterceptor>(Lifetime.Transient);
                    r.Register<IUnitOfWork, UnifOfWork>(Lifetime.LifetimeScope);
                    r.Register<IUserInfoRepository, UserInfoRepository>(Lifetime.LifetimeScope);
                });
            });
        }

        [UnitOfWork]
        [Fact]
        public async Task Simple_UnitOfWork_TestAsync()
        {
            #region Sql
            /*
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `Id` char(36) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `UserName` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Password` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  `Age` int(11) NULL DEFAULT NULL,
  `PhoneNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;*/
            #endregion
            var un = The<IUnitOfWork>();
            var repository = The<IUserInfoRepository>();
            un.BeginTransaction();
            var users = new List<UserInfo>()
            {
                new UserInfo()
                {
                    Id = Guid.NewGuid(),
                    Password = "Password",
                    PhoneNumber = "12345",
                    UserName = "Jon"
                },
                new UserInfo()
                {
                    Id = Guid.NewGuid(),
                    Password = "Password",
                    PhoneNumber = "54321",
                    UserName = "Virgo"
                },
                new UserInfo()
                {
                    Id = Guid.NewGuid(),
                    Password = "55555",
                    PhoneNumber = "phone",
                    UserName = "LiuDaDa"
                }
            };
            //增
            await repository.InsertAsync(users);
            //删
            await repository.DeleteAsync(users[0]);
            //查
            var userInfos = await repository.GetAllAsync();
            //改
            var virgo = userInfos.FirstOrDefault();
            virgo.UserName = "新名称";
            await repository.UpdateAsync(virgo);
            un.Commit();
        }
    }
}
