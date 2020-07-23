using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MVCYorbitService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMVCYorbitService" in both code and config file together.
    [ServiceContract]
    public interface IMVCYorbitService
    {
        [OperationContract]
        string GetUser(string uName , string password);

        [OperationContract]
        Nullable<int> Validate(string uName, string password);

        [OperationContract]
        string Register(Account userRegister);

        [OperationContract]
        int ValidateRegistration(string uName, string Email);

        [OperationContract]
        string AddMember(AddMember addMember,string uName);

        [OperationContract]
        Nullable<int> GetMemberDataCount(string uName);

        [OperationContract]
        string GetFirstMemberData();

        [OperationContract]
        IList<AddMember> GetMembers(string uName);

        [OperationContract]
        int MemberDel(int ID);

        [OperationContract]
        bool EditMember(AddMember memberdata);

        [OperationContract]
        string SaveAppId(AddMember memberdata);

        [OperationContract]
        string SubmitRelationShip(RelationShip relationdata,string uName);

        [OperationContract]
        string SubmitApplication(string uName, string appID);

        [OperationContract]
        void AppIDinAddMembers(string appID);

        [OperationContract]
        string GetRelationAppID(string uName);

        [OperationContract]
        IList<AddMember> SearchApplication();

        [OperationContract]
        IList<AddMember> SearchMember(string fName, string lName, DateTime? DOB, string appID);

        [OperationContract]
        [FaultContract(typeof(List<MessageContract.FamilyResponse>))]
        MessageContract.FamilyResponse GetInfo(MessageContract.FamilyRequest Req);

    }
}
