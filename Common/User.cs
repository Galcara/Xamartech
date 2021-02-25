namespace Common
{
    public class User
    {
        public static object Claims { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
