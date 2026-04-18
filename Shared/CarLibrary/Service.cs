namespace CarLibrary
{
    public class Service
    {
        public Service()
        {
        }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        private bool IsServiceWorking { get; set; }

        public Service(string name, string address, bool isServiceWorking)
        {
            this.Create(name, address, isServiceWorking);
        }

        public Service Create(string name, string address, bool isServiceWorking)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(address);

            this.Name = name;
            this.Address = address;
            this.IsServiceWorking = isServiceWorking;

            return this;
        }

        public string PrintObject()
        {
            string status = this.IsServiceWorking ? "working" : "closed";
            return $"Service {this.Name}: {this.Address} ({status})";
        }

        public override string ToString()
        {
            return this.PrintObject();
        }
    }
}
