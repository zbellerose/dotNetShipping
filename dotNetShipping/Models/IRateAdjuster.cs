namespace dotNetShipping.Models
{
    public interface IRateAdjuster
    {
        Rate AdjustRate(Rate rate);
    }
}