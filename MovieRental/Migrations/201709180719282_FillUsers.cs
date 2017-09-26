namespace MovieRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName) 
                    VALUES (N'3517275c-e64b-4941-a317-bf74d8659583', N'guest@test.com', 0, N'ABUGkKrZ3HWgctLBmks9maGPIzviLTlT2kiV3yveeObkSPsLqekb6pzfPrRoluYQnA==', N'36bfd65e-08dd-4718-9bc6-bb49f8c66acb', NULL, 0, 0, NULL, 1, 0, N'guest@test.com')
                  INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName) 
                    VALUES (N'c0cbb274-6187-4c13-b536-94958fb05da0', N'admin@test.com', 0, N'AM9amhSF36tKTTUawxqag3WFVeagbMFzpReUXUGLc61JgbvjH4U28Pgfco13gxeJPw==', N'7080aaaf-84b1-436c-9c5d-1d38230b1ffa', NULL, 0, 0, NULL, 1, 0, N'admin@test.com')
            ");

            Sql(@"INSERT INTO AspNetRoles (Id, Name) VALUES (N'b63a6bdc-7dc1-4aeb-9a35-47bb1b3cd33b', N'CanManageMovies')");

            Sql(@"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (N'c0cbb274-6187-4c13-b536-94958fb05da0', N'b63a6bdc-7dc1-4aeb-9a35-47bb1b3cd33b')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AspNetUsers WHERE Id IN ('3517275c-e64b-4941-a317-bf74d8659583','c0cbb274-6187-4c13-b536-94958fb05da0')");
            Sql("DELETE FROM AspNetRoles WHERE Id = 'b63a6bdc-7dc1-4aeb-9a35-47bb1b3cd33b'");
        }
    }
}
