namespace MovieRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillMembershipTypes : DbMigration
    {
        public override void Up()
        {
            
            Sql("SET IDENTITY_INSERT MembershipTypes ON");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, Duration, DiscountRate) VALUES (1, 0, 0, 0)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, Duration, DiscountRate) VALUES (2, 10, 1, 10)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, Duration, DiscountRate) VALUES (3, 30, 3, 20)");
            Sql("INSERT INTO MembershipTypes (Id, SignUpFee, Duration, DiscountRate) VALUES (4, 100, 12, 30)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM MembershipTypes WHERE Id IN (1,2,3,4)");
        }
    }
}
