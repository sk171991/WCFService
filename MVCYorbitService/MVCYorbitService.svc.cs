using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MVCYorbitService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MVCYorbitService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MVCYorbitService.svc or MVCYorbitService.svc.cs at the Solution Explorer and start debugging.
    public class MVCYorbitService : IMVCYorbitService
    {
        MVC201Entities1 mVC201 = new MVC201Entities1();
        public string GetUser(string uName,string password)
        {
           string _role = mVC201.sp_GetRole(uName, password).FirstOrDefault();
            if (_role != null)
            {
                return _role;
            }
            else
                return "NotFound";
        }

        public Nullable<int> Validate(string uName, string password)
        {
           
            var _validate =  mVC201.sp_Validate(uName, password);
            return _validate.FirstOrDefault();
            
        }


        public int ValidateRegistration(string uName, string Email)
        {
            ObjectParameter retVal = new ObjectParameter("retVal", typeof(int)); //Create Object parameter to receive a output value.It will behave like output parameter  
            mVC201.sp_ValidateRegistration(uName, Email, retVal);
            int val = Convert.ToInt32(retVal.Value);
            return val;
        }

        public string Register(Account userRegister)
        {
            try
            {
                mVC201.Accounts.Add(userRegister);
                mVC201.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public Nullable<int> GetMemberDataCount(string uName)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == uName
                          select o.ID).First();
            var count = (from o in mVC201.AddMembers
                         where o.ApplicationID == null && o.MetaActive == 1 && o.UserAccountID == userId
                         select o.ID).Count();
            return count;
        }

        public string GetFirstMemberData()
        {
            var _Count = (from o in mVC201.AddMembers
                          select o.DateOfBirth);
            return _Count.First().ToString();
        }

        public string AddMember(AddMember addMember,string uName)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == uName
                          select o.ID).First();
            try
            {

                addMember.UserAccountID = userId;
                mVC201.AddMembers.Add(addMember);
                mVC201.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public IList<AddMember> GetMembers(string uName)
        {
            //IList<AddMember> memberList =
            //    mVC201.AddMembers.SqlQuery("Select FirstName , LastName from Addmember where MetaActive = 1 and ApplicationID IS NULL").ToList<AddMember>();
            //return memberData;
            int accountID = (from x in mVC201.Accounts
                         where x.UserName == uName
                         select x.ID).First();
            //var memberData = (from x in mVC201.AddMembers
            //                  where x.MetaActive == 1 && x.ApplicationID == null && x.UserAccountID == accountID
            //                  select x);
            var memberData = (from x in mVC201.AddMembers
                              where x.MetaActive == 1 && x.ApplicationID == null && x.UserAccountID == accountID
                              select new {x.ID, x.FirstName , x.LastName , x.Suffix , x.DateOfBirth , x.Gender }).ToList();
            List<AddMember> members = new List<AddMember>();
            for (int i = 0; i < memberData.Count; i++)
            {
                members.Add(new AddMember()
                {
                    ID = memberData[i].ID,
                    Suffix = memberData[i].Suffix,
                    FirstName = memberData[i].FirstName,
                    LastName = memberData[i].LastName,
                    Gender = memberData[i].Gender,
                    DateOfBirth = Convert.ToDateTime(memberData[i].DateOfBirth)
                });
            }
            return members;
        }
    
        public bool EditMember(AddMember memberdata)
        {
            var member = (from x in mVC201.AddMembers
                          where x.ID == memberdata.ID
                          select x).First();
            //member = memberdata;
            mVC201.Entry(member).CurrentValues.SetValues(memberdata);
            if (mVC201.SaveChanges() == 1)
            {
                return true;
            }
                return false;
        }
    
        public int MemberDel(int ID)
         {
            var member = (from x in mVC201.AddMembers
                       where x.ID == ID
                       select x).First();
            member.MetaActive = 0;
           
            if (mVC201.SaveChanges() == 1)
            {
                return 1;
            }
            return 0;
        }

        public string SaveAppId(AddMember memberdata)
        {
            var member = (from x in mVC201.AddMembers
                          where x.ApplicationID == memberdata.ApplicationID
                          select x).ToList();
           RelationShip relationShip = new RelationShip();

                foreach (var item in member)
                {
                    item.Suffix = memberdata.Suffix;
                    item.FirstName = memberdata.FirstName;
                    item.MiddleName = memberdata.MiddleName;
                    item.LastName = memberdata.LastName;
                    item.Gender = memberdata.Gender;
                    item.DateOfBirth = memberdata.DateOfBirth;
                }
                if(mVC201.SaveChanges() == 1)
                    {
                        //mVC201.Entry(member).CurrentValues.SetValues(memberdata);

                        return "Success";
                    }
            return "Failure";

        }

        public string SubmitRelationShip(RelationShip relationShip, string uName)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == uName
                          select o.ID).First();
            try
            {
                relationShip.AccountID = userId;
                mVC201.RelationShips.Add(relationShip);
                mVC201.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public string SubmitApplication(string uName,string appID)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == uName
                          select o.ID).First();
            var members = (from x in mVC201.RelationShips
                          where x.AccountID == userId
                          select x).ToList();

            try
            {
                RelationShip relationShip = new RelationShip();

                foreach (var member in members)
                {
                    if (member.AccountID == userId)
                    {
                        member.ApplicationID += appID;
                    }
                }
                mVC201.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public string GetRelationAppID(string uName)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == uName
                          select o.ID).First();
            string appID = (from x in mVC201.RelationShips
                           where x.AccountID == userId
                           select x.ApplicationID).First();
            return appID;
        }

        public IList<AddMember> SearchMember(string fName , string lName , DateTime? DOB , string appID)
        {
            List<AddMember> members = new List<AddMember>();

            var memberdata = (from am in mVC201.AddMembers
                                            join rl in mVC201.RelationShips on
                                            am.ID equals rl.MemberId
                              where am.FirstName == fName || am.LastName == lName || am.DateOfBirth == DOB || rl.ApplicationID == appID
                                            select new
                                            {
                                                am.ID,
                                                am.Suffix,
                                                am.FirstName,
                                                am.LastName,
                                                am.Gender,
                                                am.DateOfBirth,
                                                rl.ApplicationID
                                            }).ToList();

            for (int i = 0; i < memberdata.Count; i++)
            {
                members.Add(new AddMember()
                {
                    ID = memberdata[i].ID,
                    Suffix = memberdata[i].Suffix,
                    FirstName = memberdata[i].FirstName,
                    LastName = memberdata[i].LastName,
                    Gender = memberdata[i].Gender,
                    DateOfBirth = Convert.ToDateTime(memberdata[i].DateOfBirth),
                    ApplicationID = memberdata[i].ApplicationID
                });
            }

            return members;
        }

        public IList<AddMember> SearchApplication()
        {
            List<AddMember> members = new List<AddMember>();

            var memberdata = (from am in mVC201.AddMembers
                              join rl in mVC201.RelationShips on
                              am.ID equals rl.MemberId
                              where rl.ApplicationID != null
                              select new
                              {
                                  am.ID,
                                  am.Suffix,
                                  am.FirstName,
                                  am.LastName,
                                  am.Gender,
                                  am.DateOfBirth,
                                  rl.ApplicationID
                              }).ToList();

            for (int i = 0; i < memberdata.Count; i++)
            {
                members.Add(new AddMember()
                {
                    ID = memberdata[i].ID,
                    Suffix = memberdata[i].Suffix,
                    FirstName = memberdata[i].FirstName,
                    LastName = memberdata[i].LastName,
                    Gender = memberdata[i].Gender,
                    DateOfBirth = Convert.ToDateTime(memberdata[i].DateOfBirth),
                    ApplicationID = memberdata[i].ApplicationID
                });
            }

            return members;
        }

        public void AppIDinAddMembers(string appID)
        {
            var getMemberIds = (from r in mVC201.RelationShips
                                where r.ApplicationID == appID
                                select r.MemberId).ToList();

            
            foreach (var Id in getMemberIds)
            {
                var members = (from r in mVC201.AddMembers
                                  where r.ID == Id
                                  select r).First();
                members.ApplicationID += appID;
               
            }
            mVC201.SaveChanges();
        }

        public MessageContract.FamilyResponse GetInfo(MessageContract.FamilyRequest Req)
        {
            int userId = (from o in mVC201.Accounts
                          where o.UserName == Req.AccountId
                          select o.ID).First();
            MessageContract.FamilyResponse response = new MessageContract.FamilyResponse()
            {
                // create an object of FamilyDetails Class  
                Obj = new List<MessageContract.FamilyDetails>()
            };
            //var members = mVC201.AddMembers.Where(x => x.UserAccountID == userId).ToList();
            var members = (from am in mVC201.AddMembers
                           join rl in mVC201.RelationShips on
                           am.ID equals rl.MemberId
                           select new
                           {
                               am.Suffix,
                               am.FirstName,
                               am.LastName,
                               am.Gender,
                               am.DateOfBirth,
                               rl.RelationType
                           }).ToList();
            for (int i = 0; i < members.Count; i++)
            {
                response.Obj.Add(new MessageContract.FamilyDetails()
                {
                    Suffix = members[i].Suffix,
                    FirstName = members[i].FirstName,
                    LastName = members[i].LastName,
                    Gender = members[i].Gender,
                    DateOfBirth = Convert.ToDateTime(members[i].DateOfBirth),
                    Relation = members[i].RelationType
                });
            }

            return response;
            
            //r.S.Artical = "Learn WCF";  
           
        }
    }
}
