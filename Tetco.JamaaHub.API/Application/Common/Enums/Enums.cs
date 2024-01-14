namespace Application.Common.Enums
{
    public class Enums
    {
        public enum GetBatchesRequirement
        {
            GetAll=0,
            GetOnlyFaild=1,
            GetOnlySuccess=2,
            GetOnlyNew=3,
            GetOnlyInprogress=4,
            GetNewOrFail=5 
                //you can more requirements here as u need
        }
    }
}
