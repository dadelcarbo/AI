using NeuralNetwork.Activation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class ViewModel : INotifyPropertyChanged
    {
        double[] input;
        public double[] Input { get { return input; } set { input = value; NotifyPropertyChanged("Input"); } }

        double[] weights;
        public double[] Weights { get { return weights; } set { weights = value; NotifyPropertyChanged("Weights"); } }

        double[] actualOutput;
        public double[] ActualOutput { get { return actualOutput; } set { actualOutput = value; NotifyPropertyChanged("ActualOutput"); } }

        double[] expectedOutput;
        public double[] ExpectedOutput { get { return expectedOutput; } set { expectedOutput = value; NotifyPropertyChanged("ExpectedOutput"); } }

        static private List<IActivation> activations;
        static public List<IActivation> Activations
        {
            get
            {
                if (activations == null)
                {
                    activations = new List<IActivation>()
                    {
                        new IdentityActivation(),
                        new Relu(),
                        new Sigmoid(),
                        new Softmax()
                    };
                }
                return activations;
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
