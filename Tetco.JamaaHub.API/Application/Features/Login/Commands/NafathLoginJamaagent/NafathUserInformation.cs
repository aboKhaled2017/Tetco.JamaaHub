using Newtonsoft.Json;

namespace Application.Features.Login.Commands.NafathLoginJameaAgent
    {

    public class NafathUserInformation
        {
        [JsonProperty ( "id" )]
        public string UserId
            {
            get; set;
            }
        [JsonProperty ( "id_version" )]
        public int IdVersion
            {
            get; set;
            }
        [JsonProperty ( "first_name#ar" )]
        public string FirstNameAr
            {
            get; set;
            }
        [JsonProperty ( "father_name#ar" )]
        public string FatherNameAr
            {
            get; set;
            }
        [JsonProperty ( "grand_name#ar" )]
        public string GrandNameAr
            {
            get; set;
            }
        [JsonProperty ( "family_name#ar" )]
        public string FamilyNameAr
            {
            get; set;
            }
        [JsonProperty ( "first_name#en" )]
        public string FirstNameEn
            {
            get; set;
            }
        [JsonProperty ( "father_name#en" )]
        public string FatherNameEn
            {
            get; set;
            }
        [JsonProperty ( "grand_name#en" )]
        public string GrandNameEn
            {
            get; set;
            }
        [JsonProperty ( "family_name#en" )]
        public string FamilyNameEn
            {
            get; set;
            }
        [JsonProperty ( "two_names#ar" )]
        public string TwoNamesAr
            {
            get; set;
            }
        [JsonProperty ( "two_names#en" )]
        public string TwoNamesEn
            {
            get; set;
            }
        [JsonProperty ( "full_name#ar" )]
        public string FullNameAr
            {
            get; set;
            }
        [JsonProperty ( "full_name#en" )]
        public string FullNameEn
            {
            get; set;
            }
        [JsonProperty ( "gender" )]
        public string Gender
            {
            get; set;
            }
        [JsonProperty ( "id_issue_date#g" )]
        public string IdIssueDateG
            {
            get; set;
            }
        [JsonProperty ( "id_issue_date#h" )]
        public long IdIssueDateH
            {
            get; set;
            }
        [JsonProperty ( "id_expiry_date#g" )]
        public string IdExpiryDateG
            {
            get; set;
            }
        [JsonProperty ( "id_expiry_date#h" )]
        public long IdExpiryDateH
            {
            get; set;
            }
        [JsonProperty ( "language" )]
        public string Language
            {
            get; set;
            }
        [JsonProperty ( "nationality" )]
        public int Nationality
            {
            get; set;
            }
        [JsonProperty ( "nationality#ar" )]
        public string NationalityAr
            {
            get; set;
            }
        [JsonProperty ( "nationality#en" )]
        public string NationalityEn
            {
            get; set;
            }
        [JsonProperty ( "dob#g" )]
        public string DobG
            {
            get; set;
            }
        [JsonProperty ( "dob#h" )]
        public long DobH
            {
            get; set;
            }
        [JsonProperty ( "card_issue_place#ar" )]
        public string CardIssuePlaceAr
            {
            get; set;
            }
        [JsonProperty ( "card_issue_place#en" )]
        public string CardIssuePlaceEn
            {
            get; set;
            }

        }
    }
