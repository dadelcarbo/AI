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
        public ViewModel()
        {
            this.Input = new double[10];
            this.ActualOutput = new double[5];
            this.ExpectedOutput = new double[5];
            this.Weights = new double[5];



        }
        static ViewModel current;
        public static ViewModel Current
        {
            get
            {
                if (current == null) current = new ViewModel();
                return current;
            }
        }

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

        private IActivation activation;
        public IActivation Activation
        {
            get { return activation; }
            set { if (activation != value) { activation = value; NotifyPropertyChanged("Activation"); } }
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
