using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace MVCYorbitService
{
    public class MessageContract
    {
        [MessageContract]
        public class FamilyRequest
        {
            [MessageHeader]
            public string AccountId;
        }
        [MessageContract]
        public class FamilyResponse
        {
            [MessageBodyMember]
            public List<FamilyDetails> Obj;
        }
        [DataContract]
        public class FamilyDetails
        {
            [DataMember]
            public string Suffix { get; set; }
            [DataMember]
            public string FirstName { get; set; }
            [DataMember]
            public string LastName { get; set; }
            [DataMember]
            public string Gender { get; set; }
            [DataMember]
            public DateTime DateOfBirth { get; set; }
            [DataMember]
            public string Relation { get; set; }
        }

      
    }
}