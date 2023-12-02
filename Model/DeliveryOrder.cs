namespace BDAS2_Restaurace.Model
{
    public class DeliveryOrder : Order
    {
        public Address Address { get; set; }

        /*
		public DeliveryOrder (DateTime orderDate, Customer customer, Address address) : base (orderDate, customer)
		{
			Address = address;
		}
		*/

        public override string ToString()
        {
            string result = "";
            result += "Objednavka s sebou\n";
            result += "==================================\n";
            result += Address.ToString();
            result += base.ToString();
            return result;
        }
    }
}
