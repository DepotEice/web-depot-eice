namespace Web.DepotEice.UIL.Models.Forms
{
    public class AddressForm
    {
        public int Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string? BoxNumber { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
