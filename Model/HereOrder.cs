namespace BDAS2_Restaurace.Model
{
    public class HereOrder : Order
    {
        public int TableNumber { get; set; }

        /*
		public HereOrder(DateTime orderDate, Customer customer, int tableNumber) : base(orderDate, customer)
		{
			TableNumber = tableNumber;	
		}
		*/

        public override string ToString()
        {
            string result = "";
            result += "Objednavka na miste\n";
            result += "==================================\n";
            result += $"Cislo stolu: {TableNumber}";
            result += base.ToString();
            return result;
        }
    }
}
