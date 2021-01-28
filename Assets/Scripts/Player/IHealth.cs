interface IHealth
{
    int currentHealth { get; set; }
    int DecreaseHealth(int value);
    int IncreaseHealth(int value);
}
