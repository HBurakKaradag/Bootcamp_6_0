using System;

namespace Hotels.API.Queries
{
    public class UserQueries
    {
        public string SelectUserAllQueries => "Select * From dbo.Users";

        public string SelectUserManyQuery => @"Select * From dbo.Users
                                                Where 1 = 1";
    }
}
