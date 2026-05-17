namespace Task1.InteractiveMethodInvocation
{
    public class CarExample
    {
        private int VANID { get; set; }
        public string Model { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public CarExample Create(int vanid, string model, string plateNumber)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(vanid);
            ArgumentException.ThrowIfNullOrWhiteSpace(model);
            ArgumentException.ThrowIfNullOrWhiteSpace(plateNumber);

            this.VANID = vanid;
            this.Model = model;
            this.PlateNumber = plateNumber;

            return this;
        }
        public CarExample Create(int vanid, string model)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(vanid);
            ArgumentException.ThrowIfNullOrWhiteSpace(model);

            this.VANID = vanid;
            this.Model = model;

            return this;
        }

        public string PrintObject()
        {
            return $"Vehicle {this.VANID} ({this.PlateNumber}): {this.Model}";
        }

        public override string ToString()
        {
            return this.PrintObject();
        }
    }
}
