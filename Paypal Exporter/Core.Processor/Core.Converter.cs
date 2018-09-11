using Examples.Core.DataStructures;
using AutoMapper;

namespace Examples.Core
{
    public static class Converter  
    {
        public static PaypalRecord ConvertToPaypalRecord(PaypalRecordOldType paypalrecordoldtype)
        {
            Mapper.CreateMap<PaypalRecordOldType, PaypalRecord>();
            var paypalrecord = Mapper.Map<PaypalRecord>(paypalrecordoldtype);
            return paypalrecord;
        }
        public static PaypalRecord ConvertToPaypalRecord(PaypalRecordBusinessAccount paypalrecordbusinessaccount)
        {
            Mapper.CreateMap<PaypalRecordBusinessAccount, PaypalRecord>();
            var paypalrecord = Mapper.Map<PaypalRecord>(paypalrecordbusinessaccount);
            return paypalrecord;
        }
    }
}
