namespace NeuralNetwork.Activation
{
    public interface IActivation
    {
        string Name { get;}
        void Activate(double[] input, double [] output);
        
        double[] Derivative(double[] x);
    }
}
