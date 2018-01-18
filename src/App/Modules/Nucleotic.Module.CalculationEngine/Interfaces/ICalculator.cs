namespace Nucleotic.Module.CalculationEngine.Interfaces
{
    public interface ICalculator<out T>
    {
        T Context { get; }
    }
}