namespace SnowaTec.Test.Domain.Validation.Field
{
    public class CheckPhoneNumber
    {
        public static bool IsValid(string input)
        {
            try
            {
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(input, null);
                return phoneNumber.HasNationalNumber;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
